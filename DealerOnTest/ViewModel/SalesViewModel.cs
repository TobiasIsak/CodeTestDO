using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DealerOnTest
{
    class SalesViewModel : UpdateBase
    {
        private readonly decimal _salesTaxRate = 0.10M;
        private readonly decimal _importTaxRate = 0.05M;
        private bool _shouldShowCleaningButton;

        public ObservableCollection<SalesItem> Items { get; private set; }
        public ObservableCollection<SalesItem> ShoppingList { get; private set; }
        public ObservableCollection<string> Receipt { get; private set; }
        public SalesItem NewItem { get; set; }

        public bool ShouldShowCleaningButton
        {
            get { return _shouldShowCleaningButton; }
            set
            {
                _shouldShowCleaningButton = value;
                OnPropertyChanged("ShouldShowCleaningButton");
            }
        }

        public SalesViewModel()
        {
            ShoppingList = new ObservableCollection<SalesItem>();
            Items = new SalesItem().CreateDefaultItems();
            Receipt = new ObservableCollection<string>();
            NewItem = new SalesItem("", 0m, 1, false, false);
            ShouldShowCleaningButton = false;
        }

        #region Commands
        private RelayCommand _addToShoppingListCommand;
        private RelayCommand _removeCommand;
        private RelayCommand _removeAllCommand;
        private RelayCommand _addNewItemCommand;

        public RelayCommand BuyItem
        {
            get
            {
                if (_addToShoppingListCommand == null)
                    _addToShoppingListCommand = new RelayCommand((param) =>
                    {
                        SalesItem item = param as SalesItem;
                        AddItemToShoppingList(item);
                        ProduceReceipt();
                    });

                return _addToShoppingListCommand;
            }
        }

        public RelayCommand RemoveItem
        {
            get
            {
                if (_removeCommand == null)
                    _removeCommand = new RelayCommand((param) =>
                    {
                        SalesItem item = param as SalesItem;
                        RemoveItemFromShoppingList(item);
                        ProduceReceipt();
                    });

                return _removeCommand;
            }
        }
        public RelayCommand RemoveAll
        {
            get
            {
                if (_removeAllCommand == null)
                    _removeAllCommand = new RelayCommand((param) =>
                    {
                        RemoveAllItemsFromShoppingList();
                        ProduceReceipt();
                    });

                return _removeAllCommand;
            }
        }
        public RelayCommand AddNewItem
        {
            get
            {
                if (_addNewItemCommand == null)
                    _addNewItemCommand = new RelayCommand((param) =>
                    {
                        AddNewItemToStore();
                    });

                return _addNewItemCommand;
            }
        }

        private void AddNewItemToStore()
        {
            try
            {
                if (NewItem.Name.Length < 1 || NewItem.Price < 0.05m)
                    throw new Exception();

                Items.Add(new SalesItem(NewItem));
                NewItem.Name = "";
                NewItem.Price = 0;
                NewItem.Imported = false;
                NewItem.SalesTaxed = false;
            }
            catch
            {
                MessageBox.Show("Oups! No proper validation error here (yet), but try again!");
            }
        }
        #endregion

        private void RemoveAllItemsFromShoppingList()
        {
            ShoppingList.Clean(x => x != null);
        }

        private void AddItemToShoppingList(SalesItem item)
        {
            var matchingItem = ShoppingList.Where(x => x.Name.Contains(item.Name)).FirstOrDefault();

            if (matchingItem != null)
            {
                matchingItem.Quantity += 1;
                var s = ShoppingList;
            }
            else
            {
                ShoppingList.Add(new SalesItem(item));
            }
        }

        private void RemoveItemFromShoppingList(SalesItem item)
        {
            var matchingItem = ShoppingList.Where(x => x.Name.Contains(item.Name)).FirstOrDefault();

            if (matchingItem == null)
                return;
            else if (matchingItem != null && matchingItem.Quantity > 1)
                matchingItem.Quantity -= 1;
            else if (matchingItem.Quantity == 1)
                ShoppingList.Remove(item);
        }

        private void ProduceReceipt()
        {
            Receipt.Clean(x => x != null);

            if (ShoppingList.Count == 0)
            {
                ShouldShowCleaningButton = false;
                return;
            }

            var taxSummary = 0m;
            var shoppingCartTotal = 0m;

            foreach (var item in ShoppingList)
            {
                var taxPerItemTotal = 0m;

                if (item.SalesTaxed)
                    taxPerItemTotal += CalculateItemTax(item, _salesTaxRate);

                if (item.Imported)
                    taxPerItemTotal += CalculateItemTax(item, _importTaxRate);

                var itemPriceIncludingQuantityAndTaxes = (item.Price * item.Quantity) + (taxPerItemTotal * item.Quantity);
                var receiptText = $"{item.Name}: {itemPriceIncludingQuantityAndTaxes.ToString("0.00")}";
                var singleItemPricePlusTaxes = item.Price + taxPerItemTotal;

                shoppingCartTotal += itemPriceIncludingQuantityAndTaxes;
                taxSummary += taxPerItemTotal * item.Quantity;

                if (item.Quantity > 1)
                    receiptText += $" ({item.Quantity} @ {singleItemPricePlusTaxes.ToString("0.00")})";

                Receipt.Add(receiptText);
            }

            ShouldShowCleaningButton = true;

            Receipt.Add($"Sales Taxes: {taxSummary.ToString("0.00")}");
            Receipt.Add($"Total: {shoppingCartTotal.ToString("0.00")}");
            OnPropertyChanged("ShoppingList");
        }

        private decimal CalculateItemTax(SalesItem item, decimal rate)
        {
            var minimumUnit = 0.05M;
            var taxBaseValue = item.Price * rate;
            var taxValue = taxBaseValue + minimumUnit;
            var remainder = taxBaseValue % minimumUnit;

            if (remainder == 0)
                remainder = minimumUnit;

            return taxValue - remainder;
        }
    }
}