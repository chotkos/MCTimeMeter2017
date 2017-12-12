using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TimePanelControl;

namespace Data
{
    public static class DayRegister
    {
        public static List<TimeRow> Rows = new List<TimeRow>();
        public static ObservableCollection<TimeControl> Controls = new ObservableCollection<TimeControl>();
        public static ObservableCollection<TimeControl> InactiveControls = new ObservableCollection<TimeControl>();
        public static string ActiveDesigner { get; set; }
        public static void DeacticateTask(string name)
        {
            if (Controls.Any(x => x.TaskName == name))
            {
                var c = Controls.Single(x => x.TaskName == name);
                if (c.Timer.IsEnabled)
                {
                    c.StopTimer();
                }
                InactiveControls.Add(c);
                SortInactives();
                Controls.Remove(c);
            }
        }
        public static void ActivateTask(string name, string MainDesignerCode, string ClientCode, string TraderCode, int CRKId)
        {
            if (InactiveControls.Any(x => x.TaskName == name))
            {
                var item = InactiveControls.Single(x => x.TaskName == name);
                Controls.Add(item);
                InactiveControls.Remove(item);
            }
            else
            {
                var c = new TimeControl(name, MainDesignerCode, ClientCode, TraderCode, CRKId);
                Controls.Add(c);
            }
            SortInactives();
        }
        public static void ActivateTask(string name)
        {
            if (InactiveControls.Any(x => x.TaskName == name))
            {
                var item = InactiveControls.Single(x => x.TaskName == name);
                Controls.Add(item);
                InactiveControls.Remove(item);
            }
            else throw new Exception("Zadanie o podanej nazwie nie istnieje");

        }

        public static void ActivateTask(TimeControl control)
        {
            if (InactiveControls.Any(x => x.TaskName == control.TaskName))
            {
                Controls.Add(control);
                InactiveControls.Remove(control);
                SortInactives();
            }
            else throw new Exception("Zadanie o podanej nazwie nie istnieje w zbiorze nieaktywnych");
            
        }

        public static void AddUnactiveTask(string name, string MainDesignerCode, string ClientCode, string TraderCode,int CRKId)
        {
            InactiveControls.Add(new TimeControl(name, MainDesignerCode, ClientCode, TraderCode,CRKId));
        }


        public static void SortInactives()
        {
            var list = InactiveControls.ToList();
            list.Sort();
            InactiveControls = new ObservableCollection<TimeControl>(list);
        }
    }
}
