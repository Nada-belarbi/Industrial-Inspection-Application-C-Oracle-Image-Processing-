
namespace Ressuage
{
    partial class ConfirmForm
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
            this.dataGridViewConfirm = new System.Windows.Forms.DataGridView();
            this.numeroSerie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numerIndication = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nomPhoto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seq_before = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonNon = new System.Windows.Forms.Button();
            this.buttonOui = new System.Windows.Forms.Button();
            this.textBoxTitre = new System.Windows.Forms.TextBox();
            this.checkedListBoxCopieSn = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConfirm)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewConfirm
            // 
            this.dataGridViewConfirm.AllowUserToAddRows = false;
            this.dataGridViewConfirm.BackgroundColor = System.Drawing.Color.Silver;
            this.dataGridViewConfirm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewConfirm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numeroSerie,
            this.numerIndication,
            this.nomPhoto,
            this.sequence,
            this.seq_before});
            this.dataGridViewConfirm.Location = new System.Drawing.Point(14, 76);
            this.dataGridViewConfirm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewConfirm.Name = "dataGridViewConfirm";
            this.dataGridViewConfirm.RowHeadersWidth = 51;
            this.dataGridViewConfirm.RowTemplate.Height = 24;
            this.dataGridViewConfirm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewConfirm.Size = new System.Drawing.Size(599, 150);
            this.dataGridViewConfirm.TabIndex = 11;
            // 
            // numeroSerie
            // 
            this.numeroSerie.HeaderText = "Numero série";
            this.numeroSerie.MinimumWidth = 7;
            this.numeroSerie.Name = "numeroSerie";
            this.numeroSerie.ReadOnly = true;
            this.numeroSerie.Width = 125;
            // 
            // numerIndication
            // 
            this.numerIndication.HeaderText = "N° indication";
            this.numerIndication.MinimumWidth = 7;
            this.numerIndication.Name = "numerIndication";
            this.numerIndication.ReadOnly = true;
            this.numerIndication.Width = 120;
            // 
            // nomPhoto
            // 
            this.nomPhoto.HeaderText = "Nom Photo";
            this.nomPhoto.MinimumWidth = 7;
            this.nomPhoto.Name = "nomPhoto";
            this.nomPhoto.ReadOnly = true;
            this.nomPhoto.Width = 192;
            // 
            // sequence
            // 
            this.sequence.HeaderText = "Séq";
            this.sequence.MinimumWidth = 6;
            this.sequence.Name = "sequence";
            this.sequence.Width = 108;
            // 
            // seq_before
            // 
            this.seq_before.HeaderText = "seq_before";
            this.seq_before.MinimumWidth = 2;
            this.seq_before.Name = "seq_before";
            this.seq_before.Visible = false;
            this.seq_before.Width = 10;
            // 
            // buttonNon
            // 
            this.buttonNon.BackColor = System.Drawing.Color.DarkGray;
            this.buttonNon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonNon.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonNon.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNon.Location = new System.Drawing.Point(118, 230);
            this.buttonNon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonNon.Name = "buttonNon";
            this.buttonNon.Size = new System.Drawing.Size(120, 37);
            this.buttonNon.TabIndex = 12;
            this.buttonNon.Text = "Annuler";
            this.buttonNon.UseVisualStyleBackColor = false;
            this.buttonNon.Click += new System.EventHandler(this.buttonNon_Click);
            // 
            // buttonOui
            // 
            this.buttonOui.BackColor = System.Drawing.Color.DarkGray;
            this.buttonOui.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonOui.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOui.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOui.Location = new System.Drawing.Point(367, 230);
            this.buttonOui.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonOui.Name = "buttonOui";
            this.buttonOui.Size = new System.Drawing.Size(120, 37);
            this.buttonOui.TabIndex = 13;
            this.buttonOui.Text = "Valider";
            this.buttonOui.UseVisualStyleBackColor = false;
            this.buttonOui.Click += new System.EventHandler(this.buttonOui_Click);
            // 
            // textBoxTitre
            // 
            this.textBoxTitre.BackColor = System.Drawing.Color.DarkGray;
            this.textBoxTitre.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxTitre.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTitre.Location = new System.Drawing.Point(14, 10);
            this.textBoxTitre.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxTitre.Multiline = true;
            this.textBoxTitre.Name = "textBoxTitre";
            this.textBoxTitre.Size = new System.Drawing.Size(599, 63);
            this.textBoxTitre.TabIndex = 14;
            this.textBoxTitre.TextChanged += new System.EventHandler(this.textBoxTitre_TextChanged);
            // 
            // checkedListBoxCopieSn
            // 
            this.checkedListBoxCopieSn.BackColor = System.Drawing.Color.Silver;
            this.checkedListBoxCopieSn.CheckOnClick = true;
            this.checkedListBoxCopieSn.FormattingEnabled = true;
            this.checkedListBoxCopieSn.Location = new System.Drawing.Point(235, 76);
            this.checkedListBoxCopieSn.Name = "checkedListBoxCopieSn";
            this.checkedListBoxCopieSn.Size = new System.Drawing.Size(205, 124);
            this.checkedListBoxCopieSn.TabIndex = 15;
            this.checkedListBoxCopieSn.Visible = false;
            this.checkedListBoxCopieSn.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxCopieSn_ItemCheck);
            // 
            // ConfirmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(633, 275);
            this.Controls.Add(this.checkedListBoxCopieSn);
            this.Controls.Add(this.textBoxTitre);
            this.Controls.Add(this.buttonOui);
            this.Controls.Add(this.buttonNon);
            this.Controls.Add(this.dataGridViewConfirm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ConfirmForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UUM_Ressuage";
            this.Load += new System.EventHandler(this.ConfirmForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConfirm)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridViewConfirm;
        public System.Windows.Forms.Button buttonNon;
        public System.Windows.Forms.Button buttonOui;
        public System.Windows.Forms.TextBox textBoxTitre;
        private System.Windows.Forms.CheckedListBox checkedListBoxCopieSn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numeroSerie;
        private System.Windows.Forms.DataGridViewTextBoxColumn numerIndication;
        private System.Windows.Forms.DataGridViewTextBoxColumn nomPhoto;
        private System.Windows.Forms.DataGridViewTextBoxColumn sequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn seq_before;
    }
}