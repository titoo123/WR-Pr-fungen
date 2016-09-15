using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        enum Page
        {
            Anschreiben,
            Ergebnis
        };

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
                string path_Anschreiben = Directory.GetCurrentDirectory() + @"\Vorlagen\Anschreiben_Eigenüberwachung_2016.docx";
                string path_Ergebnis = Directory.GetCurrentDirectory() + @"\Vorlagen\Vorlage_Eigenüberwachung_2016.docx";

                Word.Application app_Anschreiben = new Word.Application();

                string pdf_Pfad_Anschreiben = @"C:\WR Export\Export_Anschreiben_" + kunde.Firma + "_" + DateTime.Today.ToShortDateString() + ".pdf";
                string pdf_Pfad_Ergebnis = @"C:\WR Export\Export_Eigenüberwachung_" + kunde.Firma + "_" + DateTime.Today.ToShortDateString() + ".pdf";

                if (File.Exists(pdf_Pfad_Anschreiben))
                {
                    File.Delete(pdf_Pfad_Anschreiben);
                    File.Delete(pdf_Pfad_Ergebnis);

                    Thread.Sleep(1000);
                }

                try
                {
                    Thread.Sleep(1000);
                    Word.Document doc_Anschreiben = app_Anschreiben.Documents.Open(FileName: path_Anschreiben, ReadOnly: false);
                    Word.Document doc_Ergebnis = app_Anschreiben.Documents.Open(FileName: path_Ergebnis, ReadOnly: false);

                    doc_Anschreiben = SetBookmarks_Anschreiben(doc_Anschreiben, kunde);
                    doc_Ergebnis = SetBookmarks_Ergebnis(doc_Ergebnis, kunde);

                    doc_Anschreiben.SaveAs2(pdf_Pfad_Anschreiben, Word.WdSaveFormat.wdFormatPDF);
                    doc_Anschreiben.Close();

                    doc_Ergebnis.SaveAs2(pdf_Pfad_Ergebnis, Word.WdSaveFormat.wdFormatPDF);
                    doc_Ergebnis.Close();

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    app_Anschreiben.Quit(SaveChanges: false);
                }
                Process.Start(pdf_Pfad_Anschreiben);

            }

        }

        private Word.Document SetBookmarks_Anschreiben(Word.Document doc, Kunde kunde) {
            SetValidBookmark(doc, "Anschrift1", kunde.Firma);
            SetValidBookmark(doc, "Anschrift2", kunde.Straße + " " + kunde.Nummer);
            SetValidBookmark(doc, "Anschrift3", kunde.PLZ + " " + kunde.Stadt + "\n" + kunde.Land);

            return doc;
        }

        private Word.Document SetBookmarks_Ergebnis(Word.Document doc, Kunde kunde) {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            var fpr = from x in d.Prüfung
                      where x.Art == "Fremdprüfung" && x.Id_Kunde == kunde.Id
                      && (x.Prüfdatum > d1 && x.Prüfdatum < d2)
                      select x;

            if (fpr.Count() > 0)
            {
                Prüfung p = fpr.First();

                SetValidBookmark(doc, "Kunde", kunde.Firma);
                SetValidBookmark(doc, "Wareneingang", Convert.ToDateTime(p.Wareneingangdatum).ToShortDateString());
                SetValidBookmark(doc, "Lieferschein", Convert.ToDateTime(p.Lieferscheindatum).ToShortDateString());
                SetValidBookmark(doc, "Bemerkungen", p.Bemerkungen);
                SetValidBookmark(doc, "Hersteller", p.Hersteller);
                SetValidBookmark(doc, "ChargenNr", p.Charge);
                SetValidBookmark(doc, "Bundnummer", p.Bundnummer);
                SetValidBookmark(doc, "Durchmesser", p.Du.ToString());
                SetValidBookmark(doc, "Re", p.Re.ToString());
                SetValidBookmark(doc, "Rm", p.Rm.ToString());
                SetValidBookmark(doc, "RmRe", p.RmRe.ToString());
                SetValidBookmark(doc, "Agt", p.Agt.ToString());
                SetValidBookmark(doc, "G", p.Dgs.ToString());
                SetValidBookmark(doc, "fR", p.fR.ToString());
                SetValidBookmark(doc, "Materialart", p.Materialart);
                SetValidBookmark(doc, "Verarbeitungsdatum", Convert.ToDateTime(p.Produktionsdatum).ToShortDateString());
                SetValidBookmark(doc, "Maschine", p.Maschine);
            }
            else
            {
                MessageBox.Show("Keine gültige Prüfung gefunden!","Fehler!");
            }


            return doc;
        }

        private void SetValidBookmark(  Word.Document doc, string bookmark, string value) {
            if (value != null)
            {
                if (doc.Bookmarks.Exists(bookmark))
                {
                    doc.Bookmarks[bookmark].Range.Text = value;
                }
            }
            else
            {
                if (doc.Bookmarks.Exists(bookmark))
                {
                    doc.Bookmarks[bookmark].Range.Text = String.Empty;
                }
            }


        }
    }
}
