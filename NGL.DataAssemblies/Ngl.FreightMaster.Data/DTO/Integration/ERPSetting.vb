Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


' Modified by RHR for v-8.5.4.004 on 11/13/2023 
Namespace DataTransferObjects
    <DataContract()> _
    Public Class ERPSetting
        Inherits DTOBaseClass

#Region " Data Members"
        Private _ERPSettingControl As Integer
        <DataMember()> _
        Public Property ERPSettingControl() As Integer
            Get
                Return _ERPSettingControl
            End Get
            Set(ByVal value As Integer)
                _ERPSettingControl = value
            End Set
        End Property


        Private _LegalEntity As String
        <DataMember()> _
        Public Property LegalEntity() As String
            Get
                Return Left(_LegalEntity, 50)
            End Get
            Set(ByVal value As String)
                _LegalEntity = Left(value, 50)
            End Set
        End Property


        Private _ERPTypeControl As Integer
        <DataMember()> _
        Public Property ERPTypeControl() As Integer
            Get
                Return _ERPTypeControl
            End Get
            Set(ByVal value As Integer)
                _ERPTypeControl = value
            End Set
        End Property


        Private _Description As String
        <DataMember()> _
        Public Property Description() As String
            Get
                Return Left(_Description, 100)
            End Get
            Set(ByVal value As String)
                _Description = Left(value, 100)
            End Set
        End Property


        Private _Version As String
        <DataMember()> _
        Public Property Version() As String
            Get
                Return Left(_Version, 10)
            End Get
            Set(ByVal value As String)
                _Version = Left(value, 10)
            End Set
        End Property


        Private _ERPAuth As Integer
        <DataMember()> _
        Public Property ERPAuth() As Integer
            Get
                Return _ERPAuth
            End Get
            Set(ByVal value As Integer)
                _ERPAuth = value
            End Set
        End Property


        Private _ERPUser As String
        <DataMember()> _
        Public Property ERPUser() As String
            Get
                Return Left(_ERPUser, 100)
            End Get
            Set(ByVal value As String)
                _ERPUser = Left(value, 100)
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
                Return Left(_ERPPassword, 100)
            End Get
            Set(ByVal value As String)
                _ERPPassword = Left(value, 100)
            End Set
        End Property

        Private _ERPCertificate As String
        <DataMember()> _
        Public Property ERPCertificate() As String
            Get
                Return Left(_ERPCertificate, 1024)
            End Get
            Set(ByVal value As String)
                _ERPCertificate = Left(value, 1024)
            End Set
        End Property
       
        Private _ERPSettingModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ERPSettingModDate() As System.Nullable(Of Date)
            Get
                Return Me._ERPSettingModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._ERPSettingModDate.Equals(value) = False) Then
                    Me._ERPSettingModDate = value
                    Me.SendPropertyChanged("ERPSettingModDate")
                End If
            End Set
        End Property


        Private _ERPSettingModUser As String
        <DataMember()> _
        Public Property ERPSettingModUser() As String
            Get
                Return Left(_ERPSettingModUser, 100)
            End Get
            Set(ByVal value As String)
                _ERPSettingModUser = Left(value, 100)
            End Set
        End Property

        Private _ERPSettingUpdated As Byte()
        <DataMember()> _
        Public Property ERPSettingUpdated() As Byte()
            Get
                Return Me._ERPSettingUpdated
            End Get
            Set(value As Byte())
                Me._ERPSettingUpdated = value
            End Set
        End Property

        Private _ERPExportMaxRetry As Integer = 3
        <DataMember()> _
        Public Property ERPExportMaxRetry() As Integer
            Get
                Return _ERPExportMaxRetry
            End Get
            Set(ByVal value As Integer)
                _ERPExportMaxRetry = value
            End Set
        End Property

        Private _ERPExportRetryMinutes As Integer = 15
        <DataMember()> _
        Public Property ERPExportRetryMinutes() As Integer
            Get
                Return _ERPExportRetryMinutes
            End Get
            Set(ByVal value As Integer)
                _ERPExportRetryMinutes = value
            End Set
        End Property

        Private _ERPExportMaxRowsReturned As Integer = 100
        <DataMember()> _
        Public Property ERPExportMaxRowsReturned() As Integer
            Get
                Return _ERPExportMaxRowsReturned
            End Get
            Set(ByVal value As Integer)
                _ERPExportMaxRowsReturned = value
            End Set
        End Property

        Private _ERPExportAutoConfirmation As Boolean = False
        <DataMember()>
        Public Property ERPExportAutoConfirmation() As Boolean
            Get
                Return _ERPExportAutoConfirmation
            End Get
            Set(ByVal value As Boolean)
                _ERPExportAutoConfirmation = value
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



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New ERPSetting

            instance = DirectCast(MemberwiseClone(), ERPSetting)
            Return instance
        End Function

#End Region

    End Class
End Namespace
 
