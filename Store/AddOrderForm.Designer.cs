namespace Store
{
    partial class AddOrderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.customerLabel = new System.Windows.Forms.Label();
            this.itemLabel = new System.Windows.Forms.Label();
            this.quantityLabel = new System.Windows.Forms.Label();
            this.costLabel = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.itemNameBox = new System.Windows.Forms.TextBox();
            this.quantityBox = new System.Windows.Forms.TextBox();
            this.totalBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // customerLabel
            // 
            this.customerLabel.AutoSize = true;
            this.customerLabel.Location = new System.Drawing.Point(24, 22);
            this.customerLabel.Name = "customerLabel";
            this.customerLabel.Size = new System.Drawing.Size(85, 13);
            this.customerLabel.TabIndex = 0;
            this.customerLabel.Text = "Customer Name:";
            // 
            // itemLabel
            // 
            this.itemLabel.AutoSize = true;
            this.itemLabel.Location = new System.Drawing.Point(24, 52);
            this.itemLabel.Name = "itemLabel";
            this.itemLabel.Size = new System.Drawing.Size(61, 13);
            this.itemLabel.TabIndex = 1;
            this.itemLabel.Text = "Item Name:";
            // 
            // quantityLabel
            // 
            this.quantityLabel.AutoSize = true;
            this.quantityLabel.Location = new System.Drawing.Point(24, 80);
            this.quantityLabel.Name = "quantityLabel";
            this.quantityLabel.Size = new System.Drawing.Size(49, 13);
            this.quantityLabel.TabIndex = 2;
            this.quantityLabel.Text = "Quantity:";
            // 
            // costLabel
            // 
            this.costLabel.AutoSize = true;
            this.costLabel.Location = new System.Drawing.Point(24, 109);
            this.costLabel.Name = "costLabel";
            this.costLabel.Size = new System.Drawing.Size(58, 13);
            this.costLabel.TabIndex = 3;
            this.costLabel.Text = "Total Cost:";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(116, 22);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(170, 20);
            this.nameBox.TabIndex = 4;
            // 
            // itemNameBox
            // 
            this.itemNameBox.Location = new System.Drawing.Point(116, 52);
            this.itemNameBox.Name = "itemNameBox";
            this.itemNameBox.Size = new System.Drawing.Size(170, 20);
            this.itemNameBox.TabIndex = 5;
            // 
            // quantityBox
            // 
            this.quantityBox.Location = new System.Drawing.Point(116, 80);
            this.quantityBox.Name = "quantityBox";
            this.quantityBox.Size = new System.Drawing.Size(100, 20);
            this.quantityBox.TabIndex = 6;
            // 
            // totalBox
            // 
            this.totalBox.Location = new System.Drawing.Point(116, 109);
            this.totalBox.Name = "totalBox";
            this.totalBox.Size = new System.Drawing.Size(100, 20);
            this.totalBox.TabIndex = 7;
            // 
            // AddOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 233);
            this.Controls.Add(this.totalBox);
            this.Controls.Add(this.quantityBox);
            this.Controls.Add(this.itemNameBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.costLabel);
            this.Controls.Add(this.quantityLabel);
            this.Controls.Add(this.itemLabel);
            this.Controls.Add(this.customerLabel);
            this.Name = "AddOrderForm";
            this.Text = "AddOrderForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label customerLabel;
        private System.Windows.Forms.Label itemLabel;
        private System.Windows.Forms.Label quantityLabel;
        private System.Windows.Forms.Label costLabel;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.TextBox itemNameBox;
        private System.Windows.Forms.TextBox quantityBox;
        private System.Windows.Forms.TextBox totalBox;
    }
}