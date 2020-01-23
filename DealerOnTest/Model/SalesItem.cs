using System;
using System.Collections.ObjectModel;

namespace DealerOnTest
{
    public class SalesItem : UpdateBase
    {
        private string _name;
        private decimal _price;
        private bool _imported;
        private bool _salesTaxed;
        private int _quantity;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = Math.Round(value, 2);
                OnPropertyChanged("Price");
            }
        }
        public bool Imported
        {
            get { return _imported; }
            set
            {
                _imported = value;
                OnPropertyChanged("Imported");
            }
        }
        public bool SalesTaxed
        {
            get { return _salesTaxed; }
            set
            {
                _salesTaxed = value;
                OnPropertyChanged("SalesTaxed");
            }
        }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public SalesItem() { }

        public SalesItem(string name, decimal price, int quantity, bool imported, bool salesTaxed)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Imported = imported;
            SalesTaxed = salesTaxed;
        }
        public SalesItem(SalesItem item)
        {
            Name = item.Name;
            Price = item.Price;
            Quantity = item.Quantity;
            Imported = item.Imported;
            SalesTaxed = item.SalesTaxed;
        }

        public ObservableCollection<SalesItem> CreateDefaultItems()
        {
            var items = new ObservableCollection<SalesItem>
            {
                new SalesItem("Book", 12.49M, 1, false, false),
                new SalesItem("Music CD", 14.99M, 1, false, true),
                new SalesItem("Packet of headache pills", 9.75M, 1, false, false),
                new SalesItem("Chocolate bar", 0.85M, 1, false, false),
                new SalesItem("Imported box of chocolates", 10.00M, 1, true, false),
                new SalesItem("Imported box of chocolates", 11.25M, 1, true, false),
                new SalesItem("Bottle of perfume", 18.99M, 1, false, true),
                new SalesItem("Imported bottle of perfume", 27.99M, 1, true, true),
                new SalesItem("Imported bottle of perfume", 47.50M, 1, true, true)
            };

            return items;
        }
    }
}
