Imports System.Runtime.InteropServices



<Guid("E853D288-8AD0-4754-9AAE-47B2ADA97D84"), _
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


<Guid("F35044C8-508A-449a-AEFC-B8909ABE9CEA"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.FreightMaster.PCMiler.clsPCMDataStop")> _
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