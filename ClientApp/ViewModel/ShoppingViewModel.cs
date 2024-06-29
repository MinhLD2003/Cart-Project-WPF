using ClientApp.Service;
using ClientApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace ClientApp.ViewModel
{
    public class ShoppingViewModel : INotifyPropertyChanged
    {
        private readonly IProductService _productService;
        private ObservableCollection<Product> _shoppingItems;
        private ObservableCollection<OrderDetail> _shoppingCart;
        public ObservableCollection<Product> ShoppingItems
        {
            get { return _shoppingItems; }
            set
            {
                _shoppingItems = value;
                OnPropertyChanged(nameof(ShoppingItems));
            }
        }
        public ObservableCollection<OrderDetail> ShoppingCart
        {
            get { return _shoppingCart; }
            set
            {
                _shoppingCart = value;
                OnPropertyChanged(nameof(ShoppingCart));
            }
        }
        public ICommand AddToCartCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand CheckOutCommand { get; }
        public ShoppingViewModel(IProductService productService)
        {

            ShoppingCart = new ObservableCollection<OrderDetail>();
            AddToCartCommand = new RelayCommand<Product>(AddToCart);
            RemoveFromCartCommand = new RelayCommand<OrderDetail>(RemoveFromCart);
            IncreaseQuantityCommand = new RelayCommand<OrderDetail>(IncreaseQuantity);
            DecreaseQuantityCommand = new RelayCommand<OrderDetail>(DecreaseQuantity);
            CheckOutCommand = new RelayCommand<ObservableCollection<OrderDetail>>(CheckOut);
            _productService = productService;
        }
        private async void CheckOut(ObservableCollection<OrderDetail> shoppingCart)
        {
            var order = new Order
            {
                OrderDetails = shoppingCart.ToList()
            };

            if (order != null && order.OrderDetails.Count > 0)
            {
                bool OK = await _productService.Order(order);

                if (OK)
                {
                    MessageBox.Show("Purchase successful!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                    shoppingCart.Clear();
                }
                else
                {
                    MessageBox.Show("Purchase failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return;
            }
            MessageBox.Show("Cart is empty ! Please choose products", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }
        private void AddToCart(Product product)
        {
            if (product != null)
            {
                var itemInCart = ShoppingCart.FirstOrDefault(od => od.ProductId == product.ProductId);
                if (itemInCart != null)
                {
                    itemInCart.Quantity++;
                }
                else
                {
                    var newOrderDetail = new OrderDetail
                    {
                        ProductId = product.ProductId,
                        UnitPrice = product.UnitPrice ?? 0,
                        Quantity = 1,
                        Discount = 0,
                        Product = product,
                    };
                    ShoppingCart.Add(newOrderDetail);
                }
            }
        }
        private void IncreaseQuantity(OrderDetail orderDetail)
        {
            if (orderDetail != null)
            {
                orderDetail.Quantity++;
            }
        }
        private void DecreaseQuantity(OrderDetail orderDetail)
        {
            if (orderDetail != null)
            {
                if (orderDetail.Quantity >= 1)
                {
                    orderDetail.Quantity--;
                }
                else
                {
                    RemoveFromCart(orderDetail);
                }
            }
        }
        private void RemoveFromCart(OrderDetail orderDetail)
        {
            if (orderDetail != null)
            {
                ShoppingCart.Remove(orderDetail);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
