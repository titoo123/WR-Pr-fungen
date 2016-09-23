using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace WR_Prüfungen
{
    class CSV_Reader
    {
        string path;
        List<Prüfung> p_List = new List<Prüfung>();

        public CSV_Reader(string path)
        {
            this.path = path;
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();

                    Prüfung p = new Prüfung();

                    try
                    {

                        //if (((DateTime.Parse(fields[0]).ToShortDateString()) == (Convert.ToDateTime(p.Produktionsdatum).ToShortDateString()))
                        //    && fields[1] == "WR"
                        //    && fields[2] == p.Charge
                        //    && fields[4] == p.Bundnummer
                        //    )
                        //{
                        p.Prüfdatum = DateTime.Parse(fields[0]);
                        p.Charge = fields[2];
                        p.Bundnummer = fields[4];
                        //5 Durchmesser
                        p.a1m = Convert.ToDouble(fields[6]);
                        p.a2m = Convert.ToDouble(fields[7]);
                        p.a3m = Convert.ToDouble(fields[8]);
                        p.a4m = Convert.ToDouble(fields[9]);
                        //10 Mittelwert
                        p.a1_025 = Convert.ToDouble(fields[11]);
                        p.a2_025 = Convert.ToDouble(fields[12]);
                        p.a3_025 = Convert.ToDouble(fields[13]);
                        p.a4_025 = Convert.ToDouble(fields[14]);
                        //15 Mittelwert
                        p.a1_075 = Convert.ToDouble(fields[16]);
                        p.a2_075 = Convert.ToDouble(fields[17]);
                        p.a3_075 = Convert.ToDouble(fields[18]);
                        p.a4_075 = Convert.ToDouble(fields[19]);
                        //20 Mittelwert
                        p.c1 = Convert.ToDouble(fields[21]);
                        p.c2 = Convert.ToDouble(fields[22]);
                        p.c3 = Convert.ToDouble(fields[23]);
                        p.c4 = Convert.ToDouble(fields[24]);
                        //25 Mittelwert
                        p.se1 = Convert.ToDouble(fields[26]);
                        p.se2 = Convert.ToDouble(fields[27]);
                        p.se3 = Convert.ToDouble(fields[28]);
                        p.se4 = Convert.ToDouble(fields[29]);
                        //30 Summe
                        p.Beta = Convert.ToDouble(fields[36]);
                        p.Alpha = Convert.ToDouble(fields[37]);
                        p.fR = Convert.ToDouble(fields[38]);
                        //}
                        p_List.Add(p);

                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }
        internal Prüfung Read(Prüfung p)
        {
            if (p != null)
            {
                string n = Helper.DeleteLetter(Helper.DeleteLetter(Helper.DeleteLetter(Helper.DeleteLetter(Helper.DeleteLetter(p.Bundnummer, "-"), "U"), "N"), "F"), "S");

                foreach (Prüfung item in p_List)
                {
                    if (((Convert.ToDateTime(item.Prüfdatum).ToShortDateString()) == (Convert.ToDateTime(p.Prüfdatum).ToShortDateString()))
                    && item.Charge == p.Charge
                    && item.Bundnummer == n
                    )
                    {
                        return item;
                    }
                }

            }

            return null;
        }
    }
}
