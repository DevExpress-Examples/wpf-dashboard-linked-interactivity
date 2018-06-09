using System.Windows;

namespace WpfDashboard_LinkedInteractivity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainDashboardControl.LoadDashboard("Data\\Dashboard.xml");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChildWindow childWindow1 = new ChildWindow(mainDashboardControl);
            childWindow1.Show();
        }
    }
}
