Module Module1

    Sub Main()
        Try
            'http://nglwcfdev70.nextgeneration.com/AutoRunTMS.aspx
            Dim URL = My.Application.CommandLineArgs(0)
            If URL.Trim.Length < 10 Then Throw New System.ApplicationException("Invalid Command Line Switch.  A URL to the Click Once Applicaiton is Requred. " & vbCrLf & "You provided: " & URL)
            Process.Start("iexplore.exe", URL)
        Catch ex As Exception
            Console.WriteLine("There was a problem launching the Application: " & vbCrLf & ex.Message)
            Console.WriteLine("Press Enter To Continue")
            Dim strRet = Console.ReadLine()
        End Try
    End Sub

End Module
