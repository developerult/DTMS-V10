Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LaneTransLoadXref
        Inherits DTOBaseClass


#Region " Data Members"
        Private _LaneTranXControl As Integer = 0
        <DataMember()> _
        Public Property LaneTranXControl() As Integer
            Get
                Return _LaneTranXControl
            End Get
            Set(ByVal value As Integer)
                _LaneTranXControl = value
            End Set
        End Property

        Private _LaneTranXName As String = ""
        <DataMember()> _
        Public Property LaneTranXName() As String
            Get
                Return Left(_LaneTranXName, 100)
            End Get
            Set(ByVal value As String)
                _LaneTranXName = Left(value, 100)
            End Set
        End Property

        Private _LaneTranXLaneName As String = ""
        <DataMember()> _
        Public Property LaneTranXLaneName() As String
            Get
                Return _LaneTranXLaneName
            End Get
            Set(ByVal value As String)
                _LaneTranXLaneName = value
            End Set
        End Property

        Private _LaneTranXLaneNumber As String = ""
        <DataMember()> _
        Public Property LaneTranXLaneNumber() As String
            Get
                Return _LaneTranXLaneNumber
            End Get
            Set(ByVal value As String)
                _LaneTranXLaneNumber = value
            End Set
        End Property

        Private _LaneTranXLaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneTranXLaneControl() As Integer
            Get
                Return _LaneTranXLaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneTranXLaneControl = value
            End Set
        End Property

        Private _LaneTranXModeTypeName As String = ""
        <DataMember()> _
        Public Property LaneTranXModeTypeName() As String
            Get
                Return _LaneTranXModeTypeName
            End Get
            Set(ByVal value As String)
                _LaneTranXModeTypeName = value
            End Set
        End Property

        Private _LaneTranXModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property LaneTranXModeTypeControl() As Integer
            Get
                Return _LaneTranXModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _LaneTranXModeTypeControl = value
            End Set
        End Property

        Private _LaneTranXSequence As Integer = 0
        <DataMember()> _
        Public Property LaneTranXSequence() As Integer
            Get
                Return _LaneTranXSequence
            End Get
            Set(ByVal value As Integer)
                _LaneTranXSequence = value
            End Set
        End Property

        Private _LaneTranXTempTypeName As String = ""
        <DataMember()> _
        Public Property LaneTranXTempTypeName() As String
            Get
                Return _LaneTranXTempTypeName
            End Get
            Set(ByVal value As String)
                _LaneTranXTempTypeName = value
            End Set
        End Property

        Private _LaneTranXTempType As String = ""
        <DataMember()> _
        Public Property LaneTranXTempType() As String
            Get
                Return Left(_LaneTranXTempType, 50)
            End Get
            Set(ByVal value As String)
                _LaneTranXTempType = Left(value, 50)
            End Set
        End Property

        Private _LaneTranXMinCases As Integer = 0
        <DataMember()> _
        Public Property LaneTranXMinCases() As Integer
            Get
                Return _LaneTranXMinCases
            End Get
            Set(ByVal value As Integer)
                _LaneTranXMinCases = value
            End Set
        End Property

        Private _LaneTranXMinWgt As Double = 0
        <DataMember()> _
        Public Property LaneTranXMinWgt() As Double
            Get
                Return _LaneTranXMinWgt
            End Get
            Set(ByVal value As Double)
                _LaneTranXMinWgt = value
            End Set
        End Property

        Private _LaneTranXMinCube As Integer = 0
        <DataMember()> _
        Public Property LaneTranXMinCube() As Integer
            Get
                Return _LaneTranXMinCube
            End Get
            Set(ByVal value As Integer)
                _LaneTranXMinCube = value
            End Set
        End Property

        Private _LaneTranXMinPL As Double = 0
        <DataMember()> _
        Public Property LaneTranXMinPL() As Double
            Get
                Return _LaneTranXMinPL
            End Get
            Set(ByVal value As Double)
                _LaneTranXMinPL = value
            End Set
        End Property

        Private _LaneTranXMaxCases As Integer = 0
        <DataMember()> _
        Public Property LaneTranXMaxCases() As Integer
            Get
                Return _LaneTranXMaxCases
            End Get
            Set(ByVal value As Integer)
                _LaneTranXMaxCases = value
            End Set
        End Property

        Private _LaneTranXMaxWgt As Double = 0
        <DataMember()> _
        Public Property LaneTranXMaxWgt() As Double
            Get
                Return _LaneTranXMaxWgt
            End Get
            Set(ByVal value As Double)
                _LaneTranXMaxWgt = value
            End Set
        End Property

        Private _LaneTranXMaxCube As Integer = 0
        <DataMember()> _
        Public Property LaneTranXMaxCube() As Integer
            Get
                Return _LaneTranXMaxCube
            End Get
            Set(ByVal value As Integer)
                _LaneTranXMaxCube = value
            End Set
        End Property

        Private _LaneTranXMaxPL As Double = 0
        <DataMember()> _
        Public Property LaneTranXMaxPL() As Double
            Get
                Return _LaneTranXMaxPL
            End Get
            Set(ByVal value As Double)
                _LaneTranXMaxPL = value
            End Set
        End Property

        Private _LaneTranXBenchMiles As Double = 0
        <DataMember()> _
        Public Property LaneTranXBenchMiles() As Double
            Get
                Return _LaneTranXBenchMiles
            End Get
            Set(ByVal value As Double)
                _LaneTranXBenchMiles = value
            End Set
        End Property

        Private _LaneTranXUser1 As String = ""
        <DataMember()> _
        Public Property LaneTranXUser1() As String
            Get
                Return Left(_LaneTranXUser1, 4000)
            End Get
            Set(ByVal value As String)
                _LaneTranXUser1 = Left(value, 4000)
            End Set
        End Property

        Private _LaneTranXUser2 As String = ""
        <DataMember()> _
        Public Property LaneTranXUser2() As String
            Get
                Return Left(_LaneTranXUser2, 4000)
            End Get
            Set(ByVal value As String)
                _LaneTranXUser2 = Left(value, 4000)
            End Set
        End Property

        Private _LaneTranXUser3 As String = ""
        <DataMember()> _
        Public Property LaneTranXUser3() As String
            Get
                Return Left(_LaneTranXUser3, 4000)
            End Get
            Set(ByVal value As String)
                _LaneTranXUser3 = Left(value, 4000)
            End Set
        End Property

        Private _LaneTranXUser4 As String = ""
        <DataMember()> _
        Public Property LaneTranXUser4() As String
            Get
                Return Left(_LaneTranXUser4, 4000)
            End Get
            Set(ByVal value As String)
                _LaneTranXUser4 = Left(value, 4000)
            End Set
        End Property

        Private _LaneTranXModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneTranXModDate() As System.Nullable(Of Date)
            Get
                Return _LaneTranXModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneTranXModDate = value
            End Set
        End Property

        Private _LaneTranXModUser As String = ""
        <DataMember()> _
        Public Property LaneTranXModUser() As String
            Get
                Return Left(_LaneTranXModUser, 100)
            End Get
            Set(ByVal value As String)
                _LaneTranXModUser = Left(value, 100)
            End Set
        End Property

        Private _LaneTranXUpdated As Byte()
        <DataMember()> _
        Public Property LaneTranXUpdated() As Byte()
            Get
                Return _LaneTranXUpdated
            End Get
            Set(ByVal value As Byte())
                _LaneTranXUpdated = value
            End Set
        End Property

        Private _LaneTransLoadXrefDets As List(Of LaneTransLoadXrefDet)
        <DataMember()> _
        Public Property LaneTransLoadXrefDets() As List(Of LaneTransLoadXrefDet)
            Get
                Return _LaneTransLoadXrefDets
            End Get
            Set(ByVal value As List(Of LaneTransLoadXrefDet))
                _LaneTransLoadXrefDets = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LaneTransLoadXref
            instance = DirectCast(MemberwiseClone(), LaneTransLoadXref)
            instance.LaneTransLoadXrefDets = Nothing
            For Each item In LaneTransLoadXrefDets
                instance.LaneTransLoadXrefDets.Add(DirectCast(item.Clone, LaneTransLoadXrefDet))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace