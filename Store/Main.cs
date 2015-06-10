using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Store
{
    public partial class Main : Form
    {
        private static TableLayoutPanel itemTable; //Table containing all items currently in store

        private static TableLayoutPanel ordersToProcessTable; //Table containing all orders that need to be processed

        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddItemDialog(); //Open the Add Item Form
        }

        public void AddItemDialog()
        {
            AddItemForm otherForm = new AddItemForm();
            otherForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddOrderForm otherForm = new AddOrderForm();
            otherForm.Show();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            itemTable = tableLayoutPanel1;
        }

        public static void UpdateStorefront()
        {
            //Populate item table
            for (int i = 0; i < Storefront.Inventory.Count; i++) //Loop through every item in inventory
            {
                for (int x = 0; x < 5; x++) //Fill out information for each item
                {
                    Control c = itemTable.GetControlFromPosition(x, i+1);

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
                        } else 
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
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
            ordersToProcessTable = tableLayoutPanel2;
        }

        public static void UpdateOrders()
        {
            //Populate orders to process table
            for (int i = 0; i < OrderManager.GetOrdersToProcess().Count; i++) //Loop through every order
            {
                for (int x = 0; x < 5; x++) //Fill out information for each order
                {
                    Control c = ordersToProcessTable.GetControlFromPosition(x, i + 1);

                    if (x == 0) //Customer Name
                    {
                        c.Text = OrderManager.GetOrdersToProcess()[i].GetName();
                    }
                    else if (x == 1) //Item Name
                    {
                        c.Text = c.Text = OrderManager.GetOrdersToProcess()[i].GetItem().GetName();
                    }
                    else if (x == 2) //Order ID
                    {
                        c.Text = OrderManager.GetOrdersToProcess()[i].GetOrderID().ToString();
                    }
                    else if (x == 3) //Quantity
                    {
                        c.Text = OrderManager.GetOrdersToProcess()[i].GetQuantity().ToString();
                    }
                    else if (x == 4) //Total cost
                    {
                        float totalCost = OrderManager.GetOrdersToProcess()[i].GetItem().GetPrice() * OrderManager.GetOrdersToProcess()[i].GetQuantity();

                        c.Text = totalCost.ToString();
                    }
                }
            }
        }

    }
}
