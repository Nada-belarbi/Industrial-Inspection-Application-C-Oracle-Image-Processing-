using MediaDevices;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ressuage
{
    public partial class RessuageForm : Form
    {
        //instantiete class
        readonly Camera photo = new Camera();
        readonly MES mes = new MES();
        readonly LOGS log = new LOGS();
        readonly DataBase db = new DataBase(); //call database class and instantiete
        //readonly MailLog mlog = new MailLog();
        readonly SnValided vld = new SnValided();
        decimal oldNb_indValue;
        int SEQUENCE;

        //initiate variables
        readonly string photominilampe = @ConfigurationManager.AppSettings["photo_deportee_directory"]; //repertoire des photos niveau la caméra(caméra déportée)
        readonly string photolampe = @ConfigurationManager.AppSettings["photo_lampe_directory"]; //idem (caméra 13MP)
        readonly string path_dwl_photo_minilampe = @ConfigurationManager.AppSettings["telecharger_photo_minilampe_directory"]; //caméra déportée
        readonly string path_dwl_photo_lampe = @ConfigurationManager.AppSettings["telecharger_photo_lampe_directory"]; //caméra 13MP
        readonly string arch_photosdirectory = @ConfigurationManager.AppSettings["archivage_photo"]; //idem
        readonly string arch_pdfdirectory = @ConfigurationManager.AppSettings["archivage_pdf"]; //idem
        readonly string NonCamera = @ConfigurationManager.AppSettings["device"]; //idem
        
        //GECHG1598219 : Get the help file 
        public readonly string HelpFile = @ConfigurationManager.AppSettings["Help_File"];
        
        bool done;
        public List<string> snTraite;

        public RessuageForm()
        {
            //initializeComponent
            log.writeLog("Initialisation des composants du formulaire", "historique",0);
            InitializeComponent();
            
      

            //intialize op,of,cdc,operator
            log.writeLog("Initialisation des valeurs op, of, cdc et opérateur", "historique",0);
            textBoxOf.Text = mes.GetOf();
            textBoxOp.Text = mes.GetOp();
            textBoxCdc.Text = mes.GetCdc();
            textBoxOperateur.Text = mes.GetOperateur();
       
          
            try
            {
                log.writeLog("initialisation des numéros de series (SmartShop)", "historique", 0);

                var snList = mes.GetSnList();
                if (snList == null || snList.Length == 0)
                {
                    log.writeLog("Aucun SN reçu via l'argument SN", "log", 1);
                    MessageBox.Show("Aucun numéro de série reçu. Relancez depuis SmartShop.",
                        "SN manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    foreach (var sn in snList)
                    {
                        checkedListBoxNumSerie.Items.Add(sn);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog("Erreur initialisation SN SmartShop: " + ex.Message, "log", 1);
            }
           
            //intitialize component comboBox()  =>menu déroulant
            comboBoxPN.DropDownStyle = ComboBoxStyle.DropDownList; //value not modifiable 
            comboBoxTypeDefaut.DropDownStyle = ComboBoxStyle.DropDownList; //idem
            comboBoxTypeInd.DropDownStyle = ComboBoxStyle.DropDownList; //idem
            //GECHG1598219
            comboBoxProduitLocal.DropDownStyle = ComboBoxStyle.DropDownList; //idem
            comboBoxZoneLocal.DropDownStyle = ComboBoxStyle.DropDownList; //idem
            comboBoxEmplLocal.DropDownStyle = ComboBoxStyle.DropDownList; //idem

            //initialize type indication
            try
            {
                log.writeLog("initialisation type indication",  "historique",0);
                OracleDataReader typeind = mes.GetTypeIndication("select type_indication from TBSCT.epe_ressuage_typeindication");
                while(typeind.Read())
                {
                    comboBoxTypeInd.Items.Add(typeind["type_indication"]); //menu déroulant  type indication value
                }
                if (typeind.IsClosed == false)
                {
                    typeind.Close();
                }
            }
            catch (NullReferenceException ex)
            {
                log.writeLog("Erreur connection à la base de données " + ex.Message + ex.Source, "log",1);
            };

            //Initialize caracterisation indication positif/negatif/autre
            try
            {
                log.writeLog("Initialisation caracterisation indication positif/negatif/autre", "historique",0);
                OracleDataReader pn = mes.GetCaracterisation_indPN("select valeur from TBSCT.epe_ressuage_indicationPN");
                while (pn.Read())
                {
                    comboBoxPN.Items.Add(pn["valeur"]); //menu déroulant  type indication value
                }
                if (pn.IsClosed == false)
                {
                    pn.Close();
                }
            }
            catch (Exception exx)
            {
                log.writeLog("Erreur connection à la base de données " + exx.Message + exx.Source, "log",1);
            }

            //GECHG1598219 : initialize description local
            try
            {
                log.writeLog("initialisation de la liste Produits local", "historique", 0);
                OracleDataReader ProdLocal = mes.GetProduitLocal("select TBSCT.epe_ressuage_produitlocal.des_produit from TBSCT.epe_ressuage_produitlocal");
                while (ProdLocal.Read())
                {
                    comboBoxProduitLocal.Items.Add(ProdLocal["des_produit"]); //menu déroulant description des produits
                }
                if (ProdLocal.IsClosed == false)
                {
                    ProdLocal.Close();
                }
            }
            catch (NullReferenceException ex)
            {
                log.writeLog("Erreur connection à la base de données " + ex.Message + ex.Source, "log", 1);
            };

            log.writeLog("Initialisation type de defaut et la classe du type d'indication", "historique",0);
            comboBoxTypeDefaut.Items.Add("");
            comboBoxClasse.Items.Add("");

          

            //add the value to datagrid for each sn in the begening
            //initialize the board
            foreach (var item in checkedListBoxNumSerie.Items)
            {
                string sn = item?.ToString();
                if (string.IsNullOrWhiteSpace(sn)) continue;

                string queryData =
                    "select epe_ressuage_piece.sn,epe_ressuage_indice.indication, epe_ressuage_photos.lien_photo,epe_ressuage_photos.seq,epe_ressuage_indice.type_indication,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.type_defaut,epe_ressuage_indice.positif_negatif_autre" +
                    " from TBSCT.epe_ressuage_piece" +
                    " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                    " inner join  TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                    $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_piece.flag_pvcree={0} order by epe_ressuage_photos.seq desc";

                OracleDataReader data = db.Request(queryData);
                log.writeLog(queryData, "trace", 0);

                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        if (Convert.IsDBNull(data["lien_photo"]))
                        {
                            dataGridViewRecap.Rows.Insert(0, data["sn"], data["indication"], "", "",
                                data["type_indication"], data["taille_indice"], data["classe_indice"], data["type_defaut"], data["positif_negatif_autre"]);
                        }
                        else
                        {
                            FileInfo photo_name = new FileInfo((string)data["lien_photo"]);
                            dataGridViewRecap.Rows.Insert(0, data["sn"], data["indication"], photo_name.Name, data["seq"],
                                data["type_indication"], data["taille_indice"], data["classe_indice"], data["type_defaut"], data["positif_negatif_autre"]);
                        }
                    }
                }

                if (data != null && !data.IsClosed) data.Close();
            }

            //when application reload if sn flag is 1
            string snValide = "select sn from TBSCT.epe_ressuage_piece " +
                $"where epe_ressuage_piece.flag_valided={1} and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_piece.flag_PVcree={0}";
            OracleDataReader datasnV = db.Request(snValide);
            if (datasnV.HasRows)
            {
                while (datasnV.Read())
                {
                    checkedListBoxNumSerie.Items.Remove(datasnV["SN"]);
                    checkedListBoxSnTaite.Items.Add(datasnV["SN"]);
                }
                if (datasnV.IsClosed == false) datasnV.Close();
            }
        }

        //Resize the width of dropdown (menu déroulant) when the text is too long
        //methode take the in parameter the compenent name
        private void ResizeComboBox(ComboBox cbb)
        {
            Graphics g = cbb.CreateGraphics();
            float largestSize = 0;
            for (int i = 0; i < cbb.Items.Count; i++)
            {
                SizeF textSize = g.MeasureString(cbb.Items[i].ToString(), cbb.Font);
                if (textSize.Width > largestSize) largestSize = textSize.Width+4;
            }
            if (largestSize > 0) cbb.DropDownWidth = (int)largestSize;
        }

        //Allow only one selection for the SN not traited
        private void CheckedListBoxNumSerie_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && checkedListBoxNumSerie.CheckedItems.Count > 0)
            {
                checkedListBoxNumSerie.ItemCheck -= CheckedListBoxNumSerie_ItemCheck;
                checkedListBoxNumSerie.SetItemChecked(checkedListBoxNumSerie.CheckedIndices[0], false);
                checkedListBoxNumSerie.ItemCheck += CheckedListBoxNumSerie_ItemCheck;
            }
            else
            {
                //clear fields when item uncheck
                numericUpDownNumind.Refresh();
                numericUpDownNbInd.Refresh();
                textBoxCommentaire.Text = "";
                textBoxTaille.Text = "";
                comboBoxTypeInd.SelectedIndex = -1;
                comboBoxPN.SelectedIndex = -1;
                comboBoxClasse.ResetText();
                comboBoxTypeDefaut.Items.Clear();
                comboBoxTypeDefaut.Items.Add("");
                //GECHG1598219
                comboBoxProduitLocal.SelectedIndex = -1;
                comboBoxZoneLocal.SelectedIndex = -1;
                comboBoxEmplLocal.SelectedIndex = -1;
            }
        }
        //action made when i select one SN not traited
        private void CheckedListBoxNumSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            //on SN change clear,reset fields
            foreach (int i in checkedListBoxNumSerie.CheckedIndices)
            {
                if (Convert.ToBoolean(checkedListBoxNumSerie.GetItemCheckState(i)) == true)//if checked
                {
                    SelectSnRefreshForm(checkedListBoxNumSerie.CheckedItems[0].ToString());//init field value
                }
            }
        }

        //methode to refresh form when one Sn is selected
        private void SelectSnRefreshForm(string sn)
        {
            //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone" et "Emplaceemnt"
            //GECHG1598219.Old
            //string query = "select epe_ressuage_indice.type_indication,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.positif_negatif_autre,epe_ressuage_indice.type_defaut,epe_ressuage_indice.commentaire" +
            //            " from TBSCT.epe_ressuage_piece" +
            //            " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
            //            $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication='{1}' " +
            //           "order by epe_ressuage_indice.indication";
            
            //GECHG1598219.New
            string query = "select epe_ressuage_indice.type_indication,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.positif_negatif_autre,epe_ressuage_indice.type_defaut,epe_ressuage_indice.commentaire,epe_ressuage_indice.des_produit,epe_ressuage_indice.des_zone,epe_ressuage_indice.des_emplacement" +
                        " from TBSCT.epe_ressuage_piece" +
                        " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                        $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication='{1}' " +
                        "order by epe_ressuage_indice.indication";
            OracleDataReader data = db.Request(query);
            if (data.HasRows)
            {
                if (data.Read())
                {
                    //add nombre indication
                    string queryNb_ind = "select max(epe_ressuage_indice.indication) nb_ind from tbsct.epe_ressuage_indice " +
                        " inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                        $" where (epe_ressuage_piece.sn = '{sn}'and epe_ressuage_piece.op ='{mes.GetOp()}' and epe_ressuage_piece.job_nub ='{mes.GetOf()}') order by epe_ressuage_indice.indication ";
                    OracleDataReader dataNb_ind = db.Request(queryNb_ind);
                    if (dataNb_ind.Read()) numericUpDownNbInd.Value = Convert.ToInt32(dataNb_ind["nb_ind"]);
                    numericUpDownNumind.Value = 1;
                    comboBoxTypeInd.SelectedItem = data["type_indication"].ToString();
                    comboBoxPN.SelectedItem = data["positif_negatif_autre"].ToString();
                    comboBoxTypeDefaut.SelectedItem = data["type_defaut"].ToString();
                    textBoxCommentaire.Text = data["commentaire"].ToString();
                    //GECHG1598219 : Récupère les nouveaux champs "Produit", "Zone" et "Emplaceemnt"
                    comboBoxProduitLocal.SelectedItem = data["des_produit"].ToString();
                    comboBoxZoneLocal.SelectedItem = data["des_zone"].ToString();
                    comboBoxEmplLocal.SelectedItem = data["des_emplacement"].ToString();

                    if (data.IsDBNull(1)) //if taille is null
                    {
                        labelTaile.Visible = false;
                        labelClasse.Visible = true;
                        labelUnit.Visible = false;
                        textBoxTaille.Visible = false;
                        comboBoxClasse.Visible = true;
                        textBoxTaille.Clear();
                        comboBoxClasse.SelectedItem = data["classe_indice"].ToString();
                    }
                    else
                    {
                        labelTaile.Visible = true;
                        labelClasse.Visible = false;
                        labelUnit.Visible = true;
                        textBoxTaille.Visible = true;
                        comboBoxClasse.Visible = false;
                        comboBoxClasse.Items.Clear();
                        textBoxTaille.Text = data["taille_indice"].ToString();
                    }
                }
                if (data.IsClosed == false) data.Close();
            }
            else
            {
                //clear fields when the value change
                numericUpDownNumind.Refresh();
                numericUpDownNbInd.Refresh();
                textBoxCommentaire.Text = "";
                textBoxTaille.Text = "";
                comboBoxTypeInd.SelectedIndex = -1;
                comboBoxPN.SelectedIndex = -1;
                comboBoxClasse.ResetText();
                comboBoxTypeDefaut.Items.Clear();
                comboBoxTypeDefaut.Items.Add("");
                //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone" et "Emplacement"
                comboBoxProduitLocal.SelectedIndex = -1;
                comboBoxZoneLocal.SelectedIndex = -1;
                comboBoxEmplLocal.SelectedIndex = -1;
            }
        }
        //Allo the user to check only one sn when he want to modifie the sn
        private void CheckedListBoxSnTaite_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && checkedListBoxSnTaite.CheckedItems.Count > 0)
            {
                checkedListBoxSnTaite.ItemCheck -= CheckedListBoxSnTaite_ItemCheck;
                checkedListBoxSnTaite.SetItemChecked(checkedListBoxSnTaite.CheckedIndices[0], false);
                checkedListBoxSnTaite.ItemCheck += CheckedListBoxSnTaite_ItemCheck;
            }
        }
        //action to move sn when operato answer is yes
        //refresh the form with the SN moved
        private void CheckedListBoxSnTaite_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (int i in checkedListBoxSnTaite.CheckedIndices)
            {
                if (Convert.ToBoolean(checkedListBoxSnTaite.GetItemCheckState(i)) == true)
                {
                    if (checkedListBoxNumSerie.CheckedItems.Count > 0 && checkedListBoxSnTaite.SelectedIndex != -1)
                    {
                        MessageBox.Show("Terminer la cartographie du SN non traité en cours à gauche");
                        checkedListBoxSnTaite.SetItemCheckState(i, CheckState.Unchecked); //uncheck the item
                    }
                    else
                    {
                        if (checkedListBoxSnTaite.SelectedIndex != -1)
                        {
                            MessageBoxButtons buttons = MessageBoxButtons.YesNo;   // Initializes the variables to pass to the MessageBox.Show method.                                   
                            DialogResult result = MessageBox.Show($"Voulez-vous modifier les infomations du SN {checkedListBoxSnTaite.CheckedItems[0]}?", "Modification SN", buttons);  // Displays the MessageBox.
                            if (result == DialogResult.Yes)
                            {
                                // mark the sn as valided in data base 
                                string queryflag = "UPDATE tbsct.epe_ressuage_piece" +
                                    $" set epe_ressuage_piece.flag_valided ={0}" +
                                    $" where epe_ressuage_piece.sn='{checkedListBoxSnTaite.CheckedItems[0]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}'";
                                db.Request(queryflag);
                                checkedListBoxNumSerie.Items.Insert(0, checkedListBoxSnTaite.CheckedItems[0]); //add the sn to the sn not valided yet from the top
                                checkedListBoxNumSerie.SetItemChecked(0, true); //check the value 
                                checkedListBoxSnTaite.Items.Remove(checkedListBoxSnTaite.CheckedItems[0]);//remove from cheked list
                                SelectSnRefreshForm(checkedListBoxNumSerie.CheckedItems[0].ToString());//refrech form
                            }
                            else
                            {
                                checkedListBoxSnTaite.SetItemCheckState(i, CheckState.Unchecked); //uncheck the item
                            }
                        }
                    }
                }
            }
        }
        //type defaut comboBox depends on the choise of comboBox positif, négatif and autre.  
        private void ComboBoxPN_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxTypeDefaut.Items.Clear();
            comboBoxTypeDefaut.Text = "";

            //For item in dropdown(menu déroumant) if item = to element selected then make selection with that value
            for (int i = 0; i < comboBoxPN.Items.Count; i++)
            {
                if (comboBoxPN.Items[i].ToString() == comboBoxPN.Text)
                {
                    string query = $"select TBSCT.epe_ressuage_typeDefaut.valeur from TBSCT.epe_ressuage_indicationPN " +
                        $"inner join TBSCT.epe_ressuage_typeDefaut on TBSCT.epe_ressuage_indicationPN.idx = TBSCT.epe_ressuage_typeDefaut.idx " +
                        $"where TBSCT.epe_ressuage_indicationPN.valeur = '{comboBoxPN.Text}'";
                    OracleDataReader data = db.Request(query);
                    log.writeLog(query, "trace",0);
                    while (data.Read())
                    {
                        comboBoxTypeDefaut.Items.Add(data["valeur"]);
                    }
                    if (data.IsClosed == false) data.Close();
                }
            }
            //resize combo box is the dropdown vlue is too long
            ResizeComboBox(comboBoxTypeDefaut);
        }
        //when type indiction is type II, taille change to combobox(menu déroulant)and set dropdown classe value  
        private void ComboBoxTypeInd_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxClasse.Items.Clear();
            textBoxTaille.Clear();
            string query = $"select TBSCT.epe_ressuage_classetype.valeur from TBSCT.epe_ressuage_classetype " +
                    $"inner join TBSCT.epe_ressuage_typeindication on TBSCT.epe_ressuage_typeindication.id = TBSCT.epe_ressuage_classetype.t_i " +
                    $"where TBSCT.epe_ressuage_typeindication.type_indication = '{comboBoxTypeInd.Text}'";
            OracleDataReader data = db.Request(query);
            log.writeLog(query, "trace",0);
            if (data.HasRows)
            {
                while (data.Read())
                {
                    comboBoxClasse.Items.Add(data["valeur"]);
                }
                if (data.IsClosed == false) data.Close();
                //resize combo box if the dropdown value is too long
                ResizeComboBox(comboBoxClasse);
                textBoxTaille.Clear();
                labelTaile.Visible = false;
                labelClasse.Visible = true;
                labelUnit.Visible = false;
                textBoxTaille.Visible = false;
                comboBoxClasse.Visible = true;
            }
            else
            {
                comboBoxClasse.Items.Clear();
                labelTaile.Visible = true;
                labelClasse.Visible = false;
                labelUnit.Visible = true;
                textBoxTaille.Visible = true;
                comboBoxClasse.Visible = false;
            }
        }
        //set taille field value to null
        private void TextBoxTaille_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxTypeInd.SelectedIndex == -1)
            {
                textBoxTaille.Clear();
                return;
            }
        }
        //convert text field to float field and limite two value after the dot
        private void TextBoxTaille_KeyPress(object sender, KeyPressEventArgs e)
        {
            //accept only numeric value and dot
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
            // accept float
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
                return;
            }
            //allow only two decimal after de dot
            if (!char.IsControl(e.KeyChar) && System.Text.RegularExpressions.Regex.IsMatch(textBoxTaille.Text, "\\.\\d\\d")) e.Handled = true;
        }
        //check if nombre d'indication is null
        private void NumericUpDownNbInd_ValueChanged(object sender, EventArgs e)
        {
            if (checkedListBoxNumSerie.CheckedItems.Count == 0) return;
            else { 
                if (numericUpDownNbInd.Value == 0)
                {
                    numericUpDownNbInd.Value += 1;
                }
                if (numericUpDownNbInd.Value >= oldNb_indValue && numericUpDownNbInd.Value != 0) 
                {
                }
                else
                {
                    //get the nombre indication and compare with old value
                    string queryNb_ind = "select max(epe_ressuage_indice.indication) nb_ind from tbsct.epe_ressuage_indice " +
                        " inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                        $" where (epe_ressuage_piece.sn = '{checkedListBoxNumSerie.CheckedItems[0]}' and epe_ressuage_piece.op ='{mes.GetOp()}' and epe_ressuage_piece.job_nub ='{mes.GetOf()}') order by epe_ressuage_indice.indication ";
                    OracleDataReader dataNb_ind = db.Request(queryNb_ind);
                    if (dataNb_ind.HasRows)
                    {
                        if (dataNb_ind.Read())
                        {
                            if (dataNb_ind.IsDBNull(0)) return;
                            else
                            {
                                if (oldNb_indValue == Convert.ToDecimal(dataNb_ind["nb_ind"]))
                                {
                                    MessageBox.Show($"Merci de supprimer les photos de l'indication {oldNb_indValue} si votre souhait est de reduire le nombre d'indication");
                                    numericUpDownNbInd.Value = oldNb_indValue;
                                }
                            }
                        }
                        if (dataNb_ind.IsClosed == false) dataNb_ind.Close();
                    }
                }
                oldNb_indValue = numericUpDownNbInd.Value;
            }
        }

        //methode to complet field forn with an indication number we got 
        private void NumIndConpleteFields(string sn, int numeroInd)
        {
            OracleDataReader data = null;
            try
            {
                //GECHG1598219 : Prendre en compte les nouveaux chmaps "Produit", "Zone" et "Emplaceemnt"
                //GECHG1598219.Old
                //string query = "select epe_ressuage_indice.type_indication,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.positif_negatif_autre,epe_ressuage_indice.type_defaut,epe_ressuage_indice.commentaire" +
                //            " from TBSCT.epe_ressuage_piece" +
                //            " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                //            $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication='{numeroInd}' order by epe_ressuage_indice.indication";

                //GECHG1598219.New
                string query = "select epe_ressuage_indice.type_indication,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.positif_negatif_autre,epe_ressuage_indice.type_defaut,epe_ressuage_indice.commentaire, epe_ressuage_indice.des_produit,epe_ressuage_indice.des_zone,epe_ressuage_indice.des_emplacement" +
                            " from TBSCT.epe_ressuage_piece" +
                            " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                            $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication='{numeroInd}' order by epe_ressuage_indice.indication";

                data = db.Request(query);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        comboBoxTypeInd.SelectedItem = data["type_indication"].ToString();
                        comboBoxPN.SelectedItem = data["positif_negatif_autre"].ToString();
                        comboBoxTypeDefaut.SelectedItem = data["type_defaut"].ToString();
                        textBoxCommentaire.Text = data["commentaire"].ToString();
                        //GECHG1598219.New
                        comboBoxProduitLocal.SelectedItem = data["des_produit"].ToString();
                        comboBoxZoneLocal.SelectedItem = data["des_zone"].ToString();
                        comboBoxEmplLocal.SelectedItem = data["des_emplacement"].ToString();

                        if (data.IsDBNull(1)) //if taille is null
                        {
                            labelTaile.Visible = false;
                            labelClasse.Visible = true;
                            labelUnit.Visible = false;
                            textBoxTaille.Visible = false;
                            comboBoxClasse.Visible = true;
                            textBoxTaille.Clear();
                            comboBoxClasse.SelectedItem = data["classe_indice"].ToString();
                        }
                        else
                        {
                            labelTaile.Visible = true;
                            labelClasse.Visible = false;
                            labelUnit.Visible = true;
                            textBoxTaille.Visible = true;
                            comboBoxClasse.Visible = false;
                            comboBoxClasse.Items.Clear();
                            textBoxTaille.Text = data["taille_indice"].ToString();
                        }
                    }
                    if (data.IsClosed == false) data.Close();
                }
                else
                {
                    //clear fields when the value change 
                    textBoxCommentaire.Text = "";
                    textBoxTaille.Text = "";
                    comboBoxTypeInd.SelectedIndex = -1;
                    comboBoxPN.SelectedIndex = -1;
                    comboBoxClasse.ResetText();
                    comboBoxTypeDefaut.Items.Clear();
                    comboBoxTypeDefaut.Items.Add("");
                    //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone" et "Emplacement"
                    comboBoxProduitLocal.SelectedIndex = -1;
                    comboBoxZoneLocal.SelectedIndex = -1;
                    comboBoxEmplLocal.SelectedIndex = -1;
                }
            }
            finally
            {
                if (data.IsClosed == false)
                {
                    data.Close();
                }
            }
        }
        //check if numéro indiation>nombre indication and nombre inndication and numero indication not null
        //allow user to come back to one numero indication
        private void NumericUpDownNumind_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownNumind.Value > numericUpDownNbInd.Value) numericUpDownNumind.Value -= 1;
            if (numericUpDownNumind.Value == 0) numericUpDownNumind.Value += 1;
            if (checkedListBoxNumSerie.CheckedItems.Count > 0 && numericUpDownNumind.Value > 1)
            {
                foreach (object sn in checkedListBoxNumSerie.CheckedItems)
                {
                    OracleDataReader dataCHeck=null;
                    try
                    {
                        string qcheckPhoto = "select epe_ressuage_photos.seq" +
                            " from TBSCT.epe_ressuage_piece" +
                            " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                            " inner join TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                            $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value - 1}";
                        dataCHeck = db.Request(qcheckPhoto);
                        if (dataCHeck.HasRows) //if we get photo for the last value : we can go up
                        {
                            //The following line is incase we go down to show value
                            NumIndConpleteFields(sn.ToString(), Convert.ToInt32(numericUpDownNumind.Value));
                        }
                        else //if the selection return 0, data doesnt has row
                        {
                            //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone", "Emplacement"
                            //GECHG1598219.Old
                            //if (comboBoxTypeInd.SelectedIndex == -1 || textBoxTaille.Visible == true && String.IsNullOrEmpty(textBoxTaille.Text) ||
                            //        comboBoxClasse.Visible == true && String.IsNullOrEmpty(comboBoxClasse.Text) || c.SelectedIndex == -1 ||
                            //        comboBoxTypeDefaut.SelectedIndex == -1 || checkedListBoxNumSerie.CheckedItems.Count == 0)
                            //GECHG1598219.New
                            if (comboBoxTypeInd.SelectedIndex == -1 || textBoxTaille.Visible == true && String.IsNullOrEmpty(textBoxTaille.Text) ||
                                comboBoxClasse.Visible == true && String.IsNullOrEmpty(comboBoxClasse.Text) || comboBoxPN.SelectedIndex == -1 ||
                                comboBoxTypeDefaut.SelectedIndex == -1 || checkedListBoxNumSerie.CheckedItems.Count == 0 ||
                                comboBoxProduitLocal.SelectedIndex == -1 || comboBoxZoneLocal.SelectedIndex == -1 || comboBoxEmplLocal.SelectedIndex == -1)
                                {
                                string TypeInd = comboBoxTypeInd.Text;
                                string PN = comboBoxPN.Text;
                                string TypeDefaut = comboBoxTypeDefaut.Text;
                                string Classe = comboBoxClasse.Text;
                                string Taille = textBoxTaille.Text;
                                //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone", "Emplacement"
                                string Produit = comboBoxProduitLocal.Text;
                                string Zone = comboBoxZoneLocal.Text;
                                string Emplacement = comboBoxEmplLocal.Text;

                                numericUpDownNumind.Value -= 1;
                                MessageBox.Show($"Vous devez compléter les champs pour l'indice {numericUpDownNumind.Value}");
                                comboBoxTypeInd.Text = TypeInd;
                                comboBoxPN.Text = PN;
                                comboBoxTypeDefaut.Text = TypeDefaut;
                                comboBoxClasse.Text = Classe;
                                textBoxTaille.Text = Taille;
                                //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone", "Emplacement"
                                comboBoxProduitLocal.Text = Produit;
                                comboBoxZoneLocal.Text = Zone;
                                comboBoxEmplLocal.Text = Emplacement;

                            }
                            else
                            {
                                DialogResult res = MessageBox.Show($"Confirmez-vous la validation de l'indice {numericUpDownNumind.Value-1} sans photo?\r\n" +
                                "si 'Non' appuyez sur le bouton Ajouter photo", "Validation de l'indice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (res == DialogResult.Yes)
                                {
                                    if (String.IsNullOrEmpty(textBoxCommentaire.Text))
                                    {
                                        string TypeInd = comboBoxTypeInd.Text;
                                        string PN = comboBoxPN.Text;
                                        string TypeDefaut = comboBoxTypeDefaut.Text;
                                        string Classe = comboBoxClasse.Text;
                                        string Taille = textBoxTaille.Text;
                                        //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone", "Emplacement"
                                        string Produit = comboBoxProduitLocal.Text;
                                        string Zone = comboBoxZoneLocal.Text;
                                        string Emplacement = comboBoxEmplLocal.Text;
                                        numericUpDownNumind.Value -= 1;
                                        MessageBox.Show($"Vous devez obligatoirement ajouter un commentaire si vous souhaitez valider le SN ({checkedListBoxNumSerie.CheckedItems[0]}) sans photo", "Validation de l'indice");
                                        comboBoxTypeInd.Text= TypeInd;
                                        comboBoxPN.Text=PN;
                                        comboBoxTypeDefaut.Text= TypeDefaut;
                                        comboBoxClasse.Text= Classe;
                                        textBoxTaille.Text=Taille;
                                        //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone", "Emplacement"
                                        comboBoxProduitLocal.Text = Produit;
                                        comboBoxZoneLocal.Text = Zone;
                                        comboBoxEmplLocal.Text = Emplacement;
                                    }
                                    else
                                    {
                                        log.writeLog("validation des champs oK", "historique", 0);
                                        Decimal indice = numericUpDownNumind.Value-1;
                                        string TypeInd = comboBoxTypeInd.Text;
                                        string PN = comboBoxPN.Text;
                                        string TypeDefaut = comboBoxTypeDefaut.Text;
                                        string Classe = comboBoxClasse.Text;
                                        string Taille = textBoxTaille.Text.Replace('.', ',');
                                        string Commentaire=textBoxCommentaire.Text.Replace("'", "''");
                                        //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone", "Emplacement"
                                        string Produit = comboBoxProduitLocal.Text;
                                        string Zone = comboBoxZoneLocal.Text;
                                        string Emplacement = comboBoxEmplLocal.Text;
                                        //insert value typed by the operator
                                        //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone", "Emplacement"
                                        //GECHG1598219.Old
                                        //AddDataHorsProcess(indice,TypeInd,PN,TypeDefaut,Classe,Taille,Commentaire);
                                        //GECHG1598219.New
                                        AddDataHorsProcess(indice,TypeInd,PN,TypeDefaut,Classe,Taille,Commentaire,Produit,Zone,Emplacement);
                                        //refresh for the next indice
                                        NumIndConpleteFields(checkedListBoxNumSerie.CheckedItems[0].ToString(), Convert.ToInt32(numericUpDownNumind.Value));
                                        //log.writeLog("Ajout des élements sélectionés en base de données OK", "historique", 0);
                                    }
                                }
                                else
                                {
                                    string TypeInd = comboBoxTypeInd.Text;
                                    string PN = comboBoxPN.Text;
                                    string TypeDefaut = comboBoxTypeDefaut.Text;
                                    string Classe = comboBoxClasse.Text;
                                    string Taille = textBoxTaille.Text;
                                    numericUpDownNumind.Value -= 1;
                                    //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone", "Emplacement"
                                    string Produit = comboBoxProduitLocal.Text;
                                    string Zone = comboBoxZoneLocal.Text;
                                    string Emplacement = comboBoxEmplLocal.Text;
                                    comboBoxTypeInd.Text = TypeInd;
                                    comboBoxPN.Text = PN;
                                    comboBoxTypeDefaut.Text = TypeDefaut;
                                    comboBoxClasse.Text = Classe;
                                    textBoxTaille.Text = Taille;
                                    //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone", "Emplacement"
                                    comboBoxProduitLocal.Text = Produit;
                                    comboBoxZoneLocal.Text = Zone;
                                    comboBoxEmplLocal.Text = Emplacement;
                                }
                            }
                        }
                        if (dataCHeck.IsClosed == false) dataCHeck.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Il y a un problème au niveau de la gestion du champs numéro indication", "Champs numéro indication", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (dataCHeck.IsClosed == false) dataCHeck.Close();
                    }
                }
            }
            if (checkedListBoxNumSerie.CheckedItems.Count > 0 && numericUpDownNumind.Value == 1)
            {
                numericUpDownNumind.Refresh();
                NumIndConpleteFields(checkedListBoxNumSerie.CheckedItems[0].ToString(), Convert.ToInt32(numericUpDownNumind.Value));
            }
        }
        //comment, in case the  opérateur want to add more détails 
        //the sixe can be more the 1000 caractère 
        private void TextBoxCommentaire_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCommentaire.Text.Length > 1000) {
                MessageBox.Show("Votre commentaire ne dois pas dépasser 1000 caractères");
                textBoxCommentaire.Text = (textBoxCommentaire.Text).Remove(textBoxCommentaire.Text.Length - 1);//delete de last caractere
            }
        }

        //Methode to insert information into Table and fill the datagrid
        private void DataActionNormalProcess(string pathPhoto)
        {
            try
            {
                string qnumberseq = "select epe_ressuage_photos.seq " +
                            "from TBSCT.epe_ressuage_photos " +
                            "inner join  TBSCT.epe_ressuage_indice on epe_ressuage_indice.id = epe_ressuage_photos.id_indice " +
                            "inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece " +
                            $" where epe_ressuage_piece.sn ='{checkedListBoxNumSerie.CheckedItems[0]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value}";
                OracleDataReader dataqnumberseq = db.Request(qnumberseq);
                if (dataqnumberseq.HasRows && dataqnumberseq.Read() && Convert.ToDecimal(dataqnumberseq["seq"]) == 0)
                {
                    if (dataqnumberseq.IsClosed == false) dataqnumberseq.Close();
                    InsertDataActionNormalProcess(pathPhoto, "horsprocess");
                }
                else
                {
                    if (dataqnumberseq.IsClosed == false) dataqnumberseq.Close();
                    InsertDataActionNormalProcess(pathPhoto,"normalprocesss");
                }
            }
            catch (Exception ex)
            {
                log.writeLog($"Ajout des photos en base de données {ex.Message} {ex.Source}", "log", 1);
                MessageBox.Show($"Un problème s'est produit au moment de l'ajout des photos","Ajout de photos");
            }
        }

        //delete de previous enregistrement
        private void CaseHorprocessChanged()
        {
            // this delete cascade delete indice and photo
            string qdltEn = "DELETE from TBSCT.epe_ressuage_indice " +
                $"where indication ={numericUpDownNumind.Value} and id_piece = (select epe_ressuage_piece.id from tbsct.epe_ressuage_piece " +
                $"where epe_ressuage_piece.sn ='{checkedListBoxNumSerie.CheckedItems[0]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}')";
            db.Request(qdltEn);
            log.writeLog(qdltEn, "trace", 0);
        }

        private void InsertDataActionNormalProcess(string pathPhoto, string cas)
        {
            log.writeLog("validation des champs OK", "historique", 0);
            log.writeLog("Ajout des élements sélectionés en base de données", "historique", 0);
            //get photos selected 
            string openDir = $"{pathPhoto}{checkedListBoxNumSerie.CheckedItems[0]}";
            var listephoto = photo.GetPhoto(openDir);

            if (listephoto.Count != 0 && cas == "horsprocess") CaseHorprocessChanged();

            //for each sn selected add inforation to data base 
            for (int x = 0; x < checkedListBoxNumSerie.CheckedItems.Count; x++)
            {
                //add selected items to database then datagridview 
                for (int seq = 0; seq < listephoto.Count; seq++)
                {
                    FileInfo photo = new FileInfo(listephoto[seq]);
                    //add one item(SN) to database
                    string queryPiece = "insert into TBSCT.epe_ressuage_piece(job_nub, op, sn, cdc, operateur, article, date_ressuage)" +
                        $" select '{mes.GetOf()}','{mes.GetOp()}','{checkedListBoxNumSerie.CheckedItems[x]}','{mes.GetCdc()}','{mes.GetOperateur()}','{mes.GetArticle()}',current_date" +
                        " from SYS.dual" +
                        $" WHERE NOT EXISTS(select * from TBSCT.epe_ressuage_piece where sn='{checkedListBoxNumSerie.CheckedItems[x]}' and job_nub='{mes.GetOf()}' and op='{mes.GetOp()}')";
                    db.Request(queryPiece);
                    log.writeLog(queryPiece, "trace", 0);

                    //return the id of the value insered for the current SN
                    OracleDataReader idPiece = db.Request($"select id from TBSCT.epe_ressuage_piece where sn='{checkedListBoxNumSerie.CheckedItems[x]}' and job_nub='{mes.GetOf()}' and op='{mes.GetOp()}'");
                    if (idPiece.HasRows && idPiece.Read()) //if idPiece content one or many rows
                    {
                        //can't insert a same numero indication for a sn existing
                        //GECHG1598219 : Prendre en compte les nouveaux chmaps "Produit", "Zone" et "Emplacement"
                        //GECHG1598219.Old
                        //string queryIndice = "insert into tbsct.epe_ressuage_indice(id_piece,indication,type_indication ,positif_negatif_autre ,type_defaut ,taille_indice, classe_indice,commentaire)" +
                        //    $"select {idPiece.GetInt32(0)},{numericUpDownNumind.Value},'{comboBoxTypeInd.Text}','{comboBoxPN.Text}','{comboBoxTypeDefaut.Text}','{textBoxTaille.Text.Replace('.', ',')}','{comboBoxClasse.Text}','{textBoxCommentaire.Text.Replace("'", "''")}'" +
                        //    " from SYS.dual" +
                        //    " where NOT EXISTS" +
                        //    $" (select id_piece,indication from tbsct.epe_ressuage_indice where id_piece={idPiece.GetInt32(0)} and indication='{numericUpDownNumind.Value}')";

                        //GECHG1598219.New
                        string queryIndice = "insert into tbsct.epe_ressuage_indice(id_piece,indication,type_indication ,positif_negatif_autre ,type_defaut ,taille_indice, classe_indice,commentaire,des_produit,des_zone,des_emplacement)" +
                           $"select {idPiece.GetInt32(0)},{numericUpDownNumind.Value},'{comboBoxTypeInd.Text}','{comboBoxPN.Text}','{comboBoxTypeDefaut.Text}','{textBoxTaille.Text.Replace('.', ',')}','{comboBoxClasse.Text}','{textBoxCommentaire.Text.Replace("'", "''")}','{comboBoxProduitLocal.Text}','{comboBoxZoneLocal.Text}','{comboBoxEmplLocal.Text}'" +
                           " from SYS.dual" +
                           " where NOT EXISTS" +
                           $" (select id_piece,indication from tbsct.epe_ressuage_indice where id_piece={idPiece.GetInt32(0)} and indication='{numericUpDownNumind.Value}')";

                        db.Request(queryIndice);
                        log.writeLog(queryIndice, "trace", 0);
                        OracleDataReader idIndice = db.Request($"select id from tbsct.epe_ressuage_indice where id_piece ={idPiece.GetInt32(0)} and indication={numericUpDownNumind.Value}");
                        if (idIndice.HasRows && idIndice.Read()) //if idIndice content one or many rows 
                        {
                            //get the last sequence of indication/picture 
                            string querySeq = "select MAX(epe_ressuage_photos.seq) max_seq" +
                                " from TBSCT.epe_ressuage_piece" +
                                " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                                " inner join TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                                $" where epe_ressuage_piece.sn ='{checkedListBoxNumSerie.CheckedItems[x]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value}";
                            log.writeLog(querySeq, "trace", 0);
                            OracleDataReader seqMax = db.Request(querySeq);

                            if (seqMax.HasRows)
                            {
                                if (seqMax.Read())
                                {
                                    //can add same photo for an indication
                                    if (seqMax.IsDBNull(0)) //if sequence is null
                                    {
                                        string queryPhoto = "insert into tbsct.epe_ressuage_photos(id_indice,lien_photo,seq) " +
                                            $"values ({idIndice.GetInt32(0)},'{photo.FullName + "#" + 1}',{1}) ";
                                        db.Request(queryPhoto);
                                        log.writeLog(queryPhoto, "trace", 0);
                                        SEQUENCE = 1;
                                    }
                                    else
                                    {
                                        SEQUENCE = Convert.ToInt32(seqMax["max_seq"]) + 1;
                                        string queryPhoto = "insert into tbsct.epe_ressuage_photos(id_indice,lien_photo,seq) " +
                                            $"values ({ idIndice.GetInt32(0)} ,'{photo.FullName + "#" + SEQUENCE}','{SEQUENCE}')";
                                        db.Request(queryPhoto);
                                        log.writeLog(queryPhoto, "trace", 0);
                                    }
                                }
                                if (seqMax.IsClosed == false) seqMax.Close();
                            }
                            //add the value insered to data grid for each photo
                            string queryData = "select epe_ressuage_piece.sn,epe_ressuage_indice.indication, epe_ressuage_photos.lien_photo,epe_ressuage_photos.seq,epe_ressuage_indice.type_indication,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.type_defaut,epe_ressuage_indice.positif_negatif_autre" +
                                " from TBSCT.epe_ressuage_piece" +
                                " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                                " inner join  TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                                $" where epe_ressuage_piece.sn ='{checkedListBoxNumSerie.CheckedItems[x]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value} and epe_ressuage_photos.lien_photo='{photo.FullName + "#" + SEQUENCE}'";
                            OracleDataReader data = db.Request(queryData);
                            log.writeLog(queryData, "trace", 0);
                            if (data.HasRows) //if data content one or many rows
                            {
                                while (data.Read())
                                {
                                    FileInfo photo_name = new FileInfo((string)data["lien_photo"]); //extract the photo name 
                                                                                                    //add items selected to datagridview from top
                                    dataGridViewRecap.Rows.Insert(0, data["sn"], data["indication"], photo_name.Name, data["seq"],
                                        data["type_indication"], data["taille_indice"], data["classe_indice"], data["type_defaut"], data["positif_negatif_autre"]);
                                }
                                if (data.IsClosed == false) data.Close();
                                Refresh_();
                            }
                        }
                    }
                    if (idPiece.IsClosed == false) idPiece.Close();
                }
            }
        }

        //button which open file directory in order to get photos taken by caméra close to aubes (caméra déportéé)
        //fill de datagridview
        private void Button_CameraDeportee(object sender, EventArgs e)
        {
            //first check if the previous steps was okay (Sn selected)
            log.writeLog("validation des champs", "historique", 0);
            if (checkedListBoxNumSerie.CheckedItems.Count == 0) MessageBox.Show("Sélectionner un numéro de série et effectuer les actions suivantes");
            else if (!Directory.Exists(path_dwl_photo_lampe + checkedListBoxNumSerie.CheckedItems[0])) MessageBox.Show("Télecharger les photos Pour le SN selectionné ou confirmer la réalisation de la cartographie en hors process en appuyant sur ''Valider SN''", 
                "Télechargement des photos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (comboBoxTypeInd.SelectedIndex == -1) MessageBox.Show("Sélectionner un type d'indication");
            else if (textBoxTaille.Visible == true && String.IsNullOrEmpty(textBoxTaille.Text)) MessageBox.Show("Indiquer la taille de l'indice");
            else if (comboBoxClasse.Visible == true && String.IsNullOrEmpty(comboBoxClasse.Text)) MessageBox.Show("Indiquer la classe de l'indice");
            else if (comboBoxPN.SelectedIndex == -1) MessageBox.Show("Sélectionner la caractérisation de l'indice ( positif,negatif...? )");
            else if (comboBoxTypeDefaut.SelectedIndex == -1) MessageBox.Show("Sélectionner la caractérisation du type de defaut");
            //GECHG1598219 : Prendre en comptes les nouveaux champ "Produit", "Zone" et "Emplacement"
            else if (comboBoxProduitLocal.SelectedIndex == -1) MessageBox.Show("Sélectionner un produit");
            else if (comboBoxZoneLocal.SelectedIndex == -1) MessageBox.Show("Sélectionner une zone");
            else if (comboBoxEmplLocal.SelectedIndex == -1) MessageBox.Show("Sélectionner un emplacement");
            else
            {
                //check that indication already have value and the information for the indication hasn't change
                foreach (object sn in checkedListBoxNumSerie.CheckedItems)
                {
                    //GECHG1598219 : Prende en compte les nouveaux champs "Produit", "Zone" et "Emplacement"
                    //GECHG1598219.Old
                    //string qcheck = "select epe_ressuage_indice.type_indication, epe_ressuage_indice.positif_negatif_autre, epe_ressuage_indice.type_defaut,epe_ressuage_indice.taille_indice, epe_ressuage_indice.classe_indice,epe_ressuage_indice.commentaire" +
                    //    " from TBSCT.epe_ressuage_indice" +
                    //    " inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                    //    " inner join TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                    //    $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value} order by epe_ressuage_indice.indication";
                    //GECHG1598219.New
                    string qcheck = "select epe_ressuage_indice.type_indication, epe_ressuage_indice.positif_negatif_autre, epe_ressuage_indice.type_defaut,epe_ressuage_indice.taille_indice, epe_ressuage_indice.classe_indice,epe_ressuage_indice.commentaire,epe_ressuage_indice.des_produit,epe_ressuage_indice.des_zone,epe_ressuage_indice.des_emplacement" +
                        " from TBSCT.epe_ressuage_indice" +
                        " inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                        " inner join TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                        $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value} order by epe_ressuage_indice.indication";
                    OracleDataReader dataCheck = db.Request(qcheck);
                    if (dataCheck.HasRows) //if mys request throw row mea i have data for the indice eles i dont and i just save the enregistrement
                    {
                        if (dataCheck.Read())//check if the value rn is the same with the one in database 
                        {
                            //GECHG1598219.Old
                            //if (dataCheck["type_indication"].ToString() == comboBoxTypeInd.Text && dataCheck["positif_negatif_autre"].ToString() == comboBoxPN.Text && dataCheck["type_defaut"].ToString() == comboBoxTypeDefaut.Text
                            //    && dataCheck["taille_indice"].ToString() == textBoxTaille.Text && dataCheck["classe_indice"].ToString() == comboBoxClasse.Text && dataCheck["commentaire"].ToString() == textBoxCommentaire.Text)
                            //GECHG1598219.New
                            if (dataCheck["type_indication"].ToString() == comboBoxTypeInd.Text && dataCheck["positif_negatif_autre"].ToString() == comboBoxPN.Text && dataCheck["type_defaut"].ToString() == comboBoxTypeDefaut.Text
                                && dataCheck["taille_indice"].ToString() == textBoxTaille.Text && dataCheck["classe_indice"].ToString() == comboBoxClasse.Text && dataCheck["commentaire"].ToString() == textBoxCommentaire.Text
                                && dataCheck["des_produit"].ToString() == comboBoxProduitLocal.Text && dataCheck["des_zone"].ToString() == comboBoxZoneLocal.Text && dataCheck["des_emplacement"].ToString() == comboBoxEmplLocal.Text)
                            {
                                DataActionNormalProcess(path_dwl_photo_minilampe);
                            }
                            else
                            {
                                MessageBoxButtons buttons = MessageBoxButtons.YesNo;   // Initializes the variables to pass to the MessageBox.Show method.
                                                                                       // Displays the MessageBox.
                                DialogResult result = MessageBox.Show($"Vous avez changer les inforamtions de l'indice {numericUpDownNumind.Value}. \r\n" +
                                    $"Pour enregister cliquer sur NON puis sur le bouton ENREGISTRE. \r\n" +
                                    $"Appuyer sur OUI enregistrera les photos avec les informations précedentes", "Enregistrement de photo", buttons);
                                if (result == DialogResult.Yes)
                                {
                                    DataActionNormalProcess(path_dwl_photo_minilampe);

                                    //update de form with last value
                                    //GECHG1598219.Old
                                    //string query = "select epe_ressuage_indice.type_indication,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.positif_negatif_autre,epe_ressuage_indice.type_defaut,epe_ressuage_indice.commentaire" +
                                    //    " from TBSCT.epe_ressuage_piece" +
                                    //    " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                                    //    $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value} order by epe_ressuage_indice.indication";
                                    //GECHG1598219.New
                                    string query = "select epe_ressuage_indice.type_indication,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.positif_negatif_autre,epe_ressuage_indice.type_defaut,epe_ressuage_indice.commentaire,epe_ressuage_indice.des_produit,epe_ressuage_indice.des_zone,epe_ressuage_indice.des_emplacement" +
                                       " from TBSCT.epe_ressuage_piece" +
                                       " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                                       $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value} order by epe_ressuage_indice.indication";

                                    OracleDataReader data = db.Request(query);
                                    if (data.HasRows)
                                    {

                                        while (data.Read())
                                        {
                                            comboBoxTypeInd.SelectedItem = data["type_indication"].ToString();
                                            comboBoxPN.SelectedItem = data["positif_negatif_autre"].ToString();
                                            comboBoxTypeDefaut.SelectedItem = data["type_defaut"].ToString();
                                            textBoxCommentaire.Text = data["commentaire"].ToString();
                                            //GECHG1598219.New
                                            comboBoxProduitLocal.SelectedItem = data["des_produit"].ToString();
                                            comboBoxZoneLocal.SelectedItem = data["des_zone"].ToString();
                                            comboBoxEmplLocal.SelectedItem = data["des_emplacement"].ToString();
                                            if (data.IsDBNull(1)) //if taille is null
                                            {
                                                comboBoxClasse.SelectedItem = data["classe_indice"].ToString();
                                                labelTaile.Visible = false;
                                                labelClasse.Visible = true;
                                                labelUnit.Visible = false;
                                                textBoxTaille.Visible = false;
                                                comboBoxClasse.Visible = true;
                                                textBoxTaille.Clear();
                                            }
                                            else
                                            {
                                                labelTaile.Visible = true;
                                                labelClasse.Visible = false;
                                                labelUnit.Visible = true;
                                                textBoxTaille.Visible = true;
                                                comboBoxClasse.Visible = false;
                                                comboBoxClasse.Items.Clear();
                                                textBoxTaille.Text = data["taille_indice"].ToString();
                                            }
                                        }
                                        if (data.IsClosed == false) data.Close();
                                    }
                                }
                            }
                        }
                        if (dataCheck.IsClosed == false) dataCheck.Close();
                    }
                    else
                    {
                        DataActionNormalProcess(path_dwl_photo_minilampe);
                    }
                }
            }
        }
        //button which open file directory in order to get photos taken by camera not close to aube (caméra 13Mp)
        //fill de datagridview
        private void Button_CameraLampe(object sender, EventArgs e)
        {
            //first check if the previous steps was okay (Sn selected)
            log.writeLog("validation des champs", "historique", 0);
            if (checkedListBoxNumSerie.CheckedItems.Count == 0) MessageBox.Show("Sélectionner un numéro de série et effectuer les actions suivantes");
            else if (!Directory.Exists(path_dwl_photo_lampe+checkedListBoxNumSerie.CheckedItems[0])) MessageBox.Show("Télecharger les photos Pour le SN selectionné ou confirmer la réalisation de la cartographie en hors process en appuyant sur ''Valider SN''",
                "Télechargement des photos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (comboBoxTypeInd.SelectedIndex == -1) MessageBox.Show("Sélectionner un type d'indication");
            else if (textBoxTaille.Visible == true && String.IsNullOrEmpty(textBoxTaille.Text)) MessageBox.Show("Indiquer la taille de l'indice");
            else if (comboBoxClasse.Visible == true && String.IsNullOrEmpty(comboBoxClasse.Text)) MessageBox.Show("Indiquer la classe de l'indice");
            else if (comboBoxPN.SelectedIndex == -1) MessageBox.Show("Sélectionner la caractérisation de l'indice ( positif,negatif...? )");
            else if (comboBoxTypeDefaut.SelectedIndex == -1) MessageBox.Show("Sélectionner la caractérisation du type de defaut");
            //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone" et "Emplacement"
            else if (comboBoxProduitLocal.SelectedIndex == -1) MessageBox.Show("Sélectionner un produit");
            else if (comboBoxZoneLocal.SelectedIndex == -1) MessageBox.Show("Sélectionner une zone");
            else if (comboBoxEmplLocal.SelectedIndex == -1) MessageBox.Show("Sélectionner un emplacement");
            else
            {
                //check that indication already have value and the information for the indication hasn't change
                foreach (object sn in checkedListBoxNumSerie.CheckedItems)
                {
                    //GECHG1598219.Old
                    //string qcheck = "select epe_ressuage_indice.type_indication, epe_ressuage_indice.positif_negatif_autre, epe_ressuage_indice.type_defaut,epe_ressuage_indice.taille_indice, epe_ressuage_indice.classe_indice,epe_ressuage_indice.commentaire" +
                    //    " from TBSCT.epe_ressuage_indice" +
                    //    " inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                    //    " inner join TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                    //    $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value} order by epe_ressuage_indice.indication";
                    //GECHG1598219.New
                    string qcheck = "select epe_ressuage_indice.type_indication, epe_ressuage_indice.positif_negatif_autre, epe_ressuage_indice.type_defaut,epe_ressuage_indice.taille_indice, epe_ressuage_indice.classe_indice,epe_ressuage_indice.commentaire,epe_ressuage_indice.des_produit,epe_ressuage_indice.des_zone,epe_ressuage_indice.des_emplacement" +
                       " from TBSCT.epe_ressuage_indice" +
                       " inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                       " inner join TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                       $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value} order by epe_ressuage_indice.indication";

                    OracleDataReader dataCheck = db.Request(qcheck);
                    if (dataCheck.HasRows) //if my request throw row mean i have data for the indice else i dont have it and i just save the enregistrement
                    {
                        if (dataCheck.Read())//check if the value rn is the same with the one in database 
                        {
                            //GECHG1598219.Old
                            //if (dataCheck["type_indication"].ToString() == comboBoxTypeInd.Text && dataCheck["positif_negatif_autre"].ToString() == comboBoxPN.Text && dataCheck["type_defaut"].ToString() == comboBoxTypeDefaut.Text
                            //    && dataCheck["taille_indice"].ToString() == textBoxTaille.Text && dataCheck["commentaire"].ToString() == textBoxCommentaire.Text)
                            //GECHG1598219.New
                            if (dataCheck["type_indication"].ToString() == comboBoxTypeInd.Text && dataCheck["positif_negatif_autre"].ToString() == comboBoxPN.Text && dataCheck["type_defaut"].ToString() == comboBoxTypeDefaut.Text
                                && dataCheck["taille_indice"].ToString() == textBoxTaille.Text && dataCheck["commentaire"].ToString() == textBoxCommentaire.Text
                                && dataCheck["des_produit"].ToString() == comboBoxProduitLocal.Text && dataCheck["des_zone"].ToString() == comboBoxZoneLocal.Text && dataCheck["des_emplacement"].ToString() == comboBoxEmplLocal.Text)

                            {
                                DataActionNormalProcess(path_dwl_photo_lampe);
                            }
                            else
                            {
                                MessageBoxButtons buttons = MessageBoxButtons.YesNo;   // Initializes the variables to pass to the MessageBox.Show method.// Displays the MessageBox.
                                DialogResult result = MessageBox.Show($"Vous avez changer les inforamtions de l'indice {numericUpDownNumind.Value}. \r\n" +
                                     $"Pour enregister cliquer sur NON puis sur le bouton ENREGISTRE. \r\n" +
                                     $"Appuyer sur OUI enregistrera les photos avec les informations précedentes", "Enregistrement de photo", buttons);
                                if (result == DialogResult.Yes)
                                {
                                    DataActionNormalProcess(path_dwl_photo_lampe);
                                    //update de form with last value
                                    //GECHG1598219.Old
                                    //string query = "select epe_ressuage_indice.type_indication,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.positif_negatif_autre,epe_ressuage_indice.type_defaut,epe_ressuage_indice.commentaire" +
                                    //    " from TBSCT.epe_ressuage_piece" +
                                    //    " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                                    //    $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value} order by epe_ressuage_indice.indication";
                                    
                                    //GECHG1598219.New
                                    string query = "select epe_ressuage_indice.type_indication,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.positif_negatif_autre,epe_ressuage_indice.type_defaut,epe_ressuage_indice.commentaire,epe_ressuage_indice.des_produit,epe_ressuage_indice.des_zone,epe_ressuage_indice.des_emplacement" +
                                        " from TBSCT.epe_ressuage_piece" +
                                        " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                                        $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value} order by epe_ressuage_indice.indication";
                                    OracleDataReader data = db.Request(query);
                                    if (data.HasRows)
                                    {
                                        while (data.Read())
                                        {
                                            comboBoxTypeInd.SelectedItem = data["type_indication"].ToString();
                                            comboBoxPN.SelectedItem = data["positif_negatif_autre"].ToString();
                                            comboBoxTypeDefaut.SelectedItem = data["type_defaut"].ToString();
                                            textBoxCommentaire.Text = data["commentaire"].ToString();
                                            //GECHG1598219.New
                                            comboBoxProduitLocal.SelectedItem = data["des_produit"].ToString();
                                            comboBoxZoneLocal.SelectedItem = data["des_zone"].ToString();
                                            comboBoxEmplLocal.SelectedItem = data["des_emplacement"].ToString();
                                            if (data["taille_indice"].ToString() == null) //if taille is null
                                            {
                                                comboBoxClasse.SelectedItem = data["classe_indice"].ToString();
                                                labelTaile.Visible = false;
                                                labelClasse.Visible = true;
                                                labelUnit.Visible = false;
                                                textBoxTaille.Visible = false;
                                                comboBoxClasse.Visible = true;
                                                textBoxTaille.Clear();
                                            }
                                            else
                                            {
                                                labelTaile.Visible = true;
                                                labelClasse.Visible = false;
                                                labelUnit.Visible = true;
                                                textBoxTaille.Visible = true;
                                                comboBoxClasse.Visible = false;
                                                comboBoxClasse.Items.Clear();
                                                textBoxTaille.Text = data["taille_indice"].ToString();
                                            }
                                        }
                                        if (data.IsClosed == false) data.Close();
                                    }
                                }
                            }
                        }
                        if (dataCheck.IsClosed == false) dataCheck.Close();
                    }
                    else
                    {
                        DataActionNormalProcess(path_dwl_photo_lampe);
                    }
                }
            }
        }

        //bouton modifier/enregistrer modifie numero indication
        private void ButtonModifier_Click(object sender, EventArgs e)
        {
            try {
                if (checkedListBoxNumSerie.CheckedItems.Count == 0) MessageBox.Show("Procédé à la sélection de SN et les étapes qui suivant");
                else
                {
                    foreach (object sn in checkedListBoxNumSerie.CheckedItems)
                    {
                        string qcheckPhoto = "select epe_ressuage_photos.seq" +
                                " from TBSCT.epe_ressuage_piece" +
                                " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                                " inner join TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                                $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value}";
                        OracleDataReader dataCHeck = db.Request(qcheckPhoto);
                        //if the selection return 0, data doesnt has row
                        if (dataCHeck.HasRows)
                        {
                            // Displays the MessageBox.
                            DialogResult result = MessageBox.Show($"Vous êtes sur le point de faire une modifacation des informations de l'indice {numericUpDownNumind.Value}", "Modification informations indice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (result == DialogResult.Yes && dataCHeck.Read())
                            {
                                if (comboBoxTypeInd.SelectedIndex == -1) MessageBox.Show("La modification a échoué, sélectionnez un type d'indication");
                                else if (textBoxTaille.Visible == true && String.IsNullOrEmpty(textBoxTaille.Text)) MessageBox.Show("La modification a échoué, indiquez la taille de l'indice");
                                else if (comboBoxClasse.Visible == true && String.IsNullOrEmpty(comboBoxClasse.Text)) MessageBox.Show("La modification a échoué, indiquez la classe de l'indice");
                                else if (comboBoxPN.SelectedIndex == -1) MessageBox.Show("La modification a échoué, sélectionnez la caractérisation de l'indice ( positif,negatif...? )");
                                else if (comboBoxTypeDefaut.SelectedIndex == -1) MessageBox.Show("La modification a échoué, sélectionnez la caractérisation du type de defaut");
                                else if (Convert.ToDecimal(dataCHeck["seq"])==0 && String.IsNullOrEmpty(textBoxCommentaire.Text)) MessageBox.Show($"Vous devez obligatoirement ajouter un commentaire pour le SN {checkedListBoxNumSerie.CheckedItems[0]}");
                                //GECHG1598219 : Prendre en compte les nouveaux champs "Produit", "Zone" et "Emplacement"
                                else if (comboBoxProduitLocal.SelectedIndex == -1) MessageBox.Show("La modification a échoué, sélectionnez un produit");
                                else if (comboBoxZoneLocal.SelectedIndex == -1) MessageBox.Show("La modification a échoué, sélectionnez une zone");
                                else if (comboBoxEmplLocal.SelectedIndex == -1) MessageBox.Show("La modification a échoué, sélectionnez un emplacement");
                                else
                                {
                                    log.writeLog("Modification des informaions indice", "historique", 0);
                                    //insert modification
                                    //GECHG1598219.Old
                                    //string qUpdateseq = "update (select epe_ressuage_indice.type_indication,epe_ressuage_indice.positif_negatif_autre,epe_ressuage_indice.type_defaut,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.commentaire " +
                                    //    " from TBSCT.epe_ressuage_indice " +
                                    //    "inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece  " +
                                    //    $"where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value}) carac " +
                                    //    $"set carac.type_indication='{comboBoxTypeInd.Text}', carac.positif_negatif_autre='{comboBoxPN.Text}' , carac.type_defaut='{comboBoxTypeDefaut.Text}' , carac.taille_indice='{textBoxTaille.Text.Replace('.', ',')}' , carac.classe_indice='{comboBoxClasse.Text}' , carac.commentaire='{textBoxCommentaire.Text}'";
                                    
                                    //GECHG1598219.New
                                    string qUpdateseq = "update (select epe_ressuage_indice.type_indication,epe_ressuage_indice.positif_negatif_autre,epe_ressuage_indice.type_defaut,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.commentaire,epe_ressuage_indice.des_produit,epe_ressuage_indice.des_zone,epe_ressuage_indice.des_emplacement " +
                                        " from TBSCT.epe_ressuage_indice " +
                                        "inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece  " +
                                        $"where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value}) carac " +
                                        $"set carac.type_indication='{comboBoxTypeInd.Text}', carac.positif_negatif_autre='{comboBoxPN.Text}' , carac.type_defaut='{comboBoxTypeDefaut.Text}' , carac.taille_indice='{textBoxTaille.Text.Replace('.', ',')}' , carac.classe_indice='{comboBoxClasse.Text}' , carac.commentaire='{textBoxCommentaire.Text}',carac.des_produit='{comboBoxProduitLocal.Text}',carac.des_zone='{comboBoxZoneLocal.Text}',carac.des_emplacement='{comboBoxEmplLocal.Text}'";

                                    db.Request(qUpdateseq);
                                    log.writeLog(qUpdateseq, "trace", 0);
                                    //update recap tableau the value from board 
                                    foreach (DataGridViewRow item in dataGridViewRecap.Rows)
                                    {
                                        if (item.Cells["numeroSerie"].Value.ToString() == sn.ToString() && Convert.ToInt32(item.Cells["numeroIndication"].Value) == numericUpDownNumind.Value)
                                        {
                                            dataGridViewRecap.Rows[item.Index].Cells["typeInd"].Value = comboBoxTypeInd.Text;
                                            dataGridViewRecap.Rows[item.Index].Cells["typeDefaut"].Value = comboBoxTypeDefaut.Text;
                                            dataGridViewRecap.Rows[item.Index].Cells["positif_négatif"].Value = comboBoxPN.Text;
                                            dataGridViewRecap.Rows[item.Index].Cells["taille"].Value = textBoxTaille.Text;
                                            dataGridViewRecap.Rows[item.Index].Cells["classe"].Value = comboBoxClasse.Text;
                                            //GECHG1598219.New
                                            //dataGridViewRecap.Rows[item.Index].Cells["Produit"].Value = comboBoxProduitLocal.Text;
                                            //dataGridViewRecap.Rows[item.Index].Cells["Zone"].Value = comboBoxZoneLocal.Text;
                                            //dataGridViewRecap.Rows[item.Index].Cells["Emplacement"].Value = comboBoxEmplLocal.Text;
                                        }
                                    }
                                }
                            }
                            else return;
                        }
                        else
                        {
                            MessageBox.Show("Le boutton enregistrer permet de faire une modification des informations d'un indice. \r\n" + 
                                " le SN n'est pas validé, Valider au préalable le SN","Modication indication", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        if (dataCHeck.IsClosed == false) dataCHeck.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                log.writeLog($"La modifiaction d'une indication a échoué {ex.Message} {ex.Source}", "log", 1);
                MessageBox.Show("La modifiaction a échoué");
            }
        }
        //valider action, valide numero indication and chack if user have done with the SN selected
        private void ButtonValider_Click(object sender, EventArgs e)
        {
            if (checkedListBoxNumSerie.CheckedItems.Count == 0) { }
            else
            {
                //First check if the previous steps was okay 
                string qcheckPhoto = "select epe_ressuage_photos.seq" +
                        " from TBSCT.epe_ressuage_piece" +
                        " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                        " inner join TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                        $" where epe_ressuage_piece.sn ='{checkedListBoxNumSerie.CheckedItems[0]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={numericUpDownNumind.Value}";
                OracleDataReader dataCHeck = db.Request(qcheckPhoto);
                if (!dataCHeck.HasRows) //if photo haven't being add and the user want to valided SN
                {
                    try
                    {
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;   // Initializes the variables to pass to the MessageBox.Show method.
                        DialogResult result = MessageBox.Show($"Confirmez-vous la validation du  numéro de série {checkedListBoxNumSerie.CheckedItems[0]} et indication {numericUpDownNumind.Value} sans ajout de photos ?", $"Validation du SN ({checkedListBoxNumSerie.CheckedItems[0]}) sans photos", buttons);
                        if (result == DialogResult.Yes)
                        {
                            //GECHG1598219 : Les nouveaux champs Produit, Zone et Emplacement sont obligatoires
                            if (comboBoxTypeInd.SelectedIndex == -1 || textBoxTaille.Visible == true && String.IsNullOrEmpty(textBoxTaille.Text) ||
                                    comboBoxClasse.Visible == true && String.IsNullOrEmpty(comboBoxClasse.Text) || comboBoxPN.SelectedIndex == -1||
                                    comboBoxTypeDefaut.SelectedIndex == -1 || checkedListBoxNumSerie.CheckedItems.Count == 0||
                                    comboBoxProduitLocal.SelectedIndex == -1 || comboBoxZoneLocal.SelectedIndex == -1 || comboBoxEmplLocal.SelectedIndex == -1) MessageBox.Show("Vous devez compléter l'ensemble des champs");
                            else if (String.IsNullOrEmpty(textBoxCommentaire.Text)) MessageBox.Show($"Vous devez obligatoirement ajouter un commentaire si vous souhaitez valider le SN ({checkedListBoxNumSerie.CheckedItems[0]}) sans photo");
                            else
                            {
                                log.writeLog("validation des champs oK", "historique", 0);
                                //insert value typed by the operator
                                //GECHG1598219 : Insérer les nouveaux champs "Produit", "Zone", "Emplacement"
                                //GECHG1598219.Old
                                //AddDataHorsProcess(numericUpDownNumind.Value, comboBoxTypeInd.Text, c.Text, comboBoxTypeDefaut.Text, comboBoxClasse.Text, textBoxTaille.Text.Replace('.', ','), textBoxCommentaire.Text.Replace("'", "''"));
                                //GECHG1598219.New
                                AddDataHorsProcess(numericUpDownNumind.Value,comboBoxTypeInd.Text, comboBoxPN.Text, comboBoxTypeDefaut.Text, comboBoxClasse.Text, textBoxTaille.Text.Replace('.', ','), textBoxCommentaire.Text.Replace("'", "''"), comboBoxProduitLocal.Text, comboBoxZoneLocal.Text, comboBoxEmplLocal.Text);
                                //update the sn status
                                Set_CheckStatusSnHP();
                                log.writeLog("Ajout des élements sélectionés en base de données OK", "historique", 0);
                                log.writeLog("Validation des numéros de séries sélectionnées OK", "historique", 0);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.writeLog("Cartographie en hors process, validation d'un indice "+ex.Message, "log", 1);
                    }
                }
                else
                {
                    try
                    {
                        List<Decimal> sequences = new List<Decimal>();
                        //select all sequences
                        string qseq = "select epe_ressuage_photos.seq" +
                                " from TBSCT.epe_ressuage_piece" +
                                " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                                " inner join TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                                $" where epe_ressuage_piece.sn ='{checkedListBoxNumSerie.CheckedItems[0]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}'";
                        OracleDataReader dataSeq = db.Request(qseq);
                        if (dataSeq.HasRows)
                        {
                            while (dataSeq.Read())
                            {
                                sequences.Add((Decimal)dataSeq["seq"]);
                            }
                            bool isAllEqual = sequences.Distinct().Count() == 1;
                            if (isAllEqual) Set_CheckStatusSnHP(); //if sequence only contains 0
                            else SetSnStatus();
                        }
                        if (dataSeq.IsClosed == false) dataSeq.Close();
                        log.writeLog("Ajout des élements sélectionés en base de données OK", "historique", 0);
                        log.writeLog("Validation des numeros de series sélectionnées OK", "historique", 0);
                    }
                    catch (Exception ex)
                    {
                        log.writeLog($"Validation du SN {checkedListBoxNumSerie.CheckedItems[0]} :{ex.Message}", "log", 1);
                    }
                }
                if (dataCHeck.IsClosed == false) dataCHeck.Close();
            }
        }

        private void Set_CheckStatusSnHP()
        {
            //For each sn selected check if if numbre indication = count nombre indication where  
            foreach (string sn in checkedListBoxNumSerie.CheckedItems.OfType<string>().ToList())
            {
                //check if nb indication = numero indication
                string querySNtraite = "select count(indication) from tbsct.epe_ressuage_indice" +
                    " inner join tbsct.epe_ressuage_piece on  epe_ressuage_indice.id_piece=epe_ressuage_piece.id" +
                    $" where (epe_ressuage_piece.sn = '{sn}' and epe_ressuage_piece.op = '{mes.GetOp()}' and epe_ressuage_piece.job_nub = '{mes.GetOf()}')";
                OracleDataReader data = db.Request(querySNtraite);
                log.writeLog(querySNtraite, "trace", 0);
                if (data.Read())
                {
                    if (data.GetInt32(0) == numericUpDownNbInd.Value)
                    {
                        // mark the sn as valided in data base 
                        string queryflag = "UPDATE tbsct.epe_ressuage_piece" +
                            $" set epe_ressuage_piece.flag_valided ={1}" +
                            $" where epe_ressuage_piece.sn='{checkedListBoxNumSerie.CheckedItems[0]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}'";
                        db.Request(queryflag);
                        log.writeLog(queryflag, "trace", 0);
                        //Transfert the sn as traited
                        checkedListBoxSnTaite.Items.Add(sn);
                        //delete the value
                        checkedListBoxNumSerie.Items.Remove(sn);
                        //sn selected is valide, clear,reset fields
                        numericUpDownNbInd.Value = 1;
                        numericUpDownNumind.Value = 1;
                        textBoxCommentaire.Text = "";
                        textBoxTaille.Text = "";
                        comboBoxTypeInd.SelectedIndex = -1;
                        comboBoxPN.SelectedIndex = -1;
                        comboBoxClasse.ResetText();
                        comboBoxTypeDefaut.Items.Clear();
                        comboBoxTypeDefaut.Items.Add("");
                        //GECHG1598219 : Prendre en compte les 3 nouveaux champs "Produit", "Zone", "Emplacement"
                        comboBoxProduitLocal.SelectedIndex = -1;
                        comboBoxZoneLocal.SelectedIndex = -1;
                        comboBoxEmplLocal.SelectedIndex = -1;
                        log.writeLog("Validation des numéros de séries sélections OK", "historique", 0);
                    }
                    else
                    {
                        MessageBox.Show("Il vous manque des informations afin de valider le SN, vérifier le nombre d'indication / numéro d'indication");
                    }
                }
                if (data.IsClosed == false) data.Close();
            }
        }

        //Methode to insert information (hors process) into Tables and fill the datagrid
        //GECHG1598219 : Prise en compte des nouveaux champs "Produit", "Zone", "Emplacement"
        //GECHG1598219.Old
        //private void AddDataHorsProcess(Decimal indice, string TypeInd, string PN, string TypeDefaut, string Classe, string Taille, string Commentaire)
        //GECHG1598219.New
        private void AddDataHorsProcess(Decimal indice, string TypeInd, string PN, string TypeDefaut, string Classe, string Taille, string Commentaire, string Produit, string Zone, string Emplacement)
        {
            //for each sn selected add inforation to data base 
            for (int x = 0; x < checkedListBoxNumSerie.CheckedItems.Count; x++)
            {
                //add one item(SN) to database
                string queryPiece = "insert into TBSCT.epe_ressuage_piece(job_nub, op, sn, cdc, operateur, article, date_ressuage)" +
                    $" select '{mes.GetOf()}','{mes.GetOp()}','{checkedListBoxNumSerie.CheckedItems[x]}','{mes.GetCdc()}','{mes.GetOperateur()}','{mes.GetArticle()}',current_date" +
                    " from SYS.dual" +
                    $" WHERE NOT EXISTS(select* from TBSCT.epe_ressuage_piece where sn='{checkedListBoxNumSerie.CheckedItems[x]}' and job_nub='{mes.GetOf()}' and op='{mes.GetOp()}')";
                db.Request(queryPiece);
                log.writeLog(queryPiece, "trace", 0);

                //return the id of the value insered for the current SN
                OracleDataReader idPiece = db.Request($"select id from TBSCT.epe_ressuage_piece where sn='{checkedListBoxNumSerie.CheckedItems[x]}' and job_nub='{mes.GetOf()}' and op='{mes.GetOp()}'");
                if (idPiece.HasRows && idPiece.Read()) //if idPiece content one or many rows
                {
                    //can't insert a same numero indication for a sn existing
                    //GECHG1598219 : Enregistrement des nouveaux champs "Produit", "Zone", "Emplacement" en base
                    //GECHG1598219.Old
                    //string queryIndice = "insert into tbsct.epe_ressuage_indice(id_piece,indication,type_indication,positif_negatif_autre ,type_defaut ,taille_indice, classe_indice,commentaire)" +
                    //$"select {idPiece.GetInt32(0)},{indice},'{TypeInd}','{PN}','{TypeDefaut}','{Taille}','{Classe}','{Commentaire}'" +
                    //" from SYS.dual" +
                    //" where NOT EXISTS" +
                    //$" (select id_piece,indication from tbsct.epe_ressuage_indice where id_piece={idPiece.GetInt32(0)} and indication='{indice}')";

                    //GECHG1598219.New
                    string queryIndice = "insert into tbsct.epe_ressuage_indice(id_piece,indication,type_indication,positif_negatif_autre ,type_defaut ,taille_indice, classe_indice,commentaire,des_produit,des_zone, des_emplacement)" +
                        $"select {idPiece.GetInt32(0)},{indice},'{TypeInd}','{PN}','{TypeDefaut}','{Taille}','{Classe}','{Commentaire}','{Produit}','{Zone}','{Emplacement}'" +
                        " from SYS.dual" +
                        " where NOT EXISTS" +
                        $" (select id_piece,indication from tbsct.epe_ressuage_indice where id_piece={idPiece.GetInt32(0)} and indication='{indice}')";
                    db.Request(queryIndice);
                    log.writeLog(queryIndice, "trace", 0);
                    OracleDataReader idIndice = db.Request($"select id from tbsct.epe_ressuage_indice where id_piece ={idPiece.GetInt32(0)} and indication={indice}");
                    if (idIndice.HasRows && idIndice.Read()) //if idIndice content one or many rows 
                    {
                        string queryPhoto = "insert into tbsct.epe_ressuage_photos(id_indice,lien_photo,seq) " +
                            $"values ({idIndice.GetInt32(0)} ,'',{0})";
                        db.Request(queryPhoto);
                        log.writeLog(queryPhoto, "trace", 0);
                            
                        //add the value inserted  data to grid 
                        string queryData = "select epe_ressuage_piece.sn,epe_ressuage_indice.indication, epe_ressuage_photos.lien_photo,epe_ressuage_photos.seq,epe_ressuage_indice.type_indication,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.type_defaut,epe_ressuage_indice.positif_negatif_autre" +
                            " from TBSCT.epe_ressuage_piece" +
                            " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                            " inner join  TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                            $" where epe_ressuage_piece.sn ='{checkedListBoxNumSerie.CheckedItems[x]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={indice}";
                        OracleDataReader data = db.Request(queryData);
                        log.writeLog(queryData, "trace", 0);
                        if (data.HasRows) //if data content one or many rows
                        {
                            while (data.Read())
                            {
                                if (Convert.IsDBNull(data["lien_photo"]))
                                {
                                    dataGridViewRecap.Rows.Insert(0, data["sn"], data["indication"], "", "",
                                        data["type_indication"], data["taille_indice"], data["classe_indice"], data["type_defaut"], data["positif_negatif_autre"]);
                                }
                                else
                                {
                                    FileInfo photo_name = new FileInfo((string)data["lien_photo"]); //extract the photo name 
                                                                                                    //add items selected to datagridview from top
                                    dataGridViewRecap.Rows.Insert(0, data["sn"], data["indication"], photo_name.Name, data["seq"],
                                        data["type_indication"], data["taille_indice"], data["classe_indice"], data["type_defaut"], data["positif_negatif_autre"]);
                                }
                            }
                            if (data.IsClosed == false) data.Close();
                            Refresh_();
                        }
                    }
                }
                if (idPiece.IsClosed == false) idPiece.Close();
            }
        }

        //modify the status of the sn as valide after all information are checked
        private void SetSnStatus()
        {
            //For each sn selected check if if numbre indiccation = count nombre indication where  ... goupe by ...
            foreach (string sn in checkedListBoxNumSerie.CheckedItems.OfType<string>().ToList())
            {
                //check if nb indication = numro indaction
                string querySNtraite = "select count(indication) from tbsct.epe_ressuage_indice" +
                    " inner join tbsct.epe_ressuage_piece on  epe_ressuage_indice.id_piece=epe_ressuage_piece.id" +
                    $" where (epe_ressuage_piece.sn = '{sn}' and epe_ressuage_piece.op = '{mes.GetOp()}' and epe_ressuage_piece.job_nub = '{mes.GetOf()}')";
                OracleDataReader data = db.Request(querySNtraite);
                log.writeLog(querySNtraite, "trace", 0);
                if (data.Read())
                {
                    if (data.GetInt32(0) == numericUpDownNbInd.Value)
                    {
                        // mark the sn as valided in data base 
                        string queryflag = "UPDATE tbsct.epe_ressuage_piece" +
                            $" set epe_ressuage_piece.flag_valided ={1}" +
                            $" where epe_ressuage_piece.sn='{checkedListBoxNumSerie.CheckedItems[0]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}'";
                        db.Request(queryflag);
                        log.writeLog(queryflag, "trace", 0);
                        //Transfert the sn as traited
                        checkedListBoxSnTaite.Items.Add(sn);
                        //delete the value
                        checkedListBoxNumSerie.Items.Remove(sn);
                        //sn selected is valide, clear,reset fields
                        numericUpDownNbInd.Value = 1;
                        numericUpDownNumind.Value = 1;
                        textBoxCommentaire.Text = "";
                        textBoxTaille.Text = "";
                        comboBoxTypeInd.SelectedIndex = -1;
                        comboBoxPN.SelectedIndex = -1;
                        comboBoxClasse.ResetText();
                        comboBoxTypeDefaut.Items.Clear();
                        comboBoxTypeDefaut.Items.Add("");
                        //GECHG1598219 : Prendre en compte les 3 nouveaux champs "Produit", "Zone", "Emplacement"
                        comboBoxProduitLocal.SelectedIndex = -1;
                        comboBoxZoneLocal.SelectedIndex = -1;
                        comboBoxEmplLocal.SelectedIndex = -1;
                        log.writeLog("Validation des numeros de series sélections OK", "historique", 0);

                        //delete photo in the camera
                        try
                        {
                            photo.DeletePhotos(photolampe, NonCamera); //delete photo lampe  
                            photo.DeletePhotos(photominilampe, NonCamera); //delete photo mini lampe
                        }
                        catch (Exception)
                        {
                            MessageBox.Show($"Erreur suppresion des photos de la caméra.\r\nVérifier que la caméra est bien connectée et est en mode transfert de fichier(MTP)"
                                , "Suppresion des photos de la caméra", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            DialogResult result = MessageBox.Show($"Après vérification de la connectivité et le mode de transfert, Ressayer la suppression des photos de la caméra\r\n" +
                                "(La suppresion des photos de la caméra facilite le choix des photos pour le SN suivant)", "Suppresion des photos de la caméra", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                try
                                {
                                    photo.DeletePhotos(photolampe, NonCamera); //delete photo lampe  
                                    photo.DeletePhotos(photominilampe, NonCamera); //delete photo mini lampe
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"L'erreur de suppresion des photos persiste, faites en par du problème à l'équipe support", "Suppresion des photos de la caméra", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    log.writeLog($"Problème de connectique caméra {ex.Message} {ex.Source}", "log", 1);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Il vous manque des informations afin de valider le SN, vérifier le nombre d'indication");
                    }
                }
                if (data.IsClosed == false) data.Close();
            }
        }

        //helper message hide when mouse not on datagridview
        private void DataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && (e.ColumnIndex == 0) || e.RowIndex > -1 && (e.ColumnIndex == 3) || e.RowIndex > -1 && (e.ColumnIndex == 2))
            {
                LabelHelperSup.Visible = false;
            }    
        }
        //helper message show when mouse on datagridview
        private void DataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1 && (e.ColumnIndex == 0) || e.RowIndex > -1 && (e.ColumnIndex == 3) || e.RowIndex > -1 && (e.ColumnIndex == 2))
            {
                LabelHelperSup.Visible = true;
            }
        }
        //happen when i double click on a SN
        //show new form
        //delete  datagridview row/ delete an enregistrement
        //modified squence
        private void DataGridViewRecap_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 && (e.ColumnIndex == 2))
                {
                    //if the celule photo is not null
                    if ((string)this.dataGridViewRecap.CurrentRow.Cells[2].Value != "")
                    {
                        string pathphoto = "select epe_ressuage_photos.lien_photo" +
                            " from TBSCT.epe_ressuage_piece" +
                            " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                            " inner join TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                            $" where epe_ressuage_photos.seq={this.dataGridViewRecap.CurrentRow.Cells[3].Value} and epe_ressuage_piece.sn ='{this.dataGridViewRecap.CurrentRow.Cells[0].Value}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={this.dataGridViewRecap.CurrentRow.Cells[1].Value}";
                        OracleDataReader data = db.Request(pathphoto);
                        if (data != null || data.HasRows)
                        {
                            while (data.Read())
                            {
                                string[] path_photo = data["lien_photo"].ToString().Split('#');
                                Process Pro = new Process
                                {
                                    StartInfo = new ProcessStartInfo()
                                    {
                                        FileName = @ConfigurationManager.AppSettings["app.exe"],
                                        Arguments = @ConfigurationManager.AppSettings["ImageView"] + path_photo[0],
                                        UseShellExecute = false,
                                    }
                                };
                                Pro.Start();
                                Pro.WaitForExit(1000);
                            }
                        }
                        if (data.IsClosed == false) data.Close();
                    }
                }
                //check that sn is not traited
                else if (e.RowIndex > -1 && !checkedListBoxSnTaite.Items.Contains(this.dataGridViewRecap.CurrentRow.Cells[0].Value.ToString()))
                {
                    vld.SetSnTraite(checkedListBoxSnTaite, checkedListBoxNumSerie);
                    List<string> value = new List<string> {
                this.dataGridViewRecap.CurrentRow.Cells[0].Value.ToString(),
                this.dataGridViewRecap.CurrentRow.Cells[1].Value.ToString(),
                this.dataGridViewRecap.CurrentRow.Cells[2].Value.ToString(),
                this.dataGridViewRecap.CurrentRow.Cells[3].Value.ToString(),
                };
                    //Supprsion of sequence
                    if (e.RowIndex > -1 && (e.ColumnIndex == 0))
                    {
                        log.writeLog("suppresion d'une séquence", "historique", 0);
                        ConfirmForm form = new ConfirmForm(dataGridViewRecap, e.RowIndex, "delete", value)
                        {
                            Text = "UUM_Ressuage -> Confirmation suppression"
                        };
                        form.textBoxTitre.Text = $"                        Êtes-vous sur de vouloir supprimer la séquence {value[3]} " +
                            $"(SN: {value[0]} , numéro d'indication: {value[1]} et non photo: {value[2]}) ?";
                        form.dataGridViewConfirm.Visible = false;
                        form.Size = new Size(700, 180);
                        form.buttonNon.Location = new Point(200, 74);
                        form.buttonOui.Location = new Point(390, 74);
                        form.StartPosition = FormStartPosition.CenterScreen;
                        form.ShowDialog(); //open the confirmation form
                    }
                    //Modification de la séquence
                    else if (e.RowIndex > -1 && (e.ColumnIndex == 3))
                    {
                        if ((string)this.dataGridViewRecap.CurrentRow.Cells[2].Value != "")
                        {
                            log.writeLog("modification sequence des photos", "historique", 0);
                            ConfirmForm form = new ConfirmForm(dataGridViewRecap, e.RowIndex, "modifier")
                            {
                                StartPosition = FormStartPosition.CenterScreen,
                                Text = "UUM_Ressuage -> Modification sequence"
                            };
                            form.textBoxTitre.Text = "                                   Modification séquence d'une indication";
                            string querySeq = "select epe_ressuage_piece.sn, epe_ressuage_indice.indication, epe_ressuage_photos.lien_photo, epe_ressuage_photos.seq from TBSCT.epe_ressuage_piece " +
                                "inner join TBSCT.epe_ressuage_indice on epe_ressuage_indice.id_piece = epe_ressuage_piece.id " +
                                "inner join  TBSCT.epe_ressuage_photos on epe_ressuage_photos.id_indice = epe_ressuage_indice.id " +
                                $"where epe_ressuage_piece.sn='{this.dataGridViewRecap.CurrentRow.Cells[0].Value}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}'" +
                                $"and epe_ressuage_indice.indication={this.dataGridViewRecap.CurrentRow.Cells[1].Value}";
                            OracleDataReader data = db.Request(querySeq);
                            while (data.Read())
                            {
                                FileInfo photo_name = new FileInfo((string)data["lien_photo"]);
                                form.dataGridViewConfirm.Rows.Add(data["sn"], data["indication"], photo_name.Name, data["seq"], data["seq"]);
                            }
                            if (data.IsClosed == false) data.Close();
                            form.Show();
                        }
                    }
                }
                else
                {
                    if ((string)this.dataGridViewRecap.CurrentRow.Cells[2].Value != "")
                    {
                        MessageBox.Show($"Le SN {this.dataGridViewRecap.CurrentRow.Cells[0].Value} à été validé. Pour supprimer/modifier la sequence, cocher le SN traité puis modifier",
                            "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (e.RowIndex > -1 && (e.ColumnIndex == 0))
                        {
                            MessageBox.Show($"Le SN {this.dataGridViewRecap.CurrentRow.Cells[0].Value} à été validé. Pour supprimer la sequence, cocher le SN traité puis modifier",
                                "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }catch(Exception ex)
            {
                log.writeLog("Table arperçu : " + ex.ToString(), "log", 1);
            }
            
        }

        //create pv bouton
        private void ButtonCreerPV_Click(object sender, EventArgs e)
        {
            //if all SN was traited
            if (checkedListBoxNumSerie.Items.Count != 0 || checkedListBoxSnTaite.Items.Count == 0) MessageBox.Show("Vous devez valider tous les SN !");
            else
            {
                SnValided vld = new SnValided();
                //PvNumber pv = new PvNumber();       //instantiete this herer becuz methode call itself
                PDF pdf = new PDF();
               // GeneratePv Gpv = new GeneratePv();
               // Etiquette etq = new Etiquette();

                ArchiveFile archive = new ArchiveFile();
                DeleteFile dlt = new DeleteFile();
                try
                {
                    log.writeLog("Création du PV", "historique", 0);
                    buttonCreerPV.Visible = false;
                    //debug1 : 
                   // MessageBox.Show("CHECKPOINT 1: Début création PV", "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    vld.SetSnTraite(checkedListBoxSnTaite, checkedListBoxNumSerie);
                   // MessageBox.Show($"CHECKPOINT 2: SetSnTraite OK\nSN traités UI: {checkedListBoxSnTaite.Items.Count}", "DEBUG");
                    // pv.CheckPVFille(); // initialise pv numer and insert value into database for each sn

                    pdf.CreatePdf(checkedListBoxSnTaite);
                    //MessageBox.Show("CHECKPOINT 3: CreatePdf OK (appel terminé)", "DEBUG");
                    // fin debug 1 
                    //Gpv.GeneratePvFile(); // this generate xfd file and send email
                    //etq.PrintEtiquette(); //this print etiquette for all SN
                    vld.SetSnTraite(checkedListBoxSnTaite, checkedListBoxNumSerie);
                    //pv.CheckPVFille(); // initialise pv numer and insert value into database for each sn
                    //pdf.CreatePdf(checkedListBoxSnTaite); //this create pdf and save pdf to the .xfd directory
                    /* MessageBox.Show(
                         "DEBUG PATHS\n\n" +
                         "Environment.CurrentDirectory = " + Environment.CurrentDirectory + "\n\n" +
                         "Application.StartupPath = " + Application.StartupPath + "\n\n" +
                         "BaseDirectory = " + AppDomain.CurrentDomain.BaseDirectory + "\n\n" +
                         "arch_pdfdirectory(app.config) = " + arch_pdfdirectory,
                         "DEBUG PATHS",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information
                     );*/
                    //Gpv.GeneratePvFile(); // this generate xfd file and send email
                    //etq.PrintEtiquette(); //this print etiquette for all SN
                    var arr = vld.SortSnTraiter(checkedListBoxSnTaite);
                    /* MessageBox.Show("CHECKPOINT 4: SortSnTraiter() OK\nLength=" + (arr?.Length ?? 0) +
                                     "\nValeurs:\n" + (arr == null ? "null" : string.Join("\n", arr)),
                                     "DEBUG");*/

                    foreach (string sn in (arr ?? Array.Empty<string>()).Where(x => !string.IsNullOrWhiteSpace(x)))
                    {
                        //string sn = arr[i];
                        //MessageBox.Show($"CHECKPOINT 5: Boucle i={i}\nSN={sn}", "DEBUG");

                        // 5.1 UPDATE flag_pvcree
                        string queryflag = "UPDATE tbsct.epe_ressuage_piece" +
                            $" set epe_ressuage_piece.flag_pvcree ={1}" +
                            $" where epe_ressuage_piece.sn='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}'";

                        //MessageBox.Show("CHECKPOINT 6: queryflag:\n" + queryflag, "DEBUG");
                        db.Request(queryflag);
                        //MessageBox.Show("CHECKPOINT 7: UPDATE flag_pvcree OK", "DEBUG");

                        // 5.2 Lire lien_PDF
                        string pathPDF = "select epe_ressuage_piece.lien_PDF " +
                           $" from tbsct.epe_ressuage_piece" +
                           $" where epe_ressuage_piece.sn='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}'";

                        //MessageBox.Show("CHECKPOINT 8: pathPDF query:\n" + pathPDF, "DEBUG");

                        OracleDataReader dataPDF = db.Request(pathPDF);

                        if (!dataPDF.HasRows)
                        {
                            MessageBox.Show("CHECKPOINT 9: Aucun lien_PDF en base pour SN=" + sn, "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            while (dataPDF.Read())
                            {

                                 FileInfo pdf_name = new FileInfo((string)dataPDF["lien_PDF"]); //extract the pdf name 
                                 archive.ArchivePDF((string)dataPDF["lien_PDF"], arch_pdfdirectory, pdf_name.Name);
                              
                            }
                        }

                        if (dataPDF != null && !dataPDF.IsClosed) dataPDF.Close();

                        // 5.3 Photos query (juste vérifier que la requête marche)
                        string pathphoto = "select epe_ressuage_photos.lien_photo,epe_ressuage_indice.indication" +
                          " from TBSCT.epe_ressuage_piece" +
                          " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                          " inner join TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                          $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}'";

                        //MessageBox.Show("CHECKPOINT 13: pathphoto query:\n" + pathphoto, "DEBUG");

                        OracleDataReader dataPhoto = db.Request(pathphoto);
                        //int photoRows = 0;
                        //while (dataPhoto.Read()) photoRows++;
                        if (dataPhoto.HasRows)
                        {
                            while (dataPhoto.Read())
                            {
                                if (!string.IsNullOrEmpty(dataPhoto["lien_photo"].ToString()))
                                {
                                    string[] p_photo = dataPhoto["lien_photo"].ToString().Split('#');
                                    // p_photo[0] = chemin photo source
                                    // p_photo[1] = séquence

                                    archive.ArchivePhoto(
                                        p_photo[0],
                                        arch_photosdirectory,
                                        $"{sn}_{dataPhoto["indication"]}_{p_photo[1]}.jpg"
                                    );
                                }
                            }
                            if (!dataPhoto.IsClosed) dataPhoto.Close();
                        }

                        //MessageBox.Show("CHECKPOINT 14: rows photos=" + photoRows, "DEBUG");
                        if (!dataPhoto.IsClosed) dataPhoto.Close();

                        // 5.4 Suppression dossiers (juste tester existence)
                        string dirLampe = Path.Combine(path_dwl_photo_lampe, sn);
                        string dirMini = Path.Combine(path_dwl_photo_minilampe, sn);

                        /*MessageBox.Show("CHECKPOINT 15: Dossiers\nLampe=" + dirLampe + "\nExists=" + Directory.Exists(dirLampe) +
                                        "\nMini=" + dirMini + "\nExists=" + Directory.Exists(dirMini),
                                        "DEBUG");*/
                    }

                    
          
                    done = true;
                }
                catch (Exception ex)
                {
                    log.writeLog("CREATION DE PV : " + ex.ToString(), "log", 1);
                    done = false;
                }
                finally
                {
                    if (done)
                    {
                        MessageBox.Show($"Le Pdf a été crée avec succès", "Création du PV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Ouvrir le dossier d'archivage PDF
                        try
                        {
                            if (Directory.Exists(arch_pdfdirectory))
                            {
                                Process.Start(new ProcessStartInfo()
                                {
                                    FileName = arch_pdfdirectory,
                                    UseShellExecute = true
                                });
                            }
                            else
                            {
                                MessageBox.Show("Le dossier d'archivage PDF est introuvable.",
                                                "Erreur dossier PDF",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.writeLog("Ouverture dossier PDF échouée : " + ex.Message, "log", 1);
                        }

                        //MessageBox.Show($"Le PV {pv.GetPv()} a été crée avec succès","Création du PV",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        db.CloseDb(); //Close db connection
                        //fige les champs
                        buttonCameraDeportee.Enabled = false;
                        buttonCameraLampe.Enabled = false;
                        buttonValider.Enabled = false;
                        buttonModifier.Enabled = false;
                        linkLabelCopieSn.Enabled = false;
                        buttonTelechargerPhoto.Enabled = false;
                        dataGridViewRecap.Enabled = false;
                        checkedListBoxSnTaite.Enabled = false;
                       // log.writeLog($"Le PV {pv.GetPv()} a été crée avec succès", "historique", 0);
                        //affiche le bouton re Réimpresion d'étiquette
                        //buttonPrintEtiquette.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Vérifier la création du PV un ou Plusieurs problèmes rencontrés","Erreur création du PV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //Methode to refresh the board
        private void Refresh_()
        {
            dataGridViewRecap.Rows.Clear(); // I fisrt clear the board

            if (dataGridViewRecap.DataSource == null)
            {
                string queryData = "select epe_ressuage_piece.sn,epe_ressuage_indice.indication, epe_ressuage_photos.lien_photo,epe_ressuage_photos.seq,epe_ressuage_indice.type_indication,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.type_defaut,epe_ressuage_indice.positif_negatif_autre" +
                    " from TBSCT.epe_ressuage_piece" +
                    " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                    " inner join  TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                    $" where epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_piece.flag_PVcree={0} order by epe_ressuage_piece.sn desc, epe_ressuage_indice.indication desc, epe_ressuage_photos.seq desc";
                OracleDataReader data = db.Request(queryData);
                log.writeLog(queryData, "trace", 0);
                if (data.HasRows) //if data content one or many rows
                {
                    while (data.Read())
                    {
                        if (Convert.IsDBNull(data["lien_photo"]))
                        {
                            dataGridViewRecap.Rows.Insert(0, data["sn"], data["indication"], "", "",
                                data["type_indication"], data["taille_indice"], data["classe_indice"], data["type_defaut"], data["positif_negatif_autre"]);
                        }
                        else
                        {
                            FileInfo photo_name = new FileInfo((string)data["lien_photo"]); //extract the photo name 
                                                                                            //add items selected to datagridview from top
                            dataGridViewRecap.Rows.Insert(0, data["sn"], data["indication"], photo_name.Name, data["seq"],
                                data["type_indication"], data["taille_indice"], data["classe_indice"], data["type_defaut"], data["positif_negatif_autre"]);
                        }
                    }
                    if (data.IsClosed == false) data.Close();
                }
            }
        }
        //button to refresh the board
        private void ButtonRefreshDataGrid_Click(object sender, EventArgs e)
        {
            Refresh_();
        }
        //copy sn informations to another sn
        private void LinkLabelCopieSn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (checkedListBoxNumSerie.CheckedItems.Count == 0) return;
            else
            {
                string qnumberseq = "select epe_ressuage_photos.seq " +
                        "from TBSCT.epe_ressuage_photos " +
                        "inner join  TBSCT.epe_ressuage_indice on epe_ressuage_indice.id = epe_ressuage_photos.id_indice " +
                        "inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece " +
                        $" where epe_ressuage_piece.sn ='{checkedListBoxNumSerie.CheckedItems[0]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' ";
                OracleDataReader data = db.Request(qnumberseq);
                if (checkedListBoxNumSerie.CheckedItems.Count == 0) MessageBox.Show("Vous devez selectionner un numéro de série non traité");
                else if (data.HasRows) MessageBox.Show("Vous ne pouvez par éffectuer de copie d'un SN ayant des valeurs");
                else if (checkedListBoxSnTaite.Items.Count == 0) MessageBox.Show("Aucun numéro de série traité, veuillez valider au moins un SN");
                else
                {
                    ConfirmForm form = new ConfirmForm(checkedListBoxNumSerie, checkedListBoxSnTaite, "copie", dataGridViewRecap)
                    {
                        Text = "UUM_Ressuage -> Copie de SN"
                    };
                    form.textBoxTitre.Text = $"                      Copie des infomations d'un SN sur le SN {checkedListBoxNumSerie.CheckedItems[0]}";
                    form.dataGridViewConfirm.Visible = false;
                    form.Size = new Size(700, 280);
                    form.buttonNon.Location = new Point(200, 178);
                    form.buttonOui.Location = new Point(390, 178);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show(); //open the confirmation form
                }
            }
        }
        //download photo from the caremara 
        //create directory  loin/sn, proche/sn
        private void ButtonTelechargerPhoto_Click(object sender, EventArgs e)
        {
            bool ok = false;
            if (checkedListBoxNumSerie.CheckedItems.Count == 0) MessageBox.Show("Vous devez selectionner un numéro de série enfin d'importer des photos");
            else
            {
                try
                {
                    Directory.CreateDirectory($"{path_dwl_photo_lampe}/{checkedListBoxNumSerie.CheckedItems[0]}");  //create directory with SN selected photo lamp
                    Directory.CreateDirectory($"{path_dwl_photo_minilampe}/{checkedListBoxNumSerie.CheckedItems[0]}"); //create directory with SN selected photo minilampe
                    photo.DownloadPhotos(photolampe, path_dwl_photo_lampe, NonCamera,  checkedListBoxNumSerie.CheckedItems[0].ToString());  //download photo lampe 
                    photo.DownloadPhotos(photominilampe, path_dwl_photo_minilampe, NonCamera, checkedListBoxNumSerie.CheckedItems[0].ToString());  //download photo lampe 
                    ok = true;
                }
                catch(Exception err)
                {
                    MessageBox.Show($"Problème de téléchargement des photos \r\nVérifier la connectique et le mode de transfert", "Téléchargement des photos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult result= MessageBox.Show($"Après vérification de la connectique et le mode de transfert, Voulez-vous ressayer le téléchagment des photos?", "Téléchargement des photos", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes){
                        try
                        {
                            Directory.CreateDirectory($"{path_dwl_photo_lampe}/{checkedListBoxNumSerie.CheckedItems[0]}");  //create directory with SN selected photo lamp
                            Directory.CreateDirectory($"{path_dwl_photo_minilampe}/{checkedListBoxNumSerie.CheckedItems[0]}"); //create directory with SN selected photo minilampe
                            photo.DownloadPhotos(photolampe, path_dwl_photo_lampe, NonCamera, checkedListBoxNumSerie.CheckedItems[0].ToString());  //download photo lampe 
                            photo.DownloadPhotos(photominilampe, path_dwl_photo_minilampe, NonCamera, checkedListBoxNumSerie.CheckedItems[0].ToString());  //download photo lampe 
                            ok = true;
                        }
                        catch (Exception)
                        {
                            ok = false;
                        }
                    }
                }
                finally
                {
                    if (ok == true) MessageBox.Show("Les photos ont été téléchargées correctement");
                    else
                    {
                        log.writeLog("Problème de téléchargement des photos, impossible d'accéder aux repertoires des photos de la caméra ", "log", 1);
                        MessageBox.Show($"Le problème téléchargement des photos persiste\r\n", "Erreur téléchargement des photos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //in case there is a problem, allow operator to print etiquettes
        //private void ButtonPrintEtiquette_Click(object sender, EventArgs e)
        //{
           /* MessageBoxButtons buttons = MessageBoxButtons.YesNo;   // Initializes the variables to pass to the MessageBox.Show method.
            DialogResult result = MessageBox.Show($"Souhaitez-vous réimprimer les étiquettes ?", "Réimpression des étiquettes", buttons);
            if (result == DialogResult.Yes)
            {
                log.writeLog("Réimpression des étiquettes suites a un problème", "historique", 0);
                //Etiquette etq = new Etiquette();
                //etq.PrintEtiquette(); //this print etiquette for all SN
            }*/
       // }

        
 
        private void buttonHelpFile_Click(object sender, EventArgs e)
        {
            if (File.Exists(HelpFile))
            {
                log.writeLog("Ouverture du fichier d'aide", "trace", 0);
                Process.Start(HelpFile);

            }
            else
            {
                log.writeLog("Le fichier d'aide n'existe pas", "trace", 0);
                string text = "Le fichier d'aide n'existe pas. Contactez l'équipe Support.";
                MessageBox.Show(text);
            }
        }

        // GECHG1598219 : Contruction de la liste déroulante "Emplacement"
        private void comboBoxZoneLocal_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxEmplLocal.Items.Clear();

            string query = $"select TBSCT.epe_ressuage_emplacementlocal.des_emplacement from TBSCT.epe_ressuage_emplacementlocal " +
                   $"inner join TBSCT.epe_ressuage_produitlocal on TBSCT.epe_ressuage_produitlocal.id_produit = TBSCT.epe_ressuage_emplacementlocal.id_produit " +
                   $"inner join TBSCT.epe_ressuage_zonelocal on TBSCT.epe_ressuage_zonelocal.id_zone = TBSCT.epe_ressuage_emplacementlocal.id_zone " +
                   $"where TBSCT.epe_ressuage_produitlocal.des_produit = '{comboBoxProduitLocal.Text}'" +
                   $"and TBSCT.epe_ressuage_zonelocal.des_zone = '{comboBoxZoneLocal.Text}'";

            OracleDataReader data = db.Request(query);
            log.writeLog(query, "trace", 0);
            if (data.HasRows)
            {
                while (data.Read())
                {
                    comboBoxEmplLocal.Items.Add(data["des_emplacement"]);
                }
                if (data.IsClosed == false) data.Close();
                //resize combo box if the dropdown value is too long
                ResizeComboBox(comboBoxEmplLocal);

            }
        }

        //GECHG1598219 : Construction de la liste déroulante Zone
        private void comboBoxProduitLocal_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxZoneLocal.Items.Clear();
            comboBoxZoneLocal.Text = "";
            comboBoxEmplLocal.Items.Clear();
            comboBoxEmplLocal.Text = "";

            string query = $"select TBSCT.epe_ressuage_zonelocal.des_zone from TBSCT.epe_ressuage_zonelocal " +
                     $"inner join TBSCT.epe_ressuage_produitlocal on TBSCT.epe_ressuage_produitlocal.id_produit = TBSCT.epe_ressuage_zonelocal.id_produit " +
                     $"where TBSCT.epe_ressuage_produitlocal.des_produit = '{comboBoxProduitLocal.Text}'";

            OracleDataReader data = db.Request(query);
            log.writeLog(query, "trace", 0);
            if (data.HasRows)
            {
                while (data.Read())
                {
                    comboBoxZoneLocal.Items.Add(data["des_zone"]);
                }
                if (data.IsClosed == false) data.Close();
                //resize combo box if the dropdown value is too long
                ResizeComboBox(comboBoxZoneLocal);
                ResizeComboBox(comboBoxEmplLocal);

            }
           
        }

        private void RessuageForm_Load(object sender, EventArgs e)
        {

        }
    }
}
               