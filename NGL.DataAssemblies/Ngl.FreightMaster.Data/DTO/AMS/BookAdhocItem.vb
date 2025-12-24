Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookAdhocItem
        Inherits DTOBaseClass


#Region " Server Properties "

        Private _TempControl As Integer = 0
        ''' <summary>
        ''' used as a place holder for a temporary control number
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TempControl As Integer
            Get
                If _TempControl = 0 Then _TempControl = BookAdhocItemControl
                Return _TempControl
            End Get
            Set(value As Integer)
                _TempControl = value
            End Set
        End Property

#End Region

#Region " Data Members"
        Private _BookAdhocItemControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocItemControl() As Integer
            Get
                Return _BookAdhocItemControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocItemControl = value
            End Set
        End Property

        Private _BookAdhocItemBookAdhocLoadControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocItemBookAdhocLoadControl() As Integer
            Get
                Return _BookAdhocItemBookAdhocLoadControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocItemBookAdhocLoadControl = value
            End Set
        End Property

        Private _BookAdhocItemFixOffInvAllow As Double = 0
        <DataMember()> _
        Public Property BookAdhocItemFixOffInvAllow() As Double
            Get
                Return _BookAdhocItemFixOffInvAllow
            End Get
            Set(ByVal value As Double)
                _BookAdhocItemFixOffInvAllow = value
            End Set
        End Property

        Private _BookAdhocItemFixFrtAllow As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocItemFixFrtAllow() As Decimal
            Get
                Return _BookAdhocItemFixFrtAllow
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocItemFixFrtAllow = value
            End Set
        End Property

        Private _BookAdhocItemItemNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocItemItemNumber() As String
            Get
                Return Left(_BookAdhocItemItemNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemItemNumber = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocItemQtyOrdered As Integer = 0
        <DataMember()> _
        Public Property BookAdhocItemQtyOrdered() As Integer
            Get
                Return _BookAdhocItemQtyOrdered
            End Get
            Set(ByVal value As Integer)
                _BookAdhocItemQtyOrdered = value
            End Set
        End Property

        Private _BookAdhocItemFreightCost As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocItemFreightCost() As Decimal
            Get
                Return _BookAdhocItemFreightCost
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocItemFreightCost = value
            End Set
        End Property

        Private _BookAdhocItemItemCost As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocItemItemCost() As Decimal
            Get
                Return _BookAdhocItemItemCost
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocItemItemCost = value
            End Set
        End Property

        Private _BookAdhocItemWeight As Double = 0
        <DataMember()> _
        Public Property BookAdhocItemWeight() As Double
            Get
                Return _BookAdhocItemWeight
            End Get
            Set(ByVal value As Double)
                _BookAdhocItemWeight = value
            End Set
        End Property

        Private _BookAdhocItemCube As Integer = 0
        <DataMember()> _
        Public Property BookAdhocItemCube() As Integer
            Get
                Return _BookAdhocItemCube
            End Get
            Set(ByVal value As Integer)
                _BookAdhocItemCube = value
            End Set
        End Property

        Private _BookAdhocItemPack As Short = 0
        <DataMember()> _
        Public Property BookAdhocItemPack() As Short
            Get
                Return _BookAdhocItemPack
            End Get
            Set(ByVal value As Short)
                _BookAdhocItemPack = value
            End Set
        End Property

        Private _BookAdhocItemSize As String = ""
        <DataMember()> _
        Public Property BookAdhocItemSize() As String
            Get
                Return Left(_BookAdhocItemSize, 255)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemSize = Left(value, 255)
            End Set
        End Property

        Private _BookAdhocItemDescription As String = ""
        <DataMember()> _
        Public Property BookAdhocItemDescription() As String
            Get
                Return Left(_BookAdhocItemDescription, 255)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemDescription = Left(value, 255)
            End Set
        End Property

        Private _BookAdhocItemHazmat As String = ""
        <DataMember()> _
        Public Property BookAdhocItemHazmat() As String
            Get
                Return Left(_BookAdhocItemHazmat, 1)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemHazmat = Left(value, 1)
            End Set
        End Property

        Private _BookAdhocItemModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocItemModDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocItemModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocItemModDate = value
            End Set
        End Property

        Private _BookAdhocItemModUser As String = ""
        <DataMember()> _
        Public Property BookAdhocItemModUser() As String
            Get
                Return Left(_BookAdhocItemModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemModUser = Left(value, 100)
            End Set
        End Property

        Private _BookAdhocItemBrand As String = ""
        <DataMember()> _
        Public Property BookAdhocItemBrand() As String
            Get
                Return Left(_BookAdhocItemBrand, 255)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemBrand = Left(value, 255)
            End Set
        End Property

        Private _BookAdhocItemCostCenter As String = ""
        <DataMember()> _
        Public Property BookAdhocItemCostCenter() As String
            Get
                Return Left(_BookAdhocItemCostCenter, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemCostCenter = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocItemLotNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocItemLotNumber() As String
            Get
                Return Left(_BookAdhocItemLotNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemLotNumber = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocItemLotExpirationDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocItemLotExpirationDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocItemLotExpirationDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocItemLotExpirationDate = value
            End Set
        End Property

        Private _BookAdhocItemGTIN As String = ""
        <DataMember()> _
        Public Property BookAdhocItemGTIN() As String
            Get
                Return Left(_BookAdhocItemGTIN, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemGTIN = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocCustItemNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocCustItemNumber() As String
            Get
                Return Left(_BookAdhocCustItemNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocCustItemNumber = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocItemBFC As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocItemBFC() As Decimal
            Get
                Return _BookAdhocItemBFC
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocItemBFC = value
            End Set
        End Property

        Private _BookAdhocItemCountryOfOrigin As String = ""
        <DataMember()> _
        Public Property BookAdhocItemCountryOfOrigin() As String
            Get
                Return Left(_BookAdhocItemCountryOfOrigin, 255)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemCountryOfOrigin = Left(value, 255)
            End Set
        End Property

        Private _BookAdhocItemHST As String = ""
        <DataMember()> _
        Public Property BookAdhocItemHST() As String
            Get
                Return Left(_BookAdhocItemHST, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemHST = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocItemPalletTypeID As Integer = 0
        <DataMember()> _
        Public Property BookAdhocItemPalletTypeID() As Integer
            Get
                Return _BookAdhocItemPalletTypeID
            End Get
            Set(ByVal value As Integer)
                _BookAdhocItemPalletTypeID = value
            End Set
        End Property

        Private _BookAdhocItemHazmatTypeCode As String = ""
        <DataMember()> _
        Public Property BookAdhocItemHazmatTypeCode() As String
            Get
                Return Left(_BookAdhocItemHazmatTypeCode, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemHazmatTypeCode = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocItem49CFRCode As String = ""
        <DataMember()> _
        Public Property BookAdhocItem49CFRCode() As String
            Get
                Return Left(_BookAdhocItem49CFRCode, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocItem49CFRCode = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocItemIATACode As String = ""
        <DataMember()> _
        Public Property BookAdhocItemIATACode() As String
            Get
                Return Left(_BookAdhocItemIATACode, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemIATACode = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocItemDOTCode As String = ""
        <DataMember()> _
        Public Property BookAdhocItemDOTCode() As String
            Get
                Return Left(_BookAdhocItemDOTCode, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemDOTCode = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocItemMarineCode As String = ""
        <DataMember()> _
        Public Property BookAdhocItemMarineCode() As String
            Get
                Return Left(_BookAdhocItemMarineCode, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemMarineCode = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocItemNMFCClass As String = ""
        <DataMember()> _
        Public Property BookAdhocItemNMFCClass() As String
            Get
                Return Left(_BookAdhocItemNMFCClass, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemNMFCClass = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocItemFAKClass As String = ""
        <DataMember()> _
        Public Property BookAdhocItemFAKClass() As String
            Get
                Return Left(_BookAdhocItemFAKClass, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocItemFAKClass = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocItemLimitedQtyFlag As Boolean = False
        <DataMember()> _
        Public Property BookAdhocItemLimitedQtyFlag() As Boolean
            Get
                Return _BookAdhocItemLimitedQtyFlag
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocItemLimitedQtyFlag = value
            End Set
        End Property

        Private _BookAdhocItemPallets As Double = 0
        <DataMember()> _
        Public Property BookAdhocItemPallets() As Double
            Get
                Return _BookAdhocItemPallets
            End Get
            Set(ByVal value As Double)
                _BookAdhocItemPallets = value
            End Set
        End Property

        Private _BookAdhocItemTies As Double = 0
        <DataMember()> _
        Public Property BookAdhocItemTies() As Double
            Get
                Return _BookAdhocItemTies
            End Get
            Set(ByVal value As Double)
                _BookAdhocItemTies = value
            End Set
        End Property

        Private _BookAdhocItemHighs As Double = 0
        <DataMember()> _
        Public Property BookAdhocItemHighs() As Double
            Get
                Return _BookAdhocItemHighs
            End Get
            Set(ByVal value As Double)
                _BookAdhocItemHighs = value
            End Set
        End Property

        Private _BookAdhocItemQtyPalletPercentage As Double = 0
        <DataMember()> _
        Public Property BookAdhocItemQtyPalletPercentage() As Double
            Get
                Return _BookAdhocItemQtyPalletPercentage
            End Get
            Set(ByVal value As Double)
                _BookAdhocItemQtyPalletPercentage = value
            End Set
        End Property

        Private _BookAdhocItemQtyLength As Double = 0
        <DataMember()> _
        Public Property BookAdhocItemQtyLength() As Double
            Get
                Return _BookAdhocItemQtyLength
            End Get
            Set(ByVal value As Double)
                _BookAdhocItemQtyLength = value
            End Set
        End Property

        Private _BookAdhocItemQtyWidth As Double = 0
        <DataMember()> _
        Public Property BookAdhocItemQtyWidth() As Double
            Get
                Return _BookAdhocItemQtyWidth
            End Get
            Set(ByVal value As Double)
                _BookAdhocItemQtyWidth = value
            End Set
        End Property

        Private _BookAdhocItemQtyHeight As Double = 0
        <DataMember()> _
        Public Property BookAdhocItemQtyHeight() As Double
            Get
                Return _BookAdhocItemQtyHeight
            End Get
            Set(ByVal value As Double)
                _BookAdhocItemQtyHeight = value
            End Set
        End Property


        Private _BookAdhocItemStackable As Boolean = False
        <DataMember()> _
        Public Property BookAdhocItemStackable() As Boolean
            Get
                Return _BookAdhocItemStackable
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocItemStackable = value
            End Set
        End Property


        Private _BookAdhocItemLevelOfDensity As Integer = 0
        <DataMember()> _
        Public Property BookAdhocItemLevelOfDensity() As Integer
            Get
                Return _BookAdhocItemLevelOfDensity
            End Get
            Set(ByVal value As Integer)
                _BookAdhocItemLevelOfDensity = value
            End Set
        End Property


        Private _BookAdhocItemUpdated As Byte()
        <DataMember()> _
        Public Property BookAdhocItemUpdated() As Byte()
            Get
                Return _BookAdhocItemUpdated
            End Get
            Set(ByVal value As Byte())
                _BookAdhocItemUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookAdhocItem
            instance = DirectCast(MemberwiseClone(), BookAdhocItem)
            Return instance
        End Function

#End Region

    End Class
End Namespace