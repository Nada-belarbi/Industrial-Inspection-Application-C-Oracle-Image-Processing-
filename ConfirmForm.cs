using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace Ressuage
{
    public partial class ConfirmForm : Form
    {
        DataGridView Datagridview;
        CheckedListBox SNTRAITE;
        CheckedListBox SNNONTRAITE;
        DataGridView DATAGRECAP;
        readonly SnValided vld = new SnValided();
        static string ACTION;
        static List<string> VALUE;
        int Index;
        bool typeInt=true;
        string SNTO;
 
        public ConfirmForm(DataGridView datagridview, int index, string action, List<string> value)
        {
            InitializeComponent();
            setDatagridview(datagridview);
            setIndex(index);
            ACTION = action;
            VALUE = value;
        }

        public ConfirmForm(DataGridView dataGridViewRecap, int rowIndex, string action)
        {
            InitializeComponent();
            setDatagridview(dataGridViewRecap);
            setIndex(rowIndex);
            ACTION = action;
        }
        public ConfirmForm(CheckedListBox clbsnNonTraite, CheckedListBox clbsnTraite ,string action, DataGridView rcap)
        {
            InitializeComponent();
            SNTRAITE = clbsnTraite;
            SNNONTRAITE = clbsnNonTraite;
            DATAGRECAP = rcap;
            ACTION = action;
            SNTO = clbsnNonTraite.CheckedItems[0].ToString();
            checkedListBoxCopieSn.Visible = true;
            checkedListBoxCopieSn.Location = new System.Drawing.Point(235, 45);
            foreach(object sn in clbsnTraite.Items)
            {
                checkedListBoxCopieSn.Items.Add(sn);
            }
        }
        private void setDatagridview(DataGridView datag)
        {
            Datagridview = datag;
        }
        private void setIndex(int ind)
        {
            Index = ind;
        }
        private DataGridView getDatagridview()
        {
            return Datagridview;
        }
        private int getIndex()
        {
            return Index;
        }
 
        //methode to execute action from the fisrt form
        private void buttonOui_Click(object sender, EventArgs e)
        {
            if (ACTION == "delete") //button delete ressuageFrom item
            {
                DataGridView Dgridview = getDatagridview();
                int idx = getIndex();
                //remove line from the recap
                Dgridview.Rows.RemoveAt(idx);
                //delete value into database
                if (String.IsNullOrEmpty(VALUE[3])) {
                    vld.DeleteEnregistrement(VALUE[0], Convert.ToInt32(VALUE[1]),"", 0);
                }
                else vld.DeleteEnregistrement(VALUE[0], Convert.ToInt32(VALUE[1]), VALUE[2], Convert.ToInt32(VALUE[3]));

            }
            if (ACTION == "modifier") //modifie sequence
            {
                if (CheckType() == true)
                {
                    DataGridView Dgridview = getDatagridview();
                    //Check to check if Items already exist in datagridview
                    var sequence = new List<string>();
                    for (int row = 0; row < dataGridViewConfirm.Rows.Count; row++)
                    {
                        sequence.Add(dataGridViewConfirm.Rows[row].Cells["sequence"].Value.ToString());
                    }
                    bool isUnique = sequence.Distinct().Count() == sequence.Count();
                    if (isUnique)
                    {
                        for (int row = 0; row < dataGridViewConfirm.Rows.Count; row++)
                        {
                            vld.UpdateSeq(dataGridViewConfirm.Rows[row].Cells["numeroSerie"].Value.ToString(),
                            Convert.ToInt32(dataGridViewConfirm.Rows[row].Cells["numerIndication"].Value), dataGridViewConfirm.Rows[row].Cells["nomPhoto"].Value.ToString(),
                            Convert.ToInt32(dataGridViewConfirm.Rows[row].Cells["sequence"].Value), Convert.ToInt32(dataGridViewConfirm.Rows[row].Cells["seq_before"].Value), Dgridview);
                        }
                        this.Close();//close confirm form
                    }
                    else
                    {
                        MessageBox.Show("Vous dévez corriger votre modification, deux sequences ne peuvent pas avoir une même valeur");
                    }
                }
                else MessageBox.Show("Vous dévez corriger votre modification, uniquement les nombres sont acceptés");
            }
            if(ACTION == "copie")//copie sn information to another SN
            {
                if (checkedListBoxCopieSn.CheckedItems.Count == 0) MessageBox.Show("Selectioner le SN dont les informations seront copiées");
                else
                {
                    vld.CopySN(checkedListBoxCopieSn.CheckedItems[0].ToString(), SNTO, SNTRAITE, SNNONTRAITE,DATAGRECAP);
                    this.Close();//close confirm form
                }
            }
        }
        //close the forme
        private void buttonNon_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //check the value change by the user
        private bool CheckType()
        {
            try
            {
                typeInt = true;
                for (int row = 0; row < dataGridViewConfirm.Rows.Count; row++)
                {
                    Convert.ToInt32(this.dataGridViewConfirm.Rows[row].Cells["sequence"].Value);
                }
            }
            catch (Exception)
            {
                typeInt = false;
            }
            return typeInt;
        }

        //allow user to only select one SN.
        private void checkedListBoxCopieSn_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && checkedListBoxCopieSn.CheckedItems.Count > 0)
            {
                checkedListBoxCopieSn.ItemCheck -= checkedListBoxCopieSn_ItemCheck;
                checkedListBoxCopieSn.SetItemChecked(checkedListBoxCopieSn.CheckedIndices[0], false);
                checkedListBoxCopieSn.ItemCheck += checkedListBoxCopieSn_ItemCheck;
            }
        }

        private void textBoxTitre_TextChanged(object sender, EventArgs e)
        {

        }

        private void ConfirmForm_Load(object sender, EventArgs e)
        {

        }
    }
}