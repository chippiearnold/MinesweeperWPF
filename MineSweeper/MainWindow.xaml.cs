namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel dataContext = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = dataContext;
        }    
    }
}
