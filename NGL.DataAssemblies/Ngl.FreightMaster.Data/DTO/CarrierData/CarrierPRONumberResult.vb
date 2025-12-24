Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierPRONumberResult
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrProControl As Integer = 0
        <DataMember()> _
        Public Property CarrProControl() As Integer
            Get
                Return Me._CarrProControl
            End Get
            Set(value As Integer)
                If ((Me._CarrProControl = value) _
                   = False) Then
                    Me._CarrProControl = value
                    Me.SendPropertyChanged("CarrProControl")
                End If
            End Set
        End Property

        Private _CarrierProNumberRaw As String
        <DataMember()> _
        Public Property CarrierProNumberRaw() As String
            Get
                Return Left(_CarrierProNumberRaw, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrierProNumberRaw, value) = False) Then
                    Me._CarrierProNumberRaw = Left(value, 20)
                    Me.SendPropertyChanged("CarrierProNumberRaw")
                End If
            End Set
        End Property

        Private _CarrierProNumber As String
        <DataMember()> _
        Public Property CarrierProNumber() As String
            Get
                Return Left(_CarrierProNumber, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrierProNumber, value) = False) Then
                    Me._CarrierProNumber = Left(value, 20)
                    Me.SendPropertyChanged("CarrierProNumber")
                End If
            End Set
        End Property

        Private _ErrorNumber As Integer
        <DataMember()> _
        Public Property ErrorNumber() As Integer
            Get
                Return _ErrorNumber
            End Get
            Set(ByVal value As Integer)
                _ErrorNumber = value
            End Set
        End Property

        Private _RetMsg As String
        <DataMember()> _
        Public Property RetMsg() As String
            Get
                Return _RetMsg
            End Get
            Set(ByVal value As String)
                _RetMsg = value
            End Set
        End Property






#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierPRONumberResult
            instance = DirectCast(MemberwiseClone(), CarrierPRONumberResult)
            Return instance
        End Function

#End Region

    End Class
End Namespace
