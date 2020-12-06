using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfMailSenderLibrary;
using WpfMailSenderScheduler.Data;
using WpfMailSenderScheduler.Interfaces;
using WpfMailSenderScheduler.Models;
using WpfUserControlLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace WpfMailSenderScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {            
            var body = MessageBody.Text;
            var subject = "Тест";

            if (string.IsNullOrWhiteSpace(body))
            {
                Tabs.SelectedItem = TabMailEditor;
                BodyEditor.Text = body;
                SubjectEditor.Text = subject;
                return;
            }
            
            var toAddr = "Здесь куда";
            var domain = "smtp-server";
            var login = "авторизация логин";
            var password = "авторизаци  пароль";
            var port = 587;
            var services = new EmailSendService
            {
                SendException = (msg) => Dialog.ShowException(msg)
            };
            services.SendMessage(login, password, domain, port, toAddr, subject, body); 
        }

        //private void GotoScheduler_Click(object sender, RoutedEventArgs e)
        //{
        //    Tabs.SelectedItem = tabScheduler;
        //}

        //private void SelectSender_ButtonAddClick(object sender, RoutedEventArgs e)
        //{
        //    App.Services.GetService<IDialogService>().ShowInfo("Нажата кнопка добавить отправителя");
        //    //Dialog.ShowInformation("Нажата кнопка добавить отправителя");
        //}

        //private void SelectSender_ButtonDelClick(object sender, RoutedEventArgs e)
        //{
        //    Dialog.ShowInformation("Нажата кнопка удалить отправителя");
        //}

        //private void SelectSender_ButtonEditClick(object sender, RoutedEventArgs e)
        //{
        //    Dialog.ShowInformation("Нажата кнопка редактировать отправителя");
        //}

        //private void SelectServer_ButtonAddClick(object sender, RoutedEventArgs e)
        //{
        //    Dialog.ShowInformation("Нажата кнопка добавить smtp-сервер");
        //}

        //private void SelectServer_ButtonEditClick(object sender, RoutedEventArgs e)
        //{
        //    Dialog.ShowInformation("Нажата кнопка редактировать smtp-сервер");
        //}

        //private void SelectServer_ButtonDelClick(object sender, RoutedEventArgs e)
        //{
        //    Dialog.ShowInformation("Нажата кнопка удалить smtp-сервер");             
        //}

        //private string GetPlainBody()
        //{
        //    try
        //    {
        //        var range = new TextRange(BodyEditor.Document.ContentStart, BodyEditor.Document.ContentEnd);
        //        var bodyTxt = string.Empty;
        //        using (var stream = new MemoryStream())                
        //        {
        //            range.Save(stream, DataFormats.Text);
        //            stream.Position = 0;
        //            using (var r = new StreamReader(stream))
        //            {
        //                bodyTxt = r.ReadToEnd();
        //                r.Close();
        //            }
        //            stream.Close();
        //        }
        //        return bodyTxt;
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        //private void SaveLetter_Click(object sender, RoutedEventArgs e)
        //{
        //    var subject = SubjectEditor.Text;
        //    if (string.IsNullOrWhiteSpace(subject))
        //    {
        //        Dialog.ShowException("Заполните тему письма!");
        //        return;
        //    } 
        //    var body = BodyEditor.Text;//по умолчанию сохраняет в rtf
        //    if (string.IsNullOrWhiteSpace(body) || string.IsNullOrWhiteSpace(GetPlainBody()))
        //    {
        //        Dialog.ShowException("Заполните тело письма!");
        //        return;
        //    }
        //    /*сделано все исключительно как demo, просто что вот добавили, изменили*/
        //    var list = MessageList.ItemsSource.Cast<Message>().ToList() ?? new List<Message>();
        //    var current = list.FirstOrDefault(w=>w.Id == ((MessageList.SelectedItem as Message)?.Id??0))?? new Message();

        //    current.Subject = subject;
        //    current.Body = body;
            
        //    if (current.Id == 0)
        //    {                
        //        current.Id = list.Max(m => m.Id) + 1;
        //    }
        //    //так как статик сделали
        //    MessageList.ItemsSource = list;
        //    BindingOperations.GetBindingExpression(MessageList, ListBox.ItemsSourceProperty)?.UpdateTarget();
        //    MessageList.SelectedItem = current;
        //    Dialog.ShowInformation("Все прошло успешно!");
        //}

        //private void SelectAddress_ButtonAddClick(object sender, RoutedEventArgs e)
        //{
        //    Dialog.ShowInformation("Нажата кнопка добавить адрес");
        //}

        //private void SelectAddress_ButtonDelClick(object sender, RoutedEventArgs e)
        //{
        //    Dialog.ShowInformation("Нажата кнопка удалить адрес");
        //}

        //private void SelectAddress_ButtonEditClick(object sender, RoutedEventArgs e)
        //{
        //    Dialog.ShowInformation("Нажата кнопка редактировать адрес");
        //}

        //private void SwitchNextPrevios_ButtonPreviousClick(object sender, RoutedEventArgs e)
        //{
        //    var count = dgEmails.Items?.Count??0;
        //    if (count == 0)
        //    {
        //        Dialog.ShowInformation("Нет данных в таблице");
        //        return;
        //    }
        //    dgEmails.Focus();

        //    if (dgEmails.SelectedIndex > 0)
        //        dgEmails.SelectedIndex -= 1;
        //    else
        //        dgEmails.SelectedIndex = count - 1;
        //}

        //private void SwitchNextPrevios_ButtonNextClick(object sender, RoutedEventArgs e)
        //{
        //    var count = dgEmails.Items?.Count ?? 0;
        //    if (count == 0)
        //    {
        //        Dialog.ShowInformation("Нет данных в таблице");
        //        return;
        //    }
        //    dgEmails.Focus();

        //    if (dgEmails.SelectedIndex < dgEmails.Items.Count - 1)
        //        dgEmails.SelectedIndex += 1;
        //    else
        //        dgEmails.SelectedIndex = 0; 
        //}

        //private void SaveTask_Click(object sender, RoutedEventArgs e)
        //{
        //    Dialog.ShowInformation("Нажата кнопка запланировать задание");
        //}

        //private void UndoTask_Click(object sender, RoutedEventArgs e)
        //{
        //    Dialog.ShowInformation("Нажата кнопка отменить задание");
        //}
    }
}
