Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class NGLStoredProcedureParameter
        Inherits DTOBaseClass


#Region " Data Members"


        Private _ParName As String = ""
        <DataMember()> _
        Public Property ParName() As String
            Get
                Return _ParName
            End Get
            Set(ByVal value As String)
                _ParName = value
            End Set
        End Property

        Private _ParValue As String = ""
        <DataMember()> _
        Public Property ParValue() As String
            Get
                Return _ParValue
            End Get
            Set(ByVal value As String)
                _ParValue = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New NGLStoredProcedureParameter
            instance = DirectCast(MemberwiseClone(), NGLStoredProcedureParameter)
            Return instance
        End Function

#End Region

    End Class

End Namespace

