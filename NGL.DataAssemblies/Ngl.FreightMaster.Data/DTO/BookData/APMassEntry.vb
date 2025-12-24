Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class APMassEntry
        Inherits DTOBaseClass


#Region " Data Members"

        Private _APControl As Integer = 0
        <DataMember()> _
        Public Property APControl() As Integer
            Get
                Return _APControl
            End Get
            Set(ByVal value As Integer)
                _APControl = value
                NotifyPropertyChanged("APControl")
            End Set
        End Property

        Private _APCarrierNumber As Integer = 0
        <DataMember()> _
        Public Property APCarrierNumber() As Integer
            Get
                Return _APCarrierNumber
            End Get
            Set(ByVal value As Integer)
                _APCarrierNumber = value
                NotifyPropertyChanged("APCarrierNumber")
            End Set
        End Property

        Private _APBillNumber As String = ""
        <DataMember()> _
        Public Property APBillNumber() As String
            Get
                Return Left(_APBillNumber, 50)
            End Get
            Set(ByVal value As String)
                _APBillNumber = Left(value, 50)
                NotifyPropertyChanged("APBillNumber")
            End Set
        End Property

        Private _APBillDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property APBillDate() As System.Nullable(Of Date)
            Get
                Return _APBillDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _APBillDate = value
                NotifyPropertyChanged("APBillDate")
            End Set
        End Property

        Private _APPONumber As String = ""
        <DataMember()> _
        Public Property APPONumber() As String
            Get
                Return Left(_APPONumber, 20)
            End Get
            Set(ByVal value As String)
                _APPONumber = Left(value, 20)
                NotifyPropertyChanged("APPONumber")
            End Set
        End Property

        Private _APCustomerID As String = ""
        <DataMember()> _
        Public Property APCustomerID() As String
            Get
                Return Left(_APCustomerID, 50)
            End Get
            Set(ByVal value As String)
                _APCustomerID = Left(value, 50)
                NotifyPropertyChanged("APCustomerID")
            End Set
        End Property

        Private _APCostCenterNumber As String = ""
        <DataMember()> _
        Public Property APCostCenterNumber() As String
            Get
                Return Left(_APCostCenterNumber, 50)
            End Get
            Set(ByVal value As String)
                _APCostCenterNumber = Left(value, 50)
                NotifyPropertyChanged("APCostCenterNumber")
            End Set
        End Property

        Private _APTotalCost As Decimal = 0
        <DataMember()> _
        Public Property APTotalCost() As Decimal
            Get
                Return _APTotalCost
            End Get
            Set(ByVal value As Decimal)
                _APTotalCost = value
                NotifyPropertyChanged("APTotalCost")
            End Set
        End Property

        Private _APPRONumber As String = ""
        <DataMember()> _
        Public Property APPRONumber() As String
            Get
                Return Left(_APPRONumber, 20)
            End Get
            Set(ByVal value As String)
                _APPRONumber = Left(value, 20)
                NotifyPropertyChanged("APPRONumber")
            End Set
        End Property

        Private _APBLNumber As String = ""
        <DataMember()> _
        Public Property APBLNumber() As String
            Get
                Return Left(_APBLNumber, 20)
            End Get
            Set(ByVal value As String)
                _APBLNumber = Left(value, 20)
                NotifyPropertyChanged("APBLNumber")
            End Set
        End Property

        Private _APBilledWeight As Integer = 0
        <DataMember()> _
        Public Property APBilledWeight() As Integer
            Get
                Return _APBilledWeight
            End Get
            Set(ByVal value As Integer)
                _APBilledWeight = value
                NotifyPropertyChanged("APBilledWeight")
            End Set
        End Property

        Private _APCNSNumber As String = ""
        <DataMember()> _
        Public Property APCNSNumber() As String
            Get
                Return Left(_APCNSNumber, 20)
            End Get
            Set(ByVal value As String)
                _APCNSNumber = Left(value, 20)
                NotifyPropertyChanged("APCNSNumber")
            End Set
        End Property

        Private _APReceivedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property APReceivedDate() As System.Nullable(Of Date)
            Get
                Return _APReceivedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _APReceivedDate = value
                NotifyPropertyChanged("APReceivedDate")
            End Set
        End Property

        Private _APPayCode As String = ""
        <DataMember()> _
        Public Property APPayCode() As String
            Get
                Return Left(_APPayCode, 3)
            End Get
            Set(ByVal value As String)
                _APPayCode = Left(value, 3)
                NotifyPropertyChanged("APPayCode")
            End Set
        End Property

        Private _APElectronicFlag As Boolean = False
        <DataMember()> _
        Public Property APElectronicFlag() As Boolean
            Get
                Return _APElectronicFlag
            End Get
            Set(ByVal value As Boolean)
                _APElectronicFlag = value
                NotifyPropertyChanged("APElectronicFlag")
            End Set
        End Property

        Private _APApprovedFlag As Boolean = False
        <DataMember()> _
        Public Property APApprovedFlag() As Boolean
            Get
                Return _APApprovedFlag
            End Get
            Set(ByVal value As Boolean)
                _APApprovedFlag = value
                NotifyPropertyChanged("APApprovedFlag")
            End Set
        End Property

        Private _APMessage As String = ""
        <DataMember()> _
        Public Property APMessage() As String
            Get
                Return Left(_APMessage, 1000)
            End Get
            Set(ByVal value As String)
                _APMessage = Left(value, 1000)
                NotifyPropertyChanged("APMessage")
            End Set
        End Property

        Private _APTotalTax As Decimal = 0
        <DataMember()> _
        Public Property APTotalTax() As Decimal
            Get
                Return _APTotalTax
            End Get
            Set(ByVal value As Decimal)
                _APTotalTax = value
                NotifyPropertyChanged("APTotalTax")
            End Set
        End Property

        Private _APFee1 As Decimal = 0
        <DataMember()> _
        Public Property APFee1() As Decimal
            Get
                Return _APFee1
            End Get
            Set(ByVal value As Decimal)
                _APFee1 = value
                NotifyPropertyChanged("APFee1")
            End Set
        End Property

        Private _APFee2 As Decimal = 0
        <DataMember()> _
        Public Property APFee2() As Decimal
            Get
                Return _APFee2
            End Get
            Set(ByVal value As Decimal)
                _APFee2 = value
                NotifyPropertyChanged("APFee2")
            End Set
        End Property

        Private _APFee3 As Decimal = 0
        <DataMember()> _
        Public Property APFee3() As Decimal
            Get
                Return _APFee3
            End Get
            Set(ByVal value As Decimal)
                _APFee3 = value
                NotifyPropertyChanged("APFee3")
            End Set
        End Property

        Private _APFee4 As Decimal = 0
        <DataMember()> _
        Public Property APFee4() As Decimal
            Get
                Return _APFee4
            End Get
            Set(ByVal value As Decimal)
                _APFee4 = value
                NotifyPropertyChanged("APFee4")
            End Set
        End Property

        Private _APFee5 As Decimal = 0
        <DataMember()> _
        Public Property APFee5() As Decimal
            Get
                Return _APFee5
            End Get
            Set(ByVal value As Decimal)
                _APFee5 = value
                NotifyPropertyChanged("APFee5")
            End Set
        End Property

        Private _APFee6 As Decimal = 0
        <DataMember()> _
        Public Property APFee6() As Decimal
            Get
                Return _APFee6
            End Get
            Set(ByVal value As Decimal)
                _APFee6 = value
                NotifyPropertyChanged("APFee6")
            End Set
        End Property

        Private _APOtherCosts As Decimal = 0
        <DataMember()> _
        Public Property APOtherCosts() As Decimal
            Get
                Return _APOtherCosts
            End Get
            Set(ByVal value As Decimal)
                _APOtherCosts = value
                NotifyPropertyChanged("APOtherCosts")
            End Set
        End Property

        Private _APCarrierCost As Decimal = 0
        <DataMember()> _
        Public Property APCarrierCost() As Decimal
            Get
                Return _APCarrierCost
            End Get
            Set(ByVal value As Decimal)
                _APCarrierCost = value
                NotifyPropertyChanged("APCarrierCost")
            End Set
        End Property

        Private _APExportFlag As Boolean = False
        <DataMember()> _
        Public Property APExportFlag() As Boolean
            Get
                Return _APExportFlag
            End Get
            Set(ByVal value As Boolean)
                _APExportFlag = value
                NotifyPropertyChanged("APExportFlag")
            End Set
        End Property


        Private _APOrderSequence As Integer = 0
        <DataMember()> _
        Public Property APOrderSequence() As Integer
            Get
                Return _APOrderSequence
            End Get
            Set(ByVal value As Integer)
                _APOrderSequence = value
                NotifyPropertyChanged("APOrderSequence")
            End Set
        End Property



        Private _APModUser As String = ""
        <DataMember()> _
        Public Property APModUser() As String
            Get
                Return Left(_APModUser, 100)
            End Get
            Set(ByVal value As String)
                _APModUser = Left(value, 100)
                NotifyPropertyChanged("APModUser")
            End Set
        End Property

        Private _APModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property APModDate() As System.Nullable(Of Date)
            Get
                Return _APModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _APModDate = value
                NotifyPropertyChanged("APModDate")
            End Set
        End Property

        Private _APUpdated As Byte()
        <DataMember()> _
        Public Property APUpdated() As Byte()
            Get
                Return _APUpdated
            End Get
            Set(ByVal value As Byte())
                _APUpdated = value
                NotifyPropertyChanged("APUpdated")
            End Set
        End Property

        Private _APTaxDetail1 As Decimal = 0
        <DataMember()> _
        Public Property APTaxDetail1() As Decimal
            Get
                Return Me._APTaxDetail1
            End Get
            Set(ByVal value As Decimal)
                If ((Me._APTaxDetail1 = value) _
                   = False) Then
                    Me._APTaxDetail1 = value
                    NotifyPropertyChanged("APTaxDetail1")
                End If
            End Set
        End Property

        Private _APTaxDetail2 As Decimal = 0
        <DataMember()> _
        Public Property APTaxDetail2() As Decimal
            Get
                Return Me._APTaxDetail2
            End Get
            Set(ByVal value As Decimal)
                If ((Me._APTaxDetail2 = value) _
                   = False) Then
                    Me._APTaxDetail2 = value
                    NotifyPropertyChanged("APTaxDetail2")
                End If
            End Set
        End Property

        Private _APTaxDetail3 As Decimal = 0
        <DataMember()> _
        Public Property APTaxDetail3() As Decimal
            Get
                Return Me._APTaxDetail3
            End Get
            Set(ByVal value As Decimal)
                If ((Me._APTaxDetail3 = value) _
                   = False) Then
                    Me._APTaxDetail3 = value
                    NotifyPropertyChanged("APTaxDetail3")
                End If
            End Set
        End Property

        Private _APTaxDetail4 As Decimal = 0
        <DataMember()> _
        Public Property APTaxDetail4() As Decimal
            Get
                Return Me._APTaxDetail4
            End Get
            Set(ByVal value As Decimal)
                If ((Me._APTaxDetail4 = value) _
                   = False) Then
                    Me._APTaxDetail4 = value
                    NotifyPropertyChanged("APTaxDetail4")
                End If
            End Set
        End Property

        Private _APTaxDetail5 As Decimal = 0
        <DataMember()> _
        Public Property APTaxDetail5() As Decimal
            Get
                Return Me._APTaxDetail5
            End Get
            Set(ByVal value As Decimal)
                If ((Me._APTaxDetail5 = value) _
                   = False) Then
                    Me._APTaxDetail5 = value
                    NotifyPropertyChanged("APTaxDetail5")
                End If
            End Set
        End Property

        Private _APSHID As String = ""
        <DataMember()> _
        Public Property APSHID() As String
            Get
                Return Left(_APSHID, 50)
            End Get
            Set(ByVal value As String)
                _APSHID = Left(value, 50)
                NotifyPropertyChanged("APSHID")
            End Set
        End Property

        Private _APShipCarrierProNumber As String = ""
        <DataMember()>
        Public Property APShipCarrierProNumber() As String
            Get
                Return Left(_APShipCarrierProNumber, 20)
            End Get
            Set(ByVal value As String)
                _APShipCarrierProNumber = Left(value, 20)
                NotifyPropertyChanged("APShipCarrierProNumber")
            End Set
        End Property

        'Added By LVV 3/24/20 v-8.2.1.006
        Private _APReduction As Decimal = 0
        <DataMember()>
        Public Property APReduction() As Decimal
            Get
                Return _APReduction
            End Get
            Set(ByVal value As Decimal)
                _APReduction = value
                NotifyPropertyChanged("APReduction")
            End Set
        End Property

        'Added By LVV 3/24/20 v-8.2.1.006
        Private _APReductionReason As Integer = 0
        <DataMember()>
        Public Property APReductionReason() As Integer
            Get
                Return _APReductionReason
            End Get
            Set(ByVal value As Integer)
                _APReductionReason = value
                NotifyPropertyChanged("APReductionReason")
            End Set
        End Property

        'Added By LVV 3/24/20 v-8.2.1.006
        Private _APReductionAdjustedCost As Decimal = 0
        <DataMember()>
        Public Property APReductionAdjustedCost() As Decimal
            Get
                Return _APReductionAdjustedCost
            End Get
            Set(ByVal value As Decimal)
                _APReductionAdjustedCost = value
                NotifyPropertyChanged("APReductionAdjustedCost")
            End Set
        End Property



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New APMassEntry
            instance = DirectCast(MemberwiseClone(), APMassEntry)
            Return instance
        End Function

#End Region

    End Class

End Namespace
