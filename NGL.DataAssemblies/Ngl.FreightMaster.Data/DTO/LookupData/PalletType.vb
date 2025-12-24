Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class PalletType
        Inherits DTOBaseClass


#Region " Data Members"
        Private _ID As Integer = 0
        <DataMember()> _
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Private _PalletTypeName As String = ""
        <DataMember()> _
        Public Property PalletTypeName() As String
            Get
                Return Left(_PalletTypeName, 50)
            End Get
            Set(ByVal value As String)
                _PalletTypeName = Left(value, 50)
            End Set
        End Property

        Private _PalletTypeDescription As String = ""
        <DataMember()> _
        Public Property PalletTypeDescription() As String
            Get
                Return Left(_PalletTypeDescription, 50)
            End Get
            Set(ByVal value As String)
                _PalletTypeDescription = Left(value, 50)
            End Set
        End Property

        Private _PalletTypeWeight As Double = 0
        <DataMember()> _
        Public Property PalletTypeWeight() As Double
            Get
                Return _PalletTypeWeight
            End Get
            Set(ByVal value As Double)
                _PalletTypeWeight = value
            End Set
        End Property

        Private _PalletTypeHeight As Double = 0
        <DataMember()> _
        Public Property PalletTypeHeight As Double
            Get
                Return _PalletTypeHeight
            End Get
            Set(value As Double)
                _PalletTypeHeight = value
            End Set
        End Property

        Private _PalletTypeWidth As Double = 0
        <DataMember()> _
        Public Property PalletTypeWidth As Double
            Get
                Return _PalletTypeWidth
            End Get
            Set(value As Double)
                _PalletTypeWidth = value
            End Set
        End Property

        Private _PalletTypeDepth As Double = 0
        <DataMember()> _
        Public Property PalletTypeDepth As Double
            Get
                Return _PalletTypeDepth
            End Get
            Set(value As Double)
                _PalletTypeDepth = value
            End Set
        End Property

        Private _PalletTypeVolume As Double = 0
        <DataMember()> _
        Public Property PalletTypeVolume As Double
            Get
                Return _PalletTypeVolume
            End Get
            Set(value As Double)
                _PalletTypeVolume = value
            End Set
        End Property

        Private _PalletTypeContainer As Boolean = False
        <DataMember()> _
        Public Property PalletTypeContainer As Boolean
            Get
                Return _PalletTypeContainer
            End Get
            Set(value As Boolean)
                _PalletTypeContainer = value
            End Set
        End Property

        Private _PalletTypeUpdated As Byte()
        <DataMember()> _
        Public Property PalletTypeUpdated() As Byte()
            Get
                Return _PalletTypeUpdated
            End Get
            Set(ByVal value As Byte())
                _PalletTypeUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New PalletType
            instance = DirectCast(MemberwiseClone(), PalletType)
            Return instance
        End Function

#End Region

    End Class
End Namespace
