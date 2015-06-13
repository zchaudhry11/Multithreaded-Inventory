using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Store
{
    class CustomerManager
    {
        private static List<Customer> _customerList = new List<Customer>();
        public static int CustomerID = 1;

        public static List<Customer> GetCustomers()
        {
            return _customerList;
        }

        public static void AddCustomer(Customer customer)
        {
            _customerList.Add(customer);
            CustomerID++;
        }

        public static Customer FindCustomer(string name)
        {
            for (int i = 0; i < _customerList.Count; i++)
            {
                if (_customerList[i].GetName() == name)
                {
                    return _customerList[i];
                }
            }
            return null;
        }

    }
}