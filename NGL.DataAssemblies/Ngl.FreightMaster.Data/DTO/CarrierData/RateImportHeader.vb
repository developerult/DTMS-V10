Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports NGL.Core.ChangeTracker

'Added By LVV on 9/13/16 for v-7.0.5.110 Tariff Import Tool

Namespace DataTransferObjects
    <DataContract()> _
    Public Class RateImportHeader
        Inherits DTOBaseClass

#Region "Data Members"

        Private _CarrierControl As Integer = -1
        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrierControl <> value) Then
                    Me._CarrierControl = value
                    Me.SendPropertyChanged("CarrierControl")
                End If
            End Set
        End Property

        Private _CompanyControl As Integer = -1
        <DataMember()> _
        Public Property CompanyControl() As Integer
            Get
                Return _CompanyControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CompanyControl <> value) Then
                    Me._CompanyControl = value
                    Me.SendPropertyChanged("CompanyControl")
                End If
            End Set
        End Property

        Private _EffectiveDate As Nullable(Of Date) = Nothing
        ''' <summary>
        ''' Tariff Effective Date Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-7.0.6.101 on 1/25/2016 fix issue with blank Effective Date from WCF
        ''' </remarks>
        <DataMember()> _
        Public Property EffectiveDate() As Nullable(Of Date)
            Get
                Return _EffectiveDate
            End Get
            Set(ByVal value As Nullable(Of Date))
                'Modified by RHR for v-7.0.6.101 on 1/25/2016 fix issue with blank Effective Date from WCF
                _EffectiveDate = value
                'If (Me._EffectiveDate <> value) Then
                '    Me._EffectiveDate = value
                '    Me.SendPropertyChanged("EffectiveDate")
                'End If
            End Set
        End Property

        Private _EffectiveTo As Nullable(Of Date) = Nothing
        ''' <summary>
        ''' Carrier Tariff Effective To date
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-7.0.6.101 on 1/25/2016 fix issue with blank Value from WCF
        ''' </remarks>
        <DataMember()> _
        Public Property EffectiveTo() As Nullable(Of Date)
            Get
                Return _EffectiveTo
            End Get
            Set(ByVal value As Nullable(Of Date))
                'Modified by RHR for v-7.0.6.101 on 1/25/2016 fix issue with blank Value from WCF
                _EffectiveTo = value
                'If (Me._EffectiveTo <> value) Then
                '    Me._EffectiveTo = value
                '    Me.SendPropertyChanged("EffectiveTo")
                'End If
            End Set
        End Property

        Private _CarrTarControl As Integer = -1
        <DataMember()> _
        Public Property CarrTarControl() As Integer
            Get
                Return _CarrTarControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarControl <> value) Then
                    Me._CarrTarControl = value
                    Me.SendPropertyChanged("CarrTarControl")
                End If
            End Set
        End Property

        Private _CarrTarID As String = ""
        <DataMember()> _
        Public Property CarrTarID() As String
            Get
                Return _CarrTarID
            End Get
            Set(ByVal value As String)
                If (Me._CarrTarID <> value) Then
                    Me._CarrTarID = value
                    Me.SendPropertyChanged("CarrTarID")
                End If
            End Set
        End Property

        Private _CarrTarEquipControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipControl() As Integer
            Get
                Return _CarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipControl <> value) Then
                    Me._CarrTarEquipControl = value
                    Me.SendPropertyChanged("CarrTarEquipControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipName As String = ""
        <DataMember()> _
        Public Property CarrTarEquipName() As String
            Get
                Return _CarrTarEquipName
            End Get
            Set(ByVal value As String)
                If (Me._CarrTarEquipName <> value) Then
                    Me._CarrTarEquipName = value
                    Me.SendPropertyChanged("CarrTarEquipName")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatName As String = ""
        <DataMember()> _
        Public Property CarrTarEquipMatName() As String
            Get
                Return _CarrTarEquipMatName
            End Get
            Set(ByVal value As String)
                If (Me._CarrTarEquipMatName <> value) Then
                    Me._CarrTarEquipMatName = value
                    Me.SendPropertyChanged("CarrTarEquipMatName")
                End If
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

        Private _CarrTarMatBPControl As Integer
        <DataMember()> _
        Public Property CarrTarMatBPControl() As Integer
            Get
                Return _CarrTarMatBPControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPControl <> value) Then
                    Me._CarrTarMatBPControl = value
                    Me.SendPropertyChanged("CarrTarMatBPControl")
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
        <DataMember()> _
        Public Property Message() As String
            Get
                Return _Message
            End Get
            Set(ByVal value As String)
                _Message = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New RateImportHeader
            instance = DirectCast(MemberwiseClone(), RateImportHeader)
            Return instance
        End Function

#End Region

    End Class
End Namespace





