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

        private void CreateSheet(DateTime? d1, DateTime? d2)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            var exe = from x in d.Prüfung
                      where x.Datum <= d2 && x.Datum >= d1
                      select x;

            // Variablen deklarieren 
            Excel.Application myExcelApplication;
            Excel.Workbook myExcelWorkbook;
            Excel.Worksheet myExcelWorkSheet;
            myExcelApplication = null;

            try
            {
                // First Contact: Excel Prozess initialisieren
                myExcelApplication = new Excel.Application();
                myExcelApplication.Visible = true;
                myExcelApplication.ScreenUpdating = true;

                // Excel Datei anlegen: Workbook
                var myCount = myExcelApplication.Workbooks.Count;
                myExcelWorkbook = (Excel.Workbook)(myExcelApplication.Workbooks.Add(System.Reflection.Missing.Value));
                myExcelWorkSheet = (Excel.Worksheet)myExcelWorkbook.ActiveSheet;

                // Überschriften eingeben
                myExcelWorkSheet.Cells[2, 2] = "Datum";
                myExcelWorkSheet.Cells[2, 3] = "Prüfer";
                myExcelWorkSheet.Cells[2, 4] = "Charge";
                myExcelWorkSheet.Cells[2, 5] = "Bundnr";
                myExcelWorkSheet.Cells[2, 6] = "Du";
                myExcelWorkSheet.Cells[2, 7] = "Dgs";
                myExcelWorkSheet.Cells[2, 8] = "Re";
                myExcelWorkSheet.Cells[2, 9] = "Rm";
                myExcelWorkSheet.Cells[2, 10] = "Rm/Re";
                myExcelWorkSheet.Cells[2, 11] = "A*";
                myExcelWorkSheet.Cells[2, 12] = "Agt";
                myExcelWorkSheet.Cells[2, 13] = "fR";
                myExcelWorkSheet.Cells[2, 14] = "se1";
                myExcelWorkSheet.Cells[2, 15] = "se2";
                myExcelWorkSheet.Cells[2, 16] = "se3";
                myExcelWorkSheet.Cells[2, 17] = "a1-m";
                myExcelWorkSheet.Cells[2, 18] = "a2-m";
                myExcelWorkSheet.Cells[2, 19] = "a3-m";
                myExcelWorkSheet.Cells[2, 20] = "a1-0,25";
                myExcelWorkSheet.Cells[2, 21] = "a2-0,25";
                myExcelWorkSheet.Cells[2, 22] = "a3-0,25";
                myExcelWorkSheet.Cells[2, 23] = "a1-0,75";
                myExcelWorkSheet.Cells[2, 24] = "a2-0,75";
                myExcelWorkSheet.Cells[2, 25] = "a3-0,75";
                myExcelWorkSheet.Cells[2, 26] = "C";
                myExcelWorkSheet.Cells[2, 27] = "AgtM";

                // Formatieren der Überschrift
                Excel.Range myRangeHeadline;
                myRangeHeadline = myExcelWorkSheet.get_Range("B2", "AA2");
                myRangeHeadline.Font.Bold = true;
                myRangeHeadline.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                myRangeHeadline.Borders.Weight = Excel.XlBorderWeight.xlThick;

                // Daten eingeben
                int j = 0;
                foreach (var i in exe)
                {
                    myExcelWorkSheet.Cells[j + 3, 2] = i.Datum;
                    myExcelWorkSheet.Cells[j + 3, 3] = i.Prüfer.Name;
                    myExcelWorkSheet.Cells[j + 3, 4] = i.Charge;
                    myExcelWorkSheet.Cells[j + 3, 5] = i.Bundnummer;
                    myExcelWorkSheet.Cells[j + 3, 6] = i.Du;
                    myExcelWorkSheet.Cells[j + 3, 7] = i.Dgs;
                    myExcelWorkSheet.Cells[j + 3, 8] = i.Re;
                    myExcelWorkSheet.Cells[j + 3, 9] = i.Rm;

                    myExcelWorkSheet.Cells[j + 3, 10] = i.RmRe;
                    myExcelWorkSheet.Cells[j + 3, 11] = i.A;
                    myExcelWorkSheet.Cells[j + 3, 12] = i.Agt;
                    myExcelWorkSheet.Cells[j + 3, 13] = i.fR;
                    myExcelWorkSheet.Cells[j + 3, 14] = i.se1;
                    myExcelWorkSheet.Cells[j + 3, 15] = i.se2;
                    myExcelWorkSheet.Cells[j + 3, 16] = i.se3;
                    myExcelWorkSheet.Cells[j + 3, 17] = i.a1m;
                    myExcelWorkSheet.Cells[j + 3, 18] = i.a2m;
                    myExcelWorkSheet.Cells[j + 3, 19] = i.a3m;

                    myExcelWorkSheet.Cells[j + 3, 20] = i.a1_025;
                    myExcelWorkSheet.Cells[j + 3, 21] = i.a2_025;
                    myExcelWorkSheet.Cells[j + 3, 22] = i.a3_025;
                    myExcelWorkSheet.Cells[j + 3, 23] = i.a1_075;
                    myExcelWorkSheet.Cells[j + 3, 24] = i.a2_075;
                    myExcelWorkSheet.Cells[j + 3, 25] = i.a3_075;
                    myExcelWorkSheet.Cells[j + 3, 26] = i.C;
                    myExcelWorkSheet.Cells[j + 3, 27] = i.AgtM;
                    j = j+1;
                }
                myExcelWorkSheet.Name = "Prüfungen";
            }
            catch (Exception ex)
            {
                String myErrorString = ex.Message;
                MessageBox.Show(myErrorString);
            }
        }
    }
}
