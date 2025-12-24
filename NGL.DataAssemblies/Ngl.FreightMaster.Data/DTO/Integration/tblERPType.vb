Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblERPType
        Inherits DTOBaseClass


#Region " Data Members"
        Private _ERPTypeControl As Integer
        Public Property ERPTypeControl() As Integer
            Get
                Return Me._ERPTypeControl
            End Get
            Set(value As Integer)
                If ((Me._ERPTypeControl = value) _
                            = False) Then
                    Me._ERPTypeControl = value
                    Me.SendPropertyChanged("ERPTypeControl")
                End If
            End Set
        End Property

        Private _Name As String
        Public Property Name() As String
            Get
                Return Left(_Name, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._Name, value) = False) Then
                    Me._Name = Left(value, 50)
                    Me.SendPropertyChanged("Name")
                End If
            End Set
        End Property

        Private _StartVersion As String
        Public Property StartVersion() As String
            Get
                Return Left(_StartVersion, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._StartVersion, value) = False) Then
                    Me._StartVersion = Left(value, 10)
                    Me.SendPropertyChanged("StartVersion")
                End If
            End Set
        End Property

        Private _EndVersion As String
        Public Property EndVersion() As String
            Get
                Return Left(_EndVersion, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._EndVersion, value) = False) Then
                    Me._EndVersion = Left(value, 10)
                    Me.SendPropertyChanged("EndVersion")
                End If
            End Set
        End Property

        Private _Notes As String
        Public Property Notes() As String
            Get
                Return Left(_Notes, 250)
            End Get
            Set(value As String)
                If (String.Equals(Me._Notes, value) = False) Then
                    Me._Notes = Left(value, 250)
                    Me.SendPropertyChanged("Notes")
                End If
            End Set
        End Property

        Private _ERPTypeModDate As System.Nullable(Of Date)
        Public Property ERPTypeModDate() As System.Nullable(Of Date)
            Get
                Return Me._ERPTypeModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._ERPTypeModDate.Equals(value) = False) Then
                    Me._ERPTypeModDate = value
                    Me.SendPropertyChanged("ERPTypeModDate")
                End If
            End Set
        End Property

        Private _ERPTypeModUser As String
        Public Property ERPTypeModUser() As String
            Get
                Return Left(_ERPTypeModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ERPTypeModUser, value) = False) Then
                    Me._ERPTypeModUser = Left(value, 100)
                    Me.SendPropertyChanged("ERPTypeModUser")
                End If
            End Set
        End Property

        Private _ERPTypeUpdated As Byte()
        Public Property ERPTypeUpdated() As Byte()
            Get
                Return Me._ERPTypeUpdated
            End Get
            Set(value As Byte())
                If (Object.Equals(Me._ERPTypeUpdated, value) = False) Then
                    Me._ERPTypeUpdated = value
                    Me.SendPropertyChanged("ERPTypeUpdated")
                End If
            End Set
        End Property

        Private _Description As String
        Public Property Description() As String
            Get
                Return Left(_Description, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._Description, value) = False) Then
                    Me._Description = Left(value, 100)
                    Me.SendPropertyChanged("Description")
                End If
            End Set
        End Property
 

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblERPType
            instance = DirectCast(MemberwiseClone(), tblERPType)
            Return instance
        End Function

#End Region

    End Class
End Namespace

















