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
    /// Interaktionslogik für Kunde_Window.xaml
    /// </summary>
    public partial class Kunde_Window : Window
    {
        bool neu;
        public Kunde_Window()
        {
            InitializeComponent();
            Helper.IsEnabledTextBoxes(Grid_Kunden, false);
            DataGrid_Load();
        }

        public Kunde_Window(int v) 
        {
            InitializeComponent();
            Helper.IsEnabledTextBoxes(Grid_Kunden, false);
            DataGrid_Load();

            for (int i = 0; i < dataGrid_Kunde.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)dataGrid_Kunde.ItemContainerGenerator.ContainerFromIndex(i);
                if (row != null)
                {
                    TextBlock cellContent = dataGrid_Kunde.Columns[0].GetCellContent(row) as TextBlock;

                    if (v == Convert.ToInt32(cellContent))
                    {
                        object item = dataGrid_Kunde.Items[i];
                        dataGrid_Kunde.SelectedItem = item;
                        dataGrid_Kunde.ScrollIntoView(item);
                        row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        break;
                    }
                }

            }
        }

        private void button_kunde_neu_Click(object sender, RoutedEventArgs e)
        {
            neu = true;
            button_kunde_speichern.IsEnabled = true;

            Helper.ClearTextBoxes(this);
            Helper.IsEnabledTextBoxes(this, true);
        }

        private void button_kunde_bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            neu = false;
            if (dataGrid_Kunde.SelectedIndex != -1)
            {
                Helper.IsEnabledTextBoxes(Grid_Kunden, true);
                button_kunde_speichern.IsEnabled = true;
        
            }

        }

        private void button_kunde_speichern_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            if (neu)
            {
                try
                {
                    Kunde n_kunde = new Kunde()
                    {
                     Ansprechnpartner = textBox_Ansprechpartner.Text,
                     EMail = textBox_EMail.Text,
                     Firma = textBox_Firma.Text,
                     Land = textBox_Land.Text,
                     Mobil = textBox_Mobil.Text,
                     PLZ = textBox_PLZ.Text,
                     Sonstige = textBox_Sonstiges.Text,
                     Stadt = textBox_Stadt.Text,
                     Straße = textBox_Straße.Text,
                     Telefon = textBox_Telefon.Text,
                     Web = textBox_Web.Text
                    };

                    d.Kunde.InsertOnSubmit(n_kunde);
                    d.SubmitChanges();

                    Helper.IsEnabledTextBoxes(Grid_Kunden, false);
                    button_kunde_speichern.IsEnabled = false;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kunde konnte nicht gespeichert werden!/n" + ex,"Fehler!");
                }
                
            }
            else
            {
                try
                {
                    Kunde b_kunde = (from x in d.Kunde
                                     where x.Id == Helper.GetIntFromDataGrid(dataGrid_Kunde, 0)
                                     select x).First();

                    b_kunde.Ansprechnpartner = textBox_Ansprechpartner.Text;
                    b_kunde.EMail = textBox_EMail.Text;
                    b_kunde.Firma = textBox_Firma.Text;
                    b_kunde.Land = textBox_Land.Text;
                    b_kunde.Mobil = textBox_Mobil.Text;
                    b_kunde.PLZ = textBox_PLZ.Text;
                    b_kunde.Sonstige = textBox_Sonstiges.Text;
                    b_kunde.Stadt = textBox_Stadt.Text;
                    b_kunde.Straße = textBox_Straße.Text;
                    b_kunde.Telefon = textBox_Telefon.Text;
                    b_kunde.Web = textBox_Web.Text;
                    
                    d.SubmitChanges();

                    Helper.IsEnabledTextBoxes(Grid_Kunden, false);
                    button_kunde_speichern.IsEnabled = false;
            }
                catch (Exception)
            {
                MessageBox.Show("Kunde konnte nicht gespeichert werden!", "Fehler!");
            }
        }
            DataGrid_Load();

        }

        private void button_kunde_löschen_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            
            try
            {
                Kunde k = (from x in d.Kunde
                          where x.Id == Helper.GetIntFromDataGrid(dataGrid_Kunde, 0)
                          select x).First();

                d.Kunde.DeleteOnSubmit(k);
                d.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Kunde nicht gefunden! Konnte nicht gelöscht werden!","Fehler!");
            }
            Helper.ClearTextBoxes(Grid_Kunden);
            DataGrid_Load();
        }

        private void dataGrid_Kunde_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_Kunde.SelectedIndex != -1)
            {
                DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

                try
                {
                    Kunde k = (from x in d.Kunde
                               where x.Id == Helper.GetIntFromDataGrid(dataGrid_Kunde, 0)
                               select x).First();

                    textBox_Ansprechpartner.Text = k.Ansprechnpartner;
                    textBox_EMail.Text = k.EMail;
                    textBox_Firma.Text = k.Firma;
                    textBox_Land.Text = k.Land;
                    textBox_Mobil.Text = k.Mobil;
                    textBox_PLZ.Text = k.PLZ;
                    textBox_Sonstiges.Text = k.Sonstige;
                    textBox_Stadt.Text = k.Stadt;
                    textBox_Straße.Text = k.Straße;
                    textBox_Telefon.Text = k.Telefon;
                    textBox_Web.Text = k.Web;
               
                }
                catch (Exception)
                {
                    MessageBox.Show("Kunde nicht gefunden!", "Fehler!");
                }
                button_kunde_bearbeiten.IsEnabled = true;
            }
            else
            {
                Helper.ClearTextBoxes( Grid_Kunden );
            }
        }

        private void DataGrid_Load() {

            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

            try
            {
                var k = from x in d.Kunde
                        select new { x.Id, x.Firma, x.Land, x.Ansprechnpartner };
                dataGrid_Kunde.ItemsSource = k;
                
            }
            catch (Exception)
            {
                MessageBox.Show("Kunden konnten nicht geladen werden!", "Fehler!");
            }
        }
    }
}
