Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompParameter
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CompParControl As Long = 0
        <DataMember()> _
        Public Property CompParControl() As Long
            Get
                Return _CompParControl
            End Get
            Set(ByVal value As Long)
                _CompParControl = value
            End Set
        End Property

        Private _CompParCompControl As Integer = 0
        <DataMember()> _
        Public Property CompParCompControl() As Integer
            Get
                Return _CompParCompControl
            End Get
            Set(ByVal value As Integer)
                _CompParCompControl = value
            End Set
        End Property

        Private _CompNumber As Integer = 0
        <DataMember()> _
        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set(ByVal value As Integer)
                _CompNumber = value
            End Set
        End Property

        Private _CompParKey As String = ""
        <DataMember()> _
        Public Property CompParKey() As String
            Get
                Return Left(_CompParKey, 50)
            End Get
            Set(ByVal value As String)
                _CompParKey = Left(value, 50)
            End Set
        End Property

        Private _CompParValue As System.Nullable(Of Double) = 0
        <DataMember()> _
        Public Property CompParValue() As System.Nullable(Of Double)
            Get
                Return _CompParValue
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _CompParValue = value
            End Set
        End Property

        Private _CompParText As String = ""
        <DataMember()> _
        Public Property CompParText() As String
            Get
                Return Left(_CompParText, 255)
            End Get
            Set(ByVal value As String)
                _CompParText = Left(value, 255)
            End Set
        End Property

        Private _CompParDescription As String = ""
        <DataMember()> _
        Public Property CompParDescription() As String
            Get
                Return Left(_CompParDescription, 255)
            End Get
            Set(ByVal value As String)
                _CompParDescription = Left(value, 255)
            End Set
        End Property

        'Private _CompParOLE As Byte()
        '<DataMember()> _
        'Public Property CompParOLE() As Byte()
        '    Get
        '        Return _CompParOLE
        '    End Get
        '    Set(ByVal value As Byte())
        '        _CompParOLE = value
        '    End Set
        'End Property

        'Private _ParOLE As Byte()
        <DataMember()> _
        Public Property CompParOLE() As Byte()
            Get
                If _CompParOLEImage Is Nothing Then
                    Return New Byte() {}
                Else
                    Return _CompParOLEImage.ToArray()
                End If
            End Get
            Set(ByVal value As Byte())
                '_ParOLE = value
                If value Is Nothing Then
                    _CompParOLEImage = New Byte() {}
                Else
                    _CompParOLEImage = value
                End If
            End Set
        End Property


        Private _CompParOLEImage As System.Data.Linq.Binary
        Public Property CompParOLEImage() As System.Data.Linq.Binary
            Get
                Return _CompParOLEImage
            End Get
            Set(ByVal value As System.Data.Linq.Binary)
                _CompParOLEImage = value
            End Set
        End Property


        Private _CompParCategoryControl As Integer = 0
        <DataMember()> _
        Public Property CompParCategoryControl() As Integer
            Get
                Return _CompParCategoryControl
            End Get
            Set(ByVal value As Integer)
                _CompParCategoryControl = value
            End Set
        End Property

        Private _CompParUpdated As Byte()
        <DataMember()> _
        Public Property CompParUpdated() As Byte()
            Get
                Return _CompParUpdated
            End Get
            Set(ByVal value As Byte())
                _CompParUpdated = value
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
            Dim instance As New CompParameter
            instance = DirectCast(MemberwiseClone(), CompParameter)
            Return instance
        End Function

#End Region

    End Class
End Namespace