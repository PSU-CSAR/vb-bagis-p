﻿Public Class BtnPEandSRObs
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Dim frmPESRObs As FrmPEandSRObs = New FrmPEandSRObs()
        frmPESRObs.ShowDialog()
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
