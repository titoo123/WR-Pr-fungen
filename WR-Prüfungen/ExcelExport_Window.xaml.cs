using System;
using System.Linq;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace WR_Prüfungen
{
    /// <summary>
    /// Interaktionslogik für ExcelExport_Window.xaml
    /// </summary>
    public partial class ExcelExport_Window : Window
    {
        public ExcelExport_Window()
        {
            InitializeComponent();
        }

        private void button_ExportierenUndSchließen_Click(object sender, RoutedEventArgs e)
        {
            DateTime? d1 = D1_Picker.SelectedDate;
            DateTime? d2 = D2_Picker.SelectedDate;

            if (d1 != null && d2 != null)
            {
                new CreateExcelSheet(d1,d2,true,false);
                this.Close();
            }
            else
            {
                MessageBox.Show("Bitte wählen sie einen Bereich für das Datum aus!","Achtung!");
            }
           
        }
        
    }
}
