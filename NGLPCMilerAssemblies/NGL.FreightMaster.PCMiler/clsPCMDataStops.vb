Imports System.Runtime.InteropServices

<Guid("8FBD8E53-EB17-4686-B4CD-0235C24F2D94"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsPCMDataStops
#Region "Properties"
    'ReadOnly Property Stops() As Collection
    ReadOnly Property LastError() As String
#End Region

#Region "Collection Methods and Properties"
    Function Add(ByVal BookControl As Integer, ByVal BookCustCompControl As Integer, ByVal BookLoadControl As Integer, ByVal BookODControl As Integer, ByVal BookStopNo As Integer, ByVal RouteType As Integer, ByVal DistType As Integer, ByVal BookOrigZip As String, ByVal BookDestZip As String, ByVal BookOrigAddress1 As String, ByVal BookDestAddress1 As String, ByVal BookOrigCity As String, ByVal BookDestCity As String, ByVal BookOrigState As String, ByVal BookDestState As String, ByVal BookProNumber As String, ByVal LaneOriginAddressUse As Boolean) As IclsPCMDataStop ' clsPCMDataStop
    ReadOnly Property Item(ByVal vntIndexKey As Object) As IclsPCMDataStop ' clsPCMDataStop
    ReadOnly Property COUNT() As Integer
    Sub Remove(ByVal vntIndexKey As Object)
#End Region

#Region "Methods"

#End Region
End Interface


<Guid("497B41A5-73EA-43fe-8631-5B9492245E7C"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.FreightMaster.PCMiler.clsPCMDataStops")> _
<Serializable()> _
Public Class clsPCMDataStops : Implements IclsPCMDataStops
#Region "Properties"
    Private mcolStops As Collection
    'Public ReadOnly Property Stops() As Collection Implements IclsPCMDataStops.Stops
    '    Get
    '        Return mcolStops
    '    End Get
    'End Property

    Private mstrLastError As String = ""
    Public ReadOnly Property LastError() As String Implements IclsPCMDataStops.LastError
        Get
            Return mstrLastError
        End Get
    End Property

#End Region

#Region "Constructors"
    Public Sub New()
        MyBase.New()
        mcolStops = New Collection
    End Sub

    Protected Overrides Sub Finalize()
        mcolStops = Nothing
        MyBase.Finalize()
    End Sub

#End Region

#Region "Collection Methods and Properties"
    Public Function Add(ByVal BookControl As Integer, ByVal BookCustCompControl As Integer, ByVal BookLoadControl As Integer, ByVal BookODControl As Integer, ByVal BookStopNo As Integer, ByVal RouteType As Integer, ByVal DistType As Integer, ByVal BookOrigZip As String, ByVal BookDestZip As String, ByVal BookOrigAddress1 As String, ByVal BookDestAddress1 As String, ByVal BookOrigCity As String, ByVal BookDestCity As String, ByVal BookOrigState As String, ByVal BookDestState As String, ByVal BookProNumber As String, ByVal LaneOriginAddressUse As Boolean) As IclsPCMDataStop Implements IclsPCMDataStops.Add
        'create a new object
        Dim objNewMember As New clsPCMDataStop
        Try
            'set the properties passed into the method
            With objNewMember
                .BookControl = BookControl
                .BookCustCompControl = BookCustCompControl
                .BookLoadControl = BookLoadControl
                .BookODControl = BookODControl
                .BookStopNo = BookStopNo
                .RouteType = RouteType
                .DistType = DistType
                .BookOrigZip = BookOrigZip
                .BookDestZip = BookDestZip
                .BookOrigAddress1 = BookOrigAddress1
                .BookDestAddress1 = BookDestAddress1
                .BookOrigCity = BookOrigCity
                .BookDestCity = BookDestCity
                .BookOrigState = BookOrigState
                .BookDestState = BookDestState
                .BookProNumber = BookProNumber
                .LaneOriginAddressUse = LaneOriginAddressUse
            End With
            If BookControl = 0 Then
                mcolStops.Add(objNewMember)
            Else
                mcolStops.Add(objNewMember, "k" & BookControl)
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

    Public ReadOnly Property Item(ByVal vntIndexKey As Object) As IclsPCMDataStop Implements IclsPCMDataStops.Item
        Get
            Return mcolStops(vntIndexKey)
        End Get
    End Property

    Public ReadOnly Property COUNT() As Integer Implements IclsPCMDataStops.COUNT
        Get
            Return mcolStops.Count
        End Get
    End Property

    Public Sub Remove(ByVal vntIndexKey As Object) Implements IclsPCMDataStops.Remove
        'used when removing an element from the collection
        'vntIndexKey contains either the Index or Key, which is why
        'it is declared as a Variant
        'Syntax: x.Remove(xyz)
        mcolStops.Remove(vntIndexKey)
    End Sub
#End Region

#Region "Methods"

#End Region
End Class

