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
            Trace.Write("order ID: " + newOrder.GetOrderID());
        }

        public void ProcessOrders(Order orderToProcess)
        {
            //Check if the customer has enough funds
            float totalCost = orderToProcess.GetCost(); //Total cost of this order
            Customer buyer = CustomerManager.FindCustomer(orderToProcess.GetName()); //Get the customer who placed order

            //If the customer has enough money and there are enough items in stock, process the order
            if (buyer.GetFunds() >= totalCost && orderToProcess.GetQuantity() <= orderToProcess.GetItem().GetQuantity())
            {
                //Subtract funds from customer's account and subtract from inventory quantity
                buyer.SetFunds(buyer.GetFunds() - totalCost);
                Item purchasedItem = Storefront.SearchInventory(orderToProcess.GetItem().GetName());
                purchasedItem.SetQuantity(purchasedItem.GetQuantity() - orderToProcess.GetQuantity());

                _processedOrders.Add(orderToProcess); //Add order to the processed list
            }
            else
            {
                if (buyer.GetFunds() >= totalCost && orderToProcess.GetQuantity() > orderToProcess.GetItem().GetQuantity()) //Customer has enough money
                {
                    Debug.Write("There are not enough items in stock!");
                }
                if (buyer.GetFunds() < totalCost && orderToProcess.GetQuantity() <= orderToProcess.GetItem().GetQuantity()) //Not enough money
                {
                    Debug.Write("You don't have enough funds in your account.");
                }
                if (buyer.GetFunds() < totalCost && orderToProcess.GetQuantity() > orderToProcess.GetItem().GetQuantity()) //Not enough money or items
                {
                    Debug.Write("There are not enough items in stock!");
                    Debug.Write("You don't have enough funds in your account.");
                }
            }
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