Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierTariffMatrixDetail
        Inherits DTOBaseClass

#Region " Data Members"
        Private _CarrTarMatDetControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatDetControl() As Integer
            Get
                Return _CarrTarMatDetControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarMatDetControl = value
            End Set
        End Property

        Private _CarrTarMatDetCarrTarMatControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatDetCarrTarMatControl() As Integer
            Get
                Return _CarrTarMatDetCarrTarMatControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarMatDetCarrTarMatControl = value
            End Set
        End Property

        Private _CarrTarMatDetID As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatDetID() As Integer
            Get
                Return _CarrTarMatDetID
            End Get
            Set(ByVal value As Integer)
                _CarrTarMatDetID = value
            End Set
        End Property

        Private _CarrTarMatDetValue As Decimal = 0
        <DataMember()> _
        Public Property CarrTarMatDetValue() As Decimal
            Get
                Return _CarrTarMatDetValue
            End Get
            Set(ByVal value As Decimal)
                _CarrTarMatDetValue = value
            End Set
        End Property

        Private _CarrTarMatDetModUser As String = ""
        <DataMember()> _
        Public Property CarrTarMatDetModUser() As String
            Get
                Return Left(_CarrTarMatDetModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrTarMatDetModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrTarMatDetModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarMatDetModDate() As System.Nullable(Of Date)
            Get
                Return _CarrTarMatDetModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrTarMatDetModDate = value
            End Set
        End Property

        Private _CarrTarMatDetUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarMatDetUpdated() As Byte()
            Get
                Return _CarrTarMatDetUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarMatDetUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierTariffMatrixDetail
            instance = DirectCast(MemberwiseClone(), CarrierTariffMatrixDetail)
            Return instance
        End Function

#End Region

    End Class
End Namespace