Imports Ngl.FreightMaster.Data.LTS
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports Map = Ngl.API.Mapping

Namespace Models
    'Added By LVV On 8/11/17 For v-8.0 TMS365
    Public Class Item

        Private _ItemNumber As String
        Private _ItemDesc As String
        Private _ItemFreightClass As String
        Private _ItemWgt As Decimal
        Private _ItemTotalPackages As Integer
        Private _ItemPackageType As String
        Private _ItemPieces As Integer
        Private _ItemStackable As Boolean
        Private _ItemLength As Decimal
        Private _ItemWidth As Decimal
        Private _ItemHeight As Decimal
        Private _ItemNMFCItemCode As String
        Private _ItemNMFCSubCode As String
        Private _ItemIsHazmat As Boolean
        Private _ItemHazmatID As String
        Private _ItemHazmatClass As String
        Private _ItemHazmatPackingGroup As String
        Private _ItemHazmatProperShipName As String
        Private _ItemCube As Integer 'Added By LVV for BOL Report Changes

        Public Property ItemNumber() As String
            Get
                Return _ItemNumber
            End Get
            Set(ByVal value As String)
                _ItemNumber = value
            End Set
        End Property

        Public Property ItemDesc() As String
            Get
                Return _ItemDesc
            End Get
            Set(ByVal value As String)
                _ItemDesc = value
            End Set
        End Property

        Public Property ItemFreightClass() As String
            Get
                Return _ItemFreightClass
            End Get
            Set(ByVal value As String)
                _ItemFreightClass = value
            End Set
        End Property

        Public Property ItemWgt() As Decimal
            Get
                Return _ItemWgt
            End Get
            Set(ByVal value As Decimal)
                _ItemWgt = value
            End Set
        End Property

        Public Property ItemTotalPackages() As Integer
            Get
                Return _ItemTotalPackages
            End Get
            Set(ByVal value As Integer)
                _ItemTotalPackages = value
            End Set
        End Property

        Public Property ItemPackageType() As String
            Get
                Return _ItemPackageType
            End Get
            Set(ByVal value As String)
                _ItemPackageType = value
            End Set
        End Property

        Public Property ItemPieces() As Integer
            Get
                Return _ItemPieces
            End Get
            Set(ByVal value As Integer)
                _ItemPieces = value
            End Set
        End Property

        Public Property ItemStackable() As Boolean
            Get
                Return _ItemStackable
            End Get
            Set(ByVal value As Boolean)
                _ItemStackable = value
            End Set
        End Property

        Public Property ItemLength() As Decimal
            Get
                Return _ItemLength
            End Get
            Set(ByVal value As Decimal)
                _ItemLength = value
            End Set
        End Property

        Public Property ItemWidth() As Decimal
            Get
                Return _ItemWidth
            End Get
            Set(ByVal value As Decimal)
                _ItemWidth = value
            End Set
        End Property

        Public Property ItemHeight() As Decimal
            Get
                Return _ItemHeight
            End Get
            Set(ByVal value As Decimal)
                _ItemHeight = value
            End Set
        End Property

        Public Property ItemNMFCItemCode() As String
            Get
                Return _ItemNMFCItemCode
            End Get
            Set(ByVal value As String)
                _ItemNMFCItemCode = value
            End Set
        End Property

        Public Property ItemNMFCSubCode() As String
            Get
                Return _ItemNMFCSubCode
            End Get
            Set(ByVal value As String)
                _ItemNMFCSubCode = value
            End Set
        End Property

        Public Property ItemIsHazmat() As Boolean
            Get
                Return _ItemIsHazmat
            End Get
            Set(ByVal value As Boolean)
                _ItemIsHazmat = value
            End Set
        End Property

        Public Property ItemHazmatID() As String
            Get
                Return _ItemHazmatID
            End Get
            Set(ByVal value As String)
                _ItemHazmatID = value
            End Set
        End Property

        Public Property ItemHazmatClass() As String
            Get
                Return _ItemHazmatClass
            End Get
            Set(ByVal value As String)
                _ItemHazmatClass = value
            End Set
        End Property

        Public Property ItemHazmatPackingGroup() As String
            Get
                Return _ItemHazmatPackingGroup
            End Get
            Set(ByVal value As String)
                _ItemHazmatPackingGroup = value
            End Set
        End Property

        Public Property ItemHazmatProperShipName() As String
            Get
                Return _ItemHazmatProperShipName
            End Get
            Set(ByVal value As String)
                _ItemHazmatProperShipName = value
            End Set
        End Property

        Private ReadOnly _ItemDimensions As String
        Public ReadOnly Property ItemDimensions() As String
            Get
                Return Me.ItemLength.ToString() & "x" & Me.ItemWidth.ToString() & "x" & Me.ItemHeight.ToString()
            End Get
        End Property

        Private _ItemWeightUnit As String = "lbs"
        ''' <summary>
        ''' Weight Unit Of Measer Default is 'lbs'
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.2 on 3/1/2019
        ''' </remarks>
        Public Property ItemWeightUnit() As String
            Get
                Return _ItemWeightUnit
            End Get
            Set(ByVal value As String)
                _ItemWeightUnit = value
            End Set
        End Property

        Public Property ItemCube() As Integer
            Get
                Return _ItemCube
            End Get
            Set(ByVal value As Integer)
                _ItemCube = value
            End Set
        End Property

        Public Property oBookAccessorial As New List(Of BookAccessorial)

        Public Function MapNGLAPIItem() As Map.Item
            Return New Map.Item() With {
                .ItemNumber = Me.ItemNumber,
                .ItemDesc = Me.ItemDesc,
                .ItemFreightClass = Me.ItemFreightClass,
                .ItemWgt = Me.ItemWgt,
                .ItemTotalPackages = Me.ItemTotalPackages,
                .ItemPackageType = Me.ItemPackageType,
                .ItemPieces = Me.ItemPieces,
                .ItemStackable = Me.ItemStackable,
                .ItemLength = Me.ItemLength,
                .ItemWidth = Me.ItemWidth,
                .ItemHeight = Me.ItemHeight,
                .ItemNMFCItemCode = Me.ItemNMFCItemCode,
                .ItemNMFCSubCode = Me.ItemNMFCSubCode,
                .ItemIsHazmat = Me.ItemIsHazmat,
                .ItemHazmatID = Me.ItemHazmatID,
                .ItemHazmatClass = Me.ItemHazmatClass,
                .ItemHazmatPackingGroup = Me.ItemHazmatPackingGroup,
                .ItemHazmatProperShipName = Me.ItemHazmatProperShipName,
                .ItemCube = Me.ItemCube
            }

        End Function

    End Class


End Namespace

