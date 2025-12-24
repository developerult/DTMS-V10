
Imports System.Runtime.InteropServices

<Guid("93A251BA-C2B6-4fb9-BFEF-938FEC51AE4A"), _
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

<Guid("BC8E2441-A0C1-435d-87D8-61007725E7DD"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.FreightMaster.PCMiler.clsAllStop")> _
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
