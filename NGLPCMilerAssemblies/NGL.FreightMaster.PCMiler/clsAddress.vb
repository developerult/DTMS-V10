Imports System.Runtime.InteropServices


<Guid("068D2FFC-0C0B-4850-990B-122F814F5E5F"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsAddress
#Region "Properties"
    Property strAddress() As String
    Property strCity() As String
    Property strState() As String
    Property strZip() As String

#End Region
End Interface


<Guid("4A4EAEE7-4515-4c4b-A21E-95A5D6BEDBF4"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.FreightMaster.PCMiler.clsAddress")> _
<Serializable()> _
Public Class clsAddress : Implements IclsAddress
#Region "Properties"

    Private _strAddress As String = ""
    Public Property strAddress() As String Implements IclsAddress.strAddress
        Get
            Return _strAddress
        End Get
        Set(ByVal value As String)
            _strAddress = value
        End Set
    End Property

    Private _strCity As String = ""
    Public Property strCity() As String Implements IclsAddress.strCity
        Get
            Return _strCity
        End Get
        Set(ByVal value As String)
            _strCity = value
        End Set
    End Property
    Private _strState As String = ""
    Public Property strState() As String Implements IclsAddress.strState
        Get
            Return _strState
        End Get
        Set(ByVal value As String)
            _strState = value
        End Set
    End Property
    Private _strZip As String = ""
    Public Property strZip() As String Implements IclsAddress.strZip
        Get
            Return _strZip
        End Get
        Set(ByVal value As String)
            _strZip = value
        End Set
    End Property
    Private _formatedAddress As String = ""
    Public Property formatedAddress() As String
        Get
            Return String.Format("{0}, {1}, {2}  {3}", strAddress, strCity, strState, strZip)
        End Get
        Set(ByVal value As String)
            _formatedAddress = value
        End Set
    End Property
#End Region
End Class
