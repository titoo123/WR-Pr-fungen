using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace WR_Prüfungen
{
    class Helper
    {
        static public void ClearTextBoxes(Visual myMainWindow)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(myMainWindow);
            for (int i = 0; i < childrenCount; i++)
            {
                var visualChild = (Visual)VisualTreeHelper.GetChild(myMainWindow, i);
                if (visualChild is TextBox)
                {
                    TextBox tb = (TextBox)visualChild;
                    tb.Clear();
                }
                ClearTextBoxes(visualChild);
            }
        }
        static public void IsEnabledTextBoxes(Visual myMainWindow,bool choice)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(myMainWindow);
            for (int i = 0; i < childrenCount; i++)
            {
                var visualChild = (Visual)VisualTreeHelper.GetChild(myMainWindow, i);
                if (visualChild is TextBox)
                {
                    TextBox tb = (TextBox)visualChild;
                    tb.IsEnabled = choice;
                }
                IsEnabledTextBoxes(visualChild,choice);
            }
        }
        static public int GetIntFromDataGrid(DataGrid d, int l) {
            return Convert.ToInt32("" + ((TextBlock)d.Columns[l].GetCellContent(d.SelectedItem)).Text);
        }
    }
}
