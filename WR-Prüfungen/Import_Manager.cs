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
    class Import_Manager
    {
        //Pfad für die gespeicherten Einstellungen
        public static string option_pfad = "Data.xml";
        public static string xml_Importpfad;
        public static string csv_Importpfad;

        //Stellt XML Datei für auszulesende Daten dar
        private XmlDocument data_xml = new XmlDocument();
        //Stellt XML Datei für auszulesenden Pfad dar
        private XmlDocument option_xml = new XmlDocument();
        //private string pfad;
        private MainWindow mainWindow;

        public Import_Manager(MainWindow mainWindow, ProgressBar progressBar)
        {

            this.mainWindow = mainWindow;
            Load(progressBar);
        }

        public void Load(ProgressBar progressBar)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            try
            {
                option_xml.Load(Import_Manager.option_pfad);

                xml_Importpfad = option_xml.SelectSingleNode("Einstellungen").SelectSingleNode("XML_Importpfad").InnerText;
                csv_Importpfad = option_xml.SelectSingleNode("Einstellungen").SelectSingleNode("CSV_Importpfad").InnerText;
            }
            catch (Exception)
            {
                MessageBox.Show("Einstellungen nicht gefunden!", "Fehler!");
            }

            System.IO.DirectoryInfo xml_directory = new System.IO.DirectoryInfo(xml_Importpfad);


            //Setzt PBar Werte auf Dateienanzahl
            progressBar.Maximum = xml_directory.GetFiles().Count();

            int z = 0;
            foreach (System.IO.FileInfo f in xml_directory.GetFiles())
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
                            XML_Reader xr = new XML_Reader(data_xml, "TestParameters/TestParameter", f.Name);

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


            CSV_Load(csv_Importpfad);
            //Setzt Balken auf 0
            progressBar.Value = 0;

            //Meldet ob Import fertig
            MessageBox.Show("Import abgeschlossen!\n" + z + " Dateien importiert!", "Information");

            //Refresh Mainwindow
            mainWindow.LoadDatagridData();
        }

        internal void CSV_Load(string csv_Importpfad)
        {   
            //Ließt Rippendaten aus
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            System.IO.DirectoryInfo csv_directory = new System.IO.DirectoryInfo(csv_Importpfad);

            var fpr = from x in d.Prüfung
                      where x.fR == 0 && x.Prüfdatum > DateTime.Today - TimeSpan.FromDays(700)
                      select x;

            foreach (System.IO.FileInfo f in csv_directory.GetFiles())
            {
                if (f.Name.Contains(".csv"))
                {

                    CSV_Reader csv_Reader = new CSV_Reader(f.FullName);

                    foreach (Prüfung p in fpr)
                    {
                        if (p != null)
                        {
                            Prüfung np = csv_Reader.Read(p);
                            if (np != null)
                            {
                                p.a1m = np.a1m;
                                p.a2m = np.a2m;
                                p.a3m = np.a3m;
                                p.a4m = np.a4m;
                                //10 Mittelwert
                                p.a1_025 = np.a1_025;
                                p.a2_025 = np.a2_025;
                                p.a3_025 = np.a3_025;
                                p.a4_025 = np.a4_025;
                                //15 Mittelwert
                                p.a1_075 = np.a1_075;
                                p.a2_075 = np.a2_075;
                                p.a3_075 = np.a3_075;
                                p.a4_075 = np.a4_075;
                                //20 Mittelwert
                                p.c1 = np.c1;
                                p.c2 = np.c2;
                                p.c3 = np.c3;
                                p.c4 = np.c4;
                                //25 Mittelwert
                                p.se1 = np.se1;
                                p.se2 = np.se2;
                                p.se3 = np.se3;
                                p.se4 = np.se4;
                                //30 Summe
                                p.Beta = np.Beta;
                                p.Alpha = np.Alpha;
                                p.fR = np.fR;

                                d.SubmitChanges();
                            }
                        }
                        
                    }
                }
            }
        }
    }
}