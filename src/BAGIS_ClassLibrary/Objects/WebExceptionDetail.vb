Public Class ForbiddenRequestException
    Public detail As String
End Class

Public Class BadRequestException
    Public detail As ExceptionDetailArray
End Class

Public Class ExceptionDetailArray
    Public __all__ As List(Of String)
End Class


