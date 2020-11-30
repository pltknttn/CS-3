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
    /// Interaction logic for ComboBoxActionControl.xaml
    /// </summary>
    public partial class ComboBoxActionControl : UserControl
    {
        public ComboBoxActionControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ShowCaptionProperty =
            DependencyProperty.Register("ShowCaption", typeof(bool),
            typeof(ComboBoxActionControl), new UIPropertyMetadata(true));

        public bool ShowCaption
        {
            get { return (bool)GetValue(ShowCaptionProperty); }
            set
            {
                SetValue(ShowCaptionProperty, value);
            }
        }

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string),
            typeof(ComboBoxActionControl), new UIPropertyMetadata("Заголовок"));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set
            {
                SetValue(CaptionProperty, value);
            }
        }

        public static readonly DependencyProperty ShowButtonAddProperty =
            DependencyProperty.Register("ShowButtonAdd", typeof(bool),
            typeof(ComboBoxActionControl), new UIPropertyMetadata(true));

        public bool ShowButtonAdd
        {
            get { return (bool)GetValue(ShowButtonAddProperty); }
            set
            {
                SetValue(ShowButtonAddProperty, value);
            }
        }

        public static readonly DependencyProperty ShowButtonDelProperty =
            DependencyProperty.Register("ShowButtonDel", typeof(bool),
            typeof(ComboBoxActionControl), new UIPropertyMetadata(true));

        public bool ShowButtonDel
        {
            get { return (bool)GetValue(ShowButtonDelProperty); }
            set
            {
                SetValue(ShowButtonDelProperty, value);
            }
        }

        public static readonly DependencyProperty ShowButtonEditProperty =
            DependencyProperty.Register("ShowButtonEdit", typeof(bool),
            typeof(ComboBoxActionControl), new UIPropertyMetadata(true));

        public bool ShowButtonEdit
        {
            get { return (bool)GetValue(ShowButtonEditProperty); }
            set
            {
                SetValue(ShowButtonEditProperty, value);
            }
        }

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string),
            typeof(ComboBoxActionControl), new UIPropertyMetadata());

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set
            {
                SetValue(DisplayMemberPathProperty, value);
            }
        }

        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource", typeof(List<object>),
            typeof(ComboBoxActionControl), new UIPropertyMetadata());

        public List<object> ItemSource
        {
            get { return (List<object>)GetValue(ItemSourceProperty); }
            set
            {
                SetValue(ItemSourceProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(List<object>),
            typeof(ComboBoxActionControl), new UIPropertyMetadata());

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        public static readonly DependencyProperty CaptionMinWidthProperty =
            DependencyProperty.Register("CaptionMinWidth", typeof(double),
            typeof(ComboBoxActionControl), new UIPropertyMetadata(0.00));

        public double CaptionMinWidth
        {
            get { return (double)GetValue(CaptionMinWidthProperty); }
            set
            {
                SetValue(CaptionMinWidthProperty, value);
            }
        }


        public event RoutedEventHandler ButtonAddClick;
        public event RoutedEventHandler ButtonEditClick;
        public event RoutedEventHandler ButtonDelClick;

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ButtonAddClick?.Invoke(sender, e);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ButtonEditClick?.Invoke(sender, e);
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            ButtonDelClick?.Invoke(sender, e);
        }
          
    }
}
