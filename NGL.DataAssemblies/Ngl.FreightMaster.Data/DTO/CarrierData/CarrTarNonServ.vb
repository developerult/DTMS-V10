Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarNonServ
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrTarNonServControl As Integer
        <DataMember()> _
        Public Property CarrTarNonServControl() As Integer
            Get
                Return Me._CarrTarNonServControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarNonServControl = value) _
                            = False) Then
                    Me._CarrTarNonServControl = value
                    Me.SendPropertyChanged("CarrTarNonServControl")
                End If
            End Set
        End Property

        Private _CarrTarNonServCarrTarControl As Integer
        <DataMember()> _
        Public Property CarrTarNonServCarrTarControl() As Integer
            Get
                Return Me._CarrTarNonServCarrTarControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarNonServCarrTarControl = value) _
                            = False) Then
                    Me._CarrTarNonServCarrTarControl = value
                    Me.SendPropertyChanged("CarrTarNonServCarrTarControl")
                End If
            End Set
        End Property

        Private _CarrTarNonServName As String
        <DataMember()> _
        Public Property CarrTarNonServName() As String
            Get
                Return Left(Me._CarrTarNonServName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarNonServName, value) = False) Then
                    Me._CarrTarNonServName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarNonServName")
                End If
            End Set
        End Property

        Private _CarrTarNonServEffDateFrom As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarNonServEffDateFrom() As System.Nullable(Of Date)
            Get
                Return Me._CarrTarNonServEffDateFrom
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrTarNonServEffDateFrom.Equals(value) = False) Then
                    Me._CarrTarNonServEffDateFrom = value
                    Me.SendPropertyChanged("CarrTarNonServEffDateFrom")

                End If
            End Set
        End Property

        Private _CarrTarNonServEffDateTo As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarNonServEffDateTo() As System.Nullable(Of Date)
            Get
                Return Me._CarrTarNonServEffDateTo
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrTarNonServEffDateTo.Equals(value) = False) Then
                    Me._CarrTarNonServEffDateTo = value
                    Me.SendPropertyChanged("CarrTarNonServEffDateTo")
                End If
            End Set
        End Property

        Private _CarrTarNonServCountry As String
        <DataMember()> _
        Public Property CarrTarNonServCountry() As String
            Get
                Return Left(Me._CarrTarNonServCountry, 30)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarNonServCountry, value) = False) Then
                    Me._CarrTarNonServCountry = Left(value, 30)
                    Me.SendPropertyChanged("CarrTarNonServCountry")
                End If
            End Set
        End Property

        Private _CarrTarNonServState As String
        <DataMember()> _
        Public Property CarrTarNonServState() As String
            Get
                Return Left(Me._CarrTarNonServState, 2)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarNonServState, value) = False) Then
                    Me._CarrTarNonServState = Left(value, 2)
                    Me.SendPropertyChanged("CarrTarNonServState")
                End If
            End Set
        End Property

        Private _CarrTarNonServCity As String
        <DataMember()> _
        Public Property CarrTarNonServCity() As String
            Get
                Return Left(Me._CarrTarNonServCity, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarNonServCity, value) = False) Then
                    Me._CarrTarNonServCity = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarNonServCity")
                End If
            End Set
        End Property

        Private _CarrTarNonServZip As String
        <DataMember()> _
        Public Property CarrTarNonServZip() As String
            Get
                Return Left(Me._CarrTarNonServZip, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarNonServZip, value) = False) Then
                    Me._CarrTarNonServZip = Left(value, 10)
                    Me.SendPropertyChanged("CarrTarNonServZip")
                End If
            End Set
        End Property

        Private _CarrTarNonServLaneControl As Integer
        <DataMember()> _
        Public Property CarrTarNonServLaneControl() As Integer
            Get
                Return Me._CarrTarNonServLaneControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarNonServLaneControl = value) _
                            = False) Then
                    Me._CarrTarNonServLaneControl = value
                    Me.SendPropertyChanged("CarrTarNonServLaneControl")
                End If
            End Set
        End Property

        Private _CarrTarNonServModUser As String = ""
        <DataMember()> _
        Public Property CarrTarNonServModUser() As String
            Get
                Return Left(_CarrTarNonServModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarNonServModUser, value) = False) Then
                    Me._CarrTarNonServModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarNonServModUser")
                End If
            End Set
        End Property

        Private _CarrTarNonServModDate As Date = DateTime.Now
        <DataMember()> _
        Public Property CarrTarNonServModDate() As Date
            Get
                Return _CarrTarNonServModDate
            End Get
            Set(ByVal value As Date)
                If (Me._CarrTarNonServModDate.Equals(value) = False) Then
                    Me._CarrTarNonServModDate = value
                    Me.SendPropertyChanged("CarrTarNonServModDate")
                End If
            End Set
        End Property

        Private _CarrTarNonServUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarNonServUpdated() As Byte()
            Get
                Return _CarrTarNonServUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarNonServUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarNonServ
            instance = DirectCast(MemberwiseClone(), CarrTarNonServ)
            Return instance
        End Function

#End Region

    End Class
End Namespace
