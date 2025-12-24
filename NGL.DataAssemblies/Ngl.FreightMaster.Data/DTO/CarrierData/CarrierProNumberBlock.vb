Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects

    <DataContract()> _
    Public Class CarrierProNumberBlock
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrProNbrBlockControl As Integer
        <DataMember()> _
        Public Property CarrProNbrBlockControl() As Integer
            Get
                Return Me._CarrProNbrBlockControl
            End Get
            Set(value As Integer)
                If ((Me._CarrProNbrBlockControl = value) = False) Then
                    Me._CarrProNbrBlockControl = value
                    Me.SendPropertyChanged("CarrProNbrBlockControl")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockCarrProControl As Integer
        <DataMember()> _
        Public Property CarrProNbrBlockCarrProControl() As Integer
            Get
                Return Me._CarrProNbrBlockCarrProControl
            End Get
            Set(value As Integer)
                If ((Me._CarrProNbrBlockCarrProControl = value) = False) Then
                    Me._CarrProNbrBlockCarrProControl = value
                    Me.SendPropertyChanged("CarrProNbrBlockCarrProControl")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockCarrierControl As Integer
        <DataMember()> _
        Public Property CarrProNbrBlockCarrierControl() As Integer
            Get
                Return Me._CarrProNbrBlockCarrierControl
            End Get
            Set(value As Integer)
                If ((Me._CarrProNbrBlockCarrierControl = value) _
                            = False) Then
                    Me._CarrProNbrBlockCarrierControl = value
                    Me.SendPropertyChanged("CarrProNbrBlockCarrierControl")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockSortOrder As Integer
        <DataMember()> _
        Public Property CarrProNbrBlockSortOrder() As Integer
            Get
                Return Me._CarrProNbrBlockSortOrder
            End Get
            Set(value As Integer)
                If ((Me._CarrProNbrBlockSortOrder = value) _
                            = False) Then
                    Me._CarrProNbrBlockSortOrder = value
                    Me.SendPropertyChanged("CarrProNbrBlockSortOrder")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockNumber As String
        <DataMember()> _
        Public Property CarrProNbrBlockNumber() As String
            Get
                Return Left(Me._CarrProNbrBlockNumber, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProNbrBlockNumber, value) = False) Then
                    Me._CarrProNbrBlockNumber = Left(value, 20)
                    Me.SendPropertyChanged("CarrProNbrBlockNumber")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockAddedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrProNbrBlockAddedDate() As System.Nullable(Of Date)
            Get
                Return Me._CarrProNbrBlockAddedDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrProNbrBlockAddedDate.Equals(value) = False) Then
                    Me._CarrProNbrBlockAddedDate = value
                    Me.SendPropertyChanged("CarrProNbrBlockAddedDate")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockUsedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrProNbrBlockUsedDate() As System.Nullable(Of Date)
            Get
                Return Me._CarrProNbrBlockUsedDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrProNbrBlockUsedDate.Equals(value) = False) Then
                    Me._CarrProNbrBlockUsedDate = value
                    Me.SendPropertyChanged("CarrProNbrBlockUsedDate")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockUsedReference As String
        <DataMember()> _
        Public Property CarrProNbrBlockUsedReference() As String
            Get
                Return Left(Me._CarrProNbrBlockUsedReference, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProNbrBlockUsedReference, value) = False) Then
                    Me._CarrProNbrBlockUsedReference = Left(value, 50)
                    Me.SendPropertyChanged("CarrProNbrBlockUsedReference")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockAvailable As Boolean
        <DataMember()> _
        Public Property CarrProNbrBlockAvailable() As Boolean
            Get
                Return Me._CarrProNbrBlockAvailable
            End Get
            Set(value As Boolean)
                If ((Me._CarrProNbrBlockAvailable = value) _
                            = False) Then
                    Me._CarrProNbrBlockAvailable = value
                    Me.SendPropertyChanged("CarrProNbrBlockAvailable")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockRaiseRunningLowAlert As Boolean
        <DataMember()> _
        Public Property CarrProNbrBlockRaiseRunningLowAlert() As Boolean
            Get
                Return Me._CarrProNbrBlockRaiseRunningLowAlert
            End Get
            Set(value As Boolean)
                If ((Me._CarrProNbrBlockRaiseRunningLowAlert = value) _
                            = False) Then
                    Me._CarrProNbrBlockRaiseRunningLowAlert = value
                    Me.SendPropertyChanged("CarrProNbrBlockRaiseRunningLowAlert")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockDesc As String
        <DataMember()> _
        Public Property CarrProNbrBlockDesc() As String
            Get
                Return Left(Me._CarrProNbrBlockDesc, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProNbrBlockDesc, value) = False) Then
                    Me._CarrProNbrBlockDesc = Left(value, 100)
                    Me.SendPropertyChanged("CarrProNbrBlockDesc")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrProNbrBlockModDate() As System.Nullable(Of Date)
            Get
                Return Me._CarrProNbrBlockModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrProNbrBlockModDate.Equals(value) = False) Then
                    Me._CarrProNbrBlockModDate = value
                    Me.SendPropertyChanged("CarrProNbrBlockModDate")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockModUser As String
        <DataMember()> _
        Public Property CarrProNbrBlockModUser() As String
            Get
                Return Left(Me._CarrProNbrBlockModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProNbrBlockModUser, value) = False) Then
                    Me._CarrProNbrBlockModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrProNbrBlockModUser")
                End If
            End Set
        End Property

        Private _CarrProNbrBlockUpdated As Byte()
        <DataMember()> _
        Public Property CarrProNbrBlockUpdated() As Byte()
            Get
                Return Me._CarrProNbrBlockUpdated
            End Get
            Set(value As Byte())
                Me._CarrProNbrBlockUpdated = value
                Me.SendPropertyChanged("CarrProNbrBlockUpdated")
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierProNumberBlock
            instance = DirectCast(MemberwiseClone(), CarrierProNumberBlock)
            Return instance
        End Function

#End Region

    End Class
End Namespace
