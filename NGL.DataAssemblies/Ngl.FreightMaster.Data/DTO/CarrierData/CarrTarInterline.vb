Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarInterline
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrTarInterlineControl As Integer
        <DataMember()> _
        Public Property CarrTarInterlineControl() As Integer
            Get
                Return Me._CarrTarInterlineControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarInterlineControl = value) _
                            = False) Then
                    Me._CarrTarInterlineControl = value
                    Me.SendPropertyChanged("CarrTarInterlineControl")
                End If
            End Set
        End Property

        Private _CarrTarInterlineCarrTarControl As Integer
        <DataMember()> _
        Public Property CarrTarInterlineCarrTarControl() As Integer
            Get
                Return Me._CarrTarInterlineCarrTarControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarInterlineCarrTarControl = value) _
                            = False) Then
                    Me._CarrTarInterlineCarrTarControl = value
                    Me.SendPropertyChanged("CarrTarInterlineCarrTarControl")
                End If
            End Set
        End Property

        Private _CarrTarInterlineName As String
        <DataMember()> _
        Public Property CarrTarInterlineName() As String
            Get
                Return Left(Me._CarrTarInterlineName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarInterlineName, value) = False) Then
                    Me._CarrTarInterlineName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarInterlineName")
                End If
            End Set
        End Property

        Private _CarrTarInterlineEffDateFrom As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarInterlineEffDateFrom() As System.Nullable(Of Date)
            Get
                Return Me._CarrTarInterlineEffDateFrom
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrTarInterlineEffDateFrom.Equals(value) = False) Then
                    Me._CarrTarInterlineEffDateFrom = value
                    Me.SendPropertyChanged("CarrTarInterlineEffDateFrom")

                End If
            End Set
        End Property

        Private _CarrTarInterlineEffDateTo As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarInterlineEffDateTo() As System.Nullable(Of Date)
            Get
                Return Me._CarrTarInterlineEffDateTo
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrTarInterlineEffDateTo.Equals(value) = False) Then
                    Me._CarrTarInterlineEffDateTo = value
                    Me.SendPropertyChanged("CarrTarInterlineEffDateTo")
                End If
            End Set
        End Property

        Private _CarrTarInterlineCountry As String
        <DataMember()> _
        Public Property CarrTarInterlineCountry() As String
            Get
                Return Left(Me._CarrTarInterlineCountry, 30)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarInterlineCountry, value) = False) Then
                    Me._CarrTarInterlineCountry = Left(value, 30)
                    Me.SendPropertyChanged("CarrTarInterlineCountry")
                End If
            End Set
        End Property

        Private _CarrTarInterlineState As String
        <DataMember()> _
        Public Property CarrTarInterlineState() As String
            Get
                Return Left(Me._CarrTarInterlineState, 2)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarInterlineState, value) = False) Then
                    Me._CarrTarInterlineState = Left(value, 2)
                    Me.SendPropertyChanged("CarrTarInterlineState")
                End If
            End Set
        End Property

        Private _CarrTarInterlineCity As String
        <DataMember()> _
        Public Property CarrTarInterlineCity() As String
            Get
                Return Left(Me._CarrTarInterlineCity, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarInterlineCity, value) = False) Then
                    Me._CarrTarInterlineCity = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarInterlineCity")
                End If
            End Set
        End Property

        Private _CarrTarInterlineZip As String
        <DataMember()> _
        Public Property CarrTarInterlineZip() As String
            Get
                Return Left(Me._CarrTarInterlineZip, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarInterlineZip, value) = False) Then
                    Me._CarrTarInterlineZip = Left(value, 10)
                    Me.SendPropertyChanged("CarrTarInterlineZip")
                End If
            End Set
        End Property

        Private _CarrTarInterlineModUser As String = ""
        <DataMember()> _
        Public Property CarrTarInterlineModUser() As String
            Get
                Return Left(_CarrTarInterlineModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarInterlineModUser, value) = False) Then
                    Me._CarrTarInterlineModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarInterlineModUser")
                End If
            End Set
        End Property

        Private _CarrTarInterlineModDate As Date = DateTime.Now
        <DataMember()> _
        Public Property CarrTarInterlineModDate() As Date
            Get
                Return _CarrTarInterlineModDate
            End Get
            Set(ByVal value As Date)
                If (Me._CarrTarInterlineModDate.Equals(value) = False) Then
                    Me._CarrTarInterlineModDate = value
                    Me.SendPropertyChanged("CarrTarInterlineModDate")
                End If
            End Set
        End Property

        Private _CarrTarInterlineUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarInterlineUpdated() As Byte()
            Get
                Return _CarrTarInterlineUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarInterlineUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarInterline
            instance = DirectCast(MemberwiseClone(), CarrTarInterline)
            Return instance
        End Function

#End Region

    End Class
End Namespace
