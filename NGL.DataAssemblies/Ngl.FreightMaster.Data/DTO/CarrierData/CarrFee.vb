Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrFee
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrFeesControl As Integer = 0
        <DataMember()> _
        Public Property CarrFeesControl() As Integer
            Get
                Return _CarrFeesControl
            End Get
            Set(ByVal value As Integer)
                _CarrFeesControl = value
            End Set
        End Property

        Private _CarrFeesCarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrFeesCarrierControl() As Integer
            Get
                Return _CarrFeesCarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrFeesCarrierControl = value
            End Set
        End Property

        Private _CarrFeesMinimum As Decimal = 0
        <DataMember()> _
        Public Property CarrFeesMinimum() As Decimal
            Get
                Return _CarrFeesMinimum
            End Get
            Set(ByVal value As Decimal)
                _CarrFeesMinimum = value
            End Set
        End Property

        Private _CarrFeesVariable As Double = 0
        <DataMember()> _
        Public Property CarrFeesVariable() As Double
            Get
                Return _CarrFeesVariable
            End Get
            Set(ByVal value As Double)
                _CarrFeesVariable = value
            End Set
        End Property

        Private _CarrFeesAccessorialCode As Integer = 0
        <DataMember()> _
        Public Property CarrFeesAccessorialCode() As Integer
            Get
                Return _CarrFeesAccessorialCode
            End Get
            Set(ByVal value As Integer)
                _CarrFeesAccessorialCode = value
            End Set
        End Property

        Private _CarrFeesModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrFeesModDate() As System.Nullable(Of Date)
            Get
                Return _CarrFeesModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrFeesModDate = value
            End Set
        End Property

        Private _CarrFeesModUser As String = ""
        <DataMember()> _
        Public Property CarrFeesModUser() As String
            Get
                Return Left(_CarrFeesModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrFeesModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrFeesUpdated As Byte()
        <DataMember()> _
        Public Property CarrFeesUpdated() As Byte()
            Get
                Return _CarrFeesUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrFeesUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrFee
            instance = DirectCast(MemberwiseClone(), CarrFee)
            Return instance
        End Function

#End Region

    End Class
End Namespace
