Public Class PublicEnums
    ''' <summary>
    ''' Security Access to Forms, Reports and Procedures is controled via
    ''' The FormSecurityValue, ReportSecurityValue or ProcedureSecurityValue 
    ''' Objects via the checkSecurity methods  these methods have a return type of enmSecurityAccess
    ''' </summary>
    ''' <remarks>Not: in FreightMaster 5.0 only AccessDenied and FullAccess are available.  In future releases more options will be available</remarks>
    Public Enum enmSecurityAccess As Integer
        AccessDenied
        FullAccess
        ReadWriteAccess
        ReadOnlyAcess
    End Enum

End Class
