Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class ParCategory
        Inherits DTOBaseClass

#Region " Data Members"

        Private _ParCatControl As Integer = 0
        <DataMember()> _
        Public Property ParCatControl() As Integer
            Get
                Return _ParCatControl
            End Get
            Set(ByVal value As Integer)
                _ParCatControl = value
            End Set
        End Property

        Private _ParCatName As String = ""
        <DataMember()> _
        Public Property ParCatName() As String
            Get
                Return Left(_ParCatName, 50)
            End Get
            Set(ByVal value As String)
                _ParCatName = Left(value, 50)
            End Set
        End Property

        Private _ParCatDescription As String = ""
        <DataMember()> _
        Public Property ParCatDescription() As String
            Get
                Return Left(_ParCatDescription, 255)
            End Get
            Set(ByVal value As String)
                _ParCatDescription = Left(value, 255)
            End Set
        End Property

        Private _Parameters As List(Of Parameter)
        <DataMember()> _
        Public Overloads Property Parameters() As List(Of Parameter)
            Get
                Return _Parameters
            End Get
            Set(ByVal value As List(Of Parameter))
                _Parameters = value
            End Set
        End Property

        Private _ParCatUpdated As Byte()
        <DataMember()> _
        Public Property ParCatUpdated() As Byte()
            Get
                Return _ParCatUpdated
            End Get
            Set(ByVal value As Byte())
                _ParCatUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New ParCategory
            instance = DirectCast(MemberwiseClone(), ParCategory)
            Return instance
        End Function

#End Region

    End Class

End Namespace
