Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class TrackableString
        Inherits DTOBaseClass


#Region " Data Members"

        Private _Text As String = ""
        <DataMember()> _
        Public Property Text() As String
            Get
                Return _Text
            End Get
            Set(ByVal value As String)
                _Text = value
                NotifyPropertyChanged("Text")
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New TrackableString
            instance = DirectCast(MemberwiseClone(), TrackableString)
            Return instance
        End Function

#End Region

    End Class
End Namespace
