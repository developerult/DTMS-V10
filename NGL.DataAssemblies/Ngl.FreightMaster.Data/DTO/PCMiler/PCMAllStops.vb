Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


 
Namespace DataTransferObjects
    <DataContract()> _
    Public Class PCMAllStops
        Inherits DTOBaseClass

#Region " Data Members"
         

        Private mstrFailedAddressMessage As String = ""
        Public Property FailedAddressMessage() As String
            Get
                Return mstrFailedAddressMessage
            End Get
            Set(ByVal value As String)
                mstrFailedAddressMessage = value
            End Set
        End Property

        Private mintBadAddressCount As Integer = 0
        Public Property BadAddressCount() As Integer
            Get
                Return mintBadAddressCount
            End Get
            Set(ByVal value As Integer)
                mintBadAddressCount = value
            End Set
        End Property

        Private mdblTotalMiles As Double = 0
        Public Property TotalMiles() As Double
            Get
                Return mdblTotalMiles
            End Get
            Set(ByVal value As Double)
                mdblTotalMiles = value
            End Set
        End Property

        Private mstrOriginZip As String = ""
        Public Property OriginZip() As String
            Get
                Return mstrOriginZip
            End Get
            Set(ByVal value As String)
                mstrOriginZip = value
            End Set
        End Property

        Private mstrDestZip As String = ""
        Public Property DestZip() As String
            Get
                Return mstrDestZip
            End Get
            Set(ByVal value As String)
                mstrDestZip = value
            End Set
        End Property

        Private mdblAutoCorrectBadLaneZipCodes As Double = 0
        Public Property AutoCorrectBadLaneZipCodes() As Double
            Get
                Return mdblAutoCorrectBadLaneZipCodes
            End Get
            Set(ByVal value As Double)
                mdblAutoCorrectBadLaneZipCodes = value
            End Set
        End Property

        Private mdblBatchID As Double = 0
        Public Property BatchID() As Double
            Get
                Return mdblBatchID
            End Get
            Set(ByVal value As Double)
                mdblBatchID = value
            End Set
        End Property

        Private mstrLastError As String = ""
        Public Property LastError() As String
            Get
                Return mstrLastError
            End Get
            Set(ByVal value As String)
                mstrLastError = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New PCMAllStops
            instance = DirectCast(MemberwiseClone(), PCMAllStops)
            Return instance
        End Function

#End Region


    End Class
End Namespace
