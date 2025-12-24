Imports System.Runtime.InteropServices

<Guid("BF6BF9BD-179A-4958-895B-C284EF4F95A7"), _
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


<Guid("1CB671DC-8CEF-454D-A0AC-3F3714288B38"), _
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