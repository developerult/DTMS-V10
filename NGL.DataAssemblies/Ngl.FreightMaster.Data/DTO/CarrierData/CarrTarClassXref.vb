Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarClassXref
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrTarClassXrefControl As Integer
        <DataMember()> _
        Public Property CarrTarClassXrefControl() As Integer
            Get
                Return Me._CarrTarClassXrefControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarClassXrefControl = value) _
                            = False) Then
                    Me._CarrTarClassXrefControl = value
                    Me.SendPropertyChanged("CarrTarClassXrefControl")
                End If
            End Set
        End Property

        Private _CarrTarClassXrefCarrTarControl As Integer
        <DataMember()> _
        Public Property CarrTarClassXrefCarrTarControl() As Integer
            Get
                Return Me._CarrTarClassXrefCarrTarControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarClassXrefCarrTarControl = value) _
                            = False) Then
                    Me._CarrTarClassXrefCarrTarControl = value
                    Me.SendPropertyChanged("CarrTarClassXrefCarrTarControl")
                End If
            End Set
        End Property

        Private _CarrTarClassXrefPointTypeControl As Integer
        <DataMember()> _
        Public Property CarrTarClassXrefPointTypeControl() As Integer
            Get
                Return Me._CarrTarClassXrefPointTypeControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarClassXrefPointTypeControl = value) _
                            = False) Then
                    Me._CarrTarClassXrefPointTypeControl = value
                    Me.SendPropertyChanged("CarrTarClassXrefPointTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarClassXrefClassTypeControl As Integer
        <DataMember()> _
        Public Property CarrTarClassXrefClassTypeControl() As Integer
            Get
                Return Me._CarrTarClassXrefClassTypeControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarClassXrefClassTypeControl = value) _
                            = False) Then
                    Me._CarrTarClassXrefClassTypeControl = value
                    Me.SendPropertyChanged("CarrTarClassXrefClassTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarClassXrefName As String
        <DataMember()> _
        Public Property CarrTarClassXrefName() As String
            Get
                Return Left(Me._CarrTarClassXrefName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarClassXrefName, value) = False) Then
                    Me._CarrTarClassXrefName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarClassXrefName")
                End If
            End Set
        End Property

        Private _CarrTarClassXrefActualFrom As String = ""
        <DataMember()> _
        Public Property CarrTarClassXrefActualFrom() As String
            Get
                Return Left(_CarrTarClassXrefActualFrom, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarClassXrefActualFrom, value) = False) Then
                    Me._CarrTarClassXrefActualFrom = Left(value, 20)
                    Me.SendPropertyChanged("CarrTarClassXrefActualFrom")
                End If
            End Set
        End Property

        Private _CarrTarClassXrefActualTo As String = ""
        <DataMember()> _
        Public Property CarrTarClassXrefActualTo() As String
            Get
                Return Left(_CarrTarClassXrefActualTo, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarClassXrefActualTo, value) = False) Then
                    Me._CarrTarClassXrefActualTo = Left(value, 20)
                    Me.SendPropertyChanged("CarrTarClassXrefActualTo")
                End If
            End Set
        End Property

        Private _CarrTarClassXrefRated As String = ""
        <DataMember()> _
        Public Property CarrTarClassXrefRated() As String
            Get
                Return Left(_CarrTarClassXrefRated, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarClassXrefRated, value) = False) Then
                    Me._CarrTarClassXrefRated = Left(value, 20)
                    Me.SendPropertyChanged("CarrTarClassXrefRated")
                End If
            End Set
        End Property

        Private _CarrTarClassXrefModUser As String = ""
        <DataMember()> _
        Public Property CarrTarClassXrefModUser() As String
            Get
                Return Left(_CarrTarClassXrefModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarClassXrefModUser, value) = False) Then
                    Me._CarrTarClassXrefModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarClassXrefModUser")
                End If
            End Set
        End Property

        Private _CarrTarClassXrefModDate As Date = DateTime.Now
        <DataMember()> _
        Public Property CarrTarClassXrefModDate() As Date
            Get
                Return _CarrTarClassXrefModDate
            End Get
            Set(ByVal value As Date)
                If (Me._CarrTarClassXrefModDate.Equals(value) = False) Then
                    Me._CarrTarClassXrefModDate = value
                    Me.SendPropertyChanged("CarrTarClassXrefModDate")
                End If
            End Set
        End Property

        Private _CarrTarClassXrefUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarClassXrefUpdated() As Byte()
            Get
                Return _CarrTarClassXrefUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarClassXrefUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarClassXref
            instance = DirectCast(MemberwiseClone(), CarrTarClassXref)
            Return instance
        End Function

#End Region

    End Class
End Namespace