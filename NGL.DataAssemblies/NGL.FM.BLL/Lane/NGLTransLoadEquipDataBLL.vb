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

Public Class NGLTransLoadEquipDataBLL
    Implements ICloneable



#Region " Aggregates"

    Public ReadOnly Property TotalCases() As Double
        Get
            Return Me.BookItemsBLL.TotalCases
        End Get
    End Property


    Public ReadOnly Property TotalWgt() As Double
        Get
            Return Me.BookItemsBLL.TotalWgt
        End Get
    End Property


    Public ReadOnly Property TotalCubes() As Double
        Get
            Return Me.BookItemsBLL.TotalCubes
        End Get
    End Property

    Public ReadOnly Property TotalPlts() As Double
        Get
            Return Me.BookItemsBLL.TotalPlts
        End Get
    End Property


    Public ReadOnly Property AvailCases() As Double
        Get
            Return Me.CarrTarEquipCasesMaximum - Me.BookItemsBLL.TotalCases
        End Get
    End Property


    Public ReadOnly Property AvailWgt() As Double
        Get
            Return Me.CarrTarEquipWgtMaximum - Me.BookItemsBLL.TotalWgt
        End Get
    End Property


    Public ReadOnly Property AvailCubes() As Double
        Get
            Return Me.CarrTarEquipCubesMaximum - Me.BookItemsBLL.TotalCubes
        End Get
    End Property

    Public ReadOnly Property AvailPlts() As Double
        Get
            Return Me.CarrTarEquipPltsMaximum - Me.BookItemsBLL.TotalPlts
        End Get
    End Property

#End Region

#Region " Properties"

    Private _CarrTarEquipControl As Integer = 0
    Public Property CarrTarEquipControl() As Integer
        Get
            Return _CarrTarEquipControl
        End Get
        Set(value As Integer)
            If (Me._CarrTarEquipControl <> value) Then
                Me._CarrTarEquipControl = value
            End If
        End Set
    End Property


    Private _CarrTarEquipCasesMinimum As Double = 0

    Public Property CarrTarEquipCasesMinimum() As Double
        Get
            Return Me._CarrTarEquipCasesMinimum
        End Get
        Set(value As Double)
            If (Me._CarrTarEquipCasesMinimum <> value) Then
                Me._CarrTarEquipCasesMinimum = value
            End If
        End Set
    End Property

    Private _CarrTarEquipCasesConsiderFull As Double = 0

    Public Property CarrTarEquipCasesConsiderFull() As Double
        Get
            Return Me._CarrTarEquipCasesConsiderFull
        End Get
        Set(value As Double)
            If (Me._CarrTarEquipCasesConsiderFull <> value) Then
                Me._CarrTarEquipCasesConsiderFull = value
            End If
        End Set
    End Property

    Private _CarrTarEquipCasesMaximum As Double = 0

    Public Property CarrTarEquipCasesMaximum() As Double
        Get
            Return Me._CarrTarEquipCasesMaximum
        End Get
        Set(value As Double)
            If (Me._CarrTarEquipCasesMaximum <> value) Then
                Me._CarrTarEquipCasesMaximum = value
            End If
        End Set
    End Property

    Private _CarrTarEquipWgtMinimum As Double = 0

    Public Property CarrTarEquipWgtMinimum() As Double
        Get
            Return Me._CarrTarEquipWgtMinimum
        End Get
        Set(value As Double)
            If (Me._CarrTarEquipWgtMinimum <> value) Then
                Me._CarrTarEquipWgtMinimum = value
            End If
        End Set
    End Property

    Private _CarrTarEquipWgtConsiderFull As Double = 0

    Public Property CarrTarEquipWgtConsiderFull() As Double
        Get
            Return Me._CarrTarEquipWgtConsiderFull
        End Get
        Set(value As Double)
            If (Me._CarrTarEquipWgtConsiderFull <> value) Then
                Me._CarrTarEquipWgtConsiderFull = value
            End If
        End Set
    End Property

    Private _CarrTarEquipWgtMaximum As Double = 0

    Public Property CarrTarEquipWgtMaximum() As Double
        Get
            Return Me._CarrTarEquipWgtMaximum
        End Get
        Set(value As Double)
            If (Me._CarrTarEquipWgtMaximum <> value) Then
                Me._CarrTarEquipWgtMaximum = value
            End If
        End Set
    End Property

    Private _CarrTarEquipCubesMinimum As Double = 0

    Public Property CarrTarEquipCubesMinimum() As Double
        Get
            Return Me._CarrTarEquipCubesMinimum
        End Get
        Set(value As Double)
            If (Me._CarrTarEquipCubesMinimum <> value) Then
                Me._CarrTarEquipCubesMinimum = value
            End If
        End Set
    End Property

    Private _CarrTarEquipCubesConsiderFull As Double = 0

    Public Property CarrTarEquipCubesConsiderFull() As Double
        Get
            Return Me._CarrTarEquipCubesConsiderFull
        End Get
        Set(value As Double)
            If (Me._CarrTarEquipCubesConsiderFull <> value) Then
                Me._CarrTarEquipCubesConsiderFull = value
            End If
        End Set
    End Property

    Private _CarrTarEquipCubesMaximum As Double = 0

    Public Property CarrTarEquipCubesMaximum() As Double
        Get
            Return Me._CarrTarEquipCubesMaximum
        End Get
        Set(value As Double)
            If (Me._CarrTarEquipCubesMaximum <> value) Then
                Me._CarrTarEquipCubesMaximum = value
            End If
        End Set
    End Property

    Private _CarrTarEquipPltsMinimum As Integer

    Public Property CarrTarEquipPltsMinimum() As Integer
        Get
            Return Me._CarrTarEquipPltsMinimum
        End Get
        Set(value As Integer)
            If (Me._CarrTarEquipPltsMinimum <> value) Then
                Me._CarrTarEquipPltsMinimum = value
            End If
        End Set
    End Property

    Private _CarrTarEquipPltsConsiderFull As Integer

    Public Property CarrTarEquipPltsConsiderFull() As Integer
        Get
            Return Me._CarrTarEquipPltsConsiderFull
        End Get
        Set(value As Integer)
            If (Me._CarrTarEquipPltsConsiderFull <> value) Then
                Me._CarrTarEquipPltsConsiderFull = value
            End If
        End Set
    End Property

    Private _CarrTarEquipPltsMaximum As Integer

    Public Property CarrTarEquipPltsMaximum() As Integer
        Get
            Return Me._CarrTarEquipPltsMaximum
        End Get
        Set(value As Integer)
            If (Me._CarrTarEquipPltsMaximum <> value) Then
                Me._CarrTarEquipPltsMaximum = value
            End If
        End Set
    End Property

    Private _CarrTarEquipTempType As Integer

    Public Property CarrTarEquipTempType() As Integer
        Get
            Return Me._CarrTarEquipTempType
        End Get
        Set(value As Integer)
            If (Me._CarrTarEquipTempType <> value) Then
                Me._CarrTarEquipTempType = value
            End If
        End Set
    End Property

    Private _TransLoadXrefDet As DTO.LaneTransLoadXrefDet
    Public Property TransLoadXrefDet() As DTO.LaneTransLoadXrefDet
        Get
            If _TransLoadXrefDet Is Nothing Then TransLoadXrefDet = New DTO.LaneTransLoadXrefDet
            Return _TransLoadXrefDet
        End Get
        Set(ByVal value As DTO.LaneTransLoadXrefDet)
            _TransLoadXrefDet = value
        End Set
    End Property

    Private _MovementSplitSequenceNumber As Integer = 0
    Public Property MovementSplitSequenceNumber() As Integer
        Get
            Return _MovementSplitSequenceNumber
        End Get
        Set(ByVal value As Integer)
            _MovementSplitSequenceNumber = value
        End Set
    End Property

    Private _BookConsPrefix As String = ""
    Public Property BookConsPrefix() As String
        Get
            Return Left(_BookConsPrefix, 20)
        End Get
        Set(ByVal value As String)
            _BookConsPrefix = Left(value, 20)
        End Set
    End Property

    Private _BookRouteConsFlag As Boolean = False
    Public Property BookRouteConsFlag() As Boolean
        Get
            Return _BookRouteConsFlag
        End Get
        Set(ByVal value As Boolean)
            _BookRouteConsFlag = value
        End Set
    End Property

    Private _BookStopNo As Int16
    Public Property BookStopNo() As Int16
        Get
            Return _BookStopNo
        End Get
        Set(ByVal value As Int16)
            _BookStopNo = value
        End Set
    End Property

    Private _BookStemMiles As Double
    Public Property BookStemMiles() As Double
        Get
            Return _BookStemMiles
        End Get
        Set(ByVal value As Double)
            _BookStemMiles = value
        End Set
    End Property



    Private _BookItemsBLL As NGLTransLoadBookItemsBLL
    Public Property BookItemsBLL() As NGLTransLoadBookItemsBLL
        Get
            If _BookItemsBLL Is Nothing Then _BookItemsBLL = New NGLTransLoadBookItemsBLL
            Return _BookItemsBLL
        End Get
        Set(ByVal value As NGLTransLoadBookItemsBLL)
            _BookItemsBLL = value
        End Set
    End Property

#End Region

#Region "Public Methods"


    Public Function addItem(ByRef oItem As NGLTransLoadBookItemDataBLL) As Boolean
        Dim blnRet As Boolean = False
        If CanFit(oItem.BookItemQtyOrdered, oItem.BookItemWeight, oItem.BookItemPallets, oItem.BookItemCube) Then
            Me.BookItemsBLL.BookItems.Add(oItem)
            blnRet = True
        End If
        Return blnRet
    End Function

    ''' <summary>
    ''' Attempts to add as many cases (Qty) of the selected item to the equipment as possible.
    ''' returns any cases (Qty) that will not fit.
    ''' </summary>
    ''' <param name="oItem"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' NOTE:  the WgtPerCase factor may need to check parameter settings for how weight is provided to the item
    ''' in some cases the ERP system may provide the weight per case instead of the total weight of the item.
    ''' this formula expectes the total weight of the item ordered.
    ''' </remarks>
    Public Function addWithSplit(ByVal oItem As NGLTransLoadBookItemDataBLL) As NGLTransLoadBookItemDataBLL
        Dim oRet As NGLTransLoadBookItemDataBLL = oItem.Clone()
        oRet.BookItemQtyOrdered = 0
        oRet.BookItemWeight = 0
        oRet.BookItemPallets = 0
        oRet.BookItemCube = 0
        'if we can add the item it fits so just return an empty item with no qty
        If Me.addItem(oItem) Then Return oRet
        Dim casesToMove As Integer = Math.Truncate(Me.AvailWgt / oItem.WgtPerCase)
        If casesToMove < 1 Then casesToMove = 1
        'this process improves performance when we have a large number of cases. like 500,000 units of 1 lbs each
        'we use the maximum available weight on the equipment in an attempt to estimate the number of case that can
        'be moved onto this piece of equipment.
        If CanFit(casesToMove, oItem.WgtPerCase * casesToMove, oItem.PltsPerCase * casesToMove, oItem.CubesPerCase * casesToMove) Then
            casesToMove += 1
            'add cases one at a time to maximize capacity
            Do While CanFit(casesToMove, oItem.WgtPerCase * casesToMove, oItem.PltsPerCase * casesToMove, oItem.CubesPerCase * casesToMove)
                casesToMove += 1
                If casesToMove > oItem.BookItemQtyOrdered Then Exit Do
            Loop
            'jump back to the previous case count because the last loop always fails
            casesToMove -= 1
        ElseIf casesToMove > 1 Then
            'step backward becasue the last one always fails
            casesToMove -= 1
            Do Until CanFit(casesToMove, oItem.WgtPerCase * casesToMove, oItem.PltsPerCase * casesToMove, oItem.CubesPerCase * casesToMove)
                casesToMove -= 1
                If casesToMove < 1 Then Exit Do
            Loop
        End If

        If casesToMove > 0 Then
            Dim oNewItem As NGLTransLoadBookItemDataBLL = oItem.Clone()
            'update the item details and save the changes
            With oNewItem
                .BookItemQtyOrdered = casesToMove
                .BookItemWeight = oItem.WgtPerCase * casesToMove
                .BookItemPallets = oItem.PltsPerCase * casesToMove
                .BookItemCube = oItem.CubesPerCase * casesToMove
            End With
            If Me.addItem(oNewItem) Then
                oRet.BookItemQtyOrdered = oItem.BookItemQtyOrdered - oNewItem.BookItemQtyOrdered
                oRet.BookItemWeight = oItem.BookItemWeight - oNewItem.BookItemWeight
                oRet.BookItemPallets = oItem.BookItemPallets - oNewItem.BookItemPallets
                oRet.BookItemCube = oItem.BookItemCube - oNewItem.BookItemCube
            End If
        End If
        Return oRet
    End Function

    Public Function CanFit(ByVal cases As Double, ByVal wgt As Double, ByVal plts As Double, ByVal cubes As Double) As Boolean
        Dim blnRet As Boolean = False
        blnRet = (CarrTarEquipCasesMaximum = 0 OrElse (cases + TotalCases) <= CarrTarEquipCasesMaximum) _
            AndAlso _
            (CarrTarEquipWgtMaximum = 0 OrElse (wgt + TotalWgt) <= CarrTarEquipWgtMaximum) _
            AndAlso _
            (CarrTarEquipPltsMaximum = 0 OrElse (plts + TotalPlts) <= CarrTarEquipPltsMaximum) _
            AndAlso _
            (CarrTarEquipCubesMaximum = 0 OrElse (cubes + TotalCubes) <= CarrTarEquipCubesMaximum)
        Return blnRet
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return Clone(True)
    End Function

    Public Function Clone(ByVal blnNoItems As Boolean) As NGLTransLoadEquipDataBLL
        Dim instance As New NGLTransLoadEquipDataBLL
        instance = DirectCast(MemberwiseClone(), NGLTransLoadEquipDataBLL)
        If blnNoItems Then
            instance.BookItemsBLL = Nothing
        Else
            instance.BookItemsBLL = BookItemsBLL.Clone()
        End If
        Return instance
    End Function


#End Region
End Class
