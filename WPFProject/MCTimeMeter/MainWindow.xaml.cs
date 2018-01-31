using Data;
using Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
using System.Windows.Threading;
using TimePanelControl;

namespace MCTimeMeter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        public ILog Log = LogManager.GetLogger(typeof(XMLHelper));
        public MainWindow()
        {
            InitializeComponent();
            this.Width = 200;
            StopButton.Visibility = Visibility.Hidden;

            TaskComboBox.Visibility = Visibility.Hidden;
            AddTaskButton.Visibility = Visibility.Hidden;
            /*
            NewTaskClientNameTextBox.Visibility = Visibility.Hidden;
            NewTaskNameTextBox.Visibility = Visibility.Hidden;
            AddTaskByNameButton.Visibility = Visibility.Hidden;
             */
            TimersList.Visibility = Visibility.Hidden;
            TimersScroll.Visibility = Visibility.Hidden;
            DayRegister.InactiveControls.CollectionChanged += (e, v) => ReloadComboBox();

            try
            {

                var tasks = ExcelHelper.GetTasks3(ProductProperties.InputFileName);
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Tick += timer_Tick;
                timer.Start();


                foreach (var item in tasks)
                {
                    DayRegister.AddUnactiveTask(item[0], item[1], item[2], item[3], Convert.ToInt32(item[5]));
                }

            }
            catch (Exception ex)
            {
                Log.Fatal("Fatal error on loading tasks from excel\t" + ex.ToString());
                throw ex;
            }

            DayRegister.SortInactives();

            if (Helpers.ExcelHelper.DoFileExist(ProductProperties.LastSessionFileName))
            {
                try
                {

                    var lastSessionTasks = XMLHelper.ReadXML(Data.ProductProperties.LastSessionFileName);
                    foreach (var item in lastSessionTasks)
                    {
                        if (DayRegister.InactiveControls.Any(x => x.TaskName == item[0]))
                        {
                            DayRegister.ActivateTask(DayRegister.InactiveControls.Single(x => x.TaskName == item[0]));
                        }
                        else
                        {
                            var x = new TimeControl(item[0], item[1], item[2], item[3], Convert.ToInt32(item[4]));
                            DayRegister.InactiveControls.Add(x);
                            DayRegister.ActivateTask(x);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Log.Fatal("Fatal error on loading last session from xml\t" + ex.ToString());
                    throw ex;
                }

            }

            foreach (var item in DayRegister.Controls)
            {
                item.Width = 0;
            }

            DayRegister.Controls.CollectionChanged += (e, v) => ReloadTimerList();
            DayRegister.Controls.CollectionChanged += (e, v) => ReloadComboBox();
            DayRegister.SortInactives();

            try
            {

                if (ExcelHelper.DoFileExist(ProductProperties.LastLoggedUserFile))
                {
                    UserNameTextBox.Text = XMLHelper.ReadXMLString(ProductProperties.LastLoggedUserFile);
                }

            }
            catch (Exception ex)
            {
                Log.Fatal("Fatal error on loading last logged user from xml\t" + ex.ToString());
                throw ex;
            }

            ReloadTimerList();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (DayRegister.Controls.Any(x => x.Timer.IsEnabled))
            {
                StopButton.Background = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                StopButton.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private void ReloadComboBox()
        {
            var items = DayRegister.InactiveControls.Select(x => x.ToString()).ToList();
            TaskComboBox.ItemsSource = items;
        }

        public void ReloadTimerList()
        {
            TimersList.Children.Clear();
            foreach (var item in DayRegister.Controls)
            {
                TimersList.Children.Add(item);
            }
            if (DayRegister.Controls.Count() == 0)
            {
                this.Width = 200;
                StopButton.Visibility = Visibility.Hidden;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (DayRegister.Controls.Any(_ => _.Timer.IsEnabled))
            {
                DayRegister.Controls.Single(_ => _.Timer.IsEnabled).StopTimer();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (null != TaskComboBox.SelectedValue && string.Empty != ((string)TaskComboBox.SelectedValue).Trim())
            {
                DayRegister.SortInactives();
                var list = DayRegister.InactiveControls.ToList();
                //list.Sort();
                var item = list.ElementAt(TaskComboBox.SelectedIndex);
                DayRegister.ActivateTask(item);
                this.Width = 510;
                StopButton.Visibility = Visibility.Visible;
            }
        }

        private void SetUserButton_Click(object sender, RoutedEventArgs e)
        {
            DayRegister.ActiveDesigner = UserNameTextBox.Text;
            TaskComboBox.Visibility = Visibility.Visible;
            AddTaskButton.Visibility = Visibility.Visible;
            /*
            NewTaskClientNameTextBox.Visibility = Visibility.Visible;
            NewTaskNameTextBox.Visibility = Visibility.Visible;
            AddTaskByNameButton.Visibility = Visibility.Visible;
             */
            TimersList.Visibility = Visibility.Visible;
            TimersScroll.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Visible;
            foreach (var item in DayRegister.Controls)
            {
                item.Width = 300;
            }
            if (DayRegister.Controls.Any())
            {
                this.Width = 510;
            }
        }
        /*
        private void AddTaskByNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckNewName(NewTaskNameTextBox.Text) && null != NewTaskNameTextBox.Text && string.Empty != NewTaskNameTextBox.Text.Trim())
            {
                DayRegister.ActivateTask(NewTaskNameTextBox.Text, "Nieznany projektant", NewTaskClientNameTextBox.Text, "Nieznany handlowiec");
                this.Width = 510;
                StopButton.Visibility = Visibility.Visible;
                NewTaskNameTextBox.Text = "Nazwa zadania";
                NewTaskClientNameTextBox.Text = "Klient";
            }
        }
        */
        private bool CheckNewName(string taskName)
        {
            if (DayRegister.Controls.Any(x => x.TaskName == taskName) ||
                DayRegister.InactiveControls.Any(x => x.TaskName == taskName))
            {
                //Show error
                MessageBox.Show("Zadanie o takiej nazwie zostało już wprowadzone do systemu!");
                return false;
            }
            else return true;
        }

        private void SelectAddress(object sender, EventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb != null)
            {
                tb.SelectAll();
            }
        }

        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb != null)
            {
                if (!tb.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    tb.Focus();
                }
            }
        }

        private void MainWindow1_Closed(object sender, EventArgs e)
        {
            var lista = new List<List<string>>();
            foreach (var item in DayRegister.Controls)
            {
                if (item.Timer.IsEnabled)
                {
                    item.StopTimer();
                }
                //string name,  string MainDesignerCode, string ClientCode, string TraderCode
                lista.Add(new List<string>()
                {
                    item.TaskName,
                    item.MainDesignerCode,
                    item.ClientCode,
                    item.TraderCode
                });
            }

            XMLHelper.WriteXML(lista);
            XMLHelper.WriteXMLString(UserNameTextBox.Text);
        }

        private void TaskComboBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

        }

    }
}
