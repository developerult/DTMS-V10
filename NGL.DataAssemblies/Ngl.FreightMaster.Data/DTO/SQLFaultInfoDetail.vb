Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class SQLFaultInfoDetail
        Inherits DTOBaseClass



#Region " Constructors "

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal strText As String)
            MyBase.New()
            Me.Text = strText
        End Sub

#End Region

#Region " Data Members"
        Private _Text As String = ""
        <DataMember()> _
        Public Property Text() As String
            Get
                Return _Text
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Text, value) = False) Then
                    Me._Text = value
                    Me.SendPropertyChanged("Text")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New SQLFaultInfoDetail
            instance = DirectCast(MemberwiseClone(), SQLFaultInfoDetail)
            Return instance
        End Function

#End Region

    End Class
End Namespace
