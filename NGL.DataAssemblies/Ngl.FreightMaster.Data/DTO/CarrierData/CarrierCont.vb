Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierCont
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrierContControl As Integer = 0
        <DataMember()> _
        Public Property CarrierContControl() As Integer
            Get
                Return _CarrierContControl
            End Get
            Set(ByVal value As Integer)
                _CarrierContControl = value
            End Set
        End Property

        Private _CarrierContCarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierContCarrierControl() As Integer
            Get
                Return _CarrierContCarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierContCarrierControl = value
            End Set
        End Property

        Private _CarrierContName As String = ""
        <DataMember()> _
        Public Property CarrierContName() As String
            Get
                Return Left(_CarrierContName, 25)
            End Get
            Set(ByVal value As String)
                _CarrierContName = Left(value, 25)
            End Set
        End Property

        Private _CarrierContTitle As String = ""
        <DataMember()> _
        Public Property CarrierContTitle() As String
            Get
                Return Left(_CarrierContTitle, 25)
            End Get
            Set(ByVal value As String)
                _CarrierContTitle = Left(value, 25)
            End Set
        End Property

        Private _CarrierContactPhone As String = ""
        <DataMember()> _
        Public Property CarrierContactPhone() As String
            Get
                Return Left(_CarrierContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _CarrierContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _CarrierContPhoneExt As String = ""
        <DataMember()> _
        Public Property CarrierContPhoneExt() As String
            Get
                Return Left(_CarrierContPhoneExt, 5)
            End Get
            Set(ByVal value As String)
                _CarrierContPhoneExt = Left(value, 5)
            End Set
        End Property

        Private _CarrierContactFax As String = ""
        <DataMember()> _
        Public Property CarrierContactFax() As String
            Get
                Return Left(_CarrierContactFax, 15)
            End Get
            Set(ByVal value As String)
                _CarrierContactFax = Left(value, 15)
            End Set
        End Property

        Private _CarrierContact800 As String = ""
        <DataMember()> _
        Public Property CarrierContact800() As String
            Get
                Return Left(_CarrierContact800, 15)
            End Get
            Set(ByVal value As String)
                _CarrierContact800 = Left(value, 15)
            End Set
        End Property

        Private _CarrierContactEMail As String = ""
        <DataMember()> _
        Public Property CarrierContactEMail() As String
            Get
                Return Left(_CarrierContactEMail, 255)
            End Get
            Set(ByVal value As String)
                _CarrierContactEMail = Left(value, 255)
            End Set
        End Property

        Private _CarrierContUpdated As Byte()
        <DataMember()> _
        Public Property CarrierContUpdated() As Byte()
            Get
                Return _CarrierContUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrierContUpdated = value
            End Set
        End Property

        'Added By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
        Private _CarrierContactDefault As Boolean = False
        <DataMember()>
        Public Property CarrierContactDefault() As Boolean
            Get
                Return _CarrierContactDefault
            End Get
            Set(ByVal value As Boolean)
                _CarrierContactDefault = value
            End Set
        End Property

        'Added By LVV on 10/04/18 for v-8.3 TMS365 Scheduler
        Private _CarrierContSchedContact As Boolean = False
        <DataMember()>
        Public Property CarrierContSchedContact() As Boolean
            Get
                Return _CarrierContSchedContact
            End Get
            Set(ByVal value As Boolean)
                _CarrierContSchedContact = value
            End Set
        End Property

        Private _CarrierContLECarControl As Integer = 0
        <DataMember()>
        Public Property CarrierContLECarControl() As Integer
            Get
                Return _CarrierContLECarControl
            End Get
            Set(ByVal value As Integer)
                _CarrierContLECarControl = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierCont
            instance = DirectCast(MemberwiseClone(), CarrierCont)
            Return instance
        End Function

#End Region

    End Class
End Namespace