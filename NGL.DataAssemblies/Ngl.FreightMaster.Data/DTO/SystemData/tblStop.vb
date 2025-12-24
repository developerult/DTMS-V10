Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblStop
        Inherits DTOBaseClass


#Region " Data Members"

        Private _StopControl As Integer = 0
        <DataMember()> _
        Public Property StopControl() As Integer
            Get
                Return Me._StopControl
            End Get
            Set(value As Integer)
                If ((Me._StopControl = value) _
                   = False) Then
                    Me._StopControl = value
                    Me.SendPropertyChanged("StopControl")
                End If
            End Set
        End Property

        Private _StopAddress1 As String = ""
        <DataMember()> _
        Public Property StopAddress1() As String
            Get
                Return Left(Me._StopAddress1, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._StopAddress1, value) = False) Then
                    Me._StopAddress1 = Left(value, 40)
                    Me.SendPropertyChanged("StopAddress1")
                End If
            End Set
        End Property

        Private _StopCity As String = ""
        <DataMember()> _
        Public Property StopCity() As String
            Get
                Return Left(Me._StopCity, 25)
            End Get
            Set(value As String)
                If (String.Equals(Me._StopCity, value) = False) Then
                    Me._StopCity = Left(value, 25)
                    Me.SendPropertyChanged("StopCity")
                End If
            End Set
        End Property

        Private _StopState As String = ""
        <DataMember()> _
        Public Property StopState() As String
            Get
                Return Left(Me._StopState, 2)
            End Get
            Set(value As String)
                If (String.Equals(Me._StopState, value) = False) Then
                    Me._StopState = Left(value, 2)
                    Me.SendPropertyChanged("StopState")
                End If
            End Set
        End Property

        Private _StopCountry As String = ""
        <DataMember()> _
        Public Property StopCountry() As String
            Get
                Return Left(Me._StopCountry, 30)
            End Get
            Set(value As String)
                If (String.Equals(Me._StopCountry, value) = False) Then
                    Me._StopCountry = Left(value, 30)
                    Me.SendPropertyChanged("StopCountry")
                End If
            End Set
        End Property

        Private _StopZip As String = ""
        <DataMember()> _
        Public Property StopZip() As String
            Get
                Return Left(Me._StopZip, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._StopZip, value) = False) Then
                    Me._StopZip = Left(value, 10)
                    Me.SendPropertyChanged("StopZip")
                End If
            End Set
        End Property

        Private _StopLatitude As Double = 0
        <DataMember()> _
        Public Property StopLatitude() As Double
            Get
                Return Me._StopLatitude
            End Get
            Set(value As Double)
                If ((Me._StopLatitude = value) _
                   = False) Then
                    Me._StopLatitude = value
                    Me.SendPropertyChanged("StopLatitude")
                End If
            End Set
        End Property

        Private _StopLongitude As Double = 0
        <DataMember()> _
        Public Property StopLongitude() As Double
            Get
                Return Me._StopLongitude
            End Get
            Set(value As Double)
                If ((Me._StopLongitude = value) _
                   = False) Then
                    Me._StopLongitude = value
                    Me.SendPropertyChanged("StopLongitude")
                End If
            End Set
        End Property

        Private _StopModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property StopModDate() As System.Nullable(Of Date)
            Get
                Return Me._StopModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._StopModDate.Equals(value) = False) Then
                    Me._StopModDate = value
                    Me.SendPropertyChanged("StopModDate")
                End If
            End Set
        End Property

        Private _StopModUser As String = ""
        <DataMember()> _
        Public Property StopModUser() As String
            Get
                Return Left(Me._StopModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._StopModUser, value) = False) Then
                    Me._StopModUser = Left(value, 100)
                    Me.SendPropertyChanged("StopModUser")
                End If
            End Set
        End Property


        Private _StopUpdated As Byte()

        <DataMember()> _
        Public Property StopUpdated() As Byte()
            Get
                Return Me._StopUpdated
            End Get
            Set(value As Byte())
                Me._StopUpdated = value
            End Set
        End Property

        'Added By LVV on 9/19/19 for Bing Maps
        Private _StopName As String = ""
        <DataMember()>
        Public Property StopName() As String
            Get
                Return Left(Me._StopName, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._StopName, value) = False) Then
                    Me._StopName = Left(value, 100)
                    Me.SendPropertyChanged("StopName")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblStop
            instance = DirectCast(MemberwiseClone(), tblStop)
            Return instance
        End Function

        ''' <summary>
        ''' Caller must first check that the object is not null to avoid null reference exception.
        ''' Quick way to make sure the object contains at least one populated address property
        ''' </summary>
        ''' <returns></returns>
        Public Function HasData() As Boolean
            If _StopControl <> 0 Then Return True
            If Not String.IsNullOrWhiteSpace(_StopAddress1) Then Return True
            If Not String.IsNullOrWhiteSpace(_StopCity) Then Return True
            If Not String.IsNullOrWhiteSpace(_StopState) Then Return True
            If Not String.IsNullOrWhiteSpace(_StopZip) Then Return True
            If Not String.IsNullOrWhiteSpace(_StopCountry) Then Return True
            If Not String.IsNullOrWhiteSpace(_StopName) Then Return True
            If _StopLatitude <> 0 Then Return True
            If _StopLongitude <> 0 Then Return True
            Return False
        End Function

#End Region

    End Class
End Namespace