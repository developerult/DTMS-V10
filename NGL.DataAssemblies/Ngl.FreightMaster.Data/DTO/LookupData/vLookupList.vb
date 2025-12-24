Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class vLookupList
        Inherits DTOBaseClass


#Region " Constructors"
        Public Sub New()
            MyBase.new()
        End Sub

        Public Sub New(ByVal control As Integer, ByVal name As String, ByVal description As String)
            MyBase.new()
            With Me
                .Control = control
                .Name = name
                .Description = description
            End With
        End Sub
#End Region

#Region " Data Members"

        Private _Control As Integer = 0
        <DataMember()> _
        Public Property Control() As Integer
            Get
                Return _Control
            End Get
            Set(ByVal value As Integer)
                _Control = value
            End Set
        End Property

        Private _Name As String = ""
        <DataMember()> _
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        Private _Description As String = ""
        <DataMember()> _
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New vLookupList
            instance = DirectCast(MemberwiseClone(), vLookupList)
            Return instance
        End Function

#End Region
    End Class
End Namespace