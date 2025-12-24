Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class APExportFileData
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrierNumber As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CarrierNumber() As System.Nullable(Of Integer)
            Get
                Return Me._CarrierNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._CarrierNumber.Equals(value) = False) Then
                    Me._CarrierNumber = value
                    NotifyPropertyChanged("CarrierNumber")
                End If
            End Set
        End Property

        Private _BookFinAPBillNumber As String
        <DataMember()> _
        Public Property BookFinAPBillNumber() As String
            Get
                Return Me._BookFinAPBillNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookFinAPBillNumber, value) = False) Then
                    Me._BookFinAPBillNumber = value
                    NotifyPropertyChanged("BookFinAPBillNumber")
                End If
            End Set
        End Property
        Private _BookFinAPBillInvDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinAPBillInvDate() As System.Nullable(Of Date)
            Get
                Return Me._BookFinAPBillInvDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._BookFinAPBillInvDate.Equals(value) = False) Then
                    Me._BookFinAPBillInvDate = value
                    NotifyPropertyChanged("BookFinAPBillInvDate")
                End If
            End Set
        End Property
        Private _BookCarrOrderNumber As String
        <DataMember()> _
        Public Property BookCarrOrderNumber() As String
            Get
                Return Me._BookCarrOrderNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookCarrOrderNumber, value) = False) Then
                    Me._BookCarrOrderNumber = value
                    NotifyPropertyChanged("BookCarrOrderNumber")
                End If
            End Set
        End Property

        Private _LaneNumber As String
        <DataMember()> _
        Public Property LaneNumber() As String
            Get
                Return Me._LaneNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._LaneNumber, value) = False) Then
                    Me._LaneNumber = value
                    NotifyPropertyChanged("LaneNumber")
                End If
            End Set
        End Property

        Private _BookItemCostCenterNumber As String
        <DataMember()> _
        Public Property BookItemCostCenterNumber() As String
            Get
                Return Me._BookItemCostCenterNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookItemCostCenterNumber, value) = False) Then
                    Me._BookItemCostCenterNumber = value
                    NotifyPropertyChanged("BookItemCostCenterNumber")
                End If
            End Set
        End Property

        Private _BookFinAPActCost As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BookFinAPActCost() As System.Nullable(Of Decimal)
            Get
                Return Me._BookFinAPActCost
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BookFinAPActCost.Equals(value) = False) Then
                    Me._BookFinAPActCost = value
                    NotifyPropertyChanged("BookFinAPActCost")
                End If
            End Set
        End Property

        Private _BookCarrBLNumber As String
        <DataMember()> _
        Public Property BookCarrBLNumber() As String
            Get
                Return Me._BookCarrBLNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookCarrBLNumber, value) = False) Then
                    Me._BookCarrBLNumber = value
                    NotifyPropertyChanged("BookCarrBLNumber")
                End If
            End Set
        End Property

        Private _BookFinAPActWgt As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property BookFinAPActWgt() As System.Nullable(Of Integer)
            Get
                Return Me._BookFinAPActWgt
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._BookFinAPActWgt.Equals(value) = False) Then
                    Me._BookFinAPActWgt = value
                    NotifyPropertyChanged("BookFinAPActWgt")
                End If
            End Set
        End Property

        Private _BookFinAPBillNoDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinAPBillNoDate() As System.Nullable(Of Date)
            Get
                Return Me._BookFinAPBillNoDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._BookFinAPBillNoDate.Equals(value) = False) Then
                    Me._BookFinAPBillNoDate = value
                    NotifyPropertyChanged("BookFinAPBillNoDate")
                End If
            End Set
        End Property

        Private _BookFinAPActTax As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BookFinAPActTax() As System.Nullable(Of Decimal)
            Get
                Return Me._BookFinAPActTax
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BookFinAPActTax.Equals(value) = False) Then
                    Me._BookFinAPActTax = value
                    NotifyPropertyChanged("BookFinAPActTax")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New APExportFileData
            instance = DirectCast(MemberwiseClone(), APExportFileData)
            Return instance
        End Function

#End Region

    End Class

End Namespace
