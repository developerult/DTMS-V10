Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class AllStop
        Inherits DTOBaseClass


#Region " Data Members"

        Private mstrConsNumber As String = ""
        <DataMember()> _
        Public Property ConsNumber() As String
            Get
                ConsNumber = Left(mstrConsNumber, 20)
            End Get
            Set(ByVal Value As String)
                mstrConsNumber = Value
                NotifyPropertyChanged("ConsNumber")
            End Set
        End Property

        Private mdblDistToPrev As Double = 0
        <DataMember()> _
        Public Property DistToPrev() As Double
            Get
                DistToPrev = mdblDistToPrev
            End Get
            Set(ByVal Value As Double)
                mdblDistToPrev = Value
                NotifyPropertyChanged("DistToPrev")
            End Set
        End Property

        Private mdblTotalRouteCost As Double = 0
        <DataMember()> _
        Public Property TotalRouteCost() As Double
            Get
                TotalRouteCost = mdblTotalRouteCost
            End Get
            Set(ByVal Value As Double)
                mdblTotalRouteCost = Value
                NotifyPropertyChanged("TotalRouteCost")
            End Set
        End Property


        Private mintSeqNbr As Short = 0
        <DataMember()> _
        Public Property SeqNbr() As Short
            Get
                SeqNbr = mintSeqNbr
            End Get
            Set(ByVal Value As Short)
                mintSeqNbr = Value
                NotifyPropertyChanged("SeqNbr")
            End Set
        End Property


        Private mstrTruckDesignator As String = ""
        <DataMember()> _
        Public Property TruckDesignator() As String
            Get
                TruckDesignator = mstrTruckDesignator
            End Get
            Set(ByVal Value As String)
                mstrTruckDesignator = Left(Value, 12)
                NotifyPropertyChanged("TruckDesignator")
            End Set
        End Property

        Private mlngTruckNumber As Integer = 0
        <DataMember()> _
        Public Property TruckNumber() As Integer
            Get
                TruckNumber = mlngTruckNumber
            End Get
            Set(ByVal Value As Integer)
                mlngTruckNumber = Value
                NotifyPropertyChanged("TruckNumber")
            End Set
        End Property

        Private mintStopNumber As Short = 0
        <DataMember()> _
        Public Property StopNumber() As Short
            Get
                StopNumber = mintStopNumber
            End Get
            Set(ByVal Value As Short)
                mintStopNumber = Value
                NotifyPropertyChanged("StopNumber")
            End Set
        End Property

        Private mstrStopName As String = ""
        <DataMember()> _
        Public Property Stopname() As String
            Get
                Stopname = mstrStopName
            End Get
            Set(ByVal Value As String)
                mstrStopName = Left(Value, 20)
                NotifyPropertyChanged("Stopname")
            End Set
        End Property

        Private mstrID1 As String = ""
        <DataMember()> _
        Public Property ID1() As String
            Get
                ID1 = mstrID1
            End Get
            Set(ByVal Value As String)
                mstrID1 = Left(Value, 15)
                NotifyPropertyChanged("ID1")
            End Set
        End Property

        Private mstrID2 As String = ""
        <DataMember()> _
        Public Property ID2() As String
            Get
                ID2 = mstrID2
            End Get
            Set(ByVal Value As String)
                mstrID2 = Left(Value, 10)
                NotifyPropertyChanged("ID2")
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AllStop
            instance = DirectCast(MemberwiseClone(), AllStop)
            Return instance
        End Function

#End Region

    End Class

End Namespace
