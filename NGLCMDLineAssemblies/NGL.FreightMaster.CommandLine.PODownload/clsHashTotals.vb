Imports System.IO

Public Class clsHashTotals
    Public TotalLanes As Integer = 0
    Public TotalOrders As Integer = 0
    Public HashTotalOrders As Double = 0
    Public TotalDetails As Integer = 0
    Public TotalQty As Integer = 0
    Public TotalWeight As Double = 0
    Public HashTotalDetails As Double = 0
    Public TotalCompanies As Integer = 0
    Public TotalCarriers As Integer = 0
    Public TotalPayables As Integer = 0
    Public TotalSchedules As Integer = 0



    Public Debug As Boolean = False

    Public Sub Read(ByVal strFileName As String)

        Try
            Clear()
            If File.Exists(strFileName) Then
                Dim sr As StreamReader = New StreamReader(strFileName)
                Try

                    If sr.Peek() >= 0 Then
                        Dim strRow As String = sr.ReadLine()
                        Dim strRecord As String() = strRow.Split(",")
                        TotalLanes = Val(strRecord(0))
                        TotalOrders = Val(strRecord(1))
                        HashTotalOrders = Val(strRecord(2))
                        TotalDetails = Val(strRecord(3))
                        TotalQty = Val(strRecord(4))
                        TotalWeight = Val(strRecord(5))
                        HashTotalDetails = Val(strRecord(6))
                        TotalCompanies = Val(strRecord(7))
                        TotalCarriers = Val(strRecord(8))
                        TotalPayables = Val(strRecord(9))
                        TotalSchedules = Val(strRecord(10))
                    End If
                    sr.Close()
                Catch ex As Exception
                    Throw
                Finally
                    sr.Dispose()
                End Try
            End If

        Catch ex As Exception
            'the calling function must handle any errors
            Throw New ApplicationException("Cannot Read Hash Total Record", ex)
        End Try

    End Sub

    Public Sub Write(ByVal strFileName As String)
        Try
            If File.Exists(strFileName) Then
                File.Delete(strFileName)
            End If
            Dim w As StreamWriter = New StreamWriter(strFileName)
            w.Write(Chr(34))
            w.Write(TotalLanes)
            w.Write(""",""")
            w.Write(TotalOrders)
            w.Write(""",""")
            w.Write(HashTotalOrders.ToString)
            w.Write(""",""")
            w.Write(TotalDetails)
            w.Write(""",""")
            w.Write(TotalQty)
            w.Write(""",""")
            w.Write(TotalWeight)
            w.Write(""",""")
            w.Write(HashTotalDetails.ToString)
            w.Write(""",""")
            w.Write(TotalCompanies)
            w.Write(""",""")
            w.Write(TotalCarriers)
            w.Write(""",""")
            w.Write(TotalPayables)
            w.Write(""",""")
            w.Write(TotalSchedules)
            w.Write(Chr(34))
            w.Close()
        Catch ex As Exception
            'the calling function must handle any errors
            Throw New ApplicationException("Cannot Write Hash Total Record", ex)
        End Try

    End Sub


    Public Sub Delete(ByVal strFileName As String)
        Try
            If File.Exists(strFileName) Then
                File.Delete(strFileName)
            End If
        Catch ex As Exception
            'ignore any errors
        End Try
    End Sub

    Public Sub Clear()
        TotalLanes = 0
        TotalOrders = 0
        HashTotalOrders = 0
        TotalDetails = 0
        TotalQty = 0
        TotalWeight = 0
        HashTotalDetails = 0
        TotalCompanies = 0
        TotalCarriers = 0
        TotalPayables = 0
        TotalSchedules = 0
    End Sub

End Class
