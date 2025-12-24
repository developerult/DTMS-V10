Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierTariffMatrix
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrTarMatControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatControl() As Integer
            Get
                Return _CarrTarMatControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarMatControl = value
            End Set
        End Property

        Private _CarrTarMatCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatCarrTarControl() As Integer
            Get
                Return _CarrTarMatCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarMatCarrTarControl = value
            End Set
        End Property

        Private _CarrTarMatFromZip As String = ""
        <DataMember()> _
        Public Property CarrTarMatFromZip() As String
            Get
                Return Left(_CarrTarMatFromZip, 10)
            End Get
            Set(ByVal value As String)
                _CarrTarMatFromZip = Left(value, 10)
            End Set
        End Property


        Private _CarrTarMatToZip As String = ""
        <DataMember()> _
        Public Property CarrTarMatToZip() As String
            Get
                Return Left(_CarrTarMatToZip, 10)
            End Get
            Set(ByVal value As String)
                _CarrTarMatToZip = Left(value, 10)
            End Set
        End Property


        Private _CarrTarMatExptFlag As Boolean = False
        <DataMember()> _
        Public Property CarrTarMatExptFlag() As Boolean
            Get
                Return _CarrTarMatExptFlag
            End Get
            Set(ByVal value As Boolean)
                _CarrTarMatExptFlag = value
            End Set
        End Property

        Private _CarrTarMatMin As Decimal = 0
        <DataMember()> _
        Public Property CarrTarMatMin() As Decimal
            Get
                Return _CarrTarMatMin
            End Get
            Set(ByVal value As Decimal)
                _CarrTarMatMin = value
            End Set
        End Property

        Private _CarrTarMatMaxDays As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatMaxDays() As Integer
            Get
                Return _CarrTarMatMaxDays
            End Get
            Set(ByVal value As Integer)
                _CarrTarMatMaxDays = value
            End Set
        End Property

        Private _CarrTarMatModUser As String = ""
        <DataMember()> _
        Public Property CarrTarMatModUser() As String
            Get
                Return Left(_CarrTarMatModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrTarMatModUser = Left(value, 100)
            End Set
        End Property


        Private _CarrTarMatModDate As Date = DateTime.Now
        <DataMember()> _
        Public Property CarrTarMatModDate() As Date
            Get
                Return _CarrTarMatModDate
            End Get
            Set(ByVal value As Date)
                _CarrTarMatModDate = value
            End Set
        End Property

        Private _CarrTarMatUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarMatUpdated() As Byte()
            Get
                Return _CarrTarMatUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarMatUpdated = value
            End Set
        End Property

        Private _CarrierTariffMatrixDetails As List(Of CarrierTariffMatrixDetail)
        Friend Property CarrierTariffMatrixDetails() As List(Of CarrierTariffMatrixDetail)
            Get
                Return _CarrierTariffMatrixDetails
            End Get
            Set(ByVal value As List(Of CarrierTariffMatrixDetail))
                _CarrierTariffMatrixDetails = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierTariffMatrix
            instance = DirectCast(MemberwiseClone(), CarrierTariffMatrix)
            instance.CarrierTariffMatrixDetails = Nothing
            For Each item In CarrierTariffMatrixDetails
                instance.CarrierTariffMatrixDetails.Add(DirectCast(item.Clone, CarrierTariffMatrixDetail))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace