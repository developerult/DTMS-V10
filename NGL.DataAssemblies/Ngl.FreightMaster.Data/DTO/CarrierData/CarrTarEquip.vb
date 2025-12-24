Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarEquip
        Inherits DTOBaseClass

#Region " Data Members"
        Private _CarrTarEquipControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipControl() As Integer
            Get
                Return _CarrTarEquipControl
            End Get
            Set(value As Integer)
                If (Me._CarrTarEquipControl <> value) Then
                    Me._CarrTarEquipControl = value
                    Me.SendPropertyChanged("CarrTarEquipControl")
                End If
            End Set
        End Property


        Private _CarrTarEquipCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipCarrTarControl() As Integer
            Get
                Return _CarrTarEquipCarrTarControl
            End Get
            Set(value As Integer)
                If (Me._CarrTarEquipCarrTarControl <> value) Then
                    Me._CarrTarEquipCarrTarControl = value
                    Me.SendPropertyChanged("CarrTarEquipCarrTarControl")
                End If
            End Set
        End Property


        Private _CarrTarEquipCarrierEquipControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipCarrierEquipControl() As Integer
            Get
                Return _CarrTarEquipCarrierEquipControl
            End Get
            Set(value As Integer)
                If (Me._CarrTarEquipCarrierEquipControl <> value) Then
                    Me._CarrTarEquipCarrierEquipControl = value
                    Me.SendPropertyChanged("CarrTarEquipCarrierEquipControl")
                End If
            End Set
        End Property


        Private _CarrTarEquipName As String = ""
        <DataMember()> _
        Public Property CarrTarEquipName() As String
            Get
                Return Left(_CarrTarEquipName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipName, value) = False) Then
                    Me._CarrTarEquipName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarEquipName")
                End If
            End Set
        End Property

        Private _CarrTarEquipDescription As String = ""
        <DataMember()> _
        Public Property CarrTarEquipDescription() As String
            Get
                Return Left(_CarrTarEquipDescription, 255)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipDescription, value) = False) Then
                    Me._CarrTarEquipDescription = Left(value, 255)
                    Me.SendPropertyChanged("CarrTarEquipDescription")
                End If
            End Set
        End Property

        Private _CarrTarEquipPUMon As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipPUMon() As Boolean
            Get
                Return _CarrTarEquipPUMon
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipPUMon <> value) Then
                    Me._CarrTarEquipPUMon = value
                    Me.SendPropertyChanged("CarrTarEquipPUMon")
                End If
            End Set
        End Property

        Private _CarrTarEquipPUTue As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipPUTue() As Boolean
            Get
                Return _CarrTarEquipPUTue
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipPUTue <> value) Then
                    Me._CarrTarEquipPUTue = value
                    Me.SendPropertyChanged("CarrTarEquipPUTue")
                End If
            End Set
        End Property

        Private _CarrTarEquipPUWed As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipPUWed() As Boolean
            Get
                Return _CarrTarEquipPUWed
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipPUWed <> value) Then
                    Me._CarrTarEquipPUWed = value
                    Me.SendPropertyChanged("CarrTarEquipPUWed")
                End If
            End Set
        End Property

        Private _CarrTarEquipPUThu As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipPUThu() As Boolean
            Get
                Return _CarrTarEquipPUThu
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipPUThu <> value) Then
                    Me._CarrTarEquipPUThu = value
                    Me.SendPropertyChanged("CarrTarEquipPUThu")
                End If
            End Set
        End Property

        Private _CarrTarEquipPUFri As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipPUFri() As Boolean
            Get
                Return _CarrTarEquipPUFri
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipPUFri <> value) Then
                    Me._CarrTarEquipPUFri = value
                    Me.SendPropertyChanged("CarrTarEquipPUFri")
                End If
            End Set
        End Property

        Private _CarrTarEquipPUSat As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipPUSat() As Boolean
            Get
                Return _CarrTarEquipPUSat
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipPUSat <> value) Then
                    Me._CarrTarEquipPUSat = value
                    Me.SendPropertyChanged("CarrTarEquipPUSat")
                End If
            End Set
        End Property

        Private _CarrTarEquipPUSun As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipPUSun() As Boolean
            Get
                Return _CarrTarEquipPUSun
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipPUSun <> value) Then
                    Me._CarrTarEquipPUSun = value
                    Me.SendPropertyChanged("CarrTarEquipPUSun")
                End If
            End Set
        End Property

        Private _CarrTarEquipDLMon As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipDLMon() As Boolean
            Get
                Return _CarrTarEquipDLMon
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipDLMon <> value) Then
                    Me._CarrTarEquipDLMon = value
                    Me.SendPropertyChanged("CarrTarEquipDLMon")
                End If
            End Set
        End Property

        Private _CarrTarEquipDLTue As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipDLTue() As Boolean
            Get
                Return _CarrTarEquipDLTue
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipDLTue <> value) Then
                    Me._CarrTarEquipDLTue = value
                    Me.SendPropertyChanged("CarrTarEquipDLTue")
                End If
            End Set
        End Property

        Private _CarrTarEquipDLWed As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipDLWed() As Boolean
            Get
                Return _CarrTarEquipDLWed
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipDLWed <> value) Then
                    Me._CarrTarEquipDLWed = value
                    Me.SendPropertyChanged("CarrTarEquipDLWed")
                End If
            End Set
        End Property

        Private _CarrTarEquipDLThu As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipDLThu() As Boolean
            Get
                Return _CarrTarEquipDLThu
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipDLThu <> value) Then
                    Me._CarrTarEquipDLThu = value
                    Me.SendPropertyChanged("CarrTarEquipDLThu")
                End If
            End Set
        End Property

        Private _CarrTarEquipDLFri As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipDLFri() As Boolean
            Get
                Return _CarrTarEquipDLFri
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipDLFri <> value) Then
                    Me._CarrTarEquipDLFri = value
                    Me.SendPropertyChanged("CarrTarEquipDLFri")
                End If
            End Set
        End Property

        Private _CarrTarEquipDLSat As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipDLSat() As Boolean
            Get
                Return _CarrTarEquipDLSat
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipDLSat <> value) Then
                    Me._CarrTarEquipDLSat = value
                    Me.SendPropertyChanged("CarrTarEquipDLSat")
                End If
            End Set
        End Property

        Private _CarrTarEquipDLSun As Boolean = True
        <DataMember()> _
        Public Property CarrTarEquipDLSun() As Boolean
            Get
                Return _CarrTarEquipDLSun
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipDLSun <> value) Then
                    Me._CarrTarEquipDLSun = value
                    Me.SendPropertyChanged("CarrTarEquipDLSun")
                End If
            End Set
        End Property

        Private _CarrTarEquipCasesMinimum As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipCasesMinimum() As Double
            Get
                Return Me._CarrTarEquipCasesMinimum
            End Get
            Set(value As Double)
                If (Me._CarrTarEquipCasesMinimum <> value) Then
                    Me._CarrTarEquipCasesMinimum = value
                    Me.SendPropertyChanged("CarrTarEquipCasesMinimum")
                End If
            End Set
        End Property

        Private _CarrTarEquipCasesConsiderFull As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipCasesConsiderFull() As Double
            Get
                Return Me._CarrTarEquipCasesConsiderFull
            End Get
            Set(value As Double)
                If (Me._CarrTarEquipCasesConsiderFull <> value) Then
                    Me._CarrTarEquipCasesConsiderFull = value
                    Me.SendPropertyChanged("CarrTarEquipCasesConsiderFull")
                End If
            End Set
        End Property

        Private _CarrTarEquipCasesMaximum As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipCasesMaximum() As Double
            Get
                Return Me._CarrTarEquipCasesMaximum
            End Get
            Set(value As Double)
                If (Me._CarrTarEquipCasesMaximum <> value) Then
                    Me._CarrTarEquipCasesMaximum = value
                    Me.SendPropertyChanged("CarrTarEquipCasesMaximum")
                End If
            End Set
        End Property

        Private _CarrTarEquipWgtMinimum As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipWgtMinimum() As Double
            Get
                Return Me._CarrTarEquipWgtMinimum
            End Get
            Set(value As Double)
                If (Me._CarrTarEquipWgtMinimum <> value) Then
                    Me._CarrTarEquipWgtMinimum = value
                    Me.SendPropertyChanged("CarrTarEquipWgtMinimum")
                End If
            End Set
        End Property

        Private _CarrTarEquipWgtConsiderFull As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipWgtConsiderFull() As Double
            Get
                Return Me._CarrTarEquipWgtConsiderFull
            End Get
            Set(value As Double)
                If (Me._CarrTarEquipWgtConsiderFull <> value) Then
                    Me._CarrTarEquipWgtConsiderFull = value
                    Me.SendPropertyChanged("CarrTarEquipWgtConsiderFull")
                End If
            End Set
        End Property

        Private _CarrTarEquipWgtMaximum As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipWgtMaximum() As Double
            Get
                Return Me._CarrTarEquipWgtMaximum
            End Get
            Set(value As Double)
                If (Me._CarrTarEquipWgtMaximum <> value) Then
                    Me._CarrTarEquipWgtMaximum = value
                    Me.SendPropertyChanged("CarrTarEquipWgtMaximum")
                End If
            End Set
        End Property

        Private _CarrTarEquipCubesMinimum As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipCubesMinimum() As Double
            Get
                Return Me._CarrTarEquipCubesMinimum
            End Get
            Set(value As Double)
                If (Me._CarrTarEquipCubesMinimum <> value) Then
                    Me._CarrTarEquipCubesMinimum = value
                    Me.SendPropertyChanged("CarrTarEquipCubesMinimum")
                End If
            End Set
        End Property

        Private _CarrTarEquipCubesConsiderFull As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipCubesConsiderFull() As Double
            Get
                Return Me._CarrTarEquipCubesConsiderFull
            End Get
            Set(value As Double)
                If (Me._CarrTarEquipCubesConsiderFull <> value) Then
                    Me._CarrTarEquipCubesConsiderFull = value
                    Me.SendPropertyChanged("CarrTarEquipCubesConsiderFull")
                End If
            End Set
        End Property

        Private _CarrTarEquipCubesMaximum As Double = 0
        <DataMember()> _
        Public Property CarrTarEquipCubesMaximum() As Double
            Get
                Return Me._CarrTarEquipCubesMaximum
            End Get
            Set(value As Double)
                If (Me._CarrTarEquipCubesMaximum <> value) Then
                    Me._CarrTarEquipCubesMaximum = value
                    Me.SendPropertyChanged("CarrTarEquipCubesMaximum")
                End If
            End Set
        End Property

        Private _CarrTarEquipPltsMinimum As Integer
        <DataMember()> _
        Public Property CarrTarEquipPltsMinimum() As Integer
            Get
                Return Me._CarrTarEquipPltsMinimum
            End Get
            Set(value As Integer)
                If (Me._CarrTarEquipPltsMinimum <> value) Then
                    Me._CarrTarEquipPltsMinimum = value
                    Me.SendPropertyChanged("CarrTarEquipPltsMinimum")
                End If
            End Set
        End Property

        Private _CarrTarEquipPltsConsiderFull As Integer
        <DataMember()> _
        Public Property CarrTarEquipPltsConsiderFull() As Integer
            Get
                Return Me._CarrTarEquipPltsConsiderFull
            End Get
            Set(value As Integer)
                If (Me._CarrTarEquipPltsConsiderFull <> value) Then
                    Me._CarrTarEquipPltsConsiderFull = value
                    Me.SendPropertyChanged("CarrTarEquipPltsConsiderFull")
                End If
            End Set
        End Property

        Private _CarrTarEquipPltsMaximum As Integer
        <DataMember()> _
        Public Property CarrTarEquipPltsMaximum() As Integer
            Get
                Return Me._CarrTarEquipPltsMaximum
            End Get
            Set(value As Integer)
                If (Me._CarrTarEquipPltsMaximum <> value) Then
                    Me._CarrTarEquipPltsMaximum = value
                    Me.SendPropertyChanged("CarrTarEquipPltsMaximum")
                End If
            End Set
        End Property

        Private _CarrTarEquipTempType As Integer
        <DataMember()> _
        Public Property CarrTarEquipTempType() As Integer
            Get
                Return Me._CarrTarEquipTempType
            End Get
            Set(value As Integer)
                If (Me._CarrTarEquipTempType <> value) Then
                    Me._CarrTarEquipTempType = value
                    Me.SendPropertyChanged("CarrTarEquipTempType")
                End If
            End Set
        End Property

        Private _CarrTarEquipCarrProName As String
        <DataMember()> _
        Public Property CarrTarEquipCarrProName() As String
            Get
                Return Left(_CarrTarEquipCarrProName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipCarrProName, value) = False) Then
                    Me._CarrTarEquipCarrProName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarEquipCarrProName")
                End If
            End Set
        End Property


        Private _CarrTarEquipModUser As String = ""
        <DataMember()> _
        Public Property CarrTarEquipModUser() As String
            Get
                Return Left(_CarrTarEquipModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarEquipModUser, value) = False) Then
                    Me._CarrTarEquipModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarEquipModUser")
                End If
            End Set
        End Property

        Private _CarrTarEquipModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarEquipModDate() As System.Nullable(Of Date)
            Get
                Return _CarrTarEquipModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrTarEquipModDate.Equals(value) = False) Then
                    Me._CarrTarEquipModDate = value
                    Me.SendPropertyChanged("CarrTarEquipModDate")
                End If
            End Set
        End Property

        Private _CarrTarEquipUpdated As Byte()
        <DataMember()>
        Public Property CarrTarEquipUpdated() As Byte()
            Get
                Return _CarrTarEquipUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarEquipUpdated = value
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
            Dim instance As New CarrTarEquip
            instance = DirectCast(MemberwiseClone(), CarrTarEquip)
            Return instance
        End Function

#End Region

    End Class
End Namespace