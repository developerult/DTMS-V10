Imports System
Imports System.Net
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Text.RegularExpressions

Public Class SyncNatFuelIndex

    Public LastError As String = ""

    Private Const USER_AGENT As String = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.2)"
    Private HtmlText As String
    Private Const Pattern As String = "<TD WIDTH=70  ALIGN=LEFT ><B><FONT style=FONT-SIZE:10pt FACE=""Times New Roman"" COLOR=#000000>(?<Date>(\d|/)*)\s*</B></TD>\r\n" & _
                                      "<TD WIDTH=40  ALIGN=RIGHT ><FONT style=FONT-SIZE:10pt FACE=""Times New Roman"" COLOR=#000000>(?<USAvg>(\d|\.)*)\s*</TD>\r\n" & _
                                      "<TD WIDTH=48  ALIGN=RIGHT ><FONT style=FONT-SIZE:10pt FACE=""Times New Roman"" COLOR=#000000>(?<EastCoast>(\d|\.)*)\s*</TD>\r\n" & _
                                      "<TD WIDTH=54  ALIGN=RIGHT ><FONT style=FONT-SIZE:10pt FACE=""Times New Roman"" COLOR=#000000>(?<NewEngland>(\d|\.)*)\s*</TD>\r\n" & _
                                      "<TD WIDTH=54  ALIGN=RIGHT ><FONT style=FONT-SIZE:10pt FACE=""Times New Roman"" COLOR=#000000>(?<CentralAtlantic>(\d|\.)*)\s*</TD>\r\n" & _
                                      "<TD WIDTH=54  ALIGN=RIGHT ><FONT style=FONT-SIZE:10pt FACE=""Times New Roman"" COLOR=#000000>(?<LowerAtlantic>(\d|\.)*)\s*</TD>\r\n" & _
                                      "<TD WIDTH=54  ALIGN=RIGHT ><FONT style=FONT-SIZE:10pt FACE=""Times New Roman"" COLOR=#000000>(?<Midwest>(\d|\.)*)\s*</TD>\r\n" & _
                                      "<TD WIDTH=54  ALIGN=RIGHT ><FONT style=FONT-SIZE:10pt FACE=""Times New Roman"" COLOR=#000000>(?<GulfCoast>(\d|\.)*)\s*</TD>\r\n" & _
                                      "<TD WIDTH=54  ALIGN=RIGHT ><FONT style=FONT-SIZE:10pt FACE=""Times New Roman"" COLOR=#000000>(?<RockyMtn>(\d|\.)*)\s*</TD>\r\n" & _
                                      "<TD WIDTH=54  ALIGN=RIGHT ><FONT style=FONT-SIZE:10pt FACE=""Times New Roman"" COLOR=#000000>(?<WestCoast>(\d|\.)*)\s*</TD>\r\n" & _
                                      "<TD WIDTH=54  ALIGN=RIGHT ><FONT style=FONT-SIZE:10pt FACE=""Times New Roman"" COLOR=#000000>(?<CA>(\d|\.)*)\s*</TD>\r\n"

    ''' <summary>
    ''' This methods checks for changes in the Fuel SurCharge data and returns 
    ''' -1 on error, 0 on no change and count of updates on changes.
    ''' </summary>
    ''' <param name="connectionString"></param>
    ''' <returns>-1 on error, 0 on no change, count of updates on success</returns>
    ''' <remarks></remarks>
    Public Function Sync(ByVal connectionString As String) As Integer
        Dim intRet As Integer = 0
        Console.WriteLine("Retreiving fuel index information...")
        Dim req As HttpWebRequest = Nothing
        Dim res As HttpWebResponse = Nothing
        Dim sr As System.IO.StreamReader = Nothing

        Try
            req = CType(System.Net.WebRequest.Create(My.Settings.EIADieselPricesURL), HttpWebRequest)
            req.Headers.Add("cookie", "")
            req.UserAgent = USER_AGENT
            res = CType(req.GetResponse(), HttpWebResponse)
            sr = New System.IO.StreamReader(res.GetResponseStream())

            HtmlText = sr.ReadToEnd()

            sr.Close()
            res.Close()

            Dim ValueTable As DataTable = GetDataTable(HtmlText)
            Console.WriteLine("Updating database...")

            'Test code for inserting data
            Dim SqlConn As SqlConnection = New SqlConnection(connectionString)
            Try
                SqlConn.Open()
                Dim cm As New SqlCommand("", SqlConn)
                For Each row As DataRow In ValueTable.Rows
                    cm.CommandText = "SELECT COUNT(*) FROM dbo.NatFuel WHERE NatFuelDate = '" & row("NatFuelDate") & "'"
                    Dim result As Integer = cm.ExecuteScalar
                    If result = 0 Then
                        cm.CommandText = "spInsertNatFuelIndex '" & row("NatFuelDate").ToString & "', " & _
                            row("NatFuelNatAvg") & ", " & row("NatFuelZone1Avg") & ", " & _
                            row("NatFuelZone2Avg") & ", " & row("NatFuelZone3Avg") & ", " & _
                            row("NatFuelZone4Avg") & ", " & row("NatFuelZone5Avg") & ", " & _
                            row("NatFuelZone6Avg") & ", " & row("NatFuelZone7Avg") & ", " & _
                            row("NatFuelZone8Avg") & ", " & row("NatFuelZone9Avg")
                        cm.ExecuteNonQuery()
                        intRet += 1
                    End If
                Next

            Catch ex As Exception
                LastError = ex.Message
                Console.WriteLine("SQL Error: " & ex.ToString)
                If intRet < 1 Then intRet = -1
            Finally
                SqlConn.Close()

            End Try
            Console.WriteLine("Fuel index update complete.")
        Catch ex As Exception
            LastError = ex.Message
            Console.WriteLine("Unexpected Error: " & ex.ToString)
            If intRet < 1 Then intRet = -1
        End Try
        Return intRet

    End Function

    Private Function GetDataTable(ByRef HtmlText As String) As DataTable
        Dim dt As DataTable = New DataTable()
        If HtmlText Is Nothing Then
            Return Nothing
        End If
        Dim matches As MatchCollection = GetMatches(Pattern, HtmlText)
        If matches Is Nothing Then
            Return Nothing
        End If
        With dt
            .Columns.Add("NatFuelDate", System.Type.GetType("System.DateTime"))        'Date
            .Columns.Add("NatFuelNatAvg", System.Type.GetType("System.Decimal"))       'US Average
            .Columns.Add("NatFuelZone1Avg", System.Type.GetType("System.Decimal"))     'East Coast
            .Columns.Add("NatFuelZone2Avg", System.Type.GetType("System.Decimal"))     'New England
            .Columns.Add("NatFuelZone3Avg", System.Type.GetType("System.Decimal"))     'Central Atlantic
            .Columns.Add("NatFuelZone4Avg", System.Type.GetType("System.Decimal"))     'Lower Atlantic
            .Columns.Add("NatFuelZone5Avg", System.Type.GetType("System.Decimal"))     'Midwest
            .Columns.Add("NatFuelZone6Avg", System.Type.GetType("System.Decimal"))     'Gulf Coast
            .Columns.Add("NatFuelZone7Avg", System.Type.GetType("System.Decimal"))     'Rocky Mtn
            .Columns.Add("NatFuelZone8Avg", System.Type.GetType("System.Decimal"))     'West Coast
            .Columns.Add("NatFuelZone9Avg", System.Type.GetType("System.Decimal"))     'California
        End With
        Dim match As Match
        For Each match In matches
            Dim dr As DataRow = dt.NewRow()
            dr("NatFuelDate") = CType(match.Groups("Date").Value.Trim(), DateTime)
            dr("NatFuelNatAvg") = CType(match.Groups("USAvg").Value.Trim(), Decimal)
            dr("NatFuelZone1Avg") = CType(match.Groups("EastCoast").Value.Trim(), Decimal)
            dr("NatFuelZone2Avg") = CType(match.Groups("NewEngland").Value.Trim(), Decimal)
            dr("NatFuelZone3Avg") = CType(match.Groups("CentralAtlantic").Value.Trim(), Decimal)
            dr("NatFuelZone4Avg") = CType(match.Groups("LowerAtlantic").Value.Trim(), Decimal)
            dr("NatFuelZone5Avg") = CType(match.Groups("Midwest").Value.Trim(), Decimal)
            dr("NatFuelZone6Avg") = CType(match.Groups("GulfCoast").Value.Trim(), Decimal)
            dr("NatFuelZone7Avg") = CType(match.Groups("RockyMtn").Value.Trim(), Decimal)
            dr("NatFuelZone8Avg") = CType(match.Groups("WestCoast").Value.Trim(), Decimal)
            dr("NatFuelZone9Avg") = CType(match.Groups("CA").Value.Trim(), Decimal)
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function

    Private Function GetMatches(ByRef Pattern As String, ByRef HtmlText As String) As MatchCollection
        Dim regex As Regex = New Regex(Pattern, RegexOptions.IgnoreCase)
        Dim matches As MatchCollection = regex.Matches(HtmlText)
        If matches.Count > 0 Then
            Return matches
        Else
            Return Nothing
        End If
    End Function

End Class
