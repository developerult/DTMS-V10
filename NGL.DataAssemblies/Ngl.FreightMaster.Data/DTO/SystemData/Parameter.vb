Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class Parameter
        Inherits DTOBaseClass


#Region " Data Members"

        Private _ParKey As String = ""
        <DataMember()> _
        Public Property ParKey() As String
            Get
                Return Left(_ParKey, 50)
            End Get
            Set(ByVal value As String)
                _ParKey = Left(value, 50)
            End Set
        End Property

        Private _ParValue As Double = 0
        <DataMember()> _
        Public Property ParValue() As Double
            Get
                Return _ParValue
            End Get
            Set(ByVal value As Double)
                _ParValue = value
            End Set
        End Property

        Private _ParText As String = ""
        <DataMember()> _
        Public Property ParText() As String
            Get
                Return Left(_ParText, 255)
            End Get
            Set(ByVal value As String)
                _ParText = Left(value, 255)
            End Set
        End Property

        Private _ParDescription As String = ""
        <DataMember()> _
        Public Property ParDescription() As String
            Get
                Return Left(_ParDescription, 255)
            End Get
            Set(ByVal value As String)
                _ParDescription = Left(value, 255)
            End Set
        End Property

        Private _ParIsGlobal As Boolean = False
        <DataMember()> _
        Public Property ParIsGlobal() As Boolean
            Get
                Return _ParIsGlobal
            End Get
            Set(ByVal value As Boolean)
                _ParIsGlobal = value
            End Set
        End Property

        Private _ParCategoryControl As Integer = 0
        <DataMember()> _
        Public Property ParCategoryControl() As Integer
            Get
                Return _ParCategoryControl
            End Get
            Set(ByVal value As Integer)
                _ParCategoryControl = value
            End Set
        End Property

        'Private _ParOLE As Byte()
        <DataMember()> _
        Public Property ParOLE() As Byte()
            Get
                If _ParOLEImage Is Nothing Then
                    Return New Byte() {}
                Else
                    Return _ParOLEImage.ToArray()
                End If
            End Get
            Set(ByVal value As Byte())
                '_ParOLE = value
                If value Is Nothing Then
                    _ParOLEImage = New Byte() {}
                Else
                    _ParOLEImage = value
                End If
            End Set
        End Property


        Private _ParOLEImage As System.Data.Linq.Binary
        Public Property ParOLEImage() As System.Data.Linq.Binary
            Get
                Return _ParOLEImage
            End Get
            Set(ByVal value As System.Data.Linq.Binary)
                _ParOLEImage = value
            End Set
        End Property

        Private _ParUpdated As Byte()
        <DataMember()> _
        Public Property ParUpdated() As Byte()
            Get
                Return _ParUpdated
            End Get
            Set(ByVal value As Byte())
                _ParUpdated = value
            End Set
        End Property

        Private _CompParameters As List(Of CompParameter)
        <DataMember()> _
        Public Property CompParameters() As List(Of CompParameter)
            Get
                Return _CompParameters
            End Get
            Set(ByVal value As List(Of CompParameter))
                _CompParameters = value
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


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New Parameter
            instance = DirectCast(MemberwiseClone(), Parameter)
            instance.CompParameters = Nothing
            For Each item In CompParameters
                instance.CompParameters.Add(DirectCast(item.Clone, CompParameter))
            Next
            Return instance
        End Function

#End Region
    End Class

End Namespace
