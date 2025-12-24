Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class TempType
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CommCodeType As String = ""
        <DataMember()> _
        Public Property CommCodeType() As String
            Get
                Return Left(_CommCodeType, 1)
            End Get
            Set(ByVal value As String)
                _CommCodeType = Left(value, 1)
            End Set
        End Property

        Private _CommCodeDescription As String = ""
        <DataMember()> _
        Public Property CommCodeDescription() As String
            Get
                Return Left(_CommCodeDescription, 40)
            End Get
            Set(ByVal value As String)
                _CommCodeDescription = Left(value, 40)
            End Set
        End Property

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

        Private _TempType As String = ""
        <DataMember()> _
        Public Property TempType() As String
            Get
                Return Left(_TempType, 50)
            End Get
            Set(ByVal value As String)
                _TempType = Left(value, 50)
            End Set
        End Property

        Private _TariffTempType As Integer = 0
        <DataMember()> _
        Public Property TariffTempType() As Integer
            Get
                Return _TariffTempType
            End Get
            Set(ByVal value As Integer)
                _TariffTempType = value
            End Set
        End Property

        Private _TempTypeUpdated As Byte()
        <DataMember()> _
        Public Property TempTypeUpdated() As Byte()
            Get
                Return _TempTypeUpdated
            End Get
            Set(ByVal value As Byte())
                _TempTypeUpdated = value
            End Set
        End Property

        'Added by LVV 5/4/16 for v-7.0.5.1 DAT
        Private _DATEquipTypeControl As Integer = 0
        <DataMember()> _
        Public Property DATEquipTypeControl() As Integer
            Get
                Return _DATEquipTypeControl
            End Get
            Set(ByVal value As Integer)
                _DATEquipTypeControl = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New TempType
            instance = DirectCast(MemberwiseClone(), TempType)
            Return instance
        End Function

#End Region

    End Class
End Namespace
