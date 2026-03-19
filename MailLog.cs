using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ressuage
{
    class MailLog
    {
        private static string ENV;
        private static string OPERATEUR;
        public void SetEnv(string environnement)
        {
            ENV = environnement;
        }
        public void SetOprateurLog(string operateur)
        {
            OPERATEUR = operateur;
        }

        //mail subject
        public string SubjectLog()
        {
            string subject = $"[UUM_Ressuage] : Erreur Application";
            return subject;
        }

        //mail body
        public string EmailLogBody(string message)
        {
            string body = "-------------------------------------------------------------------------------- <br/> " +
                $"Application : UUM_Ressuage <br/> " +
                $"Version  : {1} <br/>" +
                $"Environnement : {ENV} <br/>" +
                $"Poste : {Environment.MachineName} <br/>" +
                $"Session Windows  : {Environment.UserName} <br/>" +
                $"Utilisateur : {OPERATEUR} <br/>" +
                $" Date : {DateTime.Now} <br/>" +
                $"-------------------------------------------------------------------------------- <br/><br/>" +
                $"Ligne : {message} <br/>";
            return body;
        }
    }
}
