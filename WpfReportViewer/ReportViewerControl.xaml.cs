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
    /// Interaction logic for ReportViewerControl.xaml
    /// </summary>
    public partial class ReportViewerControl : UserControl
    {
        public ReportViewerControl()
        {
            InitializeComponent();
            ReportViewer.Load += ReportViewerOnLoad;
            ReportViewer.ReportRefresh += ReportViewer_ReportRefresh;
        }

        private bool canRefresh = false;

        private void ReportViewer_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (canRefresh) Load();
        }

        public static readonly DependencyProperty ConnStringProperty =
            DependencyProperty.Register("ConnString", typeof(string),
            typeof(ReportViewerControl), new UIPropertyMetadata("Data Source=(LocalDb)\\MSSQLLocalDB;initial catalog=MailsAndSenders;integrated security=True;MultipleActiveResultSets=True"));

        public string ConnString
        {
            get { return (string)GetValue(ConnStringProperty); }
            set
            {
                SetValue(ConnStringProperty, value);
            }
        }

        private void Load()
        {
            canRefresh = false;

            ReportViewer.ProcessingMode = ProcessingMode.Local;


            var dsMember = new MailsAndSendersDataSet();
            var reportDataSource = new ReportDataSource();

            dsMember.BeginInit();
            reportDataSource.Name = "DataSetRecipient";
            reportDataSource.Value = dsMember.Tables[0];
            ReportViewer.LocalReport.DataSources.Add(reportDataSource);
            //ReportViewer.LocalReport.ReportPath = "ReportRecipient.rdlc";
            ReportViewer.LocalReport.ReportEmbeddedResource = "WpfReportViewer.ReportRecipient.rdlc";
            dsMember.EndInit();

            using (var con = new System.Data.SqlClient.SqlConnection(ConnString))
            {
                con.Open();
                using (var adapt = new System.Data.SqlClient.SqlDataAdapter("select * from Recipients", con))
                    adapt.Fill(dsMember, "Recipients");
                con.Close();
            }

            ReportViewer.LocalReport.DataSources.Clear();
            ReportViewer.LocalReport.DataSources.Add(reportDataSource);
            ReportViewer.RefreshReport();
            canRefresh = true;
        }
         
        private void ReportViewerOnLoad(object sender, EventArgs eventArgs)
        {
            Load();
        }
    }
}
