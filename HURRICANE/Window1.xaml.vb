Imports System.Windows.Forms
Imports System.Drawing
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Net

Public Class Window1
    Sub Window1Loaded()
        StartGrid.Visibility = Visibility.Collapsed
    End Sub
    Sub Closer()
        Me.Close()
    End Sub

    Dim startmanustatus As Boolean
    Sub Start()
        'MsgBox("test")

        If startmanustatus = False Then
            StartGrid.Visibility = Visibility.Visible
            startmanustatus = True
        Else
            StartGrid.Visibility = Visibility.Collapsed
            startmanustatus = False
        End If

    End Sub
    Private Sub ViewAllTickets()
        System.Diagnostics.Process.Start("http://10.10.4.23:8081/helpdesk/WebObjects/Helpdesk.woa")
    End Sub
    Private Sub NewTicket()
        System.Diagnostics.Process.Start("http://10.10.4.23:8081/helpdesk/WebObjects/Helpdesk.woa/wo/22.7.25.0.0.0.0.2.5.0.0.1.6.3.9.3.1")
    End Sub
    Sub PullTickets()

        'We this to make an HTTP web request
        Dim req As Net.HttpWebRequest = Net.WebRequest.Create("http://10.10.4.23:8081/helpdesk/WebObjects/Helpdesk.woa/ra/Tickets/group?page=1&limit=3&apiKey=rF7bBUe1ROXYl4NjHD6zmXh2ZVSHQiXkKi0NC0VE")

        'Make the web request and get the response
        Dim response As Net.WebResponse = req.GetResponse

        Dim stream As System.IO.Stream = response.GetResponseStream

        Dim encode As Encoding = System.Text.Encoding.GetEncoding("utf-8")

        ' Pipe the stream to a higher level stream reader with the required encoding format.
        Dim readStream As New StreamReader(stream, encode)

        ' Read 256 charcters at a time    .
        Dim count As String = readStream.ReadToEnd()
        Console.WriteLine(count)
        response.Close()
        stream.Close()
    End Sub
End Class
