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
    /// Interaction logic for ScheduleActionTimeControl.xaml
    /// </summary>
    public partial class ScheduleActionTimeControl : UserControl
    {
        public ScheduleActionTimeControl()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty ActionDateTimeProperty =
            DependencyProperty.Register("ActionDateTime", typeof(DateTime?),
            typeof(ScheduleActionTimeControl), new UIPropertyMetadata(DateTime.Now));

        public DateTime? ActionDateTime
        {
            get { return (DateTime?)GetValue(ActionDateTimeProperty); }
            set
            {
                SetValue(ActionDateTimeProperty, value);
            }
        }


        public static readonly DependencyProperty ShowButtonDelProperty =
            DependencyProperty.Register("ShowButtonDel", typeof(bool),
            typeof(ScheduleActionTimeControl), new UIPropertyMetadata(true));

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
            typeof(ScheduleActionTimeControl), new UIPropertyMetadata(true));

        public bool ShowButtonEdit
        {
            get { return (bool)GetValue(ShowButtonEditProperty); }
            set
            {
                SetValue(ShowButtonEditProperty, value);
            }
        }

        public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register("EditCommand", typeof(ICommand), typeof(ScheduleActionTimeControl));
        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        public static readonly DependencyProperty EditCommandParameterProperty = DependencyProperty.Register("EditCommandParameter", typeof(object), typeof(ScheduleActionTimeControl));
        public object EditCommandParameter
        {
            get => (object)GetValue(EditCommandParameterProperty);
            set { SetValue(EditCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty DelCommandProperty = DependencyProperty.Register("DelCommand", typeof(ICommand), typeof(ScheduleActionTimeControl));
        public ICommand DelCommand
        {
            get { return (ICommand)GetValue(DelCommandProperty); }
            set { SetValue(DelCommandProperty, value); }
        }

        public static readonly DependencyProperty DelCommandParameterProperty = DependencyProperty.Register("DelCommandParameter", typeof(object), typeof(ScheduleActionTimeControl), new UIPropertyMetadata(null));
        public object DelCommandParameter
        {
            get { return (object)GetValue(DelCommandParameterProperty); }
            set { SetValue(DelCommandParameterProperty, value); }
        }

        public event RoutedEventHandler ButtonEditClick;
        public event RoutedEventHandler ButtonDelClick;

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (EditCommand != null)
            {
                var param = EditCommandParameter;
                if (EditCommand.CanExecute(param)) EditCommand.Execute(param);
                return;
            }
            ButtonEditClick?.Invoke(sender, e);
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DelCommand != null)
            {
                var param = DelCommandParameter;
                if (DelCommand.CanExecute(param)) DelCommand.Execute(param);
                return;
            }

            ButtonDelClick?.Invoke(sender, e);
        }
    }
}
