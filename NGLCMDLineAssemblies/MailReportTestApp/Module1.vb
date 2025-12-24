Imports System.Net.Mail
Imports System.IO
Imports System.ServiceModel
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports Ngl.FreightMaster.Core
Imports Ngl.Core.Communication
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports Ngl.Core.Communication.Email
Imports Ngl.Core.Communication.General
Imports NData = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core

Module Module1

    Sub Main()

        Try
            Console.WriteLine("Enter the Mail Control Number to run the report.")
            Dim strMailControl = Console.ReadLine()
            Console.WriteLine("Runing Report for Mail Control: " & strMailControl)
            Dim oApp = New clsApplication()
            oApp.GetReports(strMailControl)
        Catch ex As Exception
            Console.WriteLine("Unexpected Error: " & ex.ToString())
        Finally
            Console.WriteLine("Press Enter to exit")
            Console.ReadLine()
        End Try

    End Sub



End Module
