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
            if (AddCommand != null)
            {
                var param = AddCommandParameter;
                if (AddCommand.CanExecute(param)) AddCommand.Execute(param);
                return;
            }
            ButtonAddClick?.Invoke(sender, e);
        }

        public static readonly DependencyProperty AddCommandProperty =
               DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(ComboBoxActionControl), new UIPropertyMetadata(null));

        public ICommand AddCommand
        {
            get
            {
                return (ICommand)GetValue(AddCommandProperty);
            }
            set
            {
                SetValue(AddCommandProperty, value);
            }
        }

        public static readonly DependencyProperty AddCommandParameterProperty = DependencyProperty.Register("AddCommandParameter", typeof(object), typeof(ComboBoxActionControl));
        public object AddCommandParameter
        {
            get => (object)GetValue(AddCommandParameterProperty);
            set { SetValue(AddCommandParameterProperty, value); }
        }


        public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register("EditCommand", typeof(ICommand), typeof(ComboBoxActionControl));
        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        public static readonly DependencyProperty EditCommandParameterProperty = DependencyProperty.Register("EditCommandParameter", typeof(object), typeof(ComboBoxActionControl));
        public object EditCommandParameter
        {
            get => (object)GetValue(EditCommandParameterProperty);
            set { SetValue(EditCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty DelCommandProperty = DependencyProperty.Register("DelCommand", typeof(ICommand), typeof(ComboBoxActionControl));
        public ICommand DelCommand
        {
            get { return (ICommand)GetValue(DelCommandProperty); }
            set { SetValue(DelCommandProperty, value); }
        }

        public static readonly DependencyProperty DelCommandParameterProperty = DependencyProperty.Register("DelCommandParameter", typeof(object), typeof(ComboBoxActionControl), new UIPropertyMetadata( null));
        public object DelCommandParameter
        {
            get { return (object)GetValue(DelCommandParameterProperty); }
            set { SetValue(DelCommandParameterProperty, value); }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (EditCommand != null)
            {
                var param = EditCommandParameter ?? PART_ComboBox.SelectedItem;
                if (EditCommand.CanExecute(param)) EditCommand.Execute(param);
                return;
            }
            ButtonEditClick?.Invoke(sender, e); 
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {           
            if (DelCommand != null)
            {
                var param = DelCommandParameter ?? PART_ComboBox.SelectedItem;
                if (DelCommand.CanExecute(param)) DelCommand.Execute(param);
                return;
            }

            ButtonDelClick?.Invoke(sender, e);
        }

        public event SelectionChangedEventHandler ComboBoxSelectionChanged;
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxSelectionChanged?.Invoke(sender, e);
        }
    }
}
