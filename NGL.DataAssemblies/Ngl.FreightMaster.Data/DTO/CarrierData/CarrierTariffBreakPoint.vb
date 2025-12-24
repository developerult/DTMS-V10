Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierTariffBreakPoint
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrTarBPControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarBPControl() As Integer
            Get
                Return _CarrTarBPControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarBPControl = value
            End Set
        End Property

        Private _CarrTarBPCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarBPCarrTarControl() As Integer
            Get
                Return _CarrTarBPCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarBPCarrTarControl = value
            End Set
        End Property

        Private _CarrTarBPID As Integer = 0
        <DataMember()> _
        Public Property CarrTarBPID() As Integer
            Get
                Return _CarrTarBPID
            End Get
            Set(ByVal value As Integer)
                _CarrTarBPID = value
            End Set
        End Property

        Private _CarrTarBPValue As Decimal = 0
        <DataMember()> _
        Public Property CarrTarBPValue() As Decimal
            Get
                Return _CarrTarBPValue
            End Get
            Set(ByVal value As Decimal)
                _CarrTarBPValue = value
            End Set
        End Property

        Private _CarrTarBPModUser As String = ""
        <DataMember()> _
        Public Property CarrTarBPModUser() As String
            Get
                Return Left(_CarrTarBPModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrTarBPModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrTarBPModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarBPModDate() As System.Nullable(Of Date)
            Get
                Return _CarrTarBPModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrTarBPModDate = value
            End Set
        End Property

        Private _CarrTarBPUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarBPUpdated() As Byte()
            Get
                Return _CarrTarBPUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarBPUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierTariffBreakPoint
            instance = DirectCast(MemberwiseClone(), CarrierTariffBreakPoint)
            Return instance
        End Function

#End Region

    End Class
End Namespace