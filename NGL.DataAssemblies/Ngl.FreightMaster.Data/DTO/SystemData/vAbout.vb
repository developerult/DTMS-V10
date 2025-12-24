Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class vAbout


#Region " Data Members"
        Private _AuthKey As String

        Private _AuthNumber As String

        Private _AuthName As String

        Private _AuthAddress As String

        Private _AuthCity As String

        Private _AuthState As String

        Private _AuthZip As String

        Private _version As String

        Private _xactversion As String

        Private _masversion As String

        Private _claimversion As String

        Private _ServerLastMod As System.Nullable(Of Date)

        <DataMember()> _
        Public Property AuthKey() As String
            Get
                Return Me._AuthKey
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AuthKey, value) = False) Then
                    Me._AuthKey = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property AuthNumber() As String
            Get
                Return Me._AuthNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AuthNumber, value) = False) Then
                    Me._AuthNumber = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property AuthName() As String
            Get
                Return Me._AuthName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AuthName, value) = False) Then
                    Me._AuthName = value
                End If
            End Set
        End Property

        <DataMember()> _
       Public Property AuthAddress() As String
            Get
                Return Me._AuthAddress
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AuthAddress, value) = False) Then
                    Me._AuthAddress = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property AuthCity() As String
            Get
                Return Me._AuthCity
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AuthCity, value) = False) Then
                    Me._AuthCity = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property AuthState() As String
            Get
                Return Me._AuthState
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AuthState, value) = False) Then
                    Me._AuthState = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property AuthZip() As String
            Get
                Return Me._AuthZip
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AuthZip, value) = False) Then
                    Me._AuthZip = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property version() As String
            Get
                Return Me._version
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._version, value) = False) Then
                    Me._version = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property xactversion() As String
            Get
                Return Me._xactversion
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._xactversion, value) = False) Then
                    Me._xactversion = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property masversion() As String
            Get
                Return Me._masversion
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._masversion, value) = False) Then
                    Me._masversion = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property claimversion() As String
            Get
                Return Me._claimversion
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._claimversion, value) = False) Then
                    Me._claimversion = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ServerLastMod() As System.Nullable(Of Date)
            Get
                Return Me._ServerLastMod
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._ServerLastMod.Equals(value) = False) Then
                    Me._ServerLastMod = value
                End If
            End Set
        End Property
#End Region


    End Class

End Namespace
