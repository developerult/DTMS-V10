Imports System.Runtime.InteropServices


<Guid("F6955405-25E9-46ac-86D3-2DECDB8FAAF1"), _
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



<Guid("5665CF4B-1746-4258-B10B-2F1AB0D812CC"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.FreightMaster.PCMiler.clsPCMStop")> _
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