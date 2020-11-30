namespace SPS
{
    partial class SettingForm
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxEx1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.rbtIn = new System.Windows.Forms.RadioButton();
            this.rbtOut = new System.Windows.Forms.RadioButton();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(141, 37);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "Chọn cổng:";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.Location = new System.Drawing.Point(12, 69);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(141, 37);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "Bãi đỗ xe:";
            // 
            // comboBoxEx1
            // 
            this.comboBoxEx1.DisplayMember = "Text";
            this.comboBoxEx1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxEx1.FormattingEnabled = true;
            this.comboBoxEx1.ItemHeight = 33;
            this.comboBoxEx1.Location = new System.Drawing.Point(159, 67);
            this.comboBoxEx1.Name = "comboBoxEx1";
            this.comboBoxEx1.Size = new System.Drawing.Size(279, 39);
            this.comboBoxEx1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.comboBoxEx1.TabIndex = 2;
            // 
            // rbtIn
            // 
            this.rbtIn.AutoSize = true;
            this.rbtIn.Checked = true;
            this.rbtIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtIn.Location = new System.Drawing.Point(159, 16);
            this.rbtIn.Name = "rbtIn";
            this.rbtIn.Size = new System.Drawing.Size(133, 33);
            this.rbtIn.TabIndex = 3;
            this.rbtIn.TabStop = true;
            this.rbtIn.Text = "Cổng vào";
            this.rbtIn.UseVisualStyleBackColor = true;
            // 
            // rbtOut
            // 
            this.rbtOut.AutoSize = true;
            this.rbtOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtOut.Location = new System.Drawing.Point(322, 16);
            this.rbtOut.Name = "rbtOut";
            this.rbtOut.Size = new System.Drawing.Size(116, 33);
            this.rbtOut.TabIndex = 4;
            this.rbtOut.Text = "Cổng ra";
            this.rbtOut.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(159, 112);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 37);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(257, 112);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 37);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Hủy";
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 161);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.rbtOut);
            this.Controls.Add(this.rbtIn);
            this.Controls.Add(this.comboBoxEx1);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx1;
        private System.Windows.Forms.RadioButton rbtIn;
        private System.Windows.Forms.RadioButton rbtOut;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnCancel;
    }
}