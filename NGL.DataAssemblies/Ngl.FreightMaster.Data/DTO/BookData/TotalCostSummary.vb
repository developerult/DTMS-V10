Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


Namespace DataTransferObjects
    <DataContract()> _
    Public Class TotalCostSummary
        Inherits DTOBaseClass

#Region " Data Members"

        Private _TotalCost As Decimal
        <DataMember()> _
        Public Property TotalCost() As Decimal
            Get
                Return _TotalCost
            End Get
            Set(ByVal value As Decimal)
                If (Me._TotalCost <> value) Then
                    Me._TotalCost = value
                    Me.SendPropertyChanged("TotalCost")
                End If
            End Set
        End Property

        Private _NetCost As Decimal
        <DataMember()> _
        Public Property NetCost() As Decimal
            Get
                Return _NetCost
            End Get
            Set(ByVal value As Decimal)
                If (Me._NetCost <> value) Then
                    Me._NetCost = value
                    Me.SendPropertyChanged("NetCost")
                End If
            End Set
        End Property

        Private _FreightTax As Decimal
        <DataMember()> _
        Public Property FreightTax() As Decimal
            Get
                Return _FreightTax
            End Get
            Set(ByVal value As Decimal)
                If (Me._FreightTax <> value) Then
                    Me._FreightTax = value
                    Me.SendPropertyChanged("FreightTax")
                End If
            End Set
        End Property

        Private _NonTaxable As Decimal
        <DataMember()> _
        Public Property NonTaxable() As Decimal
            Get
                Return _NonTaxable
            End Get
            Set(ByVal value As Decimal)
                If (Me._NonTaxable <> value) Then
                    Me._NonTaxable = value
                    Me.SendPropertyChanged("NonTaxable")
                End If
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New TotalCostSummary
            instance = DirectCast(MemberwiseClone(), TotalCostSummary)
            Return instance
        End Function

#End Region

    End Class
End Namespace