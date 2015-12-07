Imports BAGIS_ClassLibrary
Imports System.Xml.Serialization

Public Class LayerParameterTable
    Inherits SerializableData

    Dim m_layerParameterList As List(Of LayerParameter)
    Public Version As String

    ' List of layer parameters
    Public Property LayerParameters() As List(Of LayerParameter)
        Get
            Return m_layerParameterList
        End Get
        Set(ByVal value As List(Of LayerParameter))
            m_layerParameterList = New List(Of LayerParameter)
            If value IsNot Nothing Then
                If value.Count > 0 Then
                    m_layerParameterList.AddRange(value)
                End If
            End If
        End Set
    End Property
End Class
