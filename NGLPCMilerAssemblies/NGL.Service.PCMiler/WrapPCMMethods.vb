Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Text
'Imports PCMTONET32 = PCMDLLTONET.PCM
'Imports PCMINT32 = PCMDLLINT.PCM
Imports PCMTONET64 = PCMDLLTONET64.PCM
Imports PCMINT64 = PCMDLLINT64.PCM

Module WrapPCMMethods

    Public Structure legInfoType
        Public legMiles As Single
        Public totMiles As Single
        Public legCost As Single
        Public totCost As Single
        Public legHours As Single
        Public totHours As Single
    End Structure


    Private _is64bit As System.Nullable(Of Boolean)
    Public ReadOnly Property is64bit As Boolean
        Get
            If Not _is64bit.HasValue Then
                If System.Environment.Is64BitProcess Then
                    _is64bit = True
                Else
                    _is64bit = False
                End If
            End If
            Return _is64bit.Value
        End Get
    End Property

    Public Function PCMSOpenServer(ByVal appInst As Integer, ByVal hWnd As Integer) As Short
        If is64bit Then
            Return PCMINT64.PCMSOpenServer(appInst, hWnd)
        Else
            '  Return PCMINT32.PCMSOpenServer(appInst, hWnd)
        End If
    End Function

    Public Function PCMSCloseServer(ByVal serverID As Short) As Integer
        If is64bit Then
            Return PCMINT64.PCMSCloseServer(serverID)
        Else
            ' Return PCMINT32.PCMSCloseServer(serverID)
        End If
    End Function

    Public Function PCMSCalcDistance(ByVal serverID As Short, ByVal orig As String, ByVal dest As String) As Integer
        If is64bit Then
            Return PCMINT64.PCMSCalcDistance(serverID, orig, dest)
        Else
            ' Return PCMINT32.PCMSCalcDistance(serverID, orig, dest)
        End If
    End Function

    Public Function PCMSNewTrip(ByVal serverID As Short) As Integer
        If is64bit Then
            Return PCMINT64.PCMSNewTrip(serverID)
        Else
            ' Return PCMINT32.PCMSNewTrip(serverID)
        End If
    End Function

    Public Sub PCMSDeleteTrip(ByVal tripID As Integer)
        If is64bit Then
            PCMINT64.PCMSDeleteTrip(tripID)
        Else
            'PCMINT32.PCMSDeleteTrip(tripID)
        End If
    End Sub

    Public Function PCMSCalculate(ByVal tripID As Integer) As Integer
        If is64bit Then
            Return PCMINT64.PCMSCalculate(tripID)
        Else
            'Return PCMINT32.PCMSCalculate(tripID)
        End If
    End Function

    Public Function PCMSAddStop(ByVal tripID As Integer, ByVal stopName As String) As Integer
        If is64bit Then
            Return PCMINT64.PCMSAddStop(tripID, stopName)
        Else
            'Return PCMINT32.PCMSAddStop(tripID, stopName)
        End If
    End Function

    Public Function PCMSLookup(ByVal tripID As Integer, ByVal cityzip As String, ByVal easy As Integer) As Integer
        If is64bit Then
            Return PCMINT64.PCMSLookup(tripID, cityzip, easy)
        Else
            'Return PCMINT32.PCMSLookup(tripID, cityzip, easy)
        End If
    End Function

    Public Function PCMSGetMatch(ByVal tripID As Integer, ByVal index As Integer, ByVal buffer As StringBuilder, ByVal buflen As Integer) As Integer
        If is64bit Then
            Return PCMINT64.PCMSGetMatch(tripID, index, buffer, buflen)
        Else
            'Return PCMINT32.PCMSGetMatch(tripID, index, buffer, buflen)
        End If
    End Function

    Public Function PCMSGetFmtMatch2(ByVal tripID As Integer, ByVal index As Integer, ByVal addrBuffer As StringBuilder, ByVal addrLen As Integer, ByVal cityBuffer As StringBuilder, ByVal cityLen As Integer, ByVal stateBuffer As StringBuilder, ByVal statelen As Integer, ByVal zipBuffer As StringBuilder, ByVal zipLen As Integer, ByVal countyBuffer As StringBuilder, ByVal countyLen As Integer) As Integer
        If is64bit Then
            Return PCMINT64.PCMSGetFmtMatch2(tripID, index, addrBuffer, addrLen, cityBuffer, cityLen, stateBuffer, statelen, zipBuffer, zipLen, countyBuffer, countyLen)
        Else
            'Return PCMINT32.PCMSGetFmtMatch2(tripID, index, addrBuffer, addrLen, cityBuffer, cityLen, stateBuffer, statelen, zipBuffer, zipLen, countyBuffer, countyLen)
        End If
    End Function

    Public Function PCMSGetHTMLRpt(ByVal tripID As Integer, ByVal rptNum As Integer, ByVal buffer As StringBuilder, ByVal buflen As Integer) As Integer
        If is64bit Then
            Return PCMINT64.PCMSGetHTMLRpt(tripID, rptNum, buffer, buflen)
        Else
            'Return PCMINT32.PCMSGetHTMLRpt(tripID, rptNum, buffer, buflen)
        End If
    End Function

    Public Function PCMSNumHTMLRptBytes(ByVal tripID As Integer, ByVal rptNum As Integer) As Integer
        If is64bit Then
            Return PCMINT64.PCMSNumHTMLRptBytes(tripID, rptNum)
        Else
            'Return PCMINT32.PCMSNumHTMLRptBytes(tripID, rptNum)
        End If
    End Function

    Public Function PCMSNumLegs(ByVal tripID As Integer) As Integer
        If is64bit Then
            Return PCMINT64.PCMSNumLegs(tripID)
        Else
            'Return PCMINT32.PCMSNumLegs(tripID)
        End If
    End Function

    Public Function PCMSGetLegInfo(ByVal tripID As Integer, ByVal indx As Integer, ByRef pLegInfo As legInfoType) As Integer
        If is64bit Then
            Dim oLegInfo As PCMINT64.legInfoType
            copyLegInfoData(pLegInfo, oLegInfo)
            Dim intRet = PCMINT64.PCMSGetLegInfo(tripID, indx, oLegInfo)
            copyLegInfoData(oLegInfo, pLegInfo)
            Return intRet
        Else
            'Dim oLegInfo As PCMINT32.legInfoType
            'copyLegInfoData(pLegInfo, oLegInfo)
            'Dim intRet = PCMINT32.PCMSGetLegInfo(tripID, indx, oLegInfo)
            'copyLegInfoData(oLegInfo, pLegInfo)
            'Return intRet

        End If
    End Function

    Public Sub PCMSSetCalcTypeEx(ByVal tripID As Integer, ByVal rtType As Integer, ByVal optFlags As Integer, ByVal vehType As Integer)
        If is64bit Then
            PCMINT64.PCMSSetCalcTypeEx(tripID, rtType, optFlags, vehType)
        Else
            'PCMINT32.PCMSSetCalcTypeEx(tripID, rtType, optFlags, vehType)
        End If
    End Sub

    'DLL to Net     

    Public Function PCMSGetError() As Integer
        If is64bit Then
            Return PCMTONET64.PCMSGetError()
        Else
            'Return PCMTONET32.PCMSGetError()
        End If
    End Function

    Public Function PCMSGetErrorString(ByVal errorCode As Integer, ByVal buffer As StringBuilder, ByVal bufLen As Integer) As Integer
        If is64bit Then
            Return PCMTONET64.PCMSGetErrorString(errorCode, buffer, bufLen)
        Else
            'Return PCMTONET32.PCMSGetErrorString(errorCode, buffer, bufLen)
        End If
    End Function

    Public Function PCMSSetMiles(ByVal tripID As Integer) As Short
        If is64bit Then
            Return PCMTONET64.PCMSSetMiles(tripID)
        Else
            'Return PCMTONET32.PCMSSetMiles(tripID)
        End If
    End Function

    Public Function PCMSCityToLatLong(ByVal serverID As Short, ByVal cityZip As String, ByVal buffer As StringBuilder, ByVal bufLen As Short) As Short
        If is64bit Then
            Return PCMTONET64.PCMSCityToLatLong(serverID, cityZip, buffer, bufLen)
        Else
            'Return PCMTONET32.PCMSCityToLatLong(serverID, cityZip, buffer, bufLen)
        End If
    End Function

    Public Function PCMSLatLongToCity(ByVal serverID As Short, ByVal cityZip As String, ByVal buffer As StringBuilder, ByVal bufLen As Short) As Short
        If is64bit Then
            Return PCMTONET64.PCMSLatLongToCity(serverID, cityZip, buffer, bufLen)
        Else
            'Return PCMTONET32.PCMSLatLongToCity(serverID, cityZip, buffer, bufLen)
        End If
    End Function

    Public Function PCMSSetKilometers(ByVal tripID As Integer) As Short
        If is64bit Then
            Return PCMTONET64.PCMSSetKilometers(tripID)
        Else
            'Return PCMTONET32.PCMSSetKilometers(tripID)
        End If
    End Function

    Public Function PCMSSetResequence(ByVal tripID As Integer, ByVal changeDest As Boolean) As Integer
        If is64bit Then
            Return PCMTONET64.PCMSSetResequence(tripID, changeDest)
        Else
            'Return PCMTONET32.PCMSSetResequence(tripID, changeDest)
        End If
    End Function

    Public Function PCMSGetRpt(ByVal tripID As Integer, ByVal index As Integer, ByVal buffer As StringBuilder, ByVal buflen As Integer) As Integer
        If is64bit Then
            Return PCMTONET64.PCMSGetRpt(tripID, index, buffer, buflen)
        Else
            'Return PCMTONET32.PCMSGetRpt(tripID, index, buffer, buflen)
        End If
    End Function

    Public Function PCMSGetRptLine(ByVal tripID As Integer, ByVal rptNum As Integer, ByVal lineNum As Integer, ByVal buffer As StringBuilder, ByVal buflen As Integer) As Integer
        If is64bit Then
            Return PCMTONET64.PCMSGetRptLine(tripID, rptNum, lineNum, buffer, buflen)
        Else
            'Return PCMTONET32.PCMSGetRptLine(tripID, rptNum, lineNum, buffer, buflen)
        End If
    End Function

    Public Function PCMSNumRptLines(ByVal tripID As Integer, ByVal rptNum As Integer) As Integer
        If is64bit Then
            Return PCMTONET64.PCMSNumRptLines(tripID, rptNum)
        Else
            'Return PCMTONET32.PCMSNumRptLines(tripID, rptNum)
        End If
    End Function

    Public Function PCMSOptimize(ByVal tripID As Integer) As Integer
        If is64bit Then
            Return PCMTONET64.PCMSOptimize(tripID)
        Else
            'Return PCMTONET32.PCMSOptimize(tripID)
        End If
    End Function

    Public Function PCMSCheckPlaceName(ByVal tripID As Integer, ByVal cityZip As String) As Integer
        If is64bit Then
            Return PCMTONET64.PCMSCheckPlaceName(tripID, cityZip)
        Else
            'Return PCMTONET32.PCMSCheckPlaceName(tripID, cityZip)
        End If
    End Function

    Public Function PCMSClearStops(ByVal tripID As Integer) As Integer
        If is64bit Then
            Return PCMTONET64.PCMSClearStops(tripID)
        Else
            'Return PCMTONET32.PCMSClearStops(tripID)
        End If
    End Function

    Public Sub PCMSSetCalcType(ByVal tripID As Integer, ByVal rtType As Integer)
        If is64bit Then
            PCMTONET64.PCMSSetCalcType(tripID, rtType)
        Else
            'PCMTONET32.PCMSSetCalcType(tripID, rtType)
        End If
    End Sub


    Public Sub copyLegInfoData(ByRef fData As Object, ByRef tData As Object)
        Try

            If fData Is Nothing OrElse tData Is Nothing Then Return
            With tData
                .legMiles = fData.legMiles
                .totMiles = fData.totMiles
                .legCost = fData.legCost
                .totCost = fData.totCost
                .legHours = fData.legHours
                .totHours = fData.totHours
            End With
        Catch ex As Exception
            'for now do nothing
        End Try
    End Sub

End Module
