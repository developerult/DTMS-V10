Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class SettlementItem
        Inherits DTOBaseClass


#Region " Data Members"

        <DataMember()> Public Property Control() As Integer
            Get
                Return m_Control
            End Get
            Set(ByVal value As Integer)
                m_Control = Value
            End Set
        End Property
        Private m_Control As Integer
        <DataMember()> Public Property CnsNumber() As String
            Get
                Return m_CnsNumber
            End Get
            Set(ByVal value As String)
                m_CnsNumber = Value
            End Set
        End Property
        Private m_CnsNumber As String
        <DataMember()> Public Property ProNumber() As String
            Get
                Return m_ProNumber
            End Get
            Set(ByVal value As String)
                m_ProNumber = Value
            End Set
        End Property
        Private m_ProNumber As String
        <DataMember()> Public Property OrderNumber() As String
            Get
                Return m_OrderNumber
            End Get
            Set(ByVal value As String)
                m_OrderNumber = Value
            End Set
        End Property
        Private m_OrderNumber As String
        <DataMember()> Public Property PickupName() As String
            Get
                Return m_PickupName
            End Get
            Set(ByVal value As String)
                m_PickupName = Value
            End Set
        End Property
        Private m_PickupName As String
        <DataMember()> Public Property DestinationName() As String
            Get
                Return m_DestinationName
            End Get
            Set(ByVal value As String)
                m_DestinationName = Value
            End Set
        End Property
        Private m_DestinationName As String
        <DataMember()> Public Property Status() As String
            Get
                Return m_Status
            End Get
            Set(ByVal value As String)
                m_Status = Value
            End Set
        End Property
        Private m_Status As String
        <DataMember()> Public Property DeliveredDate() As Nullable(Of DateTime)
            Get
                Return m_DeliveredDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_DeliveredDate = value
            End Set
        End Property
        Private m_DeliveredDate As Nullable(Of DateTime)
        <DataMember()> Public Property ContractedCost() As Decimal
            Get
                Return m_ContractedCost
            End Get
            Set(ByVal value As Decimal)
                m_ContractedCost = Value
            End Set
        End Property
        Private m_ContractedCost As Decimal
        <DataMember()> Public Property InvoiceNumber() As String
            Get
                Return m_InvoiceNumber
            End Get
            Set(ByVal value As String)
                m_InvoiceNumber = Value
            End Set
        End Property
        Private m_InvoiceNumber As String
        <DataMember()> Public Property InvoiceAmount() As Decimal
            Get
                Return m_InvoiceAmount
            End Get
            Set(ByVal value As Decimal)
                m_InvoiceAmount = value
            End Set
        End Property
        Private m_InvoiceAmount As Decimal

        'Added by LVV on 7/28/16 for v-7.0.5.110 Task #14 NxT Search Filters
        Private m_SHID As String
        <DataMember()> Public Property SHID() As String
            Get
                Return m_SHID
            End Get
            Set(ByVal value As String)
                m_SHID = value
            End Set
        End Property

        Private m_CarrierPro As String
        <DataMember()> Public Property CarrierPro() As String
            Get
                Return m_CarrierPro
            End Get
            Set(ByVal value As String)
                m_CarrierPro = value
            End Set
        End Property

        Private m_BookFinAPActWgt As Integer
        <DataMember()> Public Property BookFinAPActWgt() As Integer
            Get
                Return m_BookFinAPActWgt
            End Get
            Set(ByVal value As Integer)
                m_BookFinAPActWgt = value
            End Set
        End Property

        Private m_BookCarrBLNumber As String
        <DataMember()> Public Property BookCarrBLNumber() As String
            Get
                Return m_BookCarrBLNumber
            End Get
            Set(ByVal value As String)
                m_BookCarrBLNumber = value
            End Set
        End Property

        Private m_CarrierName As String
        <DataMember()> Public Property CarrierName() As String
            Get
                Return m_CarrierName
            End Get
            Set(ByVal value As String)
                m_CarrierName = value
            End Set
        End Property

        Private m_BookCarrierControl As Integer
        <DataMember()> Public Property BookCarrierControl() As Integer
            Get
                Return m_BookCarrierControl
            End Get
            Set(ByVal value As Integer)
                m_BookCarrierControl = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New SettlementItem
            instance = DirectCast(MemberwiseClone(), SettlementItem)
            Return instance
        End Function

#End Region

    End Class
End Namespace