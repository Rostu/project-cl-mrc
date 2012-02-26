namespace JADE
{
    partial class HauptFenster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HauptFenster));
            this.Tokenize = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.treeView_Sätze = new System.Windows.Forms.TreeView();
            this.flowLayoutPanel_Token = new System.Windows.Forms.FlowLayoutPanel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.öffnenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.detailSucheCheckBox = new System.Windows.Forms.CheckBox();
            this.dataGridView_Suchergebnisse = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Suchergebnisse)).BeginInit();
            this.SuspendLayout();
            // 
            // Tokenize
            // 
            this.Tokenize.Dock = System.Windows.Forms.DockStyle.Top;
            this.Tokenize.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Tokenize.Location = new System.Drawing.Point(137, 170);
            this.Tokenize.Margin = new System.Windows.Forms.Padding(4);
            this.Tokenize.MinimumSize = new System.Drawing.Size(100, 29);
            this.Tokenize.Name = "Tokenize";
            this.Tokenize.Size = new System.Drawing.Size(567, 29);
            this.Tokenize.TabIndex = 0;
            this.Tokenize.Text = "Tokenize";
            this.Tokenize.UseVisualStyleBackColor = true;
            this.Tokenize.Click += new System.EventHandler(this.Tokenize_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.DetectUrls = false;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextBox1.Font = new System.Drawing.Font("MS PMincho", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 24);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.MaxLength = 1000;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(704, 146);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "１３日、千葉県東金市のファミリーレストランの店内で男性が拳銃で撃たれて殺害された事件で、警察は、殺人などの疑いで全国に指名手配している元暴力団員の男の写真を公開し" +
                "、行方を捜査しています。";
            // 
            // treeView1
            // 
            this.treeView_Sätze.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView_Sätze.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView_Sätze.Location = new System.Drawing.Point(0, 170);
            this.treeView_Sätze.Margin = new System.Windows.Forms.Padding(4);
            this.treeView_Sätze.MinimumSize = new System.Drawing.Size(130, 4);
            this.treeView_Sätze.Name = "treeView1";
            this.treeView_Sätze.Size = new System.Drawing.Size(137, 352);
            this.treeView_Sätze.TabIndex = 7;
            this.treeView_Sätze.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.event_TreeViewItemSelect);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel_Token.AutoScroll = true;
            this.flowLayoutPanel_Token.BackColor = System.Drawing.SystemColors.ControlLight;
            this.flowLayoutPanel_Token.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel_Token.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel_Token.Font = new System.Drawing.Font("MS PMincho", 12F);
            this.flowLayoutPanel_Token.Location = new System.Drawing.Point(137, 265);
            this.flowLayoutPanel_Token.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel_Token.MinimumSize = new System.Drawing.Size(170, 2);
            this.flowLayoutPanel_Token.Name = "flowLayoutPanel1";
            this.flowLayoutPanel_Token.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.flowLayoutPanel_Token.Size = new System.Drawing.Size(567, 108);
            this.flowLayoutPanel_Token.TabIndex = 10;
            this.flowLayoutPanel_Token.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(704, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.öffnenToolStripMenuItem,
            this.beendenToolStripMenuItem});
            this.dateiToolStripMenuItem.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.dateiToolStripMenuItem.Text = "Datei";
            // 
            // öffnenToolStripMenuItem
            // 
            this.öffnenToolStripMenuItem.Name = "öffnenToolStripMenuItem";
            this.öffnenToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.öffnenToolStripMenuItem.Text = "Öffnen";
            this.öffnenToolStripMenuItem.Click += new System.EventHandler(this.öffnenToolStripMenu_Click);
            // 
            // beendenToolStripMenuItem
            // 
            this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            this.beendenToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.beendenToolStripMenuItem.Text = "Beenden";
            this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click_1);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.button2);
            this.flowLayoutPanel2.Controls.Add(this.button1);
            this.flowLayoutPanel2.Controls.Add(this.detailSucheCheckBox);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(137, 373);
            this.flowLayoutPanel2.MinimumSize = new System.Drawing.Size(100, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(567, 39);
            this.flowLayoutPanel2.TabIndex = 17;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(4, 4);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 31);
            this.button2.TabIndex = 14;
            this.button2.Text = "Token bearbeiten";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.TokenBearbeiten_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(150, 4);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 31);
            this.button1.TabIndex = 15;
            this.button1.Text = "Token suchen";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.TokenSuchen_Click);
            // 
            // checkBox1
            // 
            this.detailSucheCheckBox.AutoSize = true;
            this.detailSucheCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.detailSucheCheckBox.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailSucheCheckBox.Location = new System.Drawing.Point(289, 3);
            this.detailSucheCheckBox.Name = "checkBox1";
            this.detailSucheCheckBox.Size = new System.Drawing.Size(205, 33);
            this.detailSucheCheckBox.TabIndex = 17;
            this.detailSucheCheckBox.Text = "Genaue Übereinstimmung";
            this.detailSucheCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView_Suchergebnisse.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView_Suchergebnisse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Suchergebnisse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Suchergebnisse.Location = new System.Drawing.Point(137, 412);
            this.dataGridView_Suchergebnisse.Name = "dataGridView1";
            this.dataGridView_Suchergebnisse.Size = new System.Drawing.Size(567, 110);
            this.dataGridView_Suchergebnisse.TabIndex = 14;
            this.dataGridView_Suchergebnisse.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.event_dataGridView_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.Font = new System.Drawing.Font("MS PMincho", 12F);
            this.textBox1.Location = new System.Drawing.Point(137, 199);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(567, 66);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(704, 522);
            this.Controls.Add(this.dataGridView_Suchergebnisse);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel_Token);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Tokenize);
            this.Controls.Add(this.treeView_Sätze);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(720, 560);
            this.Name = "Form1";
            this.Text = "JaDe - Tokenizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Suchergebnisse)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Tokenize;
        private System.Windows.Forms.RichTextBox richTextBox1;
        public System.Windows.Forms.TreeView treeView_Sätze;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Token;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem öffnenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.DataGridView dataGridView_Suchergebnisse;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox detailSucheCheckBox;
        private System.Windows.Forms.RichTextBox textBox1;
    }
}

