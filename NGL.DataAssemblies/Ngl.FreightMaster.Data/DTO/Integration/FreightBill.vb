Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class FreightBill
        Inherits DTOBaseClass

#Region " Data Members"


        Private _APPONumber As String = ""
        <DataMember()> _
        Public Property APPONumber() As String
            Get
                Return Left(_APPONumber, 20)
            End Get
            Set(ByVal value As String)
                _APPONumber = Left(value, 20)
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
            End Set
        End Property

        Private _APBillDate As Date
        <DataMember()> _
        Public Property APBillDate() As Date
            Get
                Return _APBillDate
            End Get
            Set(ByVal value As Date)
                _APBillDate = value
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
            End Set
        End Property

        Private _APReceivedDate As Date = Date.Now
        <DataMember()> _
        Public Property APReceivedDate() As Date
            Get
                Return _APReceivedDate
            End Get
            Set(ByVal value As Date)
                _APReceivedDate = value
            End Set
        End Property

        Private _APPayCode As String = "N"
        <DataMember()> _
        Public Property APPayCode() As String
            Get
                Return Left(_APPayCode, 3)
            End Get
            Set(ByVal value As String)
                _APPayCode = Left(value, 3)
            End Set
        End Property


        Private _APElectronicFlag As Boolean = True
        <DataMember()> _
        Public Property APElectronicFlag() As Boolean
            Get
                Return _APElectronicFlag
            End Get
            Set(ByVal value As Boolean)
                _APElectronicFlag = value
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
            End Set
        End Property

        Private _APOtherCost As Decimal = 0
        <DataMember()> _
        Public Property APOtherCost() As Decimal
            Get
                Return _APOtherCost
            End Get
            Set(ByVal value As Decimal)
                _APOtherCost = value
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
            End Set
        End Property

        Private _APOverwrite As Boolean = False
        <DataMember()> _
        Public Property APOverwrite() As Boolean
            Get
                Return _APOverwrite
            End Get
            Set(ByVal value As Boolean)
                _APOverwrite = value
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
            End Set
        End Property

        Private _APTaxDetail1 As Decimal = 0
        <DataMember()> _
        Public Property APTaxDetail1() As Decimal
            Get
                Return _APTaxDetail1
            End Get
            Set(ByVal value As Decimal)
                _APTaxDetail1 = value
            End Set
        End Property

        Private _APTaxDetail2 As Decimal = 0
        <DataMember()> _
        Public Property APTaxDetail2() As Decimal
            Get
                Return _APTaxDetail2
            End Get
            Set(ByVal value As Decimal)
                _APTaxDetail2 = value
            End Set
        End Property

        Private _APTaxDetail3 As Decimal = 0
        <DataMember()> _
        Public Property APTaxDetail3() As Decimal
            Get
                Return _APTaxDetail3
            End Get
            Set(ByVal value As Decimal)
                _APTaxDetail3 = value
            End Set
        End Property

        Private _APTaxDetail4 As Decimal = 0
        <DataMember()> _
        Public Property APTaxDetail4() As Decimal
            Get
                Return _APTaxDetail4
            End Get
            Set(ByVal value As Decimal)
                _APTaxDetail4 = value
            End Set
        End Property

        Private _APTaxDetail5 As Decimal = 0
        <DataMember()> _
        Public Property APTaxDetail5() As Decimal
            Get
                Return _APTaxDetail5
            End Get
            Set(ByVal value As Decimal)
                _APTaxDetail5 = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New FreightBill
            instance = DirectCast(MemberwiseClone(), FreightBill)
            Return instance
        End Function

#End Region

    End Class
End Namespace


