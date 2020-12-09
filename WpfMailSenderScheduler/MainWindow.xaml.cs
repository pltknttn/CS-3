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
    }
}
