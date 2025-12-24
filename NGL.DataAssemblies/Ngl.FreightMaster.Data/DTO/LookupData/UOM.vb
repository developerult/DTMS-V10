Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class UOM
        Inherits DTOBaseClass


#Region " Data Members"

        Private _UOMKey As String = ""
        <DataMember()> _
        Public Property UOMKey() As String
            Get
                Return Left(_UOMKey, 4)
            End Get
            Set(ByVal value As String)
                _UOMKey = Left(value, 4)
            End Set
        End Property

        Private _UOMUpdated As Byte()
        <DataMember()> _
        Public Property UOMUpdated() As Byte()
            Get
                Return _UOMUpdated
            End Get
            Set(ByVal value As Byte())
                _UOMUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New UOM
            instance = DirectCast(MemberwiseClone(), UOM)
            Return instance
        End Function

#End Region

    End Class
End Namespace
