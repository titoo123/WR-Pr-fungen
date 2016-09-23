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
    /// Interaktionslogik für Nachprüfung_Frage_Window.xaml
    /// </summary>
    public partial class Nachprüfung_Frage_Window : Window
    {
        Prüfung p_old;
        Prüfung p_new;

        bool choice = false;

        public Nachprüfung_Frage_Window(Prüfung p_old, Prüfung p_new)
        {
            InitializeComponent();
            
            this.p_old = p_old;
            this.p_new = p_new;

            //Daten der neuen Prüfung
            textBox_Agt_New.Text = Convert.ToString( p_new.Agt );
            textBox_P_New.Text = Convert.ToString(p_new.Bundnummer);
            textBox_Re_New.Text = Convert.ToString(p_new.Re);
            textBox_Rm_New.Text = Convert.ToString(p_new.Rm);
            //Daten der alten Prüfung
            textBox_Agt_Old.Text = Convert.ToString(p_old.Agt);
            textBox_P_Old.Text = Convert.ToString(p_old.Bundnummer);
            textBox_Re_Old.Text = Convert.ToString(p_old.Re);
            textBox_Rm_Old.Text = Convert.ToString(p_old.Rm);
        }

        private void button_P_Ersetzen_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            d.Prüfung.DeleteOnSubmit(p_old);

            try
            {
                choice = true;
                d.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Keine Verbindung zur Datenbank!","Fehler!");
            }
            finally
            {
                Close();
            }
        }

        private void button_P_Behalten_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext d = new DatabaseConnectionDataContext();
            d.Prüfung.DeleteOnSubmit(p_new);

            try
            {
                choice = true;
                d.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Keine Verbindung zur Datenbank!", "Fehler!");
            }
            finally
            {
                Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!choice)
            {
                e.Cancel = true;
            }

        }
    }
}
