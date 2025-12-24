Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class Hazmat
        Inherits DTOBaseClass


#Region " Data Members"

        Private _HazControl As Integer = 0
        <DataMember()> _
        Public Property HazControl() As Integer
            Get
                Return Me._HazControl
            End Get
            Set(value As Integer)
                If ((Me._HazControl = value) _
                   = False) Then
                    Me._HazControl = value
                    Me.SendPropertyChanged("HazControl")
                End If
            End Set
        End Property

        Private _HazRegulation As String = ""
        <DataMember()> _
        Public Property HazRegulation() As String
            Get
                Return Left(Me._HazRegulation, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazRegulation, value) = False) Then
                    Me._HazRegulation = Left(value, 50)
                    Me.SendPropertyChanged("HazRegulation")
                End If
            End Set
        End Property

        Private _HazItem As String = ""
        <DataMember()> _
        Public Property HazItem() As String
            Get
                Return Left(Me._HazItem, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazItem, value) = False) Then
                    Me._HazItem = Left(value, 50)
                    Me.SendPropertyChanged("HazItem")
                End If
            End Set
        End Property

        Private _HazClass As String = ""
        <DataMember()> _
        Public Property HazClass() As String
            Get
                Return Left(Me._HazClass, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazClass, value) = False) Then
                    Me._HazClass = Left(value, 20)
                    Me.SendPropertyChanged("HazClass")
                End If
            End Set
        End Property
        Private _HazID As String = ""
        <DataMember()> _
        Public Property HazID() As String
            Get
                Return Left(Me._HazID, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazID, value) = False) Then
                    Me._HazID = Left(value, 20)
                    Me.SendPropertyChanged("HazID")
                End If
            End Set
        End Property

        Private _HazDesc01 As String = ""
        <DataMember()> _
        Public Property HazDesc01() As String
            Get
                Return Left(Me._HazDesc01, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazDesc01, value) = False) Then
                    Me._HazDesc01 = Left(value, 255)
                    Me.SendPropertyChanged("HazDesc01")
                End If
            End Set
        End Property

        Private _HazDesc02 As String = ""
        <DataMember()> _
        Public Property HazDesc02() As String
            Get
                Return Left(Me._HazDesc02, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazDesc02, value) = False) Then
                    Me._HazDesc02 = Left(value, 255)
                    Me.SendPropertyChanged("HazDesc02")
                End If
            End Set
        End Property

        Private _HazDesc03 As String = ""
        <DataMember()> _
        Public Property HazDesc03() As String
            Get
                Return Left(Me._HazDesc03, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazDesc03, value) = False) Then
                    Me._HazDesc03 = Left(value, 255)
                    Me.SendPropertyChanged("HazDesc03")
                End If
            End Set
        End Property

        Private _HazUnit As String = ""
        <DataMember()> _
        Public Property HazUnit() As String
            Get
                Return Left(Me._HazUnit, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazUnit, value) = False) Then
                    Me._HazUnit = Left(value, 100)
                    Me.SendPropertyChanged("HazUnit")
                End If
            End Set
        End Property

        Private _HazPackingGroup As String = ""
        <DataMember()> _
        Public Property HazPackingGroup() As String
            Get
                Return Left(Me._HazPackingGroup, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazPackingGroup, value) = False) Then
                    Me._HazPackingGroup = Left(value, 10)
                    Me.SendPropertyChanged("HazPackingGroup")
                End If
            End Set
        End Property

        Private _HazPackingDesc As String = ""
        <DataMember()> _
        Public Property HazPackingDesc() As String
            Get
                Return Left(Me._HazPackingDesc, 1000)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazPackingDesc, value) = False) Then
                    Me._HazPackingDesc = Left(value, 1000)
                    Me.SendPropertyChanged("HazPackingDesc")
                End If
            End Set
        End Property

        Private _HazShipInst As String = ""
        <DataMember()> _
        Public Property HazShipInst() As String
            Get
                Return Left(Me._HazShipInst, 1000)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazShipInst, value) = False) Then
                    Me._HazShipInst = Left(value, 1000)
                    Me.SendPropertyChanged("HazShipInst")
                End If
            End Set
        End Property

        Private _HazLtdQ As Boolean = False
        <DataMember()> _
        Public Property HazLtdQ() As Boolean
            Get
                Return Me._HazLtdQ
            End Get
            Set(value As Boolean)
                If ((Me._HazLtdQ = value) _
                   = False) Then
                    Me._HazLtdQ = value
                    Me.SendPropertyChanged("HazLtdQ")
                End If
            End Set
        End Property

        Private _HazMarPoll As Boolean = False
        <DataMember()> _
        Public Property HazMarPoll() As Boolean
            Get
                Return Me._HazMarPoll
            End Get
            Set(value As Boolean)
                If ((Me._HazMarPoll = value) _
                   = False) Then
                    Me._HazMarPoll = value
                    Me.SendPropertyChanged("HazMarPoll")
                End If
            End Set
        End Property

        Private _HazMarStorCat As String = ""
        <DataMember()> _
        Public Property HazMarStorCat() As String
            Get
                Return Left(Me._HazMarStorCat, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazMarStorCat, value) = False) Then
                    Me._HazMarStorCat = Left(value, 50)
                    Me.SendPropertyChanged("HazMarStorCat")
                End If
            End Set
        End Property

        Private _HazNMFCSub As Integer = 0
        <DataMember()> _
        Public Property HazNMFCSub() As Integer
            Get
                Return Me._HazNMFCSub
            End Get
            Set(value As Integer)
                If ((Me._HazNMFCSub = value) _
                   = False) Then
                    Me._HazNMFCSub = value
                    Me.SendPropertyChanged("HazNMFCSub")
                End If
            End Set
        End Property

        Private _HazNMFC As Integer = 0
        <DataMember()> _
        Public Property HazNMFC() As Integer
            Get
                Return Me._HazNMFC
            End Get
            Set(value As Integer)
                If ((Me._HazNMFC = value) _
                   = False) Then
                    Me._HazNMFC = value
                    Me.SendPropertyChanged("HazNMFC")
                End If
            End Set
        End Property

        Private _HazFrtClass As Integer = 0
        <DataMember()> _
        Public Property HazFrtClass() As Integer
            Get
                Return Me._HazFrtClass
            End Get
            Set(value As Integer)
                If ((Me._HazFrtClass = value) _
                   = False) Then
                    Me._HazFrtClass = value
                    Me.SendPropertyChanged("HazFrtClass")
                End If
            End Set
        End Property

        Private _HazFdxGndOK As Boolean = False
        <DataMember()> _
        Public Property HazFdxGndOK() As Boolean
            Get
                Return Me._HazFdxGndOK
            End Get
            Set(value As Boolean)
                If ((Me._HazFdxGndOK = value) _
                   = False) Then
                    Me._HazFdxGndOK = value
                    Me.SendPropertyChanged("HazFdxGndOK")
                End If
            End Set
        End Property

        Private _HazFdxAirOK As Boolean = False
        <DataMember()> _
        Public Property HazFdxAirOK() As Boolean
            Get
                Return Me._HazFdxAirOK
            End Get
            Set(value As Boolean)
                If ((Me._HazFdxAirOK = value) _
                   = False) Then
                    Me._HazFdxAirOK = value
                    Me.SendPropertyChanged("HazFdxAirOK")
                End If
            End Set
        End Property

        Private _HazUPSgndOK As Boolean = False
        <DataMember()> _
        Public Property HazUPSgndOK() As Boolean
            Get
                Return Me._HazUPSgndOK
            End Get
            Set(value As Boolean)
                If ((Me._HazUPSgndOK = value) _
                   = False) Then
                    Me._HazUPSgndOK = value
                    Me.SendPropertyChanged("HazUPSgndOK")
                End If
            End Set
        End Property

        Private _HazUPSAirOK As Boolean = False
        <DataMember()> _
        Public Property HazUPSAirOK() As Boolean
            Get
                Return Me._HazUPSAirOK
            End Get
            Set(value As Boolean)
                If ((Me._HazUPSAirOK = value) _
                   = False) Then
                    Me._HazUPSAirOK = value
                    Me.SendPropertyChanged("HazUPSAirOK")
                End If
            End Set
        End Property

        Private _HazModUser As String = ""
        <DataMember()> _
        Public Property HazModUser() As String
            Get
                Return Left(Me._HazModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazModUser, value) = False) Then
                    Me._HazModUser = Left(value, 100)
                    Me.SendPropertyChanged("HazModUser")
                End If
            End Set
        End Property

        Private _HazModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property HazModDate() As System.Nullable(Of Date)
            Get
                Return Me._HazModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._HazModDate.Equals(value) = False) Then
                    Me._HazModDate = value
                    Me.SendPropertyChanged("HazModDate")
                End If
            End Set
        End Property


        Private _HazUpdated As Byte()
        <DataMember()>
        Public Property HazUpdated() As Byte()
            Get
                Return _HazUpdated
            End Get
            Set(ByVal value As Byte())
                _HazUpdated = value
            End Set
        End Property

        Private _HazLabelCode As String = ""
        <DataMember()>
        Public Property HazLabelCode() As String
            Get
                Return Left(Me._HazLabelCode, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazLabelCode, value) = False) Then
                    Me._HazLabelCode = Left(value, 20)
                    Me.SendPropertyChanged("HazLabelCode")
                End If
            End Set
        End Property

        Private _HazSymbols As String = ""
        <DataMember()>
        Public Property HazSymbols() As String
            Get
                Return Left(Me._HazSymbols, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazSymbols, value) = False) Then
                    Me._HazSymbols = Left(value, 20)
                    Me.SendPropertyChanged("HazSymbols")
                End If
            End Set
        End Property

        Private _HazSpecialProvisions As String = ""
        <DataMember()>
        Public Property HazSpecialProvisions() As String
            Get
                Return Left(Me._HazSpecialProvisions, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazSpecialProvisions, value) = False) Then
                    Me._HazSpecialProvisions = Left(value, 50)
                    Me.SendPropertyChanged("HazSpecialProvisions")
                End If
            End Set
        End Property

        Private _HazPackagingExceptions As String = ""
        <DataMember()>
        Public Property HazPackagingExceptions() As String
            Get
                Return Left(Me._HazPackagingExceptions, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazPackagingExceptions, value) = False) Then
                    Me._HazPackagingExceptions = Left(value, 20)
                    Me.SendPropertyChanged("HazPackagingExceptions")
                End If
            End Set
        End Property

        Private _HazPackagingNonBulk As String = ""
        <DataMember()>
        Public Property HazPackagingNonBulk() As String
            Get
                Return Left(Me._HazPackagingNonBulk, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazPackagingNonBulk, value) = False) Then
                    Me._HazPackagingNonBulk = Left(value, 20)
                    Me.SendPropertyChanged("HazPackagingNonBulk")
                End If
            End Set
        End Property

        Private _HazPackagingBulk As String = ""
        <DataMember()>
        Public Property HazPackagingBulk() As String
            Get
                Return Left(Me._HazPackagingBulk, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazPackagingBulk, value) = False) Then
                    Me._HazPackagingBulk = Left(value, 20)
                    Me.SendPropertyChanged("HazPackagingBulk")
                End If
            End Set
        End Property

        Private _HazQLPassAirRail As String = ""
        <DataMember()>
        Public Property HazQLPassAirRail() As String
            Get
                Return Left(Me._HazQLPassAirRail, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazQLPassAirRail, value) = False) Then
                    Me._HazQLPassAirRail = Left(value, 20)
                    Me.SendPropertyChanged("HazQLPassAirRail")
                End If
            End Set
        End Property

        Private _HazQLCargoAir As String = ""
        <DataMember()>
        Public Property HazQLCargoAir() As String
            Get
                Return Left(Me._HazQLCargoAir, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazQLCargoAir, value) = False) Then
                    Me._HazQLCargoAir = Left(value, 20)
                    Me.SendPropertyChanged("HazQLCargoAir")
                End If
            End Set
        End Property

        Private _HazStowageLocation As String = ""
        <DataMember()>
        Public Property HazStowageLocation() As String
            Get
                Return Left(Me._HazStowageLocation, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazStowageLocation, value) = False) Then
                    Me._HazStowageLocation = Left(value, 20)
                    Me.SendPropertyChanged("HazStowageLocation")
                End If
            End Set
        End Property

        Private _HazStowageOther As String = ""
        <DataMember()>
        Public Property HazStowageOther() As String
            Get
                Return Left(Me._HazStowageOther, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._HazStowageOther, value) = False) Then
                    Me._HazStowageOther = Left(value, 20)
                    Me.SendPropertyChanged("HazStowageOther")
                End If
            End Set
        End Property



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New Hazmat
            instance = DirectCast(MemberwiseClone(), Hazmat)
            Return instance
        End Function

#End Region

    End Class
End Namespace