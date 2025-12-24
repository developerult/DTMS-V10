Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierFuelAdEx
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrFuelAdExControl As Integer = 0
        <DataMember()> _
        Public Property CarrFuelAdExControl() As Integer
            Get
                Return _CarrFuelAdExControl
            End Get
            Set(ByVal value As Integer)
                _CarrFuelAdExControl = value
            End Set
        End Property

        Private _CarrFuelAdExCarrFuelAdContol As Integer = 0
        <DataMember()> _
        Public Property CarrFuelAdExCarrFuelAdContol() As Integer
            Get
                Return _CarrFuelAdExCarrFuelAdContol
            End Get
            Set(ByVal value As Integer)
                _CarrFuelAdExCarrFuelAdContol = value
            End Set
        End Property

        Private _CarrFuelAdExState As String = ""
        <DataMember()> _
        Public Property CarrFuelAdExState() As String
            Get
                Return Left(_CarrFuelAdExState, 2)
            End Get
            Set(ByVal value As String)
                _CarrFuelAdExState = Left(value, 2)
            End Set
        End Property

        Private _CarrFuelAdExRatePerMile As Decimal = 0
        <DataMember()> _
        Public Property CarrFuelAdExRatePerMile() As Decimal
            Get
                Return _CarrFuelAdExRatePerMile
            End Get
            Set(ByVal value As Decimal)
                _CarrFuelAdExRatePerMile = value
            End Set
        End Property

        Private _CarrFuelAdExPercent As Decimal = 0
        <DataMember()> _
        Public Property CarrFuelAdExPercent() As Decimal
            Get
                Return _CarrFuelAdExPercent
            End Get
            Set(ByVal value As Decimal)
                _CarrFuelAdExPercent = value
            End Set
        End Property

        Private _CarrFuelAdExEffDate As Date = Now
        <DataMember()> _
        Public Property CarrFuelAdExEffDate() As Date
            Get
                Return _CarrFuelAdExEffDate
            End Get
            Set(ByVal value As Date)
                _CarrFuelAdExEffDate = value
            End Set
        End Property

        Private _CarrFuelAdExModUser As String = ""
        <DataMember()> _
        Public Property CarrFuelAdExModUser() As String
            Get
                Return Left(_CarrFuelAdExModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrFuelAdExModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrFuelAdExModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrFuelAdExModDate() As System.Nullable(Of Date)
            Get
                Return _CarrFuelAdExModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrFuelAdExModDate = value
            End Set
        End Property

        Private _CarrFuelAdExUpdated As Byte()
        <DataMember()>
        Public Property CarrFuelAdExUpdated() As Byte()
            Get
                Return _CarrFuelAdExUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrFuelAdExUpdated = value
            End Set
        End Property
        'Modified by RHR for v-8.5.4.005 on 03/27/2024 new carrier specific zone logic
        Private _CarrFuelAdExUseExceptionDefault As Boolean? = False
        <DataMember()>
        Public Property CarrFuelAdExUseExceptionDefault() As Boolean?
            Get
                Return _CarrFuelAdExUseExceptionDefault
            End Get
            Set(ByVal value As Boolean?)
                _CarrFuelAdExUseExceptionDefault = value
            End Set
        End Property

        Private _CarrFuelAdExCalcAvgWithNatAverage As Boolean? = False
        <DataMember()>
        Public Property CarrFuelAdExCalcAvgWithNatAverage() As Boolean?
            Get
                Return _CarrFuelAdExCalcAvgWithNatAverage
            End Get
            Set(ByVal value As Boolean?)
                _CarrFuelAdExCalcAvgWithNatAverage = value
            End Set
        End Property

        Private _CarrFuelAdExCalcAvgWithZone1 As Boolean? = False
        <DataMember()>
        Public Property CarrFuelAdExCalcAvgWithZone1() As Boolean?
            Get
                Return _CarrFuelAdExCalcAvgWithZone1
            End Get
            Set(ByVal value As Boolean?)
                _CarrFuelAdExCalcAvgWithZone1 = value
            End Set
        End Property

        Private _CarrFuelAdExCalcAvgWithZone2 As Boolean? = False
        <DataMember()>
        Public Property CarrFuelAdExCalcAvgWithZone2() As Boolean?
            Get
                Return _CarrFuelAdExCalcAvgWithZone2
            End Get
            Set(ByVal value As Boolean?)
                _CarrFuelAdExCalcAvgWithZone2 = value
            End Set
        End Property

        Private _CarrFuelAdExCalcAvgWithZone3 As Boolean? = False
        <DataMember()>
        Public Property CarrFuelAdExCalcAvgWithZone3() As Boolean?
            Get
                Return _CarrFuelAdExCalcAvgWithZone3
            End Get
            Set(ByVal value As Boolean?)
                _CarrFuelAdExCalcAvgWithZone3 = value
            End Set
        End Property


        Private _CarrFuelAdExCalcAvgWithZone4 As Boolean? = False
        <DataMember()>
        Public Property CarrFuelAdExCalcAvgWithZone4() As Boolean?
            Get
                Return _CarrFuelAdExCalcAvgWithZone4
            End Get
            Set(ByVal value As Boolean?)
                _CarrFuelAdExCalcAvgWithZone4 = value
            End Set
        End Property


        Private _CarrFuelAdExCalcAvgWithZone5 As Boolean? = False
        <DataMember()>
        Public Property CarrFuelAdExCalcAvgWithZone5() As Boolean?
            Get
                Return _CarrFuelAdExCalcAvgWithZone5
            End Get
            Set(ByVal value As Boolean?)
                _CarrFuelAdExCalcAvgWithZone5 = value
            End Set
        End Property



        Private _CarrFuelAdExCalcAvgWithZone6 As Boolean? = False
        <DataMember()>
        Public Property CarrFuelAdExCalcAvgWithZone6() As Boolean?
            Get
                Return _CarrFuelAdExCalcAvgWithZone6
            End Get
            Set(ByVal value As Boolean?)
                _CarrFuelAdExCalcAvgWithZone6 = value
            End Set
        End Property


        Private _CarrFuelAdExCalcAvgWithZone7 As Boolean? = False
        <DataMember()>
        Public Property CarrFuelAdExCalcAvgWithZone7() As Boolean?
            Get
                Return _CarrFuelAdExCalcAvgWithZone7
            End Get
            Set(ByVal value As Boolean?)
                _CarrFuelAdExCalcAvgWithZone7 = value
            End Set
        End Property



        Private _CarrFuelAdExCalcAvgWithZone8 As Boolean? = False
        <DataMember()>
        Public Property CarrFuelAdExCalcAvgWithZone8() As Boolean?
            Get
                Return _CarrFuelAdExCalcAvgWithZone8
            End Get
            Set(ByVal value As Boolean?)
                _CarrFuelAdExCalcAvgWithZone8 = value
            End Set
        End Property



        Private _CarrFuelAdExCalcAvgWithZone9 As Boolean? = False
        <DataMember()>
        Public Property CarrFuelAdExCalcAvgWithZone9() As Boolean?
            Get
                Return _CarrFuelAdExCalcAvgWithZone9
            End Get
            Set(ByVal value As Boolean?)
                _CarrFuelAdExCalcAvgWithZone9 = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierFuelAdEx
            instance = DirectCast(MemberwiseClone(), CarrierFuelAdEx)
            Return instance
        End Function

#End Region

    End Class
End Namespace