Imports System.Drawing
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Net

Public Class MAIN

    Public Function BuildList(ByRef ListOfIps)
        'Dim ListOfIps() As Array


        Dim p, q, r, s, secondoctet(2)
        secondoctet(0) = "101"
        secondoctet(1) = "10"
        secondoctet(2) = "100"
        For p = 0 To UBound(secondoctet)    'For q = 100 to 101
            q = secondoctet(p)
            For r = 0 To 253
                For s = 0 To 253
                    ListOfIps.Add("10." & q & "." & r + 1 & "." & s + 1)
                Next
            Next
        Next
        Return ListOfIps
    End Function






    Private Function Arp(ByRef Ip, ByRef ArpResults)
        Dim AllData() As String
        Dim psi As System.Diagnostics.ProcessStartInfo = New System.Diagnostics.ProcessStartInfo("arp", "-a " & Ip)
        psi.UseShellExecute = False
        psi.RedirectStandardOutput = True
        psi.CreateNoWindow = True

        Dim proc As System.Diagnostics.Process = System.Diagnostics.Process.Start(psi)
        AllData = Split(proc.StandardOutput.ReadToEnd, vbCrLf)

        For Each s As String In AllData
            If s <> Nothing Then
                Dim temp() As String = s.Split(New Char() {" "c}, System.StringSplitOptions.RemoveEmptyEntries)
                If UBound(temp) = 2 Then
                    'ArpResults = ArpResults & (temp(0).PadRight(20) & temp(1).PadRight(20) & temp(2) & vbCrLf)
                    ArpResults = (temp(1).PadRight(20))
                End If
            End If
        Next
        Return ArpResults
    End Function



    ' Public Sub MyForm_Loads()
    ' Dim Ip As String
    ' Dim PingReplied As String
    '     PingReplied = "NotSet"
    ' Dim ArpResults As String
    '     ArpResults = "NotSet"
    '
    '    For i = 1 To 40 'ListOfIps.Count
    '            Ip = ListOfIps(i)
    '            Ping(Ip, PingReplied)
    '            Arp(Ip, ArpResults)
    '    If PingReplied = "True" Then
    '                ListOfLiveIps.Items.Add(Ip & " " & ArpResults)
    '                ListOfLiveIps.Refresh()
    '    End If
    '    Next
    '    End Sub



    ''' <summary>
    ''' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''' </summary>
    Private Class AreaClass2
        Public Base As Double
        Public Height As Double
        Function CalcArea() As Double
            ' Calculate the area of a triangle.  
            Return 0.5 * Base * Height
        End Function
    End Class

    Private WithEvents BackgroundWorker1 As New System.ComponentModel.BackgroundWorker

    Private Sub MyForm_Loaddisabled()
        Dim AreaObject2 As New AreaClass2
        AreaObject2.Base = 30
        AreaObject2.Height = 40

        ' Start the asynchronous operation.  
        BackgroundWorker1.RunWorkerAsync(AreaObject2)
    End Sub

    ' This method runs on the background thread when it starts.  
    Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim AreaObject2 As AreaClass2 = CType(e.Argument, AreaClass2)
        ' Return the value through the Result property.  
        e.Result = AreaObject2.CalcArea()
    End Sub

    ' This method runs on the main thread when the background thread finishes.  
    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        ' Access the result through the Result property.  
        Dim Area As Double = CDbl(e.Result)
        MessageBox.Show("The area is: " & Area.ToString)
    End Sub



    'MyForm_Load

    '    Public Property MaxDegreeOfParallelism As Integer
    '    Parallel.ForEach
    '    listOfWebpages,
    '    New ParallelOptions { MaxDegreeOfParallelism = 4 },
    '    webpage => { Download(webpage); }
    '

    ''' <summary>
    ''' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''' </summary>
    Sub MyForm_Load()
        Dim ListOfIps As New ArrayList
        Dim ListOfLiveIps As New ListBox
        Dim ScanStatus As New TextBox
        Dim ScanStatusLabel As New Label
        'Dim ListOfLiveIps As New ArrayList
        Dim frm As New Form
        Dim MyTextbox As New TextBox
        MyTextbox.WordWrap = True
        MyTextbox.Height = 100
        MyTextbox.Width = frm.Width
        MyTextbox.Text = "NULLtxt"
        frm.Controls.Add(MyTextbox)
        ListOfLiveIps.Top = 20
        ListOfLiveIps.Width = frm.Width
        ScanStatus.Top = frm.Height - 80
        ScanStatus.Width = frm.Width
        ScanStatusLabel.Top = frm.Height - 95
        ScanStatusLabel.Width = frm.Width
        ScanStatusLabel.Height = 15
        ScanStatusLabel.Text = "Every IP Ending in 0"

        frm.Controls.Add(ListOfLiveIps)
        frm.Controls.Add(ScanStatusLabel)
        frm.Controls.Add(ScanStatus)
        frm.Show()

        BuildList(ListOfIps)




        Dim opts As New ParallelOptions
        opts.MaxDegreeOfParallelism = 20
        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False

        Dim PingReplied As String
        PingReplied = "NotSet"
        Dim ArpResults As String
        ArpResults = "NotSet"
        Task.Factory.StartNew(Sub() Parallel.ForEach(ListOfIps.Cast(Of Object), opts, Sub(Ip)

                                                                                          Dim pPing As New Ping
                                                                                          'Trace.WriteLine(Ip) ' & "START" & (DateTime.Now - New DateTime(1970, 1, 1)).TotalMilliseconds)

                                                                                          If pPing.Send(Ip, 100).Status = NetworkInformation.IPStatus.Success Then
                                                                                              Trace.WriteLine(Ip & "END" & (DateTime.Now - New DateTime(1970, 1, 1)).TotalMilliseconds)
                                                                                              Arp(Ip, ArpResults)
                                                                                              ListOfLiveIps.Items.Insert(0, Ip & " " & ArpResults)

                                                                                              If Right(Ip, 1) = 0 Then
                                                                                                  ScanStatus.Text = "Thread:" & Task.CurrentId & "called for IP:" & Ip
                                                                                              End If
                                                                                              ListOfLiveIps.Refresh()
                                                                                          End If
                                                                                          'Next
                                                                                          'MsgBox("pauser")

                                                                                      End Sub)
                                                          )
        MsgBox("Processing complete. Press any key to exit.")


    End Sub



    Sub Window1()
        Dim Window1 As New Window1
        Window1.Show()
    End Sub
End Class





