Imports System.Runtime.InteropServices

<Guid("C7255087-353E-47F1-9A4A-B4172A1483FA"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsFMStopData
#Region "Properties"
    Property BookControl() As Integer
    Property BookCustCompControl() As Integer
    Property BookLoadControl() As Integer
    Property BookODControl() As Integer
    Property BookProNumber() As String
    Property LaneOriginAddressUse() As Boolean
    Property StopNumber() As Integer
    Property SeqNumber() As Integer
    Property RouteType() As Integer
    Property DistType() As Integer
    Property LegMiles() As Double
    Property TotalMiles() As Double
    Property LegCost() As Double
    Property TotalCost() As Double
    Property LegTime() As String
    Property TotalTime() As String
    Property LegTolls() As Double
    Property TotalTolls() As Double
    Property LegESTCHG() As Double
    Property TotalESTCHG() As Double
    Property Zip() As String
    Property City() As String
    Property State() As String
    Property Street() As String
    Property StopName() As String
    Property PCMilerStreet() As String
    Property PCMilerState() As String
    Property PCMilerCity() As String
    Property PCMilerZip() As String
    Property Matched() As Boolean
    Property LocationisOrigin() As Boolean
    Property AddressValid() As Boolean
    Property LogBadAddress() As Boolean
    Property Warning() As String
#End Region
End Interface

''' <summary>
''' Stop Data 
''' </summary>
''' <remarks>
''' Modified by RHR for v-8.5.0.001 on 11/19/2021 added country property
''' </remarks>
<Guid("35C3C00E-6606-4E14-9C90-914776D72922"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.Interfaces.clsPCMDataStopEx")> _
<Serializable()> _
Public Class clsFMStopData : Implements IclsFMStopData
#Region "Properties"

    ' Modified by RHR for v-8.5.0.001 on 11/19/2021 added country property
    Private _PCMilerCountry As String
    Public Property PCMilerCountry() As String
        Get
            Return _PCMilerCountry
        End Get
        Set(ByVal value As String)
            _PCMilerCountry = value
        End Set
    End Property

    ' Modified by RHR for v-8.5.0.001 on 11/19/2021 added country property
    Private _Country As String
    Public Property Country() As String
        Get
            Return _Country
        End Get
        Set(ByVal value As String)
            _Country = value
        End Set
    End Property

    Private mintBookControl As Integer = 0
    Public Property BookControl() As Integer Implements IclsFMStopData.BookControl
        Get
            Return mintBookControl
        End Get
        Set(ByVal Value As Integer)
            mintBookControl = Value
        End Set
    End Property

    Private mintBookCustCompControl As Integer = 0
    Public Property BookCustCompControl() As Integer Implements IclsFMStopData.BookCustCompControl
        Get
            Return mintBookCustCompControl
        End Get
        Set(ByVal Value As Integer)
            mintBookCustCompControl = Value
        End Set
    End Property

    Private mintBookLoadControl As Integer = 0
    Public Property BookLoadControl() As Integer Implements IclsFMStopData.BookLoadControl
        Get
            Return mintBookLoadControl
        End Get
        Set(ByVal Value As Integer)
            mintBookLoadControl = Value
        End Set
    End Property

    Private mintBookODControl As Integer = 0
    Public Property BookODControl() As Integer Implements IclsFMStopData.BookODControl
        Get
            Return mintBookODControl
        End Get
        Set(ByVal Value As Integer)
            mintBookODControl = Value
        End Set
    End Property



    Private mstrBookProNumber As String = ""
    Public Property BookProNumber() As String Implements IclsFMStopData.BookProNumber
        Get
            Return mstrBookProNumber
        End Get
        Set(ByVal Value As String)
            mstrBookProNumber = Value
        End Set
    End Property

    Private mblnLaneOriginAddressUse As Boolean = True
    Public Property LaneOriginAddressUse() As Boolean Implements IclsFMStopData.LaneOriginAddressUse
        Get
            Return mblnLaneOriginAddressUse
        End Get
        Set(ByVal Value As Boolean)
            mblnLaneOriginAddressUse = Value
        End Set
    End Property
    Private mintRouteType As Integer = 0
    Public Property RouteType() As Integer Implements IclsFMStopData.RouteType
        Get
            Return mintRouteType
        End Get
        Set(ByVal Value As Integer)
            mintRouteType = Value
        End Set
    End Property

    Private mintDistType As Integer = 0
    Public Property DistType() As Integer Implements IclsFMStopData.DistType
        Get
            Return mintDistType
        End Get
        Set(ByVal Value As Integer)
            mintDistType = Value
        End Set
    End Property



    Private mintStopNumber As Integer = 0
    Public Property StopNumber() As Integer Implements IclsFMStopData.StopNumber
        Get
            Return mintStopNumber
        End Get
        Set(ByVal Value As Integer)
            mintStopNumber = Value
        End Set
    End Property

    Private mintSeqNumber As Integer = 0
    Public Property SeqNumber() As Integer Implements IclsFMStopData.SeqNumber
        Get
            Return mintSeqNumber
        End Get
        Set(ByVal Value As Integer)
            mintSeqNumber = Value
        End Set
    End Property

    Private mdblLegMiles As Double = 0
    Public Property LegMiles() As Double Implements IclsFMStopData.LegMiles
        Get
            Return mdblLegMiles
        End Get
        Set(ByVal Value As Double)
            mdblLegMiles = Value
        End Set
    End Property

    Private mdblTotalMiles As Double = 0
    Public Property TotalMiles() As Double Implements IclsFMStopData.TotalMiles
        Get
            Return mdblTotalMiles
        End Get
        Set(ByVal Value As Double)
            mdblTotalMiles = Value
        End Set
    End Property

    Private mdblLegCost As Double = 0
    Public Property LegCost() As Double Implements IclsFMStopData.LegCost
        Get
            Return mdblLegCost
        End Get
        Set(ByVal Value As Double)
            mdblLegCost = Value
        End Set
    End Property

    Private mdblTotalCost As Double = 0
    Public Property TotalCost() As Double Implements IclsFMStopData.TotalCost
        Get
            Return mdblTotalCost
        End Get
        Set(ByVal Value As Double)
            mdblTotalCost = Value
        End Set
    End Property

    Private mstrLegTime As String = ""
    Public Property LegTime() As String Implements IclsFMStopData.LegTime
        Get
            Return mstrLegTime
        End Get
        Set(ByVal Value As String)
            mstrLegTime = Value
        End Set
    End Property

    Private mstrTotalTime As String = ""
    Public Property TotalTime() As String Implements IclsFMStopData.TotalTime
        Get
            Return mstrTotalTime
        End Get
        Set(ByVal Value As String)
            mstrTotalTime = Value
        End Set
    End Property

    Private mdblLegTolls As Double = 0
    Public Property LegTolls() As Double Implements IclsFMStopData.LegTolls
        Get
            Return mdblLegTolls
        End Get
        Set(ByVal Value As Double)
            mdblLegTolls = Value
        End Set
    End Property

    Private mdblTotalTolls As Double = 0
    Public Property TotalTolls() As Double Implements IclsFMStopData.TotalTolls
        Get
            Return mdblTotalTolls
        End Get
        Set(ByVal Value As Double)
            mdblTotalTolls = Value
        End Set
    End Property

    Private mdblLegESTCHG As Double = 0
    Public Property LegESTCHG() As Double Implements IclsFMStopData.LegESTCHG
        Get
            Return mdblLegESTCHG
        End Get
        Set(ByVal Value As Double)
            mdblLegESTCHG = Value
        End Set
    End Property

    Private mdblTotalESTCHG As Double = 0
    Public Property TotalESTCHG() As Double Implements IclsFMStopData.TotalESTCHG
        Get
            Return mdblTotalESTCHG
        End Get
        Set(ByVal Value As Double)
            mdblTotalESTCHG = Value
        End Set
    End Property

    Private mstrZip As String = ""
    Public Property Zip() As String Implements IclsFMStopData.Zip
        Get
            Return mstrZip
        End Get
        Set(ByVal Value As String)
            mstrZip = Value
        End Set
    End Property

    Private mstrCity As String = ""
    Public Property City() As String Implements IclsFMStopData.City
        Get
            Return mstrCity
        End Get
        Set(ByVal Value As String)
            mstrCity = Value
        End Set
    End Property

    Private mstrState As String = ""
    Public Property State() As String Implements IclsFMStopData.State
        Get
            Return mstrState
        End Get
        Set(ByVal Value As String)
            mstrState = Value
        End Set
    End Property

    Private mstrStreet As String = ""
    Public Property Street() As String Implements IclsFMStopData.Street
        Get
            Return mstrStreet
        End Get
        Set(ByVal Value As String)
            mstrStreet = Value
        End Set
    End Property

    Private mstrStopName As String = ""
    Public Property StopName() As String Implements IclsFMStopData.StopName
        Get
            Return mstrStopName
        End Get
        Set(ByVal Value As String)
            mstrStopName = Value
        End Set
    End Property


    Private mstrPCMilerStreet As String = ""
    Public Property PCMilerStreet() As String Implements IclsFMStopData.PCMilerStreet
        Get
            Return mstrPCMilerStreet
        End Get
        Set(ByVal Value As String)
            mstrPCMilerStreet = Value
        End Set
    End Property

    Private mstrPCMilerState As String = ""
    Public Property PCMilerState() As String Implements IclsFMStopData.PCMilerState
        Get
            Return mstrPCMilerState
        End Get
        Set(ByVal Value As String)
            mstrPCMilerState = Value
        End Set
    End Property

    Private mstrPCMilerCity As String = ""
    Public Property PCMilerCity() As String Implements IclsFMStopData.PCMilerCity
        Get
            Return mstrPCMilerCity
        End Get
        Set(ByVal Value As String)
            mstrPCMilerCity = Value
        End Set
    End Property

    Private mstrPCMilerZip As String = ""
    Public Property PCMilerZip() As String Implements IclsFMStopData.PCMilerZip
        Get
            Return mstrPCMilerZip
        End Get
        Set(ByVal Value As String)
            mstrPCMilerZip = Value
        End Set
    End Property

    Private mblnMatched As Boolean = False
    Public Property Matched() As Boolean Implements IclsFMStopData.Matched
        Get
            Return mblnMatched
        End Get
        Set(ByVal Value As Boolean)
            mblnMatched = Value
        End Set
    End Property

    Private mblnLocationisOrigin As Boolean = False
    Public Property LocationisOrigin() As Boolean Implements IclsFMStopData.LocationisOrigin
        Get
            Return mblnLocationisOrigin
        End Get
        Set(ByVal Value As Boolean)
            mblnLocationisOrigin = Value
        End Set
    End Property

    Private mblnAddressValid As Boolean = False
    Public Property AddressValid() As Boolean Implements IclsFMStopData.AddressValid
        Get
            Return mblnAddressValid
        End Get
        Set(ByVal Value As Boolean)
            mblnAddressValid = Value
        End Set
    End Property

    Private mblnLogBadAddress As Boolean = False
    Public Property LogBadAddress() As Boolean Implements IclsFMStopData.LogBadAddress
        Get
            Return mblnLogBadAddress
        End Get
        Set(ByVal Value As Boolean)
            mblnLogBadAddress = Value
        End Set
    End Property

    Private mstrWarning As String = ""
    Public Property Warning() As String Implements IclsFMStopData.Warning
        Get
            Return mstrWarning
        End Get
        Set(ByVal Value As String)
            mstrWarning = Value
        End Set
    End Property

#End Region
End Class
