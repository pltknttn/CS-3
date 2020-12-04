namespace WpfMailSenderScheduler.Interfaces
{
    interface IDialogService
    {
        bool Show(string title, string msg);
        void ShowInfo(string msg);
        void ShowInfo(string title, string msg);
        void ShowWarn(string msg);
        void ShowWarn(string title, string msg) ;
        void ShowError(string msg);
        void ShowError(string title, string msg);
        void ShowQuestion(string msg);
        void ShowQuestion(string title, string msg);
    } 
}
