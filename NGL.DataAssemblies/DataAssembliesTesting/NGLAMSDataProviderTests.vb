Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports System.Data.SqlClient
Imports NGL.Core.ChangeTracker
Imports NGL.FM.BLL

<TestClass()> Public Class NGLAMSDataProviderTests
    Inherits TestBase

    <TestMethod()>
    Public Sub CreateOrUpdateAppointmentTest()
        Dim target As New DAL.NGLAMSAppointmentData(testParameters)
        Dim order As New DTO.AMSOrderList

        With order
            .AMSCompControl = 9623
            .BookAMSDeliveryApptControl = 0
            .BookAMSPickupApptControl = 0
            .BookCarrierContact = "James"
            .BookCarrierContactPhone = "8004569476"
            .BookCarrierControl = 9658
            .BookCarrOrderNumber = "SO-TstNew2222"
            .BookCarrOrderNumberSeq = "SO-TstNew2222-0"
            .BookConsPrefix = "TST1234569"
            .BookControl = 900783
            .BookCustCompControl = 9623
            .BookDateLoad = "2016-04-15 00:00:00.000"
            .BookDateOrdered = "2016-04-12 00:00:00.000"
            .BookDateRequired = "2016-04-20 00:00:00.000"
            .BookDestAddress1 = "W1344 Industrial Dr"
            .BookDestCity = "Ixonia"
            .BookDestCompControl = 0
            .BookDestCountry = "US"
            .BookDestName = "Create A Pack Foods"
            .BookDestPhone = ""
            .BookDestState = "WI"
            .BookDestZip = "53036"
            .BookItemDetailDescription = ""
            .BookLoadControl = 848235
            .BookLoadPONumber = "SO-TEST1118"
            .BookODControl = 56600
            .BookOrderSequence = 0
            .BookOrigAddress1 = "7400 South Narragansett Avenue"
            .BookOrigCity = "Bedford Park"
            .BookOrigCompControl = 9623
            .BookOrigCountry = "US"
            .BookOrigName = "Vegetable Juices, Inc."
            .BookOrigPhone = ""
            .BookOrigState = "IL"
            .BookOrigZip = "60638"
            .BookProNumber = "Rob1113"
            .BookRouteConsFlag = True
            .BookShipCarrierName = "Budreck Truck Lines"
            .BookShipCarrierNumber = "35"
            .BookShipCarrierProControl = Nothing
            .BookShipCarrierProNumber = ""
            .BookShipCarrierProNumberRaw = ""
            .BookStopNo = 1
            .BookTotalCases = 8
            .BookTotalCube = 800
            .BookTotalPL = 8.0
            .BookTotalPX = 0
            .BookTotalWgt = 8000.0
            .CarrierName = "Veterans Truck Line, Inc."
            .CarrierNumber = 80
            .CarrierSCAC = "VETS"
            .LaneNumber = "31 Create A Pack"
            .LaneOriginAddressUse = False
            .OrderType = 2
            .OrderTypeMsg = "MSG_ConsolidatedLoadOutbound"
            .TrackingState = NGL.Core.ChangeTracker.TrackingInfo.Unchanged
        End With

        Dim Orders As DTO.AMSOrderList() = {order}

        Dim Appointment As New DTO.AMSAppointment
        With Appointment
            .AMSApptActLoadCompleteDateTime = Nothing
            .AMSApptActualDateTime = Nothing
            .AMSApptCarrierControl = 0
            .AMSApptCarrierName = "Budreck Truck Lines"
            .AMSApptCarrierSCAC = "35"
            .AMSApptCompControl = 9623
            .AMSApptControl = 0
            .AMSApptDescription = ""
            .AMSApptDockdoorID = "Dock 2"
            .AMSApptEndDate = "2016-04-11 10:30:00.000"
            .AMSApptFinishLoadingDateTime = Nothing
            .AMSApptHover = Nothing
            .AMSApptLabel = Nothing
            .AMSApptModDate = Nothing
            .AMSApptModUser = Nothing
            .AMSApptNotes = ""
            .AMSApptOrderCount = 0
            .AMSApptRecurrence = ""
            .AMSApptRecurrenceParentControl = Nothing
            .AMSApptStartDate = "2016-04-10 10:30:00.000"
            .AMSApptStartLoadingDateTime = Nothing
            .AMSApptStatusCode = 0
            .AMSApptTimeZone = Nothing
            .TrackingState = NGL.Core.ChangeTracker.TrackingInfo.Created
        End With


        Dim createOrUpdate As Boolean = True


        'Dim res = target.CreateOrUpdateAppointment(Orders, Appointment, createOrUpdate)



    End Sub

End Class