using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ressuage
{
    class MAIL
    {
        readonly string server = @ConfigurationManager.AppSettings["smtp_server"]; //get the sever value from app.config
        readonly string sender = @ConfigurationManager.AppSettings["mail_sender"]; //idem
 
        //methode to send email to a list of user
        public void SendEmail(string subjet, string body,  string[] recipients)
        {
            try
            {
                foreach(string recipient in recipients) //board including recipients
                {
                    MailAddress to = new MailAddress(recipient);
                    MailAddress from = new MailAddress(sender);
                    MailMessage message = new MailMessage(from, to)
                    {
                        Subject = subjet,
                        SubjectEncoding = Encoding.UTF8,
                        IsBodyHtml = true, //This will enable using HTML elements in email body
                        Body = body,
                        BodyEncoding = Encoding.UTF8
                    };
                    SmtpClient client = new SmtpClient(server);
                    client.Send(message); //Send the message.
                    client.UseDefaultCredentials = true; // to authenticate before it will send email on the client's behalf.
                }
            }
            catch(Exception)
            {
                MessageBox.Show($"Vérifier que vous êtes connecter au resseau internet (filaire), l'envoie de mail à échoué");
            }
        }
    }
}
