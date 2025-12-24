
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarMatBP
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrTarMatBPControl As Integer = 0
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

        Private _CarrTarMatBPCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatBPCarrTarControl() As Integer
            Get
                Return _CarrTarMatBPCarrTarControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPCarrTarControl <> value) Then
                    Me._CarrTarMatBPCarrTarControl = value
                    Me.SendPropertyChanged("CarrTarMatBPCarrTarControl")
                End If
            End Set
        End Property

        Private _CarrTarMatBPName As String = ""
        <DataMember()> _
        Public Property CarrTarMatBPName() As String
            Get
                Return Left(_CarrTarMatBPName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMatBPName, value) = False) Then
                    Me._CarrTarMatBPName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarMatBPName")
                End If
            End Set
        End Property

        Private _CarrTarMatBPDesc As String = ""
        <DataMember()> _
        Public Property CarrTarMatBPDesc() As String
            Get
                Return Left(_CarrTarMatBPDesc, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMatBPDesc, value) = False) Then
                    Me._CarrTarMatBPDesc = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarMatBPDesc")
                End If
            End Set
        End Property

        Private _CarrTarMatBPClassTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatBPClassTypeControl() As Integer
            Get
                Return _CarrTarMatBPClassTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPClassTypeControl <> value) Then
                    Me._CarrTarMatBPClassTypeControl = value
                    Me.SendPropertyChanged("CarrTarMatBPClassTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarMatBPTarBracketTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatBPTarBracketTypeControl() As Integer
            Get
                Return _CarrTarMatBPTarBracketTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPTarBracketTypeControl <> value) Then
                    Me._CarrTarMatBPTarBracketTypeControl = value
                    Me.SendPropertyChanged("CarrTarMatBPTarBracketTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarMatBPTarRateTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatBPTarRateTypeControl() As Integer
            Get
                Return _CarrTarMatBPTarRateTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPTarRateTypeControl <> value) Then
                    Me._CarrTarMatBPTarRateTypeControl = value
                    Me.SendPropertyChanged("CarrTarMatBPTarRateTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarMatBPModUser As String = ""
        <DataMember()> _
        Public Property CarrTarMatBPModUser() As String
            Get
                Return Left(_CarrTarMatBPModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMatBPModUser, value) = False) Then
                    Me._CarrTarMatBPModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarMatBPModUser")
                End If
            End Set
        End Property

        Private _CarrTarMatBPModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarMatBPModDate() As System.Nullable(Of Date)
            Get
                Return _CarrTarMatBPModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CarrTarMatBPModDate.Equals(value) = False) Then
                    Me._CarrTarMatBPModDate = value
                    Me.SendPropertyChanged("CarrTarMatBPModDate")
                End If
            End Set
        End Property

        Private _CarrTarMatBPUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarMatBPUpdated() As Byte()
            Get
                Return _CarrTarMatBPUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarMatBPUpdated = value
            End Set
        End Property



        Private _CarrierTariffMatrixBPDetails As List(Of CarrTarMatBPDet)
        Friend Property CarrierTariffMatrixBPDetails() As List(Of CarrTarMatBPDet)
            Get
                Return _CarrierTariffMatrixBPDetails
            End Get
            Set(ByVal value As List(Of CarrTarMatBPDet))
                _CarrierTariffMatrixBPDetails = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarMatBP
            instance = DirectCast(MemberwiseClone(), CarrTarMatBP)
            instance.CarrierTariffMatrixBPDetails = Nothing
            For Each item In CarrierTariffMatrixBPDetails
                instance.CarrierTariffMatrixBPDetails.Add(DirectCast(item.Clone, CarrTarMatBPDet))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace