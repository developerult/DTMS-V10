Imports System.Runtime.InteropServices


<Guid("089C6C78-D00E-4EDD-AD6E-0487C058F419"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsPCMReturnEx
#Region "Properties"

    Property BadAddressControls(ByVal Index As Integer) As Integer
    Property FailedAddressMessage() As String
    Property BadAddressCount() As Integer
    Property TotalMiles() As Double
    Property OriginZip() As String
    Property DestZip() As String
    Property AutoCorrectBadLaneZipCodes() As Double
    Property BatchID() As Double
    ReadOnly Property LastError() As String
    Property Message() As String
    Property RetVal() As Integer
#End Region
End Interface


<Guid("1C2FDC27-B84F-41A9-876C-9C3692E3DC28"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.FreightMaster.PCMiler.clsPCMReturnEx")> _
<Serializable()> _
Public Class clsPCMReturnEx : Implements IclsPCMReturnEx
#Region "Properties"

    Private mintBadAddressControls() As Integer
    Public Property BadAddressControls(ByVal Index As Integer) As Integer Implements IclsPCMReturnEx.BadAddressControls
        Get
            If mintBadAddressControls Is Nothing Then Return 0
            If mintBadAddressControls.Length < (Index + 1) Then Return 0
            Return mintBadAddressControls(Index)
        End Get
        Set(ByVal value As Integer)
            If mintBadAddressControls Is Nothing Then
                ReDim mintBadAddressControls(Index)
            ElseIf Index = 0 And mintBadAddressControls.Length < 1 Then
                ReDim mintBadAddressControls(Index)

            ElseIf mintBadAddressControls.Length < (Index + 1) Then
                ReDim Preserve mintBadAddressControls(Index)
            End If
            mintBadAddressControls(Index) = value
        End Set
    End Property

    Private mstrFailedAddressMessage As String = ""
    Public Property FailedAddressMessage() As String Implements IclsPCMReturnEx.FailedAddressMessage
        Get
            Return mstrFailedAddressMessage
        End Get
        Set(ByVal value As String)
            mstrFailedAddressMessage = value
        End Set
    End Property

    Private mintBadAddressCount As Integer = 0
    Public Property BadAddressCount() As Integer Implements IclsPCMReturnEx.BadAddressCount
        Get
            Return mintBadAddressCount
        End Get
        Set(ByVal value As Integer)
            mintBadAddressCount = value
        End Set
    End Property

    Private mdblTotalMiles As Double = 0
    Public Property TotalMiles() As Double Implements IclsPCMReturnEx.TotalMiles
        Get
            Return mdblTotalMiles
        End Get
        Set(ByVal value As Double)
            mdblTotalMiles = value
        End Set
    End Property

    Private mstrOriginZip As String = ""
    Public Property OriginZip() As String Implements IclsPCMReturnEx.OriginZip
        Get
            Return mstrOriginZip
        End Get
        Set(ByVal value As String)
            mstrOriginZip = value
        End Set
    End Property

    Private mstrDestZip As String = ""
    Public Property DestZip() As String Implements IclsPCMReturnEx.DestZip
        Get
            Return mstrDestZip
        End Get
        Set(ByVal value As String)
            mstrDestZip = value
        End Set
    End Property

    Private mdblAutoCorrectBadLaneZipCodes As Double = 0
    Public Property AutoCorrectBadLaneZipCodes() As Double Implements IclsPCMReturnEx.AutoCorrectBadLaneZipCodes
        Get
            Return mdblAutoCorrectBadLaneZipCodes
        End Get
        Set(ByVal value As Double)
            mdblAutoCorrectBadLaneZipCodes = value
        End Set
    End Property

    Private mdblBatchID As Double = 0
    Public Property BatchID() As Double Implements IclsPCMReturnEx.BatchID
        Get
            Return mdblBatchID
        End Get
        Set(ByVal value As Double)
            mdblBatchID = value
        End Set
    End Property

    Private mstrLastError As String = ""
    Public ReadOnly Property LastError() As String Implements IclsPCMReturnEx.LastError
        Get
            Return mstrLastError
        End Get
    End Property

    Private _intRetVal As Integer = 0
    Public Property RetVal() As Integer Implements IclsPCMReturnEx.RetVal
        Get
            Return _intRetVal
        End Get
        Set(ByVal value As Integer)
            _intRetVal = value
        End Set
    End Property
    Private _strMessage As String = ""
    Public Property Message() As String Implements IclsPCMReturnEx.Message
        Get
            Return _strMessage
        End Get
        Set(ByVal value As String)
            _strMessage = value
        End Set
    End Property

    Private _Results As Object = Nothing
    Public Property Results() As Object
        Get
            Return _Results
        End Get
        Set(ByVal value As Object)
            _Results = value
        End Set
    End Property
#End Region
End Class

