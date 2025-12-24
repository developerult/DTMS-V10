Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LaneCode
        Inherits DTOBaseClass


#Region " Data Members"
        Private _LaneCodesControl As Long = 0
        <DataMember()> _
        Public Property LaneCodesControl() As Long
            Get
                Return _LaneCodesControl
            End Get
            Set(ByVal value As Long)
                _LaneCodesControl = value
            End Set
        End Property

        Private _LaneCodesLaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneCodesLaneControl() As Integer
            Get
                Return _LaneCodesLaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneCodesLaneControl = value
            End Set
        End Property

        Private _LaneCodesSpecialCodesControl As Long = 0
        <DataMember()> _
        Public Property LaneCodesSpecialCodesControl() As Long
            Get
                Return _LaneCodesSpecialCodesControl
            End Get
            Set(ByVal value As Long)
                _LaneCodesSpecialCodesControl = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LaneCode
            instance = DirectCast(MemberwiseClone(), LaneCode)
            Return instance
        End Function

#End Region

    End Class
End Namespace
