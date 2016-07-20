using ElmMQDNetLib;
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
    /// Interaktionslogik für AS400Export_Window.xaml
    /// </summary>
    public partial class AS400Export_Window : Window
    {
        //von
        DateTime? d1;
        //bis
        DateTime? d2;
        public AS400Export_Window()
        {
            InitializeComponent();
        }

        private void button_ExportierenUndSchließen_Click(object sender, RoutedEventArgs e)
        {
            //Sind die Datepicker ausgefüllt?
            if (D1_Picker.SelectedDate != null && D2_Picker.SelectedDate != null && D1_Picker.SelectedDate < D2_Picker.SelectedDate)
            {
                //Konvertieren des Datums
                d1 = D1_Picker.SelectedDate;
                d2 = D2_Picker.SelectedDate;

                //Datenbankabfrage der Matten mit betreffenden Datum die noch nicht exportiert wurden
                DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

                var ase = from a in d.Prüfung
                          where a.Datum >= d1 && a.Datum <= d2 //&& (a.E_AS400 == false || a.E_AS400 == null)
                          select a;
                int i = ase.Count();
                //Schleife durchfährt alle betreffenden Prüfungen
                foreach (var m in ase)
                {    

                }


                MessageBox.Show("Es wurden " + i + " Prüfungen erfolgreich übertragen!", "Erflogreich!");
            }
            else
            {
                MessageBox.Show("Bitte wählen sie für beide Datumsfelder ein Datum aus!", "Achtung!");
            }
        }

        /// <summary>
        /// Sendet Daten-String zum AS400
        /// </summary>
        /// <param name="m">Daten-String/Message</param>
        private void SendToAS400(string m)
        {

            int reasonCode = 0;
            int compCode = 0;
            ElmMQDNet ty = new ElmMQDNet("MQConfig.xml");
            ty.Put(false, "Put00", "Test message", "", "USR", ref compCode, ref reasonCode);
            ty.Close();

        }
    }
}
