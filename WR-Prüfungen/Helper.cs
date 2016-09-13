using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        static public string GetStringFromDataGrid(DataGrid d, int c)
        {
                return Convert.ToString("" + ((TextBlock)d.Columns[c].GetCellContent(d.SelectedItem)).Text);
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public static void SelectRowByIndex(DataGrid dataGrid, int rowIndex)
        {
            if (!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
                throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");

            if (rowIndex < 0 || rowIndex > (dataGrid.Items.Count - 1))
                throw new ArgumentException(string.Format("{0} is an invalid row index.", rowIndex));

            dataGrid.SelectedItems.Clear();
            /* set the SelectedItem property */
            object item = dataGrid.Items[rowIndex]; // = Product X
            dataGrid.SelectedItem = item;

            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            if (row == null)
            {
                /* bring the data item (Product object) into view
                 * in case it has been virtualized away */
                dataGrid.ScrollIntoView(item);
                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }
            //TODO: Retrieve and focus a DataGridCell object
        }
        
        public static DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    /* if the row has been virtualized away, call its ApplyTemplate() method
                     * to build its visual tree in order for the DataGridCellsPresenter
                     * and the DataGridCells to be created */
                    rowContainer.ApplyTemplate();
                    presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    if (cell == null)
                    {
                        /* bring the column into view
                         * in case it has been virtualized away */
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
        }
        
        public static string stringIsNull(string s) {
            if (s == null)
            {
                return "";
            }
            else
            {
                return s;
            }
        }
    }
}

