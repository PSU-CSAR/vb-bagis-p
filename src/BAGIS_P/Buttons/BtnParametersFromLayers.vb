Public Class BtnParametersFromLayers
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Dim paramForm As FrmParametersFromLayers = New FrmParametersFromLayers()
        paramForm.ShowDialog()
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
