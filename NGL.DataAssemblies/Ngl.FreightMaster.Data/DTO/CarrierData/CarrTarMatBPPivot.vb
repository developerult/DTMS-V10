
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarMatBPPivot
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrTarMatBPControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatBPControl() As Integer
            Get
                Return _CarrTarMatBPControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPControl <> value) Then
                    Me._CarrTarMatBPControl = value
                    Me.SendPropertyChanged("CarrTarMatBPControl")
                End If
            End Set
        End Property

        Private _CarrTarMatBPCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatBPCarrTarControl() As Integer
            Get
                Return _CarrTarMatBPCarrTarControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPCarrTarControl <> value) Then
                    Me._CarrTarMatBPCarrTarControl = value
                    Me.SendPropertyChanged("CarrTarMatBPCarrTarControl")
                End If
            End Set
        End Property

        Private _CarrTarMatBPName As String = ""
        <DataMember()> _
        Public Property CarrTarMatBPName() As String
            Get
                Return Left(_CarrTarMatBPName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMatBPName, value) = False) Then
                    Me._CarrTarMatBPName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarMatBPName")
                End If
            End Set
        End Property

        Private _CarrTarMatBPDesc As String = ""
        <DataMember()> _
        Public Property CarrTarMatBPDesc() As String
            Get
                Return Left(_CarrTarMatBPDesc, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMatBPDesc, value) = False) Then
                    Me._CarrTarMatBPDesc = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarMatBPDesc")
                End If
            End Set
        End Property

        Private _CarrTarMatBPClassTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatBPClassTypeControl() As Integer
            Get
                Return _CarrTarMatBPClassTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPClassTypeControl <> value) Then
                    Me._CarrTarMatBPClassTypeControl = value
                    Me.SendPropertyChanged("CarrTarMatBPClassTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarMatBPTarBracketTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatBPTarBracketTypeControl() As Integer
            Get
                Return _CarrTarMatBPTarBracketTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPTarBracketTypeControl <> value) Then
                    Me._CarrTarMatBPTarBracketTypeControl = value
                    Me.SendPropertyChanged("CarrTarMatBPTarBracketTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarMatBPTarRateTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatBPTarRateTypeControl() As Integer
            Get
                Return _CarrTarMatBPTarRateTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPTarRateTypeControl <> value) Then
                    Me._CarrTarMatBPTarRateTypeControl = value
                    Me.SendPropertyChanged("CarrTarMatBPTarRateTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarMatBPModUser As String = ""
        <DataMember()> _
        Public Property CarrTarMatBPModUser() As String
            Get
                Return Left(_CarrTarMatBPModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMatBPModUser, value) = False) Then
                    Me._CarrTarMatBPModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarMatBPModUser")
                End If
            End Set
        End Property

        Private _CarrTarMatBPModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarMatBPModDate() As System.Nullable(Of Date)
            Get
                Return _CarrTarMatBPModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CarrTarMatBPModDate.Equals(value) = False) Then
                    Me._CarrTarMatBPModDate = value
                    Me.SendPropertyChanged("CarrTarMatBPModDate")
                End If
            End Set
        End Property

        Private _CarrTarMatBPUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarMatBPUpdated() As Byte()
            Get
                Return _CarrTarMatBPUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarMatBPUpdated = value
            End Set
        End Property

        Private _BPVal1 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BPVal1() As System.Nullable(Of Decimal)
            Get
                Return _BPVal1
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BPVal1.Equals(value) = False) Then
                    Me._BPVal1 = value
                    Me.SendPropertyChanged("BPVal1")
                End If
            End Set
        End Property

        Private _BPVal2 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BPVal2() As System.Nullable(Of Decimal)
            Get
                Return _BPVal2
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BPVal2.Equals(value) = False) Then
                    Me._BPVal2 = value
                    Me.SendPropertyChanged("BPVal2")
                End If
            End Set
        End Property

        Private _BPVal3 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BPVal3() As System.Nullable(Of Decimal)
            Get
                Return _BPVal3
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BPVal3.Equals(value) = False) Then
                    Me._BPVal3 = value
                    Me.SendPropertyChanged("BPVal3")
                End If
            End Set
        End Property

        Private _BPVal4 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BPVal4() As System.Nullable(Of Decimal)
            Get
                Return _BPVal4
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BPVal4.Equals(value) = False) Then
                    Me._BPVal4 = value
                    Me.SendPropertyChanged("BPVal4")
                End If
            End Set
        End Property

        Private _BPVal5 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BPVal5() As System.Nullable(Of Decimal)
            Get
                Return _BPVal5
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BPVal5.Equals(value) = False) Then
                    Me._BPVal5 = value
                    Me.SendPropertyChanged("BPVal5")
                End If
            End Set
        End Property

        Private _BPVal6 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BPVal6() As System.Nullable(Of Decimal)
            Get
                Return _BPVal6
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BPVal6.Equals(value) = False) Then
                    Me._BPVal6 = value
                    Me.SendPropertyChanged("BPVal6")
                End If
            End Set
        End Property

        Private _BPVal7 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BPVal7() As System.Nullable(Of Decimal)
            Get
                Return _BPVal7
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BPVal7.Equals(value) = False) Then
                    Me._BPVal7 = value
                    Me.SendPropertyChanged("BPVal7")
                End If
            End Set
        End Property

        Private _BPVal8 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BPVal8() As System.Nullable(Of Decimal)
            Get
                Return _BPVal8
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BPVal8.Equals(value) = False) Then
                    Me._BPVal8 = value
                    Me.SendPropertyChanged("BPVal8")
                End If
            End Set
        End Property

        Private _BPVal9 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BPVal9() As System.Nullable(Of Decimal)
            Get
                Return _BPVal9
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BPVal9.Equals(value) = False) Then
                    Me._BPVal9 = value
                    Me.SendPropertyChanged("BPVal9")
                End If
            End Set
        End Property

        Private _BPVal10 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BPVal10() As System.Nullable(Of Decimal)
            Get
                Return _BPVal10
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BPVal10.Equals(value) = False) Then
                    Me._BPVal10 = value
                    Me.SendPropertyChanged("BPVal10")
                End If
            End Set
        End Property

        Private _BPName1 As String = ""
        <DataMember()> _
        Public Property BPName1() As String
            Get
                Return Left(_BPName1, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPName1, value) = False) Then
                    Me._BPName1 = Left(value, 50)
                    Me.SendPropertyChanged("BPName1")
                End If
            End Set
        End Property

        Private _BPName2 As String = ""
        <DataMember()> _
        Public Property BPName2() As String
            Get
                Return Left(_BPName2, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPName2, value) = False) Then
                    Me._BPName2 = Left(value, 50)
                    Me.SendPropertyChanged("BPName2")
                End If
            End Set
        End Property

        Private _BPName3 As String = ""
        <DataMember()> _
        Public Property BPName3() As String
            Get
                Return Left(_BPName3, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPName3, value) = False) Then
                    Me._BPName3 = Left(value, 50)
                    Me.SendPropertyChanged("BPName3")
                End If
            End Set
        End Property

        Private _BPName4 As String = ""
        <DataMember()> _
        Public Property BPName4() As String
            Get
                Return Left(_BPName4, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPName4, value) = False) Then
                    Me._BPName4 = Left(value, 50)
                    Me.SendPropertyChanged("BPName4")
                End If
            End Set
        End Property

        Private _BPName5 As String = ""
        <DataMember()> _
        Public Property BPName5() As String
            Get
                Return Left(_BPName5, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPName5, value) = False) Then
                    Me._BPName5 = Left(value, 50)
                    Me.SendPropertyChanged("BPName5")
                End If
            End Set
        End Property

        Private _BPName6 As String = ""
        <DataMember()> _
        Public Property BPName6() As String
            Get
                Return Left(_BPName6, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPName6, value) = False) Then
                    Me._BPName6 = Left(value, 50)
                    Me.SendPropertyChanged("BPName6")
                End If
            End Set
        End Property

        Private _BPName7 As String = ""
        <DataMember()> _
        Public Property BPName7() As String
            Get
                Return Left(_BPName7, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPName7, value) = False) Then
                    Me._BPName7 = Left(value, 50)
                    Me.SendPropertyChanged("BPName7")
                End If
            End Set
        End Property

        Private _BPName8 As String = ""
        <DataMember()> _
        Public Property BPName8() As String
            Get
                Return Left(_BPName8, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPName8, value) = False) Then
                    Me._BPName8 = Left(value, 50)
                    Me.SendPropertyChanged("BPName8")
                End If
            End Set
        End Property

        Private _BPName9 As String = ""
        <DataMember()> _
        Public Property BPName9() As String
            Get
                Return Left(_BPName9, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPName9, value) = False) Then
                    Me._BPName9 = Left(value, 50)
                    Me.SendPropertyChanged("BPName9")
                End If
            End Set
        End Property

        Private _BPName10 As String = ""
        <DataMember()> _
        Public Property BPName10() As String
            Get
                Return Left(_BPName10, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPName10, value) = False) Then
                    Me._BPName10 = Left(value, 50)
                    Me.SendPropertyChanged("BPName10")
                End If
            End Set
        End Property

        Private _BPDesc1 As String = ""
        <DataMember()> _
        Public Property BPDesc1() As String
            Get
                Return Left(_BPDesc1, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPDesc1, value) = False) Then
                    Me._BPDesc1 = Left(value, 100)
                    Me.SendPropertyChanged("BPDesc1")
                End If
            End Set
        End Property

        Private _BPDesc2 As String = ""
        <DataMember()> _
        Public Property BPDesc2() As String
            Get
                Return Left(_BPDesc2, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPDesc2, value) = False) Then
                    Me._BPDesc2 = Left(value, 100)
                    Me.SendPropertyChanged("BPDesc2")
                End If
            End Set
        End Property

        Private _BPDesc3 As String = ""
        <DataMember()> _
        Public Property BPDesc3() As String
            Get
                Return Left(_BPDesc3, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPDesc3, value) = False) Then
                    Me._BPDesc3 = Left(value, 100)
                    Me.SendPropertyChanged("BPDesc3")
                End If
            End Set
        End Property

        Private _BPDesc4 As String = ""
        <DataMember()> _
        Public Property BPDesc4() As String
            Get
                Return Left(_BPDesc4, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPDesc4, value) = False) Then
                    Me._BPDesc4 = Left(value, 100)
                    Me.SendPropertyChanged("BPDesc4")
                End If
            End Set
        End Property

        Private _BPDesc5 As String = ""
        <DataMember()> _
        Public Property BPDesc5() As String
            Get
                Return Left(_BPDesc5, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPDesc5, value) = False) Then
                    Me._BPDesc5 = Left(value, 100)
                    Me.SendPropertyChanged("BPDesc5")
                End If
            End Set
        End Property

        Private _BPDesc6 As String = ""
        <DataMember()> _
        Public Property BPDesc6() As String
            Get
                Return Left(_BPDesc6, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPDesc6, value) = False) Then
                    Me._BPDesc6 = Left(value, 100)
                    Me.SendPropertyChanged("BPDesc6")
                End If
            End Set
        End Property

        Private _BPDesc7 As String = ""
        <DataMember()> _
        Public Property BPDesc7() As String
            Get
                Return Left(_BPDesc7, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPDesc7, value) = False) Then
                    Me._BPDesc7 = Left(value, 100)
                    Me.SendPropertyChanged("BPDesc7")
                End If
            End Set
        End Property

        Private _BPDesc8 As String = ""
        <DataMember()> _
        Public Property BPDesc8() As String
            Get
                Return Left(_BPDesc8, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPDesc8, value) = False) Then
                    Me._BPDesc8 = Left(value, 100)
                    Me.SendPropertyChanged("BPDesc8")
                End If
            End Set
        End Property

        Private _BPDesc9 As String = ""
        <DataMember()> _
        Public Property BPDesc9() As String
            Get
                Return Left(_BPDesc9, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPDesc9, value) = False) Then
                    Me._BPDesc9 = Left(value, 100)
                    Me.SendPropertyChanged("BPDesc9")
                End If
            End Set
        End Property

        Private _BPDesc10 As String = ""
        <DataMember()> _
        Public Property BPDesc10() As String
            Get
                Return Left(_BPDesc10, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BPDesc10, value) = False) Then
                    Me._BPDesc10 = Left(value, 100)
                    Me.SendPropertyChanged("BPDesc10")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarMatBP
            instance = DirectCast(MemberwiseClone(), CarrTarMatBP)
            Return instance
        End Function

#End Region

    End Class
End Namespace