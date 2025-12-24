Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class AdjustBFC
        Inherits DTOBaseClass


#Region " Data Members"

        Private _BookRevs As List(Of BookRevenue)
        <DataMember()> _
        Public Property BookRevs() As List(Of BookRevenue)
            Get
                Return _BookRevs
            End Get
            Set(ByVal value As List(Of BookRevenue))
                _BookRevs = value
            End Set
        End Property
         
        Private _AutoCalculateBFC As Boolean = True
        <DataMember()> _
        Public Property AutoCalculateBFC() As Boolean
            Get
                Return _AutoCalculateBFC
            End Get
            Set(ByVal value As Boolean)
                _AutoCalculateBFC = value
            End Set
        End Property

        Private _totalBFC As Decimal = 0
        <DataMember()> _
        Public Property TotalBFC() As Decimal
            Get
                Return _totalBFC
            End Get
            Set(ByVal value As Decimal)
                _totalBFC = value
            End Set
        End Property

        Private _allocationBFCFormula As tblTarBracketType
        <DataMember()> _
        Public Property AllocationBFCFormula() As tblTarBracketType
            Get
                Return _allocationBFCFormula
            End Get
            Set(ByVal value As tblTarBracketType)
                _allocationBFCFormula = value
            End Set
        End Property
         
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AdjustBFC
            instance = DirectCast(MemberwiseClone(), AdjustBFC)
            Return instance
        End Function

#End Region

    End Class
End Namespace