<Serializable()>
Public Class clsBookDetailObject
    Public ItemPONumber As String = ""
    Public FixOffInvAllow As Double = 0
    Public FixFrtAllow As Double = 0
    Public ItemNumber As String = ""
    Public QtyOrdered As Integer = 0
    Public FreightCost As Double = 0
    Public ItemCost As Double = 0
    Public Weight As Double = 0
    Public Cube As Integer = 0
    Public Pack As Short = 0
    Public Size As String = ""
    Public Description As String = ""
    Public Hazmat As String = ""
    Public Brand As String = ""
    Public CostCenter As String = ""
    Public LotNumber As String = ""
    Public LotExpirationDate As String = ""
    Public GTIN As String = ""
    Public CustItemNumber As String = ""
    Public CustomerNumber As String = ""
    Public POOrderSequence As Integer
    Public PalletType As String

End Class

<Serializable()>
Public Class clsBookDetailObject60 : Inherits clsImportDataBase


    Private _ItemPONumber As String = ""
    Public Property ItemPONumber As String
        Get
            Return Left(_ItemPONumber, 20)
        End Get
        Set(value As String)
            _ItemPONumber = Left(value, 20)
        End Set
    End Property

    Private _FixOffInvAllow As Double = 0
    Public Property FixOffInvAllow As Double
        Get
            Return _FixOffInvAllow
        End Get
        Set(value As Double)
            _FixOffInvAllow = value
        End Set
    End Property

    Private _FixFrtAllow As Double = 0
    Public Property FixFrtAllow As Double
        Get
            Return _FixFrtAllow
        End Get
        Set(value As Double)
            _FixFrtAllow = value
        End Set
    End Property

    Private _ItemNumber As String = ""
    Public Property ItemNumber As String
        Get
            Return Left(_ItemNumber, 50)
        End Get
        Set(value As String)
            _ItemNumber = Left(value, 50)
        End Set
    End Property

    Private _QtyOrdered As Integer = 0
    Public Property QtyOrdered As Integer
        Get
            Return _QtyOrdered
        End Get
        Set(value As Integer)
            _QtyOrdered = value
        End Set
    End Property

    Private _FreightCost As Double = 0
    Public Property FreightCost As Double
        Get
            Return _FreightCost
        End Get
        Set(value As Double)
            _FreightCost = value
        End Set
    End Property

    Private _ItemCost As Double = 0
    Public Property ItemCost As Double
        Get
            Return _ItemCost
        End Get
        Set(value As Double)
            _ItemCost = value
        End Set
    End Property

    Private _Weight As Double = 0
    Public Property Weight As Double
        Get
            Return _Weight
        End Get
        Set(value As Double)
            _Weight = value
        End Set
    End Property

    Private _Cube As Integer = 0
    Public Property Cube As Integer
        Get
            Return _Cube
        End Get
        Set(value As Integer)
            _Cube = value
        End Set
    End Property

    Private _Pack As Short = 0
    Public Property Pack As Short
        Get
            Return _Pack
        End Get
        Set(value As Short)
            _Pack = value
        End Set
    End Property

    Private _Size As String = ""
    Public Property Size As String
        Get
            Return Left(_Size, 255)
        End Get
        Set(value As String)
            _Size = Left(value, 255)
        End Set
    End Property

    Private _Description As String = ""
    Public Property Description As String
        Get
            Return Left(_Description, 255)
        End Get
        Set(value As String)
            _Description = Left(value, 255)
        End Set
    End Property

    Private _Hazmat As String = ""
    Public Property Hazmat As String
        Get
            Return Left(_Hazmat, 1)
        End Get
        Set(value As String)
            _Hazmat = Left(value, 1)
        End Set
    End Property

    Private _Brand As String = ""
    Public Property Brand As String
        Get
            Return Left(_Brand, 255)
        End Get
        Set(value As String)
            _Brand = Left(value, 255)
        End Set
    End Property

    Private _CostCenter As String = ""
    Public Property CostCenter As String
        Get
            Return Left(_CostCenter, 50)
        End Get
        Set(value As String)
            _CostCenter = Left(value, 50)
        End Set
    End Property

    Private _LotNumber As String = ""
    Public Property LotNumber As String
        Get
            Return Left(_LotNumber, 50)
        End Get
        Set(value As String)
            _LotNumber = Left(value, 50)
        End Set
    End Property

    Private _LotExpirationDate As String = ""
    Public Property LotExpirationDate As String
        Get
            Return cleanDate(_LotExpirationDate)
        End Get
        Set(value As String)
            _LotExpirationDate = value
        End Set
    End Property

    Private _GTIN As String = ""
    Public Property GTIN As String
        Get
            Return Left(_GTIN, 50)
        End Get
        Set(value As String)
            _GTIN = Left(value, 50)
        End Set
    End Property

    Private _CustItemNumber As String = ""
    Public Property CustItemNumber As String
        Get
            Return Left(_CustItemNumber, 50)
        End Get
        Set(value As String)
            _CustItemNumber = Left(value, 50)
        End Set
    End Property

    Private _CustomerNumber As String = ""
    Public Property CustomerNumber As String
        Get
            Return Left(_CustomerNumber, 20)
        End Get
        Set(value As String)
            _CustomerNumber = Left(value, 20)
        End Set
    End Property

    Private _POOrderSequence As Integer
    Public Property POOrderSequence As Integer
        Get
            Return _POOrderSequence
        End Get
        Set(value As Integer)
            _POOrderSequence = value
        End Set
    End Property

    Private _PalletType As String
    Public Property PalletType As String
        Get
            Return Left(_PalletType, 50)
        End Get
        Set(value As String)
            _PalletType = Left(value, 50)
        End Set
    End Property

    Private _POItemHazmatTypeCode As String
    Public Property POItemHazmatTypeCode As String
        Get
            Return Left(_POItemHazmatTypeCode, 20)
        End Get
        Set(value As String)
            _POItemHazmatTypeCode = Left(value, 20)
        End Set
    End Property

    Private _POItem49CFRCode As String
    Public Property POItem49CFRCode As String
        Get
            Return Left(_POItem49CFRCode, 20)
        End Get
        Set(value As String)
            _POItem49CFRCode = Left(value, 20)
        End Set
    End Property

    Private _POItemIATACode As String
    Public Property POItemIATACode As String
        Get
            Return Left(_POItemIATACode, 20)
        End Get
        Set(value As String)
            _POItemIATACode = Left(value, 20)
        End Set
    End Property

    Private _POItemDOTCode As String
    Public Property POItemDOTCode As String
        Get
            Return Left(_POItemDOTCode, 20)
        End Get
        Set(value As String)
            _POItemDOTCode = Left(value, 20)
        End Set
    End Property

    Private _POItemMarineCode As String
    Public Property POItemMarineCode As String
        Get
            Return Left(_POItemMarineCode, 20)
        End Get
        Set(value As String)
            _POItemMarineCode = Left(value, 20)
        End Set
    End Property

    Private _POItemNMFCClass As String
    Public Property POItemNMFCClass As String
        Get
            Return Left(_POItemNMFCClass, 20)
        End Get
        Set(value As String)
            _POItemNMFCClass = Left(value, 20)
        End Set
    End Property

    Private _POItemFAKClass As String
    Public Property POItemFAKClass As String
        Get
            Return Left(_POItemFAKClass, 20)
        End Get
        Set(value As String)
            _POItemFAKClass = Left(value, 20)
        End Set
    End Property

    Private _POItemLimitedQtyFlag As Boolean
    Public Property POItemLimitedQtyFlag As Boolean
        Get
            Return _POItemLimitedQtyFlag
        End Get
        Set(value As Boolean)
            _POItemLimitedQtyFlag = value
        End Set
    End Property

    Private _POItemPallets As Double
    Public Property POItemPallets As Double
        Get
            Return _POItemPallets
        End Get
        Set(value As Double)
            _POItemPallets = value
        End Set
    End Property

    Private _POItemTies As Double
    Public Property POItemTies As Double
        Get
            Return _POItemTies
        End Get
        Set(value As Double)
            _POItemTies = value
        End Set
    End Property

    Private _POItemHighs As Double
    Public Property POItemHighs As Double
        Get
            Return _POItemHighs
        End Get
        Set(value As Double)
            _POItemHighs = value
        End Set
    End Property

    Private _POItemQtyPalletPercentage As Double
    Public Property POItemQtyPalletPercentage As Double
        Get
            Return _POItemQtyPalletPercentage
        End Get
        Set(value As Double)
            _POItemQtyPalletPercentage = value
        End Set
    End Property

    Private _POItemQtyLength As Double
    Public Property POItemQtyLength As Double
        Get
            Return _POItemQtyLength
        End Get
        Set(value As Double)
            _POItemQtyLength = value
        End Set
    End Property

    Private _POItemQtyWidth As Double
    Public Property POItemQtyWidth As Double
        Get
            Return _POItemQtyWidth
        End Get
        Set(value As Double)
            _POItemQtyWidth = value
        End Set
    End Property

    Private _POItemQtyHeight As Double
    Public Property POItemQtyHeight As Double
        Get
            Return _POItemQtyHeight
        End Get
        Set(value As Double)
            _POItemQtyHeight = value
        End Set
    End Property


    Private _POItemStackable As Boolean = False
    Public Property POItemStackable() As Boolean
        Get
            Return _POItemStackable
        End Get
        Set(ByVal value As Boolean)
            _POItemStackable = value
        End Set
    End Property

    Private _POItemLevelOfDensity As Integer = 0
    Public Property POItemLevelOfDensity() As Integer
        Get
            Return _POItemLevelOfDensity
        End Get
        Set(ByVal value As Integer)
            _POItemLevelOfDensity = value
        End Set
    End Property

End Class


''' <summary>
''' version 6.0.4 of the item details data object
''' </summary>
''' <remarks>
''' Created by RHR for v-6.0.4.7 on 6/8/2017
''' </remarks>
<Serializable()>
Public Class clsBookDetailObject604 : Inherits clsBookDetailObject60

    Private _BookItemCommCode As String
    Public Property BookItemCommCode As String
        Get
            Return Left(_BookItemCommCode, 1)
        End Get
        Set(value As String)
            _BookItemCommCode = Left(value, 1)
        End Set
    End Property

    Private _POItemCustomerPO As String
    Public Property POItemCustomerPO As String
        Get
            Return Left(_POItemCustomerPO, 50)
        End Get
        Set(value As String)
            _POItemCustomerPO = Left(value, 50)
        End Set
    End Property

    Private _POItemLocationCode As String
    Public Property POItemLocationCode As String
        Get
            Return Left(_POItemLocationCode, 50)
        End Get
        Set(value As String)
            _POItemLocationCode = Left(value, 50)
        End Set
    End Property

End Class


''' <summary>
''' Book Detail object for v-7.0
''' </summary>
''' <remarks>
''' Modified by RHR for v-7.0.6.105 on 6/8/2017
'''  changed Inherits to use clsBookDetailObject604 as the base class
''' </remarks>
<Serializable()>
    Public Class clsBookDetailObject70 : Inherits clsBookDetailObject604

        Private _POItemCompLegalEntity As String = ""
        Public Property POItemCompLegalEntity As String
            Get
                Return Left(_POItemCompLegalEntity, 50)
            End Get
            Set(value As String)
                _POItemCompLegalEntity = Left(value, 50)
            End Set
        End Property

        Private _POItemCompAlphaCode As String = ""
        Public Property POItemCompAlphaCode() As String
            Get
                Return Left(_POItemCompAlphaCode, 50)
            End Get
            Set(ByVal value As String)
                _POItemCompAlphaCode = Left(value, 50)
            End Set
        End Property

        Private _POItemNMFCSubClass As String = ""
        Public Property POItemNMFCSubClass As String
            Get
                Return Left(_POItemNMFCSubClass, 20)
            End Get
            Set(value As String)
                _POItemNMFCSubClass = Left(value, 20)
            End Set
        End Property

        Private _POItemUser1 As String = ""
        Public Property POItemUser1 As String
            Get
                Return Left(_POItemUser1, 4000)
            End Get
            Set(value As String)
                _POItemUser1 = Left(value, 4000)
            End Set
        End Property

        Private _POItemUser2 As String = ""
        Public Property POItemUser2 As String
            Get
                Return Left(_POItemUser2, 4000)
            End Get
            Set(value As String)
                _POItemUser2 = Left(value, 4000)
            End Set
        End Property

        Private _POItemUser3 As String = ""
        Public Property POItemUser3 As String
            Get
                Return Left(_POItemUser3, 4000)
            End Get
            Set(value As String)
                _POItemUser3 = Left(value, 4000)
            End Set
        End Property

        Private _POItemUser4 As String = ""
        Public Property POItemUser4 As String
            Get
                Return Left(_POItemUser4, 4000)
            End Get
            Set(value As String)
                _POItemUser4 = Left(value, 4000)
            End Set
        End Property

        ''' <summary>
        ''' Alpha Code lookup to dbo.tblUnitOfMeasure.UnitOfMeasureType used to 
        ''' return the UnitOfMeasureControl mapped to the item level detail for weight
        ''' </summary>
        ''' <remarks></remarks>
        Private _POItemWeightUnitOfMeasure As String = ""
        Public Property POItemWeightUnitOfMeasure As String
            Get
                Return Left(_POItemWeightUnitOfMeasure, 100)
            End Get
            Set(value As String)
                _POItemWeightUnitOfMeasure = Left(value, 100)
            End Set
        End Property

        ''' <summary>
        ''' Alpha Code lookup to dbo.tblUnitOfMeasure.UnitOfMeasureType used to 
        ''' return the UnitOfMeasureControl mapped to the item level detail for Volume
        ''' </summary>
        ''' <remarks></remarks>
        Private _POItemCubeUnitOfMeasure As String = ""
        Public Property POItemCubeUnitOfMeasure As String
            Get
                Return Left(_POItemCubeUnitOfMeasure, 100)
            End Get
            Set(value As String)
                _POItemCubeUnitOfMeasure = Left(value, 100)
            End Set
        End Property

        ''' <summary>
        ''' Alpha Code lookup to dbo.tblUnitOfMeasure.UnitOfMeasureType used to 
        ''' return the UnitOfMeasureControl mapped to the item level detail for 
        ''' Dimension: POItemQtyLength, POItemQtyWidth, and POItemQtyHeight
        ''' </summary>
        ''' <remarks></remarks>
        Private _POItemDimensionUnitOfMeasure As String = ""
        Public Property POItemDimensionUnitOfMeasure As String
            Get
                Return Left(_POItemDimensionUnitOfMeasure, 100)
            End Get
            Set(value As String)
                _POItemDimensionUnitOfMeasure = Left(value, 100)
            End Set
        End Property

        Public Shared Function GenerateSampleObject(ByVal OrderNumber As String, ByVal CompNumber As Integer, ByVal CompAlphaCode As String, ByVal CompLegalEntity As String) As clsBookDetailObject70

            Return New clsBookDetailObject70 With {
                    .POItemCompAlphaCode = CompAlphaCode,
                   .POItemCompLegalEntity = CompLegalEntity,
                   .CustomerNumber = CompNumber.ToString(),
                   .ItemPONumber = OrderNumber,
                   .ItemNumber = "ABC",
                   .QtyOrdered = 1,
                   .Weight = 14000.0,
                   .Cube = 100,
                   .POItemPallets = 1}

        End Function


    End Class


    <Serializable()>
    Public Class clsBookDetailObject705 : Inherits clsBookDetailObject70

        Private _ChangeNo As String = ""
    ''' <summary>
    ''' ERP System FK field for reference to header record
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.100 7/21/2016
    ''' Needed a reference to the ERP systems header key to assist with 
    ''' matching item detail records when duplicate records are transmitted in the same batch
    ''' </remarks>
    Public Property ChangeNo() As String
        Get
            Return Me._ChangeNo
        End Get
        Set(value As String)
            Me._ChangeNo = value
        End Set
    End Property


End Class

''' <summary>
''' v-8.0 version of the Book Item Details 
''' </summary>
''' <remarks>
''' 
''' Modifief by RHR for v-8.5.1.001 on 01/21/2022  merged properties
'''     Parent class properties do not display in WSDL so we do not inherit from older versions any longer.
'''     we just inherit from clsImportDataBase
''' </remarks>
<Serializable()>
Public Class clsBookDetailObject80 : Inherits clsImportDataBase



    Private _ItemPONumber As String = ""
    Public Property ItemPONumber As String
        Get
            Return Left(_ItemPONumber, 20)
        End Get
        Set(value As String)
            _ItemPONumber = Left(value, 20)
        End Set
    End Property

    Private _FixOffInvAllow As Double = 0
    Public Property FixOffInvAllow As Double
        Get
            Return _FixOffInvAllow
        End Get
        Set(value As Double)
            _FixOffInvAllow = value
        End Set
    End Property

    Private _FixFrtAllow As Double = 0
    Public Property FixFrtAllow As Double
        Get
            Return _FixFrtAllow
        End Get
        Set(value As Double)
            _FixFrtAllow = value
        End Set
    End Property

    Private _ItemNumber As String = ""
    Public Property ItemNumber As String
        Get
            Return Left(_ItemNumber, 50)
        End Get
        Set(value As String)
            _ItemNumber = Left(value, 50)
        End Set
    End Property

    Private _QtyOrdered As Integer = 0
    Public Property QtyOrdered As Integer
        Get
            Return _QtyOrdered
        End Get
        Set(value As Integer)
            _QtyOrdered = value
        End Set
    End Property

    Private _FreightCost As Double = 0
    Public Property FreightCost As Double
        Get
            Return _FreightCost
        End Get
        Set(value As Double)
            _FreightCost = value
        End Set
    End Property

    Private _ItemCost As Double = 0
    Public Property ItemCost As Double
        Get
            Return _ItemCost
        End Get
        Set(value As Double)
            _ItemCost = value
        End Set
    End Property

    Private _Weight As Double = 0
    Public Property Weight As Double
        Get
            Return _Weight
        End Get
        Set(value As Double)
            _Weight = value
        End Set
    End Property

    Private _Cube As Integer = 0
    Public Property Cube As Integer
        Get
            Return _Cube
        End Get
        Set(value As Integer)
            _Cube = value
        End Set
    End Property

    Private _Pack As Short = 0
    Public Property Pack As Short
        Get
            Return _Pack
        End Get
        Set(value As Short)
            _Pack = value
        End Set
    End Property

    Private _Size As String = ""
    Public Property Size As String
        Get
            Return Left(_Size, 255)
        End Get
        Set(value As String)
            _Size = Left(value, 255)
        End Set
    End Property

    Private _Description As String = ""
    Public Property Description As String
        Get
            Return Left(_Description, 255)
        End Get
        Set(value As String)
            _Description = Left(value, 255)
        End Set
    End Property

    Private _Hazmat As String = ""
    Public Property Hazmat As String
        Get
            Return Left(_Hazmat, 1)
        End Get
        Set(value As String)
            _Hazmat = Left(value, 1)
        End Set
    End Property

    Private _Brand As String = ""
    Public Property Brand As String
        Get
            Return Left(_Brand, 255)
        End Get
        Set(value As String)
            _Brand = Left(value, 255)
        End Set
    End Property

    Private _CostCenter As String = ""
    Public Property CostCenter As String
        Get
            Return Left(_CostCenter, 50)
        End Get
        Set(value As String)
            _CostCenter = Left(value, 50)
        End Set
    End Property

    Private _LotNumber As String = ""
    Public Property LotNumber As String
        Get
            Return Left(_LotNumber, 50)
        End Get
        Set(value As String)
            _LotNumber = Left(value, 50)
        End Set
    End Property

    Private _LotExpirationDate As String = ""
    Public Property LotExpirationDate As String
        Get
            Return cleanDate(_LotExpirationDate)
        End Get
        Set(value As String)
            _LotExpirationDate = value
        End Set
    End Property

    Private _GTIN As String = ""
    Public Property GTIN As String
        Get
            Return Left(_GTIN, 50)
        End Get
        Set(value As String)
            _GTIN = Left(value, 50)
        End Set
    End Property

    Private _CustItemNumber As String = ""
    Public Property CustItemNumber As String
        Get
            Return Left(_CustItemNumber, 50)
        End Get
        Set(value As String)
            _CustItemNumber = Left(value, 50)
        End Set
    End Property

    Private _CustomerNumber As String = ""
    Public Property CustomerNumber As String
        Get
            Return Left(_CustomerNumber, 20)
        End Get
        Set(value As String)
            _CustomerNumber = Left(value, 20)
        End Set
    End Property

    Private _POOrderSequence As Integer
    Public Property POOrderSequence As Integer
        Get
            Return _POOrderSequence
        End Get
        Set(value As Integer)
            _POOrderSequence = value
        End Set
    End Property

    Private _PalletType As String
    Public Property PalletType As String
        Get
            Return Left(_PalletType, 50)
        End Get
        Set(value As String)
            _PalletType = Left(value, 50)
        End Set
    End Property

    Private _POItemHazmatTypeCode As String
    Public Property POItemHazmatTypeCode As String
        Get
            Return Left(_POItemHazmatTypeCode, 20)
        End Get
        Set(value As String)
            _POItemHazmatTypeCode = Left(value, 20)
        End Set
    End Property

    Private _POItem49CFRCode As String
    Public Property POItem49CFRCode As String
        Get
            Return Left(_POItem49CFRCode, 20)
        End Get
        Set(value As String)
            _POItem49CFRCode = Left(value, 20)
        End Set
    End Property

    Private _POItemIATACode As String
    Public Property POItemIATACode As String
        Get
            Return Left(_POItemIATACode, 20)
        End Get
        Set(value As String)
            _POItemIATACode = Left(value, 20)
        End Set
    End Property

    Private _POItemDOTCode As String
    Public Property POItemDOTCode As String
        Get
            Return Left(_POItemDOTCode, 20)
        End Get
        Set(value As String)
            _POItemDOTCode = Left(value, 20)
        End Set
    End Property

    Private _POItemMarineCode As String
    Public Property POItemMarineCode As String
        Get
            Return Left(_POItemMarineCode, 20)
        End Get
        Set(value As String)
            _POItemMarineCode = Left(value, 20)
        End Set
    End Property

    Private _POItemNMFCClass As String
    Public Property POItemNMFCClass As String
        Get
            Return Left(_POItemNMFCClass, 20)
        End Get
        Set(value As String)
            _POItemNMFCClass = Left(value, 20)
        End Set
    End Property

    Private _POItemFAKClass As String
    Public Property POItemFAKClass As String
        Get
            Return Left(_POItemFAKClass, 20)
        End Get
        Set(value As String)
            _POItemFAKClass = Left(value, 20)
        End Set
    End Property

    Private _POItemLimitedQtyFlag As Boolean
    Public Property POItemLimitedQtyFlag As Boolean
        Get
            Return _POItemLimitedQtyFlag
        End Get
        Set(value As Boolean)
            _POItemLimitedQtyFlag = value
        End Set
    End Property

    Private _POItemPallets As Double
    Public Property POItemPallets As Double
        Get
            Return _POItemPallets
        End Get
        Set(value As Double)
            _POItemPallets = value
        End Set
    End Property

    Private _POItemTies As Double
    Public Property POItemTies As Double
        Get
            Return _POItemTies
        End Get
        Set(value As Double)
            _POItemTies = value
        End Set
    End Property

    Private _POItemHighs As Double
    Public Property POItemHighs As Double
        Get
            Return _POItemHighs
        End Get
        Set(value As Double)
            _POItemHighs = value
        End Set
    End Property

    Private _POItemQtyPalletPercentage As Double
    Public Property POItemQtyPalletPercentage As Double
        Get
            Return _POItemQtyPalletPercentage
        End Get
        Set(value As Double)
            _POItemQtyPalletPercentage = value
        End Set
    End Property

    Private _POItemQtyLength As Double
    Public Property POItemQtyLength As Double
        Get
            Return _POItemQtyLength
        End Get
        Set(value As Double)
            _POItemQtyLength = value
        End Set
    End Property

    Private _POItemQtyWidth As Double
    Public Property POItemQtyWidth As Double
        Get
            Return _POItemQtyWidth
        End Get
        Set(value As Double)
            _POItemQtyWidth = value
        End Set
    End Property

    Private _POItemQtyHeight As Double
    Public Property POItemQtyHeight As Double
        Get
            Return _POItemQtyHeight
        End Get
        Set(value As Double)
            _POItemQtyHeight = value
        End Set
    End Property


    Private _POItemStackable As Boolean = False
    Public Property POItemStackable() As Boolean
        Get
            Return _POItemStackable
        End Get
        Set(ByVal value As Boolean)
            _POItemStackable = value
        End Set
    End Property

    Private _POItemLevelOfDensity As Integer = 0
    Public Property POItemLevelOfDensity() As Integer
        Get
            Return _POItemLevelOfDensity
        End Get
        Set(ByVal value As Integer)
            _POItemLevelOfDensity = value
        End Set
    End Property

    Private _BookItemCommCode As String
    Public Property BookItemCommCode As String
        Get
            Return Left(_BookItemCommCode, 1)
        End Get
        Set(value As String)
            _BookItemCommCode = Left(value, 1)
        End Set
    End Property

    Private _POItemCustomerPO As String
    Public Property POItemCustomerPO As String
        Get
            Return Left(_POItemCustomerPO, 50)
        End Get
        Set(value As String)
            _POItemCustomerPO = Left(value, 50)
        End Set
    End Property

    Private _POItemLocationCode As String
    Public Property POItemLocationCode As String
        Get
            Return Left(_POItemLocationCode, 50)
        End Get
        Set(value As String)
            _POItemLocationCode = Left(value, 50)
        End Set
    End Property

    Private _POItemCompLegalEntity As String = ""
    Public Property POItemCompLegalEntity As String
        Get
            Return Left(_POItemCompLegalEntity, 50)
        End Get
        Set(value As String)
            _POItemCompLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _POItemCompAlphaCode As String = ""
    Public Property POItemCompAlphaCode() As String
        Get
            Return Left(_POItemCompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _POItemCompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _POItemNMFCSubClass As String = ""
    Public Property POItemNMFCSubClass As String
        Get
            Return Left(_POItemNMFCSubClass, 20)
        End Get
        Set(value As String)
            _POItemNMFCSubClass = Left(value, 20)
        End Set
    End Property

    Private _POItemUser1 As String = ""
    Public Property POItemUser1 As String
        Get
            Return Left(_POItemUser1, 4000)
        End Get
        Set(value As String)
            _POItemUser1 = Left(value, 4000)
        End Set
    End Property

    Private _POItemUser2 As String = ""
    Public Property POItemUser2 As String
        Get
            Return Left(_POItemUser2, 4000)
        End Get
        Set(value As String)
            _POItemUser2 = Left(value, 4000)
        End Set
    End Property

    Private _POItemUser3 As String = ""
    Public Property POItemUser3 As String
        Get
            Return Left(_POItemUser3, 4000)
        End Get
        Set(value As String)
            _POItemUser3 = Left(value, 4000)
        End Set
    End Property

    Private _POItemUser4 As String = ""
    Public Property POItemUser4 As String
        Get
            Return Left(_POItemUser4, 4000)
        End Get
        Set(value As String)
            _POItemUser4 = Left(value, 4000)
        End Set
    End Property

    ''' <summary>
    ''' Alpha Code lookup to dbo.tblUnitOfMeasure.UnitOfMeasureType used to 
    ''' return the UnitOfMeasureControl mapped to the item level detail for weight
    ''' </summary>
    ''' <remarks></remarks>
    Private _POItemWeightUnitOfMeasure As String = ""
    Public Property POItemWeightUnitOfMeasure As String
        Get
            Return Left(_POItemWeightUnitOfMeasure, 100)
        End Get
        Set(value As String)
            _POItemWeightUnitOfMeasure = Left(value, 100)
        End Set
    End Property

    ''' <summary>
    ''' Alpha Code lookup to dbo.tblUnitOfMeasure.UnitOfMeasureType used to 
    ''' return the UnitOfMeasureControl mapped to the item level detail for Volume
    ''' </summary>
    ''' <remarks></remarks>
    Private _POItemCubeUnitOfMeasure As String = ""
    Public Property POItemCubeUnitOfMeasure As String
        Get
            Return Left(_POItemCubeUnitOfMeasure, 100)
        End Get
        Set(value As String)
            _POItemCubeUnitOfMeasure = Left(value, 100)
        End Set
    End Property

    ''' <summary>
    ''' Alpha Code lookup to dbo.tblUnitOfMeasure.UnitOfMeasureType used to 
    ''' return the UnitOfMeasureControl mapped to the item level detail for 
    ''' Dimension: POItemQtyLength, POItemQtyWidth, and POItemQtyHeight
    ''' </summary>
    ''' <remarks></remarks>
    Private _POItemDimensionUnitOfMeasure As String = ""
    Public Property POItemDimensionUnitOfMeasure As String
        Get
            Return Left(_POItemDimensionUnitOfMeasure, 100)
        End Get
        Set(value As String)
            _POItemDimensionUnitOfMeasure = Left(value, 100)
        End Set
    End Property

    Private _ChangeNo As String = ""
    ''' <summary>
    ''' ERP System FK field for reference to header record
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.100 7/21/2016
    ''' Needed a reference to the ERP systems header key to assist with 
    ''' matching item detail records when duplicate records are transmitted in the same batch
    ''' </remarks>
    Public Property ChangeNo() As String
        Get
            Return Me._ChangeNo
        End Get
        Set(value As String)
            Me._ChangeNo = value
        End Set
    End Property


    '***********************************************

    Private _POItemOrderNumber As String = ""
    Public Property POItemOrderNumber As String
        Get
            Return Left(_POItemOrderNumber, 20)
        End Get
        Set(value As String)
            _POItemOrderNumber = Left(value, 20)
        End Set
    End Property

End Class
