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

        private static readonly object locker = new object();

        public static int OrderID = 1;

        //Flags that are raised when worker threads finish executing
        public static bool Thread1Executed = false;
        public static bool Thread2Executed = false;
        public static bool Thread3Executed = false;
        public static bool Thread4Executed = false;

        private static bool _usingMultithread = true;
        
        //Worker Thread 1
        private static Thread _thread1;
        public static bool _thread1OrderFinished = false; //Raised when worker thread 1 completes all available orders
        public static Order NextOrder = null; //Next order in the queue for worker thread 1
        public static bool _createdWorkerThread1 = false; //Raised when worker thread 1 is first initialized

        //Worker Thread 2
        private static Thread _thread2;
        public static bool _thread2OrderFinished = true; //Raised when worker thread 2 completes all available orders
        public static Order NextOrder2 = null; //Next order in the queue for worker thread 2
        public static bool _createdWorkerThread2 = false;//Raised when worker thread 2 is first initialized

        //Worker Thread 3
        private static Thread _thread3;
        public static bool _thread3OrderFinished = true; //Raised when worker thread 3 completes all available orders
        public static Order NextOrder3 = null; //Next order in the queue for worker thread 3
        public static bool _createdWorkerThread3 = false;//Raised when worker thread 3 is first initialized

        //Worker Thread 4
        private static Thread _thread4;
        public static bool _thread4OrderFinished = true; //Raised when worker thread 4 completes all available orders
        public static Order NextOrder4 = null; //Next order in the queue for worker thread 4
        public static bool _createdWorkerThread4 = false;//Raised when worker thread 4 is first initialized

        //Store the amount of orders processed by each thread
        public static int ordersBy1 = 0;
        public static int ordersBy2 = 0;
        public static int ordersBy3 = 0;
        public static int ordersBy4 = 0;

        public static void AddOrder(Order newOrder, int thread)
        {
            _usingMultithread = Main.GetMultiThreadingState(); //Check if multiple threads will be used

            if (thread == 5) //If order is manually added process with main thread
            {
                _usingMultithread = false;
            }

            if (_usingMultithread)
            {
                Thread1Executed = false;
                Thread2Executed = false;
                Thread3Executed = false;
                Thread4Executed = false;
            } else
            {
                Thread1Executed = true;
                Thread2Executed = true;
                Thread3Executed = true;
                Thread4Executed = true;
            }
            
            List<Order> orderToProcess = new List<Order>();

            if (Main.GetMultiThreadingState() == true) //Process orders with multiple threads
            {
                //Worker thread 1
                if (_createdWorkerThread1 == false)
                {
                    _createdWorkerThread1 = true;
                    // Trace.WriteLine("Made new thread!");
                    _individualOrders.Add(newOrder);
                    _thread1 = new Thread(() => AddCart(_individualOrders, newOrder.GetQuantity()));
                    _thread1.Name = "Thread1";
                    _thread1.Start();
                }
                else
                {
                    if (thread == 1)
                    {
                        //Worker thread 1 already exists
                        _individualOrders.Add(newOrder);
                        AddCart(_individualOrders, newOrder.GetQuantity());
                    }
                }
                //Worker thread 2
                if (_createdWorkerThread2 == false && thread == 2)
                {
                    _createdWorkerThread2 = true;
                    _individualOrders.Add(newOrder);
                    _thread2 = new Thread(() => AddCart(_individualOrders, newOrder.GetQuantity()));
                    _thread2.Name = "Thread2";
                    _thread2.Start();
                }
                else
                {
                    if (thread == 2)
                    {
                        //Worker thread 2 already exists
                        _individualOrders.Add(newOrder);
                        AddCart(_individualOrders, newOrder.GetQuantity());
                    }
                }
                //Worker thread 3
                if (_createdWorkerThread3 == false && thread == 3)
                {
                    _createdWorkerThread3 = true;
                    _individualOrders.Add(newOrder);
                    _thread3 = new Thread(() => AddCart(_individualOrders, newOrder.GetQuantity()));
                    _thread3.Name = "Thread3";
                    _thread3.Start();
                }
                else
                {
                    if (thread == 3)
                    {
                        //Worker thread 3 already exists
                        _individualOrders.Add(newOrder);
                        AddCart(_individualOrders, newOrder.GetQuantity());
                    }
                }
                //Worker thread 4
                if (_createdWorkerThread4 == false && thread == 4)
                {
                    _createdWorkerThread4 = true;
                    _individualOrders.Add(newOrder);
                    _thread4 = new Thread(() => AddCart(_individualOrders, newOrder.GetQuantity()));
                    _thread4.Name = "Thread4";
                    _thread4.Start();
                }
                else
                {
                    if (thread == 4)
                    {
                        //Worker thread 4 already exists
                        _individualOrders.Add(newOrder);
                        AddCart(_individualOrders, newOrder.GetQuantity());
                    }
                }
            }

            else //Process orders with single thread
            {
                //Trace.WriteLine("Using the main thread to process orders!");
                Main.OrderProcessTimer.Start();
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
                //Keep the thread alive
                Order previousOrder = NextOrder;
                Thread1Executed = true; //Wake up the main thread
                Main.AllThreadsBusy = false;
                ordersBy1++;

                while (NextOrder == previousOrder) //Stall the worker threads until the next order is ready to be processed
                {
                    _thread1OrderFinished = true;
                }

                //Reset thread
                //Trace.WriteLine("--STARTING NEXT ORDER--");
                if (NextOrder != previousOrder && NextOrder != null) //Check to see if there are any more orders that need to be handled
                {
                    AddOrder(NextOrder, 1);
                }
                else
                {
                    if (NextOrder == null)
                    {
                        //Trace.WriteLine("JOINING WORKER THREADS");
                        _thread1.Join();
                    }
                }
            }

            else if (Thread.CurrentThread.Name == "Thread2")
            {
                //Trace.WriteLine("THREAD 2 ENTERED");
                //Keep the thread alive
                Order previousOrder = NextOrder2;
                Thread2Executed = true;
                Main.AllThreadsBusy = false;
                ordersBy2++;

                while (NextOrder2 == previousOrder)
                {
                    _thread2OrderFinished = true;
                }

                //Reset thread
                //Trace.WriteLine("--STARTING NEXT ORDER IN THREAD 2--");

                if (NextOrder2 != previousOrder && NextOrder2 != null) //Check to see if there are any more orders that need to be handled
                {
                    AddOrder(NextOrder2, 2);
                }
                else
                {
                    if (NextOrder2 == null)
                    {
                        //Trace.WriteLine("JOINING WORKER THREAD 2");
                        _thread2.Join();
                    }
                }
            }

            else if (Thread.CurrentThread.Name == "Thread3")
            {
                //Trace.WriteLine("THREAD 3 ENTERED");
                //Keep the thread alive
                Order previousOrder = NextOrder3;
                Thread3Executed = true;
                Main.AllThreadsBusy = false;
                ordersBy3++;

                while (NextOrder3 == previousOrder)
                {
                    _thread3OrderFinished = true;
                }

                //Reset thread
                //Trace.WriteLine("--STARTING NEXT ORDER IN THREAD 3--");

                if (NextOrder3 != previousOrder && NextOrder3 != null) //Check to see if there are any more orders that need to be handled
                {
                    AddOrder(NextOrder3, 3);
                }
                else
                {
                    if (NextOrder3 == null)
                    {
                        //Trace.WriteLine("JOINING WORKER THREAD 3");
                        _thread3.Join();
                    }
                }
            }

            else if (Thread.CurrentThread.Name == "Thread4")
            {
                //Trace.WriteLine("THREAD 4 ENTERED");
                //Keep the thread alive
                Order previousOrder = NextOrder4;
                Thread4Executed = true;
                Main.AllThreadsBusy = false;
                ordersBy4++;

                while (NextOrder4 == previousOrder)
                {
                    _thread4OrderFinished = true;
                }

                //Reset thread
                //Trace.WriteLine("--STARTING NEXT ORDER IN THREAD 4--");

                if (NextOrder4 != previousOrder && NextOrder4 != null) //Check to see if there are any more orders that need to be handled
                {
                    AddOrder(NextOrder4, 4);
                }
                else
                {
                    if (NextOrder4 == null)
                    {
                        //Trace.WriteLine("JOINING WORKER THREAD 4");
                        _thread4.Join();
                    }
                }
            }

            if (_usingMultithread == false)
            {
                Main.UpdateOrders();
                Main.UpdateProcessedOrders();
                Main.UpdateStorefront();
            }
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
                //Trace.WriteLine("----ORDER COMPLETED----");
            }
            else
            {
                Item purchasedItem = Storefront.SearchInventory(orderToProcess.GetCart()[0].GetName());

                if (buyer.GetFunds() >= totalCost && quantity > purchasedItem.GetQuantity()) //Customer has enough money
                {
                    //Debug.WriteLine("There are not enough items in stock!");
                    _canceledOrders.Add(orderToProcess);
                }
                if (buyer.GetFunds() < totalCost && quantity <= purchasedItem.GetQuantity()) //Not enough money
                {
                    //Debug.WriteLine("You don't have enough funds in your account.");
                    _canceledOrders.Add(orderToProcess);
                }
                if (buyer.GetFunds() < totalCost && quantity > purchasedItem.GetQuantity()) //Not enough money or items
                {
                    //Debug.WriteLine("There are not enough items in stock and you don't have enough funds in your account!");
                    _canceledOrders.Add(orderToProcess);
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

        public static void ResetWorkerThread1(Order newOrder)
        {
            NextOrder = newOrder;
            _thread1OrderFinished = false;
        }

        public static void ResetWorkerThread2(Order newOrder)
        {
            NextOrder2 = newOrder;
            _thread2OrderFinished = false;
        }

        public static void ResetWorkerThread3(Order newOrder)
        {
            NextOrder3 = newOrder;
            _thread3OrderFinished = false;
        }

        public static void ResetWorkerThread4(Order newOrder)
        {
            NextOrder4 = newOrder;
            _thread4OrderFinished = false;
        }

    }
}