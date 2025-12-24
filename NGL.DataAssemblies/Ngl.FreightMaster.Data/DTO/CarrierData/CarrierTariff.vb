Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports Serilog

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierTariff
        Inherits DTOBaseClass
        Public sub New ()
            Me.Logger = Me.Logger.ForContext(of CarrierTariff)
        End sub

#Region " Data Members"
        Private _CarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarControl() As Integer
            Get
                Return _CarrTarControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarControl = value
            End Set
        End Property


        Private _CarrTarCarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarCarrierControl() As Integer
            Get
                Return _CarrTarCarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarCarrierControl = value
            End Set
        End Property


        Private _CarrTarCompControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarCompControl() As Integer
            Get
                Return _CarrTarCompControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarCompControl = value
            End Set
        End Property


        Private _CarrTarID As String = ""
        <DataMember()> _
        Public Property CarrTarID() As String
            Get
                Return Left(_CarrTarID, 50)
            End Get
            Set(ByVal value As String)
                _CarrTarID = Left(value, 50)
            End Set
        End Property

        Private _CarrTarBPBracketType As Integer = 0
        <DataMember()> _
        Public Property CarrTarBPBracketType() As Integer
            Get
                Return _CarrTarBPBracketType
            End Get
            Set(ByVal value As Integer)
                _CarrTarBPBracketType = value
            End Set
        End Property


        Private _CarrTarTLCapacityType As Integer = 0
        <DataMember()> _
        Public Property CarrTarTLCapacityType() As Integer
            Get
                Return _CarrTarTLCapacityType
            End Get
            Set(ByVal value As Integer)
                _CarrTarTLCapacityType = value
            End Set
        End Property


        Private _CarrTarTempType As Integer = 0
        <DataMember()> _
        Public Property CarrTarTempType() As Integer
            Get
                Return _CarrTarTempType
            End Get
            Set(ByVal value As Integer)
                _CarrTarTempType = value
            End Set
        End Property


        Private _CarrTarTariffType As Char = "I"
        <DataMember()> _
        Public Property CarrTarTariffType() As Char
            Get
                Return If(Asc(_CarrTarTariffType) < 1, "I", _CarrTarTariffType)
            End Get
            Set(ByVal value As Char)
                _CarrTarTariffType = If(Asc(value) < 1, "I", value)
            End Set
        End Property

        Private _CarrTarDefWgt As Boolean = False
        <DataMember()> _
        Public Property CarrTarDefWgt() As Boolean
            Get
                Return _CarrTarDefWgt
            End Get
            Set(ByVal value As Boolean)
                _CarrTarDefWgt = value
            End Set
        End Property

        Private _CarrTarModUser As String = ""
        <DataMember()> _
        Public Property CarrTarModUser() As String
            Get
                Return Left(_CarrTarModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrTarModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrTarModDate As Date = DateTime.Now
        <DataMember()> _
        Public Property CarrTarModDate() As Date
            Get
                Return _CarrTarModDate
            End Get
            Set(ByVal value As Date)
                _CarrTarModDate = value
            End Set
        End Property

        Private _CarrTarUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarUpdated() As Byte()
            Get
                Return _CarrTarUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarUpdated = value
            End Set
        End Property

        Private _CarrierTariffBreakPoints As List(Of CarrierTariffBreakPoint)
        Friend Property CarrierTariffBreakPoints() As List(Of CarrierTariffBreakPoint)
            Get
                Return _CarrierTariffBreakPoints
            End Get
            Set(ByVal value As List(Of CarrierTariffBreakPoint))
                _CarrierTariffBreakPoints = value
            End Set
        End Property

        Private _CarrTarWillDriveSunday As Boolean = False
        <DataMember()> _
        Public Property CarrTarWillDriveSunday() As Boolean
            Get
                Return _CarrTarWillDriveSunday
            End Get
            Set(ByVal value As Boolean)
                _CarrTarWillDriveSunday = value
            End Set
        End Property

        Private _CarrTarWillDriveSaturday As Boolean = False
        <DataMember()> _
        Public Property CarrTarWillDriveSaturday() As Boolean
            Get
                Return _CarrTarWillDriveSaturday
            End Get
            Set(ByVal value As Boolean)
                _CarrTarWillDriveSaturday = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierTariff
            instance = DirectCast(MemberwiseClone(), CarrierTariff)
            instance.CarrierTariffBreakPoints = Nothing
            For Each item In CarrierTariffBreakPoints
                instance.CarrierTariffBreakPoints.Add(DirectCast(item.Clone, CarrierTariffBreakPoint))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace