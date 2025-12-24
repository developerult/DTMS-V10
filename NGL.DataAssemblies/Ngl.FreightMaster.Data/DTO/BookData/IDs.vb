Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class IDs
        Inherits DTOBaseClass

#Region " Data Members"

        <DataMember()> _
        Public Property Control() As Integer
            Get
                Return m_Control
            End Get
            Set(ByVal value As Integer)
                m_Control = value
            End Set
        End Property
        Private m_Control As Integer
         
#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New IDs
            instance = DirectCast(MemberwiseClone(), IDs)
            Return instance
        End Function

#End Region

    End Class
End Namespace

