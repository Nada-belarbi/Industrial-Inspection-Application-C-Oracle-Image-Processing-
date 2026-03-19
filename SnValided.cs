using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ressuage
{
    //this class get the list of sn being valided and concataine to have a string 
    //ex : -sn1-sn2-sn3
    class SnValided
    {
        private static string SNTRAITE;
        readonly MES mes = new MES();
        readonly DataBase db = new DataBase();
        LOGS log = new LOGS();

        //get the list of sn for snTraite , snNonTraite
        public void SetSnTraite(CheckedListBox snTraite, CheckedListBox snNonTraite)
        {
            var listSN = snTraite.Items.Cast<object>().Aggregate(string.Empty, (current, item) => current + "-" + item.ToString()) ; // listSN = -sn1-sn2-sn3
            SNTRAITE = listSN;
        }
        //retun la list de sn valided wwith the following format : "-sn1-sn2-sn3"
        public string GetSnTraite()
        {
            return SNTRAITE;
        }
        //retun la list de sn valided wwith the following format : {"sn1","sn2","sn3"}
        public string[] SortSnTraiter(CheckedListBox checkedListBoxSnTaite)
        {
            return checkedListBoxSnTaite.Items
                .Cast<object>()
                .Select(o => o?.ToString()?.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();
        }


        //GECHG2013671 obtenir la zone a partir du sn 
        // input-> "SERIALNUMBER"
        // output-> Zone 
        public string GetZone(string snToGetZone) 
        {
            MES mes = new MES();
            String ssql = $"SELECT epe_ressuage_indice.des_zone FROM tbsct.epe_ressuage_indice WHERE id_piece = (SELECT id FROM tbsct.epe_ressuage_piece WHERE sn = '{snToGetZone}' and job_nub = '{mes.GetOf()}' and op = '{mes.GetOp()}' )";
            String Zone = "";

            OracleDataReader data = db.Request(ssql);
            if (data.Read()) Zone = data["des_zone"].ToString(); // Accède à la colonne "des_zone" et convertis la valeur en chaîne

            return Zone;
        }

        //GECHG2013671 obtenir les SN avec leur ZONE
        //Input -> "-sn1-sn2-sn3"
        //output -> sn1 : zone1
        //          sn2 : zone2
        //          sn3 : zone3
        public string GetSnWithZone(string snTraite)
        {
            SnValided snValided = new SnValided();
            string[] parts = snTraite.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            string result = "";
            foreach (string part in parts)
            {
                string zone = snValided.GetZone(part); 
                result += $"</br> {part} -> {zone} ";
            }

            return result.Trim(); 
        }


        //This methode is use for delete one field from the board and database value 
        //I need to check that when i delete a value, for a SN i have all the information
        public void DeleteEnregistrement(string sn,int indication,string photo,int seq)
        {
            try
            {
                log.writeLog("suppresion d'une séquence", "historique", 0);
                //count the nomber of sequence
                string qnumberseq = "select count(epe_ressuage_photos.seq) " +
                    "from TBSCT.epe_ressuage_photos " +
                    "inner join  TBSCT.epe_ressuage_indice on epe_ressuage_indice.id = epe_ressuage_photos.id_indice " +
                    "inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece " +
                    $" where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication={indication} ";
                OracleDataReader data = db.Request(qnumberseq);
                if (data.HasRows && data.Read()) //read the value
                {

                    if (data.GetInt64(0) < 2) //if the data is < to 2 => delete but mark sn as not done 
                    {
                        //this delete  cascade delete indice and photo
                        string qdltEn = "DELETE from TBSCT.epe_ressuage_indice " +
                            $"where indication ={indication} and id_piece = (select epe_ressuage_piece.id from tbsct.epe_ressuage_piece " +
                            $"where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}')";
                        db.Request(qdltEn);
                        log.writeLog(qdltEn, "trace", 0);

                    }
                    else //just delete photo
                    {
                        string qdltPhoto = "DELETE FROM TBSCT.epe_ressuage_photos " +
                            "where id_indice = (select epe_ressuage_photos.id_indice from tbsct.epe_ressuage_photos where id_indice in (select epe_ressuage_indice.id from TBSCT.epe_ressuage_indice " +
                            $"inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece and  epe_ressuage_piece.sn ='{sn}' " +
                            $"and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.indication = {indication} and epe_ressuage_photos.seq={seq})) and epe_ressuage_photos.seq={seq} ";                        
                        db.Request(qdltPhoto);
                    }
                    if (data.IsClosed == false) data.Close();
                }
            }catch(Exception ex)
            {
                log.writeLog(ex.ToString(), "log", 1);
                MessageBox.Show($"Problème rencontré : la sequence {seq} (sn={sn}, n° ind = {indication} et photo={photo}) n'a pas été supprimer");
            }
        }

        public void UpdateSeq(string sn, int indication, string photo, int seq , int seq_before,DataGridView dataG)
        {
            //insert modification
            string qUpdateseq = "update (select  epe_ressuage_photos.seq from TBSCT.epe_ressuage_photos " +
                "inner join  TBSCT.epe_ressuage_indice on epe_ressuage_indice.id = epe_ressuage_photos.id_indice " +
                "inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece  " +
                $"where epe_ressuage_piece.sn ='{sn}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_photos.lien_photo like '%{photo}' and epe_ressuage_photos.seq={seq_before} and epe_ressuage_indice.indication={indication}) sequence " +
                $"set sequence.seq={seq} ";
            db.Request(qUpdateseq);
            //replace the row value  the value from board 
            //1 become 2 and 2 become 1
            foreach (DataGridViewRow item in dataG.Rows)
            {
                if (item.Cells["numeroSerie"].Value.ToString()==sn && Convert.ToInt32(item.Cells["numeroIndication"].Value)==indication && item.Cells["nomPhoto"].Value.ToString()==photo && Convert.ToInt32(item.Cells["sequence"].Value)==seq_before)
                {
                    dataG.Rows[item.Index].Cells["sequence"].Value=seq;
                }
            }
        }

        //copy the value of one sn to another sn
        //sn from is the value im goimg to copy and add value sn to 
        public void CopySN(string snFrom,string snTo, CheckedListBox SNTRAITE, CheckedListBox SNNONTRAITE, DataGridView dataG)
        {
            try
            {
                log.writeLog($"copie du sn {snFrom} sur le sn {snTo}", "historique", 0);
                string qpiece = "insert into TBSCT.epe_ressuage_piece(job_nub, op, sn, cdc, operateur, article,flag_valided,date_ressuage) " +
                    $" select '{mes.GetOf()}','{mes.GetOp()}','{snTo}','{mes.GetCdc()}','{mes.GetOperateur()}','{mes.GetArticle()}','{1}' ,current_date" +
                    " from SYS.dual" +
                    $" WHERE NOT EXISTS(select* from TBSCT.epe_ressuage_piece where sn='{snTo}' and job_nub='{mes.GetOf()}' and op='{mes.GetOp()}')";
                db.Request(qpiece);
                OracleDataReader idPiece = db.Request($"select epe_ressuage_piece.id from TBSCT.epe_ressuage_piece where sn='{snTo}' and job_nub='{mes.GetOf()}' and op='{mes.GetOp()}'");
                if (idPiece.Read())
                {
                    string qgetIndice = "select epe_ressuage_indice.id, epe_ressuage_indice.INDICATION,epe_ressuage_indice.TYPE_INDICATION,epe_ressuage_indice.POSITIF_NEGATIF_AUTRE,epe_ressuage_indice.TYPE_DEFAUT,epe_ressuage_indice.TAILLE_INDICE,epe_ressuage_indice.CLASSE_INDICE,epe_ressuage_indice.COMMENTAIRE " +
                         "from tbsct.epe_ressuage_indice " +
                         "inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece "+
                         $"where(sn='{snFrom}' and job_nub='{mes.GetOf()}' and op='{mes.GetOp()}')";
                    OracleDataReader getIndice =db.Request(qgetIndice);
                    while (getIndice.Read())
                    {
                        string qsetIndice = "insert into tbsct.epe_ressuage_indice(id_piece,indication,type_indication ,positif_negatif_autre ,type_defaut ,taille_indice, classe_indice,commentaire) " +
                            $"values ({idPiece["id"]},'{getIndice["INDICATION"]}','{getIndice["type_indication"]}','{getIndice["POSITIF_NEGATIF_AUTRE"]}','{getIndice["TYPE_DEFAUT"]}','{getIndice["TAILLE_INDICE"]}','{getIndice["classe_indice"]}','{getIndice["COMMENTAIRE"]}')";
                        db.Request(qsetIndice);

                        string qidInid = "select epe_ressuage_indice.id from tbsct.epe_ressuage_indice " +
                             "inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece " +
                             $"where(sn='{snTo}' and job_nub='{mes.GetOf()}' and op='{mes.GetOp()}' and indication={getIndice["INDICATION"]})";
                        OracleDataReader idIndince = db.Request(qidInid);
                        {
                            if (idIndince.Read())
                            {
                                string qgetPhoto = "select lien_photo, seq from TBSCT.epe_ressuage_photos " +
                                    $"where id_indice={getIndice["ID"]}";
                                OracleDataReader getPhoto = db.Request(qgetPhoto);
                                while (getPhoto.Read())
                                {
                                    string qsetPhoto = "INSERT into tbsct.epe_ressuage_photos(id_indice,lien_photo,seq) " +
                                        $"values ({idIndince["ID"]},'{getPhoto["lien_photo"]}',{getPhoto["seq"]})";
                                    db.Request(qsetPhoto);
                                }
                                if (getPhoto.IsClosed == false) getPhoto.Close();
                            }
                            if (idIndince.IsClosed == false) idIndince.Close();
                        }
                    }
                    if (getIndice.IsClosed == false) getIndice.Close();
                }
                if (idPiece.IsClosed == false) idPiece.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"La copie du SN {snFrom} sur le SN {snTo} a échouée " + ex);
            }
            finally
            {
                // mark the sn as valided in data base 
                string queryflag = "UPDATE tbsct.epe_ressuage_piece" +
                    $" set epe_ressuage_piece.flag_valided ={1}" +
                    $" where epe_ressuage_piece.sn='{snTo}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}'";
                db.Request(queryflag);
                SNTRAITE.Items.Add(snTo);//Transfert the sn as traited
                SNNONTRAITE.Items.Remove(snTo); //delete the value

                MessageBox.Show($"le SN {snTo} est passé comme valide");
                Refresh_(dataG);
            }
        }
        private void Refresh_(DataGridView dataGrid)
        {
            dataGrid.Rows.Clear();

            if (dataGrid.DataSource == null)
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
                            dataGrid.Rows.Insert(0, data["sn"], data["indication"], "", "",
                                data["type_indication"], data["taille_indice"], data["classe_indice"], data["type_defaut"], data["positif_negatif_autre"]);
                        }
                        else
                        {
                            FileInfo photo_name = new FileInfo((string)data["lien_photo"]); //extract the photo name 
                                                                                            //add items selected to datagridview from top
                            dataGrid.Rows.Insert(0, data["sn"], data["indication"], photo_name.Name, data["seq"],
                                data["type_indication"], data["taille_indice"], data["classe_indice"], data["type_defaut"], data["positif_negatif_autre"]);
                        }
                    }
                    if (data.IsClosed == false) data.Close();
                }

            }
        }
    }
}
