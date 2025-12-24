Imports System.Runtime.InteropServices

<Guid("3E3D4DC4-69D4-440d-BA40-AE320AE6D433"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsAllStops
#Region "Properties"
    'ReadOnly Property AllStops() As Collection
    Property BadAddressControls(ByVal Index As Integer) As Integer
    Property FailedAddressMessage() As String
    Property BadAddressCount() As Integer
    Property TotalMiles() As Double
    Property OriginZip() As String
    Property DestZip() As String
    Property AutoCorrectBadLaneZipCodes() As Double
    Property BatchID() As Double
    ReadOnly Property LastError() As String
#End Region

#Region "Collection Methods and Properties"
    Function Add(ByVal shtStopNumber As Short, ByVal strStopName As String, ByVal strID1 As String, ByVal strID2 As String, ByVal strTruckName As String, ByVal intTruckNumber As Integer, ByVal shtSeqNbr As Short, ByVal dblDistToPrev As Double, ByVal dblTotalRouteCost As Double, ByVal strConsNumber As String) As IclsAllStop
    ReadOnly Property Item(ByVal vntIndexKey As Object) As IclsAllStop ' clsAllStop
    ReadOnly Property COUNT() As Integer
    Sub Remove(ByVal vntIndexKey As Object)
#End Region

#Region "Methods"

#End Region
End Interface


<Guid("6A577058-49CA-4a21-B6CF-359C52F94822"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.FreightMaster.PCMiler.clsAllStops")> _
<Serializable()> _
Public Class clsAllStops : Implements IclsAllStops
#Region "Properties"
    Private mcolAllStops As Collection
    'Public ReadOnly Property AllStops() As Collection Implements IclsAllStops.AllStops
    '    Get
    '        Return mcolAllStops
    '    End Get
    'End Property

    Private mintBadAddressControls() As Integer
    Public Property BadAddressControls(ByVal Index As Integer) As Integer Implements IclsAllStops.BadAddressControls
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
    Public Property FailedAddressMessage() As String Implements IclsAllStops.FailedAddressMessage
        Get
            Return mstrFailedAddressMessage
        End Get
        Set(ByVal value As String)
            mstrFailedAddressMessage = value
        End Set
    End Property

    Private mintBadAddressCount As Integer = 0
    Public Property BadAddressCount() As Integer Implements IclsAllStops.BadAddressCount
        Get
            Return mintBadAddressCount
        End Get
        Set(ByVal value As Integer)
            mintBadAddressCount = value
        End Set
    End Property

    Private mdblTotalMiles As Double = 0
    Public Property TotalMiles() As Double Implements IclsAllStops.TotalMiles
        Get
            Return mdblTotalMiles
        End Get
        Set(ByVal value As Double)
            mdblTotalMiles = value
        End Set
    End Property

    Private mstrOriginZip As String = ""
    Public Property OriginZip() As String Implements IclsAllStops.OriginZip
        Get
            Return mstrOriginZip
        End Get
        Set(ByVal value As String)
            mstrOriginZip = value
        End Set
    End Property

    Private mstrDestZip As String = ""
    Public Property DestZip() As String Implements IclsAllStops.DestZip
        Get
            Return mstrDestZip
        End Get
        Set(ByVal value As String)
            mstrDestZip = value
        End Set
    End Property

    Private mdblAutoCorrectBadLaneZipCodes As Double = 0
    Public Property AutoCorrectBadLaneZipCodes() As Double Implements IclsAllStops.AutoCorrectBadLaneZipCodes
        Get
            Return mdblAutoCorrectBadLaneZipCodes
        End Get
        Set(ByVal value As Double)
            mdblAutoCorrectBadLaneZipCodes = value
        End Set
    End Property

    Private mdblBatchID As Double = 0
    Public Property BatchID() As Double Implements IclsAllStops.BatchID
        Get
            Return mdblBatchID
        End Get
        Set(ByVal value As Double)
            mdblBatchID = value
        End Set
    End Property

    Private mstrLastError As String = ""
    Public ReadOnly Property LastError() As String Implements IclsAllStops.LastError
        Get
            Return mstrLastError
        End Get
    End Property

#End Region

#Region "Constructors"
    Public Sub New()
        MyBase.New()
        mcolAllStops = New Collection
    End Sub

    Protected Overrides Sub Finalize()
        mcolAllStops = Nothing
        MyBase.Finalize()
    End Sub

#End Region
#Region "Collection Methods and Properties"
    Public Function Add(ByVal shtStopNumber As Short, ByVal strStopName As String, ByVal strID1 As String, ByVal strID2 As String, ByVal strTruckName As String, ByVal intTruckNumber As Integer, ByVal shtSeqNbr As Short, ByVal dblDistToPrev As Double, ByVal dblTotalRouteCost As Double, ByVal strConsNumber As String) As IclsAllStop Implements IclsAllStops.Add
        'create a new object
        Dim objNewMember As New clsAllStop
        Try
            'set the properties passed into the method
            With objNewMember
                .StopNumber = shtStopNumber
                .Stopname = strStopName
                .ID1 = strID1
                .ID2 = strID2
                .TruckDesignator = strTruckName
                .TruckNumber = intTruckNumber
                .SeqNbr = shtSeqNbr
                .DistToPrev = dblDistToPrev
                .TotalRouteCost = dblTotalRouteCost
                .ConsNumber = strConsNumber
            End With
            If shtStopNumber = 0 Then
                mcolAllStops.Add(objNewMember)
            Else
                mcolAllStops.Add(objNewMember, "k" & shtStopNumber)
            End If
            Return objNewMember
        Catch ex As Exception
            mstrLastError = ex.Message
            Return Nothing
        Finally
            objNewMember = Nothing
        End Try
        Return Nothing

    End Function

    Public Function Add(ByVal objNewMember As IclsAllStop) As Boolean
        Dim blnRet As Boolean = False
        Try
            If objNewMember.StopNumber = 0 Then
                mcolAllStops.Add(objNewMember)
            Else
                mcolAllStops.Add(objNewMember, "k" & objNewMember.StopNumber)
            End If
            blnRet = True
        Catch ex As Exception
            mstrLastError = ex.Message
            Return False
        End Try
        Return blnRet
    End Function

    Public ReadOnly Property Item(ByVal vntIndexKey As Object) As IclsAllStop Implements IclsAllStops.Item
        Get
            Return mcolAllStops(vntIndexKey)
        End Get
    End Property

    Public ReadOnly Property COUNT() As Integer Implements IclsAllStops.COUNT
        Get
            Return mcolAllStops.Count
        End Get
    End Property

    Public Sub Remove(ByVal vntIndexKey As Object) Implements IclsAllStops.Remove
        'used when removing an element from the collection
        'vntIndexKey contains either the Index or Key, which is why
        'it is declared as a Variant
        'Syntax: x.Remove(xyz)
        mcolAllStops.Remove(vntIndexKey)
    End Sub
#End Region

#Region "Methods"

#End Region
End Class
