using Data;
using Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

namespace TimePanelControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TimeControl : UserControl, IComparable<TimeControl>, IEquatable<TimeControl>, IComparable
    {
        #region Properties
        public TimeSpan Counter { get; set; }
        public string Duration
        {
            get
            {
                return Counter.ToString();//.ToString(@"hh\:mm\:ss");
            }
        }
        public string TaskName { get; set; }
        public TimeRow ActiveTimeRow { get; set; }

        public string DesignerCode { get; set; }
        public string MainDesignerCode { get; set; }
        public string ClientCode { get; set; }
        public string TraderCode { get; set; }
        public int CRKId { get; set; }
        public DispatcherTimer Timer { get; set; }


        public MCTMWidget.MainWindow Widget { get; set; }
        #endregion


        #region Publics
        public void StopTimer()
        {
            Timer.Stop();
            if (Widget != null)
            {
                Widget.Timer.Stop();
                Widget.Close();
            }
            ActiveTimeRow.ClientCode = ClientCode;
            ActiveTimeRow.MainDesignerCode = MainDesignerCode;
            ActiveTimeRow.TaskName = TaskName;
            ActiveTimeRow.TraderCode = TraderCode;
            ActiveTimeRow.DesignerCode = DayRegister.ActiveDesigner;
            ActiveTimeRow.End = DateTime.Now;
            ActiveTimeRow.Duration = ActiveTimeRow.End - ActiveTimeRow.Start + ActiveTimeRow.DurationAddedManually;
            DayRegister.Rows.Add(ActiveTimeRow);

            ExcelHelper.SaveLine2(
                ProductProperties.OutputFileName,
                new List<object>() { 
                        ActiveTimeRow.TaskName,
                        ActiveTimeRow.DesignerCode,
                        ActiveTimeRow.ClientCode,
                        ActiveTimeRow.TraderCode,
                        ActiveTimeRow.MainDesignerCode,
                        ActiveTimeRow.Duration.TotalMinutes.ToString(),
                        ActiveTimeRow.Start.ToString(),
                        ActiveTimeRow.End.ToString(),
                        ActiveTimeRow.DurationAddedManually.TotalMinutes.ToString()                        
                    });

            //TimerLabel.Foreground = new SolidColorBrush(Colors.White);
            //TimerLabel.Background = new SolidColorBrush(Colors.Black);
            this.Background = new SolidColorBrush(Colors.Transparent);

        }

        #endregion

        #region ctors
        public TimeControl(string name, string MainDesignerCode, string ClientCode, string TraderCode, int CRKId)
        {
            InitializeComponent();
            this.TaskName = name;
            this.DesignerCode = DayRegister.ActiveDesigner;
            this.MainDesignerCode = MainDesignerCode;
            this.ClientCode = ClientCode;
            this.TraderCode = TraderCode;
            this.CRKId = CRKId;
            WorkItemNameLabel.Content = String.Format("{0} - {1} - {2}", TaskName, ClientCode, MainDesignerCode);
            Counter = new TimeSpan();
            Timer = new DispatcherTimer();
            Timer.Stop();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Elapsed;
        }
        public TimeControl()
        {
            InitializeComponent();
            WorkItemNameLabel.Content = TaskName;
            Counter = new TimeSpan();
            Timer = new DispatcherTimer();
            Timer.Stop();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Elapsed;
        }
        #endregion

        #region privates
        void Timer_Elapsed(object sender, EventArgs e)
        {
            Counter = Counter.Add(new TimeSpan(0, 0, 1));
            TimerLabel.Content = Duration;
        }
        private void ChangeMinutes(bool isMinus)
        {
            try
            {

                var numberTimeSpan = isMinus ? -new TimeSpan(0, Convert.ToInt32(MinutesTextBox.Text), 0) : new TimeSpan(0, Convert.ToInt32(MinutesTextBox.Text), 0);
                if (this.Timer.IsEnabled != false)
                {
                    ActiveTimeRow.DurationAddedManually += numberTimeSpan;
                    ActiveTimeRow.IsAddedManually = true;
                    Counter += numberTimeSpan;
                }
                else
                {
                    var tr = new TimeRow()
                    {
                        Duration = numberTimeSpan,
                        DurationAddedManually = numberTimeSpan,
                        IsAddedManually = true,
                        Start = DateTime.Now,
                        End = DateTime.Now,
                        TaskName = this.TaskName
                    };
                    Counter += numberTimeSpan;
                    ActiveTimeRow = tr;
                    this.StopTimer();
                }

                MinutesTextBox.Text = "";
                TimerLabel.Content = Duration;
            }
            catch (Exception e)
            {
                MessageBoxResult result = MessageBox.Show("Nie wpisano ilości minut!");

                //throw;
            }
        }

        #endregion

        #region Button actions
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (DayRegister.Controls.Any(x => x.Timer.IsEnabled))
            {
                DayRegister.Controls.Single(x => x.Timer.IsEnabled).StopTimer();
            }

            DayRegister.Controls.Move(DayRegister.Controls.IndexOf(this), 0);

            Timer.Start();

            //TimerLabel.Foreground = new SolidColorBrush(Colors.Black);
            //TimerLabel.Background = new SolidColorBrush(Colors.White);
            this.Background = new SolidColorBrush(Colors.Green);


            ActiveTimeRow = new TimeRow()
            {
                Start = DateTime.Now,
                IsAddedManually = false,
                TaskName = this.TaskName,
                DesignerCode = this.DesignerCode,
                MainDesignerCode = this.MainDesignerCode,
                ClientCode = this.ClientCode,
                TraderCode = this.TraderCode
            };


            Widget = new MCTMWidget.MainWindow(Timer, TaskName, ClientCode, Counter);
            Window.GetWindow(this).WindowState = WindowState.Minimized;
            Widget.Topmost = true;
            this.Visibility = System.Windows.Visibility.Hidden;
            Widget.ShowDialog();

            this.Visibility = System.Windows.Visibility.Visible;
            Window.GetWindow(this).WindowState = WindowState.Normal;

        }

        private void AddMinutesButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeMinutes(false);
        }

        private void SubMinutesButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeMinutes(true);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DayRegister.DeacticateTask(this.TaskName);
        }
        #endregion

        #region overrides and implementations
        public override string ToString()
        {
            return String.Format("{0} - {1} - {2}", TaskName, ClientCode, MainDesignerCode);
        }
        #endregion

        public bool Equals(TimeControl other)
        {
            return this.CRKId == other.CRKId;
        }

        public int CompareTo(TimeControl other)
        {
            return -(this.CRKId - other.CRKId);
        }

        public int CompareTo(object obj)
        {
            return -(this.CRKId - (obj as TimeControl).CRKId);
        }
    }
}
