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
            this.nameBox = new System.Windows.Forms.TextBox();
            this.itemNameBox = new System.Windows.Forms.TextBox();
            this.quantityBox = new System.Windows.Forms.TextBox();
            this.fundsBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.submitButton = new System.Windows.Forms.Button();
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
            this.itemLabel.Location = new System.Drawing.Point(24, 50);
            this.itemLabel.Name = "itemLabel";
            this.itemLabel.Size = new System.Drawing.Size(61, 13);
            this.itemLabel.TabIndex = 1;
            this.itemLabel.Text = "Item Name:";
            // 
            // quantityLabel
            // 
            this.quantityLabel.AutoSize = true;
            this.quantityLabel.Location = new System.Drawing.Point(24, 77);
            this.quantityLabel.Name = "quantityLabel";
            this.quantityLabel.Size = new System.Drawing.Size(72, 13);
            this.quantityLabel.TabIndex = 2;
            this.quantityLabel.Text = "Item Quantity:";
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
            this.itemNameBox.Location = new System.Drawing.Point(116, 47);
            this.itemNameBox.Name = "itemNameBox";
            this.itemNameBox.Size = new System.Drawing.Size(170, 20);
            this.itemNameBox.TabIndex = 5;
            // 
            // quantityBox
            // 
            this.quantityBox.Location = new System.Drawing.Point(116, 74);
            this.quantityBox.Name = "quantityBox";
            this.quantityBox.Size = new System.Drawing.Size(100, 20);
            this.quantityBox.TabIndex = 6;
            // 
            // fundsBox
            // 
            this.fundsBox.Location = new System.Drawing.Point(116, 100);
            this.fundsBox.Name = "fundsBox";
            this.fundsBox.Size = new System.Drawing.Size(100, 20);
            this.fundsBox.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Account Funds:";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(116, 165);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(211, 165);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 11;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // AddOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 233);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.fundsBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.quantityBox);
            this.Controls.Add(this.itemNameBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.quantityLabel);
            this.Controls.Add(this.itemLabel);
            this.Controls.Add(this.customerLabel);
            this.Name = "AddOrderForm";
            this.Text = "Add Order";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label customerLabel;
        private System.Windows.Forms.Label itemLabel;
        private System.Windows.Forms.Label quantityLabel;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.TextBox itemNameBox;
        private System.Windows.Forms.TextBox quantityBox;
        private System.Windows.Forms.TextBox fundsBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button submitButton;
    }
}