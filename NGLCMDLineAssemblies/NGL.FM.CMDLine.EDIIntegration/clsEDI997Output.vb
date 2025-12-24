Imports System.IO
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports NGLBC = NGL.FreightMaster.Integration

Public Class clsEDI997Output : Inherits EDIFileCore
    ''' <summary>
    ''' Read is not currently support for sending 997 output data
    ''' The caller must provide the output 
    ''' </summary>
    ''' <param name="strErrMsg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Read(Optional ByVal strErrMsg As String = "") As Boolean
        Return False

    End Function

    Public Sub Send997ResponseData(ByVal strEDIMsg As String, ByVal caller As EDIFileCore)

        Dim strFileName As String = ""
        With Me
            Try
                .cloneTaskParameters(caller)
                .LogFile = caller.LogFile
                .openLog()
                .Log("Reading carrier 997 config data for " & caller.CarrierNumber.ToString & ": " & caller.CarrierName)
                .CarrierControl = caller.CarrierControl
                .CarrierNumber = caller.CarrierNumber
                .CarrierName = caller.CarrierName
                .CreatedDate = Now.ToString
                .CreateUser = "Process 997 EDI Response Data"
                If Not .ReadCarrierEDIConfig("997") Then Return 'we cannot process this data
                'change to the EDI log file
                .closeLog(0)
                .LogFile = .EDILogFile
                .openLog()
                .Log("Begin " & .Source)
                strFileName = Me.SFun.timeStampFileName(Me.SFun.buildPath(.OutboundFilesFolder, .FileNameBaseOutbound & ".edi"), "", True)
                .FileName = strFileName
                .FileData = strEDIMsg
                .Save("Save 997 EDI Response Data to file : " & strFileName)
                .Log("Save 997 Complete")
            Catch ex As Exception
                Me.LogError(Me.Source & " Process 997 EDI Response Data Failure", "There was an unexpected error while processing the 997 EDI Response Data for Carrier " & caller.CarrierNumber.ToString & ": " & caller.CarrierName & ".  The actual error is: " & vbCrLf & readExceptionMessage(ex), Me.AdminEmail)
            Finally
                .closeLog(0)
            End Try
        End With

    End Sub

End Class
