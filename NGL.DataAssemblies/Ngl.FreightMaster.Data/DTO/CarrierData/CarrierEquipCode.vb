Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierEquipCode
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrierEquipControl As Integer = 0
        <DataMember()> _
        Public Property CarrierEquipControl() As Integer
            Get
                Return _CarrierEquipControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrierEquipControl <> value) Then
                    Me._CarrierEquipControl = value
                    Me.SendPropertyChanged("CarrierEquipControl")
                End If
            End Set
        End Property

        Private _CarrierEquipCode As String = ""
        <DataMember()> _
        Public Property CarrierEquipCode() As String
            Get
                Return Left(_CarrierEquipCode, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrierEquipCode, value) = False) Then
                    Me._CarrierEquipCode = Left(value, 20)
                    Me.SendPropertyChanged("CarrierEquipCode")
                End If
            End Set
        End Property

        Private _CarrierEquipDescription As String = ""
        <DataMember()> _
        Public Property CarrierEquipDescription() As String
            Get
                Return Left(_CarrierEquipDescription, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrierEquipDescription, value) = False) Then
                    Me._CarrierEquipDescription = Left(value, 50)
                    Me.SendPropertyChanged("CarrierEquipDescription")
                End If
            End Set
        End Property

        Private _CarrierEquipCasesMinimum As Double = 0
        <DataMember()> _
        Public Property CarrierEquipCasesMinimum() As Double
            Get
                Return Me._CarrierEquipCasesMinimum
            End Get
            Set(value As Double)
                If (Me._CarrierEquipCasesMinimum <> value) Then
                    Me._CarrierEquipCasesMinimum = value
                    Me.SendPropertyChanged("CarrierEquipCasesMinimum")
                End If
            End Set
        End Property

        Private _CarrierEquipCasesConsiderFull As Double = 0
        <DataMember()> _
        Public Property CarrierEquipCasesConsiderFull() As Double
            Get
                Return Me._CarrierEquipCasesConsiderFull
            End Get
            Set(value As Double)
                If (Me._CarrierEquipCasesConsiderFull <> value) Then
                    Me._CarrierEquipCasesConsiderFull = value
                    Me.SendPropertyChanged("CarrierEquipCasesConsiderFull")
                End If
            End Set
        End Property

        Private _CarrierEquipCasesMaximum As Double = 0
        <DataMember()> _
        Public Property CarrierEquipCasesMaximum() As Double
            Get
                Return Me._CarrierEquipCasesMaximum
            End Get
            Set(value As Double)
                If (Me._CarrierEquipCasesMaximum <> value) Then
                    Me._CarrierEquipCasesMaximum = value
                    Me.SendPropertyChanged("CarrierEquipCasesMaximum")
                End If
            End Set
        End Property

        Private _CarrierEquipWgtMinimum As Double = 0
        <DataMember()> _
        Public Property CarrierEquipWgtMinimum() As Double
            Get
                Return Me._CarrierEquipWgtMinimum
            End Get
            Set(value As Double)
                If (Me._CarrierEquipWgtMinimum <> value) Then
                    Me._CarrierEquipWgtMinimum = value
                    Me.SendPropertyChanged("CarrierEquipWgtMinimum")
                End If
            End Set
        End Property

        Private _CarrierEquipWgtConsiderFull As Double = 0
        <DataMember()> _
        Public Property CarrierEquipWgtConsiderFull() As Double
            Get
                Return Me._CarrierEquipWgtConsiderFull
            End Get
            Set(value As Double)
                If (Me._CarrierEquipWgtConsiderFull <> value) Then
                    Me._CarrierEquipWgtConsiderFull = value
                    Me.SendPropertyChanged("CarrierEquipWgtConsiderFull")
                End If
            End Set
        End Property

        Private _CarrierEquipWgtMaximum As Double = 0
        <DataMember()> _
        Public Property CarrierEquipWgtMaximum() As Double
            Get
                Return Me._CarrierEquipWgtMaximum
            End Get
            Set(value As Double)
                If (Me._CarrierEquipWgtMaximum <> value) Then
                    Me._CarrierEquipWgtMaximum = value
                    Me.SendPropertyChanged("CarrierEquipWgtMaximum")
                End If
            End Set
        End Property

        Private _CarrierEquipCubesMinimum As Double = 0
        <DataMember()> _
        Public Property CarrierEquipCubesMinimum() As Double
            Get
                Return Me._CarrierEquipCubesMinimum
            End Get
            Set(value As Double)
                If (Me._CarrierEquipCubesMinimum <> value) Then
                    Me._CarrierEquipCubesMinimum = value
                    Me.SendPropertyChanged("CarrierEquipCubesMinimum")
                End If
            End Set
        End Property

        Private _CarrierEquipCubesConsiderFull As Double = 0
        <DataMember()> _
        Public Property CarrierEquipCubesConsiderFull() As Double
            Get
                Return Me._CarrierEquipCubesConsiderFull
            End Get
            Set(value As Double)
                If (Me._CarrierEquipCubesConsiderFull <> value) Then
                    Me._CarrierEquipCubesConsiderFull = value
                    Me.SendPropertyChanged("CarrierEquipCubesConsiderFull")
                End If
            End Set
        End Property

        Private _CarrierEquipCubesMaximum As Double = 0
        <DataMember()> _
        Public Property CarrierEquipCubesMaximum() As Double
            Get
                Return Me._CarrierEquipCubesMaximum
            End Get
            Set(value As Double)
                If (Me._CarrierEquipCubesMaximum <> value) Then
                    Me._CarrierEquipCubesMaximum = value
                    Me.SendPropertyChanged("CarrierEquipCubesMaximum")
                End If
            End Set
        End Property

        Private _CarrierEquipPltsMinimum As Integer
        <DataMember()> _
        Public Property CarrierEquipPltsMinimum() As Integer
            Get
                Return Me._CarrierEquipPltsMinimum
            End Get
            Set(value As Integer)
                If (Me._CarrierEquipPltsMinimum <> value) Then
                    Me._CarrierEquipPltsMinimum = value
                    Me.SendPropertyChanged("CarrierEquipPltsMinimum")
                End If
            End Set
        End Property

        Private _CarrierEquipPltsConsiderFull As Integer
        <DataMember()> _
        Public Property CarrierEquipPltsConsiderFull() As Integer
            Get
                Return Me._CarrierEquipPltsConsiderFull
            End Get
            Set(value As Integer)
                If (Me._CarrierEquipPltsConsiderFull <> value) Then
                    Me._CarrierEquipPltsConsiderFull = value
                    Me.SendPropertyChanged("CarrierEquipPltsConsiderFull")
                End If
            End Set
        End Property

        Private _CarrierEquipPltsMaximum As Integer
        <DataMember()> _
        Public Property CarrierEquipPltsMaximum() As Integer
            Get
                Return Me._CarrierEquipPltsMaximum
            End Get
            Set(value As Integer)
                If (Me._CarrierEquipPltsMaximum <> value) Then
                    Me._CarrierEquipPltsMaximum = value
                    Me.SendPropertyChanged("CarrierEquipPltsMaximum")
                End If
            End Set
        End Property

        Private _CarrierEquipTempType As Integer
        <DataMember()> _
        Public Property CarrierEquipTempType() As Integer
            Get
                Return Me._CarrierEquipTempType
            End Get
            Set(value As Integer)
                If (Me._CarrierEquipTempType <> value) Then
                    Me._CarrierEquipTempType = value
                    Me.SendPropertyChanged("CarrierEquipTempType")
                End If
            End Set
        End Property


        Private _CarrierEquipModUser As String = ""
        <DataMember()> _
        Public Property CarrierEquipModUser() As String
            Get
                Return Left(_CarrierEquipModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierEquipModUser, value) = False) Then
                    Me._CarrierEquipModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrierEquipModUser")
                End If
            End Set
        End Property

        Private _CarrierEquipModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierEquipModDate() As System.Nullable(Of Date)
            Get
                Return _CarrierEquipModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrierEquipModDate.Equals(value) = False) Then
                    Me._CarrierEquipModDate = value
                    Me.SendPropertyChanged("CarrierEquipModDate")
                End If
            End Set
        End Property

        Private _CarrierEquipCodesUpdated As Byte()
        <DataMember()>
        Public Property CarrierEquipCodesUpdated() As Byte()
            Get
                Return _CarrierEquipCodesUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrierEquipCodesUpdated = value
            End Set
        End Property

        '[CarrierEquipMapCode] Nvarchar(4) 
        Private _CarrierEquipMapCode As String = Nothing
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks> 
        ''' New property added by RHR for v- 8.5.3.005 on 08/25/2022 
        ''' all referenes must check for missing data from desktop client.
        ''' </remarks>
        <DataMember()>
        Public Property CarrierEquipMapCode() As String
            Get
                If Not String.IsNullOrWhiteSpace(_CarrierEquipMapCode) Then
                    Return Left(_CarrierEquipMapCode, 4)
                Else
                    Return Nothing
                End If

            End Get
            Set(value As String)
                If Not String.IsNullOrWhiteSpace(value) Then
                    If (String.Equals(Me._CarrierEquipMapCode, value) = False) Then
                        Me._CarrierEquipMapCode = Left(value, 4)
                        Me.SendPropertyChanged("CarrierEquipMapCode")
                    End If
                Else
                    'value is nothing only update if _CarrierEquipMapCode is not nothing
                    If Not String.IsNullOrWhiteSpace(_CarrierEquipMapCode) Then
                        Me._CarrierEquipMapCode = Nothing
                        Me.SendPropertyChanged("CarrierEquipMapCode")

                    End If
                End If

            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierEquipCode
            instance = DirectCast(MemberwiseClone(), CarrierEquipCode)
            Return instance
        End Function

#End Region

    End Class
End Namespace
