Imports System
Imports System.Net
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Text.RegularExpressions
Imports IG = Infragistics.Excel
Imports System.IO

Public Class SyncNatFuelIndexByXLS

    Public LastError As String = ""

    Private Const USER_AGENT As String = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.2)"

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
        Dim sw As New System.IO.MemoryStream()

        Try
            req = CType(System.Net.WebRequest.Create(My.Settings.EIADieselPricesURL_XLS), HttpWebRequest)
            req.Headers.Add("cookie", "")
            req.UserAgent = USER_AGENT
            res = CType(req.GetResponse(), HttpWebResponse)
           res.GetResponseStream().CopyTo(sw)

            'We are using Infragistics Excel Engine so we dont have to reinvent the wheel.

            'create variables
            Dim wb As IG.Workbook = Nothing
            Dim ws As IG.Worksheet = Nothing
            Dim rowCollection As New List(Of FuelUpdateRecord)

            'load workbook from memory
            wb = Infragistics.Excel.Workbook.Load(sw)

            'jump to appropriate worksheet which is 'Data 1' sheet
            ws = wb.Worksheets(My.Settings.WorkSheetName)

            'find the sourceKey row
            Dim sourcKeyRow As IG.WorksheetRow = Nothing
            sourcKeyRow = (From r In ws.Rows() Where r.Cells(0).Value.ToString.ToUpper.Equals(My.Settings.SourceKeyStr.ToUpper)).First

            If sourcKeyRow Is Nothing AndAlso sourcKeyRow.Cells Is Nothing Then Throw New Exception("Fuel Updater Failed, sourceKey row is not present.")

            'weekDate,weeklyUS,weeklyEastCoast,weeklyNewEngland,weeklyCentralAtlantic,weeklyLowerAtlantic,weeklyMidWest,weeklyGulfCoast,weeklyRockeyMountain,weeklyWestCoast,weeklyCalifornia
            'Find the column indexs for by using the sourceKey values.
            Dim weeklyUSIndex As Integer = (From d In sourcKeyRow.Cells() Where d.Value.ToString.ToUpper = My.Settings.WklyUSKey.ToUpper).First.ColumnIndex
            Dim weeklyEastCoastIndex As Integer = (From d In sourcKeyRow.Cells() Where d.Value.ToString.ToUpper = My.Settings.WklyEastCoastKey.ToUpper).First.ColumnIndex
            Dim weeklyNewEnglandIndex As Integer = (From d In sourcKeyRow.Cells() Where d.Value.ToString.ToUpper = My.Settings.WklyNewEnglandKey.ToUpper).First.ColumnIndex
            Dim weeklyCentralAtlanticIndex As Integer = (From d In sourcKeyRow.Cells() Where d.Value.ToString.ToUpper = My.Settings.WklyCentralAtlanticKey.ToUpper).First.ColumnIndex
            Dim weeklyLowerAtlanticIndex As Integer = (From d In sourcKeyRow.Cells() Where d.Value.ToString.ToUpper = My.Settings.WklyLowerAtlanticKey.ToUpper).First.ColumnIndex
            Dim weeklyMidWestIndex As Integer = (From d In sourcKeyRow.Cells() Where d.Value.ToString.ToUpper = My.Settings.WklyMidWestKey.ToUpper).First.ColumnIndex
            Dim weeklyGulfCoastIndex As Integer = (From d In sourcKeyRow.Cells() Where d.Value.ToString.ToUpper = My.Settings.WklyGulfCoastKey.ToUpper).First.ColumnIndex
            Dim weeklyRockeyMountainIndex As Integer = (From d In sourcKeyRow.Cells() Where d.Value.ToString.ToUpper = My.Settings.WklyRockeyMTNKey.ToUpper).First.ColumnIndex
            Dim weeklyWestCoastIndex As Integer = (From d In sourcKeyRow.Cells() Where d.Value.ToString.ToUpper = My.Settings.WklyWestCoastKey.ToUpper).First.ColumnIndex
            Dim weeklyCaliforniaIndex As Integer = (From d In sourcKeyRow.Cells() Where d.Value.ToString.ToUpper = My.Settings.WklkCaliforniaKey.ToUpper).First.ColumnIndex

            'jump to the last row, this is where the lastest update should be

            'work our way backwards until we find the right week.
            For i As Integer = ws.Rows.Count To 0 Step -1

                Dim row As IG.WorksheetRow = ws.Rows(i)
                'if it is a real date then we can add it to the collection
                'The value is actually a double so we have to convert it.
                If row IsNot Nothing AndAlso row.Cells(0).Value IsNot Nothing AndAlso row.Cells(0).Value.ToString.Trim.Length > 0 Then

                    Dim rec As New FuelUpdateRecord
                    'now get the values
                    rec.weekDate = Date.FromOADate(row.Cells(0).Value)
                    rec.weeklyUS = row.Cells(weeklyUSIndex).Value
                    rec.weeklyEastCoast = row.Cells(weeklyEastCoastIndex).Value
                    rec.weeklyNewEngland = row.Cells(weeklyNewEnglandIndex).Value
                    rec.weeklyCentralAtlantic = row.Cells(weeklyCentralAtlanticIndex).Value
                    rec.weeklyLowerAtlantic = row.Cells(weeklyLowerAtlanticIndex).Value
                    rec.weeklyMidWest = row.Cells(weeklyMidWestIndex).Value
                    rec.weeklyGulfCoast = row.Cells(weeklyGulfCoastIndex).Value
                    rec.weeklyRockeyMountain = row.Cells(weeklyRockeyMountainIndex).Value
                    rec.weeklyWestCoast = row.Cells(weeklyWestCoastIndex).Value
                    rec.weeklyCalifornia = row.Cells(weeklyCaliforniaIndex).Value

                    'add to collection of rows
                    rowCollection.Add(rec)

                    'Console.WriteLine("Record: " & rowCollection.Count _
                    '            & vbCrLf & "weekDate: " & rec.weekDate _
                    '            & vbCrLf & "weeklyUS: " & rec.weeklyUS _
                    '            & vbCrLf & "weeklyEastCoast : " & rec.weeklyEastCoast _
                    '            & vbCrLf & "weeklyNewEngland: " & rec.weeklyNewEngland _
                    '            & vbCrLf & "weeklyCentralAtlantic: " & rec.weeklyCentralAtlantic _
                    '            & vbCrLf & "weeklyLowerAtlantic: " & rec.weeklyLowerAtlantic _
                    '            & vbCrLf & "weeklyMidWest: " & rec.weeklyMidWest _
                    '            & vbCrLf & "weeklyGulfCoast: " & rec.weeklyGulfCoast _
                    '            & vbCrLf & "weeklyRockeyMountain: " & rec.weeklyRockeyMountain _
                    '            & vbCrLf & "weeklyWestCoast: " & rec.weeklyWestCoast _
                    '            & vbCrLf & "weeklyCalifornia: " & rec.weeklyCalifornia)
                    'Console.WriteLine("End")
                    'Console.WriteLine(vbCrLf)
                    'Console.WriteLine(vbCrLf)

                    'if we have enough records lets exit.
                    If rowCollection.Count >= My.Settings.GetWeeksBack Then
                        Exit For
                    End If
                End If
            Next

            sw.Close()
            res.Close()

            Dim ValueTable As DataTable = GetDataTable(rowCollection)
            Console.WriteLine("Updating database...")

            'Test code for inserting data
            Dim SqlConn As SqlConnection = New SqlConnection(connectionString)
            Try
                SqlConn.Open()
                Dim retMessage As String = ""
                Dim cm As New SqlCommand("", SqlConn)
                Dim dt As DateTime = Nothing
                Dim NatFuelNatAvg As Decimal = 0
                Dim NatFuelZone1Avg As Decimal = 0
                Dim NatFuelZone2Avg As Decimal = 0
                Dim NatFuelZone3Avg As Decimal = 0
                Dim NatFuelZone4Avg As Decimal = 0
                Dim NatFuelZone5Avg As Decimal = 0
                Dim NatFuelZone6Avg As Decimal = 0 
                Dim NatFuelZone7Avg As Decimal = 0
                Dim NatFuelZone8Avg As Decimal = 0
                Dim NatFuelZone9Avg As Decimal = 0 

                For Each row As DataRow In ValueTable.Rows
                    cm.CommandText = "SELECT COUNT(*) FROM dbo.NatFuel WHERE NatFuelDate = '" & row("NatFuelDate") & "'"
                    cm.CommandType = System.Data.CommandType.Text
                    cm.Parameters.Clear()
                    Dim result As Integer = cm.ExecuteScalar
                    If result = 0 Then
                        'cm.CommandText = "spInsertNatFuelIndex50 '" & row("NatFuelDate").ToString & "', " & _
                        '    row("NatFuelNatAvg") & ", " & row("NatFuelZone1Avg") & ", " & _
                        '    row("NatFuelZone2Avg") & ", " & row("NatFuelZone3Avg") & ", " & _
                        '    row("NatFuelZone4Avg") & ", " & row("NatFuelZone5Avg") & ", " & _
                        '    row("NatFuelZone6Avg") & ", " & row("NatFuelZone7Avg") & ", " & _
                        '    row("NatFuelZone8Avg") & ", " & row("NatFuelZone9Avg")

                        cm.CommandText = "spInsertNatFuelIndex50"
                        dt = Nothing
                        If DateTime.TryParse(row("NatFuelDate").ToString, dt) Then
                            NatFuelNatAvg = 0
                            NatFuelZone1Avg = 0
                            NatFuelZone2Avg = 0
                            NatFuelZone3Avg = 0
                            NatFuelZone4Avg = 0
                            NatFuelZone5Avg = 0
                            NatFuelZone6Avg = 0
                            NatFuelZone7Avg = 0
                            NatFuelZone8Avg = 0
                            NatFuelZone9Avg = 0
                            Decimal.TryParse(row("NatFuelNatAvg").ToString, NatFuelNatAvg)
                            Decimal.TryParse(row("NatFuelZone1Avg").ToString, NatFuelZone1Avg)
                            Decimal.TryParse(row("NatFuelZone2Avg").ToString, NatFuelZone2Avg)
                            Decimal.TryParse(row("NatFuelZone3Avg").ToString, NatFuelZone3Avg)
                            Decimal.TryParse(row("NatFuelZone4Avg").ToString, NatFuelZone4Avg)
                            Decimal.TryParse(row("NatFuelZone5Avg").ToString, NatFuelZone5Avg)
                            Decimal.TryParse(row("NatFuelZone6Avg").ToString, NatFuelZone6Avg)
                            Decimal.TryParse(row("NatFuelZone7Avg").ToString, NatFuelZone7Avg)
                            Decimal.TryParse(row("NatFuelZone8Avg").ToString, NatFuelZone8Avg)
                            Decimal.TryParse(row("NatFuelZone9Avg").ToString, NatFuelZone9Avg)

                            cm.CommandType = System.Data.CommandType.StoredProcedure
                            cm.Parameters.Clear()
                            cm.Parameters.Add("@NatFuelDate", SqlDbType.DateTime).Value = dt
                            cm.Parameters.Add("@NatFuelNatAvg", SqlDbType.Decimal).Value = NatFuelNatAvg
                            cm.Parameters.Add("@NatFuelZone1Avg", SqlDbType.Decimal).Value = NatFuelZone1Avg
                            cm.Parameters.Add("@NatFuelZone2Avg", SqlDbType.Decimal).Value = NatFuelZone2Avg
                            cm.Parameters.Add("@NatFuelZone3Avg", SqlDbType.Decimal).Value = NatFuelZone3Avg
                            cm.Parameters.Add("@NatFuelZone4Avg", SqlDbType.Decimal).Value = NatFuelZone4Avg
                            cm.Parameters.Add("@NatFuelZone5Avg", SqlDbType.Decimal).Value = NatFuelZone5Avg
                            cm.Parameters.Add("@NatFuelZone6Avg", SqlDbType.Decimal).Value = NatFuelZone6Avg
                            cm.Parameters.Add("@NatFuelZone7Avg", SqlDbType.Decimal).Value = NatFuelZone7Avg
                            cm.Parameters.Add("@NatFuelZone8Avg", SqlDbType.Decimal).Value = NatFuelZone8Avg
                            cm.Parameters.Add("@NatFuelZone9Avg", SqlDbType.Decimal).Value = NatFuelZone9Avg
                            cm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = ""
                            cm.Parameters.Add("@RetMsg", SqlDbType.NVarChar).Value = ""
                            cm.Parameters.Add("@ErrNumber", SqlDbType.Int).Value = 0
                            cm.ExecuteNonQuery()
                        End If
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

    Private Function GetDataTable(ByRef list As List(Of FuelUpdateRecord)) As DataTable
        Dim dt As DataTable = New DataTable()
        If list Is Nothing Then
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

        For Each match As FuelUpdateRecord In list
            Dim dr As DataRow = dt.NewRow()
            dr("NatFuelDate") = match.weekDate
            dr("NatFuelNatAvg") = match.weeklyUS
            dr("NatFuelZone1Avg") = match.weeklyEastCoast
            dr("NatFuelZone2Avg") = match.weeklyNewEngland
            dr("NatFuelZone3Avg") = match.weeklyCentralAtlantic
            dr("NatFuelZone4Avg") = match.weeklyLowerAtlantic
            dr("NatFuelZone5Avg") = match.weeklyMidWest
            dr("NatFuelZone6Avg") = match.weeklyGulfCoast
            dr("NatFuelZone7Avg") = match.weeklyRockeyMountain
            dr("NatFuelZone8Avg") = match.weeklyWestCoast
            dr("NatFuelZone9Avg") = match.weeklyCalifornia
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function

    'Function Copy(ByVal fromStream As Stream, ByVal toStream As Stream) As MemoryStream
    '    Dim reader As New StreamReader(fromStream)
    '    Dim writer As New StreamWriter(toStream)
    '    writer.WriteLine(reader.ReadToEnd())
    '    writer.Flush()
    '    Dim m = New MemoryStream()
    '    m = writer.BaseStre
    '    Return m
    'End Function


End Class

Public Class FuelUpdateRecord

    Public weekDate As Date = Date.MinValue
    Public weeklyUS As Double = 0
    Public weeklyEastCoast As Double = 0
    Public weeklyNewEngland As Double = 0
    Public weeklyCentralAtlantic As Double = 0
    Public weeklyLowerAtlantic As Double = 0
    Public weeklyMidWest As Double = 0
    Public weeklyGulfCoast As Double = 0
    Public weeklyRockeyMountain As Double = 0
    Public weeklyWestCoast As Double = 0
    Public weeklyCalifornia As Double = 0

End Class