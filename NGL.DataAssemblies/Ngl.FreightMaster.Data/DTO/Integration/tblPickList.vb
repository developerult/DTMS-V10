Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblPickList
        Inherits DTOBaseClass

#Region " Data Members"
        Private _PLControl As Long
        <DataMember()> _
        Public Property PLControl() As Long
            Get
                Return Me._PLControl
            End Get
            Set(ByVal value As Long)
                If ((Me._PLControl = value) _
                   = False) Then
                    Me._PLControl = value
                    Me.SendPropertyChanged("PLControl")
                End If
            End Set
        End Property

        Private _PLExportDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property PLExportDate() As System.Nullable(Of Date)
            Get
                Return Me._PLExportDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._PLExportDate.Equals(value) = False) Then
                    Me._PLExportDate = value
                    Me.SendPropertyChanged("PLExportDate")
                End If
            End Set
        End Property

        Private _PLExportRetry As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property PLExportRetry() As System.Nullable(Of Integer)
            Get
                Return Me._PLExportRetry
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._PLExportRetry.Equals(value) = False) Then
                    Me._PLExportRetry = value
                    Me.SendPropertyChanged("PLExportRetry")
                End If
            End Set
        End Property

        Private _PLExported As System.Nullable(Of Boolean)
        <DataMember()> _
        Public Property PLExported() As System.Nullable(Of Boolean)
            Get
                Return Me._PLExported
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                If (Me._PLExported.Equals(value) = False) Then
                    Me._PLExported = value
                    Me.SendPropertyChanged("PLExported")
                End If
            End Set
        End Property

        Private _BookCarrOrderNumber As String
        <DataMember()> _
        Public Property BookCarrOrderNumber() As String
            Get
                Return Left(Me._BookCarrOrderNumber, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookCarrOrderNumber, value) = False) Then
                    Me._BookCarrOrderNumber = Left(value, 20)
                    Me.SendPropertyChanged("BookCarrOrderNumber")
                End If
            End Set
        End Property

        Private _BookConsPrefix As String
        <DataMember()> _
        Public Property BookConsPrefix() As String
            Get
                Return Left(Me._BookConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookConsPrefix, value) = False) Then
                    Me._BookConsPrefix = Left(value, 20)
                    Me.SendPropertyChanged("BookConsPrefix")
                End If
            End Set
        End Property

        Private _CarrierNumber As String
        <DataMember()> _
        Public Property CarrierNumber() As String
            Get
                Return Left(Me._CarrierNumber, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrierNumber, value) = False) Then
                    Me._CarrierNumber = Left(value, 50)
                    Me.SendPropertyChanged("CarrierNumber")
                End If
            End Set
        End Property

        Private _BookRevTotalCost As String
        <DataMember()> _
        Public Property BookRevTotalCost() As String
            Get
                Return Left(Me._BookRevTotalCost, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookRevTotalCost, value) = False) Then
                    Me._BookRevTotalCost = Left(value, 20)
                    Me.SendPropertyChanged("BookRevTotalCost")
                End If
            End Set
        End Property

        Private _LoadOrder As String
        <DataMember()> _
        Public Property LoadOrder() As String
            Get
                Return Left(Me._LoadOrder, 6)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._LoadOrder, value) = False) Then
                    Me._LoadOrder = Left(value, 6)
                    Me.SendPropertyChanged("LoadOrder")
                End If
            End Set
        End Property

        Private _BookDateLoad As String
        <DataMember()> _
        Public Property BookDateLoad() As String
            Get
                Return Left(Me._BookDateLoad, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDateLoad, value) = False) Then
                    Me._BookDateLoad = Left(value, 20)
                    Me.SendPropertyChanged("BookDateLoad")
                End If
            End Set
        End Property

        Private _BookDateRequired As String
        <DataMember()> _
        Public Property BookDateRequired() As String
            Get
                Return Left(Me._BookDateRequired, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDateRequired, value) = False) Then
                    Me._BookDateRequired = Left(value, 20)
                    Me.SendPropertyChanged("BookDateRequired")
                End If
            End Set
        End Property

        Private _BookLoadCom As String
        <DataMember()> _
        Public Property BookLoadCom() As String
            Get
                Return Left(Me._BookLoadCom, 1)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookLoadCom, value) = False) Then
                    Me._BookLoadCom = Left(value, 1)
                    Me.SendPropertyChanged("BookLoadCom")
                End If
            End Set
        End Property

        Private _BookProNumber As String
        <DataMember()> _
        Public Property BookProNumber() As String
            Get
                Return Left(Me._BookProNumber, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookProNumber, value) = False) Then
                    Me._BookProNumber = Left(value, 20)
                    Me.SendPropertyChanged("BookProNumber")
                End If
            End Set
        End Property

        Private _BookRouteFinalCode As String
        <DataMember()> _
        Public Property BookRouteFinalCode() As String
            Get
                Return Left(Me._BookRouteFinalCode, 2)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookRouteFinalCode, value) = False) Then
                    Me._BookRouteFinalCode = Left(value, 2)
                    Me.SendPropertyChanged("BookRouteFinalCode")
                End If
            End Set
        End Property

        Private _BookRouteFinalDate As String
        <DataMember()> _
        Public Property BookRouteFinalDate() As String
            Get
                Return Left(Me._BookRouteFinalDate, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookRouteFinalDate, value) = False) Then
                    Me._BookRouteFinalDate = Left(value, 20)
                    Me.SendPropertyChanged("BookRouteFinalDate")
                End If
            End Set
        End Property

        Private _BookTotalCases As String
        <DataMember()> _
        Public Property BookTotalCases() As String
            Get
                Return Left(Me._BookTotalCases, 6)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookTotalCases, value) = False) Then
                    Me._BookTotalCases = Left(value, 6)
                    Me.SendPropertyChanged("BookTotalCases")
                End If
            End Set
        End Property

        Private _BookTotalWgt As String
        <DataMember()> _
        Public Property BookTotalWgt() As String
            Get
                Return Left(Me._BookTotalWgt, 22)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookTotalWgt, value) = False) Then
                    Me._BookTotalWgt = Left(value, 22)
                    Me.SendPropertyChanged("BookTotalWgt")
                End If
            End Set
        End Property

        Private _BookTotalPL As String
        <DataMember()> _
        Public Property BookTotalPL() As String
            Get
                Return Left(Me._BookTotalPL, 22)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookTotalPL, value) = False) Then
                    Me._BookTotalPL = Left(value, 22)
                    Me.SendPropertyChanged("BookTotalPL")
                End If
            End Set
        End Property

        Private _BookTotalCube As String
        <DataMember()> _
        Public Property BookTotalCube() As String
            Get
                Return Left(Me._BookTotalCube, 6)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookTotalCube, value) = False) Then
                    Me._BookTotalCube = Left(value, 6)
                    Me.SendPropertyChanged("BookTotalCube")
                End If
            End Set
        End Property

        Private _BookTotalBFC As String
        <DataMember()> _
        Public Property BookTotalBFC() As String
            Get
                Return Left(Me._BookTotalBFC, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookTotalBFC, value) = False) Then
                    Me._BookTotalBFC = Left(value, 20)
                    Me.SendPropertyChanged("BookTotalBFC")
                End If
            End Set
        End Property

        Private _BookStopNo As String
        <DataMember()> _
        Public Property BookStopNo() As String
            Get
                Return left(Me._BookStopNo, 6)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookStopNo, value) = False) Then
                    Me._BookStopNo = Left(value, 6)
                    Me.SendPropertyChanged("BookStopNo")
                End If
            End Set
        End Property

        Private _CompName As String
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return Left(Me._CompName, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CompName, value) = False) Then
                    Me._CompName = Left(value, 40)
                    Me.SendPropertyChanged("CompName")
                End If
            End Set
        End Property

        Private _CompNumber As String
        <DataMember()> _
        Public Property CompNumber() As String
            Get
                Return Left(Me._CompNumber, 11)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CompNumber, value) = False) Then
                    Me._CompNumber = Left(value, 11)
                    Me.SendPropertyChanged("CompNumber")
                End If
            End Set
        End Property

        Private _BookTypeCode As String
        <DataMember()> _
        Public Property BookTypeCode() As String
            Get
                Return Left(Me._BookTypeCode, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookTypeCode, value) = False) Then
                    Me._BookTypeCode = Left(value, 20)
                    Me.SendPropertyChanged("BookTypeCode")
                End If
            End Set
        End Property

        Private _BookDateOrdered As String
        <DataMember()> _
        Public Property BookDateOrdered() As String
            Get
                Return Left(Me._BookDateOrdered, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDateOrdered, value) = False) Then
                    Me._BookDateOrdered = Left(value, 20)
                    Me.SendPropertyChanged("BookDateOrdered")
                End If
            End Set
        End Property

        Private _BookOrigName As String
        <DataMember()> _
        Public Property BookOrigName() As String
            Get
                Return Left(Me._BookOrigName, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigName, value) = False) Then
                    Me._BookOrigName = Left(value, 40)
                    Me.SendPropertyChanged("BookOrigName")
                End If
            End Set
        End Property

        Private _BookOrigAddress1 As String
        <DataMember()> _
        Public Property BookOrigAddress1() As String
            Get
                Return Left(Me._BookOrigAddress1, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigAddress1, value) = False) Then
                    Me._BookOrigAddress1 = Left(value, 40)
                    Me.SendPropertyChanged("BookOrigAddress1")
                End If
            End Set
        End Property

        Private _BookOrigAddress2 As String
        <DataMember()> _
        Public Property BookOrigAddress2() As String
            Get
                Return Left(Me._BookOrigAddress2, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigAddress2, value) = False) Then
                    Me._BookOrigAddress2 = Left(value, 40)
                    Me.SendPropertyChanged("BookOrigAddress2")
                End If
            End Set
        End Property

        Private _BookOrigAddress3 As String
        <DataMember()> _
        Public Property BookOrigAddress3() As String
            Get
                Return Left(Me._BookOrigAddress3, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigAddress3, value) = False) Then
                    Me._BookOrigAddress3 = Left(value, 40)
                    Me.SendPropertyChanged("BookOrigAddress3")
                End If
            End Set
        End Property

        Private _BookOrigCity As String
        <DataMember()> _
        Public Property BookOrigCity() As String
            Get
                Return Left(Me._BookOrigCity, 25)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigCity, value) = False) Then
                    Me._BookOrigCity = Left(value, 25)
                    Me.SendPropertyChanged("BookOrigCity")
                End If
            End Set
        End Property

        Private _BookOrigState As String
        <DataMember()> _
        Public Property BookOrigState() As String
            Get
                Return Left(Me._BookOrigState, 8)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigState, value) = False) Then
                    Me._BookOrigState = Left(value, 8)
                    Me.SendPropertyChanged("BookOrigState")
                End If
            End Set
        End Property

        Private _BookOrigCountry As String
        <DataMember()> _
        Public Property BookOrigCountry() As String
            Get
                Return Left(Me._BookOrigCountry, 30)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigCountry, value) = False) Then
                    Me._BookOrigCountry = Left(value, 30)
                    Me.SendPropertyChanged("BookOrigCountry")
                End If
            End Set
        End Property

        Private _BookOrigZip As String
        <DataMember()> _
        Public Property BookOrigZip() As String
            Get
                Return Left(Me._BookOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigZip, value) = False) Then
                    Me._BookOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                    Me.SendPropertyChanged("BookOrigZip")
                End If
            End Set
        End Property

        Private _BookDestName As String
        <DataMember()> _
        Public Property BookDestName() As String
            Get
                Return Left(Me._BookDestName, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestName, value) = False) Then
                    Me._BookDestName = Left(value, 40)
                    Me.SendPropertyChanged("BookDestName")
                End If
            End Set
        End Property

        Private _BookDestAddress1 As String
        <DataMember()> _
        Public Property BookDestAddress1() As String
            Get
                Return Left(Me._BookDestAddress1, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestAddress1, value) = False) Then
                    Me._BookDestAddress1 = Left(value, 40)
                    Me.SendPropertyChanged("BookDestAddress1")
                End If
            End Set
        End Property

        Private _BookDestAddress2 As String
        <DataMember()> _
        Public Property BookDestAddress2() As String
            Get
                Return Left(Me._BookDestAddress2, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestAddress2, value) = False) Then
                    Me._BookDestAddress2 = Left(value, 40)
                    Me.SendPropertyChanged("BookDestAddress2")
                End If
            End Set
        End Property

        Private _BookDestAddress3 As String
        <DataMember()> _
        Public Property BookDestAddress3() As String
            Get
                Return Left(Me._BookDestAddress3, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestAddress3, value) = False) Then
                    Me._BookDestAddress3 = Left(value, 40)
                    Me.SendPropertyChanged("BookDestAddress3")
                End If
            End Set
        End Property

        Private _BookDestCity As String
        <DataMember()> _
        Public Property BookDestCity() As String
            Get
                Return Left(Me._BookDestCity, 25)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestCity, value) = False) Then
                    Me._BookDestCity = Left(value, 25)
                    Me.SendPropertyChanged("BookDestCity")
                End If
            End Set
        End Property

        Private _BookDestState As String
        <DataMember()> _
        Public Property BookDestState() As String
            Get
                Return Left(Me._BookDestState, 2)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestState, value) = False) Then
                    Me._BookDestState = Left(value, 2)
                    Me.SendPropertyChanged("BookDestState")
                End If
            End Set
        End Property


        Private _BookDestCountry As String
        <DataMember()> _
        Public Property BookDestCountry() As String
            Get
                Return Left(Me._BookDestCountry, 30)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestCountry, value) = False) Then
                    Me._BookDestCountry = Left(value, 30)
                    Me.SendPropertyChanged("BookDestCountry")
                End If
            End Set
        End Property

        Private _BookDestZip As String
        <DataMember()> _
        Public Property BookDestZip() As String
            Get
                Return Left(Me._BookDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestZip, value) = False) Then
                    Me._BookDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                    Me.SendPropertyChanged("BookDestZip")
                End If
            End Set
        End Property

        Private _BookLoadPONumber As String
        <DataMember()> _
        Public Property BookLoadPONumber() As String
            Get
                Return Left(Me._BookLoadPONumber, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookLoadPONumber, value) = False) Then
                    Me._BookLoadPONumber = Left(value, 20)
                    Me.SendPropertyChanged("BookLoadPONumber")
                End If
            End Set
        End Property

        Private _CarrierName As String
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Left(Me._CarrierName, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrierName, value) = False) Then
                    Me._CarrierName = Left(value, 40)
                    Me.SendPropertyChanged("CarrierName")
                End If
            End Set
        End Property

        Private _LaneNumber As String
        <DataMember()> _
        Public Property LaneNumber() As String
            Get
                Return Left(Me._LaneNumber, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._LaneNumber, value) = False) Then
                    Me._LaneNumber = Left(value, 50)
                    Me.SendPropertyChanged("LaneNumber")
                End If
            End Set
        End Property

        Private _CommCodeDescription As String
        <DataMember()> _
        Public Property CommCodeDescription() As String
            Get
                Return Left(Me._CommCodeDescription, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CommCodeDescription, value) = False) Then
                    Me._CommCodeDescription = Left(value, 40)
                    Me.SendPropertyChanged("CommCodeDescription")
                End If
            End Set
        End Property

        Private _BookMilesFrom As String
        <DataMember()> _
        Public Property BookMilesFrom() As String
            Get
                Return Left(Me._BookMilesFrom, 22)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookMilesFrom, value) = False) Then
                    Me._BookMilesFrom = Left(value, 22)
                    Me.SendPropertyChanged("BookMilesFrom")
                End If
            End Set
        End Property

        Private _BookCommCompControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property BookCommCompControl() As System.Nullable(Of Integer)
            Get
                Return Me._BookCommCompControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._BookCommCompControl.Equals(value) = False) Then
                    Me._BookCommCompControl = value
                    Me.SendPropertyChanged("BookCommCompControl")
                End If
            End Set
        End Property

        Private _BookRevCommCost As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BookRevCommCost() As System.Nullable(Of Decimal)
            Get
                Return Me._BookRevCommCost
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BookRevCommCost.Equals(value) = False) Then
                    Me._BookRevCommCost = value
                    Me.SendPropertyChanged("BookRevCommCost")
                End If
            End Set
        End Property

        Private _BookRevGrossRevenue As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BookRevGrossRevenue() As System.Nullable(Of Decimal)
            Get
                Return Me._BookRevGrossRevenue
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BookRevGrossRevenue.Equals(value) = False) Then
                    Me._BookRevGrossRevenue = value
                    Me.SendPropertyChanged("BookRevGrossRevenue")
                End If
            End Set
        End Property

        Private _BookFinCommStd As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BookFinCommStd() As System.Nullable(Of Decimal)
            Get
                Return Me._BookFinCommStd
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BookFinCommStd.Equals(value) = False) Then
                    Me._BookFinCommStd = value
                    Me.SendPropertyChanged("BookFinCommStd")
                End If
            End Set
        End Property

        Private _BookDoNotInvoice As Boolean
        <DataMember()> _
        Public Property BookDoNotInvoice() As Boolean
            Get
                Return Me._BookDoNotInvoice
            End Get
            Set(ByVal value As Boolean)
                If ((Me._BookDoNotInvoice = value) _
                   = False) Then
                    Me._BookDoNotInvoice = value
                    Me.SendPropertyChanged("BookDoNotInvoice")
                End If
            End Set
        End Property

        Private _BookOrderSequence As Integer
        <DataMember()> _
        Public Property BookOrderSequence() As Integer
            Get
                Return Me._BookOrderSequence
            End Get
            Set(ByVal value As Integer)
                If ((Me._BookOrderSequence = value) _
                   = False) Then
                    Me._BookOrderSequence = value
                    Me.SendPropertyChanged("BookOrderSequence")
                End If
            End Set
        End Property

        Private _CarrierEquipmentCodes As String
        <DataMember()> _
        Public Property CarrierEquipmentCodes() As String
            Get
                Return Left(Me._CarrierEquipmentCodes, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrierEquipmentCodes, value) = False) Then
                    Me._CarrierEquipmentCodes = Left(value, 50)
                    Me.SendPropertyChanged("CarrierEquipmentCodes")
                End If
            End Set
        End Property

        Private _BookCarrierTypeCode As String
        <DataMember()> _
        Public Property BookCarrierTypeCode() As String
            Get
                Return Left(Me._BookCarrierTypeCode, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookCarrierTypeCode, value) = False) Then
                    Me._BookCarrierTypeCode = Left(value, 20)
                    Me.SendPropertyChanged("BookCarrierTypeCode")
                End If
            End Set
        End Property

        Private _BookWarehouseNumber As String
        <DataMember()> _
        Public Property BookWarehouseNumber() As String
            Get
                Return Left(Me._BookWarehouseNumber, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookWarehouseNumber, value) = False) Then
                    Me._BookWarehouseNumber = Left(value, 20)
                    Me.SendPropertyChanged("BookWarehouseNumber")
                End If
            End Set
        End Property

        Private _BookShipCarrierProNumber As String
        <DataMember()> _
        Public Property BookShipCarrierProNumber() As String
            Get
                Return Left(Me._BookShipCarrierProNumber, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookShipCarrierProNumber, value) = False) Then
                    Me._BookShipCarrierProNumber = Left(value, 20)
                    Me.SendPropertyChanged("BookShipCarrierProNumber")
                End If
            End Set
        End Property

        Private _CompNatNumber As Integer
        <DataMember()> _
        Public Property CompNatNumber() As Integer
            Get
                Return Me._CompNatNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._CompNatNumber = value) _
                   = False) Then
                    Me._CompNatNumber = value
                    Me.SendPropertyChanged("CompNatNumber")
                End If
            End Set
        End Property

        Private _BookTransType As String
        <DataMember()> _
        Public Property BookTransType() As String
            Get
                Return Left(Me._BookTransType, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookTransType, value) = False) Then
                    Me._BookTransType = Left(value, 50)
                    Me.SendPropertyChanged("BookTransType")
                End If
            End Set
        End Property

        Private _BookShipCarrierNumber As String
        <DataMember()> _
        Public Property BookShipCarrierNumber() As String
            Get
                Return Left(Me._BookShipCarrierNumber, 80)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookShipCarrierNumber, value) = False) Then
                    Me._BookShipCarrierNumber = Left(value, 80)
                    Me.SendPropertyChanged("BookShipCarrierNumber")
                End If
            End Set
        End Property

        Private _LaneComments As String
        <DataMember()> _
        Public Property LaneComments() As String
            Get
                Return Left(Me._LaneComments, 255)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._LaneComments, value) = False) Then
                    Me._LaneComments = Left(value, 255)
                    Me.SendPropertyChanged("LaneComments")
                End If
            End Set
        End Property

        Private _FuelSurCharge As Decimal
        <DataMember()> _
        Public Property FuelSurCharge() As Decimal
            Get
                Return Me._FuelSurCharge
            End Get
            Set(ByVal value As Decimal)
                If ((Me._FuelSurCharge = value) _
                   = False) Then
                    Me._FuelSurCharge = value
                    Me.SendPropertyChanged("FuelSurCharge")
                End If
            End Set
        End Property

        Private _BookRevCarrierCost As Decimal
        <DataMember()> _
        Public Property BookRevCarrierCost() As Decimal
            Get
                Return Me._BookRevCarrierCost
            End Get
            Set(ByVal value As Decimal)
                If ((Me._BookRevCarrierCost = value) _
                   = False) Then
                    Me._BookRevCarrierCost = value
                    Me.SendPropertyChanged("BookRevCarrierCost")
                End If
            End Set
        End Property

        Private _BookRevOtherCost As Decimal
        <DataMember()> _
        Public Property BookRevOtherCost() As Decimal
            Get
                Return Me._BookRevOtherCost
            End Get
            Set(ByVal value As Decimal)
                If ((Me._BookRevOtherCost = value) _
                   = False) Then
                    Me._BookRevOtherCost = value
                    Me.SendPropertyChanged("BookRevOtherCost")
                End If
            End Set
        End Property

        Private _BookRevNetCost As Decimal
        <DataMember()> _
        Public Property BookRevNetCost() As Decimal
            Get
                Return Me._BookRevNetCost
            End Get
            Set(ByVal value As Decimal)
                If ((Me._BookRevNetCost = value) _
                   = False) Then
                    Me._BookRevNetCost = value
                    Me.SendPropertyChanged("BookRevNetCost")
                End If
            End Set
        End Property

        Private _BookRevFreightTax As Decimal
        <DataMember()> _
        Public Property BookRevFreightTax() As Decimal
            Get
                Return Me._BookRevFreightTax
            End Get
            Set(ByVal value As Decimal)
                If ((Me._BookRevFreightTax = value) _
                   = False) Then
                    Me._BookRevFreightTax = value
                    Me.SendPropertyChanged("BookRevFreightTax")
                End If
            End Set
        End Property

        Private _BookFinServiceFee As Decimal
        <DataMember()> _
        Public Property BookFinServiceFee() As Decimal
            Get
                Return Me._BookFinServiceFee
            End Get
            Set(ByVal value As Decimal)
                If ((Me._BookFinServiceFee = value) _
                   = False) Then
                    Me._BookFinServiceFee = value
                    Me.SendPropertyChanged("BookFinServiceFee")
                End If
            End Set
        End Property

        Private _BookRevLoadSavings As Decimal
        <DataMember()> _
        Public Property BookRevLoadSavings() As Decimal
            Get
                Return Me._BookRevLoadSavings
            End Get
            Set(ByVal value As Decimal)
                If ((Me._BookRevLoadSavings = value) _
                   = False) Then
                    Me._BookRevLoadSavings = value
                    Me.SendPropertyChanged("BookRevLoadSavings")
                End If
            End Set
        End Property

        Private _TotalNonFuelFees As Decimal
        <DataMember()> _
        Public Property TotalNonFuelFees() As Decimal
            Get
                Return Me._TotalNonFuelFees
            End Get
            Set(ByVal value As Decimal)
                If ((Me._TotalNonFuelFees = value) _
                   = False) Then
                    Me._TotalNonFuelFees = value
                    Me.SendPropertyChanged("TotalNonFuelFees")
                End If
            End Set
        End Property

        Private _BookPickNumber As Integer
        <DataMember()> _
        Public Property BookPickNumber() As Integer
            Get
                Return Me._BookPickNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._BookPickNumber = value) _
                   = False) Then
                    Me._BookPickNumber = value
                    Me.SendPropertyChanged("BookPickNumber")
                End If
            End Set
        End Property

        Private _BookPickupStopNumber As Integer
        <DataMember()> _
        Public Property BookPickupStopNumber() As Integer
            Get
                Return Me._BookPickupStopNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._BookPickupStopNumber = value) _
                   = False) Then
                    Me._BookPickupStopNumber = value
                    Me.SendPropertyChanged("BookPickupStopNumber")
                End If
            End Set
        End Property


        Private _BookRouteConsFlag As Boolean
        <DataMember()> _
        Public Property BookRouteConsFlag() As Boolean
            Get
                Return Me._BookRouteConsFlag
            End Get
            Set(ByVal value As Boolean)
                If ((Me._BookRouteConsFlag = value) = False) Then
                    Me._BookRouteConsFlag = value
                    Me.SendPropertyChanged("BookRouteConsFlag")
                End If
            End Set
        End Property

        Private _BookAlternateAddressLaneNumber As String
        <DataMember()> _
        Public Property BookAlternateAddressLaneNumber() As String
            Get
                Return Left(Me._BookAlternateAddressLaneNumber, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookAlternateAddressLaneNumber, value) = False) Then
                    Me._BookAlternateAddressLaneNumber = Left(value, 50)
                    Me.SendPropertyChanged("BookAlternateAddressLaneNumber")
                End If
            End Set
        End Property

        Private _BookSHID As String
        <DataMember()> _
        Public Property BookSHID() As String
            Get
                Return Left(_BookSHID, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookSHID, value) = False) Then
                    Me._BookSHID = Left(value, 50)
                    Me.SendPropertyChanged("BookSHID")
                End If
            End Set
        End Property

        Private _BookShipCarrierDetails As String
        <DataMember()> _
        Public Property BookShipCarrierDetails() As String
            Get
                Return Left(_BookShipCarrierDetails, 4000)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookShipCarrierDetails, value) = False) Then
                    Me._BookShipCarrierDetails = Left(value, 4000)
                    Me.SendPropertyChanged("BookShipCarrierDetails")
                End If
            End Set
        End Property

        Private _BookShipCarrierName As String
        <DataMember()> _
        Public Property BookShipCarrierName() As String
            Get
                Return Left(_BookShipCarrierName, 60)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookShipCarrierName, value) = False) Then
                    Me._BookShipCarrierName = Left(value, 60)
                    Me.SendPropertyChanged("BookShipCarrierName")
                End If
            End Set
        End Property

        Private _BookRevNonTaxable As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BookRevNonTaxable() As System.Nullable(Of Decimal)
            Get
                Return _BookRevNonTaxable
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BookRevNonTaxable.Equals(value) = False) Then
                    Me._BookRevNonTaxable = value
                    Me.SendPropertyChanged("BookRevNonTaxable")
                End If
            End Set
        End Property

        Private _BookWhseAuthorizationNo As String
        <DataMember()> _
        Public Property BookWhseAuthorizationNo() As String
            Get
                Return Left(_BookWhseAuthorizationNo, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookWhseAuthorizationNo, value) = False) Then
                    Me._BookWhseAuthorizationNo = Left(value, 20)
                    Me.SendPropertyChanged("BookWhseAuthorizationNo")
                End If
            End Set
        End Property

        Private _BookControl As Integer?
        <DataMember()> _
        Public Property BookControl() As Integer?
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer?)
                If (Me._BookControl.Equals(value) = False) Then
                    Me._BookControl = value
                    Me.SendPropertyChanged("BookControl")
                End If
            End Set
        End Property
        

        Private _PLUpdated As Byte()
        <DataMember()>
        Public Property PLUpdated() As Byte()
            Get
                Return _PLUpdated
            End Get
            Set(ByVal value As Byte())
                _PLUpdated = value
            End Set
        End Property

        'Modified by RHR for v-8.5.0.002 on 12/03/2021 added Scheduler Fields
        Private _BookCarrTrailerNo As String
        Public Property BookCarrTrailerNo() As String
            Get
                Return Left(_BookCarrTrailerNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrTrailerNo = Left(value, 50)
            End Set
        End Property

        Private _BookCarrSealNo As String
        Public Property BookCarrSealNo() As String
            Get
                Return Left(_BookCarrSealNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrSealNo = Left(value, 50)
            End Set
        End Property

        Private _BookCarrDriverNo As String
        Public Property BookCarrDriverNo() As String
            Get
                Return Left(_BookCarrDriverNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrDriverNo = Left(value, 50)
            End Set
        End Property

        Private _BookCarrDriverName As String
        Public Property BookCarrDriverName() As String
            Get
                Return Left(_BookCarrDriverName, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrDriverName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrRouteNo As String
        Public Property BookCarrRouteNo() As String
            Get
                Return Left(_BookCarrRouteNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrRouteNo = Left(value, 50)
            End Set
        End Property


        Private _BookCarrTripNo As String
        Public Property BookCarrTripNo() As String
            Get
                Return Left(_BookCarrTripNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrTripNo = Left(value, 50)
            End Set
        End Property

        Private _BookCarrApptDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrApptDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrApptDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrApptDate = value
            End Set
        End Property

        Private _BookCarrApptTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrApptTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrApptTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrApptTime = value
            End Set
        End Property

        Private _BookCarrActDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrActDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActDate = value
            End Set
        End Property

        Private _BookCarrActTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActTime = value
            End Set
        End Property

        Private _BookCarrStartUnloadingDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrStartUnloadingDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrStartUnloadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrStartUnloadingDate = value
            End Set
        End Property

        Private _BookCarrStartUnloadingTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrStartUnloadingTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrStartUnloadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrStartUnloadingTime = value
            End Set
        End Property

        Private _BookCarrFinishUnloadingDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrFinishUnloadingDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrFinishUnloadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrFinishUnloadingDate = value
            End Set
        End Property

        Private _BookCarrFinishUnloadingTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrFinishUnloadingTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrFinishUnloadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrFinishUnloadingTime = value
            End Set
        End Property


        Private _BookCarrActUnloadCompDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActUnloadCompDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrActUnloadCompDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActUnloadCompDate = value
            End Set
        End Property

        Private _BookCarrActUnloadCompTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActUnloadCompTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActUnloadCompTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActUnloadCompTime = value
            End Set
        End Property

        Private _BookCarrScheduleDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrScheduleDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrScheduleDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrScheduleDate = value
            End Set
        End Property

        Private _BookCarrScheduleTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrScheduleTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrScheduleTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrScheduleTime = value
            End Set
        End Property

        Private _BookCarrActualDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActualDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrActualDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActualDate = value
            End Set
        End Property

        Private _BookCarrActualTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActualTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActualTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActualTime = value
            End Set
        End Property

        Private _BookCarrStartLoadingDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrStartLoadingDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrStartLoadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrStartLoadingDate = value
            End Set
        End Property

        Private _BookCarrStartLoadingTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrStartLoadingTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrStartLoadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrStartLoadingTime = value
            End Set
        End Property

        Private _BookCarrFinishLoadingDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrFinishLoadingDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrFinishLoadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrFinishLoadingDate = value
            End Set
        End Property

        Private _BookCarrFinishLoadingTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrFinishLoadingTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrFinishLoadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrFinishLoadingTime = value
            End Set
        End Property

        Private _BookCarrActLoadComplete_Date As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActLoadComplete_Date() As System.Nullable(Of Date)
            Get
                Return _BookCarrActLoadComplete_Date
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActLoadComplete_Date = value
            End Set
        End Property

        Private _BookCarrActLoadCompleteTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActLoadCompleteTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActLoadCompleteTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActLoadCompleteTime = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblPickList
            instance = DirectCast(MemberwiseClone(), tblPickList)
            Return instance
        End Function

#End Region

    End Class
End Namespace

