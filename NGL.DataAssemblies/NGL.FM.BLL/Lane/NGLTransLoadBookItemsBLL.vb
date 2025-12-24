Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports LTS = NGL.FreightMaster.Data.LTS
Imports DTran = NGL.Core.Utility.DataTransformation
Imports TAR = NGL.FM.CarTar

Public Class NGLTransLoadBookItemsBLL
    Implements ICloneable



#Region " Aggregates"

    Public ReadOnly Property TotalCases() As Double
        Get
            If Me.BookItems Is Nothing OrElse Me.BookItems.Count < 1 Then
                Return 0
            Else
                Return BookItems.Sum(Function(x) x.BookItemQtyOrdered)
            End If
        End Get
    End Property

    Public ReadOnly Property TotalWgt() As Double
        Get
            If Me.BookItems Is Nothing OrElse Me.BookItems.Count < 1 Then
                Return 0
            Else
                Return BookItems.Sum(Function(x) x.BookItemWeight)
            End If
        End Get
    End Property

    Public ReadOnly Property TotalCubes() As Double
        Get
            If Me.BookItems Is Nothing OrElse Me.BookItems.Count < 1 Then
                Return 0
            Else
                Return BookItems.Sum(Function(x) x.BookItemCube)
            End If
        End Get
    End Property

    Public ReadOnly Property TotalPlts() As Double
        Get
            If Me.BookItems Is Nothing OrElse Me.BookItems.Count < 1 Then
                Return 0
            Else
                Return BookItems.Sum(Function(x) x.BookItemPallets)
            End If
        End Get
    End Property

    Private _BookItems As List(Of NGLTransLoadBookItemDataBLL)
    Public Property BookItems() As List(Of NGLTransLoadBookItemDataBLL)
        Get
            If _BookItems Is Nothing Then _BookItems = New List(Of NGLTransLoadBookItemDataBLL)
            Return _BookItems
        End Get
        Set(ByVal value As List(Of NGLTransLoadBookItemDataBLL))
            _BookItems = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Sub populateItems(ByRef oBookItems As List(Of DTO.BookItem))
        For Each i In oBookItems
            BookItems.Add(New NGLTransLoadBookItemDataBLL With {.BookItem = i, .BookItemCube = i.BookItemCube, .BookItemPallets = i.BookItemPallets, .BookItemQtyOrdered = i.BookItemQtyOrdered, .BookItemWeight = i.BookItemWeight})
        Next
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim instance As New NGLTransLoadBookItemsBLL
        instance = DirectCast(MemberwiseClone(), NGLTransLoadBookItemsBLL)
        instance.BookItems = Nothing
        For Each item In BookItems
            instance.BookItems.Add(DirectCast(item.Clone, NGLTransLoadBookItemDataBLL))
        Next
        Return instance
    End Function


#End Region
End Class
