using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace WR_Prüfungen
{
    /// <summary>
    /// Interaktionslogik für ExcelExport_Window.xaml
    /// </summary>
    public partial class ExcelExport_Window : Window
    {
        public ExcelExport_Window()
        {
            InitializeComponent();
        }

        private void button_ExportierenUndSchließen_Click(object sender, RoutedEventArgs e)
        {
            DateTime? d1 = D1_Picker.SelectedDate;
            DateTime? d2 = D2_Picker.SelectedDate;

            if (d1 != null && d2 != null)
            {
                switch (((ComboBoxItem)comboBox_Modus.SelectedItem).Content.ToString())
                {
                    case "Standard":
                        new CreateExcelSheet(d1, d2, true, false, "no_mode");
                        Close();
                        break;
                    case "Ungereckt":
                        new CreateExcelSheet(d1, d2, true, false, "Ungereckt");
                        Close();
                        break;
                    case "Stäbe":
                        new CreateExcelSheet(d1, d2, true, false, "Stäbe");
                        Close();
                        break;
                    case "Fremdprüfungen":
                        if (dataGrid_Kunden.Items != null)
                        {
                            new CreateWordSheet(d1,d2, Helper.GetIntFromDataGrid(dataGrid_Kunden, 0));
                            //new CreateExcelSheet(d1, d2, true, false, "Kunde ", ReadDataGrid(dataGrid_Kunden,0));
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Keinen gültigen Kunden gefunden!","Achtung!");
                        }

                        break;
                    default:
                        MessageBox.Show("ComboBox hat keine eindeutige Auswahl!", "Fehler!");
                        break;
                }

            }
            else
            {
                MessageBox.Show("Bitte wählen sie einen Bereich für das Datum aus!", "Achtung!");
            }

        }

        private void comboBox_Modus_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)comboBox_Modus.SelectedItem).Content.ToString() == "Fremdprüfungen")
            {
                this.Height = this.Height + 100;
                this.dataGrid_Kunden.Height = 100;

                this.Height = this.Height + 50;
                this.textBox_KundenNr.Height = 50;

                this.textBox_KundenNr.Text = "Hier Kundennummer eingeben!";

            }
            else if (this.Height != 150)
            {
                this.Height = 150;
                this.dataGrid_Kunden.Height = 0;
                this.textBox_KundenNr.Height = 0;

                this.textBox_KundenNr.Text = String.Empty;
            }




        }

        private void textBox_KundenNr_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox_KundenNr.Text != String.Empty && !textBox_KundenNr.Text.Contains("Hier"))
            {
                int value = 0;


                if (Int32.TryParse(textBox_KundenNr.Text, out value))
                {

                    DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

                    var ksu = from x in d.Kunde
                              where x.Id == value
                              select new { x.Id, x.Firma, x.Land };

                    dataGrid_Kunden.ItemsSource = ksu;
                }
                else
                {
                    MessageBox.Show("Bitte geben sie eine gültige Zahl ein!", "Fehler!");
                }
            }

        }
    }
}
