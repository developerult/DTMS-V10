Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports System.Data.SqlClient
Imports Ngl.Core.ChangeTracker

<TestClass()> Public Class AMSUnitTests
    Inherits TestBase

    'Set up a company With 4 docks - 2 inbound And 2 outbound

    '* TEST 1 *
    'Set it up so dock 1 has 2pm available but dock 2 does not
    'Get available times slots and verify that 2pm is available
    'Create an appt in the 2pm slot
    'Validate the availability for the 2pm slot -- it should fail

    '* TEST 2 *
    'Set it up so dock 1 has 2pm available but dock 2 does not
    'Get available times slots and verify that 2pm is available
    'Validate the availability for the 2pm slot -- it should be available

    ' * TEST 3 *
    'Set it up so dock 1 has 2pm available but dock 2 does not
    'Get available times slots and verify that 2pm is available
    'Create an appt in the 2pm slot
    'Call book carrier appt for the 2pm slot -- it should fail

    '* TEST 4 *
    'Set it up so dock 1 has 2pm available but dock 2 does not
    'Get available times slots and verify that 2pm is available
    'Call book carrier appt for the 2pm slot -- it should be available
    'Verify the appointment was created

    Private Sub setupTestConnction(ByVal UserName As String, ByVal UserCarrierControl As Integer)
        'Set up the test connection
        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPROD"
        testParameters.ConnectionString = "Server=NGLRDP07D;User ID=nglweb;Password=5529;Database=NGLMASPROD"
        testParameters.UserName = UserName
        testParameters.UserCarrierControl = UserCarrierControl
    End Sub

    Private Function createDummyAppointment(ByVal CompControl As Integer, ByVal CarrierControl As Integer, ByVal CarrierNumber As Integer, ByVal CarrierName As String, ByVal dtStart As Date, ByVal dtEnd As Date, ByVal strDockID As String) As Integer
        Dim oAMS As New DAL.NGLAMSAppointmentData(testParameters)
        Dim aptControl As Integer = 0
        Dim Appointment As New DTO.AMSAppointment
        With Appointment
            .AMSApptCompControl = CompControl
            .AMSApptCarrierControl = CarrierControl
            .AMSApptCarrierSCAC = CarrierNumber 'I don't know why we do this but it was like this in the old NEXTrack so I didn't change it
            .AMSApptCarrierName = CarrierName
            .AMSApptStartDate = dtStart
            .AMSApptEndDate = dtEnd
            .AMSApptDockdoorID = strDockID
            .AMSApptModDate = Date.Now
            .AMSApptModUser = "AMS Unit Tests"
        End With
        'Create the appointment record
        Appointment = oAMS.CreateRecord(Appointment)
        If Appointment Is Nothing Then Return aptControl
        aptControl = Appointment.AMSApptControl
        Return aptControl
    End Function


    ''' <summary>
    ''' Dock Configuration - 1 dock has the 2pm timeslot available but the others do not
    ''' Get the available times slots and verify that 2pm is in the results set
    ''' Create a dummy appt in the 2pm timeslot for the open dock to simulate a concurrency issue
    ''' Call new method to validate the availability for the 2pm slot -- it should fail
    ''' </summary>
    <TestMethod()>
    Public Sub CarrierAvailabilityFailPickTest()
        'Set up the test connection
        setupTestConnction("London Postmaster", 2)

        Dim oAMS As New DAL.NGLAMSAppointmentData(testParameters)
        Dim oDock As New DAL.NGLDockSettingData(testParameters)

        'Set the parameters for the test record selection
        Dim SHID As String = "AMS-12-1119"
        Dim blnIsPickup As Boolean = True
        Dim pWarehouseControl As Integer = 50
        Dim targetDate As Date
        Date.TryParse("8/17/2020 12:00:00 AM", targetDate)

        'Get the test record
        Dim d = oAMS.GetAMSCarrierPickNeedApptBySHID(SHID, pWarehouseControl, targetDate)
        If d?.Count() < 1 Then Assert.Fail("No Pickups were returned using SHID: " + SHID + ", WarehouseControl: " + pWarehouseControl.ToString() + ", and LoadDate: " + targetDate.ToString() + "Unit Test Failed (CarrierAvailabilityFailPickTest)")
        Dim BookControl = d(0).BookControl
        Dim CarrierControl = d(0).BookCarrierControl
        Dim CompControl = d(0).BookOrigCompControl.Value
        Dim LoadDate = d(0).BookDateLoad.Value
        Dim RequiredDate = d(0).BookDateRequired.Value
        Dim EquipID = d(0).BookCarrTrailerNo
        Dim Inbound = d(0).Inbound.Value
        Dim Warehouse = d(0).Warehouse
        Dim CarrierName = d(0).CarrierName
        Dim CarrierNumber = d(0).CarrierNumber.Value
        Dim ScheduledDate = d(0).ScheduledDate
        Dim ScheduledTime = d(0).ScheduledTime

        Dim time = New TimeSpan(14, 0, 0)
        Dim t2PM As Date
        t2PM = targetDate.Date + time 'Create a 2PM time on the target date

        time = New TimeSpan(15, 59, 0)
        Dim t359PM As Date
        t359PM = targetDate.Date + time 'Create a 359PM time on the target date

        '***** Create dummy appointments in the 2pm time slot for docks 2 and 3 *****
        Dim d1Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Outbound 1")
        Dim d3Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Outbound 3")


        '***** Get available times slots and verify that 2pm is available *****
        Dim oTData = oDock.GetCarrierAvailableAppointments(CarrierControl, CompControl, BookControl, EquipID, LoadDate, RequiredDate, blnIsPickup, Inbound, Warehouse, CarrierName, CarrierNumber, ScheduledDate, ScheduledTime, SHID)
        If Not oTData.blnMustRequestAppt Then
            Dim retVals = oTData.AvailableSlots.ToArray()
            Dim bln2PMSlotStillAvailable = False
            For Each r In retVals
                If Date.Compare(r.StartTime, t2PM) = 0 Then
                    'Yes the 2pm is still available
                    bln2PMSlotStillAvailable = True

                    '***** Create an appt in the 2pm slot for Dock 2 (simulate concurrency issue) *****
                    Dim d2Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Outbound 2")

                    '***** Validate the availability for the 2pm slot on Dock 2 -- it should fail *****
                    Dim dtApptReqEndTime As Date
                    Dim strDockID As String = ""
                    Dim blnValidateSuccess = oDock.ValidateCarrierAppointmentAvailability(strDockID, dtApptReqEndTime, CarrierControl, CompControl, BookControl, Warehouse, CarrierName, CarrierNumber, r.StartTime)

                    'Test Cleanup - Delete the dummy appointments
                    oAMS.DeleteAMSAppointment(d1Ctrl, False)
                    oAMS.DeleteAMSAppointment(d2Ctrl, False)
                    oAMS.DeleteAMSAppointment(d3Ctrl, False)

                    If blnValidateSuccess Then Assert.Fail("Validation should have failed because the timeslot was unavailable. Unit Test Failed (CarrierAvailabilityFailPickTest)") Else Trace.WriteLine("Unit Test Passed (CarrierAvailabilityFailPickTest). Expected Result: Validation failure. Result: blnValidateSuccess = " + blnValidateSuccess.ToString())
                    Exit For
                End If
            Next
            If Not bln2PMSlotStillAvailable Then Assert.Fail("The test could not be run because the 2PM slot was not returned in the available appointment times list from the call to GetCarrierAvailableAppointments(). Unit Test Failed (CarrierAvailabilityFailPickTest)")
        End If
    End Sub

    ''' <summary>
    ''' Dock Configuration - 1 dock has the 2pm timeslot available but the others do not
    ''' Get the available times slots and verify that 2pm is in the results set
    ''' Create a dummy appt in the 2pm timeslot for the open dock to simulate a concurrency issue
    ''' Call new method to validate the availability for the 2pm slot -- it should fail
    ''' </summary>
    <TestMethod()>
    Public Sub CarrierAvailabilityFailDelTest()
        Dim source As String = "CarrierAvailabilityFailDelTest"

        'Set up the test connection
        setupTestConnction("London Postmaster", 2)

        Dim oAMS As New DAL.NGLAMSAppointmentData(testParameters)
        Dim oDock As New DAL.NGLDockSettingData(testParameters)

        'Set the parameters for the test record selection
        Dim SHID As String = "AMS-12-1120"
        Dim blnIsPickup As Boolean = False
        Dim pWarehouseControl As Integer = 50
        Dim targetDate As Date
        Date.TryParse("8/21/2020 12:00:00 AM", targetDate)

        'Get the test record
        Dim d = oAMS.GetAMSCarrierDelNeedApptBySHID(SHID, pWarehouseControl, targetDate)
        If d?.Count() < 1 Then Assert.Fail(String.Format("No Deliveries were returned using SHID: {0}, WarehouseControl: {1}, and RequiredDate: {2}. Unit Test Failed ({3})", SHID, pWarehouseControl.ToString(), targetDate.ToString(), source))
        Dim BookControl = d(0).BookControl
        Dim CarrierControl = d(0).BookCarrierControl
        Dim CompControl = d(0).BookDestCompControl.Value
        Dim LoadDate = d(0).BookDateLoad.Value
        Dim RequiredDate = d(0).BookDateRequired.Value
        Dim EquipID = d(0).BookCarrTrailerNo
        Dim Inbound = d(0).Inbound.Value
        Dim Warehouse = d(0).Warehouse
        Dim CarrierName = d(0).CarrierName
        Dim CarrierNumber = d(0).CarrierNumber.Value
        Dim ScheduledDate = d(0).ScheduledDate
        Dim ScheduledTime = d(0).ScheduledTime

        Dim time = New TimeSpan(14, 0, 0)
        Dim t2PM As Date
        t2PM = targetDate.Date + time 'Create a 2PM time on the target date

        time = New TimeSpan(15, 59, 0)
        Dim t359PM As Date
        t359PM = targetDate.Date + time 'Create a 359PM time on the target date

        '***** Create dummy appointments in the 2pm time slot for docks 2 and 3 *****
        Dim d1Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Inbound 1")
        Dim d3Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Inbound 3")

        '***** Get available times slots and verify that 2pm is available *****
        Dim oTData = oDock.GetCarrierAvailableAppointments(CarrierControl, CompControl, BookControl, EquipID, LoadDate, RequiredDate, blnIsPickup, Inbound, Warehouse, CarrierName, CarrierNumber, ScheduledDate, ScheduledTime, SHID)
        If Not oTData.blnMustRequestAppt Then
            Dim retVals = oTData.AvailableSlots.ToArray()
            Dim bln2PMSlotStillAvailable = False
            For Each r In retVals
                If Date.Compare(r.StartTime, t2PM) = 0 Then
                    'Yes the 2pm is still available
                    bln2PMSlotStillAvailable = True

                    '***** Create an appt in the 2pm slot for Dock 2 (simulate concurrency issue) *****
                    Dim d2Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Inbound 2")

                    '***** Validate the availability for the 2pm slot on Dock 2 -- it should fail *****
                    Dim dtApptReqEndTime As Date
                    Dim strDockID As String = ""
                    Dim blnValidateSuccess = oDock.ValidateCarrierAppointmentAvailability(strDockID, dtApptReqEndTime, CarrierControl, CompControl, BookControl, Warehouse, CarrierName, CarrierNumber, r.StartTime)

                    'Test Cleanup - Delete the dummy appointments
                    oAMS.DeleteAMSAppointment(d1Ctrl, False)
                    oAMS.DeleteAMSAppointment(d2Ctrl, False)
                    oAMS.DeleteAMSAppointment(d3Ctrl, False)

                    If blnValidateSuccess Then Assert.Fail("Validation should have failed because the timeslot was unavailable. Unit Test Failed (CarrierAvailabilityFailDelTest)") Else Trace.WriteLine("Unit Test Passed (CarrierAvailabilityFailDelTest). Expected Result: Validation failure. Result: blnValidateSuccess = " + blnValidateSuccess.ToString())
                    Exit For
                End If
            Next
            If Not bln2PMSlotStillAvailable Then Assert.Fail("The test could not be run because the 2PM slot was not returned in the available appointment times list from the call to GetCarrierAvailableAppointments(). Unit Test Failed (CarrierAvailabilityFailDelTest)")
        End If
    End Sub


    ''' <summary>
    ''' Dock Configuration - All docks have 2pm timeslot available
    ''' Get the available times slots and verify that 2pm is in the results set
    ''' Call new method to validate the availability for the 2pm slot -- it should pass
    ''' </summary>
    <TestMethod()>
    Public Sub CarrierAvailabilityPassPickTest()
        'Set up the test connection
        setupTestConnction("London Postmaster", 2)

        Dim oAMS As New DAL.NGLAMSAppointmentData(testParameters)
        Dim oDock As New DAL.NGLDockSettingData(testParameters)

        'Set the parameters for the test record selection
        Dim SHID As String = "AMS-12-1119"
        Dim blnIsPickup As Boolean = True
        Dim pWarehouseControl As Integer = 50
        Dim targetDate As Date
        Date.TryParse("8/17/2020 12:00:00 AM", targetDate)

        'Get the test record
        Dim d = oAMS.GetAMSCarrierPickNeedApptBySHID(SHID, pWarehouseControl, targetDate)
        If d?.Count() < 1 Then Assert.Fail("No Pickups were returned using SHID: " + SHID + ", WarehouseControl: " + pWarehouseControl.ToString() + ", and LoadDate: " + targetDate.ToString() + ". Unit Test Failed (CarrierAvailabilityPassPickTest)")
        If d Is Nothing Then Return
        Dim BookControl = d(0).BookControl
        Dim CarrierControl = d(0).BookCarrierControl
        Dim CompControl = d(0).BookOrigCompControl.Value
        Dim LoadDate = d(0).BookDateLoad.Value
        Dim RequiredDate = d(0).BookDateRequired.Value
        Dim EquipID = d(0).BookCarrTrailerNo
        Dim Inbound = d(0).Inbound.Value
        Dim Warehouse = d(0).Warehouse
        Dim CarrierName = d(0).CarrierName
        Dim CarrierNumber = d(0).CarrierNumber.Value
        Dim ScheduledDate = d(0).ScheduledDate
        Dim ScheduledTime = d(0).ScheduledTime

        Dim time = New TimeSpan(14, 0, 0)
        Dim t2PM As Date
        t2PM = targetDate.Date + time 'Create a 2PM time on the target date

        '***** Get available times slots and verify that 2pm is available *****
        Dim oTData = oDock.GetCarrierAvailableAppointments(CarrierControl, CompControl, BookControl, EquipID, LoadDate, RequiredDate, blnIsPickup, Inbound, Warehouse, CarrierName, CarrierNumber, ScheduledDate, ScheduledTime, SHID)
        If Not oTData.blnMustRequestAppt Then
            Dim retVals = oTData.AvailableSlots.ToArray()
            Dim bln2PMSlotStillAvailable = False
            For Each r In retVals
                If Date.Compare(r.StartTime, t2PM) = 0 Then
                    'Yes the 2pm is still available
                    bln2PMSlotStillAvailable = True

                    '***** Validate the availability for the 2pm slot -- it should pass *****
                    Dim dtApptReqEndTime As Date
                    Dim strDockID As String = ""
                    Dim blnValidateSuccess = oDock.ValidateCarrierAppointmentAvailability(strDockID, dtApptReqEndTime, CarrierControl, CompControl, BookControl, Warehouse, CarrierName, CarrierNumber, r.StartTime)

                    If Not blnValidateSuccess Then Assert.Fail("Validation should have passed because the timeslot is still available. Unit Test Failed (CarrierAvailabilityPassPickTest)") Else Trace.WriteLine("Unit Test Passed (CarrierAvailabilityPassPickTest). Expected Result: Validation passed. Result: blnValidateSuccess = " + blnValidateSuccess.ToString())
                    Exit For
                End If
            Next
            If Not bln2PMSlotStillAvailable Then Assert.Fail("The test could not be run because the 2PM slot was not returned in the available appointment times list from the call to GetCarrierAvailableAppointments(). Unit Test Failed (CarrierAvailabilityPassPickTest)")
        End If
    End Sub

    ''' <summary>
    ''' Dock Configuration - All docks have 2pm timeslot available
    ''' Get the available times slots and verify that 2pm is in the results set
    ''' Call new method to validate the availability for the 2pm slot -- it should pass
    ''' </summary>
    <TestMethod()>
    Public Sub CarrierAvailabilityPassDelTest()
        Dim source As String = "CarrierAvailabilityPassDelTest"
        'Set up the test connection
        setupTestConnction("London Postmaster", 2)

        Dim oAMS As New DAL.NGLAMSAppointmentData(testParameters)
        Dim oDock As New DAL.NGLDockSettingData(testParameters)

        'Set the parameters for the test record selection
        Dim SHID As String = "AMS-12-1120"
        Dim blnIsPickup As Boolean = False
        Dim pWarehouseControl As Integer = 50
        Dim targetDate As Date
        Date.TryParse("8/21/2020 12:00:00 AM", targetDate)

        'Get the test record
        Dim d = oAMS.GetAMSCarrierDelNeedApptBySHID(SHID, pWarehouseControl, targetDate)
        If d?.Count() < 1 Then Assert.Fail(String.Format("No Deliveries were returned using SHID: {0}, WarehouseControl: {1}, and RequiredDate: {2}. Unit Test Failed ({3})", SHID, pWarehouseControl.ToString(), targetDate.ToString(), source))
        If d Is Nothing Then Return
        Dim BookControl = d(0).BookControl
        Dim CarrierControl = d(0).BookCarrierControl
        Dim CompControl = d(0).BookDestCompControl.Value
        Dim LoadDate = d(0).BookDateLoad.Value
        Dim RequiredDate = d(0).BookDateRequired.Value
        Dim EquipID = d(0).BookCarrTrailerNo
        Dim Inbound = d(0).Inbound.Value
        Dim Warehouse = d(0).Warehouse
        Dim CarrierName = d(0).CarrierName
        Dim CarrierNumber = d(0).CarrierNumber.Value
        Dim ScheduledDate = d(0).ScheduledDate
        Dim ScheduledTime = d(0).ScheduledTime

        Dim time = New TimeSpan(14, 0, 0)
        Dim t2PM As Date
        t2PM = targetDate.Date + time 'Create a 2PM time on the target date

        '***** Get available times slots and verify that 2pm is available *****
        Dim oTData = oDock.GetCarrierAvailableAppointments(CarrierControl, CompControl, BookControl, EquipID, LoadDate, RequiredDate, blnIsPickup, Inbound, Warehouse, CarrierName, CarrierNumber, ScheduledDate, ScheduledTime, SHID)
        If Not oTData.blnMustRequestAppt Then
            Dim retVals = oTData.AvailableSlots.ToArray()
            Dim bln2PMSlotStillAvailable = False
            For Each r In retVals
                If Date.Compare(r.StartTime, t2PM) = 0 Then
                    'Yes the 2pm is still available
                    bln2PMSlotStillAvailable = True

                    '***** Validate the availability for the 2pm slot -- it should pass *****
                    Dim dtApptReqEndTime As Date
                    Dim strDockID As String = ""
                    Dim blnValidateSuccess = oDock.ValidateCarrierAppointmentAvailability(strDockID, dtApptReqEndTime, CarrierControl, CompControl, BookControl, Warehouse, CarrierName, CarrierNumber, r.StartTime)

                    If Not blnValidateSuccess Then Assert.Fail("Validation should have passed because the timeslot is still available. Unit Test Failed (CarrierAvailabilityPassDelTest)") Else Trace.WriteLine("Unit Test Passed (CarrierAvailabilityPassDelTest). Expected Result: Validation passed. Result: blnValidateSuccess = " + blnValidateSuccess.ToString())
                    Exit For
                End If
            Next
            If Not bln2PMSlotStillAvailable Then Assert.Fail("The test could not be run because the 2PM slot was not returned in the available appointment times list from the call to GetCarrierAvailableAppointments(). Unit Test Failed (CarrierAvailabilityPassDelTest)")
        End If
    End Sub


    ''' <summary>
    ''' Dock Configuration - 1 dock has the 2pm timeslot available but the others do not
    ''' Get the available times slots and verify that 2pm is in the results set
    ''' Create a dummy appt in the 2pm timeslot for the open dock to simulate a concurrency issue
    ''' Call BookCarrierAppointment() for the 2pm slot -- it should fail
    ''' </summary>
    <TestMethod()>
    Public Sub BookCarrierAppointmentFailPickTest()
        'Set up the test connection
        setupTestConnction("London Postmaster", 2)

        Dim oAMS As New DAL.NGLAMSAppointmentData(testParameters)
        Dim oDock As New DAL.NGLDockSettingData(testParameters)

        'Set the parameters for the test record selection
        Dim SHID As String = "AMS-12-1119"
        Dim blnIsPickup As Boolean = True
        Dim pWarehouseControl As Integer = 50
        Dim targetDate As Date
        Date.TryParse("8/17/2020 12:00:00 AM", targetDate)

        'Get the test record
        Dim d = oAMS.GetAMSCarrierPickNeedApptBySHID(SHID, pWarehouseControl, targetDate)
        If d?.Count() < 1 Then Assert.Fail("No Pickups were returned using SHID: " + SHID + ", WarehouseControl: " + pWarehouseControl.ToString() + ", and LoadDate: " + targetDate.ToString() + "Unit Test Failed (BookCarrierAppointmentFailPickTest)")
        Dim BookControl = d(0).BookControl
        Dim CarrierControl = d(0).BookCarrierControl
        Dim CompControl = d(0).BookOrigCompControl.Value
        Dim LoadDate = d(0).BookDateLoad.Value
        Dim RequiredDate = d(0).BookDateRequired.Value
        Dim EquipID = d(0).BookCarrTrailerNo
        Dim Inbound = d(0).Inbound.Value
        Dim Warehouse = d(0).Warehouse
        Dim CarrierName = d(0).CarrierName
        Dim CarrierNumber = d(0).CarrierNumber.Value
        Dim ScheduledDate = d(0).ScheduledDate
        Dim ScheduledTime = d(0).ScheduledTime

        Dim time = New TimeSpan(14, 0, 0)
        Dim t2PM As Date
        t2PM = targetDate.Date + time 'Create a 2PM time on the target date

        time = New TimeSpan(15, 59, 0)
        Dim t359PM As Date
        t359PM = targetDate.Date + time 'Create a 359PM time on the target date

        '***** Create dummy appointments in the 2pm time slot for docks 2 and 3 *****
        Dim d1Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Outbound 1")
        Dim d3Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Outbound 3")

        '***** Get available times slots and verify that 2pm is available *****
        Dim oTData = oDock.GetCarrierAvailableAppointments(CarrierControl, CompControl, BookControl, EquipID, LoadDate, RequiredDate, blnIsPickup, Inbound, Warehouse, CarrierName, CarrierNumber, ScheduledDate, ScheduledTime, SHID)
        If Not oTData.blnMustRequestAppt Then
            Dim retVals = oTData.AvailableSlots.ToArray()
            Dim bln2PMSlotStillAvailable = False
            For Each r In retVals
                If Date.Compare(r.StartTime, t2PM) = 0 Then
                    'Yes the 2pm is still available
                    bln2PMSlotStillAvailable = True

                    '***** Create an appt in the 2pm slot for Dock 2 (simulate concurrency issue) *****
                    Dim d2Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Outbound 2")

                    '***** Call BookCarrierAppointment() for the 2pm slot -- it should fail *****
                    Dim ts As New DAL.Models.AMSCarrierAvailableSlots
                    With ts
                        .Books = BookControl.ToString()
                        .CarrierControl = CarrierControl
                        .CarrierName = CarrierName
                        .CarrierNumber = CarrierNumber
                        .CompControl = CompControl
                        .StartTime = t2PM
                        .Warehouse = Warehouse
                    End With

                    Dim strRet = oAMS.BookCarrierAppointment(ts)

                    'Test Cleanup - Delete the dummy appointments
                    oAMS.DeleteAMSAppointment(d1Ctrl, False)
                    oAMS.DeleteAMSAppointment(d2Ctrl, False)
                    oAMS.DeleteAMSAppointment(d3Ctrl, False)

                    If String.IsNullOrWhiteSpace(strRet) Then Assert.Fail("Booking the appointment should have failed because the timeslot was unavailable. Unit Test Failed (BookCarrierAppointmentFailPickTest). " + strRet) Else Trace.WriteLine("Unit Test Passed (BookCarrierAppointmentFailPickTest). Expected Result - Couldn't book because time slot no longer available. Result - " + strRet)

                    Exit For
                End If
            Next
            If Not bln2PMSlotStillAvailable Then Assert.Fail("The test could not be run because the 2PM slot was not returned in the available appointment times list from the call to GetCarrierAvailableAppointments(). Unit Test Failed (BookCarrierAppointmentFailPickTest)")
        End If
    End Sub

    ''' <summary>
    ''' Dock Configuration - 1 dock has the 2pm timeslot available but the others do not
    ''' Get the available times slots and verify that 2pm is in the results set
    ''' Create a dummy appt in the 2pm timeslot for the open dock to simulate a concurrency issue
    ''' Call BookCarrierAppointment() for the 2pm slot -- it should fail
    ''' </summary>
    <TestMethod()>
    Public Sub BookCarrierAppointmentFailDelTest()
        Try
            Dim source As String = "BookCarrierAppointmentFailDelTest"

            'Set up the test connection
            setupTestConnction("London Postmaster", 2)

            Dim oAMS As New DAL.NGLAMSAppointmentData(testParameters)
            Dim oDock As New DAL.NGLDockSettingData(testParameters)

            'Set the parameters for the test record selection
            Dim SHID As String = "AMS-12-1120"
            Dim blnIsPickup As Boolean = False
            Dim pWarehouseControl As Integer = 50
            Dim targetDate As Date
            Date.TryParse("8/21/2020 12:00:00 AM", targetDate)

            'Get the test record
            Dim d = oAMS.GetAMSCarrierDelNeedApptBySHID(SHID, pWarehouseControl, targetDate)
            If d?.Count() < 1 Then Assert.Fail(String.Format("No Deliveries were returned using SHID: {0}, WarehouseControl: {1}, and RequiredDate: {2}. Unit Test Failed ({3})", SHID, pWarehouseControl.ToString(), targetDate.ToString(), source))
            Dim BookControl = d(0).BookControl
            Dim CarrierControl = d(0).BookCarrierControl
            Dim CompControl = d(0).BookDestCompControl.Value
            Dim LoadDate = d(0).BookDateLoad.Value
            Dim RequiredDate = d(0).BookDateRequired.Value
            Dim EquipID = d(0).BookCarrTrailerNo
            Dim Inbound = d(0).Inbound.Value
            Dim Warehouse = d(0).Warehouse
            Dim CarrierName = d(0).CarrierName
            Dim CarrierNumber = d(0).CarrierNumber.Value
            Dim ScheduledDate = d(0).ScheduledDate
            Dim ScheduledTime = d(0).ScheduledTime

            Dim time = New TimeSpan(14, 0, 0)
            Dim t2PM As Date
            t2PM = targetDate.Date + time 'Create a 2PM time on the target date

            time = New TimeSpan(15, 59, 0)
            Dim t359PM As Date
            t359PM = targetDate.Date + time 'Create a 359PM time on the target date

            '***** Create dummy appointments in the 2pm time slot for docks 2 and 3 *****
            Dim d1Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Inbound 1")
            Dim d3Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Inbound 3")

            '***** Get available times slots and verify that 2pm is available *****
            Dim oTData = oDock.GetCarrierAvailableAppointments(CarrierControl, CompControl, BookControl, EquipID, LoadDate, RequiredDate, blnIsPickup, Inbound, Warehouse, CarrierName, CarrierNumber, ScheduledDate, ScheduledTime, SHID)
            If Not oTData.blnMustRequestAppt Then
                Dim retVals = oTData.AvailableSlots.ToArray()
                Dim bln2PMSlotStillAvailable = False
                For Each r In retVals
                    If Date.Compare(r.StartTime, t2PM) = 0 Then
                        'Yes the 2pm is still available
                        bln2PMSlotStillAvailable = True

                        '***** Create an appt in the 2pm slot for Dock 2 (simulate concurrency issue) *****
                        Dim d2Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Inbound 2")

                        '***** Call BookCarrierAppointment() for the 2pm slot -- it should fail *****
                        Dim ts As New DAL.Models.AMSCarrierAvailableSlots
                        With ts
                            .Books = BookControl.ToString()
                            .CarrierControl = CarrierControl
                            .CarrierName = CarrierName
                            .CarrierNumber = CarrierNumber
                            .CompControl = CompControl
                            .StartTime = t2PM
                            .Warehouse = Warehouse
                        End With

                        Dim strRet = oAMS.BookCarrierAppointment(ts)

                        'Test Cleanup - Delete the dummy appointments
                        oAMS.DeleteAMSAppointment(d1Ctrl, False)
                        oAMS.DeleteAMSAppointment(d2Ctrl, False)
                        oAMS.DeleteAMSAppointment(d3Ctrl, False)

                        If String.IsNullOrWhiteSpace(strRet) Then Assert.Fail(String.Format("Booking the appointment should have failed because the timeslot was unavailable. Unit Test Failed ({0}). {1}", source, strRet)) Else Trace.WriteLine(String.Format("Unit Test Passed ({0}). Expected Result - Couldn't book because time slot no longer available. Result - {1}", source, strRet))

                        Exit For
                    End If
                Next
                If Not bln2PMSlotStillAvailable Then Assert.Fail(String.Format("The test could not be run because the 2PM slot was not returned in the available appointment times list from the call to GetCarrierAvailableAppointments(). Unit Test Failed ({0})"), source)
            End If
        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try
    End Sub


    ''' <summary>
    ''' Dock Configuration - All docks have 2pm timeslot available
    ''' Get the available times slots and verify that 2pm is in the results set
    ''' Call BookCarrierAppointment() for the 2pm slot -- it should pass
    ''' Verify the appointment was created
    ''' </summary>
    <TestMethod()>
    Public Sub BookCarrierAppointmentPassPickTest()
        Dim source As String = "BookCarrierAppointmentPassPickTest"

        'Set up the test connection
        setupTestConnction("London Postmaster", 2)

        Dim oAMS As New DAL.NGLAMSAppointmentData(testParameters)
        Dim oDock As New DAL.NGLDockSettingData(testParameters)

        'Set the parameters for the test record selection
        Dim SHID As String = "AMS-12-1119"
        Dim blnIsPickup As Boolean = True
        Dim pWarehouseControl As Integer = 50
        Dim targetDate As Date
        Date.TryParse("8/17/2020 12:00:00 AM", targetDate)

        'Get the test record
        Dim d = oAMS.GetAMSCarrierPickNeedApptBySHID(SHID, pWarehouseControl, targetDate)
        If d?.Count() < 1 Then Assert.Fail(String.Format("No Pickups were returned using SHID: {0}, WarehouseControl: {1}, and LoadDate: {2}. Unit Test Failed ({3})", SHID, pWarehouseControl.ToString(), targetDate.ToString(), source))
        Dim BookControl = d(0).BookControl
        Dim CarrierControl = d(0).BookCarrierControl
        Dim CompControl = d(0).BookOrigCompControl.Value
        Dim LoadDate = d(0).BookDateLoad.Value
        Dim RequiredDate = d(0).BookDateRequired.Value
        Dim EquipID = d(0).BookCarrTrailerNo
        Dim Inbound = d(0).Inbound.Value
        Dim Warehouse = d(0).Warehouse
        Dim CarrierName = d(0).CarrierName
        Dim CarrierNumber = d(0).CarrierNumber.Value
        Dim ScheduledDate = d(0).ScheduledDate
        Dim ScheduledTime = d(0).ScheduledTime

        Dim time = New TimeSpan(14, 0, 0)
        Dim t2PM As Date
        t2PM = targetDate.Date + time 'Create a 2PM time on the target date

        '***** Get available times slots and verify that 2pm is available *****
        Dim oTData = oDock.GetCarrierAvailableAppointments(CarrierControl, CompControl, BookControl, EquipID, LoadDate, RequiredDate, blnIsPickup, Inbound, Warehouse, CarrierName, CarrierNumber, ScheduledDate, ScheduledTime, SHID)
        If Not oTData.blnMustRequestAppt Then
            Dim retVals = oTData.AvailableSlots.ToArray()
            Dim bln2PMSlotStillAvailable = False
            For Each r In retVals
                If Date.Compare(r.StartTime, t2PM) = 0 Then
                    'Yes the 2pm is still available
                    bln2PMSlotStillAvailable = True

                    '***** Call book carrier appt For the 2pm slot -- it should pass *****
                    Dim ts As New DAL.Models.AMSCarrierAvailableSlots
                    With ts
                        .Books = BookControl.ToString()
                        .CarrierControl = CarrierControl
                        .CarrierName = CarrierName
                        .CarrierNumber = CarrierNumber
                        .CompControl = CompControl
                        .StartTime = t2PM
                        .Warehouse = Warehouse
                    End With

                    Dim strRet = oAMS.BookCarrierAppointment(ts)

                    If Not String.IsNullOrWhiteSpace(strRet) Then
                        Assert.Fail(String.Format("Booking the appointment should have succeeded because the timeslot is available. Unit Test Failed ({0} - {1})", source, strRet))
                    Else
                        Dim AptControl = oAMS.VerifyAppointmentExists(BookControl, blnIsPickup, CompControl, Nothing, t2PM)
                        If AptControl > 0 Then
                            Trace.WriteLine(String.Format("Unit Test Passed ({0}). An appointment was created.", source))
                            'Test Cleanup - Delete the appointment
                            oAMS.DeleteAMSAppointment(AptControl, False)
                        Else
                            Assert.Fail(String.Format("Appointment creation could not be verified using BookControl: {0}, IsPickup: {1}, CompControl: {2}, DockID: {3}, and StartTime: {4}. Unit Test Failed ({5} - {6})", BookControl, blnIsPickup, CompControl, Nothing, t2PM.ToString(), source, strRet))
                        End If
                    End If

                    Exit For
                End If
            Next
            If Not bln2PMSlotStillAvailable Then Assert.Fail(String.Format("The test could not be run because the 2PM slot was not returned in the available appointment times list from the call to GetCarrierAvailableAppointments(). Unit Test Failed ({0})", source))
        End If
    End Sub


    ''' <summary>
    ''' Dock Configuration - 2 docks have the 2pm timeslot available but the other does not
    ''' Get the available times slots and verify that 2pm is in the results set
    ''' Call BookCarrierAppointment() for the 2pm slot -- it should pass
    ''' Verify the appointment was created on dock with lowest sequence
    ''' </summary>
    <TestMethod()>
    Public Sub BookCarrierAppointmentDockSeqTest()
        Dim source As String = "BookCarrierAppointmentDockSeqTest"

        'Set up the test connection
        setupTestConnction("London Postmaster", 2)

        Dim oAMS As New DAL.NGLAMSAppointmentData(testParameters)
        Dim oDock As New DAL.NGLDockSettingData(testParameters)

        'Set the parameters for the test record selection
        Dim SHID As String = "AMS-12-1119"
        Dim blnIsPickup As Boolean = True
        Dim pWarehouseControl As Integer = 50
        Dim targetDate As Date
        Date.TryParse("8/17/2020 12:00:00 AM", targetDate)

        'Get the test record
        Dim d = oAMS.GetAMSCarrierPickNeedApptBySHID(SHID, pWarehouseControl, targetDate)
        If d?.Count() < 1 Then Assert.Fail(String.Format("No Pickups were returned using SHID: {0}, WarehouseControl: {1}, and LoadDate: {2}. Unit Test Failed ({3})", SHID, pWarehouseControl.ToString(), targetDate.ToString(), source))
        Dim BookControl = d(0).BookControl
        Dim CarrierControl = d(0).BookCarrierControl
        Dim CompControl = d(0).BookOrigCompControl.Value
        Dim LoadDate = d(0).BookDateLoad.Value
        Dim RequiredDate = d(0).BookDateRequired.Value
        Dim EquipID = d(0).BookCarrTrailerNo
        Dim Inbound = d(0).Inbound.Value
        Dim Warehouse = d(0).Warehouse
        Dim CarrierName = d(0).CarrierName
        Dim CarrierNumber = d(0).CarrierNumber.Value
        Dim ScheduledDate = d(0).ScheduledDate
        Dim ScheduledTime = d(0).ScheduledTime

        Dim time = New TimeSpan(14, 0, 0)
        Dim t2PM As Date
        t2PM = targetDate.Date + time 'Create a 2PM time on the target date

        time = New TimeSpan(15, 59, 0)
        Dim t359PM As Date
        t359PM = targetDate.Date + time 'Create a 359PM time on the target date

        '***** Create dummy appointments in the 2pm time slot for dock 1 *****
        Dim d1Ctrl = createDummyAppointment(CompControl, CarrierControl, CarrierNumber, CarrierName, t2PM, t359PM, "Outbound 1")

        '***** Get available times slots and verify that 2pm is available *****
        Dim oTData = oDock.GetCarrierAvailableAppointments(CarrierControl, CompControl, BookControl, EquipID, LoadDate, RequiredDate, blnIsPickup, Inbound, Warehouse, CarrierName, CarrierNumber, ScheduledDate, ScheduledTime, SHID)
        If Not oTData.blnMustRequestAppt Then
            Dim retVals = oTData.AvailableSlots.ToArray()
            Dim bln2PMSlotStillAvailable = False
            For Each r In retVals
                If Date.Compare(r.StartTime, t2PM) = 0 Then
                    'Yes the 2pm is still available
                    bln2PMSlotStillAvailable = True

                    '***** Call book carrier appt For the 2pm slot -- it should pass *****
                    Dim ts As New DAL.Models.AMSCarrierAvailableSlots
                    With ts
                        .Books = BookControl.ToString()
                        .CarrierControl = CarrierControl
                        .CarrierName = CarrierName
                        .CarrierNumber = CarrierNumber
                        .CompControl = CompControl
                        .StartTime = t2PM
                        .Warehouse = Warehouse
                    End With

                    Dim strRet = oAMS.BookCarrierAppointment(ts)

                    If Not String.IsNullOrWhiteSpace(strRet) Then
                        Assert.Fail(String.Format("Booking the appointment should have succeeded because the timeslot is available. Unit Test Failed ({0} - {1})", source, strRet))
                    Else
                        'The appointment should have been booked on dock 2 because it has the lowest sequence
                        Dim AptControl = oAMS.VerifyAppointmentExists(BookControl, blnIsPickup, CompControl, "Outbound 2", t2PM)
                        If AptControl > 0 Then
                            Trace.WriteLine(String.Format("Unit Test Passed ({0}). An appointment was created on the dock with the lowest sequence.", source))
                            'Test Cleanup - Delete the appointments
                            oAMS.DeleteAMSAppointment(AptControl, False)
                            oAMS.DeleteAMSAppointment(d1Ctrl, False)
                        Else
                            Assert.Fail(String.Format("Appointment creation could not be verified using BookControl: {0}, IsPickup: {1}, CompControl: {2}, DockID: {3}, and StartTime: {4}. Unit Test Failed ({5} - {6})", BookControl, blnIsPickup, CompControl, Nothing, t2PM.ToString(), source, strRet))
                        End If
                    End If

                    Exit For
                End If
            Next
            If Not bln2PMSlotStillAvailable Then Assert.Fail(String.Format("The test could not be run because the 2PM slot was not returned in the available appointment times list from the call to GetCarrierAvailableAppointments(). Unit Test Failed ({0})", source))
        End If
    End Sub


End Class