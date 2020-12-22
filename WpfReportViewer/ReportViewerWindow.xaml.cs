using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
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
using WpfReportViewer.MailsAndSendersDataSetTableAdapters;

namespace WpfReportViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ReportViewerWindow : Window
    {
        public ReportViewerWindow()
        {
            InitializeComponent();
            ReportViewer.Load += ReportViewerOnLoad;
        }

        private void ReportViewerOnLoad(object sender, EventArgs eventArgs)
        {
            ReportViewer.ProcessingMode = ProcessingMode.Local;

            //var reportDataSource = new ReportDataSource(); 
            //var dataset = new MailsAndSendersDataSet(); 

            //dataset.BeginInit();
            //reportDataSource.Name = "DataSetRecipient";
            //reportDataSource.Value = dataset.Recipients;
            //ReportViewer.LocalReport.DataSources.Add(reportDataSource);
            //ReportViewer.LocalReport.ReportPath = "../../ReportRecipient.rdlc";
            //dataset.EndInit();
             

            //var recipientsTableAdapter = new RecipientsTableAdapter { ClearBeforeFill = true };
            //recipientsTableAdapter.Fill(dataset.Recipients);
            //ReportViewer.RefreshReport();
               
            var dsMember = new MailsAndSendersDataSet(); 
            var reportDataSource = new ReportDataSource();

            dsMember.BeginInit();
            reportDataSource.Name = "DataSetRecipient";
            reportDataSource.Value = dsMember.Tables[0];
            ReportViewer.LocalReport.DataSources.Add(reportDataSource);
            ReportViewer.LocalReport.ReportPath = "../../ReportRecipient.rdlc";
            dsMember.EndInit();

            const string ConStr = "Data Source=(LocalDb)\\MSSQLLocalDB;initial catalog=MailsAndSenders;integrated security=True;MultipleActiveResultSets=True";
            using (var con = new System.Data.SqlClient.SqlConnection(ConStr))
            {
                con.Open();
                using (var adapt = new System.Data.SqlClient.SqlDataAdapter("select * from Recipients", con))
                    adapt.Fill(dsMember, "Recipients");
                con.Close();
            }
             
            ReportViewer.LocalReport.DataSources.Clear(); 
            ReportViewer.LocalReport.DataSources.Add(reportDataSource);
            ReportViewer.RefreshReport();  
        }
    }
}
