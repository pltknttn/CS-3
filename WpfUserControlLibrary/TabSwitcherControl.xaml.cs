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
        public static readonly DependencyProperty SwitchNextCommandProperty = DependencyProperty.Register("SwitchNextCommand", typeof(ICommand), typeof(TabSwitcherControl));
        public ICommand SwitchNextCommand
        {
            get { return (ICommand)GetValue(SwitchNextCommandProperty); }
            set { SetValue(SwitchNextCommandProperty, value); }
        }         

        public static readonly DependencyProperty SwitchPreviousCommandProperty = DependencyProperty.Register("SwitchPreviousCommand", typeof(ICommand), typeof(TabSwitcherControl));
        public ICommand SwitchPreviousCommand
        {
            get { return (ICommand)GetValue(SwitchPreviousCommandProperty); }
            set { SetValue(SwitchPreviousCommandProperty, value); }
        }

        public static readonly DependencyProperty SwitchCommandParameterProperty = DependencyProperty.Register("SwitchCommandParameter", typeof(object), typeof(TabSwitcherControl), new UIPropertyMetadata(null));
        public object SwitchCommandParameter
        {
            get { return (object)GetValue(SwitchCommandParameterProperty); }
            set { SetValue(SwitchCommandParameterProperty, value); }
        }


        public event RoutedEventHandler ButtonNextClick;
        public event RoutedEventHandler ButtonPreviousClick;
        
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (SwitchNextCommand != null)
            {
                var param = SwitchCommandParameter;
                if (SwitchNextCommand.CanExecute(param)) SwitchNextCommand.Execute(param);
                return;
            }
            ButtonNextClick?.Invoke(sender, e);
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (SwitchPreviousCommand != null)
            {
                var param = SwitchCommandParameter;
                if (SwitchPreviousCommand.CanExecute(param)) SwitchPreviousCommand.Execute(param);
                return;
            }
            ButtonPreviousClick?.Invoke(sender, e);
        }         
    }
}
