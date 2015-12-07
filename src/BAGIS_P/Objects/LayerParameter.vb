Imports BAGIS_ClassLibrary
Imports System.Xml.Serialization

Public Class LayerParameter
    Inherits SerializableData

    Public Name As String
    Public DateUpdated As DateTime
    'Use this where we have a collection of layer values for the parameter
    Public LayerValuesList As List(Of String)
    'Parallel list to LayerValuesList to represent look-up table
    Public ParameterValuesList As List(Of String)
    'Hru vs. raster radio buttons on form
    Public LayerType As String
    'Path to source layer on disk
    Public LayerPath As String

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.new()
    End Sub

    ' Use this when instantiating a new object
    Public Sub New(ByVal name As String)
        Me.Name = name
        Me.DateUpdated = DateTime.Now
    End Sub

End Class
