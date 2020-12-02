using System;
using System.Collections;
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

        public static readonly DependencyProperty ShowComboBoxProperty =
            DependencyProperty.Register("ShowComboBox", typeof(bool),
            typeof(ComboBoxActionControl), new UIPropertyMetadata(true));

        public bool ShowComboBox
        {
            get { return (bool)GetValue(ShowComboBoxProperty); }
            set
            {
                SetValue(ShowComboBoxProperty, value);
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

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate",
            typeof(DataTemplate), typeof(ComboBoxActionControl),
            new UIPropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get => GetValue(ItemTemplateProperty) as DataTemplate;
            set => SetValue(ItemTemplateProperty, (DataTemplate)value);
        }

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string),
            typeof(ComboBoxActionControl), new UIPropertyMetadata(null));

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set
            {
                if (ShowComboBox)
                    SetValue(DisplayMemberPathProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register("SelectedValuePath", typeof(string),
            typeof(ComboBoxActionControl), new UIPropertyMetadata(null));

        public string SelectedValuePath
        {
            get { return (string)GetValue(SelectedValuePathProperty); }
            set
            {
                if (ShowComboBox)
                    SetValue(SelectedValuePathProperty, value);
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource",
            typeof(System.Collections.IEnumerable),
            typeof(ComboBoxActionControl),
            new PropertyMetadata(null));

        public System.Collections.IEnumerable ItemsSource
        {
            get => GetValue(ItemsSourceProperty) as IEnumerable;
            set
            {
                if (ShowComboBox) 
                    SetValue(ItemsSourceProperty, (IEnumerable)value);
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object),
            typeof(ComboBoxActionControl), new UIPropertyMetadata(null));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set
            {
                if (ShowComboBox)
                    SetValue(SelectedItemProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int),
            typeof(ComboBoxActionControl), new UIPropertyMetadata(-1));

        public int SelectedIndex
        {
            get { return ((int?)GetValue(SelectedIndexProperty)??-1); }
            set
            {
                if (ShowComboBox) 
                    SetValue(SelectedIndexProperty, value);
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

        public event SelectionChangedEventHandler ComboBoxSelectionChanged;
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxSelectionChanged?.Invoke(sender, e);
        }
    }
}
