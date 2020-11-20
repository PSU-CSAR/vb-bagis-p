Public Class BtnParameterDescription
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Process.Start("https://sites.google.com/site/nwccspatialservices/users-references/users-manuals/bagis-p-usersmanual/ewsf-prms-parameters")
    End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
