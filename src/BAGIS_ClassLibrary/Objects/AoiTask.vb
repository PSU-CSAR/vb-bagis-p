''' <summary>
''' This object is used to deserialize the json that is returned from either an
''' aoi upload or an aoidownload task
''' </summary>
''' <remarks>
''' To access the docs for the rest api that describes these fields
''' 1. Login here:  https://webservices.geog.pdx.edu/api/rest/ using test login
''' 2. Docs here: https://webservices.geog.pdx.edu/api/rest/docs
''' </remarks>
Public Class AoiTask

    ' The id for the upload or download object
    Public id As String
    ' Url to query to get the status of an upload/download task (object)
    Public url As String
    ' The url representing the user account making the request
    Public user As String
    ' The md5 hash for the uploaded object
    Public md5 As String
    ' The url representing the file upload object in django; PC-BAGIS does not use
    Public file As String
    ' The name of the file without the .zip extension; This is the same as the AOI name
    Public filename As String
    Public offset As String
    Public created_at As String
    ' Status code used by django
    Public status As Integer
    Public completed_at As String
    Public task As Task

    ''' <summary>
    ''' This read-only property parses the JSON date to create a date
    ''' object that can be consumed by VB.NET
    ''' </summary>
    ''' <value></value>
    ''' <returns>Date</returns>
    ''' <remarks></remarks>
    ReadOnly Property DateCreated As Date
        Get
            Dim aDate As DateTime
            DateTime.TryParse(created_at, aDate)
            Return aDate
        End Get
    End Property

    ''' <summary>
    ''' this read-only property parses the JSON date to create a date
    ''' object that can be consumed by VB.NET
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property DateCompleted As Date
        Get
            Dim aDate As DateTime
            DateTime.TryParse(completed_at, aDate)
            Return aDate
        End Get
    End Property


End Class

Public Class Task
    Public id As Integer
    Public task_id As String
    ' This is used for tracking status; Valid values are PENDING, SUCCESS, FAILURE
    Public status As String
    Public date_done As String
    ' Holds the error that we pass back to the user
    Public traceback As String

    ReadOnly Property DateDone As Date
        Get
            Dim aDate As DateTime
            DateTime.TryParse(date_done, aDate)
            Return aDate
        End Get
    End Property
End Class
