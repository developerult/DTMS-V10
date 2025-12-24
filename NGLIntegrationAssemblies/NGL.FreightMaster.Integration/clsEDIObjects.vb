Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Imports Ngl.FreightMaster.Integration.FMDataTableAdapters
Imports Ngl.FreightMaster.Integration.Configuration
Imports System.IO
Imports NDT = Ngl.Core.Utility.DataTransformation
Imports BLL = NGL.FM.BLL
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data

'NOTE:  this entire file has been modified by RHR for v-7.0.6.105 on 5/16/2017 to add check for null reference before .trim using new conditional operator ?

#Region " New Base Class for Files and Loops"
''' <summary>
''' Tracks the number of elements inside of an EDI Loop object or File object
''' Items Allowed determines the maximum number of elements allowed and
''' ItemsUsed returns the numnber of elements actually found.
''' </summary>
''' <remarks>
''' Created by RHR on 5/5/2017 for v-7.0.6.105
''' </remarks>
Public Class EDIItemTracker

    Sub New()
        MyBase.New()
    End Sub
    Sub New(ByVal intAllowed As Integer)
        MyBase.New()
        ItemsAllowed = intAllowed
    End Sub

    Private _ItemsAllowed As Integer
    Public Property ItemsAllowed() As Integer
        Get
            Return _ItemsAllowed
        End Get
        Set(ByVal value As Integer)
            _ItemsAllowed = value
        End Set
    End Property

    Private _ItemsUsed As Integer
    Public Property ItemsUsed() As Integer
        Get
            Return _ItemsUsed
        End Get
        Set(ByVal value As Integer)
            _ItemsUsed = value
        End Set
    End Property

End Class

''' <summary>
''' New base class for EDI objects required to process EDI files 
''' using the new insertElements algorithm
''' </summary>
''' <remarks>
''' Created by RHR on 5/5/2017 for v-7.0.6.105
''' </remarks>
Public MustInherit Class EDIObjBase : Inherits clsDownload

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
        MyBase.New(config)
    End Sub

    Private _Name As String = "EDIObjBase"
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property


    ''' <summary>
    ''' this array can be used to determine if an element like OID belongs to the loop
    ''' this array should be populated in the constructor
    ''' </summary>
    Private _Elements As Dictionary(Of String, EDIItemTracker)
    Public Property Elements() As Dictionary(Of String, EDIItemTracker)
        Get
            Return _Elements
        End Get
        Set(ByVal value As Dictionary(Of String, EDIItemTracker))
            _Elements = value
        End Set
    End Property

    Private _Loops As Dictionary(Of String, EDIItemTracker)
    Public Property Loops() As Dictionary(Of String, EDIItemTracker)
        Get
            Return _Loops
        End Get
        Set(ByVal value As Dictionary(Of String, EDIItemTracker))
            _Loops = value
        End Set
    End Property


    Public MustOverride Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)

    Public MustOverride Function addSegment(ByVal strSource As String, Optional ByVal strKey As String = "") As Boolean

    Public MustOverride Function addLoopFromKey(ByVal strElemKey As String, ByRef strElements As String(), ByRef NextIndex As Integer) As Boolean

    Protected MustOverride Sub populateElements()

    Public Overridable Function usesElement(ByVal sElement As String) As Boolean
        Dim blnRet As Boolean = False
        If Not Me.Elements Is Nothing AndAlso Me.Elements.Count() > 0 Then

            If Elements.ContainsKey(sElement) Then blnRet = True
        End If
        Return blnRet
    End Function

    Public Overridable Function getKey(ByVal strSource As String, ByRef strKey As String) As Boolean
        If Not strSource.Contains("*") Then Return False
        strKey = strSource.Split("*")(0)
        If String.IsNullOrWhiteSpace(strKey) Then
            Return False
        Else
            Return True
        End If
    End Function


    ''' <summary>
    ''' inserts Elements into loops or file objects based on the next index using the entire list of elements
    ''' this method is called recursively by nested child loops, each succssful call should increase the 
    ''' NextIndex by one
    ''' </summary>
    ''' <param name="oEDI"></param>
    ''' <param name="strElements"></param>
    ''' <param name="NextIndex"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR on 5/5/2017 for v-7.0.6.105
    ''' </remarks>
    Public Overridable Function insertElements(ByRef oEDI As EDIObjBase, ByRef strElements As String(), ByRef NextIndex As Integer) As EDIObjBase
        If oEDI Is Nothing Then Return oEDI 'nothing to do
        'For i As Integer = NextIndex To strElements.Count - 1
        Do While NextIndex < strElements.Count - 1
            'get the key from the next segment
            Dim strKey As String = ""
            If Not getKey(strElements(NextIndex), strKey) Then
                'bad element just increase the index counter
                NextIndex += 1
                Continue Do
            End If

            If oEDI.usesElement(strKey) Then
                'if this key is one of this objects elements add it
                If oEDI.addSegment(strElements(NextIndex), strKey) Then
                    NextIndex += 1
                Else
                    'go back one index
                    NextIndex -= 1
                    'Key does not match so return this data object and continue process the 
                    'previous call
                    Return oEDI
                    Exit Do
                End If
            Else
                'check if the key exists in one of the child loops
                'Note the child must manage loop priority in the 
                'addLoopFromKey method
                If oEDI.addLoopFromKey(strKey, strElements, NextIndex) Then
                    NextIndex += 1
                Else
                    'go back one index
                    NextIndex -= 1
                    'Key does not match so return this data object and continue process the 
                    'previous call
                    Return oEDI
                    Exit Do
                End If
            End If
            'on success increase the index

        Loop
        'Next

        Return oEDI
    End Function



End Class

#End Region

#Region " Sample 204 EDI Segment File"
'ST*204*34112~
'B2**RUAN**442901**PP***E~
'B2A*00*LT~
'L11*442901*BM*FS~
'PLD*22~
'S5*1*CL*43890*L*2310*CA~
'L11*442901*PO*FS~
'G62*10*20090911*Y*000001*LT~
'PLD*22~
'N1*SF*LYNDONVILLE PLANT*ZZ*310~
'N3*247 WEST AVE~
'N4*LYNDONVILLE*NY*14098*USA~
'S5*2*CU*43890*L*2310*CA~
'L11*6492642115*PO*FS~
'G62*02*20090912*Z*000002*LT~
'PLD*22~
'N1*ST*SAM'S-AKRON #6492*ZZ*33371/16~
'N3*2150 INTERNATIONAL PARKWAY~
'N4*NORTH CANTON*OH*44270*USA~
'L3*43890*G*0*CW*00~
'SE*21*34112~
#End Region

#Region " ISA Interchange Control Header"

''' <summary>
'''  Standard ISA Interchange Header Element
''' </summary>
''' <remarks>
''' Modifed by RHR for v-7.0.6.105 on 6/8/2017
'''   added new fill subroutine
''' </remarks>
Public Class clsEDIISA

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is ISA
            Select Case i
                Case 1
                    ISA01 = sSegs(i)
                Case 2
                    ISA02 = sSegs(i)
                Case 3
                    ISA03 = sSegs(i)
                Case 4
                    ISA04 = sSegs(i)
                Case 5
                    ISA05 = sSegs(i)
                Case 6
                    ISA06 = sSegs(i)
                Case 7
                    ISA07 = sSegs(i)
                Case 8
                    ISA08 = sSegs(i)
                Case 9
                    ISA09 = sSegs(i)
                Case 10
                    ISA10 = sSegs(i)
                Case 11
                    ISA11 = sSegs(i)
                Case 12
                    ISA12 = sSegs(i)
                Case 13
                    ISA13 = sSegs(i)
                Case 14
                    ISA14 = sSegs(i)
                Case 15
                    ISA15 = sSegs(i)
                Case 16
                    ISA16 = Left(sSegs(i), 1)
                    If sSegs(i).Length > 1 Then SegmentTerminator = sSegs(i).Substring(1, 1)
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Fills the segments with the data provided by Name
    ''' </summary>
    ''' <param name="sendSecurityQual"></param>
    ''' <param name="sendSecurityCode"></param>
    ''' <param name="recSecurityQual"></param>
    ''' <param name="recSecurityCode"></param>
    ''' <param name="sendPartnerQual"></param>
    ''' <param name="sendPartnerCode"></param>
    ''' <param name="recPartnerQual"></param>
    ''' <param name="recPartnerCode"></param>
    ''' <param name="sendDate"></param>
    ''' <param name="sendTime"></param>
    ''' <param name="sequenceNbr"></param>
    ''' <param name="acknowledmentRequested"></param>
    ''' <param name="tstOrProd"></param>
    ''' <param name="controlStandardID"></param>
    ''' <param name="controlVersion"></param>
    ''' <param name="subElementSeperator"></param>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 6/8/2017
    ''' </remarks>
    Public Sub fillSegments(ByVal sendSecurityQual As String,
                                 ByVal sendSecurityCode As String,
                                 ByVal recSecurityQual As String,
                                 ByVal recSecurityCode As String,
                                 ByVal sendPartnerQual As String,
                                 ByVal sendPartnerCode As String,
                                 ByVal recPartnerQual As String,
                                 ByVal recPartnerCode As String,
                                 ByVal sendDate As String,
                                 ByVal sendTime As String,
                                 ByVal sequenceNbr As String,
                                 ByVal acknowledmentRequested As String,
                                 ByVal tstOrProd As String,
                                 Optional ByVal controlStandardID As String = "U",
                                 Optional ByVal controlVersion As String = "00401",
                                 Optional ByVal subElementSeperator As String = ":")

        ISA01 = sendSecurityQual
        ISA02 = sendSecurityCode
        ISA03 = recSecurityQual
        ISA04 = recSecurityCode
        ISA05 = sendPartnerQual
        ISA06 = sendPartnerCode
        ISA07 = recPartnerQual
        ISA08 = recPartnerCode
        ISA09 = sendDate
        ISA10 = sendTime
        ISA11 = controlStandardID
        ISA12 = controlVersion
        ISA13 = sequenceNbr
        ISA14 = acknowledmentRequested
        ISA15 = tstOrProd
        ISA16 = subElementSeperator

    End Sub

    ''' <summary>
    ''' Generates a properly formatted EDI ISA Segment using the current objects ISA data previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    ''' add check for null reference before .Trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 16 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "ISA")
        End If

        Return sEdi.ToString
    End Function

    ''' <summary>
    ''' Generates a properly formatted EDI ISA Segment using sender and reciever ids while providing some default values
    ''' Typically used for testing
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <param name="sSenderPartnerID"></param>
    ''' <param name="sReceiverPartnerID"></param>
    ''' <param name="sSenderPartnerQual"></param>
    ''' <param name="sReceiverPartnerQual"></param>
    ''' <param name="dtTransmission"></param>
    ''' <param name="sInterchangeControlNo"></param>
    ''' <returns>String</returns>
    ''' <remarks>
    ''' Added by LVV on 4/11/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String, ByVal sSenderPartnerID As String, ByVal sReceiverPartnerID As String, ByVal sSenderPartnerQual As String, ByVal sReceiverPartnerQual As String, Optional ByVal dtTransmission As Date? = Nothing, Optional ByVal sInterchangeControlNo As String = "1") As String

        If Not dtTransmission.HasValue() Then dtTransmission = Date.Now()
        Dim oISA As New clsEDIISA()
        With oISA
            oISA.ISA05 = sSenderPartnerQual
            oISA.ISA06 = sSenderPartnerID
            oISA.ISA07 = sReceiverPartnerQual
            oISA.ISA08 = sReceiverPartnerID
            oISA.ISA09 = Configuration.nzDate(dtTransmission.Value, "yyMMdd", "00000000")
            oISA.ISA10 = Configuration.nzDate(dtTransmission.Value, "HHmm", "0000")
            oISA.ISA11 = "U"
            oISA.ISA12 = "00401"
            oISA.ISA13 = sInterchangeControlNo
            oISA.ISA14 = "0"
            oISA.ISA15 = "T"
        End With
        Return oISA.getEDIString(sSegTerm)
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = ISA01
            Case 2
                strRet = ISA02
            Case 3
                strRet = ISA03
            Case 4
                strRet = ISA04
            Case 5
                strRet = ISA05
            Case 6
                strRet = ISA06
            Case 7
                strRet = ISA07
            Case 8
                strRet = ISA08
            Case 9
                strRet = ISA09
            Case 10
                strRet = ISA10
            Case 11
                strRet = ISA11
            Case 12
                strRet = ISA12
            Case 13
                strRet = ISA13
            Case 14
                strRet = ISA14
            Case 15
                strRet = ISA15
            Case 16
                strRet = ISA16
        End Select

        Return strRet
    End Function



    Public CarrierEDIISAControl As Integer = 0
    Public CarrierEDIControl As Integer = 0
    Public CompEDIControl As Integer = 0
    Private _ISA01 As String = ""
    ''' <summary>
    ''' Authorization Information Qualifier (2) typically 00
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ISA01() As String
        Get

            Return buildFixWidth(_ISA01, 2, "0")

        End Get
        Set(ByVal value As String)
            _ISA01 = value
        End Set
    End Property
    Private _ISA02 As String = ""
    ''' <summary>
    ''' Authorization Information (10) typically blank
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ISA02() As String
        Get

            Return buildFixWidth(_ISA02, 10, " ")

        End Get
        Set(ByVal value As String)
            _ISA02 = value
        End Set
    End Property
    Private _ISA03 As String = ""
    ''' <summary>
    ''' Security Information Qualifier (2) typically 00
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ISA03() As String
        Get

            Return buildFixWidth(_ISA03, 2, "0")

        End Get
        Set(ByVal value As String)
            _ISA03 = value
        End Set
    End Property
    Private _ISA04 As String = ""
    ''' <summary>
    ''' Security Information (10) typically blank
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ISA04() As String
        Get

            Return buildFixWidth(_ISA04, 10, " ", False)

        End Get
        Set(ByVal value As String)
            _ISA04 = value
        End Set
    End Property
    Private _ISA05 As String = "" 'Interchange ID Qualifier (2) Sender Code RUAN is 02 Mizkan is 12
    Public Property ISA05() As String
        Get

            Return buildFixWidth(_ISA05, 2, "0")

        End Get
        Set(ByVal value As String)
            _ISA05 = value
        End Set
    End Property
    Private _ISA06 As String = "" 'Interchange Sender ID (15)  Sender Code like RUAN Right Padding
    Public Property ISA06() As String
        Get

            Return buildFixWidth(_ISA06, 15, " ", False)

        End Get
        Set(ByVal value As String)
            _ISA06 = value
        End Set
    End Property
    Private _ISA07 As String = "" 'Interchange ID Qualifier (2) Receiver Code RUAN is 02 Mizkan is 12
    Public Property ISA07() As String
        Get

            Return buildFixWidth(_ISA07, 2, "0")

        End Get
        Set(ByVal value As String)
            _ISA07 = value
        End Set
    End Property
    Private _ISA08 As String = "" 'Interchange Receiver ID (15)  Receiver Code like RUAN Right Padding
    Public Property ISA08() As String
        Get

            Return buildFixWidth(_ISA08, 15, " ", False)

        End Get
        Set(ByVal value As String)
            _ISA08 = value
        End Set
    End Property
    Private _ISA09 As String = "" 'Interchange Date (6) Date of transmission yyMMdd
    Public Property ISA09() As String
        Get

            Return buildFixWidth(_ISA09, 6, "0")

        End Get
        Set(ByVal value As String)
            _ISA09 = value
        End Set
    End Property
    Private _ISA10 As String = "" 'Interchange Time (4) Time of transmission HHmm eg 1325 for 01:25 pm
    Public Property ISA10() As String
        Get

            Return buildFixWidth(_ISA10, 4, "0")

        End Get
        Set(ByVal value As String)
            _ISA10 = value
        End Set
    End Property
    Private _ISA11 As String = "" 'Interchange Control Standards ID (1) Like U
    Public Property ISA11() As String
        Get

            Return buildFixWidth(_ISA11, 1, "U")

        End Get
        Set(ByVal value As String)
            _ISA11 = value
        End Set
    End Property
    Private _ISA12 As String = "" 'Interchange Control Version Number (5) like 00401 (stands for 4010 format I think)
    Public Property ISA12() As String
        Get

            Return buildFixWidth(_ISA12, 5, "0")

        End Get
        Set(ByVal value As String)
            _ISA12 = value
        End Set
    End Property
    Private _ISA13 As String = "" 'Interchange Control Number (9) control number same as GS06 if only one GS segment per ISA typical 5 digit control number like 24682 restarts at 99999
    Public Property ISA13() As String
        Get

            Return buildFixWidth(_ISA13, 9, "0")

        End Get
        Set(ByVal value As String)
            _ISA13 = value
        End Set
    End Property
    Private _ISA14 As String = "" 'Acknowledgement Requested (1) typically 0
    Public Property ISA14() As String
        Get

            Return buildFixWidth(_ISA14, 1, "0")

        End Get
        Set(ByVal value As String)
            _ISA14 = value
        End Set
    End Property
    Private _ISA15 As String = "" 'Test Indicator (1) P = production
    Public Property ISA15() As String
        Get

            Return buildFixWidth(_ISA15, 1, "P")

        End Get
        Set(ByVal value As String)
            _ISA15 = value
        End Set
    End Property
    Private _ISA16 As String = ":" 'Subelement Separator(1) 'note Segment separator follows immediately after ISA16"
    Public Property ISA16() As String
        Get

            Return buildFixWidth(_ISA16, 1, ":")

        End Get
        Set(ByVal value As String)
            _ISA16 = value
        End Set
    End Property
    Public SegmentTerminator As String = "~" 'Chr(175)
End Class

#End Region

#Region " IEA Interchange Control Trailer"

Public Class clsEDIIEA

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is IEA
            Select Case i
                Case 1
                    IEA01 = sSegs(i)
                Case 2
                    IEA02 = sSegs(i)
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Generates a properly formatted EDI IEA Segment using the current objects IEA data previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "IEA")
        End If

        Return sEdi.ToString
    End Function

    ''' <summary>
    ''' Generates a properly formatted EDI IEA Segment using sender and reciever ids while providing some default values
    ''' Typically used for testing
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <param name="sInterchangeControlNo"></param>
    ''' <returns>String</returns>
    ''' <remarks>
    ''' Added by LVV on 4/11/16 for v-7.0.5.1 EDI Sim Testing Tool 
    ''' Modified by RHR for v-7.0.6.105 on 5/31/2017 for v-6.0.4.70
    '''   cannot overload with only optional parameters changed intTotalTranSets to non-optional  
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String, ByVal sInterchangeControlNo As String) As String

        Dim oIEA As New clsEDIIEA()
        With oIEA
            oIEA.IEA01 = 1
            oIEA.IEA02 = sInterchangeControlNo
        End With
        Return getEDIString(sSegTerm)
    End Function


    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = IEA01
            Case 2
                strRet = IEA02
        End Select

        Return strRet
    End Function


    Private _IEA01 As String = "1" 'Number of Included Functional Groups GE segments typically 1
    Public Property IEA01() As String
        Get

            Return Left(_IEA01, 10)

        End Get
        Set(ByVal value As String)
            _IEA01 = value
        End Set
    End Property
    Private _IEA02 As String = "" 'Control Number (9) (same as ISA13)
    Public Property IEA02() As String
        Get

            Return buildFixWidth(_IEA02, 9, "0")

        End Get
        Set(ByVal value As String)
            _IEA02 = value
        End Set
    End Property

End Class

#End Region

#Region " GS  Function Group Header"

Public Class clsEDIGS
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Populates all GS Objects using a properly formated GS Segment
    ''' Always splits on *
    ''' </summary>
    ''' <param name="strSegment"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is GS
            Select Case i
                Case 1
                    GS01 = sSegs(i)
                Case 2
                    GS02 = sSegs(i)
                Case 3
                    GS03 = sSegs(i)
                Case 4
                    GS04 = sSegs(i)
                Case 5
                    GS05 = sSegs(i)
                Case 6
                    GS06 = sSegs(i)
                Case 7
                    GS07 = sSegs(i)
                Case 8
                    GS08 = sSegs(i)
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Generates a properly formatted EDI GS Segment using the GS objects populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Example: GS*IM*HMBY*8479630007*20160317*2255*1*X*004010
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 8 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "GS")
        End If

        Return sEdi.ToString
    End Function

    ''' <summary>
    ''' Generates a properly formatted EDI GS Segment using sender and reciever ids while providing some default values
    ''' Typically used for testing
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <param name="sGroupHeader"></param>
    ''' <param name="sSenderPartnerID"></param>
    ''' <param name="sReceiverPartnerID"></param>
    ''' <param name="dtTransmission"></param>
    ''' <param name="sGroupControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getEDIString(ByVal sSegTerm As String, ByVal sGroupHeader As String, ByVal sSenderPartnerID As String, ByVal sReceiverPartnerID As String, Optional ByVal dtTransmission As Date? = Nothing, Optional ByVal sGroupControl As String = "1") As String

        If Not dtTransmission.HasValue() Then dtTransmission = Date.Now()
        Dim oGS As New clsEDIGS()
        With oGS
            .GS01 = sGroupHeader
            .GS02 = sSenderPartnerID
            .GS03 = sReceiverPartnerID
            .GS04 = Configuration.nzDate(dtTransmission.Value, "yyyyMMdd", "00000000")
            .GS05 = Configuration.nzDate(dtTransmission.Value, "HHmm", "0000")
            .GS06 = sGroupControl
        End With
        Return oGS.getEDIString(sSegTerm)
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = GS01
            Case 2
                strRet = GS02
            Case 3
                strRet = GS03
            Case 4
                strRet = GS04
            Case 5
                strRet = GS05
            Case 6
                strRet = GS06
            Case 7
                strRet = GS07
            Case 8
                strRet = GS08
        End Select

        Return strRet
    End Function

    Public CarrierEDIGSControl As Integer = 0
    Public CarrierEDIControl As Integer = 0
    Public CompEDIControl As Integer = 0
    Private _GS01 As String = ""
    ''' <summary>
    ''' min/max 2/2 Functional Group Header Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GS01() As String
        Get

            Return Left(buildFixWidth(_GS01, 2, " "), 2)

        End Get
        Set(ByVal value As String)
            _GS01 = value
        End Set
    End Property
    Private _GS02 As String = ""
    ''' <summary>
    ''' min/max 2/15 Application Sender's Partner Code (From)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GS02() As String
        Get

            Return Left(buildFixWidth(_GS02, 2, " ", False), 15)

        End Get
        Set(ByVal value As String)
            _GS02 = value
        End Set
    End Property
    Private _GS03 As String = ""
    ''' <summary>
    ''' min/max 2/15 Application Receiver's Partner Code (To)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GS03() As String
        Get

            Return Left(buildFixWidth(_GS03, 2, " ", False), 15)

        End Get
        Set(ByVal value As String)
            _GS03 = value
        End Set
    End Property

    Private _GS04 As String = ""
    ''' <summary>
    ''' min/max 8/8 Transmission/Create Date yyyyMMdd
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GS04() As String
        Get

            Return Left(buildFixWidth(_GS04, 8, "0"), 8)

        End Get
        Set(ByVal value As String)
            _GS04 = value
        End Set
    End Property
    Private _GS05 As String = ""
    ''' <summary>
    ''' min/max 4/8 Transmission/Create Time - HHmm 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GS05() As String
        Get

            Return Left(buildFixWidth(_GS05, 4, "0"), 8)

        End Get
        Set(ByVal value As String)
            _GS05 = value
        End Set
    End Property
    Private _GS06 As String = "1"
    ''' <summary>
    ''' min/max 1/9 Group Control Number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GS06() As String
        Get

            Return buildFixWidth(_GS06, 1, "1")

        End Get
        Set(ByVal value As String)
            _GS06 = value
        End Set
    End Property
    Private _GS07 As String = "X"
    ''' <summary>
    ''' min/max 1/2 Responsible Agency Code X for X12 or T for TDCC  Default is X
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GS07() As String
        Get

            Return Left(buildFixWidth(_GS07, 1, "X"), 2)

        End Get
        Set(ByVal value As String)
            _GS07 = value
        End Set
    End Property
    Private _GS08 As String = "004010"
    ''' <summary>
    ''' min/max 1/12 Version/Release/Industry Identifier Code; 
    ''' The first six characters specify the X12 version and release, while any of the last six may be used to indicate an Industry standard or Implementation Convention reference. “004010VICS
    ''' Default = 004010
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GS08() As String
        Get

            Return Left(buildFixWidth(_GS08, 1, "0"), 12)

        End Get
        Set(ByVal value As String)
            _GS08 = value
        End Set
    End Property
End Class

#End Region

#Region " GE  Function Group Trailer"

Public Class clsEDIGE
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is GE
            Select Case i
                Case 1
                    GE01 = sSegs(i)
                Case 2
                    GE02 = sSegs(i)
            End Select
        Next
    End Sub
    ''' <summary>
    ''' Generates a properly formatted EDI GE Segment using the current objects GE data previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "GE")
        End If

        Return sEdi.ToString
    End Function

    ''' <summary>
    ''' Generates a properly formatted EDI GE Segment using parameters or some default values
    ''' Typically used for testing
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <param name="intTotalTranSets"></param>
    ''' <param name="sGroupControl"></param>
    ''' <returns>String</returns>
    ''' <remarks>
    ''' Added by LVV on 4/11/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' Modified by RHR for v-7.0.6.105 on 5/31/2017 for v-6.0.4.70
    '''   cannot overload with only optional parameters changed intTotalTranSets to non-optional  
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String, ByVal intTotalTranSets As Integer, Optional ByVal sGroupControl As String = "1") As String

        Dim oGE As New clsEDIGE()
        With oGE
            .GE01 = intTotalTranSets
            .GE02 = sGroupControl
        End With
        Return getEDIString(sSegTerm)
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = GE01
            Case 2
                strRet = GE02
        End Select

        Return strRet
    End Function


    Private _GE01 As String = "1" 'Number of Transaction Sets Included in this Function Group
    Public Property GE01() As String
        Get

            Return buildFixWidth(_GE01, 1, "1")

        End Get
        Set(ByVal value As String)
            _GE01 = value
        End Set
    End Property
    Private _GE02 As String = "1" 'min/max 1/9 Group Control Number (same as GS06)
    Public Property GE02() As String
        Get

            Return buildFixWidth(_GE02, 1, "1")

        End Get
        Set(ByVal value As String)
            _GE02 = value
        End Set
    End Property
End Class

#End Region

#Region " ST - TRANSACTION SET HEADER (Envelope - Mandatory)"

Public Class clsEDIST
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is ST
            Select Case i
                Case 1
                    ST01 = sSegs(i)
                Case 2
                    ST02 = sSegs(i)
            End Select
        Next
    End Sub

    'Added by LVV on 4/11/16 for v-7.0.5.1 EDI Sim Testing Tool
    Public Sub New(ByVal docType As String, ByVal tranControlNo As String)
        MyBase.New()
        ST01 = docType
        ST02 = tranControlNo
    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Public Sub New(ByVal blnNoSpaces As Boolean)
        MyBase.New()

        NoSpaces = blnNoSpaces

    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _NoSpaces As Boolean = False
    Public Property NoSpaces() As Boolean
        Get
            Return _NoSpaces
        End Get
        Set(ByVal value As Boolean)
            _NoSpaces = value
        End Set
    End Property
    ''' <summary>
    ''' Generates a properly formatted EDI ST Segment using the current objects ST data previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "ST")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = ST01
            Case 2
                strRet = ST02
        End Select

        Return strRet
    End Function

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration

    Private _ST01 As String = ""
    ''' <summary>
    ''' min/max 3/3 RANSACTION SET IDENTIFIER CODE Like 204 for Motor Carrier Load Tender
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Property ST01() As String
        Get
            If NoSpaces Then
                If _ST01?.Trim().Length > 0 Then
                    Return Left(NDT.buildFixWidth(_ST01, 3, " ", True), 3)
                Else
                    Return Left(_ST01?.Trim(), 3)
                End If
            Else
                Return Left(NDT.buildFixWidth(_ST01, 3, " ", True), 3)
            End If

            Return Left(NDT.buildFixWidth(_ST01, 3, " ", True), 3)

        End Get
        Set(ByVal value As String)
            _ST01 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _ST02 As String = ""

    ''' <summary>
    ''' min/max 4/9 TRANSACTION SET CONTROL NUMBER Sequentially assigned by sender starting with one within each functional control header, incremented by one for each document
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Property ST02() As String
        Get
            If NoSpaces Then
                If _ST02?.Trim().Length > 0 Then
                    Return Left(NDT.buildFixWidth(_ST02, 4, "0", True), 9)
                Else
                    Return Left(_ST02?.Trim(), 9)
                End If
            Else
                Return Left(NDT.buildFixWidth(_ST02, 4, "0", True), 9)
            End If

        End Get
        Set(ByVal value As String)
            _ST02 = value
        End Set
    End Property
End Class

#End Region

#Region " SE - TRANSACTION SET TRAILER (Envelope - Mandatory)"

Public Class clsEDISE
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is SE
            Select Case i
                Case 1
                    SE01 = sSegs(i)
                Case 2
                    SE02 = sSegs(i)
            End Select
        Next
    End Sub

    'Added by LVV on 4/11/16 for v-7.0.5.1 EDI Sim Testing Tool
    Public Sub New(ByVal intSegments As String, ByVal tranControlNo As String)
        MyBase.New()
        SE01 = intSegments
        SE02 = tranControlNo
    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Public Sub New(ByVal blnNoSpaces As Boolean)
        MyBase.New()

        NoSpaces = blnNoSpaces

    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _NoSpaces As Boolean = False
    Public Property NoSpaces() As Boolean
        Get
            Return _NoSpaces
        End Get
        Set(ByVal value As Boolean)
            _NoSpaces = value
        End Set
    End Property
    ''' <summary>
    ''' Generates a properly formatted EDI SE Segment using the current objects SE data previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "SE")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = SE01
            Case 2
                strRet = SE02
        End Select

        Return strRet
    End Function


    Private _SE01 As String = "" 'max 10 NUMBER OF INCLUDED SEGMENTS Count of Segments Within the Transmission including ST and SE
    Public Property SE01() As String
        Get

            Return Left(_SE01, 10)

        End Get
        Set(ByVal value As String)
            _SE01 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _SE02 As String = ""

    ''' <summary>
    ''' min/max 4/9  TRANSACTION SET CONTROL NUMBER Control Number Assigned by the Sender in the ST Segment (ST02)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Property SE02() As String
        Get
            If NoSpaces Then
                If _SE02?.Trim().Length > 0 Then
                    Return Left(NDT.buildFixWidth(_SE02, 4, "0", True), 9)
                Else
                    Return Left(_SE02?.Trim(), 9)
                End If
            Else
                Return Left(NDT.buildFixWidth(_SE02, 4, "0", True), 9)
            End If
        End Get
        Set(ByVal value As String)
            _SE02 = value
        End Set
    End Property
End Class

#End Region

#Region " AK1 - Functional Group Response Header (matches the GS record)"

Public Class clsEDIAK1
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is AK1
            Select Case i
                Case 1
                    AK101 = sSegs(i)
                Case 2
                    AK102 = sSegs(i)
            End Select
        Next
    End Sub


    ''' <summary>
    ''' Overload used by the EDI Sim Testing tool 
    ''' </summary>
    ''' <param name="sGroupHeader"></param>
    ''' <param name="sGroupControl"></param>
    ''' <remarks>
    ''' Added by LVV on 4/13/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' 'Modified by RHR for v-7.0.6.105 on 5/31/2017 for v-6.0.4.70
    '''   cannot overload with only optional parameters changed intTotalTranSets to non-optional 
    ''' </remarks>
    Public Sub New(ByVal sGroupHeader As String, ByVal sGroupControl As String)
        MyBase.New()
        AK101 = sGroupHeader
        AK102 = sGroupControl
    End Sub

    ''' <summary>
    ''' Generates a properly formatted EDI Segment using the current child objects previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String

        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "AK1")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = AK101
            Case 2
                strRet = AK102
        End Select

        Return strRet
    End Function

    ''' <summary>
    ''' NGL Supported Values
    ''' QM - 214
    ''' IM = 210
    ''' SM = 204
    ''' </summary>
    ''' <remarks></remarks>
    Private _AK101 As String = "" 'min/max 2/2 functional group ID (GS01) of the functional group being acknowledged
    Public Property AK101() As String
        Get

            Return Left(buildFixWidth(_AK101, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _AK101 = value
        End Set
    End Property
    Private _AK102 As String = "" 'min/max  1/9 the group control number (GS06 and GE02) of the functional group being acknowledged
    Public Property AK102() As String
        Get

            Return Left(_AK102, 9)

        End Get
        Set(ByVal value As String)
            _AK102 = value
        End Set
    End Property
End Class

#End Region

#Region " AK2 - Transaction Set Response Header (matches each ST record in the GS transmitted)"

Public Class clsEDIAK2
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is AK2
            Select Case i
                Case 1
                    AK201 = sSegs(i)
                Case 2
                    AK202 = sSegs(i)
            End Select
        Next
    End Sub
    ''' <summary>
    ''' Generates a properly formatted EDI Segment using the current child objects previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String

        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "AK2")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = AK201
            Case 2
                strRet = AK202
        End Select

        Return strRet
    End Function


    Private _AK201 As String = "" 'min/max 3/3 transaction set ID (ST01) of the transaction set being acknowledged 
    Public Property AK201() As String
        Get

            Return Left(buildFixWidth(_AK201, 3, " ", False), 3)

        End Get
        Set(ByVal value As String)
            _AK201 = value
        End Set
    End Property
    Private _AK202 As String = "" 'min/max 4/9 transaction set control number (ST02 and SE02) of the transaction set being acknowledged 
    Public Property AK202() As String
        Get

            Return Left(buildFixWidth(_AK202, 4, " ", False), 9)

        End Get
        Set(ByVal value As String)
            _AK202 = value
        End Set
    End Property

End Class

#End Region

#Region " AK3 - Data Segment Note An AK3 segment is created for each segment in a transaction set that has one or more errors"

Public Class clsEDIAK3
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is AK3
            Select Case i
                Case 1
                    AK301 = sSegs(i)
                Case 2
                    AK302 = sSegs(i)
                Case 3
                    AK303 = sSegs(i)
                Case 4
                    AK304 = sSegs(i)
            End Select
        Next
    End Sub
    ''' <summary>
    ''' Generates a properly formatted EDI Segment using the current child objects previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String

        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 4 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "AK3")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = AK301
            Case 2
                strRet = AK302
            Case 3
                strRet = AK303
            Case 4
                strRet = AK304
        End Select

        Return strRet
    End Function


    Private _AK301 As String = "" 'min/max 1/3 identifies the segment in error with its X12 segment ID, for example, NM1 
    Public Property AK301() As String
        Get

            Return Left(_AK301, 3)

        End Get
        Set(ByVal value As String)
            _AK301 = value
        End Set
    End Property
    Private _AK302 As String = "" 'min/max 1/6 the segment count of the segment in error. The ST segment is "1" and each segment increments the segment 
    Public Property AK302() As String
        Get

            Return Left(_AK302, 6)

        End Get
        Set(ByVal value As String)
            _AK302 = value
        End Set
    End Property
    Private _AK303 As String = "" 'min/max 1/6 identifies a bounded loop 
    Public Property AK303() As String
        Get

            Return Left(_AK303, 6)

        End Get
        Set(ByVal value As String)
            _AK303 = value
        End Set
    End Property
    Private _AK304 As String = "" 'min/max 1/3 the error code for the error in the data segment 
    Public Property AK304() As String
        Get

            Return Left(_AK304, 3)

        End Get
        Set(ByVal value As String)
            _AK304 = value
        End Set
    End Property

End Class

#End Region

#Region " AK4 - (Not currently used) Data Segment Note An AK3 segment is created for each segment in a transaction set that has one or more errors"

Public Class clsEDIAK4
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is AK4
            Select Case i
                Case 1
                    AK401 = sSegs(i)
                Case 2
                    AK402 = sSegs(i)
                Case 3
                    AK403 = sSegs(i)
                Case 4
                    AK404 = sSegs(i)
            End Select
        Next
    End Sub
    ''' <summary>
    ''' Generates a properly formatted EDI Segment using the current child objects previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String

        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 4 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "AK4")
        End If
        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = AK401
            Case 2
                strRet = AK402
            Case 3
                strRet = AK403
            Case 4
                strRet = AK404
        End Select

        Return strRet
    End Function


    Private _AK401 As String = "" 'min/max 1/2 
    Public Property AK401() As String
        Get

            Return Left(_AK401, 2)

        End Get
        Set(ByVal value As String)
            _AK401 = value
        End Set
    End Property
    Private _AK402 As String = "" 'min/max 1/4 
    Public Property AK402() As String
        Get

            Return Left(_AK402, 4)

        End Get
        Set(ByVal value As String)
            _AK402 = value
        End Set
    End Property
    Private _AK403 As String = "" 'min/max 1/3 
    Public Property AK403() As String
        Get

            Return Left(_AK403, 3)

        End Get
        Set(ByVal value As String)
            _AK403 = value
        End Set
    End Property
    Private _AK404 As String = "" 'min/max 1/99  
    Public Property AK404() As String
        Get

            Return Left(_AK404, 99)

        End Get
        Set(ByVal value As String)
            _AK404 = value
        End Set
    End Property

End Class

#End Region

#Region " AK5 - (Not currently used) Data Segment Note An AK3 segment is created for each segment in a transaction set that has one or more errors"

Public Class clsEDIAK5
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is AK5
            Select Case i
                Case 1
                    AK501 = sSegs(i)
                Case 2
                    AK502 = sSegs(i)
                Case 3
                    AK503 = sSegs(i)
                Case 4
                    AK504 = sSegs(i)
                Case 5
                    AK505 = sSegs(i)
                Case 6
                    AK506 = sSegs(i)
            End Select
        Next
    End Sub


    ''' <summary>
    ''' Overload used to populate the AK501 segment with arQual value only ak502 is ignored
    ''' </summary>
    ''' <param name="arQual"></param>
    ''' <param name="ak502"></param>
    ''' <remarks>
    ''' Added by LVV on 4/13/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' Modified by RHR for v-7.0.6.105 on 5/31/2017 for v-6.0.4.70
    '''   cannot overload with only optional parameters changed intTotalTranSets to non-optional   
    ''' </remarks>
    Public Sub New(ByVal arQual As String, ByVal ak502 As String)
        MyBase.New()
        AK501 = arQual

    End Sub
    ''' <summary>
    ''' Generates a properly formatted EDI Segment using the current child objects previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String

        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 6 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "AK5")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = AK501
            Case 2
                strRet = AK502
            Case 3
                strRet = AK503
            Case 4
                strRet = AK504
            Case 5
                strRet = AK505
            Case 6
                strRet = AK506
        End Select

        Return strRet
    End Function

    Private _AK501 As String = "" 'min/max 1/1 specifies whether the identified transaction set is accepted or rejected
    Public Property AK501() As String
        Get

            Return Left(_AK501, 1)

        End Get
        Set(ByVal value As String)
            _AK501 = value
        End Set
    End Property
    Private _AK502 As String = "" 'min/max 1/3 indicates the nature of the error
    Public Property AK502() As String
        Get

            Return Left(_AK502, 3)

        End Get
        Set(ByVal value As String)
            _AK502 = value
        End Set
    End Property
    Private _AK503 As String = "" 'min/max 1/3 indicates the nature of the error
    Public Property AK503() As String
        Get

            Return Left(_AK503, 3)

        End Get
        Set(ByVal value As String)
            _AK503 = value
        End Set
    End Property
    Private _AK504 As String = "" 'min/max 1/3 indicates the nature of the error
    Public Property AK504() As String
        Get

            Return Left(_AK504, 3)

        End Get
        Set(ByVal value As String)
            _AK504 = value
        End Set
    End Property
    Private _AK505 As String = "" 'min/max 1/3 indicates the nature of the error
    Public Property AK505() As String
        Get

            Return Left(_AK505, 3)

        End Get
        Set(ByVal value As String)
            _AK505 = value
        End Set
    End Property
    Private _AK506 As String = "" 'min/max 1/3 indicates the nature of the error
    Public Property AK506() As String
        Get

            Return Left(_AK506, 3)

        End Get
        Set(ByVal value As String)
            _AK506 = value
        End Set
    End Property

End Class

#End Region

#Region " AK9 - Functional Group Response Trailer"

Public Class clsEDIAK9
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is AK9
            Select Case i
                Case 1
                    AK901 = sSegs(i)
                Case 2
                    AK902 = sSegs(i)
                Case 3
                    AK903 = sSegs(i)
                Case 4
                    AK904 = sSegs(i)
                Case 5
                    AK905 = sSegs(i)
                Case 6
                    AK906 = sSegs(i)
                Case 7
                    AK907 = sSegs(i)
                Case 8
                    AK908 = sSegs(i)
                Case 9
                    AK909 = sSegs(i)
            End Select
        Next
    End Sub

    'Added by LVV on 4/13/16 for v-7.0.5.1 EDI Sim Testing Tool
    Public Sub New(ByVal arQual As String, ByVal numberAccepted As String, Optional ByVal intTotalTranSets As Integer = 1)
        MyBase.New()
        AK901 = arQual
        AK902 = intTotalTranSets
        AK903 = "1"
        AK904 = numberAccepted
    End Sub
    ''' <summary>
    ''' Generates a properly formatted EDI Segment using the current child objects previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 9 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "AK9")
        End If
        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = AK901
            Case 2
                strRet = AK902
            Case 3
                strRet = AK903
            Case 4
                strRet = AK904
            Case 5
                strRet = AK905
            Case 6
                strRet = AK906
            Case 7
                strRet = AK907
            Case 8
                strRet = AK908
            Case 9
                strRet = AK909
        End Select

        Return strRet
    End Function

    Private _AK901 As String = "" 'min/max 1/1 mandatory (specifies whether the functional group identified in AK1 is accepted or rejected A or E = Accepted)
    Public Property AK901() As String
        Get

            Return Left(_AK901, 1)

        End Get
        Set(ByVal value As String)
            _AK901 = value
        End Set
    End Property
    Private _AK902 As String = "" 'min/max 1/6 the number of transaction sets included in the identified functional group trailer (GE01).
    Public Property AK902() As String
        Get

            Return Left(_AK902, 6)

        End Get
        Set(ByVal value As String)
            _AK902 = value
        End Set
    End Property
    Private _AK903 As String = "" 'min/max 1/6 the number of transaction sets received
    Public Property AK903() As String
        Get

            Return Left(_AK903, 6)

        End Get
        Set(ByVal value As String)
            _AK903 = value
        End Set
    End Property
    Private _AK904 As String = "" 'min/max 1/6 indicates the number of transaction sets accepted in the identified functional group
    Public Property AK904() As String
        Get

            Return Left(_AK904, 6)

        End Get
        Set(ByVal value As String)
            _AK904 = value
        End Set
    End Property
    Private _AK905 As String = "" 'min/max 1/3 errors noted in the identified functional group
    Public Property AK905() As String
        Get

            Return Left(_AK905, 3)

        End Get
        Set(ByVal value As String)
            _AK905 = value
        End Set
    End Property
    Private _AK906 As String = "" 'min/max 1/3 errors noted in the identified functional group
    Public Property AK906() As String
        Get

            Return Left(_AK906, 3)

        End Get
        Set(ByVal value As String)
            _AK906 = value
        End Set
    End Property
    Private _AK907 As String = "" 'min/max 1/3 errors noted in the identified functional group
    Public Property AK907() As String
        Get

            Return Left(_AK907, 3)

        End Get
        Set(ByVal value As String)
            _AK907 = value
        End Set
    End Property
    Private _AK908 As String = "" 'min/max 1/3 errors noted in the identified functional group
    Public Property AK908() As String
        Get

            Return Left(_AK908, 3)

        End Get
        Set(ByVal value As String)
            _AK908 = value
        End Set
    End Property
    Private _AK909 As String = "" 'min/max 1/3 errors noted in the identified functional group
    Public Property AK909() As String
        Get

            Return Left(_AK909, 3)

        End Get
        Set(ByVal value As String)
            _AK909 = value
        End Set
    End Property

End Class

#End Region

#Region " AT7 - SHIPMENT STATUS DETAILS (Header - Optional) "

Public Class clsEDIAT7
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is AT7
            Select Case i
                Case 1
                    AT701 = sSegs(i)
                Case 2
                    AT702 = sSegs(i)
                Case 3
                    AT703 = sSegs(i)
                Case 4
                    AT704 = sSegs(i)
                Case 5
                    AT705 = sSegs(i)
                Case 6
                    AT706 = sSegs(i)
                Case 7
                    AT707 = sSegs(i)
            End Select
        Next
    End Sub
    ''' <summary>
    ''' Generates a properly formatted EDI Segment using the current child objects previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 7 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "AT7")
        End If
        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = AT701
            Case 2
                strRet = AT702
            Case 3
                strRet = AT703
            Case 4
                strRet = AT704
            Case 5
                strRet = AT705
            Case 6
                strRet = AT706
            Case 7
                strRet = AT707
        End Select

        Return strRet
    End Function


    Private _AT701 As String = "" 'min/max 2/2 SHIPMENT STATUS CODE AF - Carrier departed Pickup Location,CD - Carrier departed Delivery Location,SD - Shipment Delayed,X1 - Arrived at Delivery Location,X2 – Estimated Arrival Date/Time (Required after every depart location), X3 - Arrived at Pickup,X6 - Enroute to Delivery Location (See MS101)
    Public Property AT701() As String
        Get

            Return Left(buildFixWidth(_AT701, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _AT701 = value
        End Set
    End Property
    Private _AT702 As String = "" 'min/max 2/2 SHIPMENT STATUS OR APPOINTMENT REASON CODE NA - Normal Appointment,BG – Other,BF  -  Carrier Keying Error,AG - Consignee Related,AI - Mechanical Breakdown,AJ - Other Carrier Related,AM - Shipper Related,NA - Normal Appointment   
    Public Property AT702() As String
        Get

            Return Left(buildFixWidth(_AT702, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _AT702 = value
        End Set
    End Property
    Private _AT703 As String = "" 'min/max 2/2 SHIPMENT APPOINTMENT STATUS CODE AA - Pick-up Appointment Date and/or Time,AB - Delivery Appointment Date and/or Time
    Public Property AT703() As String
        Get

            Return Left(buildFixWidth(_AT703, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _AT703 = value
        End Set
    End Property
    Private _AT704 As String = "" 'min/max 2/2 SHIPMENT STATUS OR APPOINTMENT REASON CODE NA - Normal Appointment,BG – Other,BF  -  Carrier Keying Error,AG - Consignee Related,AI - Mechanical Breakdown,AJ - Other Carrier Related,AM - Shipper Related,NA - Normal Appointment   
    Public Property AT704() As String
        Get

            Return Left(buildFixWidth(_AT704, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _AT704 = value
        End Set
    End Property
    Private _AT705 As String = "" 'min/max 8/8 DATE Like Appointment Date
    Public Property AT705() As String
        Get

            Return Left(buildFixWidth(_AT705, 8, "0", True), 8)

        End Get
        Set(ByVal value As String)
            _AT705 = value
        End Set
    End Property
    Private _AT706 As String = "" 'min/max 4/8 TIME like Appointment Time
    Public Property AT706() As String
        Get

            Return Left(buildFixWidth(_AT706, 4, " ", False), 8)

        End Get
        Set(ByVal value As String)
            _AT706 = value
        End Set
    End Property
    Private _AT707 As String = "" 'min/max 2/2 TIME CODE LT = Local Time                   
    Public Property AT707() As String
        Get

            Return Left(buildFixWidth(_AT707, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _AT707 = value
        End Set
    End Property
End Class

#End Region

#Region " AT8 - SHIPMENT WEIGHT  (Header - Optional) "

Public Class clsEDIAT8
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is AT8
            Select Case i
                Case 1
                    AT801 = sSegs(i)
                Case 2
                    AT802 = sSegs(i)
                Case 3
                    AT803 = sSegs(i)
                Case 4
                    AT804 = sSegs(i)
                Case 5
                    AT805 = sSegs(i)
                Case 6
                    AT806 = sSegs(i)
                Case 7
                    AT807 = sSegs(i)
            End Select
        Next
    End Sub
#Disable Warning BC42304 ' XML documentation parse error: XML end element must be preceded by a matching start element. XML comment will be ignored.
    ''' <summary>
    ''' Generates a properly formatted EDI Segment using the current child objects previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String
#Enable Warning BC42304 ' XML documentation parse error: XML end element must be preceded by a matching start element. XML comment will be ignored.
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 7 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "AT8")
        End If
        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = AT801
            Case 2
                strRet = AT802
            Case 3
                strRet = AT803
            Case 4
                strRet = AT804
            Case 5
                strRet = AT805
            Case 6
                strRet = AT806
            Case 7
                strRet = AT807
        End Select

        Return strRet
    End Function

    Private _AT801 As String = "" 'min/max 1/2 WEIGHT QUALIFIER G - Gross Weight
    Public Property AT801() As String
        Get

            Return Left(_AT801, 2)

        End Get
        Set(ByVal value As String)
            _AT801 = value
        End Set
    End Property
    Private _AT802 As String = "" 'min/max 1/1 WEIGHT UNIT CODE L - Pounds
    Public Property AT802() As String
        Get

            Return Left(_AT802, 1)

        End Get
        Set(ByVal value As String)
            _AT802 = value
        End Set
    End Property
    Private _AT803 As String = "" 'min/max 1/10 WEIGHT
    Public Property AT803() As String
        Get

            Return Left(_AT803, 10)

        End Get
        Set(ByVal value As String)
            _AT803 = value
        End Set
    End Property
    Private _AT804 As String = "" 'min/max 1/7 LADING QUANTITY Number of actual pallets picked up or delivered
    Public Property AT804() As String
        Get

            Return Left(_AT804, 7)

        End Get
        Set(ByVal value As String)
            _AT804 = value
        End Set
    End Property
    Private _AT805 As String = "" 'min/max 1/7 LADING QUANTITY Number of pieces like cases                 
    Public Property AT805() As String
        Get

            Return Left(_AT805, 7)

        End Get
        Set(ByVal value As String)
            _AT805 = value
        End Set
    End Property
    Private _AT806 As String = "" 'min/max 1/1 VOLUME UNIT QUALIFIER             
    Public Property AT806() As String
        Get

            Return Left(_AT806, 1)

        End Get
        Set(ByVal value As String)
            _AT806 = value
        End Set
    End Property
    Private _AT807 As String = "" 'min/max 1/8 VOLUME                
    Public Property AT807() As String
        Get

            Return Left(_AT807, 8)

        End Get
        Set(ByVal value As String)
            _AT807 = value
        End Set
    End Property
End Class

#End Region


#Region " B1 - BEGINNING SEGMENT FOR BOOKING PICKUP/DELIVERY (Header - Mandatory)"

Public Class clsEDIB1
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is B1
            Select Case i
                Case 1
                    B101 = sSegs(i)
                Case 2
                    B102 = sSegs(i)
                Case 3
                    B103 = sSegs(i)
                Case 4
                    B104 = sSegs(i)
            End Select
        Next
    End Sub


    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="SCAC"></param>
    ''' <param name="SHID"></param>
    ''' <param name="strDate"></param>
    ''' <param name="ActionCode"></param>
    ''' <remarks>
    ''' Added by LVV on 4/11/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub New(ByVal SCAC As String, ByVal SHID As String, ByVal strDate As String, ByVal ActionCode As String)
        MyBase.New()
        B101 = SCAC
        B102 = SHID
        B103 = strDate
        B104 = ActionCode
    End Sub
    ''' <summary>
    ''' Generates a properly formatted EDI Segment using the current child objects previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 4 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "B1")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = B101
            Case 2
                strRet = B102
            Case 3
                strRet = B103
            Case 4
                strRet = B104
        End Select

        Return strRet
    End Function


    Private _B101 As String = "" 'min/max 2/4 Standard Carrier Alpha Code
    Public Property B101() As String
        Get

            Return Left(buildFixWidth(_B101, 2, " ", False), 4)

        End Get
        Set(ByVal value As String)
            _B101 = value
        End Set
    End Property
    Private _B102 As String = "" 'min/max 1/30 Shipment Identification Number (This usually matches what was sent in the B206 on the 204.)
    Public Property B102() As String
        Get

            Return Left(_B102, 30)

        End Get
        Set(ByVal value As String)
            _B102 = value
        End Set
    End Property
    Private _B103 As String = "" 'min/max 8/8 DATE (CCYYMMDD)
    Public Property B103() As String
        Get

            Return Left(buildFixWidth(_B103, 8, "0", True), 8)
        End Get
        Set(ByVal value As String)
            _B103 = value
        End Set
    End Property
    Private _B104 As String = "" 'min/max 1/1 Reservation Action Code (A – Reservation Accepted,D – Reservation Cancelled,r-Delete)
    Public Property B104() As String
        Get

            Return Left(_B104, 1)

        End Get
        Set(ByVal value As String)
            _B104 = value
        End Set
    End Property

End Class

#End Region

#Region " B2 - BEGINNING SEGMENT FOR SHIPMENT (Header - Mandatory)"

Public Class clsEDIB2
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is B2
            Select Case i
                Case 1
                    B201 = sSegs(i)
                Case 2
                    B202 = sSegs(i)
                Case 3
                    B203 = sSegs(i)
                Case 4
                    B204 = sSegs(i)
                Case 5
                    B205 = sSegs(i)
                Case 6
                    B206 = sSegs(i)
                Case 7
                    B207 = sSegs(i)
                Case 8
                    B208 = sSegs(i)
                Case 9
                    B209 = sSegs(i)
                Case 10
                    B210 = sSegs(i)
                Case 11
                    B211 = sSegs(i)
                Case 12
                    B212 = sSegs(i)
            End Select
        Next
    End Sub

    'Added by LVV 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    Public Sub New(ByVal blnNoSpaces As Boolean)
        MyBase.New()

        NoSpaces = blnNoSpaces

    End Sub

    'Added by LVV 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    Private _NoSpaces As Boolean = False
    Public Property NoSpaces() As Boolean
        Get
            Return _NoSpaces
        End Get
        Set(ByVal value As Boolean)
            _NoSpaces = value
        End Set
    End Property
    ''' <summary>
    ''' Generates a properly formatted EDI Segment using the current child objects previously populated
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 12 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "B2")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = B201
            Case 2
                strRet = B202
            Case 3
                strRet = B203
            Case 4
                strRet = B204
            Case 5
                strRet = B205
            Case 6
                strRet = B206
            Case 7
                strRet = B207
            Case 8
                strRet = B208
            Case 9
                strRet = B209
            Case 10
                strRet = B210
            Case 11
                strRet = B211
            Case 12
                strRet = B212
        End Select

        Return strRet
    End Function


    Private _B201 As String = "DD"
    ''' <summary>
    ''' min/max 2/2 TARIFF SERVICE CODE
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' Modified by RHR for v-7.0.6.105 on 5/16/2017 
    '''     add check for null reference before .trim using new conditional operator ?
    ''' </remarks>
    Public Property B201() As String
        Get
            If NoSpaces Then
                If _B201?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B201, 2, " ", False), 2)
                Else
                    Return Left(_B201?.Trim(), 2)
                End If
            Else
                Return Left(buildFixWidth(_B201, 2, " ", False), 2)
            End If
        End Get
        Set(ByVal value As String)
            _B201 = value
        End Set
    End Property
    'Modified by LVV 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    Private _B202 As String = "" 'min/max 2/4 STANDARD CARRIER ALPHA Carrier SCAC for assigned loads
    Public Property B202() As String
        Get
            If NoSpaces Then
                If _B202?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B202, 2, " ", False), 4)
                Else
                    Return Left(_B202?.Trim(), 4)
                End If
            Else
                Return Left(buildFixWidth(_B202, 2, " ", False), 4)
            End If
        End Get
        Set(ByVal value As String)
            _B202 = value
        End Set
    End Property
    'Modified by LVV 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    Private _B203 As String = "" 'min/max 6/9 STANDARD POINT LOCATION CODE
    Public Property B203() As String
        Get
            If NoSpaces Then
                If _B203?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B203, 6, " ", False), 9)
                Else
                    Return Left(_B203?.Trim(), 9)
                End If
            Else
                Return Left(buildFixWidth(_B203, 6, " ", False), 9)
            End If
        End Get
        Set(ByVal value As String)
            _B203 = value
        End Set
    End Property
    Private _B204 As String = "" 'min/max 1/30 SHIPMENT IDENTIFICATION assigned Load Number
    Public Property B204() As String
        Get

            Return Left(_B204, 30)

        End Get
        Set(ByVal value As String)
            _B204 = value
        End Set
    End Property
    Private _B205 As String = "L" 'min/max 1/1 WEIGHT UNIT CODE L – Pounds
    Public Property B205() As String
        Get

            Return Left(_B205, 1)

        End Get
        Set(ByVal value As String)
            _B205 = value
        End Set
    End Property
    'Modified by LVV 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    Private _B206 As String = "" 'min/max 2/2 SHIPMENT METHOD OF PAYMENT (PP-Prepaid, CC-collect,TP-Third party
    Public Property B206() As String
        Get
            If NoSpaces Then
                If _B206?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B206, 2, " ", False), 2)
                Else
                    Return Left(_B206?.Trim(), 2)
                End If
            Else
            Return Left(buildFixWidth(_B206, 2, " ", False), 2)
            End If
        End Get
        Set(ByVal value As String)
            _B206 = value
        End Set
    End Property
    Private _B207 As String = "" 'min/max 1/1 SHIPMENT QUALIFIER
    Public Property B207() As String
        Get

            Return Left(_B207, 1)

        End Get
        Set(ByVal value As String)
            _B207 = value
        End Set
    End Property
    Private _B208 As String = "" 'min/max 1/3 TOTAL EQUIPMENT
    Public Property B208() As String
        Get

            Return Left(_B208, 3)

        End Get
        Set(ByVal value As String)
            _B208 = value
        End Set
    End Property
    Private _B209 As String = "E" 'min/max 1/1 SHIPMENT WEIGHT CODE E – Estimated Weight
    Public Property B209() As String
        Get

            Return Left(_B209, 1)

        End Get
        Set(ByVal value As String)
            _B209 = value
        End Set
    End Property
    'Modified by LVV 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    Private _B210 As String = "" 'min/max 2/2 CUSTOMS DOCUMENTATION HANDLING CODE
    Public Property B210() As String
        Get
            If NoSpaces Then
                If _B210?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B210, 2, " ", False), 2)
                Else
                    Return Left(_B210?.Trim(), 2)
                End If
            Else
            Return Left(buildFixWidth(_B210, 2, " ", False), 2)
            End If
        End Get
        Set(ByVal value As String)
            _B210 = value
        End Set
    End Property
    'Modified by LVV 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    Private _B211 As String = "" 'min/max 3/3 TRANSPORTATION TERMS CODE
    Public Property B211() As String
        Get
            If NoSpaces Then
                If _B211?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B211, 3, " ", False), 3)
                Else
                    Return Left(_B211?.Trim(), 3)
                End If
            Else
            Return Left(buildFixWidth(_B211, 3, " ", False), 3)
            End If
        End Get
        Set(ByVal value As String)
            _B211 = value
        End Set
    End Property
    'Modified by LVV 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    Private _B212 As String = "" 'min/max 3/3 PAYMENT METHOD CODE
    Public Property B212() As String
        Get
            If NoSpaces Then
                If _B212?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B212, 3, " ", False), 3)
                Else
                    Return Left(_B212?.Trim(), 3)
                End If
            Else
            Return Left(buildFixWidth(_B212, 3, " ", False), 3)
            End If
        End Get
        Set(ByVal value As String)
            _B212 = value
        End Set
    End Property

End Class

#End Region

#Region " B2A - SET PURPOSE (Header - Mandatory)"

Public Class clsEDIB2A
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is B2A
            Select Case i
                Case 1
                    B2A01 = sSegs(i)
                Case 2
                    B2A02 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "B2A")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = B2A01
            Case 2
                strRet = B2A02
        End Select

        Return strRet
    End Function

    Private _B2A01 As String = "" 'min/max 2/2 TRANSACTION SET PURPOSE CODE (00-original,01-cancelation,04-change)
    Public Property B2A01() As String
        Get

            Return Left(buildFixWidth(_B2A01, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _B2A01 = value
        End Set
    End Property
    Private _B2A02 As String = "" 'min/max 2/2 APPLICATION TYPE (LT - Load Tender – Truckload)
    Public Property B2A02() As String
        Get

            Return Left(buildFixWidth(_B2A02, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _B2A02 = value
        End Set
    End Property

End Class

#End Region

#Region " B3 - BEGINNING SEGMENT FOR CARRIERS INVOICE (Header - Mandatory)"

Public Class clsEDIB3
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is B3
            Select Case i
                Case 1
                    B301 = sSegs(i)
                Case 2
                    B302 = sSegs(i)
                Case 3
                    B303 = sSegs(i)
                Case 4
                    B304 = sSegs(i)
                Case 5
                    B305 = sSegs(i)
                Case 6
                    B306 = sSegs(i)
                Case 7
                    B307 = sSegs(i)
                Case 8
                    B308 = sSegs(i)
                Case 9
                    B309 = sSegs(i)
                Case 10
                    B310 = sSegs(i)
                Case 11
                    B311 = sSegs(i)
                Case 12
                    B312 = sSegs(i)
            End Select
        Next
    End Sub


    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="FrtBillNo"></param>
    ''' <param name="SHID"></param>
    ''' <param name="strBilledDate"></param>
    ''' <param name="strTotalCost"></param>
    ''' <remarks>
    ''' Added by LVV on 4/13/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub New(ByVal FrtBillNo As String, ByVal SHID As String, ByVal strBilledDate As String, ByVal strTotalCost As String, Optional ByVal blnNoSpaces As Boolean = False)
        MyBase.New()
        B302 = FrtBillNo
        B303 = SHID
        B304 = "PP"
        B306 = strBilledDate
        B307 = strTotalCost
        NoSpaces = blnNoSpaces
    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Public Sub New(ByVal blnNoSpaces As Boolean)
        MyBase.New()

        NoSpaces = blnNoSpaces

    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _NoSpaces As Boolean = False
    Public Property NoSpaces() As Boolean
        Get
            Return _NoSpaces
        End Get
        Set(ByVal value As Boolean)
            _NoSpaces = value
        End Set
    End Property

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 12 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "B3")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = B301
            Case 2
                strRet = B302
            Case 3
                strRet = B303
            Case 4
                strRet = B304
            Case 5
                strRet = B305
            Case 6
                strRet = B306
            Case 7
                strRet = B307
            Case 8
                strRet = B308
            Case 9
                strRet = B309
            Case 10
                strRet = B310
            Case 11
                strRet = B311
            Case 12
                strRet = B312
        End Select

        Return strRet
    End Function


    Private _B301 As String = "" 'min/max 0/1 SHIPMENT QUALIFIER
    Public Property B301() As String
        Get

            Return Left(_B301, 1)

        End Get
        Set(ByVal value As String)
            _B301 = value
        End Set
    End Property
    Private _B302 As String = "" 'min/max 1/22 INVOICE NUMBER
    Public Property B302() As String
        Get

            Return Left(_B302, 22)

        End Get
        Set(ByVal value As String)
            _B302 = value
        End Set
    End Property
    Private _B303 As String = "" 'min/max 0/30 SHIPMENT IDENTIFICATION NUMBER
    Public Property B303() As String
        Get

            Return Left(_B303, 30)

        End Get
        Set(ByVal value As String)
            _B303 = value
        End Set
    End Property
    Private _B304 As String = "" 'min/max 1/2 SHIPMENT METHOD OF PAYMENT CC – Collect  TP – third Party  P – Pre-Paid
    Public Property B304() As String
        Get

            Return Left(_B304, 2)

        End Get
        Set(ByVal value As String)
            _B304 = value
        End Set
    End Property
    Private _B305 As String = "L" 'min/max 1/1 WEIGHT UNIT CODE L – Pounds
    Public Property B305() As String
        Get

            Return Left(_B305, 1)

        End Get
        Set(ByVal value As String)
            _B305 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _B306 As String = "" 'min/max 8/8 DATE  YYYYMMDD
    Public Property B306() As String
        Get
            If NoSpaces Then
                If _B306?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B306, 8, "0", True), 8)
                Else
                    Return Left(_B306?.Trim(), 8)
                End If
            Else
            Return Left(buildFixWidth(_B306, 8, "0", True), 8)
            End If
        End Get
        Set(ByVal value As String)
            _B306 = value
        End Set
    End Property
    Private _B307 As String = "" 'min/max 1/12 NET AMOUNT DUE
    Public Property B307() As String
        Get

            Return Left(_B307, 12)

        End Get
        Set(ByVal value As String)
            _B307 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _B308 As String = "" 'min/max 2/2 CORRECTION INDICATOR
    Public Property B308() As String
        Get
            If NoSpaces Then
                If _B308?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B308, 2, " ", False), 2)
                Else
                    Return Left(_B308?.Trim(), 2)
                End If
            Else
            Return Left(buildFixWidth(_B308, 2, " ", False), 2)
            End If
        End Get
        Set(ByVal value As String)
            _B308 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _B309 As String = "" 'min/max 8/8 DELIVERY DATE  YYYYMMDD
    Public Property B309() As String
        Get
            If NoSpaces Then
                If _B309?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B309, 8, "0", True), 8)
                Else
                    Return Left(_B309?.Trim(), 8)
                End If
            Else
            Return Left(buildFixWidth(_B309, 8, "0", True), 8)
            End If
        End Get
        Set(ByVal value As String)
            _B309 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _B310 As String = "" 'min/max 3/3 DATE/TIME QUALIFIER (like 035 - Delivered)
    Public Property B310() As String
        Get
            If NoSpaces Then
                If _B310?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B310, 3, "0", True), 3)
                Else
                    Return Left(_B310?.Trim(), 3)
                End If
            Else
            Return Left(buildFixWidth(_B310, 3, "0", True), 3)
            End If
        End Get
        Set(ByVal value As String)
            _B310 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _B311 As String = "" 'min/max 2/4 STANDARD CARRIER ALPHA CODE (like RUAN)
    Public Property B311() As String
        Get
            If NoSpaces Then
                If _B311?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B311, 2, " ", False), 4)
                Else
                    Return Left(_B311?.Trim(), 4)
                End If
            Else
            Return Left(buildFixWidth(_B311, 2, " ", False), 4)
            End If
        End Get
        Set(ByVal value As String)
            _B311 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _B312 As String = "" 'min/max 8/8 SHIP DATE (optional) YYYYMMDD
    Public Property B312() As String
        Get
            If NoSpaces Then
                If _B312?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_B312, 8, "0", True), 8)
                Else
                    Return Left(_B312?.Trim(), 8)
                End If
            Else
            Return Left(buildFixWidth(_B312, 8, "0", True), 8)
            End If
        End Get
        Set(ByVal value As String)
            _B312 = value
        End Set
    End Property

End Class

#End Region

#Region " B10 - BEGINNING SEGMENT FOR TRANSPORTATION  (Header - Mandatory)"

Public Class clsEDIB10
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is B10
            Select Case i
                Case 1
                    B1001 = sSegs(i)
                Case 2
                    B1002 = sSegs(i)
                Case 3
                    B1003 = sSegs(i)
                Case 4
                    B1004 = sSegs(i)
                Case 5
                    B1005 = sSegs(i)
                Case 6
                    B1006 = sSegs(i)
                Case 7
                    B1007 = sSegs(i)
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="CarrierPRO"></param>
    ''' <param name="SHID"></param>
    ''' <param name="SCAC"></param>
    ''' <remarks>
    ''' Added by LVV on 4/14/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub New(ByVal CarrierPRO As String, ByVal SHID As String, ByVal SCAC As String)
        MyBase.New()
        B1001 = CarrierPRO
        B1002 = SHID
        B1003 = SCAC
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 7 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "B10")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = B1001
            Case 2
                strRet = B1002
            Case 3
                strRet = B1003
            Case 4
                strRet = B1004
            Case 5
                strRet = B1005
            Case 6
                strRet = B1006
            Case 7
                strRet = B1007
        End Select

        Return strRet
    End Function


    Private _B1001 As String = "" 'min/max 1/30 REFERENCE IDENTIFICATION Carrier BOL Number
    Public Property B1001() As String
        Get

            Return Left(_B1001, 30)

        End Get
        Set(ByVal value As String)
            _B1001 = value
        End Set
    End Property
    Private _B1002 As String = "" 'min/max 1/30 SHIPMENT IDENTIFICATION NUMBER FreightMaster BOL Number
    Public Property B1002() As String
        Get

            Return Left(_B1002, 30)

        End Get
        Set(ByVal value As String)
            _B1002 = value
        End Set
    End Property
    Private _B1003 As String = "" 'min/max 2/4 STANDARD CARRIER ALPHA CODE  Carrier SCAC Code     
    Public Property B1003() As String
        Get

            Return Left(buildFixWidth(_B1003, 2, " ", False), 4)

        End Get
        Set(ByVal value As String)
            _B1003 = value
        End Set
    End Property
    Private _B1004 As String = "" 'min/max 1/3 INQUIRY REQUEST NUMBER            
    Public Property B1004() As String
        Get

            Return Left(_B1004, 3)

        End Get
        Set(ByVal value As String)
            _B1004 = value
        End Set
    End Property
    Private _B1005 As String = "" 'min/max 2/3 REFERENCE IDENTIFICATION QUALIFIER
    Public Property B1005() As String
        Get

            Return Left(buildFixWidth(_B1005, 2, " ", False), 3)

        End Get
        Set(ByVal value As String)
            _B1005 = value
        End Set
    End Property
    Private _B1006 As String = "" 'min/max 1/30 REFERENCE IDENTIFICATION          
    Public Property B1006() As String
        Get

            Return Left(_B1006, 30)

        End Get
        Set(ByVal value As String)
            _B1006 = value
        End Set
    End Property
    Private _B1007 As String = "" 'min/max 1/1 YES/NO CONDITION OR RESPONSE CODE Y for Yes and N for No
    Public Property B1007() As String
        Get

            Return Left(_B1007, 1)

        End Get
        Set(ByVal value As String)
            _B1007 = value
        End Set
    End Property
End Class

#End Region

#Region " C3 - Currency"

Public Class clsEDIC3

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is C3
            Select Case i
                Case 1
                    C301 = sSegs(i)
            End Select
        Next
    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Public Sub New(ByVal blnNoSpaces As Boolean)
        MyBase.New()

        NoSpaces = blnNoSpaces

    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _NoSpaces As Boolean = False
    Public Property NoSpaces() As Boolean
        Get
            Return _NoSpaces
        End Get
        Set(ByVal value As Boolean)
            _NoSpaces = value
        End Set
    End Property

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 3 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "C3")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = C301
        End Select

        Return strRet
    End Function

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _C301 As String = "USD" 'min/max 3/3 CURRENCY CODE like USD - United States Dollars
    Public Property C301() As String
        Get
            If NoSpaces Then
                If _C301?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_C301, 3, " ", False), 3)
                Else
                    Return Left(_C301?.Trim(), 3)
                End If
            Else
            Return Left(buildFixWidth(_C301, 3, " ", False), 3)
            End If
        End Get
        Set(ByVal value As String)
            _C301 = value
        End Set
    End Property

End Class

#End Region

#Region " G61 - CONTACT (Header - Optional) "

Public Class clsEDIG61
    Public Sub New()
        MyBase.New()
        populateElements()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        populateElements()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is G61
            Select Case i
                Case 1
                    G6101 = sSegs(i)
                Case 2
                    G6102 = sSegs(i)
                Case 3
                    G6103 = sSegs(i)
                Case 4
                    G6104 = sSegs(i)
            End Select
        Next
    End Sub

    Private Sub populateElements()
        Dim sElem As String() = New String(3) {"Contact Function Code", "Name", "Communication Number Qualifier", "Communication Number"}
        Me.Elements = sElem
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 4 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "G61")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = G6101
            Case 2
                strRet = G6102
            Case 3
                strRet = G6103
            Case 4
                strRet = G6104
        End Select

        Return strRet
    End Function

    Public Function getEDIText() As String
        Dim sEdi As New System.Text.StringBuilder("", 500)
        sEdi.Append(Me.Description)
        sEdi.Append(vbCrLf)
        For i As Integer = 1 To 4 Step -1

            Dim strVal As String = getDataByIndex(i)
            If Not String.IsNullOrEmpty(strVal) Then
                sEdi.Append(Elements(i))
                sEdi.Append(vbCrLf)
                sEdi.Append(strVal)
                sEdi.Append(vbCrLf)
            End If

        Next
        Return sEdi.ToString
    End Function

    Private _Elements As String()
    Public Property Elements() As String()
        Get
            Return _Elements
        End Get
        Set(ByVal value As String())
            _Elements = value
        End Set
    End Property

    Private _Description As String = "Contact"
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Private _G6101 As String = ""
    ''' <summary>
    ''' min/max 2/2 Contact Function Code - Code identifying the major duty or responsibility of the person or group named 
    ''' Like HM Hazardous Material Contact or (BI - Billing Inquiry Contact)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property G6101() As String
        Get

            Return Left(buildFixWidth(_G6101, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _G6101 = value
        End Set
    End Property
    Private _G6102 As String = ""
    ''' <summary>
    ''' min/max 1/60 Name - Free-form name 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property G6102() As String
        Get

            Return Left(_G6102, 60)

        End Get
        Set(ByVal value As String)
            _G6102 = value
        End Set
    End Property
    Private _G6103 As String = ""
    ''' <summary>
    ''' min/max 2/2 Communication Number Qualifier - Code identifying the type of communication number 
    ''' Like TE Telephone
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property G6103() As String
        Get

            Return Left(buildFixWidth(_G6103, 2, " ", False), 60)

        End Get
        Set(ByVal value As String)
            _G6103 = value
        End Set
    End Property
    Private _G6104 As String = ""
    ''' <summary>
    ''' min/max 1/80 Communication Number - Complete communications number including country or area code when applicable 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property G6104() As String
        Get

            Return Left(_G6104, 80)

        End Get
        Set(ByVal value As String)
            _G6104 = value
        End Set
    End Property
End Class

#End Region

#Region " G62 - DATE/TIME (Detail - Optional)  "

Public Class clsEDIG62
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is G62
            Select Case i
                Case 1
                    G6201 = sSegs(i)
                Case 2
                    G6202 = sSegs(i)
                Case 3
                    G6203 = sSegs(i)
                Case 4
                    G6204 = sSegs(i)
                Case 5
                    G6205 = sSegs(i)
            End Select
        Next
    End Sub


    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="strDateQualifier"></param>
    ''' <param name="strDate"></param>
    ''' <param name="strTimeQualifier"></param>
    ''' <param name="strTime"></param>
    ''' <param name="strTimeCode"></param>
    ''' <remarks>
    ''' Added by LVV on 4/13/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub New(ByVal strDateQualifier As String, ByVal strDate As String, Optional ByVal strTimeQualifier As String = "", Optional ByVal strTime As String = "", Optional ByVal strTimeCode As String = "")
        MyBase.New()
        G6201 = strDateQualifier
        G6202 = strDate
        If strTimeQualifier?.Trim = "" Then
            G6203 = strTimeQualifier
            G6204 = strTime
        End If
        If strTimeCode?.Trim = "" Then G6205 = strTimeCode
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 5 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "G62")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = G6201
            Case 2
                strRet = G6202
            Case 3
                strRet = G6203
            Case 4
                strRet = G6204
            Case 5
                strRet = G6205
        End Select

        Return strRet
    End Function

    Private _G6201 As String = "" 'max 2 DATE QUALIFIER (02 - Requested Delivery Date,10 - Requested Ship Date/Pick up Date,69 - Scheduled Pickup Date,70 - Scheduled delivery date)
    Public Property G6201() As String
        Get

            Return Left(_G6201, 2)

        End Get
        Set(ByVal value As String)
            _G6201 = value
        End Set
    End Property
    Private _G6202 As String = "" 'max 8 DATE YYYYMMDD
    Public Property G6202() As String
        Get

            Return Left(_G6202, 8)

        End Get
        Set(ByVal value As String)
            _G6202 = value
        End Set
    End Property
    Private _G6203 As String = "" 'max 2 TIME QUALIFIER (U - Scheduled Pick up,X - Scheduled Delivery,Y - Requested Pick up,Z - Requested Delivery)
    Public Property G6203() As String
        Get

            Return Left(_G6203, 2)

        End Get
        Set(ByVal value As String)
            _G6203 = value
        End Set
    End Property
    Private _G6204 As String = "" 'max 8 TIME HHMMSS
    Public Property G6204() As String
        Get

            Return Left(_G6204, 8)

        End Get
        Set(ByVal value As String)
            _G6204 = value
        End Set
    End Property
    Private _G6205 As String = "" 'max 2 TIME CODE LT - Local Time
    Public Property G6205() As String
        Get

            Return Left(_G6205, 2)

        End Get
        Set(ByVal value As String)
            _G6205 = value
        End Set
    End Property

End Class

#End Region

#Region " K1 - REMARKS (Header - Optional) "

Public Class clsEDIK1
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is K1
            Select Case i
                Case 1
                    K101 = sSegs(i)
                Case 2
                    K102 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "K1")
        End If
        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = K101
            Case 2
                strRet = K102
        End Select

        Return strRet
    End Function


    Private _K101 As String = "" 'min/max 1/30 FREE-FORM MESSAGE
    Public Property K101() As String
        Get

            Return Left(_K101, 30)

        End Get
        Set(ByVal value As String)
            _K101 = value
        End Set
    End Property
    Private _K102 As String = "" 'min/max 1/30 FREE-FORM MESSAGE
    Public Property K102() As String
        Get

            Return Left(_K102, 30)

        End Get
        Set(ByVal value As String)
            _K102 = value
        End Set
    End Property

End Class

#End Region

#Region " L0 - LINE ITEM - QUANTITY AND WEIGHT"

Public Class clsEDIL0
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is L0
            Select Case i
                Case 1
                    L001 = sSegs(i)
                Case 2
                    L002 = sSegs(i)
                Case 3
                    L003 = sSegs(i)
                Case 4
                    L004 = sSegs(i)
                Case 5
                    L005 = sSegs(i)
                Case 6
                    L006 = sSegs(i)
                Case 7
                    L007 = sSegs(i)
                Case 8
                    L008 = sSegs(i)
                Case 9
                    L009 = sSegs(i)
                Case 10
                    L010 = sSegs(i)
                Case 11
                    L011 = sSegs(i)
            End Select
        Next
    End Sub


    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="ctSeqNo"></param>
    ''' <param name="strWeight"></param>
    ''' <param name="strLadingQty"></param>
    ''' <param name="strBilledRateAsQty">Optional = ""</param>
    ''' <param name="strBilledRateAsQual">Optional = ""</param>
    ''' <param name="strWeightQual">Optional = G</param>
    ''' <param name="strVolume">Optional = ""</param>
    ''' <param name="strVolumeQual">Optional = ""</param>
    ''' <param name="strPackFormCode">Optional = PLT</param>
    ''' <param name="strDunnageDesc">Optional = ""</param>
    ''' <param name="strWgtUnitCode">Optional = L</param>
    ''' <remarks>
    ''' Added by LVV on 4/14/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub New(ByVal ctSeqNo As String,
                   ByVal strWeight As String,
                   ByVal strLadingQty As String,
                   Optional ByVal strBilledRateAsQty As String = "",
                   Optional ByVal strBilledRateAsQual As String = "",
                   Optional ByVal strWeightQual As String = "G",
                   Optional ByVal strVolume As String = "",
                   Optional ByVal strVolumeQual As String = "",
                   Optional ByVal strPackFormCode As String = "PLT",
                   Optional ByVal strDunnageDesc As String = "",
                   Optional ByVal strWgtUnitCode As String = "L",
                   Optional ByVal blnNoSpaces As Boolean = False)

        MyBase.New()
        L001 = ctSeqNo
        L002 = strBilledRateAsQty
        L003 = strBilledRateAsQual
        L004 = strWeight
        L005 = strWeightQual
        L006 = strVolume
        L007 = strVolumeQual
        L008 = strLadingQty
        L009 = strPackFormCode
        L010 = strDunnageDesc
        L011 = strWgtUnitCode
        NoSpaces = blnNoSpaces

    End Sub


    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Public Sub New(ByVal blnNoSpaces As Boolean)
        MyBase.New()

        NoSpaces = blnNoSpaces

    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _NoSpaces As Boolean = False
    Public Property NoSpaces() As Boolean
        Get
            Return _NoSpaces
        End Get
        Set(ByVal value As Boolean)
            _NoSpaces = value
        End Set
    End Property

    Public Function getEDIString(ByVal sSegTerm As String) As String

        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 15 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "L0")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = L001
            Case 2
                strRet = L002
            Case 3
                strRet = L003
            Case 4
                strRet = L004
            Case 5
                strRet = L005
            Case 6
                strRet = L006
            Case 7
                strRet = L007
            Case 8
                strRet = L008
            Case 9
                strRet = L009
            Case 10
                strRet = L010
            Case 11
                strRet = L011
        End Select

        Return strRet
    End Function


    Private _L001 As String = "" 'min/max 1/3 LADING LINE ITEM NUMBER (Counter)
    Public Property L001() As String
        Get
            Return Left(_L001, 3)
        End Get
        Set(ByVal value As String)
            _L001 = value
        End Set
    End Property

    Private _L002 As String = "" 'min/max 1/11 BILLED/RATED-AS QUANTITY
    Public Property L002() As String
        Get
            Return Left(_L002, 11)
        End Get
        Set(ByVal value As String)
            _L002 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _L003 As String = "" 'min/max 2/2 BILLED/RATED-AS QUALIFIER (Like TN - Tons)
    Public Property L003() As String
        Get
            If NoSpaces Then
                If _L003?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_L003, 2, " ", False), 2)
                Else
                    Return Left(_L003?.Trim(), 2)
                End If
            Else
            Return Left(buildFixWidth(_L003, 2, " ", False), 2)
            End If
        End Get
        Set(ByVal value As String)
            _L003 = value
        End Set
    End Property

    Private _L004 As String = "" 'min/max 1/10 WEIGHT
    Public Property L004() As String
        Get
            Return Left(_L004, 10)
        End Get
        Set(ByVal value As String)
            _L004 = value
        End Set
    End Property

    Private _L005 As String = "" 'min/max 1/2 WEIGHT QUALIFIER (like N - Actual Net Weight or  FR)
    Public Property L005() As String
        Get
            Return Left(_L005, 2)
        End Get
        Set(ByVal value As String)
            _L005 = value
        End Set
    End Property

    Private _L006 As String = "" 'min/max 1/8 VOLUME
    Public Property L006() As String
        Get
            Return Left(_L006, 8)
        End Get
        Set(ByVal value As String)
            _L006 = value
        End Set
    End Property

    Private _L007 As String = "" 'min/max 1/1 VOLUME UNIT QUALIFIER
    Public Property L007() As String
        Get
            Return Left(_L007, 1)
        End Get
        Set(ByVal value As String)
            _L007 = value
        End Set
    End Property

    Private _L008 As String = "" 'min/max 1/7 LADING QUANTITY
    Public Property L008() As String
        Get
            Return Left(_L008, 7)
        End Get
        Set(ByVal value As String)
            _L008 = value
        End Set
    End Property

    Private _L009 As String = "" 'min/max 1/3 PACKAGING FORM CODE (like PLT -- Pallet)
    Public Property L009() As String
        Get
            Return Left(_L009, 3)
        End Get
        Set(ByVal value As String)
            _L009 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _L010 As String = "" 'min/max 2/25 DUNNAGE DESCRIPTION
    Public Property L010() As String
        Get
            If NoSpaces Then
                If _L010?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_L010, 2, " ", False), 25)
                Else
                    Return Left(_L010?.Trim(), 25)
                End If
            Else
            Return Left(buildFixWidth(_L010, 2, " ", False), 25)
            End If
        End Get
        Set(ByVal value As String)
            _L010 = value
        End Set
    End Property

    Private _L011 As String = "L" 'min/max 1/1 WEIGHT UNIT CODE (L - Pounds)
    Public Property L011() As String
        Get
            Return Left(_L011, 1)
        End Get
        Set(ByVal value As String)
            _L011 = value
        End Set
    End Property


End Class

#End Region

#Region " L1 - RATE AND CHARGES"

Public Class clsEDIL1
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is L1
            Select Case i
                Case 1
                    L101 = sSegs(i)
                Case 2
                    L102 = sSegs(i)
                Case 3
                    L103 = sSegs(i)
                Case 4
                    L104 = sSegs(i)
                Case 5
                    L105 = sSegs(i)
                Case 6
                    L106 = sSegs(i)
                Case 7
                    L107 = sSegs(i)
                Case 8
                    L108 = sSegs(i)
                Case 9
                    L109 = sSegs(i)
                Case 10
                    L110 = sSegs(i)
                Case 11
                    L111 = sSegs(i)
                Case 12
                    L112 = sSegs(i)
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="ctSeqNo"></param>
    ''' <param name="strFrtRate"></param>
    ''' <param name="strCharge"></param>
    ''' <param name="EDICode"></param>
    ''' <param name="strRateValQual">Optional = FR</param>
    ''' <param name="strAdvances">Optional = ""</param>
    ''' <param name="strPrePaidAmt">Optional = ""</param>
    ''' <param name="strRatePtCode">Optional = ""</param>
    ''' <remarks>
    ''' Added by LVV on 4/14/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub New(ByVal ctSeqNo As String,
                   ByVal strFrtRate As String,
                   ByVal strCharge As String,
                   ByVal EDICode As String,
                   Optional ByVal strRateValQual As String = "FR",
                   Optional ByVal strAdvances As String = "",
                   Optional ByVal strPrePaidAmt As String = "",
                   Optional ByVal strRatePtCode As String = "")

        MyBase.New()
        L101 = ctSeqNo
        L102 = strFrtRate
        L103 = strRateValQual
        L104 = strCharge
        L105 = strAdvances
        L106 = strPrePaidAmt
        L107 = strRatePtCode
        L108 = EDICode

    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Public Sub New(ByVal blnNoSpaces As Boolean)
        MyBase.New()

        NoSpaces = blnNoSpaces

    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _NoSpaces As Boolean = False
    Public Property NoSpaces() As Boolean
        Get
            Return _NoSpaces
        End Get
        Set(ByVal value As Boolean)
            _NoSpaces = value
        End Set
    End Property

    Public Function getEDIString(ByVal sSegTerm As String) As String

        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 15 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "L1")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = L101
            Case 2
                strRet = L102
            Case 3
                strRet = L103
            Case 4
                strRet = L104
            Case 5
                strRet = L105
            Case 6
                strRet = L106
            Case 7
                strRet = L107
            Case 8
                strRet = L108
            Case 9
                strRet = L109
            Case 10
                strRet = L110
            Case 11
                strRet = L111
            Case 12
                strRet = L112
        End Select

        Return strRet
    End Function


    Private _L101 As String = "" 'min/max 1/3 LADING LINE ITEM NUMBER (Counter)
    Public Property L101() As String
        Get
            Return Left(_L101, 3)
        End Get
        Set(ByVal value As String)
            _L101 = value
        End Set
    End Property

    Private _L102 As String = "" 'min/max 1/9 FREIGHT RATE
    Public Property L102() As String
        Get
            Return Left(_L102, 9)
        End Get
        Set(ByVal value As String)
            _L102 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _L103 As String = "" 'min/max 2/2 RATE/VALUE QUALIFIER (like CT - Charge or Credit Based on Percentage of Total TN - Per Train Rate  CW – Per 100 Wt.)
    Public Property L103() As String
        Get
            If NoSpaces Then
                If _L103?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_L103, 2, " ", False), 2)
                Else
                    Return Left(_L103?.Trim(), 2)
                End If
            Else
            Return Left(buildFixWidth(_L103, 2, " ", False), 2)
            End If
        End Get
        Set(ByVal value As String)
            _L103 = value
        End Set
    End Property

    Private _L104 As String = "" 'min/max 1/12 CHARGE
    Public Property L104() As String
        Get
            Return Left(_L104, 12)
        End Get
        Set(ByVal value As String)
            _L104 = value
        End Set
    End Property

    Private _L105 As String = "" 'min/max 1/9 ADVANCES
    Public Property L105() As String
        Get
            Return Left(_L105, 9)
        End Get
        Set(ByVal value As String)
            _L105 = value
        End Set
    End Property

    Private _L106 As String = "" 'min/max 1/9 PREPAID AMOUNT
    Public Property L106() As String
        Get
            Return Left(_L106, 9)
        End Get
        Set(ByVal value As String)
            _L106 = value
        End Set
    End Property

    Private _L107 As String = "" 'min/max 1/9 RATE COMBINATION POINT CODE
    Public Property L107() As String
        Get
            Return Left(_L107, 9)
        End Get
        Set(ByVal value As String)
            _L107 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _L108 As String = "" 'min/max 3/3 SPECIAL CHARGE OR ALLOWANCE CODE (like FUE - Fuel Charge or 400 – Freight)
    Public Property L108() As String
        Get
            If NoSpaces Then
                If _L108?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_L108, 3, " ", False), 3)
                Else
                    Return Left(_L108?.Trim(), 3)
                End If
            Else
                Return Left(buildFixWidth(_L108, 3, " ", False), 3)
            End If
        End Get
        Set(ByVal value As String)
            _L108 = value
        End Set
    End Property


    '********Merge from 6.0.4.70*************

    Private _L109 As String = "" 'min/max 0/20 User Defined
    ''' <summary>
    ''' min/max 0/20 User Defined
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-6.0.4.107 on 09/12/2017
    ''' </remarks>
    Public Property L109() As String
        Get
            Return Left(_L109.Trim(), 20)
        End Get
        Set(ByVal value As String)
            _L109 = value
        End Set
    End Property

    Private _L110 As String = "" 'min/max 0/20 User Defined
    ''' <summary>
    ''' min/max 0/20 User Defined
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-6.0.4.107 on 09/12/2017
    ''' </remarks>
    Public Property L110() As String
        Get
            Return Left(_L110.Trim(), 20)
        End Get
        Set(ByVal value As String)
            _L110 = value
        End Set
    End Property

    Private _L111 As String = "" 'min/max 0/20 User Defined
    ''' <summary>
    ''' min/max 0/20 User Defined NGL uses 'ON' as a qualifier for the L112 Order Number or 'SN' as a qualifier for stop Number  the default is stop number even when L111 is blank
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-6.0.4.107 on 09/12/2017
    ''' typically used to store the L112 qualifier
    ''' </remarks>
    Public Property L111() As String
        Get
            Return Left(_L111.Trim(), 20)
        End Get
        Set(ByVal value As String)
            _L111 = value
        End Set
    End Property

    Private _L112 As String = "" 'min/max 0/20 User Defined
    ''' <summary>
    ''' min/max 0/20 User Defined for NGL  When L111 = 'ON' this is the  Order Number or when L111 is blank or 'SN' this is the stop Number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-6.0.4.107 on 09/12/2017
    ''' typically used to return the order number or stop number associated with the charge.
    ''' </remarks>
    Public Property L112() As String
        Get
            Return Left(_L112.Trim(), 20)
        End Get
        Set(ByVal value As String)
            _L112 = value
        End Set
    End Property

    '********Merge from 6.0.4.70*************

End Class

#End Region

#Region " L3 - TOTAL WEIGHT AND CHARGES (Detail - Optional)"

Public Class clsEDIL3
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is L3
            Select Case i
                Case 1
                    L301 = sSegs(i)
                Case 2
                    L302 = sSegs(i)
                Case 3
                    L303 = sSegs(i)
                Case 4
                    L304 = sSegs(i)
                Case 5
                    L305 = sSegs(i)
                Case 6
                    L306 = sSegs(i)
                Case 7
                    L307 = sSegs(i)
                Case 8
                    L308 = sSegs(i)
                Case 9
                    L309 = sSegs(i)
                Case 10
                    L310 = sSegs(i)
                Case 11
                    L311 = sSegs(i)
                Case 12
                    L312 = sSegs(i)
                Case 13
                    L313 = sSegs(i)
                Case 14
                    L314 = sSegs(i)
                Case 15
                    L315 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String

        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 15 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "L3")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = L301
            Case 2
                strRet = L302
            Case 3
                strRet = L303
            Case 4
                strRet = L304
            Case 5
                strRet = L305
            Case 6
                strRet = L306
            Case 7
                strRet = L307
            Case 8
                strRet = L308
            Case 9
                strRet = L309
            Case 10
                strRet = L310
            Case 11
                strRet = L311
            Case 12
                strRet = L312
            Case 13
                strRet = L313
            Case 14
                strRet = L314
            Case 15
                strRet = L315
        End Select

        Return strRet
    End Function


    Private _L301 As String = "" 'max 10 WEIGHT
    Public Property L301() As String
        Get

            Return Left(_L301, 10)

        End Get
        Set(ByVal value As String)
            _L301 = value
        End Set
    End Property
    Private _L302 As String = "" 'max 2 WEIGHT QUALIFIER (G - Gross Weight)
    Public Property L302() As String
        Get

            Return Left(_L302, 2)

        End Get
        Set(ByVal value As String)
            _L302 = value
        End Set
    End Property
    Private _L303 As String = "" 'max 9 FREIGHT RATE
    Public Property L303() As String
        Get

            Return Left(_L303, 9)

        End Get
        Set(ByVal value As String)
            _L303 = value
        End Set
    End Property
    Private _L304 As String = "" 'max 2 RATE/VALUE QUALIFIER (CW - Per Hundred Weight)
    Public Property L304() As String
        Get

            Return Left(_L304, 2)

        End Get
        Set(ByVal value As String)
            _L304 = value
        End Set
    End Property
    Private _L305 As String = "" 'max 12 CHARGE
    Public Property L305() As String
        Get

            Return Left(_L305, 12)

        End Get
        Set(ByVal value As String)
            _L305 = value
        End Set
    End Property
    Private _L306 As String = "" 'max 9 ADVANCES
    Public Property L306() As String
        Get

            Return Left(_L306, 9)

        End Get
        Set(ByVal value As String)
            _L306 = value
        End Set
    End Property
    Private _L307 As String = "" 'max 9 PREPAID AMOUNT
    Public Property L307() As String
        Get

            Return Left(_L307, 9)

        End Get
        Set(ByVal value As String)
            _L307 = value
        End Set
    End Property
    Private _L308 As String = "" 'max 3 SPECIAL CHARGE OR ALLOWANCE CODE
    Public Property L308() As String
        Get

            Return Left(_L308, 3)

        End Get
        Set(ByVal value As String)
            _L308 = value
        End Set
    End Property
    Private _L309 As String = "" 'max 8 VOLUME
    Public Property L309() As String
        Get

            Return Left(_L309, 8)

        End Get
        Set(ByVal value As String)
            _L309 = value
        End Set
    End Property
    Private _L310 As String = "" 'max 1 VOLUME UNIT QUALIFIER (E - Cubic Feet)
    Public Property L310() As String
        Get

            Return Left(_L310, 1)

        End Get
        Set(ByVal value As String)
            _L310 = value
        End Set
    End Property
    Private _L311 As String = "" 'max 7 LADING QUANTITY
    Public Property L311() As String
        Get

            Return Left(_L311, 7)

        End Get
        Set(ByVal value As String)
            _L311 = value
        End Set
    End Property
    Private _L312 As String = "" 'max 1 WEIGHT UNIT CODE (L - Pounds)
    Public Property L312() As String
        Get

            Return Left(_L312, 1)

        End Get
        Set(ByVal value As String)
            _L312 = value
        End Set
    End Property
    Private _L313 As String = "" 'max 7 TARIFF NUMBER
    Public Property L313() As String
        Get

            Return Left(_L313, 7)

        End Get
        Set(ByVal value As String)
            _L313 = value
        End Set
    End Property
    Private _L314 As String = "" 'max 12 DECLARED VALUE
    Public Property L314() As String
        Get

            Return Left(_L314, 12)

        End Get
        Set(ByVal value As String)
            _L314 = value
        End Set
    End Property
    Private _L315 As String = "" 'max 2 RATE/VALUE QUALIFIER
    Public Property L315() As String
        Get

            Return Left(_L315, 2)

        End Get
        Set(ByVal value As String)
            _L315 = value
        End Set
    End Property

End Class

#End Region

#Region " L5 - TOTAL WEIGHT AND CHARGES"

Public Class clsEDIL5

    Public Sub New()
        MyBase.New()
    End Sub

    'Modified by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
    'Added L505, L506, and L507
    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is L5
            Select Case i
                Case 1
                    L501 = sSegs(i)
                Case 2
                    L502 = sSegs(i)
                Case 3
                    L503 = sSegs(i)
                Case 4
                    L504 = sSegs(i)
                Case 5
                    L505 = sSegs(i)
                Case 6
                    L506 = sSegs(i)
                Case 7
                    L507 = sSegs(i)
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="strseqNo"></param>
    ''' <param name="strDesc"></param>
    ''' <param name="strEDICode"></param>
    ''' <remarks>
    ''' Added by LVV on 4/13/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub New(ByVal strseqNo As String, ByVal strDesc As String, ByVal strEDICode As String)
        MyBase.New()
        L501 = strseqNo
        L502 = strDesc
        L503 = strEDICode
    End Sub

    'Modified by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
    'Added L505, L506, and L507 (Changed For counter from 4 to 7
    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 7 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "L5")
        End If

        Return sEdi.ToString
    End Function

    'Modified by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
    'Added L505, L506, and L507
    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = L501
            Case 2
                strRet = L502
            Case 3
                strRet = L503
            Case 4
                strRet = L504
            Case 5
                strRet = L505
            Case 6
                strRet = L506
            Case 7
                strRet = L507
        End Select

        Return strRet
    End Function


    Private _L501 As String = "" 'min/max 1/6 LADING LINE ITEM NUMBER (Counter)
    Public Property L501() As String
        Get

            Return Left(_L501, 6)

        End Get
        Set(ByVal value As String)
            _L501 = value
        End Set
    End Property
    Private _L502 As String = "" 'min/max 1/50 LADING DESCRIPTION (Example:  Fuel Surcharge)
    Public Property L502() As String
        Get

            Return Left(_L502, 50)

        End Get
        Set(ByVal value As String)
            _L502 = value
        End Set
    End Property

    Private _L503 As String = "" 'min/max 1/30 COMMODITY CODE (Like FUE )
    Public Property L503() As String
        Get
            Return Left(_L503, 30)
        End Get
        Set(ByVal value As String)
            _L503 = value
        End Set
    End Property

    Private _L504 As String = "" 'min/max 1/1 COMMODITY CODE QUALIFIER (N – National Motor Freight Classification or T – Standard Transportation Commodity Cd)
    Public Property L504() As String
        Get
            Return Left(_L504, 1)
        End Get
        Set(ByVal value As String)
            _L504 = value
        End Set
    End Property

    'Added by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
    Private _L505 As String = "" 'min/max 1/50 SKU Item Number (Example:  Inventory Item Number)
    Public Property L505() As String
        Get

            Return Left(_L505, 50)

        End Get
        Set(ByVal value As String)
            _L505 = value
        End Set
    End Property

    'Added by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
    Private _L506 As String = "" 'min/max 1/12 SKU Cost (Example:  Item Cost $ amount for Insurance last two numbers are after the decimal place)
    Public Property L506() As String
        Get

            Return Left(_L506, 12)

        End Get
        Set(ByVal value As String)
            _L506 = value
        End Set
    End Property

    'Added by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
    Private _L507 As String = "" 'min/max 1/50 SKU Lot Number (Example:  Item Lot Number)
    Public Property L507() As String
        Get

            Return Left(_L507, 50)

        End Get
        Set(ByVal value As String)
            _L507 = value
        End Set
    End Property

End Class

#End Region

#Region " L11 - REFERENCE NUMBER (Header - Mandatory)"

Public Class clsEDIL11

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is L11
            Select Case i
                Case 1
                    L1101 = sSegs(i)
                Case 2
                    L1102 = sSegs(i)
                Case 3
                    L1103 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 3 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "L11")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = L1101
            Case 2
                strRet = L1102
            Case 3
                strRet = L1103
        End Select

        Return strRet
    End Function


    Private _L1101 As String = "" 'min/max 1/30 REFERENCE IDENTIFICATION
    Public Property L1101() As String
        Get

            Return Left(_L1101, 30)

        End Get
        Set(ByVal value As String)
            _L1101 = value
        End Set
    End Property
    Private _L1102 As String = "" 'min/max 2/3 REFERENCE IDENTIFICATION QUALIFIER (PO-Purchase Order Number,SI-Shippers Number [SID],MB-Master Bill of Lading,19-Division Identifier,4C-Shipment Destination Code,BM-Bill of Lading Number,CO-Customer Order Number,EU-End user purchase order number,KK-Order Type,ON-Shipping customer Order number)
    Public Property L1102() As String
        Get

            Return Left(buildFixWidth(_L1102, 2, " ", False), 3)

        End Get
        Set(ByVal value As String)
            _L1102 = value
        End Set
    End Property
    Private _L1103 As String = "" 'min/max 1/80 DESCRIPTION (FS-?,CUS - Customer Sales Order,PUR - Inbound Purchase Product,RES - Interplant Re-supply)
    Public Property L1103() As String
        Get

            Return Left(_L1103, 80)

        End Get
        Set(ByVal value As String)
            _L1103 = value
        End Set
    End Property

End Class

#End Region

#Region " LX - ASSIGNED NUMBER (Header - Optional)"

Public Class clsEDILX

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is LX
            Select Case i
                Case 1
                    LX01 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 3 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "LX")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = LX01
        End Select

        Return strRet
    End Function


    Private _LX01 As String = "" 'min/max 1/6 ASSIGNED NUMBER  Sequentially Assigned Number Stop Number or sequentially assigned
    Public Property LX01() As String
        Get

            Return Left(_LX01, 6)

        End Get
        Set(ByVal value As String)
            _LX01 = value
        End Set
    End Property

End Class

#End Region

#Region " MS1 - EQUIPMENT (Header - Mandatory) "

Public Class clsEDIMS1
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is MS1
            Select Case i
                Case 1
                    MS101 = sSegs(i)
                Case 2
                    MS102 = sSegs(i)
                Case 3
                    MS103 = sSegs(i)
                Case 4
                    MS104 = sSegs(i)
                Case 5
                    MS105 = sSegs(i)
                Case 6
                    MS106 = sSegs(i)
                Case 7
                    MS107 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 7 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "MS1")
        End If
        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = MS101
            Case 2
                strRet = MS102
            Case 3
                strRet = MS103
            Case 4
                strRet = MS104
            Case 5
                strRet = MS105
            Case 6
                strRet = MS106
            Case 7
                strRet = MS107
        End Select

        Return strRet
    End Function


    Private _MS101 As String = "" 'min/max 2/30 CITY NAME (Required if value in AT701 is X6)
    Public Property MS101() As String
        Get

            Return Left(buildFixWidth(_MS101, 2, " ", False), 30)

        End Get
        Set(ByVal value As String)
            _MS101 = value
        End Set
    End Property
    Private _MS102 As String = "" 'min/max 2/2 STATE OR PROVINCE CODE (Required if value in AT701 is X6)
    Public Property MS102() As String
        Get

            Return Left(buildFixWidth(_MS102, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _MS102 = value
        End Set
    End Property
    Private _MS103 As String = "" 'min/max 2/3 COUNTRY CODE (Required if value in AT701 is X6)
    Public Property MS103() As String
        Get

            Return Left(buildFixWidth(_MS103, 2, " ", False), 3)

        End Get
        Set(ByVal value As String)
            _MS103 = value
        End Set
    End Property
    Private _MS104 As String = "" 'min/max 7/7 LONGITUDE CODE
    Public Property MS104() As String
        Get

            Return Left(buildFixWidth(_MS104, 7, " ", False), 7)

        End Get
        Set(ByVal value As String)
            _MS104 = value
        End Set
    End Property
    Private _MS105 As String = "" 'min/max 7/7 LATITUDE CODE
    Public Property MS105() As String
        Get

            Return Left(buildFixWidth(_MS105, 7, " ", False), 7)

        End Get
        Set(ByVal value As String)
            _MS105 = value
        End Set
    End Property
    Private _MS106 As String = "" 'min/max 1/1 DIRECTION IDENTIFIER CODE
    Public Property MS106() As String
        Get

            Return Left(_MS106, 1)

        End Get
        Set(ByVal value As String)
            _MS106 = value
        End Set
    End Property
    Private _MS107 As String = "" 'min/max 1/1 DIRECTION IDENTIFIER CODE                 
    Public Property MS107() As String
        Get

            Return Left(_MS107, 2)

        End Get
        Set(ByVal value As String)
            _MS107 = value
        End Set
    End Property
End Class

#End Region

#Region " MS2 - EQUIPMENT OR CONTAINER OWNER AND TYPE (Header - Mandatory) "

Public Class clsEDIMS2
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is MS2
            Select Case i
                Case 1
                    MS201 = sSegs(i)
                Case 2
                    MS202 = sSegs(i)
                Case 3
                    MS203 = sSegs(i)
                Case 4
                    MS204 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 4 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "MS2")
        End If
        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = MS201
            Case 2
                strRet = MS202
            Case 3
                strRet = MS203
            Case 4
                strRet = MS204
        End Select

        Return strRet
    End Function


    Private _MS201 As String = "" 'min/max 2/4 STANDARD CARRIER ALPHA CODE
    Public Property MS201() As String
        Get

            Return Left(buildFixWidth(_MS201, 2, " ", False), 4)

        End Get
        Set(ByVal value As String)
            _MS201 = value
        End Set
    End Property
    Private _MS202 As String = "" 'min/max 2/10 EQUIPMENT NUMBER
    Public Property MS202() As String
        Get

            Return Left(buildFixWidth(_MS202, 2, " ", False), 10)

        End Get
        Set(ByVal value As String)
            _MS202 = value
        End Set
    End Property
    Private _MS203 As String = "" 'min/max 2/2 EQUIPMENT DESCRIPTION CODE FT – Flatbed,TV – Truck, Van,TW – Reefer
    Public Property MS203() As String
        Get

            Return Left(buildFixWidth(_MS203, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _MS203 = value
        End Set
    End Property
    Private _MS204 As String = "" 'min/max 1/1 EQUIPMENT NUMBER CHECK DIGIT
    Public Property MS204() As String
        Get

            Return Left(_MS204, 1)

        End Get
        Set(ByVal value As String)
            _MS204 = value
        End Set
    End Property
End Class

#End Region

#Region " N1 – NAME (Header - Mandatory)"

Public Class clsEDIN1
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is N1
            Select Case i
                Case 1
                    N101 = sSegs(i)
                Case 2
                    N102 = sSegs(i)
                Case 3
                    N103 = sSegs(i)
                Case 4
                    N104 = sSegs(i)
                Case 5
                    N105 = sSegs(i)
                Case 6
                    N106 = sSegs(i)
            End Select
        Next
    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Public Sub New(ByVal blnNoSpaces As Boolean)
        MyBase.New()

        NoSpaces = blnNoSpaces

    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _NoSpaces As Boolean = False
    Public Property NoSpaces() As Boolean
        Get
            Return _NoSpaces
        End Get
        Set(ByVal value As Boolean)
            _NoSpaces = value
        End Set
    End Property

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 6 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "N1")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = N101
            Case 2
                strRet = N102
            Case 3
                strRet = N103
            Case 4
                strRet = N104
            Case 5
                strRet = N105
            Case 6
                strRet = N106
        End Select

        Return strRet
    End Function

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _N101 As String = "" 'min/max 2/3 ENTITY IDENTIFIER CODE (SF - Ship From, ST - Ship To, PF - Party To Receive Freight Bill)
    Public Property N101() As String
        Get
            If NoSpaces Then
                If _N101?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_N101, 2, " ", False), 3)
                Else
                    Return Left(_N101?.Trim(), 3)
                End If
            Else
                Return Left(buildFixWidth(_N101, 2, " ", False), 3)
            End If
        End Get
        Set(ByVal value As String)
            _N101 = value
        End Set
    End Property
    Private _N102 As String = "" 'min/max 1/60 NAME 
    Public Property N102() As String
        Get

            Return Left(_N102, 60)

        End Get
        Set(ByVal value As String)
            _N102 = value
        End Set
    End Property
    Private _N103 As String = "ZZ" 'min/max 1/2 IDENTIFICATION CODE QUALIFIER Like ZZ
    Public Property N103() As String
        Get

            Return Left(_N103, 2)

        End Get
        Set(ByVal value As String)
            _N103 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _N104 As String = "" 'min/max 2/80 IDENTIFICATION CODE Ship From - Ship To number as supplied  Like Company Number
    Public Property N104() As String
        Get
            If NoSpaces Then
                If _N104?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_N104, 2, "0", True), 80)
                Else
                    Return Left(_N104?.Trim(), 80)
                End If
            Else
                Return Left(buildFixWidth(_N104, 2, "0", True), 80)
            End If
        End Get
        Set(ByVal value As String)
            _N104 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _N105 As String = "" 'min/max 2/2 ENTITY RELATIONSHIP CODE
    Public Property N105() As String
        Get
            If NoSpaces Then
                If _N105?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_N105, 2, " ", False), 2)
                Else
                    Return Left(_N105?.Trim(), 2)
                End If
            Else
                Return Left(buildFixWidth(_N105, 2, " ", False), 2)
            End If
        End Get
        Set(ByVal value As String)
            _N105 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _N106 As String = "" 'min/max 2/3 ENTITY IDENTIFIER CODE
    Public Property N106() As String
        Get
            If NoSpaces Then
                If _N106?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_N106, 2, "0", True), 3)
                Else
                    Return Left(_N106?.Trim(), 3)
                End If
            Else
                Return Left(buildFixWidth(_N106, 2, "0", True), 3)
            End If
        End Get
        Set(ByVal value As String)
            _N106 = value
        End Set
    End Property
End Class

#End Region

#Region " N2 - ADDITIONAL NAME INFORMATION  (Header - Optional)    "

Public Class clsEDIN2
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is N2
            Select Case i
                Case 1
                    N201 = sSegs(i)
                Case 2
                    N202 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "N2")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = N201
            Case 2
                strRet = N202
        End Select

        Return strRet
    End Function


    Private _N201 As String = "" 'min/max 1/60 NAME INFORMATION
    Public Property N201() As String
        Get

            Return Left(_N201, 60)

        End Get
        Set(ByVal value As String)
            _N201 = value
        End Set
    End Property
    Private _N202 As String = "" 'min/max 1/60 NAME INFORMATION
    Public Property N202() As String
        Get

            Return Left(_N202, 60)

        End Get
        Set(ByVal value As String)
            _N202 = value
        End Set
    End Property

End Class

#End Region

#Region " N3 - ADDRESS INFORMATION (Header - Mandatory) "

Public Class clsEDIN3
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is N3
            Select Case i
                Case 1
                    N301 = sSegs(i)
                Case 2
                    N302 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "N3")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = N301
            Case 2
                strRet = N302
        End Select

        Return strRet
    End Function


    Private _N301 As String = "" 'min/max 1/55 ADDRESS INFORMATION
    Public Property N301() As String
        Get

            Return Left(_N301, 55)

        End Get
        Set(ByVal value As String)
            _N301 = value
        End Set
    End Property
    Private _N302 As String = "" 'min/max 1/55 ADDRESS INFORMATION
    Public Property N302() As String
        Get

            Return Left(_N302, 55)

        End Get
        Set(ByVal value As String)
            _N302 = value
        End Set
    End Property

End Class

#End Region

#Region " N4 - GEOGRAPHIC LOCATION (Header - Mandatory) "

Public Class clsEDIN4
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is N4
            Select Case i
                Case 1
                    N401 = sSegs(i)
                Case 2
                    N402 = sSegs(i)
                Case 3
                    N403 = sSegs(i)
                Case 4
                    N404 = sSegs(i)
                Case 5
                    N405 = sSegs(i)
                Case 6
                    N406 = sSegs(i)
            End Select
        Next
    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Public Sub New(ByVal blnNoSpaces As Boolean)
        MyBase.New()

        NoSpaces = blnNoSpaces

    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _NoSpaces As Boolean = False
    Public Property NoSpaces() As Boolean
        Get
            Return _NoSpaces
        End Get
        Set(ByVal value As Boolean)
            _NoSpaces = value
        End Set
    End Property

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 6 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "N4")
        End If
        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = N401
            Case 2
                strRet = N402
            Case 3
                strRet = N403
            Case 4
                strRet = N404
            Case 5
                strRet = N405
            Case 6
                strRet = N406
        End Select

        Return strRet
    End Function

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _N401 As String = "" 'min/max 2/30 CITY NAME
    Public Property N401() As String
        Get
            If NoSpaces Then
                If _N401?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_N401, 2, " ", False), 30)
                Else
                    Return Left(_N401?.Trim(), 30)
                End If
            Else
                Return Left(buildFixWidth(_N401, 2, " ", False), 30)
            End If
        End Get
        Set(ByVal value As String)
            _N401 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _N402 As String = "" 'min/max 2/2 STATE OR PROVINCE CODE
    Public Property N402() As String
        Get
            If NoSpaces Then
                If _N402?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_N402, 2, " ", False), 2)
                Else
                    Return Left(_N402?.Trim(), 2)
                End If
            Else
                Return Left(buildFixWidth(_N402, 2, " ", False), 2)
            End If
        End Get
        Set(ByVal value As String)
            _N402 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _N403 As String = "" 'min/max 3/15 POSTAL CODE
    Public Property N403() As String
        Get
            If NoSpaces Then
                If _N403?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_N403, 3, " ", False), 15)
                Else
                    Return Left(_N403?.Trim(), 15)
                End If
            Else
                Return Left(buildFixWidth(_N403, 3, " ", False), 15)
            End If
        End Get
        Set(ByVal value As String)
            _N403 = value
        End Set
    End Property

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _N404 As String = "" 'min/max 2/3 COUNTRY CODE
    Public Property N404() As String
        Get
            If NoSpaces Then
                If _N404?.Trim().Length > 0 Then
                    Return Left(buildFixWidth(_N404, 2, " ", False), 3)
                Else
                    Return Left(_N404?.Trim(), 3)
                End If
            Else
                Return Left(buildFixWidth(_N404, 2, " ", False), 3)
            End If
        End Get
        Set(ByVal value As String)
            _N404 = value
        End Set
    End Property
    Private _N405 As String = "" 'min/max 1/2 LOCATION QUALIFIER (DL - Delivery Location,OR - Origin)
    Public Property N405() As String
        Get

            Return Left(_N405, 2)

        End Get
        Set(ByVal value As String)
            _N405 = value
        End Set
    End Property
    Private _N406 As String = "" 'min/max 1/30 LOCATION IDENTIFIER
    Public Property N406() As String
        Get

            Return Left(_N406, 30)

        End Get
        Set(ByVal value As String)
            _N406 = value
        End Set
    End Property
End Class

#End Region

#Region " N7 - Equipment Details (Header - Optional) "

Public Class clsEDIN7
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is N7
            Select Case i
                Case 1
                    N701 = sSegs(i)
                Case 2
                    N702 = sSegs(i)
                Case 3
                    N703 = sSegs(i)
                Case 4
                    N704 = sSegs(i)
                Case 5
                    N705 = sSegs(i)
                Case 6
                    N706 = sSegs(i)
                Case 7
                    N707 = sSegs(i)
                Case 8
                    N708 = sSegs(i)
                Case 9
                    N709 = sSegs(i)
                Case 10
                    N710 = sSegs(i)
                Case 11
                    N711 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 11 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "N7")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = N701
            Case 2
                strRet = N702
            Case 3
                strRet = N703
            Case 4
                strRet = N704
            Case 5
                strRet = N705
            Case 6
                strRet = N706
            Case 7
                strRet = N707
            Case 8
                strRet = N708
            Case 9
                strRet = N709
            Case 10
                strRet = N710
            Case 11
                strRet = N711
        End Select

        Return strRet
    End Function


    Private _N701 As String = "" 'min/max 1/4 Equipment Initial
    Public Property N701() As String
        Get

            Return Left(_N701, 4)

        End Get
        Set(ByVal value As String)
            _N701 = value
        End Set
    End Property
    Private _N702 As String = "" 'min/max 1/10 Equipment Number
    Public Property N702() As String
        Get

            Return Left(_N702, 10)

        End Get
        Set(ByVal value As String)
            _N702 = value
        End Set
    End Property
    Private _N703 As String = "" 'min/max 1/10 Weight
    Public Property N703() As String
        Get

            Return Left(_N703, 10)

        End Get
        Set(ByVal value As String)
            _N703 = value
        End Set
    End Property
    Private _N704 As String = "L" 'min/max 1/2 Weight Qualifier
    Public Property N704() As String
        Get
            If Len(Trim(N703)) > 0 Then
                Return Left(_N704, 2)
            Else
                Return ""
            End If

        End Get
        Set(ByVal value As String)
            _N704 = value
        End Set
    End Property
    Private _N705 As String = "" 'min/max 3/8 Tare Weight
    Public Property N705() As String
        Get
            'Modified by RHR v-7.0.5.102 on 10/25/2016.  If N705 is required N706 is required
            ' so we always send an empty string now
            'Return Left(buildFixWidth(_N705, 3, " ", False), 8)
            Return Left(_N705, 8)

        End Get
        Set(ByVal value As String)
            _N705 = value
        End Set
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.3.0.002 on 10/16/2020
    '''   N706 is dependent upon N705,  so an empty string can cause an error in some systems
    '''   when N705 is empty n706 must also be empty
    '''   The minimun length of 2 has been removed,  we do not fill this with spaces 
    ''' </remarks>
    Private _N706 As String = "" 'min/max 0/6 Weight Allowance
    Public Property N706() As String
        Get

            'Return Left(buildFixWidth(_N706, 2, " ", False), 6)
            Return Left(_N706, 6)

        End Get
        Set(ByVal value As String)
            _N706 = value
        End Set
    End Property
    Private _N707 As String = "" 'min/max 1/6 Dunnage
    Public Property N707() As String
        Get

            Return Left(_N707, 6)

        End Get
        Set(ByVal value As String)
            _N707 = value
        End Set
    End Property
    Private _N708 As String = "" 'min/max 1/8 Volume
    Public Property N708() As String
        Get

            Return Left(_N708, 8)

        End Get
        Set(ByVal value As String)
            _N708 = value
        End Set
    End Property
    Private _N709 As String = "E" 'min/max 1/1 Volume Unit Qualifier
    Public Property N709() As String
        Get
            If Len(Trim(N708)) > 0 Then
                Return Left(_N709, 2)
            Else
                Return ""
            End If

        End Get
        Set(ByVal value As String)
            _N709 = value
        End Set
    End Property
    Private _N710 As String = "" 'min/max 1/1 Ownership Code
    Public Property N710() As String
        Get

            Return Left(_N710, 1)

        End Get
        Set(ByVal value As String)
            _N710 = value
        End Set
    End Property
    Private _N711 As String = "" 'min/max 2/2 Equipment Description Code (FT - Flat Bed Trailer,CV – Van,RT – Reefer)
    Public Property N711() As String
        Get

            Return Left(buildFixWidth(_N711, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _N711 = value
        End Set
    End Property
End Class

#End Region

#Region " N9 - REFERENCE IDENTIFICATION "
'NOTE: 210 messages may have multiple N9 records outside of any defined loop
'N9 records are used to idenitfy keys values like PO - Purchase Order BM - Master Bill Of Lading BT - Bill To
Public Class clsEDIN9
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is N9
            Select Case i
                Case 1
                    N901 = sSegs(i)
                Case 2
                    N902 = sSegs(i)
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="refQual"></param>
    ''' <param name="refNo"></param>
    ''' <remarks>
    ''' Added by LVV on 4/13/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub New(ByVal refQual As String, ByVal refNo As String)
        MyBase.New()
        N901 = refQual
        N902 = refNo
    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Public Sub New(ByVal blnNoSpaces As Boolean)
        MyBase.New()

        NoSpaces = blnNoSpaces

    End Sub

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _NoSpaces As Boolean = False
    Public Property NoSpaces() As Boolean
        Get
            Return _NoSpaces
        End Get
        Set(ByVal value As Boolean)
            _NoSpaces = value
        End Set
    End Property

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "N9")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = N901
            Case 2
                strRet = N902
        End Select

        Return strRet
    End Function

    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _N901 As String = "" 'min/max 2/3 REFERENCE IDENTIFICATION QUALIFIER (PO - Purchase Order, BM - Master Bill Of Lading, BT - Bill To,CN - Carrier's Reference Number (Pro/Invoice) etc...)
    Public Property N901() As String
        Get
            If NoSpaces Then
                Return Left(_N901?.Trim(), 3)
            Else
            Return Left(buildFixWidth(_N901, 2, " ", False), 3)
            End If
        End Get
        Set(ByVal value As String)
            _N901 = value
        End Set
    End Property
    Private _N902 As String = "" 'min/max 1/30 REFERENCE IDENTIFICATION like the Order number or PO Number
    Public Property N902() As String
        Get
            Return Left(_N902, 30)
        End Get
        Set(ByVal value As String)
            _N902 = value
        End Set
    End Property

End Class

#End Region

#Region " NTE - NOTE/SPECIAL INSTRUCTION (Header - Mandatory)"

Public Class clsEDINTE
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is NTE
            Select Case i
                Case 1
                    NTE01 = sSegs(i)
                Case 2
                    NTE02 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "NTE")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = NTE01
            Case 2
                strRet = NTE02
        End Select

        Return strRet
    End Function


    Private _NTE01 As String = "" 'min/max 3/3 NOTE REFERENCE CODE (DEL-delivery,LOI-Loading,EED-Equipment Description)
    Public Property NTE01() As String
        Get

            Return Left(buildFixWidth(_NTE01, 3, " ", False), 3)

        End Get
        Set(ByVal value As String)
            _NTE01 = value
        End Set
    End Property
    Private _NTE02 As String = "" 'min/max 1/80 DESCRIPTION  (COMMENTS FLT, VAN, BLK REF if equipment)
    Public Property NTE02() As String
        Get

            Return Left(_NTE02, 80)

        End Get
        Set(ByVal value As String)
            _NTE02 = value
        End Set
    End Property

End Class

#End Region

#Region " PLD - PALLET INFORMATION (Header - Optional)"

Public Class clsEDIPLD
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is PLD
            Select Case i
                Case 1
                    PLD01 = sSegs(i)
                Case 2
                    PLD02 = sSegs(i)
                Case 3
                    PLD03 = sSegs(i)
                Case 4
                    PLD04 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 4 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "PLD")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = PLD01
            Case 2
                strRet = PLD02
            Case 3
                strRet = PLD03
            Case 4
                strRet = PLD04
        End Select

        Return strRet
    End Function


    Private _PLD01 As String = "" 'min/max 3/3 QUANTITY Number of pallets
    Public Property PLD01() As String
        Get

            Return Left(buildFixWidth(_PLD01, 3, "0", True), 3)

        End Get
        Set(ByVal value As String)
            _PLD01 = value
        End Set
    End Property
    Private _PLD02 As String = "" 'min/max 1/1 PALLET EXCHANGE CODE
    Public Property PLD02() As String
        Get

            Return Left(_PLD02, 1)

        End Get
        Set(ByVal value As String)
            _PLD02 = value
        End Set
    End Property
    Private _PLD03 As String = "" 'min/max 1/1 WEIGHT UNIT CODE
    Public Property PLD03() As String
        Get

            Return Left(_PLD03, 1)

        End Get
        Set(ByVal value As String)
            _PLD03 = value
        End Set
    End Property
    Private _PLD04 As String = "" 'min/max 1/10 WEIGHT
    Public Property PLD04() As String
        Get

            Return Left(_PLD04, 10)

        End Get
        Set(ByVal value As String)
            _PLD04 = value
        End Set
    End Property
End Class

#End Region

#Region " S5 - - STOP OFF DETAILS (Detail - Mandatory"

Public Class clsEDIS5
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is S5
            Select Case i
                Case 1
                    S501 = sSegs(i)
                Case 2
                    S502 = sSegs(i)
                Case 3
                    S503 = sSegs(i)
                Case 4
                    S504 = sSegs(i)
                Case 5
                    S505 = sSegs(i)
                Case 6
                    S506 = sSegs(i)
                Case 7
                    S507 = sSegs(i)
                Case 8
                    S508 = sSegs(i)
                Case 9
                    S509 = sSegs(i)
                Case 10
                    S510 = sSegs(i)
                Case 11
                    S511 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 11 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "S5")
        End If
        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = S501
            Case 2
                strRet = S502
            Case 3
                strRet = S503
            Case 4
                strRet = S504
            Case 5
                strRet = S505
            Case 6
                strRet = S506
            Case 7
                strRet = S507
            Case 8
                strRet = S508
            Case 9
                strRet = S509
            Case 10
                strRet = S510
            Case 11
                strRet = S511
        End Select

        Return strRet
    End Function


    Private _S501 As String = "" 'min/max 1/3 STOP SEQUENCE NUMBER
    Public Property S501() As String
        Get

            Return Left(_S501, 3)

        End Get
        Set(ByVal value As String)
            _S501 = value
        End Set
    End Property
    Private _S502 As String = "" 'min/max 2/2 STOP REASON CODE (CL - Complete Load,CU - Complete Unload,LD - Load,PL - Partial Load,PU - Partial Unload,UL - Unload)
    Public Property S502() As String
        Get

            Return Left(buildFixWidth(_S502, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _S502 = value
        End Set
    End Property
    Private _S503 As String = "" 'min/max 1/10 Weight
    Public Property S503() As String
        Get

            Return Left(_S503, 10)

        End Get
        Set(ByVal value As String)
            _S503 = value
        End Set
    End Property
    Private _S504 As String = "" 'min/max 1/1 WEIGHT UNIT CODE (L - Pounds)
    Public Property S504() As String
        Get

            Return Left(_S504, 1)

        End Get
        Set(ByVal value As String)
            _S504 = value
        End Set
    End Property
    Private _S505 As String = "" 'min/max 1/10 NUMBER OF UNITS SHIPPED
    Public Property S505() As String
        Get

            Return Left(_S505, 10)

        End Get
        Set(ByVal value As String)
            _S505 = value
        End Set
    End Property
    Private _S506 As String = "" 'min/max 2/2 UNIT OR BASIS FOR MEASUREMENT CODE (CA - Case,1N - Count)
    Public Property S506() As String
        Get

            Return Left(buildFixWidth(_S506, 2, " ", False), 2)

        End Get
        Set(ByVal value As String)
            _S506 = value
        End Set
    End Property
    Private _S507 As String = "" 'min/max 1/8 VOLUME
    Public Property S507() As String
        Get

            Return Left(_S507, 8)

        End Get
        Set(ByVal value As String)
            _S507 = value
        End Set
    End Property
    Private _S508 As String = "" 'min/max 1/1 VOLUME UNIT QUALIFIER (E - Cubic Feet)
    Public Property S508() As String
        Get

            Return Left(_S508, 1)

        End Get
        Set(ByVal value As String)
            _S508 = value
        End Set
    End Property
    Private _S509 As String = "" 'min/max 1/80 DESCRIPTION
    Public Property S509() As String
        Get

            Return Left(_S509, 80)

        End Get
        Set(ByVal value As String)
            _S509 = value
        End Set
    End Property
    Private _S510 As String = "" 'min/max 6/9 STANDARD POINT LOCATION CODE
    Public Property S510() As String
        Get

            Return Left(buildFixWidth(_S510, 6, " ", False), 9)

        End Get
        Set(ByVal value As String)
            _S510 = value
        End Set
    End Property
    Private _S511 As String = "" 'min/max 1/1 ACCOMPLISH CODE
    Public Property S511() As String
        Get

            Return Left(_S511, 1)

        End Get
        Set(ByVal value As String)
            _S511 = value
        End Set
    End Property
End Class

#End Region

#Region " OID - Order Identification Detail (Optional)"

''' <summary>
''' Order Identification Detail (Optional)
''' </summary>
''' <remarks>
''' Created by RHR for v-7.0.6.105 on 5/5/2017
''' </remarks>
Public Class clsEDIOID
    Public Sub New()
        MyBase.New()
        populateElements()

    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        populateElements()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is OID
            Select Case i
                Case 1
                    OID01 = sSegs(i)
                Case 2
                    OID02 = sSegs(i)
                Case 3
                    OID03 = sSegs(i)
                Case 4
                    OID04 = sSegs(i)
                Case 5
                    OID05 = sSegs(i)
                Case 6
                    OID06 = sSegs(i)
                Case 7
                    OID07 = sSegs(i)
                Case 8
                    OID08 = sSegs(i)
                Case 9
                    OID09 = sSegs(i)
                Case Else
                    Exit For
            End Select
        Next
    End Sub

    Private Sub populateElements()
        Dim sElem As String() = New String(8) {"Seller's Order Identification Nunber", "Purchase Order Number", "Purchase Order Qualifier", "Unit or Basis for Measurement Code", "Quantity", "Weight Unit Code", "Weight", "Volume Unit Qualifier", "Volume"}
        Me.Elements = sElem
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 9 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "OID")
        End If
        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = OID01
            Case 2
                strRet = OID02
            Case 3
                strRet = OID03
            Case 4
                strRet = OID04
            Case 5
                strRet = OID05
            Case 6
                strRet = OID06
            Case 7
                strRet = OID07
            Case 8
                strRet = OID08
            Case 9
                strRet = OID09
        End Select

        Return strRet
    End Function


    Public Function getEDIText() As String
        Dim sEdi As New System.Text.StringBuilder("", 500)
        sEdi.Append(Me.Description)
        sEdi.Append(vbCrLf)
        For i As Integer = 1 To 9 Step -1
            Dim strVal As String = getDataByIndex(i)
            If Not String.IsNullOrEmpty(strVal) Then
                sEdi.Append(Elements(i))
                sEdi.Append(vbCrLf)
                sEdi.Append(strVal)
                sEdi.Append(vbCrLf)
            End If
        Next
        Return sEdi.ToString
    End Function

    Private _Description As String = "Order Identification Detail"
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Private _Elements As String()
    Public Property Elements() As String()
        Get
            Return _Elements
        End Get
        Set(ByVal value As String())
            _Elements = value
        End Set
    End Property


    Private _OID01 As String = ""
    ''' <summary>
    ''' min/max 1/30 Seller's Order or Item Identification Nunber - Typically Item number when inclued with item detail 300 loop
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property OID01() As String
        Get

            Return Left(_OID01, 30)

        End Get
        Set(ByVal value As String)
            _OID01 = value
        End Set
    End Property

    Private _OID02 As String = ""
    ''' <summary>
    ''' min/max 1/22 Purchase Order Number- Identifying number for Purchase Order assigned by the orderer/purchaser
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property OID02() As String
        Get

            Return Left(_OID02, 22)

        End Get
        Set(ByVal value As String)
            _OID02 = value
        End Set
    End Property

    Private _OID03 As String = ""
    ''' <summary>
    ''' min/max 1/30 Purchase Order Qualifier - Reference information as defined for a particular Transaction Set or 
    ''' as specified by the Reference Identification Qualifier
    ''' Like PO for Customer's Purchase Order Number or SO for Shippers Sales Order Number
    ''' Required if OID02 is provided
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property OID03() As String
        Get

            Return Left(_OID03, 30)

        End Get
        Set(ByVal value As String)
            _OID03 = value
        End Set
    End Property

    Private _OID04 As String = ""
    ''' <summary>
    ''' min/max 2/2 Unit or Basis for Measurement Code - Code specifying the units in which a value is being expressed, 
    ''' or manner in which a measurement has been taken
    ''' Like CA for Case 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property OID04() As String
        Get

            Return Left(_OID04, 2)

        End Get
        Set(ByVal value As String)
            _OID04 = value
        End Set
    End Property

    Private _OID05 As String = ""
    ''' <summary>
    ''' min/max 1/15 Quantity - Numeric value of quantity
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property OID05() As String
        Get

            Return Left(_OID05, 15)

        End Get
        Set(ByVal value As String)
            _OID05 = value
        End Set
    End Property

    Private _OID06 As String = ""
    ''' <summary>
    ''' min/max 1/1 Weight Unit Code - Code specifying the weight unit
    ''' Like L Pounds
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property OID06() As String
        Get

            Return Left(_OID06, 1)

        End Get
        Set(ByVal value As String)
            _OID06 = value
        End Set
    End Property

    Private _OID07 As String = ""
    ''' <summary>
    ''' min/max 1/10 Weight -  UOM Code specifying the weight unit like L
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property OID07() As String
        Get

            Return Left(_OID07, 10)

        End Get
        Set(ByVal value As String)
            _OID07 = value
        End Set
    End Property

    Private _OID08 As String = ""
    ''' <summary>
    ''' min/max 1/1 Volume Unit Qualifier - Code identifying the volume unit
    ''' Like E cubic feet or X cubic meters
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property OID08() As String
        Get

            Return Left(_OID08, 1)

        End Get
        Set(ByVal value As String)
            _OID08 = value
        End Set
    End Property

    Private _OID09 As String = ""
    ''' <summary>
    ''' min/max 1/8 Volume - Value of volumetric measure
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property OID09() As String
        Get

            Return Left(_OID09, 8)

        End Get
        Set(ByVal value As String)
            _OID09 = value
        End Set
    End Property

End Class

#End Region

#Region " LAD - Lading Detail (Optional)"

''' <summary>
''' Lading Detail (Optional)
''' </summary>
''' <remarks>
''' Created by RHR for v-7.0.6.105 on 5/5/2017
''' </remarks>
Public Class clsEDILAD
    Public Sub New()
        MyBase.New()
        populateElements()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        populateElements()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is LAD
            Select Case i
                Case 1
                    LAD01 = sSegs(i)
                Case 2
                    LAD02 = sSegs(i)
                Case 3
                    LAD03 = sSegs(i)
                Case 4
                    LAD04 = sSegs(i)
                Case 5
                    LAD05 = sSegs(i)
                Case 6
                    LAD06 = sSegs(i)
                Case 7
                    LAD07 = sSegs(i)
                Case 8
                    LAD08 = sSegs(i)
                Case 9
                    LAD09 = sSegs(i)
                Case 10
                    LAD10 = sSegs(i)
                Case 11
                    LAD11 = sSegs(i)
                Case 12
                    LAD12 = sSegs(i)
                Case 13
                    LAD13 = sSegs(i)
                Case Else
                    Exit For
            End Select
        Next
    End Sub

    Private Sub populateElements()
        Dim sElem As String() = New String(12) {"Packaging Form Code", "Lading Quantity", "Weight Unit Code", "Unit Weight", "Weight Unit Code", "Weight", "Product/Service ID Qualifier", "Product/Service ID", "Product/Service ID Qualifier", "Product/Service ID", "Product/Service ID Qualifier", "Product/Service ID", "Lading Description"}
        Me.Elements = sElem
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 13 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format(" * {0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "LAD")
        End If
        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = LAD01
            Case 2
                strRet = LAD02
            Case 3
                strRet = LAD03
            Case 4
                strRet = LAD04
            Case 5
                strRet = LAD05
            Case 6
                strRet = LAD06
            Case 7
                strRet = LAD07
            Case 8
                strRet = LAD08
            Case 9
                strRet = LAD09
            Case 10
                strRet = LAD10
            Case 11
                strRet = LAD11
            Case 12
                strRet = LAD12
            Case 13
                strRet = LAD13
        End Select

        Return strRet
    End Function

    Public Function getEDIText() As String
        Dim sEdi As New System.Text.StringBuilder("", 500)
        sEdi.Append(Me.Description)
        sEdi.Append(vbCrLf)
        For i As Integer = 1 To 13 Step -1

            Dim strVal As String = getDataByIndex(i)
            If Not String.IsNullOrEmpty(strVal) Then
                sEdi.Append(Elements(i))
                sEdi.Append(vbCrLf)
                sEdi.Append(strVal)
                sEdi.Append(vbCrLf)
            End If

        Next
        Return sEdi.ToString
    End Function

    Private _Elements As String()
    Public Property Elements() As String()
        Get
            Return _Elements
        End Get
        Set(ByVal value As String())
            _Elements = value
        End Set
    End Property

    Private _Description As String = "Lading Detail "
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Private _LAD01 As String = ""
    ''' <summary>
    ''' min/max 3/3 Packaging Form Code - Code for packaging form of the lading quantity 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD01() As String
        Get

            Return Left(_LAD01, 3)

        End Get
        Set(ByVal value As String)
            _LAD01 = value
        End Set
    End Property

    Private _LAD02 As String = ""
    ''' <summary>
    ''' min/max 1/7 Lading Quantity -  Number of units (pieces) of the lading commodity
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD02() As String
        Get

            Return Left(_LAD02, 7)

        End Get
        Set(ByVal value As String)
            _LAD02 = value
        End Set
    End Property

    Private _LAD03 As String = ""
    ''' <summary>
    ''' min/max 1/1 Weight Unit Code -  UOM Code specifying the weight unit like L
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD03() As String
        Get

            Return Left(_LAD03, 1)

        End Get
        Set(ByVal value As String)
            _LAD03 = value
        End Set
    End Property

    Private _LAD04 As String = ""
    ''' <summary>
    ''' min/max 1/8 Unit Weight -  Numeric value of weight per unit
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD04() As String
        Get

            Return Left(_LAD04, 8)

        End Get
        Set(ByVal value As String)
            _LAD04 = value
        End Set
    End Property

    Private _LAD05 As String = ""
    ''' <summary>
    ''' min/max 1/1 Weight Unit Code -  UOM Code specifying the weight unit like L
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD05() As String
        Get

            Return Left(_LAD05, 1)

        End Get
        Set(ByVal value As String)
            _LAD05 = value
        End Set
    End Property

    Private _LAD06 As String = ""
    ''' <summary>
    ''' min/max 1/10 Weight -  Numeric value of weight
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD06() As String
        Get

            Return Left(_LAD06, 10)

        End Get
        Set(ByVal value As String)
            _LAD06 = value
        End Set
    End Property

    Private _LAD07 As String = ""
    ''' <summary>
    ''' min/max 2/2 Product/Service ID Qualifier -  Code identifying the type/source of the descriptive number 
    ''' used in Product/Service ID (234) like PD for Part Number or PN for Company Part Number 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD07() As String
        Get

            Return Left(_LAD07, 2)

        End Get
        Set(ByVal value As String)
            _LAD07 = value
        End Set
    End Property

    Private _LAD08 As String = ""
    ''' <summary>>
    ''' min/max 1/48 Product/Service ID - Identifying number for a product or service
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD08() As String
        Get

            Return Left(_LAD08, 48)

        End Get
        Set(ByVal value As String)
            _LAD08 = value
        End Set
    End Property

    Private _LAD09 As String = ""
    ''' <summary>
    ''' min/max 2/2 Product/Service ID Qualifier - Code identifying the type/source of the descriptive number used in 
    ''' Product/Service ID (234) like TP for Product type Code
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD09() As String
        Get

            Return Left(_LAD09, 2)

        End Get
        Set(ByVal value As String)
            _LAD09 = value
        End Set
    End Property

    Private _LAD10 As String = ""
    ''' <summary>
    ''' min/max 1/48 Product/Service ID - Identifying number for a product or service
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD10() As String
        Get

            Return Left(_LAD10, 48)

        End Get
        Set(ByVal value As String)
            _LAD10 = value
        End Set
    End Property

    Private _LAD11 As String = ""
    ''' <summary>
    ''' min/max 2/2 Product/Service ID Qualifier -  Code identifying the type/source of the descriptive number used in 
    ''' Product/Service ID (234) like A7 Subline Item Number
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD11() As String
        Get

            Return Left(_LAD11, 2)

        End Get
        Set(ByVal value As String)
            _LAD11 = value
        End Set
    End Property

    Private _LAD12 As String = ""
    ''' <summary>
    ''' min/max 1/48 Product/Service ID -  Identifying number for a product or service
    ''' Product Line Item – User Defined Reference Id
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD12() As String
        Get

            Return Left(_LAD12, 48)

        End Get
        Set(ByVal value As String)
            _LAD12 = value
        End Set
    End Property

    Private _LAD13 As String = ""
    ''' <summary>
    ''' min/max 1/50 Lading Description -  Description of an item as required for rating and billing purposes
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LAD13() As String
        Get

            Return Left(_LAD13, 50)

        End Get
        Set(ByVal value As String)
            _LAD13 = value
        End Set
    End Property

End Class

#End Region

#Region " LH1 - Hazardous Identification Information (Optional)"

''' <summary>
''' Hazardous Identification Information (Optional)
''' </summary>
''' <remarks>
''' Created by RHR for v-7.0.6.105 on 5/5/2017
''' </remarks>
Public Class clsEDILH1
    Public Sub New()
        MyBase.New()
        populateElements()

    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        populateElements()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is LH1
            Select Case i
                Case 1
                    LH101 = sSegs(i)
                Case 2
                    LH102 = sSegs(i)
                Case 3
                    LH103 = sSegs(i)
                Case 4
                    LH104 = sSegs(i)
                Case 5
                    LH105 = sSegs(i)

                Case Else
                    Exit For
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 5 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format(" * {0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "LH1")
        End If
        Return sEdi.ToString
    End Function

    Private Sub populateElements()
        Dim sElem As String() = New String(4) {"Unit or Basis for Measurement Code", "Lading Quantity", "UN/NA Identification Code", "Undefined", "Commodity Code"}
        Me.Elements = sElem
    End Sub

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = LH101
            Case 2
                strRet = LH102
            Case 3
                strRet = LH103
            Case 4
                strRet = LH104
            Case 5
                strRet = LH105

        End Select

        Return strRet
    End Function


    Public Function getEDIText() As String
        Dim sEdi As New System.Text.StringBuilder("", 500)
        sEdi.Append(Me.Description)
        sEdi.Append(vbCrLf)
        For i As Integer = 1 To 13 Step -1

            Dim strVal As String = getDataByIndex(i)
            If Not String.IsNullOrEmpty(strVal) Then
                sEdi.Append(Elements(i))
                sEdi.Append(vbCrLf)
                sEdi.Append(strVal)
                sEdi.Append(vbCrLf)
            End If

        Next
        Return sEdi.ToString
    End Function

    Private _Elements As String()
    Public Property Elements() As String()
        Get
            Return _Elements
        End Get
        Set(ByVal value As String())
            _Elements = value
        End Set
    End Property

    Private _Description As String = "Hazardous Identification Information "
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Private _LH101 As String = ""
    ''' <summary>
    ''' min/max 2/2 Unit or Basis for Measurement Code - Code specifying the units in which a value is being expressed, 
    ''' or manner in which a measurement has been taken like CA Case
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LH101() As String
        Get

            Return Left(_LH101, 2)

        End Get
        Set(ByVal value As String)
            _LH101 = value
        End Set
    End Property

    Private _LH102 As String = ""
    ''' <summary>
    ''' min/max 1/7 Lading Quantity - Number of units (pieces) of the lading commodity
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LH102() As String
        Get

            Return Left(_LH102, 7)

        End Get
        Set(ByVal value As String)
            _LH102 = value
        End Set
    End Property

    Private _LH103 As String = ""
    ''' <summary>
    ''' min/max 6/6 UN/NA Identification Code - Code identifying the hazardous material 
    ''' identification number as required by Title 49 of the code of Federal Regulations; 
    ''' UN/NA stands for United Nations/North America
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LH103() As String
        Get

            Return Left(_LH103, 6)

        End Get
        Set(ByVal value As String)
            _LH103 = value
        End Set
    End Property

    Private _LH104 As String = ""
    ''' <summary>
    ''' min/max 1/1 Undefined
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LH104() As String
        Get

            Return Left(_LH104, 1)

        End Get
        Set(ByVal value As String)
            _LH104 = value
        End Set
    End Property

    Private _LH105 As String = ""
    ''' <summary>
    ''' min/max 1/30 Commodity Code - Code describing a commodity or group of commodities
    ''' Hazardous material desctiption
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property LH105() As String
        Get

            Return Left(_LH105, 30)

        End Get
        Set(ByVal value As String)
            _LH105 = value
        End Set
    End Property



End Class

#End Region

#Region " AT5 - Bill Of Lading Handling Requirements (Optional)"

''' <summary>
''' Bill Of Lading Handling Requirements (Optional)
''' </summary>
''' <remarks>
''' Created by RHR for v-7.0.6.105 on 5/5/2017
''' </remarks>
Public Class clsEDIAT5
    Public Sub New()
        MyBase.New()
        populateElements()

    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        populateElements()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is AT5
            Select Case i
                Case 1
                    AT501 = sSegs(i)
                Case 2
                    AT502 = sSegs(i)
                Case Else
                    Exit For
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 2 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format(" * {0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "AT5")
        End If
        Return sEdi.ToString
    End Function

    Private Sub populateElements()
        Dim sElem As String() = New String(1) {"Special Handling Code", "Special Services Code"}
        Me.Elements = sElem
    End Sub

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = AT501
            Case 2
                strRet = AT502
        End Select

        Return strRet
    End Function


    Public Function getEDIText() As String
        Dim sEdi As New System.Text.StringBuilder("", 500)
        sEdi.Append(Me.Description)
        sEdi.Append(vbCrLf)
        For i As Integer = 1 To 2 Step -1

            Dim strVal As String = getDataByIndex(i)
            If Not String.IsNullOrEmpty(strVal) Then
                sEdi.Append(Elements(i))
                sEdi.Append(vbCrLf)
                sEdi.Append(strVal)
                sEdi.Append(vbCrLf)
            End If

        Next
        Return sEdi.ToString
    End Function

    Private _Elements As String()
    Public Property Elements() As String()
        Get
            Return _Elements
        End Get
        Set(ByVal value As String())
            _Elements = value
        End Set
    End Property

    Private _Description As String = "Bill Of Lading Handling Requirements"
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Private _AT501 As String = ""
    ''' <summary>
    ''' min/max 2/3 Special Handling Code 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property AT501() As String
        Get

            Return Left(_AT501, 3)

        End Get
        Set(ByVal value As String)
            _AT501 = value
        End Set
    End Property

    Private _AT502 As String = ""
    ''' <summary>
    ''' min/max 2/10 Special Services Code
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Property AT502() As String
        Get

            Return Left(_AT502, 10)

        End Get
        Set(ByVal value As String)
            _AT502 = value
        End Set
    End Property

End Class

#End Region

#Region "204 Loop 100"

''' <summary>
''' EDI 204 100 Loop Contact Information
''' </summary>
''' <remarks>
''' Modified by RHR for v-7.0.6.105 on 5/5/2017
''' </remarks>
Public Class clsEDI204Loop100
    Inherits EDIObjBase

    Public N1 As New clsEDIN1
    Public N3s As New List(Of clsEDIN3)
    ''' <summary>
    ''' First N3 item in List
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 6/3/2017
    '''   added logic to create a new item in the list if it does not exist on Set
    ''' </remarks>
    Public Property N3() As clsEDIN3
        Get
            If Not N3s Is Nothing AndAlso N3s.Count() > 0 Then
                Return N3s(0)
            Else
                Return New clsEDIN3()
            End If
        End Get
        Set(ByVal value As clsEDIN3)
            If N3s Is Nothing Then N3s = New List(Of clsEDIN3)
            If N3s.Count > 0 Then
                N3s(0) = value
            Else
                N3s.Add(value)
            End If
        End Set
    End Property

    Public N4 As New clsEDIN4
    Public G61s As New List(Of clsEDIG61)
    ''' <summary>
    ''' First G61 item in List
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 6/3/2017
    '''   added logic to create a new item in the list if it does not exist on Set
    ''' </remarks>
    Public Property G61() As clsEDIG61
        Get
            If Not G61s Is Nothing AndAlso G61s.Count() > 0 Then
                Return G61s(0)
            Else
                Return New clsEDIG61()
            End If
        End Get
        Set(ByVal value As clsEDIG61)
            If G61s Is Nothing Then G61s = New List(Of clsEDIG61)
            If G61s.Count > 0 Then
                G61s(0) = value
            Else
                G61s.Add(value)
            End If
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        populateElements()
    End Sub


    Sub New(ByVal oN1 As clsEDIN1, ByVal oN3s As List(Of clsEDIN3), ByVal oN4 As clsEDIN4, ByVal oG61s As List(Of clsEDIG61))
        MyBase.New()
        populateElements()
        Me.N1 = oN1
        Me.N3s = oN3s
        Me.N4 = oN4
        Me.G61s = oG61s

    End Sub

    Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        populateElements()
        addDataFromString(strSource, strSegSep)
    End Sub

    'Added by LVV 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    Public Sub New(ByVal blnNoSpaces As Boolean)
        MyBase.New()
        populateElements()
        NoSpaces = blnNoSpaces

    End Sub

    'Added by LVV 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    Private _NoSpaces As Boolean = False
    Public Property NoSpaces() As Boolean
        Get
            Return _NoSpaces
        End Get
        Set(ByVal value As Boolean)
            _NoSpaces = value
        End Set
    End Property

    Public Sub New(ByVal Name As String, ByVal Number As String, ByVal Address1 As String, ByVal Address2 As String, ByVal City As String, ByVal State As String, ByVal Zip As String, ByVal Country As String)
        MyBase.New()
        populateElements()
        N1 = New clsEDIN1(NoSpaces) With {.N101 = "PF", .N102 = Left(Name, 60), .N104 = Number}
        N3 = New clsEDIN3 With {.N301 = Left(Address1, 55), .N302 = Left(Address2, 55)}
        N4 = New clsEDIN4(NoSpaces) With {.N401 = Left(City, 30), .N402 = Left(State, 2), .N403 = Left(Zip, 15)}
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    ''' <remarks>
    ''' Added by LVV on 4/15/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Overrides Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        Dim sElements = strSource.Split(strSegSep)
        Dim strKey As String = ""
        If Not sElements Is Nothing AndAlso sElements.Count() > 0 Then
            For Each e In sElements
                If getKey(e, strKey) Then If Me.usesElement(strKey) Then addSegment(e, strKey)
            Next
        End If
    End Sub


    Public Overrides Function addSegment(strSource As String, Optional strKey As String = "") As Boolean
        If String.IsNullOrWhiteSpace(strKey) Then
            If Not getKey(strSource, strKey) Then
                'nothing to do just return
                Return False
            End If
        End If
        If Not Me.usesElement(strKey) Then Return False
        If Not Me.Elements.ContainsKey(strKey) Then Return False
        Dim tracker As EDIItemTracker = Me.Elements(strKey)
        If tracker.ItemsUsed < tracker.ItemsAllowed Then
            Select Case strKey
                Case "N1"
                    Me.N1 = New clsEDIN1(strSource)
                Case "N3"
                    If N3s Is Nothing Then N3s = New List(Of clsEDIN3)
                    N3s.Add(New clsEDIN3(strSource))
                Case "N4"
                    Me.N4 = New clsEDIN4(strSource)
                Case "G61"
                    If G61s Is Nothing Then G61s = New List(Of clsEDIG61)
                    G61s.Add(New clsEDIG61(strSource))
                Case Else
                    Return False
            End Select
            tracker.ItemsUsed += 1
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function addLoopFromKey(strElemKey As String, ByRef strElements() As String, ByRef NextIndex As Integer) As Boolean
        Return False 'no loops in 100 Loop
    End Function

    Protected Overrides Sub populateElements()
        Me.Name = "204 Loop 100"
        Me.Elements = New Dictionary(Of String, EDIItemTracker)
        Me.Loops = New Dictionary(Of String, EDIItemTracker)
        Me.Elements.Add("N1", New EDIItemTracker(1))
        Me.Elements.Add("N3", New EDIItemTracker(2))
        Me.Elements.Add("N4", New EDIItemTracker(1))
        Me.Elements.Add("G61", New EDIItemTracker(3))
    End Sub
End Class

#End Region

#Region "204 Loop 200"

''' <summary>
''' EDI 204 200 Loop Equipment Details
''' </summary>
''' <remarks>
''' Modified by RHR for v-7.0.6.105 on 5/5/2017
''' </remarks>
Public Class clsEDI204Loop200
    Inherits EDIObjBase

    Public N7 As New clsEDIN7

    Public Sub New()
        MyBase.New()
        populateElements()
    End Sub

    Sub New(ByVal oN7 As clsEDIN7)
        MyBase.New()
        populateElements()
        Me.N7 = oN7
    End Sub

    Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        populateElements()
        addDataFromString(strSource, strSegSep)
    End Sub

    Public Sub New(ByVal intSeq As Integer, ByVal Desc As String, ByVal Code As String)
        MyBase.New()
        populateElements()
        N7 = New clsEDIN7 With {.N701 = Left(intSeq.ToString(), 4), .N702 = Left(Desc, 10), .N711 = Left(Code, 2)}
    End Sub

    Public Overrides Sub addDataFromString(strSource As String, strSegSep As String)
        Dim sElements = strSource.Split(strSegSep)
        Dim strKey As String = ""
        If Not sElements Is Nothing AndAlso sElements.Count() > 0 Then
            For Each e In sElements
                If getKey(e, strKey) Then If Me.usesElement(strKey) Then addSegment(e, strKey)
            Next
        End If
    End Sub

    Public Overrides Function addSegment(strSource As String, Optional strKey As String = "") As Boolean
        If String.IsNullOrWhiteSpace(strKey) Then
            If Not getKey(strSource, strKey) Then
                'nothing to do just return
                Return False
            End If
        End If
        If Not Me.usesElement(strKey) Then Return False
        If Not Me.Elements.ContainsKey(strKey) Then Return False
        Dim tracker As EDIItemTracker = Me.Elements(strKey)
        If tracker.ItemsUsed < tracker.ItemsAllowed Then
            Select Case strKey
                Case "N7"
                    Me.N7 = New clsEDIN7(strSource)
                Case Else
                    Return False
            End Select
            tracker.ItemsUsed += 1
            Return True
        Else
            Return False
        End If
    End Function

    Protected Overrides Sub populateElements()
        Me.Name = "204 Loop 200"
        Me.Elements = New Dictionary(Of String, EDIItemTracker)
        Me.Loops = New Dictionary(Of String, EDIItemTracker)
        Me.Elements.Add("N7", New EDIItemTracker(1))
    End Sub

    Public Overrides Function addLoopFromKey(strElemKey As String, ByRef strElements() As String, ByRef NextIndex As Integer) As Boolean
        Return False 'no loops in 200 Loop
    End Function
End Class

#End Region

#Region "204 Loop 300"
''' <summary>
''' STOP OFF DETAILS  Loop id - 0300
''' </summary>
''' <remarks>
''' Modified by RHR on 05/30/2018 for v-6.0.4.4-m
'''   Added support for multiple NTE segments
''' Modified by RHR on 08/08/2018 for v-6.0.4.4.m
'''   Added support for multiple G62 segments
''' </remarks>
Public Class clsEDI204Loop300

    Public S5 As New clsEDIS5
    Public L11s As New List(Of clsEDIL11)
    Public PLD As New clsEDIPLD
    Public Loop310() As clsEDI204Loop310
    Public Loop320 As List(Of clsEDI204Loop320)

    Private _G62s As New List(Of clsEDIG62)
    ''' <summary>
    ''' contains a list of G62 Date and Time References
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR on 08/08/2018 for v-6.0.4.4-m
    ''' </remarks>
    Public Property G62s() As List(Of clsEDIG62)
        Get
            If _G62s Is Nothing Then _G62s = New List(Of clsEDIG62)
            Return _G62s
        End Get
        Set(ByVal value As List(Of clsEDIG62))
            _G62s = value
        End Set
    End Property

    ''' <summary>
    ''' contains the first Note or Special instructions in the NTEs list.  Should use the NTEs property for access to all items
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR on 08/08/2018 for v-6.0.4.4-m
    ''' </remarks>
    Public Property G62() As clsEDIG62
        Get
            If G62s Is Nothing Then G62s = New List(Of clsEDIG62)
            If G62s.Count < 1 Then G62s.Add(New clsEDIG62())
            Return G62s(0)
        End Get
        Set(ByVal value As clsEDIG62)
            If G62s Is Nothing Then G62s = New List(Of clsEDIG62)
            If G62s.Count > 0 Then
                G62s(0) = value
            Else
                G62s.Add(value)
            End If
        End Set
    End Property

    Private _NTEs As New List(Of clsEDINTE)
    ''' <summary>
    ''' contains a list of NOTEs or SPECIAL INSTRUCTION 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR on 05/30/2018 for v-6.0.4.4-m
    ''' </remarks>
    Public Property NTEs() As List(Of clsEDINTE)
        Get
            If _NTEs Is Nothing Then _NTEs = New List(Of clsEDINTE)
            Return _NTEs
        End Get
        Set(ByVal value As List(Of clsEDINTE))
            _NTEs = value
        End Set
    End Property



    ''' <summary>
    ''' contains the first Note or Special instructions in the NTEs list.  Should use the NTEs property for access to all items
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR on 05/30/2018 for v-6.0.4.4-m
    ''' </remarks>
    Public Property NTE() As clsEDINTE
        Get
            If NTEs Is Nothing Then NTEs = New List(Of clsEDINTE)
            If NTEs.Count < 1 Then NTEs.Add(New clsEDINTE())
            Return NTEs(0)
        End Get
        Set(ByVal value As clsEDINTE)
            If NTEs Is Nothing Then NTEs = New List(Of clsEDINTE)
            If NTEs.Count > 0 Then
                NTEs(0) = value
            Else
                NTEs.Add(value)
            End If
        End Set
    End Property


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    ''' <remarks>
    ''' Added by LVV on 4/15/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        'Dim o300 As New clsEDI204Loop300

        Dim o310List As New List(Of clsEDI204Loop310)
        Dim o320List As New List(Of clsEDI204Loop320)

        'Split off any loop320 segments if they exist (by L5)
        Dim s320s() As String = Regex.Split(strSource, "\" & strSegSep & "L5\*")

        If s320s.Length > 1 Then
            'There are 320s so process them
            'the first item of s320s() contains the rest of the Loop300 data so skip it
            For i320 = 1 To s320s.Length - 1
                'process all the 320s and add them to the list
                Dim o320 As New clsEDI204Loop320
                o320.addDataFromString(s320s(i320), strSegSep)
                o320List.Add(o320)
            Next
        End If

        Loop320 = o320List

        'the first item of s320s() contains the rest of the Loop 300 data -- S5, L11s, G62, PLD, NTE, Loop310()
        'now split off any Loop 310s by N1
        Dim s310s() As String = Regex.Split(s320s(0), "\" & strSegSep & "N1\*")

        'the first item of s310s() contains the rest of the Loop300 data so skip it
        For i310 = 1 To s310s.Length - 1
            'process all the 310s and add them to the list
            Dim o310 As New clsEDI204Loop310
            o310.addDataFromString(s310s(i310), strSegSep)
            o310List.Add(o310)
        Next

        Loop310 = o310List.ToArray()

        'the first item of s310s() contains the rest of the Loop 300 data -- (S5, L11s, G62, PLD, NTE)
        'everything but S5 is optional so we have to split segements off one by one

        'now split off the L11s
        Dim sL11s() As String = Regex.Split(s310s(0), "\" & strSegSep & "L11\*")

        Dim oL11List As New List(Of clsEDIL11)
        If sL11s.Length > 1 Then
            For iL11 = 1 To sL11s.Length - 2
                'the last one might have more segs after it
                If Strings.Left(sL11s(iL11), 4) <> "L11*" Then sL11s(iL11) = "L11*" & sL11s(iL11)
                oL11List.Add(New clsEDIL11(sL11s(iL11)))
            Next

            If sL11s(sL11s.Length - 1).Contains("G62") Then
                Dim sG62s() As String = Regex.Split(sL11s(sL11s.Length - 1), "\" & strSegSep & "G62\*")
                addG62_PLD_NTEDataFromString(sG62s(1), strSegSep)
                'process the last L11
                If Strings.Left(sG62s(0), 4) <> "L11*" Then sG62s(0) = "L11*" & sG62s(0)
                oL11List.Add(New clsEDIL11(sG62s(0)))
            Else
                Dim sNTEs() As String = Regex.Split(sL11s(sL11s.Length - 1), "\" & strSegSep & "NTE\*")
                If sNTEs.Length > 1 Then
                    If Strings.Left(sNTEs(1), 4) <> "NTE*" Then sNTEs(1) = "NTE*" & sNTEs(1)
                    NTE = New clsEDINTE(sNTEs(1))
                End If
                Dim sPLDs() As String = Regex.Split(sNTEs(0), "\" & strSegSep & "PLD\*")
                If sPLDs.Length > 1 Then
                    If Strings.Left(sPLDs(1), 4) <> "PLD*" Then sPLDs(1) = "PLD*" & sPLDs(1)
                    PLD = New clsEDIPLD(sPLDs(1))
                End If
                'process the last L11
                If Strings.Left(sPLDs(0), 4) <> "L11*" Then sPLDs(0) = "L11*" & sPLDs(0)
                oL11List.Add(New clsEDIL11(sPLDs(0)))
            End If

        End If

        L11s = oL11List

        'Create the S5
        If Strings.Left(sL11s(0), 3) <> "S5*" Then sL11s(0) = "S5*" & sL11s(0)
        S5 = New clsEDIS5(sL11s(0))


    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    ''' <remarks>
    ''' Added by LVV on 4/15/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub addG62_PLD_NTEDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        'the G62, PLD, and NTE elements are not required.
        For isubsegs As Integer = 2 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the G62 record (it should be all that is left 
                    If Left(sElems(0), 4) <> "G62*" Then sElems(0) = "G62*" & sElems(0)
                    G62 = New clsEDIG62(sElems(0))
                Case 1
                    'read the PLD record
                    segs = Regex.Split(strSource, "\" & strSegSep & "PLD\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        PLD = New clsEDIPLD("PLD*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 2
                    'read the NTE record
                    segs = Regex.Split(strSource, "\" & strSegSep & "NTE\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        NTE = New clsEDINTE("NTE*" & sElems(0))
                        strSource = segs(0)
                    End If

            End Select
        Next

    End Sub


End Class

#End Region

#Region "204 Loop 310 (Nested Inside 204 300)"

Public Class clsEDI204Loop310
    Public N1 As New clsEDIN1
    Public N3 As New clsEDIN3
    Public N4 As New clsEDIN4
    Public G61 As New clsEDIG61

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    ''' <remarks>
    ''' Added by LVV on 4/15/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        'the N1, N3, and N4 are requred but the G61 is not.
        For isubsegs As Integer = 3 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the N1 record (it should be all that is left 
                    If Left(sElems(0), 3) <> "N1*" Then sElems(0) = "N1*" & sElems(0)
                    N1 = New clsEDIN1(sElems(0))
                Case 1
                    'read the N3 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "N3\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        N3 = New clsEDIN3("N3*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 2
                    'read the N4 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "N4\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        N4 = New clsEDIN4("N4*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 3
                    'read the G61 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "G61\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        G61 = New clsEDIG61("G61*" & sElems(0))
                        strSource = segs(0)
                    End If

            End Select
        Next

    End Sub



End Class

#End Region

#Region "204 Loop 320 (Nested Inside 204 300)"

Public Class clsEDI204Loop320
    Inherits EDIObjBase

    Public L5 As New clsEDIL5
    Public AT8 As New clsEDIAT8

    Sub New()
        MyBase.New()
        populateElements()
    End Sub

    Sub New(ByVal oL5 As clsEDIL5, ByVal oAT8 As clsEDIAT8)
        MyBase.New()
        populateElements()
        Me.L5 = oL5
        Me.AT8 = oAT8

    End Sub

    Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        populateElements()
        addDataFromString(strSource, strSegSep)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    '''  <remarks>
    ''' Added by LVV on 4/15/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' Modified for Backward compatibility by RHR for v-7.0.6.105 on 5/5/2017
    '''   New implementations should use the insertElements method
    ''' </remarks>
    Public Overrides Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        Dim sElements = strSource.Split(strSegSep)
        Dim strKey As String = ""
        If Not sElements Is Nothing AndAlso sElements.Count() > 0 Then
            For Each e In sElements
                If getKey(e, strKey) Then If Me.usesElement(strKey) Then addSegment(e, strKey)
            Next
        End If
    End Sub


    Public Overrides Function addSegment(strSource As String, Optional strKey As String = "") As Boolean
        If String.IsNullOrWhiteSpace(strKey) Then
            If Not getKey(strSource, strKey) Then
                'nothing to do just return
                Return False
            End If
        End If
        If Not Me.usesElement(strKey) Then Return False
        If Not Me.Elements.ContainsKey(strKey) Then Return False
        Dim tracker As EDIItemTracker = Me.Elements(strKey)
        If tracker.ItemsUsed < tracker.ItemsAllowed Then
            Select Case strKey
                Case "L5"
                    Me.L5 = New clsEDIL5(strSource)
                Case "AT8"
                    Me.AT8 = New clsEDIAT8(strSource)
                Case Else
                    Return False
            End Select
            tracker.ItemsUsed += 1
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function addLoopFromKey(strElemKey As String, ByRef strElements() As String, ByRef NextIndex As Integer) As Boolean
        Return False 'no loops in 320 Loop
    End Function

    Protected Overrides Sub populateElements()
        Me.Name = "204 Loop 320"
        Me.Elements = New Dictionary(Of String, EDIItemTracker)
        Me.Loops = New Dictionary(Of String, EDIItemTracker)
        Me.Elements.Add("L5", New EDIItemTracker(1))
        Me.Elements.Add("AT8", New EDIItemTracker(1))
    End Sub
End Class

#End Region

#Region " 204 Full Truck Load (Single Stop Load)"

Public Class clsEDITruckLoad
    Public ST As New clsEDIST
    Public B2 As New clsEDIB2
    Public B2A As New clsEDIB2A
    Public L11 As New clsEDIL11
    Public NTE As New clsEDINTE
    Public PLD As New clsEDIPLD
    Public Loop100 As New List(Of clsEDI204Loop100)
    Public Loop200 As New List(Of clsEDI204Loop200)
    Public Loop300() As clsEDI204Loop300
    Public L3 As New clsEDIL3
    Public SE As New clsEDISE


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    ''' <param name="strSEElem"></param>
    ''' <remarks>
    ''' Added by LVV on 4/14/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
        'The first item in the s100Ls list contains the B2, B2A, L11 (and possibly NTE and PLD ) data.  We use this to create the 204 object
        'the ST, B2, B2A, and L11 elements are requred but NTE and PLD are not.
        For isubsegs As Integer = 5 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the ST record (it should be all that is left 
                    If Left(sElems(0), 3) <> "ST*" Then sElems(0) = "ST*" & sElems(0)
                    ST = New clsEDIST(sElems(0))
                Case 1
                    'read the B2 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "B2\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        B2 = New clsEDIB2("B2*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 2
                    'read the B2A record
                    segs = Regex.Split(strSource, "\" & strSegSep & "B2A\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        B2A = New clsEDIB2A("B2A*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 3
                    'read the L11 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "L11\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        L11 = New clsEDIL11("L11*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 4
                    'read the NTE record
                    segs = Regex.Split(strSource, "\" & strSegSep & "NTE\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        NTE = New clsEDINTE("NTE*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 5
                    'read the PLD record
                    segs = Regex.Split(strSource, "\" & strSegSep & "PLD\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        PLD = New clsEDIPLD("PLD*" & sElems(0))
                        strSource = segs(0)
                    End If

            End Select
        Next
        'Add the SE
        If Not String.IsNullOrEmpty(strSEElem) Then
            'split out any unwanted elements 
            Dim sElems() As String = strSEElem.Split(strSegSep)
            If Left(sElems(0), 3) <> "SE*" Then sElems(0) = "SE*" & sElems(0)
            SE = New clsEDISE(sElems(0))
        End If
    End Sub

    ''' <summary>
    ''' Creates an EDI 204 document string
    ''' </summary>
    ''' <param name="strISA"></param>
    ''' <param name="strGS"></param>
    ''' <param name="strIEA"></param>
    ''' <param name="strGE"></param>
    ''' <param name="SegmentTerminator"></param>
    ''' <returns>String</returns>
    ''' <remarks>
    ''' Added by LVV on 4/15/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Function getEDIRecord(ByVal strISA As String, ByVal strGS As String, ByVal strIEA As String, ByVal strGE As String, ByVal SegmentTerminator As String) As String
        'Format the edi record
        Dim strToPrint As String = ""
        strToPrint &= strISA
        strToPrint &= strGS
        strToPrint &= ST.getEDIString(SegmentTerminator)
        strToPrint &= B2.getEDIString(SegmentTerminator)
        strToPrint &= B2A.getEDIString(SegmentTerminator)
        strToPrint &= L11.getEDIString(SegmentTerminator)
        strToPrint &= NTE.getEDIString(SegmentTerminator)
        strToPrint &= PLD.getEDIString(SegmentTerminator)
        If Not Loop100 Is Nothing Then
            For Each L As clsEDI204Loop100 In Loop100
                strToPrint &= L.N1.getEDIString(SegmentTerminator)
                strToPrint &= L.N3.getEDIString(SegmentTerminator)
                strToPrint &= L.N4.getEDIString(SegmentTerminator)
                strToPrint &= L.G61.getEDIString(SegmentTerminator)
            Next
        End If
        If Not Loop200 Is Nothing Then
            For Each L As clsEDI204Loop200 In Loop200
                strToPrint &= L.N7.getEDIString(SegmentTerminator)
            Next
        End If
        If Not Loop300 Is Nothing Then
            For Each L As clsEDI204Loop300 In Loop300
                strToPrint &= L.S5.getEDIString(SegmentTerminator)
                If Not L.L11s Is Nothing Then
                    For Each L11 As clsEDIL11 In L.L11s
                        strToPrint &= L11.getEDIString(SegmentTerminator)
                    Next
                End If
                strToPrint &= L.G62.getEDIString(SegmentTerminator)
                strToPrint &= L.PLD.getEDIString(SegmentTerminator)
                strToPrint &= L.NTE.getEDIString(SegmentTerminator)
                If Not L.Loop310 Is Nothing Then
                    For Each L310 As clsEDI204Loop310 In L.Loop310
                        strToPrint &= L310.N1.getEDIString(SegmentTerminator)
                        strToPrint &= L310.N3.getEDIString(SegmentTerminator)
                        strToPrint &= L310.N4.getEDIString(SegmentTerminator)
                        strToPrint &= L310.G61.getEDIString(SegmentTerminator)
                    Next
                End If
                If Not L.Loop320 Is Nothing Then
                    For Each L320 As clsEDI204Loop320 In L.Loop320
                        strToPrint &= L320.L5.getEDIString(SegmentTerminator)
                        strToPrint &= L320.AT8.getEDIString(SegmentTerminator)
                    Next
                End If
            Next
        End If
        strToPrint &= L3.getEDIString(SegmentTerminator)
        strToPrint &= SE.getEDIString(SegmentTerminator)
        strToPrint &= strGE
        strToPrint &= strIEA

        Return strToPrint

    End Function

    ''' <summary>
    ''' Creates an EDI 204 document string FOR TESTING
    ''' </summary>
    ''' <param name="oISA"></param>
    ''' <param name="oGS"></param>
    ''' <param name="oIEA"></param>
    ''' <param name="oGE"></param>
    ''' <param name="SegmentTerminator"></param>
    ''' <returns>String</returns>
    ''' <remarks>
    ''' Added by LVV on 4/15/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Function getEDIRecord(ByVal oISA As clsEDIISA, ByVal oGS As clsEDIGS, ByVal oIEA As clsEDIIEA, ByVal oGE As clsEDIGE, ByVal SegmentTerminator As String) As String
        'Format the edi record
        Dim strToPrint As String = ""
        strToPrint &= oISA.getEDIString(SegmentTerminator)
        strToPrint &= oGS.getEDIString(SegmentTerminator)
        strToPrint &= ST.getEDIString(SegmentTerminator)
        strToPrint &= B2.getEDIString(SegmentTerminator)
        strToPrint &= B2A.getEDIString(SegmentTerminator)
        strToPrint &= L11.getEDIString(SegmentTerminator)
        strToPrint &= NTE.getEDIString(SegmentTerminator)
        strToPrint &= PLD.getEDIString(SegmentTerminator)
        If Not Loop100 Is Nothing Then
            For Each L As clsEDI204Loop100 In Loop100
                strToPrint &= L.N1.getEDIString(SegmentTerminator)
                strToPrint &= L.N3.getEDIString(SegmentTerminator)
                strToPrint &= L.N4.getEDIString(SegmentTerminator)
                strToPrint &= L.G61.getEDIString(SegmentTerminator)
            Next
        End If
        If Not Loop200 Is Nothing Then
            For Each L As clsEDI204Loop200 In Loop200
                strToPrint &= L.N7.getEDIString(SegmentTerminator)
            Next
        End If
        If Not Loop300 Is Nothing Then
            For Each L As clsEDI204Loop300 In Loop300
                strToPrint &= L.S5.getEDIString(SegmentTerminator)
                If Not L.L11s Is Nothing Then
                    For Each L11 As clsEDIL11 In L.L11s
                        strToPrint &= L11.getEDIString(SegmentTerminator)
                    Next
                End If
                strToPrint &= L.G62.getEDIString(SegmentTerminator)
                strToPrint &= L.PLD.getEDIString(SegmentTerminator)
                strToPrint &= L.NTE.getEDIString(SegmentTerminator)
                If Not L.Loop310 Is Nothing Then
                    For Each L310 As clsEDI204Loop310 In L.Loop310
                        strToPrint &= L310.N1.getEDIString(SegmentTerminator)
                        strToPrint &= L310.N3.getEDIString(SegmentTerminator)
                        strToPrint &= L310.N4.getEDIString(SegmentTerminator)
                        strToPrint &= L310.G61.getEDIString(SegmentTerminator)
                    Next
                End If
                If Not L.Loop320 Is Nothing Then
                    For Each L320 As clsEDI204Loop320 In L.Loop320
                        strToPrint &= L320.L5.getEDIString(SegmentTerminator)
                        strToPrint &= L320.AT8.getEDIString(SegmentTerminator)
                    Next
                End If
            Next
        End If
        strToPrint &= L3.getEDIString(SegmentTerminator)
        strToPrint &= SE.getEDIString(SegmentTerminator)
        strToPrint &= oGE.getEDIString(SegmentTerminator)
        strToPrint &= oIEA.getEDIString(SegmentTerminator)

        Return strToPrint

    End Function

End Class

#End Region

#Region "204 In Loop 300"
''' <summary>
''' EDI Inbound 204 300 Loop Stop Off Details
''' </summary>
''' <remarks>
''' Created by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
''' Modified by RHR on 5/5/2017 for v-7.0.6.105
''' </remarks>
Public Class clsEDI204InLoop300
    Inherits EDIObjBase

    'Added by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
    Public S5 As New clsEDIS5
    Public L11s As New List(Of clsEDIL11)
    Public G62s As New List(Of clsEDIG62)
    Public AT5s As New List(Of clsEDIAT5)
    Public PLD As New clsEDIPLD
    Public NTEs As New List(Of clsEDINTE)
    Public Loop310 As New List(Of clsEDI204InLoop310)
    Public Loop320 As List(Of clsEDI204Loop320)
    'Loop 350 replaces Loop320  When Loop 350 exists 
    'database ignores loop 320
    Public Loop350 As List(Of clsEDI204InLoop350)

    Sub New()
        MyBase.New()
        populateElements()
    End Sub

    Sub New(ByVal oS5 As clsEDIS5, ByVal oL11s As List(Of clsEDIL11), ByVal oG62s As List(Of clsEDIG62), ByVal oPLD As clsEDIPLD, ByVal oNTEs As List(Of clsEDINTE))
        MyBase.New()
        populateElements()
        Me.S5 = oS5
        Me.L11s = oL11s
        Me.G62s = oG62s
        Me.PLD = oPLD
        Me.NTEs = oNTEs
    End Sub

    Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        populateElements()
        addDataFromString(strSource, strSegSep)
    End Sub

    ''' <summary>
    ''' addDataFromString
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    ''' <remarks>
    ''' Added by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
    ''' </remarks>
    Public Overrides Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        'Dim o300 As New clsEDI204Loop300

        'Dim o310List As New List(Of clsEDI204InLoop310)
        'Dim o320List As New List(Of clsEDI204Loop320)

        'split off any 350 loops
        Loop350 = New List(Of clsEDI204InLoop350)
        'Split off any loop350 segments if they exist (by L5)
        Dim s350s() As String = Regex.Split(strSource, "\" & strSegSep & "L5\*")
        If s350s.Length > 1 Then
            strSource = s350s(0)
            'There are 350s so process them
            'the first item of s350s() contains the rest of the Loop300 data so skip it
            For i350 = 1 To s350s.Length - 1
                'process all the 350s and add them to the list
                Dim o350 As New clsEDI204InLoop350
                o350.addDataFromString(s350s(i350), strSegSep)
                Loop350.Add(o350)
            Next
        Else
            Loop320 = New List(Of clsEDI204Loop320)
            'check for any 320 (Legacy 204s)
            'Split off any loop320 segments if they exist (by L5)
            Dim s320s() As String = Regex.Split(strSource, "\" & strSegSep & "L5\*")

            If s320s.Length > 1 Then
                strSource = s320s(0)
                'There are 320s so process them
                'the first item of s320s() contains the rest of the Loop300 data so skip it
                For i320 = 1 To s320s.Length - 1
                    'process all the 320s and add them to the list
                    Dim o320 As New clsEDI204Loop320
                    o320.addDataFromString(s320s(i320), strSegSep)
                    Loop320.Add(o320)
                Next
            End If
        End If

        'the first item of s320s() contains the rest of the Loop 300 data -- S5, L11s, G62, PLD, NTE, Loop310()
        'now split off any Loop 310s by N1
        Loop310 = New List(Of clsEDI204InLoop310)
        Dim s310s() As String = Regex.Split(strSource, "\" & strSegSep & "N1\*")
        If s310s.Length > 1 Then
            strSource = s310s(0)
            'There are 310s so process them
            'the first item of s310s() contains the rest of the Loop300 data so skip it
            For i310 = 1 To s310s.Length - 1
                'process all the 310s and add them to the list
                Dim o310 As New clsEDI204InLoop310
                o310.addDataFromString(s310s(i310), strSegSep)
                Loop310.Add(o310)
            Next
        End If

        Dim sElements = strSource.Split(strSegSep)
        Dim strKey As String = ""
        If Not sElements Is Nothing AndAlso sElements.Count() > 0 Then
            For Each e In sElements
                If getKey(e, strKey) Then If Me.usesElement(strKey) Then addSegment(e, strKey)
            Next
        End If
    End Sub

    Public Overrides Function addSegment(strSource As String, Optional strKey As String = "") As Boolean
        If String.IsNullOrWhiteSpace(strKey) Then
            If Not getKey(strSource, strKey) Then
                'nothing to do just return
                Return False
            End If
        End If
        If Not Me.usesElement(strKey) Then Return False
        If Not Me.Elements.ContainsKey(strKey) Then Return False
        Dim tracker As EDIItemTracker = Me.Elements(strKey)
        If tracker.ItemsUsed < tracker.ItemsAllowed Then
            Select Case strKey
                Case "S5"
                    Me.S5 = New clsEDIS5(strSource)
                Case "L11"
                    If L11s Is Nothing Then L11s = New List(Of clsEDIL11)
                    L11s.Add(New clsEDIL11(strSource))
                Case "G62"
                    If G62s Is Nothing Then G62s = New List(Of clsEDIG62)
                    G62s.Add(New clsEDIG62(strSource))
                Case "PLD"
                    Me.PLD = New clsEDIPLD(strSource)
                Case "AT5"
                    If AT5s Is Nothing Then AT5s = New List(Of clsEDIAT5)
                    AT5s.Add(New clsEDIAT5(strSource))
                Case "NTE"
                    If NTEs Is Nothing Then NTEs = New List(Of clsEDINTE)
                    NTEs.Add(New clsEDINTE(strSource))
                Case Else
                    Return False
            End Select
            tracker.ItemsUsed += 1
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function addLoopFromKey(strElemKey As String, ByRef strElements() As String, ByRef NextIndex As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim tracker As EDIItemTracker
        If Not Me.Loops Is Nothing AndAlso Me.Loops.ContainsKey("Loop310") Then
            tracker = Me.Loops("Loop310")
            If Not tracker Is Nothing Then
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Dim oEDILoop310 As New clsEDI204InLoop310()
                    If Not oEDILoop310.Elements Is Nothing AndAlso oEDILoop310.Elements.ContainsKey(strElemKey) Then
                        If Loop310 Is Nothing Then Loop310 = New List(Of clsEDI204InLoop310)
                        oEDILoop310 = Me.insertElements(New clsEDI204InLoop310(), strElements, NextIndex)
                        If Not oEDILoop310 Is Nothing Then
                            Loop310.Add(oEDILoop310)
                            tracker.ItemsUsed += 1
                        End If
                        'if the key exists for the loop object we return true even if the insert elements returns nothing
                        'this allows us to move on to the next element
                        Return True
                    End If
                End If
            End If
        End If

        If Me.Loops.ContainsKey("Loop320") Then
            tracker = Me.Loops("Loop320")
            If Not tracker Is Nothing Then
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Dim oEDILoop320 As New clsEDI204Loop320()
                    If oEDILoop320.Elements.ContainsKey(strElemKey) Then
                        If Loop320 Is Nothing Then Loop320 = New List(Of clsEDI204Loop320)
                        oEDILoop320 = Me.insertElements(New clsEDI204Loop320(), strElements, NextIndex)
                        If Not oEDILoop320 Is Nothing Then
                            Loop320.Add(oEDILoop320)
                            tracker.ItemsUsed += 1
                        End If
                        'if the key exists for the loop object we return true even if the insert elements returns nothing
                        'this allows us to move on to the next element
                        Return True
                    End If
                End If
            End If
        End If

        If Me.Loops.ContainsKey("Loop350") Then
            tracker = Me.Loops("Loop350")
            If Not tracker Is Nothing Then
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Dim oEDILoop350 As New clsEDI204InLoop350()
                    If oEDILoop350.Elements.ContainsKey(strElemKey) Then
                        If Loop350 Is Nothing Then Loop350 = New List(Of clsEDI204InLoop350)
                        oEDILoop350 = Me.insertElements(New clsEDI204InLoop350(), strElements, NextIndex)
                        If Not oEDILoop350 Is Nothing Then
                            Loop350.Add(oEDILoop350)
                            tracker.ItemsUsed += 1
                        End If
                        'if the key exists for the loop object we return true even if the insert elements returns nothing
                        'this allows us to move on to the next element
                        Return True
                    End If
                End If
            End If
        End If
        Return blnRet
    End Function

    Protected Overrides Sub populateElements()
        Me.Name = "204 Loop 300"
        Me.Elements = New Dictionary(Of String, EDIItemTracker)
        Me.Loops = New Dictionary(Of String, EDIItemTracker)

        Me.Elements.Add("S5", New EDIItemTracker(1))
        Me.Elements.Add("L11", New EDIItemTracker(50))
        Me.Elements.Add("G62", New EDIItemTracker(2))
        Me.Elements.Add("AT5", New EDIItemTracker(6))
        Me.Elements.Add("PLD", New EDIItemTracker(1))
        Me.Elements.Add("NTE", New EDIItemTracker(20))
        Me.Loops.Add("Loop310", New EDIItemTracker(1))
        Me.Loops.Add("Loop320", New EDIItemTracker(99))
        Me.Loops.Add("Loop350", New EDIItemTracker(99))

    End Sub
End Class

#End Region

#Region "204 In Loop 310 (Nested Inside 204In 300)"

''' <summary>
''' EDI Inbound 204 310 Loop Stop Name Information 
''' </summary>
''' <remarks>
''' Added by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
''' Modified by RHR for v-7.0.6.105 on 5/5/2017
'''   added new constructors
'''   added new parser
'''   changed N3 to a list of clsEDIN3
''' </remarks>
Public Class clsEDI204InLoop310
    Inherits EDIObjBase

    Public N1 As New clsEDIN1

    Public N3s As New List(Of clsEDIN3)
    ''' <summary>
    ''' First N3 item in List
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 6/3/2017
    '''   added logic to create a new item in the list if it does not exist on Set
    ''' </remarks>
    Public Property N3() As clsEDIN3
        Get
            If Not N3s Is Nothing AndAlso N3s.Count() > 0 Then
                Return N3s(0)
            Else
                Return New clsEDIN3()
            End If
        End Get
        Set(ByVal value As clsEDIN3)
            If N3s Is Nothing Then N3s = New List(Of clsEDIN3)
            If N3s.Count > 0 Then
                N3s(0) = value
            Else
                N3s.Add(value)
            End If
        End Set
    End Property
    Public N4 As New clsEDIN4
    Public G61s As New List(Of clsEDIG61)

    ''' <summary>
    ''' First G61 item in List
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 6/3/2017
    '''   added logic to create a new item in the list if it does not exist on Set
    ''' </remarks>
    Public Property G61() As clsEDIG61
        Get
            If Not G61s Is Nothing AndAlso G61s.Count() > 0 Then
                Return G61s(0)
            Else
                Return New clsEDIG61()
            End If
        End Get
        Set(ByVal value As clsEDIG61)
            If G61s Is Nothing Then G61s = New List(Of clsEDIG61)
            If G61s.Count > 0 Then
                G61s(0) = value
            Else
                G61s.Add(value)
            End If
        End Set
    End Property

    Sub New()
        MyBase.New()
        populateElements()
    End Sub

    Sub New(ByVal oN1 As clsEDIN1, ByVal oN3s As List(Of clsEDIN3), ByVal oN4 As clsEDIN4, ByVal oG61s As List(Of clsEDIG61))
        MyBase.New()
        populateElements()
        Me.N1 = oN1
        Me.N3s = oN3s
        Me.N4 = oN4
        Me.G61s = oG61s

    End Sub

    Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        populateElements()
        addDataFromString(strSource, strSegSep)
    End Sub

    ''' <summary>
    ''' add EDI Data to class From String using the provided seperator.
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    ''' <remarks>
    ''' Created for Backward compatibility by RHR for v-7.0.6.105 on 5/5/2017
    '''   New implementations should use the insertElements method
    ''' </remarks>
    Public Overrides Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)

        Dim sElements = strSource.Split(strSegSep)
        Dim strKey As String = ""
        If Not sElements Is Nothing AndAlso sElements.Count() > 0 Then
            For Each e In sElements
                If getKey(e, strKey) Then If Me.usesElement(strKey) Then addSegment(e, strKey)
            Next
        End If
    End Sub



    '''' <summary>
    '''' addDataFromString
    '''' </summary>
    '''' <param name="strSource"></param>
    '''' <param name="strSegSep"></param>
    '''' <remarks>
    '''' Added by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
    '''' Removed by RHR new method manages missing elements better
    '''' </remarks>
    'Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
    '    'the N1, N3, and N4 are requred but the G61 is not.
    '    For isubsegs As Integer = 3 To 0 Step -1
    '        Dim segs() As String
    '        Select Case isubsegs
    '            Case 0
    '                'split out any unwanted elements 
    '                Dim sElems() As String = strSource.Split(strSegSep)
    '                'read the N1 record (it should be all that is left 
    '                If Left(sElems(0), 3) <> "N1*" Then sElems(0) = "N1*" & sElems(0)
    '                N1 = New clsEDIN1(sElems(0))
    '            Case 1
    '                'read the N3 record
    '                segs = Regex.Split(strSource, "\" & strSegSep & "N3\*")
    '                If segs.Length > 1 Then
    '                    'split out any unwanted elements 
    '                    Dim sElems() As String = segs(1).Split(strSegSep)
    '                    N3 = New clsEDIN3("N3*" & sElems(0))
    '                    strSource = segs(0)
    '                End If
    '            Case 2
    '                'read the N4 record
    '                segs = Regex.Split(strSource, "\" & strSegSep & "N4\*")
    '                If segs.Length > 1 Then
    '                    'split out any unwanted elements 
    '                    Dim sElems() As String = segs(1).Split(strSegSep)
    '                    N4 = New clsEDIN4("N4*" & sElems(0))
    '                    strSource = segs(0)
    '                End If
    '            Case 3
    '                'read the G61 record
    '                segs = Regex.Split(strSource, "\" & strSegSep & "G61\*")
    '                If segs.Length > 1 Then
    '                    Dim oG61List As New List(Of clsEDIG61)
    '                    For i = 1 To segs.Length - 1
    '                        'split out any unwanted elements
    '                        Dim sElems() As String = segs(i).Split(strSegSep)
    '                        Dim g61 = New clsEDIG61("G61*" & sElems(0))
    '                        oG61List.Add(g61)
    '                    Next
    '                    strSource = segs(0)
    '                    G61s = oG61List
    '                End If

    '        End Select
    '    Next

    'End Sub



    Public Overrides Function addSegment(strSource As String, Optional strKey As String = "") As Boolean
        If String.IsNullOrWhiteSpace(strKey) Then
            If Not getKey(strSource, strKey) Then
                'nothing to do just return
                Return False
            End If
        End If
        If Not Me.usesElement(strKey) Then Return False
        If Not Me.Elements.ContainsKey(strKey) Then Return False
        Dim tracker As EDIItemTracker = Me.Elements(strKey)
        If tracker.ItemsUsed < tracker.ItemsAllowed Then
            Select Case strKey
                Case "N1"
                    Me.N1 = New clsEDIN1(strSource)
                Case "N3"
                    If N3s Is Nothing Then N3s = New List(Of clsEDIN3)
                    N3s.Add(New clsEDIN3(strSource))
                Case "N4"
                    Me.N4 = New clsEDIN4(strSource)
                Case "G61"
                    If G61s Is Nothing Then G61s = New List(Of clsEDIG61)
                    G61s.Add(New clsEDIG61(strSource))
                Case Else
                    Return False
            End Select
            tracker.ItemsUsed += 1
            Return True
        Else
            Return False
        End If

    End Function

    Public Overrides Function addLoopFromKey(strElemKey As String, ByRef strElements() As String, ByRef NextIndex As Integer) As Boolean
        Return False 'no nested loops
    End Function

    Protected Overrides Sub populateElements()
        Me.Name = "204 Loop 310"
        Me.Elements = New Dictionary(Of String, EDIItemTracker)
        Me.Loops = New Dictionary(Of String, EDIItemTracker)
        Me.Elements.Add("N1", New EDIItemTracker(1))
        Me.Elements.Add("N3", New EDIItemTracker(2))
        Me.Elements.Add("N4", New EDIItemTracker(1))
        Me.Elements.Add("G61", New EDIItemTracker(3))
    End Sub
End Class

#End Region


#Region "204 In Loop 350 (Nested Inside 204 In 300 Loop)"

''' <summary>
''' EDI 204 Inbound 350 Loop Order Identification Detail Loop 
''' </summary>
''' <remarks>
''' Created by RHR for v-7.0.6.105 on 5/5/2017
''' </remarks>
Public Class clsEDI204InLoop350
    Inherits EDIObjBase

    Public OID As New clsEDIOID
    Public LADs As New List(Of clsEDILAD)
    Public Loop360 As New List(Of clsEDI204InLoop360)

    Sub New()
        MyBase.New()
        populateElements()
    End Sub

    Sub New(ByVal oOID As clsEDIOID, ByVal oLADs As List(Of clsEDILAD))
        MyBase.New()
        populateElements()
        Me.OID = oOID
        Me.LADs = oLADs
    End Sub

    Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        populateElements()
        addDataFromString(strSource, strSegSep)
    End Sub


    ''' <summary>
    ''' add EDI Data to class From String using the provided seperator.
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    ''' <remarks>
    ''' Created for Backward compatibility by RHR for v-7.0.6.105 on 5/5/2017
    '''   New implementations should use the insertElements method
    ''' </remarks>
    Public Overrides Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)

        Loop360 = New List(Of clsEDI204InLoop360)
        'Split off any loop360 segments if they exist (by L5)
        Dim s360s() As String = Regex.Split(strSource, "\" & strSegSep & "L5\*")
        If s360s.Length > 1 Then
            strSource = s360s(0)
            'There are 360s so process them
            'the first item of s360s() contains the rest of the Loop300 data so skip it
            For i360 = 1 To s360s.Length - 1
                'process all the 360s and add them to the list
                Dim o360 As New clsEDI204InLoop360
                o360.addDataFromString(s360s(i360), strSegSep)
                Loop360.Add(o360)
            Next
        End If
        Dim sElements = strSource.Split(strSegSep)
        Dim strKey As String = ""
        If Not sElements Is Nothing AndAlso sElements.Count() > 0 Then
            For Each e In sElements
                If getKey(e, strKey) Then If Me.usesElement(strKey) Then addSegment(e, strKey)
            Next
        End If
    End Sub



    Public Overrides Function addSegment(strSource As String, Optional strKey As String = "") As Boolean
        If String.IsNullOrWhiteSpace(strKey) Then
            If Not getKey(strSource, strKey) Then
                'nothing to do just return
                Return False
            End If
        End If
        If Not Me.usesElement(strKey) Then Return False
        If Not Me.Elements.ContainsKey(strKey) Then Return False
        Dim tracker As EDIItemTracker = Me.Elements(strKey)
        If tracker.ItemsUsed < tracker.ItemsAllowed Then
            Select Case strKey
                Case "OID"
                    Me.OID = New clsEDIOID(strSource)
                Case "LAD"
                    If LADs Is Nothing Then LADs = New List(Of clsEDILAD)
                    LADs.Add(New clsEDILAD(strSource))
                Case Else
                    Return False
            End Select
            tracker.ItemsUsed += 1
            Return True
        Else
            Return False
        End If
    End Function


    Public Overrides Function addLoopFromKey(strElemKey As String, ByRef strElements() As String, ByRef NextIndex As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim tracker As EDIItemTracker
        If Not Me.Loops Is Nothing AndAlso Me.Loops.ContainsKey("Loop360") Then
            tracker = Me.Loops("Loop360")
            If Not tracker Is Nothing Then
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Dim oEDILoop360 As New clsEDI204InLoop360()
                    If Not oEDILoop360.Elements Is Nothing AndAlso oEDILoop360.Elements.ContainsKey(strElemKey) Then
                        If Loop360 Is Nothing Then Loop360 = New List(Of clsEDI204InLoop360)
                        oEDILoop360 = Me.insertElements(New clsEDI204InLoop360(), strElements, NextIndex)
                        If Not oEDILoop360 Is Nothing Then
                            Loop360.Add(oEDILoop360)
                            tracker.ItemsUsed += 1
                        End If
                        'if the key exists for the loop object we return true even if the insert elements returns nothing
                        'this allows us to move on to the next element
                        Return True
                    End If
                End If
            End If
        End If
        Return blnRet
    End Function

    Protected Overrides Sub populateElements()
        Me.Name = "204 Loop 350"
        Me.Elements = New Dictionary(Of String, EDIItemTracker)
        Me.Loops = New Dictionary(Of String, EDIItemTracker)

        Me.Elements.Add("OID", New EDIItemTracker(1))
        Me.Elements.Add("LAD", New EDIItemTracker(1))
        Me.Loops.Add("Loop360", New EDIItemTracker(99))

    End Sub
End Class

#End Region

#Region "204 In Loop 360 (Nested Inside 204 In 350 Loop)"

''' <summary>
''' EDI 204 Inbound 360 Loop Description, Marks and Numbers
''' </summary>
''' <remarks>
''' Created by RHR for v-7.0.6.105 on 5/5/2017
''' </remarks>
Public Class clsEDI204InLoop360
    Inherits EDIObjBase

    Public L5 As New clsEDIL5
    Public AT8 As New clsEDIAT8
    Public Loop365 As New List(Of clsEDI204InLoop365)

    Sub New()
        MyBase.New()
        populateElements()
    End Sub

    Sub New(ByVal oL5 As clsEDIL5, ByVal oAT8 As clsEDIAT8)
        MyBase.New()
        populateElements()
        Me.L5 = oL5
        Me.AT8 = oAT8
    End Sub

    Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        populateElements()
        addDataFromString(strSource, strSegSep)
    End Sub

    ''' <summary>
    ''' add EDI Data to class From String using the provided seperator.
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    ''' <remarks>
    ''' Created for Backward compatibility by RHR for v-7.0.6.105 on 5/5/2017
    '''   New implementations should use the insertElements method
    ''' </remarks>
    Public Overrides Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)

        Loop365 = New List(Of clsEDI204InLoop365)
        'Split off any loop360 segments if they exist (by L5)
        Dim s365s() As String = Regex.Split(strSource, "\" & strSegSep & "G61\*")
        If s365s.Length > 1 Then
            strSource = s365s(0)
            'There are 360s so process them
            'the first item of s360s() contains the rest of the Loop300 data so skip it
            For i365 = 1 To s365s.Length - 1
                'process all the 360s and add them to the list
                Dim o365 As New clsEDI204InLoop365
                o365.addDataFromString(s365s(i365), strSegSep)
                Loop365.Add(o365)
            Next
        End If
        Dim sElements = strSource.Split(strSegSep)
        Dim strKey As String = ""
        If Not sElements Is Nothing AndAlso sElements.Count() > 0 Then
            For Each e In sElements
                If getKey(e, strKey) Then If Me.usesElement(strKey) Then addSegment(e, strKey)
            Next
        End If

    End Sub


    Public Overrides Function addSegment(ByVal strSource As String, Optional ByVal strKey As String = "") As Boolean

        If String.IsNullOrWhiteSpace(strKey) Then
            If Not getKey(strSource, strKey) Then
                'nothing to do just return
                Return False
            End If
        End If
        Dim tracker As EDIItemTracker
        Select Case strKey
            Case "L5"
                tracker = Me.Elements(strKey)
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Me.L5 = New clsEDIL5(strSource)
                    tracker.ItemsUsed += 1
                    Return True
                End If
            Case "AT8"
                tracker = Me.Elements(strKey)
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Me.AT8 = New clsEDIAT8(strSource)
                    tracker.ItemsUsed += 1
                    Return True
                End If

            Case Else
                Return False
        End Select

    End Function


    Public Overrides Function addLoopFromKey(ByVal strElemKey As String, ByRef strElements As String(), ByRef NextIndex As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim tracker As EDIItemTracker
        If Not Me.Loops Is Nothing AndAlso Me.Loops.ContainsKey("Loop365") Then
            tracker = Me.Loops("Loop365")
            If Not tracker Is Nothing Then
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Dim oEDILoop365 As New clsEDI204InLoop365()
                    If Not oEDILoop365.Elements Is Nothing AndAlso oEDILoop365.Elements.ContainsKey(strElemKey) Then
                        If Loop365 Is Nothing Then Loop365 = New List(Of clsEDI204InLoop365)
                        oEDILoop365 = Me.insertElements(New clsEDI204InLoop365(), strElements, NextIndex)
                        If Not oEDILoop365 Is Nothing Then
                            Loop365.Add(oEDILoop365)
                            tracker.ItemsUsed += 1
                        End If
                        'if key is LH1 we return true even if the insert elements returns nothing
                        'this allows us to move on to the next element
                        blnRet = True
                    End If
                End If
            End If
        End If

        Return blnRet
    End Function

    Protected Overrides Sub populateElements()
        Me.Name = "204 Loop 360"
        Me.Elements = New Dictionary(Of String, EDIItemTracker)
        Me.Loops = New Dictionary(Of String, EDIItemTracker)
        Me.Elements.Add("L5", New EDIItemTracker(1))
        Me.Elements.Add("AT8", New EDIItemTracker(1))
        Me.Loops.Add("Loop365", New EDIItemTracker(99))
    End Sub

End Class

#End Region

#Region "204 In Loop 365 (Nested Inside 204 In 360 Loop)"

''' <summary>
''' EDI 204 Inbound 365 Loop Hazard Material Contact Information
''' </summary>
''' <remarks>
''' Created by RHR for v-7.0.6.105 on 5/5/2017
''' </remarks>
Public Class clsEDI204InLoop365
    Inherits EDIObjBase

    Public G61 As New clsEDIG61
    Public Loop370 As New List(Of clsEDI204InLoop370)

    Sub New()
        MyBase.New()
        populateElements()
    End Sub

    Sub New(ByVal oG61 As clsEDIG61, ByVal oLoop370s As List(Of clsEDI204InLoop370))
        MyBase.New()
        populateElements()
        Me.G61 = oG61
        Me.Loop370 = oLoop370s
    End Sub

    Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        populateElements()
        addDataFromString(strSource, strSegSep)
    End Sub


    ''' <summary>
    ''' add EDI Data to class From String using the provided seperator.
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    ''' <remarks>
    ''' Created for Backward compatibility by RHR for v-7.0.6.105 on 5/5/2017
    '''   New implementations should use the insertElements method
    ''' </remarks>
    Public Overrides Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        'ByVal oG61 As clsEDIG61, ByVal oLoop370s As List(Of clsEDI204InLoop370)
        Dim sElements = strSource.Split(strSegSep)
        Dim strKey As String = ""
        'Dim oLoop370s As List(Of clsEDI204InLoop370)
        If Not sElements Is Nothing AndAlso sElements.Count() > 0 Then
            For Each e In sElements
                If getKey(e, strKey) Then
                    If strKey = "G61" Then
                        addSegment(e, strKey)
                    ElseIf strKey = "LH1" Then 'this only works  when the loop contains a single element
                        Dim tracker As EDIItemTracker = Me.Loops("Loop370")
                        If Loop370 Is Nothing Then
                            Loop370 = New List(Of clsEDI204InLoop370)
                        Else

                            If Not tracker Is Nothing Then
                                If tracker.ItemsAllowed >= tracker.ItemsUsed Then
                                    Continue For
                                End If
                            End If
                            'create a new object
                            Dim oEDILH1 As New clsEDILH1(e)
                            Loop370.Add(New clsEDI204InLoop370(oEDILH1))
                            tracker.ItemsUsed += 1
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Public Overrides Function addSegment(ByVal strSource As String, Optional ByVal strKey As String = "") As Boolean

        If String.IsNullOrWhiteSpace(strKey) Then
            If Not getKey(strSource, strKey) Then
                'nothing to do just return
                Return False
            End If
        End If
        Dim tracker As EDIItemTracker
        Select Case strKey
            Case "G61"
                tracker = Me.Elements(strKey)
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Me.G61 = New clsEDIG61(strSource)
                    tracker.ItemsUsed += 1
                    Return True
                End If
            Case Else
                Return False
        End Select

    End Function

    Protected Overrides Sub populateElements()
        Me.Name = "204 Loop 365"
        Me.Elements = New Dictionary(Of String, EDIItemTracker)
        Me.Loops = New Dictionary(Of String, EDIItemTracker)
        Me.Elements.Add("G61", New EDIItemTracker(1))
        Me.Loops.Add("Loop370", New EDIItemTracker(25))
    End Sub


    Public Overrides Function addLoopFromKey(ByVal strElemKey As String, ByRef strElements As String(), ByRef NextIndex As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim tracker As EDIItemTracker
        If Not Me.Loops Is Nothing AndAlso Me.Loops.ContainsKey("Loop370") Then
            tracker = Me.Loops("Loop370")
            If Not tracker Is Nothing Then
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Dim oEDILoop370 As New clsEDI204InLoop370()
                    If Not oEDILoop370.Elements Is Nothing AndAlso oEDILoop370.Elements.ContainsKey(strElemKey) Then
                        If Loop370 Is Nothing Then Loop370 = New List(Of clsEDI204InLoop370)
                        oEDILoop370 = Me.insertElements(New clsEDI204InLoop370(), strElements, NextIndex)
                        If Not oEDILoop370 Is Nothing Then
                            Loop370.Add(oEDILoop370)
                            tracker.ItemsUsed += 1
                        End If
                        'if key is LH1 we return true even if the insert elements returns nothing
                        'this allows us to move on to the next element
                        blnRet = True
                    End If
                End If
            End If
        End If
        Return blnRet
    End Function

End Class

#End Region

#Region "204 In Loop 370 (Nested Inside 204 In 365 Loop)"

''' <summary>
''' EDI 204 Inbound 370 Loop Hazardous Identification Information
''' </summary>
''' <remarks>
''' Created by RHR for v-7.0.6.105 on 5/5/2017
''' </remarks>
Public Class clsEDI204InLoop370
    Inherits EDIObjBase

    Public LH1 As New clsEDILH1

    Sub New()
        MyBase.New()
        populateElements()
    End Sub

    Sub New(ByVal oLH1 As clsEDILH1)
        MyBase.New()
        populateElements()
        Me.LH1 = oLH1
    End Sub

    Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        populateElements()
        addDataFromString(strSource, strSegSep)
    End Sub

    ''' <summary>
    ''' add EDI Data to class From String using the provided seperator.
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/5/2017
    ''' </remarks>
    Public Overrides Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)

        Dim sElements = strSource.Split(strSegSep)
        Dim strKey As String = ""
        If Not sElements Is Nothing AndAlso sElements.Count() > 0 Then
            For Each e In sElements
                If getKey(e, strKey) Then
                    If strKey = "LH1" Then
                        addSegment(e, strKey)
                    End If
                End If
            Next
        End If
    End Sub


    Public Overrides Function addSegment(ByVal strSource As String, Optional ByVal strKey As String = "") As Boolean

        If String.IsNullOrWhiteSpace(strKey) Then
            If Not getKey(strSource, strKey) Then
                'nothing to do just return
                Return False
            End If
        End If
        Dim tracker As EDIItemTracker
        Select Case strKey
            Case "LH1"
                tracker = Me.Elements(strKey)
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Me.LH1 = New clsEDILH1(strSource)
                    tracker.ItemsUsed += 1
                    Return True
                End If
            Case Else
                Return False
        End Select
    End Function

    Protected Overrides Sub populateElements()
        Me.Name = "204 Loop 370"
        Me.Elements = New Dictionary(Of String, EDIItemTracker)
        Me.Loops = New Dictionary(Of String, EDIItemTracker)
        Me.Elements.Add("LH1", New EDIItemTracker(1))
    End Sub

    Public Overrides Function addLoopFromKey(ByVal strElemKey As String, ByRef strElements As String(), ByRef NextIndex As Integer) As Boolean
        Return False 'no loops in 370 Loop
    End Function



End Class

#End Region
#Region " 204 Inbound - Full Truck Load (Single Stop Load)"
''' <summary>
''' EDI 204 Inbound Processing Class
''' </summary>
''' <remarks>
''' Created by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
''' Modified by RHR for v-7.0.6.105 on 6/13/2017 
''' </remarks>
Public Class clsEDI204In
    Inherits EDIObjBase


    'Added by LVV On 5/1/17 For v-7.0.6.105 EDI 204In
    Public ST As New clsEDIST
    Public B2 As New clsEDIB2
    Public B2A As New clsEDIB2A
    Public L11s As New List(Of clsEDIL11)
    Public G62 As New clsEDIG62
    Public PLD As New clsEDIPLD
    Public NTEs As New List(Of clsEDINTE)
    Public Loop100 As New List(Of clsEDI204Loop100)
    Public Loop200 As New List(Of clsEDI204Loop200)
    Public Loop300 As List(Of clsEDI204InLoop300) 'in the current version we only use the first 300 loop but we could have multiple in the future specs support up to 999 300 loops
    Public L3 As New clsEDIL3
    Public SE As New clsEDISE
    Public SOs As New List(Of String)
    Public POs As New List(Of String)
    Public LastPickSequence As Integer = 0
    Public LastDropSequence As Integer = 0
    Public Pickups As New List(Of clsAddressInfo)
    Public DropOffs As New List(Of clsAddressInfo)
    Public intVersion As Integer = 6
    Public blnInbound As Boolean = False

    Private _EDI204InSetting As clsEDI204InSetting
    Public Property EDI204InSetting() As clsEDI204InSetting
        Get
            Return _EDI204InSetting
        End Get
        Set(ByVal value As clsEDI204InSetting)
            _EDI204InSetting = value
        End Set
    End Property

    Public ReadOnly Property OutboundCompNumber() As Integer
        Get
            Dim intCompNumber As Integer = 0
            Integer.TryParse(Me.EDI204InSetting.MappingRules.OutboundCompNumber, intCompNumber)
            Return intCompNumber
        End Get
    End Property

    Public ReadOnly Property InboundCompNumber() As Integer
        Get
            Dim intCompNumber As Integer = 0
            Integer.TryParse(Me.EDI204InSetting.MappingRules.InboundCompNumber, intCompNumber)
            Return intCompNumber
        End Get
    End Property


    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
        MyBase.New(config)
        populateElements()
    End Sub

    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration, ByVal oST As clsEDIST, ByVal oB2 As clsEDIB2, ByVal oB2A As clsEDIB2A, ByVal oL11s As List(Of clsEDIL11), ByVal oG62 As clsEDIG62, ByVal oPLD As clsEDIPLD, ByVal oNTEs As List(Of clsEDINTE), ByVal oL3 As clsEDIL3, ByVal oSE As clsEDISE)
        MyBase.New(config)
        populateElements()
        Me.ST = oST
        Me.B2 = oB2
        Me.B2A = oB2A
        Me.L11s = oL11s
        Me.G62 = oG62
        Me.PLD = oPLD
        Me.NTEs = oNTEs
        Me.L3 = oL3
        Me.SE = oSE
    End Sub

    'Sub New(ByVal strSource As String, ByVal strSegSep As String)
    '    MyBase.New()
    '    populateElements()
    '    addDataFromString(strSource, strSegSep)
    'End Sub

    Public Overrides Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        Throw New NotImplementedException()
    End Sub

    ''' <summary>
    ''' addDataFromString
    ''' </summary>
    ''' <param name="strSource"></param>
    ''' <param name="strSegSep"></param>
    ''' <param name="strSEElem"></param>
    ''' <remarks>
    ''' Added by LVV on 5/2/17 for v-7.0.6.105 EDI 204In
    ''' </remarks>
    Public Overloads Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
        'The first item in the s100Ls list contains the B2, B2A, L11s, NTEs, and PLD data.  We use this to create the 204 object
        'the ST, B2, B2A, and L11s, NTEs, and PLD elements are all requred.
        For isubsegs As Integer = 5 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the ST record (it should be all that is left 
                    If Left(sElems(0), 3) <> "ST*" Then sElems(0) = "ST*" & sElems(0)
                    ST = New clsEDIST(sElems(0))
                Case 1
                    'read the B2 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "B2\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        B2 = New clsEDIB2("B2*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 2
                    'read the B2A record
                    segs = Regex.Split(strSource, "\" & strSegSep & "B2A\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        B2A = New clsEDIB2A("B2A*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 3
                    'read the L11 record(s)
                    segs = Regex.Split(strSource, "\" & strSegSep & "L11\*")
                    If segs.Length > 1 Then
                        Dim oL11List As New List(Of clsEDIL11)
                        For i = 1 To segs.Length - 1
                            'split out any unwanted elements
                            Dim sElems() As String = segs(i).Split(strSegSep)
                            Dim l11 = New clsEDIL11("L11*" & sElems(0))
                            oL11List.Add(l11)
                        Next
                        strSource = segs(0)
                        L11s = oL11List
                    End If
                Case 4
                    'read the NTE record(s)
                    segs = Regex.Split(strSource, "\" & strSegSep & "NTE\*")
                    If segs.Length > 1 Then
                        Dim oNTEList As New List(Of clsEDINTE)
                        For i = 1 To segs.Length - 1
                            'split out any unwanted elements
                            Dim sElems() As String = segs(i).Split(strSegSep)
                            Dim nte = New clsEDINTE("NTE*" & sElems(0))
                            oNTEList.Add(nte)
                        Next
                        strSource = segs(0)
                        NTEs = oNTEList
                    End If
                Case 5
                    'read the PLD record
                    segs = Regex.Split(strSource, "\" & strSegSep & "PLD\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        PLD = New clsEDIPLD("PLD*" & sElems(0))
                        strSource = segs(0)
                    End If

            End Select
        Next
        'Add the SE
        If Not String.IsNullOrEmpty(strSEElem) Then
            'split out any unwanted elements 
            Dim sElems() As String = strSEElem.Split(strSegSep)
            If Left(sElems(0), 3) <> "SE*" Then sElems(0) = "SE*" & sElems(0)
            SE = New clsEDISE(sElems(0))
        End If
    End Sub

    ''' <summary>
    ''' Creates an EDI 204In document string
    ''' </summary>
    ''' <param name="strISA"></param>
    ''' <param name="strGS"></param>
    ''' <param name="strIEA"></param>
    ''' <param name="strGE"></param>
    ''' <param name="SegmentTerminator"></param>
    ''' <returns>String</returns>
    ''' <remarks>
    ''' Added by LVV on 5/2/17 for v-7.0.6.105 EDI 204In
    ''' </remarks>
    Public Function getEDIRecord(ByVal strISA As String, ByVal strGS As String, ByVal strIEA As String, ByVal strGE As String, ByVal SegmentTerminator As String) As String
        'Format the edi record
        Dim strToPrint As String = ""
        strToPrint &= strISA
        strToPrint &= strGS
        strToPrint &= ST.getEDIString(SegmentTerminator)
        strToPrint &= B2.getEDIString(SegmentTerminator)
        strToPrint &= B2A.getEDIString(SegmentTerminator)
        If Not L11s Is Nothing Then
            For Each L As clsEDIL11 In L11s
                strToPrint &= L.getEDIString(SegmentTerminator)
                strToPrint &= L.getEDIString(SegmentTerminator)
                strToPrint &= L.getEDIString(SegmentTerminator)
                strToPrint &= L.getEDIString(SegmentTerminator)
            Next
        End If
        If Not NTEs Is Nothing Then
            For Each N As clsEDINTE In NTEs
                strToPrint &= N.getEDIString(SegmentTerminator)
                strToPrint &= N.getEDIString(SegmentTerminator)
                strToPrint &= N.getEDIString(SegmentTerminator)
                strToPrint &= N.getEDIString(SegmentTerminator)
            Next
        End If
        strToPrint &= PLD.getEDIString(SegmentTerminator)
        If Not Loop100 Is Nothing Then
            For Each L As clsEDI204Loop100 In Loop100
                strToPrint &= L.N1.getEDIString(SegmentTerminator)
                strToPrint &= L.N3.getEDIString(SegmentTerminator)
                strToPrint &= L.N4.getEDIString(SegmentTerminator)
                strToPrint &= L.G61.getEDIString(SegmentTerminator)
            Next
        End If
        If Not Loop200 Is Nothing Then
            For Each L As clsEDI204Loop200 In Loop200
                strToPrint &= L.N7.getEDIString(SegmentTerminator)
            Next
        End If
        If Not Loop300 Is Nothing Then
            For Each L As clsEDI204InLoop300 In Loop300
                strToPrint &= L.S5.getEDIString(SegmentTerminator)
                If Not L.L11s Is Nothing Then
                    For Each L11 As clsEDIL11 In L.L11s
                        strToPrint &= L11.getEDIString(SegmentTerminator)
                    Next
                End If
                If Not L.G62s Is Nothing Then
                    For Each G62 As clsEDIG62 In L.G62s
                        strToPrint &= G62.getEDIString(SegmentTerminator)
                    Next
                End If
                strToPrint &= L.PLD.getEDIString(SegmentTerminator)
                If Not L.NTEs Is Nothing Then
                    For Each NTE As clsEDINTE In L.NTEs
                        strToPrint &= NTE.getEDIString(SegmentTerminator)
                    Next
                End If
                If Not L.Loop310 Is Nothing Then
                    For Each L310 As clsEDI204InLoop310 In L.Loop310
                        strToPrint &= L310.N1.getEDIString(SegmentTerminator)
                        If Not L310.N3s Is Nothing AndAlso L310.N3s.Count() > 0 Then
                            For Each N3 In L310.N3s
                                strToPrint &= N3.getEDIString(SegmentTerminator)
                            Next
                        End If
                        strToPrint &= L310.N4.getEDIString(SegmentTerminator)
                        If Not L310.G61s Is Nothing Then
                            For Each G61 As clsEDIG61 In L310.G61s
                                strToPrint &= G61.getEDIString(SegmentTerminator)
                            Next
                        End If
                    Next
                End If
                If Not L.Loop320 Is Nothing Then
                    For Each L320 As clsEDI204Loop320 In L.Loop320
                        strToPrint &= L320.L5.getEDIString(SegmentTerminator)
                        strToPrint &= L320.AT8.getEDIString(SegmentTerminator)
                    Next
                End If
            Next
        End If
        strToPrint &= L3.getEDIString(SegmentTerminator)
        strToPrint &= SE.getEDIString(SegmentTerminator)
        strToPrint &= strGE
        strToPrint &= strIEA

        Return strToPrint

    End Function

    ''' <summary>
    ''' Configure Inbound vs Outbound using the BL Number
    ''' </summary>
    ''' <param name="BLNumber"></param>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 - 6.0.4.7 on 6/13/2017
    ''' </remarks>
    Public Sub updateInboundUsingDefaultSettings(ByVal BLNumber As String)


        If Not String.IsNullOrWhiteSpace(Me.EDI204InSetting.MappingRules.InboundPrefixKeys) Then
            Dim strKeys As New List(Of String)
            If Me.EDI204InSetting.MappingRules.InboundPrefixKeys.Contains("|") Then
                strKeys = Me.EDI204InSetting.MappingRules.InboundPrefixKeys.Split("|").ToList()
            Else
                strKeys.Add(Me.EDI204InSetting.MappingRules.InboundPrefixKeys)
            End If
            For Each key In strKeys
                If BLNumber.StartsWith(key, StringComparison.OrdinalIgnoreCase) Then
                    Me.blnInbound = True
                    Exit For
                End If
            Next
        End If

        Return
    End Sub



    ''' <summary>
    ''' Creates an EDI 204In document string FOR TESTING
    ''' </summary>
    ''' <param name="oISA"></param>
    ''' <param name="oGS"></param>
    ''' <param name="oIEA"></param>
    ''' <param name="oGE"></param>
    ''' <param name="SegmentTerminator"></param>
    ''' <returns>String</returns>
    ''' <remarks>
    ''' Added by LVV on 5/2/17 for v-7.0.6.105 EDI 204In
    ''' </remarks>
    Public Function getEDIRecord(ByVal oISA As clsEDIISA, ByVal oGS As clsEDIGS, ByVal oIEA As clsEDIIEA, ByVal oGE As clsEDIGE, ByVal SegmentTerminator As String) As String
        'Format the edi record
        Dim strToPrint As String = ""
        strToPrint &= oISA.getEDIString(SegmentTerminator)
        strToPrint &= oGS.getEDIString(SegmentTerminator)
        strToPrint &= ST.getEDIString(SegmentTerminator)
        strToPrint &= B2.getEDIString(SegmentTerminator)
        strToPrint &= B2A.getEDIString(SegmentTerminator)
        If Not L11s Is Nothing Then
            For Each L As clsEDIL11 In L11s
                strToPrint &= L.getEDIString(SegmentTerminator)
                strToPrint &= L.getEDIString(SegmentTerminator)
                strToPrint &= L.getEDIString(SegmentTerminator)
                strToPrint &= L.getEDIString(SegmentTerminator)
            Next
        End If
        If Not NTEs Is Nothing Then
            For Each N As clsEDINTE In NTEs
                strToPrint &= N.getEDIString(SegmentTerminator)
                strToPrint &= N.getEDIString(SegmentTerminator)
                strToPrint &= N.getEDIString(SegmentTerminator)
                strToPrint &= N.getEDIString(SegmentTerminator)
            Next
        End If
        strToPrint &= PLD.getEDIString(SegmentTerminator)
        If Not Loop100 Is Nothing Then
            For Each L As clsEDI204Loop100 In Loop100
                strToPrint &= L.N1.getEDIString(SegmentTerminator)
                strToPrint &= L.N3.getEDIString(SegmentTerminator)
                strToPrint &= L.N4.getEDIString(SegmentTerminator)
                strToPrint &= L.G61.getEDIString(SegmentTerminator)
            Next
        End If
        If Not Loop200 Is Nothing Then
            For Each L As clsEDI204Loop200 In Loop200
                strToPrint &= L.N7.getEDIString(SegmentTerminator)
            Next
        End If
        If Not Loop300 Is Nothing Then
            For Each L As clsEDI204InLoop300 In Loop300
                strToPrint &= L.S5.getEDIString(SegmentTerminator)
                If Not L.L11s Is Nothing Then
                    For Each L11 As clsEDIL11 In L.L11s
                        strToPrint &= L11.getEDIString(SegmentTerminator)
                    Next
                End If
                If Not L.G62s Is Nothing Then
                    For Each G62 As clsEDIG62 In L.G62s
                        strToPrint &= G62.getEDIString(SegmentTerminator)
                    Next
                End If
                strToPrint &= L.PLD.getEDIString(SegmentTerminator)
                If Not L.NTEs Is Nothing Then
                    For Each NTE As clsEDINTE In L.NTEs
                        strToPrint &= NTE.getEDIString(SegmentTerminator)
                    Next
                End If
                If Not L.Loop310 Is Nothing Then
                    For Each L310 As clsEDI204InLoop310 In L.Loop310
                        strToPrint &= L310.N1.getEDIString(SegmentTerminator)
                        If Not L310.N3s Is Nothing AndAlso L310.N3s.Count() > 0 Then
                            For Each N3 In L310.N3s
                                strToPrint &= N3.getEDIString(SegmentTerminator)
                            Next
                        End If
                        strToPrint &= L310.N4.getEDIString(SegmentTerminator)
                        If Not L310.G61s Is Nothing Then
                            For Each G61 As clsEDIG61 In L310.G61s
                                strToPrint &= G61.getEDIString(SegmentTerminator)
                            Next
                        End If
                    Next
                End If
                If Not L.Loop320 Is Nothing Then
                    For Each L320 As clsEDI204Loop320 In L.Loop320
                        strToPrint &= L320.L5.getEDIString(SegmentTerminator)
                        strToPrint &= L320.AT8.getEDIString(SegmentTerminator)
                    Next
                End If
            Next
        End If
        strToPrint &= L3.getEDIString(SegmentTerminator)
        strToPrint &= SE.getEDIString(SegmentTerminator)
        strToPrint &= oGE.getEDIString(SegmentTerminator)
        strToPrint &= oIEA.getEDIString(SegmentTerminator)

        Return strToPrint

    End Function

    ''' <summary>
    ''' Process a single load inside of a 204 inbound EDI document.  Multi-Pick and Multi-Stop not supported
    ''' The individual segments must have been previously processed using the insertElements method
    ''' </summary>
    ''' <param name="ISA"></param>
    ''' <param name="GS"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="strMSG"></param>
    ''' <param name="fileName"></param>
    ''' <param name="DateProcessed"></param>
    ''' <param name="insertErrorMsg"></param>
    ''' <returns>
    ''' on success returns DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Accept
    ''' if the can be read but data is missing return DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
    ''' If there is an internal error log the information and return DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error
    '''     we do not send a 990 or a 997 on internal errors?  how do we notify the sender?
    ''' </returns>
    ''' <remarks>
    ''' Created by RHR on 5/10/2017 for v-7.0.6.105
    '''   Uses new EDI 204 inbound specifications
    '''   Anticipated file Layout
    '''   ST
    '''   B2
    '''   B2A
    '''   L11s
    '''   G62
    '''   PlD
    '''   NTE
    '''   Loop100s
    '''     N1
    '''     N3s
    '''     N4
    '''     G61s
    '''   Loop200s
    '''     N7
    '''   Loop300s
    '''     S5
    '''     L11s
    '''     G62s
    '''     AT5s
    '''     NTEs
    '''     Loop310
    '''         N1
    '''         N3s
    '''         N4
    '''         G61s
    '''     Loop350s
    '''         OID
    '''         LADs
    '''         Loop360s
    '''             L5
    '''             AT8
    '''             Loop365s
    '''                 G61s
    '''                 Loop370s
    '''                     LH1 
    ''' L3
    ''' SE 
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Public Function processData204InSingle(ByVal ISA As clsEDIISA,
                                ByVal GS As clsEDIGS,
                                ByVal CarrierControl As Integer,
                                ByRef strMSG As String,
                                Optional ByVal fileName As String = "",
                                Optional ByVal DateProcessed As Date = Nothing,
                                Optional ByRef insertErrorMsg As String = "") As DTO.tblLoadTender.LoadTenderStatusCodeEnum
        Dim statusCode As DTO.tblLoadTender.LoadTenderStatusCodeEnum = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Accept
        If Me.oConfig Is Nothing OrElse String.IsNullOrWhiteSpace(Me.Database) OrElse String.IsNullOrWhiteSpace(Me.DBServer) OrElse String.IsNullOrWhiteSpace(Me.ConnectionString) Then
            Throw New NotImplementedException("Cannot  processData204InSingle because the data configuration parameters were not provided")
        End If
        If String.IsNullOrWhiteSpace(Me.EDI204InSetting.MappingRules.WCFServiceURL) Then
            Throw New NotImplementedException("Cannot  processData204InSingle because the WCFServiceURL EDI configuration parameter was not provided")
        End If
        If String.IsNullOrWhiteSpace(Me.EDI204InSetting.MappingRules.WCFTCPServiceURL) Then
            Throw New NotImplementedException("Cannot  processData204InSingle because the WCFTCPServiceURL EDI configuration parameter was not provided")
        End If
        Dim blnHadErrors As Boolean = False
        Try
            Me.DALParameters.UserName = "EDI Integration"
            Me.DALParameters.WCFServiceURL = Me.EDI204InSetting.MappingRules.WCFServiceURL
            Me.oConfig.WCFURL = Me.EDI204InSetting.MappingRules.WCFServiceURL
            Me.oConfig.WCFTCPURL = Me.EDI204InSetting.MappingRules.WCFTCPServiceURL
            Me.oConfig.WCFAuthCode = Me.DALParameters.WCFAuthCode
            Dim oSysData As New DAL.NGLSystemDataProvider(Me.DALParameters)

            Dim archived As Boolean = False
            Dim blnCanProcess As Boolean = True
            Dim dblPlaceHolder As Double = 0
            Dim intPlaceHolder As Integer = 0
            Dim intVersion As Integer = 7
            Try
                intVersion = oSysData.GetMajorVersionRelease()
            Catch ex As Exception
                'just set the version to 7 by default
                intVersion = 7
            End Try
            Dim BLNumber As String = "N/A"
            If Not B2 Is Nothing AndAlso Not String.IsNullOrWhiteSpace(B2.B204) Then BLNumber = B2.B204.Trim()
            updateInboundUsingDefaultSettings(BLNumber)
            If (Me.blnInbound And Me.InboundCompNumber = 0) OrElse Me.OutboundCompNumber = 0 Then
                'read the 100 loop info and create a company if it is missing
                Dim oCompAddress As clsAddressInfo = get100LoopAddress(Me.Loop100)
                If String.IsNullOrWhiteSpace(oCompAddress.CompNumber) OrElse oCompAddress.CompNumber.Trim() = "0" Then
                    createNewCompany(oCompAddress, intVersion, Me.EDI204InSetting.MappingRules.CompLegalEntity)
                End If


            End If
            Dim oHeaders As New List(Of clsHeaderInfo)
            If Not Me.L11s Is Nothing AndAlso Me.L11s.Count() > 0 Then
                If Me.SOs Is Nothing Then Me.SOs = New List(Of String)
                If Me.POs Is Nothing Then Me.POs = New List(Of String)
                For Each L11 In Me.L11s
                    Select Case L11.L1102
                        Case "CO"
                            Me.SOs.Add(L11.L1101)
                        Case "PO"
                            Me.POs.Add(L11.L1101)
                    End Select
                Next
            End If

            If Not processStops(statusCode, strMSG) Then
                strMSG &= " using BOL Number: " & BLNumber
                Return statusCode
            End If
            If Me.SOs Is Nothing OrElse Me.SOs.Count() < 1 Then
                strMSG = "Cannot process pickup and delivery information because no sales orders were found using BOL Number: " & BLNumber
                statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error

                Return statusCode
            End If
            For Each SO In Me.SOs
                Dim oHeader As New clsHeaderInfo()
                If Not Me.EDI204InSetting Is Nothing Then
                    oHeader.CompLegalEntity = Me.EDI204InSetting.MappingRules.CompLegalEntity
                    oHeader.DefaultTemp = Me.EDI204InSetting.MappingRules.DefaultTemp
                    oHeader.DefaultPalletType = Me.EDI204InSetting.MappingRules.DefaultPalletType
                    oHeader.InboundPrefixKeys = Me.EDI204InSetting.MappingRules.InboundPrefixKeys
                Else
                    oHeader.DefaultTemp = "D"
                    oHeader.DefaultPalletType = "N"
                    oHeader.InboundPrefixKeys = "PO"
                End If

                'populate the data
                oHeader.CarrBLNumber = BLNumber
                oHeader.OrderNumber = SO
                If Not B2 Is Nothing AndAlso Not String.IsNullOrWhiteSpace(B2.B211) Then oHeader.TransCode = Me.B2.B211.Trim()
                'get the order date if provided
                If Not Me.G62 Is Nothing Then
                    Dim oG62s As New List(Of clsEDIG62) From {Me.G62}
                    processG62s(oG62s, oHeader.HeaderShipDates)
                End If
                If Not PLD Is Nothing AndAlso Not String.IsNullOrWhiteSpace(PLD.PLD01) Then
                    Integer.TryParse(PLD.PLD01, oHeader.TotalPLTS)
                Else
                    oHeader.TotalPLTS = 1
                End If
                oHeader.temp = oHeader.DefaultTemp
                process200Loop(oHeader)

                oHeader.POStatusFlag = Me.EDI204InSetting.MappingRules.POStatusFlag
                Dim blnFirstPickProcessed As Boolean = False
                For Each pick In Me.Pickups.Where(Function(x) x.OrderNumber = oHeader.OrderNumber).OrderBy(Function(x) x.StopSequence)
                    If pick Is Nothing Then
                        blnHadErrors = True
                        oHeader.strWarnings.Add("  The pickup information is missing for SO: " & SO & " using BL Number: " & BLNumber)
                        Continue For
                    End If
                    'get the first matching PO
                    Dim intPicksequence = pick.pickOrDropOrder
                    Dim oDrop As clsAddressInfo = DropOffs.Where(Function(x) x.pickOrDropOrder = intPicksequence).FirstOrDefault()
                    If oDrop Is Nothing OrElse oDrop.oItems Is Nothing OrElse oDrop.oItems.Count() < 1 Then
                        'we assume shared drop so skip this pick
                        Continue For
                    End If
                    If blnFirstPickProcessed Then
                        'add the items to the order but only if the deliver address matches.  if it does not we create a new sequence number
                        If compareStreetAddress(oHeader.DestAdd, oDrop) Then
                            If Not oDrop.oItems Is Nothing AndAlso oDrop.oItems.Count() > 0 Then
                                For Each item In oDrop.oItems
                                    item.ItemPONumber = oHeader.OrderNumber
                                    item.Temp = oHeader.temp
                                Next
                                oHeader.oItems.AddRange(oDrop.oItems)
                            End If
                        Else
                            'create a new sales order with a sequence number                          
                            Dim NewHeader As New clsHeaderInfo()
                            With NewHeader
                                .POStatusFlag = oHeader.POStatusFlag
                                .laneComments = oDrop.laneComments
                                .ShipInstructions = pick.ShipInstructions
                                .OrderNumber = SO
                                .PONumber = oDrop.CustomerPO
                                .DestAdd = oDrop
                                .OrigAdd = pick
                                .POHDRPallets = "0"
                                .ModeTypeControl = oHeader.ModeTypeControl
                                .temp = oHeader.temp
                                .TransCode = oHeader.TransCode
                                .HeaderShipDates = pick.StopShipDates
                                .ReqDate = oDrop.StopShipDates.ReqDate
                                .OrderDate = oHeader.OrderDate
                                .OrderSequence = intPicksequence
                                .oItems = oDrop.oItems
                                If Not oHeader.oItems Is Nothing AndAlso oHeader.oItems.Count() > 0 Then
                                    For Each item In .oItems
                                        item.OrderSequence = intPicksequence
                                        item.ItemPONumber = .OrderNumber
                                        item.Temp = oHeader.temp
                                    Next
                                End If
                                .HazmatCode = pick.HazmatCode
                                .CompLegalEntity = oHeader.CompLegalEntity
                                .DefaultTemp = oHeader.DefaultTemp
                                .DefaultPalletType = oHeader.DefaultPalletType
                                .InboundPrefixKeys = oHeader.InboundPrefixKeys
                                .CarrBLNumber = oHeader.CarrBLNumber
                            End With
                            NewHeader.Inbound = blnInbound
                            'validate company information.
                            If Me.blnInbound Then
                                If Me.InboundCompNumber = 0 Then
                                    If String.IsNullOrWhiteSpace(NewHeader.DestAdd.CompNumber) OrElse NewHeader.DestAdd.CompNumber.Trim() = "0" Then
                                        NewHeader.strErrors.Add("  The delivery location company information does not exist for SO: " & SO & ", using BL Number: " & BLNumber & ", and Location Code: " & oHeader.DestAdd.CompLocationID)
                                        NewHeader.statusCode = Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                                        statusCode = Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                                    End If
                                End If
                            Else
                                If Me.OutboundCompNumber = 0 Then
                                    If String.IsNullOrWhiteSpace(NewHeader.OrigAdd.CompNumber) OrElse NewHeader.OrigAdd.CompNumber.Trim() = "0" Then
                                        NewHeader.strErrors.Add("  The pickup location company information does not exist for SO: " & SO & ", using BL Number: " & BLNumber & ", and Location Code: " & oHeader.DestAdd.CompLocationID)
                                        oHeader.statusCode = Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                                        statusCode = Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                                    End If
                                End If
                            End If
                            'get the Lane Information
                            If Not identifyLaneNumber(NewHeader, statusCode) Then
                                NewHeader.strErrors.Add("  Could not read or create the lane information for SO: " & SO & ", using BL Number: " & BLNumber & ", Pickup Location Code: " & oHeader.OrigAdd.CompLocationID & ", and Delivery Location Code: " & oHeader.DestAdd.CompLocationID)
                                NewHeader.statusCode = statusCode
                            End If
                            If oHeaders Is Nothing Then oHeaders = New List(Of clsHeaderInfo)
                            oHeaders.Add(NewHeader)
                        End If
                    Else
                        blnFirstPickProcessed = True
                        oHeader.OrigAdd = pick
                        oHeader.DestAdd = oDrop
                        oHeader.HeaderShipDates = pick.StopShipDates
                        oHeader.ReqDate = oDrop.StopShipDates.ReqDate
                        oHeader.ShipInstructions = pick.ShipInstructions
                        oHeader.laneComments = oDrop.laneComments
                        oHeader.oItems = oDrop.oItems
                        oHeader.HazmatCode = pick.HazmatCode
                        oHeader.PONumber = oDrop.CustomerPO
                        If Not oHeader.oItems Is Nothing AndAlso oHeader.oItems.Count() > 0 Then
                            For Each item In oHeader.oItems
                                item.ItemPONumber = oHeader.OrderNumber
                                item.Temp = oHeader.temp
                            Next
                        End If
                        'check for inbound vs outbound
                        'oHeader.checkForInbound()
                        oHeader.Inbound = blnInbound
                        'validate company information.
                        If Me.blnInbound Then
                            If Me.InboundCompNumber = 0 Then
                                If String.IsNullOrWhiteSpace(oHeader.DestAdd.CompNumber) OrElse oHeader.DestAdd.CompNumber.Trim() = "0" Then
                                    oHeader.strErrors.Add("  The delivery location company information does not exist for SO: " & SO & ", using BL Number: " & BLNumber & ", and Location Code: " & oHeader.DestAdd.CompLocationID)
                                    oHeader.statusCode = Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                                    statusCode = Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                                End If
                            End If
                        Else
                            If Me.OutboundCompNumber = 0 Then
                                If String.IsNullOrWhiteSpace(oHeader.OrigAdd.CompNumber) OrElse oHeader.OrigAdd.CompNumber.Trim() = "0" Then
                                    oHeader.strErrors.Add("  The pickup location company information does not exist for SO: " & SO & ", using BL Number: " & BLNumber & ", and Location Code: " & oHeader.DestAdd.CompLocationID)
                                    oHeader.statusCode = Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                                    statusCode = Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                                End If
                            End If
                        End If
                        'get the Lane Information
                        If Not identifyLaneNumber(oHeader, statusCode) Then
                            oHeader.strErrors.Add("  Could not read or create the lane information for SO: " & SO & ", using BL Number: " & BLNumber & ", Pickup Location Code: " & oHeader.OrigAdd.CompLocationID & ", and Delivery Location Code: " & oHeader.DestAdd.CompLocationID)
                            oHeader.statusCode = statusCode
                        End If
                    End If
                Next
                If oHeaders Is Nothing Then oHeaders = New List(Of clsHeaderInfo)
                'validate that all the required informaiton has been recieved.
                If oHeader.OrigAdd Is Nothing OrElse oHeader.OrigAdd.isAddressEmpty() Then
                    oHeader.strErrors.Add("  Cannot accecpt loads without a pickup address.  Load tender is rejected for SO: " & SO & ", using BL Number: " & BLNumber)
                    statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                    oHeader.statusCode = statusCode
                End If

                If oHeader.DestAdd Is Nothing OrElse oHeader.DestAdd.isAddressEmpty() Then
                    oHeader.strErrors.Add("  Cannot accecpt loads without a delivery address.  Load tender is rejected for SO: " & SO & ", using BL Number: " & BLNumber)
                    statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                    oHeader.statusCode = statusCode
                End If
                oHeaders.Add(oHeader)
            Next
            If Not oHeaders Is Nothing AndAlso oHeaders.Count() > 0 Then
                For Each oHeader In oHeaders
                    saveBookingData(oHeader, statusCode, intVersion)
                    fillEDI204InTable(oHeader, strMSG, blnHadErrors, statusCode, insertErrorMsg, CarrierControl, DateProcessed, archived, fileName)
                Next
            End If
            Return statusCode

        Catch ex As Exception
            Throw
        End Try
        Return statusCode
    End Function


    ''' <summary>
    ''' Maps the data recieved from the 204In to the POHdr/Item objects
    ''' to send to the ProcessData method
    ''' </summary>
    ''' <param name="ISA"></param>
    ''' <param name="GS"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="strDBServer"></param>
    ''' <param name="strDatabase"></param>
    ''' <param name="strMSG"></param>
    ''' <param name="fileName"></param>
    ''' <param name="DateProcessed"></param>
    ''' <param name="insertErrorMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 5/2/17 for v-7.0.6.105 EDI 204In
    ''' 
    '''Many Loop 300 (expect 2 CL {pick up} CU {delivery})
    '''	 1 S5
    '''	 many L11(expect 2 On And PO)
    '''  many G62(expect 2 Ship And Req)
    '''  1 PLD
    '''	 many NTE(optional)
    '''  One Loop 310
    '''		1 N1 (require SH Or ST should match SF Or ST)
    '''		1 N3
    '''		1 N4
    '''		many G61(optional expecting a TE)
    '''  End 310
    '''  Many 320 Loop for each 300 (expect 1 required)
    '''		1 L5
    '''		1 AT8
    '''	 End 320 	
    '''End 300
    '''Modified by RHR for v-7.0.6.105 on 5/31/2017
    '''  added null reference checks to laneComments processing
    ''' </remarks>
    Public Function processData204In(ByVal ISA As clsEDIISA,
                                ByVal GS As clsEDIGS,
                                ByVal CarrierControl As Integer,
                                ByVal strDBServer As String,
                                ByVal strDatabase As String,
                                ByRef oCon As System.Data.SqlClient.SqlConnection,
                                ByRef strMSG As String,
                                Optional ByVal fileName As String = "",
                                Optional ByVal DateProcessed As Date = Nothing,
                                Optional ByRef insertErrorMsg As String = "") As Boolean
        strMSG = "Success!"
        Dim strWarnings = ""
        Try
            Dim oWCFPar = New DAL.WCFParameters() With {.Database = strDatabase,
                                                             .DBServer = strDBServer,
                                                             .UserName = "EDI Integration",
                                                             .WCFAuthCode = "NGLSystem"}
            Dim oSysData As New DAL.NGLSystemDataProvider(oWCFPar)
            Dim oEDIData As New DAL.NGLEDIData(oWCFPar)
            Dim oCarrierData As New DAL.NGLCarrierData(oWCFPar)
            Dim book As New Ngl.FreightMaster.Integration.clsBook

            Dim oOrders As New List(Of clsBookHeaderObject705)
            Dim oDetails As New List(Of clsBookDetailObject705)
            Dim o60Ords As New List(Of clsBookHeaderObject60)
            Dim o60Dets As New List(Of clsBookDetailObject60)

            Dim order As New clsBookHeaderObject705
            Dim archived As Boolean = False
            Dim statusCode As DTO.tblLoadTender.LoadTenderStatusCodeEnum = DTO.tblLoadTender.LoadTenderStatusCodeEnum.None
            Dim blnCanProcess As Boolean = True
            Dim dblPlaceHolder As Double = 0
            Dim intPlaceHolder As Integer = 0
            Dim POStatusFlag = 0
            Dim laneComments = ""
            Dim ShipInstructions = ""
            Dim OrderNumber = "", PONumber = ""
            Dim LaneNumber = ""
            Dim DestAdd As New clsAddressInfo
            Dim OrigAdd As New clsAddressInfo
            Dim Inbound As Boolean = False
            Dim POHDRPallets = 0
            Dim strModeTypeControl = ""
            Dim ModeTypeControl = 3 'Default 3 Truck
            Dim temp As String = ""
            Dim blnFrozen As Boolean = False
            Dim blnCooler As Boolean = False
            Dim blnDry As Boolean = False
            'These are set to Nothing initially because later we use them only if they have been populated
            Dim ReqDate As Date = Nothing
            Dim ShipDate As Date = Nothing
            Dim SchedulePUDate As Date = Nothing
            Dim ScheduleDelDate As Date = Nothing
            Dim SchedulePUTime As Date = Nothing
            Dim ScheduleDelTime As Date = Nothing

            'NGL will map the standard B2A01 data to the correct POStatusFlag as follows:
            '0 = New order, Ngl value of 0
            '4 = modified order, Ngl value of 1
            '1 = deleted order, Ngl value of 2
            Select Case B2A.B2A01?.Trim
                Case "00"
                    POStatusFlag = 0
                Case "04"
                    POStatusFlag = 1
                Case "01"
                    POStatusFlag = 2
            End Select
            'Modified by RHR for v-7.0.6.105 on 5/31/2017
            ' added null reference checks to laneComments processing
            For Each nte In NTEs
                If Not nte Is Nothing Then
                    Dim n = If(String.IsNullOrWhiteSpace(nte.NTE01), "", nte.NTE01.Trim()) + "- " + If(String.IsNullOrWhiteSpace(nte.NTE02), "", nte.NTE02.Trim()) + "| "
                    laneComments += n
                End If
            Next

            'Get the Mode Type from the 200 Loop -- we only use the first one
            strModeTypeControl = Loop200(0).N7.N711?.Trim
            If Not String.IsNullOrWhiteSpace(strModeTypeControl) Then
                If Not Integer.TryParse(strModeTypeControl, ModeTypeControl) Then
                    strWarnings += "Error parsing value Of N711 To Integer. ModeTypeControl Set To Default value Of 3 (Truck). "
                End If
            End If


            'We want to classify our 300 Loops into Pickup and Dropoff
            Dim CL300 As clsEDI204InLoop300 = Nothing 'Pickup
            Dim CU300 As clsEDI204InLoop300 = Nothing 'Dropoff

            'We want separate variables (for Address Info) to classify each 310 Loop as (in order of data priority)
            'DropOff Dest, Dropoff Orig, Pickup Dest, Pickup Orig
            'These are set to Nothing initially because later we use them only if they have been populated
            Dim CL310SF As clsEDI204InLoop310 = Nothing 'Pickup SF
            Dim CL310ST As clsEDI204InLoop310 = Nothing 'Pickup ST
            Dim CU310SF As clsEDI204InLoop310 = Nothing 'Dest SF
            Dim CU310ST As clsEDI204InLoop310 = Nothing 'Dest ST

            'Since we only allow single orders there should only be one CL and one CU
            For Each o300 In Loop300
                If o300.S5.S502?.Trim = "CL" Then
                    CL300 = o300
                End If
                If o300.S5.S502?.Trim = "CU" Then
                    CU300 = o300
                End If
            Next

            'Order of Precedence -- Dest 300Loop, Orig 300Loop, Header

            'Populate the Order/PO Numbers following the Order of Precedence
            If Not CU300 Is Nothing AndAlso Not CU300.L11s Is Nothing Then
                processL11s(CU300.L11s, OrderNumber, PONumber)
            Else
                If Not CL300 Is Nothing AndAlso Not CL300.L11s Is Nothing Then
                    processL11s(CL300.L11s, OrderNumber, PONumber)
                Else
                    If Not L11s Is Nothing AndAlso Not L11s Is Nothing Then
                        processL11s(L11s, OrderNumber, PONumber)
                    Else
                        'There are no L11s anywhere in the document
                        'ERROR
                        strMSG += "Could Not process the L11s because they could Not be found. "
                        blnCanProcess = False
                        archived = True
                        statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error
                    End If
                End If
            End If

            'Populate the DateTimes following the Order of Precedence
            'Process the CU300 (Dest) values first if they exist because they have first priority
            processG62s(CU300.G62s, ReqDate, ShipDate, SchedulePUDate, ScheduleDelDate, SchedulePUTime, ScheduleDelTime)
            processG62s(CL300.G62s, ReqDate, ShipDate, SchedulePUDate, ScheduleDelDate, SchedulePUTime, ScheduleDelTime)

            'Populate the Pallets following the Order of Precedence
            POHDRPallets = getHDRPallets(CU300, CL300, strWarnings)

            '**TODO** -- Do we need to check notes on both Pickup and Dropoff -- will they only have one set or could they
            'have diff notes for p vs d? Do we have to check for duplicates?
            'Populate the ShipInstructions following the Order of Precedence
            If Not CU300.NTEs Is Nothing AndAlso CU300.NTEs.Count > 0 Then
                processShipInstructions(CU300.NTEs, ShipInstructions)
            Else
                If Not CL300.NTEs Is Nothing AndAlso CL300.NTEs.Count > 0 Then
                    processShipInstructions(CL300.NTEs, ShipInstructions)
                End If
            End If

            'Populate variables corresponding to address information if the segments exist in the document
            'If the segments do not exist the variables = Nothing
            'Variables correspond to these segments: 
            'PickUp: 300Loop-CL-310Loop-SF (CL310SF), 300Loop-CL-310Loop-ST (CL310ST)
            'Dropoff: 300Loop-CU-310Loop-SF (CU310SF), 300Loop-CU-310Loop-ST (CU310ST)
            'This is done because we could have multiple 310s for each 300 (of which there can be multiples)
            'and we want to use the info from the Detail Destination(310 CU) segments as highest priority
            'follwed by Detail Origin (310 CL) and lastly Header (Loop 100)
            process310s(CU300, CU310SF, CU310ST)
            process310s(CL300, CL310SF, CL310ST)


            'Populate Dest Address following the Order of Precedence
            If Not CU310ST Is Nothing Then
                DestAdd = processAddressInfo(CU310ST, oWCFPar)
            Else
                If Not CL310ST Is Nothing Then
                    DestAdd = processAddressInfo(CL310ST, oWCFPar)
                Else
                    If Not Loop100 Is Nothing Then
                        DestAdd = processLoop100(Loop100, "ST", oWCFPar)
                    Else
                        'There is no Dest Address Info
                        'ERROR
                        strMSG += "Destination Address information could Not be found (N101 = ST). "
                        blnCanProcess = False
                        archived = True
                        statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error
                    End If
                End If
            End If
            'Populate Orig Address following the Order of Precedence
            If Not CU310SF Is Nothing Then
                OrigAdd = processAddressInfo(CU310SF, oWCFPar)
            Else
                If Not CL310SF Is Nothing Then
                    OrigAdd = processAddressInfo(CL310SF, oWCFPar)
                Else
                    If Not Loop100 Is Nothing Then
                        OrigAdd = processLoop100(Loop100, "SF", oWCFPar)
                    Else
                        'There is no Orig Address Info
                        'ERROR
                        strMSG += "Origin Address information could Not be found (N101 = SF). "
                        blnCanProcess = False
                        archived = True
                        statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error
                    End If
                End If
            End If


            For Each o300 In Loop300
                '320 Loop
                For Each o320 In o300.Loop320
                    Dim detail As New clsBookDetailObject705
                    With detail
                        .ItemPONumber = OrderNumber
                        .ItemNumber = o320.L5.L505?.Trim
                        .QtyOrdered = Integer.Parse(o320.AT8.AT805?.Trim)
                        .FreightCost = 0 'set to zero; Item level BFC is not available vie EDI 204
                        If Double.TryParse(NDT.FormatEDICurrencyToDouble(o320.L5.L506?.Trim), dblPlaceHolder) Then
                            .ItemCost = dblPlaceHolder
                        Else
                            strWarnings += "Failed To parse ItemCost To a Double. Check the formatting Of the EDI document For o320.L5.L506 (EDI currency format). "
                        End If
                        .Weight = Double.Parse(o320.AT8.AT803?.Trim)
                        .Cube = Integer.Parse(o320.AT8.AT807?.Trim)
                        .Pack = 0 'set to NULL or current value no updates allowed via EDI 204
                        .Size = "" 'set to NULL or current value no updates allowed via EDI 204
                        .Description = o320.L5.L502?.Trim
                        '.Hazmat = ""
                        .Brand = "" 'set to NULL or current value no updates allowed via EDI 204
                        .LotNumber = o320.L5.L507?.Trim
                        '.CustItemNumber = ""
                        '.CustomerNumber = ""
                        .POOrderSequence = 0 'NGL will use a default value of zero
                        '.PalletType = ""
                        '.POItemHazmatTypeCode = ""
                        .POItemPallets = Double.Parse(o320.AT8.AT804?.Trim)
                        .BookItemCommCode = o320.L5.L503?.Trim

                        '**TODO** - Do we need these?
                        '.POItemCompLegalEntity = ""
                        '.POItemCompAlphaCode = ""
                        '.POItemWeightUnitOfMeasure = ""
                        ''Alpha Code lookup to dbo.tblUnitOfMeasure.UnitOfMeasureType used to 
                        ''return the UnitOfMeasureControl mapped to the item level detail for weight
                        '.POItemCubeUnitOfMeasure = ""
                        ''Alpha Code lookup to dbo.tblUnitOfMeasure.UnitOfMeasureType used to 
                        ''return the UnitOfMeasureControl mapped to the item level detail for Volume
                        '.POItemDimensionUnitOfMeasure = ""
                        ''Alpha Code lookup to dbo.tblUnitOfMeasure.UnitOfMeasureType used to 
                        ''return the UnitOfMeasureControl mapped to the item level detail for 
                        ''Dimension: POItemQtyLength, POItemQtyWidth, and POItemQtyHeight
                        '.ChangeNo = ""
                        ''ERP System FK field for reference to header record
                        ''Created by RHR v-7.0.5.100 7/21/2016
                        ''Needed a reference to the ERP systems header key to assist with 
                        ''matching item detail records when duplicate records are transmitted in the same batch
                    End With
                    oDetails.Add(detail)
                Next
            Next

            'Inbound/Outbound Rule
            'Inbound vs Outbound is determined by the SF and ST location codes in Loop 310 N104.  
            'If the Then SF (orig) value maps To a qualified NGL shipping warehouse (company) the order Is outbound.  
            'If the Then ST (dest) value maps To a qualified NGL Shipping warehouse the order Is considered inbound.  
            'If both Then SF(orig) And ST (dest) values map To a qualified NGL shipping warehouse the order Is outbound.  Be sure To map the correct values To the Loop 310 N104 value
            If OrigAdd.CompNumber <> 0 Then
                Inbound = False
            Else
                If DestAdd.CompNumber <> 0 Then
                    Inbound = True
                Else
                    'Neither of the locations are valid so this is an error
                End If
            End If

            Dim POHDRWgt As Double = 0          'POHDRWgt (note check L304) L3.L301?.trim
            Dim POHDRTotalFrt As Double = 0     'POHDRTotalFrt (BFC) L3.L305?.trim
            Dim POHDRCube As Integer = 0        'POHDRCube L3.L309?.trim
            Dim POHDRQty As Integer = 0         'POHDRQty L3.L311?.trim

            dblPlaceHolder = 0
            If Double.TryParse(L3.L301?.Trim, dblPlaceHolder) Then
                POHDRWgt = dblPlaceHolder
            Else
                strWarnings += "Failed To parse POHDRWgt To a Double. Check the formatting Of the EDI document For L3.L301. "
            End If
            dblPlaceHolder = 0
            If Double.TryParse(L3.L305?.Trim, dblPlaceHolder) Then
                POHDRTotalFrt = dblPlaceHolder
            Else
                strWarnings += "Failed To parse POHDRTotalFrt To a Double. Check the formatting Of the EDI document For L3.L305. "
            End If
            intPlaceHolder = 0
            If Integer.TryParse(L3.L309?.Trim, intPlaceHolder) Then
                POHDRCube = intPlaceHolder
            Else
                strWarnings += "Failed To parse POHDRCube To an Integer. Check the formatting Of the EDI document For L3.L309. "
            End If
            intPlaceHolder = 0
            If Integer.TryParse(L3.L311?.Trim, intPlaceHolder) Then
                POHDRQty = intPlaceHolder
            Else
                strWarnings += "Failed To parse POHDRQty To an Integer. Check the formatting Of the EDI document For L3.L311. "
            End If

            'Outbound
            '  OrigCompanynumber-DestinationName - locationID
            '  Prod Ex:  18-C10682-S00672-307
            'Inbound
            '  DestinationCompanyNumber-OriginName - locationID
            '  Prod Ex:  23-inbound Tyson

            '**TODO** - Why am I settings the POStatus Flag to 6?? Also, we don't have location ID
            If Inbound Then
                'Inbound
                LaneNumber = DestAdd.CompNumber + "-" + OrigAdd.AddrName
            Else
                'Outbound
                LaneNumber = OrigAdd.CompNumber + "-" + DestAdd.AddrName
            End If

            getTempInfo(oDetails, temp, blnFrozen, blnCooler, blnDry)

            With order
                .PONumber = OrderNumber
                .POVendor = LaneNumber 'Lane Number Customer Shipping From – Shipping to Identifier:  NGL will need to generate this number automatically using a special formula that includes the values provided in the N104 data.

                '------------------------------------
                '**TODO** - How do we populate this?
                .POdate = "" 'Order Date in ERP system.  NGL will use the date that the 204 was processed on New Orders as the Order Date and will not allow updates to the Order Date on modified orders.
                '------------------------------------

                .POShipdate = ShipDate
                '**TODO** - figure out what these custom codes are and how to map them
                .POFrt = 4 'Double.Parse(B2.B211?.trim) 'Custom B211 Codes will be use to map to the PO Frt Trans Code if provided.  If empty NGL will use a default value of four (4) equals Outbound Vendor Delivered.
                .POTotalFrt = POHDRTotalFrt

                '------------------------------------
                '.POTotalCost = 0
                '------------------------------------

                .POWgt = POHDRWgt
                .POCube = POHDRCube
                .POQty = POHDRQty
                .POPallets = POHDRPallets

                '------------------------------------
                '.PODefaultCustomer = ""
                '.PODefaultCarrier = 0
                '------------------------------------

                .POReqDate = ReqDate
                .POShipInstructions = ShipInstructions
                .POCooler = blnCooler
                .POFrozen = blnFrozen
                .PODry = blnDry
                '**TODO** - is this the correct mapping?
                .POTemp = temp
                .POCustomerPO = PONumber
                .POStatusFlag = POStatusFlag
                .POOrderSequence = 0 'NGL will use a default value of zero
                .POSchedulePUDate = SchedulePUDate
                .POSchedulePUTime = SchedulePUTime
                .POScheduleDelDate = ScheduleDelDate
                .POSCheduleDelTime = ScheduleDelTime
                'Origin company number When creating lanes Or Using an alternate shipping address, 
                'set to zero when using company alpha codes,  
                'If this value is empty and this is an outbound load (POInbound = false)  the
                'system will use  PO Default Customer value by default
                .POOrigCompNumber = OrigAdd.CompNumber
                'Origin company legal entity used When creating lanes Or Using an alternate shipping address, 
                'set to empty string when shipping from a non-managed facility,  
                'If this value is empty and this is an outbound load (POInbound = false)  the
                'system will use PO Company Legal Entity value by default
                'Not Stored in POHDR Table,  used for automatic lane generation only
                .POOrigLegalEntity = OrigAdd.CompLegalEntity
                .POOrigName = OrigAdd.AddrName
                .POOrigAddress1 = OrigAdd.Addr1
                .POOrigAddress2 = OrigAdd.Addr2
                .POOrigAddress3 = "" 'set to NULL or current value no updates allowed via EDI 204
                .POOrigCity = OrigAdd.City
                .POOrigState = OrigAdd.State
                .POOrigCountry = OrigAdd.Country
                .POOrigZip = OrigAdd.Zip
                .POOrigContactPhone = OrigAdd.ContactPhone
                .POOrigContactPhoneExt = OrigAdd.ContactPhoneExt
                .POOrigContactFax = OrigAdd.ContactFAX
                'Destination company number When creating lanes Or Using an alternate shipping address, 
                'set to zero when using company alpha codes,  
                'If this value is empty and this is an inbound load (POInbound = true)  the
                'system will use  PO Default Customer value by default
                .PODestCompNumber = DestAdd.CompNumber
                'Destination company legal entity used When creating lanes Or Using an alternate shipping address, 
                'set to empty string when shipping to a non-managed facility,  
                'If this value is empty and this is an inbound load (POInbound = true)  the
                'system will use PO Company Legal Entity value by default
                'Not Stored in POHDR Table,  used for automatic lane generation only
                .PODestLegalEntity = DestAdd.CompLegalEntity
                .PODestName = DestAdd.AddrName
                .PODestAddress1 = DestAdd.Addr1
                .PODestAddress2 = DestAdd.Addr2
                .PODestAddress3 = "" 'set to NULL or current value no updates allowed via EDI 204
                .PODestCity = DestAdd.City
                .PODestState = DestAdd.State
                .PODestCountry = DestAdd.Country
                .PODestZip = DestAdd.Zip
                .PODestContactPhone = DestAdd.ContactPhone
                .PODestContactPhoneExt = DestAdd.ContactPhoneExt
                .PODestContactFax = DestAdd.ContactFAX
                .POInbound = Inbound 'Inbound vs Outbound is determined by the SF and ST location codes in Loop 310 N104.  If the SF value maps to a qualified NGL shipping warehouse (company) the order is outbound.  If the ST value maps to a qualified NGL Shipping warehouse the order is considered inbound.  If both SF and ST values map to a qualified NGL shipping warehouse the order is outbound.  Be sure to map the correct values to the Loop 310 N104 value
                .POPalletType = "N" 'Mapping is not available via 204.  NGL will use N for normal pallets by default.
                .POComments = laneComments
                '.POCommentsConfidential = ""
                '.POCompLegalEntity = ""
                .POModeTypeControl = ModeTypeControl '3 'Truck
                '**TODO**  - Do we need this?
                'ERP System key field for header record
                'Created by RHR v-7.0.5.100 7/21/2016
                'Needed a reference to the ERP systems header key to assist with 
                'matching item detail records when duplicate records are transmitted in the same batch
                .ChangeNo = ""
            End With
            oOrders.Add(order)

            Try
                Dim carr = oCarrierData.getCarrierNameNumberSCAC(CarrierControl)

                Dim strEDIMessage = ""
                If strMSG <> "Success!" Then
                    strEDIMessage = strMSG
                End If
                If Not String.IsNullOrWhiteSpace(strWarnings) Then
                    strEDIMessage += "Warnings: " + strWarnings
                End If

                Dim lts204In As New DAL.LTS.tblEDI204In
                With lts204In
                    .CarrierSCAC = carr.Item("CarrierSCAC")
                    .CarrierNumber = carr.Item("CarrierNumber")
                    .CarrierName = carr.Item("CarrierName")
                    .OrderNumber = OrderNumber
                    .OrderSequence = 0
                    .PONumber = PONumber
                    .TotalCases = POHDRQty
                    .TotalWgt = POHDRWgt
                    .TotalPL = POHDRPallets
                    .TotalCube = POHDRCube
                    .ShipDate = ShipDate
                    .ReqDate = ReqDate
                    .SchedulePUDate = SchedulePUDate
                    .SchedulePUTime = SchedulePUTime
                    .ScheduleDelDate = ScheduleDelDate
                    .ScheduleDelTime = ScheduleDelTime
                    .OrigName = OrigAdd.AddrName
                    .OrigAddress1 = OrigAdd.Addr1
                    .OrigAddress2 = OrigAdd.Addr2
                    .OrigAddress3 = ""
                    .OrigCity = OrigAdd.City
                    .OrigState = OrigAdd.State
                    .OrigCountry = OrigAdd.Country
                    .OrigZip = OrigAdd.Zip
                    .OrigPhone = OrigAdd.ContactPhone
                    .OrigCompanyNumber = "CompNo"
                    .DestName = DestAdd.AddrName
                    .DestAddress1 = DestAdd.Addr1
                    .DestAddress2 = DestAdd.Addr2
                    .DestAddress3 = ""
                    .DestCity = DestAdd.City
                    .DestState = DestAdd.State
                    .DestCountry = DestAdd.Country
                    .DestZip = DestAdd.Zip
                    .DestPhone = DestAdd.ContactPhone
                    .DestCompanyNumber = "CompNo"
                    .LaneComments = laneComments
                    .Inbound = Inbound
                    .EDI204InStatusCode = statusCode
                    .EDI204InMessage = strEDIMessage
                    .Archived = archived
                    .EDI204InFileName204In = fileName
                End With
                'Insert History record into tblEDI204In
                If Not oEDIData.InsertIntoEDI204In(lts204In, DateProcessed) Then
                    insertErrorMsg += "Could not insert record into tblEDI204In for Order Number: " + OrderNumber + ", PONumber: " + PONumber + ", SCAC: " + carr.Item("CarrierSCAC") + ", DateProcessed, : " + DateProcessed.ToString + ", and filename: " + fileName
                End If

                Dim intRes As Integer = 0
                If blnCanProcess Then
                    Dim intVersion = oSysData.GetMajorVersionRelease()

                    'Call ProcessData 705 vs 60 based on version
                    Select Case intVersion
                        Case 6
                            populate60Variables(oOrders, oDetails, o60Ords, o60Dets)
                            intRes = book.ProcessObjectData(o60Ords, o60Dets, oCon.ConnectionString)
                        Case 7
                            intRes = book.ProcessObjectData(oOrders, oDetails, oCon.ConnectionString)
                    End Select
                End If

            Catch ex As Ngl.Core.DatabaseRetryExceededException
                strMSG = "Failed to update the accept or reject load status: " & ex.Message
                Return False
            Catch ex As Ngl.Core.DatabaseLogInException
                strMSG = "Database login failure: " & ex.Message
                Return False
            Catch ex As Ngl.Core.DatabaseInvalidException
                strMSG = "Database access failure : " & ex.Message
                Return False
            Catch ex As Ngl.Core.DatabaseDataValidationException
                strMSG = ex.Message
                Return False
            Catch ex As Exception
                Throw
                Return False
            End Try


        Catch ex As Exception
            Throw
        End Try
        Return True
    End Function

    Private Sub processL11s(ByVal L11s As List(Of clsEDIL11), ByRef OrderNumber As String, ByRef PONumber As String)
        For Each L11 In L11s
            If L11.L1102?.Trim = "ON" Then
                OrderNumber = L11.L1101?.Trim
            End If
            If L11.L1102?.Trim = "PO" Then
                PONumber = L11.L1101?.Trim
            End If
        Next
    End Sub

    Private Sub process310s(ByVal Loop300 As clsEDI204InLoop300, ByRef Loop310SF As clsEDI204InLoop310, ByRef Loop310ST As clsEDI204InLoop310)
        If Not Loop300 Is Nothing AndAlso Not Loop300.Loop310 Is Nothing Then
            For Each o310 In Loop300.Loop310
                If o310.N1.N101 = "SF" Then
                    Loop310SF = o310
                End If
                If o310.N1.N101 = "ST" Then
                    Loop310ST = o310
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' uses the clsCompanyHeaderObject for the current version to 
    ''' Create a new company record if one does not exist.
    ''' </summary>
    ''' <param name="oCompAddress"></param>
    ''' <param name="intVersion"></param>
    ''' <param name="sCompLegalEntity"></param>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/18/2017
    ''' Modified by RHR for v-6.0.4.7 on 6/9/2017
    '''   added logic to use CompAbrev from clsAddressInfo
    ''' </remarks>
    Private Sub createNewCompany(ByVal oCompAddress As clsAddressInfo, ByVal intVersion As Integer, ByVal sCompLegalEntity As String)
        'we need to create a new company
        Dim oComp As New Ngl.FreightMaster.Integration.clsCompany(Me.oConfig)

        If intVersion = 6 Then
            Dim oCompHeader As New Ngl.FreightMaster.Integration.clsCompanyHeaderObject
            With oCompHeader
                .CompNumber = oCompAddress.CompAlphaCode
                .CompName = oCompAddress.AddrName
                .CompNatNumber = 0
                .CompNatName = ""
                .CompStreetAddress1 = oCompAddress.Addr1
                .CompStreetAddress2 = oCompAddress.Addr2
                .CompStreetAddress3 = oCompAddress.Addr3
                .CompStreetCity = oCompAddress.City
                .CompStreetState = oCompAddress.State
                .CompStreetCountry = oCompAddress.Country
                .CompStreetZip = oCompAddress.Zip
                .CompMailAddress1 = oCompAddress.Addr1
                .CompMailAddress2 = oCompAddress.Addr2
                .CompMailAddress3 = oCompAddress.Addr3
                .CompMailCity = oCompAddress.City
                .CompMailState = oCompAddress.State
                .CompMailCountry = oCompAddress.Country
                .CompMailZip = oCompAddress.Zip
                .CompWeb = ""
                .CompEmail = ""
                .CompDirections = ""
                'Modified by RHR for v-6.0.4.7 on 6/9/2017
                .CompAbrev = If(String.IsNullOrWhiteSpace(oCompAddress.CompAbrev), Left(oCompAddress.AddrName, 3), Left(oCompAddress.CompAbrev, 3))
                .CompActive = True
                .CompNEXTrack = False
                .CompFinCreditLimit = 100000
                .CompFinCreditUsed = 0
                .CompFinUseImportFrtCost = False
            End With
            Dim oCompCont As New Ngl.FreightMaster.Integration.clsCompanyContactObject
            With oCompCont
                .CompNumber = oCompAddress.CompAlphaCode
                .CompContName = If(String.IsNullOrWhiteSpace(oCompAddress.ContactName), "Customer Service", oCompAddress.ContactName)
                .CompContPhone = oCompAddress.ContactPhone
                .CompContFax = oCompAddress.ContactFAX
            End With
            Dim oComps() As clsCompanyHeaderObject = {oCompHeader}
            Dim oConts() As clsCompanyContactObject = {oCompCont}
            Try
                Dim intRet As Integer = oComp.ProcessObjectData(oComps, oConts, Me.ConnectionString)

            Catch ex As Exception
                'do nothing we catch the missing company down below?
            End Try
        Else
            Dim oCompHeader As New Ngl.FreightMaster.Integration.clsCompanyHeaderObject70
            With oCompHeader
                .CompNumber = 0
                .CompName = oCompAddress.AddrName
                .CompNatNumber = 0
                .CompNatName = ""
                .CompStreetAddress1 = oCompAddress.Addr1
                .CompStreetAddress2 = oCompAddress.Addr2
                .CompStreetAddress3 = oCompAddress.Addr3
                .CompStreetCity = oCompAddress.City
                .CompStreetState = oCompAddress.State
                .CompStreetCountry = oCompAddress.Country
                .CompStreetZip = oCompAddress.Zip
                .CompMailAddress1 = oCompAddress.Addr1
                .CompMailAddress2 = oCompAddress.Addr2
                .CompMailAddress3 = oCompAddress.Addr3
                .CompMailCity = oCompAddress.City
                .CompMailState = oCompAddress.State
                .CompMailCountry = oCompAddress.Country
                .CompMailZip = oCompAddress.Zip
                .CompWeb = ""
                .CompEmail = ""
                .CompDirections = ""
                'Modified by RHR for v-6.0.4.7 on 6/9/2017
                .CompAbrev = If(String.IsNullOrWhiteSpace(oCompAddress.CompAbrev), Left(oCompAddress.AddrName, 3), Left(oCompAddress.CompAbrev, 3))
                .CompActive = True
                .CompNEXTrack = False
                .CompFinCreditLimit = 100000
                .CompFinCreditUsed = 0
                .CompFinUseImportFrtCost = False
                .CompAlphaCode = oCompAddress.CompAlphaCode
                .CompLegalEntity = sCompLegalEntity
            End With
            Dim oCompCont As New Ngl.FreightMaster.Integration.clsCompanyContactObject70
            With oCompCont
                .CompNumber = 0
                .CompContName = If(String.IsNullOrWhiteSpace(oCompAddress.ContactName), "Customer Service", oCompAddress.ContactName)
                .CompContPhone = oCompAddress.ContactPhone
                .CompContFax = oCompAddress.ContactFAX
                .CompAlphaCode = oCompAddress.CompAlphaCode
                .CompContPhoneExt = oCompAddress.ContactPhoneExt
                .CompLegalEntity = sCompLegalEntity
            End With
            Dim oComps As New List(Of clsCompanyHeaderObject70) From {oCompHeader}
            Dim oConts As New List(Of clsCompanyContactObject70) From {oCompCont}
            Try
                Dim oRet = oComp.ProcessObjectData70(oComps, oConts, Me.ConnectionString, Nothing)

            Catch ex As Exception
                'do nothing we catch the missing company down below?
            End Try
        End If
    End Sub

    ''' <summary>
    ''' populates a clsAddressInfo object with Loop 100 data.  Looks up the company informaiton using N104 if available
    ''' If not it returns 0 for the company number.
    ''' </summary>
    ''' <param name="o100s"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR on 5/10/2017 for v-7.0.6.105
    ''' </remarks>
    Private Function get100LoopAddress(ByVal o100s As List(Of clsEDI204Loop100)) As clsAddressInfo
        Dim Addr As New clsAddressInfo
        If o100s?.Count() < 1 Then Return Addr
        Dim o100 As clsEDI204Loop100
        For Each item As clsEDI204Loop100 In o100s
            If Not item Is Nothing AndAlso Not item.N1 Is Nothing Then
                Dim strEntityCode = If(String.IsNullOrWhiteSpace(item.N1.N101), "", item.N1.N101)
                If Not String.IsNullOrWhiteSpace(strEntityCode) Then
                    If strEntityCode.Trim().ToUpper = "BT" Then
                        o100 = item
                        Exit For
                    End If
                End If
            End If
        Next
#Disable Warning BC42104 ' Variable 'o100' is used before it has been assigned a value. A null reference exception could result at runtime.
        If o100 Is Nothing Then Return Addr
#Enable Warning BC42104 ' Variable 'o100' is used before it has been assigned a value. A null reference exception could result at runtime.
        Addr.CompAbrev = Me.EDI204InSetting.MappingRules.CompAbrev?.Trim()
        Addr.CompLocationID = o100.N1.N104?.Trim()
        Addr.CompAlphaCode = Addr.CompAbrev & o100.N1.N104?.Trim()
        If String.IsNullOrEmpty(Addr.CompLegalEntity) Then
            Addr.CompLegalEntity = Me.EDI204InSetting.MappingRules.CompLegalEntity
        End If
        Addr.AddrName = o100.N1.N102?.Trim
        If o100.N3s?.Count() > 0 Then

            Addr.Addr1 = o100.N3s(0).N301?.Trim
            Addr.Addr2 = o100.N3s(0).N302?.Trim
            If o100.N3s.Count() > 1 Then
                Addr.Addr3 = Left(o100.N3s(1).N301?.Trim() & " " & o100.N3s(1).N302?.Trim(), 40)
            End If
        End If
        Addr.City = o100.N4.N401?.Trim
        Addr.State = o100.N4.N402?.Trim
        Addr.Zip = o100.N4.N403?.Trim
        Addr.Country = o100.N4.N404?.Trim
        If String.IsNullOrWhiteSpace(Addr.Country) Then Addr.Country = "US"
        If o100.G61s?.Count() > 0 Then
            For Each g61 In o100.G61s
                If Not String.IsNullOrWhiteSpace(g61.G6102?.Trim) Then
                    Addr.ContactName = g61.G6102?.Trim
                End If
                Select Case g61.G6103?.Trim
                    Case "FX"
                        Addr.ContactFAX = g61.G6104?.Trim
                    Case "TE"
                        Addr.ContactPhone = g61.G6104?.Trim
                    Case "EX"
                        Addr.ContactPhoneExt = g61.G6104?.Trim
                End Select
            Next
        End If
        Dim comp = getEDI204InCompInfo(Addr)
        If Not comp Is Nothing Then
            Addr.CompNumber = comp.CompNumber
            Addr.CompLegalEntity = comp.CompLegalEntity
        End If


        Return Addr
    End Function


    ''' <summary>
    ''' populates a new clsAddressInfo object with Loop 310 data    ''' 
    ''' </summary>
    ''' <param name="o310"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR on 5/10/2017 for v-7.0.6.105
    ''' </remarks>
    Private Function get310LoopAddress(ByVal o310 As clsEDI204InLoop310) As clsAddressInfo
        Dim Addr As New clsAddressInfo
        If o310 Is Nothing Then Return Addr
        Addr.CompAbrev = Me.EDI204InSetting.MappingRules.CompAbrev?.Trim()
        Addr.CompLocationID = o310.N1.N104?.Trim()
        Addr.CompAlphaCode = Addr.CompAbrev & o310.N1.N104?.Trim()
        If String.IsNullOrEmpty(Addr.CompLegalEntity) Then
            Addr.CompLegalEntity = Me.EDI204InSetting.MappingRules.CompLegalEntity
        End If
        Addr.AddrName = o310.N1.N102?.Trim
        If Not o310.N3s Is Nothing AndAlso o310.N3s.Count() > 0 Then

            Addr.Addr1 = o310.N3s(0).N301?.Trim
            Addr.Addr2 = o310.N3s(0).N302?.Trim
            If o310.N3s.Count() > 1 Then
                Addr.Addr3 = Left(o310.N3s(1).N301?.Trim & " " & o310.N3s(1).N302, 40)
            End If
        End If
        Addr.City = o310.N4.N401?.Trim
        Addr.State = o310.N4.N402?.Trim
        Addr.Zip = o310.N4.N403?.Trim
        Addr.Country = o310.N4.N404?.Trim
        If String.IsNullOrWhiteSpace(Addr.Country) Then Addr.Country = "US"
        If Not o310.G61s Is Nothing AndAlso o310.G61s.Count() > 0 Then
            For Each g61 In o310.G61s
                If Not String.IsNullOrWhiteSpace(g61.G6102?.Trim) Then
                    Addr.ContactName = g61.G6102?.Trim
                End If
                Select Case g61.G6103?.Trim
                    Case "FX"
                        Addr.ContactFAX = g61.G6104?.Trim
                    Case "TE"
                        Addr.ContactPhone = g61.G6104?.Trim
                    Case "EX"
                        Addr.ContactPhoneExt = g61.G6104?.Trim
                End Select
            Next
        End If
        Dim comp = getEDI204InCompInfo(Addr)
        If Not comp Is Nothing Then
            Addr.CompNumber = comp.CompNumber
            Addr.CompLegalEntity = comp.CompLegalEntity
        End If

        Return Addr
    End Function

    ''' <summary>
    ''' allocate and/or calculate the item pallet counts for v-6.0.4.7
    ''' </summary>
    ''' <param name="totalPlts"></param>
    ''' <param name="items"></param>
    ''' <remarks>
    ''' Modified by RHR for vb-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Sub allocatePalletsToItems(ByVal totalPlts As Integer, ByRef items As List(Of clsBookDetailObject604))

        Dim remainingTotalFee As Double = totalPlts
        Dim count As Integer = 0
        If items Is Nothing OrElse items.Count() < 1 Then Return 'nothing to do so this is true
        Dim totalWgt = items.Sum(Function(x) x.Weight)
        Dim totalItems As Integer = items.Count()
        For Each item In items.OrderBy(Function(x) x.Weight)
            Dim allocatedPlts As Double = 0
            If count = items.Count - 1 Then
                allocatedPlts = remainingTotalFee
            Else
                ' Calculate the allocated pallets by weight
                allocatedPlts = allocatePallet(totalPlts, item.Weight, totalWgt, totalItems)
                remainingTotalFee -= allocatedPlts
                count += 1
            End If
            item.POItemPallets = allocatedPlts
        Next

    End Sub

    ''' <summary>
    ''' llocate and/or calculate the item pallet counts for v-7.0.6.105
    ''' </summary>
    ''' <param name="totalPlts"></param>
    ''' <param name="items"></param>
    ''' <remarks>
    ''' Modified by RHR for vb-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Sub allocatePalletsToItems(ByVal totalPlts As Integer, ByRef items As List(Of clsBookDetailObject705))

        Dim remainingTotalFee As Double = totalPlts
        Dim count As Integer = 0
        If items Is Nothing OrElse items.Count() < 1 Then Return 'nothing to do so this is true
        Dim totalWgt = items.Sum(Function(x) x.Weight)
        Dim totalItems As Integer = items.Count()
        For Each item In items.OrderBy(Function(x) x.Weight)
            Dim allocatedPlts As Double = 0
            If count = items.Count - 1 Then
                allocatedPlts = remainingTotalFee
            Else
                ' Calculate the allocated pallets by weight
                allocatedPlts = allocatePallet(totalPlts, item.Weight, totalWgt, totalItems)
                remainingTotalFee -= allocatedPlts
                count += 1
            End If
            item.POItemPallets = allocatedPlts
        Next

    End Sub

    ''' <summary>
    ''' determine the % of pallets to allcate to this item
    ''' </summary>
    ''' <param name="totalPlts"></param>
    ''' <param name="ItemWgt"></param>
    ''' <param name="totalWgt"></param>
    ''' <param name="totalItems"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Function allocatePallet(ByVal totalPlts As Integer, ByVal ItemWgt As Double, ByVal totalWgt As Double, ByVal totalItems As Integer) As Double
        Dim allocatedPlts As Double = 0
        If totalWgt > 0 Then
            allocatedPlts = totalPlts * ItemWgt / totalWgt
        Else
            allocatedPlts = totalPlts / totalItems
        End If
        Return Math.Round(allocatedPlts, 2)
    End Function

    ''' <summary>
    ''' compare street city and state.  we do not check zip
    ''' </summary>
    ''' <param name="A1"></param>
    ''' <param name="A2"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' created by RHR for v-7.0.6.105 -- v-6.0.4.7 on 6/8/2017
    '''   check street, city and state for a match. 
    ''' </remarks>
    Public Function compareStreetAddress(ByVal A1 As clsAddressInfo, ByVal A2 As clsAddressInfo) As Boolean
        Dim blnRet As Boolean = True
        If A1 Is Nothing OrElse A2 Is Nothing Then Return False
        If A1.Addr1 <> A2.Addr1 Or A1.City <> A2.City Or A1.State <> A2.State Then Return False
        Return blnRet
    End Function

    ''' <summary>
    ''' Compares a new Loop 310 Address with the a previous address to determine if they match.
    ''' compares Company, Street, City, State, and zip data.  All must match or the method
    ''' returns false
    ''' </summary>
    ''' <param name="o310"></param>
    ''' <param name="Addr"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR on 5/10/2017 for v-7.0.6.105
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Function Does310LoopAddressMatch(ByVal o310 As clsEDI204InLoop310, ByVal Addr As clsAddressInfo, ByRef strMsg As String, ByVal blnIsOrigin As Boolean) As Boolean
        Dim blnRet As Boolean = True

        If o310 Is Nothing OrElse Addr Is Nothing Then Return True

        If blnIsOrigin Then
            If Me.EDI204InSetting.MappingRules.doPULocationCodeValidation Then
                If Addr.CompLocationID <> If(String.IsNullOrWhiteSpace(o310.N1.N104), "", o310.N1.N104.Trim()) Then
                    strMsg &= " Unmatched Pickup Location Code: " & Addr.CompLocationID
                    blnRet = False
                End If
            End If
            If Me.EDI204InSetting.MappingRules.doPUAddressNameValidation Then
                If Addr.AddrName <> If(String.IsNullOrWhiteSpace(o310.N1.N102), "", o310.N1.N102.Trim()) Then
                    strMsg &= " Unmatched Pickup Address Name: " & Addr.AddrName
                    blnRet = False
                End If
            End If
            If Me.EDI204InSetting.MappingRules.doPUAddressValidation Then
                If Not o310.N3s Is Nothing AndAlso o310.N3s.Count() > 0 Then
                    If Addr.Addr1 <> If(String.IsNullOrWhiteSpace(o310.N3s(0).N301), "", o310.N3s(0).N301.Trim()) Then
                        strMsg &= " Unmatched Pickup Street Address: " & Addr.Addr1
                        blnRet = False
                    End If
                End If
                If Addr.City <> If(String.IsNullOrWhiteSpace(o310.N4.N401), "", o310.N4.N401.Trim()) Then
                    strMsg &= " Unmatched Pickup City: " & Addr.City
                    blnRet = False
                End If
                If Addr.State <> If(String.IsNullOrWhiteSpace(o310.N4.N402), "", o310.N4.N402.Trim()) Then
                    strMsg &= " Unmatched Pickup State: " & Addr.State
                    blnRet = False
                End If
                If Addr.Zip <> If(String.IsNullOrWhiteSpace(o310.N4.N403), "", o310.N4.N403.Trim()) Then
                    strMsg &= " Unmatched Pickup Zip: " & Addr.Zip
                    blnRet = False
                End If
            End If
        Else
            If Me.EDI204InSetting.MappingRules.doDelLocationCodeValidation Then
                If Addr.CompLocationID <> If(String.IsNullOrWhiteSpace(o310.N1.N104), "", o310.N1.N104.Trim()) Then
                    strMsg &= " Unmatched Delivery Location Code: " & Addr.CompLocationID
                    blnRet = False
                End If
            End If

            If Me.EDI204InSetting.MappingRules.doDelAddressNameValidation Then
                If Addr.AddrName <> If(String.IsNullOrWhiteSpace(o310.N1.N102), "", o310.N1.N102.Trim()) Then
                    strMsg &= " Unmatched Delivery Address Name: " & Addr.AddrName
                    blnRet = False
                End If
            End If

            If Me.EDI204InSetting.MappingRules.doDelAddressValidation Then
                If Not o310.N3s Is Nothing AndAlso o310.N3s.Count() > 0 Then
                    If Addr.Addr1 <> If(String.IsNullOrWhiteSpace(o310.N3s(0).N301), "", o310.N3s(0).N301.Trim()) Then
                        strMsg &= " Unmatched Delivery Street Address: " & Addr.Addr1
                        blnRet = False
                    End If
                End If
                If Addr.City <> If(String.IsNullOrWhiteSpace(o310.N4.N401), "", o310.N4.N401.Trim()) Then
                    strMsg &= " Unmatched Delivery City: " & Addr.City
                    blnRet = False
                End If
                If Addr.State <> If(String.IsNullOrWhiteSpace(o310.N4.N402), "", o310.N4.N402.Trim()) Then
                    strMsg &= " Unmatched Delivery State: " & Addr.State
                    blnRet = False
                End If
                If Addr.Zip <> If(String.IsNullOrWhiteSpace(o310.N4.N403), "", o310.N4.N403.Trim()) Then
                    strMsg &= " Unmatched Delivery Zip: " & Addr.Zip
                    blnRet = False
                End If
            End If

        End If

        Return blnRet
    End Function

    ''' <summary>
    ''' Deprecated version used to get 310 loop address informaiton  use the new get310LoopAddress instead
    ''' </summary>
    ''' <param name="o310"></param>
    ''' <param name="oWCFPar"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Deprecated by RHR for v-7.0.6.105 on 5/22/2017
    '''   use the new get310LoopAddress method instead
    ''' </remarks>
    Private Function processAddressInfo(ByVal o310 As clsEDI204InLoop310, ByVal oWCFPar As DAL.WCFParameters) As clsAddressInfo

        Dim Addr As New clsAddressInfo
        If o310 Is Nothing Then Return Addr
        'Origin And Destination warehouse ID values in N104 map to NGL Location ID Or Alpha Codes.  The Alpha Code in NGL Is compared first followed by the Company Number, if a match Is Not found for the Alpha Code.
        Addr.CompAbrev = Me.EDI204InSetting.MappingRules.CompAbrev?.Trim()
        Addr.CompLocationID = o310.N1.N104?.Trim()
        Addr.CompAlphaCode = Addr.CompAbrev & o310.N1.N104?.Trim()
        If String.IsNullOrEmpty(Addr.CompLegalEntity) Then
            Addr.CompLegalEntity = Me.EDI204InSetting.MappingRules.CompLegalEntity
        End If
        Addr.AddrName = o310.N1.N102?.Trim
        If Not o310.N3s Is Nothing AndAlso o310.N3s.Count() > 0 Then
            Addr.Addr1 = o310.N3s(0).N301?.Trim
            Addr.Addr2 = o310.N3s(0).N302?.Trim
        End If
        Addr.City = o310.N4.N401?.Trim
        Addr.State = o310.N4.N402?.Trim
        Addr.Zip = o310.N4.N403?.Trim
        Addr.Country = o310.N4.N404?.Trim
        If String.IsNullOrWhiteSpace(Addr.Country) Then Addr.Country = "US"
        If o310.G61s?.Count() > 0 Then
            For Each g61 In o310.G61s
                If Not String.IsNullOrWhiteSpace(g61.G6102?.Trim) Then
                    Addr.ContactName = g61.G6102?.Trim
                End If
                Select Case g61.G6103?.Trim
                    Case "FX"
                        Addr.ContactFAX = g61.G6104?.Trim
                    Case "TE"
                        Addr.ContactPhone = g61.G6104?.Trim
                    Case "EX"
                        Addr.ContactPhoneExt = g61.G6104?.Trim
                End Select
            Next
        End If
        Dim comp = getEDI204InCompInfo(Addr)
        If Not comp Is Nothing Then
            Addr.CompNumber = comp.CompNumber
            Addr.CompLegalEntity = comp.CompLegalEntity

        End If
        Return Addr
    End Function

    ''' <summary>
    ''' process the 204 inbound 300 loop data
    ''' </summary>
    ''' <param name="oHeader"></param>
    ''' <param name="Loop300"></param>
    ''' <param name="statusCode"></param>
    ''' <param name="archived"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Function processLoop300s(ByRef oHeader As clsHeaderInfo, ByVal Loop300 As List(Of clsEDI204InLoop300),
                                     ByRef statusCode As DTO.tblLoadTender.LoadTenderStatusCodeEnum,
                                     ByRef archived As Boolean) As Boolean
        Dim blnRet As Boolean = False
        Try
            If Loop300?.Count() > 0 Then
                For Each o300 In Loop300
                    processG62s(o300.G62s, oHeader.HeaderShipDates)
                    If Not o300.AT5s Is Nothing AndAlso o300.AT5s.Count() > 0 Then
                        oHeader.HazmatCode = o300.AT5s(0).AT501?.Trim()
                    End If
                    If o300.S5.S502 = "LD" Or o300.S5.S502 = "CL" Then
                        If Not o300.NTEs Is Nothing AndAlso o300.NTEs.Count() > 0 Then
                            For Each n In NTEs
                                If Len(If(String.IsNullOrWhiteSpace(oHeader.ShipInstructions), "", oHeader.ShipInstructions.Trim())) + Len(If(String.IsNullOrWhiteSpace(n.NTE01), "", n.NTE01.Trim())) < 255 Then
                                    oHeader.ShipInstructions &= If(String.IsNullOrWhiteSpace(n.NTE01), "", n.NTE01.Trim())
                                Else
                                    Exit For
                                End If
                                If Len(If(String.IsNullOrWhiteSpace(oHeader.ShipInstructions), "", oHeader.ShipInstructions.Trim())) + Len(If(String.IsNullOrWhiteSpace(n.NTE02), "", n.NTE02.Trim())) < 255 Then
                                    oHeader.ShipInstructions &= If(String.IsNullOrWhiteSpace(n.NTE02), "", n.NTE02.Trim())
                                Else
                                    Exit For
                                End If
                            Next
                        End If
                        If Not o300.Loop310 Is Nothing AndAlso o300.Loop310.Count() > 0 Then
                            If oHeader.OrigAdd Is Nothing Then
                                oHeader.OrigAdd = get310LoopAddress(o300.Loop310(0))
                            Else
                                Dim strMsg As String = ""
                                If Not Does310LoopAddressMatch(o300.Loop310(0), oHeader.OrigAdd, strMsg, True) Then
                                    oHeader.strErrors.Add("Cannot accecpt loads with more than one pickup address. Load tender is rejected because: " & strMsg)
                                    archived = True
                                    statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                                End If
                            End If
                        End If
                        ''add the items for the pickup only
                        'If Not o300.Loop350 Is Nothing AndAlso o300.Loop350.Count() > 0 Then
                        '    For Each o350 In o300.Loop350
                        '        Dim item As New clsItemDetails()
                        '        With item
                        '            .ItemPONumber = oHeader.OrderNumber
                        '            .ItemCost = 0
                        '            .ItemCompNumber = oHeader.OrigAdd.CompNumber
                        '            .Temp = oHeader.temp
                        '            If Not o350.OID Is Nothing Then
                        '                .ItemNumber = o350.OID.OID01?.Trim()
                        '                If o350.OID.OID04 = "CA" Then
                        '                    Integer.TryParse(o350.OID.OID05?.Trim(), .Qty)
                        '                Else
                        '                    .Qty = 1
                        '                End If
                        '                If o350.OID.OID06 = "L" Then
                        '                    Double.TryParse(o350.OID.OID07?.Trim(), .Wgt)
                        '                Else
                        '                    .Wgt = 0
                        '                End If
                        '            End If
                        '            If o350.LADs?.Count() > 0 Then
                        '                If Len(item.Description?.Trim()) + Len(o350.LADs(0)?.LAD13?.Trim()) < 255 Then
                        '                    item.Description &= o350.LADs(0).LAD13?.Trim()
                        '                End If
                        '            End If
                        '            processLoop360s(item, o350.Loop360, oHeader.HazmatCode)
                        '            If item.Qty = 0 Then item.Qty = 1
                        '        End With
                        '        oHeader.oItems.Add(item)
                        '    Next
                        'End If
                    ElseIf o300.S5.S502 = "UL" Or o300.S5.S502 = "CU" Then
                        If Not o300.NTEs Is Nothing AndAlso o300.NTEs.Count() > 0 Then
                            For Each n In NTEs
                                If Len(If(String.IsNullOrWhiteSpace(oHeader.laneComments), "", oHeader.laneComments.Trim())) + Len(If(String.IsNullOrWhiteSpace(n.NTE01), "", n.NTE01.Trim())) < 255 Then
                                    oHeader.laneComments &= If(String.IsNullOrWhiteSpace(n.NTE01), "", n.NTE01.Trim())
                                Else
                                    Exit For
                                End If
                                If Len(If(String.IsNullOrWhiteSpace(oHeader.laneComments), "", oHeader.laneComments.Trim())) + Len(If(String.IsNullOrWhiteSpace(n.NTE02), "", n.NTE02.Trim())) < 255 Then
                                    oHeader.laneComments &= If(String.IsNullOrWhiteSpace(n.NTE02), "", n.NTE02.Trim())
                                Else
                                    Exit For
                                End If
                            Next
                        End If
                        If Not o300.Loop310 Is Nothing AndAlso o300.Loop310.Count() > 0 Then
                            If oHeader.DestAdd Is Nothing Then
                                oHeader.DestAdd = get310LoopAddress(o300.Loop310(0))
                            Else
                                Dim strMsg As String = ""
                                If Not Does310LoopAddressMatch(o300.Loop310(0), oHeader.DestAdd, strMsg, False) Then
                                    oHeader.strErrors.Add("Cannot accecpt loads with more than one delivery address.  Load tender is rejected because: " & strMsg)
                                    archived = True
                                    statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                                End If
                            End If
                        End If
                        'add the items for the delivery only
                        If Not o300.Loop350 Is Nothing AndAlso o300.Loop350.Count() > 0 Then
                            For Each o350 In o300.Loop350
                                Dim item As New clsItemDetails()
                                With item
                                    .ItemPONumber = oHeader.OrderNumber
                                    .ItemCost = 0
                                    .ItemCompNumber = oHeader.OrigAdd.CompNumber
                                    .Temp = oHeader.temp
                                    If Not o300.L11s Is Nothing AndAlso o300.L11s.Count() > 0 Then
                                        For Each l In o300.L11s
                                            If l.L1102 = "PO" Then
                                                .POItemCustomerPO = l.L1101
                                                Exit For
                                            End If
                                        Next
                                    End If
                                    If Not o300.Loop310 Is Nothing AndAlso o300.Loop310.Count() > 0 AndAlso Not o300.Loop310(0).N1 Is Nothing Then
                                        .POItemLocationCode = o300.Loop310(0).N1.N104
                                    End If
                                    If Not o350.OID Is Nothing Then
                                        .ItemNumber = If(String.IsNullOrWhiteSpace(o350.OID.OID01), "", o350.OID.OID01.Trim())
                                        If o350.OID.OID04 = "CA" Then
                                            Integer.TryParse(If(String.IsNullOrWhiteSpace(o350.OID.OID05), "", o350.OID.OID05.Trim()), .Qty)
                                        Else
                                            .Qty = 1
                                        End If
                                        If o350.OID.OID06 = "L" Then
                                            Double.TryParse(If(String.IsNullOrWhiteSpace(o350.OID.OID07), "", o350.OID.OID07.Trim()), .Wgt)
                                        Else
                                            .Wgt = 0
                                        End If
                                    End If
                                    If Not o350 Is Nothing AndAlso o350.LADs.Count() > 0 Then
                                        If Len(If(String.IsNullOrWhiteSpace(item.Description), "", item.Description.Trim())) + Len(If(String.IsNullOrWhiteSpace(o350.LADs(0).LAD13), "", o350.LADs(0).LAD13.Trim())) < 254 Then
                                            If Not String.IsNullOrWhiteSpace(item.Description) Then item.Description &= " "
                                            item.Description &= If(String.IsNullOrWhiteSpace(o350.LADs(0).LAD13), "", o350.LADs(0).LAD13.Trim())
                                        End If
                                    End If
                                    processLoop360s(item, o350.Loop360, oHeader.HazmatCode)
                                    If item.Qty = 0 Then item.Qty = 1
                                End With
                                oHeader.oItems.Add(item)
                            Next
                        End If
                    End If
                Next
            End If
            blnRet = True
        Catch ex As Exception
            oHeader.strErrors.Add("Cannot process pickup and delivery information because of an unexpected system error: " & ex.Message)
            statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Read the stop data from the 300 loop and add to pickup and dest lists
    ''' </summary>
    ''' <param name="statusCode"></param>
    ''' <param name="strError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-6.0.4.7 on 6/8/2016
    '''   added new logic to split orders by SO number when sending to TMS
    ''' </remarks>
    Private Function processStops(ByRef statusCode As DTO.tblLoadTender.LoadTenderStatusCodeEnum, ByRef strError As String) As Boolean
        Dim blnRet As Boolean = False
        Try

            If Not Loop300 Is Nothing AndAlso Loop300.Count() > 0 Then
                For Each o300 In Loop300
                    Dim oStop As New clsAddressInfo()
                    processG62s(o300.G62s, oStop.StopShipDates)
                    If Not o300.AT5s Is Nothing AndAlso o300.AT5s.Count() > 0 Then
                        oStop.HazmatCode = If(String.IsNullOrWhiteSpace(o300.AT5s(0).AT501), "", o300.AT5s(0).AT501.Trim())
                    End If
                    If Not o300.L11s Is Nothing AndAlso o300.L11s.Count() > 0 Then
                        For Each oL11 As clsEDIL11 In o300.L11s
                            Select Case oL11.L1102
                                Case "CO"
                                    oStop.OrderNumber = oL11.L1101
                                    If Me.SOs Is Nothing Then Me.SOs = New List(Of String)
                                    If Not Me.SOs.Contains(oStop.OrderNumber) Then Me.SOs.Add(oStop.OrderNumber)
                                Case "PO"
                                    oStop.CustomerPO = oL11.L1101
                                    If Me.POs Is Nothing Then Me.POs = New List(Of String)
                                    If Not Me.POs.Contains(oStop.CustomerPO) Then Me.POs.Add(oStop.CustomerPO)
                            End Select
                        Next
                    End If
                    If Not o300.Loop310 Is Nothing AndAlso o300.Loop310.Count() > 0 Then
                        oStop.populateAddressInfo(o300.Loop310(0), Me.EDI204InSetting)
                    End If
                    If Not o300.Loop350 Is Nothing AndAlso o300.Loop350.Count() > 0 Then
                        For Each o350 In o300.Loop350
                            Dim item As New clsItemDetails()
                            With item
                                .ItemPONumber = oStop.OrderNumber
                                .POItemCustomerPO = oStop.CustomerPO
                                .ItemCost = 0
                                If Not o300.Loop310 Is Nothing AndAlso o300.Loop310.Count() > 0 AndAlso Not o300.Loop310(0).N1 Is Nothing Then
                                    .POItemLocationCode = o300.Loop310(0).N1.N104
                                End If
                                If Not o350.OID Is Nothing Then
                                    .ItemNumber = If(String.IsNullOrWhiteSpace(o350.OID.OID01), "", o350.OID.OID01.Trim())
                                    If o350.OID.OID04 = "CA" Then
                                        Integer.TryParse(If(String.IsNullOrWhiteSpace(o350.OID.OID05), "", o350.OID.OID05.Trim()), .Qty)
                                    Else
                                        .Qty = 0
                                    End If
                                    If o350.OID.OID06 = "L" Then
                                        Double.TryParse(If(String.IsNullOrWhiteSpace(o350.OID.OID07), "", o350.OID.OID07.Trim()), .Wgt)
                                    Else
                                        .Wgt = 0
                                    End If
                                End If
                                If Not o350 Is Nothing AndAlso o350.LADs.Count() > 0 Then
                                    If Len(If(String.IsNullOrWhiteSpace(item.Description), "", item.Description.Trim())) + Len(If(String.IsNullOrWhiteSpace(o350.LADs(0).LAD13), "", o350.LADs(0).LAD13.Trim())) < 254 Then
                                        If Not String.IsNullOrWhiteSpace(item.Description) Then item.Description &= " "
                                        item.Description &= If(String.IsNullOrWhiteSpace(o350.LADs(0).LAD13), "", o350.LADs(0).LAD13.Trim())
                                    End If
                                End If
                                processLoop360s(item, o350.Loop360, oStop.HazmatCode)
                                If item.Qty = 0 Then item.Qty = 1
                            End With
                            oStop.oItems.Add(item)
                        Next
                    End If
                    If o300.S5.S502 = "LD" Or o300.S5.S502 = "CL" Then
                        oStop.Pickup = True
                        'check if this is an outbound load and we do not have a default company number
                        If Not Me.blnInbound AndAlso Me.OutboundCompNumber = 0 Then
                            'see if the company exists
                            Dim comp = getEDI204InCompInfo(oStop)
                            If comp Is Nothing OrElse comp.CompNumber = 0 Then
                                'we need to create this company because the pickup does not exist
                                createNewCompany(oStop, Me.intVersion, Me.EDI204InSetting.MappingRules.CompLegalEntity)
                                'get the comp data back
                                comp = getEDI204InCompInfo(oStop)
                            End If
                            If Not comp Is Nothing Then
                                oStop.CompNumber = comp.CompNumber.ToString()
                            Else
                                oStop.CompNumber = "0"
                            End If
                        End If

                        Integer.TryParse(o300.S5.S501, oStop.StopSequence)
                        Me.LastPickSequence += 1
                        oStop.pickOrDropOrder = Me.LastPickSequence

                        If Not o300.NTEs Is Nothing AndAlso o300.NTEs.Count() > 0 Then
                            For Each n In NTEs
                                If Len(If(String.IsNullOrWhiteSpace(oStop.ShipInstructions), "", oStop.ShipInstructions.Trim())) + Len(If(String.IsNullOrWhiteSpace(n.NTE01), "", n.NTE01.Trim())) < 255 Then
                                    oStop.ShipInstructions &= If(String.IsNullOrWhiteSpace(n.NTE01), "", n.NTE01.Trim())
                                Else
                                    Exit For
                                End If
                                If Len(If(String.IsNullOrWhiteSpace(oStop.ShipInstructions), "", oStop.ShipInstructions.Trim())) + Len(If(String.IsNullOrWhiteSpace(n.NTE02), "", n.NTE02.Trim())) < 255 Then
                                    oStop.ShipInstructions &= If(String.IsNullOrWhiteSpace(n.NTE02), "", n.NTE02.Trim())
                                Else
                                    Exit For
                                End If
                            Next
                        End If
                    ElseIf o300.S5.S502 = "UL" Or o300.S5.S502 = "CU" Then
                        oStop.Pickup = False
                        'check if this is an inbound load and we do not have a default company number
                        If Me.blnInbound AndAlso Me.InboundCompNumber = 0 Then
                            'see if the company exists
                            Dim comp = getEDI204InCompInfo(oStop)
                            If comp Is Nothing OrElse comp.CompNumber = 0 Then
                                'we need to create this company because the pickup does not exist
                                createNewCompany(oStop, Me.intVersion, Me.EDI204InSetting.MappingRules.CompLegalEntity)
                                'get the comp data back
                                comp = getEDI204InCompInfo(oStop)
                            End If
                            If Not comp Is Nothing Then
                                oStop.CompNumber = comp.CompNumber.ToString()
                            Else
                                oStop.CompNumber = "0"
                            End If
                        End If
                        Integer.TryParse(o300.S5.S501, oStop.StopSequence)
                        Me.LastDropSequence += 1
                        oStop.pickOrDropOrder = Me.LastDropSequence
                        If Not o300.NTEs Is Nothing AndAlso o300.NTEs.Count() > 0 Then
                            For Each n In NTEs
                                If Len(If(String.IsNullOrWhiteSpace(oStop.laneComments), "", oStop.laneComments.Trim())) + Len(If(String.IsNullOrWhiteSpace(n.NTE01), "", n.NTE01.Trim())) < 255 Then
                                    oStop.laneComments &= If(String.IsNullOrWhiteSpace(n.NTE01), "", n.NTE01.Trim())
                                Else
                                    Exit For
                                End If
                                If Len(If(String.IsNullOrWhiteSpace(oStop.laneComments), "", oStop.laneComments.Trim())) + Len(If(String.IsNullOrWhiteSpace(n.NTE02), "", n.NTE02.Trim())) < 255 Then
                                    oStop.laneComments &= If(String.IsNullOrWhiteSpace(n.NTE02), "", n.NTE02.Trim())
                                Else
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                    If oStop.Pickup Then
                        If Me.Pickups Is Nothing Then Me.Pickups = New List(Of clsAddressInfo)
                        Me.Pickups.Add(oStop)
                    Else
                        If Me.DropOffs Is Nothing Then Me.DropOffs = New List(Of clsAddressInfo)
                        Me.DropOffs.Add(oStop)
                    End If
                Next
            End If
            blnRet = True
        Catch ex As Exception
            strError = "Cannot process pickup and delivery information because of an unexpected system error: " & ex.Message
            statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Process the 204 inbound 360 loop
    ''' </summary>
    ''' <param name="item"></param>
    ''' <param name="Loop360"></param>
    ''' <param name="HazmatCode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Function processLoop360s(ByRef item As clsItemDetails, ByVal Loop360 As List(Of clsEDI204InLoop360), Optional ByVal HazmatCode As String = "") As Boolean
        If Not Loop360 Is Nothing AndAlso Loop360.Count() > 0 Then
            For Each o360 In Loop360
                With item
                    If Not o360.L5 Is Nothing Then
                        If Len(If(String.IsNullOrWhiteSpace(.Description), "", .Description.Trim())) + Len(If(String.IsNullOrWhiteSpace(o360.L5.L502), "", o360.L5.L502.Trim())) < 254 Then
                            If Not String.IsNullOrWhiteSpace(.Description) Then .Description &= " "
                            .Description &= If(String.IsNullOrWhiteSpace(o360.L5.L502), "", o360.L5.L502.Trim())
                        End If
                        If .ItemCost = 0 Then
                            Integer.TryParse(If(String.IsNullOrWhiteSpace(o360.L5.L506), "", o360.L5.L506.Trim()), .ItemCost)
                        End If
                        .LotNumber = If(String.IsNullOrWhiteSpace(o360.L5.L507), "", o360.L5.L507.Trim())
                    End If
                    'Modified by RHR for v-6.0.4.7 on 6/8/2017 
                    'changed palles and qty mapping  qty maps to AT804 and Pallets maps to AT805
                    If Not o360.AT8 Is Nothing Then
                        If .Wgt = 0 Then Double.TryParse(If(String.IsNullOrWhiteSpace(o360.AT8.AT803), "", o360.AT8.AT803.Trim()), .Wgt)
                        Double.TryParse(If(String.IsNullOrWhiteSpace(o360.AT8.AT805), "", o360.AT8.AT805.Trim()), .Plts)
                        If .Qty = 0 Then Integer.TryParse(If(String.IsNullOrWhiteSpace(o360.AT8.AT804), "", o360.AT8.AT804.Trim()), .Qty)
                        .Cubes = Integer.TryParse(If(String.IsNullOrWhiteSpace(o360.AT8.AT807), "", o360.AT8.AT807.Trim()), .Cubes)
                    End If
                End With
                If Not o360.Loop365 Is Nothing AndAlso o360.Loop365.Count() > 0 Then
                    'in v-7.0.5.106 we do not have a place to put the Hazmat contact informaiton.
                    'but we do need to set the items hazmat info
                    item.HazmatCode = HazmatCode
                    'additional information may be available in the o370 loop about the hazmat product
                    'but for now we do not have anywhere to put it.
                End If
            Next
        End If
    End Function

    ''' <summary>
    ''' Deprecated version used to get 310 loop address informaiton  use the new get100LoopAddress instead
    ''' </summary>
    ''' <param name="Loop100"></param>
    ''' <param name="N101"></param>
    ''' <param name="oWCFPar"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Deprecated by RHR for v-7.0.6.105 on 5/22/2017
    '''   use the new get100LoopAddress method instead
    ''' </remarks>
    Private Function processLoop100(ByVal Loop100 As List(Of clsEDI204Loop100), ByVal N101 As String, ByVal oWCFPar As DAL.WCFParameters) As clsAddressInfo
        Dim Addr As New clsAddressInfo
        If Loop100 Is Nothing OrElse Loop100.Count() < 1 Then Return Addr
        For Each o100 In Loop100
            If o100.N1.N101 = N101 Then
                Addr.CompAbrev = If(String.IsNullOrWhiteSpace(Me.EDI204InSetting.MappingRules.CompAbrev), "", Me.EDI204InSetting.MappingRules.CompAbrev.Trim())
                Addr.CompLocationID = If(String.IsNullOrWhiteSpace(o100.N1.N104), "", o100.N1.N104.Trim())
                Addr.CompAlphaCode = Addr.CompAbrev & If(String.IsNullOrWhiteSpace(o100.N1.N104), "", o100.N1.N104.Trim())
                If String.IsNullOrEmpty(Addr.CompLegalEntity) Then
                    Addr.CompLegalEntity = Me.EDI204InSetting.MappingRules.CompLegalEntity
                End If
                Addr.AddrName = If(String.IsNullOrWhiteSpace(o100.N1.N102), "", o100.N1.N102.Trim())
                Addr.Addr1 = If(String.IsNullOrWhiteSpace(o100.N3.N301), "", o100.N3.N301.Trim())
                Addr.Addr2 = If(String.IsNullOrWhiteSpace(o100.N3.N302), "", o100.N3.N302.Trim())
                Addr.City = If(String.IsNullOrWhiteSpace(o100.N4.N401), "", o100.N4.N401.Trim())
                Addr.State = If(String.IsNullOrWhiteSpace(o100.N4.N402), "", o100.N4.N402.Trim())
                Addr.Zip = If(String.IsNullOrWhiteSpace(o100.N4.N403), "", o100.N4.N403.Trim())
                Addr.Country = If(String.IsNullOrWhiteSpace(o100.N4.N404), "", o100.N4.N404.Trim())
                Dim comp = getEDI204InCompInfo(Addr)
                If Not comp Is Nothing Then
                    Addr.CompNumber = comp.CompNumber
                    'Addr.CompLegalEntity = comp.CompLegalEntity
                End If
                Exit For
            End If
        Next
        Return Addr
    End Function

    Public Function getEDI204InCompInfo(ByRef oAddress As clsAddressInfo) As DTO.Comp
        Dim NGLCompData As New DAL.NGLCompData(DALParameters)
        Dim compNumber = NGLCompData.GetCompNumberByAlpha(oAddress.CompAlphaCode)
        If compNumber <> 0 Then
            Dim comp = NGLCompData.GetCompFiltered(Number:=compNumber)
            Return comp
        Else
            Dim comp = NGLCompData.GetCompFilteredByAddress(oAddress.CompAlphaCode, oAddress.CompAbrev, oAddress.Addr1, oAddress.City, oAddress.State, oAddress.Zip, oAddress.CompLegalEntity)
            Return comp
        End If
    End Function

    ''' <summary>
    ''' For each 200 Loop Item read the equipment details including temperature and Mode type.
    ''' </summary>
    ''' <param name="oHeader"></param>
    ''' <remarks>
    ''' Created by RHR on 5/10/2017 for v-7.0.6.105
    ''' </remarks>
    Private Sub process200Loop(ByRef oHeader As clsHeaderInfo)
        If Me.Loop200?.Count() > 0 Then
            For Each item In Me.Loop200
                If Not item?.N7 Is Nothing Then
                    Dim strEquipDetails = item?.N7?.N711?.Trim().ToUpper()
                    If Not String.IsNullOrWhiteSpace(strEquipDetails) Then
                        Select Case strEquipDetails
                            Case "F"
                                oHeader.temp = "F"
                            Case "FF"
                                oHeader.temp = "F"
                            Case "R"
                                oHeader.temp = "R"
                            Case "RT"
                                oHeader.temp = "R"
                            Case "D"
                                oHeader.temp = "D"
                            Case "BK"
                                oHeader.temp = "D"
                            Case "CN"
                                oHeader.temp = "D"
                            Case "DD"
                                oHeader.temp = "D"
                            Case "FR"
                                oHeader.temp = "D"
                            Case "FT"
                                oHeader.temp = "D"
                            Case "M"
                                oHeader.temp = "M"
                            Case "H"
                                oHeader.temp = "H"
                            Case "G"
                                oHeader.temp = "G"
                            Case "TL"
                                oHeader.temp = "G"
                            Case "TV"
                                oHeader.temp = "G"
                            Case "U"
                                oHeader.temp = "U"
                            Case "C"
                                oHeader.temp = "C"
                            Case Else
                                Dim intModeType As Integer
                                If Integer.TryParse(strEquipDetails, intModeType) Then
                                    If intModeType > 0 And intModeType < 6 Then
                                        oHeader.ModeTypeControl = intModeType
                                    End If
                                End If
                        End Select
                    End If
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Only populates the parameter variables if they do not yet have a value.
    ''' This is so that the order of precedence is maintained and data doesn't get overwritten
    ''' by a lower priority set
    ''' Order of Preference -- Destination , Origin
    ''' </summary>
    ''' <param name="G62s"></param>
    ''' <param name="EDIShipDates"></param>
    ''' <remarks>
    ''' Created by RHR on 5/10/2017 for v-7.0.6.105
    '''   this overload uses a header objec to store the data
    '''   the header object uses strings to store dates
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Sub processG62s(ByVal G62s As List(Of clsEDIG62),
                            ByRef EDIShipDates As clsEDIShipDates)

        If Not G62s Is Nothing AndAlso G62s.Count > 0 Then
            For Each oG62 As clsEDIG62 In G62s
                Dim ReqDt As New Date
                Dim ReqTime As New Date
                Dim ShipDt As New Date
                Dim ShipTime As New Date
                Dim blnReqDateChanged As Boolean = False
                Dim blnShipDateChanged As Boolean = False

                Dim dtTry As Date
                Select Case If(String.IsNullOrWhiteSpace(oG62.G6201), "", oG62.G6201.Trim())
                    Case "02"
                        'ReqDate
                        If String.IsNullOrWhiteSpace(EDIShipDates.ReqDate) Then
                            If Date.TryParse(NDT.convertEDIDateToDateString(If(String.IsNullOrWhiteSpace(oG62.G6202), "", oG62.G6202.Trim())), dtTry) Then
                                ReqDt = dtTry
                                blnReqDateChanged = True
                            End If
                        End If
                    Case "10"
                        'ShipDate
                        If String.IsNullOrWhiteSpace(EDIShipDates.ShipDate) Then
                            If Date.TryParse(NDT.convertEDIDateToDateString(If(String.IsNullOrWhiteSpace(oG62.G6202), "", oG62.G6202.Trim())), dtTry) Then
                                ShipDt = dtTry
                                blnShipDateChanged = True
                            End If
                        End If
                    Case "45"
                        'Notified = Order Date
                        If String.IsNullOrWhiteSpace(EDIShipDates.OrderDate) Then
                            If Date.TryParse(NDT.convertEDIDateToDateString(If(String.IsNullOrWhiteSpace(oG62.G6202), "", oG62.G6202.Trim())), dtTry) Then
                                EDIShipDates.OrderDate = dtTry.ToShortDateString()
                            End If
                        End If
                    Case "69"
                        'SchedulePUDate
                        If String.IsNullOrWhiteSpace(EDIShipDates.SchedulePUDate) Then
                            If Date.TryParse(NDT.convertEDIDateToDateString(If(String.IsNullOrWhiteSpace(oG62.G6202), "", oG62.G6202.Trim())), dtTry) Then
                                EDIShipDates.SchedulePUDate = dtTry.ToShortDateString()
                            End If
                        End If
                    Case "68"
                        'ReqDate
                        If String.IsNullOrWhiteSpace(EDIShipDates.ReqDate) Then
                            If Date.TryParse(NDT.convertEDIDateToDateString(If(String.IsNullOrWhiteSpace(oG62.G6202), "", oG62.G6202.Trim())), dtTry) Then
                                ReqDt = dtTry
                                blnReqDateChanged = True
                            End If
                        End If
                    Case "70"
                        'ScheduleDelDate 
                        If String.IsNullOrWhiteSpace(EDIShipDates.ScheduleDelDate) Then
                            If Date.TryParse(NDT.convertEDIDateToDateString(If(String.IsNullOrWhiteSpace(oG62.G6202), "", oG62.G6202.Trim())), dtTry) Then
                                EDIShipDates.ScheduleDelDate = dtTry.ToShortDateString()
                            End If
                        End If
                End Select

                Dim tmTry As Date
                Select Case If(String.IsNullOrWhiteSpace(oG62.G6203), "", oG62.G6203.Trim())
                    Case "U"
                        'SchedulePUTime
                        If String.IsNullOrWhiteSpace(EDIShipDates.SchedulePUTime) Then
                            If Date.TryParse(NDT.convertEDITimeToDateString(If(String.IsNullOrWhiteSpace(oG62.G6204), "", oG62.G6204.Trim())), tmTry) Then
                                EDIShipDates.SchedulePUTime = tmTry.ToShortTimeString()
                            End If
                        End If
                    Case "X"
                        'ScheduleDelTime
                        If String.IsNullOrWhiteSpace(EDIShipDates.ScheduleDelTime) Then
                            If Date.TryParse(NDT.convertEDITimeToDateString(If(String.IsNullOrWhiteSpace(oG62.G6204), "", oG62.G6204.Trim())), tmTry) Then
                                EDIShipDates.ScheduleDelTime = tmTry.ToShortTimeString()
                            End If
                        End If
                    Case "Y"
                        'ShipTime
                        If blnShipDateChanged Then
                            If Date.TryParse(NDT.convertEDITimeToDateString(If(String.IsNullOrWhiteSpace(oG62.G6204), "", oG62.G6204.Trim())), tmTry) Then
                                ShipTime = tmTry
                            End If
                        End If
                    Case "Z"
                        'ReqTime
                        If blnReqDateChanged Then
                            If Date.TryParse(NDT.convertEDITimeToDateString(If(String.IsNullOrWhiteSpace(oG62.G6204), "", oG62.G6204.Trim())), tmTry) Then
                                ReqTime = tmTry
                            End If
                        End If
                End Select
                If blnReqDateChanged Then EDIShipDates.ReqDate = ReqDt.ToShortDateString & " " & ReqTime.ToShortTimeString()
                If blnShipDateChanged Then EDIShipDates.ShipDate = ShipDt.ToShortDateString & " " & ShipTime.ToShortTimeString()
            Next
        End If
    End Sub


    ''' <summary>
    ''' Only populates the parameter variables if they do not yet have a value.
    ''' This is so that the order of precedence is maintained and data doesn't get overwritten
    ''' by a lower priority set
    ''' Order of Preference -- Destination , Origin
    ''' </summary>
    ''' <param name="G62s"></param>
    ''' <param name="ReqDate"></param>
    ''' <param name="ShipDate"></param>
    ''' <param name="SchedulePUDate"></param>
    ''' <param name="ScheduleDelDate"></param>
    ''' <param name="SchedulePUTime"></param>
    ''' <param name="ScheduleDelTime"></param>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Sub processG62s(ByVal G62s As List(Of clsEDIG62),
                            ByRef ReqDate As Date, ByRef ShipDate As Date,
                            ByRef SchedulePUDate As Date, ByRef ScheduleDelDate As Date,
                            ByRef SchedulePUTime As Date, ByRef ScheduleDelTime As Date)

        If Not G62s Is Nothing AndAlso G62s.Count > 0 Then
            For Each oG62 In G62s
                Dim ReqDt As New Date
                Dim ReqTime As New Date
                Dim ShipDt As New Date
                Dim ShipTime As New Date

                Dim dtTry As Date
                Select Case If(String.IsNullOrWhiteSpace(oG62.G6201), "", oG62.G6201.Trim())
                    Case "02"
                        'ReqDate
                        If ReqDate = Nothing Then
                            If Date.TryParse(NDT.convertEDIDateToDateString(If(String.IsNullOrWhiteSpace(oG62.G6202), "", oG62.G6202.Trim())), dtTry) Then ReqDt = dtTry
                        End If
                    Case "10"
                        'ShipDate
                        If ShipDate = Nothing Then
                            If Date.TryParse(NDT.convertEDIDateToDateString(If(String.IsNullOrWhiteSpace(oG62.G6202), "", oG62.G6202.Trim())), dtTry) Then ShipDt = dtTry
                        End If
                    Case "69"
                        'SchedulePUDate
                        If SchedulePUDate = Nothing Then
                            If Date.TryParse(NDT.convertEDIDateToDateString(If(String.IsNullOrWhiteSpace(oG62.G6202), "", oG62.G6202.Trim())), dtTry) Then SchedulePUDate = dtTry
                        End If
                    Case "68"
                        'LoadDate
                        If ShipDate = Nothing Then
                            If Date.TryParse(NDT.convertEDIDateToDateString(If(String.IsNullOrWhiteSpace(oG62.G6202), "", oG62.G6202.Trim())), dtTry) Then ShipDt = dtTry
                        End If
                    Case "70"
                        'ScheduleDelDate 
                        If ScheduleDelDate = Nothing Then
                            If Date.TryParse(NDT.convertEDIDateToDateString(If(String.IsNullOrWhiteSpace(oG62.G6202), "", oG62.G6202.Trim())), dtTry) Then ScheduleDelDate = dtTry
                        End If
                End Select

                Dim tmTry As Date
                Select Case If(String.IsNullOrWhiteSpace(oG62.G6203), "", oG62.G6203.Trim())
                    Case "U"
                        'SchedulePUTime
                        If SchedulePUTime = Nothing Then
                            If Date.TryParse(NDT.convertEDITimeToDateString(If(String.IsNullOrWhiteSpace(oG62.G6204), "", oG62.G6204.Trim())), tmTry) Then SchedulePUTime = tmTry
                        End If
                    Case "X"
                        'ScheduleDelTime
                        If ScheduleDelTime = Nothing Then
                            If Date.TryParse(NDT.convertEDITimeToDateString(If(String.IsNullOrWhiteSpace(oG62.G6204), "", oG62.G6204.Trim())), tmTry) Then ScheduleDelTime = tmTry
                        End If
                    Case "Y"
                        'ShipTime
                        If ShipDate = Nothing Then
                            If Date.TryParse(NDT.convertEDITimeToDateString(If(String.IsNullOrWhiteSpace(oG62.G6204), "", oG62.G6204.Trim())), tmTry) Then ShipTime = tmTry
                            ShipDate = ShipDt + New TimeSpan(ShipTime.Hour, ShipTime.Minute, ShipTime.Second)
                        End If
                    Case "Z"
                        'ReqTime
                        If ReqDate = Nothing Then
                            If Date.TryParse(NDT.convertEDITimeToDateString(If(String.IsNullOrWhiteSpace(oG62.G6204), "", oG62.G6204.Trim())), tmTry) Then ReqTime = tmTry
                            ReqDate = ReqDt + New TimeSpan(ReqTime.Hour, ReqTime.Minute, ReqTime.Second)
                        End If
                End Select
            Next
        End If
    End Sub

    Private Sub processShipInstructions(ByVal NTEs As List(Of clsEDINTE), ByRef ShipInstructions As String)
        If Not NTEs Is Nothing AndAlso NTEs.Count() <> 0 Then
            For Each nte In NTEs
                If Not nte Is Nothing Then
                    Dim n = If(String.IsNullOrWhiteSpace(nte.NTE01), "", nte.NTE01.Trim()) & "- " & If(String.IsNullOrWhiteSpace(nte.NTE02), "", nte.NTE02.Trim()) & "| "
                    ShipInstructions &= n
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Fill the 705 data 
    ''' </summary>
    ''' <param name="oHeader"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Function fill705Header(ByVal oHeader As clsHeaderInfo) As clsBookHeaderObject705
        Dim order As New clsBookHeaderObject705
        If oHeader Is Nothing Then Return order
        With order
            .PONumber = oHeader.OrderNumber
            .POOrderSequence = oHeader.OrderSequence
            .POVendor = oHeader.LaneNumber 'Lane Number Customer Shipping From – Shipping to Identifier:  NGL will need to generate this number automatically using a special formula that includes the values provided in the N104 data.
            .POCustomerPO = oHeader.PONumber
            .POCarrBLNumber = oHeader.CarrBLNumber
            '------------------------------------
            .POdate = If(String.IsNullOrWhiteSpace(oHeader.OrderDate), Date.Now.ToShortDateString(), oHeader.OrderDate)  'Order Date in ERP system.  NGL will use the date that the 204 was processed on New Orders as the Order Date and will not allow updates to the Order Date on modified orders.
            '------------------------------------

            .POShipdate = oHeader.ShipDate
            'TransCode comes from B211 if provided else we use the inbound flag to set up defaults
            If Not Short.TryParse(oHeader.TransCode, .POFrt) Then
                If oHeader.Inbound Then
                    .POFrt = 5 'inbound vendor delivered
                Else
                    .POFrt = 4 'outbound vendor delivered
                End If
            End If
            .POTotalFrt = 0

            '------------------------------------
            '.POTotalCost = 0
            '------------------------------------

            .POWgt = oHeader.TotalWgt
            .POCube = oHeader.TotalCube
            .POQty = oHeader.TotalQTY
            .POPallets = oHeader.TotalPLTS

            If oHeader.Inbound Then
                If Me.InboundCompNumber <> 0 Then
                    .PODefaultCustomer = Me.InboundCompNumber
                Else
                    .PODefaultCustomer = oHeader.DestAdd.CompNumber
                    .POCompAlphaCode = oHeader.DestAdd.CompAlphaCode
                    .POCompLegalEntity = oHeader.DestAdd.CompLegalEntity
                End If
                'ToDO:  add code to process the default  compalphacode and legal entity?
                .PODefaultCustomer = oHeader.DestAdd.CompNumber
            Else
                If Me.OutboundCompNumber <> 0 Then
                    .PODefaultCustomer = Me.OutboundCompNumber
                Else
                    .PODefaultCustomer = oHeader.OrigAdd.CompNumber
                    .POCompAlphaCode = oHeader.OrigAdd.CompAlphaCode
                    .POCompLegalEntity = oHeader.OrigAdd.CompLegalEntity
                End If
            End If


            .POReqDate = oHeader.ReqDate
            .POShipInstructions = oHeader.ShipInstructions
            .POTemp = oHeader.temp
            .POCustomerPO = oHeader.PONumber
            .POStatusFlag = oHeader.POStatusFlag
            .POOrderSequence = 0 'NGL will use a default value of zero
            .POSchedulePUDate = oHeader.SchedulePUDate
            .POSchedulePUTime = oHeader.SchedulePUTime
            .POScheduleDelDate = oHeader.ScheduleDelDate
            .POSCheduleDelTime = oHeader.ScheduleDelTime
            If Not oHeader.OrigAdd Is Nothing Then
                'Origin company number When creating lanes Or Using an alternate shipping address, 
                'set to zero when using company alpha codes,  
                'If this value is empty and this is an outbound load (POInbound = false)  the
                'system will use  PO Default Customer value by default
                .POOrigCompNumber = oHeader.OrigAdd.CompNumber
                'Origin company legal entity used When creating lanes Or Using an alternate shipping address, 
                'set to empty string when shipping from a non-managed facility,  
                'If this value is empty and this is an outbound load (POInbound = false)  the
                'system will use PO Company Legal Entity value by default
                'Not Stored in POHDR Table,  used for automatic lane generation only
                .POOrigLegalEntity = oHeader.OrigAdd.CompLegalEntity
                .POOrigCompAlphaCode = oHeader.OrigAdd.CompAlphaCode
                .POOrigName = oHeader.OrigAdd.AddrName
                .POOrigAddress1 = oHeader.OrigAdd.Addr1
                .POOrigAddress2 = oHeader.OrigAdd.Addr2
                .POOrigAddress3 = oHeader.OrigAdd.Addr3
                .POOrigCity = oHeader.OrigAdd.City
                .POOrigState = oHeader.OrigAdd.State
                .POOrigCountry = oHeader.OrigAdd.Country
                .POOrigZip = oHeader.OrigAdd.Zip
                .POOrigContactPhone = oHeader.OrigAdd.ContactPhone
                .POOrigContactPhoneExt = oHeader.OrigAdd.ContactPhoneExt
                .POOrigContactFax = oHeader.OrigAdd.ContactFAX
            End If
            If Not oHeader.DestAdd Is Nothing Then
                'Destination company number When creating lanes Or Using an alternate shipping address, 
                'set to zero when using company alpha codes,  
                'If this value is empty and this is an inbound load (POInbound = true)  the
                'system will use  PO Default Customer value by default
                .PODestCompNumber = oHeader.DestAdd.CompNumber
                'Destination company legal entity used When creating lanes Or Using an alternate shipping address, 
                'set to empty string when shipping to a non-managed facility,  
                'If this value is empty and this is an inbound load (POInbound = true)  the
                'system will use PO Company Legal Entity value by default
                'Not Stored in POHDR Table,  used for automatic lane generation only
                .PODestLegalEntity = oHeader.DestAdd.CompLegalEntity
                .PODestCompAlphaCode = oHeader.DestAdd.CompAlphaCode
                .PODestName = oHeader.DestAdd.AddrName
                .PODestAddress1 = oHeader.DestAdd.Addr1
                .PODestAddress2 = oHeader.DestAdd.Addr2
                .PODestAddress3 = oHeader.DestAdd.Addr3
                .PODestCity = oHeader.DestAdd.City
                .PODestState = oHeader.DestAdd.State
                .PODestCountry = oHeader.DestAdd.Country
                .PODestZip = oHeader.DestAdd.Zip
                .PODestContactPhone = oHeader.DestAdd.ContactPhone
                .PODestContactPhoneExt = oHeader.DestAdd.ContactPhoneExt
                .PODestContactFax = oHeader.DestAdd.ContactFAX
            End If

            .POInbound = oHeader.Inbound 'Inbound vs Outbound is determined by the SF and ST location codes in Loop 310 N104.  If the SF value maps to a qualified NGL shipping warehouse (company) the order is outbound.  If the ST value maps to a qualified NGL Shipping warehouse the order is considered inbound.  If both SF and ST values map to a qualified NGL shipping warehouse the order is outbound.  Be sure to map the correct values to the Loop 310 N104 value
            .POPalletType = oHeader.DefaultPalletType
            .POComments = oHeader.laneComments
            '.POCommentsConfidential = ""
            '.POCompLegalEntity = ""
            .POModeTypeControl = oHeader.ModeTypeControl '3 'Truck
            .ChangeNo = ""
            .POConsigneeNumber = Me.EDI204InSetting.MappingRules.POConsigneeNumber
            Integer.TryParse(Me.EDI204InSetting.MappingRules.PORecMinIn, .PORecMinIn)
            Integer.TryParse(Me.EDI204InSetting.MappingRules.PORecMinOut, .PORecMinOut)
            Integer.TryParse(Me.EDI204InSetting.MappingRules.PORecMinUnload, .PORecMinUnload)
            Integer.TryParse(Me.EDI204InSetting.MappingRules.PORecMinIn, .PORecMinIn)
            Boolean.TryParse(Me.EDI204InSetting.MappingRules.POAppt, .POAppt)
            If String.IsNullOrEmpty(Me.EDI204InSetting.MappingRules.POBFC) Then Me.EDI204InSetting.MappingRules.POBFC = "100"
            Double.TryParse(Me.EDI204InSetting.MappingRules.POBFC, .POBFC)
            If String.IsNullOrEmpty(Me.EDI204InSetting.MappingRules.POBFCType) Then Me.EDI204InSetting.MappingRules.POBFCType = "PERC"
            .POBFCType = Me.EDI204InSetting.MappingRules.POBFCType
        End With
        Return order
    End Function

    ''' <summary>
    ''' Fill the 705 item details data
    ''' </summary>
    ''' <param name="items"></param>
    ''' <param name="strWarnings"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Function fill705ItemDetails(ByVal items As List(Of clsItemDetails), ByRef strWarnings As String) As List(Of clsBookDetailObject705)
        Dim oDetails As New List(Of clsBookDetailObject705)
        If items Is Nothing OrElse items.Count < 1 Then
            strWarnings &= "  Item details are missing or empty."
            Return oDetails
        End If
        For Each item In items


            Dim detail As New clsBookDetailObject705
            With detail
                .ItemPONumber = item.ItemPONumber
                .POOrderSequence = item.OrderSequence
                .ItemNumber = item.ItemNumber
                If Not Integer.TryParse(item.Qty, .QtyOrdered) Then
                    strWarnings &= "  Failed To process quantity of items orderd,  the value," & item.Qty & " Is not a valid whole number. "
                End If
                .FreightCost = 0 'set to zero; Item level BFC is not available vie EDI 204
                If Not Double.TryParse(NDT.FormatEDICurrencyToDouble(item.ItemCost), .ItemCost) Then
                    strWarnings &= "  Failed To process Item Cost. the value," & item.ItemCost & " Is not a valid currency number. "
                Else

                End If
                If Not Double.TryParse(item.Wgt, .Weight) Then
                    strWarnings &= "  Failed To process Item weight. the value," & item.Wgt & " Is not a valid number. "
                End If
                Integer.TryParse(item.Cubes, .Cube)
                .Pack = 0 'set to NULL or current value no updates allowed via EDI 204
                .Size = "" 'set to NULL or current value no updates allowed via EDI 204
                .Description = item.Description
                If Not String.IsNullOrWhiteSpace(item.HazmatCode) Then
                    .Hazmat = "H"
                End If

                .Brand = "" 'set to NULL or current value no updates allowed via EDI 204
                .LotNumber = item.LotNumber
                '.CustItemNumber = ""
                '.CustomerNumber = ""
                .POOrderSequence = 0 'NGL will use a default value of zero
                '.PalletType = ""
                '.POItemHazmatTypeCode = ""
                Double.TryParse(item.Plts, .POItemPallets)
                .BookItemCommCode = item.Temp
                .CustomerNumber = item.ItemCompNumber
                .POItemCustomerPO = item.POItemCustomerPO
                .POItemLocationCode = item.POItemLocationCode
            End With
            oDetails.Add(detail)
        Next
        Return oDetails
    End Function

    ''' <summary>
    ''' Overload that populates a clsBookHeaderObject60 object with clsHeaderInfo
    ''' </summary>
    ''' <param name="oHeader"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/12/2015
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Function fill604Header(ByVal oHeader As clsHeaderInfo) As clsBookHeaderObject604

        Dim order As New clsBookHeaderObject604()
        If oHeader Is Nothing Then Return order
        With order
            .PONumber = oHeader.OrderNumber
            .POOrderSequence = oHeader.OrderSequence
            .POVendor = oHeader.LaneNumber 'Lane Number Customer Shipping From – Shipping to Identifier:  NGL will need to generate this number automatically using a special formula that includes the values provided in the N104 data.
            .POCustomerPO = oHeader.PONumber
            .POCarrBLNumber = oHeader.CarrBLNumber
            '------------------------------------
            .POdate = If(String.IsNullOrWhiteSpace(oHeader.OrderDate), Date.Now.ToShortDateString(), oHeader.OrderDate)  'Order Date in ERP system.  NGL will use the date that the 204 was processed on New Orders as the Order Date and will not allow updates to the Order Date on modified orders.
            '------------------------------------

            .POShipdate = oHeader.ShipDate
            'TransCode comes from B211 if provided else we use the inbound flag to set up defaults
            If Not Short.TryParse(oHeader.TransCode, .POFrt) Then
                If oHeader.Inbound Then
                    .POFrt = 5 'inbound vendor delivered
                Else
                    .POFrt = 4 'outbound vendor delivered
                End If
            End If

            .POTotalFrt = 0

            '------------------------------------
            '.POTotalCost = 0
            '------------------------------------

            .POWgt = oHeader.TotalWgt
            .POCube = oHeader.TotalCube
            .POQty = oHeader.TotalQTY
            .POPallets = oHeader.TotalPLTS

            If oHeader.Inbound Then
                If Me.InboundCompNumber <> 0 Then
                    .PODefaultCustomer = Me.InboundCompNumber
                Else
                    .PODefaultCustomer = oHeader.DestAdd.CompNumber
                End If
            Else
                If Me.OutboundCompNumber <> 0 Then
                    .PODefaultCustomer = Me.OutboundCompNumber
                Else
                    .PODefaultCustomer = oHeader.OrigAdd.CompNumber
                End If
            End If

            .POReqDate = oHeader.ReqDate
            .POShipInstructions = oHeader.ShipInstructions
            .POTemp = oHeader.temp
            .POCustomerPO = oHeader.PONumber
            .POStatusFlag = oHeader.POStatusFlag
            .POOrderSequence = 0 'NGL will use a default value of zero
            .POSchedulePUDate = oHeader.SchedulePUDate
            .POSchedulePUTime = oHeader.SchedulePUTime
            .POScheduleDelDate = oHeader.ScheduleDelDate
            .POSCheduleDelTime = oHeader.ScheduleDelTime
            If Not oHeader.OrigAdd Is Nothing Then
                'Origin company number When creating lanes Or Using an alternate shipping address, 
                'set to zero when using company alpha codes,  
                'If this value is empty and this is an outbound load (POInbound = false)  the
                'system will use  PO Default Customer value by default
                .POOrigCompNumber = oHeader.OrigAdd.CompNumber
                'Origin company legal entity used When creating lanes Or Using an alternate shipping address, 
                'set to empty string when shipping from a non-managed facility,  
                'If this value is empty and this is an outbound load (POInbound = false)  the
                'system will use PO Company Legal Entity value by default
                'Not Stored in POHDR Table,  used for automatic lane generation only
                .POOrigName = oHeader.OrigAdd.AddrName
                .POOrigAddress1 = oHeader.OrigAdd.Addr1
                .POOrigAddress2 = oHeader.OrigAdd.Addr2
                .POOrigAddress3 = oHeader.OrigAdd.Addr3
                .POOrigCity = oHeader.OrigAdd.City
                .POOrigState = oHeader.OrigAdd.State
                .POOrigCountry = oHeader.OrigAdd.Country
                .POOrigZip = oHeader.OrigAdd.Zip
                .POOrigContactPhone = oHeader.OrigAdd.ContactPhone
                .POOrigContactPhoneExt = oHeader.OrigAdd.ContactPhoneExt
                .POOrigContactFax = oHeader.OrigAdd.ContactFAX
            End If
            If Not oHeader.DestAdd Is Nothing Then
                'Destination company number When creating lanes Or Using an alternate shipping address, 
                'set to zero when using company alpha codes,  
                'If this value is empty and this is an inbound load (POInbound = true)  the
                'system will use  PO Default Customer value by default
                .PODestCompNumber = oHeader.DestAdd.CompNumber
                'Destination company legal entity used When creating lanes Or Using an alternate shipping address, 
                'set to empty string when shipping to a non-managed facility,  
                'If this value is empty and this is an inbound load (POInbound = true)  the
                'system will use PO Company Legal Entity value by default
                'Not Stored in POHDR Table,  used for automatic lane generation only
                .PODestName = oHeader.DestAdd.AddrName
                .PODestAddress1 = oHeader.DestAdd.Addr1
                .PODestAddress2 = oHeader.DestAdd.Addr2
                .PODestAddress3 = oHeader.DestAdd.Addr3
                .PODestCity = oHeader.DestAdd.City
                .PODestState = oHeader.DestAdd.State
                .PODestCountry = oHeader.DestAdd.Country
                .PODestZip = oHeader.DestAdd.Zip
                .PODestContactPhone = oHeader.DestAdd.ContactPhone
                .PODestContactPhoneExt = oHeader.DestAdd.ContactPhoneExt
                .PODestContactFax = oHeader.DestAdd.ContactFAX
            End If
            .POInbound = oHeader.Inbound 'Inbound vs Outbound is determined by the SF and ST location codes in Loop 310 N104.  If the SF value maps to a qualified NGL shipping warehouse (company) the order is outbound.  If the ST value maps to a qualified NGL Shipping warehouse the order is considered inbound.  If both SF and ST values map to a qualified NGL shipping warehouse the order is outbound.  Be sure to map the correct values to the Loop 310 N104 value
            .POPalletType = oHeader.DefaultPalletType
            .POComments = oHeader.laneComments
            .POConsigneeNumber = Me.EDI204InSetting.MappingRules.POConsigneeNumber
            Integer.TryParse(Me.EDI204InSetting.MappingRules.PORecMinIn, .PORecMinIn)
            Integer.TryParse(Me.EDI204InSetting.MappingRules.PORecMinOut, .PORecMinOut)
            Integer.TryParse(Me.EDI204InSetting.MappingRules.PORecMinUnload, .PORecMinUnload)
            Integer.TryParse(Me.EDI204InSetting.MappingRules.PORecMinIn, .PORecMinIn)
            Boolean.TryParse(Me.EDI204InSetting.MappingRules.POAppt, .POAppt)
            If String.IsNullOrEmpty(Me.EDI204InSetting.MappingRules.POBFC) Then Me.EDI204InSetting.MappingRules.POBFC = "100"
            Double.TryParse(Me.EDI204InSetting.MappingRules.POBFC, .POBFC)
            If String.IsNullOrEmpty(Me.EDI204InSetting.MappingRules.POBFCType) Then Me.EDI204InSetting.MappingRules.POBFCType = "PERC"
            .POBFCType = Me.EDI204InSetting.MappingRules.POBFCType
        End With
        Return order
    End Function

    ''' <summary>
    ''' Overload the populates a clsBookDetailObject60 object with clsItemDetails data
    ''' </summary>
    ''' <param name="items"></param>
    ''' <param name="strWarnings"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/123/2017
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Function fill60ItemDetails(ByVal items As List(Of clsItemDetails), ByRef strWarnings As String) As List(Of clsBookDetailObject604)
        Dim oDetails As New List(Of clsBookDetailObject604)
        If items Is Nothing OrElse items.Count < 1 Then
            strWarnings &= "  Item details are missing or empty."
            Return oDetails
        End If
        For Each item In items


            Dim detail As New clsBookDetailObject604
            With detail
                .ItemPONumber = item.ItemPONumber
                .POOrderSequence = item.OrderSequence
                .ItemNumber = item.ItemNumber
                If Not Integer.TryParse(item.Qty, .QtyOrdered) Then
                    strWarnings &= "  Failed To process quantity of items orderd,  the value," & item.Qty & " Is not a valid whole number. "
                End If
                .FreightCost = 0 'set to zero; Item level BFC is not available vie EDI 204
                If Not Double.TryParse(NDT.FormatEDICurrencyToDouble(item.ItemCost), .ItemCost) Then
                    strWarnings &= "  Failed To process Item Cost. the value," & item.ItemCost & " Is not a valid currency number. "
                Else

                End If
                If Not Double.TryParse(item.Wgt, .Weight) Then
                    strWarnings &= "  Failed To process Item weight. the value," & item.Wgt & " Is not a valid number. "
                End If
                Integer.TryParse(item.Cubes, .Cube)
                .Pack = 0 'set to NULL or current value no updates allowed via EDI 204
                .Size = "" 'set to NULL or current value no updates allowed via EDI 204
                .Description = item.Description
                If Not String.IsNullOrWhiteSpace(item.HazmatCode) Then
                    .Hazmat = "H"
                End If

                .Brand = "" 'set to NULL or current value no updates allowed via EDI 204
                .LotNumber = item.LotNumber
                '.CustItemNumber = ""
                '.CustomerNumber = ""
                .POOrderSequence = 0 'NGL will use a default value of zero
                '.PalletType = ""
                '.POItemHazmatTypeCode = ""
                Double.TryParse(item.Plts, .POItemPallets)
                .BookItemCommCode = item.Temp
                .CustomerNumber = item.ItemCompNumber
                .POItemCustomerPO = item.POItemCustomerPO
                .POItemLocationCode = item.POItemLocationCode

            End With
            oDetails.Add(detail)
        Next
        Return oDetails
    End Function

    ''' <summary>
    ''' Populates the EDI204In log table with header information
    ''' </summary>
    ''' <param name="oHeader"></param>
    ''' <param name="strMsg"></param>
    ''' <param name="blnHadErrors"></param>
    ''' <param name="statusCode"></param>
    ''' <param name="insertErrorMsg"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="DateProcessed"></param>
    ''' <param name="archived"></param>
    ''' <param name="fileName"></param>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.106 on 5/18/2017
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Sub fillEDI204InTable(ByRef oHeader As clsHeaderInfo,
                                       ByRef strMsg As String,
                                       ByRef blnHadErrors As Boolean,
                                       ByRef statusCode As Integer,
                                       ByRef insertErrorMsg As String,
                                       ByVal CarrierControl As Integer,
                                       ByVal DateProcessed As Date,
                                       ByVal archived As Boolean,
                                       ByVal fileName As String)
        Try

            Dim oCarrierData As New DAL.NGLCarrierData(Me.DALParameters)
            Dim oEDIData As New DAL.NGLEDIData(Me.DALParameters)
            Dim carr As Dictionary(Of String, String)
            Try
                If CarrierControl <> 0 Then
                    carr = oCarrierData.getCarrierNameNumberSCAC(CarrierControl)
                End If
            Catch ex As Exception

            End Try

            Dim strEDIMessage As String = ""
            If Not oHeader.strErrors Is Nothing AndAlso oHeader.strErrors.Count() > 0 Then
                strEDIMessage = "  Errors: " & String.Concat(oHeader.strErrors)
                strMsg = strEDIMessage
                blnHadErrors = True
            End If
            If Not oHeader.strWarnings Is Nothing AndAlso oHeader.strWarnings.Count() > 0 Then
                strEDIMessage &= "  Warnings: " & String.Concat(oHeader.strWarnings)
            End If


            Dim lts204In As New DAL.LTS.tblEDI204In
            With lts204In
#Disable Warning BC42104 ' Variable 'carr' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not carr Is Nothing Then
#Enable Warning BC42104 ' Variable 'carr' is used before it has been assigned a value. A null reference exception could result at runtime.
                    If carr.ContainsKey("CarrierSCAC") Then .CarrierSCAC = carr.Item("CarrierSCAC") Else .CarrierSCAC = "N/A"
                    If carr.ContainsKey("CarrierNumber") Then .CarrierNumber = carr.Item("CarrierNumber") Else .CarrierNumber = 0
                    If carr.ContainsKey("CarrierName") Then .CarrierName = carr.Item("CarrierName") Else .CarrierName = "N/A"
                Else
                    .CarrierSCAC = "N/A"
                    .CarrierNumber = 0
                    .CarrierName = "N/A"
                End If
                .OrderNumber = oHeader.OrderNumber
                .OrderSequence = oHeader.OrderSequence
                .PONumber = oHeader.PONumber
                .TotalCases = oHeader.TotalQTY
                .TotalWgt = oHeader.TotalWgt
                .TotalPL = oHeader.TotalPLTS
                .TotalCube = oHeader.TotalCube
                Dim dtVal As Date
                If Date.TryParse(oHeader.ShipDate, dtVal) Then
                    .ShipDate = dtVal
                End If
                If Date.TryParse(oHeader.ReqDate, dtVal) Then
                    .ReqDate = dtVal
                End If
                If Date.TryParse(oHeader.SchedulePUDate, dtVal) Then
                    .SchedulePUDate = dtVal
                End If
                If Date.TryParse(oHeader.SchedulePUTime, dtVal) Then
                    .SchedulePUTime = dtVal
                End If
                If Date.TryParse(oHeader.ScheduleDelDate, dtVal) Then
                    .ScheduleDelDate = dtVal
                End If
                If Date.TryParse(oHeader.ScheduleDelTime, dtVal) Then
                    .ScheduleDelTime = dtVal
                End If
                If Not oHeader.OrigAdd Is Nothing Then
                    .OrigName = oHeader.OrigAdd.AddrName
                    .OrigAddress1 = oHeader.OrigAdd.Addr1
                    .OrigAddress2 = oHeader.OrigAdd.Addr2
                    .OrigAddress3 = oHeader.OrigAdd.Addr3
                    .OrigCity = oHeader.OrigAdd.City
                    .OrigState = oHeader.OrigAdd.State
                    .OrigCountry = oHeader.OrigAdd.Country
                    .OrigZip = oHeader.OrigAdd.Zip
                    .OrigPhone = oHeader.OrigAdd.ContactPhone
                    .OrigCompanyNumber = oHeader.OrigAdd.CompNumber
                End If
                If Not oHeader.DestAdd Is Nothing Then
                    .DestName = oHeader.DestAdd.AddrName
                    .DestAddress1 = oHeader.DestAdd.Addr1
                    .DestAddress2 = oHeader.DestAdd.Addr2
                    .DestAddress3 = oHeader.DestAdd.Addr3
                    .DestCity = oHeader.DestAdd.City
                    .DestState = oHeader.DestAdd.State
                    .DestCountry = oHeader.DestAdd.Country
                    .DestZip = oHeader.DestAdd.Zip
                    .DestPhone = oHeader.DestAdd.ContactPhone
                    .DestCompanyNumber = oHeader.DestAdd.CompNumber
                End If
                .LaneComments = oHeader.laneComments
                .Inbound = oHeader.Inbound
                .EDI204InStatusCode = statusCode
                .EDI204InMessage = strEDIMessage
                .Archived = archived
                .EDI204InFileName204In = fileName
            End With
            'Insert History record into tblEDI204In
            If Not oEDIData.InsertIntoEDI204In(lts204In, DateProcessed) Then
                insertErrorMsg &= "Could not insert record into tblEDI204In for Order Number: " + oHeader.OrderNumber + ", PONumber: " + oHeader.PONumber + ", SCAC: " + carr.Item("CarrierSCAC") + ", DateProcessed, : " + DateProcessed.ToString + ", and filename: " + fileName & "  " & strEDIMessage
            End If
        Catch ex As Exception
            insertErrorMsg &= "Could not insert record into tblEDI204In for Order Number: " + oHeader.OrderNumber + ", PONumber: " + oHeader.PONumber + ", DateProcessed, : " + DateProcessed.ToString + ", and filename: " + fileName & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Imports the EDI data into the TMS system using the standard booking integration for the current version
    ''' </summary>
    ''' <param name="oHeader"></param>
    ''' <param name="statusCode"></param>
    ''' <param name="intVersion"></param>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.106 on 5/18/2017
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Sub saveBookingData(ByRef oHeader As clsHeaderInfo,
                                ByRef statusCode As DTO.tblLoadTender.LoadTenderStatusCodeEnum,
                                ByVal intVersion As Integer)
        Try
            Dim book As New Ngl.FreightMaster.Integration.clsBook(Me.oConfig)
            Dim oOrders As New List(Of clsBookHeaderObject705)
            Dim oOrder As New clsBookHeaderObject705()
            Dim oDetails As New List(Of clsBookDetailObject705)
            Dim o604Ords As New List(Of clsBookHeaderObject604)
            Dim o604Ord As New clsBookHeaderObject604()
            Dim o60Dets As New List(Of clsBookDetailObject604)

            Dim swarnings As String = ""

            If intVersion = 6 Then
                o60Dets = fill60ItemDetails(oHeader.oItems, swarnings)
                If Not String.IsNullOrWhiteSpace(swarnings) Then oHeader.strWarnings.Add(swarnings)
                If o60Dets Is Nothing OrElse o60Dets.Count() < 1 Then
                    oHeader.TotalCube = 0
                    oHeader.TotalQTY = 1
                    oHeader.TotalWgt = 1
                Else
                    oHeader.TotalCube = o60Dets.Sum(Function(x) x.Cube)
                    oHeader.TotalQTY = o60Dets.Sum(Function(x) x.QtyOrdered)
                    oHeader.TotalWgt = o60Dets.Sum(Function(x) x.Weight)
                    Dim dblPLTS As Double = o60Dets.Sum(Function(x) x.POItemPallets)
                    If dblPLTS = 0 Then
                        'allocate pallets
                        allocatePalletsToItems(oHeader.TotalPLTS, o60Dets)
                    Else
                        'update header pallets
                        oHeader.TotalPLTS = Math.Ceiling(dblPLTS)
                    End If
                End If

                o604Ord = fill604Header(oHeader)
                If Not o604Ord Is Nothing Then o604Ords.Add(o604Ord)
                'update the key fields in the item details to match the header
                If Not o60Dets Is Nothing AndAlso o60Dets.Count() > 0 Then
                    For Each oDet In o60Dets
                        oDet.POOrderSequence = o604Ord.POOrderSequence
                        oDet.ItemPONumber = o604Ord.PONumber
                        oDet.CustomerNumber = o604Ord.PODefaultCustomer
                    Next
                End If
            Else

                oDetails = fill705ItemDetails(oHeader.oItems, swarnings)
                If Not String.IsNullOrWhiteSpace(swarnings) Then oHeader.strWarnings.Add(swarnings)
                If oDetails Is Nothing OrElse oDetails.Count() < 1 Then
                    oHeader.TotalCube = 0
                    oHeader.TotalQTY = 1
                    oHeader.TotalWgt = 1
                Else
                    oHeader.TotalCube = oDetails.Sum(Function(x) x.Cube)
                    oHeader.TotalQTY = oDetails.Sum(Function(x) x.QtyOrdered)
                    oHeader.TotalWgt = oDetails.Sum(Function(x) x.Weight)
                    Dim dblPLTS As Double = oDetails.Sum(Function(x) x.POItemPallets)
                    If dblPLTS = 0 Then
                        'allocate pallets
                        allocatePalletsToItems(oHeader.TotalPLTS, oDetails)
                    Else
                        'update header pallets
                        oHeader.TotalPLTS = Math.Ceiling(dblPLTS)
                    End If
                End If
                oOrder = fill705Header(oHeader)
                If Not oOrder Is Nothing Then oOrders.Add(oOrder)
                'update the key fields in the item details to match the header
                If Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
                    For Each oDet In oDetails
                        oDet.POOrderSequence = oOrder.POOrderSequence
                        oDet.ItemPONumber = oOrder.PONumber
                        oDet.CustomerNumber = oOrder.PODefaultCustomer
                    Next
                End If
            End If

            Dim intRes As ProcessDataReturnValues
            Dim strLastError As String = ""
            If oHeader.statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Accept Then
                If intVersion = 6 Then
                    If o604Ords?.Count() > 0 Then
                        intRes = book.ProcessObjectData(o604Ords, o60Dets, Me.ConnectionString)
                        strLastError = book.LastError
                    Else
                        intRes = ProcessDataReturnValues.nglDataValidationFailure
                        strLastError = "No Orders Were Found to Process,  Please check the EDI document for errors or missing segments"
                    End If
                Else
                    If oOrders?.Count() > 0 Then
                        intRes = book.ProcessObjectData(oOrders, oDetails, Me.ConnectionString)
                        strLastError = book.LastError
                    Else
                        intRes = ProcessDataReturnValues.nglDataValidationFailure
                        strLastError = "No Orders Were Found to Process,  Please check the EDI document for errors or missing segments"
                    End If
                End If

                Select Case intRes
                    Case ProcessDataReturnValues.nglDataConnectionFailure
                        oHeader.strErrors.Add("  Could not process EDI 204 Order information for," & oHeader.OrderNumber & ", because of a Database Connection Failure: " & strLastError)
                        statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error
                    Case ProcessDataReturnValues.nglDataIntegrationFailure
                        oHeader.strErrors.Add("  Could not process EDI 204 Order information for," & oHeader.OrderNumber & ", because of an integration validation failure: " & strLastError)
                        statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error
                    Case ProcessDataReturnValues.nglDataIntegrationHadErrors
                        oHeader.strErrors.Add("  Could not process EDI 204 Order information for," & oHeader.OrderNumber & ", because of an integration validation failure: " & strLastError)
                        statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error
                    Case ProcessDataReturnValues.nglDataValidationFailure
                End Select

            End If

        Catch ex As Exception
            oHeader.strErrors.Add(" Could not process EDI 204 Order information for," & oHeader.OrderNumber & ", because of an unexpected system error: " & ex.Message)
            statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error
        End Try
    End Sub

    ''' <summary>
    ''' Looks for an existing Lane or creates a new Lane Number if needed.  the save booking routine will create the new Lane if needed.
    ''' </summary>
    ''' <param name="oHeader"></param>
    ''' <param name="statusCode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.106 on 5/18/2017
    ''' Modified by RHR for v-7.0.6.105 on 6/13/2017
    ''' </remarks>
    Private Function identifyLaneNumber(ByRef oHeader As clsHeaderInfo, ByRef statusCode As DTO.tblLoadTender.LoadTenderStatusCodeEnum) As Boolean
        Dim blnRet As Boolean = False
        Try
            Dim olane As New Ngl.FreightMaster.Integration.clsLane(Me.oConfig)
            'check for an existing Lane
            oHeader.LaneNumber = olane.doesLaneExist(oHeader.OrigAdd, oHeader.DestAdd, Me.OutboundCompNumber, Me.InboundCompNumber, oHeader.Inbound)
            'if a lane does not already exist for this shipfrom-shipto location create a new lanenumber
            'the book integration logic will automatically create the new lane with default values
            If String.IsNullOrWhiteSpace(oHeader.LaneNumber) Then
                'Note:  the CompLocationID contains the actual location IDs provided In the 204 310.N1.N104 segment 
                Dim sCompNumber As String = "0"
                If oHeader.Inbound Then
                    If Me.InboundCompNumber = 0 Then
                        sCompNumber = If(String.IsNullOrWhiteSpace(oHeader.DestAdd.CompNumber) = True, "0", oHeader.DestAdd.CompNumber)
                    Else
                        sCompNumber = Me.InboundCompNumber.ToString()
                    End If
                    'use the dest address
                    oHeader.LaneNumber = olane.getNextLaneNumberInSequence(sCompNumber, oHeader.DestAdd.CompLocationID, oHeader.OrigAdd.CompLocationID)
                Else
                    If Me.OutboundCompNumber = 0 Then
                        sCompNumber = If(String.IsNullOrWhiteSpace(oHeader.OrigAdd.CompNumber) = True, "0", oHeader.OrigAdd.CompNumber)
                    Else
                        sCompNumber = Me.OutboundCompNumber.ToString()
                    End If
                    'use the orig address 
                    oHeader.LaneNumber = olane.getNextLaneNumberInSequence(sCompNumber, oHeader.DestAdd.CompLocationID, oHeader.OrigAdd.CompLocationID)
                End If
            End If
            blnRet = True
        Catch ex As Exception
            oHeader.strErrors.Add(" Could not identify a Lane Number when processing EDI 204 Inbound Order information for," & oHeader.OrderNumber & ", because of an unexpected system error: " & ex.Message)
            statusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error
        End Try
        Return blnRet
    End Function

    Private Sub populate60Variables(ByVal o705Ords As List(Of clsBookHeaderObject705), ByVal o705Dets As List(Of clsBookDetailObject705),
                                    ByRef o60Ords As List(Of clsBookHeaderObject60), ByRef o60Dets As List(Of clsBookDetailObject60))
        Dim book As New Ngl.FreightMaster.Integration.clsBook
        Dim strMsg = ""
        'Headers
        For Each h7 In o705Ords
            Dim h6 As New clsBookHeaderObject60
            strMsg = ""
            h6 = NDT.CopyMatchingFields(h6, h7, Nothing, strMsg)
            If Not h6 Is Nothing Then
                o60Ords.Add(h6)
            End If
        Next
        'Details
        For Each d7 In o705Dets
            Dim d6 As New clsBookDetailObject60
            strMsg = ""
            d6 = NDT.CopyMatchingFields(d6, d7, Nothing, strMsg)
            If Not d6 Is Nothing Then
                o60Dets.Add(d6)
            End If
        Next

    End Sub

    Private Sub getTempInfo(ByVal oDetails As List(Of clsBookDetailObject705), ByRef temp As String, ByRef blnFrozen As Boolean, ByRef blnCooler As Boolean, ByRef blnDry As Boolean)
        Dim c = oDetails(0).BookItemCommCode
        temp = c
        For Each d In oDetails
            If Not String.Equals(d.BookItemCommCode, c) Then
                temp = "M"
                Exit For
            End If
        Next
        '**TODO** - what do we do here if it is mixed?
        Select Case temp
            Case "D"
                blnDry = True
            Case "F"
                blnFrozen = True
            Case "C"
                blnCooler = True
        End Select
    End Sub

    ''' <summary>
    ''' Populate the Pallets following the Order of Precedence
    ''' </summary>
    ''' <param name="CU300"></param>
    ''' <param name="CL300"></param>
    ''' <returns>Integer</returns>
    ''' <remarks>
    ''' Added by LVV On 5/2/17 For v-7.0.6.105 EDI 204In
    ''' </remarks>
    Private Function getHDRPallets(ByVal CU300 As clsEDI204InLoop300, ByVal CL300 As clsEDI204InLoop300, ByRef strWarnings As String) As Integer
        'Populate the Pallets following the Order of Precedence
        Dim pallets As Integer = 0
        Dim strPallets = ""
        If Not String.IsNullOrWhiteSpace(If(String.IsNullOrWhiteSpace(CU300.PLD.PLD01), "", CU300.PLD.PLD01.Trim())) Then
            strPallets = If(String.IsNullOrWhiteSpace(CU300.PLD.PLD01), "", CU300.PLD.PLD01.Trim())
        Else
            If Not String.IsNullOrWhiteSpace(If(String.IsNullOrWhiteSpace(CL300.PLD.PLD01), "", CL300.PLD.PLD01.Trim())) Then
                strPallets = If(String.IsNullOrWhiteSpace(CL300.PLD.PLD01), "", CL300.PLD.PLD01.Trim())
            Else
                strPallets = If(String.IsNullOrWhiteSpace(PLD.PLD01), "", PLD.PLD01.Trim())
            End If
        End If
        If Not Integer.TryParse(strPallets, pallets) Then
            strWarnings += "Failed To parse pallets To an Integer. Check the formatting Of the EDI document For PLD01. "
        End If
        Return pallets
    End Function


    Public Overrides Function addSegment(strSource As String, Optional strKey As String = "") As Boolean
        If String.IsNullOrWhiteSpace(strKey) Then
            If Not getKey(strSource, strKey) Then
                'nothing to do just return
                Return False
            End If
        End If
        If Not Me.usesElement(strKey) Then Return False
        If Not Me.Elements.ContainsKey(strKey) Then Return False
        Dim tracker As EDIItemTracker = Me.Elements(strKey)
        If tracker.ItemsUsed < tracker.ItemsAllowed Then
            Select Case strKey
                Case "ST"
                    Me.ST = New clsEDIST(strSource)
                Case "B2"
                    Me.B2 = New clsEDIB2(strSource)
                Case "B2A"
                    Me.B2A = New clsEDIB2A(strSource)
                Case "L11"
                    If L11s Is Nothing Then L11s = New List(Of clsEDIL11)
                    L11s.Add(New clsEDIL11(strSource))
                Case "G62"
                    Me.G62 = New clsEDIG62(strSource)
                Case "PLD"
                    Me.PLD = New clsEDIPLD(strSource)
                Case "NTE"
                    If NTEs Is Nothing Then NTEs = New List(Of clsEDINTE)
                    NTEs.Add(New clsEDINTE(strSource))
                Case "L3"
                    Me.L3 = New clsEDIL3(strSource)
                Case "SE"
                    Me.SE = New clsEDISE(strSource)
                Case Else
                    Return False
            End Select
            tracker.ItemsUsed += 1
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function addLoopFromKey(strElemKey As String, ByRef strElements() As String, ByRef NextIndex As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim tracker As EDIItemTracker
        If Not Me.Loops Is Nothing AndAlso Me.Loops.ContainsKey("Loop100") Then
            tracker = Me.Loops("Loop100")
            If Not tracker Is Nothing Then
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Dim oEDILoop100 As New clsEDI204Loop100()
                    If Not oEDILoop100.Elements Is Nothing AndAlso oEDILoop100.Elements.ContainsKey(strElemKey) Then
                        If Loop100 Is Nothing Then Loop100 = New List(Of clsEDI204Loop100)
                        oEDILoop100 = Me.insertElements(New clsEDI204Loop100(), strElements, NextIndex)
                        If Not oEDILoop100 Is Nothing Then
                            Loop100.Add(oEDILoop100)
                            tracker.ItemsUsed += 1
                        End If
                        'if the key exists for the loop object we return true even if the insert elements returns nothing
                        'this allows us to move on to the next element
                        Return True
                    End If
                End If
            End If
        End If

        If Me.Loops.ContainsKey("Loop200") Then
            tracker = Me.Loops("Loop200")
            If Not tracker Is Nothing Then
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Dim oEDILoop200 As New clsEDI204Loop200()
                    If oEDILoop200.Elements.ContainsKey(strElemKey) Then
                        If Loop200 Is Nothing Then Loop200 = New List(Of clsEDI204Loop200)
                        oEDILoop200 = Me.insertElements(New clsEDI204Loop200(), strElements, NextIndex)
                        If Not oEDILoop200 Is Nothing Then
                            Loop200.Add(oEDILoop200)
                            tracker.ItemsUsed += 1
                        End If
                        'if the key exists for the loop object we return true even if the insert elements returns nothing
                        'this allows us to move on to the next element
                        Return True
                    End If
                End If
            End If
        End If

        If Me.Loops.ContainsKey("Loop300") Then
            tracker = Me.Loops("Loop300")
            If Not tracker Is Nothing Then
                If tracker.ItemsUsed < tracker.ItemsAllowed Then
                    Dim oEDILoop300 As New clsEDI204InLoop300()
                    If oEDILoop300.Elements.ContainsKey(strElemKey) Then
                        If Loop300 Is Nothing Then Loop300 = New List(Of clsEDI204InLoop300)
                        oEDILoop300 = Me.insertElements(New clsEDI204InLoop300(), strElements, NextIndex)
                        If Not oEDILoop300 Is Nothing Then
                            Loop300.Add(oEDILoop300)
                            tracker.ItemsUsed += 1
                        End If
                        'if the key exists for the loop object we return true even if the insert elements returns nothing
                        'this allows us to move on to the next element
                        Return True
                    End If
                End If
            End If
        End If
        Return blnRet
    End Function

    Protected Overrides Sub populateElements()
        Me.Name = "204 File"
        Me.Elements = New Dictionary(Of String, EDIItemTracker)
        Me.Loops = New Dictionary(Of String, EDIItemTracker)
        Me.Elements.Add("ST", New EDIItemTracker(1))
        Me.Elements.Add("B2", New EDIItemTracker(1))
        Me.Elements.Add("B2A", New EDIItemTracker(1))
        Me.Elements.Add("L11", New EDIItemTracker(20))
        Me.Elements.Add("G62", New EDIItemTracker(1))
        Me.Elements.Add("PLD", New EDIItemTracker(1))
        Me.Elements.Add("NTE", New EDIItemTracker(10))
        Me.Elements.Add("L3", New EDIItemTracker(1))
        Me.Elements.Add("SE", New EDIItemTracker(1))
        Me.Loops.Add("Loop100", New EDIItemTracker(5))
        Me.Loops.Add("Loop200", New EDIItemTracker(10))
        Me.Loops.Add("Loop300", New EDIItemTracker(999))
    End Sub
End Class

#End Region

#Region "214 Loop 100"

Public Class clsEDI214Loop100

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        addDataFromString(strSource, strSegSep)

    End Sub

    Public N1 As New clsEDIN1
    Public N2 As New clsEDIN2
    Public N3 As New clsEDIN3
    Public N4 As New clsEDIN4
    Public G62 As New clsEDIG62

    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        For isubsegs As Integer = 4 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the N1 record (it should be all that is left
                    If Left(sElems(0), 3) <> "N1*" Then sElems(0) = "N1*" & sElems(0)
                    N1 = New clsEDIN1(sElems(0))
                Case 1
                    'read the N2 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "N2\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        N2 = New clsEDIN2("N2*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 2
                    'read the N3 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "N3\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        N3 = New clsEDIN3("N3*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 3
                    'read the N4 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "N4\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        N4 = New clsEDIN4("N4*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 4
                    'read the G62 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "G62\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        G62 = New clsEDIG62("G62*" & sElems(0))
                        strSource = segs(0)
                    End If
            End Select
        Next
    End Sub

End Class

#End Region

#Region "214 Loop 200"

Public Class clsEDI214Loop200

    Public Sub New()
        MyBase.New()

    End Sub

    Public Sub New(ByVal strLXSource As String, ByVal strSegSep As String)
        MyBase.New()
        addDataFromString(strLXSource, strSegSep)

    End Sub

    Public LX As New clsEDILX
    Public Loop205() As clsEDI214Loop205
    Public L11s As New List(Of clsEDIL11)
    Public L11 As New clsEDIL11
    Public K1 As New clsEDIK1
    Public AT8 As New clsEDIAT8

    Public Sub addFooterFromString(ByVal strSegSep As String, ByVal strFooterElements As String)
        'split out all items
        Dim sSegs() As String = Regex.Split(strFooterElements, "\" & strSegSep)
        For Each s In sSegs
            Select Case Left(s, 3)
                Case "L11"
                    'split out any unwanted elements 
                    Dim sElems() As String = s.Split(strSegSep)
                    'read the L11 record (it should be all that is left)
                    If Left(sElems(0), 4) <> "L11*" Then sElems(0) = "L11*" & sElems(0)
                    Dim L11 As New clsEDIL11(sElems(0))
                    L11s.Add(L11)
                Case "K1*"
                    'read the K1 record                   
                    'split out any unwanted elements 
                    Dim sElems() As String = s.Split(strSegSep)
                    K1 = New clsEDIK1(sElems(0))

                Case "AT8"
                    'split out any unwanted elements 
                    Dim sElems() As String = s.Split(strSegSep)
                    AT8 = New clsEDIAT8(sElems(0))
            End Select
        Next
        'now add the L11, K1 and AT8 records if they exist
        'For isubsegs As Integer = 2 To 0 Step -1
        '    Dim segs() As String
        '    Select Case isubsegs
        '        Case 0
        '            split out any unwanted elements 
        '            Dim sElems() As String = strFooterElements.Split(strSegSep)
        '            read the L11 record (it should be all that is left)
        '            If Left(sElems(0), 4) <> "L11*" Then sElems(0) = "L11*" & sElems(0)
        '            L11 = New clsEDIL11(sElems(0))
        '        Case 1
        '            read the K1 record
        '            segs = Regex.Split(strFooterElements, "\" & strSegSep & "K1\*")
        '            If segs.Length > 1 Then
        '                split out any unwanted elements 
        '                Dim sElems() As String = segs(1).Split(strSegSep)
        '                K1 = New clsEDIK1("K1*" & sElems(0))
        '                strFooterElements = segs(0)
        '            End If
        '        Case 2
        '            read the AT8 record
        '            segs = Regex.Split(strFooterElements, "\" & strSegSep & "AT8\*")
        '            If segs.Length > 1 Then
        '                split out any unwanted elements 
        '                Dim sElems() As String = segs(1).Split(strSegSep)
        '                AT8 = New clsEDIAT8("AT8*" & sElems(0))
        '                strFooterElements = segs(0)
        '            End If
        '    End Select
        'Next
    End Sub

    Public Sub addDataFromString(ByVal strLXSource As String, ByVal strSegSep As String)
        'add the LX data
        If Not String.IsNullOrEmpty(strLXSource) Then
            'split out any unwanted elements 
            Dim sElems() As String = strLXSource.Split(strSegSep)
            If Left(sElems(0), 3) <> "LX*" Then sElems(0) = "LX*" & sElems(0)
            LX = New clsEDILX(sElems(0))
        End If

    End Sub
End Class

#End Region

#Region "214 Loop 205 (Nested Inside 214 200)"

Public Class clsEDI214Loop205

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        addDataFromString(strSource, strSegSep)
    End Sub

    Public AT7 As New clsEDIAT7
    Public MS1 As New clsEDIMS1
    Public MS2 As New clsEDIMS2

    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        For isubsegs As Integer = 2 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the AT7 record (it should be all that is left)
                    If Left(sElems(0), 4) <> "AT7*" Then sElems(0) = "AT7*" & sElems(0)
                    AT7 = New clsEDIAT7(sElems(0))
                Case 1
                    'read the MS1 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "MS1\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        MS1 = New clsEDIMS1("MS1*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 2
                    'read the MS2 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "MS2\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        MS2 = New clsEDIMS2("MS2*" & sElems(0))
                        'set the original search string to the left over portion
                        strSource = segs(0)
                    End If
            End Select
        Next
    End Sub
End Class

#End Region

#Region " TMS Data"

''' <summary>
''' mapping to FMData.spGetEDI204TruckLoadDataRow 
''' </summary>
''' <remarks>
''' Modified by RHR on 05/20/2018 for v-6.0.4.4-m
'''  Added support for BookNotesVisable1, BookWhseAuthorizationNo
'''  'Modified by RHR for v-8.5.3.007 on 04/03/2023 added logic for BookStopNo
''' </remarks>
Public Class clsEDITMSData
    Public CarrierSCAC As String = "" 'dbo.Carrier.CarrierSCAC
    Public CarrierNumber As Integer = 0 'dbo.Carrier.CarrierNumber
    Public CarrierName As String = "" 'dbo.Carrier.CarrierName
    Public CarrierControl As Integer = 0
    Public BookControl As Integer = 0
    Public BookProNumber As String = "" 'dbo.Book.BookProNumber
    Public BookConsPrefix As String = "" 'dbo.Book.BookConsPrefix
    Public BookCarrOrderNumber As String = "" 'dbo.Book.BookCarrOrderNumber
    Public BookOrderSequence As Integer = 0 'dbo.Book.BookOrderSequence
    Public BookRouteFinalCode As String = "" 'dbo.Book.BookRouteFinalCode
    Public BookTransactionPurpose As String = "" 'Calculated in Stored Procedure based on BookRouteFinalCode
    Public BookTotalCases As Integer = 0 'dbo.Book.BookTotalCases
    Public BookTotalWgt As Double = 0 'dbo.Book.BookTotalWgt
    Public BookTotalPL As Integer = 0 'dbo.Book.BookTotalPL
    Public BookTotalCube As Integer = 0 'dbo.Book.BookTotalCube
    Public BookDateLoad As String = "" 'dbo.Book.BookDateLoad (note this field must be formated like YYYYMMDD)
    Public BookDateLoadTime As String = "" 'Calculated field when data is retrieved time portion of BookDateLoad (note this field must be formated like HHmm)
    Public BookDateRequired As String = "" 'dbo.Book.BookDateRequired (note this field must be formated like YYYYMMDD)
    Public BookDateRequiredTime As String = "" 'dbo.Book.BookDateRequired (note this field must be formated like HHmm)
    Public BookCarrScheduleDate As String = "" 'dbo.Book.BookCarrScheduleDate (note this field must be formated like YYYYMMDD)
    Public BookCarrScheduleTime As String = "" 'dbo.Book.BookCarrScheduleTime (note this field must be formated like HHmm)
    Public BookCarrApptDate As String = "" 'dbo.Book.BookCarrApptDate (note this field must be formated like YYYYMMDD)
    Public BookCarrApptTime As String = "" 'dbo.Book.BookCarrApptTime (note this field must be formated like HHmm)
    Public BookOrigName As String = "" 'dbo.Book.BookOrigName
    Public BookOrigAddress1 As String = "" 'dbo.Book.BookOrigAddress1
    Public BookOrigAddress2 As String = "" 'dbo.Book.BookOrigAddress2
    Public BookOrigAddress3 As String = "" 'dbo.Book.BookOrigAddress3
    Public BookOrigCity As String = "" 'dbo.Book.BookOrigCity
    Public BookOrigState As String = "" 'dbo.Book.BookOrigState
    Public BookOrigCountry As String = "" 'dbo.Book.BookOrigCountry
    Public BookOrigZip As String = "" 'dbo.Book.BookOrigZip
    Public BookOrigPhone As String = "" 'dbo.Book.BookOrigPhone
    Public BookOrigIDENTIFICATIONCODEQUALIFIER As String = "" 'Lookup value in stored procedure
    Public BookOrigCompanyNumber As String = "" 'Lookup the company number or use the lane number.
    Public BookDestName As String = "" 'dbo.Book.BookDestName
    Public BookDestAddress1 As String = "" 'dbo.Book.BookDestAddress1
    Public BookDestAddress2 As String = "" 'dbo.Book.BookDestAddress2
    Public BookDestAddress3 As String = "" 'dbo.Book.BookDestAddress3
    Public BookDestCity As String = "" 'dbo.Book.BookDestCity
    Public BookDestState As String = "" 'dbo.Book.BookDestState
    Public BookDestCountry As String = "" 'dbo.Book.BookDestCountry
    Public BookDestZip As String = "" 'dbo.Book.BookDestZip
    Public BookDestPhone As String = "" 'dbo.Book.BookDestPhone
    Public BookDestIDENTIFICATIONCODEQUALIFIER As String = "" 'Lookup value in stored procedure
    Public BookDestCompanyNumber As String = "" 'Lookup the company number or use the lane number.
    Public BookLoadPONumber As String = "" 'First Customer PO associated with order. (we currently do not support multiple customer PO's on a single order for 204s)
    Public BookLoadCom As String = "U" 'default = U for undefined
    Public CommCodeDescription As String = "Unknown Temprature"
    Public LaneOriginAddressUse As Boolean = False 'dbo.Lane.LaneOriginAddressUse determines inbound or outbound
    Public LaneComments As String = "" 'dbo.Lane.LaneComments
    Public CompEDISecurityQual As String = ""
    Public CompEDISecurityCode As String = ""
    Public CompEDIPartnerQual As String = ""
    Public CompEDIPartnerCode As String = ""
    Public CompEDIEmailNotificationOn As Boolean = False
    Public CompEDIEmailAddress As String = ""
    Public CompEDIAcknowledgementRequested As Boolean = False
    Public CompEDIMethodOfPayment As String = ""
    Public BookRouteConsFlag As Boolean = False
    Public BookRevTotalCost As Decimal
    Public BillToCompName As String
    Public BillToCompNumber As String
    Public BillToCompAddress1 As String
    Public BillToCompAddress2 As String
    Public BillToCompCity As String
    Public BillToCompState As String
    Public BillToCompZip As String
    Public BillToCompCountry As String
    Public EDICombineOrdersForStops As Double
    Public BookCustCompControl As Integer
    Public BookSHID As String
    Public BookNotesVisable1 As String
    Public BookWhseAuthorizationNo As String
    Public BookStopNo As Integer = 1 ' Modified by RHR for v-8.5.3.007 on 04/03/2023 used for .S5.S501 on Destinations

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByRef oRow As FMData.spGetEDI204TruckLoadDataRow)
        MyBase.New()
        CarrierSCAC = cleanEDI(nz(oRow, "CarrierSCAC", "")) 'dbo.Carrier.CarrierSCAC)
        CarrierNumber = nz(oRow, "CarrierNumber", 0).ToString 'dbo.Carrier.CarrierNumber
        CarrierName = cleanEDI(nz(oRow, "CarrierName", "")) 'dbo.Carrier.CarrierName
        BookControl = nz(oRow, "BookControl", 0)
        BookProNumber = cleanEDI(nz(oRow, "BookProNumber", "")) 'dbo.Book.BookProNumber
        BookConsPrefix = cleanEDI(nz(oRow, "BookConsPrefix", "")) 'dbo.Book.BookConsPrefix
        BookCarrOrderNumber = cleanEDI(nz(oRow, "BookCarrOrderNumber", "")) 'dbo.Book.BookCarrOrderNumber
        BookOrderSequence = nz(oRow, "BookOrderSequence", 0).ToString 'dbo.Book.BookOrderSequence
        BookRouteFinalCode = cleanEDI(nz(oRow, "BookRouteFinalCode", "")) 'dbo.Book.BookRouteFinalCode
        BookTransactionPurpose = cleanEDI(nz(oRow, "BookTransactionPurpose", "")) 'Calculated in Stored Procedure based on BookRouteFinalCode
        BookTotalCases = nz(oRow, "BookTotalCases", 0).ToString 'dbo.Book.BookTotalCases
        BookTotalWgt = nz(oRow, "BookTotalWgt", 0).ToString 'dbo.Book.BookTotalWgt
        BookTotalPL = nz(oRow, "BookTotalPL", 0).ToString 'dbo.Book.BookTotalPL
        BookTotalCube = nz(oRow, "BookTotalCube", 0).ToString 'dbo.Book.BookTotalCube

        BookDateLoad = nzDate(oRow, "BookDateLoad", "yyyyMMdd", "00000000") 'dbo.Book.BookDateLoad (note this field must be formated like YYYYMMDD)
        BookDateLoadTime = nzDate(oRow, "BookDateLoad", "HHmm", "0000") 'Calculated field when data is retrieved time portion of BookDateLoad (note this field must be formated like HHmm)
        BookDateRequired = nzDate(oRow, "BookDateRequired", "yyyyMMdd", "00000000")  'dbo.Book.BookDateRequired (note this field must be formated like YYYYMMDD)
        BookDateRequiredTime = nzDate(oRow, "BookDateRequired", "HHmm", "0000") 'dbo.Book.BookDateRequired (note this field must be formated like HHmm)

        BookCarrScheduleDate = nzDate(oRow, "BookCarrScheduleDate", "yyyyMMdd", "00000000")  'dbo.Book.BookCarrScheduleDate (note this field must be formated like YYYYMMDD)
        BookCarrScheduleTime = nzDate(oRow, "BookCarrScheduleTime", "HHmm", "0000") 'dbo.Book.BookCarrScheduleTime (note this field must be formated like HHmm)
        BookCarrApptDate = nzDate(oRow, "BookCarrApptDate", "yyyyMMdd", "00000000")  'dbo.Book.BookCarrApptDate (note this field must be formated like YYYYMMDD)
        BookCarrApptTime = nzDate(oRow, "BookCarrApptTime", "HHmm", "0000") 'dbo.Book.BookCarrApptTime (note this field must be formated like HHmm)
        BookOrigName = cleanEDI(nz(oRow, "BookOrigName", "")) 'dbo.Book.BookOrigName
        BookOrigAddress1 = cleanEDI(nz(oRow, "BookOrigAddress1", "")) 'dbo.Book.BookOrigAddress1
        BookOrigAddress2 = cleanEDI(nz(oRow, "BookOrigAddress2", "")) 'dbo.Book.BookOrigAddress2
        BookOrigAddress3 = cleanEDI(nz(oRow, "BookOrigAddress3", "")) 'dbo.Book.BookOrigAddress3
        BookOrigCity = cleanEDI(nz(oRow, "BookOrigCity", "")) 'dbo.Book.BookOrigCity
        BookOrigState = cleanEDI(nz(oRow, "BookOrigState", "")) 'dbo.Book.BookOrigState
        BookOrigCountry = cleanEDI(nz(oRow, "BookOrigCountry", "")) 'dbo.Book.BookOrigCountry
        BookOrigZip = cleanEDI(nz(oRow, "BookOrigZip", "")) 'dbo.Book.BookOrigZip
        BookOrigPhone = cleanEDI(nz(oRow, "BookOrigPhone", "")) 'dbo.Book.BookOrigPhone
        BookOrigIDENTIFICATIONCODEQUALIFIER = cleanEDI(nz(oRow, "BookOrigIDENTIFICATIONCODEQUALIFIER", "")) 'Lookup value in stored procedure
        BookOrigCompanyNumber = cleanEDI(nz(oRow, "BookOrigCompanyNumber", "")) 'Lookup the company number or use the lane number.
        BookDestName = cleanEDI(nz(oRow, "BookDestName", "")) 'dbo.Book.BookDestName
        BookDestAddress1 = cleanEDI(nz(oRow, "BookDestAddress1", "")) 'dbo.Book.BookDestAddress1
        BookDestAddress2 = cleanEDI(nz(oRow, "BookDestAddress2", "")) 'dbo.Book.BookDestAddress2
        BookDestAddress3 = cleanEDI(nz(oRow, "BookDestAddress3", "")) 'dbo.Book.BookDestAddress3
        BookDestCity = cleanEDI(nz(oRow, "BookDestCity", "")) 'dbo.Book.BookDestCity
        BookDestState = cleanEDI(nz(oRow, "BookDestState", "")) 'dbo.Book.BookDestState
        BookDestCountry = cleanEDI(nz(oRow, "BookDestCountry", "")) 'dbo.Book.BookDestCountry
        BookDestZip = cleanEDI(nz(oRow, "BookDestZip", "")) 'dbo.Book.BookDestZip
        BookDestPhone = cleanEDI(nz(oRow, "BookDestPhone", "")) 'dbo.Book.BookDestPhone
        BookDestIDENTIFICATIONCODEQUALIFIER = cleanEDI(nz(oRow, "BookDestIDENTIFICATIONCODEQUALIFIER", "")) 'Lookup value in stored procedure
        BookDestCompanyNumber = cleanEDI(nz(oRow, "BookDestCompanyNumber", "")) 'Lookup the company number or use the lane number.
        BookLoadPONumber = cleanEDI(nz(oRow, "BookLoadPONumber", "")) 'First Customer PO associated with order. (we currently do not support multiple customer PO's on a single order for 204s)
        BookLoadCom = cleanEDI(nz(oRow, "BookLoadCom", ""))
        CommCodeDescription = cleanEDI(nz(oRow, "CommCodeDescription", ""))
        LaneOriginAddressUse = nz(oRow, "LaneOriginAddressUse", "") 'dbo.Lane.LaneOriginAddressUse determines inbound or outbound
        LaneComments = cleanEDI(nz(oRow, "LaneComments", "")) 'dbo.Lane.LaneComments
        CompEDISecurityQual = cleanEDI(nz(oRow, "CompEDISecurityQual", "00"))
        CompEDISecurityCode = cleanEDI(nz(oRow, "CompEDISecurityCode", ""))
        CompEDIPartnerQual = cleanEDI(nz(oRow, "CompEDIPartnerQual", ""))
        CompEDIPartnerCode = cleanEDI(nz(oRow, "CompEDIPartnerCode", ""))
        CompEDIEmailNotificationOn = nz(oRow, "CompEDIEmailNotificationOn", False)
        CompEDIEmailAddress = cleanEDI(nz(oRow, "CompEDIEmailAddress", ""))
        CompEDIAcknowledgementRequested = nz(oRow, "CompEDIAcknowledgementRequested", False)
        CompEDIMethodOfPayment = cleanEDI(nz(oRow, "CompEDIMethodOfPayment", "PP"))
        BookRouteConsFlag = nz(oRow, "BookRouteConsFlag", False)
        BookRevTotalCost = nz(oRow, "BookRevTotalCost", 0)
        BillToCompName = cleanEDI(nz(oRow, "BillToCompName", ""))
        BillToCompNumber = cleanEDI(nz(oRow, "BillToCompNumber", ""))
        BillToCompAddress1 = cleanEDI(nz(oRow, "BillToCompAddress1", ""))
        BillToCompAddress2 = cleanEDI(nz(oRow, "BillToCompAddress2", ""))
        BillToCompCity = cleanEDI(nz(oRow, "BillToCompCity", ""))
        BillToCompState = cleanEDI(nz(oRow, "BillToCompState", ""))
        BillToCompZip = cleanEDI(nz(oRow, "BillToCompZip", ""))
        BillToCompCountry = cleanEDI(nz(oRow, "BillToCompCountry", ""))
        EDICombineOrdersForStops = nz(oRow, "EDICombineOrdersForStops", 0)
        BookSHID = cleanEDI(nz(oRow, "BookSHID", ""))
        BookCustCompControl = nz(oRow, "BookCustCompControl", 0)
        BookNotesVisable1 = cleanEDI(nz(oRow, "BookNotesVisable1", ""))
        BookWhseAuthorizationNo = cleanEDI(nz(oRow, "BookWhseAuthorizationNo", ""))

    End Sub

    'LTS.spGetEDI204TruckLoadDataResult
    Public Sub New(ByRef oRow As LTS.spGetEDI204TruckLoadDataResult)
        MyBase.New()
        CarrierSCAC = cleanEDI(nz(oRow.CarrierSCAC, "")) 'dbo.Carrier.CarrierSCAC)
        CarrierNumber = nz(oRow.CarrierNumber, 0).ToString 'dbo.Carrier.CarrierNumber
        CarrierName = cleanEDI(nz(oRow.CarrierName, "")) 'dbo.Carrier.CarrierName
        BookControl = nz(oRow.BookControl, 0)
        BookProNumber = cleanEDI(nz(oRow.BookProNumber, "")) 'dbo.Book.BookProNumber
        BookConsPrefix = cleanEDI(nz(oRow.BookConsPrefix, "")) 'dbo.Book.BookConsPrefix
        BookCarrOrderNumber = cleanEDI(nz(oRow.BookCarrOrderNumber, "")) 'dbo.Book.BookCarrOrderNumber
        BookOrderSequence = nz(oRow.BookOrderSequence, 0).ToString 'dbo.Book.BookOrderSequence
        BookRouteFinalCode = cleanEDI(nz(oRow.BookRouteFinalCode, "")) 'dbo.Book.BookRouteFinalCode
        BookTransactionPurpose = cleanEDI(nz(oRow.BookTransactionPurpose, "")) 'Calculated in Stored Procedure based on BookRouteFinalCode
        BookTotalCases = nz(oRow.BookTotalCases, 0).ToString 'dbo.Book.BookTotalCases
        BookTotalWgt = nz(oRow.BookTotalWgt, 0).ToString 'dbo.Book.BookTotalWgt
        BookTotalPL = nz(oRow.BookTotalPL, 0).ToString 'dbo.Book.BookTotalPL
        BookTotalCube = nz(oRow.BookTotalCube, 0).ToString 'dbo.Book.BookTotalCube

        BookDateLoad = nzDate(oRow.BookDateLoad, "yyyyMMdd", "00000000") 'dbo.Book.BookDateLoad (note this field must be formated like YYYYMMDD)
        BookDateLoadTime = nzDate(oRow.BookDateLoad, "HHmm", "0000") 'Calculated field when data is retrieved time portion of BookDateLoad (note this field must be formated like HHmm)
        BookDateRequired = nzDate(oRow.BookDateRequired, "yyyyMMdd", "00000000")  'dbo.Book.BookDateRequired (note this field must be formated like YYYYMMDD)
        BookDateRequiredTime = nzDate(oRow.BookDateRequired, "HHmm", "0000") 'dbo.Book.BookDateRequired (note this field must be formated like HHmm)

        BookCarrScheduleDate = nzDate(oRow.BookCarrScheduleDate, "yyyyMMdd", "00000000")  'dbo.Book.BookCarrScheduleDate (note this field must be formated like YYYYMMDD)
        BookCarrScheduleTime = nzDate(oRow.BookCarrScheduleTime, "HHmm", "0000") 'dbo.Book.BookCarrScheduleTime (note this field must be formated like HHmm)
        BookCarrApptDate = nzDate(oRow.BookCarrApptDate, "yyyyMMdd", "00000000")  'dbo.Book.BookCarrApptDate (note this field must be formated like YYYYMMDD)
        BookCarrApptTime = nzDate(oRow.BookCarrApptTime, "HHmm", "0000") 'dbo.Book.BookCarrApptTime (note this field must be formated like HHmm)
        BookOrigName = cleanEDI(nz(oRow.BookOrigName, "")) 'dbo.Book.BookOrigName
        BookOrigAddress1 = cleanEDI(nz(oRow.BookOrigAddress1, "")) 'dbo.Book.BookOrigAddress1
        BookOrigAddress2 = cleanEDI(nz(oRow.BookOrigAddress2, "")) 'dbo.Book.BookOrigAddress2
        BookOrigAddress3 = cleanEDI(nz(oRow.BookOrigAddress3, "")) 'dbo.Book.BookOrigAddress3
        BookOrigCity = cleanEDI(nz(oRow.BookOrigCity, "")) 'dbo.Book.BookOrigCity
        BookOrigState = cleanEDI(nz(oRow.BookOrigState, "")) 'dbo.Book.BookOrigState
        BookOrigCountry = cleanEDI(nz(oRow.BookOrigCountry, "")) 'dbo.Book.BookOrigCountry
        BookOrigZip = cleanEDI(nz(oRow.BookOrigZip, "")) 'dbo.Book.BookOrigZip
        BookOrigPhone = cleanEDI(nz(oRow.BookOrigPhone, "")) 'dbo.Book.BookOrigPhone
        BookOrigIDENTIFICATIONCODEQUALIFIER = cleanEDI(nz(oRow.BookOrigIDENTIFICATIONCODEQUALIFIER, "")) 'Lookup value in stored procedure
        BookOrigCompanyNumber = cleanEDI(nz(oRow.BookOrigCompanyNumber, "")) 'Lookup the company number or use the lane number.
        BookDestName = cleanEDI(nz(oRow.BookDestName, "")) 'dbo.Book.BookDestName
        BookDestAddress1 = cleanEDI(nz(oRow.BookDestAddress1, "")) 'dbo.Book.BookDestAddress1
        BookDestAddress2 = cleanEDI(nz(oRow.BookDestAddress2, "")) 'dbo.Book.BookDestAddress2
        BookDestAddress3 = cleanEDI(nz(oRow.BookDestAddress3, "")) 'dbo.Book.BookDestAddress3
        BookDestCity = cleanEDI(nz(oRow.BookDestCity, "")) 'dbo.Book.BookDestCity
        BookDestState = cleanEDI(nz(oRow.BookDestState, "")) 'dbo.Book.BookDestState
        BookDestCountry = cleanEDI(nz(oRow.BookDestCountry, "")) 'dbo.Book.BookDestCountry
        BookDestZip = cleanEDI(nz(oRow.BookDestZip, "")) 'dbo.Book.BookDestZip
        BookDestPhone = cleanEDI(nz(oRow.BookDestPhone, "")) 'dbo.Book.BookDestPhone
        BookDestIDENTIFICATIONCODEQUALIFIER = cleanEDI(nz(oRow.BookDestIDENTIFICATIONCODEQUALIFIER, "")) 'Lookup value in stored procedure
        BookDestCompanyNumber = cleanEDI(nz(oRow.BookDestCompanyNumber, "")) 'Lookup the company number or use the lane number.
        BookLoadPONumber = cleanEDI(nz(oRow.BookLoadPONumber, "")) 'First Customer PO associated with order. (we currently do not support multiple customer PO's on a single order for 204s)
        BookLoadCom = cleanEDI(nz(oRow.BookLoadCom, ""))
        CommCodeDescription = cleanEDI(nz(oRow.CommCodeDescription, ""))
        LaneOriginAddressUse = nz(oRow.LaneOriginAddressUse, "") 'dbo.Lane.LaneOriginAddressUse determines inbound or outbound
        LaneComments = cleanEDI(nz(oRow.LaneComments, "")) 'dbo.Lane.LaneComments
        CompEDISecurityQual = cleanEDI(nz(oRow.CompEDISecurityQual, "00"))
        CompEDISecurityCode = cleanEDI(nz(oRow.CompEDISecurityCode, ""))
        CompEDIPartnerQual = cleanEDI(nz(oRow.CompEDIPartnerQual, ""))
        CompEDIPartnerCode = cleanEDI(nz(oRow.CompEDIPartnerCode, ""))
        CompEDIEmailNotificationOn = nz(oRow.CompEDIEmailNotificationOn, False)
        CompEDIEmailAddress = cleanEDI(nz(oRow.CompEDIEmailAddress, ""))
        CompEDIAcknowledgementRequested = nz(oRow.CompEDIAcknowledgementRequested, False)
        CompEDIMethodOfPayment = cleanEDI(nz(oRow.CompEDIMethodOfPayment, "PP"))
        BookRouteConsFlag = nz(oRow.BookRouteConsFlag, False)
        BookRevTotalCost = nz(oRow.BookRevTotalCost, 0)
        BillToCompName = cleanEDI(nz(oRow.BillToCompName, ""))
        BillToCompNumber = cleanEDI(nz(oRow.BillToCompNumber, ""))
        BillToCompAddress1 = cleanEDI(nz(oRow.BillToCompAddress1, ""))
        BillToCompAddress2 = cleanEDI(nz(oRow.BillToCompAddress2, ""))
        BillToCompCity = cleanEDI(nz(oRow.BillToCompCity, ""))
        BillToCompState = cleanEDI(nz(oRow.BillToCompState, ""))
        BillToCompZip = cleanEDI(nz(oRow.BillToCompZip, ""))
        BillToCompCountry = cleanEDI(nz(oRow.BillToCompCountry, ""))
        EDICombineOrdersForStops = nz(oRow.EDICombineOrdersForStops, 0)
        BookSHID = cleanEDI(nz(oRow.BookSHID, ""))
        BookCustCompControl = nz(oRow.BookCustCompControl, 0)
        BookNotesVisable1 = cleanEDI(nz(oRow.BookNotesVisable1, ""))
        BookWhseAuthorizationNo = cleanEDI(nz(oRow.BookWhseAuthorizationNo, ""))
        'Modified by RHR for v-8.5.3.007 on 04/03/2023 added logic for BookStopNo
        Dim iStopNo As Integer = 1
        If (oRow.BookStopNo.HasValue) Then
            iStopNo = CInt(oRow.BookStopNo)
            If iStopNo < 1 Then iStopNo = 1
        End If
        BookStopNo = iStopNo

    End Sub

    Public Sub New(ByRef oRow As EDI204Result)
        MyBase.New()
        CarrierSCAC = cleanEDI(oRow.CarrierSCAC) 'dbo.Carrier.CarrierSCAC)
        CarrierNumber = If(oRow.CarrierNumber, 0).ToString 'dbo.Carrier.CarrierNumber
        CarrierName = cleanEDI(oRow.CarrierName) 'dbo.Carrier.CarrierName
        CarrierControl = If(oRow.CarrierControl, 0)
        BookControl = If(oRow.BookControl, 0)
        BookProNumber = cleanEDI(oRow.BookProNumber) 'dbo.Book.BookProNumber
        BookConsPrefix = cleanEDI(oRow.BookConsPrefix) 'dbo.Book.BookConsPrefix
        BookCarrOrderNumber = cleanEDI(oRow.BookCarrOrderNumber) 'dbo.Book.BookCarrOrderNumber
        BookOrderSequence = If(oRow.BookOrderSequence, 0).ToString 'dbo.Book.BookOrderSequence
        BookRouteFinalCode = cleanEDI(oRow.BookRouteFinalCode) 'dbo.Book.BookRouteFinalCode
        BookTransactionPurpose = cleanEDI(oRow.BookTransactionPurpose) 'Calculated in Stored Procedure based on BookRouteFinalCode
        BookTotalCases = If(oRow.BookTotalCases, 0).ToString 'dbo.Book.BookTotalCases
        BookTotalWgt = If(oRow.BookTotalWgt, 0).ToString 'dbo.Book.BookTotalWgt
        BookTotalPL = If(oRow.BookTotalPL, 0).ToString 'dbo.Book.BookTotalPL
        BookTotalCube = If(oRow.BookTotalCube, 0).ToString 'dbo.Book.BookTotalCube

        BookDateLoad = If(oRow.BookDateLoad.HasValue, oRow.BookDateLoad.Value.ToString("yyyyMMdd"), "00000000") 'dbo.Book.BookDateLoad (note this field must be formated like YYYYMMDD)
        BookDateLoadTime = If(oRow.BookDateLoad.HasValue, oRow.BookDateLoad.Value.ToString("HHmm"), "0000") 'Calculated field when data is retrieved time portion of BookDateLoad (note this field must be formated like HHmm)
        BookDateRequired = If(oRow.BookDateRequired.HasValue, oRow.BookDateRequired.Value.ToString("yyyyMMdd"), "00000000")  'dbo.Book.BookDateRequired (note this field must be formated like YYYYMMDD)
        BookDateRequiredTime = If(oRow.BookDateRequired.HasValue, oRow.BookDateRequired.Value.ToString("HHmm"), "0000") 'dbo.Book.BookDateRequired (note this field must be formated like HHmm)

        BookCarrScheduleDate = If(oRow.BookCarrScheduleDate.HasValue, oRow.BookCarrScheduleDate.Value.ToString("yyyyMMdd"), "00000000")  'dbo.Book.BookCarrScheduleDate (note this field must be formated like YYYYMMDD)
        BookCarrScheduleTime = If(oRow.BookCarrScheduleTime.HasValue, oRow.BookCarrScheduleTime.Value.ToString("HHmm"), "0000") 'dbo.Book.BookCarrScheduleTime (note this field must be formated like HHmm)
        BookCarrApptDate = If(oRow.BookCarrApptDate.HasValue, oRow.BookCarrApptDate.Value.ToString("yyyyMMdd"), "00000000")  'dbo.Book.BookCarrApptDate (note this field must be formated like YYYYMMDD)
        BookCarrApptTime = If(oRow.BookCarrApptTime.HasValue, oRow.BookCarrApptTime.Value.ToString("HHmm"), "0000") 'dbo.Book.BookCarrApptTime (note this field must be formated like HHmm)
        BookOrigName = cleanEDI(oRow.BookOrigName) 'dbo.Book.BookOrigName
        BookOrigAddress1 = cleanEDI(oRow.BookOrigAddress1) 'dbo.Book.BookOrigAddress1
        BookOrigAddress2 = cleanEDI(oRow.BookOrigAddress2) 'dbo.Book.BookOrigAddress2
        BookOrigAddress3 = cleanEDI(oRow.BookOrigAddress3) 'dbo.Book.BookOrigAddress3
        BookOrigCity = cleanEDI(oRow.BookOrigCity) 'dbo.Book.BookOrigCity
        BookOrigState = cleanEDI(oRow.BookOrigState) 'dbo.Book.BookOrigState
        BookOrigCountry = cleanEDI(oRow.BookOrigCountry) 'dbo.Book.BookOrigCountry
        BookOrigZip = cleanEDI(oRow.BookOrigZip) 'dbo.Book.BookOrigZip
        BookOrigPhone = cleanEDI(oRow.BookOrigPhone) 'dbo.Book.BookOrigPhone
        BookOrigIDENTIFICATIONCODEQUALIFIER = cleanEDI(oRow.BookOrigIDENTIFICATIONCODEQUALIFIER) 'Lookup value in stored procedure
        BookOrigCompanyNumber = cleanEDI(oRow.BookOrigCompanyNumber) 'Lookup the company number or use the lane number.
        BookDestName = cleanEDI(oRow.BookDestName) 'dbo.Book.BookDestName
        BookDestAddress1 = cleanEDI(oRow.BookDestAddress1) 'dbo.Book.BookDestAddress1
        BookDestAddress2 = cleanEDI(oRow.BookDestAddress2) 'dbo.Book.BookDestAddress2
        BookDestAddress3 = cleanEDI(oRow.BookDestAddress3) 'dbo.Book.BookDestAddress3
        BookDestCity = cleanEDI(oRow.BookDestCity) 'dbo.Book.BookDestCity
        BookDestState = cleanEDI(oRow.BookDestState) 'dbo.Book.BookDestState
        BookDestCountry = cleanEDI(oRow.BookDestCountry) 'dbo.Book.BookDestCountry
        BookDestZip = cleanEDI(oRow.BookDestZip) 'dbo.Book.BookDestZip
        BookDestPhone = cleanEDI(oRow.BookDestPhone) 'dbo.Book.BookDestPhone
        BookDestIDENTIFICATIONCODEQUALIFIER = cleanEDI(oRow.BookDestIDENTIFICATIONCODEQUALIFIER) 'Lookup value in stored procedure
        BookDestCompanyNumber = cleanEDI(oRow.BookDestCompanyNumber) 'Lookup the company number or use the lane number.
        BookLoadPONumber = cleanEDI(oRow.BookLoadPONumber) 'First Customer PO associated with order. (we currently do not support multiple customer PO's on a single order for 204s)
        BookLoadCom = cleanEDI(oRow.BookLoadCom)
        CommCodeDescription = cleanEDI(oRow.CommCodeDescription)
        LaneOriginAddressUse = If(oRow.LaneOriginAddressUse, False) 'dbo.Lane.LaneOriginAddressUse determines inbound or outbound
        LaneComments = cleanEDI(oRow.LaneComments) 'dbo.Lane.LaneComments
        CompEDISecurityQual = cleanEDI(If(String.IsNullOrWhiteSpace(oRow.CompEDISecurityQual), "00", oRow.CompEDISecurityQual))
        CompEDISecurityCode = cleanEDI(oRow.CompEDISecurityCode)
        CompEDIPartnerQual = cleanEDI(oRow.CompEDIPartnerQual)
        CompEDIPartnerCode = cleanEDI(oRow.CompEDIPartnerCode)
        CompEDIEmailNotificationOn = If(oRow.CompEDIEmailNotificationOn, False)
        CompEDIEmailAddress = cleanEDI(oRow.CompEDIEmailAddress)
        CompEDIAcknowledgementRequested = If(oRow.CompEDIAcknowledgementRequested, False)
        CompEDIMethodOfPayment = cleanEDI(If(String.IsNullOrWhiteSpace(oRow.CompEDIMethodOfPayment), "PP", oRow.CompEDIMethodOfPayment))
        BookRouteConsFlag = If(oRow.BookRouteConsFlag, False)
        BookRevTotalCost = If(oRow.BookRevTotalCost, 0)
        BillToCompName = cleanEDI(oRow.BillToCompName)
        BillToCompNumber = cleanEDI(oRow.BillToCompNumber)
        BillToCompAddress1 = cleanEDI(oRow.BillToCompAddress1)
        BillToCompAddress2 = cleanEDI(oRow.BillToCompAddress2)
        BillToCompCity = cleanEDI(oRow.BillToCompCity)
        BillToCompState = cleanEDI(oRow.BillToCompState)
        BillToCompZip = cleanEDI(oRow.BillToCompZip)
        BillToCompCountry = cleanEDI(oRow.BillToCompCountry)
        EDICombineOrdersForStops = If(oRow.EDICombineOrdersForStops, 0)
        BookSHID = cleanEDI(oRow.BookSHID)
        BookCustCompControl = If(oRow.BookCustCompControl, 0)
        BookNotesVisable1 = cleanEDI(oRow.BookNotesVisable1)
        BookWhseAuthorizationNo = cleanEDI(oRow.BookWhseAuthorizationNo)

    End Sub
End Class

#End Region

#Region " EDI 997 Class"

Public Class clsEDI997

    Public LastError As String = ""
    Public ST As New clsEDIST
    Public AK1 As New clsEDIAK1
    Public AK2 As New clsEDIAK2
    Public AK3 As New clsEDIAK3
    Public AK4 As New clsEDIAK4
    Public AK5 As New clsEDIAK5
    Public AK9 As New clsEDIAK9
    Public SE As New clsEDISE



    Private _ExternalSecurityQualifier As String
    ''' <summary>
    ''' maps to 997's ISA03 (Carrier for 214 response, Comp for 204 in response)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 -- 6.0.4.7 on 6/7/217
    '''   provides a way for EDI 204 ins to send a 997
    ''' </remarks>
    Public Property ExternalSecurityQualifier() As String
        Get
            Return _ExternalSecurityQualifier
        End Get
        Set(ByVal value As String)
            _ExternalSecurityQualifier = value
        End Set
    End Property

    Private _ExternalSecurityCode As String
    ''' <summary>
    ''' maps to 997's ISA04 (Carrier for 214 response, Comp for 204 in response)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 -- 6.0.4.7 on 6/7/217
    '''   provides a way for EDI 204 ins to send a 997
    ''' </remarks>
    Public Property ExternalSecurityCode() As String
        Get
            Return _ExternalSecurityCode
        End Get
        Set(ByVal value As String)
            _ExternalSecurityCode = value
        End Set
    End Property


    Private _ExternalPartnerQualifier As String
    ''' <summary>
    ''' maps to 997's ISA07 (Carrier for 214 response, Comp for 204 in response)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 -- 6.0.4.7 on 6/7/217
    '''   provides a way for EDI 204 ins to send a 997
    ''' </remarks>
    Public Property ExternalPartnerQualifier() As String
        Get
            Return _ExternalPartnerQualifier
        End Get
        Set(ByVal value As String)
            _ExternalPartnerQualifier = value
        End Set
    End Property

    Private _ExternalPartnerCode As String
    ''' <summary>
    ''' maps to 997's ISA08 (Carrier for 214 response, Comp for 204 in response)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 -- 6.0.4.7 on 6/7/217
    '''   provides a way for EDI 204 ins to send a 997
    ''' </remarks>
    Public Property ExternalPartnerCode() As String
        Get
            Return _ExternalPartnerCode
        End Get
        Set(ByVal value As String)
            _ExternalPartnerCode = value
        End Set
    End Property

    Private _InternalSecurityQualifier As String
    ''' <summary>
    ''' maps to 997's ISA01 (Comp for 214 response, Carrier for 204 in response)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 -- 6.0.4.7 on 6/7/217
    '''   provides a way for EDI 204 ins to send a 997
    ''' </remarks>
    Public Property InternalSecurityQualifier() As String
        Get
            Return _InternalSecurityQualifier
        End Get
        Set(ByVal value As String)
            _InternalSecurityQualifier = value
        End Set
    End Property

    Private _InternalSecurityCode As String
    ''' <summary>
    ''' maps to 997's ISA02 (Comp for 214 response, Carrier for 204 in response)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 -- 6.0.4.7 on 6/7/217
    '''   provides a way for EDI 204 ins to send a 997
    ''' </remarks>
    Public Property InternalSecurityCode() As String
        Get
            Return _InternalSecurityCode
        End Get
        Set(ByVal value As String)
            _InternalSecurityCode = value
        End Set
    End Property

    Private _InternalPartnerQualifier As String
    ''' <summary>
    ''' maps to 997's ISA05 (Comp for 214 response, Carrier for 204 in response)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 -- 6.0.4.7 on 6/7/217
    '''   provides a way for EDI 204 ins to send a 997
    ''' </remarks>
    Public Property InternalPartnerQualifier() As String
        Get
            Return _InternalPartnerQualifier
        End Get
        Set(ByVal value As String)
            _InternalPartnerQualifier = value
        End Set
    End Property

    Private _InternalPartnerCode As String
    ''' <summary>
    ''' maps to 997's ISA06 (Comp for 214 response, Carrier for 204 in response)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 -- 6.0.4.7 on 6/7/217
    '''   provides a way for EDI 204 ins to send a 997
    ''' </remarks>
    Public Property InternalPartnerCode() As String
        Get
            Return _InternalPartnerCode
        End Get
        Set(ByVal value As String)
            _InternalPartnerCode = value
        End Set
    End Property

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Public AlertList As New Dictionary(Of Integer, String)

    Public Sub New()
        MyBase.New()

    End Sub

    Public Sub New(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
        MyBase.New()
        addDataFromString(strSource, strSegSep, strSEElem)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ISA"></param>
    ''' <param name="GS"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="strDBServer"></param>
    ''' <param name="strDatabase"></param>
    ''' <param name="strMSG"></param>
    ''' <param name="oCon"></param>
    ''' <param name="fileName"></param>
    ''' <param name="DateProcessed"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' Added FileName and DateProcessed functionality
    ''' Added process997s from 210Out
    ''' </remarks>
    Public Function processData(ByVal ISA As clsEDIISA,
                                ByVal GS As clsEDIGS,
                                ByVal CarrierControl As Integer,
                                ByVal strDBServer As String,
                                ByVal strDatabase As String,
                                ByRef strMSG As String,
                                ByRef oCon As System.Data.SqlClient.SqlConnection,
                                Optional ByVal fileName As String = "",
                                Optional ByVal DateProcessed As Date = Nothing) As Boolean
        'Dim oCon As New System.Data.SqlClient.SqlConnection
        strMSG = "Success!"
        Try
            'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
            Dim oWCFPar = New DAL.WCFParameters() With {.Database = strDatabase,
                                                               .DBServer = strDBServer,
                                                               .UserName = "EDI Integration",
                                                               .WCFAuthCode = "NGLSystem"}
            Dim oEDIData As New DAL.NGLEDIData(oWCFPar)
            Dim oSecData As New DAL.NGLSecurityDataProvider(oWCFPar)

            'Define the fields we will use to process the 997 data
            'For now we only process 204 997 responses.
            If AK1.AK101 = "SM" Then
                'this is a 204 so process the data
                Dim intGSSequence As Integer = 0
                Integer.TryParse(AK1.AK102, intGSSequence)
                If intGSSequence > 0 Then
                    'we have a valid GS identifier
                    'Use the GSSequence # to get the current book control from the tblEDITrans table using the current
                    'ISA Sender Code in the 997 this is a reference to the tblEDITransReceiverCode 
                    'We sort the order by tblEDITransControl descending and only read one record so 
                    'we always get the most recent data (in case we have duplicate GS Sequence Numbers 
                    'In the log).
                    '@TODO Make sure that the field tblEDITransGSSequence is 100%  unique in this table
                    'This field is populated by the 204 export routine (inside field getTransactionLog)
                    'Find out what tblEDITransReceiverCode is and if the above field is unique by tblEDITransReceiverCode
                    Dim strSQL As String = "Select top 1 tblEDITransBookControl From dbo.tblEDITrans Where tblEDITransGSSequence = " & intGSSequence & " And tblEDITransReceiverCode = '" & Left(Trim(GS.GS02), 15) & "' AND tblEDITransXaction = '204' Order by tblEDITransControl Desc"
                    Dim intBookControl As Integer = 0
                    Try
                        Dim oQuery As New Ngl.Core.Data.Query(strDBServer, strDatabase)
                        If Not Integer.TryParse(oQuery.getScalarValue(oCon, strSQL, 1), intBookControl) Then
                            strMSG = "The following query did not return a valid Book Control Number from the EDI transaction log: " & strSQL
                            Return False
                        Else

                            'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                            If AK5.AK501 = "A" Then
                                'The 204 transmission has been accepted
                                oEDIData.InsertFileNamesTo204Table(intGSSequence, DateProcessed, DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI997Pass, FileName997:=fileName)
                            ElseIf AK5.AK501 = "R" Then
                                'The 204 transmission has been rejected
                                oEDIData.InsertFileNamesTo204Table(intGSSequence, DateProcessed, DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI997Fail, FileName997:=fileName)
                            End If

                            'Update the status of the record to received.
                            strSQL = "Select top 1 CarrierEDIAcceptOn997 From dbo.CarrierEDI Where CarrierEDICarrierControl = " & CarrierControl & " AND CarrierEDIXaction = '204'"
                            Dim intCarrierEDIAcceptOn997 As Integer
                            Dim strScalar = oQuery.getScalarValue(oCon, strSQL, 1)
                            If IsNumeric(strScalar) Then
                                Integer.TryParse(strScalar, intCarrierEDIAcceptOn997)
                            ElseIf Not String.IsNullOrWhiteSpace(strScalar) AndAlso strScalar.ToUpper = "TRUE" Then
                                intCarrierEDIAcceptOn997 = 1
                            Else
                                intCarrierEDIAcceptOn997 = 0
                            End If
                            If intCarrierEDIAcceptOn997 <> 1 Then intCarrierEDIAcceptOn997 = 0
                            Dim CarrierEDIAcceptOn997 As Boolean = True
                            If intCarrierEDIAcceptOn997 = 0 Then
                                CarrierEDIAcceptOn997 = False
                            End If
                            Try
                                Dim oCmd As New System.Data.SqlClient.SqlCommand
                                oCmd.Parameters.AddWithValue("@BookControl", intBookControl)
                                oCmd.Parameters.AddWithValue("@BookTrackComment", "Load Tender Confirmation Received Via 997")
                                oCmd.Parameters.AddWithValue("@BookTrackContact", "EDI 997")
                                oCmd.Parameters.AddWithValue("@EDITransaction", 1)
                                oQuery.execNGLStoredProcedure(oCon, oCmd, "sp204ConfirmedBy99770", 3)
                            Catch ex As Exception
                                strMSG = ex.Message
                            End Try

                            Dim obll As New BLL.NGLBookBLL(Configuration.createWCFParameters(strDBServer, strDatabase, oCon.ConnectionString))

                            If CarrierEDIAcceptOn997 = True Then
                                strSQL = "Select top 1 CarrierEDIEmailNotificationOn From dbo.CarrierEDI Where CarrierEDIXaction = '997' AND CarrierEDICarrierControl = " & CarrierControl
                                Dim intSendEmail As Integer
                                Dim strScalar2 = oQuery.getScalarValue(oCon, strSQL, 1)
                                If IsNumeric(strScalar2) Then
                                    Integer.TryParse(strScalar2, intSendEmail)
                                ElseIf Not String.IsNullOrWhiteSpace(strScalar2) AndAlso strScalar2.ToUpper = "TRUE" Then
                                    intSendEmail = 1
                                Else
                                    intSendEmail = 0
                                End If
                                If intSendEmail <> 1 Then intSendEmail = 0
                                Dim sendEmail As Boolean = True
                                If intSendEmail = 0 Then
                                    sendEmail = False
                                End If
                                'this is an accept
                                Dim result = obll.AcceptOrRejectLoad(intBookControl, CarrierControl, 0, FM.BLL.NGLBookBLL.AcceptRejectEnum.Accepted, sendEmail, "Load Tender Accepted Via 997", 0, "", "", FM.BLL.NGLBookBLL.AcceptRejectModeEnum.EDI, "EDI 997")
                                If result.Success = False Then
                                    strMSG &= result.getAllMessagesNotLocalized()
                                    Return False
                                End If
                            End If

                        End If
                    Catch ex As Ngl.Core.DatabaseRetryExceededException
                        strMSG = "Failed to update the load confirmation status: " & ex.Message
                        Return False
                    Catch ex As Ngl.Core.DatabaseLogInException
                        strMSG = "Database login failure: " & ex.Message
                        Return False
                    Catch ex As Ngl.Core.DatabaseInvalidException
                        strMSG = "Database access failure : " & ex.Message
                        Return False
                    Catch ex As Ngl.Core.DatabaseDataValidationException
                        strMSG = ex.Message
                        Return False
                    Catch ex As Exception
                        Throw
                        Return False
                    End Try
                End If
            End If
            '********************** Test code used to write data to a file *******************************
            'Dim strToPrint As String = "*************** 997 *********************" & vbCrLf
            'strToPrint &= "ISA Sender: " & ISA.ISA06 & vbCrLf
            'strToPrint &= "GS Sender: " & GS.GS02 & vbCrLf
            'strToPrint &= "GS Date: " & GS.GS04 & vbCrLf
            'strToPrint &= "GS Time: " & GS.GS05 & vbCrLf
            'strToPrint &= "GS Group Control: " & GS.GS06 & vbCrLf
            'strToPrint &= "ST: " & ST.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'strToPrint &= "AK1: " & AK1.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'strToPrint &= "AK2: " & AK2.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'strToPrint &= "AK3: " & AK3.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'strToPrint &= "AK4: " & AK4.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'strToPrint &= "AK5: " & AK5.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'strToPrint &= "AK9: " & AK9.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'strToPrint &= "SE: " & SE.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'strToPrint &= "*****************************************" & vbCrLf
            'strToPrint &= vbCrLf

            'Dim FileName As String = "C:\Data\Mizkan\Test\Ruan997" & Date.Now.ToString("ddMMHHmmss") & ".txt"

            'Dim fi As FileInfo = New FileInfo(FileName)
            ''create the file if it does not exist
            'If Not File.Exists(FileName) Then
            '    Using w As StreamWriter = fi.CreateText
            '        w.Close()
            '    End Using
            'End If
            ''now open the file and wite the data
            'Using sw As New StreamWriter(FileName)
            '    sw.Write(strToPrint)
            '    sw.Flush()
            'End Using

            'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
            If AK1.AK101 = "IM" Then
                'this is a 210 so process the data
                Dim intGSSequence As Integer = 0
                Integer.TryParse(AK1.AK102, intGSSequence)
                If intGSSequence > 0 Then
                    'we have a valid GS identifier
                    'Use the GSSequence # to get the current reference control from the tblEDITrans table using the current
                    'ISA Sender Code in the 997 this is a reference to the tblEDITransReceiverCode 
                    'We sort the order by tblEDITransControl descending and only read one record so 
                    'we always get the most recent data (in case we have duplicate GS Sequence Numbers 
                    'In the log).
                    Dim intRefControl As Integer = 0
                    Dim strSQL As String = "Select top 1 tblEDITransRefControl From dbo.tblEDITrans Where tblEDITransGSSequence = " & intGSSequence & " AND tblEDITransReceiverCode = '" & Left(Trim(GS.GS02), 15) & "' AND tblEDITransXaction = '888' Order by tblEDITransControl Desc"

                    Try
                        Dim oQuery As New Ngl.Core.Data.Query(strDBServer, strDatabase)
                        If Not Integer.TryParse(oQuery.getScalarValue(oCon, strSQL, 1), intRefControl) Then
                            strMSG = "The following query did not return a valid Reference Control Number from the EDI transaction log: " & strSQL
                            Return False
                        Else
                            If intRefControl <> 0 Then
                                If AK5.AK501 = "A" Then
                                    'The 210 transmission has been accepted
                                    Dim results = oEDIData.InvoiceUpdateOn997(intRefControl)

                                    If results.ErrNumber <> 0 Then
                                        'There were errors, add them to the dictionary
                                        If AlertList.ContainsKey(results.ErrNumber) Then
                                            AlertList(results.ErrNumber) += results.RetMsg
                                        Else
                                            AlertList.Add(results.ErrNumber, results.RetMsg)
                                        End If
                                    End If

                                    oEDIData.InsertFileNameTo210Table(intRefControl, FileName997:=fileName)

                                ElseIf AK5.AK501 = "R" Then
                                    'The 210 transmission has been rejected
                                    Dim strSQL2 As String = "Select top 1 tblEDITransLoadNumber From dbo.tblEDITrans Where tblEDITransRefControl = " & intRefControl & " AND tblEDITransXaction = '888' Order by tblEDITransControl Desc"
                                    Dim BookPro = oQuery.getScalarValue(oCon, strSQL2, 1)
                                    Dim str997 = Me.getRecord(ISA, GS)
                                    Dim rejectMsg = "An EDI 210 transmission with Invoice Number : " + BookPro + " was rejected via 997." + vbCrLf + str997 + vbCrLf + vbCrLf


                                    If AlertList.ContainsKey(1) Then
                                        AlertList(1) += rejectMsg
                                    Else
                                        AlertList.Add(1, rejectMsg)
                                    End If


                                    Dim results = oEDIData.EDI210InvoiceRejectedVia997(intRefControl)
                                    If results.ErrNumber <> 0 Then
                                        'There were errors, add them to the dictionary
                                        If AlertList.ContainsKey(results.ErrNumber) Then
                                            AlertList(results.ErrNumber) += results.RetMsg
                                        Else
                                            AlertList.Add(results.ErrNumber, results.RetMsg)
                                        End If
                                    End If

                                    oEDIData.InsertFileNameTo210Table(intRefControl, FileName997:=fileName)

                                End If
                            Else
                                strMSG = "997 file failed to process because 210EDIControl cannot be 0." + vbCrLf + "The following query did not return a valid Reference Control Number from the EDI transaction log: " & strSQL
                                Return False
                            End If
                        End If
                    Catch ex As Ngl.Core.DatabaseRetryExceededException
                        strMSG = "Failed to update the load confirmation status: " & ex.Message
                        Return False
                    Catch ex As Ngl.Core.DatabaseLogInException
                        strMSG = "Database login failure: " & ex.Message
                        Return False
                    Catch ex As Ngl.Core.DatabaseInvalidException
                        strMSG = "Database access failure : " & ex.Message
                        Return False
                    Catch ex As Ngl.Core.DatabaseDataValidationException
                        strMSG = ex.Message
                        Return False
                    Catch ex As Exception
                        Throw
                        Return False
                    End Try
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
        Return True
    End Function

    Public Function testData(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS) As Boolean
        'For testing we just write the data to a file
        Try
            Dim strFilePath As String = "C:\Data\EDI\Test\"
            If Not Directory.Exists(strFilePath) Then Directory.CreateDirectory(strFilePath)
            Dim FileName As String = strFilePath & "997" & Date.Now.ToString("ddMMHHmmss") & ".txt"

            Dim fi As FileInfo = New FileInfo(FileName)
            'create the file if it does not exist
            If Not File.Exists(FileName) Then
                Using w As StreamWriter = fi.CreateText
                    w.Close()
                End Using
            End If
            'now open the file and wite the data
            Using sw As New StreamWriter(FileName)
                sw.Write(getRecord(ISA, GS))
                sw.Flush()
            End Using
        Catch ex As Exception
            'do nothing for test
        End Try
        Return True
    End Function

    Public Function writeRecord(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS, Optional ByVal strFilePath As String = "C:\Data\EDI\Test\") As Boolean
        'For testing we just write the data to a file
        Try
            strFilePath = strFilePath?.Trim
            If Not Right(strFilePath, 1) = "\" Then strFilePath &= "\"
            If Not Directory.Exists(strFilePath) Then Directory.CreateDirectory(strFilePath)
            Dim FileName As String = strFilePath & "997Record" & Date.Now.ToString("ddMMHHmmss") & ".txt"

            Dim fi As FileInfo = New FileInfo(FileName)
            'create the file if it does not exist
            If Not File.Exists(FileName) Then
                Using w As StreamWriter = fi.CreateText
                    w.Close()
                End Using
            End If
            'now open the file and wite the data
            Using sw As New StreamWriter(FileName)
                sw.Write(getRecord(ISA, GS))
                sw.Flush()
            End Using
        Catch ex As Exception
            'do nothing for test
        End Try
        Return True
    End Function

    Public Function getRecord(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS) As String
        'Format the record for reporting.
        Dim strToPrint As String = "*************** 997 *********************" & vbCrLf
        strToPrint &= "ISA Sender: " & ISA.ISA06 & vbCrLf
        strToPrint &= "GS Sender: " & GS.GS02 & vbCrLf
        strToPrint &= "GS Date: " & GS.GS04 & vbCrLf
        strToPrint &= "GS Time: " & GS.GS05 & vbCrLf
        strToPrint &= "GS Group Control: " & GS.GS06 & vbCrLf
        strToPrint &= "ST: " & ST.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "AK1: " & AK1.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "AK2: " & AK2.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "AK3: " & AK3.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "AK4: " & AK4.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "AK5: " & AK5.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "AK9: " & AK9.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "SE: " & SE.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "*****************************************" & vbCrLf
        strToPrint &= vbCrLf

        Return strToPrint

    End Function

    ''' <summary>
    ''' Creates an EDI 997 document string
    ''' </summary>
    ''' <param name="strISA"></param>
    ''' <param name="strGS"></param>
    ''' <param name="strIEA"></param>
    ''' <param name="strGE"></param>
    ''' <param name="SegmentTerminator"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 4/13/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Function getEDIRecord(ByVal strISA As String, ByVal strGS As String, ByVal strIEA As String, ByVal strGE As String, ByVal SegmentTerminator As String) As String
        'Format the edi record
        Dim strToPrint As String = ""
        strToPrint &= strISA
        strToPrint &= strGS
        strToPrint &= ST.getEDIString(SegmentTerminator)
        strToPrint &= AK1.getEDIString(SegmentTerminator)
        strToPrint &= AK5.getEDIString(SegmentTerminator)
        strToPrint &= AK9.getEDIString(SegmentTerminator)
        strToPrint &= SE.getEDIString(SegmentTerminator)
        strToPrint &= strGE
        strToPrint &= strIEA

        Return strToPrint

    End Function

    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
        'add the AK9 to ST elements if they exist
        For isubsegs As Integer = 9 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the ST record (it should be all that is left  
                    If Left(sElems(0), 3) <> "ST*" Then sElems(0) = "ST*" & sElems(0)
                    ST = New clsEDIST(sElems(0))
                Case 1
                    'read the AK1 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "AK1\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        AK1 = New clsEDIAK1("AK1*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 2
                    'read the AK2 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "AK2\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        AK2 = New clsEDIAK2("AK2*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 3
                    'read the AK3 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "AK3\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        AK3 = New clsEDIAK3("AK3*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 4
                    'read the AK4 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "AK4\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        AK4 = New clsEDIAK4("AK4*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 5
                    'read the AK5 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "AK5\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        AK5 = New clsEDIAK5("AK5*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 9
                    'read the AK9 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "AK9\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        AK9 = New clsEDIAK9("AK9*" & sElems(0))
                        strSource = segs(0)
                    End If
            End Select
        Next
        'Add the SE
        If Not String.IsNullOrEmpty(strSEElem) Then
            'split out any unwanted elements 
            Dim sElems() As String = strSEElem.Split(strSegSep)
            If Left(sElems(0), 3) <> "SE*" Then sElems(0) = "SE*" & sElems(0)
            SE = New clsEDISE(sElems(0))
        End If
    End Sub
    ''' <summary>
    ''' This method only uses the AK1 and AK9 segments
    ''' </summary>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getEDIString(ByVal sSegTerm As String) As String
        Return ST.getEDIString(sSegTerm) _
            & AK1.getEDIString(sSegTerm) _
            & AK9.getEDIString(sSegTerm) _
            & SE.getEDIString(sSegTerm)

    End Function
End Class

#End Region

#Region " EDI 214 Class"

Public Class clsEDI214EventSettings

    Public Enum EventDateType
        AppointmentSet
        Checkin
        Start
        Finish
        Checkout
    End Enum

    Public blnPickup As Boolean = True
    Public enmEventDateType As clsEDI214EventSettings.EventDateType = EventDateType.AppointmentSet
    Public DateTimeValidationType As BLL.NGLBookBLL.AMSDateTimeValidationType = BLL.NGLBookBLL.AMSDateTimeValidationType.AMSApptStartDate
    Public strEventMsg As String = "Pickup Appointment Date and Time"
    Public strNonManagedMsg As String = "Carrier appointment set at pickup location."

End Class

Public Class clsEDI214FactoryDetail

    Private _FactoryName As String = "Default"
    Public Property FactoryName() As String
        Get
            Return _FactoryName
        End Get
        Set(ByVal value As String)
            _FactoryName = value
        End Set
    End Property

    Private _StatusDetail As clsEDILoadStatusData
    ''' <summary>
    ''' AT701 or AT703 data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StatusDetail() As clsEDILoadStatusData
        Get
            Return _StatusDetail
        End Get
        Set(ByVal value As clsEDILoadStatusData)
            _StatusDetail = value
        End Set
    End Property

    Private _StatusMessage As String
    ''' <summary>
    ''' text from EDISDescription for StatusDetail if available
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StatusMessage() As String
        Get
            Return _StatusMessage
        End Get
        Set(ByVal value As String)
            _StatusMessage = value
        End Set
    End Property

    Private _StatusLoadStatusControl As Integer
    ''' <summary>
    ''' Maps to EDISLoadStatusControl for StatusDetail
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StatusLoadStatusControl() As Integer
        Get
            Return _StatusLoadStatusControl
        End Get
        Set(ByVal value As Integer)
            _StatusLoadStatusControl = value
        End Set
    End Property

    Private _ReasonDetail As clsEDILoadStatusData
    ''' <summary>
    ''' AT702 or AT704 data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ReasonDetail() As clsEDILoadStatusData
        Get
            Return _ReasonDetail
        End Get
        Set(ByVal value As clsEDILoadStatusData)
            _ReasonDetail = value
        End Set
    End Property

    Private _ReasonMessage As String
    ''' <summary>
    ''' text from EDISDescription for StatusReason if available
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ReasonMessage() As String
        Get
            Return _ReasonMessage
        End Get
        Set(ByVal value As String)
            _ReasonMessage = value
        End Set
    End Property

    Private _ReasonLoadStatusControl As Integer
    ''' <summary>
    ''' Maps to EDISLoadStatusControl for ReasonDetail
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ReasonLoadStatusControl() As Integer
        Get
            Return _ReasonLoadStatusControl
        End Get
        Set(ByVal value As Integer)
            _ReasonLoadStatusControl = value
        End Set
    End Property



End Class


Public Class clsEDI214

#Region " Enums "

    Public Enum Alerts
        None 'Do not raise an alert
        AlertPick2141ApptNotMatch 'Do Not Update BookCarrier date and time
        AlertPick2141ApptMissing  'Update BookCarrier date and time
        AlertPickBook1ApptNotMatch 'Do Not Update BookCarrier date and time
        AlertPick2142CheckInMissingAppt 'Update BookCarrier date and time
        AlertPick2142CheckInMissingApptCheckIn  'Update BookCarrier date and time
        AlertPick2142CheckInNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertPickBook2CheckInNotMatchAppt 'Do Not Update BookCarrier date and time 
        AlertPick2143StartMissingAppt 'Update BookCarrier date and time
        AlertPick2143StartMissingApptStart  'Update BookCarrier date and time
        AlertPick2143StartNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertPickBook3StartNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertPick2144FinishMissingAppt 'Update BookCarrier date and time
        AlertPick2144FinishMissingApptFinish  'Update BookCarrier date and time
        AlertPick2144FinishNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertPickBook4FinishNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertPick2145DepartMissingAppt 'Update BookCarrier date and time
        AlertPick2145DepartMissingApptCheckout  'Update BookCarrier date and time
        AlertPick2145DepartNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertPickBook5DepartNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertDel2141ApptNotMatch 'Do Not Update BookCarrier date and time
        AlertDel2141ApptMissing  'Update BookCarrier date and time
        AlertDelBook1ApptNotMatch 'Do Not Update BookCarrier date and time
        AlertDel2142CheckInMissingAppt 'Update BookCarrier date and time
        AlertDel2142CheckInMissingApptCheckIn  'Update BookCarrier date and time
        AlertDel2142CheckInNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertDelBook2CheckInNotMatchAppt 'Do Not Update BookCarrier date and time 
        AlertDel2143StartMissingAppt 'Update BookCarrier date and time
        AlertDel2143StartMissingApptStart  'Update BookCarrier date and time
        AlertDel2143StartNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertDelBook3StartNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertDel2144FinishMissingAppt 'Update BookCarrier date and time
        AlertDel2144FinishMissingApptFinish  'Update BookCarrier date and time
        AlertDel2144FinishNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertDelBook4FinishNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertDel2145DepartMissingAppt 'Update BookCarrier date and time
        AlertDel2145DepartMissingApptCheckout  'Update BookCarrier date and time
        AlertDel2145DepartNotMatchAppt 'Do Not Update BookCarrier date and time
        AlertDelBook5DepartNotMatchAppt 'Do Not Update BookCarrier date and time
    End Enum

#End Region

#Region " Properties"


    Private _WCFParameters As DAL.WCFParameters
    Public Property WCFParameters() As DAL.WCFParameters
        Get
            If _WCFParameters Is Nothing Then
                With _WCFParameters
                    .UserName = "System Download"
                    .Database = Me.Database
                    .DBServer = Me.DBServer
                    .ConnectionString = Me.ConnectionString
                    .WCFAuthCode = "NGLSystem"
                    .ValidateAccess = False
                End With
            End If
            Return _WCFParameters
        End Get
        Set(ByVal value As DAL.WCFParameters)
            _WCFParameters = value
        End Set
    End Property

    Private mstrDBServer As String = ""
    Public Property DBServer() As String
        Get
            Return mstrDBServer
        End Get
        Set(ByVal value As String)
            mstrDBServer = value
        End Set
    End Property

    Private mstrDatabase As String = ""
    Public Property Database() As String
        Get
            Return mstrDatabase
        End Get
        Set(ByVal value As String)
            mstrDatabase = value
        End Set
    End Property

    Private mstrConnection As String = ""
    Public Property ConnectionString() As String
        Get
            If mstrConnection?.Trim.Length < 1 Then
                mstrConnection = String.Format("Server={0};Database={1};Integrated Security=SSPI", Me.DBServer, Me.Database)
            End If
            Return mstrConnection
        End Get
        Set(ByVal value As String)
            mstrConnection = value
        End Set
    End Property

    Private _otblAlertMessageDAL As DAL.NGLtblAlertMessageData
    Public Property otblAlertMessageDAL() As DAL.NGLtblAlertMessageData
        Get
            If _otblAlertMessageDAL Is Nothing Then _otblAlertMessageDAL = New DAL.NGLtblAlertMessageData(Me.WCFParameters)
            Return _otblAlertMessageDAL
        End Get
        Set(ByVal value As DAL.NGLtblAlertMessageData)
            _otblAlertMessageDAL = value
        End Set
    End Property


    Private _oBookDAL As DAL.NGLBookData
    Public Property oBookDAL() As DAL.NGLBookData
        Get
            If _oBookDAL Is Nothing Then _oBookDAL = New DAL.NGLBookData(Me.WCFParameters)
            Return _oBookDAL
        End Get
        Set(ByVal value As DAL.NGLBookData)
            _oBookDAL = value
        End Set
    End Property

    Private _oBookTrackDAL As DAL.NGLBookTrackData
    Public Property oBookTrackDAL() As DAL.NGLBookTrackData
        Get
            If _oBookTrackDAL Is Nothing Then _oBookTrackDAL = New DAL.NGLBookTrackData(Me.WCFParameters)
            Return _oBookTrackDAL
        End Get
        Set(ByVal value As DAL.NGLBookTrackData)
            _oBookTrackDAL = value
        End Set
    End Property

    Private _oBookBLL As BLL.NGLBookBLL
    Public Property oBookBLL() As BLL.NGLBookBLL
        Get
            If _oBookBLL Is Nothing Then
                _oBookBLL = New BLL.NGLBookBLL(Me.WCFParameters)
            End If
            Return _oBookBLL
        End Get
        Set(ByVal value As BLL.NGLBookBLL)
            _oBookBLL = value
        End Set
    End Property


    Private _oStatusCodeDAL As DAL.NGLEDIStatusCodeData
    Public Property oStatusCodeDAL() As DAL.NGLEDIStatusCodeData
        Get
            If _oStatusCodeDAL Is Nothing Then _oStatusCodeDAL = New DAL.NGLEDIStatusCodeData(Me.WCFParameters)
            Return _oStatusCodeDAL
        End Get
        Set(ByVal value As DAL.NGLEDIStatusCodeData)
            _oStatusCodeDAL = value
        End Set
    End Property

    Private _oNGLLoadStatusDAL As DAL.NGLLoadStatusCodeData
    Public Property oNGLLoadStatusDAL() As DAL.NGLLoadStatusCodeData
        Get
            If _oNGLLoadStatusDAL Is Nothing Then _oNGLLoadStatusDAL = New DAL.NGLLoadStatusCodeData(Me.WCFParameters)
            Return _oNGLLoadStatusDAL
        End Get
        Set(ByVal value As DAL.NGLLoadStatusCodeData)
            _oNGLLoadStatusDAL = value
        End Set
    End Property

    Private _oNGLAMSAppointmentDAL As DAL.NGLAMSAppointmentData
    Public Property oNGLAMSAppointmentDAL() As DAL.NGLAMSAppointmentData
        Get
            If _oNGLAMSAppointmentDAL Is Nothing Then _oNGLAMSAppointmentDAL = New DAL.NGLAMSAppointmentData(Me.WCFParameters)
            Return _oNGLAMSAppointmentDAL
        End Get
        Set(ByVal value As DAL.NGLAMSAppointmentData)
            _oNGLAMSAppointmentDAL = value
        End Set
    End Property

    Private _oNGLLoadStatusCodeDataDAL As DAL.NGLLoadStatusCodeData
    Public Property oNGLLoadStatusCodeDataDAL() As DAL.NGLLoadStatusCodeData
        Get
            If _oNGLLoadStatusCodeDataDAL Is Nothing Then _oNGLLoadStatusCodeDataDAL = New DAL.NGLLoadStatusCodeData(Me.WCFParameters)
            Return _oNGLLoadStatusCodeDataDAL
        End Get
        Set(ByVal value As DAL.NGLLoadStatusCodeData)
            _oNGLLoadStatusCodeDataDAL = value
        End Set
    End Property

    Private _Alert21409LoadStatusControl As Integer
    Public Property Alert21409LoadStatusControl() As Integer
        Get
            If _Alert21409LoadStatusControl = 0 Then
                'we need to lookup the value from the database
                _Alert21409LoadStatusControl = oNGLLoadStatusCodeDataDAL.GetLoadStatusControl(21409, "EDI: 214 Load Status Alert Message", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.EDI)
            End If
            Return _Alert21409LoadStatusControl
        End Get
        Set(ByVal value As Integer)
            _Alert21409LoadStatusControl = value
        End Set
    End Property




#End Region

#Region " Constructors"

    Public Sub New()
        MyBase.New()

    End Sub


    Public Sub New(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
        MyBase.New()
        addDataFromString(strSource, strSegSep, strSEElem)
    End Sub

#End Region

    Public Function getAlertTypeBySetting(ByVal oSettings As clsEDI214EventSettings, ByVal enmValidationResult As BLL.NGLBookBLL.AMSDateTimeValidationResults, ByRef strAlertComment As String, Optional ByVal blnBookAlertOnly As Boolean = False) As clsEDI214.Alerts
        Dim enmRet As clsEDI214.Alerts = Alerts.None
        strAlertComment = ""
        'Note: we add a space at the end of each strAlertComment in case messages are concatenated together
        If oSettings.blnPickup Then
            Select Case oSettings.enmEventDateType
                Case clsEDI214EventSettings.EventDateType.AppointmentSet
                    If blnBookAlertOnly Then
                        enmRet = clsEDI214.Alerts.AlertPickBook1ApptNotMatch
                        strAlertComment = "Booked Pickup Appt Does Not Match Schedule. "
                    Else
                        Select Case enmValidationResult
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistAndMatches
                                enmRet = Alerts.None
                                strAlertComment = "Carrier acknowledges pickup appointment date and time. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist
                                enmRet = clsEDI214.Alerts.AlertPick2141ApptMissing
                                strAlertComment = "Carrier appointment accepted at pickup but does not exist in calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistDateIsBeforeAppt
                                enmRet = clsEDI214.Alerts.AlertPick2141ApptNotMatch
                                strAlertComment = "Carrier pickup appointment date is before scheduled appointment date. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty
                                strAlertComment = "Scheduled pickup appointment date is empty. "
                                enmRet = Alerts.AlertPick2141ApptNotMatch
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDataAndTimeDoNotMatch
                                enmRet = clsEDI214.Alerts.AlertPick2141ApptNotMatch
                                strAlertComment = "Carrier pickup appointment date and time does not match calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDateIsAfterAppt
                                enmRet = clsEDI214.Alerts.AlertPick2141ApptNotMatch
                                strAlertComment = "Carrier pickup appointment date is after scheduled appointment date. "
                        End Select
                    End If
                Case clsEDI214EventSettings.EventDateType.Checkin
                    If blnBookAlertOnly Then
                        enmRet = clsEDI214.Alerts.AlertPickBook2CheckInNotMatchAppt
                        strAlertComment = "Booked Pickup Check In Does Not Match Schedule. "
                    Else
                        Select Case enmValidationResult
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistAndMatches
                                enmRet = Alerts.None
                                strAlertComment = "Carrier arrived at pickup location. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist
                                enmRet = clsEDI214.Alerts.AlertPick2142CheckInMissingAppt
                                strAlertComment = "Carrier arrived at pickup but an appointment was not created in the calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistDateIsBeforeAppt
                                enmRet = clsEDI214.Alerts.AlertPick2142CheckInNotMatchAppt
                                strAlertComment = "Carrier pickup check in date is before scheduled check in date. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty
                                strAlertComment = "Scheduled pickup check in date is empty. "
                                enmRet = Alerts.AlertPick2142CheckInMissingApptCheckIn
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDataAndTimeDoNotMatch
                                enmRet = clsEDI214.Alerts.AlertPick2142CheckInNotMatchAppt
                                strAlertComment = "Carrier pickup check in date and time does not match calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDateIsAfterAppt
                                enmRet = clsEDI214.Alerts.AlertPick2142CheckInNotMatchAppt
                                strAlertComment = "Carrier pickup check in date is after scheduled check in date. "
                        End Select
                    End If
                Case clsEDI214EventSettings.EventDateType.Start
                    If blnBookAlertOnly Then
                        enmRet = clsEDI214.Alerts.AlertPickBook3StartNotMatchAppt
                        strAlertComment = "Booked Start Loading Does Not Match Schedule. "
                    Else
                        Select Case enmValidationResult
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistAndMatches
                                enmRet = Alerts.None
                                strAlertComment = "Carrier started loading at pickup location. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist
                                enmRet = clsEDI214.Alerts.AlertPick2143StartMissingAppt
                                strAlertComment = "Carrier started loading at pickup but an appointment was not created in the calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistDateIsBeforeAppt
                                enmRet = clsEDI214.Alerts.AlertPick2143StartNotMatchAppt
                                strAlertComment = "Carrier pickup start loading date is before scheduled start loading date. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty
                                strAlertComment = "Scheduled pickup start loading date is empty. "
                                enmRet = Alerts.AlertPick2143StartMissingApptStart
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDataAndTimeDoNotMatch
                                enmRet = clsEDI214.Alerts.AlertPick2143StartNotMatchAppt
                                strAlertComment = "Carrier pickup start loading date and time does not match calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDateIsAfterAppt
                                enmRet = clsEDI214.Alerts.AlertPick2143StartNotMatchAppt
                                strAlertComment = "Carrier pickup start loading date is after scheduled start loading date. "
                        End Select
                    End If
                Case clsEDI214EventSettings.EventDateType.Finish
                    If blnBookAlertOnly Then
                        enmRet = clsEDI214.Alerts.AlertPickBook4FinishNotMatchAppt
                        strAlertComment = "Booked Finish Loading Does Not Match Schedule. "
                    Else
                        Select Case enmValidationResult
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistAndMatches
                                enmRet = Alerts.None
                                strAlertComment = "Carrier completed loading at pickup location. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist
                                enmRet = clsEDI214.Alerts.AlertPick2144FinishMissingAppt
                                strAlertComment = "Carrier completed loading but an appointment was not created in the calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistDateIsBeforeAppt
                                enmRet = clsEDI214.Alerts.AlertPick2144FinishNotMatchAppt
                                strAlertComment = "Carrier pickup completed loading date is before scheduled completed loading date. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty
                                strAlertComment = "Scheduled pickup completed loading date is empty. "
                                enmRet = Alerts.AlertPick2144FinishMissingApptFinish
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDataAndTimeDoNotMatch
                                enmRet = clsEDI214.Alerts.AlertPick2144FinishNotMatchAppt
                                strAlertComment = "Carrier pickup completed loading date and time does not match calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDateIsAfterAppt
                                enmRet = clsEDI214.Alerts.AlertPick2144FinishNotMatchAppt
                                strAlertComment = "Carrier pickup completed loading date is after scheduled completed loading date. "
                        End Select
                    End If
                Case clsEDI214EventSettings.EventDateType.Checkout
                    If blnBookAlertOnly Then
                        enmRet = clsEDI214.Alerts.AlertPickBook5DepartNotMatchAppt
                        strAlertComment = "Booked Pickup Checkout Does Not Match Schedule"
                    Else
                        Select Case enmValidationResult
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistAndMatches
                                enmRet = Alerts.None
                                strAlertComment = "Carrier departed pickup location. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist
                                enmRet = clsEDI214.Alerts.AlertPick2145DepartMissingAppt
                                strAlertComment = "Carrier departed pickup location but an appointment was not created in the calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistDateIsBeforeAppt
                                enmRet = clsEDI214.Alerts.AlertPick2145DepartNotMatchAppt
                                strAlertComment = "Carrier pickup check out date is before scheduled check out date. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty
                                strAlertComment = "Scheduled pickup check out date is empty. "
                                enmRet = Alerts.AlertPick2145DepartMissingApptCheckout
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDataAndTimeDoNotMatch
                                enmRet = clsEDI214.Alerts.AlertPick2145DepartNotMatchAppt
                                strAlertComment = "Carrier pickup check out date and time does not match calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDateIsAfterAppt
                                enmRet = clsEDI214.Alerts.AlertPick2145DepartNotMatchAppt
                                strAlertComment = "Carrier pickup check out date is after scheduled check out date. "
                        End Select
                    End If
            End Select
        Else
            Select Case oSettings.enmEventDateType
                Case clsEDI214EventSettings.EventDateType.AppointmentSet
                    If blnBookAlertOnly Then
                        enmRet = clsEDI214.Alerts.AlertDelBook1ApptNotMatch
                        strAlertComment = "Booked Delivery Appt Does Not Match Schedule. "
                    Else
                        Select Case enmValidationResult
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistAndMatches
                                enmRet = Alerts.None
                                strAlertComment = "Carrier acknowledges delivery appointment date and time. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist
                                enmRet = clsEDI214.Alerts.AlertDel2141ApptMissing
                                strAlertComment = "Carrier appointment accepted at Delivery but does not exist in calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistDateIsBeforeAppt
                                enmRet = clsEDI214.Alerts.AlertDel2141ApptNotMatch
                                strAlertComment = "Carrier delivery appointment date is before scheduled appointment date. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty
                                strAlertComment = "Scheduled delivery appointment date is empty. "
                                enmRet = Alerts.AlertDel2141ApptNotMatch
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDataAndTimeDoNotMatch
                                enmRet = clsEDI214.Alerts.AlertDel2141ApptNotMatch
                                strAlertComment = "Carrier delivery appointment date and time does not match calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDateIsAfterAppt
                                enmRet = clsEDI214.Alerts.AlertDel2141ApptNotMatch
                                strAlertComment = "Carrier delivery appointment date is after scheduled appointment date. "
                        End Select
                    End If
                Case clsEDI214EventSettings.EventDateType.Checkin
                    If blnBookAlertOnly Then
                        enmRet = clsEDI214.Alerts.AlertDelBook2CheckInNotMatchAppt
                        strAlertComment = "Booked Delivery Check In Does Not Match Schedule. "
                    Else
                        Select Case enmValidationResult
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistAndMatches
                                enmRet = Alerts.None
                                strAlertComment = "Carrier arrived at delivery location. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist
                                enmRet = clsEDI214.Alerts.AlertDel2142CheckInMissingAppt
                                strAlertComment = "Carrier arrived at delivery but an appointment was not created in the calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistDateIsBeforeAppt
                                enmRet = clsEDI214.Alerts.AlertDel2142CheckInNotMatchAppt
                                strAlertComment = "Carrier delivery check in date is before scheduled check in date. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty
                                strAlertComment = "Scheduled delivery check in date is empty. "
                                enmRet = Alerts.AlertDel2142CheckInMissingApptCheckIn
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDataAndTimeDoNotMatch
                                enmRet = clsEDI214.Alerts.AlertDel2142CheckInNotMatchAppt
                                strAlertComment = "Carrier delivery check in date and time does not match calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDateIsAfterAppt
                                enmRet = clsEDI214.Alerts.AlertDel2142CheckInNotMatchAppt
                                strAlertComment = "Carrier delivery check in date is after scheduled check in date. "
                        End Select
                    End If
                Case clsEDI214EventSettings.EventDateType.Start
                    If blnBookAlertOnly Then
                        enmRet = clsEDI214.Alerts.AlertDelBook3StartNotMatchAppt
                        strAlertComment = "Booked Start Unloading Does Not Match Schedule. "
                    Else
                        Select Case enmValidationResult
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistAndMatches
                                enmRet = Alerts.None
                                strAlertComment = "Carrier started unloading at delivery location. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist
                                enmRet = clsEDI214.Alerts.AlertDel2143StartMissingAppt
                                strAlertComment = "Carrier started unloading at delivery but an appointment was not created in the calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistDateIsBeforeAppt
                                enmRet = clsEDI214.Alerts.AlertDel2143StartNotMatchAppt
                                strAlertComment = "Carrier delivery start unloading date is before scheduled start unloading date. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty
                                strAlertComment = "Scheduled delivery start unloading date is empty. "
                                enmRet = Alerts.AlertDel2143StartMissingApptStart
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDataAndTimeDoNotMatch
                                enmRet = clsEDI214.Alerts.AlertDel2143StartNotMatchAppt
                                strAlertComment = "Carrier delivery start unloading date and time does not match calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDateIsAfterAppt
                                enmRet = clsEDI214.Alerts.AlertDel2143StartNotMatchAppt
                                strAlertComment = "Carrier delivery start unloading date is after scheduled start unloading date. "
                        End Select
                    End If
                Case clsEDI214EventSettings.EventDateType.Finish
                    If blnBookAlertOnly Then
                        enmRet = clsEDI214.Alerts.AlertPickBook4FinishNotMatchAppt
                        strAlertComment = "Booked Finish Unloading Does Not Match Schedule. "
                    Else
                        Select Case enmValidationResult
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistAndMatches
                                enmRet = Alerts.None
                                strAlertComment = "Carrier completed unloading at delivery location. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist
                                enmRet = clsEDI214.Alerts.AlertDel2144FinishMissingAppt
                                strAlertComment = "Carrier completed unloading but an appointment was not created in the calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistDateIsBeforeAppt
                                enmRet = clsEDI214.Alerts.AlertDel2144FinishNotMatchAppt
                                strAlertComment = "Carrier delivery completed unloading date is before scheduled completed unloading date. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty
                                strAlertComment = "Scheduled delivery completed unloading date is empty. "
                                enmRet = Alerts.AlertDel2144FinishMissingApptFinish
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDataAndTimeDoNotMatch
                                enmRet = clsEDI214.Alerts.AlertDel2144FinishNotMatchAppt
                                strAlertComment = "Carrier delivery completed unloading date and time does not match calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDateIsAfterAppt
                                enmRet = clsEDI214.Alerts.AlertDel2144FinishNotMatchAppt
                                strAlertComment = "Carrier delivery completed unloading date is after scheduled completed unloading date. "
                        End Select
                    End If
                Case clsEDI214EventSettings.EventDateType.Checkout
                    If blnBookAlertOnly Then
                        enmRet = clsEDI214.Alerts.AlertDelBook5DepartNotMatchAppt
                        strAlertComment = "Booked Delivery Checkout Does Not Match Schedule"
                    Else
                        Select Case enmValidationResult
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistAndMatches
                                enmRet = Alerts.None
                                strAlertComment = "Carrier departed delivery location. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist
                                enmRet = clsEDI214.Alerts.AlertDel2145DepartMissingAppt
                                strAlertComment = "Carrier departed delivery location but an appointment was not created in the calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistDateIsBeforeAppt
                                enmRet = clsEDI214.Alerts.AlertDel2145DepartNotMatchAppt
                                strAlertComment = "Carrier delivery check out date is before scheduled check out date. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty
                                strAlertComment = "Scheduled delivery check out date is empty. "
                                enmRet = Alerts.AlertDel2145DepartMissingApptCheckout
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDataAndTimeDoNotMatch
                                enmRet = clsEDI214.Alerts.AlertDel2145DepartNotMatchAppt
                                strAlertComment = "Carrier delivery check out date and time does not match calendar. "
                            Case BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsDateIsAfterAppt
                                enmRet = clsEDI214.Alerts.AlertDel2145DepartNotMatchAppt
                                strAlertComment = "Carrier delivery check out date is after scheduled check out date. "
                        End Select
                    End If
            End Select

        End If
        Return enmRet
    End Function

    Public Sub updateLoadCarrierData(ByVal oSettings As clsEDI214EventSettings, ByRef oBook As DTO.Book, ByVal EventDate As Date?, ByVal EventTime As Date?)
        If oSettings.blnPickup Then
            Select Case oSettings.enmEventDateType
                Case clsEDI214EventSettings.EventDateType.AppointmentSet
                    oBook.BookCarrScheduleDate = EventDate
                    oBook.BookCarrScheduleTime = EventTime
                Case clsEDI214EventSettings.EventDateType.Checkin
                    oBook.BookCarrActualDate = EventDate
                    oBook.BookCarrActualTime = EventTime
                Case clsEDI214EventSettings.EventDateType.Start
                    oBook.BookCarrStartLoadingDate = EventDate
                    oBook.BookCarrStartLoadingTime = EventTime
                Case clsEDI214EventSettings.EventDateType.Finish
                    oBook.BookCarrFinishLoadingDate = EventDate
                    oBook.BookCarrFinishLoadingTime = EventTime
                Case clsEDI214EventSettings.EventDateType.Checkout
                    oBook.BookCarrActLoadComplete_Date = EventDate
                    oBook.BookCarrActLoadCompleteTime = EventTime
            End Select
        Else
            Select Case oSettings.enmEventDateType
                Case clsEDI214EventSettings.EventDateType.AppointmentSet
                    oBook.BookCarrApptDate = EventDate
                    oBook.BookCarrApptTime = EventTime
                Case clsEDI214EventSettings.EventDateType.Checkin
                    oBook.BookCarrActDate = EventDate
                    oBook.BookCarrActTime = EventTime
                Case clsEDI214EventSettings.EventDateType.Start
                    oBook.BookCarrStartUnloadingDate = EventDate
                    oBook.BookCarrStartUnloadingTime = EventTime
                Case clsEDI214EventSettings.EventDateType.Finish
                    oBook.BookCarrFinishUnloadingDate = EventDate
                    oBook.BookCarrFinishUnloadingTime = EventTime
                Case clsEDI214EventSettings.EventDateType.Checkout
                    oBook.BookCarrActUnloadCompDate = EventDate
                    oBook.BookCarrActUnloadCompTime = EventTime
            End Select
        End If
    End Sub

    Public Function getLoadCarrierDataDateString(ByVal oSettings As clsEDI214EventSettings, ByRef oBook As DTO.Book) As String
        Dim strCarrierDateTime As String = ""
        If oSettings.blnPickup Then
            Select Case oSettings.enmEventDateType
                Case clsEDI214EventSettings.EventDateType.AppointmentSet
                    If oBook.BookCarrScheduleDate.HasValue Then
                        strCarrierDateTime = oBook.BookCarrScheduleDate.Value.ToShortDateString()
                        If oBook.BookCarrScheduleTime.HasValue Then
                            strCarrierDateTime &= " " & oBook.BookCarrScheduleTime.Value.ToShortTimeString()
                        End If
                    End If
                Case clsEDI214EventSettings.EventDateType.Checkin
                    If oBook.BookCarrActualDate.HasValue Then
                        strCarrierDateTime = oBook.BookCarrActualDate.Value.ToShortDateString()
                        If oBook.BookCarrActualTime.HasValue Then
                            strCarrierDateTime &= " " & oBook.BookCarrActualTime.Value.ToShortTimeString()
                        End If
                    End If
                Case clsEDI214EventSettings.EventDateType.Start
                    If oBook.BookCarrStartLoadingDate.HasValue Then
                        strCarrierDateTime = oBook.BookCarrStartLoadingDate.Value.ToShortDateString()
                        If oBook.BookCarrStartLoadingTime.HasValue Then
                            strCarrierDateTime &= " " & oBook.BookCarrStartLoadingTime.Value.ToShortTimeString()
                        End If
                    End If
                Case clsEDI214EventSettings.EventDateType.Finish
                    If oBook.BookCarrActLoadComplete_Date.HasValue Then
                        strCarrierDateTime = oBook.BookCarrActLoadComplete_Date.Value.ToShortDateString()
                        If oBook.BookCarrActLoadCompleteTime.HasValue Then
                            strCarrierDateTime &= " " & oBook.BookCarrActLoadCompleteTime.Value.ToShortTimeString()
                        End If
                    End If
                Case clsEDI214EventSettings.EventDateType.Checkout
                    If oBook.BookCarrFinishLoadingDate.HasValue Then
                        strCarrierDateTime = oBook.BookCarrFinishLoadingDate.Value.ToShortDateString()
                        If oBook.BookCarrFinishLoadingTime.HasValue Then
                            strCarrierDateTime &= " " & oBook.BookCarrFinishLoadingTime.Value.ToShortTimeString()
                        End If
                    End If
            End Select
        Else
            Select Case oSettings.enmEventDateType
                Case clsEDI214EventSettings.EventDateType.AppointmentSet
                    If oBook.BookCarrApptDate.HasValue Then
                        strCarrierDateTime = oBook.BookCarrApptDate.Value.ToShortDateString()
                        If oBook.BookCarrApptTime.HasValue Then
                            strCarrierDateTime &= " " & oBook.BookCarrApptTime.Value.ToShortTimeString()
                        End If
                    End If
                Case clsEDI214EventSettings.EventDateType.Checkin
                    If oBook.BookCarrActDate.HasValue Then
                        strCarrierDateTime = oBook.BookCarrActDate.Value.ToShortDateString()
                        If oBook.BookCarrActTime.HasValue Then
                            strCarrierDateTime &= " " & oBook.BookCarrActTime.Value.ToShortTimeString()
                        End If
                    End If
                Case clsEDI214EventSettings.EventDateType.Start
                    If oBook.BookCarrStartUnloadingDate.HasValue Then
                        strCarrierDateTime = oBook.BookCarrStartUnloadingDate.Value.ToShortDateString()
                        If oBook.BookCarrStartUnloadingTime.HasValue Then
                            strCarrierDateTime &= " " & oBook.BookCarrStartUnloadingTime.Value.ToShortTimeString()
                        End If
                    End If
                Case clsEDI214EventSettings.EventDateType.Finish
                    If oBook.BookCarrFinishUnloadingDate.HasValue Then
                        strCarrierDateTime = oBook.BookCarrFinishUnloadingDate.Value.ToShortDateString()
                        If oBook.BookCarrFinishUnloadingTime.HasValue Then
                            strCarrierDateTime &= " " & oBook.BookCarrFinishUnloadingTime.Value.ToShortTimeString()
                        End If
                    End If
                Case clsEDI214EventSettings.EventDateType.Checkout
                    If oBook.BookCarrActUnloadCompDate.HasValue Then
                        strCarrierDateTime = oBook.BookCarrActUnloadCompDate.Value.ToShortDateString()
                        If oBook.BookCarrActUnloadCompTime.HasValue Then
                            strCarrierDateTime &= " " & oBook.BookCarrActUnloadCompTime.Value.ToShortTimeString()
                        End If
                    End If
            End Select
        End If
        Return strCarrierDateTime
    End Function

    ''' <summary>
    ''' Deprecated overload left to support backward compatibility with older methods.  
    ''' New methods should use the overload without the dblRequireCNS parameter as it is no longer used.
    ''' </summary>
    ''' <param name="ISA"></param>
    ''' <param name="GS"></param>
    ''' <param name="strDBServer"></param>
    ''' <param name="strDatabase"></param>
    ''' <param name="strMSG"></param>
    ''' <param name="dblRequireCNS"></param>
    ''' <param name="oCon"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function processData(ByVal ISA As clsEDIISA,
                               ByVal GS As clsEDIGS,
                               ByVal strDBServer As String,
                               ByVal strDatabase As String,
                               ByRef strMSG As String,
                               ByVal dblRequireCNS As Double,
                               ByRef oCon As System.Data.SqlClient.SqlConnection) As Boolean
        Return processData(ISA, GS, strDBServer, strDatabase, strMSG, oCon)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ISA"></param>
    ''' <param name="GS"></param>
    ''' <param name="strDBServer"></param>
    ''' <param name="strDatabase"></param>
    ''' <param name="strMSG"></param>
    ''' <param name="oCon"></param>
    ''' <param name="fileName">Optional default ""</param>
    ''' <param name="DateProcessed">Optonal default Nothing</param>
    ''' <param name="insertErrorMsg">Optional defualt ""</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 1/6/2015 v-7.0
    '''   Removed References to old RequireCNS logic
    '''   We now always use the BookSHID as the key value
    '''   mapped to segment B204 
    '''   Removed old code with comment tags no longer needed
    ''' Modified by RHR 1/9/15 v-7.0
    '''   Added new Reference to cls214Data object.
    ''' Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' Added fileName and DateProcessed functionality
    ''' Modified by RHR for v-8.2.1 on 11/12/2019 
    '''     Added logic to make sure at least one order record exists.
    '''     If Not we insert a blank record so the system can use a Default On LTL Orders.
    ''' Modified by LVV on 9/8/20 - fixed null reference exception if Loop 100 is missing
    ''' </remarks>
    Public Function processData(ByVal ISA As clsEDIISA,
                                ByVal GS As clsEDIGS,
                                ByVal strDBServer As String,
                                ByVal strDatabase As String,
                                ByRef strMSG As String,
                                ByRef oCon As System.Data.SqlClient.SqlConnection,
                                Optional ByVal fileName As String = "",
                                Optional ByVal DateProcessed As Date = Nothing,
                                Optional ByRef insertErrorMsg As String = "") As Boolean
        Dim blnRet As Boolean = True
        strMSG = "Success!"
        'update the data components
        Me.DBServer = strDBServer
        Me.Database = strDatabase
        Me.WCFParameters = New DAL.WCFParameters With {.UserName = "System Download",
                                                    .Database = Me.Database,
                                                    .DBServer = Me.DBServer,
                                                    .ConnectionString = Me.ConnectionString,
                                                    .WCFAuthCode = "NGLSystem",
                                                    .ValidateAccess = False}
        Dim oEDIData As New DAL.NGLEDIData(Me.WCFParameters) 'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
        Dim oCarrEDIData As New DAL.NGLCarrierEDIData(Me.WCFParameters) 'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
        Dim o214Data As New cls214LoadData
        Try
            'get the data from the isa record
            o214Data.CarrierPartnerCode = ISA.ISA06?.Trim
            o214Data.CompPartnerCode = ISA.ISA08?.Trim

            'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
            Dim dto214 As New DTO.tblEDI214
            dto214.CarrierPartnerCode = ISA.ISA06?.Trim
            dto214.CompPartnerCode = ISA.ISA08?.Trim
            Dim carrierName = oCarrEDIData.getCarrierNameByPartnerCode(dto214.CarrierPartnerCode, "214")
            Dim compName As String = ""
            getCompInfoByCNS(Me.WCFParameters, B10.B1002?.Trim, compName)
            If Not carrierName?.Trim.Length > 0 Then insertErrorMsg = String.Format("Could not look up Carrier Name using CarrierPartnerCode {0} for EDI 214. Attempting to process file {1}; but some data may be missing.  Please check your status updates.  ", dto214.CarrierPartnerCode, fileName)
            If Not compName?.Trim.Length > 0 Then insertErrorMsg += String.Format("Could not look up Company Name using Shipment ID or Pro Number {0} for EDI 214. Attempting to process file {1}; but some data may be missing.  Please check your status updates. ", B10.B1002?.Trim, fileName)

            'Get the Order Data
            o214Data.ShipCarrierProNumber = B10.B1001?.Trim
            o214Data.SHID = B10.B1002?.Trim

            dto214.BookShipCarrierProNumber = B10.B1001?.Trim 'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
            dto214.SHID = B10.B1002?.Trim 'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration

            'get the default Load Status Control used for generic messages 
            'or when a cross reference does not exist for the provided event code.
            o214Data.DefaultLoadStatusControl = oNGLLoadStatusDAL.GetLoadStatusControl(o214Data.DefaultLoadStatusCode, "EDI: 214 Load Status Update Message", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.EDI)
            'Check for an update to the Assigned Carrier Information
            If Loop100 Is Nothing OrElse Loop100.Count < 1 OrElse Loop100(0) Is Nothing Then 'Added By LVV on 9/8/20 - fixed null reference exception if Loop 100 is missing
                strMSG = "Missing Assigned Carrier Information or invalid 100 Loop"
                'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                With dto214
                    .EDI214Message = strMSG + insertErrorMsg
                    .EDI214StatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI214ReceivedWithErrors
                    .EDI214FileName = fileName
                End With
                If Not oEDIData.InsertIntoEDI214(dto214, DateProcessed) Then insertErrorMsg = String.Format("Could not insert record into tblEDI214 for SHID: {0}, Date Processed: {1}, and File Name: {2}", dto214.SHID, DateProcessed.ToString(), fileName)
                Return False
            Else
                For Each L As clsEDI214Loop100 In Loop100
                    If L.N1.N101 = "CA" Then
                        'this is a carrier update so get the name and number 
                        o214Data.ShipCarrierNumber = L.N1.N104?.Trim
                        o214Data.ShipCarrierName = L.N1.N102?.Trim
                        'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                        dto214.BookShipCarrierNumber = L.N1.N104?.Trim
                        dto214.BookShipCarrierName = L.N1.N102?.Trim
                    End If
                Next
            End If

            If Loop200 Is Nothing OrElse Loop200.Count < 1 OrElse Loop200(0) Is Nothing Then
                strMSG = "Missing Order Details or invalid 200 Loop"
                'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                With dto214
                    .EDI214Message = strMSG + insertErrorMsg
                    .EDI214StatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI214ReceivedWithErrors
                    .EDI214FileName = fileName
                End With
                If Not oEDIData.InsertIntoEDI214(dto214, DateProcessed) Then insertErrorMsg = String.Format("Could not insert record into tblEDI214 for SHID: {0}, Date Processed: {1}, and File Name: {2}", dto214.SHID, DateProcessed.ToString(), fileName)
                Return False
            Else
                'process the data in the 200 loop.  Updates require this information.
                For Each L As clsEDI214Loop200 In Loop200
                    If L Is Nothing Then Exit For
                    Dim sLoadComments As String = ""
                    Dim sCarrierComments As String = "" 'Added By LVV on 6/19/19 - carrier comments alerts
                    'Add any free form text messages sent by the carrier each 200 loop starts a new load comments message
                    sLoadComments = (" " & L.K1.K101?.Trim & " " & L.K1.K102?.Trim)?.Trim
                    sCarrierComments = sLoadComments 'Added By LVV on 6/19/19 - carrier comments alerts
                    'Add the weight info if provided.
                    If L.AT8.AT803?.Trim.Length > 0 Then
                        sLoadComments &= " Weight: " & (L.AT8.AT803?.Trim & " " & L.AT8.AT802?.Trim & " " & L.AT8.AT801?.Trim)?.Trim
                    End If
                    'Add the pallet info if provided.
                    If L.AT8.AT804?.Trim.Length > 0 Then
                        sLoadComments &= " Pallets: " & L.AT8.AT804?.Trim
                    End If
                    'Add the quantity info if provided.
                    If L.AT8.AT805?.Trim.Length > 0 Then
                        sLoadComments &= " Quantity: " & L.AT8.AT805?.Trim
                    End If
                    'Add the volume info if provided.
                    If L.AT8.AT807?.Trim.Length > 0 Then
                        sLoadComments &= " Volume: " & (L.AT8.AT807?.Trim & " " & L.AT8.AT806?.Trim)?.Trim
                    End If
                    Dim lOrders As New List(Of cls214OrderData)
                    'Create a list of all the orders transmitted with this stop data.
                    For Each oL11 As clsEDIL11 In L.L11s
                        If oL11.L1102.ToUpper = "ON" Then
                            Dim o214OrderData As New cls214OrderData
                            'this is an order number so add it to the collection for processing
                            'but we must split off the order sequence number
                            o214OrderData.OrderReference = oL11.L1101?.Trim
                            o214OrderData.splitOrderReference()
                            lOrders.Add(o214OrderData)
                        End If
                    Next
                    'Modified by RHR for v-8.2.1 on 11/12/2019 
                    '  Added logic to make sure at least one order record exists.  
                    '  If Not we Then insert a blank one so the system can use a Default On LTL Orders.
                    If lOrders Is Nothing OrElse lOrders.Count() < 1 Then
                        lOrders = New List(Of cls214OrderData) From {New cls214OrderData()}
                    End If
                    If Not L.Loop205 Is Nothing AndAlso L.Loop205.Count() > 0 Then
                        'process each 205 loop
                        For Each SL As clsEDI214Loop205 In L.Loop205
                            Dim o214StopData As New cls214StopData
                            'Add any free form text messages sent by the carrier each 200 loop starts a new load comments message
                            o214StopData.LoadComments = sLoadComments
                            o214StopData.setCarrierComments(sCarrierComments) 'Added By LVV on 6/19/19 - carrier comments alerts
                            'Add the weight info if provided.
                            'o214StopData.EventCodes.Add(SL.AT7.AT701?.trim)
                            'o214StopData.dictEventComments.Add(SL.AT7.AT701?.trim, " SHIPMENT STATUS CODE: " & SL.AT7.AT701?.trim)
                            Dim dtTry As Date
                            If Date.TryParse(NDT.convertEDIDateToDateString(SL.AT7.AT705), dtTry) Then o214StopData.EventDate = dtTry Else o214StopData.EventDate = Nothing
                            Dim strTime = NDT.convertEDITimeToDateString(SL.AT7.AT706)
                            Dim strTimeDt As String = ""
                            If Not String.IsNullOrWhiteSpace(strTime) Then
                                If o214StopData.EventDate.HasValue Then
                                    strTimeDt = o214StopData.EventDate.Value.ToShortDateString + " " + strTime
                                Else
                                    strTimeDt = Date.Today.ToShortDateString + " " + strTime
                                End If

                                If Date.TryParse(strTimeDt, dtTry) Then o214StopData.EventTime = dtTry Else o214StopData.EventTime = Nothing

                            Else
                                o214StopData.EventTime = Nothing
                            End If

                            Dim oLoadStatus As New clsEDILoadStatusData()
                            With oLoadStatus
                                .DocTypeName = "214"
                                .EDIElementName = "AT701"
                                .EventDate = o214StopData.EventDate
                                .EventTime = o214StopData.EventTime
                                .EventCode = SL.AT7.AT701?.Trim()
                            End With
                            o214StopData.StatusDetails.Add(oLoadStatus)
                            'o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "AT701", o214StopData.EventDate, o214StopData.EventTime, SL.AT7.AT701?.trim, " SHIPMENT STATUS CODE: " & SL.AT7.AT701?.trim))

                            'Add the reason code info if provided.
                            If SL.AT7.AT702?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "AT702", o214StopData.EventDate, o214StopData.EventTime, SL.AT7.AT702?.Trim, ""))
                            'Add the appointment reason code info if provided.
                            If SL.AT7.AT704?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "AT704", o214StopData.EventDate, o214StopData.EventTime, SL.AT7.AT704?.Trim, ""))
                            'Add the type code info if provided.
                            If SL.AT7.AT703?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "AT703", o214StopData.EventDate, o214StopData.EventTime, SL.AT7.AT703?.Trim, ""))
                            'Add the time zone code info if provided.
                            If SL.AT7.AT707?.Trim.Length > 0 Then
                                o214StopData.EventComments &= " Time Zone: " & SL.AT7.AT707?.Trim
                                'Added By LVV on 9/17/19 for Bing Maps
                                o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "AT707", o214StopData.EventDate, o214StopData.EventTime, SL.AT7.AT707?.Trim, ""))
                            End If

                            'Added By LVV on 9/17/19 for Bing Maps
                            If SL.MS1.MS101?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "MS101", Nothing, Nothing, SL.MS1.MS101?.Trim, ""))
                            If SL.MS1.MS102?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "MS102", Nothing, Nothing, SL.MS1.MS102?.Trim, ""))
                            If SL.MS1.MS103?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "MS103", Nothing, Nothing, SL.MS1.MS103?.Trim, ""))
                            If SL.MS1.MS104?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "MS104", Nothing, Nothing, SL.MS1.MS104?.Trim, ""))
                            If SL.MS1.MS105?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "MS105", Nothing, Nothing, SL.MS1.MS105?.Trim, ""))
                            If SL.MS1.MS106?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "MS106", Nothing, Nothing, SL.MS1.MS106?.Trim, ""))
                            If SL.MS1.MS107?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "MS107", Nothing, Nothing, SL.MS1.MS107?.Trim, ""))
                            If SL.MS2.MS201?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "MS201", Nothing, Nothing, SL.MS2.MS201?.Trim, ""))
                            If SL.MS2.MS202?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "MS202", Nothing, Nothing, SL.MS2.MS202?.Trim, ""))
                            If SL.MS2.MS203?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "MS203", Nothing, Nothing, SL.MS2.MS203?.Trim, ""))
                            If SL.MS2.MS204?.Trim.Length > 0 Then o214StopData.StatusDetails.Add(New clsEDILoadStatusData("214", "MS204", Nothing, Nothing, SL.MS2.MS204?.Trim, ""))

                            'Add the City State and Country info
                            If (SL.MS1.MS101?.Trim & " " & SL.MS1.MS102?.Trim & " " & SL.MS1.MS103?.Trim)?.Trim.Length > 0 Then
                                o214StopData.EventComments &= " " & (SL.MS1.MS101?.Trim & " " & SL.MS1.MS102?.Trim & " " & SL.MS1.MS103?.Trim)?.Trim
                            End If
                            'Add any equipment codes
                            Dim strComma As String = ""
                            Dim strEq As String = " Eq: "
                            If SL.MS2.MS201?.Trim.Length > 0 Then
                                o214StopData.EventComments &= strEq & SL.MS2.MS201?.Trim
                                strComma = ","
                                strEq = " "
                            End If
                            If SL.MS2.MS202?.Trim.Length > 0 Then
                                o214StopData.EventComments &= strEq & strComma & SL.MS2.MS202?.Trim
                                strComma = ","
                                strEq = " "
                            End If
                            If SL.MS2.MS203?.Trim.Length > 0 Then
                                o214StopData.EventComments &= strEq & strComma & SL.MS2.MS203?.Trim
                                strComma = ","
                                strEq = " "
                            End If
                            If SL.MS2.MS204?.Trim.Length > 0 Then
                                o214StopData.EventComments &= strEq & strComma & SL.MS2.MS204?.Trim
                            End If
                            o214StopData.Orders = lOrders
                            'add the stop data to the load data
                            o214Data.Stops.Add(o214StopData)
                        Next
                    Else
                        Dim o214StopData As New cls214StopData
                        o214StopData.LoadComments = sLoadComments
                        o214StopData.setCarrierComments(sCarrierComments) 'Added By LVV on 6/19/19 - carrier comments alerts
                        o214StopData.Orders = lOrders
                        'add the stop data to the load data
                        o214Data.Stops.Add(o214StopData)
                    End If
                Next
            End If
            'process the 214 data new code added 1/12/2015 bypassing the stored procedure
            'validate that we have data
            If o214Data Is Nothing OrElse o214Data.Stops Is Nothing OrElse o214Data.Stops.Count < 1 Then
                'we do not have any stop data so we cannot continue.
                strMSG = "Missing Stop Details"
                'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                With dto214
                    .EDI214Message = strMSG + insertErrorMsg
                    .EDI214StatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI214ReceivedWithErrors
                    .EDI214FileName = fileName
                End With
                If Not oEDIData.InsertIntoEDI214(dto214, DateProcessed) Then insertErrorMsg = String.Format("Could not insert record into tblEDI214 for SHID: {0}, Date Processed: {1}, and File Name: {2}", dto214.SHID, DateProcessed.ToString(), fileName)
                Return False
            End If
            'get the book data
            'If oBookDAL Is Nothing Then
            '    oBookDAL = New DAL.NGLBookData(Me.WCFParameters)
            'End If
            Dim oBooks = oBookDAL.GetBooksBySHID(o214Data.SHID)
            If oBooks Is Nothing OrElse oBooks.Count() < 1 Then
                'we do not have any stop data so we cannot continue.
                strMSG = "Missing or Invalid SHID Number " & o214Data.SHID
                'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                With dto214
                    .EDI214Message = strMSG + insertErrorMsg
                    .EDI214StatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI214ReceivedWithErrors
                    .EDI214FileName = fileName
                End With
                If Not oEDIData.InsertIntoEDI214(dto214, DateProcessed) Then insertErrorMsg = String.Format("Could not insert record into tblEDI214 for SHID: {0}, Date Processed: {1}, and File Name: {2}", dto214.SHID, DateProcessed.ToString(), fileName)
                Return False
            End If
            'Get the Load Status Tracking Cross Reference Data
            For Each oStop In o214Data.Stops
                If Not oStop.StatusDetails Is Nothing AndAlso oStop.StatusDetails.Count() > 0 Then
                    For Each oDetail In oStop.StatusDetails
                        'get the DAL data
                        If Not String.IsNullOrWhiteSpace(oDetail.EventCode) AndAlso oDetail.EventCode.ToUpper() <> "NS" AndAlso oDetail.EventCode.ToUpper() <> "NA" Then
                            Dim oDALCodes = oStatusCodeDAL.GetEDIStatusCodesByDocAndElement(oDetail.DocTypeName, oDetail.EDIElementName, oDetail.EventCode)
                            If Not oDALCodes Is Nothing AndAlso oDALCodes.Count > 0 Then
                                'use the configured Code Setting 
                                oDetail.EDIStatusCodes = oDALCodes(0) 'get the first one in case we have duplicates
                                oDetail.EDIDocTypeControl = oDetail.EDIStatusCodes.EDISEDITControl
                                oDetail.EDIElementControl = oDetail.EDIStatusCodes.EDISEDIEControl
                            Else
                                'use the default code settings
                                Dim sNotSupportedList As New List(Of String) From {"AT701", "AT702", "AT703", "AT704"}
                                If sNotSupportedList.Contains(oDetail.EDIElementName) Then
                                    oStop.EventComments &= " Status Code, " & oDetail.EventCode & ", is Not supported"
                                End If
                            End If
                        End If
                    Next
                End If
            Next
            'TODO: Add code to validate the carrier and company partner codes.

            '************************** OLD NOTES **************************************************************************
            'modify the read 214 data above to use the new clsEDILoadStatusData instead of the dictEventComments object
            '      to store the event codes. then for clsEDILoadStatusData assigned to each stop look up the EDI Status Codes 
            '      using the NGLCarrierDataProvider.NGLEDIStatusCodeData.GetEDIStatusCodesByDocAndElement.
            '      Then use the first record returned from each clsEDILoadStatusData (or use default values if no records) to
            '      call the software factory data and execute the configured process for each order on the stop. Using the 
            '      EDIStatusCodes.EDISLoadStatusControl to update the BookTrackTable as needed {after we save changes to the 
            '      book table as defined in the software factory logic.
            '******************************************************************************************************************
            If EDI214ActionFactory(oBooks, o214Data) Then
                For Each b In oBooks
                    oBookDAL.UpdateRecordNoReturn(b)
                Next
                Dim lBookControls As New List(Of Integer)
                For Each s In o214Data.Stops
                    For Each order In s.Orders
                        If Not lBookControls.Contains(order.BookControl) Then
                            lBookControls.Add(order.BookControl)
                            For Each t In order.Tracks
                                'oBookTrackDAL.CreateRecord(t)
                                oBookTrackDAL.InsertBookTrackWithDetails(t)
                            Next
                            If order.ApptSchedulingUpdateRequired Then
                                oBookBLL.UpdateAssignedCarrier(order.BookControl, o214Data.ShipCarrierNumber, o214Data.ShipCarrierName)
                            End If
                            'Added By LVV on 6/19/19 - carrier comments alerts
                            Dim subject = "Carrier Comments Added To " + order.OrderNumber
                            Dim body = "<h4>" + subject + "</h4><br/>" + "Comments" + "<br/>" + s.getCarrierComments() + "<br/>" + "</p>"
                            otblAlertMessageDAL.InsertAlertMessage("AlertCarrierLoadStatusComment", "Alert When carrier enters comments In NEXTrack Or via EDI", subject, body)
                        End If
                    Next
                Next
                blnRet = True
            End If
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' returns true if at least one status update is created else returns false
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EDI214ActionFactory(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData) As Boolean
        Dim blnRet As Boolean = False
        If o214Data.Stops Is Nothing OrElse o214Data.Stops.Count() < 1 Then Return False
        For Each oStop In o214Data.Stops
            If oStop.StatusDetails Is Nothing OrElse oStop.StatusDetails.Count < 1 Then
                'execute default processing method
                If DefaultLoadStatusUpdateMethod(oBooks, o214Data, oStop) Then blnRet = True
            Else
                Dim lFactoryDetails As New List(Of clsEDI214FactoryDetail)
                'each stop can have upto 4 status details one each AT701, AT702, AT703, or AT704
                'AT702 is dependent upon AT701 so it is only processed if an AT701 is present
                'AT704 is dependent upon AT703 so it is only processed if an AT703 is present
                'Factory codes are not currently linked to AT702 or AT704 codes
                'check if we have any AT701 details
                If oStop.StatusDetails.Any(Function(x) x.EDIElementName = "AT701") Then
                    'get the detail
                    Dim oDetail = oStop.StatusDetails.Where(Function(x) x.EDIElementName = "AT701").FirstOrDefault()
                    'check if we have a factory name
                    If Not oDetail Is Nothing AndAlso Not oDetail.EDIStatusCodes Is Nothing Then
                        Dim oFactoryDetail As New clsEDI214FactoryDetail
                        If oDetail.EDIStatusCodes.EDISEDIAControl <> 0 Then
                            oFactoryDetail.StatusDetail = oDetail
                            oFactoryDetail.FactoryName = oStatusCodeDAL.GetEDIStatusActionFactoryName(oDetail.EDIStatusCodes.EDISEDIAControl)
                            oFactoryDetail.StatusMessage = oDetail.EDIStatusCodes.EDISDescription?.Trim()
                            oFactoryDetail.StatusLoadStatusControl = oDetail.EDIStatusCodes.EDISLoadStatusControl
                        End If
                        'check if we have any AT702 details that do not have an EventCode of NS - Normal 
                        If oStop.StatusDetails.Any(Function(x) x.EDIElementName = "AT702" And x.EventCode <> "NS") Then
                            'get the Reason Code
                            Dim oReason = oStop.StatusDetails.Where(Function(x) x.EDIElementName = "AT702" And x.EventCode <> "NS").FirstOrDefault()
                            If Not oReason Is Nothing AndAlso Not oReason.EDIStatusCodes Is Nothing Then
                                If oReason.EDIStatusCodes.EDISEDIAControl <> 0 Then
                                    oFactoryDetail.ReasonDetail = oReason
                                    oFactoryDetail.ReasonMessage = oReason.EDIStatusCodes.EDISDescription?.Trim()
                                    oFactoryDetail.ReasonLoadStatusControl = oReason.EDIStatusCodes.EDISLoadStatusControl
                                End If
                            End If
                        End If
                        lFactoryDetails.Add(oFactoryDetail)
                    End If
                End If
                'check if we have any AT703 details
                If oStop.StatusDetails.Any(Function(x) x.EDIElementName = "AT703") Then
                    'get the detail
                    Dim oDetail = oStop.StatusDetails.Where(Function(x) x.EDIElementName = "AT703").FirstOrDefault()
                    'check if we have a factory name
                    If Not oDetail Is Nothing AndAlso Not oDetail.EDIStatusCodes Is Nothing Then
                        Dim oFactoryDetail As New clsEDI214FactoryDetail
                        If oDetail.EDIStatusCodes.EDISEDIAControl <> 0 Then
                            oFactoryDetail.StatusDetail = oDetail
                            oFactoryDetail.FactoryName = oStatusCodeDAL.GetEDIStatusActionFactoryName(oDetail.EDIStatusCodes.EDISEDIAControl)
                            oFactoryDetail.StatusMessage = oDetail.EDIStatusCodes.EDISDescription?.Trim()
                            oFactoryDetail.StatusLoadStatusControl = oDetail.EDIStatusCodes.EDISLoadStatusControl
                        End If
                        'check if we have any AT704 details that do not have an EventCode of NA - Normal 
                        If oStop.StatusDetails.Any(Function(x) x.EDIElementName = "AT704" And x.EventCode <> "NA") Then
                            'get the Reason Code
                            Dim oReason = oStop.StatusDetails.Where(Function(x) x.EDIElementName = "AT704" And x.EventCode <> "NA").FirstOrDefault()
                            If Not oReason Is Nothing AndAlso Not oReason.EDIStatusCodes Is Nothing Then
                                If oReason.EDIStatusCodes.EDISEDIAControl <> 0 Then
                                    oFactoryDetail.ReasonDetail = oReason
                                    oFactoryDetail.ReasonMessage = oReason.EDIStatusCodes.EDISDescription?.Trim()
                                    oFactoryDetail.ReasonLoadStatusControl = oReason.EDIStatusCodes.EDISLoadStatusControl
                                End If
                            End If
                        End If
                        lFactoryDetails.Add(oFactoryDetail)
                    End If
                End If

                'Added By LVV on 9/19/19 for Bing Maps
                'If the MS202 is not blank AND if the trailer number of any book record != MS202 THEN set the trailer number of all book records to MS202
                'Eventually we can add code so that the factory can take care of this save but for now this works because we don't have time
                If oStop.StatusDetails.Any(Function(x) x.EDIElementName = "MS202") Then
                    Dim trailerNo = oStop.StatusDetails.Where(Function(x) x.EDIElementName = "MS202").Select(Function(y) y.EventCode).FirstOrDefault()
                    If oBooks.Any(Function(x) Not x.BookCarrTrailerNo = trailerNo) Then
                        For Each b In oBooks
                            b.BookCarrTrailerNo = trailerNo
                        Next
                    End If
                End If

                If Not lFactoryDetails Is Nothing AndAlso lFactoryDetails.Count() > 0 Then
                    For Each oDetail In lFactoryDetails
                        Select Case oDetail.FactoryName
                            Case "CarrierDepartsPickup"
                                If CarrierDepartsPickup(oBooks, o214Data, oStop, oDetail) Then blnRet = True
                            Case "CarrierDepartsDelivery"
                                If CarrierDepartsDelivery(oBooks, o214Data, oStop, oDetail) Then blnRet = True
                            Case "CarrierArrivedAtPickup"
                                If CarrierArrivedAtPickup(oBooks, o214Data, oStop, oDetail) Then blnRet = True
                            Case "CarrierArrivedAtDelivery"
                                If CarrierArrivedAtDelivery(oBooks, o214Data, oStop, oDetail) Then blnRet = True
                            Case "CarrierAppointmentSetAtPickup"
                                If CarrierAppointmentSetAtPickup(oBooks, o214Data, oStop, oDetail) Then blnRet = True
                            Case "CarrierAppointmentSetAtDelivery"
                                If CarrierAppointmentSetAtDelivery(oBooks, o214Data, oStop, oDetail) Then blnRet = True
                            Case "CarrierCompleteUnloadAtDelivery"
                                If CarrierCompleteUnloadAtDelivery(oBooks, o214Data, oStop, oDetail) Then blnRet = True
                            Case "CarrierStartUnloadAtDelivery"
                                If CarrierStartUnloadAtDelivery(oBooks, o214Data, oStop, oDetail) Then blnRet = True
                            Case "CarrierCompleteLoadAtPickup"
                                If CarrierCompleteLoadAtPickup(oBooks, o214Data, oStop, oDetail) Then blnRet = True
                            Case "CarrierStartLoadAtPickup"
                                If CarrierStartLoadAtPickup(oBooks, o214Data, oStop, oDetail) Then blnRet = True
                            Case "CarrierShipmentDelayed"
                                If CarrierShipmentDelayed(oBooks, o214Data, oStop, oDetail) Then blnRet = True
                            Case Else
                                'execute default processing method
                                If DefaultLoadStatusUpdateMethod(oBooks, o214Data, oStop, oDetail) Then blnRet = True
                        End Select
                    Next
                Else
                    If DefaultLoadStatusUpdateMethod(oBooks, o214Data, oStop) Then blnRet = True
                End If
            End If
        Next
        Return blnRet
    End Function

    
#Disable Warning BC42301 ' Only one XML comment block is allowed per language element.
''' <summary>
    ''' adds up to four load status messages to the order.Tracks collection based on load comments and 255 character size limitation
    ''' populates the strErrMessage if a problem is encountered and returns false
    ''' combine the strEventDescription and the strEventDateTime together truncating strEventDescription so date and time always fit in 255 character field
    ''' Caller must save the order.Tracks collection to the database
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="strLoadComments">
    ''' Constructed from 214 segments including K1 (comments) AT8 (metrics)
    ''' </param>
    ''' <param name="strBookTrackComments">
    ''' Constructed using 214 segments which include AT707, MS101, MS102, MS103, MS201, MS202, MS203, and MS204 data  
    ''' </param>
    ''' <param name="strContact"></param>
    ''' <param name="intDefaultLoadStatusControl"></param>
    ''' <param name="dtTrackDate"></param>
    ''' <param name="strErrMessage"></param>
    ''' <param name="strEventComment">
    ''' Constructed using 214 segments AT701, AT702, AT703, or AT704 and hard coded status code messages like SHIPMENT STATUS OR APPOINTMENT REASON CODE:
    ''' Any special messages or warnings should be concatenated to the beginning of this text so it will not get truncated
    ''' </param>
    ''' <param name="strEventDateTime"></param>
    ''' <param name="strEventDescription">
    ''' Read from the database using the Load Status Xref data and reason codes
    ''' </param>
    ''' <param name="intEventStatusControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' We have up to 4 load status messages each limited to 255 characters
    ''' 1 = strEventDescription from Load Status xref table
    ''' 2 = strBookTrackComments
    ''' 3 = strLoadComments and strEventComments
    ''' 4 = strEventComments Overflow if  strLoadComments and strEventComments is too long
    ''' strEventComments will be truncated if it is too long
    ''' BookTrackComments will try to incorporate strLoadComments and/or strEventComments when possible
    ''' but each value will not be split only truncated.
    ''' </remarks>
    ''Public Function addBookTracksToOrder(ByRef order As cls214OrderData,
    ''                                     ByVal strLoadComments As String,
    ''                                     ByVal strBookTrackComments As String,
    ''                                     ByVal strContact As String,
    ''                                     ByVal intDefaultLoadStatusControl As Integer,
    ''                                     ByVal dtTrackDate As Date,
    ''                                     ByRef strErrMessage As String,
    ''                                     Optional strEventComment As String = "",
    ''                                     Optional strEventDateTime As String = "",
    ''                                     Optional strEventDescription As String = "",
    ''                                     Optional ByVal intEventStatusControl As Integer = 0) As Boolean
    ''    Dim blnRet As Boolean = True
    ''    If order Is Nothing OrElse order.BookControl = 0 Then
    ''        strErrMessage = "Invalid Order Number Reference"
    ''        Return False
    ''    End If
    ''    If intDefaultLoadStatusControl = 0 Then
    ''        strErrMessage = "Invalid Default Load Status Code, 214 Code does Not exist"
    ''        Return False
    ''    End If
    ''    'try to combine strBookTrackComments and strLoadComments
    ''    If Not String.IsNullOrWhiteSpace(strLoadComments) AndAlso strLoadComments?.Trim().Length + strBookTrackComments?.Trim().Length < 255 Then
    ''        strBookTrackComments = strLoadComments?.Trim() & " " & strBookTrackComments?.Trim()
    ''        strLoadComments = ""
    ''    End If
    ''    'try to combine  strBookTrackComments and strEventComments
    ''    If Not String.IsNullOrWhiteSpace(strEventComment) AndAlso strEventComment?.Trim().Length + strBookTrackComments?.Trim().Length < 255 Then
    ''        strBookTrackComments = strBookTrackComments?.Trim() & " " & strEventComment?.Trim()
    ''        strEventComment = ""
    ''    End If
    ''    'try to combine strLoadComments with strEventComments
    ''    If Not String.IsNullOrWhiteSpace(strLoadComments) OrElse Not String.IsNullOrWhiteSpace(strEventComment) AndAlso strEventComment?.Trim().Length + strLoadComments?.Trim().Length < 255 Then
    ''        'add the event comments to the Load Comments
    ''        strLoadComments &= " " & strEventComment?.Trim()
    ''    End If
    ''    'add any status codes that are mapped.
    ''    If Not String.IsNullOrWhiteSpace(strEventDescription) Then
    ''        If Not String.IsNullOrWhiteSpace(strEventDateTime) Then
    ''            'combine the EventDescription and the EventDateTime together
    ''            strEventDescription = Left(strEventDescription?.Trim(), 254 - strEventDateTime.Length) & " " & strEventDateTime
    ''        End If
    ''        'try to combine strBookTrackComments and strEventDescription
    ''        If Not String.IsNullOrWhiteSpace(strBookTrackComments) AndAlso strBookTrackComments?.Trim().Length + strEventDescription?.Trim().Length < 255 Then
    ''            strEventDescription &= " " & strBookTrackComments
    ''            strBookTrackComments = ""
    ''        End If
    ''        If intEventStatusControl = 0 Then intEventStatusControl = intDefaultLoadStatusControl
    ''        order.Tracks.Add(New DTO.BookTrack() With {.BookTrackBookControl = order.BookControl, .BookTrackComment = Left(strEventDescription?.Trim(), 255), .BookTrackContact = strContact, .BookTrackDate = dtTrackDate, .BookTrackStatus = intEventStatusControl})
    ''    End If
    ''    'add the strBookTrackComments
    ''    If Not String.IsNullOrWhiteSpace(strBookTrackComments) Then
    ''        order.Tracks.Add(New DTO.BookTrack() With {.BookTrackBookControl = order.BookControl, .BookTrackComment = Left(strBookTrackComments?.Trim(), 255), .BookTrackContact = strContact, .BookTrackDate = dtTrackDate, .BookTrackStatus = intDefaultLoadStatusControl})
    ''    End If
    ''    'add any additional strLoadComments
    ''    If Not String.IsNullOrWhiteSpace(strLoadComments) Then
    ''        order.Tracks.Add(New DTO.BookTrack() With {.BookTrackBookControl = order.BookControl, .BookTrackComment = Left(strLoadComments?.Trim(), 255), .BookTrackContact = strContact, .BookTrackDate = dtTrackDate, .BookTrackStatus = intDefaultLoadStatusControl})
    ''    End If
    ''    'add any left over strEventComment
    ''    If Not String.IsNullOrWhiteSpace(strEventComment) Then
    ''        order.Tracks.Add(New DTO.BookTrack() With {.BookTrackBookControl = order.BookControl, .BookTrackComment = Left(strEventComment?.Trim(), 255), .BookTrackContact = strContact, .BookTrackDate = dtTrackDate, .BookTrackStatus = intDefaultLoadStatusControl})
    ''    End If
    ''    Return blnRet
    ''End Function


    ''' <summary>
    ''' Creates the BookTrackDetail object from the StatusDetails
    ''' </summary>
    ''' <param name="StatusDetails"></param>
    ''' <param name="blnLinkStopRecord"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created By LVV on 9/17/19 for Bing Maps
    ''' </remarks>
    Public Function getBookTrackDetail(ByVal StatusDetails As List(Of clsEDILoadStatusData), ByVal blnLinkStopRecord As Boolean) As List(Of DTO.BookTrackDetail)
#Enable Warning BC42301 ' Only one XML comment block is allowed per language element.
        Dim details As New List(Of DTO.BookTrackDetail)
        If StatusDetails?.Count > 0 Then
            Dim det As New DTO.BookTrackDetail
            det.setLinkStopRecord(blnLinkStopRecord)
            If StatusDetails.Any(Function(x) x.EDIElementName = "AT701") Then det.BookTrackDetailAT701 = StatusDetails.Where(Function(x) x.EDIElementName = "AT701").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "AT702") Then det.BookTrackDetailAT702 = StatusDetails.Where(Function(x) x.EDIElementName = "AT702").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "AT703") Then det.BookTrackDetailAT703 = StatusDetails.Where(Function(x) x.EDIElementName = "AT703").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "AT704") Then det.BookTrackDetailAT704 = StatusDetails.Where(Function(x) x.EDIElementName = "AT704").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "AT707") Then det.BookTrackDetailAT707 = StatusDetails.Where(Function(x) x.EDIElementName = "AT707").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName.Substring(0, 3) = "AT7" And x.EventDate.HasValue) Then
                Dim dtAT705 = StatusDetails.Where(Function(x) x.EDIElementName.Substring(0, 3) = "AT7" And x.EventDate.HasValue).Select(Function(y) y.EventDate).FirstOrDefault()
                det.BookTrackDetailAT705 = dtAT705.Value.ToShortDateString()
            End If
            If StatusDetails.Any(Function(x) x.EDIElementName.Substring(0, 3) = "AT7" And x.EventTime.HasValue) Then
                Dim dtAT706 = StatusDetails.Where(Function(x) x.EDIElementName.Substring(0, 3) = "AT7" And x.EventTime.HasValue).Select(Function(y) y.EventTime).FirstOrDefault()
                det.BookTrackDetailAT706 = dtAT706.Value.ToString("HH:mm")
            End If
            If StatusDetails.Any(Function(x) x.EDIElementName = "MS101" OrElse x.EDIElementName = "MS102" OrElse x.EDIElementName = "MS104" OrElse x.EDIElementName = "MS105") Then det.BookTrackDetailMS1StatusUpdate = True Else det.BookTrackDetailMS1StatusUpdate = False
            If StatusDetails.Any(Function(x) x.EDIElementName = "MS101") Then det.BookTrackDetailMS101 = StatusDetails.Where(Function(x) x.EDIElementName = "MS101").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "MS102") Then det.BookTrackDetailMS102 = StatusDetails.Where(Function(x) x.EDIElementName = "MS102").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "MS103") Then det.BookTrackDetailMS103 = StatusDetails.Where(Function(x) x.EDIElementName = "MS103").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "MS104") Then det.BookTrackDetailMS104 = StatusDetails.Where(Function(x) x.EDIElementName = "MS104").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "MS105") Then det.BookTrackDetailMS105 = StatusDetails.Where(Function(x) x.EDIElementName = "MS105").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "MS106") Then det.BookTrackDetailMS106 = StatusDetails.Where(Function(x) x.EDIElementName = "MS106").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "MS107") Then det.BookTrackDetailMS107 = StatusDetails.Where(Function(x) x.EDIElementName = "MS107").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "MS201") Then det.BookTrackDetailMS201 = StatusDetails.Where(Function(x) x.EDIElementName = "MS201").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "MS202") Then det.BookTrackDetailMS202 = StatusDetails.Where(Function(x) x.EDIElementName = "MS202").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "MS203") Then det.BookTrackDetailMS203 = StatusDetails.Where(Function(x) x.EDIElementName = "MS203").Select(Function(y) y.EventCode).FirstOrDefault()
            If StatusDetails.Any(Function(x) x.EDIElementName = "MS204") Then det.BookTrackDetailMS204 = StatusDetails.Where(Function(x) x.EDIElementName = "MS204").Select(Function(y) y.EventCode).FirstOrDefault()
            details.Add(det)
        End If
        Return details
    End Function

    ''' <summary>
    ''' Adds up to four load status messages to the order.
    ''' Tracks collection based on load comments and 255 character size limitation populates the strErrMessage if a problem is encountered and returns false
    ''' Combine the strEventDescription and the strEventDateTime together truncating strEventDescription so date and time always fit in 255 character field
    ''' Caller must save the order.Tracks collection to the database
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="strLoadComments">Constructed from 214 segments including K1 (comments) AT8 (metrics)</param>
    ''' <param name="strBookTrackComments">Constructed using 214 segments which include AT707, MS101, MS102, MS103, MS201, MS202, MS203, and MS204 data</param>
    ''' <param name="strContact"></param>
    ''' <param name="intDefaultLoadStatusControl"></param>
    ''' <param name="dtTrackDate"></param>
    ''' <param name="strErrMessage"></param>
    ''' <param name="strEventComment">
    ''' Constructed using 214 segments AT701, AT702, AT703, or AT704 and hard coded status code messages like SHIPMENT STATUS OR APPOINTMENT REASON CODE:
    ''' Any special messages or warnings should be concatenated to the beginning of this text so it will not get truncated
    ''' </param>
    ''' <param name="strEventDateTime"></param>
    ''' <param name="strEventDescription">Read from the database using the Load Status Xref data and reason codes</param>
    ''' <param name="intEventStatusControl"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' We have up to 4 load status messages each limited to 255 characters
    '''   1 = strEventDescription from Load Status xref table
    '''   2 = strBookTrackComments
    '''   3 = strLoadComments and strEventComments
    '''   4 = strEventComments Overflow if  strLoadComments and strEventComments is too long
    ''' strEventComments will be truncated if it is too long
    ''' BookTrackComments will try to incorporate strLoadComments and/or strEventComments when possible
    ''' but each value will not be split only truncated.
    ''' Modified By LVV on 9/17/19 for Bing Maps
    '''  Added logic for BookTrackDetails
    ''' </remarks>
    Public Function addBookTracksToOrder(ByRef order As cls214OrderData,
                                         ByVal strLoadComments As String,
                                         ByVal strBookTrackComments As String,
                                         ByVal strContact As String,
                                         ByVal intDefaultLoadStatusControl As Integer,
                                         ByVal dtTrackDate As Date,
                                         ByRef strErrMessage As String,
                                         ByVal StatusDetails As List(Of clsEDILoadStatusData),
                                         Optional strEventComment As String = "",
                                         Optional strEventDateTime As String = "",
                                         Optional strEventDescription As String = "",
                                         Optional ByVal intEventStatusControl As Integer = 0,
                                         Optional ByVal blnLinkStopRecord As Boolean = True) As Boolean
        Dim blnRet As Boolean = True
        If order Is Nothing OrElse order.BookControl = 0 Then
            strErrMessage = "Invalid Order Number Reference"
            Return False
        End If
        If intDefaultLoadStatusControl = 0 Then
            strErrMessage = "Invalid Default Load Status Code, 214 Code does not exist"
            Return False
        End If

        Dim dets = getBookTrackDetail(StatusDetails, blnLinkStopRecord)

        'try to combine strBookTrackComments and strLoadComments
        If Not String.IsNullOrWhiteSpace(strLoadComments) AndAlso strLoadComments?.Trim().Length + strBookTrackComments?.Trim().Length < 255 Then
            strBookTrackComments = strLoadComments?.Trim() & " " & strBookTrackComments?.Trim()
            strLoadComments = ""
        End If
        'try to combine  strBookTrackComments and strEventComments
        If Not String.IsNullOrWhiteSpace(strEventComment) AndAlso strEventComment?.Trim().Length + strBookTrackComments?.Trim().Length < 255 Then
            strBookTrackComments = strBookTrackComments?.Trim() & " " & strEventComment?.Trim()
            strEventComment = ""
        End If
        'try to combine strLoadComments with strEventComments
        If Not String.IsNullOrWhiteSpace(strLoadComments) OrElse Not String.IsNullOrWhiteSpace(strEventComment) AndAlso strEventComment?.Trim().Length + strLoadComments?.Trim().Length < 255 Then
            'add the event comments to the Load Comments
            strLoadComments &= " " & strEventComment?.Trim()
        End If
        'add any status codes that are mapped.
        If Not String.IsNullOrWhiteSpace(strEventDescription) Then
            If Not String.IsNullOrWhiteSpace(strEventDateTime) Then
                'combine the EventDescription and the EventDateTime together
                strEventDescription = Left(strEventDescription?.Trim(), 254 - strEventDateTime.Length) & " " & strEventDateTime
            End If
            'try to combine strBookTrackComments and strEventDescription
            If Not String.IsNullOrWhiteSpace(strBookTrackComments) AndAlso strBookTrackComments?.Trim().Length + strEventDescription?.Trim().Length < 255 Then
                strEventDescription &= " " & strBookTrackComments
                strBookTrackComments = ""
            End If
            If intEventStatusControl = 0 Then intEventStatusControl = intDefaultLoadStatusControl
            Dim bt = New DTO.BookTrack() With {.BookTrackBookControl = order.BookControl, .BookTrackComment = Left(strEventDescription?.Trim(), 255), .BookTrackContact = strContact, .BookTrackDate = dtTrackDate, .BookTrackStatus = intEventStatusControl}
            bt.setDetails(dets)
            order.Tracks.Add(bt)
        End If
        'add the strBookTrackComments
        If Not String.IsNullOrWhiteSpace(strBookTrackComments) Then
            Dim bt = New DTO.BookTrack() With {.BookTrackBookControl = order.BookControl, .BookTrackComment = Left(strBookTrackComments?.Trim(), 255), .BookTrackContact = strContact, .BookTrackDate = dtTrackDate, .BookTrackStatus = intDefaultLoadStatusControl}
            bt.setDetails(dets)
            order.Tracks.Add(bt)
        End If
        'add any additional strLoadComments
        If Not String.IsNullOrWhiteSpace(strLoadComments) Then
            Dim bt = New DTO.BookTrack() With {.BookTrackBookControl = order.BookControl, .BookTrackComment = Left(strLoadComments?.Trim(), 255), .BookTrackContact = strContact, .BookTrackDate = dtTrackDate, .BookTrackStatus = intDefaultLoadStatusControl}
            bt.setDetails(dets)
            order.Tracks.Add(bt)
        End If
        'add any left over strEventComment
        If Not String.IsNullOrWhiteSpace(strEventComment) Then
            Dim bt = New DTO.BookTrack() With {.BookTrackBookControl = order.BookControl, .BookTrackComment = Left(strEventComment?.Trim(), 255), .BookTrackContact = strContact, .BookTrackDate = dtTrackDate, .BookTrackStatus = intDefaultLoadStatusControl}
            bt.setDetails(dets)
            order.Tracks.Add(bt)
        End If
        Return blnRet
    End Function




    ''' <summary>
    ''' Determines if a change has been made to the actual Shipping carrier information
    ''' Caller must update the ams appointment data if the order.ApptSchedulingUpdateRequired is set to true
    ''' Caller must save the DTO.Book data to the database when finished.
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="oBook"></param>
    ''' <param name="o214Data"></param>
    ''' <remarks>
    ''' Sets the ApptSchedulingUpdateRequired flag to true if needed
    ''' updates the oBook ship carrier fields as needed
    ''' generates a AlertInvalid214CarrierProNumberAssignment if an Carrier Pro Number already exists 
    ''' </remarks>
    Public Sub processShipCarrierData(ByRef order As cls214OrderData, ByRef oBook As DTO.Book, ByRef o214Data As cls214LoadData)
        If (Not String.IsNullOrWhiteSpace(o214Data.ShipCarrierNumber) AndAlso o214Data.ShipCarrierNumber <> oBook.BookShipCarrierNumber) Then
            order.CarrierChanged = True
            oBook.BookShipCarrierNumber = o214Data.ShipCarrierNumber
            If (Not String.IsNullOrWhiteSpace(o214Data.ShipCarrierName) AndAlso o214Data.ShipCarrierName <> oBook.BookShipCarrierName) Then
                'both ShipCarrierName and ShipCarrierNumber are required to update the appointment data using
                'the assigned carrier information; executed later by using the spUpdateAssignedCarrier for each order.
                '? we may want to create a copy of the spUpdateAssignedCarrier that works different because we really only
                'need to run the update once for each SHID?
                order.ApptSchedulingUpdateRequired = True
                oBook.BookShipCarrierName = o214Data.ShipCarrierName
            End If
        End If
        If Not String.IsNullOrWhiteSpace(o214Data.ShipCarrierProNumber) Then
            'we need to test both the BookShipCarrierProNumber and BookShipCarrierProNumberRaw Raw first.
            If o214Data.ShipCarrierProNumber?.Trim() <> oBook.BookShipCarrierProNumberRaw Then
                If o214Data.ShipCarrierProNumber?.Trim() <> oBook.BookShipCarrierProNumber Then
                    'we do not have a match so update the CarrierProNumber if possible
                    If oBook.BookShipCarrierProControl <> 0 Then
                        'raise alert that an invalid carrier pro number is being provided.
                        createInvalid214CarrierProNumberAssignmentAlert(oBook.BookCustCompControl, oBook.BookCarrierControl, oBook.BookProNumber, oBook.BookSHID, oBook.BookCarrOrderNumber, oBook.BookOrderSequence, oBook.BookConsPrefix, o214Data.ShipCarrierProNumber)
                    Else
                        oBook.BookShipCarrierProNumber = o214Data.ShipCarrierProNumber?.Trim()
                        oBook.BookShipCarrierProNumberRaw = o214Data.ShipCarrierProNumber?.Trim()
                    End If
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' populates strEventDateTime with EventDate and EventTime as formatted string when available
    ''' populates the strEventDescription with oDetail.EDIStatusCodes.EDISDescription when available
    ''' populates the intEventStatusControl with oDetail.EDIStatusCodes.EDISLoadStatusControl when available
    ''' </summary>
    ''' <param name="oDetail"></param>
    ''' <param name="EventDate"></param>
    ''' <param name="EventTime"></param>
    ''' <param name="strEventComment"></param>
    ''' <param name="strEventDateTime"></param>
    ''' <param name="strEventDescription"></param>
    ''' <param name="intEventStatusControl"></param>
    ''' <remarks></remarks>
    Public Sub getLoadStatusDetailComments(ByRef oDetail As clsEDI214FactoryDetail,
                                           ByVal EventDate As Date?,
                                           ByVal EventTime As Date?,
                                           ByRef strEventComment As String,
                                           ByRef strEventDateTime As String,
                                           ByRef strEventDescription As String,
                                           ByRef intEventStatusControl As Integer)
        'check for Event Code Detail Mapping.
        If Not oDetail.StatusDetail Is Nothing Then
            strEventComment = oDetail.StatusDetail.EventComment
            If Not EventDate Is Nothing Then
                strEventDateTime = EventDate.Value.ToShortDateString
                If Not EventTime Is Nothing Then
                    strEventDateTime &= " " & EventTime.Value.ToShortTimeString
                End If
            End If
            strEventDescription = oDetail.StatusMessage & " " & oDetail.ReasonMessage
            intEventStatusControl = oDetail.StatusLoadStatusControl
        End If
    End Sub


    ''' <summary>
    ''' populates strEventDateTime with EventDate and EventTime as formatted string when available
    ''' populates the strEventDescription with oDetail.EDIStatusCodes.EDISDescription when available
    ''' populates the intEventStatusControl with oDetail.EDIStatusCodes.EDISLoadStatusControl when available
    ''' </summary>
    ''' <param name="oDetail"></param>
    ''' <param name="EventDate"></param>
    ''' <param name="EventTime"></param>
    ''' <param name="strEventComment"></param>
    ''' <param name="strEventDateTime"></param>
    ''' <param name="strEventDescription"></param>
    ''' <param name="intEventStatusControl"></param>
    ''' <remarks></remarks>
    Public Sub getLoadStatusDetailComments(ByRef oDetail As clsEDILoadStatusData,
                                           ByVal EventDate As Date?,
                                           ByVal EventTime As Date?,
                                           ByRef strEventComment As String,
                                           ByRef strEventDateTime As String,
                                           ByRef strEventDescription As String,
                                           ByRef intEventStatusControl As Integer)
        'check for Event Code Detail Mapping.
        If Not oDetail Is Nothing Then
            strEventComment = oDetail.EventComment
            If Not EventDate Is Nothing Then
                strEventDateTime = EventDate.Value.ToShortDateString
                If Not EventTime Is Nothing Then
                    strEventDateTime &= " " & EventTime.Value.ToShortTimeString
                End If
            End If

            If Not oDetail.EDIStatusCodes Is Nothing Then
                strEventDescription = oDetail.EDIStatusCodes.EDISDescription?.Trim()
                intEventStatusControl = oDetail.EDIStatusCodes.EDISLoadStatusControl
            End If
        End If

    End Sub

    '
    Public Function DefaultLoadStatusUpdateMethod(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, ByRef oDetail As clsEDI214FactoryDetail) As Boolean
        Return DefaultLoadStatusUpdateMethod(oBooks, o214Data, o214StopData, oDetail.StatusDetail)
    End Function

    ''' <summary>
    ''' DefaultLoadStatusUpdateMethod()
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <param name="o214StopData"></param>
    ''' <param name="oDetail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 9/4/2019
    '''  NEW RULE
    '''  If the order number is missing/wrong we use the SHID to get the correct order number.
    '''  We only do this if there is 1 order number on the SHID. If the SHID is wrong then the entire process won't work. 
    '''  If there is more than 1 order on the SHID then continue as normal - Do Nothing.
    ''' </remarks>
    Public Function DefaultLoadStatusUpdateMethod(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, Optional ByRef oDetail As clsEDILoadStatusData = Nothing) As Boolean
        Dim blnRet As Boolean = False
        Dim strEventComment As String = ""
        Dim strEventDateTime As String = ""
        Dim strEventDescription As String = ""
        Dim intEventStatusControl As Integer = 0
        Dim strErrMessage As String = "Unexpected Error"
        If Not o214StopData Is Nothing AndAlso Not o214StopData.Orders Is Nothing AndAlso o214StopData.Orders.Count() > 0 Then
            'process the changes for each order in the orders list
            For Each order In o214StopData.Orders
                'get the booking record this is already filtered using SHID

                'Modified By LVV on 9/4/2019
                'NEW RULE
                'If the order number is missing/wrong we use the SHID to get the correct order number.
                'We only do this if there is 1 order number on the SHID. If the SHID is wrong then the entire process won't work. 
                'If there is more than 1 order on the SHID then continue as normal - Do Nothing.
                Dim blnOrderValidated = False
                If oBooks.Any(Function(x) x.BookCarrOrderNumber = order.OrderNumber And x.BookOrderSequence = order.OrderSequence) Then
                    blnOrderValidated = True
                Else
                    'The order number provided by the EDI doc does not match any order number for the provided SHID
                    If oBooks?.Count() = 1 Then
                        'if we only have 1 order on this SHID - replace the incorrect order info with the correct info from the database (only supported for single stop loads (LTL?))
                        order.OrderNumber = oBooks(0).BookCarrOrderNumber
                        order.OrderSequence = oBooks(0).BookOrderSequence
                        blnOrderValidated = True
                    End If
                End If
                If blnOrderValidated Then
                    Dim oBook = oBooks.Where(Function(x) x.BookCarrOrderNumber = order.OrderNumber And x.BookOrderSequence = order.OrderSequence).FirstOrDefault()
                    If Not oBook Is Nothing AndAlso oBook.BookControl <> 0 Then
                        order.BookControl = oBook.BookControl
                        'we have a match so update the required elements
                        processShipCarrierData(order, oBook, o214Data)
                        'check for Event Code Detail Mapping.
                        getLoadStatusDetailComments(oDetail, o214StopData.EventDate, o214StopData.EventTime, strEventComment, strEventDateTime, strEventDescription, intEventStatusControl)
                        'Modified By LVV on 9/17/19 for Bing Maps
                        If addBookTracksToOrder(order, o214StopData.LoadComments, o214StopData.EventComments, "EDI 214", o214Data.DefaultLoadStatusControl, Date.Now(), strErrMessage, o214StopData.StatusDetails, strEventComment, strEventDateTime, strEventDescription, intEventStatusControl) Then
                            blnRet = True
                        Else
                            create214LoadStatusUpdateFailureAlert(oBook.BookCustCompControl, oBook.BookCarrierControl, oBook.BookProNumber, oBook.BookSHID, oBook.BookCarrOrderNumber, oBook.BookOrderSequence, oBook.BookConsPrefix, o214Data.ShipCarrierProNumber, "", "", strErrMessage)
                        End If
                    End If
                End If
            Next
        End If
        Return blnRet
    End Function

    Public Function isDateStringDiff(ByVal strSourceDate As String, ByVal strTargetDate As String) As Boolean
        Dim blnRet As Boolean = False
        Dim sourceDate As Date? = Nothing
        Dim targetDate As Date? = Nothing
        Dim tstDate As Date
        If Date.TryParse(strSourceDate, tstDate) Then sourceDate = tstDate
        If Date.TryParse(strTargetDate, tstDate) Then targetDate = tstDate
        blnRet = isDateDiff(sourceDate, targetDate)
        Return blnRet
    End Function

    Public Function isDateDiff(ByVal sourceDate As Date?, ByVal targetDate As Date?) As Boolean
        Dim blnRet As Boolean = False
        'when source and target dates are nothing then return zero 
        'when 
        If sourceDate Is Nothing Then
            If targetDate Is Nothing Then
                blnRet = False 'they are the same
            Else
                blnRet = True 'if source is null but target is not then the dates are different
            End If
        Else
            If targetDate Is Nothing Then
                blnRet = True 'if source is not null but target is null the dates are different
            Else
                If Date.Compare(sourceDate.Value, targetDate.Value) <> 0 Then blnRet = True 'if date.compare not zero dates are different
            End If
        End If
        Return blnRet

    End Function


    ''' <summary>
    ''' Provide the current date and time for this status update, 
    ''' set all the others to nothing.  Will return the the highest 
    ''' status code based on the condition of the load in reverse order.  
    ''' Once a status code level has been reached updates do not affect the status code already set.
    ''' See Remarks for more details.
    ''' </summary>
    ''' <param name="oBook"></param>
    ''' <param name="order"></param>
    ''' <param name="PickUpFlag"></param>
    ''' <param name="SetDate"></param>
    ''' <param name="SetTime"></param>
    ''' <param name="ArrivedDate"></param>
    ''' <param name="ArrivedTime"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="StartTime"></param>
    ''' <param name="CompleteDate"></param>
    ''' <param name="CompleteTime"></param>
    ''' <param name="DepartDate"></param>
    ''' <param name="DepartTime"></param>
    ''' <returns>
    ''' 6 = Departed
    ''' 5 = Complete
    ''' 4 = Start
    ''' 3 = Arrived
    ''' 2 = Appointment Set
    ''' 1 = New Load
    ''' </returns>
    ''' <remarks> 
    ''' AMS Appt Status Codes are based on book data according to the Chart below for Pickup or Delivery in reverse order (Depart first etc...)
    ''' If the book data is not empty or the associated parameter is not empty the function returns the associated status code.
    ''' the function moves backwared until it finds a match returning 1 as the default for no dates being set.
    ''' 
    ''' Pickup Data Fields
    ''' Set Appointment    BookCarrScheduleDate         and BookCarrScheduleTime
    ''' Arrived            BookCarrActualDate           and BookCarrActualtime
    ''' Start (Loading)    BookCarrStartLoadingDate     and BookCarrStartLoadingTime
    ''' Complete (Loading) BookCarrFinishLoadingDate    and BookCarrFinishLoadingTime
    ''' Departs            BookCarrActLoadComplete_Date and BookCarrActLoadCompleteTime
    ''' 
    ''' Delivery Data Fields
    ''' Set Appointment    BookCarrApptDate            and BookCarrApptTime
    ''' Arrived            BookCarrActDate             and BookCarrActTime
    ''' Start (Unloading)  BookCarrStartUnloadingDate  and BookcarrStartUnloadingTime
    ''' Complete (Loading) BookCarrFinishUnloadingDate and BookCarrfinishUnloadingTime
    ''' Departs            BookCarrActUnloadCompDate   and BookCarrActUnloadCompTime 
    ''' </remarks>
    Public Function getAMSApptStatusCode(ByRef oBook As DTO.Book,
                                         ByRef order As cls214OrderData,
                                         ByVal PickUpFlag As Boolean,
                                         ByVal SetDate As Date?,
                                         ByVal SetTime As Date?,
                                         ByVal ArrivedDate As Date?,
                                         ByVal ArrivedTime As Date?,
                                         ByVal StartDate As Date?,
                                         ByVal StartTime As Date?,
                                         ByVal CompleteDate As Date?,
                                         ByVal CompleteTime As Date?,
                                         ByVal DepartDate As Date?,
                                         ByVal DepartTime As Date?) As Integer
        Dim AMSApptStatusCode As Integer = 1 'default
        'TODO: double check this logic to make sure the code is finished.
        If PickUpFlag Then
            'Pickup Data Fields
            'Set Appointment    BookCarrScheduleDate            & BookCarrScheduleTime
            'Arrived            BookCarrActualDate              & BookCarrActualtime
            'Start (Loading)    BookCarrStartLoadingDate        & BookCarrStartLoadingTime
            'Complete (Loading) BookCarrFinishLoadingDate       & BookCarrFinishLoadingTime
            'Departs            [BookCarrActLoadComplete Date]  & BookCarrActLoadCompleteTime
            With oBook
                'Return the first test that matches;  if the book data has already been populated in reverse order
                'starting with departs we use the last (highest) status code,  any other values are just updates 
                'or error corrections so the  AMSApptStatusCode  does not get updated
                If .BookCarrActLoadCompleteTime.HasValue Or DepartTime.HasValue Then
                    AMSApptStatusCode = 6
                ElseIf .BookCarrActLoadComplete_Date.HasValue Or DepartDate.HasValue Then
                    AMSApptStatusCode = 6
                ElseIf .BookCarrFinishLoadingTime.HasValue Or CompleteTime.HasValue Then
                    AMSApptStatusCode = 5
                ElseIf .BookCarrFinishLoadingDate.HasValue Or CompleteDate.HasValue Then
                    AMSApptStatusCode = 5
                ElseIf .BookCarrStartLoadingTime.HasValue Or StartTime.HasValue Then
                    AMSApptStatusCode = 4
                ElseIf .BookCarrStartLoadingDate.HasValue Or StartDate.HasValue Then
                    AMSApptStatusCode = 4
                ElseIf .BookCarrActualTime.HasValue Or ArrivedTime.HasValue Then
                    AMSApptStatusCode = 3
                ElseIf .BookCarrActualDate.HasValue Or ArrivedDate.HasValue Then
                    AMSApptStatusCode = 3
                ElseIf .BookCarrScheduleTime.HasValue Or SetTime.HasValue Then
                    AMSApptStatusCode = 2
                ElseIf .BookCarrScheduleDate.HasValue Or SetDate.HasValue Then
                    AMSApptStatusCode = 2
                Else
                    AMSApptStatusCode = 1
                End If
            End With
        Else
            'Delivery Data Fields
            'Set Appointment    BookCarrApptDate            & BookCarrApptTime
            'Arrived            BookCarrActDate             & BookCarrActTime
            'Start (Unloading)  BookCarrStartUnloadingDate  & BookcarrStartUnloadingTime
            'Complete (Loading) BookCarrFinishUnloadingDate & BookCarrfinishUnloadingTime
            'Departs            BookCarrActUnloadCompDate   & BookCarrActUnloadCompTime
            With oBook
                'Return the first test that matches;  if the book data has already been populated in reverse order
                'starting with departs we use the last (highest) status code,  any other values are just updates 
                'or error corrections so the  AMSApptStatusCode  does not get updated
                If .BookCarrActUnloadCompTime.HasValue Or DepartTime.HasValue Then
                    AMSApptStatusCode = 6
                ElseIf .BookCarrActUnloadCompDate.HasValue Or DepartDate.HasValue Then
                    AMSApptStatusCode = 6
                ElseIf .BookCarrFinishUnloadingTime.HasValue Or CompleteTime.HasValue Then
                    AMSApptStatusCode = 5
                ElseIf .BookCarrFinishUnloadingDate.HasValue Or CompleteDate.HasValue Then
                    AMSApptStatusCode = 5
                ElseIf .BookCarrStartUnloadingTime.HasValue Or StartTime.HasValue Then
                    AMSApptStatusCode = 4
                ElseIf .BookCarrStartUnloadingDate.HasValue Or StartDate.HasValue Then
                    AMSApptStatusCode = 4
                ElseIf .BookCarrActTime.HasValue Or ArrivedTime.HasValue Then
                    AMSApptStatusCode = 3
                ElseIf .BookCarrActDate.HasValue Or ArrivedDate.HasValue Then
                    AMSApptStatusCode = 3
                ElseIf .BookCarrApptTime.HasValue Or SetTime.HasValue Then
                    AMSApptStatusCode = 2
                ElseIf .BookCarrApptDate.HasValue Or SetDate.HasValue Then
                    AMSApptStatusCode = 2
                Else
                    AMSApptStatusCode = 1
                End If
            End With

        End If

        Return AMSApptStatusCode

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <param name="o214StopData"></param>
    ''' <param name="oDetail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' business rules: 
    ''' 1. set up data for Pickup location 
    '''   a.	Managed Facility:
    '''     i.	Appointment exists in calendar
    '''       1.	Compare Date and Time values:  
    '''         a.	Date and Time values match calendar data: The System updates load status “Carrier departed pickup location”.
    '''         b.	Date and Time values do not match calendar data: 
    '''           i.	The System updates load status “Carrier departed pickup location date and time is different from the Scheduler Calendar”.   
    '''           ii.	Generate subscription alert: (AlertPick2145DepartNotMatchAppt) 214 Pickup Checkout Does Not Match Schedule.
    '''         c.	If the Scheduler Calendar data is empty:
    '''           i.	The System updates the Check Out Date and Time on the Load Carrier Data Tab, [BookCarrActLoadComplete Date] and [BookCarrActLoadCompleteTime], with the information provided by the carrier.
    '''           ii.	Generate subscription alert: (AlertPick2145DepartMissingApptCheckout) 214 Pickup Checkout Date is Missing in Schedule.
    '''       2.	Calendar Checkout Loading date and time does not match Carrier Data Tab information (this should not happen but we check in case of a third party application or other unexpected problem):
    '''         a.	Generate subscription alert: (AlertPickBook5DepartNotMatchAppt) Booked Pickup Checkout Does Not Match Schedule.
    '''     ii.	Appointment does not exist in calendar
    '''       1.	The System creates a Load Status Message “Carrier departed pickup location but an appointment was not created in the calendar”
    '''       2.	The system updates the Check Out Date and Time on the Load Carrier Data Tab, [BookCarrActLoadComplete Date] and [BookCarrActLoadCompleteTime].
    '''       3.	Generate subscription alert: (AlertPick2145DepartMissingAppt) 214 Pickup Checkout Appt is Missing in Schedule.
    '''   b.	Non-managed Facility:
    '''     i.	The System creates a Load Status Message “Carrier departed pickup location”.
    '''     ii.	The system updates the Check Out Date and Time on the Load Carrier Data Tab, [BookCarrActLoadComplete Date] and [BookCarrActLoadCompleteTime]
    ''' </remarks>
    Public Function CarrierDepartsPickup(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, ByRef oDetail As clsEDI214FactoryDetail) As Boolean

        If oDetail.StatusDetail.EDIElementName <> "AT701" Then
            'this is not a valid Action so raise an alert and update the load status using defaults

        End If
        Dim oSettings As New clsEDI214EventSettings With {.blnPickup = True,
                                                        .DateTimeValidationType = BLL.NGLBookBLL.AMSDateTimeValidationType.AMSApptActLoadCompleteDateTime,
                                                        .enmEventDateType = clsEDI214EventSettings.EventDateType.Checkout,
                                                        .strEventMsg = "Pickup check out Date and Time",
                                                        .strNonManagedMsg = "Carrier departed pickup location."}

        Return ProcessLoadStatusUpdates(oBooks, o214Data, o214StopData, oDetail, oSettings)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <param name="o214StopData"></param>
    ''' <param name="oDetail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' business rules: 
    ''' 1. set up data for Delivery location 
    '''   a.	Managed Facility:
    '''     i.	Appointment exists in calendar
    '''       1.	Compare Date and Time values:  
    '''         a.	Date and Time values match calendar data: The System updates load status “Carrier departs delivery location”.
    '''         b.	Date and Time values do not match calendar data: 
    '''           i.	The System updates load status “Carrier departed but date and time is different from the Scheduler Calendar”.   
    '''           ii.	Generate subscription alert: (AlertDel2145DepartNotMatchAppt) 214 Delivery Checkout Does Not Match Schedule.
    '''         c.	If the Scheduler Calendar data is empty:
    '''           i.	The System updates the Check Out Date and Time on the Load Carrier Data Tab, [BookCarrActUnloadCompDate] and [BookCarrActUnloadCompTime], with the information provided by the carrier.
    '''           ii.	Generate subscription alert: (AlertDel2145DepartMissingApptCheckout) 214 Delivery Checkout Date is Missing in Schedule.  Traffic Managers will need to coordinate with Dock Managers to be sure Scheduler information is correctly entered 
    '''       2.	Calendar Check Out date and time does not match Carrier Data Tab information (this should not happen but we check in case of a third party application or other unexpected problem):
    '''         a.	Generate subscription alert: (AlertDelBook5DepartNotMatchAppt) Booked Delivery Checkout Does Not Match Schedule.
    '''     ii.	Appointment does not exist in calendar
    '''       1.	The System creates a Load Status Message “Carrier departed delivery but an appointment was not created in the calendar”
    '''       2.	The system updates the Check Out Date and Time on the Load Carrier Data Tab, [BookCarrActUnloadCompDate] and [BookCarrActUnloadCompTime].
    '''       3.	Generate subscription alert: (AlertDel2141ApptNotMatch) 214 Delivery Checkout Appt is Missing in Schedule. 
    '''   b.	Non-managed Facility:
    '''     i.	The System creates a Load Status Message “Carrier departed delivery location”.
    '''     ii.	The system updates the Check Out Date and Time on the Load Carrier Data Tab, [BookCarrActUnloadCompDate] and [BookCarrActUnloadCompTime]
    ''' </remarks>
    Public Function CarrierDepartsDelivery(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, ByRef oDetail As clsEDI214FactoryDetail) As Boolean
        Dim oSettings As New clsEDI214EventSettings With {.blnPickup = False,
                                                        .DateTimeValidationType = BLL.NGLBookBLL.AMSDateTimeValidationType.AMSApptActLoadCompleteDateTime,
                                                        .enmEventDateType = clsEDI214EventSettings.EventDateType.Checkout,
                                                        .strEventMsg = "Delivery check out Date and Time",
                                                        .strNonManagedMsg = "Carrier departed delivery location."}
        Return ProcessLoadStatusUpdates(oBooks, o214Data, o214StopData, oDetail, oSettings)
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <param name="o214StopData"></param>
    ''' <param name="oDetail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Business Rules: 
    ''' 1. set up data for Pickup location 
    '''   a.	Managed Facility:
    '''     i.	Appointment exists in calendar
    '''       1.	Compare Date and Time values:  
    '''         a.	Date and Time values match calendar data: The System updates load status “Carrier arrived at pickup location”.
    '''         b.	Date and Time values do not match calendar data: 
    '''           i.	The System updates load status “Carrier check in date and time is different from the Scheduler Calendar”.   
    '''           ii.	Generate subscription alert: (AlertPick2142CheckInNotMatchAppt) 214 Pickup Check In Does Not Match Schedule.
    '''         c.	If the Scheduler Calendar data is empty:
    '''           i.	The System updates the Check in Date and Time on the Load Carrier Data Tab, [BookCarrActualDate] and [BookCarrActualTime], with the information provided by the carrier 
    '''           ii.	Generate subscription alert:  (AlertPick2142CheckInMissingApptCheckIn) 214 Pickup Check In Date is Missing in Schedule.
    '''       2.	Calendar Check In date and time does not match Carrier Data Tab information (this should not happen but we check in case of a third party application or other unexpected problem):
    '''         a.	Generate subscription alert: (AlertPickBook2CheckInNotMatchAppt) Booked Pickup Check In Does Not Match Schedule.  
    '''           ii.	Appointment does not exist in calendar
    '''             1.	The System creates a Load Status Message “Carrier arrived at pickup but an appointment was not created in the calendar”
    '''             2.	The System updates the Check in Date and Time on the Load Carrier Data Tab, [BookCarrActualDate] and [BookCarrActualTime].
    '''       3.	Generate subscription alert: (AlertPick2142CheckInMissingAppt) 214 Pickup Check In Appt is Missing in Schedule.
    '''         b.	Non-managed Facility:
    '''           i.	The System creates a Load Status Message “Carrier arrived at pickup location”.
    '''           ii.	The System updates the Check in Date and Time on the Load Carrier Data Tab, [BookCarrActualDate] and [BookCarrActualTime]
    ''' </remarks>
    Public Function CarrierArrivedAtPickup(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, ByRef oDetail As clsEDI214FactoryDetail) As Boolean
        Dim oSettings As New clsEDI214EventSettings With {.blnPickup = True,
                                                        .DateTimeValidationType = BLL.NGLBookBLL.AMSDateTimeValidationType.AMSApptActualDateTime,
                                                        .enmEventDateType = clsEDI214EventSettings.EventDateType.Checkin,
                                                        .strEventMsg = "Pickup Check In Date and Time",
                                                        .strNonManagedMsg = "Carrier arrived at pickup location."}
        Return ProcessLoadStatusUpdates(oBooks, o214Data, o214StopData, oDetail, oSettings)

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <param name="o214StopData"></param>
    ''' <param name="oDetail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' business rules: 
    ''' 1. set up data for Delivery location 
    '''   a.	Managed Facility:
    '''     i.	Appointment exists in calendar
    '''       1.	Compare Date and Time values:  
    '''         a.	Date and Time values match calendar data: The System updates load status “Carrier arrived at delivery location”.
    '''         b.	Date and Time values do not match calendar data:
    '''           i.	The System updates load status “Carrier check in date and time is different from the Scheduler Calendar”.  
    '''           ii.	Generate subscription alert: (AlertDel2142CheckInMissingApptCheckIn) 214 Delivery Check In Date is Missing in Schedule.
    '''         c.	If the Scheduler Calendar data is empty:
    '''           i.	The System updates the Check in Date and Time on the Load Carrier Data Tab, [BookCarrActDate] and [BookCarrActTime], with the information provided by the carrier 
    '''           ii.	Generate subscription alert: (AlertDel2142CheckInMissingApptCheckIn) 214 Delivery Check In Date is Missing in Schedule.
    '''       2.	Calendar Check In date and time does not match Carrier Data Tab information (this should not happen but we check in case of a third party application or other unexpected problem):
    '''         a.	Generate subscription alert: (AlertDelBook2CheckInNotMatchAppt) Booked Delivery Check In Does Not Match Schedule.
    '''     ii.	Appointment does not exist in calendar
    '''       1.	The System creates a Load Status Message “Carrier arrived at Delivery but an appointment was not created in the calendar”
    '''       2.	The System updates the Check in Date and Time on the Load Carrier Data Tab, [BookCarrActDate] and [BookCarrActTime].
    '''       3.	Generate subscription alert: (AlertDel2142CheckInMissingAppt) 214 Delivery Check In Appt is Missing in Schedule.
    '''   b.	Non-managed Facility:
    '''     i.	The System creates a Load Status Message “Carrier arrived at Delivery location”.
    '''     ii.	The System updates the Check in Date and Time on the Load Carrier Data Tab, [BookCarrActDate] and [BookCarrActTime]
    ''' </remarks>
    Public Function CarrierArrivedAtDelivery(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, ByRef oDetail As clsEDI214FactoryDetail) As Boolean
        Dim oSettings As New clsEDI214EventSettings With {.blnPickup = False,
                                                        .DateTimeValidationType = BLL.NGLBookBLL.AMSDateTimeValidationType.AMSApptActualDateTime,
                                                        .enmEventDateType = clsEDI214EventSettings.EventDateType.Checkin,
                                                        .strEventMsg = "Delivery Check In Date and Time",
                                                        .strNonManagedMsg = "Carrier arrived at delivery location."}
        Return ProcessLoadStatusUpdates(oBooks, o214Data, o214StopData, oDetail, oSettings)
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <param name="o214StopData"></param>
    ''' <param name="oDetail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' business rules: 
    ''' 1. set up data for Pickup location  
    '''   a. is this a Managed Facility
    '''     i. Does the Appointment exists in calendar 
    '''       1. Carriers Appointment date and time matches the calendar data.
    '''         a. Update the Load Status Message Carrier acknowledges pickup appointment date and time
    '''       2. Carriers Appointment date and time does not match calendar data.
    '''         a.	Update the Load Status Message Carrier pickup appointment date and time does not match calendar.
    '''         b.	Generate subscription alert: (AlertPick2141ApptMissing ) 214 Pickup Appt Does Not Match Schedule. 
    '''       3. Calendars Appointment date and time does not match Carrier Data Tab information.
    '''         a. Generate subscription alert: (AlertPickBook1ApptNotMatch) Booked Pickup Appt Does Not Match Schedule.
    '''     ii. Appointment does not exist in calendar.
    '''       1. Create a Load Status Message Carrier appointment accepted at pickup but does not exist in calendar.
    '''       2. Update Scheduled Appt Date and Time information on the Load Carrier Data Tab, [BookCarrScheduleDate] and [BookCarrScheduleTime].
    '''          The Load Carrier Data Tab will be updated but it will not exist in the calendar
    '''       3. Generate subscription alert: (AlertPick2141ApptMissing) 214 Pickup Appt is Missing in Schedule.
    '''   b. Non-managed Facility:
    '''     i. The System creates a Load Status Message Carrier appointment set at pickup location. 
    '''     ii. The System updates Scheduled Appt Date and Time information on the Load Carrier Data Tab, [BookCarrScheduleDate] and [BookCarrScheduleTime]. 
    '''</remarks>
    Public Function CarrierAppointmentSetAtPickup(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, ByRef oDetail As clsEDI214FactoryDetail) As Boolean
        Dim oSettings As New clsEDI214EventSettings With {.blnPickup = True,
                                                         .DateTimeValidationType = BLL.NGLBookBLL.AMSDateTimeValidationType.AMSApptStartDate,
                                                         .enmEventDateType = clsEDI214EventSettings.EventDateType.AppointmentSet,
                                                         .strEventMsg = "Pickup Appointment Date and Time",
                                                         .strNonManagedMsg = "Carrier appointment set at pickup location."}
        Return ProcessLoadStatusUpdates(oBooks, o214Data, o214StopData, oDetail, oSettings)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <param name="o214StopData"></param>
    ''' <param name="oDetail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' business rules: 
    ''' 1. set up data for Delivery location 
    '''   a.	Managed Facility:
    '''     i.	Appointment exists in calendar
    '''       1.	Carrier’s Appointment date and time matches calendar data.
    '''         a.	The System updates Load Status Message “Carrier acknowledges delivery appointment date and time”
    '''       2.	Carrier’s Appointment date and time does not match calendar data.
    '''         a.	The System updates Load Status Message “Carrier delivery appointment date and time does not match calendar”.
    '''         b.	Generate subscription alert: (AlertDel2141ApptNotMatch) 214 Delivery Appt Does Not Match Schedule..
    '''       3.	Calendar Appointment date and time does not match Carrier Data Tab information (this should not happen but we check in case of a third party application or other unexpected problem):
    '''         a.	Generate subscription alert: (AlertDelBook1ApptNotMatch) Booked Delivery Appt Does Not Match Schedule.  
    '''     ii.	Appointment does not exist in calendar.
    '''       1.	In 7.0 we will create a Load Status Message “Carrier appointment accepted at Delivery but does not exist in calendar”.  
    '''       2.	The System updates “Scheduled Appt Date and Time” information on the Load Carrier Data Tab, [BookCarrApptDate] and [BookCarrApptTime]. The Load Carrier Data Tab will be updated but it will not exist in the calendar.
    '''       3.	Generate subscription alert: (AlertDel2141ApptMissing) 214 Delivery Appt is Missing in Schedule.  Traffic Managers will need to coordinate with Dock Managers to be sure appointment date and times match available time slots in the calendar. 
    '''   b.	Non-managed Facility:
    '''     i.	The System creates a Load Status Message “Carrier appointment set at Delivery location”.
    '''     ii.	The System updates “Scheduled Appt Date and Time” information on the Load Carrier Data Tab, [BookCarrApptDate] and [BookCarrApptTime]. 
    ''' </remarks>
    Public Function CarrierAppointmentSetAtDelivery(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, ByRef oDetail As clsEDI214FactoryDetail) As Boolean
        Dim oSettings As New clsEDI214EventSettings With {.blnPickup = False,
                                                         .DateTimeValidationType = BLL.NGLBookBLL.AMSDateTimeValidationType.AMSApptStartDate,
                                                         .enmEventDateType = clsEDI214EventSettings.EventDateType.AppointmentSet,
                                                         .strEventMsg = "Delivery Appointment Date and Time",
                                                         .strNonManagedMsg = "Carrier appointment set at delivery location."}
        Return ProcessLoadStatusUpdates(oBooks, o214Data, o214StopData, oDetail, oSettings)
    End Function

    Public Function CarrierShipmentDelayed(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, ByRef oDetail As clsEDI214FactoryDetail) As Boolean
        Dim blnRet As Boolean = False
        Dim strEventComment As String = ""
        Dim strEventDateTime As String = ""
        Dim strEventDescription As String = ""
        Dim intEventStatusControl As Integer = 0
        Dim strErrMessage As String = "Unexpected Error"
        If Not o214StopData Is Nothing AndAlso Not o214StopData.Orders Is Nothing AndAlso o214StopData.Orders.Count() > 0 Then
            'process the changes for each order in the orders list
            For Each order In o214StopData.Orders
                'get the booking record this is already filtered using SHID
                If oBooks.Any(Function(x) x.BookCarrOrderNumber = order.OrderNumber And x.BookOrderSequence = order.OrderSequence) Then
                    Dim oBook = oBooks.Where(Function(x) x.BookCarrOrderNumber = order.OrderNumber And x.BookOrderSequence = order.OrderSequence).FirstOrDefault()
                    If Not oBook Is Nothing AndAlso oBook.BookControl <> 0 Then
                        order.BookControl = oBook.BookControl
                        'we have a match so update the required elements
                        processShipCarrierData(order, oBook, o214Data)
                        'check for Event Code Detail Mapping.
                        getLoadStatusDetailComments(oDetail, o214StopData.EventDate, o214StopData.EventTime, strEventComment, strEventDateTime, strEventDescription, intEventStatusControl)
                        If oDetail.ReasonLoadStatusControl <> 0 Then
                            intEventStatusControl = oDetail.ReasonLoadStatusControl
                            'TODO:  check the Reason Factory setting and execue the function if available.

                        End If
                        'Modified By LVV on 9/17/19 for Bing Maps
                        If addBookTracksToOrder(order, o214StopData.LoadComments, o214StopData.EventComments, "EDI 214", o214Data.DefaultLoadStatusControl, Date.Now(), strErrMessage, o214StopData.StatusDetails, strEventComment, strEventDateTime, strEventDescription, intEventStatusControl) Then
                            blnRet = True
                        Else
                            create214LoadStatusUpdateFailureAlert(oBook.BookCustCompControl, oBook.BookCarrierControl, oBook.BookProNumber, oBook.BookSHID, oBook.BookCarrOrderNumber, oBook.BookOrderSequence, oBook.BookConsPrefix, o214Data.ShipCarrierProNumber, "", "", strErrMessage)
                        End If
                    End If
                End If
            Next
        End If
        Return blnRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <param name="o214StopData"></param>
    ''' <param name="oDetail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' business rules: 
    ''' 1. set up data for Delivery location 
    '''   a.	Managed Facility:
    '''     i.	Appointment exists in calendar
    '''       1.	Compare Date and Time values:  
    '''         a.	Date and Time values match calendar data: The System updates load status “Carrier completed unloading at Delivery location”.
    '''         b.	Date and Time values do not match calendar data: 
    '''           i.	The System updates load status “Carrier completed unloading but date and time is different from the Scheduler Calendar”.  
    '''           ii.	Generate subscription alert: (AlertDel2144FinishNotMatchAppt) 214 Finish Unloading Does Not Match Schedule.
    '''         c.	If the Scheduler Calendar data is empty:
    '''           i.	The System updates the Finish Unloading Date and Time on the Load Carrier Data Tab, [BookCarrFinishUnloadingDate] and [BookCarrFinishUnloadingTime], with the information provided by the carrier.
    '''           ii.	Generate subscription alert: (AlertDel2144FinishMissingApptFinish) 214 Finish Unloading Date is Missing in Schedule.
    '''       2.	Calendar Finished Unloading date and time does not match Carrier Data Tab information (this should not happen but we check in case of a third party application or other unexpected problem):
    '''         a.	Generate subscription alert: (AlertDelBook4FinishNotMatchAppt) Booked Finish Unloading Does Not Match Schedule.
    '''     ii.	Appointment does not exist in calendar
    '''       1.	The System creates a Load Status Message “Carrier completed unloading at Delivery but an appointment was not created in the calendar”
    '''       2.	The system updates the Finish Unloading Date and Time on the Load Carrier Data Tab, [BookCarrFinishUnloadingDate] and [BookCarrFinishUnloadingTime].
    '''       3.	Generate subscription alert: (AlertDel2144FinishMissingAppt) 214 Finish Unloading Appt is Missing in Schedule.
    '''   b.	Non-managed Facility:
    '''     i.	The System creates a Load Status Message “Carrier completed unloading at Delivery location”.
    '''     ii.	The system updates the Finish unloading Date and Time on the Load Carrier Data Tab, [BookCarrFinishUnloadingDate] and [BookCarrFinishUnloadingTime]
    ''' </remarks>
    Public Function CarrierCompleteUnloadAtDelivery(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, ByRef oDetail As clsEDI214FactoryDetail) As Boolean
        Dim oSettings As New clsEDI214EventSettings With {.blnPickup = False,
                                                        .DateTimeValidationType = BLL.NGLBookBLL.AMSDateTimeValidationType.AMSApptFinishLoadingDateTime,
                                                        .enmEventDateType = clsEDI214EventSettings.EventDateType.Finish,
                                                        .strEventMsg = "Delivery complete unloading Date and Time",
                                                        .strNonManagedMsg = "Carrier completed unloading at delivery location."}
        Return ProcessLoadStatusUpdates(oBooks, o214Data, o214StopData, oDetail, oSettings)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <param name="o214StopData"></param>
    ''' <param name="oDetail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' business rules: 
    ''' 1. set up data for Delivery location 
    '''   a.	Managed Facility:
    '''     i.	Appointment exists in calendar
    '''       1.	Compare Date and Time values:  
    '''         a.	Date and Time values match calendar data: The System updates load status “Carrier started unloading at Delivery location”.
    '''         b.	Date and Time values do not match calendar data: 
    '''           i.	The System updates load status “Carrier arrived at dock door but unloading date and time is different in the Scheduler Calendar”.   
    '''           ii.	Generate subscription alert: (AlertDel2143StartNotMatchAppt) 214 Start Unloading Does Not Match Schedule.
    '''         c.	If the Scheduler Calendar data is empty:
    '''           i.	The System updates the Start Unloading Date and Time on the Load Carrier Data Tab, [BookCarrStartUnLoadingDate] and [BookCarrStartUnLoadingTime], with the information provided by the carrier.
    '''           ii.	Generate subscription alert: (AlertDel2143StartMissingApptStart) 214 Start Unloading Date is Missing in Schedule.  Traffic Managers will need to coordinate with Dock Managers to be sure Scheduler information is correctly entered.
    '''       2.	Calendar Start Unloading date and time does not match Carrier Data Tab information (this should not happen but we check in case of a third party application or other unexpected problem):
    '''         a.	Generate subscription alert: (AlertDelBook3StartNotMatchAppt) Booked Start Unloading Does Not Match Schedule.
    '''     ii.	Appointment does not exist in calendar
    '''       1.	The System creates a Load Status Message “Carrier started unloading at delivery but an appointment was not created in the calendar”
    '''       2.	The system updates the Start Unloading Date and Time on the Load Carrier Data Tab, [BookCarrStartUnLoadingDate] and [BookCarrStartUnLoadingTime].
    '''       3.	Generate subscription alert: (AlertDel2143StartMissingAppt) 214 Start Unloading Appt is Missing in Schedule.
    '''   b.	Non-managed Facility:
    '''     i.	The System creates a Load Status Message “Carrier started unloading at Delivery location”.
    '''     ii.	The system updates the Start UnLoading Date and Time on the Load Carrier Data Tab, [BookCarrStartUnLoadingDate] and [BookCarrStartUnLoadingTime]
    ''' </remarks>
    Public Function CarrierStartUnloadAtDelivery(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, ByRef oDetail As clsEDI214FactoryDetail) As Boolean
        Dim oSettings As New clsEDI214EventSettings With {.blnPickup = False,
                                                          .DateTimeValidationType = BLL.NGLBookBLL.AMSDateTimeValidationType.AMSApptStartLoadingDateTime,
                                                          .enmEventDateType = clsEDI214EventSettings.EventDateType.Start,
                                                          .strEventMsg = "Delivery Start Unloading Date and Time",
                                                          .strNonManagedMsg = "Carrier started unloading at delivery location."}
        Return ProcessLoadStatusUpdates(oBooks, o214Data, o214StopData, oDetail, oSettings)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <param name="o214StopData"></param>
    ''' <param name="oDetail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' business rules: 
    ''' 1. set up data for Pickup location 
    '''   a.	Managed Facility:
    '''     i.	Appointment exists in calendar
    '''       1.	Compare Date and Time values:  
    '''         a.	Date and Time values match calendar data: The System updates load status “Carrier complete loading at pickup location”.
    '''         b.	Date and Time values do not match calendar data: 
    '''           i.	The System updates load status “Carrier completed loading date and time is different from the Scheduler Calendar”.   
    '''           ii.	Generate subscription alert: (AlertPick2144FinishNotMatchAppt) 214 Finish Loading Does Not Match Schedule.
    '''         c.	If the Scheduler Calendar data is empty:
    '''           i.	The System updates the Finish Loading Date and Time on the Load Carrier Data Tab, [BookCarrFinishLoadingDate] and [BookCarrFinishLoadingTime], with the information provided by the carrier.
    '''           ii.	Generate subscription alert: (AlertPick2144FinishMissingApptFinish) 214 Finish Loading Date is Missing in Schedule. 
    '''       2.	Calendar Finish Loading date and time does not match Carrier Data Tab information (this should not happen but we check in case of a third party application or other unexpected problem):
    '''         a.	Generate subscription alert: (AlertPickBook4FinishNotMatchAppt) Booked Finish Loading Does Not Match Schedule.    
    '''     ii.	Appointment does not exist in calendar
    '''       1.	The System creates a Load Status Message “Carrier completed loading but an appointment was not created in the calendar”
    '''       2.	The system updates the Finish Loading Date and Time on the Load Carrier Data Tab, [BookCarrFinishLoadingDate] and [BookCarrFinishLoadingTime].
    '''       3.	Generate subscription alert: (AlertPick2144FinishMissingAppt) 214 Finish Loading Appt is Missing in Schedule.
    '''   b.	Non-managed Facility:
    '''     i.	The System creates a Load Status Message “Carrier completed loading”.
    '''     ii.	The system updates the Finish Loading Date and Time on the Load Carrier Data Tab, [BookCarrFinishLoadingDate] and [BookCarrFinishLoadingTime]
    ''' </remarks>
    Public Function CarrierCompleteLoadAtPickup(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, ByRef oDetail As clsEDI214FactoryDetail) As Boolean
        Dim oSettings As New clsEDI214EventSettings With {.blnPickup = True,
                                                        .DateTimeValidationType = BLL.NGLBookBLL.AMSDateTimeValidationType.AMSApptFinishLoadingDateTime,
                                                        .enmEventDateType = clsEDI214EventSettings.EventDateType.Finish,
                                                        .strEventMsg = "Pickup complete loading Date and Time",
                                                        .strNonManagedMsg = "Carrier completed loading at pickup location."}
        Return ProcessLoadStatusUpdates(oBooks, o214Data, o214StopData, oDetail, oSettings)
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <param name="o214StopData"></param>
    ''' <param name="oDetail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' business rules: 
    ''' 1. set up data for Pickup location 
    '''   a.	Managed Facility:
    '''     i.	Appointment exists in calendar
    '''       1.	Compare Date and Time values:  
    '''         a.	Date and Time values match calendar data: The System updates load status “Carrier started loading at pickup location”.
    '''         b.	Date and Time values do not match calendar data: 
    '''           i.	The System updates load status “Carrier start loading date and time is different from the Scheduler Calendar”.
    '''           ii.	Generate subscription alert: (AlertPick2143StartNotMatchAppt) 214 Start Loading Does Not Match Schedule.
    '''         c.	If the Scheduler Calendar data is empty:
    '''           i.	The System updates the Start Loading Date and Time on the Load Carrier Data Tab, [BookCarrStartLoadingDate] and [BookCarrStartLoadingTime], with the information provided by the carrier.
    '''           ii.	Generate subscription alert: (AlertPick2143StartMissingApptStart) 214 Start Loading Date is Missing in Schedule.  Traffic Managers will need to coordinate with Dock Managers to be sure Scheduler information is correctly entered.
    '''       2.	Calendar Start Loading date and time does not match Carrier Data Tab information (this should not happen but we check in case of a third party application or other unexpected problem):
    '''         a.	Generate subscription alert: (AlertPickBook3StartNotMatchAppt) Booked Start Loading Does Not Match Schedule.  
    '''     ii.	Appointment does not exist in calendar
    '''       1.	The System creates a Load Status Message “Carrier started loading at pickup but an appointment was not created in the calendar”
    '''       2.	The system updates the Start Loading Date and Time on the Load Carrier Data Tab, [BookCarrStartLoadingDate] and [BookCarrStartLoadingTime].
    '''       3.	Generate subscription alert: (AlertPick2143StartMissingAppt) 214 Start Loading Appt is Missing in Schedule.
    '''   b.	Non-managed Facility:
    '''     i.	The System creates a Load Status Message “Carrier started loading at pickup location”.
    '''     ii.	The system updates the Start Loading Date and Time on the Load Carrier Data Tab, [BookCarrStartLoadingDate] and [BookCarrStartLoadingTime]
    ''' </remarks>
    Public Function CarrierStartLoadAtPickup(ByRef oBooks As DTO.Book(), ByRef o214Data As cls214LoadData, ByRef o214StopData As cls214StopData, ByRef oDetail As clsEDI214FactoryDetail) As Boolean

        Dim oSettings As New clsEDI214EventSettings With {.blnPickup = True,
                                                          .DateTimeValidationType = BLL.NGLBookBLL.AMSDateTimeValidationType.AMSApptStartLoadingDateTime,
                                                          .enmEventDateType = clsEDI214EventSettings.EventDateType.Start,
                                                          .strEventMsg = "Pickup Start Loading Date and Time",
                                                          .strNonManagedMsg = "Carrier started loading at pickup location."}
        Return ProcessLoadStatusUpdates(oBooks, o214Data, o214StopData, oDetail, oSettings)

    End Function


    ''' <summary>
    ''' returns true if at least one load status update is created.
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="o214Data"></param>
    ''' <param name="o214StopData"></param>
    ''' <param name="oDetail"></param>
    ''' <param name="oSettings"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 9/5/2019
    '''  NEW RULE
    '''  If the order number is missing/wrong we use the SHID to get the correct order number.
    '''  We only do this if there is 1 order number on the SHID. If the SHID is wrong then the entire process won't work. 
    '''  If there is more than 1 order on the SHID then continue as normal - Do Nothing.
    ''' </remarks>
    Public Function ProcessLoadStatusUpdates(ByRef oBooks As DTO.Book(),
                                             ByRef o214Data As cls214LoadData,
                                             ByRef o214StopData As cls214StopData,
                                             ByRef oDetail As clsEDI214FactoryDetail,
                                             ByVal oSettings As clsEDI214EventSettings) As Boolean
        Dim blnRet As Boolean = False
        If oSettings Is Nothing Then Return False
        ' business rules: 
        ' 1. set up data for Pickup location 
        Dim strEventComment As String = ""
        Dim strEventDescription As String = ""
        Dim intEventStatusControl As Integer = 0
        Dim strErrMessage As String = "Unexpected Error"
        Dim strEventDateTime As String = ""
        Dim dtEvent As Date
        Dim strApptEventDateTime As String = ""
        Dim strCarrierDateTime As String = ""
        Dim blnUpdateLoadCarrierData As Boolean = False
        Dim blnCreate214DateTimeValidationAlert As Boolean = False
        Dim strDateTimeValidationAlertMessage As String = ""
        Dim enmAlertType As clsEDI214.Alerts = Alerts.None
        Dim blnReadAppointmentError As Boolean = False
        Dim ManagedComp As Integer = 0
        Dim ApptControl As Integer = 0
        Dim strAlertComment As String = ""
        If Not o214StopData Is Nothing AndAlso Not o214StopData.Orders Is Nothing AndAlso o214StopData.Orders.Count() > 0 Then
            'process the changes for each order in the orders list
            For Each order In o214StopData.Orders
                'get the booking record this is already filtered using SHID

                'Modified By LVV on 9/4/2019
                'NEW RULE
                'If the order number is missing/wrong we use the SHID to get the correct order number.
                'We only do this if there is 1 order number on the SHID. If the SHID is wrong then the entire process won't work. 
                'If there is more than 1 order on the SHID then continue as normal - Do Nothing.
                Dim blnOrderValidated = False
                If oBooks.Any(Function(x) x.BookCarrOrderNumber = order.OrderNumber And x.BookOrderSequence = order.OrderSequence) Then
                    blnOrderValidated = True
                Else
                    'The order number provided by the EDI doc does not match any order number for the provided SHID
                    If oBooks?.Count() = 1 Then
                        'if we only have 1 order on this SHID - replace the incorrect order info with the correct info from the database (only supported for single stop loads (LTL?))
                        order.OrderNumber = oBooks(0).BookCarrOrderNumber
                        order.OrderSequence = oBooks(0).BookOrderSequence
                        blnOrderValidated = True
                    End If
                End If
                If blnOrderValidated Then
                    Dim oBook = oBooks.Where(Function(x) x.BookCarrOrderNumber = order.OrderNumber And x.BookOrderSequence = order.OrderSequence).FirstOrDefault()
                    If Not oBook Is Nothing AndAlso oBook.BookControl <> 0 Then
                        strAlertComment = ""
                        order.BookControl = oBook.BookControl
                        'we have a match so update the required elements
                        processShipCarrierData(order, oBook, o214Data)
                        'check for Event Code Detail Mapping.
                        getLoadStatusDetailComments(oDetail, o214StopData.EventDate, o214StopData.EventTime, strEventComment, strEventDateTime, strEventDescription, intEventStatusControl)
                        If Date.TryParse(strEventDateTime, dtEvent) Then
                            blnUpdateLoadCarrierData = False
                            blnCreate214DateTimeValidationAlert = False
                            strDateTimeValidationAlertMessage = ""
                            enmAlertType = Alerts.None
                            'a.	Managed Facility:
                            If oSettings.blnPickup Then
                                ManagedComp = oBook.BookOrigCompControl
                                ApptControl = oBook.BookAMSPickupApptControl
                            Else
                                ManagedComp = oBook.BookDestCompControl
                                ApptControl = oBook.BookAMSDeliveryApptControl
                            End If
                            If ManagedComp <> 0 Then
                                'i. Does Appointment exists in calendar
                                If ApptControl <> 0 Then
                                    'an appointment exists so validate the date and time
                                    blnReadAppointmentError = False
                                    Dim enmRet As BLL.NGLBookBLL.AMSDateTimeValidationResults = BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist
                                    Try
                                        strApptEventDateTime = ""
                                        enmRet = oBookBLL.validateAMSDateTimes(dtEvent, ApptControl, oSettings.DateTimeValidationType, strApptEventDateTime)
                                    Catch ex As System.ServiceModel.FaultException(Of Ngl.FreightMaster.Data.SqlFaultInfo)
                                        If ex.Detail.Message = "E_NoData" Then
                                            enmRet = BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist
                                        Else
                                            blnReadAppointmentError = True
                                            blnCreate214DateTimeValidationAlert = False
                                            blnUpdateLoadCarrierData = False
                                            create214LoadStatusUpdateFailureAlert(oBook.BookCustCompControl, oBook.BookCarrierControl, oBook.BookProNumber, oBook.BookSHID, oBook.BookCarrOrderNumber, oBook.BookOrderSequence, oBook.BookConsPrefix, o214Data.ShipCarrierProNumber, "Cannot Read Appointment Data: " & ex.Detail.Details, "", oSettings.strEventMsg & " not updated: " & strApptEventDateTime)
                                        End If
                                    Catch ex As Exception
                                        blnReadAppointmentError = True
                                        blnCreate214DateTimeValidationAlert = False
                                        blnUpdateLoadCarrierData = False
                                        create214LoadStatusUpdateFailureAlert(oBook.BookCustCompControl, oBook.BookCarrierControl, oBook.BookProNumber, oBook.BookSHID, oBook.BookCarrOrderNumber, oBook.BookOrderSequence, oBook.BookConsPrefix, o214Data.ShipCarrierProNumber, "Unexpected Error Cannot Update Load Status: " & ex.Message, "", oSettings.strEventMsg & " not updated: " & strApptEventDateTime)
                                    End Try
                                    If Not blnReadAppointmentError Then
                                        '1.	Compare Date and Time values: 
                                        'get the alert settings
                                        enmAlertType = getAlertTypeBySetting(oSettings, enmRet, strAlertComment)
                                        If enmRet = BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistAndMatches Then
                                            'a.	Date and Time values match calendar data: The System updates load status “Carrier started loading at pickup location”.
                                            strEventComment = strAlertComment & " " & strEventComment
                                        ElseIf enmRet = BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist Then
                                            'the order has an appt control but the actual appointment is missing.  this should not normally happen
                                            'but we test for the exception just in case
                                            'ii.	Appointment does not exist in calendar
                                            '  1.	The System creates a Load Status Message like: “Carrier started loading at pickup but an appointment was not created in the calendar”
                                            If enmAlertType = Alerts.None Then
                                                'if alerts is None we just add the message to the event comments 
                                                'in the load status message
                                                strEventComment = strAlertComment & " " & strEventComment
                                            Else
                                                'NOTE: if an alert is needed we add a seperate load status message
                                                'with a special load status code 21409 (see below)
                                                strDateTimeValidationAlertMessage &= strAlertComment
                                                '  3.	Generate subscription alert
                                                blnCreate214DateTimeValidationAlert = True
                                            End If
                                            '  2.	The system updates the Start Loading Date and Time on the Load Carrier Data Tab, [BookCarrStartLoadingDate] and [BookCarrStartLoadingTime].
                                            blnUpdateLoadCarrierData = True
                                        Else
                                            'b.	Date and Time values do not match calendar data: 
                                            '  i.The System updates load status with a message like: “Carrier start loading date and time is different from the Scheduler Calendar”.
                                            'set default alert type 
                                            If enmAlertType = Alerts.None Then
                                                'if alerts is None we just add the message to the event comments 
                                                'in the load status message
                                                strEventComment = strAlertComment & " " & strEventComment
                                            Else
                                                'NOTE: if an alert is needed we add a seperate load status message
                                                'with a special load status code 21409 (see below)
                                                strDateTimeValidationAlertMessage &= strAlertComment
                                                '  3.	Generate subscription alert
                                                blnCreate214DateTimeValidationAlert = True
                                            End If
                                            If enmRet = BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty Then
                                                'Set Flag to update the Start Loading Date and Time on the Load Carrier Data Tab, [BookCarrStartLoadingDate] and [BookCarrStartLoadingTime], with the information provided by the carrier.
                                                blnUpdateLoadCarrierData = True
                                            End If
                                        End If
                                    End If
                                    If enmAlertType = Alerts.None Then blnCreate214DateTimeValidationAlert = False
                                Else
                                    'ii.	Appointment does not exist in calendar
                                    '  1.	The System creates a Load Status Message “Carrier started loading at pickup but an appointment was not created in the calendar”
                                    enmAlertType = getAlertTypeBySetting(oSettings, BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptDoesNotExist, strAlertComment)
                                    If enmAlertType = Alerts.None Then
                                        'if alerts is None we just add the message to the event comments 
                                        'in the load status message
                                        strEventComment = strAlertComment & " " & strEventComment
                                    Else
                                        'NOTE: if an alert is needed we add a seperate load status message
                                        'with a special load status code 21409 (see below)
                                        strDateTimeValidationAlertMessage &= strAlertComment
                                        '  3.	Generate subscription alert
                                        blnCreate214DateTimeValidationAlert = True
                                    End If
                                    '  2.	The system updates the Start Loading Date and Time on the Load Carrier Data Tab, [BookCarrStartLoadingDate] and [BookCarrStartLoadingTime].
                                    blnUpdateLoadCarrierData = True
                                End If
                            Else
                                'b. Non-managed Facility:
                                '  i. The System creates a Load Status Message “Carrier started loading at pickup location”.
                                strEventComment = oSettings.strNonManagedMsg & " " & strEventComment
                                '  ii.	The system updates the Start Loading Date and Time on the Load Carrier Data Tab, [BookCarrStartLoadingDate] and [BookCarrStartLoadingTime]
                                blnUpdateLoadCarrierData = True
                            End If
                            If blnUpdateLoadCarrierData Then
                                updateLoadCarrierData(oSettings, oBook, o214StopData.EventDate, o214StopData.EventTime)
                            Else
                                'compare the carrier load date and time with the calendar date and time
                                strCarrierDateTime = getLoadCarrierDataDateString(oSettings, oBook)
                                If isDateStringDiff(strCarrierDateTime, strApptEventDateTime) Then
                                    enmAlertType = getAlertTypeBySetting(oSettings, BLL.NGLBookBLL.AMSDateTimeValidationResults.ApptExistAndMatches, strAlertComment, True)
                                    'if enmAlertType is not NONE we generate a seperate load status message
                                    '2.	Calendar date and time does not match Carrier Data Tab information:
                                    If enmAlertType = Alerts.None Then
                                        'if alerts is None we just add the message to the event comments 
                                        'in the load status message
                                        strEventComment = strAlertComment & " " & strEventComment
                                    Else
                                        'NOTE: if an alert is needed we add a seperate load status message
                                        'with a special load status code 21409 (see below)
                                        strDateTimeValidationAlertMessage &= strAlertComment
                                        '  3.	Generate subscription alert
                                        blnCreate214DateTimeValidationAlert = True
                                    End If
                                End If
                            End If
                            If blnCreate214DateTimeValidationAlert AndAlso enmAlertType <> Alerts.None Then
                                create214DateTimeValidationAlert(oBook.BookCustCompControl, oBook.BookCarrierControl, oBook.BookProNumber, oBook.BookSHID, oBook.BookCarrOrderNumber, oBook.BookOrderSequence, oBook.BookConsPrefix, strEventDateTime, enmAlertType, "", "", strEventComment)
                                'add the load status message
                                'Modified By LVV on 9/17/19 for Bing Maps
                                If Not addBookTracksToOrder(order, strDateTimeValidationAlertMessage, "", "EDI 214", o214Data.DefaultLoadStatusControl, Date.Now(), "", o214StopData.StatusDetails, "", strEventDateTime, "", Alert21409LoadStatusControl, False) Then
                                    create214LoadStatusUpdateFailureAlert(oBook.BookCustCompControl, oBook.BookCarrierControl, oBook.BookProNumber, oBook.BookSHID, oBook.BookCarrOrderNumber, oBook.BookOrderSequence, oBook.BookConsPrefix, o214Data.ShipCarrierProNumber, "", "", strErrMessage)
                                End If
                            End If
                        End If
                        'Modified By LVV on 9/17/19 for Bing Maps
                        If addBookTracksToOrder(order, o214StopData.LoadComments, o214StopData.EventComments, "EDI 214", o214Data.DefaultLoadStatusControl, Date.Now(), strErrMessage, o214StopData.StatusDetails, strEventComment, strEventDateTime, strEventDescription, intEventStatusControl) Then
                            blnRet = True
                        Else
                            create214LoadStatusUpdateFailureAlert(oBook.BookCustCompControl, oBook.BookCarrierControl, oBook.BookProNumber, oBook.BookSHID, oBook.BookCarrOrderNumber, oBook.BookOrderSequence, oBook.BookConsPrefix, o214Data.ShipCarrierProNumber, "", "", strErrMessage)
                        End If
                    End If
                End If
            Next
        End If
        Return blnRet
    End Function


    Friend Function createInvalid214CarrierProNumberAssignmentAlert(ByVal CompControl As Integer,
                                       ByVal CarrierControl As Integer,
                                       ByVal BookProNumber As String,
                                       ByVal BookSHID As String,
                                       ByVal OrderNumber As String,
                                       ByVal OrderSequence As String,
                                       ByVal BookConsPrefix As String,
                                       ByVal CarrierProNumber As String,
                                       Optional ByVal Errors As String = "",
                                       Optional ByVal Warnings As String = "",
                                       Optional ByVal Messages As String = "") As Boolean
        Dim Subject As String = String.Format("Alert - cannot update carrier pro, {2}, for OrderNumber - Sequence: {0}-{1}", OrderNumber, OrderSequence, CarrierProNumber)
        Dim Note1 As String = String.Format(" Book Pro Number: {0} Order Number - Sequence: {1}-{2} CNS: {3} SHID: {4} ", BookProNumber, OrderNumber, OrderSequence, BookConsPrefix, BookSHID)
        Dim Note2 As String = " Errors: " & Errors
        Dim Note3 As String = " Warnings: " & Warnings
        Dim Note4 As String = " Messages: " & Messages
        Dim Body As String = String.Concat("Alert - Cannot update carrier assigned pro number ", CarrierProNumber, ", because a pro number has automatically been generated by the system, for: ", vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the descriptiong will be displayed in the alert subscription selection screen.
        Return otblAlertMessageDAL.InsertAlertMessage("AlertInvalid214CarrierProNumberAssignment", "Alert cannot update carrier pro", Subject, Body, CompControl, 0, CarrierControl, Note1, Note2, Note3, Note4, "")
    End Function

    Friend Function create214LoadStatusUpdateFailureAlert(ByVal CompControl As Integer,
                                       ByVal CarrierControl As Integer,
                                       ByVal BookProNumber As String,
                                       ByVal BookSHID As String,
                                       ByVal OrderNumber As String,
                                       ByVal OrderSequence As String,
                                       ByVal BookConsPrefix As String,
                                       ByVal CarrierProNumber As String,
                                       Optional ByVal Errors As String = "",
                                       Optional ByVal Warnings As String = "",
                                       Optional ByVal Messages As String = "") As Boolean
        Dim Subject As String = String.Format("Alert - 214 update load status failure for OrderNumber - Sequence: {0}-{1}", OrderNumber, OrderSequence, CarrierProNumber)
        Dim Note1 As String = String.Format(" Book Pro Number: {0} Order Number - Sequence: {1}-{2} CNS: {3} SHID: {4} ", BookProNumber, OrderNumber, OrderSequence, BookConsPrefix, BookSHID)
        Dim Note2 As String = " Errors: " & Errors
        Dim Note3 As String = " Warnings: " & Warnings
        Dim Note4 As String = " Messages: " & Messages
        Dim Body As String = String.Concat("Alert -214 update load status failure for: ", vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the description will be displayed in the alert subscription selection screen.
        Return otblAlertMessageDAL.InsertAlertMessage("Alert214LoadStatusUpdateFailure", "Alert 214 update load status failure", Subject, Body, CompControl, 0, CarrierControl, Note1, Note2, Note3, Note4, "")
    End Function

    Friend Function create214DateTimeValidationAlert(ByVal CompControl As Integer,
                                       ByVal CarrierControl As Integer,
                                       ByVal BookProNumber As String,
                                       ByVal BookSHID As String,
                                       ByVal OrderNumber As String,
                                       ByVal OrderSequence As String,
                                       ByVal BookConsPrefix As String,
                                       ByVal strDateTime As String,
                                       ByVal enmAlertType As clsEDI214.Alerts,
                                       Optional ByVal Errors As String = "",
                                       Optional ByVal Warnings As String = "",
                                       Optional ByVal Messages As String = "") As Boolean
        Dim strName As String = ""
        Dim strDesc As String = ""
        Dim strBodyNote As String = ""

        populateAlertNameAndDescription(strName, strDesc, strBodyNote, enmAlertType)
        Dim Subject As String = String.Format("Alert - {0} for Order Number - Sequence: {1}-{2}", strDesc, OrderNumber, OrderSequence)
        Dim Note1 As String = String.Format(" Book Pro Number: {0} Order Number - Sequence: {1}-{2} CNS: {3} SHID: {4} Carrier Date and Time {5} ", BookProNumber, OrderNumber, OrderSequence, BookConsPrefix, BookSHID, strDateTime)
        Dim Note2 As String = " Errors: " & Errors
        Dim Note3 As String = " Warnings: " & Warnings
        Dim Note4 As String = " Messages: " & Messages
        Dim Body As String = String.Concat("Alert - ", strDesc, " for: ", vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the description will be displayed in the alert subscription selection screen.
        Return otblAlertMessageDAL.InsertAlertMessage(strName, strDesc, Subject, Body, CompControl, 0, CarrierControl, 0, Note1, Note2, Note3, Note4, "")
    End Function

    Private Sub populateAlertNameAndDescription(ByRef strName As String, ByRef strDesc As String, ByRef strBodyNote As String, ByVal enmAlertType As clsEDI214.Alerts)
        Select Case enmAlertType
            Case clsEDI214.Alerts.AlertPick2141ApptNotMatch
                strName = "AlertPick2141ApptNotMatch"
                strDesc = "214 Pickup Appt Does Not Match Schedule"
                strBodyNote = "Booking Pickup Appt Date & Time was not modified"
            Case clsEDI214.Alerts.AlertPick2141ApptMissing
                strName = "AlertPick2141ApptMissing"
                strDesc = "214 Pickup Appt is Missing in Schedule"
                strBodyNote = "Booking Pickup Appt Date & Time was updated"
            Case clsEDI214.Alerts.AlertPickBook1ApptNotMatch
                strName = "AlertPickBook1ApptNotMatch"
                strDesc = "Booked Pickup Appt Does Not Match Schedule"
                strBodyNote = "Booking Pickup Appt Date & Time was not modified"
            Case clsEDI214.Alerts.AlertPick2142CheckInMissingAppt
                strName = "AlertPick2142CheckInMissingAppt"
                strDesc = "214 Pickup Check In Appt is Missing in Schedule"
                strBodyNote = "Booking Pickup Check In Date & Time was modified"
            Case clsEDI214.Alerts.AlertPick2142CheckInMissingApptCheckIn
                strName = "AlertPick2142CheckInMissingApptCheckIn"
                strDesc = "214 Pickup Check In Date is Missing in Schedule"
                strBodyNote = "Booking Pickup Check In Date & Time was modified"
            Case clsEDI214.Alerts.AlertPick2142CheckInNotMatchAppt
                strName = "AlertPick2142CheckInNotMatchAppt"
                strDesc = "214 Pickup Check In Does Not Match Schedule"
                strBodyNote = "Booking Pickup Check In Date & Time was not modified"
            Case clsEDI214.Alerts.AlertPickBook2CheckInNotMatchAppt
                strName = "AlertPickBook2CheckInNotMatchAppt"
                strDesc = "Booked Pickup Check In Does Not Match Schedule"
                strBodyNote = "Booking Pickup Check In Date & Time was not modified"
            Case clsEDI214.Alerts.AlertPick2143StartMissingAppt
                strName = "AlertPick2143StartMissingAppt"
                strDesc = "214 Start Loading Appt is Missing in Schedule"
                strBodyNote = "Booking Start Loading Date & Time was modified"
            Case clsEDI214.Alerts.AlertPick2143StartMissingApptStart
                strName = "AlertPick2143StartMissingApptStart"
                strDesc = "214 Start Loading Date is Missing in Schedule"
                strBodyNote = "Booking Start Loading Date & Time was modified"
            Case clsEDI214.Alerts.AlertPick2143StartNotMatchAppt
                strName = "AlertPick2143StartNotMatchAppt"
                strDesc = "214 Start Loading Does Not Match Schedule"
                strBodyNote = "Booking Start Loading Date & Time was not modified"
            Case clsEDI214.Alerts.AlertPickBook3StartNotMatchAppt
                strName = "AlertPickBook3StartNotMatchAppt"
                strDesc = "Booked Start Loading Does Not Match Schedule"
                strBodyNote = "Booking Start Loading Date & Time was not modified"
            Case clsEDI214.Alerts.AlertPick2144FinishMissingAppt
                strName = "AlertPick2144FinishMissingAppt"
                strDesc = "214 Finish Loading Appt is Missing in Schedule"
                strBodyNote = "Booking Finished Loading Date & Time was updated"
            Case clsEDI214.Alerts.AlertPick2144FinishMissingApptFinish
                strName = "AlertPick2144FinishMissingApptFinish"
                strDesc = "214 Finish Loading Date is Missing in Schedule"
                strBodyNote = "Booking Finished Loading Date & Time was updated"
            Case clsEDI214.Alerts.AlertPick2144FinishNotMatchAppt
                strName = "AlertPick2144FinishNotMatchAppt"
                strDesc = "214 Finish Loading Does Not Match Schedule"
                strBodyNote = "Booking Finished Loading Date & Time was not modified"
            Case clsEDI214.Alerts.AlertPickBook4FinishNotMatchAppt
                strName = "AlertPickBook4FinishNotMatchAppt"
                strDesc = "Booked Finish Loading Does Not Match Schedule"
                strBodyNote = "Booking Finished Loading Date & Time was not modified"
            Case clsEDI214.Alerts.AlertPick2145DepartMissingAppt
                strName = "AlertPick2145DepartMissingAppt"
                strDesc = "214 Pickup Checkout Appt is Missing in Schedule"
                strBodyNote = "Booking Pickup Checkout Date & Time was updated"
            Case clsEDI214.Alerts.AlertPick2145DepartMissingApptCheckout
                strName = "AlertPick2145DepartMissingApptCheckout"
                strDesc = "214 Pickup Checkout Date is Missing in Schedule"
                strBodyNote = "Booking Pickup Checkout Date & Time was updated"
            Case clsEDI214.Alerts.AlertPick2145DepartNotMatchAppt
                strName = "AlertPick2145DepartNotMatchAppt"
                strDesc = "214 Pickup Checkout Does Not Match Schedule"
                strBodyNote = "Booking Pickup Checkout Date & Time was not modified"
            Case clsEDI214.Alerts.AlertPickBook5DepartNotMatchAppt
                strName = "AlertPickBook5DepartNotMatchAppt"
                strDesc = "Booked Pickup Checkout Does Not Match Schedule"
                strBodyNote = "Booking Pickup Checkout Date & Time was not modified"
            Case clsEDI214.Alerts.AlertDel2141ApptNotMatch
                strName = "AlertDel2141ApptNotMatch"
                strDesc = "214 Delivery Appt Does Not Match Schedule"
                strBodyNote = "Booking Delivery Appt Date & Time was not modified"
            Case clsEDI214.Alerts.AlertDel2141ApptMissing
                strName = "AlertDel2141ApptMissing"
                strDesc = "214 Delivery Appt is Missing in Schedule"
                strBodyNote = "Booking Delivery Appt Date & Time was updated"
            Case clsEDI214.Alerts.AlertDelBook1ApptNotMatch
                strName = "AlertDelBook1ApptNotMatch"
                strDesc = "Booked Delivery Appt Does Not Match Schedule"
                strBodyNote = "Booking Delivery Appt Date & Time was not modified"
            Case clsEDI214.Alerts.AlertDel2142CheckInMissingAppt
                strName = "AlertDel2142CheckInMissingAppt"
                strDesc = "214 Delivery Check In Appt is Missing in Schedule"
                strBodyNote = "Booking Delivery Check In Date & Time was updated"
            Case clsEDI214.Alerts.AlertDel2142CheckInMissingApptCheckIn
                strName = "AlertDel2142CheckInMissingApptCheckIn"
                strDesc = "214 Delivery Check In Date is Missing in Schedule"
                strBodyNote = "Booking Delivery Check In Date & Time was updated"
            Case clsEDI214.Alerts.AlertDel2142CheckInNotMatchAppt
                strName = "AlertDel2142CheckInNotMatchAppt"
                strDesc = "214 Delivery Check In Does Not Match Schedule"
                strBodyNote = "Booking Delivery Check In Date & Time was not modified"
            Case clsEDI214.Alerts.AlertDelBook2CheckInNotMatchAppt
                strName = "AlertDelBook2CheckInNotMatchAppt"
                strDesc = "Booked Delivery Check In Does Not Match Schedule"
                strBodyNote = "Booking Delivery Check In Date & Time was not modified"
            Case clsEDI214.Alerts.AlertDel2143StartMissingAppt
                strName = "AlertDel2143StartMissingAppt"
                strDesc = "214 Start Unloading Appt is Missing in Schedule"
                strBodyNote = "Booking Start Unloading Date & Time was updated"
            Case clsEDI214.Alerts.AlertDel2143StartMissingApptStart
                strName = "AlertDel2143StartMissingApptStart"
                strDesc = "214 Start Unloading Date is Missing in Schedule"
                strBodyNote = "Booking Start Unloading Date & Time was updated"
            Case clsEDI214.Alerts.AlertDel2143StartNotMatchAppt
                strName = "AlertDel2143StartNotMatchAppt"
                strDesc = "214 Start Unloading Does Not Match Schedule"
                strBodyNote = "Booking Start Unloading Date & Time was not modified"
            Case clsEDI214.Alerts.AlertDelBook3StartNotMatchAppt
                strName = "AlertDelBook3StartNotMatchAppt"
                strDesc = "Booked Start Unloading Does Not Match Schedule"
                strBodyNote = "Booking Start Unloading Date & Time was not modified"
            Case clsEDI214.Alerts.AlertDel2144FinishMissingAppt
                strName = "AlertDel2144FinishMissingAppt"
                strDesc = "214 Finish Unloading Appt is Missing in Schedule"
                strBodyNote = "Booking Finished Unloading Date & Time was updated"
            Case clsEDI214.Alerts.AlertDel2144FinishMissingApptFinish
                strName = "AlertDel2144FinishMissingApptFinish"
                strDesc = "214 Finish Unloading Date is Missing in Schedule"
                strBodyNote = "Booking Finished Unloading Date & Time was updated"
            Case clsEDI214.Alerts.AlertDel2144FinishNotMatchAppt
                strName = "AlertDel2144FinishNotMatchAppt"
                strDesc = "214 Finish Unloading Does Not Match Schedule"
                strBodyNote = "Booking Finished Unloading Date & Time was not modified"
            Case clsEDI214.Alerts.AlertDelBook4FinishNotMatchAppt
                strName = "AlertDelBook4FinishNotMatchAppt"
                strDesc = "Booked Finish Unloading Does Not Match Schedule"
                strBodyNote = "Booking Finished Unloading Date & Time was not modified"
            Case clsEDI214.Alerts.AlertDel2145DepartMissingAppt
                strName = "AlertDel2141ApptNotMatch"
                strDesc = "214 Delivery Checkout Appt is Missing in Schedule"
                strBodyNote = "Booking Delivery Checkout Date & Time was updated"
            Case clsEDI214.Alerts.AlertDel2145DepartMissingApptCheckout
                strName = "AlertDel2145DepartMissingApptCheckout"
                strDesc = "214 Delivery Checkout Date is Missing in Schedule"
                strBodyNote = "Booking Delivery Check Out Date & Time was updated"
            Case clsEDI214.Alerts.AlertDel2145DepartNotMatchAppt
                strName = "AlertDel2145DepartNotMatchAppt"
                strDesc = "214 Delivery Checkout Does Not Match Schedule"
                strBodyNote = "Booking Delivery Checkout Date & Time was not modified"
            Case clsEDI214.Alerts.AlertDelBook5DepartNotMatchAppt
                strName = "AlertDelBook5DepartNotMatchAppt"
                strDesc = "Booked Delivery Checkout Does Not Match Schedule"
                strBodyNote = "Booking Delivery Checkout Date & Time was not modified"
            Case Else
                strName = "Alert214Unexpected"
                strDesc = "Unexpected 214 Error"
                strBodyNote = ""
        End Select
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ISA"></param>
    ''' <param name="GS"></param>
    ''' <param name="strDBServer"></param>
    ''' <param name="strDatabase"></param>
    ''' <param name="strMSG"></param>
    ''' <param name="oCon"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 1/6/2015 v-7.0
    '''   Removed References to old RequireCNS logic
    '''   We now always use the BookSHID as the key value
    '''   mapped to segment B204 
    '''   Removed old code with comment tags no longer needed    ''' 
    ''' Modified by LVV 3/3/16 for v-7.0.5.1 EDI Migration
    ''' Added fileName functionality
    ''' </remarks>
    Public Function processDataOld(ByVal ISA As clsEDIISA,
                                ByVal GS As clsEDIGS,
                                ByVal strDBServer As String,
                                ByVal strDatabase As String,
                                ByRef strMSG As String,
                                ByRef oCon As System.Data.SqlClient.SqlConnection,
                                Optional ByVal fileName As String = "",
                                Optional ByVal DateProcessed As Date = Nothing,
                                Optional ByRef insertErrorMsg As String = "") As Boolean
        Dim blnRet As Boolean = True
        'Dim oCon As New System.Data.SqlClient.SqlConnection
        'Added by LVV 3/3/16 for v-7.0.5.1 EDI Migration
        Dim oWCFPar = New DAL.WCFParameters() With {.Database = strDatabase,
                                                             .DBServer = strDBServer,
                                                             .UserName = "EDI Integration",
                                                             .WCFAuthCode = "NGLSystem"}
        Dim oEDIData As New DAL.NGLEDIData(oWCFPar)
        Dim oCarrEDIData As New DAL.NGLCarrierEDIData(oWCFPar)

        strMSG = "Success!"

        Try
            'Added by LVV 3/3/16 for v-7.0.5.1 EDI Migration
            Dim dto214 As New DTO.tblEDI214
            Dim strBookCarrOrderNumber As String = ""
            Dim intBookOrderSequence As Integer = 0
            Dim strCarrierPartnerCode As String = ""
            Dim strCompPartnerCode As String = ""
            Dim dtBookTrackDate As Date = Now
            Dim strEventCode As String = ""
            Dim strEventDate As String = ""
            Dim strEventTime As String = ""
            Dim strBookShipCarrierProNumber As String = ""
            Dim strBookShipCarrierNumber As String = ""
            Dim strBookShipCarrierName As String = ""
            Dim strLoadComments As String = ""
            Dim strSHID As String = ""

            'the load comments are limited to 255 characters so any extra items are added to this list 
            Dim strExtraCommentsList As New List(Of String)
            'get the data from the isa record
            strCarrierPartnerCode = ISA.ISA06?.Trim
            strCompPartnerCode = ISA.ISA08?.Trim

            'Added by LVV 3/3/16 for v-7.0.5.1 EDI Migration
            Dim carrierName = oCarrEDIData.getCarrierNameByPartnerCode(strCarrierPartnerCode, "214")
            Dim compName As String = ""
            getCompInfoByCNS(oWCFPar, B10.B1002?.Trim, compName)

            If Not carrierName?.Trim.Length > 0 Then
                insertErrorMsg = "Could not look up Carrier Name using CarrierPartnerCode " + strCarrierPartnerCode + " and EDIXaction 214. "
            End If
            If Not compName?.Trim.Length > 0 Then
                insertErrorMsg += "Could not look up Company Name using CompPartnerCode " + strCompPartnerCode + " and EDIXaction 214. "
            End If

            'Get the Order Data
            strBookShipCarrierProNumber = B10.B1001?.Trim

            strSHID = B10.B1002?.Trim

            'Check for an update to the Assigned Carrier Information
            For Each L As clsEDI214Loop100 In Loop100
                If L.N1.N101 = "CA" Then
                    'this is a carrier update so get the name and number 
                    strBookShipCarrierNumber = L.N1.N104?.Trim
                    strBookShipCarrierName = L.N1.N102?.Trim
                End If
            Next
            If Loop200 Is Nothing OrElse Loop200.Count < 1 OrElse Loop200(0) Is Nothing Then
                strMSG = "Missing Order Details or invalid 200 Loop"
                'Added by LVV 3/3/16 for v-7.0.5.1 EDI Migration
                With dto214
                    .CarrierName = carrierName
                    .CompName = compName
                    .CarrierPartnerCode = strCarrierPartnerCode
                    .CompPartnerCode = strCompPartnerCode
                    .BookShipCarrierProNumber = strBookShipCarrierProNumber
                    .SHID = strSHID
                    .BookCarrOrderNumber = strBookCarrOrderNumber
                    .BookOrderSequence = intBookOrderSequence
                    .BookShipCarrierNumber = strBookShipCarrierNumber
                    .BookShipCarrierName = strBookShipCarrierName
                    .EDI214Message = strMSG + insertErrorMsg
                    .EDI214StatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI214ReceivedWithErrors
                    .EDI214FileName = fileName
                End With
                If Not oEDIData.InsertIntoEDI214(dto214, DateProcessed) Then
                    insertErrorMsg = "Could not insert record into tblEDI214 for CNS: " + dto214.BookConsPrefix + ", Order Number: " + dto214.BookCarrOrderNumber + ", DateProcessed, : " + DateProcessed.ToString + ", and filename: " + fileName
                End If

                blnRet = False
            Else
                'process the data in the 200 loop.  Updates require this information.
                For Each L As clsEDI214Loop200 In Loop200
                    If L Is Nothing Then Exit For
                    'Add any free form text messages sent by the carrier each 200 loop starts a new load comments message
                    strLoadComments = (" " & L.K1.K101?.Trim & L.K1.K102?.Trim)?.Trim
                    'Add the weight info if provided.
                    If L.AT8.AT803?.Trim.Length > 0 Then
                        strLoadComments &= " Weight: " & (L.AT8.AT803?.Trim & " " & L.AT8.AT802?.Trim & " " & L.AT8.AT801?.Trim)?.Trim
                    End If
                    'Add the pallet info if provided.
                    If L.AT8.AT804?.Trim.Length > 0 Then
                        strLoadComments &= " Pallets: " & L.AT8.AT804?.Trim
                    End If
                    'Add the quantity info if provided.
                    If L.AT8.AT805?.Trim.Length > 0 Then
                        strLoadComments &= " Quantity: " & L.AT8.AT805?.Trim
                    End If
                    'Add the volume info if provided.
                    If L.AT8.AT807?.Trim.Length > 0 Then
                        strLoadComments &= " Volume: " & (L.AT8.AT807?.Trim & " " & L.AT8.AT806?.Trim)?.Trim
                    End If
                    'process each 205 loop
                    For Each SL As clsEDI214Loop205 In L.Loop205
                        Dim EventCodeList As New List(Of String)
                        Dim dictEventComments As New Dictionary(Of String, String)

                        EventCodeList.Add(SL.AT7.AT701?.Trim)
                        dictEventComments.Add(SL.AT7.AT701?.Trim, " SHIPMENT STATUS CODE: " & SL.AT7.AT701?.Trim)

                        strEventDate = Ngl.Core.Utility.DataTransformation.convertEDIDateToDateString(SL.AT7.AT705)
                        strEventTime = Ngl.Core.Utility.DataTransformation.convertEDITimeToDateString(SL.AT7.AT706)
                        Dim strEventComments As String = ""
                        'Add the reason code info if provided.
                        If SL.AT7.AT702?.Trim.Length > 0 Then
                            EventCodeList.Add(SL.AT7.AT702?.Trim)
                            dictEventComments.Add(SL.AT7.AT702?.Trim, " SHIPMENT STATUS OR APPOINTMENT REASON CODE: " & SL.AT7.AT702?.Trim)
                        End If
                        'Add the appointment reason code info if provided.
                        If SL.AT7.AT704?.Trim.Length > 0 Then
                            EventCodeList.Add(SL.AT7.AT704?.Trim)
                            dictEventComments.Add(SL.AT7.AT704?.Trim, " SHIPMENT STATUS OR APPOINTMENT REASON CODE: " & SL.AT7.AT704?.Trim)
                        End If
                        'Add the type code info if provided.
                        If SL.AT7.AT703?.Trim.Length > 0 Then
                            EventCodeList.Add(SL.AT7.AT703?.Trim)
                            dictEventComments.Add(SL.AT7.AT703?.Trim, " SHIPMENT APPOINTMENT STATUS CODE: " & SL.AT7.AT703?.Trim)
                        End If

                        'Add the time zone code info if provided.
                        If SL.AT7.AT707?.Trim.Length > 0 Then
                            strEventComments &= " Time Zone Code: " & SL.AT7.AT707?.Trim
                        End If
                        'Add the City State and Country infor
                        If (SL.MS1.MS101?.Trim & " " & SL.MS1.MS102?.Trim & " " & SL.MS1.MS103?.Trim)?.Trim.Length > 0 Then
                            strEventComments &= " " & (SL.MS1.MS101?.Trim & " " & SL.MS1.MS102?.Trim & " " & SL.MS1.MS103?.Trim)?.Trim
                        End If
                        'Add any equipment codes
                        Dim strComma As String = ""
                        Dim strEq As String = " Eq: "
                        If SL.MS2.MS201?.Trim.Length > 0 Then
                            strEventComments &= strEq & SL.MS2.MS201?.Trim
                            strComma = ","
                            strEq = " "
                        End If
                        If SL.MS2.MS202?.Trim.Length > 0 Then
                            strEventComments &= strEq & strComma & SL.MS2.MS202?.Trim
                            strComma = ","
                            strEq = " "
                        End If
                        If SL.MS2.MS203?.Trim.Length > 0 Then
                            strEventComments &= strEq & strComma & SL.MS2.MS203?.Trim
                            strComma = ","
                            strEq = " "
                        End If
                        If SL.MS2.MS204?.Trim.Length > 0 Then
                            strEventComments &= strEq & strComma & SL.MS2.MS204?.Trim
                        End If
                        Dim oOrdeNumbers As New List(Of clsOrderNumberSeq)
                        Dim strSep As String = ""
                        For Each oL11 As clsEDIL11 In L.L11s
                            If oL11.L1102.ToUpper = "ON" Then
                                'this is an order number so add it to the collection for processing
                                'but we must split off the order sequence number
                                Dim sON() As String = oL11.L1101?.Trim.Split("-")
                                Dim oON As New clsOrderNumberSeq
                                strSep = ""
                                If sON.Length > 1 Then
                                    'We allow for extra "-"  in the order number like SO-0001-1
                                    For i As Integer = 0 To sON.Length - 1
                                        If i < sON.Length - 1 Then
                                            oON.BookCarrOrderNumber &= strSep & sON(i)
                                            strSep = "-"
                                        Else
                                            Integer.TryParse(sON(i)?.Trim, oON.BookOrderSequence)
                                        End If
                                    Next
                                Else
                                    'This should not happen but we test for a missing sequence number in case of a mix up
                                    'The default sequence number of zero is used
                                    oON.BookCarrOrderNumber = oL11.L1101?.Trim
                                End If
                                oOrdeNumbers.Add(oON)
                            End If
                        Next

                        'Now loop  through each order and update the load status
                        For Each oON As clsOrderNumberSeq In oOrdeNumbers
                            'Save data for each EventCode
                            For Each oEventCode As String In EventCodeList
                                Dim oEventComments = dictEventComments(oEventCode) & strEventComments

                                'Save the data to the database
                                If (strLoadComments & " " & oEventComments)?.Trim.Length > 255 Then
                                    'We send the loadcomments seperate from the event comments 
                                    'any data over 255 charactes for each will be truncated.
                                    'we send the data with just the event comments first

                                    blnRet = blnRet And save214Data(oON.BookCarrOrderNumber,
                                                   oON.BookOrderSequence,
                                                   strSHID,
                                                   strCarrierPartnerCode,
                                                   strCompPartnerCode,
                                                   dtBookTrackDate,
                                                   oEventCode,
                                                   strEventDate,
                                                   strEventTime,
                                                   strBookShipCarrierProNumber,
                                                   strBookShipCarrierNumber,
                                                   strBookShipCarrierName,
                                                   oEventComments?.Trim,
                                                   strDBServer,
                                                   strDatabase,
                                                   strMSG)

                                    'next we send the load comments
                                    blnRet = blnRet And save214Data(oON.BookCarrOrderNumber,
                                                   oON.BookOrderSequence,
                                                   strSHID,
                                                   strCarrierPartnerCode,
                                                   strCompPartnerCode,
                                                   dtBookTrackDate,
                                                   "",
                                                   "",
                                                   "",
                                                   "",
                                                   "",
                                                   "",
                                                   strLoadComments?.Trim,
                                                   strDBServer,
                                                   strDatabase,
                                                   strMSG)
                                Else
                                    'We can combine the load comments with the event comments.
                                    blnRet = blnRet And save214Data(oON.BookCarrOrderNumber,
                                                   oON.BookOrderSequence,
                                                   strSHID,
                                                   strCarrierPartnerCode,
                                                   strCompPartnerCode,
                                                   dtBookTrackDate,
                                                   oEventCode,
                                                   strEventDate,
                                                   strEventTime,
                                                   strBookShipCarrierProNumber,
                                                   strBookShipCarrierNumber,
                                                   strBookShipCarrierName,
                                                   (strLoadComments & " " & oEventComments)?.Trim,
                                                   strDBServer,
                                                   strDatabase,
                                                   strMSG)
                                End If
                                'Added by LVV 3/3/16 for v-7.0.5.1 EDI Migration
                                With dto214
                                    .BookCarrOrderNumber = oON.BookCarrOrderNumber
                                    .BookOrderSequence = oON.BookOrderSequence
                                    .SHID = strSHID
                                    .CarrierPartnerCode = strCarrierPartnerCode
                                    .CompPartnerCode = strCompPartnerCode
                                    .EventCode = strEventCode
                                    .EventDate = strEventDate
                                    .EventTime = strEventTime
                                    .BookShipCarrierProNumber = strBookShipCarrierProNumber
                                    .BookShipCarrierNumber = strBookShipCarrierNumber
                                    .BookShipCarrierName = strBookShipCarrierName
                                    .EventComments = (strLoadComments & " " & strEventComments)?.Trim
                                    .CarrierName = carrierName
                                    .CompName = compName
                                    .EDI214Message = strMSG + insertErrorMsg
                                    .EDI214StatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI214Received
                                    .EDI214FileName = fileName
                                End With
                                If Not oEDIData.InsertIntoEDI214(dto214, DateProcessed) Then
                                    insertErrorMsg = "Could not insert record into tblEDI214 for CNS: " + dto214.BookConsPrefix + ", Order Number: " + dto214.BookCarrOrderNumber + ", DateProcessed, : " + DateProcessed.ToString + ", and filename: " + fileName
                                End If

                            Next
                        Next
                    Next
                Next
            End If

            Return blnRet


            'Dim strToPrint As String = "*************** 214 *********************" & vbCrLf
            'strToPrint &= "ISA Sender: " & ISA.ISA06 & vbCrLf
            'strToPrint &= "GS Sender: " & GS.GS02 & vbCrLf
            'strToPrint &= "GS Date: " & GS.GS04 & vbCrLf
            'strToPrint &= "GS Time: " & GS.GS05 & vbCrLf
            'strToPrint &= "GS Group Control: " & GS.GS06 & vbCrLf
            'strToPrint &= "ST: " & ST.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'strToPrint &= "B10: " & B10.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'strToPrint &= "L11: " & L11.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'For Each L As clsEDI214Loop100 In Loop100
            '    strToPrint &= "N1: " & L.N1.getEDIString(ISA.SegmentTerminator) & vbCrLf
            '    strToPrint &= "N2: " & L.N2.getEDIString(ISA.SegmentTerminator) & vbCrLf
            '    strToPrint &= "N3: " & L.N3.getEDIString(ISA.SegmentTerminator) & vbCrLf
            '    strToPrint &= "N4: " & L.N4.getEDIString(ISA.SegmentTerminator) & vbCrLf
            '    strToPrint &= "G62: " & L.G62.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'Next
            'For Each L As clsEDI214Loop200 In Loop200
            '    strToPrint &= "LX: " & L.LX.getEDIString(ISA.SegmentTerminator) & vbCrLf
            '    For Each SL As clsEDI214Loop205 In L.Loop205
            '        strToPrint &= "AT7: " & SL.AT7.getEDIString(ISA.SegmentTerminator) & vbCrLf
            '        strToPrint &= "MS1: " & SL.MS1.getEDIString(ISA.SegmentTerminator) & vbCrLf
            '        strToPrint &= "MS2: " & SL.MS2.getEDIString(ISA.SegmentTerminator) & vbCrLf
            '    Next
            '    strToPrint &= "L11: " & L.L11.getEDIString(ISA.SegmentTerminator) & vbCrLf
            '    strToPrint &= "K1: " & L.K1.getEDIString(ISA.SegmentTerminator) & vbCrLf
            '    strToPrint &= "AT8: " & L.AT8.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'Next
            'strToPrint &= "SE: " & SE.getEDIString(ISA.SegmentTerminator) & vbCrLf
            'strToPrint &= "*****************************************" & vbCrLf
            'strToPrint &= vbCrLf

            'Dim FileName As String = "C:\Data\Mizkan\Test\Ruan214" & Date.Now.ToString("ddMMHHmmss") & ".txt"

            'Dim fi As FileInfo = New FileInfo(FileName)
            ''create the file if it does not exist
            'If Not File.Exists(FileName) Then
            '    Using w As StreamWriter = fi.CreateText
            '        w.Close()
            '    End Using
            'End If
            ''now open the file and read the data
            'Using sw As New StreamWriter(FileName)
            '    sw.Write(strToPrint)
            '    sw.Flush()
            'End Using
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    Public Function TestprocessData(ByVal ISA As clsEDIISA,
                                ByVal GS As clsEDIGS,
                                ByRef strMSG As String,
                                ByVal dblRequireCNS As Double) As Boolean
        Dim blnRet As Boolean = True
        'Dim oCon As New System.Data.SqlClient.SqlConnection
        strMSG = "Success!"
        Try
            Dim strBookCarrOrderNumber As String = ""
            Dim intBookOrderSequence As Integer = 0
            Dim strCarrierPartnerCode As String = ""
            Dim strCompPartnerCode As String = ""
            Dim dtBookTrackDate As Date = Now
            Dim strEventCode As String = ""
            Dim strEventDate As String = ""
            Dim strEventTime As String = ""
            Dim strBookShipCarrierProNumber As String = ""
            Dim strBookShipCarrierNumber As String = ""
            Dim strBookShipCarrierName As String = ""
            Dim strLoadComments As String = ""
            Dim strCNSNumber As String = ""

            'the load comments are limited to 255 characters so any extra items are added to this list 
            Dim strExtraCommentsList As New List(Of String)
            'get the data from the isa record
            strCarrierPartnerCode = ISA.ISA06?.Trim
            strCompPartnerCode = ISA.ISA08?.Trim
            'Get the Order Data
            strBookShipCarrierProNumber = B10.B1001?.Trim
            Dim sSplit() As String = B10.B1002?.Trim.Split("-")
            Dim strSeq As String = ""
            Dim strSep As String = ""
            If dblRequireCNS > 0 Then
                strCNSNumber = B10.B1002?.Trim
            Else
                If sSplit.Length > 1 Then
                    'We allow for extra "-"  in the order number like SO-0001-1
                    For i As Integer = 0 To sSplit.Length - 1
                        If i < sSplit.Length - 1 Then
                            strBookCarrOrderNumber &= strSep & sSplit(i)
                            strSep = "-"
                        Else
                            Integer.TryParse(sSplit(i)?.Trim, intBookOrderSequence)
                        End If
                    Next
                Else
                    'This should not happen but we test for a missing sequence number in case of a mix up
                    'The default sequence number of zero is used
                    strBookCarrOrderNumber = B10.B1002?.Trim
                End If
            End If
            'Check for an update to the Assigned Carrier Information
            For Each L As clsEDI214Loop100 In Loop100
                If L.N1.N101 = "CA" Then
                    'this is a carrier update so get the name and number 
                    strBookShipCarrierNumber = L.N1.N104?.Trim
                    strBookShipCarrierName = L.N1.N102?.Trim
                End If
            Next
            If Loop200 Is Nothing OrElse Loop200.Count < 1 OrElse Loop200(0) Is Nothing Then

                blnRet = False
            Else
                'process the data in the 200 loop.  Updates require this information.
                For Each L As clsEDI214Loop200 In Loop200
                    If L Is Nothing Then Exit For
                    'Add any free form text messages sent by the carrier each 200 loop starts a new load comments message
                    strLoadComments = (" " & L.K1.K101?.Trim & L.K1.K102?.Trim)?.Trim
                    'Add the weight info if provided.
                    If L.AT8.AT803?.Trim.Length > 0 Then
                        strLoadComments &= " Weight: " & (L.AT8.AT803?.Trim & " " & L.AT8.AT802?.Trim & " " & L.AT8.AT801?.Trim)?.Trim
                    End If
                    'Add the pallet info if provided.
                    If L.AT8.AT804?.Trim.Length > 0 Then
                        strLoadComments &= " Pallets: " & L.AT8.AT804?.Trim
                    End If
                    'Add the quantity info if provided.
                    If L.AT8.AT805?.Trim.Length > 0 Then
                        strLoadComments &= " Quantity: " & L.AT8.AT805?.Trim
                    End If
                    'Add the volume info if provided.
                    If L.AT8.AT807?.Trim.Length > 0 Then
                        strLoadComments &= " Volume: " & (L.AT8.AT807?.Trim & " " & L.AT8.AT806?.Trim)?.Trim
                    End If
                    'process each 205 loop
                    For Each SL As clsEDI214Loop205 In L.Loop205
                        strEventCode = SL.AT7.AT701?.Trim
                        strEventDate = Ngl.Core.Utility.DataTransformation.convertEDIDateToDateString(SL.AT7.AT705)
                        strEventTime = Ngl.Core.Utility.DataTransformation.convertEDITimeToDateString(SL.AT7.AT706)
                        Dim strEventComments As String = ""
                        'Add the reason code info if provided.
                        If SL.AT7.AT702?.Trim.Length > 0 Then
                            strEventComments &= " Reason Code: " & SL.AT7.AT702?.Trim
                        End If
                        'Add the appointment reason code info if provided.
                        If SL.AT7.AT704?.Trim.Length > 0 Then
                            strEventComments &= " Reason Code: " & SL.AT7.AT704?.Trim
                        End If
                        'Add the type code info if provided.
                        If SL.AT7.AT703?.Trim.Length > 0 Then
                            strEventCode = SL.AT7.AT703?.Trim
                        End If
                        strEventComments &= " Type Code: " & strEventCode
                        'Add the time zone code info if provided.
                        If SL.AT7.AT707?.Trim.Length > 0 Then
                            strEventComments &= " Time Zone Code: " & SL.AT7.AT707?.Trim
                        End If
                        'Add the City State and Country infor
                        If (SL.MS1.MS101?.Trim & " " & SL.MS1.MS102?.Trim & " " & SL.MS1.MS103?.Trim)?.Trim.Length > 0 Then
                            strEventComments &= " " & (SL.MS1.MS101?.Trim & " " & SL.MS1.MS102?.Trim & " " & SL.MS1.MS103?.Trim)?.Trim
                        End If
                        'Add any equipment codes
                        Dim strComma As String = ""
                        Dim strEq As String = " Eq: "
                        If SL.MS2.MS201?.Trim.Length > 0 Then
                            strEventComments &= strEq & SL.MS2.MS201?.Trim
                            strComma = ","
                            strEq = " "
                        End If
                        If SL.MS2.MS202?.Trim.Length > 0 Then
                            strEventComments &= strEq & strComma & SL.MS2.MS202?.Trim
                            strComma = ","
                            strEq = " "
                        End If
                        If SL.MS2.MS203?.Trim.Length > 0 Then
                            strEventComments &= strEq & strComma & SL.MS2.MS203?.Trim
                            strComma = ","
                            strEq = " "
                        End If
                        If SL.MS2.MS204?.Trim.Length > 0 Then
                            strEventComments &= strEq & strComma & SL.MS2.MS204?.Trim
                        End If
                        Dim oOrdeNumbers As New List(Of clsOrderNumberSeq)
                        If dblRequireCNS > 0 Then
                            For Each oL11 As clsEDIL11 In L.L11s
                                If oL11.L1102.ToUpper = "ON" Then
                                    'this is an order number so add it to the collection for processing
                                    'but we must split off the order sequence number
                                    Dim sON() As String = oL11.L1101?.Trim.Split("-")
                                    Dim oON As New clsOrderNumberSeq
                                    strSep = ""
                                    If sON.Length > 1 Then
                                        'We allow for extra "-"  in the order number like SO-0001-1
                                        For i As Integer = 0 To sON.Length - 1
                                            If i < sON.Length - 1 Then
                                                oON.BookCarrOrderNumber &= strSep & sON(i)
                                                strSep = "-"
                                            Else
                                                Integer.TryParse(sON(i)?.Trim, oON.BookOrderSequence)
                                            End If
                                        Next
                                    Else
                                        'This should not happen but we test for a missing sequence number in case of a mix up
                                        'The default sequence number of zero is used
                                        oON.BookCarrOrderNumber = oL11.L1101?.Trim
                                    End If
                                    oOrdeNumbers.Add(oON)
                                End If
                            Next
                        Else
                            Dim oON As New clsOrderNumberSeq
                            oON.BookCarrOrderNumber = strBookCarrOrderNumber
                            oON.BookOrderSequence = intBookOrderSequence
                            oOrdeNumbers.Add(oON)
                        End If
                        'Now loop  through each order and update the load status
                        For Each oON As clsOrderNumberSeq In oOrdeNumbers
                            strMSG = String.Join(";", New List(Of String) From {oON.BookCarrOrderNumber,
                                           oON.BookOrderSequence,
                                           strCarrierPartnerCode,
                                           strCompPartnerCode,
                                           dtBookTrackDate,
                                           strEventCode,
                                           strEventDate,
                                           strEventTime,
                                           strBookShipCarrierProNumber,
                                           strBookShipCarrierNumber,
                                           strBookShipCarrierName,
                                           strEventComments?.Trim})

                        Next
                    Next
                Next
            End If
            Return blnRet
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    Public Function processDataTest(ByVal ISA As clsEDIISA,
                                ByVal GS As clsEDIGS,
                                ByRef strMSG As String,
                                ByVal dblRequireCNS As Double) As String
        Dim strRet As String = "No Data"
        strMSG = "Success!"
        Try
            Dim strBookCarrOrderNumber As String = ""
            Dim intBookOrderSequence As Integer = 0
            Dim strCarrierPartnerCode As String = ""
            Dim strCompPartnerCode As String = ""
            Dim dtBookTrackDate As Date = Now
            Dim strEventCode As String = ""
            Dim strEventDate As String = ""
            Dim strEventTime As String = ""
            Dim strBookShipCarrierProNumber As String = ""
            Dim strBookShipCarrierNumber As String = ""
            Dim strBookShipCarrierName As String = ""
            Dim strLoadComments As String = ""
            Dim strCNSNumber As String = ""

            'the load comments are limited to 255 characters so any extra items are added to this list 
            Dim strExtraCommentsList As New List(Of String)
            'get the data from the isa record
            strCarrierPartnerCode = ISA.ISA06?.Trim
            strCompPartnerCode = ISA.ISA08?.Trim
            'Get the Order Data
            strBookShipCarrierProNumber = B10.B1001?.Trim
            Dim sSplit() As String = B10.B1002?.Trim.Split("-")
            Dim strSeq As String = ""
            Dim strSep As String = ""
            If dblRequireCNS > 0 Then
                strCNSNumber = B10.B1002?.Trim
            Else
                If sSplit.Length > 1 Then
                    'We allow for extra "-"  in the order number like SO-0001-1
                    For i As Integer = 0 To sSplit.Length - 1
                        If i < sSplit.Length - 1 Then
                            strBookCarrOrderNumber &= strSep & sSplit(i)
                            strSep = "-"
                        Else
                            Integer.TryParse(sSplit(i)?.Trim, intBookOrderSequence)
                        End If
                    Next
                Else
                    'This should not happen but we test for a missing sequence number in case of a mix up
                    'The default sequence number of zero is used
                    strBookCarrOrderNumber = B10.B1002?.Trim
                End If
            End If
            'Check for an update to the Assigned Carrier Information
            For Each L As clsEDI214Loop100 In Loop100
                If L.N1.N101 = "CA" Then
                    'this is a carrier update so get the name and number 
                    strBookShipCarrierNumber = L.N1.N104?.Trim
                    strBookShipCarrierName = L.N1.N102?.Trim
                End If
            Next
            If Loop200 Is Nothing OrElse Loop200.Count < 1 OrElse Loop200(0) Is Nothing Then

                strRet = "No 200 Loop"
            Else
                'process the data in the 200 loop.  Updates require this information.
                For Each L As clsEDI214Loop200 In Loop200
                    If L Is Nothing Then Exit For
                    'Add any free form text messages sent by the carrier each 200 loop starts a new load comments message
                    strLoadComments = (" " & L.K1.K101?.Trim & L.K1.K102?.Trim)?.Trim
                    'Add the weight info if provided.
                    If L.AT8.AT803?.Trim.Length > 0 Then
                        strLoadComments &= " Weight: " & (L.AT8.AT803?.Trim & " " & L.AT8.AT802?.Trim & " " & L.AT8.AT801?.Trim)?.Trim
                    End If
                    'Add the pallet info if provided.
                    If L.AT8.AT804?.Trim.Length > 0 Then
                        strLoadComments &= " Pallets: " & L.AT8.AT804?.Trim
                    End If
                    'Add the quantity info if provided.
                    If L.AT8.AT805?.Trim.Length > 0 Then
                        strLoadComments &= " Quantity: " & L.AT8.AT805?.Trim
                    End If
                    'Add the volume info if provided.
                    If L.AT8.AT807?.Trim.Length > 0 Then
                        strLoadComments &= " Volume: " & (L.AT8.AT807?.Trim & " " & L.AT8.AT806?.Trim)?.Trim
                    End If
                    'process each 205 loop
                    For Each SL As clsEDI214Loop205 In L.Loop205
                        strEventCode = SL.AT7.AT701?.Trim
                        strEventDate = Ngl.Core.Utility.DataTransformation.convertEDIDateToDateString(SL.AT7.AT705)
                        strEventTime = Ngl.Core.Utility.DataTransformation.convertEDITimeToDateString(SL.AT7.AT706)
                        Dim strEventComments As String = ""
                        'Add the reason code info if provided.
                        If SL.AT7.AT702?.Trim.Length > 0 Then
                            strEventComments &= " Reason Code: " & SL.AT7.AT702?.Trim
                        End If
                        'Add the appointment reason code info if provided.
                        If SL.AT7.AT704?.Trim.Length > 0 Then
                            strEventComments &= " Reason Code: " & SL.AT7.AT704?.Trim
                        End If
                        'Add the type code info if provided.
                        If SL.AT7.AT703?.Trim.Length > 0 Then
                            strEventCode = SL.AT7.AT703?.Trim
                        End If
                        strEventComments &= " Type Code: " & strEventCode
                        'Add the time zone code info if provided.
                        If SL.AT7.AT707?.Trim.Length > 0 Then
                            strEventComments &= " Time Zone Code: " & SL.AT7.AT707?.Trim
                        End If
                        'Add the City State and Country infor
                        If (SL.MS1.MS101?.Trim & " " & SL.MS1.MS102?.Trim & " " & SL.MS1.MS103?.Trim)?.Trim.Length > 0 Then
                            strEventComments &= " " & (SL.MS1.MS101?.Trim & " " & SL.MS1.MS102?.Trim & " " & SL.MS1.MS103?.Trim)?.Trim
                        End If
                        'Add any equipment codes
                        Dim strComma As String = ""
                        Dim strEq As String = " Eq: "
                        If SL.MS2.MS201?.Trim.Length > 0 Then
                            strEventComments &= strEq & SL.MS2.MS201?.Trim
                            strComma = ","
                            strEq = " "
                        End If
                        If SL.MS2.MS202?.Trim.Length > 0 Then
                            strEventComments &= strEq & strComma & SL.MS2.MS202?.Trim
                            strComma = ","
                            strEq = " "
                        End If
                        If SL.MS2.MS203?.Trim.Length > 0 Then
                            strEventComments &= strEq & strComma & SL.MS2.MS203?.Trim
                            strComma = ","
                            strEq = " "
                        End If
                        If SL.MS2.MS204?.Trim.Length > 0 Then
                            strEventComments &= strEq & strComma & SL.MS2.MS204?.Trim
                        End If
                        Dim oOrdeNumbers As New List(Of clsOrderNumberSeq)
                        If dblRequireCNS > 0 Then
                            For Each oL11 As clsEDIL11 In L.L11s
                                If oL11.L1102.ToUpper = "ON" Then
                                    'this is an order number so add it to the collection for processing
                                    'but we must split off the order sequence number
                                    Dim sON() As String = oL11.L1101?.Trim.Split("-")
                                    Dim oON As New clsOrderNumberSeq
                                    strSep = ""
                                    If sON.Length > 1 Then
                                        'We allow for extra "-"  in the order number like SO-0001-1
                                        For i As Integer = 0 To sON.Length - 1
                                            If i < sON.Length - 1 Then
                                                oON.BookCarrOrderNumber &= strSep & sON(i)
                                                strSep = "-"
                                            Else
                                                Integer.TryParse(sON(i)?.Trim, oON.BookOrderSequence)
                                            End If
                                        Next
                                    Else
                                        'This should not happen but we test for a missing sequence number in case of a mix up
                                        'The default sequence number of zero is used
                                        oON.BookCarrOrderNumber = oL11.L1101?.Trim
                                    End If
                                    oOrdeNumbers.Add(oON)
                                End If
                            Next
                        Else
                            Dim oON As New clsOrderNumberSeq
                            oON.BookCarrOrderNumber = strBookCarrOrderNumber
                            oON.BookOrderSequence = intBookOrderSequence
                            oOrdeNumbers.Add(oON)
                        End If
                        'Now loop  through each order and update the load status
                        For Each oON As clsOrderNumberSeq In oOrdeNumbers
                            'Save the data to the database
                            If (strLoadComments & " " & strEventComments)?.Trim.Length > 255 Then
                                'We send the loadcomments seperate from the event comments 
                                'any data over 255 charactes for each will be truncated.
                                'we send the data with just the event comments first
                                strRet = vbCrLf & vbCrLf & " | " & String.Join(", ", oON.BookCarrOrderNumber,
                                               oON.BookOrderSequence,
                                               strCarrierPartnerCode,
                                               strCompPartnerCode,
                                               dtBookTrackDate,
                                               strEventCode,
                                               strEventDate,
                                               strEventTime,
                                               strBookShipCarrierProNumber,
                                               strBookShipCarrierNumber,
                                               strBookShipCarrierName,
                                               strEventComments?.Trim,
                                               strMSG) & " | "

                                'next we send the load comments
                                strRet &= vbCrLf & vbCrLf & " | " & String.Join(", ", oON.BookCarrOrderNumber,
                                               oON.BookOrderSequence,
                                               strCarrierPartnerCode,
                                               strCompPartnerCode,
                                               dtBookTrackDate,
                                               "",
                                               "",
                                               "",
                                               "",
                                               "",
                                               "",
                                               strLoadComments?.Trim,
                                               strMSG) & " | "
                            Else
                                'We can combine the load comments with the event comments.
                                strRet = vbCrLf & vbCrLf & " | " & String.Join(", ", oON.BookCarrOrderNumber,
                                               oON.BookOrderSequence,
                                               strCarrierPartnerCode,
                                               strCompPartnerCode,
                                               dtBookTrackDate,
                                               strEventCode,
                                               strEventDate,
                                               strEventTime,
                                               strBookShipCarrierProNumber,
                                               strBookShipCarrierNumber,
                                               strBookShipCarrierName,
                                               (strLoadComments & " " & strEventComments)?.Trim,
                                               strMSG) & " | "
                            End If
                        Next
                    Next
                Next
            End If

            Return strRet


        Catch ex As Exception
            Throw
        End Try
        Return strRet
    End Function

    Public Function writeRecord(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS, Optional ByVal strFilePath As String = "C:\Data\EDI\Test\") As Boolean
        'For testing we just write the data to a file
        Try
            strFilePath = strFilePath?.Trim
            If Not Right(strFilePath, 1) = "\" Then strFilePath &= "\"
            If Not Directory.Exists(strFilePath) Then Directory.CreateDirectory(strFilePath)
            Dim FileName As String = strFilePath & "214Record" & Date.Now.ToString("ddMMHHmmss") & ".txt"

            Dim fi As FileInfo = New FileInfo(FileName)
            'create the file if it does not exist
            If Not File.Exists(FileName) Then
                Using w As StreamWriter = fi.CreateText
                    w.Close()
                End Using
            End If
            'now open the file and wite the data
            Using sw As New StreamWriter(FileName)
                sw.Write(getRecord(ISA, GS))
                sw.Flush()
            End Using
        Catch ex As Exception
            'do nothing for test
        End Try
        Return True
    End Function

    Public Function getEDIString(ByVal sSegTerm As String) As String
        Dim sEdi As New System.Text.StringBuilder("", 4000)
        sEdi.Append(ST.getEDIString(sSegTerm))
        sEdi.Append(B10.getEDIString(sSegTerm))
        sEdi.Append(L11.getEDIString(sSegTerm))
        If Not Loop100 Is Nothing AndAlso Loop100.Count() > 0 Then
            For Each L As clsEDI214Loop100 In Loop100
                If Not L Is Nothing Then
                    sEdi.Append(L.N1.getEDIString(sSegTerm))
                    sEdi.Append(L.N2.getEDIString(sSegTerm))
                    sEdi.Append(L.N3.getEDIString(sSegTerm))
                    sEdi.Append(L.N4.getEDIString(sSegTerm))
                    sEdi.Append(L.G62.getEDIString(sSegTerm))
                End If
            Next
        End If
        If Not Loop200 Is Nothing AndAlso Loop200.Count() > 0 Then
            For Each L As clsEDI214Loop200 In Loop200
                If Not L Is Nothing Then
                    sEdi.Append(L.LX.getEDIString(sSegTerm))
                    If Not L.Loop205 Is Nothing AndAlso L.Loop205.Count() > 0 Then
                        For Each SL As clsEDI214Loop205 In L.Loop205
                            If Not SL Is Nothing Then
                                sEdi.Append(SL.AT7.getEDIString(sSegTerm))
                                sEdi.Append(SL.MS1.getEDIString(sSegTerm))
                                sEdi.Append(SL.MS2.getEDIString(sSegTerm))
                            End If
                        Next
                    End If
                    sEdi.Append(L.L11.getEDIString(sSegTerm))
                    If Not L.L11s Is Nothing AndAlso L.L11s.Count > 0 Then
                        For Each oL11 As clsEDIL11 In L.L11s
                            If Not oL11 Is Nothing Then sEdi.Append(oL11.getEDIString(sSegTerm))
                        Next
                    End If
                    sEdi.Append(L.K1.getEDIString(sSegTerm))
                    sEdi.Append(L.AT8.getEDIString(sSegTerm))
                End If
            Next
        End If
        sEdi.Append(SE.getEDIString(sSegTerm))
        Return sEdi.ToString
    End Function

    Public Function getRecord(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS) As String
        'Format the record for reporting.
        Dim strToPrint As String = "*************** 214 *********************" & vbCrLf
        strToPrint &= "ISA Sender: " & ISA.ISA06 & vbCrLf
        strToPrint &= "GS Sender: " & GS.GS02 & vbCrLf
        strToPrint &= "GS Date: " & GS.GS04 & vbCrLf
        strToPrint &= "GS Time: " & GS.GS05 & vbCrLf
        strToPrint &= "GS Group Control: " & GS.GS06 & vbCrLf
        strToPrint &= "ST: " & ST.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "B10: " & B10.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "L11: " & L11.getEDIString(ISA.SegmentTerminator) & vbCrLf
        For Each L As clsEDI214Loop100 In Loop100
            If L Is Nothing Then
                strToPrint &= "**** Warning no 100 Loop (N1, N2, N3, N4, and G62 segments could not be processed) *****" & vbCrLf
            Else
                strToPrint &= "N1: " & L.N1.getEDIString(ISA.SegmentTerminator) & vbCrLf
                strToPrint &= "N2: " & L.N2.getEDIString(ISA.SegmentTerminator) & vbCrLf
                strToPrint &= "N3: " & L.N3.getEDIString(ISA.SegmentTerminator) & vbCrLf
                strToPrint &= "N4: " & L.N4.getEDIString(ISA.SegmentTerminator) & vbCrLf
                strToPrint &= "G62: " & L.G62.getEDIString(ISA.SegmentTerminator) & vbCrLf
            End If
        Next
        For Each L As clsEDI214Loop200 In Loop200
            If L Is Nothing Then
                strToPrint &= "**** Warning no 200 Loop (LX, AT7, MS1, MS2, L11, K1 and AT8 segments could not be processed) *****" & vbCrLf
            Else


                strToPrint &= "LX: " & L.LX.getEDIString(ISA.SegmentTerminator) & vbCrLf
                For Each SL As clsEDI214Loop205 In L.Loop205
                    strToPrint &= "AT7: " & SL.AT7.getEDIString(ISA.SegmentTerminator) & vbCrLf
                    strToPrint &= "MS1: " & SL.MS1.getEDIString(ISA.SegmentTerminator) & vbCrLf
                    strToPrint &= "MS2: " & SL.MS2.getEDIString(ISA.SegmentTerminator) & vbCrLf
                Next
                strToPrint &= "L11: " & L.L11.getEDIString(ISA.SegmentTerminator) & vbCrLf
                If Not L.L11s Is Nothing AndAlso L.L11s.Count > 0 Then
                    For Each oL11 As clsEDIL11 In L.L11s
                        strToPrint &= "L11: " & oL11.getEDIString(ISA.SegmentTerminator) & vbCrLf
                    Next
                End If
                strToPrint &= "K1: " & L.K1.getEDIString(ISA.SegmentTerminator) & vbCrLf
                strToPrint &= "AT8: " & L.AT8.getEDIString(ISA.SegmentTerminator) & vbCrLf
            End If

        Next
        strToPrint &= "SE: " & SE.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "*****************************************" & vbCrLf
        strToPrint &= vbCrLf
        Return strToPrint
    End Function

    ''' <summary>
    ''' Creates an EDI 214 document string
    ''' </summary>
    ''' <param name="strISA"></param>
    ''' <param name="strGS"></param>
    ''' <param name="strIEA"></param>
    ''' <param name="strGE"></param>
    ''' <param name="SegmentTerminator"></param>
    ''' <returns>String</returns>
    ''' <remarks>
    ''' Added by LVV on 4/11/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Function getEDIRecord(ByVal strISA As String, ByVal strGS As String, ByVal strIEA As String, ByVal strGE As String, ByVal SegmentTerminator As String) As String
        'Format the edi record 
        Dim strToPrint As String = ""
        strToPrint &= strISA
        strToPrint &= strGS
        strToPrint &= ST.getEDIString(SegmentTerminator)
        strToPrint &= B10.getEDIString(SegmentTerminator)
        strToPrint &= L11.getEDIString(SegmentTerminator)
        For Each L As clsEDI214Loop100 In Loop100
            If L Is Nothing Then
                strToPrint &= "**** Warning no 100 Loop (N1, N2, N3, N4, and G62 segments could not be processed) *****" & vbCrLf
            Else
                strToPrint &= L.N1.getEDIString(SegmentTerminator)
                strToPrint &= L.N2.getEDIString(SegmentTerminator)
                strToPrint &= L.N3.getEDIString(SegmentTerminator)
                strToPrint &= L.N4.getEDIString(SegmentTerminator)
                strToPrint &= L.G62.getEDIString(SegmentTerminator)
            End If
        Next
        For Each L As clsEDI214Loop200 In Loop200
            If L Is Nothing Then
                strToPrint &= "**** Warning no 200 Loop (LX, AT7, MS1, MS2, L11, K1 and AT8 segments could not be processed) *****" & vbCrLf
            Else

                strToPrint &= L.LX.getEDIString(SegmentTerminator)
                For Each SL As clsEDI214Loop205 In L.Loop205
                    strToPrint &= SL.AT7.getEDIString(SegmentTerminator)
                    strToPrint &= SL.MS1.getEDIString(SegmentTerminator)
                    strToPrint &= SL.MS2.getEDIString(SegmentTerminator)
                Next
                strToPrint &= L.L11.getEDIString(SegmentTerminator)
                If Not L.L11s Is Nothing AndAlso L.L11s.Count > 0 Then
                    For Each oL11 As clsEDIL11 In L.L11s
                        strToPrint &= oL11.getEDIString(SegmentTerminator)
                    Next
                End If
                strToPrint &= L.K1.getEDIString(SegmentTerminator)
                strToPrint &= L.AT8.getEDIString(SegmentTerminator)
            End If

        Next
        strToPrint &= SE.getEDIString(SegmentTerminator)
        strToPrint &= strGE
        strToPrint &= strIEA
        Return strToPrint
    End Function

    Private Function save214Data(ByVal strBookCarrOrderNumber As String,
                                ByVal intBookOrderSequence As Integer,
                                ByVal strBookSHID As String,
                                ByVal strCarrierPartnerCode As String,
                                ByVal strCompPartnerCode As String,
                                ByVal dtBookTrackDate As Date,
                                ByVal strEventCode As String,
                                ByVal strEventDate As String,
                                ByVal strEventTime As String,
                                ByVal strBookShipCarrierProNumber As String,
                                ByVal strBookShipCarrierNumber As String,
                                ByVal strBookShipCarrierName As String,
                                ByVal strLoadComments As String,
                                ByVal strDBServer As String,
                                ByVal strDatabase As String,
                                ByRef strMSG As String) As Boolean
        Dim oCon As New System.Data.SqlClient.SqlConnection
        strMSG = "Success!"
        Try
            Dim oQuery As New Ngl.Core.Data.Query(strDBServer, strDatabase)
            'Just send the update to the database
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookCarrOrderNumber", Left(strBookCarrOrderNumber, 20))
            oCmd.Parameters.AddWithValue("@BookOrderSequence", intBookOrderSequence)
            oCmd.Parameters.AddWithValue("@BookSHID", strBookSHID)
            oCmd.Parameters.AddWithValue("@CarrierPartnerCode", Left(strCarrierPartnerCode, 15))
            oCmd.Parameters.AddWithValue("@CompPartnerCode", Left(strCompPartnerCode, 15))
            oCmd.Parameters.AddWithValue("@BookTrackDate", dtBookTrackDate)
            If strEventCode?.Trim.Length > 0 Then oCmd.Parameters.AddWithValue("@EventCode", Left(strEventCode, 2))
            If strEventDate?.Trim.Length > 0 Then oCmd.Parameters.AddWithValue("@EventDate", Left(strEventDate, 10))
            If strEventTime?.Trim.Length > 0 Then oCmd.Parameters.AddWithValue("@EventTime", Left(strEventTime, 10))
            If strBookShipCarrierProNumber?.Trim.Length > 0 Then oCmd.Parameters.AddWithValue("@BookShipCarrierProNumber", Left(strBookShipCarrierProNumber, 20))
            If strBookShipCarrierNumber?.Trim.Length > 0 Then oCmd.Parameters.AddWithValue("@BookShipCarrierNumber", Left(strBookShipCarrierNumber, 80))
            If strBookShipCarrierName?.Trim.Length > 0 Then oCmd.Parameters.AddWithValue("@BookShipCarrierName", Left(strBookShipCarrierName, 60))
            If (strLoadComments)?.Trim.Length > 0 Then oCmd.Parameters.AddWithValue("@LoadComments", Left(strLoadComments, 255))
            Return oQuery.execNGLStoredProcedure(oCon, oCmd, "dbo.spUpdate214StatusEDI", 3)
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            strMSG = "Failed to update load status information: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseLogInException
            strMSG = "Database login failure: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseInvalidException
            strMSG = "Database access failure : " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseDataValidationException
            strMSG = ex.Message
            Return False
        Catch ex As Exception
            Throw
            Return False
        End Try
    End Function

    Public LastError As String = ""
    Public ST As New clsEDIST
    Public B10 As New clsEDIB10
    Public L11 As New clsEDIL11
    Public Loop100() As clsEDI214Loop100
    Public Loop200() As clsEDI214Loop200
    Public SE As New clsEDISE

    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
        'add the L11 to ST elements if they exist
        For isubsegs As Integer = 2 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the ST record (it should be all that is left 
                    If Left(sElems(0), 3) <> "ST*" Then sElems(0) = "ST*" & sElems(0)
                    ST = New clsEDIST(sElems(0))
                Case 1
                    'read the B10 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "B10\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        B10 = New clsEDIB10("B10*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 2
                    'read the L11 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "L11\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        L11 = New clsEDIL11("L11*" & sElems(0))
                        strSource = segs(0)
                    End If
            End Select
        Next
        'Add the SE
        If Not String.IsNullOrEmpty(strSEElem) Then
            'split out any unwanted elements 
            Dim sElems() As String = strSEElem.Split(strSegSep)
            If Left(sElems(0), 3) <> "SE*" Then sElems(0) = "SE*" & sElems(0)
            SE = New clsEDISE(sElems(0))
        End If
    End Sub

    ''' <summary>
    ''' Gets the Company Name and Number by CNS or SHID
    ''' </summary>
    ''' <param name="oWCFPar"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <param name="CompName"></param>
    ''' <remarks>
    ''' Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' Modified by RHR for v-8.2.1 on 10/11/2019
    '''   The parameter BookConsPrefix was changed to BookConsPrefixOrSHID to suppport 
    '''   new functionality where SHID is sent via EDI not CNS, and the SHID can be 
    '''   The carrier pro number.  The function oBook.GetBooksFilteredNoChildren was 
    '''   also modified, but the parameter name was not changed,  to first look up using 
    '''   SHID and second to lookup using CNS if the SHID does not match.
    '''
    ''' </remarks>
    Public Sub getCompInfoByCNS(ByVal oWCFPar As DAL.WCFParameters, ByVal BookConsPrefix As String, ByRef CompName As String)
        Dim oBook As New DAL.NGLBookData(oWCFPar)
        Dim oCarrier As New DAL.NGLCarrierData(oWCFPar)
        Dim oComp As New DAL.NGLCompData(oWCFPar)
        Dim blnMultiple As Boolean = False

        'Use the CNS to get the booking records
        Dim books = oBook.GetBooksFilteredNoChildren(BookConsPrefix:=BookConsPrefix)

        If books.Length > 0 Then
            Dim compControl = books(0).BookCustCompControl
            For Each b In books
                'Check to see if all orders on the CNS have the same Company
                If Not b.BookCustCompControl = compControl Then
                    blnMultiple = True
                    Exit For
                End If
            Next

            If blnMultiple Then
                CompName = "Multiple"
            Else
                Dim comp = oComp.GetCompFiltered(Control:=compControl)
                CompName = comp.CompName
            End If

        End If

    End Sub


End Class

#End Region

#Region "210 N9 Loop"

Public Class clsEDI210N9Loop

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        addDataFromString(strSource, strSegSep)

    End Sub

    Public N9 As New clsEDIN9

    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        'split out any unwanted elements 
        Dim sElems() As String = strSource.Split(strSegSep)
        'read the N9 record (it should be all that is left)
        If Left(sElems(0), 3) <> "N9*" Then sElems(0) = "N9*" & sElems(0)
        N9 = New clsEDIN9(sElems(0))
    End Sub
End Class

#End Region

#Region "210 Loop 100"

Public Class clsEDI210Loop100

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        addDataFromString(strSource, strSegSep)

    End Sub

    Public N1 As New clsEDIN1
    Public N2 As New clsEDIN2
    Public N3 As New clsEDIN3
    Public N4 As New clsEDIN4

    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        For isubsegs As Integer = 3 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the N1 record (it should be all that is left
                    If Left(sElems(0), 3) <> "N1*" Then sElems(0) = "N1*" & sElems(0)
                    N1 = New clsEDIN1(sElems(0))
                Case 1
                    'read the N2 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "N2\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        N2 = New clsEDIN2("N2*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 2
                    'read the N3 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "N3\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        N3 = New clsEDIN3("N3*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 3
                    'read the N4 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "N4\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        N4 = New clsEDIN4("N4*" & sElems(0))
                        strSource = segs(0)
                    End If
            End Select
        Next
    End Sub

End Class

#End Region

#Region "210 Loop 200"

Public Class clsEDI210Loop200

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        addDataFromString(strSource, strSegSep)

    End Sub

    Public N7 As New clsEDIN7

    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        'split out any unwanted elements 
        Dim sElems() As String = strSource.Split(strSegSep)
        'read the N7 record (it should be all that is left)
        If Left(sElems(0), 3) <> "N7*" Then sElems(0) = "N7*" & sElems(0)
        N7 = New clsEDIN7(sElems(0))
    End Sub
End Class

#End Region

#Region "210 Loop 400"

Public Class clsEDI210Loop400

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        addDataFromString(strSource, strSegSep)

    End Sub

    Public LX As New clsEDILX
    Public L5 As New clsEDIL5
    Public L0 As New clsEDIL0
    'Public L1 As New clsEDIL1
    '********Merge from 6.0.4.70*************
    Public _L1 As clsEDIL1
    Public Property L1() As clsEDIL1
        Get
            If Not L1s Is Nothing AndAlso L1s.Count() > 0 Then
                Return L1s(0)
            Else
                Return New clsEDIL1()
            End If
        End Get
        Set(ByVal value As clsEDIL1)
            If L1s Is Nothing Then L1s = New List(Of clsEDIL1)
            If L1s.Count > 0 Then
                L1s(0) = value
            Else
                L1s.Add(value)
            End If
        End Set
    End Property

    Public L1s As New List(Of clsEDIL1)
    '********Merge from 6.0.4.70*************

    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        For isubsegs As Integer = 3 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the LX record (it should be all that is left)
                    If Left(sElems(0), 3) <> "LX*" Then sElems(0) = "LX*" & sElems(0)
                    LX = New clsEDILX(sElems(0))
                Case 1
                    'read the L5 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "L5\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        L5 = New clsEDIL5("L5*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 2
                    'read the L0 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "L0\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        L0 = New clsEDIL0("L0*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 3
                    'read the L1 record

                    '********Merge from 6.0.4.70*************
                    'Modified by RHR for v-6.0.4.107 on 10/11/2017
                    segs = Regex.Split(strSource, "\" & strSegSep & "L1\*")
                    If segs.Length > 1 Then
                        'loop through each L1 segment and add it to the list
                        Dim iL1 As Integer = 1
                        For iL1 = 1 To (segs.Count() - 1)
                            Dim sElems() As String = segs(iL1).Split(strSegSep)
                            'L1 = New clsEDIL1("L1*" & sElems(0))
                            Me.L1s.Add(New clsEDIL1("L1*" & sElems(0)))
                        Next
                        'Dim sElems() As String = segs(1).Split(strSegSep)
                        'L1 = New clsEDIL1("L1*" & sElems(0))
                        strSource = segs(0)
                    End If
                    '********Merge from 6.0.4.70*************

                    ''segs = Regex.Split(strSource, "\" & strSegSep & "L1\*")
                    ''If segs.Length > 1 Then
                    ''    'split out any unwanted elements 
                    ''    Dim sElems() As String = segs(1).Split(strSegSep)
                    ''    L1 = New clsEDIL1("L1*" & sElems(0))
                    ''    strSource = segs(0)
                    ''End If
            End Select
        Next
    End Sub

End Class

#End Region

#Region " EDI 210 Class"

Public Class clsEDI210

    '********Merge from 6.0.4.70*************
    Private _EDI210InSetting As clsEDI210InSetting
    Public Property EDI210InSetting() As clsEDI210InSetting
        Get
            Return _EDI210InSetting
        End Get
        Set(ByVal value As clsEDI210InSetting)
            _EDI210InSetting = value
        End Set
    End Property
    '********Merge from 6.0.4.70*************

    Public Sub New()
        MyBase.New()

    End Sub

    Public Sub New(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
        MyBase.New()
        addDataFromString(strSource, strSegSep, strSEElem)
    End Sub

    ''' <summary>
    ''' Deprecated overload left to support backward compatibility with older methods.  
    ''' New methods should use the overload without the dblRequireCNS parameter as it is no longer used.
    ''' </summary>
    ''' <param name="ISA"></param>
    ''' <param name="GS"></param>
    ''' <param name="strDBServer"></param>
    ''' <param name="strDatabase"></param>
    ''' <param name="strAdminEmail"></param>
    ''' <param name="strGroupEmail"></param>
    ''' <param name="strSMTP"></param>
    ''' <param name="strFromEmail"></param>
    ''' <param name="strMSG"></param>
    ''' <param name="dblRequireCNS"></param>
    ''' <param name="oCon"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function processData(ByVal ISA As clsEDIISA,
                               ByVal GS As clsEDIGS,
                               ByVal strDBServer As String,
                               ByVal strDatabase As String,
                               ByVal strAdminEmail As String,
                               ByVal strGroupEmail As String,
                               ByVal strSMTP As String,
                               ByVal strFromEmail As String,
                               ByRef strMSG As String,
                               ByVal dblRequireCNS As Double,
                               ByRef oCon As System.Data.SqlClient.SqlConnection) As Boolean
        Return processData(ISA, GS, strDBServer, strDatabase, strAdminEmail, strGroupEmail, strSMTP, strFromEmail, strMSG, oCon)

    End Function

    '********Merge from 6.0.4.70*************
    Private Function readtblDataEntryFields(ByRef oQuery As Ngl.Core.Data.Query) As Dictionary(Of String, String)
        Dim dRet As New Dictionary(Of String, String)
        Dim strSQL As String = "Select DEFieldName,DEFieldMapCode from dbo.tblDataEntryFields Where DEFileType = 1 and Len(LTrim(isnull(DEFieldMapCode,''))) > 0 "
        Try
            Dim oQR As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQR.Exception Is Nothing Then Return dRet

            Dim dt As System.Data.DataTable = oQR.Data
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In dt.Rows
                    Dim strMapCode As String = NDT.getDataRowString(oRow, "DEFieldMapCode", "")
                    If Not String.IsNullOrEmpty(strMapCode) Then
                        dRet.Add(strMapCode, NDT.getDataRowString(oRow, "DEFieldName", ""))
                    End If
                Next
            End If
        Catch ex As Exception
            'do nothing on error
        End Try

        Return dRet
    End Function

    Private Function readL1AccessorialFee(ByRef drFreightBill As FreightBillData.FreightBillRow, ByRef dEDICodes As Dictionary(Of String, String), ByVal oL1 As clsEDIL1, ByVal o400 As clsEDI210Loop400) As ediOrderSpecificFee
        Dim oRet As ediOrderSpecificFee
        Dim dblCost As Double = 0
        Double.TryParse(Ngl.Core.Utility.DataTransformation.FormatEDICurrencyToDouble(oL1.L104), dblCost)
        Dim strCode As String = oL1.L108.Trim
        Dim blnAddToOtherCost As Boolean = True
        If strCode.Length > 0 Then
            If strCode = "400" Then
                drFreightBill.APCarrierCost = drFreightBill.APCarrierCost + dblCost
                blnAddToOtherCost = False
            Else
                'Note: in v-7.x and later we need to use the SHID instead of the CNS number.
                'oRet = New ediOrderSpecificFee(drFreightBill.APBillNumber, drFreightBill.APCarrierNumber, drFreightBill.APCNSNumber)
                oRet = New ediOrderSpecificFee(drFreightBill.APBillNumber, drFreightBill.APCarrierNumber, drFreightBill.APSHID)
                oRet.EDICode = strCode
                oRet.FeeValue = dblCost

                If (Not String.IsNullOrEmpty(oL1.L111) AndAlso oL1.L111.ToUpper() = "ON") Then
                    oRet.BookCarrOrderNumber = oL1.L112
                Else
                    'Modified by RHR for v-6.0.4.7 on 10/20/2017 
                    oRet.StopSequence = EDI210InSetting.MappingRules.getAdjustedStopSequence(oL1.L112)
                End If

                Dim strFieldName As String = ""
                If dEDICodes.ContainsKey(strCode) Then
                    strFieldName = dEDICodes(strCode)
                End If

                If (Not o400.L5 Is Nothing AndAlso Not String.IsNullOrEmpty(o400.L5.L502)) Then
                    oRet.EDIDescription = o400.L5.L502
                End If

                If Not String.IsNullOrWhiteSpace(strFieldName) Then
                    Select Case strFieldName.ToUpper
                        Case "APFEE1"
                            drFreightBill.APFee1 = drFreightBill.APFee1 + dblCost
                            blnAddToOtherCost = False
                        Case "APFEE2"
                            drFreightBill.APFee2 = drFreightBill.APFee2 + dblCost
                            blnAddToOtherCost = False
                        Case "APFEE3"
                            drFreightBill.APFee3 = drFreightBill.APFee3 + dblCost
                            blnAddToOtherCost = False
                        Case "APFEE4"
                            drFreightBill.APFee4 = drFreightBill.APFee4 + dblCost
                            blnAddToOtherCost = False
                        Case "APFEE5"
                            drFreightBill.APFee5 = drFreightBill.APFee5 + dblCost
                            blnAddToOtherCost = False
                        Case "APFEE6"
                            drFreightBill.APFee6 = drFreightBill.APFee6 + dblCost
                            blnAddToOtherCost = False
                        Case Else
                            blnAddToOtherCost = True
                    End Select
                Else
                    blnAddToOtherCost = True
                End If

            End If
        Else
            'just add the cost without an accessorial code?
            'oRet = New ediOrderSpecificFee(drFreightBill.APBillNumber, drFreightBill.APCarrierNumber, drFreightBill.APCNSNumber)
            oRet = New ediOrderSpecificFee(drFreightBill.APBillNumber, drFreightBill.APCarrierNumber, drFreightBill.APSHID)
                oRet.EDICode = "MSC"
                oRet.FeeValue = dblCost
            If (Not String.IsNullOrEmpty(oL1.L111) AndAlso oL1.L111.ToUpper() = "ON") Then
                oRet.BookCarrOrderNumber = oL1.L112
            Else
                'Modified by RHR for v-6.0.4.7 on 10/20/2017 
                oRet.StopSequence = EDI210InSetting.MappingRules.getAdjustedStopSequence(oL1.L112)
            End If
            If (Not o400.L5 Is Nothing AndAlso Not String.IsNullOrEmpty(o400.L5.L502)) Then
                oRet.EDIDescription = o400.L5.L502
            End If

        End If
            If blnAddToOtherCost Then
                drFreightBill.APOtherCost += dblCost
            End If
#Disable Warning BC42104 ' Variable 'oRet' is used before it has been assigned a value. A null reference exception could result at runtime.
            Return oRet
#Enable Warning BC42104 ' Variable 'oRet' is used before it has been assigned a value. A null reference exception could result at runtime.

    End Function
    '********Merge from 6.0.4.70*************

#Disable Warning BC42304 ' XML documentation parse error: XML entity references are not supported. XML comment will be ignored.
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ISA"></param>
    ''' <param name="GS"></param>
    ''' <param name="strDBServer"></param>
    ''' <param name="strDatabase"></param>
    ''' <param name="strAdminEmail"></param>
    ''' <param name="strGroupEmail"></param>
    ''' <param name="strSMTP"></param>
    ''' <param name="strFromEmail"></param>
    ''' <param name="strMSG"></param>
    ''' <param name="oCon"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Modified by RHR 1/6/2015 v-7.0
    '''   Removed References to old RequireCNS logic
    '''   We now always use the BookSHID as the key value
    '''   mapped to segment B204 
    '''   Removed old code with comment tags no longer needed
    ''' Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' Added fileName functionality
    ''' Modified by RHR for v-8.2 on 07/05/2019
    '''  Added additional mapping from lEDIOrderSpecificFees to  SettlementFee.
    '''  We now use the NGLTMS365BLL.SettlementSave function to write to both the 
    '''  APMassEntryHistoryFees table and the APMassEntryFees we no longer use spSaveAndAllocateAPMassEntryFee
    '''  Here is the new mapping logic for APMassEntryHistoryFees:
    '''  APBillNumber As String:            Reference to APMassEntryHistory table not mapped to APMassEntryHistoryFees
    '''  APCarrierNumber As Integer:        Reference to APMassEntryHistory table Not mapped to APMassEntryHistoryFees
    '''  BookSHID As String:                Reference to APMassEntryHistory table Not mapped to APMassEntryHistoryFees
    '''  BookCarrOrderNumber As String:     Maps to SettlementSave.BookCarrOrderNumber
    '''  StopSequence As Integer:           Maps to SettlementSave.StopSequence
    '''  FeeValue As Decimal:               Maps to SettlementSave.Cost & to SettlementSave.Minimum
    '''  EDICode As String:                 if blank Or empty set accessorial code to zero
    '''  AccessorialCode Formula::          if EDICode Is Not blank Or missing lookup AccessoiralCode using these rules in order:
    '''     1. Legal Entity Carrier Profile Setting
    '''     2. The default accessorial table setting
    '''     3. tblDataEntryFields  Where DEFlag = 1 And DEFileType =  1
    '''     The result maps to SettlementSave.AccessorialCode
    '''  Caption Formula:                   if EDICode Is Not blank Or missing lookup Caption using these rules in order:
    '''     1. Legal Entity Carrier Profile Setting
    '''     2. The default accessorial table setting 
    '''     3. tblDataEntryFields  Where DEFlag = 1 And DEFileType =  1
    ''' Note:  this function is one patch after another with objects passing data between objects with no actual workflow
    '''         it is not even clear if it will work as expected.  if Unit and Cycle testing fail we need to clean this 
    '''         logic up before we go live.  If it does pass testing a project to re-write this method should be started
    '''         as soon as possible.  Do not add any more patches to this function.
    ''' Modified by RHR for v-8.2.0.118 on 9/13/19
    '''   Added new code to complete integration with Standard AP Rules to match the new Settlement page logic
    ''' Modified by RHR for v-8.2.1.004 on 01/03/2020
    '''     Added logic to process missing fees using list of expected/pending fees
    '''     Update billed fees with the new billed and missing fee properties
    ''' </remarks>
    Public Function processData(ByVal ISA As clsEDIISA,
                                ByVal GS As clsEDIGS,
                                ByVal strDBServer As String,
                                ByVal strDatabase As String,
                                ByVal strAdminEmail As String,
                                ByVal strGroupEmail As String,
                                ByVal strSMTP As String,
                                ByVal strFromEmail As String,
                                ByRef strMSG As String,
                                ByRef oCon As System.Data.SqlClient.SqlConnection,
                                Optional ByVal fileName As String = "",
                                Optional ByVal DateProcessed As Date = Nothing,
                                Optional ByRef insertErrorMsg As String = "") As Boolean
#Enable Warning BC42304 ' XML documentation parse error: XML entity references are not supported. XML comment will be ignored.
        Dim blnRet As Boolean = True
        'Dim oCon As New System.Data.SqlClient.SqlConnection
        strMSG = "Success!"

        'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
        Dim oWCFPar = New DAL.WCFParameters() With {.Database = strDatabase,
                                                               .DBServer = strDBServer,
                                                               .UserName = "EDI Integration",
                                                               .WCFAuthCode = "NGLSystem"}

        Dim oEDIData As New DAL.NGLEDIData(oWCFPar)
        Dim blnSqlSuccess As Boolean = True

        Try
            Dim TotalCost As Double
            Dim FreightBillNumber As String = ""
            Dim SHID As String = ""
            Dim strCarrierPartnerCode As String = ""
            Dim strCompPartnerCode As String = ""
            Dim strEventTime As String = ""
            Dim strBookShipCarrierProNumber As String = ""
            Dim strBookShipCarrierNumber As String = ""
            Dim strBookShipCarrierName As String = ""
            Dim strLoadComments As String = ""
            Dim dtPlaceHolder As Date
            Dim dblPlaceHolder As Double
            'the load comments are limited to 255 characters so any extra items are added to this list 
            Dim strExtraCommentsList As New List(Of String)
            Dim oQuery As New Ngl.Core.Data.Query(strDBServer, strDatabase)
            'get the data from the isa record
            strCarrierPartnerCode = ISA.ISA06?.Trim
            Dim strCarrierNumber As String = "0"
            Dim strCompNumber As String = "0"
            'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
            Dim strCompName As String = ""
            Dim strCarrierName As String = ""

            '********Merge from 6.0.4.70*************
            Dim dEDICodes As New Dictionary(Of String, String)
            dEDICodes = readtblDataEntryFields(oQuery)
            '********Merge from 6.0.4.70*************

            'Get the default Carrier Number using EDI settings
            Dim strSQL As String = "Select top 1 CarrierNumber " _
                & " From dbo.Carrier inner join dbo.CarrierEDI on dbo.Carrier.CarrierControl = dbo.CarrierEDI.CarrierEDICarrierControl " _
                & " Where dbo.CarrierEDI.CarrierEDIPartnerCode = '" & strCarrierPartnerCode & "' " _
                & " AND dbo.CarrierEDI.CarrierEDIXAction = '210' " _
                & " Order By dbo.CarrierEDI.CarrierEDIControl Desc "
            Try
                strCarrierNumber = oQuery.getScalarValue(oCon, strSQL, 1)
            Catch ex As Ngl.Core.DatabaseRetryExceededException
                strMSG = "Failed to get Carrier information using Partner Code " & strCarrierPartnerCode & ": " & ex.Message
                Return False
            Catch ex As Ngl.Core.DatabaseLogInException
                strMSG = "Database login failure cannot get Carrier information using Partner Code " & strCarrierPartnerCode & ": " & ex.Message
                Return False
            Catch ex As Ngl.Core.DatabaseInvalidException
                strMSG = "Database access failure cannot get Carrier information using Partner Code " & strCarrierPartnerCode & ": " & ex.Message
                Return False
            Catch ex As Exception
                Throw
                Return False
            End Try
            strCompPartnerCode = ISA.ISA08?.Trim
            'Get the company number
            'Modified (commented out) by LVV 3/2/16 for v-7.0.5.1 EDI 
            'strSQL = "Select top 1 CompNumber " _
            '    & " From dbo.Comp inner join dbo.CompEDI on dbo.Comp.CompControl = dbo.CompEDI.CompEDICompControl " _
            '    & " Where dbo.CompEDI.CompEDIPartnerCode = '" & strCompPartnerCode & "' " _
            '    & " AND dbo.CompEDI.CompEDIXAction = '210' " _
            '    & " Order By dbo.CompEDI.CompEDIControl Desc "
            'Try
            '    strCompNumber = oQuery.getScalarValue(oCon, strSQL, 1)
            'Catch ex As Ngl.Core.DatabaseRetryExceededException
            '    strMSG = "Failed to get Company information using Partner Code " & strCompPartnerCode & ": " & ex.Message
            '    Return False
            'Catch ex As Ngl.Core.DatabaseLogInException
            '    strMSG = "Database login failure cannot get Company information using Partner Code " & strCompPartnerCode & ": " & ex.Message
            '    Return False
            'Catch ex As Ngl.Core.DatabaseInvalidException
            '    strMSG = "Database access failure cannot get Company information using Partner Code " & strCompPartnerCode & ": " & ex.Message
            '    Return False
            'Catch ex As Exception
            '    Throw
            '    Return False
            'End Try

            'CreateObject a Freight Bill Data Table
            Dim dtFreightBill As New FreightBillData.FreightBillDataTable
            Dim drFreightBill As FreightBillData.FreightBillRow = dtFreightBill.NewFreightBillRow

            '********Merge from 6.0.4.70*************
            Dim lEDIOrderSpecificFees As New List(Of ediOrderSpecificFee)
            '********Merge from 6.0.4.70*************
            'Modified by RHR for v-8.2 on 7/11/19 
            ' added or moved variables for new read carrier and company procedures
            Dim oBookData As New DAL.NGLBookData(oWCFPar)
            Dim compNo As Integer = 0
            Dim compControl As Integer = 0
            Dim carrierControl As Integer = 0
            Dim iBookControl As Integer = 0
            With drFreightBill
                'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                'Get the company and carrier names
                'Modified by RHR for v-8.2 on 7/11/19  the new method GetBookCarrierCompBySHID to read the compcontrol and the carrier control
                ' getCompInfoByCNS(oWCFPar, B3.B303?.Trim, strCompName, strCompNumber, strCarrierNumber, strCarrierName)
                .APSHID = B3.B303?.Trim
                .APBillNumber = B3.B302

                Dim ovBookCarrierCompData As LTS.vBookCarrierComp = oBookData.GetBookCarrierCompBySHID(.APSHID)
                If Not ovBookCarrierCompData Is Nothing AndAlso ovBookCarrierCompData.BookControl > 0 Then
                    iBookControl = ovBookCarrierCompData.BookControl
                    compControl = ovBookCarrierCompData.CompControl
                    strCompName = ovBookCarrierCompData.CompName
                    strCompNumber = ovBookCarrierCompData.CompNumber
                    carrierControl = ovBookCarrierCompData.CarrierControl
                    strCarrierName = ovBookCarrierCompData.CarrierName
                    strCarrierNumber = ovBookCarrierCompData.CarrierNumber
                Else
                    strMSG = String.Format("Failed to get booking and company information using SHID {0} : Cannot process freight bill number {1} for Carrier Number {2}.", .APSHID, .APBillNumber, strCarrierNumber)
                    Return False
                End If


                If Date.TryParse(NDT.FormatFlatDateToString(B3.B306), dtPlaceHolder) Then
                    .APBillDate = dtPlaceHolder
                End If
                If Double.TryParse(NDT.FormatEDICurrencyToDouble(B3.B307), dblPlaceHolder) Then
                    .APTotalCost = dblPlaceHolder
                End If
                If String.IsNullOrEmpty(strCarrierNumber) OrElse strCarrierNumber?.Trim.Length < 1 Then
                    strCarrierNumber = "0"
                End If
                ''.APCarrierNumber = strCarrierNumber

                '********Merge from 6.0.4.70*************
                'Modified by RHR for v-6.0.4.7 added integer.tryparse logic
                Integer.TryParse(strCarrierNumber, .APCarrierNumber)
                '.APCarrierNumber = strCarrierNumber
                '********Merge from 6.0.4.70*************

                If String.IsNullOrWhiteSpace(strCompNumber) Then
                    strCompNumber = "0"
                End If
                .APCustomerID = strCompNumber
                'Get the carrier BL Number
                If Not LoopN9 Is Nothing Then
                    For Each oN9 As clsEDI210N9Loop In LoopN9
                        'Modified By LVV on 2/19/18 for v-8.1 TMS 365 PQ EDI 
                        'From Task 269
                        'If oN9.N9.N901 = "PO" Then
                        '    .APBLNumber = Left(oN9.N9.N902, 20)
                        'End If
                        If oN9.N9.N901 = "BM" Then
                            .APBLNumber = Left(oN9.N9.N902, 20)
                        End If

                    Next
                End If
                'Get the Fees and costs stored in the 400 loop
                If Not Loop400 Is Nothing Then
                    For Each o400 As clsEDI210Loop400 In Loop400

                        '********Merge from 6.0.4.70*************
                        Dim oEDIOrderSpecificFee As ediOrderSpecificFee
                        'Modified by RHR for v-6.0.4.7 on 10/18/2017
                        If o400.L1s.Count > 0 Then
                            For Each oL1 In o400.L1s
                                'Note: as of 8.2.0.117 the logic in readL1AccessorialFee for mapping to APFees(1 to 6) is 
                                'no longer used because this logic is now implemented in NGLTMS365BLL.SettlementSave
                                'We do still use the logic in readL1AccessorialFee to populate fee info and stop or order number data
                                'in the future readL1AccessorialFee function could be replaced or optimized for better performance
                                oEDIOrderSpecificFee = readL1AccessorialFee(drFreightBill, dEDICodes, oL1, o400)
                                If Not oEDIOrderSpecificFee Is Nothing Then
                                    lEDIOrderSpecificFees.Add(oEDIOrderSpecificFee)
                                End If
                            Next
                        ElseIf Not o400.L1 Is Nothing Then
                            'the list is empty so just process the the L1 object 
                            '(For backward compatibility only should not acutally be used).
                            'in older version we did not support a list of fees
                            oEDIOrderSpecificFee = readL1AccessorialFee(drFreightBill, dEDICodes, o400.L1, o400)
                            If Not oEDIOrderSpecificFee Is Nothing Then
                                lEDIOrderSpecificFees.Add(oEDIOrderSpecificFee)
                            End If
                        End If
                        '********Merge from 6.0.4.70*************

                        '********Merge from 6.0.4.70************* Commented out
                        ''Dim dblCost As Double = 0
                        ''Double.TryParse(Ngl.Core.Utility.DataTransformation.FormatEDICurrencyToDouble(o400.L1.L104), dblCost)
                        ''If o400.L1.L101 = "1" Then
                        ''    'this is the carrier freight cost
                        ''    .APCarrierCost = dblCost
                        ''Else
                        ''    Dim strCode As String = o400.L1.L108?.Trim
                        ''    Dim blnAddToOtherCost As Boolean = True
                        ''    If strCode.Length > 0 Then
                        ''        'Lookup the field name in the database if one exists
                        ''        strSQL = "Select top 1 DEFieldName from dbo.tblDataEntryFields " _
                        ''            & " Where DEFileType = 1 And DEFieldMapCode = '" & strCode & "' "
                    ''        Try
                    ''            Dim strFieldName = oQuery.getScalarValue(oCon, strSQL, 1)
                    ''            Select Case strFieldName.ToUpper
                    ''                Case "APFEE1"
                    ''                    .APFee1 = dblCost
                    ''                    blnAddToOtherCost = False
                    ''                Case "APFEE2"
                    ''                    .APFee2 = dblCost
                    ''                    blnAddToOtherCost = False
                    ''                Case "APFEE3"
                    ''                    .APFee3 = dblCost
                    ''                    blnAddToOtherCost = False
                    ''                Case "APFEE4"
                    ''                    .APFee4 = dblCost
                    ''                    blnAddToOtherCost = False
                    ''                Case "APFEE5"
                    ''                    .APFee5 = dblCost
                    ''                    blnAddToOtherCost = False
                    ''                Case "APFEE6"
                    ''                    .APFee6 = dblCost
                    ''                    blnAddToOtherCost = False
                    ''                Case Else
                    ''                    blnAddToOtherCost = True
                    ''            End Select

                    ''        Catch ex As Exception
                    ''            'here we ignore all errors and just save the data to the other cost field by defaul
                    ''        End Try
                    ''    End If
                    ''    If blnAddToOtherCost Then
                    ''        .APOtherCost += dblCost
                    ''    End If
                    ''End If
                    Next
                End If
                'Get the L301 total weight
                .APBilledWeight = CInt(Val(L3.L301.ToString))

                TotalCost = .APTotalCost
                FreightBillNumber = .APBillNumber
                SHID = .APSHID
            End With
            'add the row to the table
            dtFreightBill.AddFreightBillRow(drFreightBill)
            dtFreightBill.AcceptChanges()
            'Use the clsFreightBillImport to save the data
            Dim oFreightBillImport As New clsFreightBillImport
            With oFreightBillImport
                .AdminEmail = strAdminEmail
                .GroupEmail = strGroupEmail
                .SMTPServer = strSMTP
                .Retry = 1
                .DBServer = strDBServer
                .Database = strDatabase
                .Debug = False
                .FromEmail = strFromEmail
            End With

            'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
            Dim dto210In As New DTO.tblEDI210In
            With dto210In
                .APPONumber = drFreightBill.APPONumber
                .APPRONumber = drFreightBill.APPRONumber
                .APCNSNumber = drFreightBill.APCNSNumber
                .APCarrierNumber = drFreightBill.APCarrierNumber
                .APBillNumber = drFreightBill.APBillNumber
                .APBillDate = drFreightBill.APBillDate
                .APCustomerID = drFreightBill.APCustomerID
                .APCostCenterNumber = drFreightBill.APCostCenterNumber
                .APTotalCost = drFreightBill.APTotalCost
                .APBLNumber = drFreightBill.APBLNumber
                .APBilledWeight = drFreightBill.APBilledWeight
                .APTotalTax = drFreightBill.APTotalTax
                .APFee1 = drFreightBill.APFee1
                .APFee2 = drFreightBill.APFee2
                .APFee3 = drFreightBill.APFee3
                .APFee4 = drFreightBill.APFee4
                .APFee5 = drFreightBill.APFee5
                .APFee6 = drFreightBill.APFee6
                .APOtherCosts = drFreightBill.APOtherCost
                .APCarrierCost = drFreightBill.APCarrierCost
                .APOrderSequence = drFreightBill.APOrderSequence
                .EDI210InFileName = fileName
                .EDI210InMessage = strMSG
                .CompName = strCompName
                .CarrierName = strCarrierName
            End With
            'Log the summary information in the tblEDI210In table
            If Not oEDIData.InsertIntoEDI210In(dto210In, DateProcessed) Then
                insertErrorMsg = "Could not insert record into tblEDI210In for CNS: " + dto210In.APCNSNumber + ", Bill Number: " + dto210In.APBillNumber + ", DateProcessed, : " + DateProcessed.ToString + ", and filename: " + fileName
            End If

            'check blnFlag here return false
            If blnSqlSuccess = False Then
                Return False
            End If
            Dim dDiscount As Decimal = 0
            'Modified By LVV on 2/19/18 for v-8.1 TMS 365 PQ EDI           
            Try
                Dim oBLL As New BLL.NGLTMS365BLL(oWCFPar)
                Dim oBook As New DAL.NGLBookData(oWCFPar)

                For Each oRow As FreightBillData.FreightBillRow In dtFreightBill
                    Dim s As New DAL.Models.SettlementSave
                    Dim sFeeList As New List(Of DAL.Models.SettlementFee)
                    compNo = 0
                    Integer.TryParse(oRow.APCustomerID, compNo)
                    s.BookSHID = oRow.APSHID
                    s.BookControl = iBookControl

                    'extract all the fees from the main fee list with the same FBNo, Carrier, and SHID as the row into a sub list
                    Dim FBFees = lEDIOrderSpecificFees.Where(Function(x) x.APBillNumber = oRow.APBillNumber And x.BookSHID = oRow.APSHID And x.APCarrierNumber = oRow.APCarrierNumber).ToArray()

                    For Each f In FBFees
                        If f.FeeValue < 0 Then
                            'add this to the discounts
                            dDiscount += f.FeeValue
                        Else
                            'Get the rest of the information needed for each fee
                            Dim r = oEDIData.GetDataForEDIFee365(f.BookSHID, f.APCarrierNumber, compNo, f.BookCarrOrderNumber, f.EDICode, f.StopSequence, f.EDIDescription)

                            If Not r Is Nothing Then
                                'Modified by RHR for v-8.2 on 7/11/19 
                                'we no longer use the fee's compcontrol or carriercontrol because it does not support multi-stop
                                'new logic above looks up the compcontrol and carriercontrol using the EDI settings
                                'compControl = r.CompControl
                                'carrierControl = r.CarrierControl


                                Dim sf As New DAL.Models.SettlementFee

                                With sf
                                    .Control = r.FeeControl
                                    .BookControl = r.BookControl
                                    .AccessorialCode = r.AccessorialCode
                                    If (String.IsNullOrWhiteSpace(f.EDICode) Or f.EDICode.ToUpper() = "MSC") And Not String.IsNullOrWhiteSpace(f.EDIDescription) Then
                                        .Caption = f.EDIDescription
                                    Else
                                        .Caption = r.Caption
                                    End If
                                    .Pending = r.Pending
                                    .Cost = f.FeeValue
                                    'Modified by RHR for v-8.2 on 7/11/19 added Order Number and stop sequence to SettlementFee data
                                    .BookCarrOrderNumber = f.BookCarrOrderNumber
                                    .StopSequence = f.StopSequence
                                    .EDICode = f.EDICode
                                    'Modified by RHR for v-8.2.1.004 on 01/03/2020 Update billed fees with the new billed and missing fee properties
                                    .BilledFee = True
                                    .MissingFee = False
                                End With
                                sFeeList.Add(sf)
                            End If
                        End If

                    Next


                    s.CompControl = compControl
                    s.APCustomerID = oRow.APCustomerID
                    s.CarrierControl = carrierControl
                    s.CarrierNumber = oRow.APCarrierNumber
                    s.InvoiceNo = oRow.APBillNumber
                    s.InvoiceAmt = oRow.APTotalCost
                    s.LineHaul = oRow.APCarrierCost + dDiscount
                    s.BookFinAPActWgt = oRow.APBilledWeight
                    s.BookCarrBLNumber = oRow.APBLNumber
                    If s.BookControl < 1 Then
                        s.BookControl = s.Fees(0).BookControl
                    End If
                    s.APBillDate = oRow.APBillDate
                    s.APReceivedDate = Date.Now()

                    'Modified by RHR for v-8.2.1.004 on 01/03/2020
                    ' added logic to process missing fees.
                    'first get all of the expected fees
                    Dim oExpectedFees() As DAL.Models.SettlementFee = oEDIData.GetDataForEDIExpectedFees(s.BookSHID, s.CarrierNumber, s.APCustomerID)
                    'next loop through the list of billed fees and check for missing EDI Codes
                    'we use EDI codes to identify missing fees
                    If Not oExpectedFees Is Nothing AndAlso oExpectedFees.Count() > 0 Then
                        If sFeeList Is Nothing Then
                            sFeeList = New List(Of DAL.Models.SettlementFee)
                        End If
                        For Each f In oExpectedFees
                            If Not sFeeList.Any(Function(x) x.EDICode = f.EDICode) Then
                                'update the fees missing , billed and pending properties
                                f.MissingFee = True
                                f.BilledFee = False
                                f.Pending = True
                                sFeeList.Add(f)
                            End If
                        Next
                    End If
                    'now we can add the fees to the  Settlement Save Fees property
                    s.Fees = sFeeList.ToArray()

                    'Process the Freight Bill (365 BR Process - used by Web Tender as well)

                    Dim oResults = oBLL.SettlementSave(s, True)
                    If Not String.IsNullOrWhiteSpace(oResults.ErrMsg) Then
                        If strMSG = "Success!" Then
                            strMSG = " Error: " & oResults.ErrMsg
                        Else
                            strMSG &= " Error: " & oResults.ErrMsg
                        End If

                    End If
                    If Not String.IsNullOrWhiteSpace(oResults.WarningMsg) Then
                        If strMSG = "Success!" Then
                            strMSG = " Warning: " & oResults.WarningMsg
                        Else
                            strMSG &= " Warning: " & oResults.WarningMsg
                        End If
                    End If

                Next

                'call spUpdateLoadStatus only log errors, do not stop here is sp fails. 
                Using db As New LTSIntegrationDataDataContext(oQuery.ConnectionString)
                    Try
                        Dim oReturnData = (From d In db.spUpdate210StatusEDI(TotalCost, FreightBillNumber, SHID) Select d).FirstOrDefault()
                        If oReturnData.Success = 0 Then
                            If strMSG = "Success!" Then
                                strMSG = " Warning: Freight Bill " & FreightBillNumber & " was processed but the Load Status couldn't be updated. Check Admin email for more details."
                            Else
                                strMSG = strMSG & " Warning: Freight Bill " & FreightBillNumber & " was processed but the Load Status couldn't be updated. Check Admin email for more details."
                            End If

                        End If
                    Catch ex As Exception
                        'ManageLinqDataExceptions(ex, buildProcedureName("GetExportFeeRows70"))
                    End Try
                End Using

                blnRet = True

            Catch ex As Exception
                'log error
                If strMSG = "Success!" Then
                    strMSG = ex.Message
                Else
                    strMSG &= " " & ex.Message
                End If

                blnRet = False
            End Try

            'Depreciated By LVV on 2/19/18 for v-8.1 TMS 365 PQ EDI 
            'Replaced by above code
            ''Dim enmRetVal As ProcessDataReturnValues = oFreightBillImport.ProcessData(dtFreightBill, oQuery.ConnectionString)
            ''Select Case enmRetVal
            ''    Case Configuration.ProcessDataReturnValues.nglDataConnectionFailure
            ''        strMSG = "Database Connection Failure Error: " & oFreightBillImport.LastError
            ''        blnRet = False
            ''    Case Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            ''        strMSG = "Data Integration Failure Error: " & oFreightBillImport.LastError
            ''        blnRet = False
            ''    Case Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
            ''        strMSG = "Some Errors: " & oFreightBillImport.LastError
            ''        blnRet = False
            ''    Case Configuration.ProcessDataReturnValues.nglDataValidationFailure
            ''        strMSG = "Data Validation Failure Error: " & oFreightBillImport.LastError
            ''        blnRet = False
            ''    Case Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            ''        strMSG = "Success! Data imported."
            ''        blnRet = True
            ''        'call spUpdateLoadStatus only log errors, do not stop here is sp fails. 
            ''        Using db As New LTSIntegrationDataDataContext(oQuery.ConnectionString)
            ''            Try
            ''                Dim oReturnData = (From d In db.spUpdate210StatusEDI(TotalCost, FreightBillNumber, SHID) Select d).FirstOrDefault()
            ''                If oReturnData.Success = 0 Then
            ''                    strMSG = strMSG & " Warning: Freight Bill " & FreightBillNumber & " was processed but the Load Status couldn't be updated. Check Admin email for more details."
            ''                End If
            ''            Catch ex As Exception
            ''                'ManageLinqDataExceptions(ex, buildProcedureName("GetExportFeeRows70"))
            ''            End Try
            ''        End Using
            ''    Case Else
            ''        strMSG = "Invalid Return Value."
            ''        blnRet = False
            ''End Select
            dtFreightBill = Nothing
            oFreightBillImport = Nothing

            'Depreciated By RHR on 7/23/19 for v-8.2.0.117 we now use new BLL logic instead of spSaveAndAllocateAPMassEntryFee
            '********Merge from 6.0.4.70*************
            'ToDo add code to update the order specific fees for each OrderSpecificFees in the  lEDIOrderSpecificFees  list
            'If blnRet AndAlso Not lEDIOrderSpecificFees Is Nothing AndAlso lEDIOrderSpecificFees.Count > 0 Then
            '    For Each oFee In lEDIOrderSpecificFees
            '        Try
            '            Dim cmd As New SqlCommand()
            '            cmd.CommandType = CommandType.StoredProcedure
            '            cmd.CommandText = "spSaveAndAllocateAPMassEntryFee"
            '            cmd.Parameters.Add("@APCarrierNumber", SqlDbType.Int).Value = oFee.APCarrierNumber
            '            cmd.Parameters.Add("@APBillNumber", SqlDbType.NVarChar).Value = oFee.APBillNumber
            '            cmd.Parameters.Add("@BookSHID", SqlDbType.NVarChar).Value = oFee.BookSHID
            '            cmd.Parameters.Add("@BookCarrOrderNumber", SqlDbType.NVarChar).Value = oFee.BookCarrOrderNumber
            '            cmd.Parameters.Add("@BookStopSequence", SqlDbType.Int).Value = oFee.StopSequence
            '            cmd.Parameters.Add("@APMFeesValue", SqlDbType.Money).Value = oFee.FeeValue
            '            cmd.Parameters.Add("@APMFeesEDICode", SqlDbType.NVarChar).Value = oFee.EDICode
            '            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = "System"
            '            Dim oQR As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(cmd)
            '            If Not oQR.Exception Is Nothing Then
            '                'log exception
            '                strMSG &= " " & oQR.Exception.Message
            '            End If

            '            Dim dt As System.Data.DataTable = oQR.Data
            '            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
            '                For Each oRow As System.Data.DataRow In dt.Rows
            '                    Dim iErrNumber As Integer? = NDT.getDataRowString(oRow, "ErrNumber", "")
            '                    If iErrNumber.HasValue AndAlso iErrNumber <> 0 Then
            '                        Dim sMsg = NDT.getDataRowString(oRow, "RetMsg", "")
            '                        'log msg
            '                        strMSG &= " " & sMsg
            '                    End If
            '                Next
            '            End If
            '        Catch ex As Exception
            '            'log error
            '            strMSG &= " " & ex.Message
            '        End Try
            '    Next
            'End If
            '********Merge from 6.0.4.70*************

            Return blnRet
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    Public Function writeToFile(ByVal ISA As clsEDIISA,
                                ByVal GS As clsEDIGS) As Boolean
        Dim blnRet As Boolean = True

        Try
            Dim strFilePath As String = "C:\Data\EDI\Test\"
            If Not Directory.Exists(strFilePath) Then Directory.CreateDirectory(strFilePath)
            Dim FileName As String = strFilePath & "210" & Date.Now.ToString("ddMMHHmmss") & ".txt"


            Dim fi As FileInfo = New FileInfo(FileName)
            'create the file if it does not exist
            If Not File.Exists(FileName) Then
                Using w As StreamWriter = fi.CreateText
                    w.Close()
                End Using
            End If
            'now open the file and read the data
            Using sw As New StreamWriter(FileName)
                sw.Write(getRecord(ISA, GS))
                sw.Flush()
            End Using
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function



    Public Function writeRecord(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS, Optional ByVal strFilePath As String = "C:\Data\EDI\Test\") As Boolean
        'For testing we just write the data to a file
        Try
            strFilePath = strFilePath?.Trim
            If Not Right(strFilePath, 1) = "\" Then strFilePath &= "\"
            If Not Directory.Exists(strFilePath) Then Directory.CreateDirectory(strFilePath)
            Dim FileName As String = strFilePath & "210Record" & Date.Now.ToString("ddMMHHmmss") & ".txt"

            Dim fi As FileInfo = New FileInfo(FileName)
            'create the file if it does not exist
            If Not File.Exists(FileName) Then
                Using w As StreamWriter = fi.CreateText
                    w.Close()
                End Using
            End If
            'now open the file and wite the data
            Using sw As New StreamWriter(FileName)
                sw.Write(getRecord(ISA, GS))
                sw.Flush()
            End Using
        Catch ex As Exception
            'do nothing for test
        End Try
        Return True
    End Function

    Public Function getRecord(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS) As String
        'Format the record for reporting.
        Dim strToPrint As String = "*************** 210 *********************" & vbCrLf
        strToPrint &= "ISA Sender: " & ISA.ISA06 & vbCrLf
        strToPrint &= "GS Sender: " & GS.GS02 & vbCrLf
        strToPrint &= "GS Date: " & GS.GS04 & vbCrLf
        strToPrint &= "GS Time: " & GS.GS05 & vbCrLf
        strToPrint &= "GS Group Control: " & GS.GS06 & vbCrLf
        strToPrint &= "ST: " & ST.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "B3: " & B3.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "C3: " & C3.getEDIString(ISA.SegmentTerminator) & vbCrLf
        If Not LoopN9 Is Nothing Then
            For Each L As clsEDI210N9Loop In LoopN9
                strToPrint &= "N9: " & L.N9.getEDIString(ISA.SegmentTerminator) & vbCrLf
            Next
        End If
        strToPrint &= "G62: " & G62.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "K1: " & K1.getEDIString(ISA.SegmentTerminator) & vbCrLf
        If Not Loop100 Is Nothing Then
            For Each L As clsEDI210Loop100 In Loop100
                strToPrint &= "N1: " & L.N1.getEDIString(ISA.SegmentTerminator) & vbCrLf
                strToPrint &= "N2: " & L.N2.getEDIString(ISA.SegmentTerminator) & vbCrLf
                strToPrint &= "N3: " & L.N3.getEDIString(ISA.SegmentTerminator) & vbCrLf
                strToPrint &= "N4: " & L.N4.getEDIString(ISA.SegmentTerminator) & vbCrLf
            Next
        End If
        If Not Loop200 Is Nothing Then
            For Each L As clsEDI210Loop200 In Loop200
                strToPrint &= "N7: " & L.N7.getEDIString(ISA.SegmentTerminator) & vbCrLf
            Next
        End If
        If Not Loop400 Is Nothing Then
            For Each L As clsEDI210Loop400 In Loop400
                strToPrint &= "LX: " & L.LX.getEDIString(ISA.SegmentTerminator) & vbCrLf
                strToPrint &= "L5: " & L.L5.getEDIString(ISA.SegmentTerminator) & vbCrLf
                strToPrint &= "L0: " & L.L0.getEDIString(ISA.SegmentTerminator) & vbCrLf
                strToPrint &= "L1: " & L.L1.getEDIString(ISA.SegmentTerminator) & vbCrLf
            Next
        End If
        strToPrint &= "L3: " & L3.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "SE: " & SE.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "*****************************************" & vbCrLf
        strToPrint &= vbCrLf

        Return strToPrint

    End Function

    ''' <summary>
    ''' Creates an EDI 210 document string
    ''' </summary>
    ''' <param name="strISA"></param>
    ''' <param name="strGS"></param>
    ''' <param name="strIEA"></param>
    ''' <param name="strGE"></param>
    ''' <param name="SegmentTerminator"></param>
    ''' <returns>String</returns>
    ''' <remarks>
    ''' Added by LVV on 4/14/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Function getEDIRecord(ByVal strISA As String, ByVal strGS As String, ByVal strIEA As String, ByVal strGE As String, ByVal SegmentTerminator As String) As String
        'Format the edi record
        Dim strToPrint As String = ""
        strToPrint &= strISA
        strToPrint &= strGS
        strToPrint &= ST.getEDIString(SegmentTerminator)
        strToPrint &= B3.getEDIString(SegmentTerminator)
        strToPrint &= C3.getEDIString(SegmentTerminator)
        If Not LoopN9 Is Nothing Then
            For Each L As clsEDI210N9Loop In LoopN9
                strToPrint &= L.N9.getEDIString(SegmentTerminator)
            Next
        End If
        strToPrint &= G62.getEDIString(SegmentTerminator)
        strToPrint &= K1.getEDIString(SegmentTerminator)
        If Not Loop100 Is Nothing Then
            For Each L As clsEDI210Loop100 In Loop100
                strToPrint &= L.N1.getEDIString(SegmentTerminator)
                strToPrint &= L.N2.getEDIString(SegmentTerminator)
                strToPrint &= L.N3.getEDIString(SegmentTerminator)
                strToPrint &= L.N4.getEDIString(SegmentTerminator)
            Next
        End If
        If Not Loop200 Is Nothing Then
            For Each L As clsEDI210Loop200 In Loop200
                strToPrint &= L.N7.getEDIString(SegmentTerminator)
            Next
        End If
        If Not Loop400 Is Nothing Then
            For Each L As clsEDI210Loop400 In Loop400
                strToPrint &= L.LX.getEDIString(SegmentTerminator)
                strToPrint &= L.L5.getEDIString(SegmentTerminator)
                strToPrint &= L.L0.getEDIString(SegmentTerminator)
                strToPrint &= L.L1.getEDIString(SegmentTerminator)
            Next
        End If
        strToPrint &= L3.getEDIString(SegmentTerminator)
        strToPrint &= SE.getEDIString(SegmentTerminator)
        strToPrint &= strGE
        strToPrint &= strIEA

        Return strToPrint

    End Function


    Public LastError As String = ""
    Public ST As New clsEDIST
    Public B3 As New clsEDIB3
    Public C3 As New clsEDIC3
    Public LoopN9() As clsEDI210N9Loop
    Public G62 As New clsEDIG62
    Public K1 As New clsEDIK1
    Public Loop100() As clsEDI210Loop100
    Public Loop200() As clsEDI210Loop200
    Public Loop400() As clsEDI210Loop400
    Public L3 As New clsEDIL3
    Public SE As New clsEDISE

    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
        'add the ST, B3 and C3 elements if they exist
        'the ST and B3 elements are requred but C3 is not.
        For isubsegs As Integer = 2 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the ST record (it should be all that is left 
                    If Left(sElems(0), 3) <> "ST*" Then sElems(0) = "ST*" & sElems(0)
                    ST = New clsEDIST(sElems(0))
                Case 1
                    'read the B3 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "B3\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        B3 = New clsEDIB3("B3*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 2
                    'read the B3 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "C3\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        C3 = New clsEDIC3("C3*" & sElems(0))
                        strSource = segs(0)
                    End If
            End Select
        Next
        'Add the SE
        If Not String.IsNullOrEmpty(strSEElem) Then
            'split out any unwanted elements 
            Dim sElems() As String = strSEElem.Split(strSegSep)
            If Left(sElems(0), 3) <> "SE*" Then sElems(0) = "SE*" & sElems(0)
            SE = New clsEDISE(sElems(0))
        End If
    End Sub

    Public Sub addG62andK1DataFromString(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
        'add the G62 and K1 elements if they exist
        'NOTE: we assume that the G62 element is required but K1 is not
        For isubsegs As Integer = 1 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'read the G62 record
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the G62 record (it should be all that is left 
                    If Left(sElems(0), 4) <> "G62*" Then sElems(0) = "G62*" & sElems(0)
                    G62 = New clsEDIG62(sElems(0))
                Case 1
                    'read the K1 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "K1\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        K1 = New clsEDIK1("K1*" & sElems(0))
                        strSource = segs(0)
                    End If
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Gets the Company Name and Number by CNS. Also, optionally gets the Carrier Name by Carrier Number.
    ''' </summary>
    ''' <param name="oWCFPar"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <param name="CompName"></param>
    ''' <param name="CompNumber"></param>
    ''' <param name="strCarrierNumber"></param>
    ''' <param name="CarrierName"></param>
    ''' <remarks>
    ''' Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' </remarks>
    Public Sub getCompInfoByCNS(ByVal oWCFPar As DAL.WCFParameters, ByVal BookConsPrefix As String, ByRef CompName As String, ByRef CompNumber As Integer, Optional ByVal strCarrierNumber As String = "", Optional ByRef CarrierName As String = "")
        Dim oBook As New DAL.NGLBookData(oWCFPar)
        Dim oCarrier As New DAL.NGLCarrierData(oWCFPar)
        Dim oComp As New DAL.NGLCompData(oWCFPar)
        Dim blnMultiple As Boolean = False

        'Use the CNS to get the booking records
        Dim books = oBook.GetBooksFilteredNoChildren(BookConsPrefix:=BookConsPrefix)

        If books.Length > 0 Then
            Dim compControl = books(0).BookCustCompControl
            For Each b In books
                'Check to see if all orders on the CNS have the same Company
                If Not b.BookCustCompControl = compControl Then
                    blnMultiple = True
                    Exit For
                End If
            Next

            If blnMultiple Then
                CompName = "Multiple"
                CompNumber = 0
            Else
                Dim comp = oComp.GetCompFiltered(Control:=compControl)
                CompNumber = comp.CompNumber
                CompName = comp.CompName
            End If

        End If

        'If a CarrierNumber parameter is provided, use it to get the CarrierName
        Dim intCarrierNumber As Integer = 0
        If strCarrierNumber?.Trim.Length > 0 Then
            Integer.TryParse(strCarrierNumber, intCarrierNumber)
            Dim carrier = oCarrier.GetCarrierFiltered(Number:=intCarrierNumber)
            CarrierName = carrier.CarrierName
        End If

    End Sub

End Class

#End Region


#Region " EDI 990 Class"

''' <summary>
''' EDI 990 Accept or Reject Object
''' </summary>
''' <remarks>
''' Modified by RHR for v-7.0.6.105
''' </remarks>
Public Class clsEDI990

    Public LastError As String = ""
    Public ST As New clsEDIST
    Public B1 As New clsEDIB1
    Public K1 As New clsEDIK1
    Public N9 As New clsEDIN9
    Public SE As New clsEDISE

    Public Sub New()
        MyBase.New()

    End Sub

    Public Sub New(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
        MyBase.New()
        addDataFromString(strSource, strSegSep, strSEElem)
    End Sub

    'Add a parameter called CarrierControl
    'pass 0 as CarrierCont
    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Public Function processData(ByVal ISA As clsEDIISA,
                                ByVal GS As clsEDIGS,
                                ByVal CarrierControl As Integer,
                                ByVal strDBServer As String,
                                ByVal strDatabase As String,
                                ByRef strMSG As String,
                                ByRef oCon As System.Data.SqlClient.SqlConnection,
                                Optional ByVal fileName As String = "",
                                Optional ByVal DateProcessed As Date = Nothing,
                                Optional ByRef insertErrorMsg As String = "") As Boolean
        'Dim oCon As New System.Data.SqlClient.SqlConnection
        strMSG = "Success!"
        Try
            'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
            Dim oWCFPar = New DAL.WCFParameters() With {.Database = strDatabase,
                                                             .DBServer = strDBServer,
                                                             .UserName = "EDI Integration",
                                                             .WCFAuthCode = "NGLSystem"}

            Dim oEDIData As New DAL.NGLEDIData(oWCFPar)
            Dim CNS As String = B1.B102?.Trim
            Dim SCAC As String = B1.B101?.Trim

            'Get the Firt BookControl Number that matches the Shipment Identification Number (Always the CNS Number)
            '@TODO We need to modify this query in the future to use the BookSHID instead of the BookConsPrefix
            Dim strSQL As String = "Select top 1 BookControl From dbo.Book Where BookSHID = '" & B1.B102 & "'"
            Dim intBookControl As Integer = 0
            Try
                Dim oQuery As New Ngl.Core.Data.Query(strDBServer, strDatabase)
                If Not Integer.TryParse(oQuery.getScalarValue(oCon, strSQL, 1), intBookControl) Then
                    strMSG = "The following query did not return a valid Book Control Number for the load: " & strSQL
                    Return False
                Else
                    'Check if we are sending email updates for 990s by carrier
                    strSQL = "Select top 1 CarrierEDIEmailNotificationOn From dbo.CarrierEDI Where CarrierEDIXaction = '990' AND CarrierEDICarrierControl = " & CarrierControl
                    Dim intSendEmail As Integer
                    Dim strScalar = oQuery.getScalarValue(oCon, strSQL, 1)
                    If IsNumeric(strScalar) Then
                        Integer.TryParse(strScalar, intSendEmail)
                    ElseIf Not String.IsNullOrWhiteSpace(strScalar) AndAlso strScalar.ToUpper = "TRUE" Then
                        intSendEmail = 1
                    Else
                        intSendEmail = 0
                    End If
                    If intSendEmail <> 1 Then intSendEmail = 0
                    Dim sendEmail As Boolean = True
                    If intSendEmail = 0 Then
                        sendEmail = False
                    End If

                    'Added by LVV 8/5/16 for v-7.0.5.110 Ticket #2137
                    Dim blnCanUpdate990 As Boolean = True
                    Dim intupdateOnPC As Integer = CInt(oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalProcess990OnlyPCStatus'"))
                    If intupdateOnPC = 1 Then
                        'The parameter is turned on so we have to check the BookTranCode
                        Dim strSQL2 As String = "Select top 1 BookTranCode From dbo.Book Where BookControl = '" & intBookControl & "'"
                        Dim oQuery2 As New Ngl.Core.Data.Query(strDBServer, strDatabase)
                        Dim bookTranCode = oQuery2.getScalarValue(oCon, strSQL2, 1)
                        If String.IsNullOrWhiteSpace(bookTranCode) Then
                            strMSG = "The following query did not return a valid Book Tran Code for the load: " & strSQL2
                            Return False
                        End If
                        If bookTranCode <> "PC" Then blnCanUpdate990 = False
                    End If

                    If blnCanUpdate990 Then
                        'Update the status of the record to received.
                        Dim bll As New BLL.NGLBookBLL(Configuration.createWCFParameters(strDBServer, strDatabase, oCon.ConnectionString))
                        If B1.B104.ToUpper = "A" Then
                            'this is an accept

                            'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                            If Not oEDIData.Update204990Received(CNS, DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Accept, DateProcessed) Then
                                insertErrorMsg = "Could not update tblEDI204 fields 990Received: 1, 990ReceivedDate: " + DateProcessed.ToString + ", and Status Code: 990 A for " + CNS + "."
                            End If
                            If Not oEDIData.InsertIntoEDI990(CNS, SCAC, DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Accept, DateProcessed, fileName) Then
                                insertErrorMsg += "Could not insert record into tblEDI990 for CNS: " + CNS + ", SCAC: " + SCAC + ", Status Code: 10, DateProcessed, : " + DateProcessed.ToString + ", and filename: " + fileName
                            End If

                            Dim result = bll.AcceptOrRejectLoad(intBookControl, CarrierControl, 0, FM.BLL.NGLBookBLL.AcceptRejectEnum.Accepted, sendEmail, "Load Tender Accepted Via 990", 0, "", "", FM.BLL.NGLBookBLL.AcceptRejectModeEnum.EDI, "EDI 990")
                            If result.Success = False Then
                                strMSG = result.getAllMessagesNotLocalized()
                                Return False
                            End If
                        Else
                            'anything else returned is a reject

                            'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                            If Not oEDIData.Update204990Received(CNS, DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject, DateProcessed) Then
                                insertErrorMsg = "Could not update tblEDI204 fields 990Received: 1, 990ReceivedDate: " + DateProcessed.ToString + ", and Status Code: 990 R for " + CNS + "."
                            End If
                            If Not oEDIData.InsertIntoEDI990(CNS, SCAC, DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject, DateProcessed, fileName) Then
                                insertErrorMsg += "Could not insert record into tblEDI990 for CNS: " + CNS + ", SCAC: " + SCAC + ", Status Code: 11, DateProcessed, : " + DateProcessed.ToString + ", and filename: " + fileName
                            End If

                            Dim result = bll.AcceptOrRejectLoad(intBookControl, CarrierControl, 0, FM.BLL.NGLBookBLL.AcceptRejectEnum.Rejected, sendEmail, "Load Tender Rejected Via 990", 0, "", "", FM.BLL.NGLBookBLL.AcceptRejectModeEnum.EDI, "EDI 990")
                            If result.Success = False Then
                                strMSG = result.getAllMessagesNotLocalized()
                                Return False
                            End If
                        End If
                    Else
                        'Added by LVV 8/5/16 for v-7.0.5.110 Ticket #2137
                        'send an alert
                        Dim strAction = "Reject"
                        If B1.B104.ToUpper = "A" Then strAction = "Accept"
                        Dim strAlertMsg = "Carrier attempted to " + strAction + " a load that was no longer in PC Status. This process cannot be completed automatically. Status must be manually configured."
                        Dim oBatchData As New DAL.NGLBatchProcessDataProvider(oWCFPar)
                        oBatchData.executeInsertAlertMessage("AlertEDI", 0, 0, 0, 0, "Failed to Process EDI 990", strAlertMsg, "CNS: " + CNS, "SCAC: " + SCAC, "DateProcessed: " + DateProcessed.ToString, "Filename: " + fileName, "")
                        'insertErrorMsg += strAlertMsg + "CNS: " + CNS + ", SCAC: " + SCAC + ", DateProcessed: " + DateProcessed.ToString + ", and filename: " + fileName
                        strMSG += strAlertMsg
                        Return False
                    End If

                End If

            Catch ex As Ngl.Core.DatabaseRetryExceededException
                strMSG = "Failed to update the accept or reject load status: " & ex.Message
                Return False
            Catch ex As Ngl.Core.DatabaseLogInException
                strMSG = "Database login failure: " & ex.Message
                Return False
            Catch ex As Ngl.Core.DatabaseInvalidException
                strMSG = "Database access failure : " & ex.Message
                Return False
            Catch ex As Ngl.Core.DatabaseDataValidationException
                strMSG = ex.Message
                Return False
            Catch ex As Exception
                Throw
                Return False
            End Try


        Catch ex As Exception
            Throw
        End Try
        Return True
    End Function

    Public Function testData(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS, Optional ByVal blnWriteToFile As Boolean = False, Optional ByVal strFilePath As String = "C:\Data\EDI\Test\") As String
        Dim strToPrint As String = ""
        Try
            strToPrint = getRecord(ISA, GS)

            If blnWriteToFile Then
                strFilePath = strFilePath?.Trim
                If Not Right(strFilePath, 1) = "\" Then strFilePath &= "\"
                Dim FileName As String = strFilePath & "990" & Date.Now.ToString("ddMMHHmmss") & ".txt"
                If Not Directory.Exists(strFilePath) Then Directory.CreateDirectory(strFilePath)

                Dim fi As FileInfo = New FileInfo(FileName)
                'create the file if it does not exist
                If Not File.Exists(FileName) Then
                    Using w As StreamWriter = fi.CreateText
                        w.Close()
                    End Using
                End If
                'now open the file and wite the data
                Using sw As New StreamWriter(FileName)
                    sw.Write(strToPrint)
                    sw.Flush()
                End Using
            End If
        Catch ex As Exception
            'do nothing for test
        End Try
        Return strToPrint
    End Function

    Public Function writeRecord(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS, Optional ByVal strFilePath As String = "C:\Data\EDI\Test\") As Boolean
        'For testing we just write the data to a file
        Try
            strFilePath = strFilePath?.Trim
            If Not Right(strFilePath, 1) = "\" Then strFilePath &= "\"
            If Not Directory.Exists(strFilePath) Then Directory.CreateDirectory(strFilePath)
            Dim FileName As String = strFilePath & "990Record" & Date.Now.ToString("ddMMHHmmss") & ".txt"

            Dim fi As FileInfo = New FileInfo(FileName)
            'create the file if it does not exist
            If Not File.Exists(FileName) Then
                Using w As StreamWriter = fi.CreateText
                    w.Close()
                End Using
            End If
            'now open the file and wite the data
            Using sw As New StreamWriter(FileName)
                sw.Write(getRecord(ISA, GS))
                sw.Flush()
            End Using
        Catch ex As Exception
            'do nothing for test
        End Try
        Return True
    End Function

    Public Function getRecord(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS) As String
        'Format the record for reporting.
        Dim strToPrint As String = "*************** 990 *********************" & vbCrLf
        strToPrint &= "ISA Sender: " & ISA.ISA06 & vbCrLf
        strToPrint &= "GS Sender: " & GS.GS02 & vbCrLf
        strToPrint &= "GS Date: " & GS.GS04 & vbCrLf
        strToPrint &= "GS Time: " & GS.GS05 & vbCrLf
        strToPrint &= "GS Group Control: " & GS.GS06 & vbCrLf
        strToPrint &= "ST: " & ST.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "B1: " & B1.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "K1: " & K1.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "N9: " & N9.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "SE: " & SE.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "*****************************************" & vbCrLf
        strToPrint &= vbCrLf

        Return strToPrint

    End Function

    ''' <summary>
    ''' Creates an EDI 990 document string
    ''' </summary>
    ''' <param name="strISA"></param>
    ''' <param name="strGS"></param>
    ''' <param name="strIEA"></param>
    ''' <param name="strGE"></param>
    ''' <param name="SegmentTerminator"></param>
    ''' <returns>String</returns>
    ''' <remarks>
    ''' Added by LVV on 4/11/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Function getEDIRecord(ByVal strISA As String, ByVal strGS As String, ByVal strIEA As String, ByVal strGE As String, ByVal SegmentTerminator As String) As String
        'Format the edi record
        Dim strToPrint As String = ""
        strToPrint &= strISA
        strToPrint &= strGS
        strToPrint &= ST.getEDIString(SegmentTerminator)
        strToPrint &= B1.getEDIString(SegmentTerminator)
        strToPrint &= K1.getEDIString(SegmentTerminator)
        strToPrint &= N9.getEDIString(SegmentTerminator)
        strToPrint &= SE.getEDIString(SegmentTerminator)
        strToPrint &= strGE
        strToPrint &= strIEA

        Return strToPrint

    End Function

    Public Function getEDI204In990Response(ByVal strISA As String, ByVal strGS As String, ByVal strIEA As String, ByVal strGE As String, ByVal SegmentTerminator As String) As String
        'Format the edi record
        Dim strToPrint As String = ""
        strToPrint &= strISA
        strToPrint &= strGS
        strToPrint &= ST.getEDIString(SegmentTerminator)
        strToPrint &= B1.getEDIString(SegmentTerminator)
        strToPrint &= K1.getEDIString(SegmentTerminator)
        strToPrint &= SE.getEDIString(SegmentTerminator)
        strToPrint &= strGE
        strToPrint &= strIEA

        Return strToPrint

    End Function


    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
        'add the AK9 to ST elements if they exist
        For isubsegs As Integer = 2 To 0 Step -1
            Dim segs() As String
            Select Case isubsegs
                Case 0
                    'split out any unwanted elements 
                    Dim sElems() As String = strSource.Split(strSegSep)
                    'read the ST record (it should be all that is left  
                    If Left(sElems(0), 3) <> "ST*" Then sElems(0) = "ST*" & sElems(0)
                    ST = New clsEDIST(sElems(0))
                Case 1
                    'read the AK1 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "B1\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        B1 = New clsEDIB1("B1*" & sElems(0))
                        strSource = segs(0)
                    End If
                Case 2
                    'read the AK2 record
                    segs = Regex.Split(strSource, "\" & strSegSep & "N9\*")
                    If segs.Length > 1 Then
                        'split out any unwanted elements 
                        Dim sElems() As String = segs(1).Split(strSegSep)
                        N9 = New clsEDIN9("N9*" & sElems(0))
                        strSource = segs(0)
                    End If
            End Select
        Next
        'Add the SE
        If Not String.IsNullOrEmpty(strSEElem) Then
            'split out any unwanted elements 
            Dim sElems() As String = strSEElem.Split(strSegSep)
            If Left(sElems(0), 3) <> "SE*" Then sElems(0) = "SE*" & sElems(0)
            SE = New clsEDISE(sElems(0))
        End If
    End Sub
End Class

#End Region

#Region "clsOrderNumberSeq"
Public Class clsOrderNumberSeq
    Public BookCarrOrderNumber As String = ""
    Public BookOrderSequence As Integer = 0


End Class
#End Region

'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
#Region " R3 - Route Information - Motor  "

Public Class clsEDIR3
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 1 To sSegs.Length - 1
            'we skip the first item because it is R3
            Select Case i
                Case 1
                    R301 = sSegs(i)
                Case 2
                    R302 = sSegs(i)
                Case 3
                    R303 = sSegs(i)
                Case 4
                    R304 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 5 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "R3")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = R301
            Case 2
                strRet = R302
            Case 3
                strRet = R303
            Case 4
                strRet = R304
        End Select

        Return strRet
    End Function

    Private _R301 As String = "" 'Standard Carrier Alpha Code
    Public Property R301() As String
        Get

            Return Left(_R301, 4)

        End Get
        Set(ByVal value As String)
            _R301 = value
        End Set
    End Property

    Private _R302 As String = "" 'Routing Sequence Code (B - Origin/Delivery Carrier (Any Mode))
    Public Property R302() As String
        Get

            Return Left(_R302, 2)

        End Get
        Set(ByVal value As String)
            _R302 = value
        End Set
    End Property

    Private _R303 As String = "" ' (Not Used)
    Public Property R303() As String
        Get

            Return Left(_R303, 2)

        End Get
        Set(ByVal value As String)
            _R303 = value
        End Set
    End Property

    Private _R304 As String = "" 'Mode (LTL - Less than Truck, TL - Truckload, R - Rail)
    Public Property R304() As String
        Get

            Return Left(_R304, 3)

        End Get
        Set(ByVal value As String)
            _R304 = value
        End Set
    End Property

End Class

#End Region

#Region " REF - Reference Identification - Payment Reference Number  "

Public Class clsEDIREF
    Public Sub New()
        MyBase.New()
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="refIDQual"></param>
    ''' <param name="paymentRefNo"></param>
    ''' <remarks>
    ''' Added by LVV on 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub New(ByVal refIDQual As String, ByVal paymentRefNo As String)
        MyBase.New()
        REF01 = refIDQual
        REF02 = paymentRefNo
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 0 To sSegs.Length - 1
            Select Case i
                Case 0
                    REF01 = sSegs(i)
                Case 1
                    REF02 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 5 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "REF")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = REF01
            Case 2
                strRet = REF02
        End Select

        Return strRet
    End Function

    Private _REF01 As String = "" 'Reference Identification Qualifier
    Public Property REF01() As String
        Get

            Return Left(_REF01, 3)

        End Get
        Set(ByVal value As String)
            _REF01 = value
        End Set
    End Property

    Private _REF02 As String = "" 'Reference Identification (Payment reference provided by Syncada - ACH I think)
    Public Property REF02() As String
        Get

            Return Left(_REF02, 30)

        End Get
        Set(ByVal value As String)
            _REF02 = value
        End Set
    End Property

End Class

#End Region

#Region " DTM - DATE/TIME Reference - Settlement Date  "

Public Class clsEDIDTM
    Public Sub New()
        MyBase.New()
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dtQual"></param>
    ''' <param name="dtDate"></param>
    ''' <remarks>
    ''' Added by LVV on 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub New(ByVal dtQual As String, ByVal dtDate As String)
        MyBase.New()
        DTM01 = dtQual
        DTM02 = dtDate
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 0 To sSegs.Length - 1
            Select Case i
                Case 0
                    DTM01 = sSegs(i)
                Case 1
                    DTM02 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 5 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "DTM")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = DTM01
            Case 2
                strRet = DTM02
        End Select

        Return strRet
    End Function

    Private _DTM01 As String = "" 'max 3 DATE QUALIFIER (234 - Settlement Date)
    Public Property DTM01() As String
        Get

            Return Left(_DTM01, 3)

        End Get
        Set(ByVal value As String)
            _DTM01 = value
        End Set
    End Property

    Private _DTM02 As String = "" 'max 8 DATE YYYYMMDD
    Public Property DTM02() As String
        Get

            Return Left(_DTM02, 8)

        End Get
        Set(ByVal value As String)
            _DTM02 = value
        End Set
    End Property

End Class

#End Region

#Region " RMR - Remittance Advice Accounts Receivable Open Item Reference  "

Public Class clsEDIRMR
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="refIDQual"></param>
    ''' <param name="refID"></param>
    ''' <param name="paymentAmt"></param>
    ''' <remarks>
    ''' Added by LVV on 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Sub New(ByVal refIDQual As String, ByVal refID As String, ByVal paymentAmt As String)
        MyBase.New()
        RMR01 = refIDQual
        RMR02 = refID
        RMR04 = paymentAmt
    End Sub

    Public Sub New(ByVal strSegment As String)
        MyBase.New()
        Dim sSegs() As String = Regex.Split(strSegment, "\*")
        For i As Integer = 0 To sSegs.Length - 1
            Select Case i
                Case 0
                    RMR01 = sSegs(i)
                Case 1
                    RMR02 = sSegs(i)
                Case 2
                    RMR03 = sSegs(i)
                Case 3
                    RMR04 = sSegs(i)
            End Select
        Next
    End Sub

    Public Function getEDIString(ByVal sSegTerm As String) As String
        'the string is built in reverse order 
        Dim blnPreviousFound As Boolean = False 'Used to identify if a previous match was found.  Once a match is found all subsequent items must be inserted
        Dim sEdi As New System.Text.StringBuilder("", 500)
        For i As Integer = 5 To 1 Step -1
            Dim strVal As String = getDataByIndex(i)
            If blnPreviousFound OrElse strVal?.Trim.Length > 0 Then sEdi.Insert(0, String.Format("*{0}", strVal)) : blnPreviousFound = True
        Next
        If blnPreviousFound Then
            sEdi.Append(sSegTerm)
            sEdi.Insert(0, "RMR")
        End If

        Return sEdi.ToString
    End Function

    Private Function getDataByIndex(ByVal index As Integer) As String
        Dim strRet As String = ""
        Select Case index
            Case 1
                strRet = RMR01
            Case 2
                strRet = RMR02
            Case 3
                strRet = RMR03
            Case 4
                strRet = RMR04
        End Select

        Return strRet
    End Function

    Private _RMR01 As String = "" 'Reference Identification Qualifier (CN = Carrier's Reference Number - PRO/Invoice) 
    Public Property RMR01() As String
        Get

            Return Left(_RMR01, 3)

        End Get
        Set(ByVal value As String)
            _RMR01 = value
        End Set
    End Property

    Private _RMR02 As String = "" 'Reference Identification (PRO/Invoice)
    Public Property RMR02() As String
        Get

            Return Left(_RMR02, 30)

        End Get
        Set(ByVal value As String)
            _RMR02 = value
        End Set
    End Property

    Private _RMR03 As String = "" 'I don't know what this is. Not in USB documentation. Place holder
    Public Property RMR03() As String
        Get

            Return Left(_RMR03, 3)

        End Get
        Set(ByVal value As String)
            _RMR03 = value
        End Set
    End Property

    Private _RMR04 As String = "" 'Monetary Amount (Individual transaction amount)
    Public Property RMR04() As String
        Get

            Return Left(_RMR04, 18)

        End Get
        Set(ByVal value As String)
            _RMR04 = value
        End Set
    End Property

End Class

#End Region

#Region "820 RMR Loop"

Public Class clsEDI820RMRLoop

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strSource As String, ByVal strSegSep As String)
        MyBase.New()
        addDataFromString(strSource, strSegSep)

    End Sub

    Public RMR As New clsEDIRMR

    Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String)
        'split out any unwanted elements 
        Dim sElems() As String = strSource.Split(strSegSep)
        'read the RMR record (it should be all that is left)
        If Left(sElems(0), 3) <> "RMR*" Then sElems(0) = "RMR*" & sElems(0)
        RMR = New clsEDIRMR(sElems(0))
    End Sub
End Class

#End Region

#Region " EDI 820 Class"

Public Class clsEDI820

    Public LastError As String = ""
    Public ST As New clsEDIST
    Public REF As New clsEDIREF
    Public DTM As New clsEDIDTM
    Public LoopRMR() As clsEDIRMR
    Public SE As New clsEDISE

    Public AlertList As New Dictionary(Of Integer, String)

    Public Sub New()
        MyBase.New()

    End Sub

    'Public Sub New(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
    '    MyBase.New()
    '    addDataFromString(strSource, strSegSep, strSEElem)
    'End Sub

    Public Function processData(ByVal ISA As clsEDIISA,
                               ByVal GS As clsEDIGS,
                               ByVal strDBServer As String,
                               ByVal strDatabase As String,
                               ByRef RecordErrors As Integer,
                               ByRef TotalRecords As Integer,
                               ByVal fileName As String) As Boolean
        Dim blnRet As Boolean = True
        Try
            Dim InvoiceNumber As String = ""
            Dim PayDate820 As Date
            Dim NullablePayDate820 As Date?
            Dim PayAmt820 As Decimal = 0
            Dim ACH820 As String = ""
            Dim strLogMsg820Header As String = ""
            Dim o820List = New List(Of cls820Params)
            Dim oWCFPar = New DAL.WCFParameters() With {.Database = strDatabase,
                                                              .DBServer = strDBServer,
                                                              .UserName = "EDI Integration",
                                                              .WCFAuthCode = "NGLSystem"}
            Dim oEDIData As New DAL.NGLEDIData(oWCFPar)
            Dim oBatchData As New DAL.NGLBatchProcessDataProvider(oWCFPar)

            'The ACH check number is found in header REF02 where REF01 = EM
            If REF.REF01?.Trim() = "EM" Then
                ACH820 = REF.REF02?.Trim()
            Else
                strLogMsg820Header += "REF01 not set to qualifier EM - the ACH number could not be read. "
            End If

            'The Settlement Date is found in DTM02 where DTM01 = 234
            If DTM.DTM01?.Trim() = "234" Then
                Dim dp = Ngl.Core.Utility.DataTransformation.convertEDIDateToDateString(DTM.DTM02?.Trim())
                If Date.TryParse(dp, PayDate820) Then
                    NullablePayDate820 = PayDate820
                Else
                    strLogMsg820Header += "Unable to parse the data in DTM02 (Date Paid) from type String to type Date . "
                End If
            Else
                strLogMsg820Header += "DTM01 not set to qualifier 234 - the settlement date could not be read. "
            End If

            If Not LoopRMR Is Nothing Then
                For Each oRMR As clsEDIRMR In LoopRMR
                    Dim o820 As New cls820Params
                    Dim strLogMsg820Detail As String = ""

                    'Every invoice included in this 820 has the same pay date and check number
                    o820.PayDate820 = NullablePayDate820
                    o820.ACH820 = ACH820

                    'The Invoice Number is found in RMR02 Where RMR01 = CN
                    If oRMR.RMR01?.Trim() = "CN" Then
                        o820.InvoiceNumber = oRMR.RMR02?.Trim()
                    Else
                        strLogMsg820Detail += "RMR01 not set to qualifier CN - the Invoice Number could not be read. "
                    End If
                    'The Amount Paid for the invoice is found in RMR04
                    Dim dec As Decimal = 0
                    If Decimal.TryParse(oRMR.RMR04?.Trim(), dec) Then
                        o820.PayAmt820 = dec
                    Else
                        strLogMsg820Detail += "Unable to parse the data in RMR04 (Amount Paid) from type String to type Decimal. "
                    End If
                    o820.strLogMessage820 = strLogMsg820Header + strLogMsg820Detail
                    o820List.Add(o820)
                Next
            Else
                'If there are no RMRs then we can't know what got paid. Send an alert
                Dim strAlertMsg = "The RMR Loop was not populated or could not be read. Unable to read Invoice Number or Payment information. 820 Processing failed for the 820 with ISA13: " + ISA.ISA13 + " GS06: " + GS.GS06 + "."
                oBatchData.executeInsertAlertMessage("AlertProcessEDI820Error", 0, 0, 0, 0, "Failed to Process EDI 820", strAlertMsg, "", "", "", "", "")
                'oSecData.InsertAlertMessageWithEmail("AlertProcessEDI820Error", 0, 0, 0, 0, "Failed to Process EDI 820", strAlertMsg, "", "", "", "", "")
                blnRet = False
                Return blnRet
            End If

            For Each p In o820List
                'run sp
                If oEDIData.Process820(p.InvoiceNumber, p.PayDate820, p.PayAmt820, p.ACH820, p.strLogMessage820, fileName) Then
                    TotalRecords += 1
                Else
                    RecordErrors += 1
                End If
            Next

            If RecordErrors > 0 Then blnRet = False

            Return blnRet
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Creates an EDI 820 document string
    ''' </summary>
    ''' <param name="strISA"></param>
    ''' <param name="strGS"></param>
    ''' <param name="strIEA"></param>
    ''' <param name="strGE"></param>
    ''' <param name="SegmentTerminator"></param>
    ''' <returns>String</returns>
    ''' <remarks>
    ''' Added by LVV on 4/19/16 for v-7.0.5.1 EDI Sim Testing Tool
    ''' </remarks>
    Public Function getEDIRecord(ByVal strISA As String, ByVal strGS As String, ByVal strIEA As String, ByVal strGE As String, ByVal SegmentTerminator As String) As String
        'Format the edi record
        Dim strToPrint As String = ""
        strToPrint &= strISA
        strToPrint &= strGS
        strToPrint &= ST.getEDIString(SegmentTerminator)
        strToPrint &= REF.getEDIString(SegmentTerminator)
        strToPrint &= DTM.getEDIString(SegmentTerminator)
        If Not LoopRMR Is Nothing Then
            For Each L As clsEDIRMR In LoopRMR
                strToPrint &= L.getEDIString(SegmentTerminator)
            Next
        End If
        strToPrint &= SE.getEDIString(SegmentTerminator)
        strToPrint &= strGE
        strToPrint &= strIEA

        Return strToPrint

    End Function

    Public Function testData(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS) As Boolean
        'For testing we just write the data to a file
        Try
            Dim strFilePath As String = "C:\Data\EDI\Test\"
            If Not Directory.Exists(strFilePath) Then Directory.CreateDirectory(strFilePath)
            Dim FileName As String = strFilePath & "820" & Date.Now.ToString("ddMMHHmmss") & ".txt"

            Dim fi As FileInfo = New FileInfo(FileName)
            'create the file if it does not exist
            If Not File.Exists(FileName) Then
                Using w As StreamWriter = fi.CreateText
                    w.Close()
                End Using
            End If
            'now open the file and wite the data
            Using sw As New StreamWriter(FileName)
                sw.Write(getRecord(ISA, GS))
                sw.Flush()
            End Using
        Catch ex As Exception
            'do nothing for test
        End Try
        Return True
    End Function

    Public Function writeRecord(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS, Optional ByVal strFilePath As String = "C:\Data\EDI\Test\") As Boolean
        'For testing we just write the data to a file
        Try
            strFilePath = strFilePath?.Trim
            If Not Right(strFilePath, 1) = "\" Then strFilePath &= "\"
            If Not Directory.Exists(strFilePath) Then Directory.CreateDirectory(strFilePath)
            Dim FileName As String = strFilePath & "820Record" & Date.Now.ToString("ddMMHHmmss") & ".txt"

            Dim fi As FileInfo = New FileInfo(FileName)
            'create the file if it does not exist
            If Not File.Exists(FileName) Then
                Using w As StreamWriter = fi.CreateText
                    w.Close()
                End Using
            End If
            'now open the file and wite the data
            Using sw As New StreamWriter(FileName)
                sw.Write(getRecord(ISA, GS))
                sw.Flush()
            End Using
        Catch ex As Exception
            'do nothing for test
        End Try
        Return True
    End Function

    Public Function getRecord(ByVal ISA As clsEDIISA, ByVal GS As clsEDIGS) As String
        'Format the record for reporting.
        Dim strToPrint As String = "*************** 820 *********************" & vbCrLf
        strToPrint &= "ISA Sender: " & ISA.ISA06 & vbCrLf
        strToPrint &= "GS Sender: " & GS.GS02 & vbCrLf
        strToPrint &= "GS Date: " & GS.GS04 & vbCrLf
        strToPrint &= "GS Time: " & GS.GS05 & vbCrLf
        strToPrint &= "GS Group Control: " & GS.GS06 & vbCrLf
        strToPrint &= "ST: " & ST.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "REF: " & REF.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "DTM: " & DTM.getEDIString(ISA.SegmentTerminator) & vbCrLf
        If Not LoopRMR Is Nothing Then
            For Each L As clsEDIRMR In LoopRMR
                strToPrint &= "RMR: " & L.getEDIString(ISA.SegmentTerminator) & vbCrLf
            Next
        End If
        strToPrint &= "SE: " & SE.getEDIString(ISA.SegmentTerminator) & vbCrLf
        strToPrint &= "*****************************************" & vbCrLf
        strToPrint &= vbCrLf

        Return strToPrint

    End Function

    'Public Sub addDataFromString(ByVal strSource As String, ByVal strSegSep As String, ByVal strSEElem As String)
    '    'add the AK9 to ST elements if they exist
    '    For isubsegs As Integer = 9 To 0 Step -1
    '        Dim segs() As String
    '        Select Case isubsegs
    '            Case 0
    '                'split out any unwanted elements 
    '                Dim sElems() As String = strSource.Split(strSegSep)
    '                'read the ST record (it should be all that is left  
    '                If Left(sElems(0), 3) <> "ST*" Then sElems(0) = "ST*" & sElems(0)
    '                ST = New clsEDIST(sElems(0))
    '            Case 1
    '                'read the AK1 record
    '                segs = Regex.Split(strSource, "\" & strSegSep & "AK1\*")
    '                If segs.Length > 1 Then
    '                    'split out any unwanted elements 
    '                    Dim sElems() As String = segs(1).Split(strSegSep)
    '                    AK1 = New clsEDIAK1("AK1*" & sElems(0))
    '                    strSource = segs(0)
    '                End If
    '            Case 2
    '                'read the AK2 record
    '                segs = Regex.Split(strSource, "\" & strSegSep & "AK2\*")
    '                If segs.Length > 1 Then
    '                    'split out any unwanted elements 
    '                    Dim sElems() As String = segs(1).Split(strSegSep)
    '                    AK2 = New clsEDIAK2("AK2*" & sElems(0))
    '                    strSource = segs(0)
    '                End If
    '            Case 3
    '                'read the AK3 record
    '                segs = Regex.Split(strSource, "\" & strSegSep & "AK3\*")
    '                If segs.Length > 1 Then
    '                    'split out any unwanted elements 
    '                    Dim sElems() As String = segs(1).Split(strSegSep)
    '                    AK3 = New clsEDIAK3("AK3*" & sElems(0))
    '                    strSource = segs(0)
    '                End If
    '            Case 4
    '                'read the AK4 record
    '                segs = Regex.Split(strSource, "\" & strSegSep & "AK4\*")
    '                If segs.Length > 1 Then
    '                    'split out any unwanted elements 
    '                    Dim sElems() As String = segs(1).Split(strSegSep)
    '                    AK4 = New clsEDIAK4("AK4*" & sElems(0))
    '                    strSource = segs(0)
    '                End If
    '            Case 5
    '                'read the AK5 record
    '                segs = Regex.Split(strSource, "\" & strSegSep & "AK5\*")
    '                If segs.Length > 1 Then
    '                    'split out any unwanted elements 
    '                    Dim sElems() As String = segs(1).Split(strSegSep)
    '                    AK5 = New clsEDIAK5("AK5*" & sElems(0))
    '                    strSource = segs(0)
    '                End If
    '            Case 9
    '                'read the AK9 record
    '                segs = Regex.Split(strSource, "\" & strSegSep & "AK9\*")
    '                If segs.Length > 1 Then
    '                    'split out any unwanted elements 
    '                    Dim sElems() As String = segs(1).Split(strSegSep)
    '                    AK9 = New clsEDIAK9("AK9*" & sElems(0))
    '                    strSource = segs(0)
    '                End If
    '        End Select
    '    Next
    '    'Add the SE
    '    If Not String.IsNullOrEmpty(strSEElem) Then
    '        'split out any unwanted elements 
    '        Dim sElems() As String = strSEElem.Split(strSegSep)
    '        If Left(sElems(0), 3) <> "SE*" Then sElems(0) = "SE*" & sElems(0)
    '        SE = New clsEDISE(sElems(0))
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' This method only uses the AK1 and AK9 segments
    ' ''' </summary>
    ' ''' <param name="sSegTerm"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function getEDIString(ByVal sSegTerm As String) As String
    '    Return ST.getEDIString(sSegTerm) _
    '        & AK1.getEDIString(sSegTerm) _
    '        & AK9.getEDIString(sSegTerm) _
    '        & SE.getEDIString(sSegTerm)

    'End Function
End Class

#End Region

#Region "210 Outbound Loop 100"

Public Class clsEDI210OutLoop100

    Public N1 As New clsEDIN1(True)
    Public N2 As New clsEDIN2
    Public N3 As New clsEDIN3
    Public N4 As New clsEDIN4(True)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal N101 As String,
                   ByVal N102 As String,
                   ByVal N103 As String,
                   ByVal N104 As String,
                   Optional ByVal Address1 As String = "",
                   Optional ByVal Address2 As String = "",
                   Optional ByVal City As String = "",
                   Optional ByVal State As String = "",
                   Optional ByVal Zip As String = "",
                   Optional ByVal Country As String = "")
        MyBase.New()
        N1 = New clsEDIN1(True) With {.N101 = Left(N101, 3), .N102 = Left(cleanEDI(nz(N102, "")), 60), .N103 = Left(N103, 2), .N104 = N104}
        If Not Address1 Is Nothing OrElse Address1?.Trim.Length > 0 Then
            N3 = New clsEDIN3 With {.N301 = Left(cleanEDI(nz(Address1, "")), 55), .N302 = Left(cleanEDI(nz(Address2, "")), 55)}
            N4 = New clsEDIN4(True) With {.N401 = Left(cleanEDI(nz(City, "")), 30), .N402 = Left(cleanEDI(nz(State, "")), 2), .N403 = Left(cleanEDI(nz(Zip, "")), 15), .N404 = Left(cleanEDI(nz(Country, "")), 3)}
        End If
    End Sub

End Class

#End Region

#Region "210 Outbound Loop 400"

Public Class clsEDI210OutLoop400

    Public LX As New clsEDILX
    Public L5 As New clsEDIL5
    Public L0 As New clsEDIL0(True)
    Public L1 As New clsEDIL1(True)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal LX01 As String,
                   ByVal L501 As String,
                   ByVal L502 As String,
                   ByVal L504 As String,
                   ByVal L001 As String,
                   ByVal L002 As String,
                   ByVal L003 As String,
                   ByVal L004 As String,
                   ByVal L005 As String,
                   ByVal L008 As String,
                   ByVal L009 As String,
                   ByVal L101 As String,
                   ByVal L102 As String,
                   ByVal L103 As String,
                   ByVal L104 As String,
                   ByVal L108 As String)
        MyBase.New()
        LX = New clsEDILX With {.LX01 = LX01}
        L5 = New clsEDIL5 With {.L501 = L501, .L502 = L502, .L504 = L504}
        L0 = New clsEDIL0(True) With {.L001 = L001, .L002 = L002, .L003 = L003, .L004 = L004, .L005 = L005, .L008 = L008, .L009 = L009}
        L1 = New clsEDIL1(True) With {.L101 = L101, .L102 = L102, .L103 = L103, .L104 = L104, .L108 = L108}
    End Sub

End Class

#End Region

#Region "210 Outbound Loop N9 "

Public Class clsEDI210OutLoopN9

    Public N9 As New clsEDIN9(True)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal N901 As String,
                   ByVal N902 As String)
        MyBase.New()
        N9 = New clsEDIN9(True) With {.N901 = N901, .N902 = N902}

    End Sub

End Class

#End Region

#Region " 210 Outbound Invoice "

Public Class clsEDI210Invoice
    Public ST As New clsEDIST(True)
    Public B3 As New clsEDIB3(True)
    Public C3 As New clsEDIC3(True)
    Public LoopN9() As clsEDI210OutLoopN9
    Public G62 As New clsEDIG62
    Public R3 As New clsEDIR3
    Public Loop100() As clsEDI210OutLoop100
    Public Loop400() As clsEDI210OutLoop400
    Public L3 As New clsEDIL3
    Public SE As New clsEDISE(True)

End Class

#End Region

Public Class cls820Params
    Public InvoiceNumber As String = ""
    Public PayDate820 As Date?
    Public PayAmt820 As Decimal = 0
    Public ACH820 As String = ""
    Public strLogMessage820 As String = ""
End Class


Public Class clsISAGSInfo
    Public sSegTerm As String = ""
    Public sGroupHeader As String = ""
    Public sSenderPartnerID As String = ""
    Public sReceiverPartnerID As String = ""
    Public dtTransmission As Date? = Nothing
End Class

''' <summary>
''' EDI Address Information Class
''' </summary>
''' <remarks>
''' Modified by RHR for v-7.0.6.105 on 6/13/2017
''' </remarks>
Public Class clsAddressInfo
    Public CompNumber As String = "0"
    Public CompLegalEntity As String = ""
    Public CompAlphaCode As String = ""
    Public CompLocationID As String = ""
    Public CompAbrev As String = ""
    Public AddrName As String = ""
    Public Addr1 As String = ""
    Public Addr2 As String = ""
    Public Addr3 As String = ""
    Public City As String = ""
    Public State As String = ""
    Public Zip As String = ""
    Public Country As String = "US"
    Public ContactName As String = ""
    Public ContactFAX As String = ""
    Public ContactPhone As String = ""
    Public ContactPhoneExt As String = ""
    Public oItems As New List(Of clsItemDetails)
    Public OrderNumber As String
    Public CustomerPO As String
    Public Pickup As Boolean
    Public StopSequence As Integer
    Public pickOrDropOrder As Integer
    Public laneComments As String = ""
    Public ShipInstructions As String = ""
    Public LaneNumber As String = ""
    Public HazmatCode As String = ""
    Private _StopShipDates As New clsEDIShipDates()
    Public Property StopShipDates() As clsEDIShipDates
        Get
            Return _StopShipDates
        End Get
        Set(ByVal value As clsEDIShipDates)
            _StopShipDates = value
        End Set
    End Property

    Public Function isAddressEmpty() As Boolean
        Dim blnRet As Boolean = False
        If String.IsNullOrWhiteSpace(Addr1) OrElse String.IsNullOrWhiteSpace(City) OrElse String.IsNullOrWhiteSpace(State) OrElse String.IsNullOrWhiteSpace(Zip) Then blnRet = True
        Return blnRet
    End Function

    ''' <summary>
    ''' populates the Address Info with Loop 310 data
    ''' </summary>
    ''' <param name="o310"></param>
    ''' <remarks>
    ''' Created by RHR for v-6.0.4.7 on 6/8/2017
    ''' </remarks>
    Public Sub populateAddressInfo(ByVal o310 As clsEDI204InLoop310, ByVal oEDI204InSetting As clsEDI204InSetting)

        If o310 Is Nothing Then Return
        Me.CompAbrev = If(String.IsNullOrWhiteSpace(oEDI204InSetting.MappingRules.CompAbrev), "", oEDI204InSetting.MappingRules.CompAbrev.Trim())
        Me.CompLocationID = If(String.IsNullOrWhiteSpace(o310.N1.N104), "", o310.N1.N104.Trim())
        Me.CompAlphaCode = Me.CompAbrev & Me.CompLocationID
        If String.IsNullOrEmpty(Me.CompLegalEntity) Then
            Me.CompLegalEntity = oEDI204InSetting.MappingRules.CompLegalEntity
        End If
        Me.AddrName = If(String.IsNullOrWhiteSpace(o310.N1.N102), "", o310.N1.N102.Trim())
        If Not o310.N3s Is Nothing AndAlso o310.N3s.Count() > 0 Then

            Me.Addr1 = If(String.IsNullOrWhiteSpace(o310.N3s(0).N301), "", o310.N3s(0).N301.Trim())
            Me.Addr2 = If(String.IsNullOrWhiteSpace(o310.N3s(0).N302), "", o310.N3s(0).N302.Trim())
            If o310.N3s.Count() > 1 Then
                Me.Addr3 = Left(If(String.IsNullOrWhiteSpace(o310.N3s(1).N301), "", o310.N3s(1).N301.Trim()) & " " & If(String.IsNullOrWhiteSpace(o310.N3s(1).N302), "", o310.N3s(1).N302.Trim()), 40)
            End If
        End If
        Me.City = If(String.IsNullOrWhiteSpace(o310.N4.N401), "", o310.N4.N401.Trim())
        Me.State = If(String.IsNullOrWhiteSpace(o310.N4.N402), "", o310.N4.N402.Trim())
        Me.Zip = If(String.IsNullOrWhiteSpace(o310.N4.N403), "", o310.N4.N403.Trim())
        Me.Country = If(String.IsNullOrWhiteSpace(o310.N4.N404), "", o310.N4.N404.Trim())
        If String.IsNullOrWhiteSpace(Me.Country) Then Me.Country = "US"
        If Not o310.G61s Is Nothing AndAlso o310.G61s.Count() > 0 Then
            For Each g61 In o310.G61s
                If Not String.IsNullOrWhiteSpace(g61.G6102) Then
                    Me.ContactName = g61.G6102.Trim()
                End If
                Select Case If(String.IsNullOrWhiteSpace(g61.G6103), "", g61.G6103.Trim())
                    Case "FX"
                        Me.ContactFAX = If(String.IsNullOrWhiteSpace(g61.G6104), "", g61.G6104.Trim())
                    Case "TE"
                        Me.ContactPhone = If(String.IsNullOrWhiteSpace(g61.G6104), "", g61.G6104.Trim())
                    Case "EX"
                        Me.ContactPhoneExt = If(String.IsNullOrWhiteSpace(g61.G6104), "", g61.G6104.Trim())
                End Select
            Next
        End If
    End Sub
End Class

''' <summary>
''' EDI 204 in Item Detail Information
''' </summary>
''' <remarks>
'''  Modified by RHR for v-7.0.6.105 on 6/13/2017
''' </remarks>
Public Class clsItemDetails
    Public ItemNumber As String = ""
    Public ItemPONumber As String = ""
    Public OrderSequence As Integer = 0
    Public ItemCompNumber As String = ""
    Public Qty As Integer = 0
    Public Wgt As Double = 0
    Public Plts As Double = 0
    Public Cubes As Integer = 0
    Public Description As String = ""
    Public Temp As String = ""
    Public HazmatCode As String = ""
    Public ItemCost As Double = 0
    Public LotNumber As String = ""
    Public POItemCustomerPO As String = ""
    Public POItemLocationCode As String = ""

End Class

''' <summary>
''' EDI 204 in in Shiping Date Object
''' </summary>
''' <remarks>
'''  Modified by RHR for v-7.0.6.105 on 6/13/2017
''' </remarks>
Public Class clsEDIShipDates
    Public OrderDate As String
    Public ReqDate As String = ""
    Public ShipDate As String = ""
    Public SchedulePUDate As String = ""
    Public ScheduleDelDate As String = ""
    Public SchedulePUTime As String = ""
    Public ScheduleDelTime As String = ""
End Class

''' <summary>
''' EDI 204 in Header Information
''' </summary>
''' <remarks>
'''  Modified by RHR for v-7.0.6.105 on 6/13/2017
''' </remarks>
Public Class clsHeaderInfo
    Public POStatusFlag As Integer
    Public laneComments As String = ""
    Public ShipInstructions As String = ""
    Public OrderNumber As String = ""
    Public OrderSequence As Integer = 0
    Public PONumber As String = ""
    Public LaneNumber As String = ""
    Public DestAdd As clsAddressInfo
    Public OrigAdd As clsAddressInfo
    Public Inbound As Boolean = False
    Public POHDRPallets As String
    Public ModeTypeControl As Integer = 3 'Default 3 Truck
    Public temp As String = ""
    'values
    'F = 1 for Frozen
    'R = 2 for Refrigerated 35-38
    'D = 3 for Dry
    'M = 4 for Mixed Frozen And Dry
    'H = 5 for Hazmat
    'G = 6 for General
    'U = 7 for Unknown
    'C = 8 Cooler
    Public TransCode As String = ""

    Private _HeaderShipDates As New clsEDIShipDates()
    Public Property HeaderShipDates() As clsEDIShipDates
        Get
            Return _HeaderShipDates
        End Get
        Set(ByVal value As clsEDIShipDates)
            _HeaderShipDates = value
        End Set
    End Property

    Public Property OrderDate() As String
        Get
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            Return HeaderShipDates.OrderDate
        End Get
        Set(ByVal value As String)
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            HeaderShipDates.OrderDate = value
        End Set
    End Property

    Public Property ReqDate() As String
        Get
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            Return HeaderShipDates.ReqDate
        End Get
        Set(ByVal value As String)
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            HeaderShipDates.ReqDate = value
        End Set
    End Property

    Public Property ShipDate() As String
        Get
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            Return HeaderShipDates.ShipDate
        End Get
        Set(ByVal value As String)
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            HeaderShipDates.ShipDate = value
        End Set
    End Property

    Public Property SchedulePUDate() As String
        Get
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            Return HeaderShipDates.SchedulePUDate
        End Get
        Set(ByVal value As String)
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            HeaderShipDates.SchedulePUDate = value
        End Set
    End Property

    Public Property ScheduleDelDate() As String
        Get
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            Return HeaderShipDates.ScheduleDelDate
        End Get
        Set(ByVal value As String)
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            HeaderShipDates.ScheduleDelDate = value
        End Set
    End Property

    Public Property SchedulePUTime() As String
        Get
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            Return HeaderShipDates.SchedulePUTime
        End Get
        Set(ByVal value As String)
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            HeaderShipDates.SchedulePUTime = value
        End Set
    End Property

    Public Property ScheduleDelTime() As String
        Get
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            Return HeaderShipDates.ScheduleDelTime
        End Get
        Set(ByVal value As String)
            If HeaderShipDates Is Nothing Then HeaderShipDates = New clsEDIShipDates
            HeaderShipDates.ScheduleDelTime = value
        End Set
    End Property
    Public oItems As New List(Of clsItemDetails)
    Public TotalPLTS As Integer = 1
    Public TotalQTY As Integer = 1
    Public TotalWgt As Double = 1
    Public TotalCube As Integer = 0
    Public HazmatCode As String = ""
    Public CompLegalEntity As String = ""
    Public DefaultTemp As String = ""
    Public DefaultPalletType As String = ""
    Public InboundPrefixKeys As String = ""
    Public CarrBLNumber As String = "N/A"
    Public statusCode As DTO.tblLoadTender.LoadTenderStatusCodeEnum = DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Accept

    Public strWarnings As New List(Of String)
    Public strErrors As New List(Of String)

    Public Function updateInboundUsingDefaultSettings() As Boolean
        Dim blnRet As Boolean = False

        If Not String.IsNullOrWhiteSpace(InboundPrefixKeys) Then
            Dim strKeys As New List(Of String)
            If InboundPrefixKeys.Contains("|") Then
                strKeys = InboundPrefixKeys.Split("|").ToList()
            Else
                strKeys.Add(InboundPrefixKeys)
            End If
            For Each key In strKeys
                If OrderNumber.StartsWith(key, StringComparison.OrdinalIgnoreCase) Then
                    Me.Inbound = True
                    blnRet = True
                    Exit For
                End If
            Next
        End If

        Return blnRet
    End Function

    Public Sub checkForInbound()
        If Not Me.updateInboundUsingDefaultSettings() Then
            'we need to do a manual determiniation on inbound vs outbound
            If String.IsNullOrWhiteSpace(Me.OrigAdd.CompNumber) OrElse Me.OrigAdd.CompNumber = "0" Then
                Me.Inbound = True
            Else
                Me.Inbound = False
            End If
        End If
        'now update each items company number to match 
        If Me.Inbound Then
            'change the item company number to match the destination company nunber
            If Me.oItems?.Count() > 0 Then
                For Each item In Me.oItems
                    item.ItemCompNumber = Me.DestAdd.CompNumber
                Next
            End If
        End If

    End Sub

    Public Function concateWarnings() As String
        Dim strRet As String = ""
        If Me.strWarnings?.Count() > 0 Then strRet = "   Warnings: " & String.Concat(Me.strWarnings)
        Return strRet
    End Function

    Public Function concateErrors() As String
        Dim strRet As String = ""
        If Me.strErrors?.Count() > 0 Then strRet = "  Errors: " & String.Concat(Me.strErrors)
        Return strRet
    End Function

    Public Function concateMessages() As String
        Return String.Concat(concateErrors, concateWarnings)
    End Function

End Class

'********Merge from 6.0.4.70*************
''' <summary>
''' EDI 210 Fee Details 
''' </summary>
''' <remarks>
''' Modified by RHR for v-8.2.0.118 on 9/13/2019
'''   added EDIDescription to allow information to be sent to AP Data using new logic even when the EDI Code is blank
''' </remarks>
Public Class ediOrderSpecificFee

    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal sAPBillNumber As String, ByVal iAPCarrierNumber As Integer, ByVal sBookSHID As String)
        MyBase.New()
        Me.APBillNumber = sAPBillNumber
        Me.APCarrierNumber = iAPCarrierNumber
        Me.BookSHID = sBookSHID
    End Sub

    Private _APBillNumber As String
    Public Property APBillNumber() As String
        Get
            Return _APBillNumber
        End Get
        Set(ByVal value As String)
            _APBillNumber = value
        End Set
    End Property

    Private _APCarrierNumber As Integer
    Public Property APCarrierNumber() As Integer
        Get
            Return _APCarrierNumber
        End Get
        Set(ByVal value As Integer)
            _APCarrierNumber = value
        End Set
    End Property


    Private _BookSHID As String
    Public Property BookSHID() As String
        Get
            Return Left(_BookSHID, 20)
        End Get
        Set(ByVal value As String)
            _BookSHID = Left(value, 20)
        End Set
    End Property

    Private _BookCarrOrderNumber As String
    Public Property BookCarrOrderNumber() As String
        Get
            Return Left(_BookCarrOrderNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookCarrOrderNumber = Left(value, 20)
        End Set
    End Property

    Private _StopSequence As Integer = 0
    Public Property StopSequence() As Integer
        Get
            Return _StopSequence
        End Get
        Set(ByVal value As Integer)
            _StopSequence = value
        End Set
    End Property

    Private _FeeValue As Decimal
    Public Property FeeValue() As Decimal
        Get
            Return _FeeValue
        End Get
        Set(ByVal value As Decimal)
            _FeeValue = value
        End Set
    End Property

    Private _EDICode As String
    Public Property EDICode() As String
        Get
            Return Left(_EDICode, 20)
        End Get
        Set(ByVal value As String)
            _EDICode = Left(value, 20)
        End Set
    End Property

    Private _EDIDescription As String
    ''' <summary>
    ''' Description of Charge
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.0.118 on 9/13/2019
    '''   allows for description to be sent to AP Data using new logic even when the EDI Code is blank
    ''' </remarks>
    Public Property EDIDescription() As String
        Get
            Return _EDIDescription
        End Get
        Set(ByVal value As String)
            _EDIDescription = value
        End Set
    End Property


End Class
'********Merge from 6.0.4.70*************