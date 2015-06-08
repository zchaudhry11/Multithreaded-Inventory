using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    class Item
    {
        private string _itemName;
        private string _itemDesc;
        private float _itemCost;
        private bool _inStock;
        private int _quantity;

        public Item(string name, string desc, float cost, bool stock, int quantity)
        {
            this._itemName = name;
            this._itemDesc = desc;
            this._itemCost = cost;
            this._inStock = stock;
            this._quantity = quantity;
        }

        public void SetName(string name)
        {
            _itemName = name;
        }

        public void SetPrice(float cost)
        {
            _itemCost = cost;
        }

        public void SetDesc(string desc)
        {
            _itemDesc = desc;
        }

        public void SetStock(bool stock)
        {
            _inStock = stock;
            if (stock == false) SetQuantity(0);
        }

        public void SetQuantity(int amount)
        {
            _quantity = amount;
            if (_quantity < 0) _quantity = 0;

            //Disable price if there are none available
            if (_quantity == 0) SetStock(false);
        }

        public string GetName()
        {
            return _itemName;
        }

        public string GetDesc()
        {
            return _itemDesc;
        }

        public float GetPrice()
        {
            return _itemCost;
        }

        public bool GetStock()
        {
            return _inStock;
        }

        public int GetQuantity()
        {
            return _quantity;
        }
    }
}
