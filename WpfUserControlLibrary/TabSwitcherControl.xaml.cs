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
    /// Interaction logic for TabSwitcherControl.xaml
    /// </summary>
    public partial class TabSwitcherControl : UserControl
    {
        public TabSwitcherControl()
        {
            InitializeComponent();         
        } 
          
        public static readonly DependencyProperty ShowButtonCaptionProperty =
            DependencyProperty.Register("ShowButtonCaption", typeof(bool),
            typeof(TabSwitcherControl), new UIPropertyMetadata(true));

        public bool ShowButtonCaption
        {
            get { return (bool)GetValue(ShowButtonCaptionProperty); }
            set
            {
                SetValue(ShowButtonCaptionProperty, value); 
            }
        }

        public static readonly DependencyProperty ShowButtonPreviousProperty =
            DependencyProperty.Register("ShowButtonPrevious", typeof(bool),
            typeof(TabSwitcherControl), new UIPropertyMetadata(true));

        public bool ShowButtonPrevious
        {
            get { return (bool)GetValue(ShowButtonPreviousProperty); }
            set
            {
                SetValue(ShowButtonPreviousProperty, value); 
            }
        }

        public static readonly DependencyProperty ShowButtonNextProperty =
            DependencyProperty.Register("ShowButtonNext", typeof(bool),
            typeof(TabSwitcherControl), new UIPropertyMetadata(true));

        public bool ShowButtonNext
        {
            get { return (bool)GetValue(ShowButtonNextProperty); }
            set
            {
                SetValue(ShowButtonNextProperty, value);
            }
        }  
        
        public event RoutedEventHandler ButtonNextClick;
        public event RoutedEventHandler ButtonPreviousClick;
        
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            ButtonNextClick?.Invoke(sender, e);
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            ButtonPreviousClick?.Invoke(sender, e);
        }         
    }
}
