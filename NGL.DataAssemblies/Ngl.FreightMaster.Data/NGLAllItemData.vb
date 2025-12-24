Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Public Class NGLAllItemData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.spNGLLOADRecord(0)
        Me.LinqDB = db
        Me.SourceClass = "NGLAllItemData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.spNGLLOADRecord(0)
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetAllItem(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetAllItem(ByVal Control As Integer) As DataTransferObjects.AllItem

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the page data
                Dim oItem As DataTransferObjects.AllItem = (From d In db.spNGLLOADRecord(Control)
                        Select New DataTransferObjects.AllItem With {.Control = d.LOADBookControl,
                        .ProNumber = d.LOADProNumber,
                        .CnsNumber = d.LOADCONSPREFIX,
                        .StopNumber = d.LOADStopNo,
                        .BookPickupStopNumber = d.BookPickupStopNumber,
                        .PurchaseOrderNumber = d.LOADPO,
                        .OrderNumber = d.LOADOrderNumber,
                        .ScheduledToLoad = d.LOADDATE,
                        .RequestedToArrive = d.SCHEDULEDATE,
                        .AssignedCarrier = d.LOADCarrierName,
                        .DestinationName = d.LOADDestName,
                        .DestinationCity = d.LOADDestCity,
                        .DestinationState = d.LOADDestState,
                        .CarrierData = New DataTransferObjects.BookCarrier With {.BookCarrScheduleDate = d.LOADToLoadDate,
                        .BookCarrScheduleTime = d.LOADScheduleTime,
                        .BookCarrActualDate = d.LOADActPickupDate,
                        .BookCarrActualTime = d.LOADActPickupTime,
                        .BookCarrStartLoadingDate = d.LOADStartLoadingDate,
                        .BookCarrStartLoadingTime = d.LOADStartLoadingTime,
                        .BookCarrFinishLoadingDate = d.LOADFinishLoadingDate,
                        .BookCarrFinishLoadingTime = d.LOADFinishLoadingTime,
                        .BookCarrActLoadComplete_Date = d.LOADActLoadComplete_Date,
                        .BookCarrActLoadCompleteTime = d.LOADActLoadCompleteTime,
                        .BookCarrDockPUAssigment = d.LOADDockPUAssigment,
                        .BookCarrApptDate = d.LOADDelScheduleDate,
                        .BookCarrApptTime = d.LOADDelScheduleTime,
                        .BookCarrActDate = d.LOADActDelDate,
                        .BookCarrActTime = d.LOADActDelTime,
                        .BookCarrStartUnloadingDate = d.LOADDelStartUnloadingDate,
                        .BookCarrStartUnloadingTime = d.LOADDelStartUnloadingTime,
                        .BookCarrFinishUnloadingDate = d.LOADDelFinishUnloadingDate,
                        .BookCarrFinishUnloadingTime = d.LOADDelFinishUnloadingTime,
                        .BookCarrActUnloadCompDate = d.LOADActLoadCompDate,
                        .BookCarrActUnloadCompTime = d.LOADActLoadCompTime,
                        .BookCarrDockDelAssignment = d.LOADDelDock,
                        .BookCarrTrailerNo = d.LOADTrailerNo,
                        .BookCarrSealNo = d.LOADSealNo,
                        .BookCarrDriverNo = d.LOADDriverNo,
                        .BookCarrDriverName = d.LOADDriverName,
                        .BookCarrTripNo = d.LOADTripNo,
                        .BookCarrRouteNo = d.LOADRouteNo,
                        .BookWhseAuthorizationNo = d.LOADWhseAuthorizationNo},
                        .Comments = d.Comments,
                        .BookNotes1 = d.BookNotesVisable1,
                        .BookNotes2 = d.BookNotesVisable2,
                        .BookNotes3 = d.BookNotesVisable3,
                        .AssignedProNumber = d.BookShipCarrierProNumber,
                        .BookShipCarrierProNumberRaw = d.BookShipCarrierProNumberRaw,
                        .BookShipCarrierProControl = d.BookShipCarrierProControl,
                        .AssignedCarrierName = d.BookShipCarrierName,
                        .AssignedCarrierNumber = d.BookShipCarrierNumber,
                        .AssignedCarrierContact = d.BookCarrierContact,
                        .AssignedCarrierContactPhone = d.BookCarrierContactPhone,
                        .BookModDate = d.BookModDate,
                        .BookModUser = d.BookModUser,
                        .BookAMSPickupApptControl = d.BookAMSPickupApptControl,
                        .BookAMSDeliveryApptControl = d.BookAMSDeliveryApptControl,
                        .BookLoadControl = d.BookLoadControl}).First

                Return oItem

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetAllItems(ByVal proNumber As String,
                                ByVal CNS As String,
                                ByVal PO As String,
                                ByVal OrderNumber As String,
                                ByVal LoadDate As Nullable(Of DateTime),
                                ByVal LoadDateTo As Nullable(Of DateTime),
                                ByVal SCHEDULEDATE As Nullable(Of DateTime),
                                ByVal SCHEDULEDATETo As Nullable(Of DateTime),
                                ByVal LOADDelScheduleDate As Nullable(Of DateTime),
                                ByVal LOADDelScheduleDateTo As Nullable(Of DateTime),
                                ByVal LOADDelScheduleTime As String,
                                ByVal LOADActDelDate As Nullable(Of DateTime),
                                ByVal LOADActDelDateTo As Nullable(Of DateTime),
                                ByVal LOADCARRIERNAME As String,
                                ByVal LOADCARRIERNUMBER As Nullable(Of Integer),
                                ByVal LOADDESTNAME As String,
                                ByVal LOADDESTCITY As String,
                                ByVal LOADDESTSTATE As String,
                                ByVal LOADBROKERNUMBER As String,
                                ByVal LOADCARRIERCONTCONTROL As Nullable(Of Integer),
                                ByVal UseCarrierFilters As Nullable(Of Boolean),
                                ByVal DaysOut As Nullable(Of Integer),
                                ByVal DelDaysOut As Nullable(Of Integer),
                                ByVal xmlTransCode As String,
                                ByVal xmlLoadCompanyIDs As String,
                                ByVal xmlLoadLanes As String,
                                ByVal p_sortordinal As String,
                                ByVal p_sortdirection As String,
                                ByVal p_datefilterfield As String,
                                ByVal p_datefilterfrom As Nullable(Of DateTime),
                                ByVal p_datefilterto As Nullable(Of DateTime),
                                Optional ByVal page As Integer = 1,
                                Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.AllItem()
        If pagesize < 1 Then pagesize = 1
        If page < 1 Then page = 1
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim xmlXmlTransCode As XElement = If(String.IsNullOrEmpty(xmlTransCode), Nothing, XElement.Parse(xmlTransCode))
                Dim xmlXmlLoadCompanyIDs As XElement = If(String.IsNullOrEmpty(xmlLoadCompanyIDs), Nothing, XElement.Parse(xmlLoadCompanyIDs))
                Dim xmlXmlLoadLanes As XElement = If(String.IsNullOrEmpty(xmlLoadLanes), Nothing, XElement.Parse(xmlLoadLanes))
                'Get the page data
                Dim oList() As DataTransferObjects.AllItem = (From d In db.spNGLLOAD_SORTEDWPages_New(proNumber,
                                                                                      CNS,
                                                                                      PO,
                                                                                      OrderNumber,
                                                                                      LoadDate,
                                                                                      LoadDateTo,
                                                                                      SCHEDULEDATE,
                                                                                      SCHEDULEDATETo,
                                                                                      LOADDelScheduleDate,
                                                                                      LOADDelScheduleDateTo,
                                                                                      LOADDelScheduleTime,
                                                                                      LOADActDelDate,
                                                                                      LOADActDelDateTo,
                                                                                      LOADCARRIERNAME,
                                                                                      LOADCARRIERNUMBER,
                                                                                      LOADDESTNAME,
                                                                                      LOADDESTCITY,
                                                                                      LOADDESTSTATE,
                                                                                      LOADBROKERNUMBER,
                                                                                      LOADCARRIERCONTCONTROL,
                                                                                      UseCarrierFilters,
                                                                                      DaysOut,
                                                                                      DelDaysOut,
                                                                                      xmlXmlTransCode,
                                                                                      xmlXmlLoadCompanyIDs,
                                                                                      xmlXmlLoadLanes,
                                                                                      p_sortordinal,
                                                                                      p_sortdirection,
                                                                                      p_datefilterfield,
                                                                                      p_datefilterfrom,
                                                                                      p_datefilterto,
                                                                                      page,
                                                                                      pagesize)
                        Select New DataTransferObjects.AllItem With {.Control = d.LOADBookControl,
                        .ProNumber = d.LOADProNumber,
                        .CnsNumber = d.LOADCONSPREFIX,
                        .StopNumber = d.LOADStopNo,
                        .BookPickupStopNumber = d.BookPickupStopNumber,
                        .PurchaseOrderNumber = d.LOADPO,
                        .OrderNumber = d.LOADOrderNumber,
                        .ScheduledToLoad = d.LOADDATE,
                        .RequestedToArrive = d.SCHEDULEDATE,
                        .AssignedCarrier = d.LOADCarrierName,
                        .DestinationName = d.LOADDestName,
                        .DestinationCity = d.LOADDestCity,
                        .DestinationState = d.LOADDestState,
                        .CarrierData = New DataTransferObjects.BookCarrier With {.BookCarrScheduleDate = d.LOADToLoadDate,
                        .BookCarrScheduleTime = d.LOADScheduleTime,
                        .BookCarrActualDate = d.LOADActPickupDate,
                        .BookCarrActualTime = d.LOADActPickupTime,
                        .BookCarrStartLoadingDate = d.LOADStartLoadingDate,
                        .BookCarrStartLoadingTime = d.LOADStartLoadingTime,
                        .BookCarrFinishLoadingDate = d.LOADFinishLoadingDate,
                        .BookCarrFinishLoadingTime = d.LOADFinishLoadingTime,
                        .BookCarrActLoadComplete_Date = d.LOADActLoadComplete_Date,
                        .BookCarrActLoadCompleteTime = d.LOADActLoadCompleteTime,
                        .BookCarrDockPUAssigment = d.LOADDockPUAssigment,
                        .BookCarrApptDate = d.LOADDelScheduleDate,
                        .BookCarrApptTime = d.LOADDelScheduleTime,
                        .BookCarrActDate = d.LOADActDelDate,
                        .BookCarrActTime = d.LOADActDelTime,
                        .BookCarrStartUnloadingDate = d.LOADDelStartUnloadingDate,
                        .BookCarrStartUnloadingTime = d.LOADDelStartUnloadingTime,
                        .BookCarrFinishUnloadingDate = d.LOADDelFinishUnloadingDate,
                        .BookCarrFinishUnloadingTime = d.LOADDelFinishUnloadingTime,
                        .BookCarrActUnloadCompDate = d.LOADActLoadCompDate,
                        .BookCarrActUnloadCompTime = d.LOADActLoadCompTime,
                        .BookCarrDockDelAssignment = d.LOADDelDock,
                        .BookCarrTrailerNo = d.LOADTrailerNo,
                        .BookCarrSealNo = d.LOADSealNo,
                        .BookCarrDriverNo = d.LOADDriverNo,
                        .BookCarrDriverName = d.LOADDriverName,
                        .BookCarrTripNo = d.LOADTripNo,
                        .BookCarrRouteNo = d.LOADRouteNo,
                        .BookWhseAuthorizationNo = d.LOADWhseAuthorizationNo},
                        .Comments = d.Comments,
                        .BookNotes1 = d.BookNotesVisable1,
                        .BookNotes2 = d.BookNotesVisable2,
                        .BookNotes3 = d.BookNotesVisable3,
                        .AssignedProNumber = d.BookShipCarrierProNumber,
                        .BookShipCarrierProNumberRaw = d.BookShipCarrierProNumberRaw,
                        .BookShipCarrierProControl = d.BookShipCarrierProControl,
                        .AssignedCarrierName = d.BookShipCarrierName,
                        .AssignedCarrierNumber = d.BookShipCarrierNumber,
                        .AssignedCarrierContact = d.BookCarrierContact,
                        .AssignedCarrierContactPhone = d.BookCarrierContactPhone,
                        .BookModDate = d.BookModDate,
                        .BookModUser = d.BookModUser,
                        .BookAMSPickupApptControl = If(d.BookAMSPickupApptControl.HasValue, d.BookAMSPickupApptControl.Value, 0),
                        .BookAMSDeliveryApptControl = If(d.BookAMSDeliveryApptControl.HasValue, d.BookAMSDeliveryApptControl.Value, 0),
                        .BookLoadControl = d.BookLoadControl,
                        .Page = page,
                        .Pages = d.Pages,
                        .RecordCount = d.RecordCount}).ToArray()

                Return oList

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' GetAllItems7052
    ''' </summary>
    ''' <param name="proNumber"></param>
    ''' <param name="CNS"></param>
    ''' <param name="PO"></param>
    ''' <param name="OrderNumber"></param>
    ''' <param name="bookSHID"></param>
    ''' <param name="bookShipCarrierProNumber"></param>
    ''' <param name="LoadDate"></param>
    ''' <param name="LoadDateTo"></param>
    ''' <param name="SCHEDULEDATE"></param>
    ''' <param name="SCHEDULEDATETo"></param>
    ''' <param name="LOADDelScheduleDate"></param>
    ''' <param name="LOADDelScheduleDateTo"></param>
    ''' <param name="LOADDelScheduleTime"></param>
    ''' <param name="LOADActDelDate"></param>
    ''' <param name="LOADActDelDateTo"></param>
    ''' <param name="LOADCARRIERNAME"></param>
    ''' <param name="LOADCARRIERNUMBER"></param>
    ''' <param name="LOADDESTNAME"></param>
    ''' <param name="LOADDESTCITY"></param>
    ''' <param name="LOADDESTSTATE"></param>
    ''' <param name="LOADBROKERNUMBER"></param>
    ''' <param name="LOADCARRIERCONTCONTROL"></param>
    ''' <param name="UseCarrierFilters"></param>
    ''' <param name="DaysOut"></param>
    ''' <param name="DelDaysOut"></param>
    ''' <param name="xmlTransCode"></param>
    ''' <param name="xmlLoadCompanyIDs"></param>
    ''' <param name="xmlLoadLanes"></param>
    ''' <param name="p_sortordinal"></param>
    ''' <param name="p_sortdirection"></param>
    ''' <param name="p_datefilterfield"></param>
    ''' <param name="p_datefilterfrom"></param>
    ''' <param name="p_datefilterto"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 7/29/16 for v-7.0.5.110 Task #14 NxT Search Filters
    ''' </remarks>
    Public Function GetAllItems7052(ByVal proNumber As String,
                                    ByVal CNS As String,
                                    ByVal PO As String,
                                    ByVal OrderNumber As String,
                                    ByVal bookSHID As String,
                                    ByVal bookShipCarrierProNumber As String,
                                    ByVal LoadDate As Nullable(Of DateTime),
                                    ByVal LoadDateTo As Nullable(Of DateTime),
                                    ByVal SCHEDULEDATE As Nullable(Of DateTime),
                                    ByVal SCHEDULEDATETo As Nullable(Of DateTime),
                                    ByVal LOADDelScheduleDate As Nullable(Of DateTime),
                                    ByVal LOADDelScheduleDateTo As Nullable(Of DateTime),
                                    ByVal LOADDelScheduleTime As String,
                                    ByVal LOADActDelDate As Nullable(Of DateTime),
                                    ByVal LOADActDelDateTo As Nullable(Of DateTime),
                                    ByVal LOADCARRIERNAME As String,
                                    ByVal LOADCARRIERNUMBER As Nullable(Of Integer),
                                    ByVal LOADDESTNAME As String,
                                    ByVal LOADDESTCITY As String,
                                    ByVal LOADDESTSTATE As String,
                                    ByVal LOADBROKERNUMBER As String,
                                    ByVal LOADCARRIERCONTCONTROL As Nullable(Of Integer),
                                    ByVal UseCarrierFilters As Nullable(Of Boolean),
                                    ByVal DaysOut As Nullable(Of Integer),
                                    ByVal DelDaysOut As Nullable(Of Integer),
                                    ByVal xmlTransCode As String,
                                    ByVal xmlLoadCompanyIDs As String,
                                    ByVal xmlLoadLanes As String,
                                    ByVal p_sortordinal As String,
                                    ByVal p_sortdirection As String,
                                    ByVal p_datefilterfield As String,
                                    ByVal p_datefilterfrom As Nullable(Of DateTime),
                                    ByVal p_datefilterto As Nullable(Of DateTime),
                                    Optional ByVal page As Integer = 1,
                                    Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.AllItem()
        If pagesize < 1 Then pagesize = 1
        If page < 1 Then page = 1
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim xmlXmlTransCode As XElement = If(String.IsNullOrEmpty(xmlTransCode), Nothing, XElement.Parse(xmlTransCode))
                Dim xmlXmlLoadCompanyIDs As XElement = If(String.IsNullOrEmpty(xmlLoadCompanyIDs), Nothing, XElement.Parse(xmlLoadCompanyIDs))
                Dim xmlXmlLoadLanes As XElement = If(String.IsNullOrEmpty(xmlLoadLanes), Nothing, XElement.Parse(xmlLoadLanes))
                'Get the page data
                Dim oList() As DataTransferObjects.AllItem = (From d In db.spNGLLOAD_SORTEDWPages_New7052(proNumber,
                                                                                          CNS,
                                                                                          PO,
                                                                                          OrderNumber,
                                                                                          bookSHID,
                                                                                          bookShipCarrierProNumber,
                                                                                          LoadDate,
                                                                                          LoadDateTo,
                                                                                          SCHEDULEDATE,
                                                                                          SCHEDULEDATETo,
                                                                                          LOADDelScheduleDate,
                                                                                          LOADDelScheduleDateTo,
                                                                                          LOADDelScheduleTime,
                                                                                          LOADActDelDate,
                                                                                          LOADActDelDateTo,
                                                                                          LOADCARRIERNAME,
                                                                                          LOADCARRIERNUMBER,
                                                                                          LOADDESTNAME,
                                                                                          LOADDESTCITY,
                                                                                          LOADDESTSTATE,
                                                                                          LOADBROKERNUMBER,
                                                                                          LOADCARRIERCONTCONTROL,
                                                                                          UseCarrierFilters,
                                                                                          DaysOut,
                                                                                          DelDaysOut,
                                                                                          xmlXmlTransCode,
                                                                                          xmlXmlLoadCompanyIDs,
                                                                                          xmlXmlLoadLanes,
                                                                                          p_sortordinal,
                                                                                          p_sortdirection,
                                                                                          p_datefilterfield,
                                                                                          p_datefilterfrom,
                                                                                          p_datefilterto,
                                                                                          page,
                                                                                          pagesize)
                        Select New DataTransferObjects.AllItem With {.Control = d.LOADBookControl,
                        .ProNumber = d.LOADProNumber,
                        .CnsNumber = d.LOADCONSPREFIX,
                        .StopNumber = d.LOADStopNo,
                        .BookPickupStopNumber = d.BookPickupStopNumber,
                        .PurchaseOrderNumber = d.LOADPO,
                        .OrderNumber = d.LOADOrderNumber,
                        .SHID = d.BookSHID,
                        .CarrierPro = d.BookShipCarrierProNumber,
                        .ScheduledToLoad = d.LOADDATE,
                        .RequestedToArrive = d.SCHEDULEDATE,
                        .AssignedCarrier = d.LOADCarrierName,
                        .DestinationName = d.LOADDestName,
                        .DestinationCity = d.LOADDestCity,
                        .DestinationState = d.LOADDestState,
                        .CarrierData = New DataTransferObjects.BookCarrier With {.BookCarrScheduleDate = d.LOADToLoadDate,
                        .BookCarrScheduleTime = d.LOADScheduleTime,
                        .BookCarrActualDate = d.LOADActPickupDate,
                        .BookCarrActualTime = d.LOADActPickupTime,
                        .BookCarrStartLoadingDate = d.LOADStartLoadingDate,
                        .BookCarrStartLoadingTime = d.LOADStartLoadingTime,
                        .BookCarrFinishLoadingDate = d.LOADFinishLoadingDate,
                        .BookCarrFinishLoadingTime = d.LOADFinishLoadingTime,
                        .BookCarrActLoadComplete_Date = d.LOADActLoadComplete_Date,
                        .BookCarrActLoadCompleteTime = d.LOADActLoadCompleteTime,
                        .BookCarrDockPUAssigment = d.LOADDockPUAssigment,
                        .BookCarrApptDate = d.LOADDelScheduleDate,
                        .BookCarrApptTime = d.LOADDelScheduleTime,
                        .BookCarrActDate = d.LOADActDelDate,
                        .BookCarrActTime = d.LOADActDelTime,
                        .BookCarrStartUnloadingDate = d.LOADDelStartUnloadingDate,
                        .BookCarrStartUnloadingTime = d.LOADDelStartUnloadingTime,
                        .BookCarrFinishUnloadingDate = d.LOADDelFinishUnloadingDate,
                        .BookCarrFinishUnloadingTime = d.LOADDelFinishUnloadingTime,
                        .BookCarrActUnloadCompDate = d.LOADActLoadCompDate,
                        .BookCarrActUnloadCompTime = d.LOADActLoadCompTime,
                        .BookCarrDockDelAssignment = d.LOADDelDock,
                        .BookCarrTrailerNo = d.LOADTrailerNo,
                        .BookCarrSealNo = d.LOADSealNo,
                        .BookCarrDriverNo = d.LOADDriverNo,
                        .BookCarrDriverName = d.LOADDriverName,
                        .BookCarrTripNo = d.LOADTripNo,
                        .BookCarrRouteNo = d.LOADRouteNo,
                        .BookWhseAuthorizationNo = d.LOADWhseAuthorizationNo},
                        .Comments = d.Comments,
                        .BookNotes1 = d.BookNotesVisable1,
                        .BookNotes2 = d.BookNotesVisable2,
                        .BookNotes3 = d.BookNotesVisable3,
                        .AssignedProNumber = d.BookShipCarrierProNumber,
                        .BookShipCarrierProNumberRaw = d.BookShipCarrierProNumberRaw,
                        .BookShipCarrierProControl = d.BookShipCarrierProControl,
                        .AssignedCarrierName = d.BookShipCarrierName,
                        .AssignedCarrierNumber = d.BookShipCarrierNumber,
                        .AssignedCarrierContact = d.BookCarrierContact,
                        .AssignedCarrierContactPhone = d.BookCarrierContactPhone,
                        .BookModDate = d.BookModDate,
                        .BookModUser = d.BookModUser,
                        .BookAMSPickupApptControl = If(d.BookAMSPickupApptControl.HasValue, d.BookAMSPickupApptControl.Value, 0),
                        .BookAMSDeliveryApptControl = If(d.BookAMSDeliveryApptControl.HasValue, d.BookAMSDeliveryApptControl.Value, 0),
                        .BookLoadControl = d.BookLoadControl,
                        .Page = page,
                        .Pages = d.Pages,
                        .RecordCount = d.RecordCount}).ToArray()

                Return oList

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAllItems7052"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetAllItems365(ByVal proNumber As String,
                                   ByVal CNS As String,
                                   ByVal PO As String,
                                   ByVal OrderNumber As String,
                                   ByVal bookSHID As String,
                                   ByVal bookShipCarrierProNumber As String,
                                   ByVal LoadDate As Nullable(Of DateTime),
                                   ByVal LoadDateTo As Nullable(Of DateTime),
                                   ByVal SCHEDULEDATE As Nullable(Of DateTime),
                                   ByVal SCHEDULEDATETo As Nullable(Of DateTime),
                                   ByVal LOADDelScheduleDate As Nullable(Of DateTime),
                                   ByVal LOADDelScheduleDateTo As Nullable(Of DateTime),
                                   ByVal LOADDelScheduleTime As String,
                                   ByVal LOADActDelDate As Nullable(Of DateTime),
                                   ByVal LOADActDelDateTo As Nullable(Of DateTime),
                                   ByVal LOADCARRIERNAME As String,
                                   ByVal LOADCARRIERNUMBER As Nullable(Of Integer),
                                   ByVal LOADDESTNAME As String,
                                   ByVal LOADDESTCITY As String,
                                   ByVal LOADDESTSTATE As String,
                                   ByVal LOADBROKERNUMBER As String,
                                   ByVal LOADCARRIERCONTCONTROL As Nullable(Of Integer),
                                   ByVal UseCarrierFilters As Nullable(Of Boolean),
                                   ByVal DaysOut As Nullable(Of Integer),
                                   ByVal DelDaysOut As Nullable(Of Integer),
                                   ByVal xmlTransCode As String,
                                   ByVal xmlLoadCompanyIDs As String,
                                   ByVal xmlLoadLanes As String,
                                   ByVal p_sortordinal As String,
                                   ByVal p_sortdirection As String,
                                   ByVal p_datefilterfield As String,
                                   ByVal p_datefilterfrom As Nullable(Of DateTime),
                                   ByVal p_datefilterto As Nullable(Of DateTime),
                                   Optional ByVal page As Integer = 1,
                                   Optional ByVal pagesize As Integer = 1000) As LTS.spNGLLOAD_SORTEDWPages_New7052Result()
        If pagesize < 1 Then pagesize = 1
        If page < 1 Then page = 1
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim xmlXmlTransCode As XElement = If(String.IsNullOrEmpty(xmlTransCode), Nothing, XElement.Parse(xmlTransCode))
                Dim xmlXmlLoadCompanyIDs As XElement = If(String.IsNullOrEmpty(xmlLoadCompanyIDs), Nothing, XElement.Parse(xmlLoadCompanyIDs))
                Dim xmlXmlLoadLanes As XElement = If(String.IsNullOrEmpty(xmlLoadLanes), Nothing, XElement.Parse(xmlLoadLanes))
                'Get the page data
                Return db.spNGLLOAD_SORTEDWPages_New7052(proNumber,
                                                         CNS,
                                                         PO,
                                                         OrderNumber,
                                                         bookSHID,
                                                         bookShipCarrierProNumber,
                                                         LoadDate,
                                                         LoadDateTo,
                                                         SCHEDULEDATE,
                                                         SCHEDULEDATETo,
                                                         LOADDelScheduleDate,
                                                         LOADDelScheduleDateTo,
                                                         LOADDelScheduleTime,
                                                         LOADActDelDate,
                                                         LOADActDelDateTo,
                                                         LOADCARRIERNAME,
                                                         LOADCARRIERNUMBER,
                                                         LOADDESTNAME,
                                                         LOADDESTCITY,
                                                         LOADDESTSTATE,
                                                         LOADBROKERNUMBER,
                                                         LOADCARRIERCONTCONTROL,
                                                         UseCarrierFilters,
                                                         DaysOut,
                                                         DelDaysOut,
                                                         xmlXmlTransCode,
                                                         xmlXmlLoadCompanyIDs,
                                                         xmlXmlLoadLanes,
                                                         p_sortordinal,
                                                         p_sortdirection,
                                                         p_datefilterfield,
                                                         p_datefilterfrom,
                                                         p_datefilterto,
                                                         page,
                                                         pagesize).ToArray()


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAllItems365"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Get the records for the All page based on the provided filters and page details
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns>vBookAllItem</returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.007 on 11/12/2020 
    '''   we now allow any TranCode with user provides one or more of the 
    '''   NoDaysOutFilters filters, Added Order Number (LOADOrderNumber) to the NoDaysOutFilters list
    ''' </remarks>
    Public Function GetAllItemsFiltered365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vBookAllItem()
        'Dim oAll = New NGLAllItemData(Parameters)
        'Dim oUSC = New NGLUserSecurityCarrierData(Parameters)
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vBookAllItem
        If Parameters.UserCarrierControl <> 0 Then
            Dim oAllbooks = GetAllItemsFilteredByCarrier365(filters, RecordCount)
            If Not oAllbooks Is Nothing AndAlso oAllbooks.Count > 0 Then
                Dim sRef As String = ""
                Dim lAllItems As New List(Of LTS.vBookAllItem)
                Dim sSkip As New List(Of String)
                sSkip.Add("UserSecurityControl")
                For Each bitem In oAllbooks
                    Dim oNewItem As New LTS.vBookAllItem
                    oNewItem = CopyMatchingFields(oNewItem, bitem, sSkip, sRef)
                    If Not oNewItem Is Nothing Then
                        lAllItems.Add(oNewItem)
                    End If
                Next
                Return lAllItems.ToArray()
            Else
                Return oRet
            End If
        End If

        Dim DaysOut As Integer = 60
        Dim DelDaysOut As Integer = 360
        Dim DaysOutDate As Date = Date.Now.AddDays((DaysOut * -1))
        Dim DelDaysOutDate As Date = Date.Now.AddDays((DelDaysOut * -1))


        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                db.Log = New DebugTextWriter
                convertVALLGrid365FilterToVBookAllItemFilter(filters)
                'Dim Sec As New NGLSecurityDataProvider(Parameters)
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vBookAllItem)
                iQuery = db.vBookAllItems
                'Dim sTRANSCODEFilter As String =
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""

                'Note: the new view vCompByUserLegalEntity included in vBookItem needs to be modified to 
                '       include UserAdminCompControl and the list of companies allowed for this user.
                '       the current default is just Legal entity specific.  To allow access to multiple
                '       companies this logic must be completed and tested.  We also need to review how we used old  companyIds
                '       like in GetUserSecuritySetting
                '       Add Lane restrictions for 8.2 where LaneControl in list of LaneControl Numbers
                '       NGLSecurityData.GetUserSecurityLanes
                '       check the USSBrokerNumber? how is this used today? ' Modified by RHR for v-8.2.1.007 on 11/12/2020
                filterWhere &= "(UserSecurityControl = " & Parameters.UserControl & ")  "
                ' old code filterWhere &= "((LOADTRANSCODE = ""N"") Or (LOADTRANSCODE = ""P"") Or (LOADTRANSCODE = ""PC"") Or (LOADTRANSCODE = ""PB"")) AND (UserSecurityControl = " & Parameters.UserControl & ")  "

                sFilterSpacer = " And "
                'calculate days out
                Dim sNoDaysOutFilters() = {"LOADOD",
                                           "SCHEDULEDATE",
                                           "LOADReqDate",
                                           "LOADDelDate",
                                           "LOADCONSPREFIX",
                                           "BookSHID",
                                           "LOADToLoadDate",
                                           "LOADScheduleTime",
                                           "LOADStartLoadingDate",
                                           "LOADFinishLoadingDate",
                                           "LOADActLoadComplete_Date",
                                           "LOADDelScheduleDate",
                                           "LOADActDelDate",
                                           "LOADDelStartUnloadingDate",
                                           "LOADDelFinishUnloadingDate",
                                           "LOADActLoadCompDate",
                                           "LOADCarrierActDate",
                                           "LOADOrderNumber"}  ' Modified by RHR for v-8.2.1.007 on 11/12/2020}
                Dim blnLookupDaysOut As Boolean = True
                For Each oItem In filters.FilterValues
                    If sNoDaysOutFilters.Contains(oItem.filterName) And oItem.filterValueFrom.Length > 5 Then
                        blnLookupDaysOut = False
                        DaysOut = 10000
                        DelDaysOut = 10000
                    End If
                Next

                If blnLookupDaysOut Then
                    DaysOut = CInt(GetSystemParameterValue("GlobalNEXTrackDispLoads"))
                    DelDaysOut = CInt(GetSystemParameterValue("GlobalNEXTrackDispDelLoads"))
                    ' Modified by RHR for v-8.2.1.007 on 11/12/2020
                    ' we only add the LOADTRANSCODE filters when blnLookDaysOut is true
                    filterWhere &= " AND ((LOADTRANSCODE = ""N"") Or (LOADTRANSCODE = ""P"") Or (LOADTRANSCODE = ""PC"") Or (LOADTRANSCODE = ""PB"")) "
                End If
                If DaysOut > 0 Then
                    DaysOutDate = Date.Now.AddDays((DaysOut * -1))
                End If
                If DelDaysOut > 0 Then
                    DelDaysOutDate = Date.Now.AddDays((DelDaysOut * -1))
                End If
                If Not filters.FilterValues.Any(Function(x) x.filterName = "LOADReqDate" Or x.filterName = "SCHEDULEDATE") Then
                    filterWhere &= sFilterSpacer & " (LOADReqDate >= DateTime.Parse(""" + DaysOutDate + """)) "
                    sFilterSpacer = " And "
                End If
                If Not filters.FilterValues.Any(Function(x) x.filterName = "LOADActDelDate" Or x.filterName = "LOADCarrierActDate") Then
                    filterWhere &= sFilterSpacer & " ( (LOADActDelDate.HasValue = False) Or LOADActDelDate >= DateTime.Parse(""" + DelDaysOutDate + """) )"
                    sFilterSpacer = " And "
                End If
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "LOADDATE"
                    filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                'db.Log = Console.Out ' New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAllItemsFiltered365"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Get the records for the All page filtered by carrier based on the provided filters and page details
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns>vBookAllItemByCarrier</returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.007 on 11/12/2020 
    '''   we now allow I and IC TranCode for carriers.
    '''   Added Order Number (LOADOrderNumber) to the NoDaysOutFilters list
    ''' </remarks>
    Public Function GetAllItemsFilteredByCarrier365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vBookAllItemByCarrier()

        If filters Is Nothing Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim DaysOut As Integer = 60
        Dim DelDaysOut As Integer = 360
        Dim DaysOutDate As Date = Date.Now.AddDays((DaysOut * -1))
        Dim DelDaysOutDate As Date = Date.Now.AddDays((DelDaysOut * -1))
        Dim oRet() As LTS.vBookAllItemByCarrier
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                convertVALLGrid365FilterToVBookAllItemFilter(filters)
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vBookAllItemByCarrier)
                iQuery = db.vBookAllItemByCarriers
                'Dim sTRANSCODEFilter As String =
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                If (DirectCast(Me.NDPBaseClassFactory("NGLCarrierData"), NGLCarrierData).GetCarrierAllowWebTender(Parameters.UserCarrierControl)) Then
                    ' Modified by RHR for v-8.2.1.007 on 11/12/2020
                    filterWhere = " ((LOADTRANSCODE = ""PB"") Or (LOADTRANSCODE = ""I"") Or (LOADTRANSCODE = ""IC"")) And (BookCarrierControl = " & Parameters.UserCarrierControl & ")"
                    If Parameters.UserCarrierContControl <> 0 Then
                        filterWhere &= " And (LOADCARRIERCONTCONTROL = " & Parameters.UserCarrierContControl & ") "
                    End If
                Else
                    Return oRet 'we only show records for carriers when Allow Web Tender is true
                End If

                sFilterSpacer = " And "
                'calculate days out
                Dim sNoDaysOutFilters() = {"LOADOD",
                                           "SCHEDULEDATE",
                                           "LOADReqDate",
                                           "LOADDelDate",
                                           "LOADCONSPREFIX",
                                           "BookSHID",
                                           "LOADToLoadDate",
                                           "LOADScheduleTime",
                                           "LOADStartLoadingDate",
                                           "LOADFinishLoadingDate",
                                           "LOADActLoadComplete_Date",
                                           "LOADDelScheduleDate",
                                           "LOADActDelDate",
                                           "LOADDelStartUnloadingDate",
                                           "LOADDelFinishUnloadingDate",
                                           "LOADActLoadCompDate",
                                           "LOADCarrierActDate",
                                           "LOADOrderNumber"}  ' Modified by RHR for v-8.2.1.007 on 11/12/2020
                Dim blnLookupDaysOut As Boolean = True
                For Each oItem In filters.FilterValues
                    If sNoDaysOutFilters.Contains(oItem.filterName) And oItem.filterValueFrom.Length > 5 Then
                        blnLookupDaysOut = False
                        DaysOut = 10000
                        DelDaysOut = 10000
                    End If
                Next

                If blnLookupDaysOut Then
                    DaysOut = CInt(GetSystemParameterValue("GlobalNEXTrackDispLoads"))
                    DelDaysOut = CInt(GetSystemParameterValue("GlobalNEXTrackDispDelLoads"))
                End If
                If DaysOut > 0 Then
                    DaysOutDate = Date.Now.AddDays((DaysOut * -1))
                End If
                If DelDaysOut > 0 Then
                    DelDaysOutDate = Date.Now.AddDays((DelDaysOut * -1))
                End If
                If Not filters.FilterValues.Any(Function(x) x.filterName = "LOADReqDate" Or x.filterName = "SCHEDULEDATE") Then
                    filterWhere &= sFilterSpacer & " (LOADReqDate >= DateTime.Parse(""" + DaysOutDate + """)) "
                    sFilterSpacer = " And "
                End If
                If Not filters.FilterValues.Any(Function(x) x.filterName = "LOADActDelDate" Or x.filterName = "LOADCarrierActDate") Then
                    filterWhere &= sFilterSpacer & " ( (LOADActDelDate.HasValue = False) Or LOADActDelDate >= DateTime.Parse(""" + DelDaysOutDate + """) )"
                    sFilterSpacer = " And "
                End If
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "LOADDATE"
                    filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAllItemsFilteredByCarrier365"))
            End Try
        End Using
        Return oRet

    End Function


    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        SaveChanges(oData)
        Dim source As DataTransferObjects.AllItem = TryCast(oData, DataTransferObjects.AllItem)
        If source Is Nothing Then Return Nothing
        ' Return the updated order
        Return GetAllItem(Control:=source.Control)

    End Function

    Public Sub UpdateAll(ByVal AllItems() As DataTransferObjects.AllItem)

        For Each Item As DataTransferObjects.AllItem In AllItems
            SaveChanges(Item)
        Next


    End Sub

    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                             ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        SaveChanges(oData)
    End Sub

#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Return Nothing
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow booking records to be deleted from this class
        Utilities.SaveAppError("Cannot add data.  Records cannot be inserted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow booking records to be deleted from this class
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        ''Check if the data already exists only one allowed
        'With CType(oData, DTO.AllItem)
        '    Try
        '        'Get the newest record that matches the provided criteria
        '        Dim AllItem As DTO.AllItem = ( _
        '        From t In CType(oDB, NGLMasBookDataContext).vBookLoadMaintenances _
        '         Where _
        '             (t.BookControl <> .BookControl) _
        '             And _
        '             (t.BookProNumber = .BookProNumber) _
        '         Select New DTO.AllItem With {.BookControl = t.BookControl}).First

        '        If Not AllItem Is Nothing Then
        '            Utilities.SaveAppError("Cannot save Book changes.  The Book PRO Number, " & .BookProNumber & " already exist.", Me.Parameters)
        '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
        '        End If

        '    Catch ex As FaultException
        '        Throw
        '    Catch ex As InvalidOperationException
        '        'do nothing this is the desired result.
        '    End Try
        'End With
    End Sub

    Private Sub convertVALLGrid365FilterToVBookAllItemFilter(ByVal filters As Models.AllFilters)

        If Not filters Is Nothing AndAlso Not String.IsNullOrWhiteSpace(filters.filterName) Then
            For Each oItem In filters.FilterValues
                If Not String.IsNullOrWhiteSpace(oItem.filterName) Then
                    oItem.filterName = convertAllItem365FilterToVBookAllItemFilter(oItem.filterName)
                End If
            Next
        End If
        If Not filters Is Nothing AndAlso Not String.IsNullOrWhiteSpace(filters.sortName) Then
            For Each oItem In filters.SortValues
                If Not String.IsNullOrWhiteSpace(oItem.sortName) Then
                    oItem.sortName = convertAllItem365SortToVBookAllItemSort(oItem.sortName)
                End If
            Next
        End If

    End Sub

    Private Function convertAllItem365FilterToVBookAllItemFilter(ByVal filterName As String) As String

        Select Case filterName
            Case "Control"
                Return "LOADBookControl"
            Case "ProNumber"
                Return "LOADProNumber"
            Case "CnsNumber"
                Return "LOADCONSPREFIX"
            Case "StopNumber"
                Return "LOADStopNo"
            Case "PurchaseOrderNumber"
                Return "LOADPO"
            Case "OrderNumber"
                Return "LOADOrderNumber"
            Case "SHID"
                Return "BookSHID"
            Case "CarrierPro"
                Return "BookShipCarrierProNumber"
            Case "ScheduledToLoad"
                Return "LOADDATE"
            Case "RequestedToArrive"
                Return "SCHEDULEDATE"
            Case "AssignedCarrier"
                Return "LOADCarrierName"
            Case "DestinationName"
                Return "LOADDestName"
            Case "DestinationAddress1"
                Return "LOADDestAdd1"
            Case "DestinationAddress2"
                Return "LOADDestAdd2"
            Case "DestinationCity"
                Return "LOADDestCity"
            Case "DestinationState"
                Return "LOADDestState"
            Case "DestinationZip"
                Return "LOADDestZip"
            Case "DestinationCountry"
                Return "LOADDestCountry"
            Case "OrigName"
                Return "LOADPickUpName"
            Case "OrigAddress1"
                Return "LOADPickUpAdd1"
            Case "OrigAddress2"
                Return "LOADPickUPAdd2"
            Case "OrigCity"
                Return "LOADPickUpCity"
            Case "OrigState"
                Return "LOADPickUpState"
            Case "OrigZip"
                Return "LOADPickUpZip"
            Case "OrigCountry"
                Return "LoadPickUpCountry"
            Case "BookCarrScheduleDate"
                Return "LOADToLoadDate"
            Case "BookCarrScheduleTime"
                Return "LOADScheduleTime"
            Case "BookCarrActualDate"
                Return "LOADActPickupDate"
            Case "BookCarrActualTime" : Return "LOADActPickupTime"
            Case "BookCarrStartLoadingDate" : Return "LOADStartLoadingDate"
            Case "BookCarrStartLoadingTime" : Return "LOADStartLoadingTime"
            Case "BookCarrFinishLoadingDate" : Return "LOADFinishLoadingDate"
            Case "BookCarrFinishLoadingTime" : Return "LOADFinishLoadingTime"
            Case "BookCarrActLoadComplete_Date" : Return "LOADActLoadComplete_Date"
            Case "BookCarrActLoadCompleteTime" : Return "LOADActLoadCompleteTime"
            Case "BookCarrDockPUAssigment" : Return "LOADDockPUAssigment"
            Case "BookCarrApptDate" : Return "LOADDelScheduleDate"
            Case "BookCarrApptTime" : Return "LOADDelScheduleTime"
            Case "BookCarrActDate" : Return "LOADActDelDate"
            Case "BookCarrActTime" : Return "LOADActDelTime"
            Case "BookCarrStartUnloadingDate" : Return "LOADDelStartUnloadingDate"
            Case "BookCarrStartUnloadingTime" : Return "LOADDelStartUnloadingTime"
            Case "BookCarrFinishUnloadingDate" : Return "LOADDelFinishUnloadingDate"
            Case "BookCarrFinishUnloadingTime" : Return "LOADDelFinishUnloadingTime"
            Case "BookCarrActUnloadCompDate" : Return "LOADActLoadCompDate"
            Case "BookCarrActUnloadCompTime" : Return "LOADActLoadCompTime"
            Case "BookCarrDockDelAssignment" : Return "LOADDelDock"
            Case "BookCarrTrailerNo" : Return "LOADTrailerNo"
            Case "BookCarrSealNo" : Return "LOADSealNo"
            Case "BookCarrDriverNo" : Return "LOADDriverNo"
            Case "BookCarrDriverName" : Return "LOADDriverName"
            Case "BookCarrTripNo" : Return "LOADTripNo"
            Case "BookCarrRouteNo" : Return "LOADRouteNo"
            Case "BookWhseAuthorizationNo" : Return "LOADWhseAuthorizationNo"
            Case "BookNotes1" : Return "BookNotesVisable1"
            Case "BookNotes2" : Return "BookNotesVisable2"
            Case "BookNotes3" : Return "BookNotesVisable3"
            Case "AssignedProNumber" : Return "BookShipCarrierProNumber"
            Case "AssignedCarrierName" : Return "BookShipCarrierName"
            Case "AssignedCarrierNumber" : Return "BookShipCarrierNumber"
            Case "AssignedCarrierContact" : Return "BookCarrierContact"
            Case "AssignedCarrierContactPhone" : Return "BookCarrierContactPhone"
            Case "BookModDate" : Return "BookModDate"
            Case "BookModUser" : Return "BookModUser"
        End Select
        Return filterName

    End Function

    Private Function convertAllItem365SortToVBookAllItemSort(ByVal sortName As String) As String

        'now update the sorting options
        Select Case sortName
            Case "Control"
                Return "LOADBookControl"
            Case "ProNumber"
                Return "LOADProNumber"
            Case "CnsNumber"
                Return "LOADCONSPREFIX"
            Case "StopNumber"
                Return "LOADStopNo"
            Case "PurchaseOrderNumber"
                Return "LOADPO"
            Case "OrderNumber"
                Return "LOADOrderNumber"
            Case "SHID"
                Return "BookSHID"
            Case "CarrierPro"
                Return "BookShipCarrierProNumber"
            Case "ScheduledToLoad"
                Return "LOADDATE"
            Case "RequestedToArrive"
                Return "SCHEDULEDATE"
            Case "AssignedCarrier"
                Return "LOADCarrierName"
            Case "DestinationName"
                Return "LOADDestName"
            Case "DestinationAddress1"
                Return "LOADDestAdd1"
            Case "DestinationAddress2"
                Return "LOADDestAdd2"
            Case "DestinationCity"
                Return "LOADDestCity"
            Case "DestinationState"
                Return "LOADDestState"
            Case "DestinationZip"
                Return "LOADDestZip"
            Case "DestinationCountry"
                Return "LOADDestCountry"
            Case "OrigName"
                Return "LOADPickUpName"
            Case "OrigAddress1"
                Return "LOADPickUpAdd1"
            Case "OrigAddress2"
                Return "LOADPickUPAdd2"
            Case "OrigCity"
                Return "LOADPickUpCity"
            Case "OrigState"
                Return "LOADPickUpState"
            Case "OrigZip"
                Return "LOADPickUpZip"
            Case "OrigCountry"
                Return "LoadPickUpCountry"
            Case "BookCarrScheduleDate"
                Return "LOADToLoadDate"
            Case "BookCarrScheduleTime"
                Return "LOADScheduleTime"
            Case "BookCarrActualDate"
                Return "LOADActPickupDate"
            Case "BookCarrActualTime" : Return "LOADActPickupTime"
            Case "BookCarrStartLoadingDate" : Return "LOADStartLoadingDate"
            Case "BookCarrStartLoadingTime" : Return "LOADStartLoadingTime"
            Case "BookCarrFinishLoadingDate" : Return "LOADFinishLoadingDate"
            Case "BookCarrFinishLoadingTime" : Return "LOADFinishLoadingTime"
            Case "BookCarrActLoadComplete_Date" : Return "LOADActLoadComplete_Date"
            Case "BookCarrActLoadCompleteTime" : Return "LOADActLoadCompleteTime"
            Case "BookCarrDockPUAssigment" : Return "LOADDockPUAssigment"
            Case "BookCarrApptDate" : Return "LOADDelScheduleDate"
            Case "BookCarrApptTime" : Return "LOADDelScheduleTime"
            Case "BookCarrActDate" : Return "LOADActDelDate"
            Case "BookCarrActTime" : Return "LOADActDelTime"
            Case "BookCarrStartUnloadingDate" : Return "LOADDelStartUnloadingDate"
            Case "BookCarrStartUnloadingTime" : Return "LOADDelStartUnloadingTime"
            Case "BookCarrFinishUnloadingDate" : Return "LOADDelFinishUnloadingDate"
            Case "BookCarrFinishUnloadingTime" : Return "LOADDelFinishUnloadingTime"
            Case "BookCarrActUnloadCompDate" : Return "LOADActLoadCompDate"
            Case "BookCarrActUnloadCompTime" : Return "LOADActLoadCompTime"
            Case "BookCarrDockDelAssignment" : Return "LOADDelDock"
            Case "BookCarrTrailerNo" : Return "LOADTrailerNo"
            Case "BookCarrSealNo" : Return "LOADSealNo"
            Case "BookCarrDriverNo" : Return "LOADDriverNo"
            Case "BookCarrDriverName" : Return "LOADDriverName"
            Case "BookCarrTripNo" : Return "LOADTripNo"
            Case "BookCarrRouteNo" : Return "LOADRouteNo"
            Case "BookWhseAuthorizationNo" : Return "LOADWhseAuthorizationNo"
            Case "BookNotes1" : Return "BookNotesVisable1"
            Case "BookNotes2" : Return "BookNotesVisable2"
            Case "BookNotes3" : Return "BookNotesVisable3"
            Case "AssignedProNumber" : Return "BookShipCarrierProNumber"
            Case "AssignedCarrierName" : Return "BookShipCarrierName"
            Case "AssignedCarrierNumber" : Return "BookShipCarrierNumber"
            Case "AssignedCarrierContact" : Return "BookCarrierContact"
            Case "AssignedCarrierContactPhone" : Return "BookCarrierContactPhone"
            Case "BookModDate" : Return "BookModDate"
            Case "BookModUser" : Return "BookModUser"
        End Select
        Return sortName

    End Function


    Private Function getNewBookCodeValues(ByRef intCodeVal1 As Integer, ByRef intCodeVal2 As Integer) As Boolean
        Dim blnRet As Boolean = False
        Try
            intCodeVal1 = getScalarInteger("Select dbo.getLastBookCode1() as RetVal")
            intCodeVal2 = getScalarInteger("Select dbo.getLastBookCode2() as RetVal")
            intCodeVal2 = intCodeVal2 + 1
            If intCodeVal2 >= 255 Then
                intCodeVal2 = 33
                intCodeVal1 = intCodeVal1 + 1
                If intCodeVal1 = 160 Then intCodeVal1 = 161
                If intCodeVal1 = 173 Then intCodeVal1 = 174
            End If
            If intCodeVal1 > 255 Then
                Utilities.SaveAppError("E_MaxNbrOfBooks", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_MaxNbrOfBooks"}, New FaultReason("E_CreateRecordFailure"))
            End If
            If intCodeVal2 < 40 Then intCodeVal2 = 40
            If intCodeVal2 = 45 Then intCodeVal2 = 46
            If intCodeVal2 > 96 And intCodeVal2 < 123 Then intCodeVal2 = 124
            If intCodeVal2 = 126 Then intCodeVal2 = 128
            If intCodeVal2 = 127 Then intCodeVal2 = 128
            If intCodeVal2 = 160 Then intCodeVal2 = 161
            If intCodeVal1 = 173 Then intCodeVal1 = 174
            blnRet = True
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
        Return blnRet
    End Function

    Private Function GetCustAbrev(ByVal Control As Integer,
                                  Optional ByVal UseCompNumber As Boolean = False) As String

        Dim strSQL As String = "Select dbo.comp.compabrev as RetVal " _
                               & " From dbo.comp "
        If UseCompNumber Then
            strSQL &= " Where dbo.comp.compnumber = " & Control
        Else
            strSQL &= " Where dbo.comp.compcontrol = " & Control
        End If
        Return getScalarString(strSQL)

    End Function

    Private Sub SaveChanges(ByVal oData As DataTransferObjects.AllItem)
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            With oData
                Try
                    'Open the existing Record
                    Dim d = (From e In CType(LinqDB, NGLMasBookDataContext).Books Where e.BookControl = .Control Select e).First
                    If d Is Nothing Then
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
                    Else
                        'Check for conflicts
                        'Modified by RHR for v-8.5.2.006 on 12/29/2022 check for seconds because D365 does not save milliseconds
                        Dim iSeconds = DateDiff(DateInterval.Second, .BookModDate.Value, d.BookModDate.Value)
                        If iSeconds > 0 Then
                            'the data may have changed so check each field for conflicts
                            Dim ConflictData As New List(Of KeyValuePair(Of String, String))
                            Dim blnConflictFound As Boolean = False
                            With .CarrierData
                                addToConflicts("BookCarrScheduleDate", .BookCarrScheduleDate, d.BookCarrScheduleDate, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrScheduleTime", .BookCarrScheduleTime, d.BookCarrScheduleTime, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrActualDate", .BookCarrActualDate, d.BookCarrActualDate, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrActualTime", .BookCarrActualTime, d.BookCarrActualTime, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrStartLoadingDate", .BookCarrStartLoadingDate, d.BookCarrStartLoadingDate, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrStartLoadingTime", .BookCarrStartLoadingTime, d.BookCarrStartLoadingTime, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrFinishLoadingDate", .BookCarrFinishLoadingDate, d.BookCarrFinishLoadingDate, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrFinishLoadingTime", .BookCarrFinishLoadingTime, d.BookCarrFinishLoadingTime, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrActLoadComplete_Date", .BookCarrActLoadComplete_Date, d.BookCarrActLoadComplete_Date, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrActLoadCompleteTime", .BookCarrActLoadCompleteTime, d.BookCarrActLoadCompleteTime, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrDockPUAssigment", .BookCarrDockPUAssigment, d.BookCarrDockPUAssigment, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrApptDate", .BookCarrApptDate, d.BookCarrApptDate, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrApptTime", .BookCarrApptTime, d.BookCarrApptTime, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrActDate", .BookCarrActDate, d.BookCarrActDate, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrActTime", .BookCarrActTime, d.BookCarrActTime, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrStartUnloadingDate", .BookCarrStartUnloadingDate, d.BookCarrStartUnloadingDate, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrStartUnloadingTime", .BookCarrStartUnloadingTime, d.BookCarrStartUnloadingTime, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrFinishUnloadingDate", .BookCarrFinishUnloadingDate, d.BookCarrFinishUnloadingDate, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrFinishUnloadingTime", .BookCarrFinishUnloadingTime, d.BookCarrFinishUnloadingTime, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrActUnloadCompDate", .BookCarrActUnloadCompDate, d.BookCarrActUnloadCompDate, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrActUnloadCompTime", .BookCarrActUnloadCompTime, d.BookCarrActUnloadCompTime, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrDockDelAssignment", .BookCarrDockDelAssignment, d.BookCarrDockDelAssignment, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrTrailerNo", .BookCarrTrailerNo, d.BookCarrTrailerNo, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrSealNo", .BookCarrSealNo, d.BookCarrSealNo, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrDriverNo", .BookCarrDriverNo, d.BookCarrDriverNo, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrDriverName", .BookCarrDriverName, d.BookCarrDriverName, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrTripNo", .BookCarrTripNo, d.BookCarrTripNo, ConflictData, blnConflictFound)
                                addToConflicts("BookCarrRouteNo", .BookCarrRouteNo, d.BookCarrRouteNo, ConflictData, blnConflictFound)
                                addToConflicts("BookWhseAuthorizationNo", .BookWhseAuthorizationNo, d.BookWhseAuthorizationNo, ConflictData, blnConflictFound)
                            End With
                            'addToConflicts("Comments", .Comments, d.Comments, ConflictData, blnConflictFound)
                            addToConflicts("BookShipCarrierProNumber", .AssignedProNumber, d.BookShipCarrierProNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookShipCarrierProNumberRaw", .BookShipCarrierProNumberRaw, d.BookShipCarrierProNumberRaw, ConflictData, blnConflictFound)
                            addToConflicts("BookShipCarrierProControl", .BookShipCarrierProControl, d.BookShipCarrierProControl, ConflictData, blnConflictFound)
                            addToConflicts("BookShipCarrierName", .AssignedCarrierName, d.BookShipCarrierName, ConflictData, blnConflictFound)
                            addToConflicts("BookShipCarrierNumber", .AssignedCarrierNumber, d.BookShipCarrierNumber, ConflictData, blnConflictFound)

                            If blnConflictFound Then
                                'We only add the mod date and mod user if one or more other fields have been modified
                                addToConflicts("BookModDate", .BookModDate, d.BookModDate, ConflictData, blnConflictFound)
                                addToConflicts("BookModUser", .BookModUser, d.BookModUser, ConflictData, blnConflictFound)
                                Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(ConflictData))
                                conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                                Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                            End If
                        End If
                        Dim StatusUpdateMsg As String = "NEXTrack changes "
                        'build the status update string for the date and time changes and update the data in the book table as needed
                        With .CarrierData
                            If d.BookCarrScheduleDate <> .BookCarrScheduleDate Then
                                d.BookCarrScheduleDate = .BookCarrScheduleDate

                            End If
                            d.BookCarrScheduleTime = .BookCarrScheduleTime
                            d.BookCarrActualDate = .BookCarrActualDate
                            d.BookCarrActualTime = .BookCarrActualTime
                            d.BookCarrStartLoadingDate = .BookCarrStartLoadingDate
                            d.BookCarrStartLoadingTime = .BookCarrStartLoadingTime
                            d.BookCarrFinishLoadingDate = .BookCarrFinishLoadingDate
                            d.BookCarrFinishLoadingTime = .BookCarrFinishLoadingTime
                            d.BookCarrActLoadComplete_Date = .BookCarrActLoadComplete_Date
                            d.BookCarrActLoadCompleteTime = .BookCarrActLoadCompleteTime
                            d.BookCarrDockPUAssigment = .BookCarrDockPUAssigment
                            d.BookCarrApptDate = .BookCarrApptDate
                            d.BookCarrApptTime = .BookCarrApptTime
                            d.BookCarrActDate = .BookCarrActDate
                            d.BookCarrActTime = .BookCarrActTime
                            d.BookCarrStartUnloadingDate = .BookCarrStartUnloadingDate
                            d.BookCarrStartUnloadingTime = .BookCarrStartUnloadingTime
                            d.BookCarrFinishUnloadingDate = .BookCarrFinishUnloadingDate
                            d.BookCarrFinishUnloadingTime = .BookCarrFinishUnloadingTime
                            d.BookCarrActUnloadCompDate = .BookCarrActUnloadCompDate
                            d.BookCarrActUnloadCompTime = .BookCarrActUnloadCompTime
                            d.BookCarrDockDelAssignment = .BookCarrDockDelAssignment
                            d.BookCarrTrailerNo = .BookCarrTrailerNo
                            d.BookCarrSealNo = .BookCarrSealNo
                            d.BookCarrDriverNo = .BookCarrDriverNo
                            d.BookCarrDriverName = .BookCarrDriverName
                            d.BookCarrTripNo = .BookCarrTripNo
                            d.BookCarrRouteNo = .BookCarrRouteNo
                            d.BookWhseAuthorizationNo = .BookWhseAuthorizationNo
                        End With
                        d.BookModDate = Date.Now
                        d.BookModUser = Me.Parameters.UserName
                        d.BookShipCarrierProNumber = .AssignedProNumber
                        d.BookShipCarrierProNumberRaw = .BookShipCarrierProNumberRaw
                        d.BookShipCarrierProControl = .BookShipCarrierProControl
                        d.BookShipCarrierName = .AssignedCarrierName
                        d.BookShipCarrierNumber = .AssignedCarrierNumber
                    End If
                    LinqDB.SubmitChanges()
                Catch ex As FaultException
                    Throw
                Catch ex As SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
                Catch conflictEx As ChangeConflictException
                    Try
                        Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                        conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                        Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                    Catch ex As FaultException
                        Throw
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                Catch ex As InvalidOperationException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
                End Try

            End With
        End Using
    End Sub

    Private Function AddToTrackingMessageForDate(ByVal Original As DateTime, ByVal Updated As DateTime, ByVal prefix As String) As String
        Dim strRet As String = ""
        If Original <> Updated Then
            strRet = prefix & " [from " & Original.ToShortDateString & " " & Original.ToShortTimeString & " to " & Updated.ToShortDateString & " " & Updated.ToShortTimeString & "]" & Environment.NewLine
        End If
        Return strRet
        '//create string to hold changeds
        '  string changed = string.Empty;

        '  string pickup = trackPickupChanges ? "PICKUP: " + Environment.NewLine : string.Empty;
        '  string delivery = trackDeliveryChanges ? "DELIVERY: " + Environment.NewLine : string.Empty;

        '  if (trackPickupChanges) //vil >> 
        '  {
        '      //compare this (current item) with changed item (passed in)
        '      if ((oldAllItem.PickupScheduledAppointmentDate.ToString("MM/dd/yyyy") != newAllItem.PickupScheduledAppointmentDate.ToString("MM/dd/yyyy")) || (oldAllItem.PickupScheduledAppointmentTime.ToShortTimeString() != newAllItem.PickupScheduledAppointmentTime.ToShortTimeString()))
        '      {
        '          changed += pickup + " APPT [from " + oldAllItem.PickupScheduledAppointmentDate.ToString("MM/dd/yyyy") + " " + oldAllItem.PickupScheduledAppointmentTime.ToShortTimeString() + " to " + newAllItem.PickupScheduledAppointmentDate.ToString("MM/dd/yyyy") + " " + newAllItem.PickupScheduledAppointmentTime.ToShortTimeString() + "]" + Environment.NewLine;
        '          pickup = string.Empty;
        '      }

        '      if ((oldAllItem.PickupActualArrivalDate.ToString("MM/dd/yyyy") != newAllItem.PickupActualArrivalDate.ToString("MM/dd/yyyy")) || (oldAllItem.PickupActualArrivalTime.ToShortTimeString() != newAllItem.PickupActualArrivalTime.ToShortTimeString()))
        '      {
        '          changed += pickup + " ACT [from " + oldAllItem.PickupActualArrivalDate.ToString("MM/dd/yyyy") + " " + oldAllItem.PickupActualArrivalTime.ToShortTimeString() + " to " + newAllItem.PickupActualArrivalDate.ToString("MM/dd/yyyy") + " " + newAllItem.PickupActualArrivalTime.ToShortTimeString() + "]" + Environment.NewLine;
        '          //aan:>>
        '          pickup = string.Empty;
        '          //aan:<<
        '      }

        '      //aan:>>
        '      if ((oldAllItem.PickupStartLoadingDate.ToString("MM/dd/yyyy") != newAllItem.PickupStartLoadingDate.ToString("MM/dd/yyyy")) || (oldAllItem.PickupStartLoadingTime.ToShortTimeString() != newAllItem.PickupStartLoadingTime.ToShortTimeString()))
        '      {
        '          changed += pickup + " STRT [from " + oldAllItem.PickupStartLoadingDate.ToString("MM/dd/yyyy") + " " + oldAllItem.PickupStartLoadingTime.ToShortTimeString() + " to " + newAllItem.PickupStartLoadingDate.ToString("MM/dd/yyyy") + " " + newAllItem.PickupStartLoadingTime.ToShortTimeString() + "]" + Environment.NewLine;
        '          pickup = string.Empty;
        '      }

        '      if ((oldAllItem.PickupFinishLoadingDate.ToString("MM/dd/yyyy") != newAllItem.PickupFinishLoadingDate.ToString("MM/dd/yyyy")) || (oldAllItem.PickupFinishLoadingTime.ToShortTimeString() != newAllItem.PickupFinishLoadingTime.ToShortTimeString()))
        '      {
        '          changed += pickup + " FNSH [from " + oldAllItem.PickupFinishLoadingDate.ToString("MM/dd/yyyy") + " " + oldAllItem.PickupFinishLoadingTime.ToShortTimeString() + " to " + newAllItem.PickupFinishLoadingDate.ToString("MM/dd/yyyy") + " " + newAllItem.PickupFinishLoadingTime.ToShortTimeString() + "]" + Environment.NewLine;
        '          pickup = string.Empty;
        '      }

        '      if ((oldAllItem.PickupActLoadCompleteDate.ToString("MM/dd/yyyy") != newAllItem.PickupActLoadCompleteDate.ToString("MM/dd/yyyy")) || (oldAllItem.PickupActLoadCompleteTime.ToShortTimeString() != newAllItem.PickupActLoadCompleteTime.ToShortTimeString()))
        '      {
        '          changed += pickup + " CMPL [from " + oldAllItem.PickupActLoadCompleteDate.ToString("MM/dd/yyyy") + " " + oldAllItem.PickupActLoadCompleteTime.ToShortTimeString() + " to " + newAllItem.PickupActLoadCompleteDate.ToString("MM/dd/yyyy") + " " + newAllItem.PickupActLoadCompleteTime.ToShortTimeString() + "]" + Environment.NewLine;
        '      }
        '      //aan:<<
        '  } //vil<< 

        '                      If (trackDeliveryChanges) Then
        '  {
        '      if ((oldAllItem.DeliveryScheduledAppointmentDate.ToString("MM/dd/yyyy") != newAllItem.DeliveryScheduledAppointmentDate.ToString("MM/dd/yyyy")) || (oldAllItem.DeliveryScheduledAppointmentTime.ToShortTimeString() != newAllItem.DeliveryScheduledAppointmentTime.ToShortTimeString()))
        '      {
        '          changed += ((changed.Length > 0) ? " " : string.Empty) + delivery + " APPT [from " + oldAllItem.DeliveryScheduledAppointmentDate.ToString("MM/dd/yyyy") + " " + oldAllItem.DeliveryScheduledAppointmentTime.ToShortTimeString() + " to " + newAllItem.DeliveryScheduledAppointmentDate.ToString("MM/dd/yyyy") + " " + newAllItem.DeliveryScheduledAppointmentTime.ToShortTimeString() + "]" + Environment.NewLine;
        '          delivery = string.Empty;
        '      }

        '      if ((oldAllItem.DeliveryActualArrivalDate.ToString("MM/dd/yyyy") != newAllItem.DeliveryActualArrivalDate.ToString("MM/dd/yyyy")) || (oldAllItem.DeliveryActualArrivalTime.ToShortTimeString() != newAllItem.DeliveryActualArrivalTime.ToShortTimeString()))
        '      {
        '          changed += ((changed.Length > 0 && delivery.Length == 0) ? " " : string.Empty) + delivery + " ACT [from " + oldAllItem.DeliveryActualArrivalDate.ToString("MM/dd/yyyy") + " " + oldAllItem.DeliveryActualArrivalTime.ToShortTimeString() + " to " + newAllItem.DeliveryActualArrivalDate.ToString("MM/dd/yyyy") + " " + newAllItem.DeliveryActualArrivalTime.ToShortTimeString() + "]" + Environment.NewLine;
        '          //aan:>>
        '          delivery = string.Empty;
        '          //aan:<<
        '      }

        '      //aan:>>
        '      if ((oldAllItem.DeliveryStartUnloadingDate.ToString("MM/dd/yyyy") != newAllItem.DeliveryStartUnloadingDate.ToString("MM/dd/yyyy")) || (oldAllItem.DeliveryStartUnloadingTime.ToShortTimeString() != newAllItem.DeliveryStartUnloadingTime.ToShortTimeString()))
        '      {
        '          changed += ((changed.Length > 0 && delivery.Length == 0) ? " " : string.Empty) + delivery + " STRT [from " + oldAllItem.DeliveryStartUnloadingDate.ToString("MM/dd/yyyy") + " " + oldAllItem.DeliveryStartUnloadingTime.ToShortTimeString() + " to " + newAllItem.DeliveryStartUnloadingDate.ToString("MM/dd/yyyy") + " " + newAllItem.DeliveryStartUnloadingTime.ToShortTimeString() + "]" + Environment.NewLine;
        '          delivery = string.Empty;
        '      }

        '      if ((oldAllItem.DeliveryFinishUnloadingDate.ToString("MM/dd/yyyy") != newAllItem.DeliveryFinishUnloadingDate.ToString("MM/dd/yyyy")) || (oldAllItem.DeliveryFinishUnloadingTime.ToShortTimeString() != newAllItem.DeliveryFinishUnloadingTime.ToShortTimeString()))
        '      {
        '          changed += ((changed.Length > 0 && delivery.Length == 0) ? " " : string.Empty) + delivery + " FNSH [from " + oldAllItem.DeliveryFinishUnloadingDate.ToString("MM/dd/yyyy") + " " + oldAllItem.DeliveryFinishUnloadingTime.ToShortTimeString() + " to " + newAllItem.DeliveryFinishUnloadingDate.ToString("MM/dd/yyyy") + " " + newAllItem.DeliveryFinishUnloadingTime.ToShortTimeString() + "]" + Environment.NewLine;
        '          delivery = string.Empty;
        '      }

        '      if ((oldAllItem.DeliveryActUnloadCompDate.ToString("MM/dd/yyyy") != newAllItem.DeliveryActUnloadCompDate.ToString("MM/dd/yyyy")) || (oldAllItem.DeliveryActUnloadCompTime.ToShortTimeString() != newAllItem.DeliveryActUnloadCompTime.ToShortTimeString()))
        '      {
        '          changed += ((changed.Length > 0 && delivery.Length == 0) ? " " : string.Empty) + delivery + " CMPLT [from " + oldAllItem.DeliveryActUnloadCompDate.ToString("MM/dd/yyyy") + " " + oldAllItem.DeliveryActUnloadCompTime.ToShortTimeString() + " to " + newAllItem.DeliveryActUnloadCompDate.ToString("MM/dd/yyyy") + " " + newAllItem.DeliveryActUnloadCompTime.ToShortTimeString() + "]" + Environment.NewLine;
        '      }
        '      //aan:<<
        '  } //vil<<

        '  //check changed
        '                                              If (changed.Length > 0) Then
        '  {
        '      //prepend title
        '      changed = "NEXTrack Changes " + changed;

        '      //save in db
        '      //aan:>>
        '      //DataProviderFactory.GetDataProvider().SetChangedDateComments(newAllItem.Control, changed, newAllItem);//aan:
        '      //aan:<<
        '      //alg:>>
        '      DataProviderFactory.GetWCFDataProvider().SetChangedDateComments(newAllItem.Control, changed, newAllItem);
        '      //alg:<<
        '  }
    End Function

#End Region

End Class