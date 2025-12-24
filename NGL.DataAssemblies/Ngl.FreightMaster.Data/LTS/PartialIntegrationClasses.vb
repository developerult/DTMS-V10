Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Data.Linq
Imports System.Data.Linq.Mapping
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Reflection

Partial Public Class NGLMASIntegrationDataContext
    Inherits System.Data.Linq.DataContext

    Public Sub New(ByVal connection As String, ByVal intLngTimeOut As Integer)
        MyBase.New(connection, mappingSource)
        Me.LongTimeOut = intLngTimeOut
        OnCreated()
    End Sub

    Private _LongTimeOut As Integer
    Public Property LongTimeOut() As Integer
        Get
            Return _LongTimeOut
        End Get
        Set(ByVal value As Integer)
            _LongTimeOut = value
        End Set
    End Property


    Private Sub OnCreated()
        If LongTimeOut > 0 Then
            Me.CommandTimeout = LongTimeOut
        End If
    End Sub

End Class
