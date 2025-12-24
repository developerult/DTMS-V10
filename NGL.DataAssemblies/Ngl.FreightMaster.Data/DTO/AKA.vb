Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
Public Class AKA
        Inherits DTOBaseClass

#Region " Data Members"
        Private _AKAControl As Integer = 0
        <DataMember()> _
        Public Property AKAControl() As Integer
            Get
                Return _AKAControl
            End Get
            Set(ByVal value As Integer)
                _AKAControl = value
            End Set
        End Property

        Private _AKAValue As String = ""
        <DataMember()> _
        Public Property AKAValue() As String
            Get
                Return Left(_AKAValue, 50)
            End Get
            Set(ByVal value As String)
                _AKAValue = Left(value, 50)
            End Set
        End Property

        Private _AKA1 As String = ""
        <DataMember()> _
        Public Property AKA1() As String
            Get
                Return Left(_AKA1, 50)
            End Get
            Set(ByVal value As String)
                _AKA1 = Left(value, 50)
            End Set
        End Property
        Private _AKA2 As String = ""
        <DataMember()> _
        Public Property AKA2() As String
            Get
                Return Left(_AKA2, 50)
            End Get
            Set(ByVal value As String)
                _AKA2 = Left(value, 50)
            End Set
        End Property
        Private _AKA3 As String = ""
        <DataMember()> _
        Public Property AKA3() As String
            Get
                Return Left(_AKA3, 50)
            End Get
            Set(ByVal value As String)
                _AKA3 = Left(value, 50)
            End Set
        End Property
        Private _AKA4 As String = ""
        <DataMember()> _
        Public Property AKA4() As String
            Get
                Return Left(_AKA4, 50)
            End Get
            Set(ByVal value As String)
                _AKA4 = Left(value, 50)
            End Set
        End Property
        Private _AKA5 As String = ""
        <DataMember()> _
        Public Property AKA5() As String
            Get
                Return Left(_AKA5, 50)
            End Get
            Set(ByVal value As String)
                _AKA5 = Left(value, 50)
            End Set
        End Property
        Private _AKA6 As String = ""
        <DataMember()> _
        Public Property AKA6() As String
            Get
                Return Left(_AKA6, 50)
            End Get
            Set(ByVal value As String)
                _AKA6 = Left(value, 50)
            End Set
        End Property
        Private _AKA7 As String = ""
        <DataMember()> _
        Public Property AKA7() As String
            Get
                Return Left(_AKA7, 50)
            End Get
            Set(ByVal value As String)
                _AKA7 = Left(value, 50)
            End Set
        End Property
        Private _AKA8 As String = ""
        <DataMember()> _
        Public Property AKA8() As String
            Get
                Return Left(_AKA8, 50)
            End Get
            Set(ByVal value As String)
                _AKA8 = Left(value, 50)
            End Set
        End Property
        Private _AKA9 As String = ""
        <DataMember()> _
        Public Property AKA9() As String
            Get
                Return Left(_AKA9, 50)
            End Get
            Set(ByVal value As String)
                _AKA9 = Left(value, 50)
            End Set
        End Property
        Private _AKA10 As String = ""
        <DataMember()> _
        Public Property AKA10() As String
            Get
                Return Left(_AKA10, 50)
            End Get
            Set(ByVal value As String)
                _AKA10 = Left(value, 50)
            End Set
        End Property

        Private _AKAUpdated As Byte()
        <DataMember()> _
        Public Property AKAUpdated() As Byte()
            Get
                Return _AKAUpdated
            End Get
            Set(ByVal value As Byte())
                _AKAUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AKA
            instance = DirectCast(MemberwiseClone(), AKA)
            Return instance
        End Function

#End Region

    End Class
End Namespace