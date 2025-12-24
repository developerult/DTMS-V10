Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblFormMenu
        Inherits DTOBaseClass

#Region " Data Members"

        Private _FormMenuControl As Integer = 0
        <DataMember()> _
        Public Property FormMenuControl() As Integer
            Get
                Return _FormMenuControl
            End Get
            Set(ByVal value As Integer)
                _FormMenuControl = value
            End Set
        End Property

        Private _FormMenuName As String = ""
        <DataMember()> _
        Public Property FormMenuName() As String
            Get
                Return Left(_FormMenuName, 100)
            End Get
            Set(ByVal value As String)
                _FormMenuName = Left(value, 100)
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

        Private _FormMenuDescription As String = ""
        <DataMember()> _
        Public Property FormMenuDescription() As String
            Get
                Return Left(_FormMenuDescription, 50)
            End Get
            Set(ByVal value As String)
                _FormMenuDescription = Left(value, 50)
            End Set
        End Property

        Private _FormMenuUpdated As Byte()
        <DataMember()> _
        Public Property FormMenuUpdated() As Byte()
            Get
                Return _FormMenuUpdated
            End Get
            Set(ByVal value As Byte())
                _FormMenuUpdated = value
            End Set
        End Property

        'Private _tblFormLists As List(Of tblFormList)
        '<DataMember()> _
        'Public Property tblFormLists() As List(Of tblFormList)
        '    Get
        '        Return _tblFormLists
        '    End Get
        '    Set(ByVal value As List(Of tblFormList))
        '        _tblFormLists = value
        '    End Set
        'End Property

        

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblFormMenu
            instance = DirectCast(MemberwiseClone(), tblFormMenu)
            'instance.tblFormLists = Nothing
            'For Each item In tblFormLists
            '    instance.tblFormLists.Add(DirectCast(item.Clone, tblFormList))
            'Next
            Return instance
        End Function

#End Region
    End Class
End Namespace
