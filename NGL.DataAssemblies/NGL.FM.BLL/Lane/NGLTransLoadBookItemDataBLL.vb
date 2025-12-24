Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports TAR = Ngl.FM.CarTar

Public Class NGLTransLoadBookItemDataBLL
    Implements ICloneable


#Region " Properties"

#Region " Primary Key Fiels"

    Private _BookItem As DTO.BookItem
    Public Property BookItem() As DTO.BookItem
        Get
            Return _BookItem
        End Get
        Set(ByVal value As DTO.BookItem)
            _BookItem = value
        End Set
    End Property


#End Region


#Region " Data Members"

    Private _BookItemQtyOrdered As Integer = 0
    Public Property BookItemQtyOrdered() As Integer
        Get
            Return _BookItemQtyOrdered
        End Get
        Set(ByVal value As Integer)
            If _BookItemQtyOrdered <> value Then
                _WgtPerCase = 0
                _PltsPerCase = 0
                _CubesPerCase = 0
                _BookItemQtyOrdered = value
            End If
        End Set
    End Property

    Private _BookItemWeight As Double = 0
    Public Property BookItemWeight() As Double
        Get
            Return _BookItemWeight
        End Get
        Set(ByVal value As Double)
            If _BookItemWeight <> value Then
                _WgtPerCase = 0
                _BookItemWeight = value
            End If
        End Set
    End Property

    Private _BookItemCube As Integer = 0
    Public Property BookItemCube() As Integer
        Get
            Return _BookItemCube
        End Get
        Set(ByVal value As Integer)
            If _BookItemCube <> value Then
                _CubesPerCase = 0
                _BookItemCube = value
            End If
        End Set
    End Property


    Private _BookItemPallets As Double = 0
    Public Property BookItemPallets() As Double
        Get
            Return _BookItemPallets
        End Get
        Set(ByVal value As Double)
            If _BookItemPallets <> value Then
                _PltsPerCase = 0
                _BookItemPallets = value
            End If
        End Set
    End Property

#End Region

#End Region



#Region " Aggregates"

    Private _WgtPerCase As Double
    ''' <summary>
    ''' if the weight or cases change the object must reset _WgtPerCase to zero
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property WgtPerCase() As Double
        Get
            If _WgtPerCase = 0 Then _WgtPerCase = If(Me.BookItemWeight > 0 And Me.BookItemQtyOrdered > 0, Me.BookItemWeight / Me.BookItemQtyOrdered, 0)
            Return _WgtPerCase
        End Get
    End Property

    Private _PltsPerCase As Double
    ''' <summary>
    ''' if the pallets or cases change the object must reset _PltsPerCase to zero
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PltsPerCase() As Double
        Get
            If _PltsPerCase = 0 Then _PltsPerCase = If(Me.BookItemPallets > 0 And Me.BookItemQtyOrdered > 0, Me.BookItemPallets / Me.BookItemQtyOrdered, 0)
            Return _PltsPerCase
        End Get
    End Property

    Private _CubesPerCase As Double
    ''' <summary>
    ''' if the cubes or cases change the object must reset _PltsPerCase to zero
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CubesPerCase() As Double
        Get
            If _CubesPerCase = 0 Then _CubesPerCase = If(Me.BookItemCube > 0 And Me.BookItemQtyOrdered > 0, Me.BookItemCube / Me.BookItemQtyOrdered, 0)
            Return _CubesPerCase
        End Get
    End Property


#End Region

#Region "Public Methods"

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim instance As New NGLTransLoadBookItemDataBLL
        instance = DirectCast(MemberwiseClone(), NGLTransLoadBookItemDataBLL)
        instance.BookItem = Nothing
        instance.BookItem = Me.BookItem.Clone()
        Return instance
    End Function


#End Region

End Class
