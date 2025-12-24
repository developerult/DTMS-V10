Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LaneOptimizationSDFSettings
        Inherits DTOBaseClass


#Region " Data Members"
        Private _LaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneControl() As Integer
            Get
                Return _LaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneControl = value
            End Set
        End Property

        Private _LaneNumber As String = ""
        <DataMember()> _
        Public Property LaneNumber() As String
            Get
                Return Left(_LaneNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneNumber = Left(value, 50)
            End Set
        End Property

        Private _LaneName As String = ""
        <DataMember()> _
        Public Property LaneName() As String
            Get
                Return Left(_LaneName, 50)
            End Get
            Set(ByVal value As String)
                _LaneName = Left(value, 50)
            End Set
        End Property

        Private _LaneNumberMaster As String = ""
        <DataMember()> _
        Public Property LaneNumberMaster() As String
            Get
                Return Left(_LaneNumberMaster, 50)
            End Get
            Set(ByVal value As String)
                _LaneNumberMaster = Left(value, 50)
            End Set
        End Property

        Private _LaneNameMaster As String = ""
        <DataMember()> _
        Public Property LaneNameMaster() As String
            Get
                Return Left(_LaneNameMaster, 50)
            End Get
            Set(ByVal value As String)
                _LaneNameMaster = Left(value, 50)
            End Set
        End Property

        Private _LaneCompControl As Integer = 0
        <DataMember()> _
        Public Property LaneCompControl() As Integer
            Get
                Return _LaneCompControl
            End Get
            Set(ByVal value As Integer)
                _LaneCompControl = value
            End Set
        End Property



        Private _LaneSDFUse As Boolean = False
        <DataMember()> _
        Public Property LaneSDFUse() As Boolean
            Get
                Return _LaneSDFUse
            End Get
            Set(ByVal value As Boolean)
                _LaneSDFUse = value
            End Set
        End Property

        Private _LaneSDFSRZone As Integer = 0
        <DataMember()> _
        Public Property LaneSDFSRZone() As Integer
            Get
                Return _LaneSDFSRZone
            End Get
            Set(ByVal value As Integer)
                _LaneSDFSRZone = value
            End Set
        End Property

        Private _LaneSDFMRZone As Integer = 0
        <DataMember()> _
        Public Property LaneSDFMRZone() As Integer
            Get
                Return _LaneSDFMRZone
            End Get
            Set(ByVal value As Integer)
                _LaneSDFMRZone = value
            End Set
        End Property

        Private _LaneSDFFixedTime As Integer = 0
        <DataMember()> _
        Public Property LaneSDFFixedTime() As Integer
            Get
                Return _LaneSDFFixedTime
            End Get
            Set(ByVal value As Integer)
                _LaneSDFFixedTime = value
            End Set
        End Property

        Private _LaneSDFEarlyTM1 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM1() As Integer
            Get
                Return _LaneSDFEarlyTM1
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM1 = value
            End Set
        End Property

        Private _LaneSDFLateTM1 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM1() As Integer
            Get
                Return _LaneSDFLateTM1
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM1 = value
            End Set
        End Property

        Private _LaneSDFDay1 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay1() As String
            Get
                Return Left(_LaneSDFDay1, 50)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay1 = Left(value, 50)
            End Set
        End Property

        Private _LaneSDFEarlyTM2 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM2() As Integer
            Get
                Return _LaneSDFEarlyTM2
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM2 = value
            End Set
        End Property

        Private _LaneSDFLateTM2 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM2() As Integer
            Get
                Return _LaneSDFLateTM2
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM2 = value
            End Set
        End Property

        Private _LaneSDFDay2 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay2() As String
            Get
                Return Left(_LaneSDFDay2, 10)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay2 = Left(value, 10)
            End Set
        End Property

        Private _LaneSDFEarlyTM3 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM3() As Integer
            Get
                Return _LaneSDFEarlyTM3
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM3 = value
            End Set
        End Property

        Private _LaneSDFLateTM3 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM3() As Integer
            Get
                Return _LaneSDFLateTM3
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM3 = value
            End Set
        End Property

        Private _LaneSDFDay3 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay3() As String
            Get
                Return Left(_LaneSDFDay3, 11)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay3 = Left(value, 11)
            End Set
        End Property

        Private _LaneSDFEarlyTM4 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM4() As Integer
            Get
                Return _LaneSDFEarlyTM4
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM4 = value
            End Set
        End Property

        Private _LaneSDFLateTM4 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM4() As Integer
            Get
                Return _LaneSDFLateTM4
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM4 = value
            End Set
        End Property

        Private _LaneSDFDay4 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay4() As String
            Get
                Return Left(_LaneSDFDay4, 10)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay4 = Left(value, 10)
            End Set
        End Property

        Private _LaneSDFEarlyTM5 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM5() As Integer
            Get
                Return _LaneSDFEarlyTM5
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM5 = value
            End Set
        End Property

        Private _LaneSDFLateTM5 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM5() As Integer
            Get
                Return _LaneSDFLateTM5
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM5 = value
            End Set
        End Property

        Private _LaneSDFDay5 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay5() As String
            Get
                Return Left(_LaneSDFDay5, 10)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay5 = Left(value, 10)
            End Set
        End Property

        Private _LaneSDFEarlyTM6 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM6() As Integer
            Get
                Return _LaneSDFEarlyTM6
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM6 = value
            End Set
        End Property

        Private _LaneSDFLateTM6 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM6() As Integer
            Get
                Return _LaneSDFLateTM6
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM6 = value
            End Set
        End Property

        Private _LaneSDFDay6 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay6() As String
            Get
                Return Left(_LaneSDFDay6, 10)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay6 = Left(value, 10)
            End Set
        End Property

        Private _LaneSDFEarlyTM7 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM7() As Integer
            Get
                Return _LaneSDFEarlyTM7
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM7 = value
            End Set
        End Property

        Private _LaneSDFLateTM7 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM7() As Integer
            Get
                Return _LaneSDFLateTM7
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM7 = value
            End Set
        End Property

        Private _LaneSDFDay7 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay7() As String
            Get
                Return Left(_LaneSDFDay7, 10)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay7 = Left(value, 10)
            End Set
        End Property

        Private _LaneSDFUnldRate1 As Decimal = 0
        <DataMember()> _
        Public Property LaneSDFUnldRate1() As Decimal
            Get
                Return _LaneSDFUnldRate1
            End Get
            Set(ByVal value As Decimal)
                _LaneSDFUnldRate1 = value
            End Set
        End Property

        Private _LaneSDFUnldRate2 As Decimal = 0
        <DataMember()> _
        Public Property LaneSDFUnldRate2() As Decimal
            Get
                Return _LaneSDFUnldRate2
            End Get
            Set(ByVal value As Decimal)
                _LaneSDFUnldRate2 = value
            End Set
        End Property

        Private _LaneSDFUnldRate3 As Decimal = 0
        <DataMember()> _
        Public Property LaneSDFUnldRate3() As Decimal
            Get
                Return _LaneSDFUnldRate3
            End Get
            Set(ByVal value As Decimal)
                _LaneSDFUnldRate3 = value
            End Set
        End Property

        Private _LaneSDFUnldRate4 As Decimal = 0
        <DataMember()> _
        Public Property LaneSDFUnldRate4() As Decimal
            Get
                Return _LaneSDFUnldRate4
            End Get
            Set(ByVal value As Decimal)
                _LaneSDFUnldRate4 = value
            End Set
        End Property

        Private _LaneSDFUnldRate5 As Decimal = 0
        <DataMember()> _
        Public Property LaneSDFUnldRate5() As Decimal
            Get
                Return _LaneSDFUnldRate5
            End Get
            Set(ByVal value As Decimal)
                _LaneSDFUnldRate5 = value
            End Set
        End Property
#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LaneOptimizationSDFSettings
            instance = DirectCast(MemberwiseClone(), LaneOptimizationSDFSettings)
            Return instance
        End Function

#End Region

    End Class
End Namespace
