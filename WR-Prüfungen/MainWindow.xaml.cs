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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WR_Prüfungen
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadDatagridData();
            ResetDate();

        }

        private void Prüfer_Click(object sender, RoutedEventArgs e)
        {
            Window prüfer = new Prüfer_Window();
            prüfer.Show();

        }

        private void dataGrid_WR_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void LoadDatagridData()
        {

            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var mat = from u in db.Prüfung
                      where u.Datum >= Datepicker_Von.SelectedDate && u.Datum <= Datepicker_Bis.SelectedDate
                      select new { u.Id, u.Datum, u.Prüfer.Name,u.Charge,u.Bundnummer,u.Du,u.Dgs,u.Re,u.Rm,u.RmRe,u.A,u.Agt,
                          u.fR,u.se1,u.se2,u.se3,u.a1m,u.a2m,u.a3m,u.a1_025,u.a2_025,u.a3_025,u.a1_075,u.a2_075,u.a3_075,u.C,u.AgtM};

            dataGrid_WR.ItemsSource = mat.ToList();
            Prüfungen_Label_Count(label_Anzahl_Datensätze);
        }
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime?))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";

            if (e.Column.Header.ToString() == "Id")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }

          

        }

        private void button_P_neu_Click(object sender, RoutedEventArgs e)
        {
            Window n_prüfung = new Prüfung_Window(this);
            n_prüfung.Show();
        }

        private void button_P_bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_WR.SelectedItem != null)
            {
                int p_id = Convert.ToInt32("" + ((TextBlock)dataGrid_WR.Columns[0].GetCellContent(dataGrid_WR.SelectedItem)).Text);
                Window b_prüfung = new Prüfung_Window(this, p_id);
                b_prüfung.Show();
            }
            else
            {
                MessageBox.Show("Bitte wählen sie eine Prüfung zum bearbeiten aus!","Achtung!");
            }

        }

        private void button_P_löschen_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_WR.SelectedItem != null)
            {
                int p_id = Convert.ToInt32("" + ((TextBlock)dataGrid_WR.Columns[0].GetCellContent(dataGrid_WR.SelectedItem)).Text);
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
                    MessageBox.Show("Keine Verbindung zur Datenbank!", "Fehler!");
                }
                LoadDatagridData();
            }
            else
            {
                MessageBox.Show("Bitte wählen sie eine Prüfung zum löschen aus!", "Achtung!");
            }


           
        }

        private void button_P_einlesen_Click(object sender, RoutedEventArgs e)
        {
            XML_Manager x = new XML_Manager(this, ProgressBar_XML);
            LoadDatagridData();
        }

        private void button_P_date_löschen_Click(object sender, RoutedEventArgs e)
        {

            Window pndlw = new PrüfungenLöschen_Window(this);
            pndlw.Show();

            LoadDatagridData();
        }

        private void button_P_drucken_Click(object sender, RoutedEventArgs e)
        {
            new CreateExcelSheet(Datepicker_Von.SelectedDate, Datepicker_Bis.SelectedDate, false, true);
           
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            ExcelExport_Window excelExport_Window = new ExcelExport_Window();
            excelExport_Window.Show();
        }

        private void AS400_Click(object sender, RoutedEventArgs e)
        {
            AS400Export_Window asw = new AS400Export_Window();
            asw.Show();
        }

        private void ResetDate() {

            Datepicker_Von.SelectedDate = DateTime.Now - TimeSpan.FromDays(30);
            Datepicker_Bis.SelectedDate = DateTime.Now;
        }

        private Label Prüfungen_Label_Count(Label l) {

            l.Content = dataGrid_WR.Items.Count + " Prüfungen vom";

            return l;
        }

        private void Datepicker_Von_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Datepicker_Bis.SelectedDate != null)
            {
                if (Datepicker_Von.SelectedDate <= Datepicker_Bis.SelectedDate)
                {
                    LoadDatagridData();
                }
                else
                {
                    MessageBox.Show("Bitte geben sie ein logisches Datum ein!", "Achtung!");
                }
            }

        }

        private void Datepicker_Bis_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Datepicker_Bis.SelectedDate != null)
            {
                if (Datepicker_Von.SelectedDate <= Datepicker_Bis.SelectedDate)
                {
                    LoadDatagridData();
                }
                else
                {
                    MessageBox.Show("Bitte geben sie ein logisches Datum ein!", "Achtung!");
                }
            }


      
        }
        
        private void button_P_importPfad_Click(object sender, RoutedEventArgs e)
        {
            Pfad_Window pw = new Pfad_Window();
            pw.Show();
        }
        
    }
}
