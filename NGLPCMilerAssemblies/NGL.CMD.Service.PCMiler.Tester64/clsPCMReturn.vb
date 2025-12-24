Imports System.Runtime.InteropServices


<Guid("AAAA792E-7E5A-47fc-91FC-2C211313F6C8"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IclsPCMReturn
#Region "Properties"
    Property Message() As String
    Property RetVal() As Integer
#End Region
End Interface


<Guid("94D59A19-BB86-4a95-A9B2-CA03BDDBC0E4"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.FreightMaster.PCMiler.clsPCMReturn")> _
<Serializable()> _
Public Class clsPCMReturn : Implements IclsPCMReturn
#Region "Properties"
    Private _intRetVal As Integer = 0
    Public Property RetVal() As Integer Implements IclsPCMReturn.RetVal
        Get
            Return _intRetVal
        End Get
        Set(ByVal value As Integer)
            _intRetVal = value
        End Set
    End Property
    Private _strMessage As String = ""
    Public Property Message() As String Implements IclsPCMReturn.Message
        Get
            Return _strMessage
        End Get
        Set(ByVal value As String)
            _strMessage = value
        End Set
    End Property
#End Region
End Class

