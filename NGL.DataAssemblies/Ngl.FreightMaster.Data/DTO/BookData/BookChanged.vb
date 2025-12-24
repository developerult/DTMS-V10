Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookChanged
        Inherits DTOBaseClass


#Region " Data Members"

        Private _AreThereChanges As Boolean = False

        <DataMember()> _
        Public Property AreThereChanges() As Boolean
            Get
                Return _AreThereChanges
            End Get
            Set(ByVal value As Boolean)
                _AreThereChanges = value
            End Set
        End Property


        Private _POChanges As New BookChanges
        <DataMember()> _
        Public Property POChanges() As BookChanges
            Get
                Return _POChanges
            End Get
            Set(ByVal value As BookChanges)
                _POChanges = value
            End Set
        End Property

        Private _BookChanges As New BookChanges
        <DataMember()> _
        Public Property BookChanges() As BookChanges
            Get
                Return _BookChanges
            End Get
            Set(ByVal value As BookChanges)
                _BookChanges = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookChanged
            instance = DirectCast(MemberwiseClone(), BookChanged)


            Return instance
        End Function

#End Region

    End Class
End Namespace
