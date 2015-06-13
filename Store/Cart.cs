using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    class Cart
    {
        private List<Item> _itemsInCart = new List<Item>();
        private float _totalCartCost = 0;

        public Cart(List<Item> items, float cost)
        {
            this._itemsInCart = items;
            this._totalCartCost = cost;
        }

        public List<Item> GetCart()
        {
            return _itemsInCart;
        }

        public float GetTotalCost()
        {
            return _totalCartCost;
        }

    }
}
