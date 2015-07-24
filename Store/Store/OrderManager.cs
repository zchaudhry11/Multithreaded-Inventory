using System.Collections.Generic;
using System.Diagnostics;

namespace Store
{
    class OrderManager
    {
        private static List<Order> _individualOrders = new List<Order>(); //List of orders made from individual items
        private static List<Order> _ordersToProcess = new List<Order>(); //List of orders that need to get processed
        private static List<Order> _processedOrders = new List<Order>(); //List of orders that have been completed
        private static  List<Order> _canceledOrders = new List<Order>(); //List of all orders that were canceled

        //Thread queues that are used to process multiple orders at once
        private static List<Order> thread1 = new List<Order>();
        private static List<Order> thread2 = new List<Order>();
        private static List<Order> thread3 = new List<Order>();
        private static List<Order> thread4 = new List<Order>();

        public static int OrderID = 1;

        public static void AddOrder(Order newOrder)
        {
            _individualOrders.Add(newOrder);
            AddCart(_individualOrders, newOrder.GetQuantity());
            Main.UpdateOrders(); //Update order UI
        }
        
        public static void AddCart(List<Order> orders, int quantity)
        {
            int orderCounter = 0;
            int numOrders = _individualOrders.Count;
            //Loop through all orders and place items into carts to store in final task list
            while (orderCounter < numOrders)
            {
                Customer testCust = CustomerManager.FindCustomer(_individualOrders[0].GetName());

                if (testCust == null) //Add new customer to list
                {
                    CustomerManager.AddCustomer(new Customer(_individualOrders[0].GetName(), _individualOrders[0].GetFunds(), CustomerManager.CustomerID));
                }

                int id = CustomerManager.FindCustomer(_individualOrders[0].GetName()).GetID(); //Find all orders with this customer id
                List<Order> cartOrders = new List<Order>(); //All orders placed by a single customer

                for (int i = 0; i < _individualOrders.Count; i++)
                {
                    if (CustomerManager.FindCustomer(_individualOrders[0].GetName()).GetID() == id) //Customer found
                    {
                        //Create cart from items in order
                        cartOrders.Add(_individualOrders[i]);
                        orderCounter++;
                    }
                }

                //Place all items into cart
                float totalCost = 0;
                List<Item> items = new List<Item>();
                
                for (int i = 0; i < orders.Count; i++)
                {
                    items.Add(cartOrders[i].GetItem());
                    totalCost += cartOrders[i].GetCost();
                }

                //Create new order
                Order finalOrder = new Order(cartOrders[0].GetName(), items, cartOrders[0].GetFunds(), totalCost, OrderID);
                _ordersToProcess.Add(finalOrder);
                ProcessOrders(finalOrder, quantity);

                //Remove handled items from original list
                for (int i = 0; i < cartOrders.Count; i++)
                {
                    _individualOrders.Remove(cartOrders[i]);
                }
                OrderID++; //Set new order ID for new customer
            }

            //Update processed orders UI
            Main.OrderProcessTimer.Stop(); //Stop process timer for order
            Main.UpdateProcessedOrders();
            Main.UpdateStorefront();
        }

        public static void ProcessOrders(Order orderToProcess, int quantity)
        {
            //Check if the customer has enough funds
            float totalCost = orderToProcess.GetCost(); //Total cost of this order
            Customer buyer = CustomerManager.FindCustomer(orderToProcess.GetName()); //Get the customer who placed order

            //If the customer has enough money and there are enough items in stock, process the order
            if (buyer.GetFunds() >= totalCost && quantity <= Storefront.SearchInventory(orderToProcess.GetCart()[0].GetName()).GetQuantity())
            {
                //Subtract funds from customer's account and subtract from inventory quantity
                buyer.SetFunds(buyer.GetFunds() - totalCost);
                Item purchasedItem = Storefront.SearchInventory(orderToProcess.GetCart()[0].GetName()); //Get the item that the buyer purchased

                //Update quantity in the inventory
                int newQuantity = purchasedItem.GetQuantity() - quantity;
                Storefront.UpdateItemQuantity(purchasedItem.GetName(), newQuantity);

                _processedOrders.Add(orderToProcess); //Add order to the processed list
            }
            else
            {
                Item purchasedItem = Storefront.SearchInventory(orderToProcess.GetCart()[0].GetName());

                if (buyer.GetFunds() >= totalCost && quantity > purchasedItem.GetQuantity()) //Customer has enough money
                {
                    Debug.WriteLine("There are not enough items in stock!");
                    _canceledOrders.Add(orderToProcess);
                }
                if (buyer.GetFunds() < totalCost && quantity <= purchasedItem.GetQuantity()) //Not enough money
                {
                    Debug.WriteLine("You don't have enough funds in your account.");
                    _canceledOrders.Add(orderToProcess);
                }
                if (buyer.GetFunds() < totalCost && quantity > purchasedItem.GetQuantity()) //Not enough money or items
                {
                    Debug.WriteLine("There are not enough items in stock!");
                    Debug.WriteLine("You don't have enough funds in your account.");
                    _canceledOrders.Add(orderToProcess);
                }
            }
            Main.OrderProcessTimer.Stop();
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