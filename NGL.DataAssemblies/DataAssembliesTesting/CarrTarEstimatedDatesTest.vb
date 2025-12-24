Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports Ngl.FM.CarTar
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports BLL = Ngl.FM.BLL
<TestClass()> Public Class CarrTarEstimatedDatesTest
    Inherits TestBase

#Region "Estimated Delivery Dates Test"


    ''' <summary>
    '''  Example test 1:
    '''  Load/Ship date: Friday 05/16/2014
    '''  No Drive Saturday: true
    '''  No Drive Sunday: true
    '''  Holidays: None
    '''  Transit Days: 3
    '''  EDD = Wednesday 05/21/2014
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcEstimatedDelDateEx1Test()
        Dim expectedEDD1 As Nullable(Of DateTime) = New DateTime(2014, 5, 21)
        Dim expectedEDD2 As Nullable(Of DateTime) = New DateTime(2014, 5, 20)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateLoad = New DateTime(2014, 5, 16)
            'it should take the greatest load date
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateLoad = New DateTime(2014, 5, 15)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 3
            pivot1.CarrTarWillDriveSunday = False
            pivot1.CarrTarWillDriveSaturday = False
            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 2
            pivot2.CarrTarWillDriveSunday = False
            pivot2.CarrTarWillDriveSaturday = False
            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList. none
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()

            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcEstimatedDeliveryDate()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookExpDelDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expectedEDD1.Value, arrayResult(0).BookExpDelDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookExpDelDateTime, "arrayResult[0] is null")

            Assert.AreEqual(expectedEDD2.Value, arrayResult(1).BookExpDelDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcEstimatedDelDateEx1. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    ''' <summary>
    ''' Load/Ship date: Friday 05/16/2014
    '''No Drive Saturday: false (they will drive on Saturday)
    '''No Drive Sunday: true
    '''Holidays: None
    '''Transit Days: 3 
    '''EDD = Tuesday 05/20/2014 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcEstimatedDelDateEx2GreatestLoadDateTest()
        Dim expectedEDD1 As Nullable(Of DateTime) = New DateTime(2014, 5, 20)
        Dim expectedEDD2 As Nullable(Of DateTime) = New DateTime(2014, 5, 19)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateLoad = New DateTime(2014, 5, 16)
            'it should take the greatest load date
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateLoad = New DateTime(2014, 5, 15)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 3
            pivot1.CarrTarWillDriveSaturday = True
            pivot1.CarrTarWillDriveSunday = False

            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 2
            pivot2.CarrTarWillDriveSaturday = True
            pivot2.CarrTarWillDriveSunday = False

            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList. none
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()

            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcEstimatedDeliveryDate()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookExpDelDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expectedEDD1.Value, arrayResult(0).BookExpDelDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookExpDelDateTime, "arrayResult[0] is null")

            Assert.AreEqual(expectedEDD2.Value, arrayResult(1).BookExpDelDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcEstimatedDelDateEx2. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    ''' <summary>
    '''     Load/Ship date: Friday 05/16/2014
    '''No Drive Saturday: true
    '''No Drive Sunday: true
    '''Holidays: Monday 05/19/2014
    '''Transit Days: 3
    '''   EDD = Thursday 05/22/2014 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcEstimatedDelDateEx3Test()
        Dim expectedEDD1 As Nullable(Of DateTime) = New DateTime(2014, 5, 22)
        Dim expectedEDD2 As Nullable(Of DateTime) = New DateTime(2014, 5, 21)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateLoad = New DateTime(2014, 5, 16)
            'it should take the greatest load date
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateLoad = New DateTime(2014, 5, 15)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 3
            pivot1.CarrTarWillDriveSaturday = False
            pivot1.CarrTarWillDriveSunday = False
            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 2
            pivot2.CarrTarWillDriveSaturday = False
            pivot2.CarrTarWillDriveSunday = False
            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList.
            Dim noDriveDay As New DTO.CarrTarNoDriveDays()
            noDriveDay.CarrTarNDDNoDrivingDate = New DateTime(2014, 5, 19)
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()
            NoDriveDaysList.Add(noDriveDay)

            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcEstimatedDeliveryDate()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookExpDelDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expectedEDD1.Value, arrayResult(0).BookExpDelDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookExpDelDateTime, "arrayResult[0] is null")

            Assert.AreEqual(expectedEDD2.Value, arrayResult(1).BookExpDelDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcEstimatedDelDateEx3. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    ''' <summary>
    '''     Load/Ship date: Friday 07/31/2015 06:57 AM
    ''' Drive Saturday: false
    ''' Drive Sunday: true
    '''Holidays: None
    '''Transit Days: 1
    '''   EDD = Monday 08/3/2015 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcEstimatedDelDateEx4Test()
        Dim expectedEDD1 As Nullable(Of DateTime) = New DateTime(2015, 8, 3)
        Dim expectedEDD2 As Nullable(Of DateTime) = New DateTime(2015, 8, 3)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateLoad = New DateTime(2015, 7, 31)
            'it should take the greatest load date
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateLoad = New DateTime(2015, 7, 31)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 1
            pivot1.CarrTarWillDriveSaturday = False
            pivot1.CarrTarWillDriveSunday = False
            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 1
            pivot2.CarrTarWillDriveSaturday = False
            pivot2.CarrTarWillDriveSunday = False
            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList. 
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()


            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcEstimatedDeliveryDate()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookExpDelDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expectedEDD1.Value, arrayResult(0).BookExpDelDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookExpDelDateTime, "arrayResult[0] is null")

            Assert.AreEqual(expectedEDD2.Value, arrayResult(1).BookExpDelDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcEstimatedDelDateEx3. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub


    ''' <summary>
    '''     Load/Ship date: Saturday 08/1/2015 06:57 AM
    ''' Drive Saturday: false
    ''' Drive Sunday: true
    '''Holidays: None
    '''Transit Days: 1
    '''   EDD = Sunday 08/2/2015 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcEstimatedDelDateEx5Test()
        Dim expectedEDD1 As Nullable(Of DateTime) = New DateTime(2015, 8, 2)
        Dim expectedEDD2 As Nullable(Of DateTime) = New DateTime(2015, 8, 2)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateLoad = New DateTime(2015, 8, 1)
            'it should take the greatest load date
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateLoad = New DateTime(2015, 8, 1)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 1
            pivot1.CarrTarWillDriveSaturday = False
            pivot1.CarrTarWillDriveSunday = True
            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 1
            pivot2.CarrTarWillDriveSaturday = False
            pivot2.CarrTarWillDriveSunday = True
            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList. 
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()


            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcEstimatedDeliveryDate()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookExpDelDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expectedEDD1.Value, arrayResult(0).BookExpDelDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookExpDelDateTime, "arrayResult[0] is null")

            Assert.AreEqual(expectedEDD2.Value, arrayResult(1).BookExpDelDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcEstimatedDelDateEx3. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    ''' <summary>
    '''     Load/Ship date: Saturday 07/31/2015 06:57 AM
    ''' Drive Saturday: false
    ''' Drive Sunday: true
    '''Holidays: None
    '''Transit Days: 1
    '''   EDD = Sunday 08/2/2015 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcEstimatedDelDateEx6Test()
        Dim expectedEDD1 As Nullable(Of DateTime) = New DateTime(2015, 8, 2)
        Dim expectedEDD2 As Nullable(Of DateTime) = New DateTime(2015, 8, 2)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateLoad = New DateTime(2015, 7, 31)
            'it should take the greatest load date
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateLoad = New DateTime(2015, 7, 31)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 1
            pivot1.CarrTarWillDriveSaturday = False
            pivot1.CarrTarWillDriveSunday = True
            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 1
            pivot2.CarrTarWillDriveSaturday = False
            pivot2.CarrTarWillDriveSunday = True
            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList. 
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()


            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcEstimatedDeliveryDate()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookExpDelDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expectedEDD1.Value, arrayResult(0).BookExpDelDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookExpDelDateTime, "arrayResult[0] is null")

            Assert.AreEqual(expectedEDD2.Value, arrayResult(1).BookExpDelDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcEstimatedDelDateEx3. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    ''' <summary>
    '''     Load/Ship date: Thursday 07/30/2015 06:57 AM
    ''' Drive Saturday: false
    ''' Drive Sunday: false
    '''Holidays: None
    '''Transit Days: 1
    '''   EDD = Friday 07/31/2015 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcEstimatedDelDateEx7Test()
        Dim expectedEDD1 As Nullable(Of DateTime) = New DateTime(2015, 7, 31)
        Dim expectedEDD2 As Nullable(Of DateTime) = New DateTime(2015, 7, 31)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateLoad = New DateTime(2015, 7, 30)
            'it should take the greatest load date
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateLoad = New DateTime(2015, 7, 30)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 1
            pivot1.CarrTarWillDriveSaturday = False
            pivot1.CarrTarWillDriveSunday = True
            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 1
            pivot2.CarrTarWillDriveSaturday = False
            pivot2.CarrTarWillDriveSunday = True
            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList. 
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()


            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcEstimatedDeliveryDate()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookExpDelDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expectedEDD1.Value, arrayResult(0).BookExpDelDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookExpDelDateTime, "arrayResult[0] is null")

            Assert.AreEqual(expectedEDD2.Value, arrayResult(1).BookExpDelDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcEstimatedDelDateEx3. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    ''' <summary>
    '''     Load/Ship date: Friday 07/31/2015 06:57 AM
    ''' Drive Saturday: false
    ''' Drive Sunday: false
    '''Holidays: None
    '''Transit Days: 2
    '''   EDD = Tuesday 08/4/2015 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcEstimatedDelDateEx8Test()
        Dim expectedEDD1 As Nullable(Of DateTime) = New DateTime(2015, 8, 4)
        Dim expectedEDD2 As Nullable(Of DateTime) = New DateTime(2015, 8, 4)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateLoad = New DateTime(2015, 7, 31)
            'it should take the greatest load date
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateLoad = New DateTime(2015, 7, 31)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 2
            pivot1.CarrTarWillDriveSaturday = False
            pivot1.CarrTarWillDriveSunday = False
            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 2
            pivot2.CarrTarWillDriveSaturday = False
            pivot2.CarrTarWillDriveSunday = False
            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList. 
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()


            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcEstimatedDeliveryDate()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookExpDelDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expectedEDD1.Value, arrayResult(0).BookExpDelDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookExpDelDateTime, "arrayResult[0] is null")

            Assert.AreEqual(expectedEDD2.Value, arrayResult(1).BookExpDelDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcEstimatedDelDateEx3. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    ''' <summary>
    '''     Load/Ship date: Friday 07/31/2015 06:57 AM
    ''' Drive Saturday: false
    ''' Drive Sunday: false
    '''Holidays: None
    '''Transit Days: 2
    '''   EDD = Monday 08/3/2015 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcEstimatedDelDateEx9Test()
        Dim expectedEDD1 As Nullable(Of DateTime) = New DateTime(2015, 8, 3)
        Dim expectedEDD2 As Nullable(Of DateTime) = New DateTime(2015, 8, 3)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateLoad = New DateTime(2015, 7, 30)
            'it should take the greatest load date
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateLoad = New DateTime(2015, 7, 30)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 2
            pivot1.CarrTarWillDriveSaturday = False
            pivot1.CarrTarWillDriveSunday = False
            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 2
            pivot2.CarrTarWillDriveSaturday = False
            pivot2.CarrTarWillDriveSunday = False
            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList. 
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()


            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcEstimatedDeliveryDate()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookExpDelDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expectedEDD1.Value, arrayResult(0).BookExpDelDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookExpDelDateTime, "arrayResult[0] is null")

            Assert.AreEqual(expectedEDD2.Value, arrayResult(1).BookExpDelDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcEstimatedDelDateEx3. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    ''' <summary>
    '''     Load/Ship date: Friday 07/31/2015 06:57 AM
    ''' Drive Saturday: false
    ''' Drive Sunday: false
    '''Holidays: None
    '''Transit Days: 1
    '''   EDD = Monday 08/3/2015 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcEstimatedDelDateEx10Test()
        Dim expectedEDD1 As Nullable(Of DateTime) = New DateTime(2015, 8, 3)
        Dim expectedEDD2 As Nullable(Of DateTime) = New DateTime(2015, 8, 3)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateLoad = New DateTime(2015, 7, 31)
            'it should take the greatest load date
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateLoad = New DateTime(2015, 7, 31)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 1
            pivot1.CarrTarWillDriveSaturday = False
            pivot1.CarrTarWillDriveSunday = False
            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 1
            pivot2.CarrTarWillDriveSaturday = False
            pivot2.CarrTarWillDriveSunday = False
            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList. 
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()


            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcEstimatedDeliveryDate()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookExpDelDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expectedEDD1.Value, arrayResult(0).BookExpDelDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookExpDelDateTime, "arrayResult[0] is null")

            Assert.AreEqual(expectedEDD2.Value, arrayResult(1).BookExpDelDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcEstimatedDelDateEx3. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

#End Region

#Region "Must Leave By Dates Test"

    ''' <summary>
    '''   Ex 1:
    ''' Required date: Tuesday 05/20/2014
    ''' No Drive Saturday: true
    ''' No Drive Sunday: true
    ''' Holidays: None
    ''' Transit Days: 3 
    ''' Must Leave By = Thursday 05/15/2014 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcMustLeaveByDateEx1Test()
        Dim expected1 As Nullable(Of DateTime) = New DateTime(2014, 5, 15)
        Dim expected2 As Nullable(Of DateTime) = New DateTime(2014, 5, 16)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateRequired = New DateTime(2014, 5, 20)
            'thi is the require date we will use.
            rev1.BookDateLoad = New DateTime(2014, 5, 20)
            rev1.BookStopNo = 2
            'set this up as the last stop (final destination)
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateRequired = New DateTime(2014, 5, 21)
            rev2.BookDateLoad = New DateTime(2014, 5, 21)
            rev2.BookStopNo = 1
            'set this up as the last stop (final destination)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 3
            pivot1.CarrTarWillDriveSunday = False
            pivot1.CarrTarWillDriveSaturday = False
            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 2
            pivot2.CarrTarWillDriveSunday = False
            pivot2.CarrTarWillDriveSaturday = False
            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList. none
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()

            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcMustLeaveBy()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookMustLeaveByDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expected1.Value, arrayResult(0).BookMustLeaveByDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookMustLeaveByDateTime, "arrayResult[0] is null")

            Assert.AreEqual(expected2.Value, arrayResult(1).BookMustLeaveByDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcMustLeaveByDateEx1Test. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    ''' <summary>
    ''' Ex 2:
    ''' Required date: Tuesday 05/20/2014
    ''' No Drive Saturday: false (they will drive on Saturday)
    ''' No Drive Sunday: true
    ''' Holidays: None
    ''' Transit Days: 3
    ''' Must Leave ByLB = Friday 05/16/2014 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcMustLeaveByDateEx2Test()
        Dim expected1 As Nullable(Of DateTime) = New DateTime(2014, 5, 16)
        Dim expected2 As Nullable(Of DateTime) = New DateTime(2014, 5, 17)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateRequired = New DateTime(2014, 5, 20)
            rev1.BookDateLoad = New DateTime(2014, 5, 20)
            rev1.BookStopNo = 2
            'set this up as the last stop (final destination)
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateRequired = New DateTime(2014, 5, 21)
            rev2.BookDateLoad = New DateTime(2014, 5, 21)
            rev2.BookStopNo = 1
            'set this up as the last stop (final destination)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 3
            pivot1.CarrTarWillDriveSunday = False
            pivot1.CarrTarWillDriveSaturday = True
            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 2
            pivot2.CarrTarWillDriveSunday = False
            pivot2.CarrTarWillDriveSaturday = True
            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList. none
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()

            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcMustLeaveBy()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookMustLeaveByDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expected1.Value, arrayResult(0).BookMustLeaveByDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookMustLeaveByDateTime, "arrayResult[0] is null")

            Assert.AreEqual(expected2.Value, arrayResult(1).BookMustLeaveByDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcMustLeaveByDateEx2Test. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

    ''' <summary>
    '''  Ex 3:
    ''' Required date: Tuesday 05/20/2014
    ''' No Drive Saturday: true
    ''' No Drive Sunday: true
    ''' Holidays: Monday 05/19/2014
    ''' Transit Days: 3 
    ''' Must Leave By = Wednesday 05/14/2014 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub CalcMustLeaveByDateEx3Test()
        Dim expected1 As Nullable(Of DateTime) = New DateTime(2014, 5, 14)
        Dim expected2 As Nullable(Of DateTime) = New DateTime(2014, 5, 15)
        Try
            'setup the load ship dates.
            Dim revs As New List(Of DTO.BookRevenue)()
            Dim rev1 As New DTO.BookRevenue()
            rev1.BookDateLoad = New DateTime(2014, 5, 20)
            rev1.BookDateRequired = New DateTime(2014, 5, 20)
            rev1.BookStopNo = 2
            'set this up as the last stop (final destination)
            Dim rev2 As New DTO.BookRevenue()
            rev2.BookDateLoad = New DateTime(2014, 5, 15)
            rev2.BookDateRequired = New DateTime(2014, 5, 15)
            rev2.BookStopNo = 1
            'set this up as the last stop (final destination)
            revs.Add(rev1)
            revs.Add(rev2)
            Dim load As New Load(revs.ToArray(), testParameters)

            'set up the pivot data with transit days.  it should take the greatest trans date.
            Dim pivot1 As New DTO.CarriersByCost()
            pivot1.CarrTarEquipMatMaxDays = 3
            pivot1.CarrTarWillDriveSaturday = False
            pivot1.CarrTarWillDriveSunday = False
            Dim pivot2 As New DTO.CarriersByCost()
            pivot2.CarrTarEquipMatMaxDays = 2
            pivot2.CarrTarWillDriveSaturday = False
            pivot2.CarrTarWillDriveSunday = False
            Dim pivots As New List(Of DTO.CarriersByCost)()
            pivots.Add(pivot1)
            pivots.Add(pivot2)

            'setup the NoDriveDaysList.
            Dim noDriveDay As New DTO.CarrTarNoDriveDays()
            noDriveDay.CarrTarNDDNoDrivingDate = New DateTime(2014, 5, 19)
            Dim NoDriveDaysList As New List(Of DTO.CarrTarNoDriveDays)()
            NoDriveDaysList.Add(noDriveDay)

            Dim target As New Ngl.FM.CarTar.CarrTarEstimatedDates(testParameters, load, pivots.ToArray(), NoDriveDaysList)
            target.setupData()
            target.CalcMustLeaveBy()
            Dim arrayResult As DTO.CarriersByCost() = target.GetCarriersByCostCalculated()

            Assert.IsNotNull(arrayResult(0).BookMustLeaveByDateTime, "arrayResult[0] is null")
            Assert.AreEqual(expected1.Value, arrayResult(0).BookMustLeaveByDateTime.Value, "the dates do not match")

            Assert.IsNotNull(arrayResult(1).BookMustLeaveByDateTime, "arrayResult[1] is null")

            Assert.AreEqual(expected2.Value, arrayResult(1).BookMustLeaveByDateTime.Value, "the dates do not match")
        Catch ex As Exception
            Assert.Fail("Unexpected Error For CalcMustLeaveByDateEx3Test. Test: {0} ", ex.Message)
            'place clean up code here
        Finally
        End Try
    End Sub

  
#End Region



End Class
