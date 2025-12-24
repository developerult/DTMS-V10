Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class vBookMultiPick
        Inherits DTOBaseClass


#Region " Data Members"

        Private _MultiPickControl As Integer = 0

        Private _MultiPickBookControl As Integer = 0

        Private _MultiPickProNumber As String

        Private _MultiPickConsPrefix As String

        Private _MultiPickCustCompControl As System.Nullable(Of Integer)

        Private _MultiPickODControl As System.Nullable(Of Integer)

        Private _MultiPickCarrierControl As System.Nullable(Of Integer)

        Private _MultiPickLocationControl As System.Nullable(Of Integer)

        Private _MultiPickLocationisOrigin As System.Nullable(Of Boolean)

        Private _MultiPickName As String

        Private _MultiPickAddress1 As String

        Private _MultiPickAddress2 As String

        Private _MultiPickAddress3 As String

        Private _MultiPickCity As String

        Private _MultiPickState As String

        Private _MultiPickCountry As String

        Private _MultiPickZip As String

        Private _MultiPickPhone As String

        Private _MultiPickFax As String

        Private _MultiPickStopNumber As System.Nullable(Of Integer)

        Private _MultiPickMiles As System.Nullable(Of Double)

        Private _MultiPickPickNumber As System.Nullable(Of Integer)

        Private _MultiPickDateOrdered As System.Nullable(Of Date)

        Private _MultiPickDateLoad As System.Nullable(Of Date)

        Private _MultiPickDateRequired As System.Nullable(Of Date)

        Private _MultiPickDateDelivered As System.Nullable(Of Date)

        Private _MultiPickTotalCases As System.Nullable(Of Integer)

        Private _MultiPickTotalWgt As System.Nullable(Of Double)

        Private _MultiPickTotalPL As System.Nullable(Of Double)

        Private _MultiPickTotalCube As System.Nullable(Of Integer)

        Private _MultiPickTotalPX As System.Nullable(Of Integer)

        Private _MultiPickTranCode As String

        Private _MultiPickPayCode As String

        Private _MultiPickTypeCode As String

        Private _MultiPickDeliveryStopNumber As System.Nullable(Of Short)

        Private _MultiPickModDate As System.Nullable(Of Date)

        Private _MultiPickModUser As String

        Private _MultiPickOrderNumber As String

        Private _MultiPickOrderSequence As System.Nullable(Of Integer)

        Private _MultiPickTotalOrderMiles As System.Nullable(Of Double)

        Private _MultiPickRouteFinalDate As System.Nullable(Of Date)

        Private _MultiPickRouteFinalCode As String

        Private _MultiPickRouteFinalFlag As System.Nullable(Of Boolean)

        Private _MultiPickTransType As String

        Private _MultiPickRouteConsFlag As System.Nullable(Of Boolean)

        Private _MultiPickIsPickUpFlag As System.Nullable(Of Boolean)

        Private _MultiPickBilledBFC As System.Nullable(Of Decimal)

        Private _MultiPickCarrierCost As System.Nullable(Of Decimal)

        Private _MultiPickOtherCost As System.Nullable(Of Decimal)

        Private _MultiPickTotalCost As System.Nullable(Of Decimal)

        Private _MultiPickLoadSavings As System.Nullable(Of Decimal)

        Private _MultiPickGrossRevenue As System.Nullable(Of Decimal)

        Private _MultiPickNonTaxable As System.Nullable(Of Decimal)

        Private _MultiPickRouteBFC As System.Nullable(Of Decimal)

        Private _MultiPickRouteCarrierCost As System.Nullable(Of Decimal)

        Private _MultiPickRouteOtherCost As System.Nullable(Of Decimal)

        Private _MultiPickRouteTotalCost As System.Nullable(Of Decimal)

        Private _MultiPickRouteLoadSavings As System.Nullable(Of Decimal)

        Private _MultiPickRouteGrossRevenue As System.Nullable(Of Decimal)

        Private _MultiPickRouteNonTaxable As System.Nullable(Of Decimal)

        Private _MultiPickRouteMiles As System.Nullable(Of Double)

        Private _MultiPickLockAllCosts As System.Nullable(Of Boolean)

        Private _MultiPickLockBFCCost As System.Nullable(Of Boolean)

        Private _MultiPickCompanyName As String

        Private _MultiPickCompanyNumber As System.Nullable(Of Integer)

        Private _MultiPickUseImportFrtCost As System.Nullable(Of Boolean)

        Private _MultiPickLaneNumber As String

        Private _MultiPickLaneName As String

        Private _MultiPickCarrierName As String

        Private _MultiPickCarrierNumber As String

        Private _MultiPickPickupStopNumber As System.Nullable(Of Integer)

        Private _MultiPickBookFinARInvoiceDate As System.Nullable(Of Date)


        <DataMember()> _
        Public Property MultiPickControl() As Integer
            Get
                Return Me._MultiPickControl
            End Get
            Set(ByVal value As Integer)
                If (Me._MultiPickControl.Equals(value) = False) Then
                    Me._MultiPickControl = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property MultiPickBookControl() As Integer
            Get
                Return Me._MultiPickBookControl
            End Get
            Set(ByVal value As Integer)
                If (Me._MultiPickBookControl.Equals(value) = False) Then
                    Me._MultiPickBookControl = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickProNumber() As String
            Get
                Return Left(Me._MultiPickProNumber, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickProNumber, value) = False) Then
                    Me._MultiPickProNumber = Left(value, 20)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickConsPrefix() As String
            Get
                Return Left(Me._MultiPickConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickConsPrefix, value) = False) Then
                    Me._MultiPickConsPrefix = Left(value, 20)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickCustCompControl() As System.Nullable(Of Integer)
            Get
                Return Me._MultiPickCustCompControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._MultiPickCustCompControl.Equals(value) = False) Then
                    Me._MultiPickCustCompControl = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickODControl() As System.Nullable(Of Integer)
            Get
                Return Me._MultiPickODControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._MultiPickODControl.Equals(value) = False) Then
                    Me._MultiPickODControl = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickCarrierControl() As System.Nullable(Of Integer)
            Get
                Return Me._MultiPickCarrierControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._MultiPickCarrierControl.Equals(value) = False) Then
                    Me._MultiPickCarrierControl = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickLocationControl() As System.Nullable(Of Integer)
            Get
                Return Me._MultiPickLocationControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._MultiPickLocationControl.Equals(value) = False) Then
                    Me._MultiPickLocationControl = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickLocationisOrigin() As System.Nullable(Of Boolean)
            Get
                Return Me._MultiPickLocationisOrigin
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                If (Me._MultiPickLocationisOrigin.Equals(value) = False) Then
                    Me._MultiPickLocationisOrigin = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickName() As String
            Get
                Return Left(Me._MultiPickName, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickName, value) = False) Then
                    Me._MultiPickName = Left(value, 40)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickAddress1() As String
            Get
                Return Left(Me._MultiPickAddress1, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickAddress1, value) = False) Then
                    Me._MultiPickAddress1 = Left(value, 40)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickAddress2() As String
            Get
                Return Left(Me._MultiPickAddress2, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickAddress2, value) = False) Then
                    Me._MultiPickAddress2 = Left(value, 40)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickAddress3() As String
            Get
                Return Left(Me._MultiPickAddress3, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickAddress3, value) = False) Then
                    Me._MultiPickAddress3 = Left(value, 40)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickCity() As String
            Get
                Return Left(Me._MultiPickCity, 25)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickCity, value) = False) Then
                    Me._MultiPickCity = Left(value, 25)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickState() As String
            Get
                Return Left(Me._MultiPickState, 8)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickState, value) = False) Then
                    Me._MultiPickState = Left(value, 8)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickCountry() As String
            Get
                Return Left(Me._MultiPickCountry, 30)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickCountry, value) = False) Then
                    Me._MultiPickCountry = Left(value, 30)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickZip() As String
            Get
                Return Left(Me._MultiPickZip, 10)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickZip, value) = False) Then
                    Me._MultiPickZip = Left(value, 10)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickPhone() As String
            Get
                Return Left(Me._MultiPickPhone, 15)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickPhone, value) = False) Then
                    Me._MultiPickPhone = Left(value, 15)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickFax() As String
            Get
                Return Left(Me._MultiPickFax, 15)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickFax, value) = False) Then
                    Me._MultiPickFax = Left(value, 15)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickStopNumber() As System.Nullable(Of Integer)
            Get
                Return Me._MultiPickStopNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._MultiPickStopNumber.Equals(value) = False) Then
                    Me._MultiPickStopNumber = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickMiles() As System.Nullable(Of Double)
            Get
                Return Me._MultiPickMiles
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                If (Me._MultiPickMiles.Equals(value) = False) Then
                    Me._MultiPickMiles = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickPickNumber() As System.Nullable(Of Integer)
            Get
                Return Me._MultiPickPickNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._MultiPickPickNumber.Equals(value) = False) Then
                    Me._MultiPickPickNumber = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickDateOrdered() As System.Nullable(Of Date)
            Get
                Return Me._MultiPickDateOrdered
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._MultiPickDateOrdered.Equals(value) = False) Then
                    Me._MultiPickDateOrdered = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickDateLoad() As System.Nullable(Of Date)
            Get
                Return Me._MultiPickDateLoad
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._MultiPickDateLoad.Equals(value) = False) Then
                    Me._MultiPickDateLoad = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickDateRequired() As System.Nullable(Of Date)
            Get
                Return Me._MultiPickDateRequired
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._MultiPickDateRequired.Equals(value) = False) Then
                    Me._MultiPickDateRequired = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickDateDelivered() As System.Nullable(Of Date)
            Get
                Return Me._MultiPickDateDelivered
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._MultiPickDateDelivered.Equals(value) = False) Then
                    Me._MultiPickDateDelivered = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickTotalCases() As System.Nullable(Of Integer)
            Get
                Return Me._MultiPickTotalCases
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._MultiPickTotalCases.Equals(value) = False) Then
                    Me._MultiPickTotalCases = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickTotalWgt() As System.Nullable(Of Double)
            Get
                Return Me._MultiPickTotalWgt
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                If (Me._MultiPickTotalWgt.Equals(value) = False) Then
                    Me._MultiPickTotalWgt = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickTotalPL() As System.Nullable(Of Double)
            Get
                Return Me._MultiPickTotalPL
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                If (Me._MultiPickTotalPL.Equals(value) = False) Then
                    Me._MultiPickTotalPL = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property MultiPickTotalCube() As System.Nullable(Of Integer)
            Get
                Return Me._MultiPickTotalCube
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._MultiPickTotalCube.Equals(value) = False) Then
                    Me._MultiPickTotalCube = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickTotalPX() As System.Nullable(Of Integer)
            Get
                Return Me._MultiPickTotalPX
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._MultiPickTotalPX.Equals(value) = False) Then
                    Me._MultiPickTotalPX = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickTranCode() As String
            Get
                Return Left(Me._MultiPickTranCode, 3)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickTranCode, value) = False) Then
                    Me._MultiPickTranCode = Left(value, 3)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickPayCode() As String
            Get
                Return Left(Me._MultiPickPayCode, 3)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickPayCode, value) = False) Then
                    Me._MultiPickPayCode = Left(value, 3)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickTypeCode() As String
            Get
                Return Left(Me._MultiPickTypeCode, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickTypeCode, value) = False) Then
                    Me._MultiPickTypeCode = Left(value, 20)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickDeliveryStopNumber() As System.Nullable(Of Short)
            Get
                Return Me._MultiPickDeliveryStopNumber
            End Get
            Set(ByVal value As System.Nullable(Of Short))
                If (Me._MultiPickDeliveryStopNumber.Equals(value) = False) Then
                    Me._MultiPickDeliveryStopNumber = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickModDate() As System.Nullable(Of Date)
            Get
                Return Me._MultiPickModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._MultiPickModDate.Equals(value) = False) Then
                    Me._MultiPickModDate = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickModUser() As String
            Get
                Return Left(Me._MultiPickModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickModUser, value) = False) Then
                    Me._MultiPickModUser = Left(value, 100)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickOrderNumber() As String
            Get
                Return Left(Me._MultiPickOrderNumber, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickOrderNumber, value) = False) Then
                    Me._MultiPickOrderNumber = Left(value, 20)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickOrderSequence() As System.Nullable(Of Integer)
            Get
                Return Me._MultiPickOrderSequence
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._MultiPickOrderSequence.Equals(value) = False) Then
                    Me._MultiPickOrderSequence = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickTotalOrderMiles() As System.Nullable(Of Double)
            Get
                Return Me._MultiPickTotalOrderMiles
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                If (Me._MultiPickTotalOrderMiles.Equals(value) = False) Then
                    Me._MultiPickTotalOrderMiles = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickRouteFinalDate() As System.Nullable(Of Date)
            Get
                Return Me._MultiPickRouteFinalDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._MultiPickRouteFinalDate.Equals(value) = False) Then
                    Me._MultiPickRouteFinalDate = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickRouteFinalCode() As String
            Get
                Return Left(Me._MultiPickRouteFinalCode, 2)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickRouteFinalCode, value) = False) Then
                    Me._MultiPickRouteFinalCode = Left(value, 2)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickRouteFinalFlag() As System.Nullable(Of Boolean)
            Get
                Return Me._MultiPickRouteFinalFlag
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                If (Me._MultiPickRouteFinalFlag.Equals(value) = False) Then
                    Me._MultiPickRouteFinalFlag = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickTransType() As String
            Get
                Return Left(Me._MultiPickTransType, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickTransType, value) = False) Then
                    Me._MultiPickTransType = Left(value, 50)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickRouteConsFlag() As System.Nullable(Of Boolean)
            Get
                Return Me._MultiPickRouteConsFlag
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                If (Me._MultiPickRouteConsFlag.Equals(value) = False) Then
                    Me._MultiPickRouteConsFlag = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickIsPickUpFlag() As System.Nullable(Of Boolean)
            Get
                Return Me._MultiPickIsPickUpFlag
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                If (Me._MultiPickIsPickUpFlag.Equals(value) = False) Then
                    Me._MultiPickIsPickUpFlag = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickBilledBFC() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickBilledBFC
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickBilledBFC.Equals(value) = False) Then
                    Me._MultiPickBilledBFC = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickCarrierCost() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickCarrierCost
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickCarrierCost.Equals(value) = False) Then
                    Me._MultiPickCarrierCost = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickOtherCost() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickOtherCost
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickOtherCost.Equals(value) = False) Then
                    Me._MultiPickOtherCost = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickTotalCost() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickTotalCost
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickTotalCost.Equals(value) = False) Then
                    Me._MultiPickTotalCost = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickLoadSavings() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickLoadSavings
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickLoadSavings.Equals(value) = False) Then
                    Me._MultiPickLoadSavings = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickGrossRevenue() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickGrossRevenue
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickGrossRevenue.Equals(value) = False) Then
                    Me._MultiPickGrossRevenue = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickNonTaxable() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickNonTaxable
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickNonTaxable.Equals(value) = False) Then
                    Me._MultiPickNonTaxable = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickRouteBFC() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickRouteBFC
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickRouteBFC.Equals(value) = False) Then
                    Me._MultiPickRouteBFC = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickRouteCarrierCost() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickRouteCarrierCost
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickRouteCarrierCost.Equals(value) = False) Then
                    Me._MultiPickRouteCarrierCost = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickRouteOtherCost() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickRouteOtherCost
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickRouteOtherCost.Equals(value) = False) Then
                    Me._MultiPickRouteOtherCost = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickRouteTotalCost() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickRouteTotalCost
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickRouteTotalCost.Equals(value) = False) Then
                    Me._MultiPickRouteTotalCost = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property MultiPickRouteLoadSavings() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickRouteLoadSavings
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickRouteLoadSavings.Equals(value) = False) Then
                    Me._MultiPickRouteLoadSavings = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickRouteGrossRevenue() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickRouteGrossRevenue
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickRouteGrossRevenue.Equals(value) = False) Then
                    Me._MultiPickRouteGrossRevenue = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickRouteNonTaxable() As System.Nullable(Of Decimal)
            Get
                Return Me._MultiPickRouteNonTaxable
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._MultiPickRouteNonTaxable.Equals(value) = False) Then
                    Me._MultiPickRouteNonTaxable = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickRouteMiles() As System.Nullable(Of Double)
            Get
                Return Me._MultiPickRouteMiles
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                If (Me._MultiPickRouteMiles.Equals(value) = False) Then
                    Me._MultiPickRouteMiles = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickLockAllCosts() As System.Nullable(Of Boolean)
            Get
                Return Me._MultiPickLockAllCosts
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                If (Me._MultiPickLockAllCosts.Equals(value) = False) Then
                    Me._MultiPickLockAllCosts = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickLockBFCCost() As System.Nullable(Of Boolean)
            Get
                Return Me._MultiPickLockBFCCost
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                If (Me._MultiPickLockBFCCost.Equals(value) = False) Then
                    Me._MultiPickLockBFCCost = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickCompanyName() As String
            Get
                Return Left(Me._MultiPickCompanyName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickCompanyName, value) = False) Then
                    Me._MultiPickCompanyName = Left(value, 50)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickCompanyNumber() As System.Nullable(Of Integer)
            Get
                Return Me._MultiPickCompanyNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._MultiPickCompanyNumber.Equals(value) = False) Then
                    Me._MultiPickCompanyNumber = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickUseImportFrtCost() As System.Nullable(Of Boolean)
            Get
                Return Me._MultiPickUseImportFrtCost
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                If (Me._MultiPickUseImportFrtCost.Equals(value) = False) Then
                    Me._MultiPickUseImportFrtCost = value
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickLaneNumber() As String
            Get
                Return Left(Me._MultiPickLaneNumber, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickLaneNumber, value) = False) Then
                    Me._MultiPickLaneNumber = Left(value, 50)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickLaneName() As String
            Get
                Return Left(Me._MultiPickLaneName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickLaneName, value) = False) Then
                    Me._MultiPickLaneName = Left(value, 50)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickCarrierName() As String
            Get
                Return Left(Me._MultiPickCarrierName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickCarrierName, value) = False) Then
                    Me._MultiPickCarrierName = Left(value, 50)
                End If
            End Set
        End Property

        <DataMember()> _
         Public Property MultiPickCarrierNumber() As String
            Get
                Return Left(Me._MultiPickCarrierNumber, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MultiPickCarrierNumber, value) = False) Then
                    Me._MultiPickCarrierNumber = Left(value, 50)
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property MultiPickPickupStopNumber() As System.Nullable(Of Integer)
            Get
                Return Me._MultiPickPickupStopNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._MultiPickPickupStopNumber.Equals(value) = False) Then
                    Me._MultiPickPickupStopNumber = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property MultiPickBookFinARInvoiceDate() As System.Nullable(Of Date)
            Get
                Return _MultiPickBookFinARInvoiceDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _MultiPickBookFinARInvoiceDate = value
            End Set
        End Property

        Private _MultiPickBookSHID As String
        <DataMember()> _
        Public Property MultiPickBookSHID() As String
            Get
                Return Left(_MultiPickBookSHID, 50)
            End Get
            Set(ByVal value As String)
                _MultiPickBookSHID = Left(value, 50)
            End Set
        End Property

        Private _MultiPickBookExpDelDateTime As Date?
        <DataMember()> _
        Public Property MultiPickBookExpDelDateTime() As Date?
            Get
                Return _MultiPickBookExpDelDateTime
            End Get
            Set(ByVal value As Date?)
                _MultiPickBookExpDelDateTime = value
            End Set
        End Property

        Private _MultiPickBookMustLeaveByDateTime As Date?
        <DataMember()> _
        Public Property MultiPickBookMustLeaveByDateTime() As Date?
            Get
                Return _MultiPickBookMustLeaveByDateTime
            End Get
            Set(ByVal value As Date?)
                _MultiPickBookMustLeaveByDateTime = value
            End Set
        End Property

        Private _MultiPickBookOutOfRouteMiles As Double
        <DataMember()> _
        Public Property MultiPickBookOutOfRouteMiles() As Double
            Get
                Return _MultiPickBookOutOfRouteMiles
            End Get
            Set(ByVal value As Double)
                _MultiPickBookOutOfRouteMiles = value
            End Set
        End Property

        Private _MultiPickBookSpotRateAllocationFormula As Integer
        <DataMember()> _
        Public Property MultiPickBookSpotRateAllocationFormula() As Integer
            Get
                Return _MultiPickBookSpotRateAllocationFormula
            End Get
            Set(ByVal value As Integer)
                _MultiPickBookSpotRateAllocationFormula = value
            End Set
        End Property

        Private _MultiPickBookSpotRateAutoCalcBFC As Boolean = True
        <DataMember()> _
        Public Property MultiPickBookSpotRateAutoCalcBFC() As Boolean
            Get
                Return _MultiPickBookSpotRateAutoCalcBFC
            End Get
            Set(ByVal value As Boolean)
                _MultiPickBookSpotRateAutoCalcBFC = value
            End Set
        End Property

        Private _MultiPickBookSpotRateUseCarrierFuelAddendum As Boolean = False
        <DataMember()> _
        Public Property MultiPickBookSpotRateUseCarrierFuelAddendum() As Boolean
            Get
                Return _MultiPickBookSpotRateUseCarrierFuelAddendum
            End Get
            Set(ByVal value As Boolean)
                _MultiPickBookSpotRateUseCarrierFuelAddendum = value
            End Set
        End Property

        Private _MultiPickBookSpotRateBFCAllocationFormula As Integer
        <DataMember()> _
        Public Property MultiPickBookSpotRateBFCAllocationFormula() As Integer
            Get
                Return _MultiPickBookSpotRateBFCAllocationFormula
            End Get
            Set(ByVal value As Integer)
                _MultiPickBookSpotRateBFCAllocationFormula = value
            End Set
        End Property

        Private _MultiPickBookSpotRateTotalUnallocatedBFC As Decimal
        <DataMember()> _
        Public Property MultiPickBookSpotRateTotalUnallocatedBFC() As Decimal
            Get
                Return _MultiPickBookSpotRateTotalUnallocatedBFC
            End Get
            Set(ByVal value As Decimal)
                _MultiPickBookSpotRateTotalUnallocatedBFC = value
            End Set
        End Property

        Private _MultiPickBookSpotRateTotalUnallocatedLineHaul As Decimal
        <DataMember()> _
        Public Property MultiPickBookSpotRateTotalUnallocatedLineHaul() As Decimal
            Get
                Return _MultiPickBookSpotRateTotalUnallocatedLineHaul
            End Get
            Set(ByVal value As Decimal)
                _MultiPickBookSpotRateTotalUnallocatedLineHaul = value
            End Set
        End Property

        Private _MultiPickBookSpotRateUseFuelAddendum As Boolean = False
        <DataMember()> _
        Public Property MultiPickBookSpotRateUseFuelAddendum() As Boolean
            Get
                Return _MultiPickBookSpotRateUseFuelAddendum
            End Get
            Set(ByVal value As Boolean)
                _MultiPickBookSpotRateUseFuelAddendum = value
            End Set
        End Property

        Private _MultiPickBookRevLaneBenchMiles As Double? = 0
        <DataMember()> _
        Public Property MultiPickBookRevLaneBenchMiles() As Double?
            Get
                Return _MultiPickBookRevLaneBenchMiles
            End Get
            Set(ByVal value As Double?)
                _MultiPickBookRevLaneBenchMiles = value
            End Set
        End Property

        Private _MultiPickBookRevLoadMiles As Double? = 0
        <DataMember()> _
        Public Property MultiPickBookRevLoadMiles() As Double?
            Get
                Return _MultiPickBookRevLoadMiles
            End Get
            Set(ByVal value As Double?)
                _MultiPickBookRevLoadMiles = value
            End Set
        End Property

        Private _MultiPickBookCarrTarControl As Integer
        <DataMember()> _
        Public Property MultiPickBookCarrTarControl() As Integer
            Get
                Return _MultiPickBookCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _MultiPickBookCarrTarControl = value
            End Set
        End Property

        Private _MultiPickBookCarrTarName As String
        <DataMember()> _
        Public Property MultiPickBookCarrTarName() As String
            Get
                Return Left(_MultiPickBookCarrTarName, 50)
            End Get
            Set(ByVal value As String)
                _MultiPickBookCarrTarName = Left(value, 50)
            End Set
        End Property

        Private _MultiPickBookCarrTarEquipControl As Integer
        <DataMember()> _
        Public Property MultiPickBookCarrTarEquipControl() As Integer
            Get
                Return _MultiPickBookCarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                _MultiPickBookCarrTarEquipControl = value
            End Set
        End Property

        Private _MultiPickBookShipCarrierProControl As Integer?
        <DataMember()> _
        Public Property MultiPickBookShipCarrierProControl() As Integer?
            Get
                Return _MultiPickBookShipCarrierProControl
            End Get
            Set(ByVal value As Integer?)
                _MultiPickBookShipCarrierProControl = value
            End Set
        End Property

        Private _MultiPickBookLockAllCosts As Boolean = False
        <DataMember()> _
        Public Property MultiPickBookLockAllCosts() As Boolean
            Get
                Return _MultiPickBookLockAllCosts
            End Get
            Set(ByVal value As Boolean)
                _MultiPickBookLockAllCosts = value
            End Set
        End Property

        Private _MultiPickBookLockBFCCost As Boolean = False
        <DataMember()> _
        Public Property MultiPickBookLockBFCCost() As Boolean
            Get
                Return _MultiPickBookLockBFCCost
            End Get
            Set(ByVal value As Boolean)
                _MultiPickBookLockBFCCost = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New vBookMultiPick
            instance = DirectCast(MemberwiseClone(), vBookMultiPick)
            Return instance
        End Function

#End Region

    End Class
End Namespace