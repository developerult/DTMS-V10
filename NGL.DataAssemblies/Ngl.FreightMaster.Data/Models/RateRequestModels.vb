
Imports Map = Ngl.API.Mapping
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports Ngl.FreightMaster.Data.LTS
Imports Serilog


Namespace Models
    'Added By RHR On 12/10/18 For v-8.2 
    'replaces D365 Client Model
    'Modified by RHR for v-8.5.4.001 on 06/28/2023
    '   added logic to support temperature selection when Rate Shopping 
    '   Added 3 fields TarrifTempType, CommCodeType, CommCodeDescription
    '   Added function to set CommCodeType based on TarrifTempType
    '   Added function to set TarrifTempType based on CommCodeType
    '   Added function to set CommCodeDescription based on TarrifTempType or CommCodeType
    Public Class RateRequestOrder

        Public Property Logger As ILogger = Serilog.Log.Logger.ForContext(of RateRequestOrder)

        Private _ID As Integer
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property
        Private _ShipKey As String
        Public Property ShipKey() As String
            Get
                Return _ShipKey
            End Get
            Set(ByVal value As String)
                _ShipKey = value
            End Set
        End Property
        Private _BookConsPrefix As String
        Public Property BookConsPrefix() As String
            Get
                Return _BookConsPrefix
            End Get
            Set(ByVal value As String)
                _BookConsPrefix = value
            End Set
        End Property
        Private _ShipDate As String
        Public Property ShipDate() As String
            Get
                Return _ShipDate
            End Get
            Set(ByVal value As String)
                _ShipDate = value
            End Set
        End Property
        Private _DeliveryDate As String
        Public Property DeliveryDate() As String
            Get
                Return _DeliveryDate
            End Get
            Set(ByVal value As String)
                _DeliveryDate = value
            End Set
        End Property
        Private _BookCustCompControl As Integer
        Public Property BookCustCompControl() As Integer
            Get
                Return _BookCustCompControl
            End Get
            Set(ByVal value As Integer)
                _BookCustCompControl = value
            End Set
        End Property
        Private _CompName As String
        Public Property CompName() As String
            Get
                Return _CompName
            End Get
            Set(ByVal value As String)
                _CompName = value
            End Set
        End Property
        Private _CompNumber As Integer
        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set(ByVal value As Integer)
                _CompNumber = value
            End Set
        End Property
        Private _CompAlphaCode As String
        Public Property CompAlphaCode() As String
            Get
                Return _CompAlphaCode
            End Get
            Set(ByVal value As String)
                _CompAlphaCode = value
            End Set
        End Property
        Private _BookCarrierControl As Integer
        Public Property BookCarrierControl() As Integer
            Get
                Return _BookCarrierControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrierControl = value
            End Set
        End Property
        Private _CarrierName As String
        Public Property CarrierName() As String
            Get
                Return _CarrierName
            End Get
            Set(ByVal value As String)
                _CarrierName = value
            End Set
        End Property
        Private _CarrierNumber As Integer
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property
        Private _CarrierAlphaCode As String
        Public Property CarrierAlphaCode() As String
            Get
                Return _CarrierAlphaCode
            End Get
            Set(ByVal value As String)
                _CarrierAlphaCode = value
            End Set
        End Property
        Private _TotalCases As Integer
        Public Property TotalCases() As Integer
            Get
                Return _TotalCases
            End Get
            Set(ByVal value As Integer)
                _TotalCases = value
            End Set
        End Property
        Private _TotalWgt As Double
        Public Property TotalWgt() As Double
            Get
                Return _TotalWgt
            End Get
            Set(ByVal value As Double)
                _TotalWgt = value
            End Set
        End Property

        Private _TotalPL As Double
        Public Property TotalPL() As Double
            Get
                Return _TotalPL
            End Get
            Set(ByVal value As Double)
                _TotalPL = value
            End Set
        End Property
        Private _TotalCube As Integer
        Public Property TotalCube() As Integer
            Get
                Return _TotalCube
            End Get
            Set(ByVal value As Integer)
                _TotalCube = value
            End Set
        End Property
        Private _TotalStops As Integer
        Public Property TotalStops() As Integer
            Get
                Return _TotalStops
            End Get
            Set(ByVal value As Integer)
                _TotalStops = value
            End Set
        End Property
        Private _Pickup As RateRequestStop
        Public Property Pickup() As RateRequestStop
            Get
                Return _Pickup
            End Get
            Set(ByVal value As RateRequestStop)
                _Pickup = value
            End Set
        End Property
        Private _Stops As RateRequestStop()
        Public Property Stops() As RateRequestStop()
            Get
                Return _Stops
            End Get
            Set(ByVal value As RateRequestStop())
                _Stops = value
            End Set
        End Property
        ''' <summary>
        ''' Array of Values associated with the Accessorials array one value per fee (default value)
        ''' </summary>
        ''' <returns></returns>
        Private _AccessorialValues As String()
        Public Property AccessorialValues() As String()
            Get
                Return _AccessorialValues
            End Get
            Set(ByVal value As String())
                _AccessorialValues = value
            End Set
        End Property

        Private _Accessorials As String()
        Public Property Accessorials() As String()
            Get
                Return _Accessorials
            End Get
            Set(ByVal value As String())
                _Accessorials = value
            End Set
        End Property

        'Added By LVV on 4/20/20 LBDemo
        Private _CommCodeType As String
        Public Property CommCodeType() As String
            Get
                Return _CommCodeType
            End Get
            Set(ByVal value As String)
                _CommCodeType = value
            End Set
        End Property

        'Added By LVV on 4/20/20 LBDemo
        Private _Inbound As Boolean
        Public Property Inbound() As Boolean
            Get
                Return _Inbound
            End Get
            Set(ByVal value As Boolean)
                _Inbound = value
            End Set
        End Property

        'Added By LVV on 4/24/20 LBDemo
        Private _BookTransType As Integer
        Public Property BookTransType() As Integer
            Get
                Return _BookTransType
            End Get
            Set(ByVal value As Integer)
                _BookTransType = value
            End Set
        End Property


        ' Begin  Modified by RHR for v-8.5.4.001 on 06/28/2023

        Private _CommCodeDescription As String
        Public Property CommCodeDescription() As String
            Get
                Return _CommCodeDescription
            End Get
            Set(ByVal value As String)
                _CommCodeDescription = value
            End Set
        End Property

        Private _TariffTempType As Integer? = Nothing
        Public Property TariffTempType() As Integer?
            Get
                Return _TariffTempType
            End Get
            Set(ByVal value As Integer?)
                _TariffTempType = value
            End Set
        End Property

        Public Sub SetCommCodeType(ByVal iTariffTempType As Integer)
            Select Case iTariffTempType
                Case 0
                    CommCodeType = "M"
                Case 1
                    CommCodeType = "D"
                Case 2
                    CommCodeType = "F"
                Case 3
                    CommCodeType = "C"
                Case Else
                    CommCodeType = "M"
            End Select
        End Sub

        Public Sub SetTariffTempType(ByVal sCommCodeType As String)
            Select Case sCommCodeType
                Case "M"
                    TariffTempType = 0
                Case "D"
                    TariffTempType = 1
                Case "F"
                    TariffTempType = 2
                Case "C"
                    TariffTempType = 3
                Case "R"
                    TariffTempType = 3
                Case Else
                    TariffTempType = 0
            End Select
        End Sub

        Public Sub SetCommCodeDescription(ByVal sCommCodeType As String)
            Select Case sCommCodeType
                Case "M"
                    CommCodeDescription = "Mixed"
                Case "D"
                    CommCodeDescription = "Dry"
                Case "F"
                    CommCodeDescription = "Frz"
                Case "C"
                    CommCodeDescription = "Cooler"
                Case "R"
                    CommCodeDescription = "Ref"
                Case Else
                    CommCodeDescription = "Mixed"
            End Select
        End Sub

        Public Sub SetCommCodeDescription(ByVal iTariffTempType As Integer)
            Select Case iTariffTempType
                Case 0
                    CommCodeDescription = "Mixed"
                Case 1
                    CommCodeDescription = "Dry"
                Case 2
                    CommCodeDescription = "Frz"
                Case 3
                    CommCodeDescription = "Cooler"
                Case Else
                    CommCodeDescription = "Mixed"
            End Select
        End Sub

        Public Sub prepareTemperatureSettings()
            If (String.IsNullOrWhiteSpace(CommCodeType)) Then
                If (TariffTempType.HasValue) Then
                    SetCommCodeType(TariffTempType.Value)
                Else
                    CommCodeType = "D"
                    TariffTempType = 1
                End If
            Else
                SetTariffTempType(CommCodeType) ' CommCodeType takes precedence 
            End If

            If (String.IsNullOrWhiteSpace(CommCodeDescription)) Then
                SetCommCodeDescription(CommCodeType)
            End If
        End Sub

        'End Modified by RHR for v-8.5.4.001 on 06/28/2023

        ''' <summary>
        ''' Maps Model RateRequestOrder to Map.RateRequestOrder caller must test for null caller should use Map.RateRequestOrder instead of Model.RateRequestOrder whenever possible
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.5.4.002 on 09/28/2023
        ''' </remarks>
        Public Function MapNGLAPIRateRequestOrder() As Map.RateRequestOrder
            Dim oRet As Map.RateRequestOrder = New Map.RateRequestOrder()
            Serilog.Log.Logger.Information("RateRequestOrder.MapNGLAPIRateRequestOrder() - Skipping Stops, AccessorialValues, and Accessorials")
            Dim skipObjs As New List(Of String) From {"Pickup",
                        "Stops",
                        "AccessorialValues",
                        "Accessorials"}
            oRet = DTran.CopyMatchingFields(oRet, Me, skipObjs)
            Serilog.Log.Logger.Information("RateRequestOrder.MapNGLAPIRateRequestOrder() - CopyMatchingFields() - oRet: {@oRet}", oRet)
            'add custom formatting
            With oRet
                If (Me.Pickup IsNot Nothing) Then
                    'Modified by RHR for v-8.5.4.004 on 01/20/2024  the RateShop module does not include items for pickup location
                    'We need items for API pickup so use the first stop items
                    'Note:  for now RateShop is just one pick one stop
                    If Me.Pickup.Items Is Nothing Then
                        Serilog.Log.Logger.Information("RateRequestOrder.MapNGLAPIRateRequestOrder() - Pickup.Items is null, setting to first stop items")
                        If Me.Stops IsNot Nothing AndAlso Me.Stops.Count > 0 AndAlso Me.Stops(0).Items IsNot Nothing Then
                            Serilog.Log.Logger.Information("RateRequestOrder.MapNGLAPIRateRequestOrder() - Pickup.Items is null, setting to first stop items")
                            Me.Pickup.Items = Me.Stops(0).Items
                        End If
                    End If

                    .Pickup = Me.Pickup.MapNGLAPIRateRequestStop() ' call RateRequestStop map to api function
                End If
                If (Me.Stops IsNot Nothing AndAlso Me.Stops.Count() > 0) Then
                    Serilog.Log.Logger.Information("RateRequestOrder.MapNGLAPIRateRequestOrder() - Stops: {@Stops}", Me.Stops)
                    .Stops = (From d In Me.Stops Select d.MapNGLAPIRateRequestStop()).ToArray() ' call RateRequestStop map to api function for each
                End If
                .AccessorialValues = Me.AccessorialValues
                .Accessorials = _Accessorials
                Serilog.Log.Logger.Information("RateRequestOrder.MapNGLAPIRateRequestOrder() - Accessorials: {@Accessorials} AccessorialValues: {@AccessorialValues}", .Accessorials, .AccessorialValues)
            End With

            Return oRet
        End Function

    End Class

    Public Class RateRequestStop
        Private _ID As Integer
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property
        Private _ParentID As Integer
        Public Property ParentID() As Integer
            Get
                Return _ParentID
            End Get
            Set(ByVal value As Integer)
                _ParentID = value
            End Set
        End Property
        Private _BookControl As Integer
        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property
        Private _BookProNumber As String
        Public Property BookProNumber() As String
            Get
                Return _BookProNumber
            End Get
            Set(ByVal value As String)
                _BookProNumber = value
            End Set
        End Property
        Private _StopIndex As Integer
        Public Property StopIndex() As Integer
            Get
                Return _StopIndex
            End Get
            Set(ByVal value As Integer)
                _StopIndex = value
            End Set
        End Property
        Private _BookCarrOrderNumber As String
        Public Property BookCarrOrderNumber() As String
            Get
                Return _BookCarrOrderNumber
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumber = value
            End Set
        End Property
        Private _CompControl As Integer
        Public Property CompControl() As Integer
            Get
                Return _CompControl
            End Get
            Set(ByVal value As Integer)
                _CompControl = value
            End Set
        End Property
        Private _CompName As String
        Public Property CompName() As String
            Get
                Return _CompName
            End Get
            Set(ByVal value As String)
                _CompName = value
            End Set
        End Property
        Private _CompAddress1 As String
        Public Property CompAddress1() As String
            Get
                Return _CompAddress1
            End Get
            Set(ByVal value As String)
                _CompAddress1 = value
            End Set
        End Property
        Private _CompAddress2 As String
        Public Property CompAddress2() As String
            Get
                Return _CompAddress2
            End Get
            Set(ByVal value As String)
                _CompAddress2 = value
            End Set
        End Property
        Private _CompAddress3 As String
        Public Property CompAddress3() As String
            Get
                Return _CompAddress3
            End Get
            Set(ByVal value As String)
                _CompAddress3 = value
            End Set
        End Property
        Private _CompCity As String
        Public Property CompCity() As String
            Get
                Return _CompCity
            End Get
            Set(ByVal value As String)
                _CompCity = value
            End Set
        End Property
        Private _CompState As String
        Public Property CompState() As String
            Get
                Return _CompState
            End Get
            Set(ByVal value As String)
                _CompState = value
            End Set
        End Property
        Private _CompCountry As String
        Public Property CompCountry() As String
            Get
                Return _CompCountry
            End Get
            Set(ByVal value As String)
                _CompCountry = value
            End Set
        End Property
        Private _CompPostalCode As String
        Public Property CompPostalCode() As String
            Get
                Return CleanUp54Zip(_CompPostalCode) 'Modified by LVV on 5/24/19 for v-8.2 Bug fix - P44 does not accept 5-4 zip formats
                'Return _CompPostalCode
            End Get
            Set(ByVal value As String)
                _CompPostalCode = CleanUp54Zip(value) 'Modified by LVV on 5/24/19 for v-8.2 Bug fix - P44 does not accept 5-4 zip formats
                '_CompPostalCode = value
            End Set
        End Property
        Private _IsPickup As Boolean
        Public Property IsPickup() As Boolean
            Get
                Return _IsPickup
            End Get
            Set(ByVal value As Boolean)
                _IsPickup = value
            End Set
        End Property

        Private _StopNumber As Integer
        Public Property StopNumber() As Integer
            Get
                Return _StopNumber
            End Get
            Set(ByVal value As Integer)
                _StopNumber = value
            End Set
        End Property

        Private _TotalCases As Integer
        Public Property TotalCases() As Integer
            Get
                Return _TotalCases
            End Get
            Set(ByVal value As Integer)
                _TotalCases = value
            End Set
        End Property

        Private _TotalWgt As Double
        Public Property TotalWgt() As Double
            Get
                Return _TotalWgt
            End Get
            Set(ByVal value As Double)
                _TotalWgt = value
            End Set
        End Property
        Private _TotalPL As Double
        Public Property TotalPL() As Double
            Get
                Return _TotalPL
            End Get
            Set(ByVal value As Double)
                _TotalPL = value
            End Set
        End Property
        Private _TotalCube As Integer
        Public Property TotalCube() As Integer
            Get
                Return _TotalCube
            End Get
            Set(ByVal value As Integer)
                _TotalCube = value
            End Set
        End Property
        Private _LoadDate As String
        Public Property LoadDate() As String
            Get
                Return _LoadDate
            End Get
            Set(ByVal value As String)
                _LoadDate = value
            End Set
        End Property
        Private _SHID As String
        Public Property SHID() As String
            Get
                Return _SHID
            End Get
            Set(ByVal value As String)
                _SHID = value
            End Set
        End Property
        Private _RequiredDate As String
        Public Property RequiredDate() As String
            Get
                Return _RequiredDate
            End Get
            Set(ByVal value As String)
                _RequiredDate = value
            End Set
        End Property

        Private _Items As RateRequestItem()
        Public Property Items() As RateRequestItem()
            Get
                Return _Items
            End Get
            Set(ByVal value As RateRequestItem())
                _Items = value
            End Set
        End Property



        ''' <summary>
        ''' Truncates a 5-4 zip (xxxxx-xxxx) to be only 5 (xxxxx)
        ''' </summary>
        ''' <param name="zip"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by LVV on 5/24/19 for v-8.2
        ''' Bug fix - P44 does not accept 5-4 zip formats
        ''' </remarks>
        Private Function CleanUp54Zip(ByVal zip As String) As String
            Dim strRet As String
            Dim intDash = InStr(1, zip, "-")
            If intDash Then
                strRet = Left(zip, intDash - 1)
            Else
                strRet = zip
            End If
            Return strRet
        End Function

        ''' <summary>
        ''' Maps Model RateRequestStop to Map.RateRequestStop caller must test for null caller should use Map.RateRequestStop instead of Model.RateRequestStop whenever possible
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.5.4.002 on 09/28/2023
        ''' </remarks>
        Public Function MapNGLAPIRateRequestStop() As Map.RateRequestStop

            Dim oRet As Map.RateRequestStop = New Map.RateRequestStop()

            Dim skipObjs As New List(Of String) From {"Items"}
            oRet = DTran.CopyMatchingFields(oRet, Me, skipObjs)
            'add custom formatting
            If String.IsNullOrWhiteSpace(oRet.CompName) Then
                oRet.CompName = If(String.IsNullOrWhiteSpace(oRet.CompPostalCode), "Undefined Location", oRet.CompPostalCode)
            End If
            With oRet
                If (Me.Items IsNot Nothing AndAlso Me.Items.Count() > 0) Then

                    .Items = (From d In Me.Items Select d.MapNGLAPIRateRequestItem()).ToList() ' call MapNGLAPIRateRequestItem for each
                    Serilog.Log.Logger.Information("RateRequestStop.MapNGLAPIRateRequestStop() - Items: {@Items}", Me.Items)
                End If
            End With

            Return oRet
        End Function

    End Class

    Public Class RateRequestItem
        Private _ID As Integer
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Private _ParentID As Integer
        Public Property ParentID() As Integer
            Get
                Return _ParentID
            End Get
            Set(ByVal value As Integer)
                _ParentID = value
            End Set
        End Property

        Private _LoadID As Integer
        Public Property LoadID() As Integer
            Get
                Return _LoadID
            End Get
            Set(ByVal value As Integer)
                _LoadID = value
            End Set
        End Property

        Private _ItemStopIndex As Integer
        Public Property ItemStopIndex() As Integer
            Get
                Return _ItemStopIndex
            End Get
            Set(ByVal value As Integer)
                _ItemStopIndex = value
            End Set
        End Property
        Private _ItemControl As Integer
        Public Property ItemControl() As Integer
            Get
                Return _ItemControl
            End Get
            Set(ByVal value As Integer)
                _ItemControl = value
            End Set
        End Property
        Private _ItemNumber As String
        Public Property ItemNumber() As String
            Get
                Return _ItemNumber
            End Get
            Set(ByVal value As String)
                _ItemNumber = value
            End Set
        End Property
        Private _Weight As Decimal
        Public Property Weight() As Decimal
            Get
                Return _Weight
            End Get
            Set(ByVal value As Decimal)
                _Weight = value
            End Set
        End Property
        Private _WeightUnit As String
        Public Property WeightUnit() As String
            Get
                Return _WeightUnit
            End Get
            Set(ByVal value As String)
                _WeightUnit = value
            End Set
        End Property
        Private _FreightClass As String
        Public Property FreightClass() As String
            Get
                Return _FreightClass
            End Get
            Set(ByVal value As String)
                _FreightClass = value
            End Set
        End Property
        Private _PalletCount As Integer
        Public Property PalletCount() As Integer
            Get
                Return _PalletCount
            End Get
            Set(ByVal value As Integer)
                _PalletCount = value
            End Set
        End Property


        Private _numPieces As Integer
        Public Property NumPieces As Integer
            Get
                Integer.TryParse(Me.Quantity, _numPieces)
                If _numPieces < 1 Then _numPieces = 1
                Return _numPieces
            End Get
            Set(ByVal value As Integer)
                _numPieces = value
            End Set
        End Property

        Private _Description As String
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property
        Private _Quantity As String
        Public Property Quantity() As String
            Get
                Return _Quantity
            End Get
            Set(ByVal value As String)
                _Quantity = value
            End Set
        End Property
        Private _HazmatId As String
        Public Property HazmatId() As String
            Get
                Return _HazmatId
            End Get
            Set(ByVal value As String)
                _HazmatId = value
            End Set
        End Property
        Private _Code As String
        Public Property Code() As String
            Get
                Return _Code
            End Get
            Set(ByVal value As String)
                _Code = value
            End Set
        End Property
        Private _HazmatClass As String
        Public Property HazmatClass() As String
            Get
                Return _HazmatClass
            End Get
            Set(ByVal value As String)
                _HazmatClass = value
            End Set
        End Property
        Private _IsHazmat As Boolean
        Public Property IsHazmat() As Boolean
            Get
                Return _IsHazmat
            End Get
            Set(ByVal value As Boolean)
                _IsHazmat = value
            End Set
        End Property
        Private _Pieces As String
        Public Property Pieces() As String
            Get
                Return _Pieces
            End Get
            Set(ByVal value As String)
                _Pieces = value
            End Set
        End Property
        Private _PackageType As String
        Public Property PackageType() As String
            Get
                Return _PackageType
            End Get
            Set(ByVal value As String)
                _PackageType = value
            End Set
        End Property
        Private _Length As Decimal
        Public Property Length() As Decimal
            Get
                Return _Length
            End Get
            Set(ByVal value As Decimal)
                _Length = value
            End Set
        End Property
        Private _Width As Decimal
        Public Property Width() As Decimal
            Get
                Return _Width
            End Get
            Set(ByVal value As Decimal)
                _Width = value
            End Set
        End Property
        Private _Height As Decimal
        Public Property Height() As Decimal
            Get
                Return _Height
            End Get
            Set(ByVal value As Decimal)
                _Height = value
            End Set
        End Property
        Private _Density As String
        Public Property Density() As String
            Get
                Return _Density
            End Get
            Set(ByVal value As String)
                _Density = value
            End Set
        End Property
        Private _NMFCItem As String
        Public Property NMFCItem() As String
            Get
                Return _NMFCItem
            End Get
            Set(ByVal value As String)
                _NMFCItem = value
            End Set
        End Property
        Private _NMFCSub As String
        Public Property NMFCSub() As String
            Get
                Return _NMFCSub
            End Get
            Set(ByVal value As String)
                _NMFCSub = value
            End Set
        End Property
        Private _Stackable As Boolean
        Public Property Stackable() As Boolean
            Get
                Return _Stackable
            End Get
            Set(ByVal value As Boolean)
                _Stackable = value
            End Set
        End Property


        ''' <summary>
        ''' Maps Model RateRequestItem to Map.RateRequestItem caller must test for null caller should use Map.RateRequestItem instead of Model.RateRequestItem whenever possible
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.5.4.002 on 09/28/2023
        ''' </remarks>
        Public Function MapNGLAPIRateRequestItem() As Map.RateRequestItem
            Dim oRet As Map.RateRequestItem = New Map.RateRequestItem()

            Dim skipObjs As New List(Of String) From {""}
            oRet = DTran.CopyMatchingFields(oRet, Me, skipObjs)
            'add custom formatting
            'With oRet
            If String.IsNullOrWhiteSpace(oRet.WeightUnit) Then
                oRet.WeightUnit = "lb"
            End If
            'End With
            Return oRet
        End Function

        Public Shared Function MapAPINGLRateRequestItem(item As Map.RateRequestItem) As RateRequestItem
            Dim oRet As RateRequestItem = New RateRequestItem()

            Dim skipObjs As New List(Of String) From {""}
            oRet = DTran.CopyMatchingFields(oRet, item, skipObjs)
            'add custom formatting
            'With oRet
            If String.IsNullOrWhiteSpace(oRet.WeightUnit) Then
                oRet.WeightUnit = "lb"
            End If
            'End With
            Return oRet
        End Function


    End Class



End Namespace

