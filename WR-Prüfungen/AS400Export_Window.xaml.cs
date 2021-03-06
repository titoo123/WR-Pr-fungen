﻿using ElmMQDNetLib;
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
                string as400_Message = String.Empty;

                string string_Float_Format = "{0:00000.0000}";

                int success_Messages = 0;

                //Konvertieren des Datums
                d1 = D1_Picker.SelectedDate;
                d2 = D2_Picker.SelectedDate;

                //Datenbankabfrage der Matten mit betreffenden Datum die noch nicht exportiert wurden
                DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();

                var ase = from a in d.Prüfung
                          where a.Produktionsdatum >= d1 && a.Produktionsdatum <= d2 
                          && (a.Gesendet == false || a.Gesendet == null) 
                          && a.Art == "Standard"
                          select a;
                int i = ase.Count();
                //Schleife durchfährt alle betreffenden Prüfungen
                foreach (var m in ase)
                {
                    as400_Message = //as400_Message 
                        m.Produktionsdatum
                        + m.Charge.PadLeft(15)
                        //+ m.Bundnummer.PadLeft(15)

                        //+  (Helper.stringIsNull(m.Art)).PadLeft(15)
                        // +  (Helper.stringIsNull(m.Prüfer.Name)).PadLeft(15)

                        + String.Format(string_Float_Format, m.Du)
                        + String.Format(string_Float_Format, m.Dgs)
                        + String.Format(string_Float_Format, m.Re)
                        + String.Format(string_Float_Format, m.Rm)
                        + String.Format(string_Float_Format, m.RmRe)
                        + String.Format(string_Float_Format, m.Agt)
                        + String.Format(string_Float_Format, m.fR)
                        
                        ;
                    if (SendToAS400(as400_Message) == true)
                    {
                        success_Messages = success_Messages + 1;
                        m.Gesendet = true;

                        try
                        {
                            d.SubmitChanges();
                        }
                        catch (Exception)
                        {
                        }
                        
                    }
                }


                MessageBox.Show("Es wurden " + success_Messages + " / " + i + " Prüfungen erfolgreich übertragen!", "Erfolgreich!");
                this.Close();
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
        private bool SendToAS400(string m)
        {

            int reasonCode = 0;
            int compCode = 0;
            ElmMQDNet ty = new ElmMQDNet("MQConfig.xml");
            ty.Put(false, "Put", m,"", "USR", ref compCode, ref reasonCode);
            ty.Close();

            if (compCode == 0)
            {
                return true;
            }
            else if (compCode == 2)
            {
                MessageBox.Show("Verbindung fehlgeschlagen - Antwort vom Server! : " + reasonCode);
                return false;
            }
            else //if (reasonCode == 1)
            {
                MessageBox.Show("Unidentifizierte Antwort vom Server! : " + reasonCode);
                return false;
            }

        }
    }
}
