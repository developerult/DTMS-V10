Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblCheckDigitAlgorithm
        Inherits DTOBaseClass


#Region " Data Members"

        Private _ChkDigAlgControl As Integer = 0
        <DataMember()> _
        Public Property ChkDigAlgControl() As Integer
            Get
                Return Me._ChkDigAlgControl
            End Get
            Set(value As Integer)
                If ((Me._ChkDigAlgControl = value) _
                   = False) Then
                    Me._ChkDigAlgControl = value
                    Me.SendPropertyChanged("ChkDigAlgControl")
                End If
            End Set
        End Property

        Private _ChkDigAlgName As String
        <DataMember()> _
        Public Property ChkDigAlgName() As String
            Get
                Return Left(Me._ChkDigAlgName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._ChkDigAlgName, value) = False) Then
                    Me._ChkDigAlgName = Left(value, 50)
                    Me.SendPropertyChanged("ChkDigAlgName")
                End If
            End Set
        End Property

        Private _ChkDigAlgDesc As String
        <DataMember()> _
        Public Property ChkDigAlgDesc() As String
            Get
                Return Left(Me._ChkDigAlgDesc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._ChkDigAlgDesc, value) = False) Then
                    Me._ChkDigAlgDesc = Left(value, 255)
                    Me.SendPropertyChanged("ChkDigAlgDesc")
                End If
            End Set
        End Property

        Private _ChkDigAlgActive As Boolean = True
        <DataMember()> _
        Public Property ChkDigAlgActive() As Boolean
            Get
                Return Me._ChkDigAlgActive
            End Get
            Set(value As Boolean)
                If ((Me._ChkDigAlgActive = value) _
                            = False) Then
                    Me._ChkDigAlgActive = value
                    Me.SendPropertyChanged("ChkDigAlgActive")
                End If
            End Set
        End Property

        Private _ChkDigAlgAllowWeightFactor As Boolean = False
        <DataMember()> _
        Public Property ChkDigAlgAllowWeightFactor() As Boolean
            Get
                Return Me._ChkDigAlgAllowWeightFactor
            End Get
            Set(value As Boolean)
                If ((Me._ChkDigAlgAllowWeightFactor = value) _
                            = False) Then
                    Me._ChkDigAlgAllowWeightFactor = value
                    Me.SendPropertyChanged("ChkDigAlgAllowWeightFactor")
                End If
            End Set
        End Property

        Private _ChkDigAlgAllowErrorCode As Boolean = False
        <DataMember()> _
        Public Property ChkDigAlgAllowErrorCode() As Boolean
            Get
                Return Me._ChkDigAlgAllowErrorCode
            End Get
            Set(value As Boolean)
                If ((Me._ChkDigAlgAllowErrorCode = value) _
                            = False) Then
                    Me._ChkDigAlgAllowErrorCode = value
                    Me.SendPropertyChanged("ChkDigAlgAllowErrorCode")
                End If
            End Set
        End Property

        Private _ChkDigAlgAllow10DigitCode As Boolean = False
        <DataMember()> _
        Public Property ChkDigAlgAllow10DigitCode() As Boolean
            Get
                Return Me._ChkDigAlgAllow10DigitCode
            End Get
            Set(value As Boolean)
                If ((Me._ChkDigAlgAllow10DigitCode = value) _
                            = False) Then
                    Me._ChkDigAlgAllow10DigitCode = value
                    Me.SendPropertyChanged("ChkDigAlgAllow10DigitCode")
                End If
            End Set
        End Property

        Private _ChkDigAlgAllowOver10DigitCode As Boolean = False
        <DataMember()> _
        Public Property ChkDigAlgAllowOver10DigitCode() As Boolean
            Get
                Return Me._ChkDigAlgAllowOver10DigitCode
            End Get
            Set(value As Boolean)
                If ((Me._ChkDigAlgAllowOver10DigitCode = value) _
                            = False) Then
                    Me._ChkDigAlgAllowOver10DigitCode = value
                    Me.SendPropertyChanged("ChkDigAlgAllowOver10DigitCode")
                End If
            End Set
        End Property

        Private _ChkDigAlgAllowZeroDigitCode As Boolean = False
        <DataMember()> _
        Public Property ChkDigAlgAllowZeroDigitCode() As Boolean
            Get
                Return Me._ChkDigAlgAllowZeroDigitCode
            End Get
            Set(value As Boolean)
                If ((Me._ChkDigAlgAllowZeroDigitCode = value) _
                            = False) Then
                    Me._ChkDigAlgAllowZeroDigitCode = value
                    Me.SendPropertyChanged("ChkDigAlgAllowZeroDigitCode")
                End If
            End Set
        End Property

        Private _ChkDigAlgAllowWeightFactorDigitSplitting As Boolean = False
        <DataMember()> _
        Public Property ChkDigAlgAllowWeightFactorDigitSplitting() As Boolean
            Get
                Return Me._ChkDigAlgAllowWeightFactorDigitSplitting
            End Get
            Set(value As Boolean)
                If ((Me._ChkDigAlgAllowWeightFactorDigitSplitting = value) _
                            = False) Then
                    Me._ChkDigAlgAllowWeightFactorDigitSplitting = value
                    Me.SendPropertyChanged("ChkDigAlgAllowWeightFactorDigitSplitting")
                End If
            End Set
        End Property

        Private _ChkDigAlgAllowUseIndexForWeightFactor As Boolean = False
        <DataMember()> _
        Public Property ChkDigAlgAllowUseIndexForWeightFactor() As Boolean
            Get
                Return Me._ChkDigAlgAllowUseIndexForWeightFactor
            End Get
            Set(value As Boolean)
                If ((Me._ChkDigAlgAllowUseIndexForWeightFactor = value) _
                            = False) Then
                    Me._ChkDigAlgAllowUseIndexForWeightFactor = value
                    Me.SendPropertyChanged("ChkDigAlgAllowUseIndexForWeightFactor")
                End If
            End Set
        End Property

        Private _ChkDigAlgExp1 As String
        <DataMember()> _
        Public Property ChkDigAlgExp1() As String
            Get
                Return Left(Me._ChkDigAlgExp1, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ChkDigAlgExp1, value) = False) Then
                    Me._ChkDigAlgExp1 = Left(value, 100)
                    Me.SendPropertyChanged("ChkDigAlgExp1")
                End If
            End Set
        End Property

        Private _ChkDigAlgExp2 As String
        <DataMember()> _
        Public Property ChkDigAlgExp2() As String
            Get
                Return Left(Me._ChkDigAlgExp2, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ChkDigAlgExp2, value) = False) Then
                    Me._ChkDigAlgExp2 = Left(value, 100)
                    Me.SendPropertyChanged("ChkDigAlgExp2")
                End If
            End Set
        End Property

        Private _ChkDigAlgExp3 As String
        <DataMember()> _
        Public Property ChkDigAlgExp3() As String
            Get
                Return Left(Me._ChkDigAlgExp3, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ChkDigAlgExp3, value) = False) Then
                    Me._ChkDigAlgExp3 = Left(value, 100)
                    Me.SendPropertyChanged("ChkDigAlgExp3")
                End If
            End Set
        End Property

        Private _ChkDigAlgExp4 As String
        <DataMember()> _
        Public Property ChkDigAlgExp4() As String
            Get
                Return Left(Me._ChkDigAlgExp4, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ChkDigAlgExp4, value) = False) Then
                    Me._ChkDigAlgExp4 = Left(value, 100)
                    Me.SendPropertyChanged("ChkDigAlgExp4")
                End If
            End Set
        End Property

        Private _ChkDigAlgModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ChkDigAlgModDate() As System.Nullable(Of Date)
            Get
                Return Me._ChkDigAlgModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._ChkDigAlgModDate.Equals(value) = False) Then
                    Me._ChkDigAlgModDate = value
                    Me.SendPropertyChanged("ChkDigAlgModDate")
                End If
            End Set
        End Property

        Private _ChkDigAlgModUser As String
        <DataMember()> _
        Public Property ChkDigAlgModUser() As String
            Get
                Return Left(Me._ChkDigAlgModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ChkDigAlgModUser, value) = False) Then
                    Me._ChkDigAlgModUser = Left(value, 100)
                    Me.SendPropertyChanged("ChkDigAlgModUser")
                End If
            End Set
        End Property


        Private _ChkDigAlgUpdated As Byte()
        <DataMember()> _
        Public Property ChkDigAlgUpdated() As Byte()
            Get
                Return Me._ChkDigAlgUpdated
            End Get
            Set(value As Byte())
                Me._ChkDigAlgUpdated = value
            End Set
        End Property

        Private _ChkDigAlgAllowUseSubtractionFactor As Boolean = True
        <DataMember()> _
        Public Property ChkDigAlgAllowUseSubtractionFactor() As Boolean
            Get
                Return Me._ChkDigAlgAllowUseSubtractionFactor
            End Get
            Set(value As Boolean)
                If ((Me._ChkDigAlgAllowUseSubtractionFactor = value) _
                            = False) Then
                    Me._ChkDigAlgAllowUseSubtractionFactor = value
                    Me.SendPropertyChanged("ChkDigAlgAllowUseSubtractionFactor")
                End If
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblCheckDigitAlgorithm
            instance = DirectCast(MemberwiseClone(), tblCheckDigitAlgorithm)
            Return instance
        End Function

#End Region

    End Class
End Namespace