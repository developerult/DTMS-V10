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
    Public Class LaneFee
        Inherits DTOBaseClass

        public sub New 
            Me.Logger = Me.Logger.ForContext(of LaneFee)
        End sub
#Region " Data Members"
        Private _LaneFeesControl As Integer = 0
        <DataMember()> _
        Public Property LaneFeesControl() As Integer
            Get
                Return _LaneFeesControl
            End Get
            Set(ByVal value As Integer)
                _LaneFeesControl = value
            End Set
        End Property

        Private _LaneFeesLaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneFeesLaneControl() As Integer
            Get
                Return _LaneFeesLaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneFeesLaneControl = value
            End Set
        End Property

        Private _LaneFeesMinimum As Decimal = 0
        <DataMember()> _
        Public Property LaneFeesMinimum() As Decimal
            Get
                Return _LaneFeesMinimum
            End Get
            Set(ByVal value As Decimal)
                _LaneFeesMinimum = value
            End Set
        End Property

        Private _LaneFeesVariable As Double = 0
        <DataMember()> _
        Public Property LaneFeesVariable() As Double
            Get
                Return _LaneFeesVariable
            End Get
            Set(ByVal value As Double)
                _LaneFeesVariable = value
            End Set
        End Property

        Private _LaneFeesAccessorialCode As Integer = 0
        <DataMember()> _
        Public Property LaneFeesAccessorialCode() As Integer
            Get
                Return _LaneFeesAccessorialCode
            End Get
            Set(ByVal value As Integer)
                _LaneFeesAccessorialCode = value
            End Set
        End Property

        Private _LaneFeesVariableCode As Integer = 0
        <DataMember()> _
        Public Property LaneFeesVariableCode() As Integer
            Get
                Return _LaneFeesVariableCode
            End Get
            Set(ByVal value As Integer)
                _LaneFeesVariableCode = value
            End Set
        End Property

        Private _LaneFeesVisible As Boolean = True
        <DataMember()> _
        Public Property LaneFeesVisible() As Boolean
            Get
                Return _LaneFeesVisible
            End Get
            Set(ByVal value As Boolean)
                _LaneFeesVisible = value
            End Set
        End Property

        Private _LaneFeesAutoApprove As Boolean = False
        <DataMember()> _
        Public Property LaneFeesAutoApprove() As Boolean
            Get
                Return _LaneFeesAutoApprove
            End Get
            Set(ByVal value As Boolean)
                _LaneFeesAutoApprove = value
            End Set
        End Property

        Private _LaneFeesAllowCarrierUpdates As Boolean = False
        <DataMember()> _
        Public Property LaneFeesAllowCarrierUpdates() As Boolean
            Get
                Return _LaneFeesAllowCarrierUpdates
            End Get
            Set(ByVal value As Boolean)
                _LaneFeesAllowCarrierUpdates = value
            End Set
        End Property

        Private _LaneFeesCaption As String = ""
        <DataMember()> _
        Public Property LaneFeesCaption() As String
            Get
                Return Left(_LaneFeesCaption, 50)
            End Get
            Set(ByVal value As String)
                _LaneFeesCaption = Left(value, 50)
            End Set
        End Property

        Private _LaneFeesEDICode As String = ""
        <DataMember()> _
        Public Property LaneFeesEDICode() As String
            Get
                Return Left(_LaneFeesEDICode, 20)
            End Get
            Set(ByVal value As String)
                _LaneFeesEDICode = Left(value, 20)
            End Set
        End Property

        Private _LaneFeesTaxable As Boolean = True
        <DataMember()> _
        Public Property LaneFeesTaxable() As Boolean
            Get
                Return _LaneFeesTaxable
            End Get
            Set(ByVal value As Boolean)
                _LaneFeesTaxable = value
            End Set
        End Property

        Private _LaneFeesIsTax As Boolean = False
        <DataMember()> _
        Public Property LaneFeesIsTax() As Boolean
            Get
                Return _LaneFeesIsTax
            End Get
            Set(ByVal value As Boolean)
                _LaneFeesIsTax = value
            End Set
        End Property

        Private _LaneFeesTaxSortOrder As Integer = 0
        <DataMember()> _
        Public Property LaneFeesTaxSortOrder() As Integer
            Get
                Return _LaneFeesTaxSortOrder
            End Get
            Set(ByVal value As Integer)
                _LaneFeesTaxSortOrder = value
            End Set
        End Property

        Private _LaneFeesBOLText As String = ""
        <DataMember()> _
        Public Property LaneFeesBOLText() As String
            Get
                Return Left(_LaneFeesBOLText, 4000)
            End Get
            Set(ByVal value As String)
                _LaneFeesBOLText = Left(value, 4000)
            End Set
        End Property

        Private _LaneFeesBOLPlacement As String = ""
        <DataMember()> _
        Public Property LaneFeesBOLPlacement() As String
            Get
                Return Left(_LaneFeesBOLPlacement, 100)
            End Get
            Set(ByVal value As String)
                _LaneFeesBOLPlacement = Left(value, 100)
            End Set
        End Property

        Private _LaneFeesAccessorialFeeAllocationTypeControl As Integer = 1
        <DataMember()> _
        Public Property LaneFeesAccessorialFeeAllocationTypeControl() As Integer
            Get
                Return _LaneFeesAccessorialFeeAllocationTypeControl
            End Get
            Set(ByVal value As Integer)
                _LaneFeesAccessorialFeeAllocationTypeControl = value
            End Set
        End Property

        Private _LaneFeesTarBracketTypeControl As Integer = 4
        <DataMember()> _
        Public Property LaneFeesTarBracketTypeControl() As Integer
            Get
                Return _LaneFeesTarBracketTypeControl
            End Get
            Set(ByVal value As Integer)
                _LaneFeesTarBracketTypeControl = value
            End Set
        End Property

        Private _LaneFeesAccessorialFeeCalcTypeControl As Integer = 1
        <DataMember()> _
        Public Property LaneFeesAccessorialFeeCalcTypeControl() As Integer
            Get
                Return _LaneFeesAccessorialFeeCalcTypeControl
            End Get
            Set(ByVal value As Integer)
                _LaneFeesAccessorialFeeCalcTypeControl = value
            End Set
        End Property

        Private _LaneFeesModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneFeesModDate() As System.Nullable(Of Date)
            Get
                Return _LaneFeesModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneFeesModDate = value
            End Set
        End Property

        Private _LaneFeesModUser As String = ""
        <DataMember()> _
        Public Property LaneFeesModUser() As String
            Get
                Return Left(_LaneFeesModUser, 100)
            End Get
            Set(ByVal value As String)
                _LaneFeesModUser = Left(value, 100)
            End Set
        End Property

        Private _LaneFeesUpdated As Byte()
        <DataMember()> _
        Public Property LaneFeesUpdated() As Byte()
            Get
                Return _LaneFeesUpdated
            End Get
            Set(ByVal value As Byte())
                _LaneFeesUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LaneFee
            instance = DirectCast(MemberwiseClone(), LaneFee)
            Return instance
        End Function

#End Region

    End Class
End Namespace