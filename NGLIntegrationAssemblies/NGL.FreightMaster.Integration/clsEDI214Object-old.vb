<Serializable()> _
Public Class clsEDI214Object


    '*******  B10 Beginning Header Segment ******************************************************
    Public B1001CarrierBOLNumber As String = "" 'max 30
    Public B1002TMSBOLNumber As String = "" 'max 30
    Public B1003CarrierSCAC As String = "" 'max 4
    Public B1004InquiryRequestNumber As String = "" 'max 3
    Public B1005ReferenceIndetificationQualifier As String = "" 'max 3
    Public B1006ReferenceIndetification As String = "" 'max 30
    Public B1007ConditionResponseCode As String = "" 'max 1 Y for Yes and N for No

    '*******  L11     BUSINESS INSTRUCTIONS AND REFERENCE  ***************************************
    Public L1101BMBillOfLading As String = "" 'max 30 
    Public L1101ONCustomerOrderNumber As String = "" 'max 30 
    Public L1101POCustomerPO As String = "" 'max 30 
    Public L1101CIConsigneeID As String = "" 'max 30 
    Public L1101QNStopSequence As String = "" 'max 30 (Typically not used when LX01 is provided)
    Public L1101SIShippersIDNumber As String = "" 'max 30 
    Public L1101MBMasterBillOfLading As String = "" 'max 30 
    Public L110119DivisionIdentifier As String = "" 'max 30  
    Public L1103Description As String = "" 'max 80 

    '*******  N1      NAME   **********************************************************************
    Public N102STName As String = "" 'max 35 (ST stands for Ship To)
    Public N104STCustNumber As String = "" 'max 17 (ST stands for Ship To)
    Public N301STAddress As String = "" 'max 60 (ST stands for Ship To)
    Public N302STAddress As String = "" 'max 60 (ST stands for Ship To)
    Public N401STCity As String = "" 'max 30 (ST stands for Ship To)
    Public N402STState As String = "" 'max 2 (ST stands for Ship To)
    Public N403STPostalCode As String = "" 'max 10 (ST stands for Ship To)
    Public N404STCountry As String = "" 'max 10 (ST stands for Ship To
    Public N102SFName As String = "" 'max 35 (SF stands for Ship From)
    Public N104SFCustNumber As String = "" 'max 17 (SF stands for Ship From)
    Public N301SFAddress As String = "" 'max 60 (SF stands for Ship From)
    Public N302SFAddress As String = "" 'max 60 (SF stands for Ship From)
    Public N401SFCity As String = "" 'max 30 (SF stands for Ship From)
    Public N402SFState As String = "" 'max 2 (SF stands for Ship From)
    Public N403SFPostalCode As String = "" 'max 10 (SF stands for Ship From)
    Public N404SFCountry As String = "" 'max 10 (SF stands for Ship From)
    Public N102CAName As String = "" 'max 35 (CA stands for Carrier)
    Public N104CANumber As String = "" 'max 17 (CA stands for Carrier)
    '*******  G62     DATE/TIME   *******************************************************************
    Public G620202RequestedDeliveryDate As String = "" 'convert to date
    Public G620210RequestedPickupDate As String = "" 'convert to date
    Public G620269ScheduledPickupDate As String = "" 'convert to date
    Public G620270ScheduledDeliveryDate As String = "" 'convert to date
    Public G6204UScheduledPickupTime As String = "" 'convert to time
    Public G6204XScheduledDeliveryTime As String = "" 'convert to time
    Public G6204YRequestedPickupTime As String = "" 'convert to time
    Public G6204ZRequestedDeliveryTime As String = "" 'convert to time
    '*******  LX      ASSIGNED NUMBER  **************************************************************
    Public LX01StopNo As String = "" 'max 6 Stop Number or Sequence Number
    '*******  AT7     SHIPMENT STATUS DETAILS  ******************************************************
    Public AT701ShipmentStatusCode As String = "" 'max 10
    Public AT702ShipmentStatusReasonCode As String = "" 'max 10
    Public AT703AppointmentStautsCode As String = "" 'max 10
    Public AT704AppointmentStausReasonCode As String = "" 'max 10
    Public AT705Date As String = "" 'max 30
    Public AT706Time As String = "" 'max 30
    Public AT707TimeCode As String = "" 'max 10
    '*******  MS1     EQUIPMENT  *******************************************************************
    Public MS101City As String = "" 'max 30
    Public MS102State As String = "" 'max 2
    Public MS103Country As String = "" 'max 3
    Public MS104Longitued As String = "" 'max 22
    Public MS105Latitued As String = "" 'max 22
    Public MS106DirectionCode As String = "" 'max 1
    Public MS107DirectionCode As String = "" 'max 1
    '*******  MS2     EQUIPMENT  *******************************************************************
    Public MS201EquipCarrierAlphaCode As String = "" 'max 4
    Public MS202EquipNumber As String = "" 'max 10
    Public MS203EquipCode As String = "" 'max 2
    Public MS204EquipNumberDigit As String = "" 'max 1
    '*******  K1     REMARKS  **********************************************************************
    Public K101Message As String = "" 'max 30 Free Form Message
    Public K102Message As String = "" 'max 30 Free From Message
    '*******  AT8     SHIPMENT WEIGHT  **************************************************************
    Public AT801WeightQualifier As String = "" 'max 2 (Typically G - Gross Weight)
    Public AT802WeightUnitCode As String = "" 'max 1 (Typically L - Pounds)
    Public AT803Weight As String = "" 'max 22 Actual Weight
    Public AT804Pallets As String = "" 'max 10 BookActualPallets
    Public AT805Quantity As String = "" 'max 10 Actual Quantity like Cases
    Public AT806VolumnUintQualifier As String = "" 'max 1 Typically E for Cubic Feet
    Public AT806Volumn As String = "" 'max 10 actual volumn like Number of Cubes


End Class
