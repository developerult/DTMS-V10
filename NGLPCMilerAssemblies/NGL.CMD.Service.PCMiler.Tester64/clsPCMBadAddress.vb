Imports System.Runtime.InteropServices

<Guid("DD183253-408A-49b0-B882-EE0DD92B80FF"), _
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


<Guid("6659B02F-1D13-4dc6-AE08-6689AE4D7CA9"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.FreightMaster.PCMiler.clsPCMBadAddress")> _
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