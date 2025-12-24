Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class PCMBadAddress
        Inherits DTOBaseClass


#Region " Data Members"

        Private mintBookControl As Integer = 0
        <DataMember()> _
        Public Property BookControl() As Integer
            Get
                Return mintBookControl
            End Get
            Set(ByVal value As Integer)
                mintBookControl = value
                NotifyPropertyChanged("BookControl")
            End Set
        End Property

        Private mintLaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneControl() As Integer
            Get
                Return mintLaneControl
            End Get
            Set(ByVal value As Integer)
                mintLaneControl = value
                NotifyPropertyChanged("LaneControl")
            End Set
        End Property

        Private moobjOrig As New PCMAddress
        <DataMember()> _
        Public Property objOrig() As PCMAddress
            Get
                Return moobjOrig
            End Get
            Set(ByVal value As PCMAddress)
                moobjOrig = value
                NotifyPropertyChanged("objOrig")
            End Set
        End Property

        Private moobjDest As New PCMAddress
        <DataMember()> _
        Public Property objDest() As PCMAddress
            Get
                Return moobjDest
            End Get
            Set(ByVal value As PCMAddress)
                moobjDest = value
                NotifyPropertyChanged("objDest")
            End Set
        End Property

        Private moobjPCMOrig As New PCMAddress
        <DataMember()> _
        Public Property objPCMOrig() As PCMAddress
            Get
                Return moobjPCMOrig
            End Get
            Set(ByVal value As PCMAddress)
                moobjPCMOrig = value
                NotifyPropertyChanged("objPCMOrig")
            End Set
        End Property

        Private moobjPCMDest As New PCMAddress
        <DataMember()> _
        Public Property objPCMDest() As PCMAddress
            Get
                Return moobjPCMDest
            End Get
            Set(ByVal value As PCMAddress)
                moobjPCMDest = value
                NotifyPropertyChanged("objPCMDest")
            End Set
        End Property

        Private mstrMessage As String = ""
        <DataMember()> _
        Public Property Message() As String
            Get
                Return mstrMessage
            End Get
            Set(ByVal value As String)
                mstrMessage = value
                NotifyPropertyChanged("Message")
            End Set
        End Property

        Private mdblBatchID As Double = 0
        <DataMember()> _
        Public Property BatchID() As Double
            Get
                Return mdblBatchID
            End Get
            Set(ByVal value As Double)
                mdblBatchID = value
                NotifyPropertyChanged("BatchID")
            End Set
        End Property

#End Region


#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New PCMBadAddress
            instance = DirectCast(MemberwiseClone(), PCMBadAddress)
            Return instance
        End Function

#End Region

    End Class

End Namespace

