Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class vARMassEntry
        Inherits DTOBaseClass


#Region " Data Members"
        Private _BookControl As Integer
        <DataMember()> _
        Public Property BookControl() As Integer
            Get
                Return Me._BookControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._BookControl = value) _
                   = False) Then
                    Me._BookControl = value
                End If
            End Set
        End Property

        Private _Invoiced As System.Nullable(Of Date)

        <DataMember()> _
        Public Property Invoiced() As System.Nullable(Of Date)
            Get
                Return Me._Invoiced
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._Invoiced.Equals(value) = False) Then
                    Me._Invoiced = value
                End If
            End Set
        End Property

        Private _Pro_Number As String

        <DataMember()> _
        Public Property Pro_Number() As String
            Get
                Return Me._Pro_Number
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Pro_Number, value) = False) Then
                    Me._Pro_Number = value
                End If
            End Set
        End Property

        Private _Cons_No As String

        <DataMember()> _
        Public Property Cons_No() As String
            Get
                Return Me._Cons_No
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Cons_No, value) = False) Then
                    Me._Cons_No = value
                End If
            End Set
        End Property

        Private _BookPayCode As String

        <DataMember()> _
        Public Property BookPayCode() As String
            Get
                Return Me._BookPayCode
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookPayCode, value) = False) Then
                    Me._BookPayCode = value
                End If
            End Set
        End Property

        Private _BookTypeCode As String

        <DataMember()> _
        Public Property BookTypeCode() As String
            Get
                Return Me._BookTypeCode
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookTypeCode, value) = False) Then
                    Me._BookTypeCode = value
                End If
            End Set
        End Property

        Private _Invoice_Amt As System.Nullable(Of Decimal)

        <DataMember()> _
        Public Property Invoice_Amt() As System.Nullable(Of Decimal)
            Get
                Return Me._Invoice_Amt
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Invoice_Amt.Equals(value) = False) Then
                    Me._Invoice_Amt = value
                End If
            End Set
        End Property

        Private _BookFinARGLNumber As String

        <DataMember()> _
        Public Property BookFinARGLNumber() As String
            Get
                Return Me._BookFinARGLNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookFinARGLNumber, value) = False) Then
                    Me._BookFinARGLNumber = value
                End If
            End Set
        End Property

        Private _Payed As System.Nullable(Of Date)

        <DataMember()> _
        Public Property Payed() As System.Nullable(Of Date)
            Get
                Return Me._Payed
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._Payed.Equals(value) = False) Then
                    Me._Payed = value
                End If
            End Set
        End Property

        Private _Pay_Amt As System.Nullable(Of Decimal)

        <DataMember()> _
        Public Property Pay_Amt() As System.Nullable(Of Decimal)
            Get
                Return Me._Pay_Amt
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Pay_Amt.Equals(value) = False) Then
                    Me._Pay_Amt = value
                End If
            End Set
        End Property

        Private _Check_No As String

        <DataMember()> _
        Public Property Check_No() As String
            Get
                Return Me._Check_No
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Check_No, value) = False) Then
                    Me._Check_No = value
                End If
            End Set
        End Property

        Private _Balance As System.Nullable(Of Decimal)

        <DataMember()> _
        Public Property Balance() As System.Nullable(Of Decimal)
            Get
                Return Me._Balance
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Balance.Equals(value) = False) Then
                    Me._Balance = value
                End If
            End Set
        End Property

        Private _BookCustCompControl As Integer

        <DataMember()> _
        Public Property BookCustCompControl() As Integer
            Get
                Return Me._BookCustCompControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._BookCustCompControl = value) _
                   = False) Then
                    Me._BookCustCompControl = value
                End If
            End Set
        End Property

        Private _CurBal As System.Nullable(Of Decimal)

        <DataMember()> _
        Public Property CurBal() As System.Nullable(Of Decimal)
            Get
                Return Me._CurBal
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._CurBal.Equals(value) = False) Then
                    Me._CurBal = value
                End If
            End Set
        End Property

        Private _DaysOut As System.Nullable(Of Integer)

        <DataMember()> _
        Public Property DaysOut() As System.Nullable(Of Integer)
            Get
                Return Me._DaysOut
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._DaysOut.Equals(value) = False) Then
                    Me._DaysOut = value
                End If
            End Set
        End Property

        Private _BookUpdated As Byte()
        <DataMember()> _
        Public Property BookUpdated() As Byte()
            Get
                Return _BookUpdated
            End Get
            Set(ByVal value As Byte())
                _BookUpdated = value
            End Set
        End Property



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New vARMassEntry
            instance = DirectCast(MemberwiseClone(), vARMassEntry)
            Return instance
        End Function

#End Region

    End Class

End Namespace