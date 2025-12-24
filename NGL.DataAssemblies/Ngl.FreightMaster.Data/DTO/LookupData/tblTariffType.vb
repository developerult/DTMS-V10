Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblTariffType
        Inherits DTOBaseClass


#Region " Data Members"

        Private _TariffTypeControl As Integer = 0
        <DataMember()> _
        Public Property TariffTypeControl() As Integer
            Get
                Return Me._TariffTypeControl
            End Get
            Set(value As Integer)
                If ((Me._TariffTypeControl = value) _
                   = False) Then
                    Me._TariffTypeControl = value
                    Me.SendPropertyChanged("TariffTypeControl")
                End If
            End Set
        End Property



        Private _TariffTypeName As String
        <DataMember()> _
        Public Property TariffTypeName() As String
            Get
                Return Left(Me._TariffTypeName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._TariffTypeName, value) = False) Then
                    Me._TariffTypeName = Left(value, 50)
                    Me.SendPropertyChanged("TariffTypeName")
                End If
            End Set
        End Property

        Private _TariffTypeDesc As String
        <DataMember()> _
        Public Property TariffTypeDesc() As String
            Get
                Return Left(Me._TariffTypeDesc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._TariffTypeDesc, value) = False) Then
                    Me._TariffTypeDesc = Left(value, 255)
                    Me.SendPropertyChanged("TariffTypeDesc")
                End If
            End Set
        End Property

        Private _TariffTypeModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property TariffTypeModDate() As System.Nullable(Of Date)
            Get
                Return Me._TariffTypeModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._TariffTypeModDate.Equals(value) = False) Then
                    Me._TariffTypeModDate = value
                    Me.SendPropertyChanged("TariffTypeModDate")
                End If
            End Set
        End Property

        Private _TariffTypeModUser As String
        <DataMember()> _
        Public Property TariffTypeModUser() As String
            Get
                Return Left(Me._TariffTypeModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._TariffTypeModUser, value) = False) Then
                    Me._TariffTypeModUser = Left(value, 100)
                    Me.SendPropertyChanged("TariffTypeModUser")
                End If
            End Set
        End Property


        Private _TariffTypeUpdated As Byte()
        <DataMember()> _
        Public Property TariffTypeUpdated() As Byte()
            Get
                Return Me._TariffTypeUpdated
            End Get
            Set(value As Byte())
                Me._TariffTypeUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblTariffType
            instance = DirectCast(MemberwiseClone(), tblTariffType)
            Return instance
        End Function

#End Region

    End Class
End Namespace
