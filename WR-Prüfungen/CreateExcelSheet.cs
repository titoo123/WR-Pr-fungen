using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace WR_Prüfungen
{
    class CreateExcelSheet
    {
        DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

        public CreateExcelSheet(DateTime? d1, DateTime? d2, bool visible,bool print, string mode)
        {
            var exe = from x in d.Prüfung
                      where x.Prüfdatum <= d2 && x.Prüfdatum >= d1
                      select x;
            if (mode != "no_mode")
            {
                if (mode == "Ungereckt")
                {
                    exe = from l in exe
                          where l.Art == "Ungereckt"
                          select l; 
                }
                if (mode == "Stäbe")
                {
                    exe = from l in exe
                          where l.Art == "Stab"
                          select l;
                }
            }
            else
            {
                exe = from l in exe
                      where l.Art == "Standard"
                      select l;
            }

            // Variablen deklarieren 
            Excel.Application myExcelApplication;
            Excel.Workbook myExcelWorkbook;
            Excel.Worksheet myExcelWorkSheet;
            myExcelApplication = null;

            try
            {
                // First Contact: Excel Prozess initialisieren
                myExcelApplication = new Excel.Application();

                if (visible)
                {
                    myExcelApplication.Visible = true;
                    myExcelApplication.ScreenUpdating = true;
                }

               
                // Excel Datei anlegen: Workbook
                var myCount = myExcelApplication.Workbooks.Count;
                myExcelWorkbook = (Excel.Workbook)(myExcelApplication.Workbooks.Add(System.Reflection.Missing.Value));
                myExcelWorkSheet = (Excel.Worksheet)myExcelWorkbook.ActiveSheet;

                // Überschriften eingeben
                myExcelWorkSheet.Cells[2, 1] = "Prüfdatum";
                myExcelWorkSheet.Cells[2, 2] = "Prüfer";
                myExcelWorkSheet.Cells[2, 3] = "Schmelze";
                myExcelWorkSheet.Cells[2, 4] = "Bundnummer";
                myExcelWorkSheet.Cells[2, 5] = "Datum";

                myExcelWorkSheet.Cells[2, 6] = "d";        //Nenndurchmesser
                myExcelWorkSheet.Cells[2, 7] = "DGs";        //Querschnittsabweichung
                myExcelWorkSheet.Cells[2, 8] = "Rp";      //Festigkeit absolut
                myExcelWorkSheet.Cells[2, 9] = "Rm";        //Festigkeit absolut
                //myExcelWorkSheet.Cells[2, 10] = "Rm/Rp";    //Festigkeit verhältnis Rm/Rp
                //myExcelWorkSheet.Cells[2, 11] = "Rp_ist/Rp_nenn";   //Festogkeit verhältnis Rp/Rp

                myExcelWorkSheet.Cells[2, 12] = "Agt";      //Agt Dehnung
                myExcelWorkSheet.Cells[2, 13] = "fR";       //Oberfläche fR

                myExcelWorkSheet.Cells[2, 14] = "se1";
                myExcelWorkSheet.Cells[2, 15] = "se2";
                myExcelWorkSheet.Cells[2, 16] = "se3";
                myExcelWorkSheet.Cells[2, 17] = "se4";

                myExcelWorkSheet.Cells[2, 18] = "a1-m";
                myExcelWorkSheet.Cells[2, 19] = "a2-m";
                myExcelWorkSheet.Cells[2, 20] = "a3-m";
                myExcelWorkSheet.Cells[2, 21] = "a4-m";

                myExcelWorkSheet.Cells[2, 22] = "a1-0,25";
                myExcelWorkSheet.Cells[2, 23] = "a2-0,25";
                myExcelWorkSheet.Cells[2, 24] = "a3-0,25";
                myExcelWorkSheet.Cells[2, 25] = "a4-0,25";

                myExcelWorkSheet.Cells[2, 26] = "a1-0,75";
                myExcelWorkSheet.Cells[2, 27] = "a2-0,75";
                myExcelWorkSheet.Cells[2, 28] = "a3-0,75";
                myExcelWorkSheet.Cells[2, 29] = "a4-0,75";

                myExcelWorkSheet.Cells[2, 30] = "C1";
                myExcelWorkSheet.Cells[2, 31] = "C2";
                myExcelWorkSheet.Cells[2, 32] = "C3";
                myExcelWorkSheet.Cells[2, 33] = "C4";

                //myExcelWorkSheet.Cells[2, 6] = "Du";
                //myExcelWorkSheet.Cells[2, 7] = "Dgs";
                //myExcelWorkSheet.Cells[2, 8] = "Re";
                //myExcelWorkSheet.Cells[2, 9] = "Rm";
                //myExcelWorkSheet.Cells[2, 10] = "Rm/Re";
                //myExcelWorkSheet.Cells[2, 11] = "A*";
                //myExcelWorkSheet.Cells[2, 12] = "Agt";
                //myExcelWorkSheet.Cells[2, 13] = "fR";
                //myExcelWorkSheet.Cells[2, 14] = "se1";
                //myExcelWorkSheet.Cells[2, 15] = "se2";
                //myExcelWorkSheet.Cells[2, 16] = "se3";
                //myExcelWorkSheet.Cells[2, 17] = "a1-m";
                //myExcelWorkSheet.Cells[2, 18] = "a2-m";
                //myExcelWorkSheet.Cells[2, 19] = "a3-m";
                //myExcelWorkSheet.Cells[2, 20] = "a1-0,25";
                //myExcelWorkSheet.Cells[2, 21] = "a2-0,25";
                //myExcelWorkSheet.Cells[2, 22] = "a3-0,25";
                //myExcelWorkSheet.Cells[2, 23] = "a1-0,75";
                //myExcelWorkSheet.Cells[2, 24] = "a2-0,75";
                //myExcelWorkSheet.Cells[2, 25] = "a3-0,75";
                //myExcelWorkSheet.Cells[2, 26] = "C";
                //myExcelWorkSheet.Cells[2, 27] = "AgtM";

                // Formatieren der Überschrift
                Excel.Range myRangeHeadline;
                myRangeHeadline = myExcelWorkSheet.get_Range("A2", "AG2");
                myRangeHeadline.Font.Bold = true;
                myRangeHeadline.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                myRangeHeadline.Borders.Weight = Excel.XlBorderWeight.xlThick;

                Excel.Range myRangeValueField;
                myRangeValueField = myExcelWorkSheet.get_Range("A3", "AG" + exe.Count() + 3);
                myRangeValueField.NumberFormat = "@";
                // Daten eingeben
                int j = 0;
                foreach (var i in exe)
                {
                    myExcelWorkSheet.Cells[j + 3, 1] = i.Prüfdatum;
                    myExcelWorkSheet.Cells[j + 3, 5] = i.Produktionsdatum;
                    if (i.Id_Prüfer != null)
                    {
                        myExcelWorkSheet.Cells[j + 3, 2] = i.Prüfer.Name;
                    }

                    myExcelWorkSheet.Cells[j + 3, 3] = i.Charge;
                    myExcelWorkSheet.Cells[j + 3, 4] = i.Bundnummer;
                    myExcelWorkSheet.Cells[j + 3, 6] = i.Du;
                    myExcelWorkSheet.Cells[j + 3, 7] = i.Dgs;
                    myExcelWorkSheet.Cells[j + 3, 8] = i.Re;
                    myExcelWorkSheet.Cells[j + 3, 9] = i.Rm;

                    //myExcelWorkSheet.Cells[j + 3, 10] = i.RmRe;
                    //myExcelWorkSheet.Cells[j + 3, 11] = (i.Re/500);
                    //myExcelWorkSheet.Cells[j + 3, 11] = i.A;
                    myExcelWorkSheet.Cells[j + 3, 12] = i.Agt;
                    myExcelWorkSheet.Cells[j + 3, 13] = i.fR;

                    myExcelWorkSheet.Cells[j + 3, 14] = i.se1;
                    myExcelWorkSheet.Cells[j + 3, 15] = i.se2;
                    myExcelWorkSheet.Cells[j + 3, 16] = i.se3;
                    myExcelWorkSheet.Cells[j + 3, 17] = i.se4;

                    myExcelWorkSheet.Cells[j + 3, 18] = i.a1m;
                    myExcelWorkSheet.Cells[j + 3, 19] = i.a2m;
                    myExcelWorkSheet.Cells[j + 3, 20] = i.a3m;
                    myExcelWorkSheet.Cells[j + 3, 21] = i.a4m;

                    myExcelWorkSheet.Cells[j + 3, 22] = i.a1_025;
                    myExcelWorkSheet.Cells[j + 3, 23] = i.a2_025;
                    myExcelWorkSheet.Cells[j + 3, 24] = i.a3_025;
                    myExcelWorkSheet.Cells[j + 3, 25] = i.a4_025;

                    myExcelWorkSheet.Cells[j + 3, 26] = i.a1_075;
                    myExcelWorkSheet.Cells[j + 3, 27] = i.a2_075;
                    myExcelWorkSheet.Cells[j + 3, 28] = i.a3_075;
                    myExcelWorkSheet.Cells[j + 3, 29] = i.a4_075;

                    myExcelWorkSheet.Cells[j + 3, 30] = i.c1;
                    myExcelWorkSheet.Cells[j + 3, 31] = i.c2;
                    myExcelWorkSheet.Cells[j + 3, 32] = i.c3;
                    myExcelWorkSheet.Cells[j + 3, 33] = i.c4;

                    //myExcelWorkSheet.Cells[j + 3, 26] = i.C;
                    //myExcelWorkSheet.Cells[j + 3, 27] = i.AgtM;
                    j = j + 1;
                }
                myExcelWorkSheet.Name = "Prüfungen";

                if (print)
                {
                    myExcelWorkSheet.Columns.Font.Size = 6;

                    myExcelWorkSheet.Columns.AutoFit();
                    Print(myExcelWorkSheet, myExcelWorkbook);
                }
            }
            catch (Exception ex)
            {
                String myErrorString ="Drucken fehlgeschlagen: " + ex.Message;
                MessageBox.Show(myErrorString);
            }finally
            {
                //myExcelApplication.Quit();

            }   
        
        }
        private void Print(Excel.Worksheet myExcelWorkSheet, Excel.Workbook myExcelWorkbook)
        {
            myExcelWorkSheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
            myExcelWorkSheet.PageSetup.LeftMargin = myExcelWorkSheet.PageSetup.LeftMargin / 4;
            myExcelWorkSheet.PrintOut(
            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing);



            // Cleanup:
            GC.Collect();
            GC.WaitForPendingFinalizers();

            myExcelWorkbook.Close(false, Type.Missing, Type.Missing);
            //Marshal.FinalReleaseComObject(myExcelWorkSheet);

            //myExcelWorkSheet.Close(false, Type.Missing, Type.Missing);
            //Marshal.FinalReleaseComObject(myExcelWorkSheet);

            //APP.Quit();
            //Marshal.FinalReleaseComObject(APP);
        }



    }
}
