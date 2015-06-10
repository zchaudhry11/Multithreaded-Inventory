using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    class OrderManager
    {
        private static List<Order> _ordersToProcess = new List<Order>(); //List of orders that need to get processed
        private static List<Order> _processedOrders = new List<Order>(); //List of orders that have been completed
        private static  List<Order> _canceledOrders = new List<Order>(); //List of all orders that were canceled

        public static int OrderID = 1;

        public static void AddOrder(Order newOrder)
        {
            _ordersToProcess.Add(newOrder);
            OrderID++;

            /*
            //Check if the customer has enough funds
            float totalCost = newOrder.GetItem().GetPrice() * newOrder.GetQuantity(); //Total cost of this order

            //If the customer has enough money and there are enough items in stock, process the order
            if (newOrder.GetFunds() >= totalCost && newOrder.GetQuantity() <= newOrder.GetItem().GetQuantity())
            {
                _ordersToProcess.Add(newOrder);
            }
            else
            {
                if (newOrder.GetFunds() >= totalCost && newOrder.GetQuantity() > newOrder.GetItem().GetQuantity()) //Customer has enough money
                {
                    Debug.Write("There are not enough items in stock!");
                }
                if (newOrder.GetFunds() < totalCost && newOrder.GetQuantity() <= newOrder.GetItem().GetQuantity()) //Not enough money
                {
                    Debug.Write("You don't have enough funds in your account.");
                }
                if (newOrder.GetFunds() < totalCost && newOrder.GetQuantity() > newOrder.GetItem().GetQuantity()) //Not enough money or items
                {
                    Debug.Write("There are not enough items in stock!");
                    Debug.Write("You don't have enough funds in your account.");
                }
            }*/
        }

        public void ProcessOrders()
        {

        }

        public static List<Order> GetOrdersToProcess()
        {
            return _ordersToProcess;
        }

        public static List<Order> GetProcessedOrders()
        {
            return _processedOrders;
        }

        public static List<Order> GetCanceledOrders()
        {
            return _canceledOrders;
        }

    }
}
