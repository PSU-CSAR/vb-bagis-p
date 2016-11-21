Imports System.ComponentModel

'Used with IGxObject filters 
'Each entry represents an IGxObject category
Public Enum GxFilterCategory

    <Description("Folder")> Folder
    <Description("Folder Connection")> FolderConnection
    <Description("XML Document")> XmlDocument
    <Description("Feature Service")> FeatureService
    <Description("ArcGIS Server")> ArcGisServer
    <Description("ArcGIS Server Folder")> ArcGisServerFolder
    <Description("Image Service")> ImageService
    <Description("Map Service")> MapService
End Enum