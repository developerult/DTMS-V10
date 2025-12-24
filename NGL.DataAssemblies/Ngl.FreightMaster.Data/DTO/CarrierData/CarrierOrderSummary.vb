Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierOrderSummary
        Inherits DTOBaseClass


#Region " Data Members"

        Private _BookDateLoad As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateLoad() As System.Nullable(Of Date)
            Get
                Return _BookDateLoad
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateLoad = value
            End Set
        End Property

        Private _Pickup As Integer = 0
        <DataMember()> _
        Public Property Pickup() As Integer
            Get
                Return _Pickup
            End Get
            Set(ByVal value As Integer)
                _Pickup = value
            End Set
        End Property

        Private _Delivery As Integer = 0
        <DataMember()> _
        Public Property Delivery() As Integer
            Get
                Return _Delivery
            End Get
            Set(ByVal value As Integer)
                _Delivery = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierOrderSummary
            instance = DirectCast(MemberwiseClone(), CarrierOrderSummary)
            Return instance
        End Function

#End Region

    End Class
End Namespace
