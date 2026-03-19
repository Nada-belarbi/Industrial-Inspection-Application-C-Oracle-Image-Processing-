using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ressuage
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            LOGS log = new LOGS();
            DataBase db = new DataBase();
            log.writeLog("Lancement application ressuage ", "historique", 0);
            MES mes = new MES();

            SnValided snValided = new SnValided();

            // Check whether the environment variable exists.
            string value = Environment.GetEnvironmentVariable("DB_ROOT");
            if (value == null)
            {
                log.writeLog("Problème récupération environement d'exécution de l'application", "log", 1);
            }
            else
            {
                // set the value of db root
                db.setDB_root(value);
                db.readDBfile();
                db.Connect(); //connection to database
                // Invoke this sample with an arbitrary set of command line arguments.
                //get arguments values and set values 
                //app won't be launch if can't get all arguments parameters
                try
                {

                    mes.SetOf(args[0]);         // OF
                    mes.SetOp(args[1]);         // OP
                    mes.SetCdc(args[2]);        // CDC
                    mes.SetArticle(args[3]);    // ARTICLE
                    mes.SetOperateur(args[4]);  // USER/OPERATEUR
                    mes.SetSnList(args[5]);     // SN "123$102$..."

                    var snList = mes.GetSnList();
                    /*MessageBox.Show(
                        snList == null ? "SN = null"
                        : snList.Length == 0 ? "SN = vide (Length=0)"
                        : "SN reçus:\n- " + string.Join("\n- ", snList),
                        "DEBUG SN",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );*/

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);



                    Application.Run(new RessuageForm());
                }
                catch (IndexOutOfRangeException e)
                {
                    //message show when parameters mising
                    MessageBox.Show("Veuillez suivre correctement la procédure avant ouverture de l'application");
                    log.writeLog("Problème de recupération des arguments lors de l'exécusion de l'application depuis MES" + e.StackTrace, "log", 0);

                    ///////////////////////to be remove//////
                    ///////////////////////////// valuer de test/////////////////////////////
                    //string[] arrgs = { "00714790", "460071", "A86.1", "101T9980P001_002002", "wilfrid.Beaunes" };
                    //mes.SetOf(arrgs[0]); // argument 1 = numero OF
                    //mes.SetOp(arrgs[1]); // argument 2 = numero OP
                    //mes.SetCdc(arrgs[2]); // argument 3 = numero CDC
                    //mes.SetArticle(arrgs[3]); //argument 4 = numero Article 
                    //mes.SetOperateur(arrgs[4]); // argument 5 = nom de l'Operateur 
                    //Application.EnableVisualStyles();
                    //Application.SetCompatibleTextRenderingDefault(false);
                    //Application.Run(new RessuageForm());
                    ////////////////////////////// fin test /////////////////////////////////////////
                }

            }

        }
    }
}
