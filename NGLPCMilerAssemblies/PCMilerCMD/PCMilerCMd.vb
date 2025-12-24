
Public Class PCMilerCMd


    Public Shared Sub Main()
        Try
            Dim oWrapper As New PCMWrapper()
            oWrapper.ProcessData()
        Catch ex As Exception
            Console.WriteLine() 'be sure we are on a new line
            Console.WriteLine("Error! " & ex.ToString)

        Finally
            Console.WriteLine() 'be sure we are on a new line
            Console.WriteLine("Press Enter To Continue")
            Console.ReadLine()
        End Try
    End Sub

End Class
