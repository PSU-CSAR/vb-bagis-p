Public Class FeatureService

    Private m_name As String
    Private m_featureServiceFields As FeatureServiceField()
    Public extent As Extent

    Property name As String
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property

    Property fields() As FeatureServiceField()
        Get
            Return m_featureServiceFields
        End Get
        Set(ByVal value As FeatureServiceField())
            m_featureServiceFields = value
        End Set
    End Property

End Class

Public Class Extent
    Public xmin As Double
    Public ymin As Double
    Public xmax As Double
    Public ymax As Double
    Public spatialReference As SpatialReference
End Class

Public Class SpatialReference
    Public wkid As Integer
    Public latestWkid As Integer
End Class
