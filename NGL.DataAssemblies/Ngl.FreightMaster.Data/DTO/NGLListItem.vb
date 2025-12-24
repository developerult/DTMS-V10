Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class NGLListItem
        Inherits DTOBaseClass


#Region " Constructor"

        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Creates a new object with a key value pair using a string value
        ''' </summary>
        ''' <param name="k">Integer</param>
        ''' <param name="s">String</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal k As Integer, ByVal s As String)
            MyBase.New()
            Me.Key = k
            Me.strValue = s
        End Sub

        ''' <summary>
        ''' Creates a new object with a key value pair using an integer value
        ''' </summary>
        ''' <param name="k">Integer</param>
        ''' <param name="i">Integer</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal k As Integer, ByVal i As Integer)
            MyBase.New()
            Me.Key = k
            Me.intValue = i
        End Sub

        ''' <summary>
        ''' Creates a new object with a key value pair using a double value
        ''' </summary>
        ''' <param name="k">Integer</param>
        ''' <param name="d">Double</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal k As Integer, ByVal d As Double)
            MyBase.New()
            Me.Key = k
            Me.dblValue = d
        End Sub

        ''' <summary>
        ''' Creates a new object with a key value pair using a date value
        ''' </summary>
        ''' <param name="k">Integer</param>
        ''' <param name="dt">Date</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal k As Integer, ByVal dt As Date)
            MyBase.New()
            Me.Key = k
            Me.dtValue = dt
        End Sub

        ''' <summary>
        ''' Creates a new object with a string value
        ''' </summary>
        ''' <param name="s">String</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal s As String)
            MyBase.New()
            Me.strValue = s
        End Sub

        ''' <summary>
        ''' Creates a new object with an integer value
        ''' </summary>
        ''' <param name="i">Integer</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal i As Integer)
            MyBase.New()
            Me.intValue = i
        End Sub

        ''' <summary>
        ''' Creates a new object with a double value
        ''' </summary>
        ''' <param name="d">Double</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal d As Double)
            MyBase.New()
            Me.dblValue = d
        End Sub

        ''' <summary>
        ''' Creates a new object with a date value
        ''' </summary>
        ''' <param name="dt">Date</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal dt As Date)
            MyBase.New()
            Me.dtValue = dt
        End Sub

        ''' <summary>
        ''' Creates a new object with a string value and reference link data
        ''' </summary>
        ''' <param name="s">String</param>
        ''' <param name="c">Control</param>
        ''' <param name="n">ControlReferenceName</param>
        ''' <param name="e">ControlReference</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal s As String, ByVal c As Integer, ByVal n As String, ByVal e As Utilities.NGLMessageKeyRef)
            MyBase.New()
            Me.strValue = s
            Me.Control = c
            Me.ControlReferenceName = n
            Me.ControlReference = e
        End Sub

        ''' <summary>
        ''' Creates a new object with an integer value and reference link data
        ''' </summary>
        ''' <param name="i">Integer</param>
        ''' <param name="c">Control</param>
        ''' <param name="n">ControlReferenceName</param>
        ''' <param name="e">ControlReference</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal i As Integer, ByVal c As Integer, ByVal n As String, ByVal e As Utilities.NGLMessageKeyRef)
            MyBase.New()
            Me.intValue = i
            Me.Control = c
            Me.ControlReferenceName = n
            Me.ControlReference = e
        End Sub

        ''' <summary>
        ''' Creates a new object with a double value and reference link data
        ''' </summary>
        ''' <param name="d">Double</param>
        ''' <param name="c">Control</param>
        ''' <param name="n">ControlReferenceName</param>
        ''' <param name="e">ControlReference</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal d As Double, ByVal c As Integer, ByVal n As String, ByVal e As Utilities.NGLMessageKeyRef)
            MyBase.New()
            Me.dblValue = d
            Me.Control = c
            Me.ControlReferenceName = n
            Me.ControlReference = e
        End Sub

        ''' <summary>
        ''' Creates a new object with a date value and reference link data
        ''' </summary>
        ''' <param name="dt">Date</param>
        ''' <param name="c">Control</param>
        ''' <param name="n">ControlReferenceName</param>
        ''' <param name="e">ControlReference</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal dt As Date, ByVal c As Integer, ByVal n As String, ByVal e As Utilities.NGLMessageKeyRef)
            MyBase.New()
            Me.dtValue = dt
            Me.Control = c
            Me.ControlReferenceName = n
            Me.ControlReference = e
        End Sub
       
#End Region

#Region " Data Members"

        Private _Key As Integer = 0
        <DataMember()> _
        Public Property Key() As Integer
            Get
                Return _Key
            End Get
            Set(ByVal value As Integer)
                _Key = value
            End Set
        End Property

        Private _strValue As String = ""
        <DataMember()> _
        Public Property strValue() As String
            Get
                Return _strValue
            End Get
            Set(ByVal value As String)
                _strValue = value
            End Set
        End Property

        Private _dtValue As Date?
        <DataMember()> _
        Public Property dtValue() As Date?
            Get
                Return _dtValue
            End Get
            Set(ByVal value As Date?)
                _dtValue = value
            End Set
        End Property

        Private _dblValue As Double?
        <DataMember()> _
        Public Property dblValue() As Double?
            Get
                Return _dblValue
            End Get
            Set(ByVal value As Double?)
                _dblValue = value
            End Set
        End Property

        Private _intValue As New Integer?
        <DataMember()> _
        Public Property intValue() As Integer?
            Get
                Return _intValue
            End Get
            Set(ByVal value As Integer?)
                _intValue = value
            End Set
        End Property

        Private _Control As Integer
        <DataMember()> _
        Public Property Control() As Integer
            Get
                Return _Control
            End Get
            Set(ByVal value As Integer)
                _Control = value
            End Set
        End Property

        Private _ControlReference As Utilities.NGLMessageKeyRef = Utilities.NGLMessageKeyRef.None
        <DataMember()> _
        Public Property ControlReference() As Utilities.NGLMessageKeyRef
            Get
                Return _ControlReference
            End Get
            Set(ByVal value As Utilities.NGLMessageKeyRef)
                _ControlReference = value
            End Set
        End Property

        Private _ControlReferenceName As String
        <DataMember()> _
        Public Property ControlReferenceName() As String
            Get
                Return _ControlReferenceName
            End Get
            Set(ByVal value As String)
                _ControlReferenceName = value
            End Set
        End Property



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New NGLListItem
            instance = DirectCast(MemberwiseClone(), NGLListItem)
            Return instance
        End Function
#End Region

    End Class
End Namespace