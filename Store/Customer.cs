using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    class Customer
    {
        private string _customerName;
        private float _accountBalance;
        private int _customerID;

        public Customer(string name, float funds, int id)
        {
            this._customerName = name;
            this._accountBalance = funds;
            this._customerID = id;
        }

        public string GetName()
        {
            return _customerName;
        }

        public float GetFunds()
        {
            return _accountBalance;
        }

        public int GetID()
        {
            return _customerID;
        }

        public void SetFunds(float amount)
        {
            _accountBalance = amount;
        }

    }
}
