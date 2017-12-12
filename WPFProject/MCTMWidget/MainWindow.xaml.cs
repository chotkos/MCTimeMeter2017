using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace MCTMWidget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string TaskName { get; set; }
        public string ClientName { get; set; }
        public DispatcherTimer Timer { get; set; }
        public TimeSpan NewTime { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(DispatcherTimer lastTimer, string taskName, string clientName,TimeSpan newTimeSpan)
        {
            InitializeComponent();
            Timer = lastTimer;
            TaskName = taskName;
            ClientName = clientName;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;
            ProjectName.Content = ClientName;

            //this.Top =  0;
            
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            //double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenWidth  = System.Windows.SystemParameters.WorkArea.Width;
            //double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height;
            this.Left = (int)(screenWidth -  this.Width);
            this.Top = (int)(screenHeight - this.Height);




            NewTime = newTimeSpan;
            Timer.Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            NewTime=NewTime.Add(new TimeSpan(0, 0, 1));
            Infos.Content = TaskName + " - " +NewTime;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = true;
            this.Activate();
        }
    }
}
