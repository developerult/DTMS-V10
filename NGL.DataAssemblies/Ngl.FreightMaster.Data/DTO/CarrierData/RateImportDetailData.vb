Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV on 9/13/16 for v-7.0.5.110 Tariff Import Tool

Namespace DataTransferObjects
    <DataContract()> _
    Public Class RateImportDetailData
        Inherits DTOBaseClass

#Region " Data Members"

        Private _TarID As String = ""
        <DataMember()> _
        Public Property TarID() As String
            Get
                Return _TarID
            End Get
            Set(ByVal value As String)
                value = If(Not value Is Nothing, value.Replace("'", ""), value)
                If (Me._TarID <> value) Then
                    Me._TarID = value
                    Me.SendPropertyChanged("TarID")
                End If
            End Set
        End Property

        Private _EquipmentName As String = ""
        <DataMember()> _
        Public Property EquipName() As String
            Get
                Return _EquipmentName
            End Get
            Set(ByVal value As String)
                value = If(Not value Is Nothing, value.Replace("'", ""), value)
                If (Me._EquipmentName <> value) Then
                    Me._EquipmentName = value
                    Me.SendPropertyChanged("EquipmentName")
                End If
            End Set
        End Property

        Private _EffectiveFrom As Nullable(Of Date) = Nothing
        ''' <summary>
        ''' Carrier Tariff Effective From Date
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-7.0.6.101 on 1/25/2016 fix issue with blank Value from WCF
        ''' </remarks>
        <DataMember()> _
        Public Property EffDateFrom() As Nullable(Of Date)
            Get
                Return _EffectiveFrom
            End Get
            Set(ByVal value As Nullable(Of Date))
                'Modified by RHR for v-7.0.6.101 on 1/25/2016 fix issue with blank Value from WCF
                Me._EffectiveFrom = value
                'If (Me._EffectiveFrom <> value) Then
                '    Me._EffectiveFrom = value
                '    Me.SendPropertyChanged("EffectiveFrom")
                'End If
            End Set
        End Property

        Private _EffectiveTo As Nullable(Of Date) = Nothing
        ''' <summary>
        ''' Carrier Tariff Effective To Date
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-7.0.6.101 on 1/25/2016 fix issue with blank Value from WCF
        ''' </remarks>
        <DataMember()> _
        Public Property EffDateTo() As Nullable(Of Date)
            Get
                Return _EffectiveTo
            End Get
            Set(ByVal value As Nullable(Of Date))
                'Modified by RHR for v-7.0.6.101 on 1/25/2016 fix issue with blank Value from WCF
                Me._EffectiveTo = value
                'If (Me._EffectiveTo <> value) Then
                '    Me._EffectiveTo = value
                '    Me.SendPropertyChanged("EffectiveTo")
                'End If
            End Set
        End Property

        Private _ClassTypeControl As Integer
        <DataMember()> _
        Public Property ClassTypeControl() As Integer
            Get
                Return _ClassTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._ClassTypeControl <> value) Then
                    Me._ClassTypeControl = value
                    Me.SendPropertyChanged("ClassTypeControl")
                End If
            End Set
        End Property

        Private _RateTypeControl As Integer
        <DataMember()> _
        Public Property RateTypeControl() As Integer
            Get
                Return _RateTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._RateTypeControl <> value) Then
                    Me._RateTypeControl = value
                    Me.SendPropertyChanged("RateTypeControl")
                End If
            End Set
        End Property

        Private _BracketTypeControl As Integer
        <DataMember()> _
        Public Property BracketTypeControl() As Integer
            Get
                Return _BracketTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._BracketTypeControl <> value) Then
                    Me._BracketTypeControl = value
                    Me.SendPropertyChanged("BracketTypeControl")
                End If
            End Set
        End Property

        Private _Country As String = ""
        <DataMember()> _
        Public Property Country() As String
            Get
                Return _Country
            End Get
            Set(ByVal value As String)
                value = If(Not value Is Nothing, value.Replace("'", ""), value)
                If (Me._Country <> value) Then
                    Me._Country = value
                    Me.SendPropertyChanged("Country")
                End If
            End Set
        End Property

        Private _State As String = ""
        <DataMember()> _
        Public Property State() As String
            Get
                Return _State
            End Get
            Set(ByVal value As String)
                value = If(Not value Is Nothing, value.Replace("'", ""), value)
                If (Me._State <> value) Then
                    Me._State = value
                    Me.SendPropertyChanged("State")
                End If
            End Set
        End Property

        Private _City As String = ""
        <DataMember()> _
        Public Property City() As String
            Get
                Return _City
            End Get
            Set(ByVal value As String)
                value = If(Not value Is Nothing, value.Replace("'", ""), value)
                If (Me._City <> value) Then
                    Me._City = value
                    Me.SendPropertyChanged("City")
                End If
            End Set
        End Property

        Private _FromZip As String = ""
        <DataMember()> _
        Public Property FromZip() As String
            Get
                Return _FromZip
            End Get
            Set(ByVal value As String)
                value = If(Not value Is Nothing, value.Replace("'", ""), value)
                If (Me._FromZip <> value) Then
                    Me._FromZip = value
                    Me.SendPropertyChanged("FromZip")
                End If
            End Set
        End Property

        Private _ToZip As String = ""
        <DataMember()> _
        Public Property ToZip() As String
            Get
                Return _ToZip
            End Get
            Set(ByVal value As String)
                value = If(Not value Is Nothing, value.Replace("'", ""), value)
                If (Me._ToZip <> value) Then
                    Me._ToZip = value
                    Me.SendPropertyChanged("ToZip")
                End If
            End Set
        End Property

        Private _Lane As Integer
        <DataMember()> _
        Public Property Lane() As Integer
            Get
                Return _Lane
            End Get
            Set(ByVal value As Integer)
                If (Me._Lane <> value) Then
                    Me._Lane = value
                    Me.SendPropertyChanged("Lane")
                End If
            End Set
        End Property

        Private _Class As String = ""
        <DataMember()> _
        Public Property TariffClass() As String
            Get
                Return _Class
            End Get
            Set(ByVal value As String)
                value = If(Not value Is Nothing, value.Replace("'", ""), value)
                If (Me._Class <> value) Then
                    Me._Class = value
                    Me.SendPropertyChanged("Class")
                End If
            End Set
        End Property

        Private _Min As Decimal
        <DataMember()> _
        Public Property Min() As Decimal
            Get
                Return _Min
            End Get
            Set(ByVal value As Decimal)
                If (Me._Min <> value) Then
                    Me._Min = value
                    Me.SendPropertyChanged("Min")
                End If
            End Set
        End Property

        Private _MaxDays As Integer
        <DataMember()> _
        Public Property MaxDays() As Integer
            Get
                Return _MaxDays
            End Get
            Set(ByVal value As Integer)
                If (Me._MaxDays <> value) Then
                    Me._MaxDays = value
                    Me.SendPropertyChanged("MaxDays")
                End If
            End Set
        End Property

        Private _Val1 As Decimal
        <DataMember()> _
        Public Property Val1() As Decimal
            Get
                Return _Val1
            End Get
            Set(ByVal value As Decimal)
                If (Me._Val1 <> value) Then
                    Me._Val1 = value
                    Me.SendPropertyChanged("Val1")
                End If
            End Set
        End Property

        Private _Val2 As Decimal
        <DataMember()> _
        Public Property Val2() As Decimal
            Get
                Return _Val2
            End Get
            Set(ByVal value As Decimal)
                If (Me._Val2 <> value) Then
                    Me._Val2 = value
                    Me.SendPropertyChanged("Val2")
                End If
            End Set
        End Property

        Private _Val3 As Decimal
        <DataMember()> _
        Public Property Val3() As Decimal
            Get
                Return _Val3
            End Get
            Set(ByVal value As Decimal)
                If (Me._Val3 <> value) Then
                    Me._Val3 = value
                    Me.SendPropertyChanged("Val3")
                End If
            End Set
        End Property

        Private _Val4 As Decimal
        <DataMember()> _
        Public Property Val4() As Decimal
            Get
                Return _Val4
            End Get
            Set(ByVal value As Decimal)
                If (Me._Val4 <> value) Then
                    Me._Val4 = value
                    Me.SendPropertyChanged("Val4")
                End If
            End Set
        End Property

        Private _Val5 As Decimal
        <DataMember()> _
        Public Property Val5() As Decimal
            Get
                Return _Val5
            End Get
            Set(ByVal value As Decimal)
                If (Me._Val5 <> value) Then
                    Me._Val5 = value
                    Me.SendPropertyChanged("Val5")
                End If
            End Set
        End Property

        Private _Val6 As Decimal
        <DataMember()> _
        Public Property Val6() As Decimal
            Get
                Return _Val6
            End Get
            Set(ByVal value As Decimal)
                If (Me._Val6 <> value) Then
                    Me._Val6 = value
                    Me.SendPropertyChanged("Val6")
                End If
            End Set
        End Property

        Private _Val7 As Decimal
        <DataMember()> _
        Public Property Val7() As Decimal
            Get
                Return _Val7
            End Get
            Set(ByVal value As Decimal)
                If (Me._Val7 <> value) Then
                    Me._Val7 = value
                    Me.SendPropertyChanged("Val7")
                End If
            End Set
        End Property

        Private _Val8 As Decimal
        <DataMember()> _
        Public Property Val8() As Decimal
            Get
                Return _Val8
            End Get
            Set(ByVal value As Decimal)
                If (Me._Val8 <> value) Then
                    Me._Val8 = value
                    Me.SendPropertyChanged("Val8")
                End If
            End Set
        End Property

        Private _Val9 As Decimal
        <DataMember()> _
        Public Property Val9() As Decimal
            Get
                Return _Val9
            End Get
            Set(ByVal value As Decimal)
                If (Me._Val9 <> value) Then
                    Me._Val9 = value
                    Me.SendPropertyChanged("Val9")
                End If
            End Set
        End Property

        Private _Val10 As Decimal
        <DataMember()> _
        Public Property Val10() As Decimal
            Get
                Return _Val10
            End Get
            Set(ByVal value As Decimal)
                If (Me._Val10 <> value) Then
                    Me._Val10 = value
                    Me.SendPropertyChanged("Val10")
                End If
            End Set
        End Property

        Private _Success As Boolean = False
        <DataMember()> _
        Public Property Success() As Boolean
            Get
                Return _Success
            End Get
            Set(ByVal value As Boolean)
                _Success = value
            End Set
        End Property

        Private _Message As String = ""
        <DataMember()>
        Public Property Message() As String
            Get
                Return _Message
            End Get
            Set(ByVal value As String)
                _Message = value
            End Set
        End Property

        Private _CarrTarEquipMatOrigZip As String = ""
        ''' <summary>
        ''' Optional Origin Zip Code for Rate Shop
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
        ''' </remarks>
        <DataMember()>
        Public Property CarrTarEquipMatOrigZip() As String
            Get
                Return Left(_CarrTarEquipMatOrigZip, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipMatOrigZip, value) = False) Then
                    Me._CarrTarEquipMatOrigZip = Left(value, 20)
                    Me.SendPropertyChanged("CarrTarEquipMatOrigZip")
                End If
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New RateImportDetailData
            instance = DirectCast(MemberwiseClone(), RateImportDetailData)
            Return instance
        End Function

#End Region

    End Class
End Namespace

