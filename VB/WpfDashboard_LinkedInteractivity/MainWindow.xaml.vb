Imports System.Windows

Namespace WpfDashboard_LinkedInteractivity

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Window_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            mainDashboardControl.LoadDashboard("Data\Dashboard.xml")
        End Sub

        Private Sub Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim childWindow1 As ChildWindow = New ChildWindow(mainDashboardControl)
            childWindow1.Show()
        End Sub
    End Class
End Namespace
