using System.Windows;

namespace DealerOnTest
{
    public partial class MainWindow : Window
    {
        private readonly SalesViewModel _salesViewModel;
        public MainWindow()
        {
            InitializeComponent();
            _salesViewModel = new SalesViewModel();
            DataContext = _salesViewModel;
        }
    }
}
