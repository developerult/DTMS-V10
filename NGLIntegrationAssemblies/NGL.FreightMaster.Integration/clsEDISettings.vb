'This entire file was created by RHR for v-7.0.6.105 on 5/17/2017
' these classes are designed to read the new EDI Config files stored in the 
' customers inbound or outbound folder based on the type of EDI being generated.
Imports System.Xml.Linq

Public Class clsEDISettings
    Private _204In As New clsEDI204InSetting()
    Public Property o204In() As clsEDI204InSetting
        Get
            Return _204In
        End Get
        Set(ByVal value As clsEDI204InSetting)
            _204In = value
        End Set
    End Property

    Public Function readEDI204InSetting(ByVal strFile As String) As clsEDI204InSetting
        Dim xdoc1 As XDocument = XDocument.Load(strFile)

        Dim obj204Setting As clsEDI204InSetting = (From _EDI204 In xdoc1.Element("EDISettings").Elements("EDI204In")
                                                   Select New clsEDI204InSetting _
                                                       With {
                                                       .MappingRules = (From _maps In _EDI204.Elements("MappingRules") Select clsEDI204InMappingRules.SelectXMLData(_maps)).FirstOrDefault(),
                                                       .AltMappingOptions = (From _alt In _EDI204.Elements("AltMappingOptions") Select clsEDI204inAltMappingOptions.SelectXMLData(_alt)).FirstOrDefault(),
                                                       .Heading = (From _head In _EDI204.Elements("Heading") Select clsEDI204inHeading.SelectXMLData(_head)).FirstOrDefault()
                                                       }).FirstOrDefault()
        Me.o204In = obj204Setting
        Return obj204Setting
    End Function


    '********Merge from 6.0.4.70*************
    Private _210In As New clsEDI210InSetting()
    Public Property o210In() As clsEDI210InSetting
        Get
            Return _210In
        End Get
        Set(ByVal value As clsEDI210InSetting)
            _210In = value
        End Set
    End Property

    Public Function readEDI210InSetting(ByVal strFile As String) As clsEDI210InSetting
        Dim xdoc1 As XDocument = XDocument.Load(strFile)

        Dim obj210InSetting As clsEDI210InSetting = (From _EDI In xdoc1.Element("EDISettings").Elements("EDI210In")
                                                     Select New clsEDI210InSetting _
                                                       With {
                                                       .MappingRules = (From _maps In _EDI.Elements("MappingRules") Select clsEDI210InMappingRules.SelectXMLData(_maps)).FirstOrDefault(),
                                                       .AltMappingOptions = (From _alt In _EDI.Elements("AltMappingOptions") Select clsEDI210InAltMappingOptions.SelectXMLData(_alt)).FirstOrDefault(),
                                                       .Heading = (From _head In _EDI.Elements("Heading") Select clsEDI210InHeading.SelectXMLData(_head)).FirstOrDefault()
                                                       }).FirstOrDefault()

        Me.o210In = obj210InSetting
        Return obj210InSetting
    End Function

    '********Merge from 6.0.4.70*************


    'Added By LVV on 2/7/18 for v-8.1 PQ EDI
    Private _204Out As New clsEDI204OutSetting()
    Public Property o204Out() As clsEDI204OutSetting
        Get
            Return _204Out
        End Get
        Set(ByVal value As clsEDI204OutSetting)
            _204Out = value
        End Set
    End Property

    'Added By LVV on 2/7/18 for v-8.1 PQ EDI
    Public Function readEDI204OutSetting(ByVal strFile As String) As clsEDI204OutSetting
        Dim xdoc1 As XDocument = XDocument.Load(strFile)

        Dim obj204OutSetting As clsEDI204OutSetting = (From _EDI204 In xdoc1.Element("EDISettings").Elements("EDI204Out")
                                                       Select New clsEDI204OutSetting _
                                                       With {
                                                       .MappingRules = (From _maps In _EDI204.Elements("MappingRules") Select clsEDI204OutMappingRules.SelectXMLData(_maps)).FirstOrDefault(),
                                                       .AltMappingOptions = (From _alt In _EDI204.Elements("AltMappingOptions") Select clsEDI204OutAltMappingOptions.SelectXMLData(_alt)).FirstOrDefault(),
                                                       .Heading = (From _head In _EDI204.Elements("Heading") Select clsEDI204OutHeading.SelectXMLData(_head)).FirstOrDefault()
                                                       }).FirstOrDefault()
        Me.o204Out = obj204OutSetting
        Return obj204OutSetting
    End Function


End Class

Public Class clsEDIMappingCodes
    Private _CodeType As String = ""
    Public Property CodeType() As String
        Get
            Return _CodeType
        End Get
        Set(ByVal value As String)
            _CodeType = value
        End Set
    End Property

    Private _Codes As New List(Of clsEDIMappingCode)
    Public Property Codes() As List(Of clsEDIMappingCode)
        Get
            Return _Codes
        End Get
        Set(ByVal value As List(Of clsEDIMappingCode))
            _Codes = value
        End Set
    End Property

    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDIMappingCodes
        Dim oRet As New clsEDIMappingCodes With {.CodeType = xmlData.Element("CodeType").Value, .Codes = (From _code In xmlData.Element("Codes").Elements("Code") Select clsEDIMappingCode.SelectXMLData(_code)).ToList()}

        Return oRet
    End Function

End Class

Public Class clsEDIMappingCode
    Private _Key As String = ""
    Public Property Key() As String
        Get
            Return _Key
        End Get
        Set(ByVal value As String)
            _Key = value
        End Set
    End Property

    Private _Value As String = ""
    Public Property value() As String
        Get
            Return _Value
        End Get
        Set(ByVal value As String)
            _Value = value
        End Set
    End Property

    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDIMappingCode
        Dim oRet As New clsEDIMappingCode With {.Key = xmlData.Element("Key").Value, .value = xmlData.Element("Value").Value}

        Return oRet
    End Function


End Class

Public Class clsEDIMappingElement
    Private _MapFrom As String = ""
    Public Property MapFrom() As String
        Get
            Return _MapFrom
        End Get
        Set(ByVal value As String)
            _MapFrom = value
        End Set
    End Property

    Private _MapTo As String = ""
    Public Property MapTo() As String
        Get
            Return _MapTo
        End Get
        Set(ByVal value As String)
            _MapTo = value
        End Set
    End Property

    Private _QualifierSegment As String = ""
    Public Property QualifierSegment() As String
        Get
            Return _QualifierSegment
        End Get
        Set(ByVal value As String)
            _QualifierSegment = value
        End Set
    End Property

    Private _Qualifier As String = ""
    Public Property Qualifier() As String
        Get
            Return _Qualifier
        End Get
        Set(ByVal value As String)
            _Qualifier = value
        End Set
    End Property


    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDIMappingElement
        Dim oRet As New clsEDIMappingElement With {
            .MapFrom = xmlData.Element("MapFrom").Value,
            .MapTo = xmlData.Element("MapTo").Value,
            .QualifierSegment = xmlData.Element("QualifierSegment").Value,
            .Qualifier = xmlData.Element("Qualifier").Value}

        Return oRet
    End Function


End Class

Public Class clsEDILoopMappings
    Private _LoopType As String = ""
    Public Property LoopType() As String
        Get
            Return _LoopType
        End Get
        Set(ByVal value As String)
            _LoopType = value
        End Set
    End Property

    Private _LoopParent As String = ""
    Public Property LoopParent() As String
        Get
            Return _LoopParent
        End Get
        Set(ByVal value As String)
            _LoopParent = value
        End Set
    End Property

    Private _QualifierSegment As String = ""
    Public Property QualifierSegment() As String
        Get
            Return _QualifierSegment
        End Get
        Set(ByVal value As String)
            _QualifierSegment = value
        End Set
    End Property

    Private _Qualifier As String = ""
    Public Property Qualifier() As String
        Get
            Return _Qualifier
        End Get
        Set(ByVal value As String)
            _Qualifier = value
        End Set
    End Property

    Private _Elements As New List(Of clsEDIMappingElement)
    Public Property Elements() As List(Of clsEDIMappingElement)
        Get
            Return _Elements
        End Get
        Set(ByVal value As List(Of clsEDIMappingElement))
            _Elements = value
        End Set
    End Property

    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDILoopMappings
        Dim oRet As New clsEDILoopMappings With {
            .LoopType = xmlData.Element("LoopType").Value,
            .LoopParent = xmlData.Element("LoopParent").Value,
            .QualifierSegment = xmlData.Element("QualifierSegment").Value,
            .Qualifier = xmlData.Element("Qualifier").Value,
            .Elements = (From _elem In xmlData.Element("Elements").Elements("Element") Select clsEDIMappingElement.SelectXMLData(_elem)).ToList()
        }

        Return oRet
    End Function
End Class


#Region "EDI 204 Inbound Settings and Rules"

Public Class clsEDI204InSetting

    Private _MappingRules As New clsEDI204InMappingRules()
    Public Property MappingRules() As clsEDI204InMappingRules
        Get
            Return _MappingRules
        End Get
        Set(ByVal value As clsEDI204InMappingRules)
            _MappingRules = value
        End Set
    End Property

    Private _AltMappingOptions As New clsEDI204inAltMappingOptions()
    Public Property AltMappingOptions() As clsEDI204inAltMappingOptions
        Get
            Return _AltMappingOptions
        End Get
        Set(ByVal value As clsEDI204inAltMappingOptions)
            _AltMappingOptions = value
        End Set
    End Property

    Private _Heading As New clsEDI204inHeading()
    Public Property Heading() As clsEDI204inHeading
        Get
            Return _Heading
        End Get
        Set(ByVal value As clsEDI204inHeading)
            _Heading = value
        End Set
    End Property

End Class

Public Class clsEDI204InMappingRules

    Private _CompLegalEntity As String = ""
    Public Property CompLegalEntity() As String
        Get
            Return _CompLegalEntity
        End Get
        Set(ByVal value As String)
            _CompLegalEntity = value
        End Set
    End Property

    Private _DefaultTemp As String = ""
    Public Property DefaultTemp() As String
        Get
            Return _DefaultTemp
        End Get
        Set(ByVal value As String)
            _DefaultTemp = value
        End Set
    End Property

    Private _DefaultPalletType As String = ""
    Public Property DefaultPalletType() As String
        Get
            Return _DefaultPalletType
        End Get
        Set(ByVal value As String)
            _DefaultPalletType = value
        End Set
    End Property

    Private _CompAbrev As String = ""
    Public Property CompAbrev() As String
        Get
            Return _CompAbrev
        End Get
        Set(ByVal value As String)
            _CompAbrev = value
        End Set
    End Property

    Private _InboundPrefixKeys As String = ""
    Public Property InboundPrefixKeys() As String
        Get
            Return _InboundPrefixKeys
        End Get
        Set(ByVal value As String)
            _InboundPrefixKeys = value
        End Set
    End Property

    Private _PORecMinIn As String = ""
    Public Property PORecMinIn As String
        Get
            Return _PORecMinIn
        End Get
        Set(value As String)
            _PORecMinIn = value
        End Set
    End Property

    Private _PORecMinUnload As String = ""
    Public Property PORecMinUnload As String
        Get
            Return _PORecMinUnload
        End Get
        Set(value As String)
            _PORecMinUnload = value
        End Set
    End Property

    Private _PORecMinOut As String = ""
    Public Property PORecMinOut As String
        Get
            Return _PORecMinOut
        End Get
        Set(value As String)
            _PORecMinOut = value
        End Set
    End Property

    Private _POAppt As String = ""
    Public Property POAppt As String
        Get
            Return _POAppt
        End Get
        Set(value As String)
            _POAppt = value
        End Set
    End Property

    Private _POBFC As String = ""
    Public Property POBFC As String
        Get
            Return _POBFC
        End Get
        Set(value As String)
            _POBFC = value
        End Set
    End Property

    Private _POBFCType As String = ""
    Public Property POBFCType As String
        Get
            Return Left(_POBFCType, 50)
        End Get
        Set(value As String)
            _POBFCType = Left(value, 50)
        End Set
    End Property

    Private _POConsigneeNumber As String = ""
    Public Property POConsigneeNumber() As String
        Get
            Return _POConsigneeNumber
        End Get
        Set(ByVal value As String)
            _POConsigneeNumber = value
        End Set
    End Property

    Private _WCFServiceURL As String = ""
    Public Property WCFServiceURL() As String
        Get
            Return _WCFServiceURL
        End Get
        Set(ByVal value As String)
            _WCFServiceURL = value
        End Set
    End Property

    Private _WCFTCPServiceURL As String = ""
    Public Property WCFTCPServiceURL() As String
        Get
            Return _WCFTCPServiceURL
        End Get
        Set(ByVal value As String)
            _WCFTCPServiceURL = value
        End Set
    End Property

    Private _ValidatePULocationCodes As String = ""
    Public Property ValidatePULocationCodes() As String
        Get
            Return _ValidatePULocationCodes
        End Get
        Set(ByVal value As String)
            _ValidatePULocationCodes = value
        End Set
    End Property

    Private _ValidateDelLocationCodes As String = ""
    Public Property ValidateDelLocationCodes() As String
        Get
            Return _ValidateDelLocationCodes
        End Get
        Set(ByVal value As String)
            _ValidateDelLocationCodes = value
        End Set
    End Property


    Private _ValidatePUAddressName As String = ""
    Public Property ValidatePUAddressName() As String
        Get
            Return _ValidatePUAddressName
        End Get
        Set(ByVal value As String)
            _ValidatePUAddressName = value
        End Set
    End Property

    Private _ValidateDelAddressName As String = ""
    Public Property ValidateDelAddressName() As String
        Get
            Return _ValidateDelAddressName
        End Get
        Set(ByVal value As String)
            _ValidateDelAddressName = value
        End Set
    End Property


    Private _ValidatePUAddress As String = ""
    Public Property ValidatePUAddress() As String
        Get
            Return _ValidatePUAddress
        End Get
        Set(ByVal value As String)
            _ValidatePUAddress = value
        End Set
    End Property

    Private _ValidateDelAddress As String = ""
    Public Property ValidateDelAddress() As String
        Get
            Return _ValidateDelAddress
        End Get
        Set(ByVal value As String)
            _ValidateDelAddress = value
        End Set
    End Property

    Private _POStatusFlag As String = "5"
    Public Property POStatusFlag() As String
        Get
            Return _POStatusFlag
        End Get
        Set(ByVal value As String)
            _POStatusFlag = value
        End Set
    End Property

    Private _InboundCompNumber As String = "0"
    Public Property InboundCompNumber() As String
        Get
            Return _InboundCompNumber
        End Get
        Set(ByVal value As String)
            _InboundCompNumber = value
        End Set
    End Property

    Private _OutboundCompNumber As String = "0"
    Public Property OutboundCompNumber() As String
        Get
            Return _OutboundCompNumber
        End Get
        Set(ByVal value As String)
            _OutboundCompNumber = value
        End Set
    End Property


    Private _MappingCodes As New List(Of clsEDIMappingCodes)
    Public Property MappingCodes() As List(Of clsEDIMappingCodes)
        Get
            Return _MappingCodes
        End Get
        Set(ByVal value As List(Of clsEDIMappingCodes))
            _MappingCodes = value
        End Set
    End Property

    Public Function doPULocationCodeValidation() As Boolean
        Dim blnRet As Boolean = True
        Boolean.TryParse(ValidatePULocationCodes, blnRet)
        Return blnRet
    End Function

    Public Function doDelLocationCodeValidation() As Boolean
        Dim blnRet As Boolean = True
        Boolean.TryParse(ValidateDelLocationCodes, blnRet)
        Return blnRet
    End Function

    Public Function doPUAddressNameValidation() As Boolean
        Dim blnRet As Boolean = True
        Boolean.TryParse(ValidatePUAddressName, blnRet)
        Return blnRet
    End Function

    Public Function doDelAddressNameValidation() As Boolean
        Dim blnRet As Boolean = True
        Boolean.TryParse(ValidateDelAddressName, blnRet)
        Return blnRet
    End Function

    Public Function doPUAddressValidation() As Boolean
        Dim blnRet As Boolean = True
        Boolean.TryParse(ValidatePUAddress, blnRet)
        Return blnRet
    End Function

    Public Function doDelAddressValidation() As Boolean
        Dim blnRet As Boolean = True
        Boolean.TryParse(ValidateDelAddress, blnRet)
        Return blnRet
    End Function

    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDI204InMappingRules
        Dim oRet As New clsEDI204InMappingRules With {
            .CompLegalEntity = xmlData.Element("CompLegalEntity").Value,
            .CompAbrev = xmlData.Element("CompAbrev").Value,
            .DefaultTemp = xmlData.Element("DefaultTemp").Value,
            .DefaultPalletType = xmlData.Element("DefaultPalletType").Value,
            .InboundPrefixKeys = xmlData.Element("InboundPrefixKeys").Value,
            .PORecMinIn = xmlData.Element("PORecMinIn").Value,
            .PORecMinUnload = xmlData.Element("PORecMinUnload").Value,
            .PORecMinOut = xmlData.Element("PORecMinOut").Value,
            .POAppt = xmlData.Element("POAppt").Value,
            .POBFC = xmlData.Element("POBFC").Value,
            .POBFCType = xmlData.Element("POBFCType").Value,
            .POConsigneeNumber = xmlData.Element("POConsigneeNumber").Value,
            .WCFServiceURL = xmlData.Element("WCFServiceURL").Value,
            .WCFTCPServiceURL = xmlData.Element("WCFTCPServiceURL").Value,
            .ValidatePULocationCodes = xmlData.Element("ValidatePULocationCodes").Value,
            .ValidateDelLocationCodes = xmlData.Element("ValidateDelLocationCodes").Value,
            .ValidatePUAddressName = xmlData.Element("ValidatePUAddressName").Value,
            .ValidateDelAddressName = xmlData.Element("ValidateDelAddressName").Value,
            .ValidatePUAddress = xmlData.Element("ValidatePUAddress").Value,
            .ValidateDelAddress = xmlData.Element("ValidateDelAddress").Value,
            .POStatusFlag = xmlData.Element("POStatusFlag").Value,
            .InboundCompNumber = xmlData.Element("InboundCompNumber").Value,
            .OutboundCompNumber = xmlData.Element("OutboundCompNumber").Value,
            .MappingCodes = (From _codes In xmlData.Element("MappingCodes").Elements("MappingCode")
                             Select clsEDIMappingCodes.SelectXMLData(_codes)).ToList()}


        Return oRet
    End Function

End Class


Public Class clsEDI204inAltMappingOptions


    Private _Elements As New List(Of clsEDIMappingElement)
    Public Property Elements() As List(Of clsEDIMappingElement)
        Get
            Return _Elements
        End Get
        Set(ByVal value As List(Of clsEDIMappingElement))
            _Elements = value
        End Set
    End Property

    Private _Loops As New List(Of clsEDILoopMappings)
    Public Property Loops() As List(Of clsEDILoopMappings)
        Get
            Return _Loops
        End Get
        Set(ByVal value As List(Of clsEDILoopMappings))
            _Loops = value
        End Set
    End Property


    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDI204inAltMappingOptions
        Dim oRet As New clsEDI204inAltMappingOptions With {
            .Elements = (From _elem In xmlData.Element("Elements").Elements("Element") Select clsEDIMappingElement.SelectXMLData(_elem)).ToList(),
            .Loops = (From _loop In xmlData.Element("Loops").Elements("Loop") Select clsEDILoopMappings.SelectXMLData(_loop)).ToList()
        }


        Return oRet
    End Function


End Class

Public Class clsEDI204inHeading

    Private _Elements As New List(Of clsEDIMappingElement)
    Public Property Elements() As List(Of clsEDIMappingElement)
        Get
            Return _Elements
        End Get
        Set(ByVal value As List(Of clsEDIMappingElement))
            _Elements = value
        End Set
    End Property

    Private _Loops As New List(Of clsEDILoopMappings)
    Public Property Loops() As List(Of clsEDILoopMappings)
        Get
            Return _Loops
        End Get
        Set(ByVal value As List(Of clsEDILoopMappings))
            _Loops = value
        End Set
    End Property

    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDI204inHeading
        Dim oRet As New clsEDI204inHeading With {
            .Elements = (From _elem In xmlData.Element("Elements").Elements("Element") Select clsEDIMappingElement.SelectXMLData(_elem)).ToList(),
            .Loops = (From _loop In xmlData.Element("Loops").Elements("Loop") Select clsEDILoopMappings.SelectXMLData(_loop)).ToList()
        }


        Return oRet
    End Function

End Class

#End Region


'********Merge from 6.0.4.70*************
#Region "EDI 210 Inbound Settings and Rules"

Public Class clsEDI210InSetting

    Private _MappingRules As New clsEDI210InMappingRules()
    Public Property MappingRules() As clsEDI210InMappingRules
        Get
            Return _MappingRules
        End Get
        Set(ByVal value As clsEDI210InMappingRules)
            _MappingRules = value
        End Set
    End Property

    Private _AltMappingOptions As New clsEDI210InAltMappingOptions()
    Public Property AltMappingOptions() As clsEDI210InAltMappingOptions
        Get
            Return _AltMappingOptions
        End Get
        Set(ByVal value As clsEDI210InAltMappingOptions)
            _AltMappingOptions = value
        End Set
    End Property

    Private _Heading As New clsEDI210InHeading()
    Public Property Heading() As clsEDI210InHeading
        Get
            Return _Heading
        End Get
        Set(ByVal value As clsEDI210InHeading)
            _Heading = value
        End Set
    End Property

End Class

Public Class clsEDI210InMappingRules

    Private _StopSequenceAdjment As String = "0"
    Public Property StopSequenceAdjment() As String
        Get
            Return _StopSequenceAdjment
        End Get
        Set(ByVal value As String)
            _StopSequenceAdjment = value
        End Set
    End Property

    Private _MappingCodes As New List(Of clsEDIMappingCodes)
    Public Property MappingCodes() As List(Of clsEDIMappingCodes)
        Get
            Return _MappingCodes
        End Get
        Set(ByVal value As List(Of clsEDIMappingCodes))
            _MappingCodes = value
        End Set
    End Property

    ''' <summary>
    ''' Overload calculate adjustment factor stored in StopSequenceAdjment config if a valid integer else returns iTMSStopNbr
    ''' </summary>
    ''' <param name="iTMSStopNbr"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-6.0.4.7 on 10/20/2017 
    ''' </remarks>
    Public Function getAdjustedStopSequence(ByVal iTMSStopNbr As Integer) As Integer
        Dim iRet As Integer = iTMSStopNbr
        Dim iFactor As Integer = 0
        If Integer.TryParse(StopSequenceAdjment, iFactor) Then
            iRet = iFactor + iTMSStopNbr
        End If
        Return iRet
    End Function

    ''' <summary>
    ''' Overload used to convert string value to integer first.  Default is zero if cannot parse string
    ''' </summary>
    ''' <param name="sTMSStopNbr"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-6.0.4.7 on 10/20/2017
    ''' TODO:  in v-8.0.1 this method would be repalce by a standard mapping formula selected from a list in the UI.
    ''' </remarks>
    Public Function getAdjustedStopSequence(ByVal sTMSStopNbr As String) As Integer
        Dim iRet As Integer = 0
        Integer.TryParse(sTMSStopNbr, iRet)
        Return getAdjustedStopSequence(iRet)
    End Function


    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDI210InMappingRules
        Dim oRet As New clsEDI210InMappingRules With {
            .StopSequenceAdjment = xmlData.Element("StopSequenceAdjment").Value,
            .MappingCodes = (From _codes In xmlData.Element("MappingCodes").Elements("MappingCode")
                             Select clsEDIMappingCodes.SelectXMLData(_codes)).ToList()}


        Return oRet
    End Function

End Class

Public Class clsEDI210InAltMappingOptions

    Private _Elements As New List(Of clsEDIMappingElement)
    Public Property Elements() As List(Of clsEDIMappingElement)
        Get
            Return _Elements
        End Get
        Set(ByVal value As List(Of clsEDIMappingElement))
            _Elements = value
        End Set
    End Property

    Private _Loops As New List(Of clsEDILoopMappings)
    Public Property Loops() As List(Of clsEDILoopMappings)
        Get
            Return _Loops
        End Get
        Set(ByVal value As List(Of clsEDILoopMappings))
            _Loops = value
        End Set
    End Property

    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDI210InAltMappingOptions
        Dim oRet As New clsEDI210InAltMappingOptions With {
            .Elements = (From _elem In xmlData.Element("Elements").Elements("Element") Select clsEDIMappingElement.SelectXMLData(_elem)).ToList(),
            .Loops = (From _loop In xmlData.Element("Loops").Elements("Loop") Select clsEDILoopMappings.SelectXMLData(_loop)).ToList()
        }


        Return oRet
    End Function


End Class

Public Class clsEDI210InHeading

    Private _Elements As New List(Of clsEDIMappingElement)
    Public Property Elements() As List(Of clsEDIMappingElement)
        Get
            Return _Elements
        End Get
        Set(ByVal value As List(Of clsEDIMappingElement))
            _Elements = value
        End Set
    End Property

    Private _Loops As New List(Of clsEDILoopMappings)
    Public Property Loops() As List(Of clsEDILoopMappings)
        Get
            Return _Loops
        End Get
        Set(ByVal value As List(Of clsEDILoopMappings))
            _Loops = value
        End Set
    End Property

    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDI210InHeading
        Dim oRet As New clsEDI210InHeading With {
            .Elements = (From _elem In xmlData.Element("Elements").Elements("Element") Select clsEDIMappingElement.SelectXMLData(_elem)).ToList(),
            .Loops = (From _loop In xmlData.Element("Loops").Elements("Loop") Select clsEDILoopMappings.SelectXMLData(_loop)).ToList()
        }


        Return oRet
    End Function

End Class

#End Region
'********Merge from 6.0.4.70*************


#Region "EDI 204 Outbound Settings and Rules"
'Added By LVV on 2/7/18 for v-8.1 PQ EDI

Public Class clsEDI204OutSetting

    Private _MappingRules As New clsEDI204OutMappingRules()
    Public Property MappingRules() As clsEDI204OutMappingRules
        Get
            Return _MappingRules
        End Get
        Set(ByVal value As clsEDI204OutMappingRules)
            _MappingRules = value
        End Set
    End Property

    Private _AltMappingOptions As New clsEDI204OutAltMappingOptions()
    Public Property AltMappingOptions() As clsEDI204OutAltMappingOptions
        Get
            Return _AltMappingOptions
        End Get
        Set(ByVal value As clsEDI204OutAltMappingOptions)
            _AltMappingOptions = value
        End Set
    End Property

    Private _Heading As New clsEDI204OutHeading()
    Public Property Heading() As clsEDI204OutHeading
        Get
            Return _Heading
        End Get
        Set(ByVal value As clsEDI204OutHeading)
            _Heading = value
        End Set
    End Property

End Class


''' <summary>
''' Read Settings and add custom mapping rules
''' </summary>
''' <remarks>
''' Modified by RHR on 08/02/2021 for v-8.4.0.003 added new MapGS03ToISA08 rule 
'''     This allow us to remove the 15 character fill on Carrier EDI Partner Code
'''     The default is False
''' </remarks>
Public Class clsEDI204OutMappingRules

    ' MapGS03ToISA08
    Private _MapGS03ToISA08 As String = "False"
    Public Property MapGS03ToISA08() As String
        Get
            Return _MapGS03ToISA08
        End Get
        Set(ByVal value As String)
            _MapGS03ToISA08 = value
        End Set
    End Property


    Private _ShowItemDetailDescrtiption As String = ""
    Public Property ShowItemDetailDescrtiption() As String
        Get
            Return _ShowItemDetailDescrtiption
        End Get
        Set(ByVal value As String)
            _ShowItemDetailDescrtiption = value
        End Set
    End Property

    Private _ShowEstimatedCarrierCosts As String = ""
    Public Property ShowEstimatedCarrierCosts() As String
        Get
            Return _ShowEstimatedCarrierCosts
        End Get
        Set(ByVal value As String)
            _ShowEstimatedCarrierCosts = value
        End Set
    End Property

    Private _IncludeNotes1 As String = ""
    Public Property IncludeNotes1() As String
        Get
            Return _IncludeNotes1
        End Get
        Set(ByVal value As String)
            _IncludeNotes1 = value
        End Set
    End Property

    Private _IncludeNotes2 As String = ""
    Public Property IncludeNotes2() As String
        Get
            Return _IncludeNotes2
        End Get
        Set(ByVal value As String)
            _IncludeNotes2 = value
        End Set
    End Property

    Private _IncludeNotes3 As String = ""
    Public Property IncludeNotes3() As String
        Get
            Return _IncludeNotes3
        End Get
        Set(ByVal value As String)
            _IncludeNotes3 = value
        End Set
    End Property

    Private _IncludeItems As String = ""
    Public Property IncludeItems() As String
        Get
            Return _IncludeItems
        End Get
        Set(ByVal value As String)
            _IncludeItems = value
        End Set
    End Property

    Private _ShowContractedCost As String = ""
    Public Property ShowContractedCost() As String
        Get
            Return _ShowContractedCost
        End Get
        Set(ByVal value As String)
            _ShowContractedCost = value
        End Set
    End Property


    Private _MappingCodes As New List(Of clsEDIMappingCodes)
    Public Property MappingCodes() As List(Of clsEDIMappingCodes)
        Get
            Return _MappingCodes
        End Get
        Set(ByVal value As List(Of clsEDIMappingCodes))
            _MappingCodes = value
        End Set
    End Property

    Public Function getShowItemDetailDescrtiption() As Boolean
        Dim blnRet As Boolean = True
        Boolean.TryParse(ShowItemDetailDescrtiption, blnRet)
        Return blnRet
    End Function

    Public Function getMapGS03ToISA08() As Boolean
        Dim blnRet As Boolean = False
        Boolean.TryParse(MapGS03ToISA08, blnRet)
        Return blnRet
    End Function

    Public Function getShowEstimatedCarrierCosts() As Boolean
        Dim blnRet As Boolean = True
        Boolean.TryParse(ShowEstimatedCarrierCosts, blnRet)
        Return blnRet
    End Function


    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDI204OutMappingRules
        Dim oRet As New clsEDI204OutMappingRules With {
            .MapGS03ToISA08 = xmlData.Element("MapGS03ToISA08").Value,
            .ShowItemDetailDescrtiption = xmlData.Element("ShowItemDetailDescrtiption").Value,
            .ShowEstimatedCarrierCosts = xmlData.Element("ShowEstimatedCarrierCosts").Value,
            .IncludeNotes1 = xmlData.Element("IncludeNotes1").Value,
            .IncludeNotes2 = xmlData.Element("IncludeNotes2").Value,
            .IncludeNotes3 = xmlData.Element("IncludeNotes3").Value,
            .IncludeItems = xmlData.Element("IncludeItems").Value,
            .ShowContractedCost = xmlData.Element("ShowContractedCost").Value,
            .MappingCodes = (From _codes In xmlData.Element("MappingCodes").Elements("MappingCode")
                             Select clsEDIMappingCodes.SelectXMLData(_codes)).ToList()}

        Return oRet
    End Function

End Class

Public Class clsEDI204OutAltMappingOptions

    Private _Elements As New List(Of clsEDIMappingElement)
    Public Property Elements() As List(Of clsEDIMappingElement)
        Get
            Return _Elements
        End Get
        Set(ByVal value As List(Of clsEDIMappingElement))
            _Elements = value
        End Set
    End Property

    Private _Loops As New List(Of clsEDILoopMappings)
    Public Property Loops() As List(Of clsEDILoopMappings)
        Get
            Return _Loops
        End Get
        Set(ByVal value As List(Of clsEDILoopMappings))
            _Loops = value
        End Set
    End Property

    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDI204OutAltMappingOptions
        Dim oRet As New clsEDI204OutAltMappingOptions With {
            .Elements = (From _elem In xmlData.Element("Elements").Elements("Element") Select clsEDIMappingElement.SelectXMLData(_elem)).ToList(),
            .Loops = (From _loop In xmlData.Element("Loops").Elements("Loop") Select clsEDILoopMappings.SelectXMLData(_loop)).ToList()
        }

        Return oRet
    End Function

End Class

Public Class clsEDI204OutHeading

    Private _Elements As New List(Of clsEDIMappingElement)
    Public Property Elements() As List(Of clsEDIMappingElement)
        Get
            Return _Elements
        End Get
        Set(ByVal value As List(Of clsEDIMappingElement))
            _Elements = value
        End Set
    End Property

    Private _Loops As New List(Of clsEDILoopMappings)
    Public Property Loops() As List(Of clsEDILoopMappings)
        Get
            Return _Loops
        End Get
        Set(ByVal value As List(Of clsEDILoopMappings))
            _Loops = value
        End Set
    End Property

    Public Shared Function SelectXMLData(ByRef xmlData As System.Xml.Linq.XElement) As clsEDI204OutHeading
        Dim oRet As New clsEDI204OutHeading With {
            .Elements = (From _elem In xmlData.Element("Elements").Elements("Element") Select clsEDIMappingElement.SelectXMLData(_elem)).ToList(),
            .Loops = (From _loop In xmlData.Element("Loops").Elements("Loop") Select clsEDILoopMappings.SelectXMLData(_loop)).ToList()
        }

        Return oRet
    End Function

End Class

#End Region






