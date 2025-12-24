Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrAdHocCont
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrAdHocContControl As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocContControl() As Integer
            Get
                Return _CarrAdHocContControl
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocContControl = value
            End Set
        End Property

        Private _CarrAdHocContCarrAdHocControl As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocContCarrAdHocControl() As Integer
            Get
                Return _CarrAdHocContCarrAdHocControl
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocContCarrAdHocControl = value
            End Set
        End Property

        Private _CarrAdHocContName As String = ""
        <DataMember()> _
        Public Property CarrAdHocContName() As String
            Get
                Return Left(_CarrAdHocContName, 25)
            End Get
            Set(ByVal value As String)
                _CarrAdHocContName = Left(value, 25)
            End Set
        End Property

        Private _CarrAdHocContTitle As String = ""
        <DataMember()> _
        Public Property CarrAdHocContTitle() As String
            Get
                Return Left(_CarrAdHocContTitle, 25)
            End Get
            Set(ByVal value As String)
                _CarrAdHocContTitle = Left(value, 25)
            End Set
        End Property

        Private _CarrAdHocContactPhone As String = ""
        <DataMember()> _
        Public Property CarrAdHocContactPhone() As String
            Get
                Return Left(_CarrAdHocContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _CarrAdHocContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _CarrAdHocContPhoneExt As String = ""
        <DataMember()> _
        Public Property CarrAdHocContPhoneExt() As String
            Get
                Return Left(_CarrAdHocContPhoneExt, 5)
            End Get
            Set(ByVal value As String)
                _CarrAdHocContPhoneExt = Left(value, 5)
            End Set
        End Property

        Private _CarrAdHocContactFax As String = ""
        <DataMember()> _
        Public Property CarrAdHocContactFax() As String
            Get
                Return Left(_CarrAdHocContactFax, 15)
            End Get
            Set(ByVal value As String)
                _CarrAdHocContactFax = Left(value, 15)
            End Set
        End Property

        Private _CarrAdHocContact800 As String = ""
        <DataMember()> _
        Public Property CarrAdHocContact800() As String
            Get
                Return Left(_CarrAdHocContact800, 15)
            End Get
            Set(ByVal value As String)
                _CarrAdHocContact800 = Left(value, 15)
            End Set
        End Property

        Private _CarrAdHocContactEMail As String = ""
        <DataMember()> _
        Public Property CarrAdHocContactEMail() As String
            Get
                Return Left(_CarrAdHocContactEMail, 255)
            End Get
            Set(ByVal value As String)
                _CarrAdHocContactEMail = Left(value, 255)
            End Set
        End Property

        Private _CarrAdHocContUpdated As Byte()
        <DataMember()> _
        Public Property CarrAdHocContUpdated() As Byte()
            Get
                Return _CarrAdHocContUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrAdHocContUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrAdHocCont
            instance = DirectCast(MemberwiseClone(), CarrAdHocCont)
            Return instance
        End Function

#End Region

    End Class
End Namespace