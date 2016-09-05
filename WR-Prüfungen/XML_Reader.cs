using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace WR_Prüfungen
{

    class XML_Reader
    {
        private static string STRING_VALUE = "Value";
        private static string STRING_PARAMETER_TYPE = "ParameterType";



        XmlDocument x;  //Objekt des XML Dokuments
        string n1;      //Name des ersten Knotens
        string n;       //Name der Datei

        public XML_Reader(XmlDocument x, string n1, string n)
        {
            this.n = n;
            this.n1 = n1;
            this.x = x;
            ReadData();
        }

        private void ReadData()
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            XmlNodeList nList = x.SelectNodes(n1);

            if (nList.Count > 1)
            {
                Prüfung p = new Prüfung();

                if (n.Contains("U"))
                {
                    p.Art = "Ungereckt";
                }
                if (n.Contains("S"))
                {
                    p.Art = "Stab";
                }
                if (n.Contains("F"))
                {
                    p.Art = "Fremdprüfung";
                }

                foreach (XmlNode xNode in nList)
                {
                    string s = xNode[STRING_PARAMETER_TYPE].InnerText;

                    switch (s)
                    {
                        case "199":  //Bediener

                            //Testet ob Prüfer in Datenbank vorhanden

                            string b = xNode[STRING_VALUE].InnerText;

                            if (b.Length > 0)
                            {

                                var prf = from x in d.Prüfer
                                          where x.Name == b
                                          select x;

                                if (prf.Count() == 0)
                                {
                                    d.Prüfer.InsertOnSubmit(new Prüfer { Name = b });

                                    try
                                    {
                                        d.SubmitChanges();
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                            try
                            {
                                int pfr = (from l in d.Prüfer
                                           where l.Name == b
                                           select l
                                          ).First().Id;

                                if (pfr == 0)
                                {
                                    pfr = 2;
                                }
                                p.Id_Prüfer = pfr;
                            }
                            catch (Exception)
                            {
                                p.Id_Prüfer = 2;
                            }


                            break;
                        case "607":  //Datum
                            try
                            {
                                p.Prüfdatum = DateTime.Parse(xNode[STRING_VALUE].InnerText);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Es konnte kein Prüfdatum in der XML-Datei: " + n + " gefunden werden!", "Fehler!");
                            }

                            break;
                        case "596":  //Datum
                            try
                            {
                                p.Produktionsdatum = DateTime.Parse(xNode[STRING_VALUE].InnerText);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Es konnte kein Produktionsdatum in der XML-Datei: " + n + " gefunden werden!", "Fehler!");
                            }

                            break;
                        case "597": //Ring-Nr

                            try
                            {
                                p.Bundnummer = xNode[STRING_VALUE].InnerText;
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Es konnte kein Bundnummer in der XML-Datei: " + n + " gefunden werden!", "Fehler!");
                            }
                            break;
                        case "163": //DGs
                            p.Dgs = Parse_Double(n, xNode, STRING_VALUE,0);
                            break;
                        case "9":   //ChargenNr

                            try
                            {
                                p.Charge = xNode[STRING_VALUE].InnerText;
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Es konnte kein ChargenNr in der XML-Datei: " + n + " gefunden werden!", "Fehler!");
                            }
                            break;
                        case "51": //Durchmesser
                            p.Du = Parse_Double(n, xNode, STRING_VALUE,2);
                            break;
                        case "318": //Agt
                            p.Agt = Parse_Double(n, xNode, STRING_VALUE,2);
                            break;
                        case "308":  //Rm
                            p.Rm = Parse_Double(n, xNode, STRING_VALUE,0);
                            break;
                        case "257":  //Rp
                            p.Re = Parse_Double(n, xNode, STRING_VALUE,0);
                            break;
                        default:
                            break;
                    }
                }
                //Berechnet Rm/Rp
                if (p.Rm != 0 && p.Re != 0)
                {
                    p.RmRe = Math.Round(Convert.ToDouble(p.Rm / p.Re),2);
                }


                d.Prüfung.InsertOnSubmit(p);
                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Keine Verbindung zur Datenbank!", "Fehler!");
                }


            }


        }
        /// <summary>
        /// Parst String-Werte zu Double-Werten
        /// </summary>
        /// <param name="s">Name das Parameters</param>
        /// <param name="xNode">Name des XML Knotens</param>
        /// <param name="u">Name das XML Unterknotens</param>
        /// <returns></returns>
        private double Parse_Double(string s, XmlNode xNode, string u, int digits)
        {
            double d = 0;

            if (Double.TryParse(xNode[u].InnerText.Replace('.', ','), out d))
            {
                return Math.Round( d, digits);
            }
            else
            {
                MessageBox.Show("Es konnte kein " + s + " in der XML-Datei: " + n + " gefunden werden!", "Fehler!");
                return 0;
            }


        }

    }
}
