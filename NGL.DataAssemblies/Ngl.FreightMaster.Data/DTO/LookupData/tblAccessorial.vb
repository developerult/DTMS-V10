Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblAccessorial
        Inherits DTOBaseClass


#Region " Data Members"
        Private _AccessorialCode As Integer
        <DataMember()> _
        Public Property AccessorialCode() As Integer
            Get
                Return Me._AccessorialCode
            End Get
            Set(ByVal value As Integer)
                If ((Me._AccessorialCode = value) _
                   = False) Then
                    Me._AccessorialCode = value
                    Me.SendPropertyChanged("AccessorialCode")
                End If
            End Set
        End Property

        Private _AccessorialName As String = ""
        <DataMember()> _
        Public Property AccessorialName() As String
            Get
                Return Me._AccessorialName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AccessorialName, value) = False) Then
                    Me._AccessorialName = value
                    Me.SendPropertyChanged("AccessorialName")
                End If
            End Set
        End Property

        Private _AccessorialDescription As String = ""
        <DataMember()> _
        Public Property AccessorialDescription() As String
            Get
                Return Me._AccessorialDescription
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AccessorialDescription, value) = False) Then
                    Me._AccessorialDescription = value
                    Me.SendPropertyChanged("AccessorialDescription")
                End If
            End Set
        End Property

        Private _AccessorialVariableCode As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property AccessorialVariableCode() As System.Nullable(Of Integer)
            Get
                Return Me._AccessorialVariableCode
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._AccessorialVariableCode.Equals(value) = False) Then
                    Me._AccessorialVariableCode = value
                    Me.SendPropertyChanged("AccessorialVariableCode")
                End If
            End Set
        End Property

        Private _AccessorialModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AccessorialModDate() As System.Nullable(Of Date)
            Get
                Return Me._AccessorialModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._AccessorialModDate.Equals(value) = False) Then
                    Me._AccessorialModDate = value
                    Me.SendPropertyChanged("AccessorialModDate")
                End If
            End Set
        End Property

        Private _AccessorialModUser As String = ""
        <DataMember()> _
        Public Property AccessorialModUser() As String
            Get
                Return Me._AccessorialModUser
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AccessorialModUser, value) = False) Then
                    Me._AccessorialModUser = value
                    Me.SendPropertyChanged("AccessorialModUser")
                End If
            End Set
        End Property

        Private _AccessorialUpdated As Byte()
        <DataMember()> _
        Public Property AccessorialUpdated() As Byte()
            Get
                Return _AccessorialUpdated
            End Get
            Set(ByVal value As Byte())
                _AccessorialUpdated = value
            End Set
        End Property

        Private _AccessorialVisible As Boolean = True
        <DataMember()> _
        Public Property AccessorialVisible() As Boolean
            Get
                Return Me._AccessorialVisible
            End Get
            Set(ByVal value As Boolean)
                If ((Me._AccessorialVisible = value) _
                   = False) Then
                    Me._AccessorialVisible = value
                End If
            End Set
        End Property

        Private _AccessorialAutoApprove As Boolean = False
        <DataMember()> _
        Public Property AccessorialAutoApprove() As Boolean
            Get
                Return Me._AccessorialAutoApprove
            End Get
            Set(ByVal value As Boolean)
                If ((Me._AccessorialAutoApprove = value) _
                   = False) Then
                    Me._AccessorialAutoApprove = value
                End If
            End Set
        End Property

        Private _AccessorialAllowCarrierUpdates As Boolean = False
        <DataMember()> _
        Public Property AccessorialAllowCarrierUpdates() As Boolean
            Get
                Return Me._AccessorialAllowCarrierUpdates
            End Get
            Set(ByVal value As Boolean)
                If ((Me._AccessorialAllowCarrierUpdates = value) _
                   = False) Then
                    Me._AccessorialAllowCarrierUpdates = value
                End If
            End Set
        End Property

        Private _AccessorialCaption As String = ""
        <DataMember()> _
        Public Property AccessorialCaption() As String
            Get
                Return Me._AccessorialCaption
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AccessorialCaption, value) = False) Then
                    Me._AccessorialCaption = value
                End If
            End Set
        End Property

        Private _AccessorialEDICode As String = ""
        <DataMember()> _
        Public Property AccessorialEDICode() As String
            Get
                Return Me._AccessorialEDICode
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AccessorialEDICode, value) = False) Then
                    Me._AccessorialEDICode = value
                End If
            End Set
        End Property

        Private _AccessorialTaxable As Boolean = True
        <DataMember()> _
        Public Property AccessorialTaxable() As Boolean
            Get
                Return Me._AccessorialTaxable
            End Get
            Set(ByVal value As Boolean)
                If ((Me._AccessorialTaxable = value) _
                   = False) Then
                    Me._AccessorialTaxable = value
                End If
            End Set
        End Property

        Private _AccessorialIsTax As Boolean = False
        <DataMember()> _
        Public Property AccessorialIsTax() As Boolean
            Get
                Return Me._AccessorialIsTax
            End Get
            Set(ByVal value As Boolean)
                If ((Me._AccessorialIsTax = value) _
                   = False) Then
                    Me._AccessorialIsTax = value
                End If
            End Set
        End Property

        Private _AccessorialTaxSortOrder As Integer = 0
        <DataMember()> _
        Public Property AccessorialTaxSortOrder() As Integer
            Get
                Return Me._AccessorialTaxSortOrder
            End Get
            Set(ByVal value As Integer)
                If ((Me._AccessorialTaxSortOrder = value) _
                   = False) Then
                    Me._AccessorialTaxSortOrder = value
                End If
            End Set
        End Property

        Private _AccessorialBOLText As String = ""
        <DataMember()> _
        Public Property AccessorialBOLText() As String
            Get
                Return Left(_AccessorialBOLText, 4000)
            End Get
            Set(ByVal value As String)
                _AccessorialBOLText = Left(value, 4000)
            End Set
        End Property

        Private _AccessorialBOLPlacement As String = ""
        <DataMember()> _
        Public Property AccessorialBOLPlacement() As String
            Get
                Return Left(_AccessorialBOLPlacement, 100)
            End Get
            Set(ByVal value As String)
                _AccessorialBOLPlacement = Left(value, 100)
            End Set
        End Property

        Private _AccessorialAccessorialFeeAllocationTypeControl As Integer = 1
        <DataMember()> _
        Public Property AccessorialAccessorialFeeAllocationTypeControl() As Integer
            Get
                Return _AccessorialAccessorialFeeAllocationTypeControl
            End Get
            Set(ByVal value As Integer)
                _AccessorialAccessorialFeeAllocationTypeControl = value
            End Set
        End Property

        Private _AccessorialTarBracketTypeControl As Integer = 4
        <DataMember()> _
        Public Property AccessorialTarBracketTypeControl() As Integer
            Get
                Return _AccessorialTarBracketTypeControl
            End Get
            Set(ByVal value As Integer)
                _AccessorialTarBracketTypeControl = value
            End Set
        End Property

        Private _AccessorialAccessorialFeeCalcTypeControl As Integer = 1
        <DataMember()> _
        Public Property AccessorialAccessorialFeeCalcTypeControl() As Integer
            Get
                Return _AccessorialAccessorialFeeCalcTypeControl
            End Get
            Set(ByVal value As Integer)
                _AccessorialAccessorialFeeCalcTypeControl = value
            End Set
        End Property

        Private _AccessorialProfileSpecific As Boolean = False
        <DataMember()> _
        Public Property AccessorialProfileSpecific() As Boolean
            Get
                Return _AccessorialProfileSpecific
            End Get
            Set(ByVal value As Boolean)
                _AccessorialProfileSpecific = value
            End Set
        End Property

        Private _AccessorialHDMControl As Integer
        <DataMember()>
        Public Property AccessorialHDMControl() As Integer
            Get
                Return _AccessorialHDMControl
            End Get
            Set(ByVal value As Integer)
                _AccessorialHDMControl = value
            End Set
        End Property

        'Added By LVV On 10/25/2018 For v-8.3
        Private _AccessorialMinimum As Decimal = 0
        <DataMember()>
        Public Property AccessorialMinimum() As Decimal
            Get
                Return _AccessorialMinimum
            End Get
            Set(ByVal value As Decimal)
                _AccessorialMinimum = value
            End Set
        End Property

        Private _AccessorialVariable As Double = 0
        <DataMember()>
        Public Property AccessorialVariable() As Double
            Get
                Return _AccessorialVariable
            End Get
            Set(ByVal value As Double)
                _AccessorialVariable = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblAccessorial
            instance = DirectCast(MemberwiseClone(), tblAccessorial)
            Return instance
        End Function

#End Region

    End Class
End Namespace