Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookItem
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
                If _TempControl = 0 Then _TempControl = BookItemControl
                Return _TempControl
            End Get
            Set(value As Integer)
                _TempControl = value
            End Set
        End Property

#End Region

#Region " Data Members"
        Private _BookItemControl As Integer = 0
        <DataMember()> _
        Public Property BookItemControl() As Integer
            Get
                Return _BookItemControl
            End Get
            Set(ByVal value As Integer)
                _BookItemControl = value
            End Set
        End Property

        Private _BookItemBookLoadControl As Integer = 0
        <DataMember()> _
        Public Property BookItemBookLoadControl() As Integer
            Get
                Return _BookItemBookLoadControl
            End Get
            Set(ByVal value As Integer)
                _BookItemBookLoadControl = value
            End Set
        End Property

        Private _BookItemFixOffInvAllow As Double = 0
        <DataMember()> _
        Public Property BookItemFixOffInvAllow() As Double
            Get
                Return _BookItemFixOffInvAllow
            End Get
            Set(ByVal value As Double)
                _BookItemFixOffInvAllow = value
            End Set
        End Property

        Private _BookItemFixFrtAllow As Decimal = 0
        <DataMember()> _
        Public Property BookItemFixFrtAllow() As Decimal
            Get
                Return _BookItemFixFrtAllow
            End Get
            Set(ByVal value As Decimal)
                _BookItemFixFrtAllow = value
            End Set
        End Property

        Private _BookItemItemNumber As String = ""
        <DataMember()> _
        Public Property BookItemItemNumber() As String
            Get
                Return Left(_BookItemItemNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookItemItemNumber = Left(value, 50)
            End Set
        End Property

        Private _BookItemQtyOrdered As Integer = 0
        <DataMember()> _
        Public Property BookItemQtyOrdered() As Integer
            Get
                Return _BookItemQtyOrdered
            End Get
            Set(ByVal value As Integer)
                _BookItemQtyOrdered = value
            End Set
        End Property

        Private _BookItemFreightCost As Decimal = 0
        <DataMember()> _
        Public Property BookItemFreightCost() As Decimal
            Get
                Return _BookItemFreightCost
            End Get
            Set(ByVal value As Decimal)
                _BookItemFreightCost = value
            End Set
        End Property

        Private _BookItemActFreightCost As Decimal? = 0
        <DataMember()> _
        Public Property BookItemActFreightCost() As Decimal?
            Get
                Return _BookItemActFreightCost
            End Get
            Set(ByVal value As Decimal?)
                _BookItemActFreightCost = value
            End Set
        End Property

        Private _BookItemItemCost As Decimal = 0
        <DataMember()> _
        Public Property BookItemItemCost() As Decimal
            Get
                Return _BookItemItemCost
            End Get
            Set(ByVal value As Decimal)
                _BookItemItemCost = value
            End Set
        End Property

        Private _BookItemWeight As Double = 0
        <DataMember()> _
        Public Property BookItemWeight() As Double
            Get
                Return _BookItemWeight
            End Get
            Set(ByVal value As Double)
                _BookItemWeight = value
            End Set
        End Property

        Private _BookItemCube As Integer = 0
        <DataMember()> _
        Public Property BookItemCube() As Integer
            Get
                Return _BookItemCube
            End Get
            Set(ByVal value As Integer)
                _BookItemCube = value
            End Set
        End Property

        Private _BookItemPack As Short = 0
        <DataMember()> _
        Public Property BookItemPack() As Short
            Get
                Return _BookItemPack
            End Get
            Set(ByVal value As Short)
                _BookItemPack = value
            End Set
        End Property

        Private _BookItemSize As String = ""
        <DataMember()> _
        Public Property BookItemSize() As String
            Get
                Return Left(_BookItemSize, 255)
            End Get
            Set(ByVal value As String)
                _BookItemSize = Left(value, 255)
            End Set
        End Property

        Private _BookItemDescription As String = ""
        <DataMember()> _
        Public Property BookItemDescription() As String
            Get
                Return Left(_BookItemDescription, 255)
            End Get
            Set(ByVal value As String)
                _BookItemDescription = Left(value, 255)
            End Set
        End Property

        Private _BookItemHazmat As String = ""
        <DataMember()> _
        Public Property BookItemHazmat() As String
            Get
                Return Left(_BookItemHazmat, 1)
            End Get
            Set(ByVal value As String)
                _BookItemHazmat = Left(value, 1)
            End Set
        End Property

        Private _BookItemModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookItemModDate() As System.Nullable(Of Date)
            Get
                Return _BookItemModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookItemModDate = value
            End Set
        End Property

        Private _BookItemModUser As String = ""
        <DataMember()> _
        Public Property BookItemModUser() As String
            Get
                Return Left(_BookItemModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookItemModUser = Left(value, 100)
            End Set
        End Property

        Private _BookItemBrand As String = ""
        <DataMember()> _
        Public Property BookItemBrand() As String
            Get
                Return Left(_BookItemBrand, 255)
            End Get
            Set(ByVal value As String)
                _BookItemBrand = Left(value, 255)
            End Set
        End Property

        Private _BookItemCostCenter As String = ""
        <DataMember()> _
        Public Property BookItemCostCenter() As String
            Get
                Return Left(_BookItemCostCenter, 50)
            End Get
            Set(ByVal value As String)
                _BookItemCostCenter = Left(value, 50)
            End Set
        End Property

        Private _BookItemLotNumber As String = ""
        <DataMember()> _
        Public Property BookItemLotNumber() As String
            Get
                Return Left(_BookItemLotNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookItemLotNumber = Left(value, 50)
            End Set
        End Property

        Private _BookItemLotExpirationDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookItemLotExpirationDate() As System.Nullable(Of Date)
            Get
                Return _BookItemLotExpirationDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookItemLotExpirationDate = value
            End Set
        End Property

        Private _BookItemGTIN As String = ""
        <DataMember()> _
        Public Property BookItemGTIN() As String
            Get
                Return Left(_BookItemGTIN, 50)
            End Get
            Set(ByVal value As String)
                _BookItemGTIN = Left(value, 50)
            End Set
        End Property

        Private _BookCustItemNumber As String = ""
        <DataMember()> _
        Public Property BookCustItemNumber() As String
            Get
                Return Left(_BookCustItemNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookCustItemNumber = Left(value, 50)
            End Set
        End Property

        Private _BookItemBFC As Decimal = 0
        <DataMember()> _
        Public Property BookItemBFC() As Decimal
            Get
                Return _BookItemBFC
            End Get
            Set(ByVal value As Decimal)
                _BookItemBFC = value
            End Set
        End Property

        Private _BookItemCountryOfOrigin As String = ""
        <DataMember()> _
        Public Property BookItemCountryOfOrigin() As String
            Get
                Return Left(_BookItemCountryOfOrigin, 255)
            End Get
            Set(ByVal value As String)
                _BookItemCountryOfOrigin = Left(value, 255)
            End Set
        End Property

        Private _BookItemHST As String = ""
        <DataMember()> _
        Public Property BookItemHST() As String
            Get
                Return Left(_BookItemHST, 50)
            End Get
            Set(ByVal value As String)
                _BookItemHST = Left(value, 50)
            End Set
        End Property

        Private _BookItemPalletTypeID As Integer = 0
        <DataMember()> _
        Public Property BookItemPalletTypeID() As Integer
            Get
                Return _BookItemPalletTypeID
            End Get
            Set(ByVal value As Integer)
                _BookItemPalletTypeID = value
            End Set
        End Property

        Private _BookItemHazmatTypeCode As String = ""
        <DataMember()> _
        Public Property BookItemHazmatTypeCode() As String
            Get
                Return Left(_BookItemHazmatTypeCode, 20)
            End Get
            Set(ByVal value As String)
                _BookItemHazmatTypeCode = Left(value, 20)
            End Set
        End Property

        Private _BookItem49CFRCode As String = ""
        <DataMember()> _
        Public Property BookItem49CFRCode() As String
            Get
                Return Left(_BookItem49CFRCode, 20)
            End Get
            Set(ByVal value As String)
                _BookItem49CFRCode = Left(value, 20)
            End Set
        End Property

        Private _BookItemIATACode As String = ""
        <DataMember()> _
        Public Property BookItemIATACode() As String
            Get
                Return Left(_BookItemIATACode, 20)
            End Get
            Set(ByVal value As String)
                _BookItemIATACode = Left(value, 20)
            End Set
        End Property

        Private _BookItemDOTCode As String = ""
        <DataMember()> _
        Public Property BookItemDOTCode() As String
            Get
                Return Left(_BookItemDOTCode, 20)
            End Get
            Set(ByVal value As String)
                _BookItemDOTCode = Left(value, 20)
            End Set
        End Property

        Private _BookItemMarineCode As String = ""
        <DataMember()> _
        Public Property BookItemMarineCode() As String
            Get
                Return Left(_BookItemMarineCode, 20)
            End Get
            Set(ByVal value As String)
                _BookItemMarineCode = Left(value, 20)
            End Set
        End Property

        Private _BookItemNMFCClass As String = ""
        <DataMember()> _
        Public Property BookItemNMFCClass() As String
            Get
                Return Left(_BookItemNMFCClass, 20)
            End Get
            Set(ByVal value As String)
                _BookItemNMFCClass = Left(value, 20)
            End Set
        End Property

        Private _BookItemFAKClass As String = ""
        <DataMember()> _
        Public Property BookItemFAKClass() As String
            Get
                Return Left(_BookItemFAKClass, 20)
            End Get
            Set(ByVal value As String)
                _BookItemFAKClass = Left(value, 20)
            End Set
        End Property

        Private _BookItemLimitedQtyFlag As Boolean = False
        <DataMember()> _
        Public Property BookItemLimitedQtyFlag() As Boolean
            Get
                Return _BookItemLimitedQtyFlag
            End Get
            Set(ByVal value As Boolean)
                _BookItemLimitedQtyFlag = value
            End Set
        End Property

        Private _BookItemPallets As Double = 0
        <DataMember()> _
        Public Property BookItemPallets() As Double
            Get
                Return _BookItemPallets
            End Get
            Set(ByVal value As Double)
                _BookItemPallets = value
            End Set
        End Property

        Private _BookItemTies As Double = 0
        <DataMember()> _
        Public Property BookItemTies() As Double
            Get
                Return _BookItemTies
            End Get
            Set(ByVal value As Double)
                _BookItemTies = value
            End Set
        End Property

        Private _BookItemHighs As Double = 0
        <DataMember()> _
        Public Property BookItemHighs() As Double
            Get
                Return _BookItemHighs
            End Get
            Set(ByVal value As Double)
                _BookItemHighs = value
            End Set
        End Property

        Private _BookItemQtyPalletPercentage As Double = 0
        <DataMember()> _
        Public Property BookItemQtyPalletPercentage() As Double
            Get
                Return _BookItemQtyPalletPercentage
            End Get
            Set(ByVal value As Double)
                _BookItemQtyPalletPercentage = value
            End Set
        End Property

        Private _BookItemQtyLength As Double = 0
        <DataMember()> _
        Public Property BookItemQtyLength() As Double
            Get
                Return _BookItemQtyLength
            End Get
            Set(ByVal value As Double)
                _BookItemQtyLength = value
            End Set
        End Property

        Private _BookItemQtyWidth As Double = 0
        <DataMember()> _
        Public Property BookItemQtyWidth() As Double
            Get
                Return _BookItemQtyWidth
            End Get
            Set(ByVal value As Double)
                _BookItemQtyWidth = value
            End Set
        End Property

        Private _BookItemQtyHeight As Double = 0
        <DataMember()> _
        Public Property BookItemQtyHeight() As Double
            Get
                Return _BookItemQtyHeight
            End Get
            Set(ByVal value As Double)
                _BookItemQtyHeight = value
            End Set
        End Property


        Private _BookItemStackable As Boolean = False
        <DataMember()> _
        Public Property BookItemStackable() As Boolean
            Get
                Return _BookItemStackable
            End Get
            Set(ByVal value As Boolean)
                _BookItemStackable = value
            End Set
        End Property


        Private _BookItemLevelOfDensity As Integer = 0
        <DataMember()> _
        Public Property BookItemLevelOfDensity() As Integer
            Get
                Return _BookItemLevelOfDensity
            End Get
            Set(ByVal value As Integer)
                _BookItemLevelOfDensity = value
            End Set
        End Property

        Private _BookItemDiscount As Decimal
        <DataMember()> _
        Public Property BookItemDiscount() As Decimal
            Get
                Return _BookItemDiscount
            End Get
            Set(ByVal value As Decimal)
                _BookItemDiscount = value
            End Set
        End Property

        Private _BookItemLineHaul As Decimal
        <DataMember()> _
        Public Property BookItemLineHaul() As Decimal
            Get
                Return _BookItemLineHaul
            End Get
            Set(ByVal value As Decimal)
                _BookItemLineHaul = value
            End Set
        End Property

        Private _BookItemTaxableFees As Decimal
        <DataMember()> _
        Public Property BookItemTaxableFees() As Decimal
            Get
                Return _BookItemTaxableFees
            End Get
            Set(ByVal value As Decimal)
                _BookItemTaxableFees = value
            End Set
        End Property

        Private _BookItemTaxes As Decimal
        <DataMember()> _
        Public Property BookItemTaxes() As Decimal
            Get
                Return _BookItemTaxes
            End Get
            Set(ByVal value As Decimal)
                _BookItemTaxes = value
            End Set
        End Property

        Private _BookItemNonTaxableFees As Decimal
        <DataMember()> _
        Public Property BookItemNonTaxableFees() As Decimal
            Get
                Return _BookItemNonTaxableFees
            End Get
            Set(ByVal value As Decimal)
                _BookItemNonTaxableFees = value
            End Set
        End Property

        Private _BookItemDeficitCostAdjustment As Decimal
        <DataMember()> _
        Public Property BookItemDeficitCostAdjustment() As Decimal
            Get
                Return _BookItemDeficitCostAdjustment
            End Get
            Set(ByVal value As Decimal)
                _BookItemDeficitCostAdjustment = value
            End Set
        End Property

        Private _BookItemDeficitWeightAdjustment As Decimal
        <DataMember()> _
        Public Property BookItemDeficitWeightAdjustment() As Decimal
            Get
                Return _BookItemDeficitWeightAdjustment
            End Get
            Set(ByVal value As Decimal)
                _BookItemDeficitWeightAdjustment = value
            End Set
        End Property

        Private _BookItemWeightBreak As Decimal
        <DataMember()> _
        Public Property BookItemWeightBreak() As Decimal
            Get
                Return _BookItemWeightBreak
            End Get
            Set(ByVal value As Decimal)
                _BookItemWeightBreak = value
            End Set
        End Property

        Private _BookItemDeficit49CFRCode As String = ""
        <DataMember()> _
        Public Property BookItemDeficit49CFRCode() As String
            Get
                Return Left(_BookItemDeficit49CFRCode, 20)
            End Get
            Set(ByVal value As String)
                _BookItemDeficit49CFRCode = Left(value, 20)
            End Set
        End Property
        
        Private _BookItemDeficitIATACode As String = ""
        <DataMember()> _
        Public Property BookItemDeficitIATACode() As String
            Get
                Return Left(_BookItemDeficitIATACode, 20)
            End Get
            Set(ByVal value As String)
                _BookItemDeficitIATACode = Left(value, 20)
            End Set
        End Property

        Private _BookItemDeficitDOTCode As String = ""
        <DataMember()> _
        Public Property BookItemDeficitDOTCode() As String
            Get
                Return Left(_BookItemDeficitDOTCode, 20)
            End Get
            Set(ByVal value As String)
                _BookItemDeficitDOTCode = Left(value, 20)
            End Set
        End Property

        Private _BookItemDeficitMarineCode As String = ""
        <DataMember()> _
        Public Property BookItemDeficitMarineCode() As String
            Get
                Return Left(_BookItemDeficitMarineCode, 20)
            End Get
            Set(ByVal value As String)
                _BookItemDeficitMarineCode = Left(value, 20)
            End Set
        End Property

        Private _BookItemDeficitNMFCClass As String = ""
        <DataMember()> _
        Public Property BookItemDeficitNMFCClass() As String
            Get
                Return Left(_BookItemDeficitNMFCClass, 20)
            End Get
            Set(ByVal value As String)
                _BookItemDeficitNMFCClass = Left(value, 20)
            End Set
        End Property

        Private _BookItemDeficitFAKClass As String = ""
        <DataMember()> _
        Public Property BookItemDeficitFAKClass() As String
            Get
                Return Left(_BookItemDeficitFAKClass, 20)
            End Get
            Set(ByVal value As String)
                _BookItemDeficitFAKClass = Left(value, 20)
            End Set
        End Property

        Private _BookItemRated49CFRCode As String = ""
        <DataMember()> _
        Public Property BookItemRated49CFRCode() As String
            Get
                Return Left(_BookItemRated49CFRCode, 20)
            End Get
            Set(ByVal value As String)
                _BookItemRated49CFRCode = Left(value, 20)
            End Set
        End Property

        Private _BookItemRatedIATACode As String = ""
        <DataMember()> _
        Public Property BookItemRatedIATACode() As String
            Get
                Return Left(_BookItemRatedIATACode, 20)
            End Get
            Set(ByVal value As String)
                _BookItemRatedIATACode = Left(value, 20)
            End Set
        End Property

        Private _BookItemRatedDOTCode As String = ""
        <DataMember()> _
        Public Property BookItemRatedDOTCode() As String
            Get
                Return Left(_BookItemRatedDOTCode, 20)
            End Get
            Set(ByVal value As String)
                _BookItemRatedDOTCode = Left(value, 20)
            End Set
        End Property

        Private _BookItemRatedMarineCode As String = ""
        <DataMember()> _
        Public Property BookItemRatedMarineCode() As String
            Get
                Return Left(_BookItemRatedMarineCode, 20)
            End Get
            Set(ByVal value As String)
                _BookItemRatedMarineCode = Left(value, 20)
            End Set
        End Property

        Private _BookItemRatedNMFCClass As String = ""
        <DataMember()> _
        Public Property BookItemRatedNMFCClass() As String
            Get
                Return Left(_BookItemRatedNMFCClass, 20)
            End Get
            Set(ByVal value As String)
                _BookItemRatedNMFCClass = Left(value, 20)
            End Set
        End Property

        Private _BookItemRatedFAKClass As String = ""
        <DataMember()> _
        Public Property BookItemRatedFAKClass() As String
            Get
                Return Left(_BookItemRatedFAKClass, 20)
            End Get
            Set(ByVal value As String)
                _BookItemRatedFAKClass = Left(value, 20)
            End Set
        End Property

        Private _BookItemCarrTarEquipMatControl As Integer
        <DataMember()> _
        Public Property BookItemCarrTarEquipMatControl() As Integer
            Get
                Return _BookItemCarrTarEquipMatControl
            End Get
            Set(ByVal value As Integer)
                _BookItemCarrTarEquipMatControl = value
            End Set
        End Property

        Private _BookItemCarrTarEquipMatName As String = ""
        <DataMember()> _
        Public Property BookItemCarrTarEquipMatName() As String
            Get
                Return Left(_BookItemCarrTarEquipMatName, 50)
            End Get
            Set(ByVal value As String)
                _BookItemCarrTarEquipMatName = Left(value, 50)
            End Set
        End Property

        Private _BookItemCarrTarEquipMatDetID As Integer
        <DataMember()> _
        Public Property BookItemCarrTarEquipMatDetID() As Integer
            Get
                Return _BookItemCarrTarEquipMatDetID
            End Get
            Set(ByVal value As Integer)
                _BookItemCarrTarEquipMatDetID = value
            End Set
        End Property

        Private _BookItemCarrTarEquipMatDetValue As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BookItemCarrTarEquipMatDetValue() As System.Nullable(Of Decimal)
            Get
                Return _BookItemCarrTarEquipMatDetValue
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                _BookItemCarrTarEquipMatDetValue = value
            End Set
        End Property

        Private _BookItemUser1 As String = ""
        <DataMember()> _
        Public Property BookItemUser1() As String
            Get
                Return Left(_BookItemUser1, 4000)
            End Get
            Set(ByVal value As String)
                _BookItemUser1 = Left(value, 4000)
            End Set
        End Property

        Private _BookItemUser2 As String = ""
        <DataMember()> _
        Public Property BookItemUser2() As String
            Get
                Return Left(_BookItemUser2, 4000)
            End Get
            Set(ByVal value As String)
                _BookItemUser2 = Left(value, 4000)
            End Set
        End Property

        Private _BookItemUser3 As String = ""
        <DataMember()> _
        Public Property BookItemUser3() As String
            Get
                Return Left(_BookItemUser3, 4000)
            End Get
            Set(ByVal value As String)
                _BookItemUser3 = Left(value, 4000)
            End Set
        End Property

        Private _BookItemUser4 As String = ""
        <DataMember()> _
        Public Property BookItemUser4() As String
            Get
                Return Left(_BookItemUser4, 4000)
            End Get
            Set(ByVal value As String)
                _BookItemUser4 = Left(value, 4000)
            End Set
        End Property

        Private _BookItemUnitOfMeasureControl As Integer
        <DataMember()> _
        Public Property BookItemUnitOfMeasureControl() As Integer
            Get
                Return _BookItemUnitOfMeasureControl
            End Get
            Set(ByVal value As Integer)
                _BookItemUnitOfMeasureControl = value
            End Set
        End Property
         
        Private _BookItemRatedNMFCSubClass As String = ""
        <DataMember()> _
        Public Property BookItemRatedNMFCSubClass() As String
            Get
                Return Left(_BookItemRatedNMFCSubClass, 20)
            End Get
            Set(ByVal value As String)
                _BookItemRatedNMFCSubClass = Left(value, 20)
            End Set
        End Property

        Private _BookItemCommCode As String = ""
        <DataMember()>
        Public Property BookItemCommCode() As String
            Get
                Return Left(_BookItemCommCode, 1)
            End Get
            Set(ByVal value As String)
                _BookItemCommCode = Left(value, 1)
            End Set
        End Property
        'End Begin by RHR for v-8.1 on 03/26/2018

        Private _BookItemHazControl As Integer
        ''' <summary>
        ''' FK to Hazmat table
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.1 on 3/25/2018
        ''' Lookup key for Hazmat type in Hazmat table optional 
        ''' </remarks>
        <DataMember()>
        Public Property BookItemHazControl() As Integer
            Get
                Return _BookItemHazControl
            End Get
            Set(ByVal value As Integer)
                _BookItemHazControl = value
            End Set
        End Property

        Private _BookItemBookPkgControl As Integer
        ''' <summary>
        ''' Reverse Lookup Reference to Book Package Control Table
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.2 on 5/13/2019
        ''' 
        ''' </remarks>
        <DataMember()>
        Public Property BookItemBookPkgControl() As Integer
            Get
                Return _BookItemBookPkgControl
            End Get
            Set(ByVal value As Integer)
                _BookItemBookPkgControl = value
            End Set
        End Property

        Private _BookItemOrderNumber As String
        ''' <summary>
        ''' Stores the Original SO/TO/PO number associated with each item 
        ''' Generally used for NAV Warhouse Shipping
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        '''  Modified by RHR for v-8.2 on 5/13/2019
        ''' </remarks>
        <DataMember()>
        Public Property BookItemOrderNumber() As String
            Get
                Return Left(_BookItemOrderNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookItemOrderNumber = Left(value, 20)
            End Set
        End Property



        'End Modified by RHR for v-8.1 on 03/26/2018
        Private _BookItemUpdated As Byte()
        <DataMember()> _
        Public Property BookItemUpdated() As Byte()
            Get
                Return _BookItemUpdated
            End Get
            Set(ByVal value As Byte())
                _BookItemUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookItem
            instance = DirectCast(MemberwiseClone(), BookItem)
            Return instance
        End Function

        Public Overrides Function ToString() As String
            Return $"[{Me.BookItemOrderNumber}]{me.BookCustItemNumber} ({Me.BookItemControl}) Temp:{Me.TempControl} {Me.BookItemWeight}"
        End Function

#End Region

    End Class
End Namespace