
namespace Ressuage
{
    partial class RessuageForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RessuageForm));
            this.checkedListBoxNumSerie = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonCameraLampe = new System.Windows.Forms.Button();
            this.textBoxCommentaire = new System.Windows.Forms.TextBox();
            this.numericUpDownNbInd = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownNumind = new System.Windows.Forms.NumericUpDown();
            this.dataGridViewRecap = new System.Windows.Forms.DataGridView();
            this.numeroSerie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numeroIndication = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nomPhoto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeInd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taille = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.classe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeDefaut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.positif_négatif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonValider = new System.Windows.Forms.Button();
            this.buttonCreerPV = new System.Windows.Forms.Button();
            this.buttonCameraDeportee = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxCdc = new System.Windows.Forms.TextBox();
            this.textBoxOp = new System.Windows.Forms.TextBox();
            this.textBoxOf = new System.Windows.Forms.TextBox();
            this.LabelHelperSup = new System.Windows.Forms.Label();
            this.checkedListBoxSnTaite = new System.Windows.Forms.CheckedListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxOperateur = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelTaile = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBoxPN = new System.Windows.Forms.ComboBox();
            this.comboBoxTypeDefaut = new System.Windows.Forms.ComboBox();
            this.textBoxTaille = new System.Windows.Forms.TextBox();
            this.labelUnit = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxTypeInd = new System.Windows.Forms.ComboBox();
            this.comboBoxEmplLocal = new System.Windows.Forms.ComboBox();
            this.comboBoxZoneLocal = new System.Windows.Forms.ComboBox();
            this.comboBoxProduitLocal = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonModifier = new System.Windows.Forms.Button();
            this.labelClasse = new System.Windows.Forms.Label();
            this.comboBoxClasse = new System.Windows.Forms.ComboBox();
            this.buttonRefreshDataGrid = new System.Windows.Forms.Button();
            this.linkLabelCopieSn = new System.Windows.Forms.LinkLabel();
            this.buttonTelechargerPhoto = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonHelpFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNbInd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRecap)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkedListBoxNumSerie
            // 
            this.checkedListBoxNumSerie.BackColor = System.Drawing.Color.Silver;
            this.checkedListBoxNumSerie.CheckOnClick = true;
            this.checkedListBoxNumSerie.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkedListBoxNumSerie.FormattingEnabled = true;
            this.checkedListBoxNumSerie.Location = new System.Drawing.Point(187, 121);
            this.checkedListBoxNumSerie.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkedListBoxNumSerie.Name = "checkedListBoxNumSerie";
            this.checkedListBoxNumSerie.Size = new System.Drawing.Size(239, 67);
            this.checkedListBoxNumSerie.TabIndex = 0;
            this.checkedListBoxNumSerie.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListBoxNumSerie_ItemCheck);
            this.checkedListBoxNumSerie.SelectedIndexChanged += new System.EventHandler(this.CheckedListBoxNumSerie_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(183, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Num série ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(170, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nb indications";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(445, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(261, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "Sélectionner N° indication";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 216);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "Commentaire";
            // 
            // buttonCameraLampe
            // 
            this.buttonCameraLampe.BackColor = System.Drawing.Color.DarkGray;
            this.buttonCameraLampe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCameraLampe.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCameraLampe.Location = new System.Drawing.Point(187, 526);
            this.buttonCameraLampe.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCameraLampe.Name = "buttonCameraLampe";
            this.buttonCameraLampe.Size = new System.Drawing.Size(256, 38);
            this.buttonCameraLampe.TabIndex = 6;
            this.buttonCameraLampe.Text = "Ajouter photo caméra lampe";
            this.buttonCameraLampe.UseVisualStyleBackColor = false;
            this.buttonCameraLampe.Click += new System.EventHandler(this.Button_CameraLampe);
            // 
            // textBoxCommentaire
            // 
            this.textBoxCommentaire.BackColor = System.Drawing.Color.Gainsboro;
            this.textBoxCommentaire.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCommentaire.Location = new System.Drawing.Point(173, 209);
            this.textBoxCommentaire.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxCommentaire.Multiline = true;
            this.textBoxCommentaire.Name = "textBoxCommentaire";
            this.textBoxCommentaire.Size = new System.Drawing.Size(518, 44);
            this.textBoxCommentaire.TabIndex = 7;
            this.textBoxCommentaire.TextChanged += new System.EventHandler(this.TextBoxCommentaire_TextChanged);
            // 
            // numericUpDownNbInd
            // 
            this.numericUpDownNbInd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownNbInd.Location = new System.Drawing.Point(325, 220);
            this.numericUpDownNbInd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDownNbInd.Name = "numericUpDownNbInd";
            this.numericUpDownNbInd.Size = new System.Drawing.Size(101, 30);
            this.numericUpDownNbInd.TabIndex = 8;
            this.numericUpDownNbInd.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownNbInd.ValueChanged += new System.EventHandler(this.NumericUpDownNbInd_ValueChanged);
            // 
            // numericUpDownNumind
            // 
            this.numericUpDownNumind.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownNumind.Location = new System.Drawing.Point(723, 222);
            this.numericUpDownNumind.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDownNumind.Name = "numericUpDownNumind";
            this.numericUpDownNumind.Size = new System.Drawing.Size(101, 30);
            this.numericUpDownNumind.TabIndex = 9;
            this.numericUpDownNumind.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownNumind.ValueChanged += new System.EventHandler(this.NumericUpDownNumind_ValueChanged);
            // 
            // dataGridViewRecap
            // 
            this.dataGridViewRecap.AllowUserToAddRows = false;
            this.dataGridViewRecap.BackgroundColor = System.Drawing.Color.Silver;
            this.dataGridViewRecap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRecap.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numeroSerie,
            this.numeroIndication,
            this.nomPhoto,
            this.sequence,
            this.typeInd,
            this.taille,
            this.classe,
            this.typeDefaut,
            this.positif_négatif});
            this.dataGridViewRecap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dataGridViewRecap.GridColor = System.Drawing.Color.Gray;
            this.dataGridViewRecap.Location = new System.Drawing.Point(32, 608);
            this.dataGridViewRecap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewRecap.Name = "dataGridViewRecap";
            this.dataGridViewRecap.RowHeadersWidth = 51;
            this.dataGridViewRecap.RowTemplate.Height = 24;
            this.dataGridViewRecap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewRecap.Size = new System.Drawing.Size(1005, 171);
            this.dataGridViewRecap.TabIndex = 10;
            this.dataGridViewRecap.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewRecap_CellDoubleClick);
            this.dataGridViewRecap.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellMouseLeave);
            this.dataGridViewRecap.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridView_CellMouseMove);
            // 
            // numeroSerie
            // 
            this.numeroSerie.HeaderText = "Num série";
            this.numeroSerie.MinimumWidth = 7;
            this.numeroSerie.Name = "numeroSerie";
            this.numeroSerie.ReadOnly = true;
            this.numeroSerie.Width = 95;
            // 
            // numeroIndication
            // 
            this.numeroIndication.HeaderText = "N° indication";
            this.numeroIndication.MinimumWidth = 7;
            this.numeroIndication.Name = "numeroIndication";
            this.numeroIndication.ReadOnly = true;
            this.numeroIndication.Width = 90;
            // 
            // nomPhoto
            // 
            this.nomPhoto.HeaderText = "Nom Photo";
            this.nomPhoto.MinimumWidth = 7;
            this.nomPhoto.Name = "nomPhoto";
            this.nomPhoto.ReadOnly = true;
            this.nomPhoto.Width = 160;
            // 
            // sequence
            // 
            this.sequence.HeaderText = "Séq";
            this.sequence.MinimumWidth = 6;
            this.sequence.Name = "sequence";
            this.sequence.ReadOnly = true;
            this.sequence.Width = 60;
            // 
            // typeInd
            // 
            this.typeInd.HeaderText = "Type ind";
            this.typeInd.MinimumWidth = 6;
            this.typeInd.Name = "typeInd";
            this.typeInd.ReadOnly = true;
            this.typeInd.Width = 140;
            // 
            // taille
            // 
            this.taille.HeaderText = "Taille(mm)";
            this.taille.MinimumWidth = 6;
            this.taille.Name = "taille";
            this.taille.ReadOnly = true;
            this.taille.Width = 75;
            // 
            // classe
            // 
            this.classe.HeaderText = "Classe";
            this.classe.MinimumWidth = 6;
            this.classe.Name = "classe";
            this.classe.ReadOnly = true;
            this.classe.Width = 60;
            // 
            // typeDefaut
            // 
            this.typeDefaut.HeaderText = "Type défaut";
            this.typeDefaut.MinimumWidth = 6;
            this.typeDefaut.Name = "typeDefaut";
            this.typeDefaut.ReadOnly = true;
            this.typeDefaut.Width = 160;
            // 
            // positif_négatif
            // 
            this.positif_négatif.HeaderText = "Positif / Négative";
            this.positif_négatif.MinimumWidth = 6;
            this.positif_négatif.Name = "positif_négatif";
            this.positif_négatif.ReadOnly = true;
            this.positif_négatif.Width = 110;
            // 
            // buttonValider
            // 
            this.buttonValider.BackColor = System.Drawing.Color.DarkGray;
            this.buttonValider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonValider.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonValider.Location = new System.Drawing.Point(476, 527);
            this.buttonValider.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonValider.Name = "buttonValider";
            this.buttonValider.Size = new System.Drawing.Size(120, 37);
            this.buttonValider.TabIndex = 11;
            this.buttonValider.Text = "Valider SN";
            this.buttonValider.UseVisualStyleBackColor = false;
            this.buttonValider.Click += new System.EventHandler(this.ButtonValider_Click);
            // 
            // buttonCreerPV
            // 
            this.buttonCreerPV.BackColor = System.Drawing.Color.Silver;
            this.buttonCreerPV.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCreerPV.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCreerPV.Location = new System.Drawing.Point(175, 783);
            this.buttonCreerPV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCreerPV.Name = "buttonCreerPV";
            this.buttonCreerPV.Size = new System.Drawing.Size(226, 37);
            this.buttonCreerPV.TabIndex = 12;
            this.buttonCreerPV.Text = "Créer PDF";
            this.buttonCreerPV.UseVisualStyleBackColor = false;
            this.buttonCreerPV.Click += new System.EventHandler(this.ButtonCreerPV_Click);
            // 
            // buttonCameraDeportee
            // 
            this.buttonCameraDeportee.BackColor = System.Drawing.Color.DarkGray;
            this.buttonCameraDeportee.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCameraDeportee.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCameraDeportee.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCameraDeportee.Location = new System.Drawing.Point(620, 526);
            this.buttonCameraDeportee.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCameraDeportee.Name = "buttonCameraDeportee";
            this.buttonCameraDeportee.Size = new System.Drawing.Size(256, 38);
            this.buttonCameraDeportee.TabIndex = 13;
            this.buttonCameraDeportee.Text = "Ajouter photo caméra déportée";
            this.buttonCameraDeportee.UseVisualStyleBackColor = false;
            this.buttonCameraDeportee.Click += new System.EventHandler(this.Button_CameraDeportee);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(411, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 25);
            this.label5.TabIndex = 14;
            this.label5.Text = "OP";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(247, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 25);
            this.label6.TabIndex = 15;
            this.label6.Text = "OF";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(576, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 25);
            this.label7.TabIndex = 16;
            this.label7.Text = "CDC";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(367, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(423, 32);
            this.label8.TabIndex = 22;
            this.label8.Text = "RESSUAGE CARTOGRAPHIE";
            // 
            // textBoxCdc
            // 
            this.textBoxCdc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCdc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCdc.Location = new System.Drawing.Point(529, 66);
            this.textBoxCdc.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCdc.Name = "textBoxCdc";
            this.textBoxCdc.ReadOnly = true;
            this.textBoxCdc.Size = new System.Drawing.Size(153, 20);
            this.textBoxCdc.TabIndex = 23;
            this.textBoxCdc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxOp
            // 
            this.textBoxOp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOp.Location = new System.Drawing.Point(360, 66);
            this.textBoxOp.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxOp.Name = "textBoxOp";
            this.textBoxOp.ReadOnly = true;
            this.textBoxOp.Size = new System.Drawing.Size(150, 20);
            this.textBoxOp.TabIndex = 24;
            this.textBoxOp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxOf
            // 
            this.textBoxOf.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOf.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOf.Location = new System.Drawing.Point(190, 66);
            this.textBoxOf.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxOf.Name = "textBoxOf";
            this.textBoxOf.ReadOnly = true;
            this.textBoxOf.Size = new System.Drawing.Size(145, 20);
            this.textBoxOf.TabIndex = 25;
            this.textBoxOf.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabelHelperSup
            // 
            this.LabelHelperSup.AutoSize = true;
            this.LabelHelperSup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelHelperSup.Location = new System.Drawing.Point(29, 580);
            this.LabelHelperSup.Name = "LabelHelperSup";
            this.LabelHelperSup.Size = new System.Drawing.Size(926, 17);
            this.LabelHelperSup.TabIndex = 26;
            this.LabelHelperSup.Text = "Double-clic sur un SN pour supprimer un enregistrement ou sur une seq pour modifi" +
    "er l\'odre des photos ou double-clic sur une photo pour l\'ouvrir";
            this.LabelHelperSup.Visible = false;
            // 
            // checkedListBoxSnTaite
            // 
            this.checkedListBoxSnTaite.BackColor = System.Drawing.Color.WhiteSmoke;
            this.checkedListBoxSnTaite.CheckOnClick = true;
            this.checkedListBoxSnTaite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkedListBoxSnTaite.FormattingEnabled = true;
            this.checkedListBoxSnTaite.Location = new System.Drawing.Point(662, 121);
            this.checkedListBoxSnTaite.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkedListBoxSnTaite.Name = "checkedListBoxSnTaite";
            this.checkedListBoxSnTaite.Size = new System.Drawing.Size(252, 67);
            this.checkedListBoxSnTaite.TabIndex = 27;
            this.checkedListBoxSnTaite.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListBoxSnTaite_ItemCheck);
            this.checkedListBoxSnTaite.SelectedIndexChanged += new System.EventHandler(this.CheckedListBoxSnTaite_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(667, 99);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(163, 25);
            this.label9.TabIndex = 28;
            this.label9.Text = "Num série traité";
            // 
            // textBoxOperateur
            // 
            this.textBoxOperateur.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOperateur.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOperateur.Location = new System.Drawing.Point(704, 66);
            this.textBoxOperateur.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxOperateur.Name = "textBoxOperateur";
            this.textBoxOperateur.ReadOnly = true;
            this.textBoxOperateur.Size = new System.Drawing.Size(208, 20);
            this.textBoxOperateur.TabIndex = 30;
            this.textBoxOperateur.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(746, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 25);
            this.label10.TabIndex = 29;
            this.label10.Text = "Opérateur";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(31, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(176, 25);
            this.label12.TabIndex = 32;
            this.label12.Text = "Type d\'indication";
            // 
            // labelTaile
            // 
            this.labelTaile.AutoSize = true;
            this.labelTaile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTaile.Location = new System.Drawing.Point(213, 21);
            this.labelTaile.Name = "labelTaile";
            this.labelTaile.Size = new System.Drawing.Size(65, 25);
            this.labelTaile.TabIndex = 33;
            this.labelTaile.Text = "Taille";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(401, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(296, 25);
            this.label14.TabIndex = 34;
            this.label14.Text = "Caractérisation de l\'indication";
            // 
            // comboBoxPN
            // 
            this.comboBoxPN.FormattingEnabled = true;
            this.comboBoxPN.Location = new System.Drawing.Point(412, 48);
            this.comboBoxPN.Name = "comboBoxPN";
            this.comboBoxPN.Size = new System.Drawing.Size(92, 28);
            this.comboBoxPN.Sorted = true;
            this.comboBoxPN.TabIndex = 36;
            this.comboBoxPN.SelectedIndexChanged += new System.EventHandler(this.ComboBoxPN_SelectedIndexChanged);
            // 
            // comboBoxTypeDefaut
            // 
            this.comboBoxTypeDefaut.DropDownWidth = 173;
            this.comboBoxTypeDefaut.FormattingEnabled = true;
            this.comboBoxTypeDefaut.Location = new System.Drawing.Point(524, 49);
            this.comboBoxTypeDefaut.Name = "comboBoxTypeDefaut";
            this.comboBoxTypeDefaut.Size = new System.Drawing.Size(173, 28);
            this.comboBoxTypeDefaut.Sorted = true;
            this.comboBoxTypeDefaut.TabIndex = 37;
            // 
            // textBoxTaille
            // 
            this.textBoxTaille.BackColor = System.Drawing.SystemColors.ControlLight;
            this.textBoxTaille.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTaille.Location = new System.Drawing.Point(209, 49);
            this.textBoxTaille.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTaille.Name = "textBoxTaille";
            this.textBoxTaille.Size = new System.Drawing.Size(78, 27);
            this.textBoxTaille.TabIndex = 38;
            this.textBoxTaille.TextChanged += new System.EventHandler(this.TextBoxTaille_TextChanged);
            this.textBoxTaille.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxTaille_KeyPress);
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Location = new System.Drawing.Point(288, 56);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(37, 20);
            this.labelUnit.TabIndex = 39;
            this.labelUnit.Text = "mm";
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.groupBox1.BackColor = System.Drawing.Color.Silver;
            this.groupBox1.Controls.Add(this.comboBoxTypeInd);
            this.groupBox1.Controls.Add(this.comboBoxEmplLocal);
            this.groupBox1.Controls.Add(this.comboBoxZoneLocal);
            this.groupBox1.Controls.Add(this.comboBoxProduitLocal);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.buttonModifier);
            this.groupBox1.Controls.Add(this.labelClasse);
            this.groupBox1.Controls.Add(this.comboBoxClasse);
            this.groupBox1.Controls.Add(this.labelUnit);
            this.groupBox1.Controls.Add(this.textBoxTaille);
            this.groupBox1.Controls.Add(this.comboBoxTypeDefaut);
            this.groupBox1.Controls.Add(this.comboBoxPN);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.labelTaile);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBoxCommentaire);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(28, 254);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(1004, 265);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            // 
            // comboBoxTypeInd
            // 
            this.comboBoxTypeInd.FormattingEnabled = true;
            this.comboBoxTypeInd.Location = new System.Drawing.Point(36, 49);
            this.comboBoxTypeInd.Name = "comboBoxTypeInd";
            this.comboBoxTypeInd.Size = new System.Drawing.Size(135, 28);
            this.comboBoxTypeInd.Sorted = true;
            this.comboBoxTypeInd.TabIndex = 49;
            // 
            // comboBoxEmplLocal
            // 
            this.comboBoxEmplLocal.FormattingEnabled = true;
            this.comboBoxEmplLocal.Location = new System.Drawing.Point(473, 160);
            this.comboBoxEmplLocal.Name = "comboBoxEmplLocal";
            this.comboBoxEmplLocal.Size = new System.Drawing.Size(335, 28);
            this.comboBoxEmplLocal.Sorted = true;
            this.comboBoxEmplLocal.TabIndex = 48;
            // 
            // comboBoxZoneLocal
            // 
            this.comboBoxZoneLocal.FormattingEnabled = true;
            this.comboBoxZoneLocal.Location = new System.Drawing.Point(297, 160);
            this.comboBoxZoneLocal.Name = "comboBoxZoneLocal";
            this.comboBoxZoneLocal.Size = new System.Drawing.Size(135, 28);
            this.comboBoxZoneLocal.Sorted = true;
            this.comboBoxZoneLocal.TabIndex = 47;
            this.comboBoxZoneLocal.SelectedIndexChanged += new System.EventHandler(this.comboBoxZoneLocal_SelectedIndexChanged);
            // 
            // comboBoxProduitLocal
            // 
            this.comboBoxProduitLocal.FormattingEnabled = true;
            this.comboBoxProduitLocal.Location = new System.Drawing.Point(129, 160);
            this.comboBoxProduitLocal.Name = "comboBoxProduitLocal";
            this.comboBoxProduitLocal.Size = new System.Drawing.Size(135, 28);
            this.comboBoxProduitLocal.Sorted = true;
            this.comboBoxProduitLocal.TabIndex = 46;
            this.comboBoxProduitLocal.SelectedIndexChanged += new System.EventHandler(this.comboBoxProduitLocal_SelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(499, 126);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(142, 25);
            this.label16.TabIndex = 45;
            this.label16.Text = "Emplacement";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(325, 127);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(61, 25);
            this.label15.TabIndex = 44;
            this.label15.Text = "Zone";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(162, 126);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 25);
            this.label13.TabIndex = 43;
            this.label13.Text = "Produit";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(287, 95);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(265, 25);
            this.label11.TabIndex = 42;
            this.label11.Text = "Localisation de l\'indication";
            // 
            // buttonModifier
            // 
            this.buttonModifier.BackColor = System.Drawing.Color.Silver;
            this.buttonModifier.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonModifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonModifier.Location = new System.Drawing.Point(702, 212);
            this.buttonModifier.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonModifier.Name = "buttonModifier";
            this.buttonModifier.Size = new System.Drawing.Size(120, 37);
            this.buttonModifier.TabIndex = 41;
            this.buttonModifier.Text = "Enregistrer";
            this.buttonModifier.UseVisualStyleBackColor = false;
            this.buttonModifier.Click += new System.EventHandler(this.ButtonModifier_Click);
            // 
            // labelClasse
            // 
            this.labelClasse.AutoSize = true;
            this.labelClasse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClasse.Location = new System.Drawing.Point(217, 22);
            this.labelClasse.Name = "labelClasse";
            this.labelClasse.Size = new System.Drawing.Size(79, 25);
            this.labelClasse.TabIndex = 41;
            this.labelClasse.Text = "Classe";
            this.labelClasse.Visible = false;
            // 
            // comboBoxClasse
            // 
            this.comboBoxClasse.BackColor = System.Drawing.SystemColors.ControlLight;
            this.comboBoxClasse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxClasse.FormattingEnabled = true;
            this.comboBoxClasse.Location = new System.Drawing.Point(210, 51);
            this.comboBoxClasse.Name = "comboBoxClasse";
            this.comboBoxClasse.Size = new System.Drawing.Size(76, 28);
            this.comboBoxClasse.Sorted = true;
            this.comboBoxClasse.TabIndex = 40;
            this.comboBoxClasse.Visible = false;
            // 
            // buttonRefreshDataGrid
            // 
            this.buttonRefreshDataGrid.BackColor = System.Drawing.Color.White;
            this.buttonRefreshDataGrid.Location = new System.Drawing.Point(1046, 673);
            this.buttonRefreshDataGrid.Name = "buttonRefreshDataGrid";
            this.buttonRefreshDataGrid.Size = new System.Drawing.Size(25, 37);
            this.buttonRefreshDataGrid.TabIndex = 41;
            this.buttonRefreshDataGrid.Text = "R";
            this.buttonRefreshDataGrid.UseVisualStyleBackColor = false;
            this.buttonRefreshDataGrid.Click += new System.EventHandler(this.ButtonRefreshDataGrid_Click);
            // 
            // linkLabelCopieSn
            // 
            this.linkLabelCopieSn.ActiveLinkColor = System.Drawing.Color.DeepSkyBlue;
            this.linkLabelCopieSn.AutoSize = true;
            this.linkLabelCopieSn.BackColor = System.Drawing.Color.Silver;
            this.linkLabelCopieSn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelCopieSn.ForeColor = System.Drawing.Color.Black;
            this.linkLabelCopieSn.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelCopieSn.LinkColor = System.Drawing.Color.Black;
            this.linkLabelCopieSn.Location = new System.Drawing.Point(508, 183);
            this.linkLabelCopieSn.Name = "linkLabelCopieSn";
            this.linkLabelCopieSn.Size = new System.Drawing.Size(97, 20);
            this.linkLabelCopieSn.TabIndex = 42;
            this.linkLabelCopieSn.TabStop = true;
            this.linkLabelCopieSn.Text = "Copier sur";
            this.linkLabelCopieSn.VisitedLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabelCopieSn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelCopieSn_LinkClicked);
            // 
            // buttonTelechargerPhoto
            // 
            this.buttonTelechargerPhoto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTelechargerPhoto.Location = new System.Drawing.Point(6, 121);
            this.buttonTelechargerPhoto.Name = "buttonTelechargerPhoto";
            this.buttonTelechargerPhoto.Size = new System.Drawing.Size(154, 30);
            this.buttonTelechargerPhoto.TabIndex = 43;
            this.buttonTelechargerPhoto.Text = "Télécharger photos";
            this.buttonTelechargerPhoto.UseVisualStyleBackColor = true;
            this.buttonTelechargerPhoto.Click += new System.EventHandler(this.ButtonTelechargerPhoto_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // buttonHelpFile
            // 
            this.buttonHelpFile.BackColor = System.Drawing.Color.DarkGray;
            this.buttonHelpFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonHelpFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHelpFile.Image = ((System.Drawing.Image)(resources.GetObject("buttonHelpFile.Image")));
            this.buttonHelpFile.Location = new System.Drawing.Point(1023, 5);
            this.buttonHelpFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonHelpFile.Name = "buttonHelpFile";
            this.buttonHelpFile.Size = new System.Drawing.Size(59, 61);
            this.buttonHelpFile.TabIndex = 45;
            this.buttonHelpFile.UseVisualStyleBackColor = false;
            this.buttonHelpFile.Click += new System.EventHandler(this.buttonHelpFile_Click);
            // 
            // RessuageForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1132, 826);
            this.Controls.Add(this.buttonHelpFile);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonTelechargerPhoto);
            this.Controls.Add(this.linkLabelCopieSn);
            this.Controls.Add(this.buttonRefreshDataGrid);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxOperateur);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.checkedListBoxSnTaite);
            this.Controls.Add(this.LabelHelperSup);
            this.Controls.Add(this.textBoxOf);
            this.Controls.Add(this.textBoxOp);
            this.Controls.Add(this.textBoxCdc);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonCameraDeportee);
            this.Controls.Add(this.buttonCreerPV);
            this.Controls.Add(this.buttonValider);
            this.Controls.Add(this.dataGridViewRecap);
            this.Controls.Add(this.numericUpDownNumind);
            this.Controls.Add(this.numericUpDownNbInd);
            this.Controls.Add(this.buttonCameraLampe);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBoxNumSerie);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "RessuageForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UUM_Ressauge";
            this.Load += new System.EventHandler(this.RessuageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNbInd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRecap)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxNumSerie;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonCameraLampe;
        private System.Windows.Forms.TextBox textBoxCommentaire;
        private System.Windows.Forms.NumericUpDown numericUpDownNbInd;
        private System.Windows.Forms.NumericUpDown numericUpDownNumind;
        private System.Windows.Forms.DataGridView dataGridViewRecap;
        private System.Windows.Forms.Button buttonValider;
        private System.Windows.Forms.Button buttonCreerPV;
        private System.Windows.Forms.Button buttonCameraDeportee;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxCdc;
        private System.Windows.Forms.TextBox textBoxOp;
        private System.Windows.Forms.TextBox textBoxOf;
        private System.Windows.Forms.Label LabelHelperSup;
        private System.Windows.Forms.CheckedListBox checkedListBoxSnTaite;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxOperateur;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelTaile;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBoxPN;
        private System.Windows.Forms.TextBox textBoxTaille;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxTypeDefaut;
        private System.Windows.Forms.Label labelClasse;
        private System.Windows.Forms.ComboBox comboBoxClasse;
        private System.Windows.Forms.Button buttonModifier;
        private System.Windows.Forms.DataGridViewTextBoxColumn numeroSerie;
        private System.Windows.Forms.DataGridViewTextBoxColumn numeroIndication;
        private System.Windows.Forms.DataGridViewTextBoxColumn nomPhoto;
        private System.Windows.Forms.DataGridViewTextBoxColumn sequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeInd;
        private System.Windows.Forms.DataGridViewTextBoxColumn taille;
        private System.Windows.Forms.DataGridViewTextBoxColumn classe;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeDefaut;
        private System.Windows.Forms.DataGridViewTextBoxColumn positif_négatif;
        private System.Windows.Forms.Button buttonRefreshDataGrid;
        private System.Windows.Forms.LinkLabel linkLabelCopieSn;
        private System.Windows.Forms.Button buttonTelechargerPhoto;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBoxProduitLocal;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBoxEmplLocal;
        private System.Windows.Forms.ComboBox comboBoxZoneLocal;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonHelpFile;
        private System.Windows.Forms.ComboBox comboBoxTypeInd;
    }
}

