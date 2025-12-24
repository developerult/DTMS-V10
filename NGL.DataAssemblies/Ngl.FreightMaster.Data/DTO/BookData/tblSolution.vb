Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblSolution
        Inherits DTOBaseClass


#Region " Data Members"
        Private _SolutionControl As Long = 0
        <DataMember()> _
        Public Property SolutionControl() As Long
            Get
                Return Me._SolutionControl
            End Get
            Set(ByVal value As Long)
                If ((Me._SolutionControl = value) = False) Then
                    Me._SolutionControl = value
                    Me.SendPropertyChanged("SolutionControl")
                End If
            End Set
        End Property

        Private _SolutionAttributeControl As Long = 0
        <DataMember()> _
        Public Property SolutionAttributeControl() As Integer
            Get
                Return Me._SolutionAttributeControl
            End Get
            Set(value As Integer)
                If ((Me._SolutionAttributeControl = value) _
                   = False) Then
                    Me._SolutionAttributeControl = value
                    Me.SendPropertyChanged("SolutionAttributeControl")
                End If
            End Set
        End Property

        Private _SolutionAttributeTypeControl As Long = 0
        <DataMember()> _
        Public Property SolutionAttributeTypeControl() As Integer
            Get
                Return Me._SolutionAttributeTypeControl
            End Get
            Set(value As Integer)
                If ((Me._SolutionAttributeTypeControl = value) _
                   = False) Then
                    Me._SolutionAttributeTypeControl = value
                    Me.SendPropertyChanged("SolutionAttributeTypeControl")
                End If
            End Set
        End Property

        Private _SolutionName As String = ""
        <DataMember()> _
        Public Property SolutionName() As String
            Get
                Return Left(Me._SolutionName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionName, value) = False) Then
                    Me._SolutionName = Left(value, 50)
                    Me.SendPropertyChanged("SolutionName")
                End If
            End Set
        End Property

        Private _SolutionDescription As String = ""
        <DataMember()> _
        Public Property SolutionDescription() As String
            Get
                Return Left(Me._SolutionDescription, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDescription, value) = False) Then
                    Me._SolutionDescription = Left(value, 255)
                    Me.SendPropertyChanged("SolutionDescription")
                End If
            End Set
        End Property

        Private _SolutionCompControl As Integer = 0
        <DataMember()> _
        Public Property SolutionCompControl() As Integer
            Get
                Return _SolutionCompControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionCompControl = value) = False) Then
                    Me._SolutionCompControl = value
                    Me.SendPropertyChanged("SolutionCompControl")
                End If
            End Set
        End Property

        Private _SolutionCompNumber As Integer = 0
        <DataMember()> _
        Public Property SolutionCompNumber() As Integer
            Get
                Return _SolutionCompNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionCompNumber = value) = False) Then
                    Me._SolutionCompNumber = value
                    Me.SendPropertyChanged("SolutionCompNumber")
                End If
            End Set
        End Property

        Private _SolutionCompName As String = ""
        <DataMember()> _
        Public Property SolutionCompName() As String
            Get
                Return Left(Me._SolutionCompName, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionCompName, value) = False) Then
                    Me._SolutionCompName = Left(value, 40)
                    Me.SendPropertyChanged("SolutionCompName")
                End If
            End Set
        End Property

        Private _SolutionCompNatNumber As Integer = 0
        <DataMember()> _
        Public Property SolutionCompNatNumber() As Integer
            Get
                Return Me._SolutionCompNatNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionCompNatNumber = value) = False) Then
                    Me._SolutionCompNatNumber = value
                    Me.SendPropertyChanged("SolutionCompNatNumber")
                End If
            End Set
        End Property

        Private _SolutionCompNatName As String = ""
        <DataMember()> _
        Public Property SolutionCompNatName() As String
            Get
                Return Left(Me._SolutionCompNatName, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionCompNatName, value) = False) Then
                    Me._SolutionCompNatName = Left(value, 40)
                    Me.SendPropertyChanged("SolutionCompNatName")
                End If
            End Set
        End Property

        Private _SolutionCreateDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionCreateDate() As System.Nullable(Of Date)
            Get
                Return Me._SolutionCreateDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._SolutionCreateDate.Equals(value) = False) Then
                    Me._SolutionCreateDate = value
                    Me.SendPropertyChanged("SolutionCreateDate")
                End If
            End Set
        End Property

        Private _SolutionTotalCases As Integer = 0
        <DataMember()> _
        Public Property SolutionTotalCases() As Integer
            Get
                Return _SolutionTotalCases
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTotalCases = value) = False) Then
                    Me._SolutionTotalCases = value
                    Me.SendPropertyChanged("SolutionTotalCases")
                End If
            End Set
        End Property

        Private _SolutionTotalWgt As Double = 0
        <DataMember()> _
        Public Property SolutionTotalWgt() As Double
            Get
                Return _SolutionTotalWgt
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTotalWgt = value) = False) Then
                    Me._SolutionTotalWgt = value
                    Me.SendPropertyChanged("SolutionTotalWgt")
                End If
            End Set
        End Property

        Private _SolutionTotalPL As Double = 0
        <DataMember()> _
        Public Property SolutionTotalPL() As Double
            Get
                Return _SolutionTotalPL
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTotalPL = value) = False) Then
                    Me._SolutionTotalPL = value
                    Me.SendPropertyChanged("SolutionTotalPL")
                End If
            End Set
        End Property

        Private _SolutionTotalCube As Integer = 0
        <DataMember()> _
        Public Property SolutionTotalCube() As Integer
            Get
                Return _SolutionTotalCube
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTotalCube = value) = False) Then
                    Me._SolutionTotalCube = value
                    Me.SendPropertyChanged("SolutionTotalCube")
                End If
            End Set
        End Property

        Private _SolutionTotalPX As Integer = 0
        <DataMember()> _
        Public Property SolutionTotalPX() As Integer
            Get
                Return _SolutionTotalPX
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTotalPX = value) = False) Then
                    Me._SolutionTotalPX = value
                    Me.SendPropertyChanged("SolutionTotalPX")
                End If
            End Set
        End Property

        Private _SolutionTotalBFC As Decimal = 0
        <DataMember()> _
        Public Property SolutionTotalBFC() As Decimal
            Get
                Return _SolutionTotalBFC
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionTotalBFC = value) = False) Then
                    Me._SolutionTotalBFC = value
                    Me.SendPropertyChanged("SolutionTotalBFC")
                End If
            End Set
        End Property

        Private _SolutionTotalOrders As Integer = 0
        <DataMember()> _
        Public Property SolutionTotalOrders() As Integer
            Get
                Return _SolutionTotalOrders
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTotalOrders = value) = False) Then
                    Me._SolutionTotalOrders = value
                    Me.SendPropertyChanged("SolutionTotalOrders")
                End If
            End Set
        End Property

        Private _SolutionTotalTLCost As Double = 0
        <DataMember()> _
        Public Property SolutionTotalTLCost() As Double
            Get
                Return _SolutionTotalTLCost
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTotalTLCost = value) = False) Then
                    Me._SolutionTotalTLCost = value
                    Me.SendPropertyChanged("SolutionTotalTLCost")
                End If
            End Set
        End Property

        Private _SolutionTotalMPCost As Double = 0
        <DataMember()> _
        Public Property SolutionTotalMPCost() As Double
            Get
                Return _SolutionTotalMPCost
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTotalMPCost = value) = False) Then
                    Me._SolutionTotalMPCost = value
                    Me.SendPropertyChanged("SolutionTotalMPCost")
                End If
            End Set
        End Property

        Private _SolutionTotalPoolCost As Double = 0
        <DataMember()> _
        Public Property SolutionTotalPoolCost() As Double
            Get
                Return _SolutionTotalPoolCost
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTotalPoolCost = value) = False) Then
                    Me._SolutionTotalPoolCost = value
                    Me.SendPropertyChanged("SolutionTotalPoolCost")
                End If
            End Set
        End Property

        Private _SolutionTotalLTLPoolCost As Double = 0
        <DataMember()> _
        Public Property SolutionTotalLTLPoolCost() As Double
            Get
                Return _SolutionTotalLTLPoolCost
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTotalLTLPoolCost = value) = False) Then
                    Me._SolutionTotalLTLPoolCost = value
                    Me.SendPropertyChanged("SolutionTotalLTLPoolCost")
                End If
            End Set
        End Property

        Private _SolutionTotalLTLCost As Double = 0
        <DataMember()> _
        Public Property SolutionTotalLTLCost() As Double
            Get
                Return _SolutionTotalLTLCost
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTotalLTLCost = value) = False) Then
                    Me._SolutionTotalLTLCost = value
                    Me.SendPropertyChanged("SolutionTotalLTLCost")
                End If
            End Set
        End Property

        Private _SolutionTotalCost As Double = 0
        <DataMember()> _
        Public Property SolutionTotalCost() As Double
            Get
                Return _SolutionTotalCost
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTotalCost = value) = False) Then
                    Me._SolutionTotalCost = value
                    Me.SendPropertyChanged("SolutionTotalCost")
                End If
            End Set
        End Property

        Private _SolutionTotalTrucks As Double = 0
        <DataMember()> _
        Public Property SolutionTotalTrucks() As Double
            Get
                Return _SolutionTotalTrucks
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTotalTrucks = value) = False) Then
                    Me._SolutionTotalTrucks = value
                    Me.SendPropertyChanged("SolutionTotalTrucks")
                End If
            End Set
        End Property

        Private _SolutionTotalMiles As Double = 0
        <DataMember()> _
        Public Property SolutionTotalMiles() As Double
            Get
                Return _SolutionTotalMiles
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTotalMiles = value) = False) Then
                    Me._SolutionTotalMiles = value
                    Me.SendPropertyChanged("SolutionTotalMiles")
                End If
            End Set
        End Property

        Private _SolutionCommitted As Boolean = False
        <DataMember()> _
        Public Property SolutionCommitted() As Boolean
            Get
                Return _SolutionCommitted
            End Get
            Set(ByVal value As Boolean)
                If ((Me._SolutionCommitted = value) = False) Then
                    Me._SolutionCommitted = value
                    Me.SendPropertyChanged("SolutionCommitted")
                End If
            End Set
        End Property

        Private _SolutionCommittedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionCommittedDate() As System.Nullable(Of Date)
            Get
                Return Me._SolutionCommittedDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._SolutionCommittedDate.Equals(value) = False) Then
                    Me._SolutionCommittedDate = value
                    Me.SendPropertyChanged("SolutionCommittedDate")
                End If
            End Set
        End Property

        Private _SolutionArchived As Boolean = False
        <DataMember()> _
        Public Property SolutionArchived() As Boolean
            Get
                Return _SolutionArchived
            End Get
            Set(ByVal value As Boolean)
                If ((Me._SolutionArchived = value) = False) Then
                    Me._SolutionArchived = value
                    Me.SendPropertyChanged("SolutionArchived")
                End If
            End Set
        End Property

        Private _SolutionArchivedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionArchivedDate() As System.Nullable(Of Date)
            Get
                Return Me._SolutionArchivedDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._SolutionArchivedDate.Equals(value) = False) Then
                    Me._SolutionArchivedDate = value
                    Me.SendPropertyChanged("SolutionArchivedDate")
                End If
            End Set
        End Property

        Private _SolutionModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionModDate() As System.Nullable(Of Date)
            Get
                Return _SolutionModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _SolutionModDate = value
                Me.SendPropertyChanged("SolutionModDate")
            End Set
        End Property

        Private _SolutionModUser As String = ""
        <DataMember()> _
        Public Property SolutionModUser() As String
            Get
                Return Left(_SolutionModUser, 100)
            End Get
            Set(ByVal value As String)
                _SolutionModUser = Left(value, 100)
                Me.SendPropertyChanged("SolutionModUser")
            End Set
        End Property

        Private _SolutionUpdated As Byte()
        <DataMember()> _
        Public Property SolutionUpdated() As Byte()
            Get
                Return _SolutionUpdated
            End Get
            Set(ByVal value As Byte())
                _SolutionUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblSolution
            instance = DirectCast(MemberwiseClone(), tblSolution)
            Return instance
        End Function

#End Region

    End Class
End Namespace