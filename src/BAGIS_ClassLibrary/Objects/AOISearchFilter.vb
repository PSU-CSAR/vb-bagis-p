Public Class AOISearchFilter

    'The current user name
    Public UserName As String
    Public CreatedAfter As Date
    Public StringSearch As String

    Public Sub Clear()
        UserName = Nothing
        CreatedAfter = Nothing
        StringSearch = Nothing
    End Sub

    ' Generate String version of date for json query
    Public Function StrCreatedAfter() As String
        Return CreatedAfter.ToString("yyyy-MM-ddTHH':'mm':'sszzz")
    End Function

End Class
