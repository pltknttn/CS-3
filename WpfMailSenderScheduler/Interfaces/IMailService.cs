 namespace WpfMailSenderScheduler.Interfaces
{
    public interface IMailService
    {
        IMailSender GetSender(string server, int port, bool iSsl, string login, string password);
    }

    public interface IMailSender
    {
        void Send(string from, string recipient, string subject, string body, bool isBodyHtml);
    }
}
