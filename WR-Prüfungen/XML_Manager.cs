using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace WR_Prüfungen
{
    class XML_Manager
    {
        //Pfad für die gespeicherten Einstellungen
        public static string option_pfad = "Data.xml";
        public static string import_pfad;

        //Stellt XML Datei für auszulesende Daten dar
        private XmlDocument data_xml = new XmlDocument();
        //Stellt XML Datei für auszulesenden Pfad dar
        private XmlDocument option_xml = new XmlDocument();
        //private string pfad;
        private MainWindow w;

        public XML_Manager(MainWindow w,ProgressBar p) {

            this.w = w;
            //this.pfad = pfad;

            Load(p);
        }

        public void Load(ProgressBar progressBar)
        {
            try
            {
                option_xml.Load(XML_Manager.option_pfad);

                import_pfad = option_xml.SelectSingleNode("Einstellungen").SelectSingleNode("Importpfad").InnerText;
            }
            catch (Exception)
            {
                MessageBox.Show("Einstellungen nicht gefunden!", "Fehler!");
            }

            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(import_pfad);
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            //Setzt PBar Werte auf Dateienanzahl
            progressBar.Maximum = directory.GetFiles().Count();

            int z = 0;
            foreach (System.IO.FileInfo f in directory.GetFiles())
            {
                var s = from h in d.Import
                        where h.Name == f.Name
                        select h;

                if (s.Count() < 1)
                {
                    if (f.Name.Contains(".xml"))
                    {
                        data_xml.Load(f.FullName);
                        try
                        {
                            XML_Reader xr = new XML_Reader(data_xml,"Testknoten");
                         
                            d.Import.InsertOnSubmit(new Import() { Name = f.Name });
                            z = z + 1;
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Import der Datei " + f.Name + " fehlgeschlagen!\n\n" + e, "Fehler!");
                        }
                    }

                    try
                    {
                        d.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Keine Verbindung zur Datenbank!", "Fehler!");
                    }

                }
                progressBar.Value++;
            }


            //Setzt Balken auf 0
            progressBar.Value = 0;

            //Meldet ob Import fertig
            MessageBox.Show("Import abgeschlossen!\n" + z + " Dateien importiert!", "Information");

            //Refresh Mainwindow
            w.LoadDatagridData();
        }

    }
}
