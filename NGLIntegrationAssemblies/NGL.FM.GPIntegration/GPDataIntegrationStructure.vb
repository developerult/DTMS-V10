#Const StandAlone = True
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.ServiceModel
Imports System.IO
Imports System.Reflection
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports Ngl.FreightMaster
Imports Ngl.Core
Imports Ngl.FM

'Public Class GPDataIntegrationSTructure : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
Public Class GPDataIntegrationSTructure : Inherits FreightMaster.Core.NGLCommandLineBaseClass


    Public Class GPPMHeaderData

        Private DebitActStr As String = ""

        Property DebitGL() As String

            Get

                Return DebitActStr

            End Get
            Set(value As String)

                DebitActStr = value

            End Set

        End Property

        Private CreditActStr As String = ""

        Property CreditGL() As String

            Get

                Return CreditActStr

            End Get
            Set(value As String)

                CreditActStr = value

            End Set

        End Property


        Private InvNum As String = ""

        Property InvoiceNumber() As String

            Get

                Return InvNum

            End Get
            Set(value As String)

                InvNum = value

            End Set

        End Property

        Private VendID As String = ""

        Property VendorID() As String

            Get

                Return VendID

            End Get
            Set(value As String)

                VendID = value

            End Set

        End Property

        Private DocType As Short = 1

        Property DocumentType() As Short

            Get

                Return DocType

            End Get
            Set(value As Short)

                DocType = value

            End Set

        End Property


        Private DocDate As Date = DateValue("01/01/1900")

        Property DocumentDate() As Date

            Get

                Return DocDate

            End Get
            Set(value As Date)

                DocDate = value

            End Set

        End Property

        Private DocAmt As Double = 0

        Property DocumentAmount() As Double

            Get

                Return DocAmt

            End Get
            Set(value As Double)

                DocAmt = value

            End Set

        End Property

        Private TaxAmt As Double = 0

        Property TaxAmount() As Double

            Get

                Return TaxAmt

            End Get
            Set(value As Double)

                TaxAmt = value

            End Set

        End Property

        Private POAmt As Double = 0

        Property PurchaseAmount() As Double

            Get

                Return POAmt

            End Get
            Set(value As Double)

                POAmt = value

            End Set

        End Property

        Private ChrgAmt As Double = 0

        Property ChargeAmount() As Double

            Get

                Return ChrgAmt

            End Get
            Set(value As Double)

                ChrgAmt = value

            End Set

        End Property


        Private FrgAmt As Double = 0

        Property FreightAmount() As Double

            Get

                Return FrgAmt

            End Get
            Set(value As Double)

                FrgAmt = value

            End Set

        End Property



        Private PONum As String = ""

        Property PONumber() As String

            Get

                Return PONum

            End Get
            Set(value As String)

                PONum = value

            End Set

        End Property

        Private TransDesc As String = ""

        Property TransactionDescription() As String

            Get

                Return TransDesc

            End Get
            Set(value As String)

                TransDesc = value

            End Set

        End Property

    End Class

    Public Class GPCompanies

        Private GPDB As String = ""

        Property GPDatabase() As String

            Get

                Return GPDB

            End Get
            Set(value As String)

                GPDB = value

            End Set

        End Property

    End Class

    'Added by SEM 2017-08-24
    'Freight Bill Costs Structure
    Public Class FreightBillCosts

        Private OrdNumber As String = ""
        Private FBAmt As Decimal = 0

        Property OrderNumber() As String

            Get

                Return OrdNumber

            End Get
            Set(value As String)

                OrdNumber = value

            End Set

        End Property

        Property FbAmount() As Decimal

            Get

                Return FBAmt

            End Get
            Set(value As Decimal)

                FBAmt = value

            End Set

        End Property

    End Class

    Public Class FromAddress

        Private AddName As String = ""

        Property AddressName() As String

            Get

                Return AddName

            End Get
            Set(value As String)

                AddName = value

            End Set

        End Property

        Private AddID As String = ""

        Property AddressID() As String

            Get

                Return AddID

            End Get
            Set(value As String)

                AddID = value

            End Set

        End Property

        Private Add1 As String = ""

        Property Address1() As String

            Get

                Return Add1

            End Get
            Set(value As String)

                Add1 = value

            End Set

        End Property

        Private Add2 As String = ""

        Property Address2() As String

            Get

                Return Add2

            End Get
            Set(value As String)

                Add2 = value

            End Set

        End Property

        Private Add3 As String = ""

        Property Address3() As String

            Get

                Return Add3

            End Get
            Set(value As String)

                Add3 = value

            End Set

        End Property

        Private Cty As String = ""

        Property City() As String

            Get

                Return Cty

            End Get
            Set(value As String)

                Cty = value

            End Set

        End Property

        Private St As String = ""

        Property State() As String

            Get

                Return St

            End Get
            Set(value As String)

                St = value

            End Set

        End Property

        Private Zip As String = ""

        Property ZipCode() As String

            Get

                Return Zip

            End Get
            Set(value As String)

                Zip = value

            End Set

        End Property

        Private Cntry As String = ""

        Property Country() As String

            Get

                Return Cntry

            End Get
            Set(value As String)

                Cntry = value

            End Set

        End Property

        Private Phn As String = ""

        Property Phone() As String

            Get

                Return Phn

            End Get
            Set(value As String)

                Phn = value

            End Set

        End Property

        Private Fx As String = ""

        Property Fax() As String

            Get

                Return Fx

            End Get
            Set(value As String)

                Fx = value

            End Set

        End Property


        Private Eml As String = ""

        Property Email() As String

            Get

                Return Eml

            End Get
            Set(value As String)

                Eml = value

            End Set

        End Property

    End Class

    'SEM Added for cost per unit
    '2017-08-02
    Public Class OrderUnitPercentage

        Private _OrderNumber As String = ""
        Private _OrderTotalQuantity As Double = 0
        Private _OrderUnitPercentage As Double = 0

        Property OrderNumber() As String

            Get

                Return _OrderNumber

            End Get
            Set(value As String)

                _OrderNumber = value

            End Set

        End Property

        Property OrderTotalQuantity() As Double

            Get

                Return _OrderTotalQuantity

            End Get
            Set(value As Double)

                _OrderTotalQuantity = value

            End Set

        End Property

        Property OrderUnitPercentage() As Double

            Get

                Return _OrderUnitPercentage

            End Get
            Set(value As Double)

                _OrderUnitPercentage = value

            End Set

        End Property

    End Class


    Public Class SOPOrders

        Private SOPOrd As String = ""

        Property SOPOrders() As String

            Get

                Return SOPOrd

            End Get
            Set(value As String)

                SOPOrd = value

            End Set

        End Property

    End Class

    Public Class InvTransfers

        Private InvTrnsfrID As String = ""

        Property InvTransferID() As String

            Get

                Return InvTrnsfrID

            End Get
            Set(value As String)

                InvTrnsfrID = value

            End Set

        End Property

    End Class


    Public Class POOrders

        Private POOrd As String = ""

        Property POOrders() As String

            Get

                Return POOrd

            End Get
            Set(value As String)

                POOrd = value

            End Set

        End Property

    End Class


    Public Class TMSHeader

        Private IntID As Guid = New Guid
        Private TransType As Int16 = 0
        Private CRUDSt As Int16 = 0
        Private TransNumber As String = ""
        Private LnNumber As String = ""
        Private TransDate As Date = DateValue("01/01/1900")
        Private ShpDate As Date = DateValue("01/01/1900")
        Private TransBuyer As String = ""
        Private FrtType As GPEnumValues.FrieghtType = 0
        Private TransTotal As Double = 0
        Private TransWeight As Double = 0
        Private TransCube As Integer = 0

        Property TransactionType() As Int16

            Get

                Return TransType

            End Get
            Set(value As Int16)

                TransType = value

            End Set

        End Property

        Property CRUDStatus() As String

            Get

                Return CRUDSt

            End Get
            Set(value As String)

                CRUDSt = value

            End Set

        End Property

        Property TransactionNumber() As String

            Get

                Return TransNumber

            End Get
            Set(value As String)

                TransNumber = value

            End Set

        End Property

        Property LaneNumber() As String

            Get

                Return LnNumber

            End Get
            Set(value As String)

                LnNumber = value

            End Set

        End Property

        Property TransactionDate() As Date

            Get

                Return TransDate

            End Get
            Set(value As Date)

                TransDate = value

            End Set

        End Property

        Property ShippmentDate() As Date

            Get

                Return ShpDate

            End Get
            Set(value As Date)

                ShpDate = value

            End Set

        End Property

        Property TransactionOriginator() As String

            Get

                Return TransBuyer

            End Get
            Set(value As String)

                TransBuyer = value

            End Set

        End Property

    End Class


    Public Class TMSLine

        Private TransNumber As String = ""
        Private FxOffInvAlow As Double = 0
        Private FxFrtAlow As Double = 0
        Private ItmNumber As String = ""
        Private QtyOrdered As Integer = 0
        Private FrtCost As Double = 0
        Private ItmCost As Double = 0
        Private TransWeight As Double = 0
        Private TransCube As Integer = 0
        Private TransPack As Int16 = 0
        Private TransSize As String = ""
        Private Descrpt As String = ""

        Property TransactionNumber() As String

            Get

                Return TransNumber

            End Get
            Set(value As String)

                TransNumber = value

            End Set

        End Property

        Property FixOffIventoryAllow() As Double

            Get

                Return FxOffInvAlow

            End Get
            Set(value As Double)

                FxOffInvAlow = value

            End Set

        End Property

        Property FixFreightAllow As Double

            Get

                Return FxFrtAlow

            End Get
            Set(value As Double)

                FxFrtAlow = value

            End Set

        End Property

        Property ItemNumber As String

            Get

                Return ItmNumber

            End Get
            Set(value As String)

                ItmNumber = value

            End Set

        End Property

        Property QuantityOrdered As Integer

            Get

                Return QtyOrdered

            End Get
            Set(value As Integer)

                QtyOrdered = value

            End Set

        End Property

        Property FreightCosts As Double

            Get

                Return FrtCost

            End Get
            Set(value As Double)

                FrtCost = value

            End Set

        End Property

        Property ItemCosts As Double

            Get

                Return ItmCost

            End Get
            Set(value As Double)

                ItmCost = value

            End Set

        End Property

        Property TransactionWeight As Double

            Get

                Return TransWeight

            End Get
            Set(value As Double)

                TransWeight = value

            End Set

        End Property

        Property TransactionCube As Integer

            Get

                Return TransCube

            End Get
            Set(value As Integer)

                TransCube = value

            End Set

        End Property

        Property TransactionPack As Int16

            Get

                Return TransPack

            End Get
            Set(value As Int16)

                TransPack = value

            End Set

        End Property

        Property TransactionSize As String

            Get

                Return TransSize

            End Get
            Set(value As String)

                TransSize = value

            End Set

        End Property

        Property TransactionDescription As String

            Get

                Return Descrpt

            End Get
            Set(value As String)

                Descrpt = value

            End Set

        End Property

    End Class

    Public Class VendorFound

        Private DB As String = ""
        Private VendID As String = ""

        Property GPCompany() As String

            Get

                Return DB

            End Get

            Set(value As String)

                DB = value

            End Set

        End Property

        Property VendorID() As String

            Get

                Return VendID

            End Get

            Set(value As String)

                VendID = value

            End Set

        End Property


    End Class

    '  Below is the vendor master record layout
    '  Added by Scott McFarland on Feb 19, 2017
    '  This will be used to query DB1 from DB2 and add a vendor to DB1
    Public Class VendorMaster

        Private VenID As String = ""
        Private VenName As String = ""
        Private VenClass As String = ""
        Private PrimAddrs As String = ""
        Private VenContact As String = ""
        Private Add1 As String = ""
        Private Add2 As String = ""
        Private Add3 As String = ""
        Private Cty As String = ""
        Private St As String = ""
        Private Zip As String = ""
        Private Ccode As String = ""
        Private Cntry As String = ""
        Private Ph1 As String = ""
        Private FaxNum As String = ""
        Private TaxSchID As String = ""
        Private ShipMethod As String = ""
        Private UPSZn As String = ""
        Private POAdd As String = ""
        Private VendRemitAdd As String = ""
        Private VendAcctNum As String = ""
        Private PayTermID As String = ""
        Private PayPriority As String = ""
        Private TrdDis As Double = 0
        Private TaxIDNum As String = ""
        Private TaxResNum As String = ""
        Private ChkBkID As String = ""
        Private UserDf1 As String = ""
        Private UserDf2 As String = ""
        Private Ten99Typ As Integer = 0
        Private Ten99Box As Integer = 0
        Private FOB As Integer = 0
        Private CRType As Integer = 0
        Private CrLmtAmt As Integer = 0
        Private CashAcct As Integer = 0
        Private AcctPayAcct As Integer = 0
        Private TermDisAcct As Integer = 0
        Private TermDisTakenAcct As Integer = 0
        Private FCAcct As Integer = 0
        Private POAcct As Integer = 0
        Private TradeDisAcct As Integer = 0
        Private MiscAcct As Integer = 0
        Private FreightAcct As Integer = 0
        Private TaxAcct As Integer = 0
        Private WOAcct As Integer = 0
        Private AccrPOAcct As Integer = 0
        Private PPVAcct As Integer = 0

        Property VendorID() As String

            Get

                Return VenID

            End Get

            Set(value As String)

                VenID = value

            End Set

        End Property

        Property Vendorname() As String

            Get

                Return VenName

            End Get

            Set(value As String)

                VenName = value

            End Set

        End Property

        Property VendorclassID() As String

            Get

                Return VenClass

            End Get

            Set(value As String)

                VenClass = value

            End Set

        End Property


        Property PrimaryvendoraddressID() As String

            Get

                Return PrimAddrs

            End Get

            Set(value As String)

                PrimAddrs = value

            End Set

        End Property

        Property Vendorcontact() As String

            Get

                Return VenContact

            End Get

            Set(value As String)

                VenContact = value

            End Set

        End Property

        Property Address1() As String

            Get

                Return Add1

            End Get

            Set(value As String)

                Add1 = value

            End Set

        End Property

        Property Address2() As String

            Get

                Return Add2

            End Get

            Set(value As String)

                Add2 = value

            End Set

        End Property

        Property Address3() As String

            Get

                Return Add3

            End Get

            Set(value As String)

                Add3 = value

            End Set

        End Property

        Property City() As String

            Get

                Return Cty

            End Get

            Set(value As String)

                Cty = value

            End Set

        End Property


        Property State() As String

            Get

                Return St

            End Get

            Set(value As String)

                St = value

            End Set

        End Property

        Property ZipCode() As String

            Get

                Return Zip

            End Get

            Set(value As String)

                Zip = value

            End Set

        End Property

        Property Countrycode() As String

            Get

                Return Ccode

            End Get

            Set(value As String)

                Ccode = value

            End Set

        End Property

        Property Country() As String

            Get

                Return Cntry

            End Get

            Set(value As String)

                Cntry = value

            End Set

        End Property

        Property Phone1() As String

            Get

                Return Ph1

            End Get

            Set(value As String)

                Ph1 = value

            End Set

        End Property

        Property Faxnumber() As String

            Get

                Return FaxNum

            End Get

            Set(value As String)

                FaxNum = value

            End Set

        End Property

        Property TaxscheduleID() As String

            Get

                Return TaxSchID

            End Get

            Set(value As String)

                TaxSchID = value

            End Set

        End Property

        Property Shippingmethod() As String

            Get

                Return ShipMethod

            End Get

            Set(value As String)

                ShipMethod = value

            End Set

        End Property

        Property UPSzone() As String

            Get

                Return UPSZn

            End Get

            Set(value As String)

                UPSZn = value

            End Set

        End Property


        Property VendorpurchaseaddressID() As String

            Get

                Return POAdd

            End Get

            Set(value As String)

                POAdd = value

            End Set

        End Property


        Property VendorremittoaddressID() As String

            Get

                Return VendRemitAdd

            End Get

            Set(value As String)

                VendRemitAdd = value

            End Set

        End Property

        Property Vendoraccountnumber() As String

            Get

                Return VendAcctNum

            End Get

            Set(value As String)

                VendAcctNum = value

            End Set

        End Property

        Property PaymenttermsID() As String

            Get

                Return PayTermID

            End Get

            Set(value As String)

                PayTermID = value

            End Set

        End Property

        Property Paymentpriority() As String

            Get

                Return PayPriority

            End Get

            Set(value As String)

                PayPriority = value

            End Set

        End Property


        Property Tradediscount() As Double

            Get

                Return TrdDis

            End Get

            Set(value As Double)

                TrdDis = value

            End Set

        End Property


        Property TaxIDnumber() As String

            Get

                Return TaxIDNum

            End Get

            Set(value As String)

                TaxIDNum = value

            End Set

        End Property


        Property Taxregistrationnumber() As String

            Get

                Return TaxResNum

            End Get

            Set(value As String)

                TaxResNum = value

            End Set

        End Property


        Property CheckbookID() As String

            Get

                Return ChkBkID

            End Get

            Set(value As String)

                ChkBkID = value

            End Set

        End Property


        Property UserDefined1 As String

            Get

                Return UserDf1

            End Get

            Set(value As String)

                UserDf1 = value

            End Set

        End Property

        Property UserDefined2 As String

            Get

                Return UserDf2

            End Get

            Set(value As String)

                UserDf2 = value

            End Set

        End Property


        Property Tax1099type() As Integer

            Get

                Return Ten99Typ

            End Get

            Set(value As Integer)

                Ten99Typ = value

            End Set

        End Property

        Property Tax1099boxnumber() As Integer

            Get

                Return Ten99Box

            End Get

            Set(value As Integer)

                Ten99Box = value

            End Set

        End Property


        Property Freeonboard() As Integer

            Get

                Return FOB

            End Get

            Set(value As Integer)

                FOB = value

            End Set

        End Property


        Property Creditlimittype() As Integer

            Get

                Return CRType

            End Get

            Set(value As Integer)

                CRType = value

            End Set

        End Property


        Property Creditlimitdollaramount() As Integer

            Get

                Return CrLmtAmt

            End Get

            Set(value As Integer)

                CrLmtAmt = value

            End Set

        End Property


        Property Cashaccount() As Integer

            Get

                Return CashAcct

            End Get

            Set(value As Integer)

                CashAcct = value

            End Set

        End Property


        Property Accountspayableaccount() As Integer

            Get

                Return AcctPayAcct

            End Get

            Set(value As Integer)

                AcctPayAcct = value

            End Set

        End Property


        Property Termsdiscountavailableaccount() As Integer

            Get

                Return TermDisAcct

            End Get

            Set(value As Integer)

                TermDisAcct = value

            End Set

        End Property


        Property Termsdiscounttakenaccount() As Integer

            Get

                Return TermDisTakenAcct

            End Get

            Set(value As Integer)

                TermDisTakenAcct = value

            End Set

        End Property


        Property Financechargesaccount() As Integer

            Get

                Return FCAcct

            End Get

            Set(value As Integer)

                FCAcct = value

            End Set

        End Property


        Property Purchasesaccount() As Integer

            Get

                Return POAcct

            End Get

            Set(value As Integer)

                POAcct = value

            End Set

        End Property


        Property Tradediscountaccount() As Integer

            Get

                Return TradeDisAcct

            End Get

            Set(value As Integer)

                TradeDisAcct = value

            End Set

        End Property


        Property Miscellaneousaccount() As Integer

            Get

                Return MiscAcct

            End Get

            Set(value As Integer)

                MiscAcct = value

            End Set

        End Property


        Property Freightaccount() As Integer

            Get

                Return FreightAcct

            End Get

            Set(value As Integer)

                FreightAcct = value

            End Set

        End Property


        Property Taxaccount() As Integer

            Get

                Return TaxAcct

            End Get

            Set(value As Integer)

                TaxAcct = value

            End Set

        End Property


        Property WriteOffAccount As Integer

            Get

                Return WOAcct

            End Get

            Set(value As Integer)

                WOAcct = value

            End Set

        End Property


        Property Accruedpurchasesaccount() As Integer

            Get

                Return AccrPOAcct

            End Get

            Set(value As Integer)

                AccrPOAcct = value

            End Set

        End Property


        Property Purchasepricevarianceaccount() As Integer

            Get

                Return PPVAcct

            End Get

            Set(value As Integer)

                PPVAcct = value

            End Set

        End Property





    End Class


End Class
