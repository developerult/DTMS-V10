Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects

    <DataContract()> _
    Public Class NegativeRevenueReason
        Inherits DTOBaseClass


#Region " Data Members"

        Private _NegativeRevenueCode As Integer = 0
        <DataMember()> _
        Public Property NegativeRevenueCode() As Integer
            Get
                Return _NegativeRevenueCode
            End Get
            Set(ByVal value As Integer)
                _NegativeRevenueCode = value
            End Set
        End Property

        Private _NegativeRevenueReason As String = ""
        <DataMember()> _
        Public Property NegativeRevenueReason() As String
            Get
                Return Left(_NegativeRevenueReason, 255)
            End Get
            Set(ByVal value As String)
                _NegativeRevenueReason = Left(value, 255)
            End Set
        End Property

        Private _NegativeRevenueUpdated As Byte()
        <DataMember()> _
        Public Property NegativeRevenueUpdated() As Byte()
            Get
                Return _NegativeRevenueUpdated
            End Get
            Set(ByVal value As Byte())
                _NegativeRevenueUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New NegativeRevenueReason
            instance = DirectCast(MemberwiseClone(), NegativeRevenueReason)
            Return instance
        End Function

#End Region

    End Class

End Namespace
