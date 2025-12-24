Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports System.IO
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports LTS = NGL.FreightMaster.Data.LTS
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports TAR = NGL.FM.CarTar
Imports LOC = NGL.FreightMaster.Data.Utilities.TariffLocalizationTypesEnum
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Serilog

Public Class NGLCarrTarBLL : Inherits BLLBaseClass


#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLCarrTarBLL"
        Me.Logger = Logger.ForContext(Of NGLCarrTarBLL)
    End Sub

#End Region

#Region " Properties "



#End Region
     
#Region "DAL Wrapper Methods"



#End Region

#Region " Public Methods"
    

    Public Function ExportCarrTarRatesToExcel(ByVal carrtarControls As ArrayList, ByVal webOrStandardDeployment As Boolean) As String
        If carrtarControls Is Nothing OrElse carrtarControls.Count = 0 Then Return Nothing
        Try
            'TARExportExcel.Parameters = Me.Parameters
            TARExportExcel.clearErrors()
            TARExportExcel.SetDeploymentState(webOrStandardDeployment)
            TARExportExcel.LoadLaneList()
            TARExportExcel.LoadModeTypeList()
            TARExportExcel.LoadBracketTypesList()
            TARExportExcel.LoadClassTypesList()
            TARExportExcel.LoadRateTypesList()
            TARExportExcel.LoadTempTypesList()
            TARExportExcel.LoadCarrTarEquipMatNodes(carrtarControls)
            If (TARExportExcel.Errors.Count > 0) Then
                'create a blank sheet with teh error.
                TARExportExcel.GenerateBlankExcelForErrorResponse()
            Else
                TARExportExcel.GenerateCarrTarRatesToExcel()
            End If
            If (TARExportExcel.Errors.Count > 0) Then
                'create a blank sheet with teh error.
                TARExportExcel.GenerateBlankExcelForErrorResponse()
            End If
            Return TARExportExcel.SavedFileName
        Catch ex As Exception
            If (TARExportExcel.Errors.Count > 0) Then
                'create a blank sheet with teh error.
                TARExportExcel.GenerateBlankExcelForErrorResponse()
                Return TARExportExcel.SavedFileName
            End If
        Finally
            TARExportExcel.CleanUp()
        End Try
        Return Nothing
    End Function

    Public Sub deleteExportedFile(ByVal filename As String)
        Try
            File.Delete(filename)
        Catch ex As Exception

        End Try
    End Sub

    Public Function ImportCarrTarRatesFromExcel(ByVal filepathAndName As String, _
                                                ByVal webOrStandardDeployment As Boolean, _
                                                ByVal EffDateFrom As Date?, _
                                                ByVal EffDateTo As Date?,
                                                ByVal userName As String,
                                                ByVal processName As String) As List(Of Dictionary(Of String, ArrayList))
        If String.IsNullOrEmpty(filepathAndName) Then Return Nothing
        NGLSystemData.StarttblBatchProcessRunning(userName, processName, Utilities.BATCHPROCESSTITLE) 'start the process before the thread starts
        Dim t As New Threading.Thread(Sub()
                                          Try
                                              'TARImportExcel.Parameters = Me.Parameters
                                              TARImportExcel.Errors.Clear()
                                              TARImportExcel.SetDeploymentState(webOrStandardDeployment)
                                              TARImportExcel.LoadLaneList()
                                              TARImportExcel.LoadModeTypeList()
                                              TARImportExcel.LoadBracketTypesList()
                                              TARImportExcel.LoadClassTypesList()
                                              TARImportExcel.LoadRateTypesList()
                                              TARImportExcel.LoadTempTypesList()
                                              TARImportExcel.SavedFileName = filepathAndName
                                              TARImportExcel.ExtractCarrTarRatesFromExcel()
                                              If (TARImportExcel.Errors.Count <> 0) Then
                                                  ''add errors to batchprocessing table
                                                  NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportExcel.ErrorsToJsonString(), Utilities.BATCHPROCESSTITLE)
                                                  Return
                                              End If
                                              If (Not TARImportExcel.validatCloning() Or TARImportExcel.Errors.Count <> 0) Then
                                                  TARImportExcel.AddErrors(LOC.UnableImportRateCloned.ToString, Nothing)
                                              End If

                                              If TARImportExcel.Errors.Count <> 0 Then 'check for errors before continuing.
                                                  ''add errors to batchprocessing table
                                                  NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportExcel.ErrorsToJsonString(), Utilities.BATCHPROCESSTITLE)
                                                  Return
                                              End If
                                              Dim resultBool As Boolean = TARImportExcel.CloneContracts(EffDateFrom, EffDateTo)
                                              If resultBool = False Then 'check for errors
                                                  TARImportExcel.AddErrors(LOC.EImportTariffUnableClone.ToString, Nothing)
                                              End If

                                              If TARImportExcel.Errors.Count = 0 Then
                                                  TARImportExcel.ImportRates(CarTar.Util.ImportExportTypes.ImportFromExcel)
                                              End If
                                              ''add errors to batchprocessing table
                                              If TARImportExcel.Errors.Count = 0 Then
                                                  NGLSystemData.EndtblBatchProcessRunning(userName, processName)
                                              Else
                                                  NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportExcel.ErrorsToJsonString(), Utilities.BATCHPROCESSTITLE)
                                              End If
                                              Return

                                          Catch ex As Exception
                                              NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, ex.Message, Utilities.BATCHPROCESSTITLE)
                                          Finally
                                              TARImportExcel.CleanUp()
                                          End Try
                                          ''add errors to batchprocessing table
                                          NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, "", Utilities.BATCHPROCESSTITLE)
                                          Return
                                      End Sub)
        t.IsBackground = True
        t.Start()
        Return Nothing
    End Function

    Public Function ImportCarrTarRatesFromCSV(ByVal filepathAndName As String, _
                                                ByVal webOrStandardDeployment As Boolean, _
                                                ByVal userName As String,
                                                ByVal processName As String, _
                                                ByVal CarrTarEquipMatName As String) As List(Of Dictionary(Of String, ArrayList))

        webOrStandardDeployment = False 'for now it will always be a local deployment on the same server as the DB.
        If String.IsNullOrEmpty(filepathAndName) Then Return Nothing
        NGLSystemData.StarttblBatchProcessRunning(userName, processName, Utilities.BATCHCSVIMPORTPROCESSTITLE) 'start the process before the thread starts
        Dim t As New Threading.Thread(Sub()
                                          importRatesCSV(filepathAndName, webOrStandardDeployment, userName, processName, CarrTarEquipMatName)
                                         
                                          Return
                                      End Sub)
        t.IsBackground = True
        t.Start()
        Return Nothing
    End Function

    Public Sub importRatesCSV(ByVal filepathAndName As String, _
                                                ByVal webOrStandardDeployment As Boolean, _
                                                ByVal userName As String,
                                                ByVal processName As String, _
                                                ByVal CarrTarEquipMatName As String)
        Try 
            TARImportRatesCSV.Errors.Clear()
            Try
                TARImportRatesCSV.SetDeploymentState(webOrStandardDeployment)
            Catch ex As Exception
                NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, "Could not set SetDeploymentState: " & webOrStandardDeployment & " " & ex.Message, Utilities.BATCHCSVIMPORTPROCESSTITLE)
            End Try
            Try
                TARImportRatesCSV.LoadLaneList()
                TARImportRatesCSV.LoadModeTypeList()
                TARImportRatesCSV.LoadBracketTypesList()
                TARImportRatesCSV.LoadClassTypesList()
                TARImportRatesCSV.LoadRateTypesList()
                TARImportRatesCSV.LoadTempTypesList()
            Catch ex As Exception
                NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, "Could not Load LookupLists: " & ex.Message, Utilities.BATCHCSVIMPORTPROCESSTITLE)
            End Try

            TARImportRatesCSV.SavedFileName = filepathAndName
            Dim importCSVToTmp As Boolean = TARImportRatesCSV.ImportCSVToTempTbl()
            If importCSVToTmp = False Then
                ''add errors to batchprocessing table
                NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportRatesCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTPROCESSTITLE)
                Return
            End If
            If (TARImportRatesCSV.Errors.Count <> 0) Then
                ''add errors to batchprocessing table
                NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportRatesCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTPROCESSTITLE)
                Return
            End If
            TARImportRatesCSV.ExtractCarrTarFirstRateFromTempTbl()
            If (TARImportRatesCSV.Errors.Count <> 0) Then
                ''add errors to batchprocessing table
                NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportRatesCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTPROCESSTITLE)
                Return
            End If
            If (Not TARImportRatesCSV.validatCloning() Or TARImportRatesCSV.Errors.Count <> 0) Then
                TARImportRatesCSV.AddErrors(LOC.UnableImportRateCloned.ToString, Nothing)
            End If

            If TARImportRatesCSV.Errors.Count <> 0 Then 'check for errors before continuing.
                ''add errors to batchprocessing table
                NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportRatesCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTPROCESSTITLE)
                Return
            End If
            Dim resultBool As Boolean = TARImportRatesCSV.CloneContracts()
            If resultBool = False Then 'check for errors
                TARImportRatesCSV.AddErrors(LOC.EImportTariffUnableClone.ToString, Nothing)
            End If

            If TARImportRatesCSV.Errors.Count = 0 Then
                TARImportRatesCSV.ImportRatesByCSV(CarTar.Util.ImportExportTypes.ImportFromCSVRates, CarrTarEquipMatName)
            End If
            ''add errors to batchprocessing table
            If TARImportRatesCSV.Errors.Count = 0 Then
                NGLSystemData.EndtblBatchProcessRunning(userName, processName)
            Else
                NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportRatesCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTPROCESSTITLE)
            End If
            Return

        Catch ex As Exception
            NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, ex.Message, Utilities.BATCHCSVIMPORTPROCESSTITLE)
        Finally
            TARImportRatesCSV.CleanUp()
        End Try
    End Sub

    Public Function ImportCarrTarInterlinePointsFromCSV(ByVal filepathAndName As String, _
                                                ByVal webOrStandardDeployment As Boolean, _
                                                ByVal userName As String,
                                                ByVal processName As String) As List(Of Dictionary(Of String, ArrayList))

        webOrStandardDeployment = False 'for now it will always be a local deployment on the same server as the DB.
        If String.IsNullOrEmpty(filepathAndName) Then Return Nothing
        NGLSystemData.StarttblBatchProcessRunning(userName, processName, Utilities.BATCHCSVIMPORTINTERLINEPROCESSTITLE) 'start the process before the thread starts
        Dim t As New Threading.Thread(Sub()
                                          Try
                                              'TARImportInterlinePointsCSV.Parameters = Parameters
                                              TARImportInterlinePointsCSV.Errors.Clear()
                                              TARImportInterlinePointsCSV.SetDeploymentState(webOrStandardDeployment)

                                              TARImportInterlinePointsCSV.SavedFileName = filepathAndName
                                              Dim importCSVToTmp As Boolean = TARImportInterlinePointsCSV.ImportCSVToTempTbl()
                                              If importCSVToTmp = False Then
                                                  ''add errors to batchprocessing table
                                                  NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportInterlinePointsCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTINTERLINEPROCESSTITLE)
                                                  Return
                                              End If
                                              If (TARImportInterlinePointsCSV.Errors.Count <> 0) Then
                                                  ''add errors to batchprocessing table
                                                  NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportInterlinePointsCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTINTERLINEPROCESSTITLE)
                                                  Return
                                              End If
                                              Dim importTmpTotariff As Boolean = TARImportInterlinePointsCSV.ImportInterlinePoints()
                                              If importTmpTotariff = False Then
                                                  ''add errors to batchprocessing table
                                                  NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportInterlinePointsCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTINTERLINEPROCESSTITLE)
                                                  Return
                                              End If
                                              ''add errors to batchprocessing table 
                                              If TARImportInterlinePointsCSV.Errors.Count = 0 Then
                                                  NGLSystemData.EndtblBatchProcessRunning(userName, processName)
                                              Else
                                                  NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportInterlinePointsCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTINTERLINEPROCESSTITLE)
                                              End If
                                              Return

                                          Catch ex As Exception
                                              NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, ex.Message, Utilities.BATCHCSVIMPORTINTERLINEPROCESSTITLE)
                                          Finally
                                              TARImportInterlinePointsCSV.CleanUp()
                                          End Try
                                          ''add errors to batchprocessing table
                                          NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, "", Utilities.BATCHCSVIMPORTINTERLINEPROCESSTITLE)
                                          Return
                                      End Sub)
        t.IsBackground = True
        t.Start()
        Return Nothing
    End Function

    Public Function ImportCarrTarNonServicePointsFromCSV(ByVal filepathAndName As String, _
                                                ByVal webOrStandardDeployment As Boolean, _
                                                ByVal userName As String,
                                                ByVal processName As String) As List(Of Dictionary(Of String, ArrayList))

        webOrStandardDeployment = False 'for now it will always be a local deployment on the same server as the DB.
        If String.IsNullOrEmpty(filepathAndName) Then Return Nothing
        NGLSystemData.StarttblBatchProcessRunning(userName, processName, Utilities.BATCHCSVIMPORTNONSERVCPROCESSTITLE) 'start the process before the thread starts
        Dim t As New Threading.Thread(Sub()
                                          Try
                                              'TARImportNonServcPointsCSV.Parameters = Parameters
                                              TARImportNonServcPointsCSV.Errors.Clear()
                                              TARImportNonServcPointsCSV.SetDeploymentState(webOrStandardDeployment)

                                              TARImportNonServcPointsCSV.SavedFileName = filepathAndName
                                              Dim importCSVToTmp As Boolean = TARImportNonServcPointsCSV.ImportCSVToTempTbl()
                                              If importCSVToTmp = False Then
                                                  ''add errors to batchprocessing table
                                                  NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportNonServcPointsCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTNONSERVCPROCESSTITLE)
                                                  Return
                                              End If
                                              If (TARImportNonServcPointsCSV.Errors.Count <> 0) Then
                                                  ''add errors to batchprocessing table
                                                  NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportNonServcPointsCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTNONSERVCPROCESSTITLE)
                                                  Return
                                              End If
                                              Dim importTmpTotariff As Boolean = TARImportNonServcPointsCSV.ImportNonServicePoints()
                                              If importTmpTotariff = False Then
                                                  ''add errors to batchprocessing table
                                                  NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportNonServcPointsCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTNONSERVCPROCESSTITLE)
                                                  Return
                                              End If
                                              ''add errors to batchprocessing table 
                                              If TARImportNonServcPointsCSV.Errors.Count = 0 Then
                                                  NGLSystemData.EndtblBatchProcessRunning(userName, processName)
                                              Else
                                                  NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, TARImportNonServcPointsCSV.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTNONSERVCPROCESSTITLE)
                                              End If
                                              Return

                                          Catch ex As Exception
                                              NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, ex.Message, Utilities.BATCHCSVIMPORTNONSERVCPROCESSTITLE)
                                          Finally
                                              TARImportNonServcPointsCSV.CleanUp()
                                          End Try
                                          ''add errors to batchprocessing table
                                          NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, "", Utilities.BATCHCSVIMPORTNONSERVCPROCESSTITLE)
                                          Return
                                      End Sub)
        t.IsBackground = True
        t.Start()
        Return Nothing
    End Function

    Public Function CheckImportTariffBatches(ByVal userName As String, ByVal procedureName As String) As List(Of Dictionary(Of String, ArrayList))

        If NGLSystemData.IsBatchProcessRunning(userName, procedureName, False) Then
            Return Nothing
        Else
            Return GetImportTariffBatcheErrors(userName, procedureName)
        End If
        Return Nothing
    End Function

    Public Function CheckImportTariffBatchesNGLResult(ByVal userName As String, ByVal procedureName As String) As Dictionary(Of String, List(Of DTO.NGLMessage))

        If NGLSystemData.IsBatchProcessRunning(userName, procedureName, False) Then
            Return New Dictionary(Of String, List(Of DTO.NGLMessage))
        Else
            Return simplyErrorList(GetImportTariffBatcheErrors(userName, procedureName))
        End If
        Return New Dictionary(Of String, List(Of DTO.NGLMessage))
    End Function

    Private Function simplyErrorList(ByVal inList As List(Of Dictionary(Of String, ArrayList))) As Dictionary(Of String, List(Of DTO.NGLMessage))
        If inList Is Nothing Then Return Nothing
        Dim newList As New Dictionary(Of String, List(Of DTO.NGLMessage))
        For Each item In inList
            If item IsNot Nothing Then
                For Each innerItem In item
                    Dim nglList As New List(Of DTO.NGLMessage)
                    If innerItem.Value IsNot Nothing Then
                        For Each arrayItem In innerItem.Value
                            nglList.Add(New DTO.NGLMessage(arrayItem))
                        Next
                    End If
                    newList.Add(innerItem.Key, nglList)
                Next
            End If 
        Next
        Return newList
    End Function

    Public Function GetImportTariffBatcheErrors(ByVal userName As String, ByVal procedureName As String) As List(Of Dictionary(Of String, ArrayList))

        Dim batchobj As DTO.tblBatchProcessRunning = NGLSystemData.GettblBatchProcessRunning(userName, procedureName)
        If batchobj IsNot Nothing AndAlso batchobj.BPRControl <> 0 Then
            If Not String.IsNullOrEmpty(batchobj.BPHErrMsg) And batchobj.BPHHadErrors = True Then
                Dim list As New List(Of Dictionary(Of String, ArrayList))
                Dim arr As JArray = JsonConvert.DeserializeObject(batchobj.BPHErrMsg) '.<List<SelectableEnumItem>>()
                Dim jarrayList = arr.ToList()
                For Each item In jarrayList
                    Dim newItem As New Dictionary(Of String, ArrayList)
                    For Each arrayitem In item.ToList
                        newItem.Add(DirectCast(arrayitem, Newtonsoft.Json.Linq.JProperty).Name, New ArrayList(arrayitem.Children().Values(Of String).ToArray))
                    Next
                    list.Add(newItem)
                Next

                Return list
            ElseIf batchobj.BPHHadErrors = False Then
                'must be succesfull
                Dim list As New List(Of Dictionary(Of String, ArrayList))
                Dim item As New Dictionary(Of String, ArrayList)
                item.Add("EImportTariffSuccess", Nothing)
                list.Add(item)
                Return list
            End If
        End If

        Return Nothing
    End Function

    Public Sub CopyContract(ByVal CarrTarControl As Integer, _
                               ByVal EffDateFrom As Date?, _
                               ByVal EffDateTo As Date?, _
                               ByVal AutoApprove As Boolean, _
                               ByVal IssuedDate As Date?, _
                               ByVal CopyClassXrefData As Boolean, _
                               ByVal CopyNoDriveDays As Boolean, _
                               ByVal CopyDiscountData As Boolean, _
                               ByVal CopyFeeData As Boolean, _
                               ByVal CopyInterlinePointData As Boolean, _
                               ByVal CopyMinChargeData As Boolean, _
                               ByVal CopyMinWeightData As Boolean, _
                               ByVal CopyNonServicePointData As Boolean, _
                               ByVal CopyMatrixBPData As Boolean, _
                               ByVal CopyEquipmentData As Boolean, _
                               ByVal CopyEquipmentRateData As Boolean, _
                               ByVal CopyFuelData As Boolean, _
                               ByVal newCompControl As Integer, _
                               ByVal newContractName As String, _
                                                ByVal userName As String,
                                                ByVal processName As String)

        NGLSystemData.StarttblBatchProcessRunning(userName, processName, Utilities.BATCHCSVIMPORTPROCESSTITLE) 'start the process before the thread starts
        Dim t As New Threading.Thread(Sub()
                                          CarrTarCopy.Errors.Clear()
                                          Dim success As Boolean = CarrTarCopy.CopyContract(CarrTarControl, _
                                                 EffDateFrom, _
                                                 EffDateTo, _
                                                 AutoApprove, _
                                                 IssuedDate, _
                                                 CopyClassXrefData, _
                                                 CopyNoDriveDays, _
                                                 CopyDiscountData, _
                                                 CopyFeeData, _
                                                 CopyInterlinePointData, _
                                                 CopyMinChargeData, _
                                                 CopyMinWeightData, _
                                                 CopyNonServicePointData, _
                                                 CopyMatrixBPData, _
                                                 CopyEquipmentData, _
                                                 CopyEquipmentRateData, _
                                                 CopyFuelData, _
                                                 newCompControl, _
                                                 newContractName)
                                          If success = False And CarrTarCopy.Errors.Count = 0 Then 'check for errors
                                              'add an error if success is false.
                                              CarrTarCopy.AddErrors(LOC.ECopyConUnableCopy.ToString, Nothing)
                                          End If
                                          If CarrTarCopy.Errors.Count = 0 Then
                                              NGLSystemData.EndtblBatchProcessRunning(userName, processName)
                                          Else
                                              NGLSystemData.EndtblBatchProcessRunningWithError(userName, processName, CarrTarCopy.ErrorsToJsonString(), Utilities.BATCHCSVIMPORTPROCESSTITLE)
                                          End If
                                          Return
                                      End Sub)
        t.IsBackground = True
        t.Start()
        Return
    End Sub

#End Region

#Region "Private Methods"


#End Region

End Class
