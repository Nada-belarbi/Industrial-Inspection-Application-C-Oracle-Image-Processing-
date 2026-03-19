using Oracle.ManagedDataAccess.Client;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Ressuage
{
    class DataBase
    {
        static OracleConnection con;
        OracleCommand cmd;
        OracleDataReader reader;
        private static string DB_ROOT;
        readonly Password pass = new Password(); // call the méthode to decypt password
        readonly LOGS log = new LOGS();
        string password;
        private static string chaineConnexion;
        private static string environnement;
        MailLog ml = new MailLog();
        public void setDB_root(string db_root)
        {
            DB_ROOT = db_root;
        }
        public string getDB_root()
        {
            return DB_ROOT;
        }
        
        //Lecture fichier connect_sercuredb
        public void readDBfile()
        {
            string uid;
            string host_name;
            string port;
            string service_name;

            string db_path = getDB_root();
            db_path += "/connect_securedb.db";
            try
            {
                log.writeLog("Lecture fichier connect_sercuredb", "historique",0);
                if (File.Exists(db_path))
                {
                    //treat the password line separatly
                    foreach (string line in File.ReadLines(db_path))
                    {
                        if (line.StartsWith("pwd"))
                        {
                            password = line.Trim(); //revove space
                            password = line.Remove(0,4); //removethe 4 first charatere
                            password = pass.Decrypt(password); //decript the password 
                        } 
                    }
                    var data = File
                        .ReadAllLines(db_path)
                        .Select(x => x.Split('='))
                        .Where(x => x.Length > 1)
                        .ToDictionary(x => x[0].Trim(), x => x[1]);
                    uid = data["uid"];
                    host_name = data["host_name"];
                    port = data["port"];
                    service_name = data["service_name"];
                    environnement = data["environnement"];

                    chaineConnexion = $"Data Source={host_name}:{port}/{service_name}; User Id={uid}; password={password}";
                }
                else
                {
                    log.writeLog($"Erreur lecture fichier du connect_sercuredb : {db_path} not found", "log", 1);
                }
            }
            catch(Exception ex)
            {
                log.writeLog($"Erreur lecture fichier du connect_sercuredb {ex.Message}", "log",1);
            }
            finally
            {
                ml.SetEnv(environnement);
            }
        }
        public string getEnv()
        {
            return environnement;
        }

        //Methode to get conected to data base
        public void Connect()
        {
            try
            {
                log.writeLog("Connection à la base de données", "historique",0);
                con = new OracleConnection();
                con.ConnectionString = chaineConnexion;
                con.Open();
            }
            catch(Exception ex)
            {
                log.writeLog($"problème connection à la base de données {ex.Message} {ex.Source}","log",1);
            }
        }
        //Methode to close database connection
        public void CloseDb()
        {
            con.Close();
            con.Dispose();
        }

        //methode to run sql request 
        public OracleDataReader Request(string query)
        {
            try
            {
                //Connect();
                cmd = con.CreateCommand();//create a commnand 
                cmd.CommandText = query;
                reader = cmd.ExecuteReader(); //exécute the command
            }
            catch(Exception ex)
            {
                log.writeLog($"Erreur exécution d'une requete {ex.Message} {ex.Source}","log",1);
            }

            //return sql data or message
            return reader;
        }

       
    }
}
