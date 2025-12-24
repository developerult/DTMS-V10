Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class Seasonality
        Inherits DTOBaseClass


#Region " Data Members"
        Private _SeasControl As Integer = 0
        <DataMember()> _
        Public Property SeasControl() As Integer
            Get
                Return _SeasControl
            End Get
            Set(ByVal value As Integer)
                _SeasControl = value
            End Set
        End Property

        Private _SeasProfileNo As Integer = 0
        <DataMember()> _
        Public Property SeasProfileNo() As Integer
            Get
                Return _SeasProfileNo
            End Get
            Set(ByVal value As Integer)
                _SeasProfileNo = value
            End Set
        End Property

        Private _SeasDescription As String = ""
        <DataMember()> _
        Public Property SeasDescription() As String
            Get
                Return Left(_SeasDescription, 50)
            End Get
            Set(ByVal value As String)
                _SeasDescription = Left(value, 50)
            End Set
        End Property

        Private _SeasMo1 As Double = 0
        <DataMember()> _
        Public Property SeasMo1() As Double
            Get
                Return _SeasMo1
            End Get
            Set(ByVal value As Double)
                _SeasMo1 = value
            End Set
        End Property

        Private _SeasMo2 As Double = 0
        <DataMember()> _
        Public Property SeasMo2() As Double
            Get
                Return _SeasMo2
            End Get
            Set(ByVal value As Double)
                _SeasMo2 = value
            End Set
        End Property

        Private _SeasMo3 As Double = 0
        <DataMember()> _
        Public Property SeasMo3() As Double
            Get
                Return _SeasMo3
            End Get
            Set(ByVal value As Double)
                _SeasMo3 = value
            End Set
        End Property

        Private _SeasMo4 As Double = 0
        <DataMember()> _
        Public Property SeasMo4() As Double
            Get
                Return _SeasMo4
            End Get
            Set(ByVal value As Double)
                _SeasMo4 = value
            End Set
        End Property

        Private _SeasMo5 As Double = 0
        <DataMember()> _
        Public Property SeasMo5() As Double
            Get
                Return _SeasMo5
            End Get
            Set(ByVal value As Double)
                _SeasMo5 = value
            End Set
        End Property

        Private _SeasMo6 As Double = 0
        <DataMember()> _
        Public Property SeasMo6() As Double
            Get
                Return _SeasMo6
            End Get
            Set(ByVal value As Double)
                _SeasMo6 = value
            End Set
        End Property

        Private _SeasMo7 As Double = 0
        <DataMember()> _
        Public Property SeasMo7() As Double
            Get
                Return _SeasMo7
            End Get
            Set(ByVal value As Double)
                _SeasMo7 = value
            End Set
        End Property

        Private _SeasMo8 As Double = 0
        <DataMember()> _
        Public Property SeasMo8() As Double
            Get
                Return _SeasMo8
            End Get
            Set(ByVal value As Double)
                _SeasMo8 = value
            End Set
        End Property

        Private _SeasMo9 As Double = 0
        <DataMember()> _
        Public Property SeasMo9() As Double
            Get
                Return _SeasMo9
            End Get
            Set(ByVal value As Double)
                _SeasMo9 = value
            End Set
        End Property

        Private _SeasMo10 As Double = 0
        <DataMember()> _
        Public Property SeasMo10() As Double
            Get
                Return _SeasMo10
            End Get
            Set(ByVal value As Double)
                _SeasMo10 = value
            End Set
        End Property

        Private _SeasMo11 As Double = 0
        <DataMember()> _
        Public Property SeasMo11() As Double
            Get
                Return _SeasMo11
            End Get
            Set(ByVal value As Double)
                _SeasMo11 = value
            End Set
        End Property

        Private _SeasMo12 As Double = 0
        <DataMember()> _
        Public Property SeasMo12() As Double
            Get
                Return _SeasMo12
            End Get
            Set(ByVal value As Double)
                _SeasMo12 = value
            End Set
        End Property

        Private _SeasModUser As String = ""
        <DataMember()> _
        Public Property SeasModUser() As String
            Get
                Return Left(_SeasModUser, 100)
            End Get
            Set(ByVal value As String)
                _SeasModUser = Left(value, 100)
            End Set
        End Property

        Private _SeasModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SeasModDate() As System.Nullable(Of Date)
            Get
                Return _SeasModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _SeasModDate = value
            End Set
        End Property

        Private _SeasUpdated As Byte()
        <DataMember()> _
        Public Property SeasUpdated() As Byte()
            Get
                Return _SeasUpdated
            End Get
            Set(ByVal value As Byte())
                _SeasUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New Seasonality
            instance = DirectCast(MemberwiseClone(), Seasonality)
            Return instance
        End Function

#End Region

    End Class
End Namespace