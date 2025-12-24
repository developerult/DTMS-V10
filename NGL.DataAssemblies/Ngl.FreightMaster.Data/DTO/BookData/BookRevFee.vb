Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookRevFee
        Inherits DTOBaseClass


#Region " Data Members"

        Private _FeeName As String = ""
        <DataMember()> _
        Public Property FeeName() As String
            Get
                Return _FeeName
            End Get
            Friend Set(ByVal value As String)
                _FeeName = value
            End Set
        End Property


        Private _FeeCost As Decimal = 0
        <DataMember()> _
        Public Property FeeCost() As Decimal
            Get
                Return _FeeCost
            End Get
            Friend Set(ByVal value As Decimal)
                _FeeCost = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookRevFee
            instance = DirectCast(MemberwiseClone(), BookRevFee)
            Return instance
        End Function

#End Region


    End Class

End Namespace