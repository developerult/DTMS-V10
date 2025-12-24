Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierTariffRow
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
        Private _DetControl1 As Integer = 0
        <DataMember()> _
        Public Property DetControl1() As Integer
            Get
                Return _DetControl1
            End Get
            Set(ByVal value As Integer)
                _DetControl1 = value
            End Set
        End Property

        Private _DetControl2 As Integer = 0
        <DataMember()> _
        Public Property DetControl2() As Integer
            Get
                Return _DetControl2
            End Get
            Set(ByVal value As Integer)
                _DetControl2 = value
            End Set
        End Property

        Private _DetControl3 As Integer = 0
        <DataMember()> _
        Public Property DetControl3() As Integer
            Get
                Return _DetControl3
            End Get
            Set(ByVal value As Integer)
                _DetControl3 = value
            End Set
        End Property

        Private _DetControl4 As Integer = 0
        <DataMember()> _
        Public Property DetControl4() As Integer
            Get
                Return _DetControl4
            End Get
            Set(ByVal value As Integer)
                _DetControl4 = value
            End Set
        End Property

        Private _DetControl5 As Integer = 0
        <DataMember()> _
        Public Property DetControl5() As Integer
            Get
                Return _DetControl5
            End Get
            Set(ByVal value As Integer)
                _DetControl5 = value
            End Set
        End Property

        Private _DetControl6 As Integer = 0
        <DataMember()> _
        Public Property DetControl6() As Integer
            Get
                Return _DetControl6
            End Get
            Set(ByVal value As Integer)
                _DetControl6 = value
            End Set
        End Property

        Private _DetControl7 As Integer = 0
        <DataMember()> _
        Public Property DetControl7() As Integer
            Get
                Return _DetControl7
            End Get
            Set(ByVal value As Integer)
                _DetControl7 = value
            End Set
        End Property

        Private _DetControl8 As Integer = 0
        <DataMember()> _
        Public Property DetControl8() As Integer
            Get
                Return _DetControl8
            End Get
            Set(ByVal value As Integer)
                _DetControl8 = value
            End Set
        End Property

        Private _DetControl9 As Integer = 0
        <DataMember()> _
        Public Property DetControl9() As Integer
            Get
                Return _DetControl9
            End Get
            Set(ByVal value As Integer)
                _DetControl9 = value
            End Set
        End Property

        Private _DetControl10 As Integer = 0
        <DataMember()> _
        Public Property DetControl10() As Integer
            Get
                Return _DetControl10
            End Get
            Set(ByVal value As Integer)
                _DetControl10 = value
            End Set
        End Property

        Private _DetControl11 As Integer = 0
        <DataMember()> _
        Public Property DetControl11() As Integer
            Get
                Return _DetControl11
            End Get
            Set(ByVal value As Integer)
                _DetControl11 = value
            End Set
        End Property

        Private _Selected As Boolean = False
        <DataMember()> _
        Public Property Selected() As Boolean
            Get
                Return _Selected
            End Get
            Set(ByVal value As Boolean)
                _Selected = value
            End Set
        End Property

        Private _ExportFlag As Boolean = False
        <DataMember()> _
        Public Property ExportFlag() As Boolean
            Get
                Return _ExportFlag
            End Get

            Set(ByVal value As Boolean)
                _ExportFlag = value
            End Set
        End Property

        Private _ZipFrom As String = ""
        <DataMember()> _
        Public Property ZipFrom() As String
            Get
                Return _ZipFrom
            End Get

            Set(ByVal value As String)
                _ZipFrom = value
            End Set
        End Property

        Private _ZipTo As String = ""
        <DataMember()> _
        Public Property ZipTo() As String
            Get
                Return _ZipTo
            End Get

            Set(ByVal value As String)
                _ZipTo = value
            End Set
        End Property

        Private _MinVal As Decimal = 0
        <DataMember()> _
        Public Property MinVal() As Decimal
            Get
                Return _MinVal
            End Get

            Set(ByVal value As Decimal)
                _MinVal = value
            End Set
        End Property

        Private _MaxDays As Integer = 0
        <DataMember()> _
        Public Property MaxDays() As Integer
            Get
                Return _MaxDays
            End Get

            Set(ByVal value As Integer)
                _MaxDays = value
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

        Private _Break1 As Decimal = 0
        <DataMember()> _
        Public Property Break1() As Decimal
            Get
                Return _Break1
            End Get

            Set(ByVal value As Decimal)
                _Break1 = value
            End Set
        End Property

        Private _Break2 As Decimal = 0
        <DataMember()> _
        Public Property Break2() As Decimal
            Get
                Return _Break2
            End Get

            Set(ByVal value As Decimal)
                _Break2 = value
            End Set
        End Property

        Private _Break3 As Decimal = 0
        <DataMember()> _
        Public Property Break3() As Decimal
            Get
                Return _Break3
            End Get

            Set(ByVal value As Decimal)
                _Break3 = value
            End Set
        End Property

        Private _Break4 As Decimal = 0
        <DataMember()> _
        Public Property Break4() As Decimal
            Get
                Return _Break4
            End Get

            Set(ByVal value As Decimal)
                _Break4 = value
            End Set
        End Property

        Private _Break5 As Decimal = 0
        <DataMember()> _
        Public Property Break5() As Decimal
            Get
                Return _Break5
            End Get

            Set(ByVal value As Decimal)
                _Break5 = value
            End Set
        End Property

        Private _Break6 As Decimal = 0
        <DataMember()> _
        Public Property Break6() As Decimal
            Get
                Return _Break6
            End Get

            Set(ByVal value As Decimal)
                _Break6 = value
            End Set
        End Property

        Private _Break7 As Decimal = 0
        <DataMember()> _
        Public Property Break7() As Decimal
            Get
                Return _Break7
            End Get

            Set(ByVal value As Decimal)
                _Break7 = value
            End Set
        End Property

        Private _Break8 As Decimal = 0
        <DataMember()> _
        Public Property Break8() As Decimal
            Get
                Return _Break8
            End Get

            Set(ByVal value As Decimal)
                _Break8 = value
            End Set
        End Property

        Private _Break9 As Decimal = 0
        <DataMember()> _
        Public Property Break9() As Decimal
            Get
                Return _Break9
            End Get

            Set(ByVal value As Decimal)
                _Break9 = value
            End Set
        End Property

        Private _Break10 As Decimal = 0
        <DataMember()> _
        Public Property Break10() As Decimal
            Get
                Return _Break10
            End Get

            Set(ByVal value As Decimal)
                _Break10 = value
            End Set
        End Property

        Private _Break11 As Decimal = 0
        <DataMember()> _
        Public Property Break11() As Decimal
            Get
                Return _Break11
            End Get

            Set(ByVal value As Decimal)
                _Break11 = value
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
            Dim instance As New CarrierTariffRow
            instance = DirectCast(MemberwiseClone(), CarrierTariffRow)
            instance.CarrierTariffMatrixDetails = Nothing
            For Each item In CarrierTariffMatrixDetails
                instance.CarrierTariffMatrixDetails.Add(DirectCast(item.Clone, CarrierTariffMatrixDetail))
            Next
            Return instance
        End Function
#End Region

    End Class
End Namespace