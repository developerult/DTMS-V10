Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV on 11/2/16 for v-7.0.5.110 Lane Default Carrier Enhancements 

Namespace DataTransferObjects
    <DataContract()> _
    Public Class PreferredCarrier
        Inherits DTOBaseClass


#Region " Data Members"

        Private _blnHasCompRestrictions As Boolean = False
        <DataMember()> _
        Public Property blnHasCompRestrictions() As Boolean
            Get
                Return _blnHasCompRestrictions
            End Get
            Set(ByVal value As Boolean)
                _blnHasCompRestrictions = value
            End Set
        End Property

        Private _LLTCControl As Integer = 0
        <DataMember()> _
        Public Property LLTCControl() As Integer
            Get
                Return _LLTCControl
            End Get
            Set(ByVal value As Integer)
                _LLTCControl = value
            End Set
        End Property

        Private _LLTCCarrierControl As Integer = 0
        <DataMember()> _
        Public Property LLTCCarrierControl() As Integer
            Get
                Return _LLTCCarrierControl
            End Get
            Set(ByVal value As Integer)
                _LLTCCarrierControl = value
            End Set
        End Property

        Private _LLTCCarrierNumber As Integer = 0
        <DataMember()> _
        Public Property LLTCCarrierNumber() As Integer
            Get
                Return _LLTCCarrierNumber
            End Get
            Set(ByVal value As Integer)
                _LLTCCarrierNumber = value
            End Set
        End Property

        Private _LLTCCarrierName As String = ""
        <DataMember()> _
        Public Property LLTCCarrierName() As String
            Get
                Return Left(_LLTCCarrierName, 40)
            End Get
            Set(ByVal value As String)
                _LLTCCarrierName = Left(value, 40)
            End Set
        End Property

        Private _LLTCLaneControl As Integer = 0
        <DataMember()> _
        Public Property LLTCLaneControl() As Integer
            Get
                Return _LLTCLaneControl
            End Get
            Set(ByVal value As Integer)
                _LLTCLaneControl = value
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

        Private _CompName As String = ""
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return Left(_CompName, 40)
            End Get
            Set(ByVal value As String)
                _CompName = Left(value, 40)
            End Set
        End Property

        Private _LLTCModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property LLTCModeTypeControl() As Integer
            Get
                Return _LLTCModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _LLTCModeTypeControl = value
            End Set
        End Property

        Private _LLTCSequenceNumber As Integer = 0
        <DataMember()> _
        Public Property LLTCSequenceNumber() As Integer
            Get
                Return _LLTCSequenceNumber
            End Get
            Set(ByVal value As Integer)
                _LLTCSequenceNumber = value
            End Set
        End Property

        Private _LLTCSActive As Boolean = True
        <DataMember()> _
        Public Property LLTCSActive() As Boolean
            Get
                Return _LLTCSActive
            End Get
            Set(ByVal value As Boolean)
                _LLTCSActive = value
            End Set
        End Property

        Private _LLTCTempType As Integer = 0
        <DataMember()> _
        Public Property LLTCTempType() As Integer
            Get
                Return _LLTCTempType
            End Get
            Set(ByVal value As Integer)
                _LLTCTempType = value
            End Set
        End Property

        Private _LLTCMaxCases As Integer = 0
        <DataMember()> _
        Public Property LLTCMaxCases() As Integer
            Get
                Return _LLTCMaxCases
            End Get
            Set(ByVal value As Integer)
                _LLTCMaxCases = value
            End Set
        End Property

        Private _LLTCMaxWgt As Double = 0
        <DataMember()> _
        Public Property LLTCMaxWgt() As Double
            Get
                Return _LLTCMaxWgt
            End Get
            Set(ByVal value As Double)
                _LLTCMaxWgt = value
            End Set
        End Property

        Private _LLTCMaxCube As Integer = 0
        <DataMember()> _
        Public Property LLTCMaxCube() As Integer
            Get
                Return _LLTCMaxCube
            End Get
            Set(ByVal value As Integer)
                _LLTCMaxCube = value
            End Set
        End Property

        Private _LLTCMaxPL As Double = 0
        <DataMember()> _
        Public Property LLTCMaxPL() As Double
            Get
                Return _LLTCMaxPL
            End Get
            Set(ByVal value As Double)
                _LLTCMaxPL = value
            End Set
        End Property

        Private _LLTCTariffControl As Integer = 0
        <DataMember()> _
        Public Property LLTCTariffControl() As Integer
            Get
                Return _LLTCTariffControl
            End Get
            Set(ByVal value As Integer)
                _LLTCTariffControl = value
            End Set
        End Property

        Private _LLTCTariffName As String = ""
        <DataMember()> _
        Public Property LLTCTariffName() As String
            Get
                Return Left(_LLTCTariffName, 50)
            End Get
            Set(ByVal value As String)
                _LLTCTariffName = Left(value, 50)
            End Set
        End Property

        Private _LLTCTariffEquip As String = ""
        <DataMember()> _
        Public Property LLTCTariffEquip() As String
            Get
                Return Left(_LLTCTariffEquip, 50)
            End Get
            Set(ByVal value As String)
                _LLTCTariffEquip = Left(value, 50)
            End Set
        End Property

        Private _LLTCMinAllowedCost As Decimal = 0
        <DataMember()> _
        Public Property LLTCMinAllowedCost() As Decimal
            Get
                Return _LLTCMinAllowedCost
            End Get
            Set(ByVal value As Decimal)
                _LLTCMinAllowedCost = value
            End Set
        End Property

        Private _LLTCMaxAllowedCost As Decimal = 0
        <DataMember()> _
        Public Property LLTCMaxAllowedCost() As Decimal
            Get
                Return _LLTCMaxAllowedCost
            End Get
            Set(ByVal value As Decimal)
                _LLTCMaxAllowedCost = value
            End Set
        End Property

        Private _LLTCAllowAutoAssignment As Boolean = True
        <DataMember()> _
        Public Property LLTCAllowAutoAssignment() As Boolean
            Get
                Return _LLTCAllowAutoAssignment
            End Get
            Set(ByVal value As Boolean)
                _LLTCAllowAutoAssignment = value
            End Set
        End Property

        Private _LLTCIgnoreTariff As Boolean = False
        <DataMember()> _
        Public Property LLTCIgnoreTariff() As Boolean
            Get
                Return _LLTCIgnoreTariff
            End Get
            Set(ByVal value As Boolean)
                _LLTCIgnoreTariff = value
            End Set
        End Property

        Private _RetMsg As String = ""
        <DataMember()> _
        Public Property RetMsg() As String
            Get
                Return Left(_RetMsg, 4000)
            End Get
            Set(ByVal value As String)
                _RetMsg = Left(value, 4000)
            End Set
        End Property

        Private _ErrNumber As Integer = 0
        <DataMember()> _
        Public Property ErrNumber() As Integer
            Get
                Return _ErrNumber
            End Get
            Set(ByVal value As Integer)
                _ErrNumber = value
            End Set
        End Property



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New PreferredCarrier
            instance = DirectCast(MemberwiseClone(), PreferredCarrier)
            Return instance
        End Function

#End Region

    End Class
End Namespace


