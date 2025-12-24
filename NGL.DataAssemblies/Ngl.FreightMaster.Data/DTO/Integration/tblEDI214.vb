Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV 2/29/16 for v-7.0.5.1 EDI Migration
Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblEDI214
        Inherits DTOBaseClass

#Region " Data Members "

        Private _EDI214Control As Integer = 0
        <DataMember()> _
        Public Property EDI214Control() As Integer
            Get
                Return _EDI214Control
            End Get
            Set(ByVal value As Integer)
                _EDI214Control = value
            End Set
        End Property

        Private _BookCarrOrderNumber As String = ""
        <DataMember()> _
        Public Property BookCarrOrderNumber() As String
            Get
                Return Left(_BookCarrOrderNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumber = Left(value, 20)
            End Set
        End Property

        Private _BookOrderSequence As Integer = 0
        <DataMember()> _
        Public Property BookOrderSequence() As Integer
            Get
                Return _BookOrderSequence
            End Get
            Set(ByVal value As Integer)
                _BookOrderSequence = value
            End Set
        End Property

        Private _BookConsPrefix As String = ""
        <DataMember()> _
        Public Property BookConsPrefix() As String
            Get
                Return Left(_BookConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                _BookConsPrefix = Left(value, 20)
            End Set
        End Property

        Private _CarrierPartnerCode As String = ""
        <DataMember()> _
        Public Property CarrierPartnerCode() As String
            Get
                Return Left(_CarrierPartnerCode, 15)
            End Get
            Set(ByVal value As String)
                _CarrierPartnerCode = Left(value, 15)
            End Set
        End Property

        Private _CompPartnerCode As String = ""
        <DataMember()> _
        Public Property CompPartnerCode() As String
            Get
                Return Left(_CompPartnerCode, 15)
            End Get
            Set(ByVal value As String)
                _CompPartnerCode = Left(value, 15)
            End Set
        End Property

        Private _EventCode As String = ""
        <DataMember()> _
        Public Property EventCode() As String
            Get
                Return Left(_EventCode, 2)
            End Get
            Set(ByVal value As String)
                _EventCode = Left(value, 2)
            End Set
        End Property

        Private _EventDate As String = ""
        <DataMember()> _
        Public Property EventDate() As String
            Get
                Return Left(_EventDate, 10)
            End Get
            Set(ByVal value As String)
                _EventDate = Left(value, 10)
            End Set
        End Property

        Private _EventTime As String = ""
        <DataMember()> _
        Public Property EventTime() As String
            Get
                Return Left(_EventTime, 10)
            End Get
            Set(ByVal value As String)
                _EventTime = Left(value, 10)
            End Set
        End Property

        Private _BookShipCarrierProNumber As String = ""
        <DataMember()> _
        Public Property BookShipCarrierProNumber() As String
            Get
                Return Left(_BookShipCarrierProNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierProNumber = Left(value, 20)
            End Set
        End Property

        Private _BookShipCarrierName As String = ""
        <DataMember()> _
        Public Property BookShipCarrierName() As String
            Get
                Return Left(_BookShipCarrierName, 60)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierName = Left(value, 60)
            End Set
        End Property

        Private _BookShipCarrierNumber As String = ""
        <DataMember()> _
        Public Property BookShipCarrierNumber() As String
            Get
                Return Left(_BookShipCarrierNumber, 80)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierNumber = Left(value, 80)
            End Set
        End Property

        Private _EventComments As String = ""
        <DataMember()> _
        Public Property EventComments() As String
            Get
                Return _EventComments
            End Get
            Set(ByVal value As String)
                _EventComments = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Left(_CarrierName, 40)
            End Get
            Set(ByVal value As String)
                _CarrierName = Left(value, 40)
            End Set
        End Property

        Private _CompName As String = ""
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return Left(_CompName, 40)
            End Get
            Set(ByVal value As String)
                _CompName = Left(value, 40)
            End Set
        End Property

        Private _EDI214Received As Boolean = False
        <DataMember()> _
        Public Property EDI214Received() As Boolean
            Get
                Return _EDI214Received
            End Get
            Set(ByVal value As Boolean)
                _EDI214Received = value
            End Set
        End Property

        Private _EDI214ReceivedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI214ReceivedDate() As System.Nullable(Of Date)
            Get
                Return _EDI214ReceivedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI214ReceivedDate = value
            End Set
        End Property

        Private _EDI214StatusCode As String = ""
        <DataMember()> _
        Public Property EDI214StatusCode() As String
            Get
                Return Left(_EDI214StatusCode, 100)
            End Get
            Set(ByVal value As String)
                _EDI214StatusCode = Left(value, 100)
            End Set
        End Property

        Private _EDI214Message As String = ""
        <DataMember()> _
        Public Property EDI214Message() As String
            Get
                Return _EDI214Message
            End Get
            Set(ByVal value As String)
                _EDI214Message = value
            End Set
        End Property

        Private _EDI214FileName As String = ""
        <DataMember()> _
        Public Property EDI214FileName() As String
            Get
                Return Left(_EDI214FileName, 255)
            End Get
            Set(ByVal value As String)
                _EDI214FileName = Left(value, 255)
                Me.SendPropertyChanged("EDI214FileName")
            End Set
        End Property

        Private _EDI214ModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI214ModDate() As System.Nullable(Of Date)
            Get
                Return _EDI214ModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI214ModDate = value
                Me.SendPropertyChanged("EDI214ModDate")
            End Set
        End Property

        Private _EDI214ModUser As String = ""
        <DataMember()> _
        Public Property EDI214ModUser() As String
            Get
                Return Left(_EDI214ModUser, 100)
            End Get
            Set(ByVal value As String)
                _EDI214ModUser = Left(value, 100)
                Me.SendPropertyChanged("EDI214ModUser")
            End Set
        End Property

        Private _EDI214Updated As Byte()
        <DataMember()> _
        Public Property EDI214Updated() As Byte()
            Get
                Return _EDI214Updated
            End Get
            Set(ByVal value As Byte())
                _EDI214Updated = value
            End Set
        End Property

        'Added for v-7.0.5.1 By LVV 3/3/16
        Private _SHID As String
        <DataMember()> _
        Public Property SHID() As String
            Get
                Return Left(_SHID, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SHID, value) = False) Then
                    Me._SHID = Left(value, 50)
                    Me.SendPropertyChanged("SHID")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblEDI214
            instance = DirectCast(MemberwiseClone(), tblEDI214)
            Return instance
        End Function

#End Region

    End Class
End Namespace
