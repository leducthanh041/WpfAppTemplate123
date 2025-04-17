using WpfAppTemplate.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//using WpfAppTemplate.Views.CustomAnimation;
using Microsoft.Extensions.DependencyInjection;

namespace WpfAppTemplate.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly double collapsedWidth = 60;
        private readonly double expandedWidth = 200;
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(MainWindowViewModel vm, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            DataContext = vm;

            // configure the window
            WindowState = WindowState.Maximized;

            //NavColumn.Width = new GridLength(collapsedWidth);

            //Loaded += (s, e) =>
            //{
            //    NavigateToPage("Dashboard");
            //};
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // ActualWidth and ActualHeight now reflect the real window size
            //double width = this.ActualWidth;
            //double height = this.ActualHeight;

            //MessageBox.Show(
            //    $"Window dimensions:\nWidth = {width:N0} px\nHeight = {height:N0} px",
            //    "Window Size",
            //    MessageBoxButton.OK,
            //    MessageBoxImage.Information);
        }
    }
}
