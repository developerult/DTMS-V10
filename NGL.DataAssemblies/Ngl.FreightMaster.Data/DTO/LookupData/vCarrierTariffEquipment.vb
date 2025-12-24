Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class vCarrierTariffEquipment
        Inherits DTOBaseClass

#Region " Data Members"

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return _CarrierName
            End Get
            Set(ByVal value As String)
                _CarrierName = value
            End Set
        End Property


        Private _ModeTypeName As String = ""
        <DataMember()> _
        Public Property ModeTypeName() As String
            Get
                Return _ModeTypeName
            End Get
            Set(ByVal value As String)
                _ModeTypeName = value
            End Set
        End Property

        Private _TariffTemp As String = ""
        <DataMember()> _
        Public Property TariffTemp() As String
            Get
                Return _TariffTemp
            End Get
            Set(ByVal value As String)
                _TariffTemp = value
            End Set
        End Property

        Private _CarrTarName As String = ""
        <DataMember()> _
        Public Property CarrTarName() As String
            Get
                Return _CarrTarName
            End Get
            Set(ByVal value As String)
                _CarrTarName = value
            End Set
        End Property

        Private _CarrTarEffDateFrom As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarEffDateFrom() As System.Nullable(Of Date)
            Get
                Return _CarrTarEffDateFrom
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrTarEffDateFrom = value
            End Set
        End Property

        Private _CarrTarRevisionNumber As Integer = 0
        <DataMember()> _
        Public Property CarrTarRevisionNumber() As Integer
            Get
                Return _CarrTarRevisionNumber
            End Get
            Set(ByVal value As Integer)
                _CarrTarRevisionNumber = value
            End Set
        End Property

        Private _CarrTarEquipName As String = ""
        <DataMember()> _
        Public Property CarrTarEquipName() As String
            Get
                Return _CarrTarEquipName
            End Get
            Set(ByVal value As String)
                _CarrTarEquipName = value
            End Set
        End Property

        Private _CarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Private _CarrierNumber As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CarrierNumber() As System.Nullable(Of Integer)
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _CarrierNumber = value
            End Set
        End Property

        Private _CarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarControl() As Integer
            Get
                Return _CarrTarControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarControl = value
            End Set
        End Property

        Private _CarrTarCompControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarCompControl() As Integer
            Get
                Return _CarrTarCompControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarCompControl = value
            End Set
        End Property

        Private _CarrTarID As String = ""
        <DataMember()> _
        Public Property CarrTarID() As String
            Get
                Return _CarrTarID
            End Get
            Set(ByVal value As String)
                _CarrTarID = value
            End Set
        End Property

        Private _CarrTarTempType As Integer = 0
        <DataMember()> _
        Public Property CarrTarTempType() As Integer
            Get
                Return _CarrTarTempType
            End Get
            Set(ByVal value As Integer)
                _CarrTarTempType = value
            End Set
        End Property

        Private _CarrTarEffDateTo As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarEffDateTo() As System.Nullable(Of Date)
            Get
                Return _CarrTarEffDateTo
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrTarEffDateTo = value
            End Set
        End Property

        Private _CarrTarApproved As Boolean = True
        <DataMember()> _
        Public Property CarrTarApproved() As Boolean
            Get
                Return _CarrTarApproved
            End Get
            Set(ByVal value As Boolean)
                _CarrTarApproved = value
            End Set
        End Property

        Private _CarrTarTariffModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarTariffModeTypeControl() As Integer
            Get
                Return _CarrTarTariffModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarTariffModeTypeControl = value
            End Set
        End Property

        Private _CarrTarEquipControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipControl() As Integer
            Get
                Return _CarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarEquipControl = value
            End Set
        End Property

        Private _CarrTarEquipDescription As String = ""
        <DataMember()> _
        Public Property CarrTarEquipDescription() As String
            Get
                Return _CarrTarEquipDescription
            End Get
            Set(ByVal value As String)
                _CarrTarEquipDescription = value
            End Set
        End Property

        Private _CarrTarEquipCasesMinimum As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipCasesMinimum() As Double
            Get
                Return _CarrTarEquipCasesMinimum
            End Get
            Set(ByVal value As Double)
                _CarrTarEquipCasesMinimum = value
            End Set
        End Property

        Private _CarrTarEquipCasesMaximum As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipCasesMaximum() As Double
            Get
                Return _CarrTarEquipCasesMaximum
            End Get
            Set(ByVal value As Double)
                _CarrTarEquipCasesMaximum = value
            End Set
        End Property

        Private _CarrTarEquipWgtMinimum As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipWgtMinimum() As Double
            Get
                Return _CarrTarEquipWgtMinimum
            End Get
            Set(ByVal value As Double)
                _CarrTarEquipWgtMinimum = value
            End Set
        End Property

        Private _CarrTarEquipWgtMaximum As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipWgtMaximum() As Double
            Get
                Return _CarrTarEquipWgtMaximum
            End Get
            Set(ByVal value As Double)
                _CarrTarEquipWgtMaximum = value
            End Set
        End Property

        Private _CarrTarEquipCubesMinimum As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipCubesMinimum() As Double
            Get
                Return _CarrTarEquipCubesMinimum
            End Get
            Set(ByVal value As Double)
                _CarrTarEquipCubesMinimum = value
            End Set
        End Property

        Private _CarrTarEquipCubesMaximum As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipCubesMaximum() As Double
            Get
                Return _CarrTarEquipCubesMaximum
            End Get
            Set(ByVal value As Double)
                _CarrTarEquipCubesMaximum = value
            End Set
        End Property

        Private _CarrTarEquipPltsMinimum As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipPltsMinimum() As Integer
            Get
                Return _CarrTarEquipPltsMinimum
            End Get
            Set(ByVal value As Integer)
                _CarrTarEquipPltsMinimum = value
            End Set
        End Property

        Private _CarrTarEquipPltsMaximum As Integer = 0
        <DataMember()>
        Public Property CarrTarEquipPltsMaximum() As Integer
            Get
                Return _CarrTarEquipPltsMaximum
            End Get
            Set(ByVal value As Integer)
                _CarrTarEquipPltsMaximum = value
            End Set
        End Property


        Private _CarrTarEquipMultiOrigRating As Boolean? = False
        ''' <summary>
        ''' Multi Origin Rating Flag
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
        ''' </remarks>
        <DataMember()>
        Public Property CarrTarEquipMultiOrigRating() As Boolean?
            Get
                Return _CarrTarEquipMultiOrigRating
            End Get
            Set(ByVal value As Boolean?)
                If (Me._CarrTarEquipMultiOrigRating <> value) Then
                    Me._CarrTarEquipMultiOrigRating = value
                    Me.SendPropertyChanged("CarrTarEquipMultiOrigRating")
                End If
            End Set
        End Property


#End Region



#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New vCarrierTariffEquipment
            instance = DirectCast(MemberwiseClone(), vCarrierTariffEquipment)
            Return instance
        End Function

#End Region
    End Class
End Namespace