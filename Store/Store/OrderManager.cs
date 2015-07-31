using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Store
{
    class OrderManager
    {
        private static List<Order> _individualOrders = new List<Order>(); //List of orders made from individual items
        private static List<Order> _ordersToProcess = new List<Order>(); //List of orders that need to get processed
        private static List<Order> _processedOrders = new List<Order>(); //List of orders that have been completed
        private static  List<Order> _canceledOrders = new List<Order>(); //List of all orders that were canceled

        //Thread queues that are used to process multiple orders at once
        private static List<Order> _thread1Order = new List<Order>();
        private static List<Order> _thread2Order = new List<Order>();
        private static List<Order> _thread3Order = new List<Order>();
        private static List<Order> _thread4Order = new List<Order>();

        private static Thread _thread1;
        private static Thread _thread2;

        static readonly object locker = new object();

        public static int OrderID = 1;

        public static bool Thread1Executed = false; //Flags that are raised when worker threads finish executing

        private static bool _usingMultithread = true;

        public static bool _orderFinished = false;
        public static Order NextOrder = null;
        public static bool _createdWorkerThread1 = false;

        public static void AddOrder(Order newOrder)
        {
            if (NextOrder != null)
            {
                Trace.WriteLine("RESET");
            }
            _usingMultithread = Main.GetMultiThreadingState();
            if (_usingMultithread)
            {
                Thread1Executed = false;
            } else
            {
                Thread1Executed = true;
            }
            
            List<Order> orderToProcess = new List<Order>();
            if (Main.GetMultiThreadingState() == true) //Process orders with multiple threads
            {
                if (_createdWorkerThread1 == false)
                {
                    _createdWorkerThread1 = true;
                    // Trace.WriteLine("Made new thread!");
                    _individualOrders.Add(newOrder);
                    _thread1 = new Thread(() => AddCart(_individualOrders, newOrder.GetQuantity()));
                    _thread1.Name = "Thread1";
                    _thread1.Start();
                } else
                {
                    Trace.WriteLine("TESTING");
                    //Worker thread already exists
                    _individualOrders.Add(newOrder);
                    AddCart(_individualOrders, newOrder.GetQuantity());
                    
                }

               /* if (_thread1 != null)
                {
                    if (_thread1.IsAlive) //thread1 is busy
                    {
                        //Main thread or thread2
                    }
                    else
                    {
                       // Trace.WriteLine("Made new thread1!");
                        _individualOrders.Add(newOrder);
                        _thread1 = new Thread(() => AddCart(_individualOrders, newOrder.GetQuantity()));
                        _thread1.Name = "Thread1";
                        _thread1.Start();
                    }
                }*/

            }
            else //Process orders with single thread
            {
                Trace.WriteLine("Using the main thread to process orders!");
                _individualOrders.Add(newOrder);
                AddCart(_individualOrders, newOrder.GetQuantity());
            }

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

            if (Thread.CurrentThread.Name == "Thread1")
            {
                // Trace.WriteLine("EXECUTED THREAD");

                //Keep the thread alive
                _orderFinished = true;
                Order previousOrder = NextOrder;
                Thread1Executed = true; //Wake up the main thread

                while (NextOrder == previousOrder)
                {
                    int q = 0;
                }

                //Reset thread
                Trace.WriteLine("--STARTING NEXT ORDER--");
                if (NextOrder != previousOrder && NextOrder != null) //Check to see if there are any more orders that need to be handled
                {
                    Trace.WriteLine("ENTERED~~");
                    AddOrder(NextOrder);
                }
                else
                {
                    if (NextOrder == null)
                    {
                        Trace.WriteLine("JOINING WORKER THREADS~~");
                        _thread1.Join();
                    }
                }

                //_thread1.Join();
            }

            if (_usingMultithread == false)
            {
                Main.OrderProcessTimer.Stop();
                Main.UpdateProcessedOrders();
                Main.UpdateStorefront();
            }
        }

        public static void ProcessOrders(Order orderToProcess, int quantity)
        {
            if (Thread.CurrentThread.Name == "Thread1")
            {
              //  Trace.WriteLine("THREAD1 MADE IT!");
            }
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
              //  Trace.WriteLine("----ORDER COMPLETED----");
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
            if (Thread.CurrentThread.Name == "Thread1")
            {
               // Trace.WriteLine("THREAD1 MADE IT TO THE END!");
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

        public static void ResetWorkerThreads(Order newOrder)
        {
            NextOrder = newOrder;
            _orderFinished = false;
        }

    }
}