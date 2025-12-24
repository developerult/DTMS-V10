Imports Ngl.FreightMaster.Integration.Configuration
Imports System.Data.SqlClient
Imports Ngl.FreightMaster.Integration.FMDataTableAdapters

<Serializable()>
Public Class clsCarrierEDIBLL : Inherits clsUpload

#Region " Class Variables and Properties "

    Private _Adapter As CarrierEDITableAdapter
    Protected ReadOnly Property Adapter() As CarrierEDITableAdapter
        Get
            If _Adapter Is Nothing Then
                _Adapter = New CarrierEDITableAdapter
                _Adapter.SetConnectionString(Me.DBConnection)
            End If

            Return _Adapter
        End Get
    End Property
#End Region


#Region " Constructors "



#End Region

#Region " Protected and Private Methods "

    Private _Control As Integer

    Protected Function GetData() As FMData.CarrierEDIDataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetData())
    End Function


    Protected Function GetDataByControl(ByVal Control As Integer) As FMData.CarrierEDIDataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetDataByControl(Control))
    End Function

    Private Function getNextISASequence(ByRef oRow As FMData.CarrierEDIRow) As Integer
        Dim intSeq As Integer = oRow.CarrierEDIISASequence

        Dim oTable As FMData.CarrierEDIDataTable = GetDataByControl(oRow.CarrierEDIControl)
        If Not oTable(0).CarrierEDIISASequence = intSeq Then
            intSeq = oTable(0).CarrierEDIISASequence
        End If
        intSeq += 1
        'reset counter at 99999
        If intSeq > 99999 Then intSeq = 11111
        oRow.CarrierEDIISASequence = intSeq
        Adapter.Update(oRow)
        Return intSeq

    End Function


    Private Function getNextGSSequence(ByRef oRow As FMData.CarrierEDIRow) As Integer
        Dim intSeq As Integer = oRow.CarrierEDIGSSequence

        Dim oTable As FMData.CarrierEDIDataTable = GetDataByControl(oRow.CarrierEDIControl)
        If Not oTable(0).CarrierEDIGSSequence = intSeq Then
            intSeq = oTable(0).CarrierEDIGSSequence
        End If
        intSeq += 1
        'reset counter at 99999
        If intSeq > 99999 Then intSeq = 11111
        oRow.CarrierEDIGSSequence = intSeq
        Adapter.Update(oRow)
        Return intSeq

    End Function




#End Region

#Region " Public Methods "

    Public Function getNextGSSequence(ByRef oGS As clsEDIGS) As Integer
        Dim intSeq As Integer = oGS.GS06

        Dim oTable As FMData.CarrierEDIDataTable = GetDataByControl(oGS.CarrierEDIControl)
        Dim oRow As FMData.CarrierEDIRow = oTable.Rows(0)
        If Not oRow.CarrierEDIGSSequence = intSeq Then
            intSeq = oRow.CarrierEDIGSSequence
        End If
        intSeq += 1
        'reset counter at 99999
        If intSeq > 99999 Then intSeq = 11111
        oRow.CarrierEDIGSSequence = intSeq
        Adapter.Update(oRow)
        Return intSeq

    End Function

    Public Function getNextISASequence(ByRef oISA As clsEDIISA) As Integer
        Dim intSeq As Integer = oISA.ISA13

        Dim oTable As FMData.CarrierEDIDataTable = GetDataByControl(oISA.CarrierEDIControl)
        Dim oRow As FMData.CarrierEDIRow = oTable.Rows(0)
        If Not oRow.CarrierEDIISASequence = intSeq Then
            intSeq = oRow.CarrierEDIISASequence
        End If
        intSeq += 1
        'reset counter at 99999
        If intSeq > 99999 Then intSeq = 11111

        oRow.CarrierEDIISASequence = intSeq
        Adapter.Update(oRow)
        Return intSeq

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)>
    Public Function GetDataByCarrierControl(ByVal CarrierControl As Integer) As FMData.CarrierEDIDataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetDataByCarrierControl(CarrierControl))
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)>
    Public Function GetDataByCarrierControlAction(ByVal CarrierControl As Integer, ByVal Action As String) As FMData.CarrierEDIDataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetDataByCarrierControlAction(CarrierControl, Left(Action, 3)))
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)>
    Public Function GetDataByPartnerCodeAction(ByVal PartnerCode As String, ByVal Action As String) As FMData.CarrierEDIDataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetDataByPartnerCodeAction(Trim(PartnerCode), Left(Action, 3)))
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oISA"></param>
    ''' <param name="oGS"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="Action"></param>
    ''' <param name="blnMapGS03ToISA08"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 08/02/2021 added optional blnMapGS03ToISA08 parameter
    '''   from EDI config setting default = false
    ''' </remarks>
    Public Function fillEDIObjects(ByRef oISA As clsEDIISA, ByRef oGS As clsEDIGS, ByVal CarrierControl As Integer, ByVal Action As String, Optional blnMapGS03ToISA08 As Boolean = False) As Boolean
        Dim blnRet As Boolean = False
        Try
            Dim oTable As FMData.CarrierEDIDataTable = GetDataByCarrierControlAction(CarrierControl, Action)
            'we only read the first data
            Dim oRow As FMData.CarrierEDIRow = oTable(0)
            Dim intISASeq As Integer = getNextISASequence(oRow)
            Dim intGSSeq As Integer = getNextGSSequence(oRow)
            With oRow
                'we use the carrier data as the receiver 
                oISA.CarrierEDIControl = .CarrierEDIControl
                oISA.ISA03 = nz(oRow, "CarrierEDISecurityQual", "00").ToString
                oISA.ISA04 = nz(oRow, "CarrierEDISecurityCode", "").ToString
                oISA.ISA07 = nz(oRow, "CarrierEDIPartnerQual", "02").ToString
                oISA.ISA08 = nz(oRow, "CarrierEDIPartnerCode", "").ToString
                oISA.ISA09 = Date.Now.ToString("yyMMdd")
                oISA.ISA10 = Date.Now.ToString("HHmm")
                oISA.ISA11 = "U"
                oISA.ISA12 = "00401"
                oISA.ISA13 = intISASeq.ToString
                If .CarrierEDIAcknowledgementRequested Then
                    oISA.ISA14 = "1"
                Else
                    oISA.ISA14 = "0"
                End If
                oISA.ISA15 = nz(.CarrierEDITestCode, "P").ToString
                oGS.CarrierEDIControl = .CarrierEDIControl
                'we have included all the supported functional groups but only the 204,210 and 997 are used by this functon
                Select Case Action
                    Case "204"
                        oGS.GS01 = "SM"
                    Case "210"
                        oGS.GS01 = "IM"
                    Case "997"
                        oGS.GS01 = "FA"
                    Case "214"
                        oGS.GS01 = "QM"
                    Case "990"
                        oGS.GS01 = "GF"
                End Select
                If blnMapGS03ToISA08 Then
                    oGS.GS03 = oISA.ISA08
                Else
                    oGS.GS03 = nz(oRow, "CarrierEDIPartnerCode", "").ToString()
                End If
                oGS.GS04 = Date.Now.ToString("yyyyMMdd")
                oGS.GS05 = oISA.ISA10
                oGS.GS06 = intGSSeq
                oGS.GS07 = "X"
                oGS.GS08 = "004010"
            End With
            blnRet = True
        Catch ex As Exception
            LogException("Read EDI Carrier Settings Failure!", "Cannot read EDI settings from Carrier EDI table for carrier control Number " & CarrierControl & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsCarrierEDIBLL.fillEDIObjects Failure")
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oISA"></param>
    ''' <param name="oGS"></param>
    ''' <param name="PartnerCode"></param>
    ''' <param name="Action"></param>
    ''' <param name="blnMapGS03ToISA08"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 08/02/2021 added optional blnMapGS03ToISA08 parameter
    '''   from EDI config setting
    ''' </remarks>
    Public Function fillEDIObjects(ByRef oISA As clsEDIISA, ByRef oGS As clsEDIGS, ByVal PartnerCode As String, ByVal Action As String, ByVal blnMapGS03ToISA08 As Boolean) As Boolean
        Dim blnRet As Boolean = False
        Try
            Dim oTable As FMData.CarrierEDIDataTable = GetDataByPartnerCodeAction(PartnerCode, Action)
            'we only read the first data
            Dim oRow As FMData.CarrierEDIRow = oTable(0)
            Dim intISASeq As Integer = getNextISASequence(oRow)
            Dim intGSSeq As Integer = getNextGSSequence(oRow)
            With oRow
                'we use the carrier data as the receiver 
                oISA.CarrierEDIControl = .CarrierEDIControl
                oISA.ISA03 = nz(oRow, "CarrierEDISecurityQual", "00").ToString
                oISA.ISA04 = nz(oRow, "CarrierEDISecurityCode", "").ToString
                oISA.ISA07 = nz(oRow, "CarrierEDIPartnerQual", "02").ToString
                oISA.ISA08 = nz(oRow, "CarrierEDIPartnerCode", "").ToString
                oISA.ISA09 = Date.Now.ToString("yyMMdd")
                oISA.ISA10 = Date.Now.ToString("HHmm")
                oISA.ISA11 = "U"
                oISA.ISA12 = "00401"
                oISA.ISA13 = intISASeq.ToString
                If .CarrierEDIAcknowledgementRequested Then
                    oISA.ISA14 = "1"
                Else
                    oISA.ISA14 = "0"
                End If
                oISA.ISA15 = nz(.CarrierEDITestCode, "P").ToString
                oGS.CarrierEDIControl = .CarrierEDIControl
                'we have included all the supported functional groups but only the 204,210 and 997 are used by this functon
                Select Case Action
                    Case "204"
                        oGS.GS01 = "SM"
                    Case "210"
                        oGS.GS01 = "IM"
                    Case "997"
                        oGS.GS01 = "FA"
                    Case "214"
                        oGS.GS01 = "QM"
                    Case "990"
                        oGS.GS01 = "GF"
                End Select

                If blnMapGS03ToISA08 Then
                    oGS.GS03 = oISA.ISA08
                Else
                    oGS.GS03 = nz(oRow, "CarrierEDIPartnerCode", "").ToString()
                End If
                oGS.GS04 = Date.Now.ToString("yyyyMMdd")
                oGS.GS05 = oISA.ISA10
                oGS.GS06 = intGSSeq
                oGS.GS07 = "X"
                oGS.GS08 = "004010"
            End With
            blnRet = True
        Catch ex As Exception
            LogException("Read EDI Carrier Settings Failure!", "Cannot read EDI settings from Carrier EDI table for partner code " & PartnerCode & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsCarrierEDIBLL.fillEDIObjects Failure")
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Overload to the above version created to return a boolean value indicating if the configuration existed in the database
    ''' </summary>
    ''' <param name="oISA"></param>
    ''' <param name="oGS"></param>
    ''' <param name="PartnerCode"></param>
    ''' <param name="Action"></param>
    ''' <param name="blnDoesConfigExist"></param>
    ''' <param name="blnMapGS03ToISA08"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created By LVV on 10/10/19
    ''' We didn't want to keep sending error emails for if the 997 config does not exist (support case 201910021431)
    ''' Modified by RHR for v-8.4.0.003 on 08/02/2021 added optional blnMapGS03ToISA08 parameter
    '''   from EDI config setting4444444444444
    ''' </remarks>
    Public Function fillEDIObjects(ByRef oISA As clsEDIISA, ByRef oGS As clsEDIGS, ByVal PartnerCode As String, ByVal Action As String, ByRef blnDoesConfigExist As Boolean, ByVal blnMapGS03ToISA08 As Boolean) As Boolean
        Dim blnRet As Boolean = False
        blnDoesConfigExist = True
        Try
            Dim oTable As FMData.CarrierEDIDataTable = GetDataByPartnerCodeAction(PartnerCode, Action)
            If oTable?.Count < 1 Then
                blnDoesConfigExist = False
            Else
                'we only read the first data
                Dim oRow As FMData.CarrierEDIRow = oTable(0)
                Dim intISASeq As Integer = getNextISASequence(oRow)
                Dim intGSSeq As Integer = getNextGSSequence(oRow)
                With oRow
                    'we use the carrier data as the receiver 
                    oISA.CarrierEDIControl = .CarrierEDIControl
                    oISA.ISA03 = nz(oRow, "CarrierEDISecurityQual", "00").ToString
                    oISA.ISA04 = nz(oRow, "CarrierEDISecurityCode", "").ToString
                    oISA.ISA07 = nz(oRow, "CarrierEDIPartnerQual", "02").ToString
                    oISA.ISA08 = nz(oRow, "CarrierEDIPartnerCode", "").ToString
                    oISA.ISA09 = Date.Now.ToString("yyMMdd")
                    oISA.ISA10 = Date.Now.ToString("HHmm")
                    oISA.ISA11 = "U"
                    oISA.ISA12 = "00401"
                    oISA.ISA13 = intISASeq.ToString
                    If .CarrierEDIAcknowledgementRequested Then
                        oISA.ISA14 = "1"
                    Else
                        oISA.ISA14 = "0"
                    End If
                    oISA.ISA15 = nz(.CarrierEDITestCode, "P").ToString
                    oGS.CarrierEDIControl = .CarrierEDIControl
                    'we have included all the supported functional groups but only the 204,210 and 997 are used by this functon
                    Select Case Action
                        Case "204"
                            oGS.GS01 = "SM"
                        Case "210"
                            oGS.GS01 = "IM"
                        Case "997"
                            oGS.GS01 = "FA"
                        Case "214"
                            oGS.GS01 = "QM"
                        Case "990"
                            oGS.GS01 = "GF"
                    End Select
                    If blnMapGS03ToISA08 Then
                        oGS.GS03 = oISA.ISA08
                    Else
                        oGS.GS03 = nz(oRow, "CarrierEDIPartnerCode", "").ToString()
                    End If
                    oGS.GS04 = Date.Now.ToString("yyyyMMdd")
                    oGS.GS05 = oISA.ISA10
                    oGS.GS06 = intGSSeq
                    oGS.GS07 = "X"
                    oGS.GS08 = "004010"
                End With
                blnRet = True
            End If
        Catch ex As Exception
            LogException("Read EDI Carrier Settings Failure!", "Cannot read EDI settings from Carrier EDI table for partner code " & PartnerCode & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsCarrierEDIBLL.fillEDIObjects Failure")
        End Try
        Return blnRet
    End Function


    ''' <summary>
    ''' Fill the EDI ISA and GS elements with data based on the o997 settings
    ''' </summary>
    ''' <param name="oISA"></param>
    ''' <param name="oGS"></param>
    ''' <param name="o997"></param>
    ''' <param name="Action"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 6/8/2017
    '''   added to support EDI 204 inbound documents
    ''' </remarks>
    Public Function fillEDIObjects(ByRef oISA As clsEDIISA, ByRef oGS As clsEDIGS, ByRef o997 As clsEDI997, ByVal Action As String) As Boolean
        Dim blnRet As Boolean = False

        If String.IsNullOrWhiteSpace(o997.InternalPartnerCode) Then
            LogError("Read EDI Carrier Settings Failure!", "Cannot read EDI settings from Carrier EDI table because the 997 specific Internal Partner Code is Missing.", AdminEmail)
            Return False
        End If
        Dim PartnerCode As String = o997.InternalPartnerCode
        Try
            Dim oTable As FMData.CarrierEDIDataTable = GetDataByPartnerCodeAction(PartnerCode, Action)
            'we only read the first data
            Dim oRow As FMData.CarrierEDIRow = oTable(0)
            Dim intISASeq As Integer = getNextISASequence(oRow)
            Dim intGSSeq As Integer = getNextGSSequence(oRow)
            With oRow
                Dim strAcknowledgeMent As String = "0"
                If .CarrierEDIAcknowledgementRequested Then strAcknowledgeMent = "1"
                oISA.fillSegments(o997.InternalSecurityQualifier,
                                  o997.InternalSecurityCode,
                                  o997.ExternalSecurityQualifier,
                                  o997.ExternalSecurityCode,
                                  o997.InternalPartnerQualifier,
                                  o997.InternalPartnerCode,
                                  o997.ExternalPartnerQualifier,
                                  o997.ExternalPartnerCode,
                                  Date.Now.ToString("yyMMdd"),
                                  Date.Now.ToString("HHmm"),
                                  intISASeq.ToString,
                                  strAcknowledgeMent,
                                  nz(.CarrierEDITestCode, "P").ToString)


                oGS.CarrierEDIControl = .CarrierEDIControl
                'we have included all the supported functional groups but only the 204,210 and 997 are used by this functon
                Select Case Action
                    Case "204"
                        oGS.GS01 = "SM"
                    Case "210"
                        oGS.GS01 = "IM"
                    Case "997"
                        oGS.GS01 = "FA"
                    Case "214"
                        oGS.GS01 = "QM"
                    Case "990"
                        oGS.GS01 = "GF"
                End Select
                oGS.GS02 = o997.InternalPartnerCode
                oGS.GS03 = o997.ExternalPartnerCode
                oGS.GS04 = Date.Now.ToString("yyyyMMdd")
                oGS.GS05 = oISA.ISA10
                oGS.GS06 = intGSSeq
                oGS.GS07 = "X"
                oGS.GS08 = "004010"
            End With
            blnRet = True
        Catch ex As Exception
            LogException("Read EDI Carrier Settings Failure!", "Cannot read EDI settings from Carrier EDI table for partner code " & PartnerCode & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsCarrierEDIBLL.fillEDIObjects Failure")
        End Try
        Return blnRet
    End Function

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Public Function fillEDIObjects210(ByRef oISA As clsEDIISA, ByRef oGS As clsEDIGS, ByVal CarrierControl As Integer, ByVal Action As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            Dim oTable As FMData.CarrierEDIDataTable = GetDataByCarrierControlAction(CarrierControl, Action)
            'we only read the first data
            Dim oRow As FMData.CarrierEDIRow = oTable(0)
            Dim intISASeq As Integer = getNextISASequence(oRow)
            Dim intGSSeq As Integer = getNextGSSequence(oRow)
            With oRow
                'we use the carrier data as the receiver 
                oISA.CarrierEDIControl = .CarrierEDIControl
                oISA.ISA01 = nz(oRow, "CarrierEDISecurityQual", "00").ToString
                oISA.ISA02 = nz(oRow, "CarrierEDISecurityCode", "").ToString
                oISA.ISA05 = (nz(oRow, "CarrierEDIPartnerQual", "2").ToString).Trim
                oISA.ISA06 = (nz(oRow, "CarrierEDIPartnerCode", "").ToString).Trim
                oISA.ISA09 = Date.Now.ToString("yyMMdd")
                oISA.ISA10 = Date.Now.ToString("HHmm")
                oISA.ISA11 = "U"
                oISA.ISA12 = "00401"
                oISA.ISA13 = intISASeq.ToString
                If .CarrierEDIAcknowledgementRequested Then
                    oISA.ISA14 = "1"
                Else
                    oISA.ISA14 = "0"
                End If
                oISA.ISA15 = nz(.CarrierEDITestCode, "P").ToString
                oGS.CarrierEDIControl = .CarrierEDIControl
                'we have included all the supported functional groups but only the 204,210 and 997 are used by this functon
                Select Case Action
                    Case "204"
                        oGS.GS01 = "SM"
                    Case "210"
                        oGS.GS01 = "IM"
                    Case "997"
                        oGS.GS01 = "FA"
                    Case "214"
                        oGS.GS01 = "QM"
                    Case "990"
                        oGS.GS01 = "GF"
                    Case "888"
                        oGS.GS01 = "IM"
                End Select
                oGS.GS02 = (oISA.ISA06).Trim
                oGS.GS04 = Date.Now.ToString("yyyyMMdd")
                oGS.GS05 = oISA.ISA10
                oGS.GS06 = intGSSeq
                oGS.GS07 = "X"
                oGS.GS08 = "004010"
            End With
            blnRet = True
        Catch ex As Exception
            LogException("Read EDI Carrier Settings Failure!", "Cannot read EDI settings from Carrier EDI table for carrier control Number " & CarrierControl & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsCarrierEDIBLL.fillEDIObjects Failure")
        End Try
        Return blnRet
    End Function

#End Region

End Class
