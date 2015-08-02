﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Store
{
    public partial class Main : Form
    {
        private static TableLayoutPanel itemTable; //Table containing all items currently in store

        private static TableLayoutPanel ordersToProcessTable; //Table containing all orders that need to be processed

        private static TableLayoutPanel processedOrdersTable; //Table containing all completed orders

        private bool _activatedTabs = false; //Raised when all tabs have been activated at least once

        private string importedOrdersFilePath; //String containing the path to the imported orders
        private string importedItemsFilePath; //String containing the path to the imported items

        public static Stopwatch OrderProcessTimer = new Stopwatch(); //Timer that keeps track of how long it takes to finish processing all orders

        private static bool _enableMultipleThreads = true; //Flag that determines whether or not to use multi-threading
        private static bool _finishedProcessingOrdersFile = false;
        private readonly object locker = new object();

        public Main()
        {
            InitializeComponent();
            frontTabs.Selected += new TabControlEventHandler(frontTabs_Selected);
        }

        //Update program statistics
        private void frontTabs_Selected(object sender, TabControlEventArgs e)
        {
            if (frontTabs.SelectedIndex == 3)
            {
                UpdateProgramStats();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddItemDialog(); //Open the Add Item Form
        }

        public void AddItemDialog()
        {
            AddItemForm otherForm = new AddItemForm();
            otherForm.Show();

            //Activate tabs if not done before
            if (_activatedTabs == false)
            {
                ActivateTab(1);
                _activatedTabs = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddOrderForm otherForm = new AddOrderForm();
            otherForm.Show();

            //Activate tabs if not done before
            if (_activatedTabs == false)
            {
                ActivateTab(1);
                _activatedTabs = true;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            itemTable = tableLayoutPanel1;
        }

        public static void UpdateStorefront()
        {

            if (Storefront.Inventory.Count <= 16)
            {
                //Populate item table
                for (int i = 0; i < Storefront.Inventory.Count; i++) //Loop through every item in inventory
                {
                    for (int x = 0; x < 5; x++) //Fill out information for each item
                    {
                        Control c = itemTable.GetControlFromPosition(x, i + 1);

                        if (x == 0) //Item name
                        {
                            c.Text = Storefront.Inventory[i].GetName();
                        }
                        else if (x == 1) //Item description
                        {
                            c.Text = Storefront.Inventory[i].GetDesc();
                        }
                        else if (x == 2) //Item cost
                        {
                            c.Text = Convert.ToSingle(Storefront.Inventory[i].GetPrice()).ToString();
                        }
                        else if (x == 3) //Item stock
                        {
                            if (Storefront.Inventory[i].GetStock() == true)
                            {
                                c.Text = "Yes";
                            }
                            else
                            {
                                c.Text = "No";
                            }
                        }
                        else if (x == 4) //Item quantity
                        {
                            c.Text = Convert.ToInt32(Storefront.Inventory[i].GetQuantity()).ToString();
                        }
                    }
                }
            } else //There are more than 16 items that need to be drawn
            {
                //Populate item table
                for (int i = 0; i < 16; i++) //Loop through every item in inventory
                {
                    for (int x = 0; x < 5; x++) //Fill out information for each item
                    {
                        Control c = itemTable.GetControlFromPosition(x, i + 1);

                        if (x == 0) //Item name
                        {
                            c.Text = Storefront.Inventory[Storefront.Inventory.Count-1-i].GetName();
                        }
                        else if (x == 1) //Item description
                        {
                            c.Text = Storefront.Inventory[Storefront.Inventory.Count - 1 - i].GetDesc();
                        }
                        else if (x == 2) //Item cost
                        {
                            c.Text = Convert.ToSingle(Storefront.Inventory[Storefront.Inventory.Count - 1 - i].GetPrice()).ToString();
                        }
                        else if (x == 3) //Item stock
                        {
                            if (Storefront.Inventory[Storefront.Inventory.Count - 1 - i].GetStock() == true)
                            {
                                c.Text = "Yes";
                            }
                            else
                            {
                                c.Text = "No";
                            }
                        }
                        else if (x == 4) //Item quantity
                        {
                            c.Text = Convert.ToInt32(Storefront.Inventory[Storefront.Inventory.Count - 1 - i].GetQuantity()).ToString();
                        }
                    }
                }
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
            ordersToProcessTable = tableLayoutPanel2;
        }
        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {
            processedOrdersTable = tableLayoutPanel3;
        }

        public static void UpdateOrders()
        {
            if (OrderManager.GetOrdersToProcess().Count <= 15)
            {
                //Populate orders to process table
                for (int i = 0; i < OrderManager.GetOrdersToProcess().Count; i++) //Loop through every order
                {
                    for (int x = 0; x < 5; x++) //Fill out information for each order
                    {
                        Control c = ordersToProcessTable.GetControlFromPosition(x, i + 1);

                        if (x == 0) //Customer Name
                        {
                            //Trace.WriteLine("CURR INDEX: " + i);
                            c.Text = OrderManager.GetOrdersToProcess()[i].GetName();
                        }
                        else if (x == 1) //Item Name
                        {
                            string itemNames = "";

                            for (int q = 0; q < OrderManager.GetOrdersToProcess()[i].GetCart().Count; q++)
                            {
                                if (q == OrderManager.GetOrdersToProcess()[i].GetCart().Count - 1) //Last item in list
                                {
                                    itemNames += OrderManager.GetOrdersToProcess()[i].GetCart()[q].GetName();
                                }
                                else
                                {
                                    itemNames += OrderManager.GetOrdersToProcess()[i].GetCart()[q].GetName() + ", ";
                                }
                            }
                            c.Text = itemNames;
                        }
                        else if (x == 2) //Account Funds
                        {
                            c.Text = OrderManager.GetOrdersToProcess()[i].GetFunds().ToString();
                        }
                        else if (x == 3) //Total Cost
                        {
                            c.Text = OrderManager.GetOrdersToProcess()[i].GetCost().ToString();
                        }
                        else if (x == 4) //Order ID
                        {
                            c.Text = OrderManager.GetOrdersToProcess()[i].GetOrderID().ToString();
                        }
                    }
                }
            } else //There are more than 16 orders that need to be drawn so draw the last 16 orders
            {
                for (int i = 0; i < 16; i++)
                {
                    for (int x = 0; x < 5; x++) //Fill out information for each order
                    {
                        Control c = ordersToProcessTable.GetControlFromPosition(x, i + 1);

                        if (x == 0) //Customer Name
                        {
                            c.Text = OrderManager.GetOrdersToProcess()[OrderManager.GetOrdersToProcess().Count-i-1].GetName();
                        }
                        else if (x == 1) //Item Name
                        {
                            string itemNames = "";
                            
                            for (int q = 0; q < OrderManager.GetOrdersToProcess()[OrderManager.GetOrdersToProcess().Count - i - 1].GetCart().Count; q++)
                            {
                                if (q == OrderManager.GetOrdersToProcess()[OrderManager.GetOrdersToProcess().Count - i - 1].GetCart().Count - 1) //Last item in list
                                {
                                    itemNames += OrderManager.GetOrdersToProcess()[OrderManager.GetOrdersToProcess().Count - i - 1].GetCart()[q].GetName();
                                }
                                else
                                {
                                    itemNames += OrderManager.GetOrdersToProcess()[OrderManager.GetOrdersToProcess().Count - i - 1].GetCart()[q].GetName() + ", ";
                                }
                            }
                            c.Text = itemNames;
                        }
                        else if (x == 2) //Account Funds
                        {
                            c.Text = OrderManager.GetOrdersToProcess()[OrderManager.GetOrdersToProcess().Count - i - 1].GetFunds().ToString();
                        }
                        else if (x == 3) //Total Cost
                        {
                            c.Text = OrderManager.GetOrdersToProcess()[OrderManager.GetOrdersToProcess().Count - i - 1].GetCost().ToString();
                        }
                        else if (x == 4) //Order ID
                        {
                            c.Text = OrderManager.GetOrdersToProcess()[OrderManager.GetOrdersToProcess().Count - i - 1].GetOrderID().ToString();
                        }
                    }
                }
            }

        }

        public static void UpdateProcessedOrders()
        {

            if (OrderManager.GetProcessedOrders().Count <= 16)
            {
                //Populate processed table
                for (int i = 0; i < OrderManager.GetProcessedOrders().Count; i++) //Loop through every order
                {
                    for (int x = 0; x < 5; x++) //Fill out information for each order
                    {
                        Control c = processedOrdersTable.GetControlFromPosition(x, i + 1);

                        if (x == 0) //Customer Name
                        {
                            c.Text = OrderManager.GetProcessedOrders()[i].GetName();
                        }
                        else if (x == 1) //Item Name
                        {
                            string itemNames = "";

                            for (int q = 0; q < OrderManager.GetProcessedOrders()[i].GetCart().Count; q++)
                            {
                                if (q == OrderManager.GetProcessedOrders()[i].GetCart().Count - 1) //Last item in list
                                {
                                    itemNames += OrderManager.GetProcessedOrders()[i].GetCart()[q].GetName();
                                }
                                else
                                {
                                    itemNames += OrderManager.GetProcessedOrders()[i].GetCart()[q].GetName() + ", ";
                                }
                            }
                            c.Text = itemNames;
                        }
                        else if (x == 2) //Account Funds
                        {
                            c.Text = OrderManager.GetProcessedOrders()[i].GetFunds().ToString();
                        }
                        else if (x == 3) //Total Cost
                        {
                            c.Text = OrderManager.GetProcessedOrders()[i].GetCost().ToString();
                        }
                        else if (x == 4) //Order ID
                        {
                            c.Text = OrderManager.GetProcessedOrders()[i].GetOrderID().ToString();
                        }
                    }
                }
            }
            else //There are more than 16 orders that need to be drawn so draw the last 16 orders that were processed
            {
                //Populate processed table
                for (int i = 0; i < 16; i++) //Loop through every order
                {
                    for (int x = 0; x < 5; x++) //Fill out information for each order
                    {
                        Control c = processedOrdersTable.GetControlFromPosition(x, i + 1);

                        if (x == 0) //Customer Name
                        {
                            c.Text = OrderManager.GetProcessedOrders()[OrderManager.GetProcessedOrders().Count - i - 1].GetName();
                        }
                        else if (x == 1) //Item Name
                        {
                            string itemNames = "";

                            for (int q = 0; q < OrderManager.GetProcessedOrders()[OrderManager.GetProcessedOrders().Count - i - 1].GetCart().Count; q++)
                            {
                                if (q == OrderManager.GetProcessedOrders()[OrderManager.GetProcessedOrders().Count - i - 1].GetCart().Count - 1) //Last item in list
                                {
                                    itemNames += OrderManager.GetProcessedOrders()[OrderManager.GetProcessedOrders().Count - i - 1].GetCart()[q].GetName();
                                }
                                else
                                {
                                    itemNames += OrderManager.GetProcessedOrders()[OrderManager.GetProcessedOrders().Count - i - 1].GetCart()[q].GetName() + ", ";
                                }
                            }
                            c.Text = itemNames;
                        }
                        else if (x == 2) //Account Funds
                        {
                            c.Text = OrderManager.GetProcessedOrders()[OrderManager.GetProcessedOrders().Count - i - 1].GetFunds().ToString();
                        }
                        else if (x == 3) //Total Cost
                        {
                            c.Text = OrderManager.GetProcessedOrders()[OrderManager.GetProcessedOrders().Count - i - 1].GetCost().ToString();
                        }
                        else if (x == 4) //Order ID
                        {
                            c.Text = OrderManager.GetProcessedOrders()[OrderManager.GetProcessedOrders().Count - i - 1].GetOrderID().ToString();
                        }
                    }
                }
            }


        }

        public void UpdateProgramStats() //Update all program statistics
        {
            processedOrdersStat.Text = OrderManager.GetProcessedOrders().Count.ToString();
            canceledOrdersStat.Text = OrderManager.GetCanceledOrders().Count.ToString();
            totalCustomersStat.Text = CustomerManager.GetCustomers().Count.ToString();
            totalProcessRuntimeStat.Text = OrderProcessTimer.ElapsedMilliseconds.ToString() + " ms";
        }

        public void ActivateTab(int tabIndex)
        {
            frontTabs.SelectedIndex = tabIndex;

            if (tabIndex == 1)
            {
                ActivateTab(2);
            }
            if (tabIndex == 2)
            {
                ActivateTab(3);
            }
            if (tabIndex == 3)
            {
                ActivateTab(0);
            }
        }

        private void importFileButton_Click(object sender, EventArgs e)
        {
            importFileDialog.ShowDialog();

            //Activate tabs if not done before
            if (_activatedTabs == false)
            {
                ActivateTab(1);
                _activatedTabs = true;
            }
        }

        private void importFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            importedOrdersFilePath = importFileDialog.FileName;
            ProcessImportedOrders(importedOrdersFilePath);
        }

        //Parse the orders that were obtained from the document
        private void ProcessImportedOrders(string path)
        {
            char[] delims = new char[] { ',' };
            string[] result;

            StreamReader openedFile = new StreamReader(path);

            Thread timerThread = new Thread(() => StartMultiThreadedTimer());
            timerThread.Name = "TimerThread";
            bool timerThreadStarted = false; //Flag used to start timer thread

            List<Order> ordersToProcess = BuildOrdersFromFile(path);
            
            for (int i = 0; i < ordersToProcess.Count; i++)
            {
                //Create order from file and add to order queue
                float totalCost = Storefront.SearchInventory(ordersToProcess[i].GetItem().GetName()).GetPrice() * Convert.ToInt32(ordersToProcess[i].GetQuantity());

                Customer buyer = CustomerManager.FindCustomer(ordersToProcess[i].GetName());

                if (buyer == null)
                {
                    //If first order was given
                    if (OrderManager._createdWorkerThread1 == false)
                    {

                        if (_enableMultipleThreads == false) //Enable timer for single-thread mode
                        {
                            OrderProcessTimer.Start();
                        }

                        OrderManager.AddOrder(ordersToProcess[i], 1);
                    }
                    else
                    {
                        bool passedOrder = false; //Flag that is raised if an order is passed to thread2 if the first worker thread is busy

                        while (OrderManager._thread1OrderFinished == false) //Wait until worker threads finish processing orders before passing another
                        {
                            Thread.Sleep(1);

                            //If thread1 is still busy, pass the next order to thread2
                            if (passedOrder == false)
                            {
                                if (OrderManager._createdWorkerThread2 == false)
                                {
                                    OrderManager.AddOrder(ordersToProcess[i], 2);
                                }
                                else
                                {
                                    OrderManager.ResetWorkerThread2(ordersToProcess[i]);
                                }
                                passedOrder = true;
                            }
                        }

                        if (passedOrder == false) //If order was not passed to thread2, then give it to thread1
                        {
                            //Worker threads already exist so reset flag and pass new orders
                            OrderManager.ResetWorkerThread1(ordersToProcess[i]);
                        }
                        else
                        {
                            passedOrder = false;
                        }
                    }

                    //Keep track of process time
                    if (_enableMultipleThreads)
                    {
                        if (timerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin) //If timer thread is sleeping
                        {
                            timerThread.Interrupt();
                        }
                        if (timerThreadStarted == false)
                        {
                            timerThread.Start(); //Tell timer thread to start
                            timerThreadStarted = true;
                        }
                    }
                    else
                    {
                        OrderProcessTimer.Start(); //Start the process timer
                    }
                }
                else
                {
                    //Obtain buyer's account history
                    ordersToProcess[i].SetFunds(buyer.GetFunds());

                    //If first order was given
                    if (OrderManager._createdWorkerThread1 == false)
                    {
                        OrderManager.AddOrder(ordersToProcess[i], 1);
                    }
                    else
                    {
                        bool passedOrder = false; //Flag that is raised if an order is passed to thread2 if the first worker thread is busy

                        while (OrderManager._thread1OrderFinished == false) //Wait until worker threads finish processing orders before passing another
                        {
                            Thread.Sleep(1);

                            //If thread1 is still busy, pass the next order to thread2
                            if (passedOrder == false)
                            {
                                if (OrderManager._createdWorkerThread2 == false)
                                {
                                    OrderManager.AddOrder(ordersToProcess[i], 2);
                                }
                                else
                                {
                                    while (OrderManager._thread2OrderFinished == false)
                                    {
                                        Thread.Sleep(1);
                                    }
                                    OrderManager.ResetWorkerThread2(ordersToProcess[i]);
                                }
                                passedOrder = true;
                            }
                        }

                        if (passedOrder == false) //If order was not passed to thread2, then give it to thread1
                        {
                            //Worker threads already exist so reset flag and pass new orders
                            OrderManager.ResetWorkerThread1(ordersToProcess[i]);
                        }
                        else
                        {
                            passedOrder = false;
                        }
                    }

                    //Keep track of process time
                    if (_enableMultipleThreads)
                    {
                        if (timerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin) //If timer thread is sleeping
                        {
                            timerThread.Interrupt();
                        }
                        if (timerThreadStarted == false)
                        {
                            timerThread.Start(); //Tell timer thread to start
                            timerThreadStarted = true;
                        }
                    }
                    else
                    {
                        OrderProcessTimer.Start(); //Start the process timer
                    }
                }

                while (OrderManager.Thread1Executed == false)
                {
                    Thread.Sleep(1);
                }

                if (_enableMultipleThreads)
                {
                    StopMultiThreadedTimer();
                    UpdateOrders();
                    UpdateProcessedOrders();
                    UpdateStorefront();
                }
                //Trace.WriteLine("WOKE UP MAIN THREAD!");
            }
            //Trace.WriteLine("MAIN THREAD FINISHED HANDING ORDERS!");

            if (_enableMultipleThreads)
            {
                OrderManager.ResetWorkerThread1(null);
                OrderManager.ResetWorkerThread2(null);
                _finishedProcessingOrdersFile = true;
                Thread.Sleep(5);
                timerThread.Join();
            }
            OrderProcessTimer.Stop();
        }

        private List<Order> BuildOrdersFromFile(string path)
        {
            char[] delims = new char[] { ',' };
            string[] result;

            List<Order> builtOrders = new List<Order>();

            StreamReader openedFile = new StreamReader(path);

            while (openedFile.EndOfStream == false)
            {
                string line = openedFile.ReadLine();

                result = line.Split(delims); //Split string by delims

                //Remove extra space in each line
                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i][0] == ' ')
                    {
                        result[i] = result[i].Substring(1);
                    }
                }

                //Create order from file and add to order queue
                float totalCost = Storefront.SearchInventory(result[1]).GetPrice() * Convert.ToInt32(result[3]);
                
                Order orderToAdd = new Order(result[0], Storefront.SearchInventory(result[1]), Convert.ToInt32(result[2]), Convert.ToInt32(result[3]), totalCost, OrderManager.GetOrdersToProcess().Count);

                if (orderToAdd != null)
                {
                    builtOrders.Add(orderToAdd);
                }

            }
            return builtOrders;
        }

        private void importInventoryButton_Click(object sender, EventArgs e)
        {
            importItemDialog.ShowDialog();

            //Activate tabs if not done before
            if (_activatedTabs == false)
            {
                ActivateTab(1);
                _activatedTabs = true;
            }
        }

        private void importItemDialog_FileOk(object sender, CancelEventArgs e)
        {
            importedItemsFilePath = importItemDialog.FileName;
            ProcessImportedInventory(importedItemsFilePath);
        }

        //Parse the items that were obtained from the document
        private void ProcessImportedInventory(string path)
        {
            char[] delims = new char[] { ',' };
            string[] result;

            StreamReader openedFile = new StreamReader(path);

            while (openedFile.EndOfStream == false)
            {
                string line = openedFile.ReadLine();

                result = line.Split(delims); //Split string by delims

                //Remove extra space in name
                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i][0] == ' ')
                    {
                        result[i] = result[i].Substring(1);
                    }
                }

                //Create item from file and add to storefront
                Item itemToAdd = new Item(result[0], result[1], Convert.ToInt32(result[2]), Convert.ToBoolean(result[3]), Convert.ToInt32(result[4]));
                Storefront.AddItem(itemToAdd);
                UpdateStorefront();
            }
        }

        private void multiThreadCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (multiThreadCheckbox.Checked == true)
            {
                _enableMultipleThreads = true;
            } else
            {
                _enableMultipleThreads = false;
            }
        }

        public static bool GetMultiThreadingState()
        {
            return _enableMultipleThreads;
        }

        public void StartMultiThreadedTimer()
        {
            while (Thread.CurrentThread.Name == "TimerThread") //Keep timer thread spinning until all orders are processed
            {
                if (OrderManager.Thread1Executed == true)
                {
                    OrderProcessTimer.Stop();
                    //Trace.WriteLine("TIMER THREAD FELL ASLEEP!");
                }
                else
                {
                    //Used to start timer by second worker thread to keep track of other worker thread speeds
                    OrderProcessTimer.Start();
                }
                if (_finishedProcessingOrdersFile)
                {
                    break;
                }
            }

        }

        public void StopMultiThreadedTimer()
        {
            OrderProcessTimer.Stop();
        }

    }
}
