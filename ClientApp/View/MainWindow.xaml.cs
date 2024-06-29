using ClientApp.Service;
using ClientApp.ViewModel;


namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    { 
        private  readonly IProductService _productService;

        public MainWindow(ShoppingViewModel viewModel, IProductService productService)
        {
            InitializeComponent();
            DataContext = viewModel;
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            LoadProduct();
        }
        private async void LoadProduct()
        {
            await _productService.GetProducts();
            ShoppingItems.ItemsSource =  _productService.GetAllProducts();
        }

      
    }
}