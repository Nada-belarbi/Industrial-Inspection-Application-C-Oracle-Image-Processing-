using iTextSharp.text;
using iTextSharp.text.pdf;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Ressuage
{
    class PDF
    {
        readonly MES mes = new MES();
        readonly LOGS log = new LOGS();
        readonly DataBase db = new DataBase();
        readonly string pdfdirectory = @ConfigurationManager.AppSettings["pdf_directory"]; //get the pdf directory from app.config
        readonly string lienBox_archivage_photo = @ConfigurationManager.AppSettings["lienBox_archivage_photo"]; //idem
        readonly string arch_pdfdirectory = @ConfigurationManager.AppSettings["archivage_pdf"]; //idem
        //readonly string PV__directory = @ConfigurationManager.AppSettings["PV_generate_directory"]; //get the directory of the place the xfd fille will be createfrom app.config
        
        public void CreatePdf(CheckedListBox snTraite)
        {
            //create one pdf for one sn and save in a directory and directory path into database
            for (int i = 0; i < snTraite.Items.Count; i++)
            {
                try
                {
                    log.writeLog("Creation du PDF "+ (string)snTraite.Items[i], "historique", 0);
                    //string codification = "/" + pv.GetPv() + "_" + (string)snTraite.Items[i] + ".pdf";
                    string codification = "/" + mes.GetOf() + "_" + mes.GetOp() + "_" + (string)snTraite.Items[i] + ".pdf";

                    string pdfdir = pdfdirectory +codification;
                    Document pdf = new Document(PageSize.A4); //Create a iTextSharp.text.Document object
                    PdfWriter.GetInstance(pdf, new FileStream(pdfdir, FileMode.Create)); //create the pdf if not exist
                    pdf.Open();

                    //add aube informations
                    //pdf.Add(new Paragraph("PV : " + pv.GetPv()));
                    pdf.Add(new Paragraph("OF : " + mes.GetOf()));
                    pdf.Add(new Paragraph("OP : " + mes.GetOp()));
                    pdf.Add(new Paragraph("CDC : " + mes.GetCdc()));
                    pdf.Add(new Paragraph("Article : " + mes.GetArticle()));
                    pdf.Add(new Paragraph("Opérateur : " + mes.GetOperateur()));
                    pdf.Add(new Paragraph("Date : " + DateTime.Now));

                    //add archive directory path of picture and pdf 
                    Paragraph a_pdf = new Paragraph("Lien PDF : " + arch_pdfdirectory)
                    {
                        Alignment = Element.ALIGN_RIGHT
                    };
                    pdf.Add(a_pdf);
                    Paragraph a_photo = new Paragraph("Lien photos : " + lienBox_archivage_photo)
                    {
                        Alignment = Element.ALIGN_RIGHT
                    };
                    pdf.Add(a_photo);

                    //add separator line
                    pdf.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1))));
                    pdf.Add(new Paragraph(" ")); //add space

                    //add sn 
                    Paragraph sn = new Paragraph("Numéro de série : " + (string)snTraite.Items[i])
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    pdf.Add(sn);

                    //add nombre indication
                    string queryNb_ind = "select max(epe_ressuage_indice.indication) nb_ind from tbsct.epe_ressuage_indice " +
                        " inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                        $" where (epe_ressuage_piece.sn = '{(string)snTraite.Items[i]}'and epe_ressuage_piece.op ='{mes.GetOp()}' and epe_ressuage_piece.job_nub ='{mes.GetOf()}') order by epe_ressuage_indice.indication ";
                    OracleDataReader dataNb_ind = db.Request(queryNb_ind);
                    while (dataNb_ind.Read())
                    {
                        Paragraph nb_ind = new Paragraph("Nombre indication(s) : " + dataNb_ind["nb_ind"])
                        {
                            Alignment = Element.ALIGN_CENTER
                        }; //nombre indication
                        pdf.Add(nb_ind);
                        //add indication
                        string queryindiation ="select epe_ressuage_indice.indication" +
                            " from TBSCT.epe_ressuage_indice" +
                            " inner join TBSCT.epe_ressuage_piece on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                            $" where epe_ressuage_piece.sn ='{(string)snTraite.Items[i]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' "+
                            " ORDER BY epe_ressuage_indice.indication ASC";
                        OracleDataReader dataInd = db.Request(queryindiation);
                        //for each indication add picture and informations
                        while (dataInd.Read())
                        {
                            pdf.Add(new Paragraph(" ")); //add space
                            pdf.Add(new Paragraph("Numéro indication : " + dataInd["indication"])); //add indication

                            //GECHG1598219 : Ajouter les nouveaux champs dans le fichier PDF
                            //GECHG1598219.Old
                            //string queryInfo = "select epe_ressuage_indice.type_indication,epe_ressuage_indice.positif_negatif_autre,epe_ressuage_indice.type_defaut,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.commentaire" +
                            //  " from TBSCT.epe_ressuage_piece" +
                            //  " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                            //  $" where epe_ressuage_piece.sn ='{(string)snTraite.Items[i]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.Indication={dataInd["indication"]}";
                            
                            //GECHG1598219.New
                            string queryInfo = "select epe_ressuage_indice.type_indication,epe_ressuage_indice.positif_negatif_autre,epe_ressuage_indice.type_defaut,epe_ressuage_indice.taille_indice,epe_ressuage_indice.classe_indice,epe_ressuage_indice.commentaire,epe_ressuage_indice.des_produit,epe_ressuage_indice.des_zone,epe_ressuage_indice.des_emplacement" +
                             " from TBSCT.epe_ressuage_piece" +
                             " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                             $" where epe_ressuage_piece.sn ='{(string)snTraite.Items[i]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.Indication={dataInd["indication"]}";

                            OracleDataReader dataInfo = db.Request(queryInfo);
                            //add information regarding an indice
                            pdf.Add(new Paragraph(" ")); //add space
                           
                            while (dataInfo.Read())
                            {
                                pdf.Add(new Paragraph("         Type indication : " + dataInfo["type_indication"]));
                                pdf.Add(new Paragraph("         Caractérisation de l'indication : " + dataInfo["positif_negatif_autre"]));
                                pdf.Add(new Paragraph("         Type de défaut : " + dataInfo["type_defaut"]));
                                if (dataInfo.IsDBNull(3)) //if taille is null
                                {
                                    pdf.Add(new Paragraph("         Classe : " + dataInfo["classe_indice"]));
                                }else
                                {
                                    pdf.Add(new Paragraph("         Taille (mm): " + dataInfo["taille_indice"]));
                                }
                                //GECHG1598219.New
                                pdf.Add(new Paragraph("         Produit : " + dataInfo["des_produit"]));
                                pdf.Add(new Paragraph("         Zone : " + dataInfo["des_zone"]));
                                pdf.Add(new Paragraph("         Emplacement : " + dataInfo["des_emplacement"]));

                                pdf.Add(new Paragraph("         Commentaire : " + dataInfo["commentaire"]));
                            }
                            if (dataInfo.IsClosed == false) dataInfo.Close();
                            string queryphoto = "select epe_ressuage_photos.lien_photo" +
                               " from TBSCT.epe_ressuage_piece" +
                               " inner join TBSCT.epe_ressuage_indice on epe_ressuage_piece.id = epe_ressuage_indice.id_piece" +
                               " inner join  TBSCT.epe_ressuage_photos on epe_ressuage_indice.id = epe_ressuage_photos.id_indice" +
                               $" where epe_ressuage_piece.sn ='{(string)snTraite.Items[i]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}' and epe_ressuage_indice.Indication={dataInd["indication"]} " +
                               " ORDER BY epe_ressuage_photos.seq ASC ";
                            OracleDataReader dataPhoto = db.Request(queryphoto);
                            //add pictures
                            if (dataPhoto != null)
                            {
                                while (dataPhoto.Read())
                                {
                                    if (!Convert.IsDBNull(dataPhoto["lien_photo"])) //si pas lien de photo
                                    {
                                        pdf.Add(new Paragraph(" ")); //add space
                                        string[] path_photo = dataPhoto["lien_photo"].ToString().Split('#'); //revove the part with index
                                        iTextSharp.text.Image ItexPhoto;
                                        string file_format = path_photo[0].Substring(path_photo[0].LastIndexOf('.') + 1);

                                        //resize the size of the image
                                        Bitmap imgbitmap = new Bitmap(System.Drawing.Image.FromFile(path_photo[0]));
                                        if (imgbitmap.Width > 1280 && imgbitmap.Height > 720 || imgbitmap.Width > 720 && imgbitmap.Height > 1280)
                                        {
                                            int div = RatioImage(GetMax(imgbitmap.Width, imgbitmap.Height));
                                            System.Drawing.Image resizedImage = new Bitmap(imgbitmap, new Size(imgbitmap.Width / div, imgbitmap.Height / div));
                                            ItexPhoto = iTextSharp.text.Image.GetInstance(resizedImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            resizedImage.Dispose();
                                            ItexPhoto.ScalePercent(30);  //scale the picture photo
                                        }
                                        else
                                        {
                                            System.Drawing.Image resizedImage = new Bitmap(imgbitmap, new Size(imgbitmap.Width, imgbitmap.Height));
                                            ItexPhoto = iTextSharp.text.Image.GetInstance(resizedImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            resizedImage.Dispose();
                                            ItexPhoto.ScalePercent(30);  //scale the picture photo
                                        }
                                        //ItexPhoto = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromFile(path_photo[0]), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        //ItexPhoto.ScalePercent(7);  //scale the picture photo
                                        ItexPhoto.Alignment = Element.ALIGN_JUSTIFIED;
                                        pdf.Add(ItexPhoto); //add photo
                                        pdf.Add(new Paragraph(" ")); //add space
                                    }
                                }
                            }
                            if (dataPhoto.IsClosed == false) dataPhoto.Close();
                            pdf.NewPage();
                        }
                    }
                    if (dataNb_ind.IsClosed == false) dataNb_ind.Close();
                    pdf.Close();

                    //store the pdf path do database 
                    string queryUpdateLienpdf = "UPDATE tbsct.epe_ressuage_piece" +
                        $" set epe_ressuage_piece.lien_pdf ='{pdfdir}'" +
                        $" where epe_ressuage_piece.sn='{(string)snTraite.Items[i]}' and epe_ressuage_piece.op='{mes.GetOp()}' and epe_ressuage_piece.job_nub='{mes.GetOf()}'";
                        db.Request(queryUpdateLienpdf);
                    //copy the pdf to RP/BAAN directory
                    //File.Copy(pdfdir, (PV__directory+codification), true); //copy the template file to another direction
                }
                catch (Exception ex)
                {
                    log.writeLog(ex.ToString(), "log", 1);
                }
            }
        }
        private static int GetMax(int width, int hight)
        {
            return width > hight ? width : hight;
        }
        private static int RatioImage(int maxVal)
        {
            int refVal = 1280;
            return (maxVal / refVal) + 1;
        }
    }
}
