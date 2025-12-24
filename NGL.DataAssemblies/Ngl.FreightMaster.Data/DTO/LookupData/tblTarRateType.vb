Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblTarRateType
        Inherits DTOBaseClass


#Region " Data Members"

        Private _TarRateTypeControl As Integer = 0
        <DataMember()> _
        Public Property TarRateTypeControl() As Integer
            Get
                Return Me._TarRateTypeControl
            End Get
            Set(value As Integer)
                If ((Me._TarRateTypeControl = value) _
                   = False) Then
                    Me._TarRateTypeControl = value
                    Me.SendPropertyChanged("TarRateTypeControl")
                End If
            End Set
        End Property



        Private _TarRateTypeName As String
        <DataMember()> _
        Public Property TarRateTypeName() As String
            Get
                Return Left(Me._TarRateTypeName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._TarRateTypeName, value) = False) Then
                    Me._TarRateTypeName = Left(value, 50)
                    Me.SendPropertyChanged("TarRateTypeName")
                End If
            End Set
        End Property

        Private _TarRateTypeDesc As String
        <DataMember()> _
        Public Property TarRateTypeDesc() As String
            Get
                Return Left(Me._TarRateTypeDesc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._TarRateTypeDesc, value) = False) Then
                    Me._TarRateTypeDesc = Left(value, 255)
                    Me.SendPropertyChanged("TarRateTypeDesc")
                End If
            End Set
        End Property

        Private _TarRateTypeModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property TarRateTypeModDate() As System.Nullable(Of Date)
            Get
                Return Me._TarRateTypeModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._TarRateTypeModDate.Equals(value) = False) Then
                    Me._TarRateTypeModDate = value
                    Me.SendPropertyChanged("TarRateTypeModDate")
                End If
            End Set
        End Property

        Private _TarRateTypeModUser As String
        <DataMember()> _
        Public Property TarRateTypeModUser() As String
            Get
                Return Left(Me._TarRateTypeModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._TarRateTypeModUser, value) = False) Then
                    Me._TarRateTypeModUser = Left(value, 100)
                    Me.SendPropertyChanged("TarRateTypeModUser")
                End If
            End Set
        End Property


        Private _TarRateTypeUpdated As Byte()
        <DataMember()> _
        Public Property TarRateTypeUpdated() As Byte()
            Get
                Return Me._TarRateTypeUpdated
            End Get
            Set(value As Byte())
                Me._TarRateTypeUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblTarRateType
            instance = DirectCast(MemberwiseClone(), tblTarRateType)
            Return instance
        End Function

#End Region

    End Class
End Namespace
