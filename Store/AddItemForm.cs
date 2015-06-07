using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Store
{
    public partial class AddItemForm : Form
    {

        public AddItemForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            //Create item to be added in list
            Item temp = new Item(nameBox.Text, descBox.Text, Convert.ToSingle(costBox.Text), stockCheckBox.Checked, Convert.ToInt32(quantityBox.Text));
            Storefront.AddItem(temp);
            this.Close();
        }
    }
}
