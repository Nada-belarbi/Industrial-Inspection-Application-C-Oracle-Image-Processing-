using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace Ressuage
{
    class DeleteFile
    {
        readonly LOGS log = new LOGS();

        //Méthode use for delete a file 
        public void deleteFile(string filedirectory) //take in argument the full path item 
        {
            try
            {
                if (File.Exists(filedirectory))
                {
                    log.writeLog("Supression de" + filedirectory, "historique", 0);
                    if (File.Exists(filedirectory))
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        File.Delete(filedirectory);
                    }
                }
            }catch(Exception ex)
            {
                log.writeLog($"Erreur suppresion d'un fichier {ex.Message} {ex.Source}", "log",1);
            }
        }
    }
}
