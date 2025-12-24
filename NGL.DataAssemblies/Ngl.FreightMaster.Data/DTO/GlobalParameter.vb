Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization

Namespace DataTransferObjects

    <DataContract()> _
    Public Class GlobalParameter : Implements ICloneable


#Region " Implements ICloneable"
        Private Function ICloneable_Clone() As Object Implements ICloneable.Clone
            Return Clone()
        End Function
#End Region

#Region " Data Members"
        Private _ParKey As String = ""
        <DataMember()> _
        Public Property ParKey() As String
            Get
                Return _ParKey
            End Get
            Set(ByVal value As String)
                _ParKey = value
            End Set
        End Property

        Private _ParValue As Double = 0
        <DataMember()> _
        Public Property ParValue() As Double
            Get

                Return _ParValue
            End Get
            Set(ByVal value As Double)
                _ParValue = value
            End Set
        End Property

        Private _ParText As String = ""
        <DataMember()> _
        Public Property ParText() As String
            Get
                Return _ParText
            End Get
            Set(ByVal value As String)
                _ParText = value
            End Set
        End Property

        Private _ParDescription As String = ""
        <DataMember()> _
        Public Property ParDescription() As String
            Get
                Return _ParDescription
            End Get
            Set(ByVal value As String)
                _ParDescription = value
            End Set
        End Property

        Private _ParOLE As Byte()
        <DataMember()> _
        Public Property ParOLE() As Byte()
            Get
                Return _ParOLE
            End Get
            Set(ByVal value As Byte())
                _ParOLE = value
            End Set
        End Property

        Private _upsize_ts As Byte()
        <DataMember()> _
        Public Property upsize_ts() As Byte()
            Get
                Return _upsize_ts
            End Get
            Set(ByVal value As Byte())
                _upsize_ts = value
            End Set
        End Property

        Private _ParCategoryControl As Integer = 0
        <DataMember()> _
        Public Property ParCategoryControl() As Integer
            Get
                Return _ParCategoryControl
            End Get
            Set(ByVal value As Integer)
                _ParCategoryControl = value
            End Set
        End Property

        Private _ParIsGlobal As Boolean = True
        <DataMember()> _
        Public Property ParIsGlobal() As Boolean
            Get
                Return _ParIsGlobal
            End Get
            Set(ByVal value As Boolean)
                _ParIsGlobal = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Function Clone() As GlobalParameter
            Dim instance As New GlobalParameter
            instance = DirectCast(MemberwiseClone(), GlobalParameter)
            Return instance
        End Function

#End Region
    End Class
End Namespace
