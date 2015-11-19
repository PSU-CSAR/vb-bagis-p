Imports System.Windows.Forms
Imports System.Text
Imports BAGIS_ClassLibrary

Public Class FrmHelp

    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOK.Click
        Me.Close()
    End Sub

    Public Sub New(ByVal topic As BA_HelpTopics)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        SetForm(topic)
    End Sub

    Private Sub SetForm(ByVal topic As BA_HelpTopics)
        Dim description As String = ""
        Dim illustration As System.Drawing.Image = Nothing

        Try
            Select Case topic
                Case BA_HelpTopics.TimberlineTool
                    description = "This tool allows users to specify a timberline elevation for each HRU zone in the AOI. The timberline values can be used to assign the Snowmelt Depletion Curve (hru_deplcrv) parameter values used in PRMS. This tool saves the timberline elevation values in the attribute table of the vector HRU featureclass (grid_zones_v). The timberline elevation will be recorded only in the attribute table of this featureclass and not be added to the exported OMS parameter files. A timberline elevation value of 0 indicates that the HRU is a regular HRU (not above timberline). The actual calculation of the hru_deplcrv is done by the hru_deplcrv_treeline parameter models built in ArcGIS ModelBuilder. Users can either use the timberline elevation extracted in this tool or the Landfire vegetation layer to set the values of hru_deplcrv. If the area of an HRU is more then 50% above timberline elevation or not covered by canopy vegetation, then the hru_deplcrv is set to 2, otherwise 1."
                Case BA_HelpTopics.PeAndObsTool
                    Dim sb As StringBuilder = New StringBuilder
                    sb.Append("This tool allows users to extract and calculate the Potential Evaporation and Observed Solar Radiation ")
                    sb.Append("values for an AOI. SR is determined by selecting a value for each month from the observation point located closest to the centroid of the AOI boundary. ")
                    sb.Append("The source data for PE is assumed to be stored in a group of twelve raster layers. The units are mm/month.  ")
                    sb.Append("The values are converted to inches/day and the mean value is calculated for the area of the AOI for each month." & vbCrLf & vbCrLf)
                    sb.Append("These values are included in the nmonths table in the parameter export to eWSF. " & vbCrLf & vbCrLf)
                    sb.Append("The input data layers for this tool must have the same spatial reference as the AOI." & vbCrLf & vbCrLf)
                    sb.Append("The SR layer should be a point layer with a column for each month named SR01, SR02 ... SR12" & vbCrLf & vbCrLf)
                    sb.Append("It is only necessary to select the PE raster layer for January; ")
                    sb.Append("BAGIS-P will locate the remaining 11 months using January file name as reference (ie: the location of 01 in the name)" & vbCrLf & vbCrLf)
                    description = sb.ToString
            End Select
            LblDescription.Text = description
            If illustration IsNot Nothing Then
                PictureBox.Image = illustration
                PictureBox.SizeMode = PictureBoxSizeMode.Normal
            End If
        Catch ex As Exception
            MsgBox("An error has occurred" & Chr(13) & Chr(13) & ex.Message)
        End Try
    End Sub
End Class