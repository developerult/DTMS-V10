Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports LTS = NGL.FreightMaster.Data.LTS
Imports DTran = NGL.Core.Utility.DataTransformation
Imports TAR = NGL.FM.CarTar
Imports NGLCoreComm = NGL.Core.Communication

Public Class StoredReferences


#Region "Dictionaries"
    Private _dictNDPBaseClasses As New Dictionary(Of String, DAL.NDPBaseClass)
    Public Property dictNDPBaseClasses() As Dictionary(Of String, DAL.NDPBaseClass)
        Get
            Return _dictNDPBaseClasses
        End Get
        Set(ByVal value As Dictionary(Of String, DAL.NDPBaseClass))
            _dictNDPBaseClasses = value
        End Set
    End Property

    Private _dictBLLBaseClasses As New Dictionary(Of String, BLLBaseClass)
    Public Property dictBLLBaseClasses() As Dictionary(Of String, BLLBaseClass)
        Get
            Return _dictBLLBaseClasses
        End Get
        Set(ByVal value As Dictionary(Of String, BLLBaseClass))
            _dictBLLBaseClasses = value
        End Set
    End Property


    Private _dictTARBaseClasses As New Dictionary(Of String, TAR.TarBaseClass)
    Public Property dictTARBaseClasses() As Dictionary(Of String, TAR.TarBaseClass)
        Get
            Return _dictTARBaseClasses
        End Get
        Set(ByVal value As Dictionary(Of String, TAR.TarBaseClass))
            _dictTARBaseClasses = value
        End Set
    End Property

    Private _dictNGLLinkDataBaseClasses As New Dictionary(Of String, DAL.NGLLinkDataBaseClass)
    Public Property dictNGLLinkDataBaseClasses() As Dictionary(Of String, DAL.NGLLinkDataBaseClass)
        Get
            Return _dictNGLLinkDataBaseClasses
        End Get
        Set(ByVal value As Dictionary(Of String, DAL.NGLLinkDataBaseClass))
            _dictNGLLinkDataBaseClasses = value
        End Set
    End Property
#End Region

#Region "Methods"

#End Region

End Class
