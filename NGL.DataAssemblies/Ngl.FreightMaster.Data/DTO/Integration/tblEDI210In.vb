Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV 2/29/16 for v-7.0.5.1 EDI Migration
Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblEDI210In
        Inherits DTOBaseClass


#Region " Data Members"

        Private _EDI210InControl As Integer = 0
        <DataMember()> _
        Public Property EDI210InControl() As Integer
            Get
                Return _EDI210InControl
            End Get
            Set(ByVal value As Integer)
                _EDI210InControl = value
                NotifyPropertyChanged("EDI210InControl")
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

        Private _EDI210InReceived As Boolean = False
        <DataMember()> _
        Public Property EDI210InReceived() As Boolean
            Get
                Return _EDI210InReceived
            End Get
            Set(ByVal value As Boolean)
                _EDI210InReceived = value
            End Set
        End Property

        Private _EDI210InReceivedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI210InReceivedDate() As System.Nullable(Of Date)
            Get
                Return _EDI210InReceivedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI210InReceivedDate = value
            End Set
        End Property

        Private _EDI210InStatusCode As Integer? = 0
        <DataMember()> _
        Public Property EDI210InStatusCode() As Integer?
            Get
                Return _EDI210InStatusCode
            End Get
            Set(ByVal value As Integer?)
                _EDI210InStatusCode = value
            End Set
        End Property

        Private _EDI210InMessage As String = ""
        <DataMember()> _
        Public Property EDI210InMessage() As String
            Get
                Return _EDI210InMessage
            End Get
            Set(ByVal value As String)
                _EDI210InMessage = value
            End Set
        End Property

        Private _EDI210InFileName As String = ""
        <DataMember()> _
        Public Property EDI210InFileName() As String
            Get
                Return Left(_EDI210InFileName, 255)
            End Get
            Set(ByVal value As String)
                _EDI210InFileName = Left(value, 255)
                Me.SendPropertyChanged("EDI210InFileName")
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Left(_CarrierName, 40)
            End Get
            Set(ByVal value As String)
                _CarrierName = Left(value, 40)
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

        Private _EDI210InModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI210InModDate() As System.Nullable(Of Date)
            Get
                Return _EDI210InModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI210InModDate = value
                Me.SendPropertyChanged("EDI210InModDate")
            End Set
        End Property

        Private _EDI210InModUser As String = ""
        <DataMember()> _
        Public Property EDI210InModUser() As String
            Get
                Return Left(_EDI210InModUser, 100)
            End Get
            Set(ByVal value As String)
                _EDI210InModUser = Left(value, 100)
                Me.SendPropertyChanged("EDI210InModUser")
            End Set
        End Property

        Private _EDI210InUpdated As Byte()
        <DataMember()> _
        Public Property EDI210InUpdated() As Byte()
            Get
                Return _EDI210InUpdated
            End Get
            Set(ByVal value As Byte())
                _EDI210InUpdated = value
            End Set
        End Property

        Private _APFeeDesc1 As String = ""
        <DataMember()> _
        Public Property APFeeDesc1() As String
            Get
                Return Left(_APFeeDesc1, 255)
            End Get
            Set(ByVal value As String)
                _APFeeDesc1 = Left(value, 255)
            End Set
        End Property

        Private _APFeeDesc2 As String = ""
        <DataMember()> _
        Public Property APFeeDesc2() As String
            Get
                Return Left(_APFeeDesc2, 255)
            End Get
            Set(ByVal value As String)
                _APFeeDesc2 = Left(value, 255)
            End Set
        End Property

        Private _APFeeDesc3 As String = ""
        <DataMember()> _
        Public Property APFeeDesc3() As String
            Get
                Return Left(_APFeeDesc3, 255)
            End Get
            Set(ByVal value As String)
                _APFeeDesc3 = Left(value, 255)
            End Set
        End Property

        Private _APFeeDesc4 As String = ""
        <DataMember()> _
        Public Property APFeeDesc4() As String
            Get
                Return Left(_APFeeDesc4, 255)
            End Get
            Set(ByVal value As String)
                _APFeeDesc4 = Left(value, 255)
            End Set
        End Property

        Private _APFeeDesc5 As String = ""
        <DataMember()> _
        Public Property APFeeDesc5() As String
            Get
                Return Left(_APFeeDesc5, 255)
            End Get
            Set(ByVal value As String)
                _APFeeDesc5 = Left(value, 255)
            End Set
        End Property

        Private _APFeeDesc6 As String = ""
        <DataMember()> _
        Public Property APFeeDesc6() As String
            Get
                Return Left(_APFeeDesc6, 255)
            End Get
            Set(ByVal value As String)
                _APFeeDesc6 = Left(value, 255)
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblEDI210In
            instance = DirectCast(MemberwiseClone(), tblEDI210In)
            Return instance
        End Function

#End Region

    End Class

End Namespace
