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
            comboBox_Prüfer_Load();
            //dataGrid_Kunde_Select_ByLoad();
        }

        public Prüfung_Window(MainWindow w,int p_id)
        {
            InitializeComponent();
            this.p_id = p_id;

            FillFieldsWithPrüfung(p_id);
            this.w = w;
            comboBox_Prüfer_Load();
            comboBox_Art_Load(p_id);
            dataGrid_Kunde_Select_ByLoad();
        }

        private void FillFieldsWithPrüfung(int i) {
            button_prüfung_bearbeiten.IsEnabled = true;
            //Grid_Eingabe.IsEnabled = true;
            neu = false;

            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            var pru = from p in d.Prüfung
                      where p.Id == i
                      select p;

            if (pru.Count() > 0)
            {
                p = pru.First();

                datepicker_prüfung.SelectedDate = p.Prüfdatum.Value;

                datepicker_produktion.SelectedDate = p.Produktionsdatum.Value;

                //textBox_prüfer_a.Text = p.A.ToString();
                textBox_prüfer_agt.Text = p.Agt.ToString();

                //textBox_prüfer_a1025.Text = p.a1_025.ToString();
                //textBox_prüfer_a2025.Text = p.a2_025.ToString();
                //textBox_prüfer_a3025.Text = p.a3_025.ToString();
                //textBox_prüfer_a4025.Text = p.a4_025.ToString();

                //textBox_prüfer_a1075.Text = p.a1_075.ToString();
                //textBox_prüfer_a2075.Text = p.a2_075.ToString();
                //textBox_prüfer_a3075.Text = p.a3_075.ToString();
                //textBox_prüfer_a4075.Text = p.a4_075.ToString();

                //textBox_prüfer_a1m.Text = p.a1m.ToString();   
                //textBox_prüfer_a2m.Text = p.a2m.ToString();
                //textBox_prüfer_a3m.Text = p.a3m.ToString();
                //textBox_prüfer_a4m.Text = p.a4m.ToString();


                textBox_prüfer_bundNr.Text = p.Bundnummer.ToString();
                if (p.Charge != null)
                {
                    textBox_prüfer_charge.Text = p.Charge.ToString();
                }


                textBox_prüfer_du.Text = p.Du.ToString();
                textBox_prüfer_fr.Text = p.fR.ToString();
                textBox_prüfer_dgs.Text = p.Dgs.ToString();

                textBox_prüfer_rm.Text = p.Rm.ToString();
                textBox_prüfer_re.Text = p.Re.ToString();

                textBox_prüfer_rmre.Text = p.RmRe.ToString();

                //textBox_prüfer_se1.Text = p.se1.ToString();
                //textBox_prüfer_se2.Text = p.se2.ToString();
                //textBox_prüfer_se3.Text = p.se3.ToString();
                //textBox_prüfer_se4.Text = p.se4.ToString();

                //textBox_prüfer_C1.Text = p.c1.ToString();
                //textBox_prüfer_C2.Text = p.c2.ToString();
                //textBox_prüfer_C3.Text = p.c3.ToString();
                //textBox_prüfer_C4.Text = p.c4.ToString();

                textBox_prüfung_Alpha.Text = p.Alpha.ToString();
                textBox_prüfung_Beta.Text = p.Beta.ToString();
                //textBox_prüfung_c.Text = p.C.ToString();
                //textBox_prüfung_AgtM.Text = p.AgtM.ToString();

            }
            else
            {
                MessageBox.Show("Datensatz in Datenbank nicht gefunden!","Fehler!");
            }

        }
        private void FillFieldWithNothing() {

           // textBox_prüfer_a.Text = String.Empty;
            textBox_prüfer_agt.Text = String.Empty;

            //textBox_prüfer_a1025.Text = String.Empty;
            //textBox_prüfer_a2025.Text = String.Empty;
            //textBox_prüfer_a3025.Text = String.Empty;
            //textBox_prüfer_a4025.Text = String.Empty;

            //textBox_prüfer_a1075.Text = String.Empty;
            //textBox_prüfer_a2075.Text = String.Empty;
            //textBox_prüfer_a3075.Text = String.Empty;
            //textBox_prüfer_a4075.Text = String.Empty;

            //textBox_prüfer_a1m.Text = String.Empty;
            //textBox_prüfer_a2m.Text = String.Empty;
            //textBox_prüfer_a3m.Text = String.Empty;
            //textBox_prüfer_a4m.Text = String.Empty;


            textBox_prüfer_bundNr.Text = String.Empty;
            textBox_prüfer_charge.Text = String.Empty;

            textBox_prüfer_du.Text = String.Empty;
            textBox_prüfer_fr.Text = String.Empty;
            textBox_prüfer_dgs.Text = String.Empty;

            textBox_prüfer_rm.Text = String.Empty;
            textBox_prüfer_re.Text = String.Empty;

            textBox_prüfer_rmre.Text = String.Empty;

            //textBox_prüfer_se1.Text = String.Empty;
            //textBox_prüfer_se2.Text = String.Empty;
            //textBox_prüfer_se3.Text = String.Empty;
            //textBox_prüfer_se4.Text = String.Empty;

            //textBox_prüfer_C1.Text = String.Empty;
            //textBox_prüfer_C2.Text = String.Empty;
            //textBox_prüfer_C3.Text = String.Empty;
            //textBox_prüfer_C4.Text = String.Empty;

            textBox_prüfung_Alpha.Text = String.Empty;
            textBox_prüfung_Beta.Text = String.Empty;

            //textBox_prüfung_c.Text = String.Empty;
            //textBox_prüfung_AgtM.Text = String.Empty;
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
                button_prüfung_speichern.IsEnabled = true;
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

                    Prüfdatum = datepicker_prüfung.SelectedDate,
                    Produktionsdatum = datepicker_produktion.SelectedDate,
                    Charge = textBox_prüfer_charge.Text,
                    Bundnummer = textBox_prüfer_bundNr.Text,
                    Du = TextboxToDouble (textBox_prüfer_du),
                    Dgs = TextboxToDouble(textBox_prüfer_dgs),
                    Re = TextboxToDouble(textBox_prüfer_re),
                    Rm = TextboxToDouble(textBox_prüfer_rm),
                    RmRe = TextboxToDouble(textBox_prüfer_rmre),

                    //A = TextboxToDouble(textBox_prüfer_a),
                    Agt = TextboxToDouble(textBox_prüfer_agt),
                    fR = TextboxToDouble(textBox_prüfer_fr),
                    Art = (string)comboBox_art.SelectedItem,
                    //se1 = TextboxToDouble(textBox_prüfer_se1),
                    //se2 = TextboxToDouble(textBox_prüfer_se2),
                    //se3 = TextboxToDouble(textBox_prüfer_se3),
                    //se4 = TextboxToDouble(textBox_prüfer_se4),

                    //a1m = TextboxToDouble(textBox_prüfer_a1m),
                    //a2m = TextboxToDouble(textBox_prüfer_a2m),
                    //a3m = TextboxToDouble(textBox_prüfer_a3m),
                    //a4m = TextboxToDouble(textBox_prüfer_a4m),

                    //a1_025 = TextboxToDouble(textBox_prüfer_a1025),
                    //a2_025 = TextboxToDouble(textBox_prüfer_a2025),
                    //a3_025 = TextboxToDouble(textBox_prüfer_a3025),
                    //a4_025 = TextboxToDouble(textBox_prüfer_a4025),

                    //a1_075 = TextboxToDouble(textBox_prüfer_a1075),
                    //a2_075 = TextboxToDouble(textBox_prüfer_a2075),
                    //a3_075 = TextboxToDouble(textBox_prüfer_a3075),
                    //a4_075 = TextboxToDouble(textBox_prüfer_a4075),

                    //c1 = TextboxToDouble(textBox_prüfer_C1),
                    //c2 = TextboxToDouble(textBox_prüfer_C2),
                    //c3 = TextboxToDouble(textBox_prüfer_C3),
                    //c4 = TextboxToDouble(textBox_prüfer_C4),

                    Alpha = TextboxToDouble( textBox_prüfung_Alpha),
                    Beta = TextboxToDouble(textBox_prüfung_Beta)
                    //C = TextboxToDouble(textBox_prüfung_c),
                    //AgtM = TextboxToDouble(textBox_prüfung_AgtM)
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
                p.Prüfdatum = datepicker_prüfung.SelectedDate;
                p.Produktionsdatum = datepicker_produktion.SelectedDate;
                p.Charge = textBox_prüfer_charge.Text;
                p.Bundnummer = textBox_prüfer_bundNr.Text;
                p.Du = TextboxToDouble(textBox_prüfer_du);
                p.Dgs = TextboxToDouble(textBox_prüfer_dgs);
                p.Re = TextboxToDouble(textBox_prüfer_re);
                p.Rm = TextboxToDouble(textBox_prüfer_rm);
                p.RmRe = TextboxToDouble(textBox_prüfer_rmre);

                //p.A = TextboxToDouble(textBox_prüfer_a);
                p.Agt = TextboxToDouble(textBox_prüfer_agt);
                p.fR = TextboxToDouble(textBox_prüfer_fr);
                p.Art = (string)comboBox_art.SelectedItem;
                //p.se1 = TextboxToDouble(textBox_prüfer_se1);
                //p.se2 = TextboxToDouble(textBox_prüfer_se2);
                //p.se3 = TextboxToDouble(textBox_prüfer_se3);
                //p.se4 = TextboxToDouble(textBox_prüfer_se4);

                //p.a1m = TextboxToDouble(textBox_prüfer_a1m);
                //p.a2m = TextboxToDouble(textBox_prüfer_a2m);
                //p.a3m = TextboxToDouble(textBox_prüfer_a3m);
                //p.a4m = TextboxToDouble(textBox_prüfer_a4m);

                //p.a1_025 = TextboxToDouble(textBox_prüfer_a1025);
                //p.a2_025 = TextboxToDouble(textBox_prüfer_a2025);
                //p.a3_025 = TextboxToDouble(textBox_prüfer_a3025);
                //p.a4_025 = TextboxToDouble(textBox_prüfer_a4025);

                //p.a1_075 = TextboxToDouble(textBox_prüfer_a1075);
                //p.a2_075 = TextboxToDouble(textBox_prüfer_a2075);
                //p.a3_075 = TextboxToDouble(textBox_prüfer_a3075);
                //p.a4_075 = TextboxToDouble(textBox_prüfer_a4075);

                //p.c1 = TextboxToDouble(textBox_prüfer_C1);
                //p.c2 = TextboxToDouble(textBox_prüfer_C2);
                //p.c3 = TextboxToDouble(textBox_prüfer_C3);
                //p.c4 = TextboxToDouble(textBox_prüfer_C4);

                p.Alpha = TextboxToDouble(textBox_prüfung_Alpha);
                p.Beta = TextboxToDouble(textBox_prüfung_Beta);

                //p.C = TextboxToDouble(textBox_prüfung_c);
                //p.AgtM = TextboxToDouble(textBox_prüfung_AgtM);

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

        private void comboBox_Prüfer_Load()
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

        private void comboBox_Art_Load(int pId) {
            if (!neu)
            {
                DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
                string art = (from a in d.Prüfung
                              where a.Id == pId
                              select a).First().Art;

                comboBox_art.Items.Add("Standard");
                comboBox_art.Items.Add("Ungereckt");
                comboBox_art.Items.Add("Stab");
                comboBox_art.Items.Add("Fremdprüfung");

                try
                {
                    comboBox_art.SelectedItem = art;
                }
                catch (Exception)
                {
                    MessageBox.Show("Es konnte keine gültige Art gefunden werden!", "Fehler!");
                }
            }
        }

        private void dataGrid_Kunde_Select_ByLoad() {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            var kvr = from x in d.Prüfung
                      where x.Charge == textBox_prüfer_charge.Text && x.Bundnummer == textBox_prüfer_bundNr.Text
                      select x;
            if (kvr.Count() > 0)
            {
                Helper.SelectRowByCellValue(dataGrid_Kunde,0, kvr.First().Id_Kunde.ToString());
            }
        }

        private void comboBox_art_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tmp = comboBox_art.SelectedValue.ToString();
            if ( tmp == "Fremdprüfung")
            {
                this.dataGrid_Kunde.Margin = new Thickness(10, 0, 10, 0);
                this.button_kunde_zuordnen.Margin = new Thickness(10, 0, 10, 0);
                this.button_art_Kundendaten.Margin = new Thickness(10, 0, 10, 0);
                this.Width = this.Width + 200;
                this.dataGrid_Kunde.Width = this.dataGrid_Kunde.Width + 200;

                this.button_art_Kundendaten.Width = Double.NaN;
                this.button_kunde_zuordnen.Width = Double.NaN;

                DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
                var k = from x in d.Kunde
                        select new { x.Id, x.Firma, x.Land };
                dataGrid_Kunde.ItemsSource = k;
            }
            else
            {
                this.dataGrid_Kunde.Margin = new Thickness(0, 0, 0, 0);
                this.button_kunde_zuordnen.Margin = new Thickness(0, 0, 0, 0);
                this.button_art_Kundendaten.Margin = new Thickness(0, 0, 0, 0);

                this.Width = 750;
                this.dataGrid_Kunde.Width = 0;
                this.button_art_Kundendaten.Width = 0;
                this.button_kunde_zuordnen.Width = 0;

                dataGrid_Kunde.ItemsSource = null;

            }
        }

        private void button_art_Kundendaten_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_Kunde.SelectedIndex != -1)
            {
                Kunde_Window kwi = new Kunde_Window(Helper.GetIntFromDataGrid(dataGrid_Kunde,0));
                kwi.Show();
            }        
        }

        private void button_kunde_zuordnen_Click(object sender, RoutedEventArgs e)
        {
            button_prüfung_speichern_Click(sender, e);
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            var kvr = from x in d.Prüfung
                      where x.Charge == textBox_prüfer_charge.Text && x.Bundnummer == textBox_prüfer_bundNr.Text
                      select x;

            if (kvr.Count() > 0)
            {
                kvr.First().Id_Kunde = Helper.GetIntFromDataGrid(dataGrid_Kunde, 0);
                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Verknüpfung zu Kunden konnte nicht gesetzt werden!","Fehler!");
                }
            }
        }
    }
}
