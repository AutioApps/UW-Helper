namespace UWHelper
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lstProcesses = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnBringToFront = new System.Windows.Forms.Button();
            this.txtpY = new System.Windows.Forms.TextBox();
            this.txtpX = new System.Windows.Forms.TextBox();
            this.txtsX = new System.Windows.Forms.TextBox();
            this.txtsY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkBorderless = new System.Windows.Forms.CheckBox();
            this.tmrSetTopMost = new System.Windows.Forms.Timer(this.components);
            this.tmrAutomatic = new System.Windows.Forms.Timer(this.components);
            this.chkAuto = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lstProcesses
            // 
            this.lstProcesses.FormattingEnabled = true;
            this.lstProcesses.Location = new System.Drawing.Point(12, 12);
            this.lstProcesses.Name = "lstProcesses";
            this.lstProcesses.Size = new System.Drawing.Size(417, 264);
            this.lstProcesses.TabIndex = 0;
            this.lstProcesses.SelectedIndexChanged += new System.EventHandler(this.lstProcesses_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(12, 282);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnBringToFront
            // 
            this.btnBringToFront.Location = new System.Drawing.Point(354, 366);
            this.btnBringToFront.Name = "btnBringToFront";
            this.btnBringToFront.Size = new System.Drawing.Size(75, 23);
            this.btnBringToFront.TabIndex = 7;
            this.btnBringToFront.Text = "Apply";
            this.btnBringToFront.UseVisualStyleBackColor = true;
            this.btnBringToFront.Click += new System.EventHandler(this.btnBringToFront_Click);
            // 
            // txtpY
            // 
            this.txtpY.Location = new System.Drawing.Point(354, 340);
            this.txtpY.Name = "txtpY";
            this.txtpY.Size = new System.Drawing.Size(75, 20);
            this.txtpY.TabIndex = 5;
            this.txtpY.Text = "0";
            this.txtpY.TextChanged += new System.EventHandler(this.txtpY_TextChanged);
            // 
            // txtpX
            // 
            this.txtpX.Location = new System.Drawing.Point(354, 314);
            this.txtpX.Name = "txtpX";
            this.txtpX.Size = new System.Drawing.Size(75, 20);
            this.txtpX.TabIndex = 3;
            this.txtpX.Text = "0";
            this.txtpX.TextChanged += new System.EventHandler(this.txtpX_TextChanged);
            // 
            // txtsX
            // 
            this.txtsX.Location = new System.Drawing.Point(232, 314);
            this.txtsX.Name = "txtsX";
            this.txtsX.Size = new System.Drawing.Size(75, 20);
            this.txtsX.TabIndex = 1;
            this.txtsX.Text = "1920";
            this.txtsX.TextChanged += new System.EventHandler(this.txtsX_TextChanged);
            // 
            // txtsY
            // 
            this.txtsY.Location = new System.Drawing.Point(232, 340);
            this.txtsY.Name = "txtsY";
            this.txtsY.Size = new System.Drawing.Size(75, 20);
            this.txtsY.TabIndex = 2;
            this.txtsY.Text = "1080";
            this.txtsY.TextChanged += new System.EventHandler(this.txtsY_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(313, 320);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "pos X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(313, 346);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "pos Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(191, 346);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "size Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(190, 320);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "size X";
            // 
            // chkBorderless
            // 
            this.chkBorderless.AutoSize = true;
            this.chkBorderless.Location = new System.Drawing.Point(232, 372);
            this.chkBorderless.Name = "chkBorderless";
            this.chkBorderless.Size = new System.Drawing.Size(75, 17);
            this.chkBorderless.TabIndex = 6;
            this.chkBorderless.Text = "Borderless";
            this.chkBorderless.UseVisualStyleBackColor = true;
            // 
            // tmrSetTopMost
            // 
            this.tmrSetTopMost.Tick += new System.EventHandler(this.tmrSetTopMost_Tick);
            // 
            // tmrAutomatic
            // 
            this.tmrAutomatic.Tick += new System.EventHandler(this.tmrAutomatic_Tick);
            // 
            // chkAuto
            // 
            this.chkAuto.AutoSize = true;
            this.chkAuto.Location = new System.Drawing.Point(12, 372);
            this.chkAuto.Name = "chkAuto";
            this.chkAuto.Size = new System.Drawing.Size(117, 17);
            this.chkAuto.TabIndex = 11;
            this.chkAuto.Text = "Apply Automatically";
            this.chkAuto.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 401);
            this.Controls.Add(this.chkAuto);
            this.Controls.Add(this.chkBorderless);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtsY);
            this.Controls.Add(this.txtsX);
            this.Controls.Add(this.txtpX);
            this.Controls.Add(this.txtpY);
            this.Controls.Add(this.btnBringToFront);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lstProcesses);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Autio Apps UW Helper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstProcesses;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnBringToFront;
        private System.Windows.Forms.TextBox txtpY;
        private System.Windows.Forms.TextBox txtpX;
        private System.Windows.Forms.TextBox txtsX;
        private System.Windows.Forms.TextBox txtsY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkBorderless;
        private System.Windows.Forms.Timer tmrSetTopMost;
        private System.Windows.Forms.Timer tmrAutomatic;
        private System.Windows.Forms.CheckBox chkAuto;
    }
}

