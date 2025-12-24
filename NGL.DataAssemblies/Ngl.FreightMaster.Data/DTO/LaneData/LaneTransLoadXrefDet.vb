Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LaneTransLoadXrefDet
        Inherits DTOBaseClass


#Region " Data Members"
        Private _LaneTranXDetControl As Integer = 0
        <DataMember()> _
        Public Property LaneTranXDetControl() As Integer
            Get
                Return _LaneTranXDetControl
            End Get
            Set(ByVal value As Integer)
                _LaneTranXDetControl = value
            End Set
        End Property

        Private _LaneTranXDetName As String = ""
        <DataMember()> _
        Public Property LaneTranXDetName() As String
            Get
                Return Left(_LaneTranXDetName, 100)
            End Get
            Set(ByVal value As String)
                _LaneTranXDetName = Left(value, 100)
            End Set
        End Property

        Private _LaneTranXDetLaneTranXControl As Integer = 0
        <DataMember()> _
        Public Property LaneTranXDetLaneTranXControl() As Integer
            Get
                Return _LaneTranXDetLaneTranXControl
            End Get
            Set(ByVal value As Integer)
                _LaneTranXDetLaneTranXControl = value
            End Set
        End Property

        Private _LaneTranXDetLaneName As String = ""
        <DataMember()> _
        Public Property LaneTranXDetLaneName() As String
            Get
                Return _LaneTranXDetLaneName
            End Get
            Set(ByVal value As String)
                _LaneTranXDetLaneName = value
            End Set
        End Property

        Private _LaneTranXDetLaneNumber As String = ""
        <DataMember()> _
        Public Property LaneTranXDetLaneNumber() As String
            Get
                Return _LaneTranXDetLaneNumber
            End Get
            Set(ByVal value As String)
                _LaneTranXDetLaneNumber = value
            End Set
        End Property

        Private _LaneTranXDetLaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneTranXDetLaneControl() As Integer
            Get
                Return _LaneTranXDetLaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneTranXDetLaneControl = value
            End Set
        End Property

        Private _LaneTranXDetSequence As Integer = 0
        <DataMember()> _
        Public Property LaneTranXDetSequence() As Integer
            Get
                Return _LaneTranXDetSequence
            End Get
            Set(ByVal value As Integer)
                _LaneTranXDetSequence = value
            End Set
        End Property

        Private _LaneTranXDetCarrierName As String = ""
        <DataMember()> _
        Public Property LaneTranXDetCarrierName() As String
            Get
                Return _LaneTranXDetCarrierName
            End Get
            Set(ByVal value As String)
                _LaneTranXDetCarrierName = value
            End Set
        End Property

        Private _LaneTranXDetCarrierNumber As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property LaneTranXDetCarrierNumber() As System.Nullable(Of Integer)
            Get
                Return _LaneTranXDetCarrierNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _LaneTranXDetCarrierNumber = value
            End Set
        End Property

        Private _LaneTranXDetCarrierControl As Integer = 0
        <DataMember()> _
        Public Property LaneTranXDetCarrierControl() As Integer
            Get
                Return _LaneTranXDetCarrierControl
            End Get
            Set(ByVal value As Integer)
                _LaneTranXDetCarrierControl = value
            End Set
        End Property

        Private _LaneTranXDetContInfo As String = ""
        <DataMember()> _
        Public Property LaneTranXDetContInfo() As String
            Get
                If String.IsNullOrEmpty(_LaneTranXDetContInfo) OrElse _LaneTranXDetContInfo.Trim.Length < 1 Then
                    _LaneTranXDetContInfo = getInfo()
                End If
                Return _LaneTranXDetContInfo
            End Get
            Set(ByVal value As String)
                _LaneTranXDetContInfo = value
            End Set
        End Property

        Private _LaneTranXDetContName As String = ""
        <DataMember()> _
        Public Property LaneTranXDetContName() As String
            Get
                Return _LaneTranXDetContName
            End Get
            Set(ByVal value As String)
                _LaneTranXDetContName = value
            End Set
        End Property

        Private _LaneTranXDetContPhone As String = ""
        <DataMember()> _
        Public Property LaneTranXDetContPhone() As String
            Get
                Return _LaneTranXDetContPhone
            End Get
            Set(ByVal value As String)
                _LaneTranXDetContPhone = value
            End Set
        End Property

        Private _LaneTranXDetContExt As String = ""
        <DataMember()> _
        Public Property LaneTranXDetContExt() As String
            Get
                Return _LaneTranXDetContExt
            End Get
            Set(ByVal value As String)
                _LaneTranXDetContExt = value
            End Set
        End Property

        Private _LaneTranXDetCont800 As String = ""
        <DataMember()> _
        Public Property LaneTranXDetCont800() As String
            Get
                Return _LaneTranXDetCont800
            End Get
            Set(ByVal value As String)
                _LaneTranXDetCont800 = value
            End Set
        End Property

        Private _LaneTranXDetCarrierContControl As Integer = 0
        <DataMember()> _
        Public Property LaneTranXDetCarrierContControl() As Integer
            Get
                Return _LaneTranXDetCarrierContControl
            End Get
            Set(ByVal value As Integer)
                _LaneTranXDetCarrierContControl = value
            End Set
        End Property

        Private _LaneTranXDetModeTypeName As String = ""
        <DataMember()> _
        Public Property LaneTranXDetModeTypeName() As String
            Get
                Return _LaneTranXDetModeTypeName
            End Get
            Set(ByVal value As String)
                _LaneTranXDetModeTypeName = value
            End Set
        End Property

        Private _LaneTranXDetModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property LaneTranXDetModeTypeControl() As Integer
            Get
                Return _LaneTranXDetModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _LaneTranXDetModeTypeControl = value
            End Set
        End Property

        Private _LaneTranXDetCarrTarName As String = ""
        <DataMember()> _
        Public Property LaneTranXDetCarrTarName() As String
            Get
                Return _LaneTranXDetCarrTarName
            End Get
            Set(ByVal value As String)
                _LaneTranXDetCarrTarName = value
            End Set
        End Property

        Private _LaneTranXDetCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property LaneTranXDetCarrTarControl() As Integer
            Get
                Return _LaneTranXDetCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _LaneTranXDetCarrTarControl = value
            End Set
        End Property

        Private _LaneTranXDetCarrTarEquipName As String = ""
        <DataMember()> _
        Public Property LaneTranXDetCarrTarEquipName() As String
            Get
                Return _LaneTranXDetCarrTarEquipName
            End Get
            Set(ByVal value As String)
                _LaneTranXDetCarrTarEquipName = value
            End Set
        End Property

        Private _LaneTranXDetCarrTarEquipControl As Integer = 0
        <DataMember()> _
        Public Property LaneTranXDetCarrTarEquipControl() As Integer
            Get
                Return _LaneTranXDetCarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                _LaneTranXDetCarrTarEquipControl = value
            End Set
        End Property

        Private _LaneTranXDetRule11Required As Boolean = False
        <DataMember()> _
        Public Property LaneTranXDetRule11Required() As Boolean
            Get
                Return _LaneTranXDetRule11Required
            End Get
            Set(ByVal value As Boolean)
                _LaneTranXDetRule11Required = value
            End Set
        End Property

        Private _LaneTranXDetConsolidateSplits As Boolean = False
        <DataMember()> _
        Public Property LaneTranXDetConsolidateSplits() As Boolean
            Get
                Return _LaneTranXDetConsolidateSplits
            End Get
            Set(ByVal value As Boolean)
                _LaneTranXDetConsolidateSplits = value
            End Set
        End Property

        Private _LaneTranXDetBilledSeperately As Boolean = False
        <DataMember()> _
        Public Property LaneTranXDetBilledSeperately() As Boolean
            Get
                Return _LaneTranXDetBilledSeperately
            End Get
            Set(ByVal value As Boolean)
                _LaneTranXDetBilledSeperately = value
            End Set
        End Property

        Private _LaneTranXDetTransTypeName As String = ""
        <DataMember()> _
        Public Property LaneTranXDetTransTypeName() As String
            Get
                Return _LaneTranXDetTransTypeName
            End Get
            Set(ByVal value As String)
                _LaneTranXDetTransTypeName = value
            End Set
        End Property

        Private _LaneTranXDetTransType As System.Nullable(Of Integer) = 0
        <DataMember()> _
        Public Property LaneTranXDetTransType() As System.Nullable(Of Integer)
            Get
                Return If(_LaneTranXDetTransType, 0)
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _LaneTranXDetTransType = If(value, 0)
            End Set
        End Property

        Private _LaneTranXDetBFC As Double = 0
        <DataMember()> _
        Public Property LaneTranXDetBFC() As Double
            Get
                Return _LaneTranXDetBFC
            End Get
            Set(ByVal value As Double)
                _LaneTranXDetBFC = value
            End Set
        End Property

        Private _LaneTranXDetBFCType As String = ""
        <DataMember()> _
        Public Property LaneTranXDetBFCType() As String
            Get
                Return Left(_LaneTranXDetBFCType, 100)
            End Get
            Set(ByVal value As String)
                _LaneTranXDetBFCType = Left(value, 100)
            End Set
        End Property

        Private _LaneTranXDetBenchMiles As Double = 0
        <DataMember()> _
        Public Property LaneTranXDetBenchMiles() As Double
            Get
                Return _LaneTranXDetBenchMiles
            End Get
            Set(ByVal value As Double)
                _LaneTranXDetBenchMiles = value
            End Set
        End Property

        Private _LaneTranXDetUser1 As String = ""
        <DataMember()> _
        Public Property LaneTranXDetUser1() As String
            Get
                Return Left(_LaneTranXDetUser1, 4000)
            End Get
            Set(ByVal value As String)
                _LaneTranXDetUser1 = Left(value, 4000)
            End Set
        End Property

        Private _LaneTranXDetUser2 As String = ""
        <DataMember()> _
        Public Property LaneTranXDetUser2() As String
            Get
                Return Left(_LaneTranXDetUser2, 4000)
            End Get
            Set(ByVal value As String)
                _LaneTranXDetUser2 = Left(value, 4000)
            End Set
        End Property

        Private _LaneTranXDetUser3 As String = ""
        <DataMember()> _
        Public Property LaneTranXDetUser3() As String
            Get
                Return Left(_LaneTranXDetUser3, 4000)
            End Get
            Set(ByVal value As String)
                _LaneTranXDetUser3 = Left(value, 4000)
            End Set
        End Property

        Private _LaneTranXDetUser4 As String = ""
        <DataMember()> _
        Public Property LaneTranXDetUser4() As String
            Get
                Return Left(_LaneTranXDetUser4, 4000)
            End Get
            Set(ByVal value As String)
                _LaneTranXDetUser4 = Left(value, 4000)
            End Set
        End Property

        Private _LaneTranXDetTransitHours As Integer = 48
        <DataMember()> _
        Public Property LaneTranXDetTransitHours() As Integer
            Get
                Return _LaneTranXDetTransitHours
            End Get
            Set(ByVal value As Integer)
                _LaneTranXDetTransitHours = value
            End Set
        End Property

        Private _LaneTranXDetTransferHours As Integer = 48
        <DataMember()> _
        Public Property LaneTranXDetTransferHours() As Integer
            Get
                Return _LaneTranXDetTransferHours
            End Get
            Set(ByVal value As Integer)
                _LaneTranXDetTransferHours = value
            End Set
        End Property

        Private _LaneTranXDetModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneTranXDetModDate() As System.Nullable(Of Date)
            Get
                Return _LaneTranXDetModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneTranXDetModDate = value
            End Set
        End Property

        Private _LaneTranXDetModUser As String = ""
        <DataMember()> _
        Public Property LaneTranXDetModUser() As String
            Get
                Return Left(_LaneTranXDetModUser, 100)
            End Get
            Set(ByVal value As String)
                _LaneTranXDetModUser = Left(value, 100)
            End Set
        End Property

        Private _LaneTranXDetUpdated As Byte()
        <DataMember()> _
        Public Property LaneTranXDetUpdated() As Byte()
            Get
                Return _LaneTranXDetUpdated
            End Get
            Set(ByVal value As Byte())
                _LaneTranXDetUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LaneTransLoadXrefDet
            instance = DirectCast(MemberwiseClone(), LaneTransLoadXrefDet)
            Return instance
        End Function

        Public Function getInfo() As String
            Return LaneTranXDetContName & " " & getPhoneInfo()
        End Function

        Public Function getPhoneInfo() As String
            If String.IsNullOrEmpty(LaneTranXDetCont800) OrElse LaneTranXDetCont800.Trim.Length < 1 Then
                Return LaneTranXDetContPhone & " " & LaneTranXDetContExt
            Else
                Return LaneTranXDetCont800
            End If
        End Function

#End Region


    End Class
End Namespace