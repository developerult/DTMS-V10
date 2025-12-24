Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


Namespace DataTransferObjects
    <DataContract()> _
    Public Class vERPIntegrationSetting
        Inherits DTOBaseClass


#Region " Data Members"

        Private _IntegrationControl As Integer
        <DataMember()> _
        Public Property IntegrationControl() As Integer
            Get
                Return Me._IntegrationControl
            End Get
            Set(value As Integer)
                If ((Me._IntegrationControl = value) _
                            = False) Then
                    Me._IntegrationControl = value
                End If
            End Set
        End Property

        Private _LegalEntity As String
        <DataMember()> _
        Public Property LegalEntity() As String
            Get
                Return Left(Me._LegalEntity, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._LegalEntity, value) = False) Then
                    Me._LegalEntity = Left(value, 50)
                End If
            End Set
        End Property

        Private _ERPExportMaxRetry As Integer = 3
        <DataMember()> _
        Public Property ERPExportMaxRetry() As Integer
            Get
                Return Me._ERPExportMaxRetry
            End Get
            Set(value As Integer)
                If ((Me._ERPExportMaxRetry = value) _
                            = False) Then
                    Me._ERPExportMaxRetry = value
                End If
            End Set
        End Property

        Private _ERPExportRetryMinutes As Integer = 15
        <DataMember()> _
        Public Property ERPExportRetryMinutes() As Integer
            Get
                Return Me._ERPExportRetryMinutes
            End Get
            Set(value As Integer)
                If ((Me._ERPExportRetryMinutes = value) _
                            = False) Then
                    Me._ERPExportRetryMinutes = value
                End If
            End Set
        End Property

        Private _ERPExportMaxRowsReturned As Integer = 100
        <DataMember()> _
        Public Property ERPExportMaxRowsReturned() As Integer
            Get
                Return Me._ERPExportMaxRowsReturned
            End Get
            Set(value As Integer)
                If ((Me._ERPExportMaxRowsReturned = value) _
                            = False) Then
                    Me._ERPExportMaxRowsReturned = value
                End If
            End Set
        End Property

        Private _ERPExportAutoConfirmation As Boolean = False
        <DataMember()> _
        Public Property ERPExportAutoConfirmation() As Boolean
            Get
                Return Me._ERPExportAutoConfirmation
            End Get
            Set(value As Boolean)
                If ((Me._ERPExportAutoConfirmation = value) _
                            = False) Then
                    Me._ERPExportAutoConfirmation = value
                End If
            End Set
        End Property

        Private _ERPSettingDescription As String
        <DataMember()> _
        Public Property ERPSettingDescription() As String
            Get
                Return Left(Me._ERPSettingDescription, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPSettingDescription, value) = False) Then
                    Me._ERPSettingDescription = Left(value, 100)
                End If
            End Set
        End Property

        Private _ERPSettingVersion As String
        <DataMember()> _
        Public Property ERPSettingVersion() As String
            Get
                Return Left(Me._ERPSettingVersion, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPSettingVersion, value) = False) Then
                    Me._ERPSettingVersion = Left(value, 10)
                End If
            End Set
        End Property

        Private _ERPAuth As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property ERPAuth() As System.Nullable(Of Integer)
            Get
                Return Me._ERPAuth
            End Get
            Set(value As System.Nullable(Of Integer))
                If (Me._ERPAuth.Equals(value) = False) Then
                    Me._ERPAuth = value
                End If
            End Set
        End Property

        Private _ERPUser As String
        <DataMember()> _
        Public Property ERPUser() As String
            Get
                Return Left(Me._ERPUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPUser, value) = False) Then
                    Me._ERPUser = Left(value, 100)
                End If
            End Set
        End Property

        Private _ERPPassword As String
        ''' <summary>
        ''' Password when a user name is required
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.4.0.003 on 07/13/2021 changed size from 25 to 100
        ''' </remarks>
        <DataMember()> _
        Public Property ERPPassword() As String
            Get
                Return Left(Me._ERPPassword, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPPassword, value) = False) Then
                    Me._ERPPassword = Left(value, 100)
                End If
            End Set
        End Property

        Private _ERPCertificate As String
        <DataMember()> _
        Public Property ERPCertificate() As String
            Get
                Return Left(Me._ERPCertificate, 1024)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPCertificate, value) = False) Then
                    Me._ERPCertificate = Left(value, 1024)
                End If
            End Set
        End Property

        Private _ERPTypeControl As Integer
        <DataMember()> _
        Public Property ERPTypeControl() As Integer
            Get
                Return Me._ERPTypeControl
            End Get
            Set(value As Integer)
                If ((Me._ERPTypeControl = value) _
                            = False) Then
                    Me._ERPTypeControl = value
                End If
            End Set
        End Property

        Private _ERPTypeName As String
        <DataMember()> _
        Public Property ERPTypeName() As String
            Get
                Return Left(Me._ERPTypeName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPTypeName, value) = False) Then
                    Me._ERPTypeName = Left(value, 50)
                End If
            End Set
        End Property

        Private _IntegrationTypeControl As Integer
        <DataMember()> _
        Public Property IntegrationTypeControl() As Integer
            Get
                Return Me._IntegrationTypeControl
            End Get
            Set(value As Integer)
                If ((Me._IntegrationTypeControl = value) _
                            = False) Then
                    Me._IntegrationTypeControl = value
                End If
            End Set
        End Property

        Private _IntegrationTypeName As String
        <DataMember()> _
        Public Property IntegrationTypeName() As String
            Get
                Return Left(Me._IntegrationTypeName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._IntegrationTypeName, value) = False) Then
                    Me._IntegrationTypeName = Left(value, 50)
                End If
            End Set
        End Property

        Private _ERPURI As String
        <DataMember()> _
        Public Property ERPURI() As String
            Get
                Return Left(Me._ERPURI, 250)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPURI, value) = False) Then
                    Me._ERPURI = Left(value, 250)
                End If
            End Set
        End Property

        Private _ERPAuthUser As String
        ''' <summary>
        ''' maps to OAuth 2 Client ID or User Name
        ''' </summary>
        ''' <returns></returns>
        <DataMember()> _
        Public Property ERPAuthUser() As String
            Get
                Return Left(Me._ERPAuthUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPAuthUser, value) = False) Then
                    Me._ERPAuthUser = Left(value, 100)
                End If
            End Set
        End Property

        Private _ERPAuthPassword As String
        ''' <summary>
        ''' Maps to OAuth 2 Secret or user password
        ''' </summary>
        ''' <returns></returns>
        <DataMember()> _
        Public Property ERPAuthPassword() As String
            Get
                Return Left(Me._ERPAuthPassword, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPAuthPassword, value) = False) Then
                    Me._ERPAuthPassword = Left(value, 100)
                End If
            End Set
        End Property

        Private _ERPAuthCode As String
        ''' <summary>
        ''' Legacy ERP Auto Code
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.004 on 11/13/2023  in crease to 100 characters from 20
        ''' </remarks>
        <DataMember()> _
        Public Property ERPAuthCode() As String
            Get
                Return Left(Me._ERPAuthCode, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPAuthCode, value) = False) Then
                    Me._ERPAuthCode = Left(value, 100)
                End If
            End Set
        End Property

        Private _ERPNotes As String
        <DataMember()> _
        Public Property ERPNotes() As String
            Get
                Return Left(Me._ERPNotes, 250)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPNotes, value) = False) Then
                    Me._ERPNotes = Left(value, 250)
                End If
            End Set
        End Property

        Private _TMSURI As String
        <DataMember()> _
        Public Property TMSURI() As String
            Get
                Return Left(Me._TMSURI, 250)
            End Get
            Set(value As String)
                If (String.Equals(Me._TMSURI, value) = False) Then
                    Me._TMSURI = Left(value, 250)
                End If
            End Set
        End Property

        Private _TMSAuthUser As String
        <DataMember()> _
        Public Property TMSAuthUser() As String
            Get
                Return Left(Me._TMSAuthUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._TMSAuthUser, value) = False) Then
                    Me._TMSAuthUser = Left(value, 100)
                End If
            End Set
        End Property

        Private _TMSAuthPassword As String
        <DataMember()> _
        Public Property TMSAuthPassword() As String
            Get
                Return Left(Me._TMSAuthPassword, 25)
            End Get
            Set(value As String)
                If (String.Equals(Me._TMSAuthPassword, value) = False) Then
                    Me._TMSAuthPassword = Left(value, 25)
                End If
            End Set
        End Property

        Private _TMSAuthCode As String
        ''' <summary>
        ''' Legacy TMS web servies Auth Code
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.004 on 11/13/2023  in crease to 100 characters from 20
        ''' </remarks>
        <DataMember()> _
        Public Property TMSAuthCode() As String
            Get
                Return Left(Me._TMSAuthCode, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._TMSAuthCode, value) = False) Then
                    Me._TMSAuthCode = Left(value, 100)
                End If
            End Set
        End Property

        Private _TMSNotes As String
        <DataMember()>
        Public Property TMSNotes() As String
            Get
                Return Left(Me._TMSNotes, 250)
            End Get
            Set(value As String)
                If (String.Equals(Me._TMSNotes, value) = False) Then
                    Me._TMSNotes = Left(value, 250)
                End If
            End Set
        End Property


        Private _ERPAuthURI As String
        ''' <summary>
        ''' maps to ERP Auth (token) URI if needed
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.004 on 11/13/2023 
        ''' </remarks>
        <DataMember()>
        Public Property ERPAuthURI() As String
            Get
                Return Left(Me._ERPAuthURI, 1024)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPAuthURI, value) = False) Then
                    Me._ERPAuthURI = Left(value, 1024)
                End If
            End Set
        End Property

        Private _TMSAuthURI As String
        ''' <summary>
        ''' maps to TMS Auth (token) URI if needed
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.004 on 11/13/2023 
        ''' </remarks>
        <DataMember()>
        Public Property TMSAuthURI() As String
            Get
                Return Left(Me._TMSAuthURI, 1024)
            End Get
            Set(value As String)
                If (String.Equals(Me._TMSAuthURI, value) = False) Then
                    Me._TMSAuthURI = Left(value, 1024)
                End If
            End Set
        End Property

        Private _TMSScopeURI As String
        ''' <summary>
        ''' maps to TMS Scope URI if needed
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.004 on 11/13/2023 
        ''' </remarks>
        <DataMember()>
        Public Property TMSScopeURI() As String
            Get
                Return Left(Me._TMSScopeURI, 1024)
            End Get
            Set(value As String)
                If (String.Equals(Me._TMSScopeURI, value) = False) Then
                    Me._TMSScopeURI = Left(value, 1024)
                End If
            End Set
        End Property


        Private _TMSActionURI As String
        ''' <summary>
        ''' maps to TMS Scope URI if needed
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.004 on 11/13/2023 
        ''' </remarks>
        <DataMember()>
        Public Property TMSActionURI() As String
            Get
                Return Left(Me._TMSActionURI, 1024)
            End Get
            Set(value As String)
                If (String.Equals(Me._TMSActionURI, value) = False) Then
                    Me._TMSActionURI = Left(value, 1024)
                End If
            End Set
        End Property

        Private _ERPActionURI As String
        ''' <summary>
        ''' maps to ERP Action URI if needed
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.004 on 11/13/2023 
        ''' </remarks>
        <DataMember()>
        Public Property ERPActionURI() As String
            Get
                Return Left(Me._ERPActionURI, 1024)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPActionURI, value) = False) Then
                    Me._ERPActionURI = Left(value, 1024)
                End If
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New vERPIntegrationSetting
            instance = DirectCast(MemberwiseClone(), vERPIntegrationSetting)
            Return instance
        End Function
#End Region
    End Class

End Namespace

