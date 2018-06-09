using DevExpress.DashboardCommon;
using DevExpress.DashboardCommon.ViewerData;
using DevExpress.DashboardWpf;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfDashboard_LinkedInteractivity
{
    /// <summary>
    /// Interaction logic for ChildWindow.xaml
    /// </summary>
    public partial class ChildWindow : Window
    {
        private DashboardControl dControl;

        public ChildWindow(DashboardControl dControl) 
        {
            InitializeComponent();
            this.dControl = dControl;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            childDashboardControl.LoadDashboard("Data\\DashboardChild.xml");
            DashboardControl dParentControl = this.dControl;
            dParentControl.MasterFilterSet += DControl_MasterFilterSet;
            dParentControl.MasterFilterCleared += DControl_MasterFilterCleared;
            dParentControl.DrillDownPerformed += DControl_DrillDownPerformed;
            dParentControl.DrillUpPerformed += DControl_DrillUpPerformed;
        }

        bool HasDashboardItem(string itemName)
        {
            return childDashboardControl.Dashboard.Items.
                Select(i => i.ComponentName).Contains(itemName);
        }

        private void DControl_MasterFilterSet(object sender, MasterFilterSetEventArgs e)
        {
            if (HasDashboardItem(e.DashboardItemName))
            {
                string itemName = e.DashboardItemName;
                if (e.SelectedValues != null)
                    childDashboardControl.SetMasterFilter(itemName, e.SelectedValues);
                if (e.SelectedRange != null)
                    childDashboardControl.SetRange(itemName, e.SelectedRange);
            }
        }
        private void DControl_MasterFilterCleared(object sender, MasterFilterClearedEventArgs e)
        {
            if (HasDashboardItem(e.DashboardItemName))
            {
                string itemName = e.DashboardItemName;
                if (childDashboardControl.CanClearMasterFilter(itemName))
                    childDashboardControl.ClearMasterFilter(itemName);
            }
        }
        private void DControl_DrillDownPerformed(object sender, DrillActionEventArgs e)
        {
            if (HasDashboardItem(e.DashboardItemName))
            {
                string itemName = e.DashboardItemName;
                DashboardDataRow row = e.Values[0];
                object value = row[row.Length - 1];

                IList<AxisPointTuple> tuple = childDashboardControl.GetAvailableDrillDownValues(itemName);
                IEnumerable<object> availableValues = tuple.Select(t => t.GetAxisPoint().UniqueValue);
                if (availableValues.Contains(value))
                {
                    childDashboardControl.PerformDrillDown(e.DashboardItemName, value);
                }
            }
        }
        private void DControl_DrillUpPerformed(object sender, DrillActionEventArgs e)
        {
            if (HasDashboardItem(e.DashboardItemName))
            {
                string itemName = e.DashboardItemName;
                int level = e.DrillDownLevel;
                AxisPointTuple tuple = childDashboardControl.GetCurrentDrillDownValues(itemName);
                if (tuple != null)
                {
                    AxisPoint point = childDashboardControl.GetCurrentDrillDownValues(itemName).GetAxisPoint();
                    int l = 0;
                    while (point.Parent != null)
                    {
                        l++;
                        point = point.Parent;
                    }
                    if ((level + 1) == l && childDashboardControl.CanPerformDrillUp(itemName))
                        childDashboardControl.PerformDrillUp(itemName);
                }
            }
        }
    }
}
