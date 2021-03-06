﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    class Order
    {
        private string _customerName;
        private Item _purchasedItem;
        private List<Item> _itemCart;
        private float _accountFunds;
        private int _orderQuantity;
        private float _totalCost;
        private int _orderID;


        public Order(string name, Item item, float funds, int quantity, float cost, int id)
        {
            this._customerName = name;
            this._purchasedItem = item;
            this._accountFunds = funds;
            this._orderQuantity = quantity;
            this._totalCost = cost;
            this._orderID = id;
        }

        public Order(string name, List<Item> items, float funds, float cost, int id)
        {
            this._customerName = name;
            this._itemCart = items;
            this._accountFunds = funds;
            this._totalCost = cost;
            this._orderID = id;
        }

        public string GetName() { return _customerName;}

        public Item GetItem() { return _purchasedItem;}

        public float GetFunds() { return _accountFunds;}

        public void SetFunds(float amount) { _accountFunds = amount; }

        public int GetQuantity() { return _orderQuantity;}

        public float GetCost() { return _totalCost; }

        public int GetOrderID() { return _orderID; }

        public List<Item> GetCart() { return _itemCart; }

    }
}
