''' <summary>
''' 
''' </summary>
''' <remarks>
''' Modified by RHR for v-8.4.0.003 on 10/19/2021
'''   added AP and Pick Retry Settings
''' </remarks>
Public Class clsDefaultIntegrationConfiguration

#Region "Properties"

    Private _ERPTestLegalEntity As String

    Public Property ERPTestLegalEntity() As String
        Get
            Return Left(Me._ERPTestLegalEntity, 50)
        End Get
        Set(value As String)
            If (String.Equals(Me._ERPTestLegalEntity, value) = False) Then
                Me._ERPTestLegalEntity = Left(value, 50)
            End If
        End Set
    End Property

    Private _ERPTestExportMaxRetry As Integer = 1

    Public Property ERPTestExportMaxRetry() As Integer
        Get
            Return Me._ERPTestExportMaxRetry
        End Get
        Set(value As Integer)
            If ((Me._ERPTestExportMaxRetry = value) _
                        = False) Then
                Me._ERPTestExportMaxRetry = value
            End If
        End Set
    End Property

    Private _ERPTestExportRetryMinutes As Integer = 15

    Public Property ERPTestExportRetryMinutes() As Integer
        Get
            Return Me._ERPTestExportRetryMinutes
        End Get
        Set(value As Integer)
            If ((Me._ERPTestExportRetryMinutes = value) _
                        = False) Then
                Me._ERPTestExportRetryMinutes = value
            End If
        End Set
    End Property

    Private _ERPTestExportMaxRowsReturned As Integer = 100

    Public Property ERPTestExportMaxRowsReturned() As Integer
        Get
            Return Me._ERPTestExportMaxRowsReturned
        End Get
        Set(value As Integer)
            If ((Me._ERPTestExportMaxRowsReturned = value) _
                        = False) Then
                Me._ERPTestExportMaxRowsReturned = value
            End If
        End Set
    End Property

    Private _ERPTestExportAutoConfirmation As Boolean = False

    Public Property ERPTestExportAutoConfirmation() As Boolean
        Get
            Return Me._ERPTestExportAutoConfirmation
        End Get
        Set(value As Boolean)
            If ((Me._ERPTestExportAutoConfirmation = value) _
                        = False) Then
                Me._ERPTestExportAutoConfirmation = value
            End If
        End Set
    End Property

    Private _ERPTestFreightCost As Double = 1000.0
    Public Property ERPTestFreightCost() As Double
        Get
            Return _ERPTestFreightCost
        End Get
        Set(ByVal value As Double)
            _ERPTestFreightCost = value
        End Set
    End Property

    Private _ERPTestFreightBillNumber As String = "FB Unit Test"
    Public Property ERPTestFreightBillNumber() As String
        Get
            Return _ERPTestFreightBillNumber
        End Get
        Set(ByVal value As String)
            _ERPTestFreightBillNumber = value
        End Set
    End Property

    Private _ERPTestURI As String
    Public Property ERPTestURI() As String
        Get
            Return Left(Me._ERPTestURI, 250)
        End Get
        Set(value As String)
            If (String.Equals(Me._ERPTestURI, value) = False) Then
                Me._ERPTestURI = Left(value, 250)
            End If
        End Set
    End Property

    Private _ERPTestAuthUser As String

    Public Property ERPTestAuthUser() As String
        Get
            Return Left(Me._ERPTestAuthUser, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._ERPTestAuthUser, value) = False) Then
                Me._ERPTestAuthUser = Left(value, 100)
            End If
        End Set
    End Property

    Private _ERPTestAuthPassword As String
    ''' <summary>
    ''' Password when a user name is required
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/13/2021 changed size from 25 to 100
    ''' </remarks>
    Public Property ERPTestAuthPassword() As String
        Get
            Return Left(Me._ERPTestAuthPassword, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._ERPTestAuthPassword, value) = False) Then
                Me._ERPTestAuthPassword = Left(value, 100)
            End If
        End Set
    End Property

    Private _ERPTestAuthCode As String

    Public Property ERPTestAuthCode() As String
        Get
            Return Left(Me._ERPTestAuthCode, 20)
        End Get
        Set(value As String)
            If (String.Equals(Me._ERPTestAuthCode, value) = False) Then
                Me._ERPTestAuthCode = Left(value, 20)
            End If
        End Set
    End Property

    Private _RunERPTest As Boolean = True
    Public Property RunERPTest() As Boolean
        Get
            Return _RunERPTest
        End Get
        Set(ByVal value As Boolean)
            _RunERPTest = value
        End Set
    End Property

    Private _TMSTestServiceURI As String
    Public Property TMSTestServiceURI() As String
        Get
            Return Left(Me._TMSTestServiceURI, 250)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSTestServiceURI, value) = False) Then
                Me._TMSTestServiceURI = Left(value, 250)
            End If
        End Set
    End Property

    Private _TMSTestServiceAuthUser As String

    Public Property TMSTestServiceAuthUser() As String
        Get
            Return Left(Me._TMSTestServiceAuthUser, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSTestServiceAuthUser, value) = False) Then
                Me._TMSTestServiceAuthUser = Left(value, 100)
            End If
        End Set
    End Property

    Private _TMSTestServiceAuthPassword As String
    ''' <summary>
    ''' Password when a user name is required
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/13/2021 changed size from 25 to 100
    ''' </remarks>
    Public Property TMSTestServiceAuthPassword() As String
        Get
            Return Left(Me._TMSTestServiceAuthPassword, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSTestServiceAuthPassword, value) = False) Then
                Me._TMSTestServiceAuthPassword = Left(value, 100)
            End If
        End Set
    End Property

    Private _TMSTestServiceAuthCode As String

    Public Property TMSTestServiceAuthCode() As String
        Get
            Return Left(Me._TMSTestServiceAuthCode, 20)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSTestServiceAuthCode, value) = False) Then
                Me._TMSTestServiceAuthCode = Left(value, 20)
            End If
        End Set
    End Property

    Private _TMSSettingsURI As String
    Public Property TMSSettingsURI() As String
        Get
            Return Left(Me._TMSSettingsURI, 250)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSSettingsURI, value) = False) Then
                Me._TMSSettingsURI = Left(value, 250)
            End If
        End Set
    End Property

    Private _TMSSettingsAuthUser As String

    Public Property TMSSettingsAuthUser() As String
        Get
            Return Left(Me._TMSSettingsAuthUser, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSSettingsAuthUser, value) = False) Then
                Me._TMSSettingsAuthUser = Left(value, 100)
            End If
        End Set
    End Property

    Private _TMSSettingsAuthPassword As String
    ''' <summary>
    ''' Password when a user name is required
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/13/2021 changed size from 25 to 100
    ''' </remarks>
    Public Property TMSSettingsAuthPassword() As String
        Get
            Return Left(Me._TMSSettingsAuthPassword, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSSettingsAuthPassword, value) = False) Then
                Me._TMSSettingsAuthPassword = Left(value, 100)
            End If
        End Set
    End Property

    Private _TMSSettingsAuthCode As String

    Public Property TMSSettingsAuthCode() As String
        Get
            Return Left(Me._TMSSettingsAuthCode, 20)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSSettingsAuthCode, value) = False) Then
                Me._TMSSettingsAuthCode = Left(value, 20)
            End If
        End Set
    End Property

    Private _Debug As Boolean = False
    Public Property Debug() As Boolean
        Get
            Return _Debug
        End Get
        Set(ByVal value As Boolean)
            _Debug = value
        End Set
    End Property

    Private _Verbos As Boolean = False
    Public Property Verbos() As Boolean
        Get
            Return _Verbos
        End Get
        Set(ByVal value As Boolean)
            _Verbos = value
        End Set
    End Property

    Private _TMSDBName As String
    Public Property TMSDBName() As String
        Get
            Return _TMSDBName
        End Get
        Set(value As String)
            _TMSDBName = value
        End Set
    End Property

    Private _TMSDBServer As String
    Public Property TMSDBServer() As String
        Get
            Return _TMSDBServer
        End Get
        Set(value As String)
            _TMSDBServer = value
        End Set
    End Property

    Private _TMSDBUser As String
    Public Property TMSDBUser() As String
        Get
            Return _TMSDBUser
        End Get
        Set(value As String)
            _TMSDBUser = value
        End Set
    End Property

    Private _TMSDBPass As String
    Public Property TMSDBPass() As String
        Get
            Return _TMSDBPass
        End Get
        Set(value As String)
            _TMSDBPass = value
        End Set
    End Property

    Private _TMSRunLegalEntity As String
    Public Property TMSRunLegalEntity() As String
        Get
            Return Left(_TMSRunLegalEntity, 50)
        End Get
        Set(value As String)
            _TMSRunLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _ERPTypeName As String
    Public Property ERPTypeName() As String
        Get
            Return _ERPTypeName
        End Get
        Set(value As String)
            _ERPTypeName = value
        End Set
    End Property

    ' Begin Modified by RHR for v-8.4.0.003 on 10/19/2021
    Private _TMSAPRetryMilliSeconds As String
    Public Property TMSAPRetryMilliSeconds() As String
        Get
            Return _TMSAPRetryMilliSeconds
        End Get
        Set(value As String)
            _TMSAPRetryMilliSeconds = value
        End Set
    End Property

    Private _TMSAPRetryAttempts As String
    Public Property TMSAPRetryAttempts() As String
        Get
            Return _TMSAPRetryAttempts
        End Get
        Set(value As String)
            _TMSAPRetryAttempts = value
        End Set
    End Property

    Private _TMSPickRetryMilliSeconds As String
    Public Property TMSPickRetryMilliSeconds() As String
        Get
            Return _TMSPickRetryMilliSeconds
        End Get
        Set(value As String)
            _TMSPickRetryMilliSeconds = value
        End Set
    End Property

    Private _TMSPickRetryAttempts As String
    Public Property TMSPickRetryAttempts() As String
        Get
            Return _TMSPickRetryAttempts
        End Get
        Set(value As String)
            _TMSPickRetryAttempts = value
        End Set
    End Property
    ' End Modified by RHR for v-8.4.0.003 on 10/19/2021

#End Region

End Class
