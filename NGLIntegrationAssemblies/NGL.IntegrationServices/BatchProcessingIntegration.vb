Imports System
Imports System.IO
Imports System.Linq
Imports System.ServiceModel
Imports NGL.FMWCFProxy.NGLBatchProcessingData
 
Public Class BatchProcessingIntegration

    Public LastError As String = ""

    Public Enum WebServiceReturnValues
        nglDataIntegrationComplete
        nglDataConnectionFailure
        nglDataValidationFailure
        nglDataIntegrationFailure
        nglDataIntegrationHadErrors
    End Enum

    Private wcfURL As String

    Private parms As WCFParameters
    Public Property WCFParameters() As WCFParameters
        Get
            Dim oWFCParameters As New WCFParameters
            With oWFCParameters
                .Database = parms.Database
                .DBServer = parms.DBServer
                .UserName = "nglweb"
                .WCFAuthCode = parms.WCFAuthCode
            End With
            Return oWFCParameters
        End Get
        Set(value As WCFParameters)

        End Set
    End Property

    Public Function getScalarInteger(ByVal strSQL As String) As Integer
        Dim intRet As Integer = 0
        Dim blnSuccess As Boolean = False
        'clear the last error
        LastError = ""
        Try
            Dim proxy As INGLBatchProcessingData = getNGLBatchProcessingDataProxy()
            With CType(proxy, ICommunicationObject)
                Try
                    intRet = proxy.returnScalarInteger(strSQL, WCFParameters)
                    If .State <> CommunicationState.Faulted Then
                        .Close()
                        blnSuccess = True
                    End If
                Catch sqlEx As FaultException(Of SqlFaultInfo)
                    Me.LastError = sqlEx.Reason.ToString & ": " & sqlEx.Detail.Message
                Catch timeoutEx As TimeoutException
                    Me.LastError = "WCF Operation Timed Out"
                Catch WCFEx As System.ServiceModel.ProtocolException
                    Me.LastError = "WCF Protocol Exception: " & WCFEx.Message
                Catch WCFEx As System.ServiceModel.CommunicationException
                    Me.LastError = "WCF Communicaiton Failure: " & WCFEx.Message
                Catch ex As Exception
                    Throw
                Finally
                    If Not blnSuccess Then
                        .Abort()
                    End If
                End Try
            End With
        Catch ex As Exception
            Throw
        End Try
        Return intRet
    End Function

    Public Function hasOrderChanged(ByVal OrderNumber As String, _
                                    ByVal LaneNumber As String, _
                                    ByVal BookDateLoad As String, _
                                    ByVal BookTotalWgt As Double, _
                                    ByVal BookTotalCases As Integer, _
                                    ByVal BookTotalPL As Integer, _
                                    ByVal PONumber As String, _
                                    ByVal BookItemNumbers As String, _
                                    ByVal NumberOfItems As Integer) As Boolean
        Dim blnRet As Boolean = True
        Dim blnSuccess As Boolean = False
        'clear the last error
        LastError = ""
        Try
            Dim proxy As INGLBatchProcessingData = getNGLBatchProcessingDataProxy()
            With CType(proxy, ICommunicationObject)
                Try
                    blnRet = proxy.hasOrderChanged(OrderNumber, LaneNumber, BookDateLoad, BookTotalWgt, BookTotalCases, BookTotalPL, PONumber, BookItemNumbers, NumberOfItems, WCFParameters)
                    If .State <> CommunicationState.Faulted Then
                        .Close()
                        blnSuccess = True
                    End If
                Catch sqlEx As FaultException(Of SqlFaultInfo)
                    Me.LastError = sqlEx.Reason.ToString & ": " & sqlEx.Detail.Message
                Catch timeoutEx As TimeoutException
                    Me.LastError = "WCF Operation Timed Out"
                Catch WCFEx As System.ServiceModel.ProtocolException
                    Me.LastError = "WCF Protocol Exception: " & WCFEx.Message
                Catch WCFEx As System.ServiceModel.CommunicationException
                    Me.LastError = "WCF Communicaiton Failure: " & WCFEx.Message
                Catch ex As Exception
                    Throw
                Finally
                    If Not blnSuccess Then
                        .Abort()
                    End If
                End Try
            End With
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    Public Function SendToNGLEmailService(ByVal MailFrom As String, _
                                ByVal EmailTo As String, _
                                ByVal CCEmail As String, _
                                ByVal Subject As String, _
                                ByVal Body As String) As Boolean
        Dim blnRet As Boolean = True
        Dim blnSuccess As Boolean = False
        'clear the last error
        LastError = ""
        Try
            Dim proxy As INGLBatchProcessingData = getNGLBatchProcessingDataProxy()
            With CType(proxy, ICommunicationObject)
                Try
                    blnRet = proxy.executeGenerateEmail2Way(MailFrom, EmailTo, CCEmail, Subject, Body, WCFParameters)
                    If .State <> CommunicationState.Faulted Then
                        .Close()
                        blnSuccess = True
                    End If
                Catch sqlEx As FaultException(Of SqlFaultInfo)
                    Me.LastError = sqlEx.Reason.ToString & ": " & sqlEx.Detail.Message
                Catch timeoutEx As TimeoutException
                    Me.LastError = "WCF Operation Timed Out"
                Catch WCFEx As System.ServiceModel.ProtocolException
                    Me.LastError = "WCF Protocol Exception: " & WCFEx.Message
                Catch WCFEx As System.ServiceModel.CommunicationException
                    Me.LastError = "WCF Communicaiton Failure: " & WCFEx.Message
                Catch ex As Exception
                    Throw
                Finally
                    If Not blnSuccess Then
                        .Abort()
                    End If
                End Try
            End With
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    Public Function getScalarString(ByVal strSQL As String) As String
        Dim strRet As String = ""
        Dim blnSuccess As Boolean = False
        Try
            Dim proxy As INGLBatchProcessingData = getNGLBatchProcessingDataProxy()
            With CType(proxy, ICommunicationObject)
                Try
                    strRet = proxy.returnScalarString(strSQL, WCFParameters)
                    If .State <> CommunicationState.Faulted Then
                        .Close()
                        blnSuccess = True
                    End If
                Catch sqlEx As FaultException(Of SqlFaultInfo)
                    Me.LastError = sqlEx.Reason.ToString & ": " & sqlEx.Detail.Message
                Catch timeoutEx As TimeoutException
                    Me.LastError = "WCF Operation Timed Out"
                Catch WCFEx As System.ServiceModel.ProtocolException
                    Me.LastError = "WCF Protocol Exception: " & WCFEx.Message
                Catch WCFEx As System.ServiceModel.CommunicationException
                    Me.LastError = "WCF Communicaiton Failure: " & WCFEx.Message
                Catch ex As Exception
                    Throw
                Finally
                    If Not blnSuccess Then
                        .Abort()
                    End If
                End Try
            End With
        Catch ex As Exception
            Throw
        End Try
        Return strRet
    End Function


    Public Function getNGLBatchProcessingDataProxy() As NGLBatchProcessingDataClient
        Dim binding As New BasicHttpBinding
        With binding
            .MaxBufferSize = 10000000
            .MaxReceivedMessageSize = 10000000
            .ReaderQuotas.MaxArrayLength = 10000000
            .ReaderQuotas.MaxStringContentLength = 10000000
        End With
        'Dynamic address assignment used for produciton code
        Return New NGLBatchProcessingDataClient(binding, New EndpointAddress(Me.wcfURL & "/NGLBatchProcessingData.svc"))

    End Function

    Public Sub New(ByVal parms As WCFParameters, ByVal WcfURL As String)
        Me.WCFParameters = parms
        Me.wcfURL = WcfURL
    End Sub

End Class
