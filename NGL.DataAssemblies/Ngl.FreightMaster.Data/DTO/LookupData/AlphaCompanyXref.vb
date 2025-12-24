Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class AlphaCompanyXref
        Inherits DTOBaseClass


#Region " Data Members"
        Private _ACXControl As Integer

        Private _ACXCompNumber As Integer

        Private _ACXAlphaNumber As String

        Private _ACXUpdated As Byte()
        <DataMember()> _
        Public Property ACXControl() As Integer
            Get
                Return Me._ACXControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._ACXControl = value) _
                   = False) Then
                    Me._ACXControl = value
                    NotifyPropertyChanged("ACXControl")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ACXCompNumber() As Integer
            Get
                Return Me._ACXCompNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._ACXCompNumber = value) _
                   = False) Then
                    Me._ACXCompNumber = value
                    NotifyPropertyChanged("ACXCompNumber")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ACXAlphaNumber() As String
            Get
                Return Me._ACXAlphaNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ACXAlphaNumber, value) = False) Then
                    Me._ACXAlphaNumber = value
                    NotifyPropertyChanged("ACXAlphaNumber")
                End If
            End Set
        End Property

        
        <DataMember()> _
        Public Property ACXUpdated() As Byte()
            Get
                Return _ACXUpdated
            End Get
            Set(ByVal value As Byte())
                _ACXUpdated = value
                NotifyPropertyChanged("ACXUpdated")
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AlphaCompanyXref
            instance = DirectCast(MemberwiseClone(), AlphaCompanyXref)
            Return instance
        End Function

#End Region

    End Class

End Namespace

