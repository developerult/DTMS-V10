Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarTree
        Inherits NGLTreeNode


#Region " Data Members"
        Private _CarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarControl() As Integer
            Get
                Return _CarrTarControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarControl <> value) Then
                    Me._CarrTarControl = value
                    Me.SendPropertyChanged("CarrTarControl")
                End If
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarTree
            instance = DirectCast(MemberwiseClone(), CarrTarTree)
            Return instance
        End Function

#End Region

    End Class
End Namespace

