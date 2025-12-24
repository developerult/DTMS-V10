Imports System.Runtime.InteropServices


<Guid("AB6BEF9A-E5F8-4997-B529-31CDA299E3AC"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsAddress
#Region "Properties"
    Property strAddress() As String
    Property strCity() As String
    Property strState() As String
    Property strZip() As String
#End Region
End Interface


<Guid("DDC042B4-00D0-489F-A74D-A27CAE2C949F"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsAllStop
#Region "Properties"
    Property ConsNumber() As String
    Property DistToPrev() As Double
    Property TotalRouteCost() As Double
    Property SeqNbr() As Short
    Property TruckDesignator() As String
    Property TruckNumber() As Integer
    Property StopNumber() As Short
    Property Stopname() As String
    Property ID1() As String
    Property ID2() As String
#End Region
End Interface



<Guid("FD9B63BA-F089-4D8D-B2F3-2BC172F6C2EE"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsPCMBadAddress
#Region "Properties"
    Property BookControl() As Integer
    Property LaneControl() As Integer
    Property objOrig() As clsAddress
    Property objDest() As clsAddress
    Property objPCMOrig() As clsAddress
    Property objPCMDest() As clsAddress
    Property Message() As String
    Property BatchID() As Double
#End Region
End Interface


<Guid("5D3301D9-BD2E-434C-8989-6E4FC96C56FE"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsPCMDataStop
#Region "Properties"
    Property BookControl() As Integer
    Property BookCustCompControl() As Integer
    Property BookLoadControl() As Integer
    Property BookODControl() As Integer
    Property BookStopNo() As Integer
    Property RouteType() As Integer
    Property DistType() As Integer
    Property BookOrigZip() As String
    Property BookDestZip() As String
    Property BookOrigAddress1() As String
    Property BookDestAddress1() As String
    Property BookOrigCity() As String
    Property BookDestCity() As String
    Property BookOrigState() As String
    Property BookDestState() As String
    Property BookProNumber() As String
    Property LaneOriginAddressUse() As Boolean
#End Region
End Interface


<Guid("49D43859-83A2-4F76-A208-D70F93F51D0D"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsPCMStop
#Region "Properties"
    Property BookLoadControl() As Integer
    Property BookODControl() As Integer
    Property LoopCt() As Short
    Property StopNumber() As Short
    Property StopSeq() As Short
    Property LegMiles() As Double
    Property TotalMiles() As Double
    Property LegCost() As Double
    Property TotalCost() As Double
    Property Zip() As String
    Property City() As String
    Property State() As String
    Property Street() As String
    Property StopName() As String
    Property LegTime() As String
    Property TotalTime() As String
    Property PCMilerStreet() As String
    Property PCMilerState() As String
    Property PCMilerCity() As String
    Property Matched() As Boolean
#End Region
End Interface

<Guid("C529F01A-8DD7-4780-AEFF-A428896BD99F"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsSimpleStop
#Region "Properties"
    Property Address() As String
    Property StopNumber() As Integer
    Property TotalMiles() As Double
    Property LegMiles() As Double
    Property LegCost() As Double
    Property TotalCost() As Double
    Property LegHours() As Double
    Property TotalHours() As Double
#End Region
End Interface

<Guid("D21B4240-D9E5-4F7A-8D91-FEE8ED15BD7A"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsPCMReturn
#Region "Properties"
    Property Message() As String
    Property RetVal() As Integer
#End Region
End Interface

'New Interfaces created for v-5.1.4

<Guid("8770800F-C6CB-48EB-AFF0-315BBD077617"), _
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


<Guid("AAFAF2C3-FDB1-437C-A6AE-7A79CA19CA21"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsPCMReportRecord
#Region "Properties"
    Property StopNumber() As Integer
    Property SeqNumber() As Integer
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
#End Region
End Interface


<Guid("747AD386-0601-4BF5-BEFD-053F5B041B47"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsPCMStopEx
#Region "Properties"
    Property BookControl() As Integer
    Property BookLoadControl() As Integer
    Property BookODControl() As Integer
    Property StopNumber() As Integer
    Property SeqNumber() As Integer
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
    Property Matched() As Boolean
    Property LocationisOrigin() As Boolean
#End Region
End Interface


'Public Class Definition

<Guid("BEA56824-B5BB-4B5C-9006-641C1348B005"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.Service.PCMiler32.clsAllStop")> _
<Serializable()> _
Public Class clsAllStop : Implements IclsAllStop
#Region "Properties"
    Private mintStopNumber As Short = 0
    Private mstrStopName As String = ""
    Private mstrID1 As String = ""
    Private mstrID2 As String = ""
    Private mdblDistToPrev As Double = 0
    Private mdblTotalRouteCost As Double = 0
    Private mintSeqNbr As Short = 0
    Private mlngTruckNumber As Integer = 0
    Private mstrTruckDesignator As String = ""
    Private mstrConsNumber As String = ""
    Public Property ConsNumber() As String Implements IclsAllStop.ConsNumber
        Get
            ConsNumber = Left(mstrConsNumber, 20)
        End Get
        Set(ByVal Value As String)
            mstrConsNumber = Value
        End Set
    End Property

    Public Property DistToPrev() As Double Implements IclsAllStop.DistToPrev
        Get
            DistToPrev = mdblDistToPrev
        End Get
        Set(ByVal Value As Double)
            mdblDistToPrev = Value
        End Set
    End Property

    Public Property TotalRouteCost() As Double Implements IclsAllStop.TotalRouteCost
        Get
            TotalRouteCost = mdblTotalRouteCost
        End Get
        Set(ByVal Value As Double)
            mdblTotalRouteCost = Value
        End Set
    End Property

    Public Property SeqNbr() As Short Implements IclsAllStop.SeqNbr
        Get
            SeqNbr = mintSeqNbr
        End Get
        Set(ByVal Value As Short)
            mintSeqNbr = Value
        End Set
    End Property

    Public Property TruckDesignator() As String Implements IclsAllStop.TruckDesignator
        Get
            TruckDesignator = mstrTruckDesignator
        End Get
        Set(ByVal Value As String)
            mstrTruckDesignator = Left(Value, 12)
        End Set
    End Property

    Public Property TruckNumber() As Integer Implements IclsAllStop.TruckNumber
        Get
            TruckNumber = mlngTruckNumber
        End Get
        Set(ByVal Value As Integer)
            mlngTruckNumber = Value
        End Set
    End Property

    Public Property StopNumber() As Short Implements IclsAllStop.StopNumber
        Get
            StopNumber = mintStopNumber
        End Get
        Set(ByVal Value As Short)
            mintStopNumber = Value
        End Set
    End Property

    Public Property Stopname() As String Implements IclsAllStop.Stopname
        Get
            Stopname = mstrStopName
        End Get
        Set(ByVal Value As String)
            mstrStopName = Left(Value, 20)
        End Set
    End Property

    Public Property ID1() As String Implements IclsAllStop.ID1
        Get
            ID1 = mstrID1
        End Get
        Set(ByVal Value As String)
            mstrID1 = Left(Value, 15)
        End Set
    End Property

    Public Property ID2() As String Implements IclsAllStop.ID2
        Get
            ID2 = mstrID2
        End Get
        Set(ByVal Value As String)
            mstrID2 = Left(Value, 10)
        End Set
    End Property
#End Region
End Class

<Guid("3CB53103-2FAC-43A0-B989-B32CA4E69AFA"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.Service.PCMiler32.clsAddress")> _
<Serializable()> _
Public Class clsAddress : Implements IclsAddress
#Region "Properties"

    Private _strAddress As String = ""
    Public Property strAddress() As String Implements IclsAddress.strAddress
        Get
            Return _strAddress
        End Get
        Set(ByVal value As String)
            _strAddress = value
        End Set
    End Property

    Private _strCity As String = ""
    Public Property strCity() As String Implements IclsAddress.strCity
        Get
            Return _strCity
        End Get
        Set(ByVal value As String)
            _strCity = value
        End Set
    End Property
    Private _strState As String = ""
    Public Property strState() As String Implements IclsAddress.strState
        Get
            Return _strState
        End Get
        Set(ByVal value As String)
            _strState = value
        End Set
    End Property
    Private _strZip As String = ""
    Public Property strZip() As String Implements IclsAddress.strZip
        Get
            Return _strZip
        End Get
        Set(ByVal value As String)
            _strZip = value
        End Set
    End Property
#End Region
End Class

<Guid("74BCBD33-3B83-47D7-BBA9-30143CEE6AC1"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.Service.PCmiler32.clsPCMBadAddress")> _
<Serializable()> _
Public Class clsPCMBadAddress : Implements IclsPCMBadAddress
#Region "Properties"
    Private mintBookControl As Integer = 0
    Public Property BookControl() As Integer Implements IclsPCMBadAddress.BookControl
        Get
            Return mintBookControl
        End Get
        Set(ByVal value As Integer)
            mintBookControl = value
        End Set
    End Property

    Private mintLaneControl As Integer = 0
    Public Property LaneControl() As Integer Implements IclsPCMBadAddress.LaneControl
        Get
            Return mintLaneControl
        End Get
        Set(ByVal value As Integer)
            mintLaneControl = value
        End Set
    End Property

    Private moobjOrig As New clsAddress
    Public Property objOrig() As clsAddress Implements IclsPCMBadAddress.objOrig
        Get
            Return moobjOrig
        End Get
        Set(ByVal value As clsAddress)
            moobjOrig = value
        End Set
    End Property

    Private moobjDest As New clsAddress
    Public Property objDest() As clsAddress Implements IclsPCMBadAddress.objDest
        Get
            Return moobjDest
        End Get
        Set(ByVal value As clsAddress)
            moobjDest = value
        End Set
    End Property

    Private moobjPCMOrig As New clsAddress
    Public Property objPCMOrig() As clsAddress Implements IclsPCMBadAddress.objPCMOrig
        Get
            Return moobjPCMOrig
        End Get
        Set(ByVal value As clsAddress)
            moobjPCMOrig = value
        End Set
    End Property

    Private moobjPCMDest As New clsAddress
    Public Property objPCMDest() As clsAddress Implements IclsPCMBadAddress.objPCMDest
        Get
            Return moobjPCMDest
        End Get
        Set(ByVal value As clsAddress)
            moobjPCMDest = value
        End Set
    End Property

    Private mstrMessage As String = ""
    Public Property Message() As String Implements IclsPCMBadAddress.Message
        Get
            Return mstrMessage
        End Get
        Set(ByVal value As String)
            mstrMessage = value
        End Set
    End Property

    Private mdblBatchID As Double = 0
    Public Property BatchID() As Double Implements IclsPCMBadAddress.BatchID
        Get
            Return mdblBatchID
        End Get
        Set(ByVal value As Double)
            mdblBatchID = value
        End Set
    End Property

#End Region
End Class

<Guid("26538770-2C5D-4AF5-A6EA-C17F128855AD"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.Service.PCMiler32.clsPCMDataStop")> _
<Serializable()> _
Public Class clsPCMDataStop : Implements IclsPCMDataStop
#Region "Properties"
    Private mintBookControl As Integer = 0
    Public Property BookControl() As Integer Implements IclsPCMDataStop.BookControl
        Get
            Return mintBookControl
        End Get
        Set(ByVal Value As Integer)
            mintBookControl = Value
        End Set
    End Property

    Private mintBookCustCompControl As Integer = 0
    Public Property BookCustCompControl() As Integer Implements IclsPCMDataStop.BookCustCompControl
        Get
            Return mintBookCustCompControl
        End Get
        Set(ByVal Value As Integer)
            mintBookCustCompControl = Value
        End Set
    End Property

    Private mintBookLoadControl As Integer = 0
    Public Property BookLoadControl() As Integer Implements IclsPCMDataStop.BookLoadControl
        Get
            Return mintBookLoadControl
        End Get
        Set(ByVal Value As Integer)
            mintBookLoadControl = Value
        End Set
    End Property

    Private mintBookODControl As Integer = 0
    Public Property BookODControl() As Integer Implements IclsPCMDataStop.BookODControl
        Get
            Return mintBookODControl
        End Get
        Set(ByVal Value As Integer)
            mintBookODControl = Value
        End Set
    End Property

    Private mintBookStopNo As Integer = 0
    Public Property BookStopNo() As Integer Implements IclsPCMDataStop.BookStopNo
        Get
            Return mintBookStopNo
        End Get
        Set(ByVal Value As Integer)
            mintBookStopNo = Value
        End Set
    End Property

    Private mintRouteType As Integer = 0
    Public Property RouteType() As Integer Implements IclsPCMDataStop.RouteType
        Get
            Return mintRouteType
        End Get
        Set(ByVal Value As Integer)
            mintRouteType = Value
        End Set
    End Property

    Private mintDistType As Integer = 0
    Public Property DistType() As Integer Implements IclsPCMDataStop.DistType
        Get
            Return mintDistType
        End Get
        Set(ByVal Value As Integer)
            mintDistType = Value
        End Set
    End Property

    Private mstrBookOrigZip As String = ""
    Public Property BookOrigZip() As String Implements IclsPCMDataStop.BookOrigZip
        Get
            Return mstrBookOrigZip
        End Get
        Set(ByVal Value As String)
            mstrBookOrigZip = Value
        End Set
    End Property

    Private mstrBookDestZip As String = ""
    Public Property BookDestZip() As String Implements IclsPCMDataStop.BookDestZip
        Get
            Return mstrBookDestZip
        End Get
        Set(ByVal Value As String)
            mstrBookDestZip = Value
        End Set
    End Property

    Private mstrBookOrigAddress1 As String = ""
    Public Property BookOrigAddress1() As String Implements IclsPCMDataStop.BookOrigAddress1
        Get
            Return mstrBookOrigAddress1
        End Get
        Set(ByVal Value As String)
            mstrBookOrigAddress1 = Value
        End Set
    End Property

    Private mstrBookDestAddress1 As String = ""
    Public Property BookDestAddress1() As String Implements IclsPCMDataStop.BookDestAddress1
        Get
            Return mstrBookDestAddress1
        End Get
        Set(ByVal Value As String)
            mstrBookDestAddress1 = Value
        End Set
    End Property

    Private mstrBookOrigCity As String = ""
    Public Property BookOrigCity() As String Implements IclsPCMDataStop.BookOrigCity
        Get
            Return mstrBookOrigCity
        End Get
        Set(ByVal Value As String)
            mstrBookOrigCity = Value
        End Set
    End Property

    Private mstrBookDestCity As String = ""
    Public Property BookDestCity() As String Implements IclsPCMDataStop.BookDestCity
        Get
            Return mstrBookDestCity
        End Get
        Set(ByVal Value As String)
            mstrBookDestCity = Value
        End Set
    End Property

    Private mstrBookOrigState As String = ""
    Public Property BookOrigState() As String Implements IclsPCMDataStop.BookOrigState
        Get
            Return mstrBookOrigState
        End Get
        Set(ByVal Value As String)
            mstrBookOrigState = Value
        End Set
    End Property

    Private mstrBookDestState As String = ""
    Public Property BookDestState() As String Implements IclsPCMDataStop.BookDestState
        Get
            Return mstrBookDestState
        End Get
        Set(ByVal Value As String)
            mstrBookDestState = Value
        End Set
    End Property

    Private mstrBookProNumber As String = ""
    Public Property BookProNumber() As String Implements IclsPCMDataStop.BookProNumber
        Get
            Return mstrBookProNumber
        End Get
        Set(ByVal Value As String)
            mstrBookProNumber = Value
        End Set
    End Property

    Private mblnLaneOriginAddressUse As Boolean = True
    Public Property LaneOriginAddressUse() As Boolean Implements IclsPCMDataStop.LaneOriginAddressUse
        Get
            Return mblnLaneOriginAddressUse
        End Get
        Set(ByVal Value As Boolean)
            mblnLaneOriginAddressUse = Value
        End Set
    End Property
#End Region
End Class

<Guid("2D5E9D5C-9950-4BEE-B329-63DD40DF39B4"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.Service.PCMiler32.clsPCMStop")> _
<Serializable()> _
Public Class clsPCMStop : Implements IclsPCMStop
#Region "Properties"
    Private mintBookLoadControl As Integer = 0
    Public Property BookLoadControl() As Integer Implements IclsPCMStop.BookLoadControl
        Get
            Return mintBookLoadControl
        End Get
        Set(ByVal Value As Integer)
            mintBookLoadControl = Value
        End Set
    End Property

    Private mintBookODControl As Integer = 0
    Public Property BookODControl() As Integer Implements IclsPCMStop.BookODControl
        Get
            Return mintBookODControl
        End Get
        Set(ByVal Value As Integer)
            mintBookODControl = Value
        End Set
    End Property

    Private mshtLoopCt As Short = 0
    Public Property LoopCt() As Short Implements IclsPCMStop.LoopCt
        Get
            Return mshtLoopCt
        End Get
        Set(ByVal Value As Short)
            mshtLoopCt = Value
        End Set
    End Property

    Private mshtStopNumber As Short = 0
    Public Property StopNumber() As Short Implements IclsPCMStop.StopNumber
        Get
            Return mshtStopNumber
        End Get
        Set(ByVal Value As Short)
            mshtStopNumber = Value
        End Set
    End Property

    Private mshtStopSeq As Short = 0
    Public Property StopSeq() As Short Implements IclsPCMStop.StopSeq
        Get
            Return mshtStopSeq
        End Get
        Set(ByVal Value As Short)
            mshtStopSeq = Value
        End Set
    End Property

    Private mdblLegMiles As Double = 0
    Public Property LegMiles() As Double Implements IclsPCMStop.LegMiles
        Get
            Return mdblLegMiles
        End Get
        Set(ByVal Value As Double)
            mdblLegMiles = Value
        End Set
    End Property

    Private mdblTotalMiles As Double = 0
    Public Property TotalMiles() As Double Implements IclsPCMStop.TotalMiles
        Get
            Return mdblTotalMiles
        End Get
        Set(ByVal Value As Double)
            mdblTotalMiles = Value
        End Set
    End Property

    Private mdblLegCost As Double = 0
    Public Property LegCost() As Double Implements IclsPCMStop.LegCost
        Get
            Return mdblLegCost
        End Get
        Set(ByVal Value As Double)
            mdblLegCost = Value
        End Set
    End Property

    Private mdblTotalCost As Double = 0
    Public Property TotalCost() As Double Implements IclsPCMStop.TotalCost
        Get
            Return mdblTotalCost
        End Get
        Set(ByVal Value As Double)
            mdblTotalCost = Value
        End Set
    End Property

    Private mstrZip As String = ""
    Public Property Zip() As String Implements IclsPCMStop.Zip
        Get
            Return mstrZip
        End Get
        Set(ByVal Value As String)
            mstrZip = Value
        End Set
    End Property

    Private mstrCity As String = ""
    Public Property City() As String Implements IclsPCMStop.City
        Get
            Return mstrCity
        End Get
        Set(ByVal Value As String)
            mstrCity = Value
        End Set
    End Property

    Private mstrState As String = ""
    Public Property State() As String Implements IclsPCMStop.State
        Get
            Return mstrState
        End Get
        Set(ByVal Value As String)
            mstrState = Value
        End Set
    End Property

    Private mstrStreet As String = ""
    Public Property Street() As String Implements IclsPCMStop.Street
        Get
            Return mstrStreet
        End Get
        Set(ByVal Value As String)
            mstrStreet = Value
        End Set
    End Property

    Private mstrStopName As String = ""
    Public Property StopName() As String Implements IclsPCMStop.StopName
        Get
            Return mstrStopName
        End Get
        Set(ByVal Value As String)
            mstrStopName = Value
        End Set
    End Property

    Private mstrLegTime As String = ""
    Public Property LegTime() As String Implements IclsPCMStop.LegTime
        Get
            Return mstrLegTime
        End Get
        Set(ByVal Value As String)
            mstrLegTime = Value
        End Set
    End Property

    Private mstrTotalTime As String = ""
    Public Property TotalTime() As String Implements IclsPCMStop.TotalTime
        Get
            Return mstrTotalTime
        End Get
        Set(ByVal Value As String)
            mstrTotalTime = Value
        End Set
    End Property

    Private mstrPCMilerStreet As String = ""
    Public Property PCMilerStreet() As String Implements IclsPCMStop.PCMilerStreet
        Get
            Return mstrPCMilerStreet
        End Get
        Set(ByVal Value As String)
            mstrPCMilerStreet = Value
        End Set
    End Property

    Private mstrPCMilerState As String = ""
    Public Property PCMilerState() As String Implements IclsPCMStop.PCMilerState
        Get
            Return mstrPCMilerState
        End Get
        Set(ByVal Value As String)
            mstrPCMilerState = Value
        End Set
    End Property

    Private mstrPCMilerCity As String = ""
    Public Property PCMilerCity() As String Implements IclsPCMStop.PCMilerCity
        Get
            Return mstrPCMilerCity
        End Get
        Set(ByVal Value As String)
            mstrPCMilerCity = Value
        End Set
    End Property

    Private mblnMatched As Boolean = False
    Public Property Matched() As Boolean Implements IclsPCMStop.Matched
        Get
            Return mblnMatched
        End Get
        Set(ByVal Value As Boolean)
            mblnMatched = Value
        End Set
    End Property
#End Region
End Class

<Guid("3238B75E-ADE3-4784-8FC1-91D008D3F167"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.Service.PCMiler32.clsSimpleStop")> _
<Serializable()> _
Public Class clsSimpleStop : Implements IclsSimpleStop
#Region "Properties"
    Private _strAddress As String = ""
    Public Property Address() As String Implements IclsSimpleStop.Address
        Get
            Return _strAddress
        End Get
        Set(ByVal value As String)
            _strAddress = value
        End Set
    End Property
    Private _intStopNumber As Integer = 0
    Public Property StopNumber() As Integer Implements IclsSimpleStop.StopNumber
        Get
            Return _intStopNumber
        End Get
        Set(ByVal value As Integer)
            _intStopNumber = value
        End Set
    End Property
    Private _dblTotalMiles As Double = 0
    Public Property TotalMiles() As Double Implements IclsSimpleStop.TotalMiles
        Get
            Return _dblTotalMiles
        End Get
        Set(ByVal value As Double)
            _dblTotalMiles = value
        End Set
    End Property
    Private _dblLegMiles As Double = 0
    Public Property LegMiles() As Double Implements IclsSimpleStop.LegMiles
        Get
            Return _dblLegMiles
        End Get
        Set(ByVal value As Double)
            _dblLegMiles = value
        End Set
    End Property


    Private _dbllegCost As Double = 0
    Public Property LegCost() As Double Implements IclsSimpleStop.LegCost
        Get
            Return _dbllegCost
        End Get
        Set(ByVal value As Double)
            _dbllegCost = value
        End Set
    End Property
    Private _dblTotalCost As Double = 0
    Public Property TotalCost() As Double Implements IclsSimpleStop.TotalCost
        Get
            Return _dblTotalCost
        End Get
        Set(ByVal value As Double)
            _dblTotalCost = value
        End Set
    End Property
    Private _dbllegHours As Double = 0
    Public Property LegHours() As Double Implements IclsSimpleStop.LegHours
        Get
            Return _dbllegHours
        End Get
        Set(ByVal value As Double)
            _dbllegHours = value
        End Set
    End Property
    Private _dblTotalHours As Double = 0
    Public Property TotalHours() As Double Implements IclsSimpleStop.TotalHours
        Get
            Return _dblTotalHours
        End Get
        Set(ByVal value As Double)
            _dblTotalHours = value
        End Set
    End Property

#End Region
End Class

<Guid("BC92AD46-6FA8-48E4-BD41-918FF1E7E549"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.Service.PCMiler32.clsPCMReturn")> _
<Serializable()> _
Public Class clsPCMReturn : Implements IclsPCMReturn
#Region "Properties"
    Private _intRetVal As Integer = 0
    Public Property RetVal() As Integer Implements IclsPCMReturn.RetVal
        Get
            Return _intRetVal
        End Get
        Set(ByVal value As Integer)
            _intRetVal = value
        End Set
    End Property
    Private _strMessage As String = ""
    Public Property Message() As String Implements IclsPCMReturn.Message
        Get
            Return _strMessage
        End Get
        Set(ByVal value As String)
            _strMessage = value
        End Set
    End Property
#End Region
End Class

'New Classes added to v-5.1.4

''' <summary>
''' Stop Data
''' </summary>
''' <remarks>
''' Modified by RHR for v-8.5.0.001 on 11/19/2021 added country property
''' </remarks>
<Guid("F3D7398A-FFC5-4F1B-95CC-ACC544C3460B"), _
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


<Guid("33BC7D09-A721-44C1-8EEB-02D0EA7077EA"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.Interfaces.clsPCMReportRecord")> _
<Serializable()> _
Public Class clsPCMReportRecord : Implements IclsPCMReportRecord
#Region "Properties"


    Private mintStopNumber As Integer = 0
    Public Property StopNumber() As Integer Implements IclsPCMReportRecord.StopNumber
        Get
            Return mintStopNumber
        End Get
        Set(ByVal Value As Integer)
            mintStopNumber = Value
        End Set
    End Property

    Private mintSeqNumber As Integer = 0
    Public Property SeqNumber() As Integer Implements IclsPCMReportRecord.SeqNumber
        Get
            Return mintSeqNumber
        End Get
        Set(ByVal Value As Integer)
            mintSeqNumber = Value
        End Set
    End Property

    Private mdblLegMiles As Double = 0
    Public Property LegMiles() As Double Implements IclsPCMReportRecord.LegMiles
        Get
            Return mdblLegMiles
        End Get
        Set(ByVal Value As Double)
            mdblLegMiles = Value
        End Set
    End Property

    Private mdblTotalMiles As Double = 0
    Public Property TotalMiles() As Double Implements IclsPCMReportRecord.TotalMiles
        Get
            Return mdblTotalMiles
        End Get
        Set(ByVal Value As Double)
            mdblTotalMiles = Value
        End Set
    End Property

    Private mdblLegCost As Double = 0
    Public Property LegCost() As Double Implements IclsPCMReportRecord.LegCost
        Get
            Return mdblLegCost
        End Get
        Set(ByVal Value As Double)
            mdblLegCost = Value
        End Set
    End Property

    Private mdblTotalCost As Double = 0
    Public Property TotalCost() As Double Implements IclsPCMReportRecord.TotalCost
        Get
            Return mdblTotalCost
        End Get
        Set(ByVal Value As Double)
            mdblTotalCost = Value
        End Set
    End Property

    Private mstrLegTime As String = ""
    Public Property LegTime() As String Implements IclsPCMReportRecord.LegTime
        Get
            Return mstrLegTime
        End Get
        Set(ByVal Value As String)
            mstrLegTime = Value
        End Set
    End Property

    Private mstrTotalTime As String = ""
    Public Property TotalTime() As String Implements IclsPCMReportRecord.TotalTime
        Get
            Return mstrTotalTime
        End Get
        Set(ByVal Value As String)
            mstrTotalTime = Value
        End Set
    End Property

    Private mdblLegTolls As Double = 0
    Public Property LegTolls() As Double Implements IclsPCMReportRecord.LegTolls
        Get
            Return mdblLegTolls
        End Get
        Set(ByVal Value As Double)
            mdblLegTolls = Value
        End Set
    End Property

    Private mdblTotalTolls As Double = 0
    Public Property TotalTolls() As Double Implements IclsPCMReportRecord.TotalTolls
        Get
            Return mdblTotalTolls
        End Get
        Set(ByVal Value As Double)
            mdblTotalTolls = Value
        End Set
    End Property

    Private mdblLegESTCHG As Double = 0
    Public Property LegESTCHG() As Double Implements IclsPCMReportRecord.LegESTCHG
        Get
            Return mdblLegESTCHG
        End Get
        Set(ByVal Value As Double)
            mdblLegESTCHG = Value
        End Set
    End Property

    Private mdblTotalESTCHG As Double = 0
    Public Property TotalESTCHG() As Double Implements IclsPCMReportRecord.TotalESTCHG
        Get
            Return mdblTotalESTCHG
        End Get
        Set(ByVal Value As Double)
            mdblTotalESTCHG = Value
        End Set
    End Property


    Private mstrZip As String = ""
    Public Property Zip() As String Implements IclsPCMReportRecord.Zip
        Get
            Return mstrZip
        End Get
        Set(ByVal Value As String)
            mstrZip = Value
        End Set
    End Property

    Private mstrCity As String = ""
    Public Property City() As String Implements IclsPCMReportRecord.City
        Get
            Return mstrCity
        End Get
        Set(ByVal Value As String)
            mstrCity = Value
        End Set
    End Property

    Private mstrState As String = ""
    Public Property State() As String Implements IclsPCMReportRecord.State
        Get
            Return mstrState
        End Get
        Set(ByVal Value As String)
            mstrState = Value
        End Set
    End Property

    Private mstrStreet As String = ""
    Public Property Street() As String Implements IclsPCMReportRecord.Street
        Get
            Return mstrStreet
        End Get
        Set(ByVal Value As String)
            mstrStreet = Value
        End Set
    End Property

    Private mstrStopName As String = ""
    Public Property StopName() As String Implements IclsPCMReportRecord.StopName
        Get
            Return mstrStopName
        End Get
        Set(ByVal Value As String)
            mstrStopName = Value
        End Set
    End Property

#End Region
End Class

<Guid("880D8E71-B8C7-4D37-B808-8412470D34AE"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.Interfaces.clsPCMStopEx")> _
<Serializable()> _
Public Class clsPCMStopEx : Implements IclsPCMStopEx
#Region "Properties"
    Private mintBookControl As Integer = 0
    Public Property BookControl() As Integer Implements IclsPCMStopEx.BookControl
        Get
            Return mintBookControl
        End Get
        Set(ByVal Value As Integer)
            mintBookControl = Value
        End Set
    End Property

    Private mintBookLoadControl As Integer = 0
    Public Property BookLoadControl() As Integer Implements IclsPCMStopEx.BookLoadControl
        Get
            Return mintBookLoadControl
        End Get
        Set(ByVal Value As Integer)
            mintBookLoadControl = Value
        End Set
    End Property

    Private mintBookODControl As Integer = 0
    Public Property BookODControl() As Integer Implements IclsPCMStopEx.BookODControl
        Get
            Return mintBookODControl
        End Get
        Set(ByVal Value As Integer)
            mintBookODControl = Value
        End Set
    End Property


    Private mintStopNumber As Integer = 0
    Public Property StopNumber() As Integer Implements IclsPCMStopEx.StopNumber
        Get
            Return mintStopNumber
        End Get
        Set(ByVal Value As Integer)
            mintStopNumber = Value
        End Set
    End Property

    Private mintSeqNumber As Integer = 0
    Public Property SeqNumber() As Integer Implements IclsPCMStopEx.SeqNumber
        Get
            Return mintSeqNumber
        End Get
        Set(ByVal Value As Integer)
            mintSeqNumber = Value
        End Set
    End Property

    Private mdblLegMiles As Double = 0
    Public Property LegMiles() As Double Implements IclsPCMStopEx.LegMiles
        Get
            Return mdblLegMiles
        End Get
        Set(ByVal Value As Double)
            mdblLegMiles = Value
        End Set
    End Property

    Private mdblTotalMiles As Double = 0
    Public Property TotalMiles() As Double Implements IclsPCMStopEx.TotalMiles
        Get
            Return mdblTotalMiles
        End Get
        Set(ByVal Value As Double)
            mdblTotalMiles = Value
        End Set
    End Property

    Private mdblLegCost As Double = 0
    Public Property LegCost() As Double Implements IclsPCMStopEx.LegCost
        Get
            Return mdblLegCost
        End Get
        Set(ByVal Value As Double)
            mdblLegCost = Value
        End Set
    End Property

    Private mdblTotalCost As Double = 0
    Public Property TotalCost() As Double Implements IclsPCMStopEx.TotalCost
        Get
            Return mdblTotalCost
        End Get
        Set(ByVal Value As Double)
            mdblTotalCost = Value
        End Set
    End Property

    Private mstrLegTime As String = ""
    Public Property LegTime() As String Implements IclsPCMStopEx.LegTime
        Get
            Return mstrLegTime
        End Get
        Set(ByVal Value As String)
            mstrLegTime = Value
        End Set
    End Property

    Private mstrTotalTime As String = ""
    Public Property TotalTime() As String Implements IclsPCMStopEx.TotalTime
        Get
            Return mstrTotalTime
        End Get
        Set(ByVal Value As String)
            mstrTotalTime = Value
        End Set
    End Property

    Private mdblLegTolls As Double = 0
    Public Property LegTolls() As Double Implements IclsPCMStopEx.LegTolls
        Get
            Return mdblLegTolls
        End Get
        Set(ByVal Value As Double)
            mdblLegTolls = Value
        End Set
    End Property

    Private mdblTotalTolls As Double = 0
    Public Property TotalTolls() As Double Implements IclsPCMStopEx.TotalTolls
        Get
            Return mdblTotalTolls
        End Get
        Set(ByVal Value As Double)
            mdblTotalTolls = Value
        End Set
    End Property

    Private mdblLegESTCHG As Double = 0
    Public Property LegESTCHG() As Double Implements IclsPCMStopEx.LegESTCHG
        Get
            Return mdblLegESTCHG
        End Get
        Set(ByVal Value As Double)
            mdblLegESTCHG = Value
        End Set
    End Property

    Private mdblTotalESTCHG As Double = 0
    Public Property TotalESTCHG() As Double Implements IclsPCMStopEx.TotalESTCHG
        Get
            Return mdblTotalESTCHG
        End Get
        Set(ByVal Value As Double)
            mdblTotalESTCHG = Value
        End Set
    End Property

    Private mstrZip As String = ""
    Public Property Zip() As String Implements IclsPCMStopEx.Zip
        Get
            Return mstrZip
        End Get
        Set(ByVal Value As String)
            mstrZip = Value
        End Set
    End Property

    Private mstrCity As String = ""
    Public Property City() As String Implements IclsPCMStopEx.City
        Get
            Return mstrCity
        End Get
        Set(ByVal Value As String)
            mstrCity = Value
        End Set
    End Property

    Private mstrState As String = ""
    Public Property State() As String Implements IclsPCMStopEx.State
        Get
            Return mstrState
        End Get
        Set(ByVal Value As String)
            mstrState = Value
        End Set
    End Property

    Private mstrStreet As String = ""
    Public Property Street() As String Implements IclsPCMStopEx.Street
        Get
            Return mstrStreet
        End Get
        Set(ByVal Value As String)
            mstrStreet = Value
        End Set
    End Property

    Private mstrStopName As String = ""
    Public Property StopName() As String Implements IclsPCMStopEx.StopName
        Get
            Return mstrStopName
        End Get
        Set(ByVal Value As String)
            mstrStopName = Value
        End Set
    End Property


    Private mstrPCMilerStreet As String = ""
    Public Property PCMilerStreet() As String Implements IclsPCMStopEx.PCMilerStreet
        Get
            Return mstrPCMilerStreet
        End Get
        Set(ByVal Value As String)
            mstrPCMilerStreet = Value
        End Set
    End Property

    Private mstrPCMilerState As String = ""
    Public Property PCMilerState() As String Implements IclsPCMStopEx.PCMilerState
        Get
            Return mstrPCMilerState
        End Get
        Set(ByVal Value As String)
            mstrPCMilerState = Value
        End Set
    End Property

    Private mstrPCMilerCity As String = ""
    Public Property PCMilerCity() As String Implements IclsPCMStopEx.PCMilerCity
        Get
            Return mstrPCMilerCity
        End Get
        Set(ByVal Value As String)
            mstrPCMilerCity = Value
        End Set
    End Property

    Private mblnMatched As Boolean = False
    Public Property Matched() As Boolean Implements IclsPCMStopEx.Matched
        Get
            Return mblnMatched
        End Get
        Set(ByVal Value As Boolean)
            mblnMatched = Value
        End Set
    End Property

    Private mblnLocationisOrigin As Boolean = False
    Public Property LocationisOrigin() As Boolean Implements IclsPCMStopEx.LocationisOrigin
        Get
            Return mblnLocationisOrigin
        End Get
        Set(ByVal Value As Boolean)
            mblnLocationisOrigin = Value
        End Set
    End Property


#End Region
End Class
