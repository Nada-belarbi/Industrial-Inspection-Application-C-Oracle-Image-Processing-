using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using System;

namespace Ressuage
{
    class MES
    {
        private static string OP;
        private static string OF;
        private static string CDC;
        private static string OPERATEUR;
        private static string ARTICLE;
        private static string LCC_MES;
        private static string DESC_CDC;
        private static string DESC_OP;
        private static string ITEM_DESC ;
        private static string[] SN_LIST = new string[] { };
        readonly DataBase db = new DataBase(); //call database claasse and instantiete
        readonly LOGS log = new LOGS(); //call log class and instentiete
        

        //the setter value are from argument passed on argument while launching the app 
        //see Program.cs
        public void SetOp(string  op)
        {
            OP = op;
        }
        public string GetOp()
        {
            return OP;
        }
        public void SetOf(string of)
        {
            OF = of;
        }
        public string GetOf()
        {
            return OF;
        }
        public void SetCdc(string cdc)
        {
            CDC = cdc;
        }

        public string GetCdc()
        {
            return CDC;
        }
        public void SetOperateur(string opt)
        {
            OPERATEUR = opt;
        }

        public string GetOperateur()
        {
            return OPERATEUR;
        }
        public void SetArticle(string art)
        {
            ARTICLE = art;
        }

        public string GetArticle()
        {
            return ARTICLE;
        }
        //get from database type indication 
        public OracleDataReader GetTypeIndication(string query)
        {
            OracleDataReader data = db.Request(query);
            log.writeLog(query,"trace",0);
            return data;
        }

        public OracleDataReader GetClasseType(string query)
        {
            OracleDataReader data = db.Request(query);
            log.writeLog(query, "trace",0);
            return data;
        }

        public OracleDataReader GetCaracterisation_indPN(string query)
        {
            OracleDataReader data = db.Request(query);
            log.writeLog(query, "trace",0);
            return data;
        }

        public OracleDataReader GetTypeDefaut(string query)
        {
            OracleDataReader data = db.Request(query);
            log.writeLog(query, "trace",0);
            return data;
        }
        //get the all SN with default
        public OracleDataReader GetNumeroSerie(string query)
        {
            OracleDataReader data = db.Request(query);
            log.writeLog(query, "trace",0);
            return data;
        }
       
        public string GetLIBELLECOURTCONSTAT()
        {
            return LCC_MES;
        }

        //GECHG1598219 : Description produit local
        public OracleDataReader GetProduitLocal(string query)
        {
            OracleDataReader data = db.Request(query);
            log.writeLog(query, "trace", 0);
            return data;
        }

        public void SetDescriptionCdc()
        {
            OracleDataReader data = db.Request($"select description from TBSCT.EPE_CDCTOEQUIPEMENT where TBSCT.EPE_CDCTOEQUIPEMENT.machine='{GetCdc()}'");
            if (data.HasRows && data.Read())
            {
                DESC_CDC = (string)data["description"];
                if (data.IsClosed == false)
                {
                    data.Close();
                }
            }
        }
        public string GetDescriptionCdc()
        {
            return DESC_CDC;
        }

        public void SetSnList(string snRaw)
        {
            if (string.IsNullOrWhiteSpace(snRaw))
            {
                SN_LIST = new string[] { };
                return;
            }

            SN_LIST = snRaw
                .Split(new[] { '$' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => s.Length > 0)
                .Distinct()
                .ToArray();
        }

        public string[] GetSnList()
        {
            return SN_LIST;
        }

    }
}
