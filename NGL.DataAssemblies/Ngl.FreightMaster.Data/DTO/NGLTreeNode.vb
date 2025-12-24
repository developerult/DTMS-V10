Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class NGLTreeNode
        Inherits DTOBaseClass


#Region " Data Members"
        ''' <summary>
        ''' Unique ID used by the tree view control 
        ''' referenced by the childeren data using the 
        ''' ParentTreeID property
        ''' </summary>
        ''' <remarks></remarks>
        Private _TreeID As Integer = 0
        <DataMember()> _
        Public Property TreeID() As Integer
            Get
                Return _TreeID
            End Get
            Set(ByVal value As Integer)
                If (Me._TreeID <> value) Then
                    Me._TreeID = value
                    Me.SendPropertyChanged("TreeID")
                End If
            End Set
        End Property
        Private _Control As Integer = 0
        <DataMember()> _
        Public Property Control() As Integer
            Get
                Return _Control
            End Get
            Set(ByVal value As Integer)
                If (Me._Control <> value) Then
                    Me._Control = value
                    Me.SendPropertyChanged("Control")
                End If
            End Set
        End Property

        Private _Name As String = ""
        <DataMember()> _
        Public Property Name() As String
            Get
                Return Left(_Name, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Name, value) = False) Then
                    Me._Name = Left(value, 50)
                    Me.SendPropertyChanged("Name")
                End If
            End Set
        End Property

        Private _Text As String = ""
        <DataMember()> _
        Public Property Text() As String
            Get
                Return Left(_Text, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Text, value) = False) Then
                    Me._Text = Left(value, 50)
                    Me.SendPropertyChanged("Text")
                End If
            End Set
        End Property

        Private _Description As String = ""
        <DataMember()> _
        Public Property Description() As String
            Get
                Return Left(_Description, 255)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Description, value) = False) Then
                    Me._Description = Left(value, 255)
                    Me.SendPropertyChanged("Description")
                End If
            End Set
        End Property

        Private _ClassName As String = ""
        <DataMember()> _
        Public Property ClassName() As String
            Get
                Return _ClassName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClassName, value) = False) Then
                    Me._ClassName = value
                    Me.SendPropertyChanged("ClassName")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Some records like Tariff Rates require a complex data key that is not an integer
        ''' in this case the control will be zero and the AltDataKey will be populated
        ''' </summary>
        ''' <remarks></remarks>
        Private _AltDataKey As String = ""
        <DataMember()> _
        Public Property AltDataKey() As String
            Get
                Return _AltDataKey
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AltDataKey, value) = False) Then
                    Me._AltDataKey = value
                    Me.SendPropertyChanged("AltDataKey")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Reference to the parent node's TreeID
        ''' </summary>
        ''' <remarks></remarks>
        Private _ParentTreeID As Integer = 0
        <DataMember()> _
        Public Property ParentTreeID() As Integer
            Get
                Return _ParentTreeID
            End Get
            Set(ByVal value As Integer)
                If (Me._ParentTreeID <> value) Then
                    Me._ParentTreeID = value
                    Me.SendPropertyChanged("ParentTreeID")
                End If
            End Set
        End Property

        ''' <summary>
        ''' May be empty when using flat data records as 
        ''' required by the RAD controls
        ''' </summary>
        ''' <remarks></remarks>
        Private _Children As New List(Of NGLTreeNode)
        <DataMember()> _
        Public Property Children() As List(Of NGLTreeNode)
            Get
                Return _Children
            End Get
            Set(ByVal value As List(Of NGLTreeNode))
                _Children = value
                Me.SendPropertyChanged("Children")
            End Set
        End Property

        ''' <summary>
        ''' Items is the same as children. Items field is required for Kendo controls as they conflict with a field called children.
        ''' </summary>
        ''' <remarks></remarks>
        Private _Items As New List(Of NGLTreeNode)
        <DataMember()> _
        Public Property Items() As List(Of NGLTreeNode)
            Get
                Return _Items
            End Get
            Set(ByVal value As List(Of NGLTreeNode))
                _Items = value
                Me.SendPropertyChanged("Items")
            End Set
        End Property

        Private _HasChildren As Boolean = False
        <DataMember()> _
         Public Property HasChildren() As Boolean
            Get
                Return _HasChildren
            End Get
            Set(ByVal value As Boolean)
                _HasChildren = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New NGLTreeNode
            instance = DirectCast(MemberwiseClone(), NGLTreeNode)
            instance.Children = Nothing
            If Not Children Is Nothing Then
                For Each item In Children
                    instance.Children.Add(DirectCast(item.Clone, NGLTreeNode))
                Next
            End If
            Return instance
        End Function

#End Region

    End Class
End Namespace
