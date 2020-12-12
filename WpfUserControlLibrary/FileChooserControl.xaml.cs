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

namespace WpfUserControlLibrary
{
    /// <summary>
    /// Interaction logic for FileChooserControl.xaml
    /// </summary>
    public partial class FileChooserControl : UserControl
    {
        public FileChooserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register("File", typeof(string),
            typeof(FileChooserControl), new UIPropertyMetadata(null));

        public string File
        {
            get { return (string)GetValue(FileProperty); }
            set
            {
                SetValue(FileProperty, value);
            }
        }

        public static readonly DependencyProperty DefaultFileProperty =
            DependencyProperty.Register("DefaultFile", typeof(string),
            typeof(FileChooserControl), new UIPropertyMetadata(null));

        public string DefaultFile
        {
            get { return (string)GetValue(DefaultFileProperty); }
            set
            {
                SetValue(DefaultFileProperty, value);
            }
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string),
            typeof(FileChooserControl), new UIPropertyMetadata(null));

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set
            {
                SetValue(FilterProperty, value);
            }
        }

        public static readonly DependencyProperty IsSaveDialogProperty =
           DependencyProperty.Register("IsSaveDialog", typeof(bool),
           typeof(FileChooserControl), new UIPropertyMetadata(false));

        public bool IsSaveDialog
        {
            get { return (bool)GetValue(IsSaveDialogProperty); }
            set
            {
                SetValue(IsSaveDialogProperty, value);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileName = string.Empty;
            var defaultFileName = string.IsNullOrWhiteSpace(File) ? DefaultFile : File;
            var dialogResult = false;

            if (IsSaveDialog) 
                dialogResult = Dialog.ShowSaveFile(Filter, defaultFileName, out fileName);
            else 
                dialogResult = Dialog.ShowOpenFile(Filter, defaultFileName, out fileName);

            if (dialogResult)
            {
                File = fileName;
                BindingOperations.GetBindingExpression(PART_File, TextBox.TextProperty).UpdateTarget();
            }
        }
    }
}
