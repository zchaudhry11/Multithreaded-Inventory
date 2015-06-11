using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace Store
{
    public partial class AddOrderForm : Form
    {
        public AddOrderForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            //Create order to be added in the order list
            Item purchasedItem = Storefront.SearchInventory(itemNameBox.Text);

            //If item in the order exists in the inventory
            if (purchasedItem != null)
            {
                int quantity = Convert.ToInt32(quantityBox.Text);
                float totalCost = purchasedItem.GetPrice() * quantity; //Total cost of this order

                Order temp = new Order(nameBox.Text, purchasedItem, Convert.ToInt32(fundsBox.Text), quantity, totalCost, OrderManager.OrderID);
                OrderManager.AddOrder(temp);
                Main.UpdateOrders();
            }

            this.Close();
        }
    }
}
