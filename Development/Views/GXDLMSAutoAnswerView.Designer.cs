namespace GXDLMSDirector.Views
{
    partial class GXDLMSAutoAnswerView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSAutoAnswerView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RingCountOutOfWindowTB = new System.Windows.Forms.TextBox();
            this.RingCountInWindowTB = new System.Windows.Forms.TextBox();
            this.RingCountOutOfWindowLbl = new System.Windows.Forms.Label();
            this.RingCountInWindowLbl = new System.Windows.Forms.Label();
            this.ListeningWindowLV = new System.Windows.Forms.ListView();
            this.StartTimeHeader = new System.Windows.Forms.ColumnHeader();
            this.EndTimeHeader = new System.Windows.Forms.ColumnHeader();
            this.ListeningWindowLbl = new System.Windows.Forms.Label();
            this.NumberOfCallsTB = new GXDLMSDirector.Views.GXValueField();
            this.NumberOfCallsLbl = new System.Windows.Forms.Label();
            this.StatusTB = new GXDLMSDirector.Views.GXValueField();
            this.StatusLbl = new System.Windows.Forms.Label();
            this.ModeTB = new GXDLMSDirector.Views.GXValueField();
            this.ModeLbl = new System.Windows.Forms.Label();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.RingCountOutOfWindowTB);
            this.groupBox1.Controls.Add(this.RingCountInWindowTB);
            this.groupBox1.Controls.Add(this.RingCountOutOfWindowLbl);
            this.groupBox1.Controls.Add(this.RingCountInWindowLbl);
            this.groupBox1.Controls.Add(this.ListeningWindowLV);
            this.groupBox1.Controls.Add(this.ListeningWindowLbl);
            this.groupBox1.Controls.Add(this.NumberOfCallsTB);
            this.groupBox1.Controls.Add(this.NumberOfCallsLbl);
            this.groupBox1.Controls.Add(this.StatusTB);
            this.groupBox1.Controls.Add(this.StatusLbl);
            this.groupBox1.Controls.Add(this.ModeTB);
            this.groupBox1.Controls.Add(this.ModeLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 283);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto Answer Object";
            this.groupBox1.UseCompatibleTextRendering = true;
            // 
            // RingCountOutOfWindowTB
            // 
            this.RingCountOutOfWindowTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RingCountOutOfWindowTB.Location = new System.Drawing.Point(173, 249);
            this.RingCountOutOfWindowTB.Name = "RingCountOutOfWindowTB";
            this.RingCountOutOfWindowTB.ReadOnly = true;
            this.RingCountOutOfWindowTB.Size = new System.Drawing.Size(98, 20);
            this.RingCountOutOfWindowTB.TabIndex = 14;
            // 
            // RingCountInWindowTB
            // 
            this.RingCountInWindowTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RingCountInWindowTB.Location = new System.Drawing.Point(173, 223);
            this.RingCountInWindowTB.Name = "RingCountInWindowTB";
            this.RingCountInWindowTB.ReadOnly = true;
            this.RingCountInWindowTB.Size = new System.Drawing.Size(98, 20);
            this.RingCountInWindowTB.TabIndex = 13;
            // 
            // RingCountOutOfWindowLbl
            // 
            this.RingCountOutOfWindowLbl.AutoSize = true;
            this.RingCountOutOfWindowLbl.Location = new System.Drawing.Point(6, 255);
            this.RingCountOutOfWindowLbl.Name = "RingCountOutOfWindowLbl";
            this.RingCountOutOfWindowLbl.Size = new System.Drawing.Size(139, 13);
            this.RingCountOutOfWindowLbl.TabIndex = 12;
            this.RingCountOutOfWindowLbl.Text = "Ring Count Out Of Window:";
            // 
            // RingCountInWindowLbl
            // 
            this.RingCountInWindowLbl.AutoSize = true;
            this.RingCountInWindowLbl.Location = new System.Drawing.Point(6, 226);
            this.RingCountInWindowLbl.Name = "RingCountInWindowLbl";
            this.RingCountInWindowLbl.Size = new System.Drawing.Size(117, 13);
            this.RingCountInWindowLbl.TabIndex = 11;
            this.RingCountInWindowLbl.Text = "Ring Count In Window:";
            // 
            // ListeningWindowLV
            // 
            this.ListeningWindowLV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ListeningWindowLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.StartTimeHeader,
            this.EndTimeHeader});
            this.ListeningWindowLV.FullRowSelect = true;
            this.ListeningWindowLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListeningWindowLV.HideSelection = false;
            this.ListeningWindowLV.Location = new System.Drawing.Point(102, 73);
            this.ListeningWindowLV.Name = "ListeningWindowLV";
            this.ListeningWindowLV.Size = new System.Drawing.Size(169, 92);
            this.ListeningWindowLV.TabIndex = 10;
            this.ListeningWindowLV.UseCompatibleStateImageBehavior = false;
            this.ListeningWindowLV.View = System.Windows.Forms.View.Details;
            // 
            // StartTimeHeader
            // 
            this.StartTimeHeader.Text = "Start Time:";
            this.StartTimeHeader.Width = 79;
            // 
            // EndTimeHeader
            // 
            this.EndTimeHeader.Text = "End Time:";
            this.EndTimeHeader.Width = 81;
            // 
            // ListeningWindowLbl
            // 
            this.ListeningWindowLbl.AutoSize = true;
            this.ListeningWindowLbl.Location = new System.Drawing.Point(6, 73);
            this.ListeningWindowLbl.Name = "ListeningWindowLbl";
            this.ListeningWindowLbl.Size = new System.Drawing.Size(94, 13);
            this.ListeningWindowLbl.TabIndex = 9;
            this.ListeningWindowLbl.Text = "Listening Window:";
            // 
            // NumberOfCallsTB
            // 
            this.NumberOfCallsTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.NumberOfCallsTB.AttributeID = 0;
            this.NumberOfCallsTB.Location = new System.Drawing.Point(102, 195);
            this.NumberOfCallsTB.Name = "NumberOfCallsTB";
            this.NumberOfCallsTB.ReadOnly = true;
            this.NumberOfCallsTB.Size = new System.Drawing.Size(169, 20);
            this.NumberOfCallsTB.TabIndex = 7;
            this.NumberOfCallsTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // NumberOfCallsLbl
            // 
            this.NumberOfCallsLbl.AutoSize = true;
            this.NumberOfCallsLbl.Location = new System.Drawing.Point(6, 198);
            this.NumberOfCallsLbl.Name = "NumberOfCallsLbl";
            this.NumberOfCallsLbl.Size = new System.Drawing.Size(86, 13);
            this.NumberOfCallsLbl.TabIndex = 6;
            this.NumberOfCallsLbl.Text = "Number Of Calls:";
            // 
            // StatusTB
            // 
            this.StatusTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusTB.AttributeID = 4;
            this.StatusTB.Location = new System.Drawing.Point(102, 169);
            this.StatusTB.Name = "StatusTB";
            this.StatusTB.ReadOnly = true;
            this.StatusTB.Size = new System.Drawing.Size(169, 20);
            this.StatusTB.TabIndex = 5;
            this.StatusTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // StatusLbl
            // 
            this.StatusLbl.AutoSize = true;
            this.StatusLbl.Location = new System.Drawing.Point(6, 172);
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(40, 13);
            this.StatusLbl.TabIndex = 4;
            this.StatusLbl.Text = "Status:";
            // 
            // ModeTB
            // 
            this.ModeTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ModeTB.AttributeID = 2;
            this.ModeTB.Location = new System.Drawing.Point(102, 47);
            this.ModeTB.Name = "ModeTB";
            this.ModeTB.ReadOnly = true;
            this.ModeTB.Size = new System.Drawing.Size(169, 20);
            this.ModeTB.TabIndex = 3;
            this.ModeTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // ModeLbl
            // 
            this.ModeLbl.AutoSize = true;
            this.ModeLbl.Location = new System.Drawing.Point(6, 50);
            this.ModeLbl.Name = "ModeLbl";
            this.ModeLbl.Size = new System.Drawing.Size(37, 13);
            this.ModeLbl.TabIndex = 2;
            this.ModeLbl.Text = "Mode:";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(102, 21);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.ReadOnly = true;
            this.LogicalNameTB.Size = new System.Drawing.Size(169, 20);
            this.LogicalNameTB.TabIndex = 1;
            this.LogicalNameTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // LogicalNameLbl
            // 
            this.LogicalNameLbl.AutoSize = true;
            this.LogicalNameLbl.Location = new System.Drawing.Point(6, 24);
            this.LogicalNameLbl.Name = "LogicalNameLbl";
            this.LogicalNameLbl.Size = new System.Drawing.Size(75, 13);
            this.LogicalNameLbl.TabIndex = 0;
            this.LogicalNameLbl.Text = "Logical Name:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider1.Icon")));
            // 
            // GXDLMSAutoAnswerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 306);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GXDLMSAutoAnswerView";
            this.Text = "GXDLMSAutoAnswerView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private GXValueField NumberOfCallsTB;
        private System.Windows.Forms.Label NumberOfCallsLbl;
        private GXValueField StatusTB;
        private System.Windows.Forms.Label StatusLbl;
        private GXValueField ModeTB;
        private System.Windows.Forms.Label ModeLbl;
        private GXValueField LogicalNameTB;
        private System.Windows.Forms.Label LogicalNameLbl;
        private System.Windows.Forms.ListView ListeningWindowLV;
        private System.Windows.Forms.ColumnHeader StartTimeHeader;
        private System.Windows.Forms.ColumnHeader EndTimeHeader;
        private System.Windows.Forms.Label ListeningWindowLbl;
        private System.Windows.Forms.TextBox RingCountOutOfWindowTB;
        private System.Windows.Forms.TextBox RingCountInWindowTB;
        private System.Windows.Forms.Label RingCountOutOfWindowLbl;
        private System.Windows.Forms.Label RingCountInWindowLbl;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}