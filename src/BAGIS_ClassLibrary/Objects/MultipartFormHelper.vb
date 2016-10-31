Imports System.Text
Imports System.IO
Imports System.Security.Cryptography

'Converted from C# code posted here:
'http://www.paraesthesia.com/archive/2009/12/16/posting-multipartform-data-using-.net-webrequest.aspx/
Public Class MultipartFormHelper

    Public Const FormDataTemplate As String = "--{0}" & vbCrLf & "Content-Disposition: form-data; name=""{1}""" & vbCrLf & vbCrLf & "{2}" & vbCrLf
    Public Const HeaderTemplate As String = "--{0}" & vbCrLf & "Content-Disposition: form-data; name=""{1}""; filename=""{2}""" & vbCrLf & "Content-Type: {3}" & vbCrLf & vbCrLf

    Public Shared Sub WriteMultipartFormData(ByRef dictionary As Dictionary(Of String, String), ByVal stream As System.IO.Stream, ByVal mimeBoundary As String)
        If dictionary Is Nothing Or dictionary.Count = 0 Then
            Exit Sub
        End If
        If stream Is Nothing Then
            Throw New ArgumentException("stream")
        End If
        If String.IsNullOrEmpty(mimeBoundary) Then
            Throw New ArgumentException("mimeBoundary")
        End If
        For Each key As String In dictionary.Keys
            Dim item As String = String.Format(FormDataTemplate, mimeBoundary, key, dictionary(key))
            Dim itemBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(item)
            stream.Write(itemBytes, 0, itemBytes.Length)
        Next
    End Sub

    Public Shared Function WriteMultipartFormData(ByRef file As System.IO.FileInfo, ByVal stream As System.IO.Stream, ByVal mimeBoundary As String, _
                                      ByVal mimeType As String, ByVal formKey As String) As Long
        If file Is Nothing Then
            Throw New ArgumentNullException("file")
        End If
        If Not file.Exists Then
            Throw New System.IO.FileNotFoundException("Unable to find file to write to stream.", file.FullName)
        End If
        If stream Is Nothing Then
            Throw New ArgumentException("stream")
        End If
        If String.IsNullOrEmpty(mimeBoundary) Then
            Throw New ArgumentException("mimeBoundary")
        End If
        If String.IsNullOrEmpty(mimeType) Then
            Throw New ArgumentException("mimeType")
        End If
        If String.IsNullOrEmpty(formKey) Then
            Throw New ArgumentException("formKey")
        End If
        Dim header As String = String.Format(HeaderTemplate, mimeBoundary, formKey, file.Name, mimeType)
        Dim headerBytes As Byte() = Encoding.UTF8.GetBytes(header)
        stream.Write(headerBytes, 0, headerBytes.Length)

        Dim lngTotalBytes As Long = 0
        Using fileStream As New FileStream(file.FullName, FileMode.Open, FileAccess.Read)
            Dim buffer() As Byte = New Byte(1024) {}
            Dim bytesRead As Integer = 0
            bytesRead = fileStream.Read(buffer, 0, buffer.Length)
            lngTotalBytes = bytesRead
            While bytesRead > 0
                stream.Write(buffer, 0, bytesRead)
                bytesRead = fileStream.Read(buffer, 0, buffer.Length)
                lngTotalBytes = lngTotalBytes + bytesRead
            End While
            fileStream.Close()
        End Using

        Dim newLineBytes As Byte() = Encoding.UTF8.GetBytes(vbCrLf)
        stream.Write(newLineBytes, 0, newLineBytes.Length)
        lngTotalBytes = lngTotalBytes + newLineBytes.Length
        Return lngTotalBytes
    End Function

    Public Shared Function CreateFormDataBoundary() As String
        Return "---------------------------" + DateTime.Now.Ticks.ToString("x")
    End Function

    Public Shared Function GenerateMD5Hash(ByVal fileInfo As FileInfo) As String
        Dim md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
        Using fileStream As New FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read)
            md5.ComputeHash(fileStream)
            fileStream.Close()
        End Using
        'converting the bytes into string
        Dim hash As Byte() = md5.Hash
        Dim sb As StringBuilder = New StringBuilder
        For Each hashByte In hash
            sb.Append(String.Format("{0:X2}", hashByte))
        Next
        'The web service wants the hash in lower-case
        Return sb.ToString.ToLower
    End Function


End Class
