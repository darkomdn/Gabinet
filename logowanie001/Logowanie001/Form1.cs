using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Logowanie001
{
    public partial class Form1 : Form
    {

        // Zmienne do połączenia z bazą MySQL

        private MySqlConnection conn;
        private string server;
        private string database;
        private string uid;
        private string password;

        
        public Form1()
        {

            // Dane do połączenia z bazą MySQL (publiczne)

            server = "localhost";
            database = "gabinet";
            uid = "root";
            password = "DarX0305!";

            string connString;
            connString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            conn = new MySqlConnection(connString);



            InitializeComponent();
        }

        public void zaloguj_Click(object sender, EventArgs e)
        {

            string user = tbUser.Text;
            string pass = tbPass.Text;

            if (isLogin(user, pass))
            {
                    
                ProgramGlowny gabinet = new ProgramGlowny();
                gabinet.Show();
                this.Hide();
                //  this.Close();

            }
            else
            {
                MessageBox.Show($"{user} nie istnieje takie konto w bazie");

            }


        }


        public bool isLogin(string user, string pass)
        {
            string query = $"SELECT id_uzytkownik FROM uzytkownik WHERE uzytkownik_nazwa='{user}' AND haslo='{pass}';";

           
           

            try
            {

                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
               //     MySqlDataReader odczyt = cmd.ExecuteReader();
               //      int uzytkownik_id = odczyt.GetInt32(0);

                  

                    if (reader.Read())
                    {
                        reader.Close();
                        conn.Close();
                        return true;

                    }

                    else
                    {
                        reader.Close();
                        conn.Close();
                        return false;

                    }

                }

                else
                {
                    conn.Close();
                    return false;
                }
            } catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message);
                return false;

            }
     
        }

        
        private bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Błąd połączenia z serwerem!");
                        break;
                    case 1045:
                        MessageBox.Show("Błędna nazwa użytkownika lub hasła!");
                        break; 
                }
                return false;
            }


        }


        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnZaloguj2_Click(object sender, EventArgs e)
        {

            string user = tbUser.Text;
            string pass = tbPass.Text;



            string query = $"SELECT id_uzytkownik FROM uzytkownik WHERE uzytkownik_nazwa='{user}' AND haslo='{pass}';";
           
            MySqlCommand cmd = new MySqlCommand(query, conn);

            try
            {
                conn.Open();

               
                cmd.ExecuteNonQuery();
             
                conn.Close();

                ProgramGlowny gabinet = new ProgramGlowny();
                gabinet.Show();
                this.Hide();
            }

            catch (Exception komunikat)
            {
                MessageBox.Show(komunikat.Message);

            }





        }
    }
}
