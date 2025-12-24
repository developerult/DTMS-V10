Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
Public Class tblFormList
        Inherits DTOBaseClass

#Region " Data Members"

        Private _FormControl As Integer = 0
        <DataMember()> _
        Public Property FormControl() As Integer
            Get
                Return _FormControl
            End Get
            Set(ByVal value As Integer)
                _FormControl = value
            End Set
        End Property

        Private _FormName As String = ""
        <DataMember()> _
        Public Property FormName() As String
            Get
                Return Left(_FormName, 50)
            End Get
            Set(ByVal value As String)
                _FormName = Left(value, 50)
            End Set
        End Property

        Private _FormDescription As String = ""
        <DataMember()> _
        Public Property FormDescription() As String
            Get
                Return Left(_FormDescription, 50)
            End Get
            Set(ByVal value As String)
                _FormDescription = Left(value, 50)
            End Set
        End Property

        Private _FormUpdated As Byte()
        <DataMember()> _
        Public Property FormUpdated() As Byte()
            Get
                Return _FormUpdated
            End Get
            Set(ByVal value As Byte())
                _FormUpdated = value
            End Set
        End Property

        Private _FormFormMenuControl As Integer = 0
        <DataMember()> _
        Public Property FormFormMenuControl() As Integer
            Get
                Return _FormFormMenuControl
            End Get
            Set(ByVal value As Integer)
                _FormFormMenuControl = value
            End Set
        End Property

        Private _FormMenuSequence As Integer = 0
        <DataMember()> _
        Public Property FormMenuSequence() As Integer
            Get
                Return _FormMenuSequence
            End Get
            Set(ByVal value As Integer)
                _FormMenuSequence = value
            End Set
        End Property

        Private _FormSecurityXrefControl As Integer = 0
        <DataMember()> _
        Public Property FormSecurityXrefControl() As Integer
            Get
                Return _FormSecurityXrefControl
            End Get
            Set(ByVal value As Integer)
                _FormSecurityXrefControl = value
            End Set
        End Property

        Private _FormSecurityGroupXrefControl As Integer = 0
        <DataMember()> _
        Public Property FormSecurityGroupXrefControl() As Integer
            Get
                Return _FormSecurityGroupXrefControl
            End Get
            Set(ByVal value As Integer)
                _FormSecurityGroupXrefControl = value
            End Set
        End Property

        Private _FormUserOverrideGroup As Boolean = False
        <DataMember()> _
        Public Property FormUserOverrideGroup() As Boolean
            Get
                Return _FormUserOverrideGroup
            End Get
            Set(ByVal value As Boolean)
                _FormUserOverrideGroup = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblFormList
            instance = DirectCast(MemberwiseClone(), tblFormList)
            Return instance
        End Function

#End Region
    End Class
End Namespace
