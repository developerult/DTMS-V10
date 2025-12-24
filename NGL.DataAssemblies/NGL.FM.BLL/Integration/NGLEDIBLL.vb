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

Public Class NGLEDIBLL : Inherits BLLBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLEDIBLL"
    End Sub

#End Region



    ''' <summary>
    ''' Gets the CarrierEDI record for the associated SHID and EDIXAction.
    ''' Uses the SHID to look up the CarrierControl and gets the CarrierEDI
    ''' record using the CarrierControl and EDIXAction as filters
    ''' </summary>
    ''' <param name="SHID"></param>
    ''' <param name="EDIXAction"></param>
    ''' <remarks>
    ''' Added by LVV on 4/12/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Function getCarrierEDIBySHID(ByVal SHID As String, ByVal EDIXAction As String) As DTO.CarrierEDI()
        Dim books = NGLBookData.GetBooksBySHID(SHID)
        Dim carrierControl = books(0).BookCarrierControl
        Dim carrierEDI = NGLCarrierEDIData.GetCarrierEDIsFiltered(carrierControl, EDIXAction)
        Return carrierEDI

    End Function

    '''' <summary>
    '''' Parameter is either a CompNumber or CompAlphaCode. First calls GetCompNumberByAlpha() which returns the CompNumber
    '''' based on the parameter. Then calls GetCompFiltered by CompNumber to get the rest of the company info.
    '''' Returns nothing if the CompanyNumber does not exist
    '''' </summary>
    '''' <param name="N104"></param>
    '''' <returns></returns>
    '''' <remarks>
    '''' Removed by RHR for v-7.0.6.105 we moved this method into the integration assembly
    '''' </remarks>
    ''Public Function getEDI204InCompInfo(ByVal N104 As String) As DTO.Comp
    ''    Dim compNumber = NGLCompData.GetCompNumberByAlpha(N104)
    ''    If compNumber <> 0 Then
    ''        Dim comp = NGLCompData.GetCompFiltered(Number:=compNumber)
    ''        Return comp
    ''    Else
    ''        Return Nothing
    ''    End If
    ''End Function



End Class
