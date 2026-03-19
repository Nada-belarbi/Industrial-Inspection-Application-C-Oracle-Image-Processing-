using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ressuage
{
    //class use to archive any file
    //to archive a file you will have to give actuel location and  destination location and how you want to name the file
    class ArchiveFile
    {
        readonly LOGS log = new LOGS();
        readonly DeleteFile delete = new DeleteFile();

        public void ArchivePDF(string actuelfilepath,string newfilepath, string codification)
        {
            try
            {
                if (File.Exists(actuelfilepath)) //check if the file we want to delete exist
                {
                    log.writeLog("Archivage fichier " + codification, "historique", 0);
                    string destFile = Path.Combine(newfilepath, codification); //define file location and name
                    Directory.CreateDirectory(newfilepath);// To copy a folder's contents to a new location:// Create a new target folder.// If the directory already exists, this method does not create a new directory.

                    File.Copy(actuelfilepath, destFile, true);// To copy a file to another location and  overwrite the destination file if it already exists.
                                                                                                       
                    delete.deleteFile(actuelfilepath);//delete file archived 
                }
            }
            catch(Exception ex)   
            {
                log.writeLog($"Problème rencontrer au moment de l'archiavage dun PDF {ex.Message} {ex.Source}",  "log",1);
            }
               
        }

        public void ArchivePhoto(string actuelfilepath, string newfilepath, string codification)
        {
            try
            {
                if (File.Exists(actuelfilepath))
                {
                    log.writeLog("Archivage fichier " + codification, "historique", 0);
                    string destFile = Path.Combine(newfilepath, codification); //define file location and name
                    Directory.CreateDirectory(newfilepath);// To copy a folder's contents to a new location:// Create a new target folder.// If the directory already exists, this method does not create a new directory.
                    File.Copy(actuelfilepath, destFile, true);// To copy a file to another location and  overwrite the destination file if it already exists.
                }
            }
            catch (Exception ex)
            {
                log.writeLog($"Problème rencontrer au moment de l'archiavage des photos {ex.Message} {ex.Source}", "log", 1);
            }
        }
    }
}
