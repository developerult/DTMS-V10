Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblBookItemCommCodeXref
        Inherits DTOBaseClass

#Region " Data Members"

        Private _BICCXrefControl As Integer = 0
        <DataMember()> _
        Public Property BICCXrefControl() As Integer
            Get
                Return _BICCXrefControl
            End Get
            Set(ByVal value As Integer)
                _BICCXrefControl = value
            End Set
        End Property

        Private _BICCXrefCompControl As Integer = 0
        <DataMember()> _
        Public Property BICCXrefCompControl() As Integer
            Get
                Return _BICCXrefCompControl
            End Get
            Set(ByVal value As Integer)
                _BICCXrefCompControl = value
            End Set
        End Property

        Private _BICCXrefFieldName As String = ""
        <DataMember()> _
        Public Property BICCXrefFieldName() As String
            Get
                Return Left(_BICCXrefFieldName, 255)
            End Get
            Set(ByVal value As String)
                _BICCXrefFieldName = Left(value, 255)
            End Set
        End Property

        Private _AvailableFieldNames As String = ""
        ''' <summary>
        ''' This is a readonly property, the Get and Set are defined because the
        ''' WCF Wizard expects both and does not support read only properties, 
        ''' but updates to the property are ignored.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()> _
        Public Property AvailableFieldNames() As String
            Get
                Return "BookItemSize,BookItemDescription"
            End Get
            Set(ByVal value As String)
                _AvailableFieldNames = value
            End Set
        End Property

        Private _BICCXrefFilter As String = ""
        <DataMember()> _
        Public Property BICCXrefFilter() As String
            Get
                Return Left(_BICCXrefFilter, 50)
            End Get
            Set(ByVal value As String)
                _BICCXrefFilter = Left(value, 50)
            End Set
        End Property

        Private _BICCXrefCommCode As String = ""
        <DataMember()> _
        Public Property BICCXrefCommCode() As String
            Get
                Return Left(_BICCXrefCommCode, 1)
            End Get
            Set(ByVal value As String)
                _BICCXrefCommCode = Left(value, 1)
            End Set
        End Property

        Private _BICCXrefModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BICCXrefModDate() As System.Nullable(Of Date)
            Get
                Return _BICCXrefModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BICCXrefModDate = value
            End Set
        End Property

        Private _BICCXrefModUser As String = ""
        <DataMember()> _
        Public Property BICCXrefModUser() As String
            Get
                Return Left(_BICCXrefModUser, 100)
            End Get
            Set(ByVal value As String)
                _BICCXrefModUser = Left(value, 100)
            End Set
        End Property

        Private _BICCXrefUpdated As Byte()
        <DataMember()> _
        Public Property BICCXrefUpdated() As Byte()
            Get
                Return _BICCXrefUpdated
            End Get
            Set(ByVal value As Byte())
                _BICCXrefUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblBookItemCommCodeXref
            instance = DirectCast(MemberwiseClone(), tblBookItemCommCodeXref)
            Return instance
        End Function



#End Region

    End Class
End Namespace

