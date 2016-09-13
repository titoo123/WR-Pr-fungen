using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace WR_Prüfungen
{
    class CreateWordSheet
    {
        private DateTime? d1;
        private DateTime? d2;
        private int v;

        public CreateWordSheet(DateTime? d1, DateTime? d2, int v)
        {
            this.d1 = d1;
            this.d2 = d2;
            this.v = v;

            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            var kun = from x in d.Kunde
                      where x.Id == v
                      select x;

            if (kun.Count() > 0)
            {
                Kunde kunde = kun.First();
                string path = @"Vorlagen\Anschreiben_Eigenüberwachung_2016.docx";
                Word.Application app = new Word.Application();
                try
                {
                    Thread.Sleep(2000);
                    Word.Document doc = app.Documents.Open(FileName: path, ReadOnly: false);

                    if (doc.Bookmarks.Exists("Anschrift1"))
                    {
                        doc.Bookmarks["Anschrift1"].Range.Text = kunde.Firma;
                    }
                    if (doc.Bookmarks.Exists("Anschrift2"))
                    {
                        doc.Bookmarks["Anschrift2"].Range.Text = kunde.Straße + " " + kunde.Nummer;
                    }
                    if (doc.Bookmarks.Exists("Anschrift3"))
                    {
                        doc.Bookmarks["Anschrift3"].Range.Text = kunde.PLZ + " " + kunde.Stadt + "\n" + kunde.Land;
                    }
                    doc.ExportAsFixedFormat(System.AppDomain.CurrentDomain.BaseDirectory + @"\Export\Export_Anschreiben_" + kunde.Firma + "_" + DateTime.Now + ".pdf",
                         Word.WdExportFormat.wdExportFormatPDF, true, Word.WdExportOptimizeFor.wdExportOptimizeForPrint,
                         Word.WdExportRange.wdExportAllDocument);
                    doc.Close();
                    //doc.Close(Word.WdSaveOptions.wdSaveChanges);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    app.Quit(SaveChanges: false);
                }
                Process.Start(path);

            }

        }
    }
}
