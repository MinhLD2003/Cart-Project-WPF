using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Model
{
    public class OrderDetail : INotifyPropertyChanged
    {
        private int _quantity;
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal UnitPrice { get; set; }
        public float Discount { get; set; }
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
