Imports System.Runtime.InteropServices


<Guid("B52DF448-6257-42a8-B4F0-A3300F9BD30E"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsPCMBadAddresses
#Region "Properties"
    'ReadOnly Property Addresses() As Collection
    ReadOnly Property LastError() As String
#End Region

#Region "Collection Methods and Properties"
    Function Add(ByVal BookControl As Integer, ByVal LaneControl As Integer, ByVal objOrig As IclsAddress, ByVal objDest As IclsAddress, ByVal objPCMOrig As IclsAddress, ByVal objPCMDest As IclsAddress, ByVal Message As String, ByVal BatchID As Double) As IclsPCMBadAddress ' clsPCMBadAddress
    ReadOnly Property Item(ByVal vntIndexKey As Object) As IclsPCMBadAddress ' clsPCMBadAddress
    ReadOnly Property COUNT() As Integer
    Sub Remove(ByVal vntIndexKey As Object)
#End Region

#Region "Methods"

#End Region
End Interface



<Guid("5EC7893B-85CA-4c81-9B68-81BC636CA9F7"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.FreightMaster.PCMiler.clsPCMBadAddresses")> _
<Serializable()> _
Public Class clsPCMBadAddresses : Implements IclsPCMBadAddresses
#Region "Properties"
    Private mcolAddresses As Collection
    'Public ReadOnly Property Addresses() As Collection Implements IclsPCMBadAddresses.Addresses
    '    Get
    '        Return mcolAddresses
    '    End Get
    'End Property

    Private mstrLastError As String = ""
    Public ReadOnly Property LastError() As String Implements IclsPCMBadAddresses.LastError
        Get
            Return mstrLastError
        End Get
    End Property

#End Region

#Region "Constructors"
    Public Sub New()
        MyBase.New()
        mcolAddresses = New Collection
    End Sub

    Protected Overrides Sub Finalize()
        mcolAddresses = Nothing
        MyBase.Finalize()
    End Sub

#End Region

#Region "Collection Methods and Properties"
    Public Function Add(ByVal BookControl As Integer, ByVal LaneControl As Integer, ByVal objOrig As IclsAddress, ByVal objDest As IclsAddress, ByVal objPCMOrig As IclsAddress, ByVal objPCMDest As IclsAddress, ByVal Message As String, ByVal BatchID As Double) As IclsPCMBadAddress Implements IclsPCMBadAddresses.Add
        'create a new object
        Dim objNewMember As New clsPCMBadAddress
        Try
            'set the properties passed into the method

            With objNewMember
                .BookControl = BookControl
                .LaneControl = LaneControl
                .objOrig = objOrig
                .objDest = objDest
                .objPCMOrig = objPCMOrig
                .objPCMDest = objPCMDest
                .Message = Message
                .BatchID = BatchID
            End With
            If BookControl = 0 Then
                mcolAddresses.Add(objNewMember)
            Else
                mcolAddresses.Add(objNewMember, "k" & BookControl)
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

    Public Function Add(ByVal objNewMember As IclsPCMBadAddress) As Boolean
        Dim blnRet As Boolean = False
        Try

            If objNewMember.BookControl = 0 Then
                mcolAddresses.Add(objNewMember)
            Else
                mcolAddresses.Add(objNewMember, "k" & objNewMember.BookControl)
            End If
            blnRet = True
        Catch ex As Exception
            mstrLastError = ex.Message
            Return Nothing
        Finally
            objNewMember = Nothing
        End Try
        Return blnRet

    End Function

    Public ReadOnly Property Item(ByVal vntIndexKey As Object) As IclsPCMBadAddress Implements IclsPCMBadAddresses.Item
        Get
            Return mcolAddresses(vntIndexKey)
        End Get
    End Property

    Public ReadOnly Property COUNT() As Integer Implements IclsPCMBadAddresses.COUNT
        Get
            Return mcolAddresses.Count
        End Get
    End Property

    Public Sub Remove(ByVal vntIndexKey As Object) Implements IclsPCMBadAddresses.Remove
        'used when removing an element from the collection
        'vntIndexKey contains either the Index or Key, which is why
        'it is declared as a Variant
        'Syntax: x.Remove(xyz)
        mcolAddresses.Remove(vntIndexKey)
    End Sub
#End Region

#Region "Methods"

#End Region
End Class

