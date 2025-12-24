Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class ExportDocResult
        Inherits DTOBaseClass


#Region " Data Members"

        Private _FACILITY As String
        <DataMember()> _
        Public Property FACILITY As String
            Get
                Return Me._FACILITY
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._FACILITY, value) = False) Then
                    Me._FACILITY = value
                End If
            End Set
        End Property

        Private _REFERENCE As String
        <DataMember()> _
        Public Property REFERENCE As String
            Get
                Return Me._REFERENCE
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._REFERENCE, value) = False) Then
                    Me._REFERENCE = value
                End If
            End Set
        End Property

        Private _CUSTOMER As String
        <DataMember()> _
        Public Property CUSTOMER As String
            Get
                Return Me._CUSTOMER
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CUSTOMER, value) = False) Then
                    Me._CUSTOMER = value
                End If
            End Set
        End Property

        Private _CUSTNAME As String
        <DataMember()> _
        Public Property CUSTNAME As String
            Get
                Return Me._CUSTNAME
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CUSTNAME, value) = False) Then
                    Me._CUSTNAME = value
                End If
            End Set
        End Property

        Private _CUSTORDNUM As String
        <DataMember()> _
        Public Property CUSTORDNUM As String
            Get
                Return Me._CUSTORDNUM
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CUSTORDNUM, value) = False) Then
                    Me._CUSTORDNUM = value
                End If
            End Set
        End Property

        Private _CUSTADDRS1 As String
        <DataMember()> _
        Public Property CUSTADDRS1 As String
            Get
                Return Me._CUSTADDRS1
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CUSTADDRS1, value) = False) Then
                    Me._CUSTADDRS1 = value
                End If
            End Set
        End Property

        Private _CUSTADDRS2 As String
        <DataMember()> _
        Public Property CUSTADDRS2 As String
            Get
                Return Me._CUSTADDRS2
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CUSTADDRS2, value) = False) Then
                    Me._CUSTADDRS2 = value
                End If
            End Set
        End Property

        Private _CUSTADDRS3 As String
        <DataMember()> _
        Public Property CUSTADDRS3 As String
            Get
                Return Me._CUSTADDRS3
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CUSTADDRS3, value) = False) Then
                    Me._CUSTADDRS3 = value
                End If
            End Set
        End Property

        Private _CUSTADDRS4 As String
        <DataMember()> _
        Public Property CUSTADDRS4 As String
            Get
                Return Me._CUSTADDRS4
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CUSTADDRS4, value) = False) Then
                    Me._CUSTADDRS4 = value
                End If
            End Set
        End Property

        Private _CUPOSTCODE As String
        <DataMember()> _
        Public Property CUPOSTCODE As String
            Get
                Return Me._CUPOSTCODE
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CUPOSTCODE, value) = False) Then
                    Me._CUPOSTCODE = value
                End If
            End Set
        End Property

        Private _SHIPNAME As String
        <DataMember()> _
        Public Property SHIPNAME As String
            Get
                Return Me._SHIPNAME
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SHIPNAME, value) = False) Then
                    Me._SHIPNAME = value
                End If
            End Set
        End Property

        Private _SHIPADDRS1 As String
        <DataMember()> _
        Public Property SHIPADDRS1 As String
            Get
                Return Me._SHIPADDRS1
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SHIPADDRS1, value) = False) Then
                    Me._SHIPADDRS1 = value
                End If
            End Set
        End Property

        Private _SHIPADDRS2 As String
        <DataMember()> _
        Public Property SHIPADDRS2 As String
            Get
                Return Me._SHIPADDRS2
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SHIPADDRS2, value) = False) Then
                    Me._SHIPADDRS2 = value
                End If
            End Set
        End Property

        Private _SHIPADDRS3 As String
        <DataMember()> _
        Public Property SHIPADDRS3 As String
            Get
                Return Me._SHIPADDRS3
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SHIPADDRS3, value) = False) Then
                    Me._SHIPADDRS3 = value
                End If
            End Set
        End Property

        Private _SHIPADDRS4 As String
        <DataMember()> _
        Public Property SHIPADDRS4 As String
            Get
                Return Me._SHIPADDRS4
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SHIPADDRS4, value) = False) Then
                    Me._SHIPADDRS4 = value
                End If
            End Set
        End Property

        Private _SHPOSTCODE As String
        <DataMember()> _
        Public Property SHPOSTCODE As String
            Get
                Return Me._SHPOSTCODE
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SHPOSTCODE, value) = False) Then
                    Me._SHPOSTCODE = value
                End If
            End Set
        End Property

        Private _EXPORTER As String
        <DataMember()> _
        Public Property EXPORTER As String
            Get
                Return Me._EXPORTER
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._EXPORTER, value) = False) Then
                    Me._EXPORTER = value
                End If
            End Set
        End Property

        Private _EXPADDRS1 As String
        <DataMember()> _
        Public Property EXPADDRS1 As String
            Get
                Return Me._EXPADDRS1
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._EXPADDRS1, value) = False) Then
                    Me._EXPADDRS1 = value
                End If
            End Set
        End Property

        Private _EXPADDRS2 As String
        <DataMember()> _
        Public Property EXPADDRS2 As String
            Get
                Return Me._EXPADDRS2
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._EXPADDRS2, value) = False) Then
                    Me._EXPADDRS2 = value
                End If
            End Set
        End Property

        Private _EXPADDRS3 As String
        <DataMember()> _
        Public Property EXPADDRS3 As String
            Get
                Return Me._EXPADDRS3
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._EXPADDRS3, value) = False) Then
                    Me._EXPADDRS3 = value
                End If
            End Set
        End Property

        Private _EXPADDRS4 As String
        <DataMember()> _
        Public Property EXPADDRS4 As String
            Get
                Return Me._EXPADDRS4
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._EXPADDRS4, value) = False) Then
                    Me._EXPADDRS4 = value
                End If
            End Set
        End Property

        Private _ExpZip As String
        <DataMember()> _
        Public Property ExpZip As String
            Get
                Return Me._ExpZip
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ExpZip, value) = False) Then
                    Me._ExpZip = value
                End If
            End Set
        End Property

        Private _ExpTel As String
        <DataMember()> _
        Public Property ExpTel As String
            Get
                Return Me._ExpTel
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ExpTel, value) = False) Then
                    Me._ExpTel = value
                End If
            End Set
        End Property

        Private _ExpFax As String
        <DataMember()> _
        Public Property ExpFax As String
            Get
                Return Me._ExpFax
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ExpFax, value) = False) Then
                    Me._ExpFax = value
                End If
            End Set
        End Property

        Private _ExpContact As String
        <DataMember()> _
        Public Property ExpContact As String
            Get
                Return Me._ExpContact
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ExpContact, value) = False) Then
                    Me._ExpContact = value
                End If
            End Set
        End Property

        Private _CURR_CODE As String
        <DataMember()> _
        Public Property CURR_CODE As String
            Get
                Return Me._CURR_CODE
            End Get
            Set(ByVal value As String) 
                Me._CURR_CODE = value 
            End Set
        End Property

        Private _CURRRATE As Integer
        <DataMember()> _
        Public Property CURRRATE As Integer
            Get
                Return Me._CURRRATE
            End Get
            Set(ByVal value As Integer)
                If (Me._CURRRATE.Equals(value) = False) Then
                    Me._CURRRATE = value
                End If
            End Set
        End Property

        Private _ENTRYDATE As Date
        <DataMember()> _
        Public Property ENTRYDATE As Date
            Get
                Return _ENTRYDATE
            End Get
            Set(ByVal value As Date)
                _ENTRYDATE = value
            End Set
        End Property

        Private _INV_DATE As Date
        <DataMember()> _
        Public Property INV_DATE As Date
            Get
                Return _INV_DATE
            End Get
            Set(ByVal value As Date)
                _INV_DATE = value
            End Set
        End Property

        Private _PRIC_DEC As Integer
        <DataMember()> _
        Public Property PRIC_DEC As Integer
            Get
                Return Me._PRIC_DEC
            End Get
            Set(ByVal value As Integer)
                If (Me._PRIC_DEC.Equals(value) = False) Then
                    Me._PRIC_DEC = value
                End If
            End Set
        End Property

        Private _QNTY_DEC As Integer
        <DataMember()> _
        Public Property QNTY_DEC As Integer
            Get
                Return Me._QNTY_DEC
            End Get
            Set(ByVal value As Integer)
                If (Me._QNTY_DEC.Equals(value) = False) Then
                    Me._QNTY_DEC = value
                End If
            End Set
        End Property

        Private _WGT_DEC As Integer
        <DataMember()> _
        Public Property WGT_DEC As Integer
            Get
                Return Me._WGT_DEC
            End Get
            Set(ByVal value As Integer)
                If (Me._WGT_DEC.Equals(value) = False) Then
                    Me._WGT_DEC = value
                End If
            End Set
        End Property

        Private _NO_LINES As Integer
        <DataMember()> _
        Public Property NO_LINES As Integer
            Get
                Return Me._NO_LINES
            End Get
            Set(ByVal value As Integer)
                If (Me._NO_LINES.Equals(value) = False) Then
                    Me._NO_LINES = value
                End If
            End Set
        End Property

        Private _COMMENTS1 As String
        <DataMember()> _
        Public Property COMMENTS1 As String
            Get
                Return Me._COMMENTS1
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._WTUM, _COMMENTS1) = False) Then
                    Me._COMMENTS1 = value
                End If
            End Set
        End Property

        Private _COMMENTS2 As String
        <DataMember()> _
        Public Property COMMENTS2 As String
            Get
                Return Me._COMMENTS2
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._COMMENTS2, value) = False) Then
                    Me._COMMENTS2 = value
                End If
            End Set
        End Property

        Private _COM_TERMS As String
        <DataMember()> _
        Public Property COM_TERMS As String
            Get
                Return Me._COM_TERMS
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._COM_TERMS, value) = False) Then
                    Me._COM_TERMS = value
                End If
            End Set
        End Property

        Private _INCO_TERMS As String
        <DataMember()> _
        Public Property INCO_TERMS As String
            Get
                Return Me._INCO_TERMS
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._INCO_TERMS, value) = False) Then
                    Me._INCO_TERMS = value
                End If
            End Set
        End Property

        Private _FOB_POINT As String
        <DataMember()> _
        Public Property FOB_POINT As String
            Get
                Return Me._FOB_POINT
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._FOB_POINT, value) = False) Then
                    Me._FOB_POINT = value
                End If
            End Set
        End Property

        Private _MODE As String
        <DataMember()> _
        Public Property MODE As String
            Get
                Return Me._MODE
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MODE, value) = False) Then
                    Me._MODE = value
                End If
            End Set
        End Property

        Private _PORT_ENTRY As String
        <DataMember()> _
        Public Property PORT_ENTRY As String
            Get
                Return Me._PORT_ENTRY
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._PORT_ENTRY, value) = False) Then
                    Me._PORT_ENTRY = value
                End If
            End Set
        End Property

        Private _LOCAL_CAR As String
        <DataMember()> _
        Public Property LOCAL_CAR As String
            Get
                Return Me._LOCAL_CAR
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._LOCAL_CAR, value) = False) Then
                    Me._LOCAL_CAR = value
                End If
            End Set
        End Property

        Private _EXPORT_CAR As String
        <DataMember()> _
        Public Property EXPORT_CAR As String
            Get
                Return Me._EXPORT_CAR
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._EXPORT_CAR, value) = False) Then
                    Me._EXPORT_CAR = value
                End If
            End Set
        End Property

        Private _INV_NUMBER As String
        <DataMember()> _
        Public Property INV_NUMBER As String
            Get
                Return Me._INV_NUMBER
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._INV_NUMBER, value) = False) Then
                    Me._INV_NUMBER = value
                End If
            End Set
        End Property

        Private _PACKING As String
        <DataMember()> _
        Public Property PACKING As String
            Get
                Return Me._PACKING
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._PACKING, value) = False) Then
                    Me._PACKING = value
                End If
            End Set
        End Property

        Private _OCEAN As String
        <DataMember()> _
        Public Property OCEAN As String
            Get
                Return Me._OCEAN
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._OCEAN, value) = False) Then
                    Me._OCEAN = value
                End If
            End Set
        End Property

        Private _DOMESTIC As String
        <DataMember()> _
        Public Property DOMESTIC As String
            Get
                Return Me._DOMESTIC
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._DOMESTIC, value) = False) Then
                    Me._DOMESTIC = value
                End If
            End Set
        End Property

        Private _INSURANCE As String
        <DataMember()> _
        Public Property INSURANCE As String
            Get
                Return Me._INSURANCE
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._INSURANCE, value) = False) Then
                    Me._INSURANCE = value
                End If
            End Set
        End Property

        Private _COMMISSION As String
        <DataMember()> _
        Public Property COMMISSION As String
            Get
                Return Me._COMMISSION
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._COMMISSION, value) = False) Then
                    Me._COMMISSION = value
                End If
            End Set
        End Property

        Private _ASSIST As String
        <DataMember()> _
        Public Property ASSIST As String
            Get
                Return Me._ASSIST
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ASSIST, value) = False) Then
                    Me._ASSIST = value
                End If
            End Set
        End Property

        Private _CONTAINER As String
        <DataMember()> _
        Public Property CONTAINER As String
            Get
                Return Me._CONTAINER
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CONTAINER, value) = False) Then
                    Me._CONTAINER = value
                End If
            End Set
        End Property

        Private _MISC As String
        <DataMember()> _
        Public Property MISC As String
            Get
                Return Me._MISC
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MISC, value) = False) Then
                    Me._MISC = value
                End If
            End Set
        End Property

        Private _TAXTYPE1 As String
        <DataMember()> _
        Public Property TAXTYPE1 As String
            Get
                Return Me._TAXTYPE1
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._TAXTYPE1, value) = False) Then
                    Me._TAXTYPE1 = value
                End If
            End Set
        End Property

        Private _TAX1_CHRG As String
        <DataMember()> _
        Public Property TAX1_CHRG As String
            Get
                Return Me._TAX1_CHRG
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._TAX1_CHRG, value) = False) Then
                    Me._TAX1_CHRG = value
                End If
            End Set
        End Property

        Private _TAXTYPE2 As String
        <DataMember()> _
        Public Property TAXTYPE2 As String
            Get
                Return Me._TAXTYPE2
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._TAXTYPE2, value) = False) Then
                    Me._TAXTYPE2 = value
                End If
            End Set
        End Property

        Private _TAX2_CHRG As String
        <DataMember()> _
        Public Property TAX2_CHRG As String
            Get
                Return Me._TAX2_CHRG
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._TAX2_CHRG, value) = False) Then
                    Me._TAX2_CHRG = value
                End If
            End Set
        End Property

        Private _INVOICEAMT As String
        <DataMember()> _
        Public Property INVOICEAMT As String
            Get
                Return Me._INVOICEAMT
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._INVOICEAMT, value) = False) Then
                    Me._INVOICEAMT = value
                End If
            End Set
        End Property

        Private _SHIP As String
        <DataMember()> _
        Public Property SHIP As String
            Get
                Return Me._SHIP
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SHIP, value) = False) Then
                    Me._SHIP = value
                End If
            End Set
        End Property

        Private _SHIPPERSBN As String
        <DataMember()> _
        Public Property SHIPPERSBN As String
            Get
                Return Me._SHIPPERSBN
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SHIPPERSBN, value) = False) Then
                    Me._SHIPPERSBN = value
                End If
            End Set
        End Property

        Private _BUYERSBN As String
        <DataMember()> _
        Public Property BUYERSBN As String
            Get
                Return Me._BUYERSBN
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BUYERSBN, value) = False) Then
                    Me._BUYERSBN = value
                End If
            End Set
        End Property

        Private _ISONUMBER As String
        <DataMember()> _
        Public Property ISONUMBER As String
            Get
                Return Me._ISONUMBER
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ISONUMBER, value) = False) Then
                    Me._ISONUMBER = value
                End If
            End Set
        End Property

        Private _CUST_2 As String
        <DataMember()> _
        Public Property CUST_2 As String
            Get
                Return Me._CUST_2
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CUST_2, value) = False) Then
                    Me._CUST_2 = value
                End If
            End Set
        End Property

        Private _ITEM As String
        <DataMember()> _
        Public Property ITEM As String
            Get
                Return Me._ITEM
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ITEM, value) = False) Then
                    Me._ITEM = value
                End If
            End Set
        End Property

        Private _ITEMREV As String
        <DataMember()> _
        Public Property ITEMREV As String
            Get
                Return Me._ITEMREV
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ITEMREV, value) = False) Then
                    Me._ITEMREV = value
                End If
            End Set
        End Property

        Private _LINE_NO As Integer
        <DataMember()> _
        Public Property LINE_NO As Integer
            Get
                Return Me._LINE_NO
            End Get
            Set(ByVal value As Integer)
                If (Me._LINE_NO.Equals(value) = False) Then
                    Me._LINE_NO = value
                End If
            End Set
        End Property

        Private _CUSTORDLN As Integer
        <DataMember()> _
        Public Property CUSTORDLN As Integer
            Get
                Return Me._CUSTORDLN
            End Get
            Set(ByVal value As Integer)
                If (Me._CUSTORDLN.Equals(value) = False) Then
                    Me._CUSTORDLN = value
                End If
            End Set
        End Property

        Private _DESC As String
        <DataMember()> _
        Public Property DESC As String
            Get
                Return Me._DESC
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._DESC, value) = False) Then
                    Me._DESC = value
                End If
            End Set
        End Property

        Private _SHPUM As String
        <DataMember()> _
        Public Property SHPUM As String
            Get
                Return Me._SHPUM
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SHPUM, value) = False) Then
                    Me._SHPUM = value
                End If
            End Set
        End Property

        Private _DETAILS1 As String
        <DataMember()> _
        Public Property DETAILS1 As String
            Get
                Return Me._DETAILS1
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._DETAILS1, value) = False) Then
                    Me._DETAILS1 = value
                End If
            End Set
        End Property

        Private _DETAILS2 As String
        <DataMember()> _
        Public Property DETAILS2 As String
            Get
                Return Me._DETAILS2
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._DETAILS2, value) = False) Then
                    Me._DETAILS2 = value
                End If
            End Set
        End Property

        Private _QNTYSHP As Integer
        <DataMember()> _
        Public Property QNTYSHP As Integer
            Get
                Return Me._QNTYSHP
            End Get
            Set(ByVal value As Integer)
                If (Me._QNTYSHP.Equals(value) = False) Then
                    Me._QNTYSHP = value
                End If
            End Set
        End Property

        Private _WEIGHT As Double
        <DataMember()> _
        Public Property WEIGHT As Double
            Get
                Return Me._WEIGHT
            End Get
            Set(ByVal value As Double)
                If (String.Equals(Me._WEIGHT, value) = False) Then
                    Me._WEIGHT = value
                End If
            End Set
        End Property

        Private _WTUM As String
        <DataMember()> _
        Public Property WTUM As String
            Get
                Return Me._WTUM
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._WTUM, value) = False) Then
                    Me._WTUM = value
                End If
            End Set
        End Property

        Private _CUBE As Integer
        <DataMember()> _
        Public Property CUBE As Integer
            Get
                Return Me._CUBE
            End Get
            Set(ByVal value As Integer)
                If (Me._CUBE.Equals(value) = False) Then
                    Me._CUBE = value
                End If
            End Set
        End Property
         
        Private _CUBEUM As String
        <DataMember()> _
        Public Property CUBEUM As String
            Get
                Return Me._CUBEUM
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CUBEUM, value) = False) Then
                    Me._CUBEUM = value
                End If
            End Set
        End Property

        Private _QUANTITY As Short
        <DataMember()> _
        Public Property QUANTITY As Short
            Get
                Return Me._QUANTITY
            End Get
            Set(ByVal value As Short)
                If (Me._QUANTITY.Equals(value) = False) Then
                    Me._QUANTITY = value
                End If
            End Set
        End Property

        Private _INVOICE As Decimal
        <DataMember()> _
        Public Property INVOICE As Decimal
            Get
                Return Me._INVOICE
            End Get
            Set(ByVal value As Decimal)
                If (Me._INVOICE.Equals(value) = False) Then
                    Me._INVOICE = value
                End If
            End Set
        End Property
         
        Private _HSCODE As String
        <DataMember()> _
        Public Property HSCODE As String
            Get
                Return Me._HSCODE
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._HSCODE, value) = False) Then
                    Me._HSCODE = value
                End If
            End Set
        End Property

        Private _ITEMCOST As Decimal
        <DataMember()> _
        Public Property ITEMCOST As Decimal
            Get
                Return Me._ITEMCOST
            End Get
            Set(ByVal value As Decimal)
                If (Me._ITEMCOST.Equals(value) = False) Then
                    Me._ITEMCOST = value
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New ExportDocResult
            instance = DirectCast(MemberwiseClone(), ExportDocResult)
            Return instance
        End Function

#End Region

    End Class
 
End Namespace