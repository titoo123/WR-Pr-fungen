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
        DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

        XmlDocument x;
        string p_node;

        public XML_Reader(XmlDocument x, string p_node)
        {

            this.p_node = p_node;
            this.x = x;
            ReadData();
        }

        private void ReadData()
        {
            try
            {
                string s = x.SelectSingleNode(p_node).SelectSingleNode("Prüfer").InnerText;

                int pfr = (  from l in d.Prüfer
                                where l.Name == s
                                select l    
                          ).First().Id;

                if (pfr == 0)
                {
                    pfr = 2;
                }

                Prüfung p = new Prüfung()
                {

                    Datum = Convert.ToDateTime(x.SelectSingleNode(p_node).SelectSingleNode("Datum").InnerText),
                    Charge = x.SelectSingleNode(p_node).SelectSingleNode("Charge").InnerText,
                    Bundnummer = x.SelectSingleNode(p_node).SelectSingleNode("Bundnummer").InnerText,
                    Id_Prüfer = pfr,
                    Du = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("Du").InnerText),
                    Dgs = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("Dgs").InnerText),
                    Re = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("Re").InnerText),
                    Rm = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("Rm").InnerText),
                    RmRe = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("RmRe").InnerText),

                    A = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("A").InnerText),
                    Agt = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("Agt").InnerText),
                    fR = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("fR").InnerText),
                    se1 = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("se1").InnerText),
                    se2 = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("se2").InnerText),
                    se3 = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("se3").InnerText),
                    a1m = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("a1m").InnerText),
                    a2m = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("a2m").InnerText),
                    a3m = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("a3m").InnerText),
                    a1_025 = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("a1_025").InnerText),
                    a2_025 = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("a2_025").InnerText),
                    a3_025 = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("a3_025").InnerText),
                    a1_075 = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("a1_075").InnerText),
                    a2_075 = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("a2_075").InnerText),
                    a3_075 = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("a3_075").InnerText),
                    C = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("C").InnerText),
                    AgtM = Convert.ToDouble(x.SelectSingleNode(p_node).SelectSingleNode("AgtM").InnerText),


                };
                d.Prüfung.InsertOnSubmit(p);
                d.SubmitChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Import fehlgeschlagen! ", "Fehler!");
            }

        }

    }
}
