using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WR_Prüfungen
{
    /// <summary>
    /// Interaktionslogik für Prüfung_Window.xaml
    /// </summary>
    public partial class Prüfung_Window : Window
    {
        private int p_id;
        private bool neu;

        private MainWindow w;
        private Prüfung p;

        public Prüfung_Window(MainWindow w)
        {
            InitializeComponent();
            neu = true;
            this.w = w;
            LoadPrüfer();
        }

        public Prüfung_Window(MainWindow w,int p_id)
        {
            InitializeComponent();
            this.p_id = p_id;

            FillFieldsWithPrüfung(p_id);
            this.w = w;
            LoadPrüfer();
        }

        private void FillFieldsWithPrüfung(int i) {
            //Grid_Eingabe.IsEnabled = true;
            neu = false;

            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            var pru = from p in d.Prüfung
                      where p.Id == i
                      select p;

            if (pru.Count() > 0)
            {
                p = pru.First();

                datepicker_prüfung.SelectedDate = p.Datum.Value;
                
                textBox_prüfer_a.Text = p.A.ToString();
                textBox_prüfer_agt.Text = p.Agt.ToString();

                textBox_prüfer_a1025.Text = p.a1_025.ToString();
                textBox_prüfer_a2025.Text = p.a2_025.ToString();
                textBox_prüfer_a3025.Text = p.a3_025.ToString();

                textBox_prüfer_a1075.Text = p.a1_075.ToString();
                textBox_prüfer_a2075.Text = p.a2_075.ToString();
                textBox_prüfer_a3075.Text = p.a3_075.ToString();

                textBox_prüfer_a1m.Text = p.a1m.ToString();   
                textBox_prüfer_a2m.Text = p.a2m.ToString();
                textBox_prüfer_a3m.Text = p.a3m.ToString();



                textBox_prüfer_bundNr.Text = p.Bundnummer.ToString();
                textBox_prüfer_charge.Text = p.Charge.ToString();

                textBox_prüfer_du.Text = p.Du.ToString();
                textBox_prüfer_fr.Text = p.fR.ToString();
                textBox_prüfer_dgs.Text = p.Dgs.ToString();

                textBox_prüfer_rm.Text = p.Rm.ToString();
                textBox_prüfer_re.Text = p.Re.ToString();

                textBox_prüfer_rmre.Text = p.RmRe.ToString();

                textBox_prüfer_se1.Text = p.se1.ToString();
                textBox_prüfer_se2.Text = p.se2.ToString();
                textBox_prüfer_se3.Text = p.se3.ToString();

                textBox_prüfung_c.Text = p.C.ToString();
                textBox_prüfung_AgtM.Text = p.AgtM.ToString();

            }
            else
            {
                MessageBox.Show("Datensatz in Datenbank nicht gefunden!","Fehler!");
            }

        }
        private void FillFieldWithNothing() {

            textBox_prüfer_a.Text = String.Empty;
            textBox_prüfer_agt.Text = String.Empty;

            textBox_prüfer_a1025.Text = String.Empty;
            textBox_prüfer_a2025.Text = String.Empty;
            textBox_prüfer_a3025.Text = String.Empty;

            textBox_prüfer_a1075.Text = String.Empty;
            textBox_prüfer_a2075.Text = String.Empty;
            textBox_prüfer_a3075.Text = String.Empty;

            textBox_prüfer_a1m.Text = String.Empty;
            textBox_prüfer_a2m.Text = String.Empty;
            textBox_prüfer_a3m.Text = String.Empty;



            textBox_prüfer_bundNr.Text = String.Empty;
            textBox_prüfer_charge.Text = String.Empty;

            textBox_prüfer_du.Text = String.Empty;
            textBox_prüfer_fr.Text = String.Empty;
            textBox_prüfer_dgs.Text = String.Empty;

            textBox_prüfer_rm.Text = String.Empty;
            textBox_prüfer_re.Text = String.Empty;

            textBox_prüfer_rmre.Text = String.Empty;

            textBox_prüfer_se1.Text = String.Empty;
            textBox_prüfer_se2.Text = String.Empty;
            textBox_prüfer_se3.Text = String.Empty;

            textBox_prüfung_c.Text = String.Empty;
            textBox_prüfung_AgtM.Text = String.Empty;
        }

        private void button_prüfung_neu_Click(object sender, RoutedEventArgs e)
        {
            neu = true;
            FillFieldWithNothing();
            Grid_Eingabe.IsEnabled = true;
        }

        private void button_prüfung_bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            if (p_id != 0)
            {
                Grid_Eingabe.IsEnabled = true;
            }
        }

        private void button_prüfung_speichern_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            int pid = (
                            from h in d.Prüfer
                            where h.Name == (string)comboBox_prüfer.SelectedItem
                            select new { h.Id }

                          ).First().Id;

            if (neu)
            {
                

                p = new Prüfung()
                {
                    Id_Prüfer = pid,

                    Datum = datepicker_prüfung.SelectedDate,
                    Charge = textBox_prüfer_charge.Text,
                    Bundnummer = textBox_prüfer_bundNr.Text,
                    Du = TextboxToDouble (textBox_prüfer_du),
                    Dgs = TextboxToDouble(textBox_prüfer_dgs),
                    Re = TextboxToDouble(textBox_prüfer_re),
                    Rm = TextboxToDouble(textBox_prüfer_rm),
                    RmRe = TextboxToDouble(textBox_prüfer_rmre),

                    A = TextboxToDouble(textBox_prüfer_a),
                    Agt = TextboxToDouble(textBox_prüfer_agt),
                    fR = TextboxToDouble(textBox_prüfer_fr),
                    se1 = TextboxToDouble(textBox_prüfer_se1),
                    se2 = TextboxToDouble(textBox_prüfer_se2),
                    se3 = TextboxToDouble(textBox_prüfer_se3),
                    a1m = TextboxToDouble(textBox_prüfer_a1m),
                    a2m = TextboxToDouble(textBox_prüfer_a2m),
                    a3m = TextboxToDouble(textBox_prüfer_a3m),
                    a1_025 = TextboxToDouble(textBox_prüfer_a1025),
                    a2_025 = TextboxToDouble(textBox_prüfer_a2025),
                    a3_025 = TextboxToDouble(textBox_prüfer_a3025),
                    a1_075 = TextboxToDouble(textBox_prüfer_a1075),
                    a2_075 = TextboxToDouble(textBox_prüfer_a2075),
                    a3_075 = TextboxToDouble(textBox_prüfer_a3075),
                    C = TextboxToDouble(textBox_prüfung_c),
                    AgtM = TextboxToDouble(textBox_prüfung_AgtM)
                };

                d.Prüfung.InsertOnSubmit(p);
            }
            else
            {
                

                var pru = from i in d.Prüfung
                          where i.Id == p_id
                          select i;

                Prüfung p = pru.First();

                p.Id_Prüfer = pid;
                p.Datum = datepicker_prüfung.SelectedDate;
                p.Charge = textBox_prüfer_charge.Text;
                p.Bundnummer = textBox_prüfer_bundNr.Text;
                p.Du = TextboxToDouble(textBox_prüfer_du);
                p.Dgs = TextboxToDouble(textBox_prüfer_dgs);
                p.Re = TextboxToDouble(textBox_prüfer_re);
                p.Rm = TextboxToDouble(textBox_prüfer_rm);
                p.RmRe = TextboxToDouble(textBox_prüfer_rmre);

                p.A = TextboxToDouble(textBox_prüfer_a);
                p.Agt = TextboxToDouble(textBox_prüfer_agt);
                p.fR = TextboxToDouble(textBox_prüfer_fr);
                p.se1 = TextboxToDouble(textBox_prüfer_se1);
                p.se2 = TextboxToDouble(textBox_prüfer_se2);
                p.se3 = TextboxToDouble(textBox_prüfer_se3);
                p.a1m = TextboxToDouble(textBox_prüfer_a1m);
                p.a2m = TextboxToDouble(textBox_prüfer_a2m);
                p.a3m = TextboxToDouble(textBox_prüfer_a3m);
                p.a1_025 = TextboxToDouble(textBox_prüfer_a1025);
                p.a2_025 = TextboxToDouble(textBox_prüfer_a2025);
                p.a3_025 = TextboxToDouble(textBox_prüfer_a3025);
                p.a1_075 = TextboxToDouble(textBox_prüfer_a1075);
                p.a2_075 = TextboxToDouble(textBox_prüfer_a2075);
                p.a3_075 = TextboxToDouble(textBox_prüfer_a3075);
                p.C = TextboxToDouble(textBox_prüfung_c);
                p.AgtM = TextboxToDouble(textBox_prüfung_AgtM);

            }
            try
            {
                d.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Keine Verbindung zur Datenbank!","Fehler!");
            }
            Grid_Eingabe.IsEnabled = false;
            w.LoadDatagridData();
        }

        private void button_prüfung_löschen_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            var pre = from p in d.Prüfung
                      where p.Id == p_id
                      select p;
            d.Prüfung.DeleteAllOnSubmit(pre);

            try
            {
                d.SubmitChanges();
            }
            catch (Exception)
            {
            }

            p_id = 0;
            FillFieldWithNothing();
            w.LoadDatagridData();
        }

        private Double TextboxToDouble( TextBox t) {
            double f = 0;
            if (!Double.TryParse(t.Text, out f))
            {
                f = 0;
            }
            return f;
        }

        private void LoadPrüfer()
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            var pru = from p in d.Prüfer
                      select p.Name;

            comboBox_prüfer.ItemsSource = pru;

            if (!neu)
            {
                var tpu = from t in d.Prüfer
                          where t.Id == p.Id_Prüfer
                          select new { t.Name };

                if (tpu.Count() > 0)
                {
                    string n = tpu.First().Name;

                    comboBox_prüfer.SelectedItem = n;
                }
            }
            

        }
    }
}
