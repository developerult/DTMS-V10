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

Public Class NGLBookFeesBLL : Inherits BLLBaseClass


#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLBookFeesBLL"
    End Sub

#End Region

#Region " Properties "

#End Region

#Region "DAL Wrapper Methods"

    Public Function GetBookFees() As DTO.BookFee()
        Return NGLBookFeeData.GetBookFeesFiltered()
    End Function

    Public Function GetBookFeesByBook(ByVal BookControl As Integer) As DTO.BookFee()
        Return NGLBookFeeData.GetBookFeesFiltered(BookControl)
    End Function

    Public Function GetBookFeesFilteredByAccessorialCode(ByVal BookControl As Integer, ByVal AccessoraialCode As Integer) As DTO.BookFee()
        Return NGLBookFeeData.GetBookFeesFilteredByAccessorialCode(BookControl, AccessoraialCode)
    End Function

    Public Function GetBookFeesForTariff(ByVal BookControl As Integer) As DTO.BookFee()
        Return NGLBookFeeData.GetBookFeesFiltered(BookControl, DAL.Utilities.AccessorialFeeType.Tariff)
    End Function

    Public Function GetBookFeesForLane(ByVal BookControl As Integer) As DTO.BookFee()
        Return NGLBookFeeData.GetBookFeesFiltered(BookControl, DAL.Utilities.AccessorialFeeType.Lane)
    End Function

    Public Function GetBookFeesForOrder(ByVal BookControl As Integer) As DTO.BookFee()
        Return NGLBookFeeData.GetBookFeesFiltered(BookControl, DAL.Utilities.AccessorialFeeType.Order)
    End Function

    Public Function GetBookFee(ByVal Control As Integer) As DTO.BookFee
        Return NGLBookFeeData.GetBookFeeFiltered(Control)
    End Function

    Public Function CreateBookFee(ByVal oData As DTO.BookFee) As DTO.BookFee
        Dim oRet As DTO.BookFee = NGLBookFeeData.CreateRecord(oData)
        BookRevenueBLL.AssignCarrier(oRet.BookFeesBookControl, True)
        Return oRet
    End Function

    Public Function CreateBookFees(ByVal oData As DTO.BookFee()) As Boolean
        For Each item In oData
            NGLBookFeeData.CreateRecord(item)
        Next
        If oData.Count > 0 Then
            BookRevenueBLL.AssignCarrier(oData(0).BookFeesBookControl, True)
        End If
        Return True
    End Function

    Public Function CreateBookFeesD365(ByVal oData As DTO.BookFee()) As DTO.CarrierCostResults
        Dim oRet As New DTO.CarrierCostResults()
        oRet.Success = True
        For Each dtoitem As DTO.BookFee In oData
            Dim iAccCode = dtoitem.BookFeesAccessorialCode
            Dim oAccessorial As DTO.tblAccessorial = NGLtblAccessorialData.GetRecordFiltered(iAccCode)
            dtoitem.BookFeesAccessorialFeeAllocationTypeControl = oAccessorial.AccessorialAccessorialFeeAllocationTypeControl
            dtoitem.BookFeesAccessorialFeeCalcTypeControl = oAccessorial.AccessorialAccessorialFeeCalcTypeControl
            dtoitem.BookFeesAccessorialProfileSpecific = oAccessorial.AccessorialProfileSpecific
            dtoitem.BookFeesAllowCarrierUpdates = oAccessorial.AccessorialAllowCarrierUpdates
            dtoitem.BookFeesAutoApprove = oAccessorial.AccessorialAutoApprove
            dtoitem.BookFeesBOLPlacement = oAccessorial.AccessorialBOLPlacement
            dtoitem.BookFeesBOLText = oAccessorial.AccessorialBOLText
            dtoitem.BookFeesCaption = oAccessorial.AccessorialCaption
            dtoitem.BookFeesEDICode = oAccessorial.AccessorialEDICode
            dtoitem.BookFeesIsTax = oAccessorial.AccessorialIsTax
            dtoitem.BookFeesTarBracketTypeControl = oAccessorial.AccessorialTarBracketTypeControl
            dtoitem.BookFeesTaxable = oAccessorial.AccessorialTaxable
            dtoitem.BookFeesTaxSortOrder = oAccessorial.AccessorialTaxSortOrder
            If dtoitem.BookFeesVariable = 0 Then dtoitem.BookFeesVariable = oAccessorial.AccessorialVariable
            dtoitem.BookFeesVariableCode = oAccessorial.AccessorialVariableCode
            dtoitem.BookFeesVisible = oAccessorial.AccessorialVisible
            NGLBookFeeData.CreateRecord(dtoitem)
        Next
        If oData.Count > 0 Then
            oRet = BookRevenueBLL.AssignCarrier(oData(0).BookFeesBookControl, True)
        End If
        Return oRet
    End Function

    Public Sub DeleteBookFee(ByVal oData As DTO.BookFee)
        NGLBookFeeData.DeleteRecord(oData)
        BookRevenueBLL.AssignCarrier(oData.BookFeesBookControl, True)
    End Sub

    Public Sub DeleteBookFee(ByVal iBookFeeControl As Integer)
        Dim oData = GetBookFee(iBookFeeControl)
        NGLBookFeeData.DeleteRecord(oData)
        BookRevenueBLL.AssignCarrier(oData.BookFeesBookControl, True)
    End Sub

    Public Function UpdateBookFee(ByVal oData As DTO.BookFee) As DTO.BookFee
        Dim oRet As DTO.BookFee = NGLBookFeeData.UpdateRecord(oData)
        BookRevenueBLL.AssignCarrier(oRet.BookFeesBookControl, True)
        Return oRet
    End Function

    Public Function UpdateBookFeeQuick(ByVal oData As DTO.BookFee) As DTO.QuickSaveResults
        Dim oRet As DTO.QuickSaveResults = NGLBookFeeData.UpdateRecordQuick(oData)
        BookRevenueBLL.AssignCarrier(oData.BookFeesBookControl, True)
        Return oRet
    End Function

    Public Sub UpdateBookFeeNoReturn(ByVal oData As DTO.BookFee)
        NGLBookFeeData.UpdateRecordNoReturn(oData)
        BookRevenueBLL.AssignCarrier(oData.BookFeesBookControl, True)
    End Sub

    Public Function UpdateBookFeeD365(ByVal oDTOFee As DTO.BookFee) As DTO.CarrierCostResults
        Dim oRet As New DTO.CarrierCostResults()
        oRet.Success = False

        NGLBookFeeData.UpdateRecordNoReturn(oDTOFee)
        oRet = BookRevenueBLL.AssignCarrier(oDTOFee.BookFeesBookControl, True)


        Return oRet
    End Function

    Public Function GetBookFeePendings() As DTO.BookFeePending()
        Return NGLBookFeePendingData.GetBookFeePendingsFiltered()
    End Function

    Public Function GetBookFeePendingsByBook(ByVal BookControl As Integer) As DTO.BookFeePending()
        Return NGLBookFeePendingData.GetBookFeePendingsFiltered(BookControl)
    End Function

    Public Function GetBookFeePending(ByVal Control As Integer) As DTO.BookFeePending
        Return NGLBookFeePendingData.GetBookFeePendingFiltered(Control)
    End Function

    Public Function CreateBookFeePending(ByVal oData As DTO.BookFeePending) As DTO.BookFeePending
        Dim oRet As DTO.BookFeePending = NGLBookFeePendingData.CreateRecord(oData)
        Return oRet
    End Function

    Public Sub DeleteBookFeePending(ByVal oData As DTO.BookFeePending)
        NGLBookFeePendingData.DeleteRecord(oData)
    End Sub

    Public Function UpdateBookFeePending(ByVal oData As DTO.BookFeePending) As DTO.BookFeePending
        Dim oRet As DTO.BookFeePending = NGLBookFeePendingData.UpdateRecord(oData)
        Return oRet
    End Function

    Public Function UpdateBookFeePendingQuick(ByVal oData As DTO.BookFeePending) As DTO.QuickSaveResults
        Dim oRet As DTO.QuickSaveResults = NGLBookFeePendingData.UpdateRecordQuick(oData)
        Return oRet
    End Function

    Public Sub UpdateBookFeePendingNoReturn(ByVal oData As DTO.BookFeePending)
        NGLBookFeePendingData.UpdateRecordNoReturn(oData)
    End Sub

    Public Sub ApproveBookFeePending(ByVal oData As DTO.BookFeePending)
        Dim intBookControl As Integer = 0
        Try
            If oData Is Nothing OrElse oData.BookFeesPendingBookControl = 0 Then Return
            intBookControl = oData.BookFeesPendingBookControl
            'Get the first BookFee that matches bookcontrol, AccessorialCode
            Dim oBookFees = GetBookFeesFilteredByAccessorialCode(oData.BookFeesPendingBookControl, oData.BookFeesPendingAccessorialCode)

            'rules:  Once a pending fee has been approved it will always override all previous fees
            Dim blnFeeFound As Boolean = False
            For Each f In oBookFees
                If blnFeeFound _
                    Or _
                    f.BookFeesAccessorialFeeCalcTypeControl = DAL.Utilities.AccessorialFeeType.Tariff _
                    Or _
                    f.BookFeesAccessorialFeeCalcTypeControl = DAL.Utilities.AccessorialFeeType.Lane Then
                    f.BookFeesOverRidden = True
                    f.BookFeesValue = 0
                    f.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                    UpdateBookFee(f)
                Else
                    blnFeeFound = True
                    With f
                        .BookFeesAccessorialFeeAllocationTypeControl = oData.BookFeesPendingAccessorialFeeAllocationTypeControl
                        .BookFeesAccessorialFeeCalcTypeControl = DAL.Utilities.FeeCalcType.Unique
                        .BookFeesEDICode = oData.BookFeesPendingEDICode
                        .BookFeesMinimum = oData.BookFeesPendingMinimum
                        .BookFeesOverRidden = False
                        .BookFeesTarBracketTypeControl = oData.BookFeesPendingTarBracketTypeControl
                        .BookFeesValue = 0
                        .BookFeesVariable = oData.BookFeesPendingVariable
                        .BookFeesVariableCode = oData.BookFeesPendingVariableCode
                        .TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                    End With
                End If
                UpdateBookFee(f)
            Next
            If Not blnFeeFound Then
                Dim newFee As New DTO.BookFee With {.BookFeesAccessorialCode = oData.BookFeesPendingAccessorialCode, _
                                                    .BookFeesAccessorialFeeAllocationTypeControl = oData.BookFeesPendingAccessorialFeeAllocationTypeControl, _
                                                    .BookFeesAccessorialFeeCalcTypeControl = DAL.Utilities.FeeCalcType.Unique, _
                                                    .BookFeesAccessorialFeeTypeControl = DAL.Utilities.AccessorialFeeType.Order, _
                                                    .BookFeesAllowCarrierUpdates = oData.BookFeesPendingAllowCarrierUpdates, _
                                                    .BookFeesAutoApprove = oData.BookFeesPendingAutoApprove, _
                                                    .BookFeesBOLPlacement = oData.BookFeesPendingBOLPlacement, _
                                                    .BookFeesBOLText = oData.BookFeesPendingBOLText, _
                                                    .BookFeesBookControl = oData.BookFeesPendingBookControl, _
                                                    .BookFeesCaption = oData.BookFeesPendingCaption, _
                                                    .BookFeesEDICode = oData.BookFeesPendingEDICode, _
                                                    .BookFeesIsTax = oData.BookFeesPendingIsTax, _
                                                    .BookFeesMinimum = oData.BookFeesPendingMinimum, _
                                                    .BookFeesOverRidden = False, _
                                                    .BookFeesTarBracketTypeControl = oData.BookFeesPendingTarBracketTypeControl, _
                                                    .BookFeesTaxable = oData.BookFeesPendingTaxable, _
                                                    .BookFeesTaxSortOrder = oData.BookFeesPendingTaxSortOrder, _
                                                    .BookFeesValue = 0, _
                                                    .BookFeesVariableCode = oData.BookFeesPendingVariableCode, _
                                                    .BookFeesVisible = oData.BookFeesPendingVisible, _
                                                    .TrackingState = Core.ChangeTracker.TrackingInfo.Created}
                CreateBookFee(newFee)
            End If

            DeleteBookFeePending(oData)

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("ApproveBookFeePending"))
        Finally
            If intBookControl <> 0 Then
                BookRevenueBLL.AssignCarrier(intBookControl, True)
            End If
        End Try
    End Sub
#End Region

#Region " Public Methods"

#End Region

End Class
