Imports NGL.FMWCFProxy.NGLIntegrationData

Public Class WebStandard
    'Inherits NGLISBaseClass
    'Implements IIntegration

    'Public Overrides Sub ProcessData() Implements IIntegration.ProcessData

    '    Me.HeaderName = "Lane"
    '    Me.ItemName = "None"

    '    Me.openLog()
    '    'Me.FileImport("Lane.txt", "", Me.LocalFolder)
    '    Me.closeLog(Me.Results)

    '    ''Backup and Delete the Lane File
    '    'MoveFile(objF.buildPath(mstrLocalFolder, "Lane.txt"), objF.buildPath(mstrLocalBackupFolder, objF.timeStampFileName("Lane.txt")))
    'End Sub

    Public Sub LaneFileImport(ByRef strHeaderFile As String, ByRef strDataPath As String)
    End Sub

End Class
