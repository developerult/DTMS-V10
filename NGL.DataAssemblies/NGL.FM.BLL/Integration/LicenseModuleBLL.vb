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
Imports Models = NGL.FreightMaster.Data.Models
Imports NGL.Core.Utility
Imports NGLREST = NGL.FM.NGLRestIntegrations

'Added By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility

Public Class LicenseModuleBLL : Inherits BLLBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "LicenseModuleBLL"
    End Sub

#End Region

#Region "Enum"

    ''' <summary>
    ''' A list of the modules that we intend to support.
    ''' *NOTE: If you add a value to the enum don't forget to also add a line for the new module to GetListOfModules()
    ''' </summary>
    Public Enum Modules
        None
        PCMilerNetworkLicense
        PCMilerAPILicense
        BingMapsLicense
        TrimbleMapsLicense
        TrimbleRouteOptimizer
        SchedulerWarehouse
        SchedulerCarrierAutomation
        NEXTStop
        DAT
        Optimizer
        P44API
    End Enum

#End Region

#Region "Module Management Methods"

    ''' <summary>
    ''' Pass in the modules selected by the user in the UI and use those values to 
    ''' create a bitwise flag to store which modules are turned on/off.
    ''' Generates the number to put in the license file.
    ''' </summary>
    ''' <param name="selected">Contains the bit positions of the modules the user selected as On</param>
    ''' <returns></returns>
    ''' <remarks>Called by the NGLSystemMaint page</remarks>
    Public Function GenerateModuleLicenceKey(ByVal selected As Models.SelectableGridSave) As Long
        Dim flagSource As Long = 0
        Dim bwModules As New BitwiseFlags(flagSource)
        For Each s In selected.BitPositionsOn
            bwModules.turnBitFlagOn(s)
        Next
        Return bwModules.FlagSource
    End Function

    ''' <summary>
    ''' Returns a list of all available modules that we intend to support
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetListOfModules() As Models.SelectableGridItem()
        Dim modulesList As New List(Of Models.SelectableGridItem)
        modulesList.Add(New Models.SelectableGridItem With {.SGItemBitPos = Modules.PCMilerNetworkLicense, .SGItemCaption = "PCMiler Network License", .SGItemOn = 0})
        modulesList.Add(New Models.SelectableGridItem With {.SGItemBitPos = Modules.PCMilerAPILicense, .SGItemCaption = "PCMiler API License", .SGItemOn = 0})
        modulesList.Add(New Models.SelectableGridItem With {.SGItemBitPos = Modules.BingMapsLicense, .SGItemCaption = "Bing Maps", .SGItemOn = 0})
        modulesList.Add(New Models.SelectableGridItem With {.SGItemBitPos = Modules.TrimbleMapsLicense, .SGItemCaption = "Trimble Maps", .SGItemOn = 0})
        modulesList.Add(New Models.SelectableGridItem With {.SGItemBitPos = Modules.TrimbleRouteOptimizer, .SGItemCaption = "Trimble Route Optimizer", .SGItemOn = 0})
        modulesList.Add(New Models.SelectableGridItem With {.SGItemBitPos = Modules.SchedulerWarehouse, .SGItemCaption = "Scheduler - Warehouse", .SGItemOn = 0})
        modulesList.Add(New Models.SelectableGridItem With {.SGItemBitPos = Modules.SchedulerCarrierAutomation, .SGItemCaption = "Scheduler - Carrier Automation", .SGItemOn = 0})
        modulesList.Add(New Models.SelectableGridItem With {.SGItemBitPos = Modules.NEXTStop, .SGItemCaption = "NEXTStop", .SGItemOn = 0})
        modulesList.Add(New Models.SelectableGridItem With {.SGItemBitPos = Modules.DAT, .SGItemCaption = "DAT Integration", .SGItemOn = 0})
        modulesList.Add(New Models.SelectableGridItem With {.SGItemBitPos = Modules.Optimizer, .SGItemCaption = "NGL Optimizer", .SGItemOn = 0})
        modulesList.Add(New Models.SelectableGridItem With {.SGItemBitPos = Modules.P44API, .SGItemCaption = "P44 Integration", .SGItemOn = 0})
        Return modulesList.ToArray()
    End Function

    ''' <summary>
    ''' Entry point in the BLL with the enumerator key which implements supported features
    ''' </summary>
    ''' <param name="licenseKey"></param>
    Public Sub ConfigureModules(ByVal licenseKey As Integer)
        Dim bwModules As New BitwiseFlags(licenseKey)
        TogglePCMilerNetworkLicenseModule(bwModules.isBitFlagOn(Modules.PCMilerNetworkLicense))
        TogglePCMilerAPILicenseModule(bwModules.isBitFlagOn(Modules.PCMilerAPILicense))
        ToggleBingMapsLicenseModule(bwModules.isBitFlagOn(Modules.BingMapsLicense))
        ToggleTrimbleMapsLicenseModule(bwModules.isBitFlagOn(Modules.TrimbleMapsLicense))
        ToggleTrimbleRouteOptimizerModule(bwModules.isBitFlagOn(Modules.TrimbleRouteOptimizer))
        ToggleSchedulerWarehouseModule(bwModules.isBitFlagOn(Modules.SchedulerWarehouse))
        ToggleSchedulerCarrierAutomationModule(bwModules.isBitFlagOn(Modules.SchedulerCarrierAutomation))
        ToggleNEXTStopModule(bwModules.isBitFlagOn(Modules.NEXTStop))
        ToggleDATModule(bwModules.isBitFlagOn(Modules.DAT))
        ToggleOptimizerModule(bwModules.isBitFlagOn(Modules.Optimizer))
        ToggleP44APIModule(bwModules.isBitFlagOn(Modules.P44API))
    End Sub


#Region "PCMilerNetworkLicense Module Methods"

    ''' <summary>
    ''' Calls code to turn the PCMiler Network License Module On/Off based on the bitwise flag value provided by the caller
    ''' </summary>
    ''' <param name="blnOn"></param>
    ''' <returns></returns>
    Public Function TogglePCMilerNetworkLicenseModule(ByVal blnOn As Boolean) As Boolean
        Dim blnRetVal As Boolean = False
        If blnOn Then
            'Execute code to turn on the PCMilerNetworkLicense Module
            PCMilerNetworkLicenseModuleOn()
        Else
            'Execute code to turn off the PCMilerNetworkLicense Module
            PCMilerNetworkLicenseModuleOff()
        End If
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the PCMiler Network License Module On
    ''' </summary>
    ''' <returns></returns>
    Public Function PCMilerNetworkLicenseModuleOn() As Boolean
        Dim blnRetVal As Boolean = False
        Dim ltsParam = NGLParameterData.GetParameter("UsePCMiler")
        ltsParam.ParValue = 1
        ltsParam.ParText = "1"
        Dim res = NGLParameterData.SaveParameter(ltsParam)
        If Not res Is Nothing AndAlso res.ParCategoryControl <> 0 Then blnRetVal = True
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the PCMiler Network License Module Off
    ''' </summary>
    ''' <returns></returns>
    Public Function PCMilerNetworkLicenseModuleOff() As Boolean
        Dim blnRetVal As Boolean = False
        Dim ltsParam = NGLParameterData.GetParameter("UsePCMiler")
        ltsParam.ParValue = 0
        ltsParam.ParText = "0"
        Dim res = NGLParameterData.SaveParameter(ltsParam)
        If Not res Is Nothing AndAlso res.ParCategoryControl <> 0 Then blnRetVal = True
        Return blnRetVal
    End Function

#End Region

#Region "PCMilerAPILicense Module Methods"

    ''' <summary>
    ''' Calls code to turn the PCMiler API License Module On/Off based on the bitwise flag value provided by the caller
    ''' </summary>
    ''' <param name="blnOn"></param>
    ''' <returns></returns>
    Public Function TogglePCMilerAPILicenseModule(ByVal blnOn As Boolean) As Boolean
        Dim blnRetVal As Boolean = False
        If blnOn Then
            'Execute code to turn on the PCMilerAPILicense Module
            PCMilerAPILicenseModuleOn()
        Else
            'Execute code to turn off the PCMilerAPILicense Module
            PCMilerAPILicenseModuleOff()
        End If
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the PCMiler API License Module On
    ''' </summary>
    ''' <returns></returns>
    Public Function PCMilerAPILicenseModuleOn() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the PCMiler API License Module Off
    ''' </summary>
    ''' <returns></returns>
    Public Function PCMilerAPILicenseModuleOff() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

#End Region

#Region "BingMapsLicense Module Methods"

    ''' <summary>
    ''' Calls code to turn the Bing Maps License Module On/Off based on the bitwise flag value provided by the caller
    ''' </summary>
    ''' <param name="blnOn"></param>
    ''' <returns></returns>
    Public Function ToggleBingMapsLicenseModule(ByVal blnOn As Boolean) As Boolean
        Dim blnRetVal As Boolean = False
        If blnOn Then
            'Execute code to turn on the BingMapsLicense Module
            BingMapsLicenseModuleOn()
        Else
            'Execute code to turn off the BingMapsLicense Module
            BingMapsLicenseModuleOff()
        End If
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the Bing Maps License Module On
    ''' </summary>
    ''' <returns></returns>
    Public Function BingMapsLicenseModuleOn() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the Bing Maps License Module Off
    ''' </summary>
    ''' <returns></returns>
    Public Function BingMapsLicenseModuleOff() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

#End Region

#Region "TrimbleMapsLicense Module Methods"

    ''' <summary>
    ''' Calls code to turn the Trimble Maps License Module On/Off based on the bitwise flag value provided by the caller
    ''' </summary>
    ''' <param name="blnOn"></param>
    ''' <returns></returns>
    Public Function ToggleTrimbleMapsLicenseModule(ByVal blnOn As Boolean) As Boolean
        Dim blnRetVal As Boolean = False
        If blnOn Then
            'Execute code to turn on the TrimbleMapsLicense Module
            TrimbleMapsLicenseModuleOn()
        Else
            'Execute code to turn off the TrimbleMapsLicense Module
            TrimbleMapsLicenseModuleOff()
        End If
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the Trimble Maps License Module On
    ''' </summary>
    ''' <returns></returns>
    Public Function TrimbleMapsLicenseModuleOn() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the Trimble Maps License Module Off
    ''' </summary>
    ''' <returns></returns>
    Public Function TrimbleMapsLicenseModuleOff() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

#End Region

#Region "TrimbleRouteOptimizer Module Methods"

    ''' <summary>
    ''' Calls code to turn the Trimble Route Optimizer Module On/Off based on the bitwise flag value provided by the caller
    ''' </summary>
    ''' <param name="blnOn"></param>
    ''' <returns></returns>
    Public Function ToggleTrimbleRouteOptimizerModule(ByVal blnOn As Boolean) As Boolean
        Dim blnRetVal As Boolean = False
        If blnOn Then
            'Execute code to turn on the TrimbleRouteOptimizer Module
            TrimbleRouteOptimizerModuleOn()
        Else
            'Execute code to turn off the TrimbleRouteOptimizer Module
            TrimbleRouteOptimizerModuleOff()
        End If
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the Trimble Route Optimizer Module On
    ''' </summary>
    ''' <returns></returns>
    Public Function TrimbleRouteOptimizerModuleOn() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the Trimble Route Optimizer Module Off
    ''' </summary>
    ''' <returns></returns>
    Public Function TrimbleRouteOptimizerModuleOff() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

#End Region

#Region "SchedulerWarehouse Module Methods"

    ''' <summary>
    ''' Calls code to turn the Scheduler Warehouse Module On/Off based on the bitwise flag value provided by the caller
    ''' </summary>
    ''' <param name="blnOn"></param>
    ''' <returns></returns>
    Public Function ToggleSchedulerWarehouseModule(ByVal blnOn As Boolean) As Boolean
        Dim blnRetVal As Boolean = False
        If blnOn Then
            'Execute code to turn on the SchedulerWarehouse Module
            SchedulerWarehouseModuleOn()
        Else
            'Execute code to turn off the SchedulerWarehouse Module
            SchedulerWarehouseModuleOff()
        End If
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the Scheduler Warehouse Module On
    ''' </summary>
    ''' <returns></returns>
    Public Function SchedulerWarehouseModuleOn() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the Scheduler Warehouse Module Off
    ''' </summary>
    ''' <returns></returns>
    Public Function SchedulerWarehouseModuleOff() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

#End Region

#Region "SchedulerCarrierAutomation Module Methods"

    ''' <summary>
    ''' Calls code to turn the Scheduler Carrier Automation Module On/Off based on the bitwise flag value provided by the caller
    ''' </summary>
    ''' <param name="blnOn"></param>
    ''' <returns></returns>
    Public Function ToggleSchedulerCarrierAutomationModule(ByVal blnOn As Boolean) As Boolean
        Dim blnRetVal As Boolean = False
        If blnOn Then
            'Execute code to turn on the SchedulerCarrierAutomation Module
            SchedulerCarrierAutomationModuleOn()
        Else
            'Execute code to turn off the SchedulerCarrierAutomation Module
            SchedulerCarrierAutomationModuleOff()
        End If
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the Scheduler Carrier Automation Module On
    ''' </summary>
    ''' <returns></returns>
    Public Function SchedulerCarrierAutomationModuleOn() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the Scheduler Carrier Automation Module Off
    ''' </summary>
    ''' <returns></returns>
    Public Function SchedulerCarrierAutomationModuleOff() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

#End Region

#Region "NEXTStop Module Methods"

    ''' <summary>
    ''' Calls code to turn the NEXTStop Module On/Off based on the bitwise flag value provided by the caller
    ''' </summary>
    ''' <param name="blnOn"></param>
    ''' <returns></returns>
    Public Function ToggleNEXTStopModule(ByVal blnOn As Boolean) As Boolean
        Dim blnRetVal As Boolean = False
        If blnOn Then
            'Execute code to turn on the NEXTStop Module
            NEXTStopModuleOn()
        Else
            'Execute code to turn off the NEXTStop Module
            NEXTStopModuleOff()
        End If
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the NEXTStop Module On
    ''' </summary>
    ''' <returns></returns>
    Public Function NEXTStopModuleOn() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the NEXTStop Module Off
    ''' </summary>
    ''' <returns></returns>
    Public Function NEXTStopModuleOff() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

#End Region

#Region "DAT Module Methods"

    ''' <summary>
    ''' Calls code to turn the DAT Module On/Off based on the bitwise flag value provided by the caller
    ''' </summary>
    ''' <param name="blnOn"></param>
    ''' <returns></returns>
    Public Function ToggleDATModule(ByVal blnOn As Boolean) As Boolean
        Dim blnRetVal As Boolean = False
        If blnOn Then
            'Execute code to turn on the DAT Module
            DATModuleOn()
        Else
            'Execute code to turn off the DAT Module
            DATModuleOff()
        End If
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the DAT Module On
    ''' </summary>
    ''' <returns></returns>
    Public Function DATModuleOn() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the DAT Module Off
    ''' </summary>
    ''' <returns></returns>
    Public Function DATModuleOff() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

#End Region

#Region "Optimizer Module Methods"

    ''' <summary>
    ''' Calls code to turn the Optimizer Module On/Off based on the bitwise flag value provided by the caller
    ''' </summary>
    ''' <param name="blnOn"></param>
    ''' <returns></returns>
    Public Function ToggleOptimizerModule(ByVal blnOn As Boolean) As Boolean
        Dim blnRetVal As Boolean = False
        If blnOn Then
            'Execute code to turn on the Optimizer Module
            OptimizerModuleOn()
        Else
            'Execute code to turn off the Optimizer Module
            OptimizerModuleOff()
        End If
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the Optimizer Module On
    ''' </summary>
    ''' <returns></returns>
    Public Function OptimizerModuleOn() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the Optimizer Module Off
    ''' </summary>
    ''' <returns></returns>
    Public Function OptimizerModuleOff() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

#End Region

#Region "P44API Module Methods"

    ''' <summary>
    ''' Calls code to turn the P44 API Module On/Off based on the bitwise flag value provided by the caller
    ''' </summary>
    ''' <param name="blnOn"></param>
    ''' <returns></returns>
    Public Function ToggleP44APIModule(ByVal blnOn As Boolean) As Boolean
        Dim blnRetVal As Boolean = False
        If blnOn Then
            'Execute code to turn on the P44API Module
            P44APIModuleOn()
        Else
            'Execute code to turn off the P44API Module
            P44APIModuleOff()
        End If
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the P44 API Module On
    ''' </summary>
    ''' <returns></returns>
    Public Function P44APIModuleOn() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

    ''' <summary>
    ''' Code to turn the P44 API Module Off
    ''' </summary>
    ''' <returns></returns>
    Public Function P44APIModuleOff() As Boolean
        Dim blnRetVal As Boolean = False
        Return blnRetVal
    End Function

#End Region

#End Region

    ''' <summary>
    ''' Loop through each page deployed on the local system and use the PageControl to contact the REST service. 
    ''' Then update the local cmPage table with the new message
    ''' </summary>
    Public Sub DownloadPageFooterMessages()
        Dim oPG As New DAL.NGLcmPageData(Parameters)
        Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
        Dim oNGLRest = New NGLREST.PageFooterMsg()

        Dim pages = oPG.GetAll() 'get a list of pages deployed on the local system

        Dim url = NGLParameterData.GetParText("GlobalPgFooterRestUrl")
        If String.IsNullOrWhiteSpace(url) Then Return

        'loop through each page deployed on the local system
        For Each p In pages
            Dim strPgFooterMsg = oNGLRest.CallPgFooterREST(url, p.PageControl) 'use the PageControl to contact the REST service
            If Not String.IsNullOrEmpty(strPgFooterMsg) AndAlso strPgFooterMsg.Length >= 7 AndAlso strPgFooterMsg.Substring(0, 7) = "!Error!" Then
                'There was an error so log it and move on
                Dim strErr = strPgFooterMsg.Substring(7)
                Dim strAppErr = GetPgFooterErrorMsg(oLocalize, p.PageControl, strErr)
                DAL.Utilities.SaveAppError(strAppErr, Me.Parameters)
            Else
                Try
                    'update the local cmPage table with the new message
                    If Not oPG.SavePgFooterMsg(p.PageControl, strPgFooterMsg) Then
                        Dim strAppErr = GetPgFooterErrorMsg(oLocalize, p.PageControl, oLocalize.GetLocalizedValueByKey("SaveIncompleteMessage", "Save was unable to complete"))
                        DAL.Utilities.SaveAppError(strAppErr, Me.Parameters)
                    End If
                Catch ex As Exception
                    Dim strAppErr = GetPgFooterErrorMsg(oLocalize, p.PageControl, ex.Message)
                    DAL.Utilities.SaveAppError(strAppErr, Me.Parameters)
                End Try
            End If
        Next
    End Sub

    Public Function GetPgFooterErrorMsg(ByRef oLocalize As DAL.NGLcmLocalizeKeyValuePairData, ByVal PageControl As Integer, ByVal strErrMsg As String) As String
        Dim strRet As String = ""
        Dim strFormat = oLocalize.GetLocalizedValueByKey("E_UpdatePgFooterMsg", "Could not update the Page Footer Message for PageControl: {0}. The actual error message is: {1}")
        strRet = String.Format(strFormat, PageControl.ToString(), strErrMsg)
        Return strRet
    End Function


End Class