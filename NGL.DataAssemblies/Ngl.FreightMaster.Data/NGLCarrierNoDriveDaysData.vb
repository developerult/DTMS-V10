Imports System.ServiceModel

Public Class NGLCarrierNoDriveDaysData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierNoDriveDays
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierNoDriveDaysData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierNoDriveDays
                Me.LinqDB = db
            End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function


    Public Function GetCarrierNoDriveDays(ByVal iCarrierControl As Integer) As LTS.CarrierNoDriveDay()
        Dim oRet() As LTS.CarrierNoDriveDay

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                oRet = db.CarrierNoDriveDays.Where(Function(x) x.CarrierNDDCarrierControl = iCarrierControl).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierNoDriveDays"), db)
            End Try
        End Using
        Return oRet
    End Function


    Public Function GetCarrierNoDriveDays(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.CarrierNoDriveDay()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.CarrierNoDriveDay

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.CarrierNoDriveDay)
                iQuery = db.CarrierNoDriveDays
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierNoDriveDays"), db)
            End Try
        End Using
        Return oRet
    End Function

    Public Function SaveOrCreateCarrierNoDriveDays(ByVal oData As LTS.CarrierNoDriveDay, Optional ByVal iCaControl As Integer = 0) As Boolean
        Dim blnRet As Boolean = False
        Dim iLECarControl As Integer
        Dim iCarrierControl As Integer
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim iCarrierNDDCarrierControl = oData.CarrierNDDCarrierControl

                If oData.CarrierNDDCarrierControl = 0 Then
                    If oData.CarrierNDDControl = 0 Then
                        Dim sMsg As String = "E_MissingParent" ' " The reference to the parent record is missing. Please select a valid parent record and try again."
                        throwNoDataFaultException(sMsg)
                    End If
                End If
                ' need to lookup carrier control number from tblLegalEntityCarriers
                iLECarControl = oData.CarrierNDDCarrierControl
                iCarrierControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = iLECarControl).Select(Function(x) x.LECarCarrierControl).FirstOrDefault()

                If iCarrierControl = 0 Then
                    throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                End If
                oData.CarrierNDDCarrierControl = iCarrierControl
                oData.CarrierNDDModDate = Date.Now()
                oData.CarrierNDDModUser = Me.Parameters.UserName

                If oData.CarrierNDDControl = 0 Then
                    db.CarrierNoDriveDays.InsertOnSubmit(oData)
                Else
                    db.CarrierNoDriveDays.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateCarrierNoDriveDays"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Deletes the CarrierNoDriveDay record
    ''' </summary>
    ''' <param name="iCarrierNDDControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 11/29/2022 
    ''' </remarks>
    Public Function DeleteCarrierNoDriveDay(ByVal iCarrierNDDControl As Integer) As Boolean

        Dim blnRet As Boolean = False
        If iCarrierNDDControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify that the record exists
                Dim oExisting = db.CarrierNoDriveDays.Where(Function(x) x.CarrierNDDControl = iCarrierNDDControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.CarrierNDDControl = 0 Then Return True
                db.CarrierNoDriveDays.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierNoDriveDay"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"

    ''' <summary>
    ''' DTO Not supported
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object

        Return Nothing

    End Function

    ''' <summary>
    ''' DTO is not supported
    ''' </summary>
    ''' <param name="LinqTable"></param>
    ''' <returns></returns>
    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function


#End Region

End Class