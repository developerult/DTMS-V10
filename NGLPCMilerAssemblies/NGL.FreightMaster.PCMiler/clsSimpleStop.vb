Imports System.Runtime.InteropServices



<Guid("3CDD10A3-A390-43ef-985F-02AA1544D874"), _
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


<Guid("90D1C6BC-9AAD-48f7-9B74-47167B3700E5"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.FreightMaster.PCMiler.clsSimpleStop")> _
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


