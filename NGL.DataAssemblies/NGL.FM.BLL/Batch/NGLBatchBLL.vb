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


Public Class NGLBatchBLL : Inherits BLLBaseClass


#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLBatchBLL"
    End Sub

#End Region

#Region " Properties "

#End Region

#Region "DAL Wrapper Methods"


    Public Sub AssignTruckStopCarrier(ByVal StopName As String, _
                                   ByVal ID1 As String, _
                                   ByVal ID2 As String, _
                                   ByVal TruckID As String, _
                                   ByVal SeqNbr As Integer, _
                                   ByVal DistToPrev As Double, _
                                   ByVal TotalRouteCost As Double, _
                                   ByVal ConsNumber As String)

        BookBLL.AssignTruckStopCarrier(StopName, ID1, ID2, TruckID, SeqNbr, DistToPrev, TotalRouteCost, ConsNumber)
    End Sub

    Public Function GettblBatchProcessRunnings(ByVal UserName As String, _
                                              Optional ByVal page As Integer = 1, _
                                           Optional ByVal pagesize As Integer = 1000) As IList(Of DTO.tblBatchProcessRunning)
        Return NGLSystemData.GettblBatchProcessRunningsByTitleGroup(UserName, Utilities.BATCHPROCESSTITLE, page, pagesize)
    End Function

    Public Function GettblBatchProcessRunning(ByVal UserName As String, ByVal processName As String) As DTO.tblBatchProcessRunning
        Return NGLSystemData.GettblBatchProcessRunning(UserName, processName)
    End Function
#End Region

End Class
