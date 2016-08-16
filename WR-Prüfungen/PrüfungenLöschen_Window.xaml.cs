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
    /// Interaktionslogik für PrüfungenLöschen_Window.xaml
    /// </summary>
    public partial class PrüfungenLöschen_Window : Window
    {
        private MainWindow mainWindow;

        public PrüfungenLöschen_Window()
        {
            InitializeComponent();
        }

        public PrüfungenLöschen_Window(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void button_ExportierenUndSchließen_Click(object sender, RoutedEventArgs e)
        {
            DateTime? d1 = D1_Picker.SelectedDate;
            DateTime? d2 = D2_Picker.SelectedDate;

            if (d1 != null && d2 != null)
            {
                DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

                var pre = from p in d.Prüfung
                          where p.Datum >= d1 && p.Datum <= d2
                          select p;
                int i = pre.Count();
                d.Prüfung.DeleteAllOnSubmit(pre);

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Datenbank konnte nicht erreicht werden!", "Fehler!");
                }
                mainWindow.LoadDatagridData();
                this.Close();
            }
            else
            {
                MessageBox.Show("Bitte wählen sie einen Bereich für das Datum aus!", "Achtung!");
            }

        }
    }
}
