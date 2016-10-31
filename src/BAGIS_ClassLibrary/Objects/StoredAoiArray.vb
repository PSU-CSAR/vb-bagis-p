Public Class StoredAoiArray

    Private m_results As StoredAoi()

    Property results() As StoredAoi()
        Get
            Return m_results
        End Get
        Set(ByVal value As StoredAoi())
            m_results = value
        End Set
    End Property

End Class
