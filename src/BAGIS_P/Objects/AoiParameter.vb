Imports BAGIS_ClassLibrary
Imports System.Xml.Serialization

Public Class AoiParameter
    Inherits SerializableData

    Public Name As String
    'Value is a string so we can hold anything; Cast to different data type when using, if needed
    Public Value As String
    Public DateUpdated As DateTime
    'Use this where we have a collection of values for the parameter, not just one
    Public ValuesList As List(Of String)

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.new()
    End Sub

    ' Use this when instantiating a new object
    Public Sub New(ByVal name As String, ByVal value As String)
        Me.Name = name
        Me.Value = value
        Me.DateUpdated = DateTime.Now
    End Sub

    ' Use this when instantiating a new object
    Public Sub New(ByVal name As String)
        Me.Name = name
        Me.DateUpdated = DateTime.Now
    End Sub

End Class
