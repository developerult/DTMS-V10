Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text


Public Class GPEnumValues

    Public Enum TransMode

        Air = 1
        Rail = 2
        Road = 3
        Sea = 4
        Service = 5

    End Enum

    Public Enum PMDocType

        Invoice = 1
        CreditMemo = 2

    End Enum

    Public Enum FrieghtType

        NA = 0
        BackHulFixedFee = 1
        BackHaulPercentage = 2
        VendorOriginDockCPU = 3
        OutboundVendorDelivered = 4
        InboundVendorDelivered = 5
        UPS = 6
        Transfer = 7
        InboundFOBVendorDock = 8
        PrePayNAdd = 9

    End Enum

End Class
