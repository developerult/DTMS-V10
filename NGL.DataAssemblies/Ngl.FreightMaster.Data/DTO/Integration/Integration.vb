Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports Serilog

'Modified by RHR for v-8.5.4.004 on 11/13/2023 
Namespace DataTransferObjects
    <DataContract()> _
    Public Class Integration
        Inherits DTOBaseClass

        public Sub New ()
            Me.Logger = Me.Logger.ForContext(Of Integration)
        End Sub
#Region " Data Members"
         
        Private _IntegrationControl As Integer = 0
        <DataMember()> _
        Public Property IntegrationControl() As Integer
            Get
                Return _IntegrationControl
            End Get
            Set(ByVal value As Integer)
                _IntegrationControl = value
            End Set
        End Property

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

        Private _IntegrationTypeControl As Integer = 0
        <DataMember()> _
        Public Property IntegrationTypeControl() As Integer
            Get
                Return _IntegrationTypeControl
            End Get
            Set(ByVal value As Integer)
                _IntegrationTypeControl = value
            End Set
        End Property

    
        Private _TMSURI As String
        <DataMember()> _
        Public Property TMSURI() As String
            Get
                Return Left(_TMSURI, 200)
            End Get
            Set(ByVal value As String)
                _TMSURI = Left(value, 200)
            End Set
        End Property


        Private _TMSAuthUser As String
        <DataMember()> _
        Public Property TMSAuthUser() As String
            Get
                Return Left(_TMSAuthUser, 100)
            End Get
            Set(ByVal value As String)
                _TMSAuthUser = Left(value, 100)
            End Set
        End Property


        Private _TMSAuthPassword As String
        <DataMember()> _
        Public Property TMSAuthPassword() As String
            Get
                Return Left(_TMSAuthPassword, 20)
            End Get
            Set(ByVal value As String)
                _TMSAuthPassword = Left(value, 20)
            End Set
        End Property

        Private _TMSAuthCode As String
        ''' <summary>
        ''' Legacy TMS Auto Code
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.004 on 11/13/2023  in crease to 100 characters from 20
        ''' </remarks>
        <DataMember()> _
        Public Property TMSAuthCode() As String
            Get
                Return Left(_TMSAuthCode, 100)
            End Get
            Set(ByVal value As String)
                _TMSAuthCode = Left(value, 100)
            End Set
        End Property

        Private _TMSSpec As XElement 
        Public Property TMSSpec() As XElement
            Get
                Return _TMSSpec
            End Get
            Set(ByVal value As XElement)
                _TMSSpec = value
            End Set
        End Property
         
        Private _TMSNotes As String
        <DataMember()> _
        Public Property TMSNotes() As String
            Get
                Return Left(_TMSNotes, 250)
            End Get
            Set(ByVal value As String)
                _TMSNotes = Left(value, 250)
            End Set
        End Property
         
        Private _ERPURI As String
        <DataMember()> _
        Public Property ERPURI() As String
            Get
                Return Left(_ERPURI, 250)
            End Get
            Set(ByVal value As String)
                _ERPURI = Left(value, 250)
            End Set
        End Property


        Private _ERPAuthUser As String
        <DataMember()> _
        Public Property ERPAuthUser() As String
            Get
                Return Left(_ERPAuthUser, 100)
            End Get
            Set(ByVal value As String)
                _ERPAuthUser = Left(value, 100)
            End Set
        End Property


        Private _ERPAuthPassword As String
        <DataMember()> _
        Public Property ERPAuthPassword() As String
            Get
                Return Left(_ERPAuthPassword, 100)
            End Get
            Set(ByVal value As String)
                _ERPAuthPassword = Left(value, 100)
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
                Return Left(_ERPAuthCode, 100)
            End Get
            Set(ByVal value As String)
                _ERPAuthCode = Left(value, 100)
            End Set
        End Property

         
        Private _ERPNotes As String
        <DataMember()> _
        Public Property ERPNotes() As String
            Get
                Return Left(_ERPNotes, 250)
            End Get
            Set(ByVal value As String)
                _ERPNotes = Left(value, 250)
            End Set
        End Property
         

        Private _IntegrationModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property IntegrationModDate() As System.Nullable(Of Date)
            Get
                Return Me._IntegrationModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._IntegrationModDate.Equals(value) = False) Then
                    Me._IntegrationModDate = value
                    Me.SendPropertyChanged("IntegrationModDate")
                End If
            End Set
        End Property


        Private _IntegrationModUser As String
        <DataMember()> _
        Public Property IntegrationModUser() As String
            Get
                Return Left(_IntegrationModUser, 100)
            End Get
            Set(ByVal value As String)
                _IntegrationModUser = Left(value, 100)
            End Set
        End Property

        Private _IntegrationUpdated As Byte()
        <DataMember()> _
        Public Property IntegrationUpdated() As Byte()
            Get
                Return Me._IntegrationUpdated
            End Get
            Set(value As Byte())
                Me._IntegrationUpdated = value
            End Set
        End Property

        Private _ERPSpec As XElement
        <DataMember()>
        Public Property ERPSpec() As XElement
            Get
                Return _ERPSpec
            End Get
            Set(ByVal value As XElement)
                _ERPSpec = value
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
            Dim instance As New Integration

            instance = DirectCast(MemberwiseClone(), Integration)
            Return instance
        End Function

#End Region

    End Class
End Namespace
 