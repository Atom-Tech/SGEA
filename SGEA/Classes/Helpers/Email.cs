using SGEA.Classes;
using System.Net;
using System.Net.Mail;

namespace SGEA
{
    public static class Email
    {
        public static bool EmailValido(string mail)
        {
            try
            {
                System.Net.Mail.MailAddress mailAddress = new System.Net.Mail.MailAddress(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void EnviarEmail(string emailDestino, string nomeDestinatario, string mensagem, string assunto)
        {
            using (SmtpClient client = new SmtpClient())
            {
                Criptografar c = new Criptografar();
                string email = c.DecryptString("046156079001089037090232140150007197033172182012209013179054221188210216226127193057059197132217");
                string aSenha = c.DecryptString("127092058151252067227149015122149038038103223233");
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(email, aSenha);
                MailMessage mail = new MailMessage();
                mail.Sender = new MailAddress(email, "SGEA");
                mail.From = new MailAddress(email, "SGEA");
                mail.To.Add(new MailAddress(emailDestino, nomeDestinatario));
                mail.IsBodyHtml = true;
                mail.Subject = assunto;
                mail.Body = mensagem;
                mail.Priority = MailPriority.High;

                client.Send(mail);

            }
        }

        public static void Contato(string emailDestino, string nomeDestinatario, string mensagem, string assunto, string nome, string em)
        {
            using (SmtpClient client = new SmtpClient())
            {
                Criptografar c = new Criptografar();
                string email = c.DecryptString("046156079001089037090232140150007197033172182012209013179054221188210216226127193057059197132217");
                string aSenha = c.DecryptString("127092058151252067227149015122149038038103223233");
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(email, aSenha);
                MailMessage mail = new MailMessage
                {
                    Sender = new MailAddress(email, "SGEA"),
                    From = new MailAddress(email, "SGEA")
                };
                mail.To.Add(new MailAddress(emailDestino, nomeDestinatario));
                mail.IsBodyHtml = true;
                mail.Subject = assunto;
                mail.Body = nome + " enviou uma mensagem: <br> "+
                    mensagem + ". <br>Seu e-mail é: " + em;
                mail.Priority = MailPriority.High;
                client.Send(mail);
            }
        }
    }
}
