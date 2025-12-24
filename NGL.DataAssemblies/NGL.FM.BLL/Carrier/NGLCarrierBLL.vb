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
Imports Serilog
Imports Serilog.Core
Imports Serilog.Events
Imports SerilogTracing

Public Class NGLCarrierBLL : Inherits BLLBaseClass


#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLCarrierBLL"
        Logger = Logger.ForContext(Of NGLCarrierBLL)
    End Sub

#End Region

#Region " Properties "



#End Region

#Region "Delegates"
    'Modified by RHR for v-7.0.5.103 on 02/19/2017
    Public Delegate Sub ProcessCarrierFeesAsyncDelegate(ByVal CarrierControl As Integer)
#End Region

#Region "DAL Wrapper Methods"

    ''' <summary>
    ''' Wrapper method for UpdateAllCarrierFuelFeesBatch which returns true or false 
    ''' and formats all batch errors into one fault exception
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateAllCarrierFuelFees() As Boolean
        Dim blnRet As Boolean = False
        Dim oRet = UpdateAllCarrierFuelFeesBatch()
        If Not oRet Is Nothing AndAlso oRet.Success = True Then
            If Not oRet.Errors Is Nothing AndAlso oRet.Errors.Count > 0 Then
                Dim strServerMessage As String = String.Join(" | ", oRet.Errors.ToArray())
                'some record could not be processed so raise an error
                throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_BatchProcessError, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, New List(Of String) From {strServerMessage}, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_ProcessProcedureFailure)
            Else
                blnRet = True
            End If
        End If
        Return blnRet
    End Function

    Public Function UpdateAllCarrierFuelFeesBatch() As DTO.WCFResults
        Dim oRet As New DTO.WCFResults With {.Key = 0, .Success = False}
        Try
            Dim intCarriers As List(Of Integer) = NGLCarrierData.GetActiveCarrierControls()
            If intCarriers Is Nothing OrElse intCarriers.Count = 0 Then
                'Nothing to do so return true
                oRet.Success = True
                Return oRet
            End If
            If oRet.BLLOnlyData Is Nothing Then
                oRet.BLLOnlyData = (New List(Of DTO.DTOBaseClass)).ToArray
            End If
            If oRet.Log Is Nothing Then
                oRet.Log = (New List(Of DTO.NGLMessage))
            End If
            If oRet.intValues Is Nothing Then
                oRet.intValues = (New List(Of DTO.NGLListItem))
            End If
            For Each c In intCarriers
                Dim carrResult As New DTO.WCFResults With {.Key = 0, .Success = False}
                carrResult = UpdateCarrierFuelFeesSync(c)
                Dim dList As New List(Of DTO.DTOBaseClass)
                If carrResult.Log Is Nothing Then carrResult.Log = New List(Of DTO.NGLMessage)
                If (Not carrResult.Log Is Nothing AndAlso carrResult.Log.Count > 0) Then oRet.Log.AddRange(carrResult.Log)
                If (Not carrResult.intValues Is Nothing AndAlso carrResult.intValues.Count > 0) Then oRet.intValues.AddRange(carrResult.intValues)
                If carrResult.BLLOnlyData IsNot Nothing AndAlso carrResult.BLLOnlyData.Count > 0 Then
                    dList = oRet.BLLOnlyData.ToList
                    dList.AddRange(carrResult.BLLOnlyData.ToList())
                    oRet.BLLOnlyData = dList.ToArray
                End If
            Next
            oRet.Success = True
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("UpdateAllCarrierFuelFees"), FreightMaster.Data.sysErrorParameters.sysErrorState.UserLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        Return oRet
    End Function


    ''' <summary>
    ''' Call an Asynch method to correct client time out error
    ''' </summary>
    ''' <param name="carrierControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.103 on 02/19/2017
    ''' </remarks>
    Public Function UpdateCarrierFuelFees(ByVal carrierControl As Integer) As DTO.WCFResults
        Dim oRet As New DTO.WCFResults With {.Key = 0, .Success = True}
        If oRet.Log Is Nothing Then oRet.Log = New List(Of DTO.NGLMessage)
        Dim msg As New DTO.NGLMessage("Background Process Running On Server.  No further notificaiton is expected")
        oRet.Log.Add(msg)
        Dim fetcher As New ProcessCarrierFeesAsyncDelegate(AddressOf Me.UpdateCarrierFuelFeesAsync)
        ' Launch thread
        fetcher.BeginInvoke(carrierControl, Nothing, Nothing)
        Return oRet

    End Function


    ''' <summary>
    ''' Process Carrier Fuel updates asynchronous
    ''' </summary>
    ''' <param name="carrierControl"></param>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.103 on 02/19/2017
    ''' Modified by RHR for v-7.0.6.103 on 2/23/2017
    '''   removed test code to write success to app error table.  No longer needed
    ''' </remarks>
    Public Sub UpdateCarrierFuelFeesAsync(ByVal carrierControl As Integer)
        Dim oRet As New DTO.WCFResults With {.Key = 0, .Success = False}
        Try
            Try
                If NGLBatchProcessData.UpdateCarrierFuelFees2Way(carrierControl) Then
                    Dim oBookRet = BookRevenueBLL.UpdateBookFuelFeesBatch(carrierControl)
                    If (Not oBookRet Is Nothing AndAlso oBookRet.Success = True) Then
                        If oBookRet.Success = False Then oRet.Success = False
                        If oRet.Log Is Nothing Then oRet.Log = New List(Of DTO.NGLMessage)
                        If (Not oBookRet.Log Is Nothing AndAlso oBookRet.Log.Count > 0) Then oRet.Log.AddRange(oBookRet.Log)
                        If (Not oBookRet.BLLOnlyData Is Nothing AndAlso oBookRet.BLLOnlyData.Count > 0) Then
                            Dim dList As New List(Of DTO.DTOBaseClass)
                            If oRet.BLLOnlyData Is Nothing Then
                                dList = oBookRet.BLLOnlyData.ToList()
                            Else
                                dList = oRet.BLLOnlyData.ToList()
                                dList.AddRange(oBookRet.BLLOnlyData.ToList())
                            End If
                            'Modified by RHR 8/26/14 cannot save BookReferenceData int DTOData so we created a server
                            'side only propert BLLOnlyData.  this must be used by any server components to read the results.
                            oRet.BLLOnlyData = dList.ToArray()
                        End If
                    End If
                End If
                oRet.intValues.Add(New DTO.NGLListItem(carrierControl))
            Catch ex As FaultException(Of DAL.SqlFaultInfo)
                oRet.Success = False
                oRet.Log.Add(New DTO.NGLMessage(ex.Message & " carriercontrol: " & carrierControl & " " & ex.Detail.ToString()))
            Catch ex As Exception
                oRet.Success = False
                oRet.Log.Add(New DTO.NGLMessage(ex.Message & " carriercontrol: " & carrierControl))
            End Try

            If Not oRet.Success AndAlso Not oRet.Log Is Nothing AndAlso oRet.Log.Count() > 0 Then
                Dim strMsg As String = ""
                For Each l In oRet.Log
                    strMsg &= l.Message
                Next
                SaveAppError("Update Carrier Fuel Fees Async Errors: " & strMsg)
                'Else
                '    SaveAppError("Update Carrier Fuel Fees Async Success!")
            End If
        Catch ex As Exception
            SaveAppError("Update Carrier Fuel Fees Async Error: " & ex.Message)
        End Try
        Return
    End Sub

    ''' <summary>
    ''' Process Carrier Fuel updates synchronous
    ''' </summary>
    ''' <param name="carrierControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.103 on 02/19/2017
    ''' </remarks>
    Public Function UpdateCarrierFuelFeesSync(ByVal carrierControl As Integer) As DTO.WCFResults




        Dim oRet As New DTO.WCFResults With {.Key = 0, .Success = False}

        Using operation = Logger.StartActivity("UpdateCarrierFuelFeesSync({carrierControl})", carrierControl)
            Try
                Try
                    Logger.Information("Check if NGLBatchProcessData.UpdateCarrierFuel2Way({CarrierControl})", carrierControl)
                    If NGLBatchProcessData.UpdateCarrierFuelFees2Way(carrierControl) Then
                        Dim oBookRet = BookRevenueBLL.UpdateBookFuelFeesBatch(carrierControl)
                        If (Not oBookRet Is Nothing AndAlso oBookRet.Success = True) Then
                            oRet.Success = True
                            If oRet.Log Is Nothing Then oRet.Log = New List(Of DTO.NGLMessage)
                            If (Not oBookRet.Log Is Nothing AndAlso oBookRet.Log.Count > 0) Then oRet.Log.AddRange(oBookRet.Log)
                            If (Not oBookRet.BLLOnlyData Is Nothing AndAlso oBookRet.BLLOnlyData.Count > 0) Then
                                Dim dList As New List(Of DTO.DTOBaseClass)
                                If oRet.BLLOnlyData Is Nothing Then
                                    dList = oBookRet.BLLOnlyData.ToList()
                                Else
                                    dList = oRet.BLLOnlyData.ToList()
                                    dList.AddRange(oBookRet.BLLOnlyData.ToList())
                                End If
                                'Modified by RHR 8/26/14 cannot save BookReferenceData int DTOData so we created a server
                                'side only propert BLLOnlyData.  this must be used by any server components to read the results.
                                oRet.BLLOnlyData = dList.ToArray()
                            End If
                        End If
                    End If
                    oRet.intValues.Add(New DTO.NGLListItem(carrierControl))
                Catch ex As FaultException(Of DAL.SqlFaultInfo)
                    oRet.Log.Add(New DTO.NGLMessage(ex.Message & " carriercontrol: " & carrierControl & " " & ex.Detail.ToString()))
                Catch ex As Exception
                    oRet.Log.Add(New DTO.NGLMessage(ex.Message & " carriercontrol: " & carrierControl))
                End Try

            Catch ex As FaultException
                Throw
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("UpdateCarrierFuelFeesSync"), FreightMaster.Data.sysErrorParameters.sysErrorState.UserLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try
        End Using
        Return oRet

    End Function


    ''' <summary>
    ''' this is just a wrapper for the tariff system. 
    ''' </summary>
    ''' <param name="carrTarControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateCarrierFuelFeesByContract(ByVal carrTarControl As Integer) As DTO.WCFResults

        Dim oRet As New DTO.WCFResults With {.Key = 0, .Success = False}
        Try
            Dim contract As DTO.CarrTarContract = Nothing
            contract = GetCarrTarContract(carrTarControl)
            If contract IsNot Nothing AndAlso contract.CarrTarControl > 0 Then
                oRet = UpdateCarrierFuelFeesSync(contract.CarrTarCarrierControl)
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("UpdateCarrierFuelFeesByContract"), FreightMaster.Data.sysErrorParameters.sysErrorState.UserLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        Return oRet
    End Function

#Region "CarrierFuelAddendum Data"

    Public Function GetCarrierFuelSurcharge(ByVal CarrierControl As Integer,
                                     ByVal CarrTarControl As Integer,
                                     ByVal CarrTarEquipControl As Integer,
                                     ByVal STATE As String,
                                     ByVal EffectiveDate As Date) As DTO.FuelSurchargeResult
        Dim result As New DTO.FuelSurchargeResult
        Dim sqlResult As LTS.spGetFuelSurchargeResult = NGLCarrierFuelAddendumData.GetFuelSurCharge(CarrierControl, CarrTarControl, CarrTarEquipControl, STATE, EffectiveDate)
        If sqlResult IsNot Nothing Then
            result = NGLCarrierFuelAddendumData.selectDTO(sqlResult)
        End If
        Return result
    End Function

    ''' <summary>
    ''' this is just a wrapper for the GetCarrierFuelSurcharge
    ''' </summary>
    ''' <param name="CarrTarControl"></param>
    ''' <param name="CarrTarEquipControl"></param>
    ''' <param name="STATE"></param>
    ''' <param name="EffectiveDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrierFuelSurchargeByContract(ByVal CarrTarControl As Integer,
                                     ByVal CarrTarEquipControl As Integer,
                                     ByVal STATE As String,
                                     ByVal EffectiveDate As Date) As DTO.FuelSurchargeResult
        Dim result As New DTO.FuelSurchargeResult
        Using operation = Logger.StartActivity("GetCarrierFuelSurchargeByContract (CarrTarControl: {CarrTarControl}, CarrTarEquipControl: {CarrTarEquipControl}, STATE: {STATE}, EffectiveDate: {EffectiveDate}",CarrTarControl, CarrTarEquipControl, STATE,EffectiveDate)

            Dim contract As DTO.CarrTarContract = NGLCarrTarContractData.GetRecordFiltered(CarrTarControl)
            If contract Is Nothing OrElse contract.CarrTarControl = 0 Then
                Return Nothing
            End If
            Dim sqlResult As LTS.spGetFuelSurchargeResult = NGLCarrierFuelAddendumData.GetFuelSurCharge(contract.CarrTarCarrierControl, CarrTarControl, CarrTarEquipControl, STATE, EffectiveDate)
            If sqlResult IsNot Nothing Then
                result = NGLCarrierFuelAddendumData.selectDTO(sqlResult)
            End If
        End Using

        Return result
    End Function
    ''' <summary>
    ''' Method to update the CarrierFuelFees By CarrierControlID
    ''' </summary>
    ''' <param name="carControl"></param>
    ''' <remarks>
    ''' Added By ManoRama on 01-SEP-2020 foe carrier Fuel addendum changes :Update All Active Loads
    ''' </remarks>
    ''' <returns></returns>
    Public Function UpdateCarrierFuelFeesByCarrier(ByVal carControl As Integer) As DTO.WCFResults

        Dim oRet As New DTO.WCFResults With {.Key = 0, .Success = False}
        Try

            If carControl > 0 Then
                oRet = UpdateCarrierFuelFeesSync(carControl)

            End If
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("UpdateCarrierFuelFeesByContract"), FreightMaster.Data.sysErrorParameters.sysErrorState.UserLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        Return oRet
    End Function

#End Region

#Region "   CarrTarContract Data"

    Public Function CreateCarrTarContract(ByVal oData As DTO.CarrTarContract) As DTO.CarrTarContract
        Return NGLCarrTarContractData.CreateRecord(oData)
    End Function

    Public Function GetCarrTarContract(ByVal carrtarcontrol As Integer) As DTO.CarrTarContract
        Return NGLCarrTarContractData.GetRecordFiltered(carrtarcontrol)
    End Function

    Public Sub DeleteCarrTarContract(ByVal oData As DTO.CarrTarContract)
        NGLCarrTarContractData.DeleteRecord(oData)
    End Sub

    Public Function UpdateCarrTarContract(ByVal oData As DTO.CarrTarContract) As DTO.CarrTarContract
        Dim result As DTO.CarrTarContract = NGLCarrTarContractData.UpdateRecord(oData)
        UpdateEquipAvailSatSun(result.CarrTarControl, result.CarrTarWillDriveSaturday, result.CarrTarWillDriveSunday)
        Return result
    End Function

    Public Function UpdateCarrTarContractQuick(ByVal oData As DTO.CarrTarContract) As DTO.QuickSaveResults
        Dim oRet As DTO.QuickSaveResults = NGLCarrTarContractData.UpdateRecordQuick(oData)
        UpdateEquipAvailSatSun(oData.CarrTarControl, oData.CarrTarWillDriveSaturday, oData.CarrTarWillDriveSunday)
        Return oRet
    End Function

    Public Sub UpdateCarrTarContractNoReturn(ByVal oData As DTO.CarrTarContract)
        NGLCarrTarContractData.UpdateRecordNoReturn(oData)
        UpdateEquipAvailSatSun(oData.CarrTarControl, oData.CarrTarWillDriveSaturday, oData.CarrTarWillDriveSunday)
    End Sub

    Public Function GetTariffShippersByCarrier(ByVal sortKey As Integer, ByVal CarrierControl As Integer) As DTO.vComp()
        Return NGLCarrTarContractData.GetTariffShippersByCarrier(sortKey, CarrierControl)
    End Function

    Public Function GetTariffShippers(ByVal sortKey As Integer) As DTO.vComp()
        Return NGLCarrTarContractData.GetTariffShippers(sortKey)
    End Function

#End Region

#Region "   CarrTarEquip Data"

    Public Function CreateCarrTarEquip(ByVal oData As DTO.CarrTarEquip) As DTO.CarrTarEquip
        If oData Is Nothing Then Return Nothing
        Dim result As DTO.CarrTarEquip = NGLCarrTarEquipData.CreateRecord(oData)
        UpdateContractWillDriveDays(result.CarrTarEquipCarrTarControl,
                                    result.CarrTarEquipDLSat,
                                    result.CarrTarEquipPUSat,
                                    result.CarrTarEquipDLSun,
                                    result.CarrTarEquipPUSun)
        Return result
    End Function

    Public Sub DeleteCarrTarEquip(ByVal oData As DTO.CarrTarEquip)
        NGLCarrTarEquipData.DeleteRecord(oData)
    End Sub

    Public Function UpdateCarrTarEquip(ByVal oData As DTO.CarrTarEquip) As DTO.CarrTarEquip
        If oData Is Nothing Then Return Nothing
        Dim result As DTO.CarrTarEquip = NGLCarrTarEquipData.UpdateRecord(oData)
        UpdateContractWillDriveDays(result.CarrTarEquipCarrTarControl,
                                    result.CarrTarEquipDLSat,
                                    result.CarrTarEquipPUSat,
                                    result.CarrTarEquipDLSun,
                                    result.CarrTarEquipPUSun)
        Return result
    End Function

    Public Function UpdateCarrTarEquipQuick(ByVal oData As DTO.CarrTarEquip) As DTO.QuickSaveResults
        If oData Is Nothing Then Return Nothing
        Dim oRet As DTO.QuickSaveResults = NGLCarrTarEquipData.UpdateRecordQuick(oData)
        UpdateContractWillDriveDays(oData.CarrTarEquipCarrTarControl,
                                    oData.CarrTarEquipDLSat,
                                    oData.CarrTarEquipPUSat,
                                    oData.CarrTarEquipDLSun,
                                    oData.CarrTarEquipPUSun)
        Return oRet
    End Function

    Public Sub UpdateCarrTarEquipNoReturn(ByVal oData As DTO.CarrTarEquip)
        If oData Is Nothing Then Return
        NGLCarrTarEquipData.UpdateRecordNoReturn(oData)
        UpdateContractWillDriveDays(oData.CarrTarEquipCarrTarControl,
                                    oData.CarrTarEquipDLSat,
                                    oData.CarrTarEquipPUSat,
                                    oData.CarrTarEquipDLSun,
                                    oData.CarrTarEquipPUSun)
    End Sub

#End Region

#End Region

#Region " Public Methods"

    ''' <summary>
    ''' Update the equipment with will drive booleans from the contract level.
    ''' Only update then equipment level weekend days if the contract level is false.
    ''' they may want teh contract level to be true but a particular equipment false
    ''' </summary>
    ''' <param name="contractControl"></param>
    ''' <param name="willDriveSat"></param>
    ''' <param name="willDriveSun"></param>
    ''' <remarks></remarks>
    Public Sub UpdateEquipAvailSatSun(ByVal contractControl As Integer,
                                   ByVal willDriveSat As Boolean,
                                   ByVal willDriveSun As Boolean)
        Try
            Dim arr() As DTO.CarrTarEquip = NGLCarrTarEquipData.GetCarrTarEquipsFiltered(contractControl)
            If arr Is Nothing Then Return
            For Each item As DTO.CarrTarEquip In arr
                If willDriveSat = False Then
                    item.CarrTarEquipDLSat = willDriveSat
                    item.CarrTarEquipPUSat = willDriveSat
                End If
                If willDriveSun = False Then
                    item.CarrTarEquipDLSun = willDriveSun
                    item.CarrTarEquipPUSun = willDriveSun
                End If
                NGLCarrTarEquipData.UpdateRecordNoReturn(item)
            Next
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("UpdateEquipAvailSatSun"))
        End Try
    End Sub

    ''' <summary>
    '''  ''' Data Integrity Rule:
    ''' If the user marks Will Drive Sunday/Saturday to false. It will go through all of the pieces of related equipment and mark Available Sat/Sun to false.
    '''	If Will Drive Sat/Sun is false on the contract and the user marks a equipment Avail Sat/Sun to true.  The system will mark Will Drive Sat/Sun to true on the contract as well.
    ''' </summary>
    ''' <param name="contractControl"></param>
    ''' <param name="CarrTarEquipDLSat"></param>
    ''' <param name="CarrTarEquipPUSat"></param>
    ''' <remarks></remarks>
    Public Sub UpdateContractWillDriveDays(ByVal contractControl As Integer,
                                   ByVal CarrTarEquipDLSat As Boolean,
                                   ByVal CarrTarEquipPUSat As Boolean,
                                    ByVal CarrTarEquipDLSun As Boolean,
                                   ByVal CarrTarEquipPUSun As Boolean)
        Try
            Dim item As DTO.CarrTarContract = NGLCarrTarContractData.GetCarrTarContractFiltered(contractControl)
            If item Is Nothing Then Return
            If (CarrTarEquipDLSat = True Or
                CarrTarEquipPUSat = True) And item.CarrTarWillDriveSaturday = False Then
                item.CarrTarWillDriveSaturday = True
            End If
            If (CarrTarEquipDLSun = True Or
                CarrTarEquipPUSun = True) And item.CarrTarWillDriveSunday = False Then
                item.CarrTarWillDriveSunday = True
            End If
            NGLCarrTarContractData.UpdateRecordNoReturn(item)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("UpdateContractWillDriveDays"))
        End Try
    End Sub

    ''' <summary>
    ''' Returns Carrier Pro Number information
    ''' </summary>
    ''' <param name="CarrTarEquipControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.100 05/24/2016
    '''   Added logic to read the carrier pro number block data if it is available
    ''' </remarks>
    Public Function GetCarrierProNumberByEquip(ByVal CarrTarEquipControl As Integer) As DTO.CarrierPRONumberResult

        Dim oProData As New DTO.CarrierPRONumberResult
        Try
            Dim oPro As New DTO.CarrierProNumber
            Dim intSeed As Long = 0
            Dim intRetryCt As Integer = 3
            Dim oCheckSum As New CheckSum
            Do While intSeed = 0
                intRetryCt -= 1
                If intRetryCt <= 0 Then NGLCarrierProNumberData.throwNoDataFaultException("Cannot find any Carrier PRO Number Seed data for " & oPro.CarrProName)
                'get the carrier pro number data:
                oPro = NGLCarrierProNumberData.GetCarrierProNumberByTarEquip(CarrTarEquipControl)
                If Not oPro Is Nothing AndAlso oPro.CarrProControl <> 0 Then
                    If oPro.CarrProChkDigAlgControl = CheckSum.CheckDigitAlgorithm.Batch Then
                        'Modified by RHR v-7.0.5.100 05/24/2016
                        'Call logic to get next block
                        Dim oResult As LTS.spGetNextCarrierProNumberBlockResult = NGLCarrierProNumberData.getNextCarrierProBlock(oPro.CarrProControl)
                        oProData.CarrierProNumber = oResult.CarrProNbrBlockNumber
                        oProData.CarrierProNumberRaw = oResult.CarrProNbrBlockNumber
                        oProData.CarrProControl = oPro.CarrProControl
                        oProData.ErrorNumber = oResult.ErrNumber
                        oProData.RetMsg = oResult.RetMsg
                        Exit Do
                    Else
                        'we have a record so get the next seed
                        intSeed = NGLCarrierProNumberData.getNextProSeed(oPro.CarrProControl)
                        If intSeed = -1 Then NGLCarrierProNumberData.throwSQLFaultException("Cannot Read Carrier PRO Number Seed data for " & oPro.CarrProName)
                        If intSeed > 0 Then

                            Dim strSeed As String = intSeed.ToString()
                            oProData.CarrProControl = oPro.CarrProControl

                            'get the length for padding
                            Dim padLen As Integer = oPro.CarrProLength
                            strSeed = strSeed.PadLeft(padLen, "0"c)

                            oProData.CarrierProNumberRaw = strSeed

                            oProData.CarrierProNumber = strSeed
                            'check if we need to append the prefix for the check digit calculation
                            If oPro.CarrProAppendPrefixForCheckDigit Then
                                strSeed = oPro.CarrProPrefix & strSeed
                            End If
                            If oPro.CarrProAppendSuffixForCheckDigit Then
                                strSeed &= oPro.CarrProSuffix
                            End If
                            Dim strWeight As String = ""
                            If If(oPro.CarrProCheckDigitWeightFactor, 0) > 0 Then
                                strWeight = oPro.CarrProCheckDigitWeightFactor.ToString()
                            End If
                            Dim strCheckDigit = oCheckSum.GetCheckDigit(oPro.CarrProChkDigAlgControl, strSeed, strWeight, oPro.CarrProCheckDigitUseIndexForWeightFactor, oPro.CarrProCheckDigitIndexForWeightFactorMin, oPro.CarrProCheckDigitErrorCode, oPro.CarrProCheckDigit10Code, oPro.CarrProCheckDigitOver10Code, oPro.CarrProCheckDigitZeroCode, oPro.CarrProCheckDigitSplitWeightFactorDigits, oPro.CarrProCheckDigitUseSubtractionFactor, oPro.CarrProCheckDigitSubtractionFactor)
                            'TODO: add better exception logic here
                            If strCheckDigit = oPro.CarrProCheckDigitErrorCode Then NGLCarrierProNumberData.throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_ApplicationException, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, "Cannot generate check digit", FreightMaster.Data.SqlFaultInfo.FaultReasons.E_UnExpected)

                            oProData.CarrierProNumberRaw = String.Concat(oPro.CarrProPrefix.Trim, oProData.CarrierProNumberRaw.Trim, oPro.CarrProSuffix.Trim, strCheckDigit.Trim)
                            oProData.CarrierProNumber = String.Concat(oPro.CarrProPrefix.Trim, oPro.CarrProPrefixSpacer.Trim, oProData.CarrierProNumber.Trim, oPro.CarrProSuffixSpacer.Trim, oPro.CarrProSuffix.Trim, oPro.CarrProCheckDigitSpacer.Trim, strCheckDigit.Trim)
                            Exit Do
                        End If
                    End If
                Else
                    Exit Do
                End If
            Loop

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrierProNumberByEquip"))
        End Try
        Return oProData
    End Function


    ''' <summary>
    ''' Reads the CarrierProNumber data from the database and simulates calculation of the next Carrier Pro Number
    ''' using the variable intSeedFactor 
    ''' </summary>
    ''' <param name="CarrProControl"></param>
    ''' <param name="intSeedFactor"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.100 05/24/2016
    '''   added logic to check the CarrProChkDigAlgControl value and read from the 
    '''   blocks table to get the next number if the CarrProChkDigAlgControl value is 6 "Block"
    ''' </remarks>
    Public Function GetCarrierSampleProNumber(ByVal CarrProControl As Integer, Optional ByVal intSeedFactor As Integer = 1) As DTO.CarrierPRONumberResult

        Dim oProData As New DTO.CarrierPRONumberResult
        Try
            Dim oPro As DTO.CarrierProNumber = NGLCarrierProNumberData.GetCarrierProNumberFiltered(CarrProControl)

            Dim intSeed As Long = 0
            Dim intRetryCt As Integer = 3
            Dim oCheckSum As New CheckSum


            If Not oPro Is Nothing AndAlso oPro.CarrProControl <> 0 Then
                If oPro.CarrProChkDigAlgControl = CheckSum.CheckDigitAlgorithm.Batch Then
                    'Modified by RHR v-7.0.5.100 05/24/2016
                    'Call logic to get next block
                    Dim oResult As LTS.spGetNextCarrierProNumberBlockResult = NGLCarrierProNumberData.getNextCarrierProBlock(CarrProControl)
                    oProData.CarrierProNumber = oResult.CarrProNbrBlockNumber
                    oProData.CarrierProNumberRaw = oResult.CarrProNbrBlockNumber
                    oProData.CarrProControl = CarrProControl
                    oProData.ErrorNumber = oResult.ErrNumber
                    oProData.RetMsg = oResult.RetMsg
                Else
                    intSeed = oPro.CarrProSeedStart + intSeedFactor

                    If intSeed > 0 Then

                        Dim strSeed As String = intSeed.ToString()
                        oProData.CarrProControl = oPro.CarrProControl

                        'get the length for padding
                        Dim padLen As Integer = oPro.CarrProLength
                        strSeed = strSeed.PadLeft(padLen, "0"c)

                        oProData.CarrierProNumberRaw = strSeed

                        oProData.CarrierProNumber = strSeed
                        'check if we need to append the prefix for the check digit calculation
                        If oPro.CarrProAppendPrefixForCheckDigit Then
                            strSeed = oPro.CarrProPrefix & strSeed
                        End If
                        If oPro.CarrProAppendSuffixForCheckDigit Then
                            strSeed &= oPro.CarrProSuffix
                        End If
                        Dim strWeight As String = ""
                        If If(oPro.CarrProCheckDigitWeightFactor, 0) > 0 Then
                            strWeight = oPro.CarrProCheckDigitWeightFactor.ToString()
                        End If
                        Dim strCheckDigit = oCheckSum.GetCheckDigit(oPro.CarrProChkDigAlgControl, strSeed, strWeight, oPro.CarrProCheckDigitUseIndexForWeightFactor, oPro.CarrProCheckDigitIndexForWeightFactorMin, oPro.CarrProCheckDigitErrorCode, oPro.CarrProCheckDigit10Code, oPro.CarrProCheckDigitOver10Code, oPro.CarrProCheckDigitZeroCode, oPro.CarrProCheckDigitSplitWeightFactorDigits, oPro.CarrProCheckDigitUseSubtractionFactor, oPro.CarrProCheckDigitSubtractionFactor)
                        'TODO: add better exception logic here
                        If strCheckDigit = oPro.CarrProCheckDigitErrorCode Then NGLCarrierProNumberData.throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_ApplicationException, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, "Cannot generate check digit", FreightMaster.Data.SqlFaultInfo.FaultReasons.E_UnExpected)

                        oProData.CarrierProNumberRaw = String.Concat(oPro.CarrProPrefix.Trim, oProData.CarrierProNumberRaw.Trim, oPro.CarrProSuffix.Trim, strCheckDigit.Trim)
                        oProData.CarrierProNumber = String.Concat(oPro.CarrProPrefix.Trim, oPro.CarrProPrefixSpacer.Trim, oProData.CarrierProNumber.Trim, oPro.CarrProSuffixSpacer.Trim, oPro.CarrProSuffix.Trim, oPro.CarrProCheckDigitSpacer.Trim, strCheckDigit.Trim)

                    End If


                End If
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("GetCarrierSampleProNumber"))
        End Try
        Return oProData
    End Function

#End Region

#Region "Private Methods"


#End Region

End Class
