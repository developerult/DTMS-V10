Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookRevTotalCost
        Inherits DTOBaseClass


#Region " Data Members"

        Private _TotalCost As Decimal = 0
        <DataMember()> _
        Public Property TotalCost() As Decimal
            Get
                Return _TotalCost
            End Get
            Friend Set(ByVal value As Decimal)
                _TotalCost = value
            End Set
        End Property


        Private _NetCost As Decimal = 0
        <DataMember()> _
        Public Property NetCost() As Decimal
            Get
                Return _NetCost
            End Get
            Friend Set(ByVal value As Decimal)
                _NetCost = value
            End Set
        End Property


        Private _FreightTax As Decimal = 0
        <DataMember()> _
        Public Property FreightTax() As Decimal
            Get
                Return _FreightTax
            End Get
            Friend Set(ByVal value As Decimal)
                _FreightTax = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookRevTotalCost
            instance = DirectCast(MemberwiseClone(), BookRevTotalCost)
            Return instance
        End Function

#End Region


    End Class
End Namespace
