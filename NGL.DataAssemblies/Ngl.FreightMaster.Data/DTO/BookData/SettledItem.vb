Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()>
    Public Class SettledItem
        Inherits DTOBaseClass


#Region " Data Members"

        <DataMember()> Public Property Control() As Integer
            Get
                Return m_Control
            End Get
            Set(ByVal value As Integer)
                m_Control = value
            End Set
        End Property
        Private m_Control As Integer
        <DataMember()> Public Property InvoiceNumber() As String
            Get
                Return m_InvoiceNumber
            End Get
            Set(ByVal value As String)
                m_InvoiceNumber = value
            End Set
        End Property
        Private m_InvoiceNumber As String
        <DataMember()> Public Property ContractedCost() As Decimal
            Get
                Return m_ContractedCost
            End Get
            Set(ByVal value As Decimal)
                m_ContractedCost = value
            End Set
        End Property
        Private m_ContractedCost As Decimal
        <DataMember()> Public Property PaidCost() As Decimal
            Get
                Return m_PaidCost
            End Get
            Set(ByVal value As Decimal)
                m_PaidCost = value
            End Set
        End Property
        Private m_PaidCost As Decimal
        <DataMember()> Public Property InvoiceAmount() As Decimal
            Get
                Return m_InvoiceAmount
            End Get
            Set(ByVal value As Decimal)
                m_InvoiceAmount = value
            End Set
        End Property
        Private m_InvoiceAmount As Decimal
        <DataMember()> Public Property PaidDate() As Nullable(Of DateTime)
            Get
                Return m_PaidDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_PaidDate = value
            End Set
        End Property
        Private m_PaidDate As Nullable(Of DateTime)
        <DataMember()> Public Property CheckNumber() As String
            Get
                Return m_CheckNumber
            End Get
            Set(ByVal value As String)
                m_CheckNumber = value
            End Set
        End Property
        Private m_CheckNumber As String
        <DataMember()> Public Property ProNumber() As String
            Get
                Return m_ProNumber
            End Get
            Set(ByVal value As String)
                m_ProNumber = value
            End Set
        End Property
        Private m_ProNumber As String
        <DataMember()> Public Property CnsNumber() As String
            Get
                Return m_CnsNumber
            End Get
            Set(ByVal value As String)
                m_CnsNumber = value
            End Set
        End Property
        Private m_CnsNumber As String

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

        'Modified by RHR for v-8.5.2.005 on 08/05/2022 added string value for  PaidDate 
        Private m_txtPaidDate As String
        <DataMember()> Public ReadOnly Property TxtPaidDate() As String
            Get
                If PaidDate.HasValue Then
                    m_txtPaidDate = PaidDate.Value.ToShortDateString()
                Else
                    m_txtPaidDate = "Open"
                End If
                Return m_txtPaidDate
            End Get
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New SettledItem
            instance = DirectCast(MemberwiseClone(), SettledItem)
            Return instance
        End Function

#End Region

    End Class
End Namespace