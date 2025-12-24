Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class vBookMaintLookup
        Inherits DTOBaseClass

#Region " Data Members"

        Private _BookControl As Integer = 0
        <DataMember()> _
        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property


        Private _BookProNumber As String = ""
        <DataMember()> _
        Public Property BookProNumber() As String
            Get
                Return _BookProNumber
            End Get
            Set(ByVal value As String)
                _BookProNumber = value
            End Set
        End Property

        Private _BookConsPrefix As String = ""
        <DataMember()> _
        Public Property BookConsPrefix() As String
            Get
                Return _BookConsPrefix
            End Get
            Set(ByVal value As String)
                _BookConsPrefix = value
            End Set
        End Property

        Private _BookCarrOrderNumberSeq As String = ""
        <DataMember()> _
        Public Property BookCarrOrderNumberSeq() As String
            Get
                Return _BookCarrOrderNumberSeq
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumberSeq = value
            End Set
        End Property


        Private _BookOrderSequence As Integer = 0
        <DataMember()> _
        Public Property BookOrderSequence() As Integer
            Get
                Return _BookOrderSequence
            End Get
            Set(ByVal value As Integer)
                _BookOrderSequence = value
            End Set
        End Property

        Private _BookCarrOrderNumber As String = ""
        <DataMember()> _
        Public Property BookCarrOrderNumber() As String
            Get
                Return _BookCarrOrderNumber
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumber = value
            End Set
        End Property

        Private _BookLoadPONumber As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber() As String
            Get
                Return _BookLoadPONumber
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber = value
            End Set
        End Property

        Private _BookCarrBLNumber As String = ""
        <DataMember()> _
        Public Property BookCarrBLNumber() As String
            Get
                Return _BookCarrBLNumber
            End Get
            Set(ByVal value As String)
                _BookCarrBLNumber = value
            End Set
        End Property

        Private _BookFinAPBillNumber As String = ""
        <DataMember()> _
        Public Property BookFinAPBillNumber() As String
            Get
                Return _BookFinAPBillNumber
            End Get
            Set(ByVal value As String)
                _BookFinAPBillNumber = value
            End Set
        End Property

        Private _BookCustCompControl As Integer = 0
        <DataMember()> _
        Public Property BookCustCompControl() As Integer
            Get
                Return _BookCustCompControl
            End Get
            Set(ByVal value As Integer)
                _BookCustCompControl = value
            End Set
        End Property

        Private _CompNumber As Integer = 0
        <DataMember()> _
        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set(ByVal value As Integer)
                _CompNumber = value
            End Set
        End Property

        Private _BookDestName As String = ""
        <DataMember()> _
        Public Property BookDestName() As String
            Get
                Return _BookDestName
            End Get
            Set(ByVal value As String)
                _BookDestName = value
            End Set
        End Property

        Private _BookDestCity As String = ""
        <DataMember()> _
        Public Property BookDestCity() As String
            Get
                Return _BookDestCity
            End Get
            Set(ByVal value As String)
                _BookDestCity = value
            End Set
        End Property

        Private _BookOrigName As String = ""
        <DataMember()> _
        Public Property BookOrigName() As String
            Get
                Return _BookOrigName
            End Get
            Set(ByVal value As String)
                _BookOrigName = value
            End Set
        End Property

        Private _BookOrigCity As String = ""
        <DataMember()> _
        Public Property BookOrigCity() As String
            Get
                Return _BookOrigCity
            End Get
            Set(ByVal value As String)
                _BookOrigCity = value
            End Set
        End Property

        Private _BookSHID As String = ""
        <DataMember()> _
        Public Property BookSHID() As String
            Get
                Return Left(_BookSHID, 50)
            End Get
            Set(ByVal value As String)
                _BookSHID = Left(value, 50)
            End Set
        End Property

        Private _BookShipCarrierProNumber As String = ""
        <DataMember()> _
        Public Property BookShipCarrierProNumber() As String
            Get
                Return Left(_BookShipCarrierProNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierProNumber = Left(value, 20)
            End Set
        End Property



#End Region



#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New vBookMaintLookup
            instance = DirectCast(MemberwiseClone(), vBookMaintLookup)
            Return instance
        End Function

#End Region
    End Class
End Namespace