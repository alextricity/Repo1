Imports System.Threading

Class Application
    ' Public Property Logoscreen As SplashScreen
    Public Property Logoscreen As SplashScreen
    Dim MINIMUM_SPLASH_TIME As Integer = 3000


    Protected Overrides Sub OnStartup(ByVal e As StartupEventArgs)
        Dim Logoscreen As New Logoscreen

        Logoscreen.Show()
        ' Step 2 - Start a stop watch  
        Dim timer As Stopwatch = New Stopwatch
        timer.Start()
        ' Step 3 - Load your windows but don't show it yet  
        MyBase.OnStartup(e)
        Dim main As MAIN = New MAIN
        timer.Stop()
        Dim remainingTimeToShowSplash As Integer = (MINIMUM_SPLASH_TIME - CType(timer.ElapsedMilliseconds, Integer))
        If ((remainingTimeToShowSplash > 0)) Then
            Thread.Sleep(remainingTimeToShowSplash)
            Logoscreen.Close()
        End If
    End Sub
    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.
End Class
