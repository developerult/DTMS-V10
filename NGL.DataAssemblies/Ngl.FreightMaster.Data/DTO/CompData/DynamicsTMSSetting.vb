Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class DynamicsTMSSetting
        Inherits DTOBaseClass


#Region " Data Members"
        Private _DTMSControl As Integer = 0
        <DataMember()> _
        Public Property DTMSControl() As Integer
            Get
                Return _DTMSControl
            End Get
            Set(ByVal value As Integer)
                _DTMSControl = value
            End Set
        End Property

        Private _DTMSLegalEntity As String = ""
        <DataMember()> _
        Public Property DTMSLegalEntity() As String
            Get
                Return Left(_DTMSLegalEntity, 50)
            End Get
            Set(ByVal value As String)
                _DTMSLegalEntity = Left(value, 50)
            End Set
        End Property


        Private _DTMSPicklistMaxRetry As Integer = 3
        <DataMember()> _
        Public Property DTMSPicklistMaxRetry() As Integer
            Get
                Return _DTMSPicklistMaxRetry
            End Get
            Set(ByVal value As Integer)
                _DTMSPicklistMaxRetry = value
            End Set
        End Property

        Private _DTMSPicklistRetryMinutes As Integer = 15
        <DataMember()> _
        Public Property DTMSPicklistRetryMinutes() As Integer
            Get
                Return _DTMSPicklistRetryMinutes
            End Get
            Set(ByVal value As Integer)
                _DTMSPicklistRetryMinutes = value
            End Set
        End Property

        Private _DTMSPicklistMaxRowsReturned As Integer = 100
        <DataMember()> _
        Public Property DTMSPicklistMaxRowsReturned() As Integer
            Get
                Return _DTMSPicklistMaxRowsReturned
            End Get
            Set(ByVal value As Integer)
                _DTMSPicklistMaxRowsReturned = value
            End Set
        End Property

        Private _DTMSPicklistAutoConfirmation As Boolean = False
        <DataMember()> _
        Public Property DTMSPicklistAutoConfirmation() As Boolean
            Get
                Return _DTMSPicklistAutoConfirmation
            End Get
            Set(ByVal value As Boolean)
                _DTMSPicklistAutoConfirmation = value
            End Set
        End Property

        Private _DTMSAPExportMaxRetry As Integer = 3
        <DataMember()> _
        Public Property DTMSAPExportMaxRetry() As Integer
            Get
                Return _DTMSAPExportMaxRetry
            End Get
            Set(ByVal value As Integer)
                _DTMSAPExportMaxRetry = value
            End Set
        End Property

        Private _DTMSAPExportRetryMinutes As Integer = 15
        <DataMember()> _
        Public Property DTMSAPExportRetryMinutes() As Integer
            Get
                Return _DTMSAPExportRetryMinutes
            End Get
            Set(ByVal value As Integer)
                _DTMSAPExportRetryMinutes = value
            End Set
        End Property

        Private _DTMSAPExportMaxRowsReturned As Integer = 100
        <DataMember()> _
        Public Property DTMSAPExportMaxRowsReturned() As Integer
            Get
                Return _DTMSAPExportMaxRowsReturned
            End Get
            Set(ByVal value As Integer)
                _DTMSAPExportMaxRowsReturned = value
            End Set
        End Property

        Private _DTMSAPExportAutoConfirmation As Boolean = False
        <DataMember()> _
        Public Property DTMSAPExportAutoConfirmation() As Boolean
            Get
                Return _DTMSAPExportAutoConfirmation
            End Get
            Set(ByVal value As Boolean)
                _DTMSAPExportAutoConfirmation = value
            End Set
        End Property

        Private _DTMSNAVWebServiceURL As String = ""
        <DataMember()> _
        Public Property DTMSNAVWebServiceURL() As String
            Get
                Return Left(_DTMSNAVWebServiceURL, 1000)
            End Get
            Set(ByVal value As String)
                _DTMSNAVWebServiceURL = Left(value, 1000)
            End Set
        End Property

        Private _DTMSNAVUserName As String = ""
        <DataMember()> _
        Public Property DTMSNAVUserName() As String
            Get
                Return Left(_DTMSNAVUserName, 100)
            End Get
            Set(ByVal value As String)
                _DTMSNAVUserName = Left(value, 100)
            End Set
        End Property

        Private _DTMSNAVPassword As String = ""
        <DataMember()> _
        Public Property DTMSNAVPassword() As String
            Get
                Return Left(_DTMSNAVPassword, 100)
            End Get
            Set(ByVal value As String)
                _DTMSNAVPassword = Left(value, 100)
            End Set
        End Property

        Private _DTMSNAVUseDefaultCredentials As Boolean = True
        <DataMember()> _
        Public Property DTMSNAVUseDefaultCredentials() As Boolean
            Get
                Return _DTMSNAVUseDefaultCredentials
            End Get
            Set(ByVal value As Boolean)
                _DTMSNAVUseDefaultCredentials = value
            End Set
        End Property

        Private _DTMSGPWebServiceURL As String = ""
        <DataMember()> _
        Public Property DTMSGPWebServiceURL() As String
            Get
                Return Left(_DTMSGPWebServiceURL, 1000)
            End Get
            Set(ByVal value As String)
                _DTMSGPWebServiceURL = Left(value, 1000)
            End Set
        End Property

        Private _DTMSGPUserName As String = ""
        <DataMember()> _
        Public Property DTMSGPUserName() As String
            Get
                Return Left(_DTMSGPUserName, 100)
            End Get
            Set(ByVal value As String)
                _DTMSGPUserName = Left(value, 100)
            End Set
        End Property

        Private _DTMSGPPassword As String = ""
        <DataMember()> _
        Public Property DTMSGPPassword() As String
            Get
                Return Left(_DTMSGPPassword, 100)
            End Get
            Set(ByVal value As String)
                _DTMSGPPassword = Left(value, 100)
            End Set
        End Property

        Private _DTMSGPUseDefaultCredentials As Boolean = True
        <DataMember()> _
        Public Property DTMSGPUseDefaultCredentials() As Boolean
            Get
                Return _DTMSGPUseDefaultCredentials
            End Get
            Set(ByVal value As Boolean)
                _DTMSGPUseDefaultCredentials = value
            End Set
        End Property

        Private _DTMSAXWebServiceURL As String = ""
        <DataMember()> _
        Public Property DTMSAXWebServiceURL() As String
            Get
                Return Left(_DTMSAXWebServiceURL, 1000)
            End Get
            Set(ByVal value As String)
                _DTMSAXWebServiceURL = Left(value, 1000)
            End Set
        End Property

        Private _DTMSAXUserName As String = ""
        <DataMember()> _
        Public Property DTMSAXUserName() As String
            Get
                Return Left(_DTMSAXUserName, 100)
            End Get
            Set(ByVal value As String)
                _DTMSAXUserName = Left(value, 100)
            End Set
        End Property

        Private _DTMSAXPassword As String = ""
        <DataMember()> _
        Public Property DTMSAXPassword() As String
            Get
                Return Left(_DTMSAXPassword, 100)
            End Get
            Set(ByVal value As String)
                _DTMSAXPassword = Left(value, 100)
            End Set
        End Property

        Private _DTMSAXUseDefaultCredentials As Boolean = True
        <DataMember()> _
        Public Property DTMSAXUseDefaultCredentials() As Boolean
            Get
                Return _DTMSAXUseDefaultCredentials
            End Get
            Set(ByVal value As Boolean)
                _DTMSAXUseDefaultCredentials = value
            End Set
        End Property

        Private _DTMSWSAuthCode As String = "WSPROD"
        <DataMember()> _
        Public Property DTMSWSAuthCode() As String
            Get
                Return Left(_DTMSWSAuthCode, 20)
            End Get
            Set(ByVal value As String)
                _DTMSWSAuthCode = Left(value, 20)
            End Set
        End Property

        Private _DTMSWSURL As String = ""
        <DataMember()> _
        Public Property DTMSWSURL() As String
            Get
                Return Left(_DTMSWSURL, 1000)
            End Get
            Set(ByVal value As String)
                _DTMSWSURL = Left(value, 1000)
            End Set
        End Property

        Private _DTMSWCFAuthCode As String = "WCFPROD"
        <DataMember()> _
        Public Property DTMSWCFAuthCode() As String
            Get
                Return Left(_DTMSWCFAuthCode, 20)
            End Get
            Set(ByVal value As String)
                _DTMSWCFAuthCode = Left(value, 20)
            End Set
        End Property

        Private _DTMSWCFURL As String = ""
        <DataMember()> _
        Public Property DTMSWCFURL() As String
            Get
                Return Left(_DTMSWCFURL, 1000)
            End Get
            Set(ByVal value As String)
                _DTMSWCFURL = Left(value, 1000)
            End Set
        End Property

        Private _DTMSWCFTCPURL As String = ""
        <DataMember()> _
        Public Property DTMSWCFTCPURL() As String
            Get
                Return Left(_DTMSWCFTCPURL, 1000)
            End Get
            Set(ByVal value As String)
                _DTMSWCFTCPURL = Left(value, 1000)
            End Set
        End Property

        Private _DTMSModUser As String = ""
        <DataMember()> _
        Public Property DTMSModUser() As String
            Get
                Return Left(_DTMSModUser, 100)
            End Get
            Set(ByVal value As String)
                _DTMSModUser = Left(value, 100)
            End Set
        End Property

        Private _DTMSModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property DTMSModDate() As System.Nullable(Of Date)
            Get
                Return _DTMSModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _DTMSModDate = value
            End Set
        End Property

        Private _DTMSUpdated As Byte()
        <DataMember()> _
        Public Property DTMSUpdated() As Byte()
            Get
                Return _DTMSUpdated
            End Get
            Set(ByVal value As Byte())
                _DTMSUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New DynamicsTMSSetting
            instance = DirectCast(MemberwiseClone(), DynamicsTMSSetting)
            Return instance
        End Function

#End Region

    End Class
End Namespace