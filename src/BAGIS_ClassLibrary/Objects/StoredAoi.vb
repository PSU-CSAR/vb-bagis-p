Public Class StoredAoi

    Dim m_name As String
    Dim m_url As String
    Dim m_created_at As String
    Public created_by As Created_By
    Public comment As String

    Property name As String
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property

    Property url As String
        Get
            Return m_url
        End Get
        Set(value As String)
            m_url = value
        End Set
    End Property

    Property created_at As String
        Get
            Return m_created_at
        End Get
        Set(value As String)
            m_created_at = value
        End Set
    End Property

    ReadOnly Property DateCreated As Date
        Get
            Dim aDate As DateTime
            DateTime.TryParse(m_created_at, aDate)
            Return aDate
        End Get
    End Property

End Class

Public Class Created_By
    Public url As String
    Public username As String
End Class
