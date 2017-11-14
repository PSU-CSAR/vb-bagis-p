Public Class ServerSettings
    Property datasources As List(Of BagisImageService)
End Class

Public Class BagisImageService

    Public Property id As Int16
    Public Property name As String
    Public Property description As String
    Public Property source As String

End Class
