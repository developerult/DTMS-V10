Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarFee
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrTarFeesControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarFeesControl() As Integer
            Get
                Return _CarrTarFeesControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarFeesControl = value
            End Set
        End Property

        Private _CarrTarFeesCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarFeesCarrTarControl() As Integer
            Get
                Return _CarrTarFeesCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarFeesCarrTarControl = value
            End Set
        End Property

        Private _CarrTarFeesMinimum As Decimal = 0
        <DataMember()> _
        Public Property CarrTarFeesMinimum() As Decimal
            Get
                Return _CarrTarFeesMinimum
            End Get
            Set(ByVal value As Decimal)
                _CarrTarFeesMinimum = value
            End Set
        End Property

        Private _CarrTarFeesVariable As Double = 0
        <DataMember()> _
        Public Property CarrTarFeesVariable() As Double
            Get
                Return _CarrTarFeesVariable
            End Get
            Set(ByVal value As Double)
                _CarrTarFeesVariable = value
            End Set
        End Property

        Private _CarrTarFeesAccessorialCode As Integer = 0
        <DataMember()> _
        Public Property CarrTarFeesAccessorialCode() As Integer
            Get
                Return _CarrTarFeesAccessorialCode
            End Get
            Set(ByVal value As Integer)
                _CarrTarFeesAccessorialCode = value
            End Set
        End Property

        Private _CarrTarFeesVariableCode As Integer = 0
        <DataMember()> _
        Public Property CarrTarFeesVariableCode() As Integer
            Get
                Return _CarrTarFeesVariableCode
            End Get
            Set(ByVal value As Integer)
                _CarrTarFeesVariableCode = value
            End Set
        End Property

        Private _CarrTarFeesVisible As Boolean = True
        <DataMember()> _
        Public Property CarrTarFeesVisible() As Boolean
            Get
                Return _CarrTarFeesVisible
            End Get
            Set(ByVal value As Boolean)
                _CarrTarFeesVisible = value
            End Set
        End Property

        Private _CarrTarFeesAutoApprove As Boolean = False
        <DataMember()> _
        Public Property CarrTarFeesAutoApprove() As Boolean
            Get
                Return _CarrTarFeesAutoApprove
            End Get
            Set(ByVal value As Boolean)
                _CarrTarFeesAutoApprove = value
            End Set
        End Property

        Private _CarrTarFeesAllowCarrierUpdates As Boolean = False
        <DataMember()> _
        Public Property CarrTarFeesAllowCarrierUpdates() As Boolean
            Get
                Return _CarrTarFeesAllowCarrierUpdates
            End Get
            Set(ByVal value As Boolean)
                _CarrTarFeesAllowCarrierUpdates = value
            End Set
        End Property

        Private _CarrTarFeesCaption As String = ""
        <DataMember()> _
        Public Property CarrTarFeesCaption() As String
            Get
                Return Left(_CarrTarFeesCaption, 50)
            End Get
            Set(ByVal value As String)
                _CarrTarFeesCaption = Left(value, 50)
            End Set
        End Property

        Private _CarrTarFeesEDICode As String = ""
        <DataMember()> _
        Public Property CarrTarFeesEDICode() As String
            Get
                Return Left(_CarrTarFeesEDICode, 20)
            End Get
            Set(ByVal value As String)
                _CarrTarFeesEDICode = Left(value, 20)
            End Set
        End Property

        Private _CarrTarFeesTaxable As Boolean = True
        <DataMember()> _
        Public Property CarrTarFeesTaxable() As Boolean
            Get
                Return _CarrTarFeesTaxable
            End Get
            Set(ByVal value As Boolean)
                _CarrTarFeesTaxable = value
            End Set
        End Property

        Private _CarrTarFeesIsTax As Boolean = False
        <DataMember()> _
        Public Property CarrTarFeesIsTax() As Boolean
            Get
                Return _CarrTarFeesIsTax
            End Get
            Set(ByVal value As Boolean)
                _CarrTarFeesIsTax = value
            End Set
        End Property

        Private _CarrTarFeesTaxSortOrder As Integer = 0
        <DataMember()> _
        Public Property CarrTarFeesTaxSortOrder() As Integer
            Get
                Return _CarrTarFeesTaxSortOrder
            End Get
            Set(ByVal value As Integer)
                _CarrTarFeesTaxSortOrder = value
            End Set
        End Property

        Private _CarrTarFeesBOLText As String = ""
        <DataMember()> _
        Public Property CarrTarFeesBOLText() As String
            Get
                Return Left(_CarrTarFeesBOLText, 4000)
            End Get
            Set(ByVal value As String)
                _CarrTarFeesBOLText = Left(value, 4000)
            End Set
        End Property

        Private _CarrTarFeesBOLPlacement As String = ""
        <DataMember()> _
        Public Property CarrTarFeesBOLPlacement() As String
            Get
                Return Left(_CarrTarFeesBOLPlacement, 100)
            End Get
            Set(ByVal value As String)
                _CarrTarFeesBOLPlacement = Left(value, 100)
            End Set
        End Property

        Private _CarrTarFeesAccessorialFeeAllocationTypeControl As Integer = 1
        <DataMember()> _
        Public Property CarrTarFeesAccessorialFeeAllocationTypeControl() As Integer
            Get
                Return _CarrTarFeesAccessorialFeeAllocationTypeControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarFeesAccessorialFeeAllocationTypeControl = value
            End Set
        End Property

        Private _CarrTarFeesTarBracketTypeControl As Integer = 4
        <DataMember()> _
        Public Property CarrTarFeesTarBracketTypeControl() As Integer
            Get
                Return _CarrTarFeesTarBracketTypeControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarFeesTarBracketTypeControl = value
            End Set
        End Property

        Private _CarrTarFeesAccessorialFeeCalcTypeControl As Integer = 1
        <DataMember()> _
        Public Property CarrTarFeesAccessorialFeeCalcTypeControl() As Integer
            Get
                Return _CarrTarFeesAccessorialFeeCalcTypeControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarFeesAccessorialFeeCalcTypeControl = value
            End Set
        End Property

        Private _CarrTarFeesPreCloneControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarFeesPreCloneControl() As Integer
            Get
                Return _CarrTarFeesPreCloneControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarFeesPreCloneControl = value
            End Set
        End Property


        Private _CarrTarFeesModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarFeesModDate() As System.Nullable(Of Date)
            Get
                Return _CarrTarFeesModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrTarFeesModDate = value
            End Set
        End Property

        Private _CarrTarFeesModUser As String = ""
        <DataMember()> _
        Public Property CarrTarFeesModUser() As String
            Get
                Return Left(_CarrTarFeesModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrTarFeesModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrTarFeesUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarFeesUpdated() As Byte()
            Get
                Return _CarrTarFeesUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarFeesUpdated = value
            End Set
        End Property

        Private _CarrTarFeesAccessorialProfileSpecific As Boolean = False
        <DataMember()> _
        Public Property CarrTarFeesAccessorialProfileSpecific() As Boolean
            Get
                Return _CarrTarFeesAccessorialProfileSpecific
            End Get
            Set(ByVal value As Boolean)
                _CarrTarFeesAccessorialProfileSpecific = value
            End Set
        End Property

        Private _NotSupported As Boolean = False
        <DataMember()> _
        Public Property NotSupported() As Boolean
            Get
                Return _NotSupported
            End Get
            Set(ByVal value As Boolean)
                _NotSupported = value
            End Set
        End Property

        'Added By LVV on 9/28/16 for v-7.0.5.110 HDM Enhancement
        Private _CarrTarFeesMaximum As Decimal = 0
        <DataMember()> _
        Public Property CarrTarFeesMaximum() As Decimal
            Get
                Return _CarrTarFeesMaximum
            End Get
            Set(ByVal value As Decimal)
                _CarrTarFeesMaximum = value
            End Set
        End Property



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarFee
            instance = DirectCast(MemberwiseClone(), CarrTarFee)
            Return instance
        End Function

#End Region

    End Class
End Namespace
