Imports System.ComponentModel

' Are we working with a raster, feature or geodatabase workspace?
Public Enum WorkspaceType
    Raster
    Geodatabase
    ImageServer
    FeatureServer
End Enum