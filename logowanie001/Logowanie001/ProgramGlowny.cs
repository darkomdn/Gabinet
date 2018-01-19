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
    public partial class ProgramGlowny : Form
    {


        // Zmienne do połączenia z bazą MySQL

        private MySqlConnection conn;
        private string server;
        private string database;
        private string uid;
        private string password;

        // Deklaracja zmiennych

        int id_rekordu;      // zmienna dla DataGridView - wybór rekordu na którym pracuję (dla wszystkich siatek).
        int uzytkownik_typ;  // zmienna dla określenia typu użytkownika. 
        int id_usluga;       // wybranie usługi.
        int id_klient;       // wybranie klienta
        //  int id_pracownik;    // wybranie pracownika
        string data_w;       // wybór daty
        string godzina_w;    // wybór godziny

        public ProgramGlowny()
        {

            // Dane do połączenia z bazą MySQL (publiczne)
            server = "localhost";
            database = "gabinet";
            uid = "root";
            password = "DarX0305!";

            // Domyślny konektor z bazą danych - conn
            string connString;
            connString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            conn = new MySqlConnection(connString);


            InitializeComponent();
        }


        // Zamknięcie programu
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }


        // Funkcja zmieniająca przycisk - "Zmień hasło" na aktywne
        // tylko kiedy są wprowadzone w polu znaki oraz gdy
        // nowe hasło jest takie samo w dwóch polach.
        private void btnHZmien_pokaz()
        {
            if (txtHStareHaslo.Text != "" && txtHSNoweHaslo.Text != "" && txtHSNoweHaslo.Text == txtHSNoweHaslo2.Text)
            {
                btnZmianaHasla.Enabled = true;
            }
            else
            {
                btnZmianaHasla.Enabled = false;
            }


        }


        private void txtHStareHaslo_TextChanged(object sender, EventArgs e)
        {
            btnHZmien_pokaz();

        }

        private void txtHSNoweHaslo_TextChanged(object sender, EventArgs e)
        {
            btnHZmien_pokaz();

        }

        private void txtHSNoweHaslo2_TextChanged(object sender, EventArgs e)
        {
            btnHZmien_pokaz();

        }


        // Funkcja wyświetlająca dane z DataGridView - pracownicy.
        private void pokaz_siatke()
        {
            string PSzukaj = txtPSzukaj.Text;

            string query = $"SELECT id_uzytkownik,imie,nazwisko,uzytkownik_nazwa AS login, pracownik FROM uzytkownik ;";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();

            try
            {
                MySqlDataAdapter moja = new MySqlDataAdapter();
                moja.SelectCommand = cmd;
                DataTable tabela = new DataTable();
                moja.Fill(tabela);

                BindingSource zrodlo = new BindingSource();
                zrodlo.DataSource = tabela;
                dgUzytkownicy.DataSource = zrodlo;
                moja.Update(tabela);
            }



            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            conn.Close();

        }

        // Funkcja czyszczenia zawartości (TextBox)
        public void wyczysc(Control zbior)
        {
            foreach (Control c in zbior.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    ((TextBox)(c)).Text = string.Empty;
                }
            }
        }



        // Przycisk - Zmiana hasła 
        private void btnZmianaHasla_Click(object sender, EventArgs e)
        {
            string noweHaslo = txtHSNoweHaslo.Text;
            string stareHaslo = txtHStareHaslo.Text;


            string query = $"UPDATE uzytkownik SET haslo ='{noweHaslo}' WHERE id_uzytkownik = 1 AND haslo='{stareHaslo}';";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Zmieniłem hasło");
                conn.Close();
            }

            catch (Exception komunikat)
            {
                MessageBox.Show(komunikat.Message);

            }



        }


        // Funkcja wyszukująca i wyświetlająca dane w DataGridView
        private void btnPSzukaj_Click(object sender, EventArgs e)
        {

            szukaj_pracownikow(txtPSzukaj, dgUzytkownicy);

            // Wyłączenie wyświetlania kolumny id_uzytkownika.
            dgUzytkownicy.Columns[0].Visible = false;


        }


        // Funkcja "Szukaj pracowników"
        // Działa przez dwa argumanty - pole i siatka

        private void szukaj_pracownikow(System.Windows.Forms.TextBox pole, System.Windows.Forms.DataGridView siatka)
        {

            string query = $"SELECT id_uzytkownik,imie,nazwisko,uzytkownik_nazwa AS login, pracownik FROM uzytkownik WHERE CONCAT(imie,' ',nazwisko,uzytkownik_nazwa) LIKE '%{pole.Text}%';";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();

            try
            {
                MySqlDataAdapter moja = new MySqlDataAdapter();
                moja.SelectCommand = cmd;
                DataTable tabela = new DataTable();
                moja.Fill(tabela);

                BindingSource zrodlo = new BindingSource();
                zrodlo.DataSource = tabela;
                siatka.DataSource = zrodlo;
                moja.Update(tabela);

                conn.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }



        // Wstawienie danych z siatki do wartości tekstowych pól imie, nazwisko ...
        private void dgUzytkownicy_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                id_rekordu = Convert.ToInt32(dgUzytkownicy.Rows[e.RowIndex].Cells[0].Value);
                txtPImie.Text = dgUzytkownicy.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtPNazwisko.Text = dgUzytkownicy.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtPLogin.Text = dgUzytkownicy.Rows[e.RowIndex].Cells[3].Value.ToString();
                chbPPracownik.Checked = Convert.ToBoolean(dgUzytkownicy.Rows[e.RowIndex].Cells[4].Value);


                // Włączenie przycisków Modyfikuj, Usuń.
                btnPmodyfikuj.Enabled = true;
                btnPusun.Enabled = true;


                // Jeżeli osoba jest pracownikiem (lekarzem) to ma możliwość edycji
                // godzin pracy w poradni.
                if (chbPPracownik.Checked)
                {

                    string query = $"SELECT * FROM godziny WHERE id_uzytkownik = '{id_rekordu}'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    try
                    {
                        conn.Open();
                        MySqlDataReader dane = cmd.ExecuteReader();

                        while (dane.Read())
                        {
                            txtP1p.Text = dane.GetString("pon_od");
                            txtP1k.Text = dane.GetString("pon_do");
                            txtP2p.Text = dane.GetString("wt_od");
                            txtP2k.Text = dane.GetString("wt_do");
                            txtP3p.Text = dane.GetString("sr_od");
                            txtP3k.Text = dane.GetString("sr_do");
                            txtP4p.Text = dane.GetString("cz_od");
                            txtP4k.Text = dane.GetString("cz_do");
                            txtP5p.Text = dane.GetString("pt_od");
                            txtP5k.Text = dane.GetString("pt_do");
                            txtP6p.Text = dane.GetString("so_od");
                            txtP6k.Text = dane.GetString("so_do");
                        }

                        dane.Close();

                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }


                    conn.Close();

                }

            }

        }


        // Dodanie użytkownika do bazy.
        private void btnPDodaj_Click(object sender, EventArgs e)
        {

            if
           (txtPImie.Text.Length < 2 || txtPNazwisko.Text.Length < 2 || txtPLogin.Text.Length < 2)
            {
                MessageBox.Show("Proszę uzupełnić dane!");
            }
            else
            {

                if (chbPPracownik.Checked)
                {
                    uzytkownik_typ = 1;
                }
                else
                {
                    uzytkownik_typ = 0;
                }


                string query = $"INSERT INTO uzytkownik SET imie='{txtPImie.Text}', nazwisko='{txtPNazwisko.Text}', uzytkownik_nazwa='{txtPLogin.Text}',haslo='{txtPLogin.Text}',pracownik='{uzytkownik_typ}';";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                string query2 = $"INSERT INTO godziny SET id_uzytkownik=last_insert_id(),pon_od='{txtP1p.Text}',pon_do='{txtP1k.Text}',wt_od='{txtP2p.Text}',wt_do='{txtP2k.Text}',sr_od='{txtP3p.Text}',sr_do='{txtP3k.Text}',cz_od='{txtP4p.Text}',cz_do='{txtP4k.Text}',pt_od='{txtP5p.Text}',pt_do='{txtP5k.Text}',so_od='{txtP6p.Text}',so_do='{txtP6k.Text}';";
                MySqlCommand cmd2 = new MySqlCommand(query2, conn);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    if (chbPPracownik.Checked == true)
                    {
                        cmd2.ExecuteNonQuery();
                    }

                    conn.Close();

                    MessageBox.Show("Dodałem użytkownika.");
                }

                catch (Exception komunikat)
                {
                    MessageBox.Show(komunikat.Message);

                }

                pokaz_siatke();

            }
        }


        // Modyfikacja danych użytkownika
        private void btnPmodyfikuj_Click(object sender, EventArgs e)
        {

            if
        (txtPImie.Text.Length < 2 || txtPNazwisko.Text.Length < 2 || txtPLogin.Text.Length < 2)
            {
                MessageBox.Show("Proszę uzupełnić dane!");
            }
            else
            {

                if (chbPPracownik.Checked)
                {
                    uzytkownik_typ = 1;
                }
                else
                {
                    uzytkownik_typ = 0;
                }



                string query = $"SELECT * FROM godziny WHERE id_uzytkownik='{id_rekordu}';";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();

                string query2 = $"UPDATE uzytkownik SET imie='{txtPImie.Text}', nazwisko='{txtPNazwisko.Text}', uzytkownik_nazwa='{txtPLogin.Text}',pracownik='{uzytkownik_typ}' WHERE id_uzytkownik='{id_rekordu}';";
                MySqlCommand cmd2 = new MySqlCommand(query2, conn);
                cmd2.ExecuteNonQuery();

                conn.Close();
                try
                {

                    conn.Open();

                    MySqlDataReader dane = cmd.ExecuteReader();

                    conn.Close();
                    if (dane.HasRows == true && chbPPracownik.Checked == true)
                    {


                        conn.Open();
                        string query3 = $"UPDATE INTO godziny pon_od='{txtP1p.Text}',pon_do='{txtP1k.Text}',wt_od='{txtP2p.Text}',wt_do='{txtP2k.Text}',sr_od='{txtP3p.Text}',sr_do='{txtP3k.Text}',cz_od='{txtP4p.Text}',cz_do='{txtP4k.Text}',pt_od='{txtP5p.Text}',pt_do='{txtP5k.Text}',so_od='{txtP6p.Text}',so_do='{txtP6k.Text}' WHERE id_uzytkownik='{id_rekordu}';";
                        MySqlCommand cmd3 = new MySqlCommand(query3, conn);
                        cmd3.ExecuteNonQuery();
                        conn.Close();
                    }

                    else if (chbPPracownik.Checked == true)
                    {
                        conn.Open();
                        string query4 = $"INSERT INTO godziny SET id_uzytkownik='{id_rekordu}',pon_od='{txtP1p.Text}',pon_do='{txtP1k.Text}',wt_od='{txtP2p.Text}',wt_do='{txtP2k.Text}',sr_od='{txtP3p.Text}',sr_do='{txtP3k.Text}',cz_od='{txtP4p.Text}',cz_do='{txtP4k.Text}',pt_od='{txtP5p.Text}',pt_do='{txtP5k.Text}',so_od='{txtP6p.Text}',so_do='{txtP6k.Text}';";
                        MySqlCommand cmd4 = new MySqlCommand(query4, conn);
                        cmd4.ExecuteNonQuery();
                        conn.Close();
                    }


                }

                catch (Exception komunikat)
                {
                    MessageBox.Show(komunikat.Message);

                }
                MessageBox.Show("Dane użytkownika zostały zmodyfikowane.");
                pokaz_siatke();
            }
        }


        // Usuwanie użytkownika.
        private void btnPusun_Click(object sender, EventArgs e)
        {
            if
      (txtPImie.Text.Length < 2 || txtPNazwisko.Text.Length < 2 || txtPLogin.Text.Length < 2)
            {
                MessageBox.Show("Proszę uzupełnić dane!");
            }
            else
            {
                //  MessageBox.Show("Dane OK!");

                if (chbPPracownik.Checked)
                {
                    uzytkownik_typ = 1;
                }
                else
                {
                    uzytkownik_typ = 0;
                }


                try
                {
                    if (MessageBox.Show("Czy na pewno chcesz usunąć wybranego użytkownika?", "UWAGA !", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string query2 = $"DELETE FROM godziny WHERE id_uzytkownik='{id_rekordu}';";
                        MySqlCommand cmd2 = new MySqlCommand(query2, conn);

                        string query = $"DELETE FROM uzytkownik WHERE id_uzytkownik='{id_rekordu}';";
                        MySqlCommand cmd = new MySqlCommand(query, conn);

                        conn.Open();
                        cmd2.ExecuteNonQuery();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                    }

                }

                catch (Exception komunikat)
                {
                    MessageBox.Show(komunikat.Message);

                }
                MessageBox.Show("Użytkownik został usunięty.");
                pokaz_siatke();
                wyczysc(gbGodziny);
                wyczysc(gbUzytkownik);
            }
        }


        // Wyświetlenie godzin pracy tylko dla pracowników.
        private void chbPPracownik_CheckedChanged(object sender, EventArgs e)
        {
            if (chbPPracownik.Checked)
            {
                gbGodziny.Visible = true;
            }
            else
            {
                gbGodziny.Visible = false;

            }


            wyczysc(gbGodziny);

        }

        // Funkcja - "Szablon godzin"
        private void czas_pracy(int czasStart)
        {
            TextBox[] godziny_poczatek = { txtP1p, txtP2p, txtP3p, txtP4p, txtP5p, txtP6p };
            TextBox[] godziny_koniec = { txtP1k, txtP2k, txtP3k, txtP4k, txtP5k, txtP6k };
            for (int i = 0; i < 6; i++)
            {
                godziny_poczatek[i].Text = czasStart + ":00";
                godziny_koniec[i].Text = czasStart + 8 + ":00";

            }

        }


        // Wywołanie funkcji "Szablon godzin"
        private void btnG7_15_Click(object sender, EventArgs e)
        {

            czas_pracy(7);

        }

        private void btnG8_16_Click(object sender, EventArgs e)
        {
            czas_pracy(8);
        }

        private void btnG9_17_Click(object sender, EventArgs e)
        {
            czas_pracy(9);
        }

        private void btnG10_18_Click(object sender, EventArgs e)
        {
            czas_pracy(10);
        }


        // Funkcja - "Pokaż usługi"
        private void pokaz_uslugi(System.Windows.Forms.TextBox pole, System.Windows.Forms.DataGridView siatka)
        {

            string query = $"SELECT * FROM uslugi WHERE nazwa LIKE '%{pole.Text}%' ORDER BY nazwa;";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();

            try
            {
                MySqlDataAdapter moja = new MySqlDataAdapter();
                moja.SelectCommand = cmd;
                DataTable tabela = new DataTable();
                moja.Fill(tabela);

                BindingSource zrodlo = new BindingSource();
                zrodlo.DataSource = tabela;
                siatka.DataSource = zrodlo;
                moja.Update(tabela);
            }



            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Wyłączenie wyświetlania kolumny id_uzytkownika.
            siatka.Columns[0].Visible = false;

            conn.Close();



        }



        // Wyszukiwanie usług przez "Pokaż usługi"
        private void btnUSzukaj_Click(object sender, EventArgs e)
        {

            pokaz_uslugi(txtUSzukaj, dgUslugi);

        }


        // Dodawanie usługi
        private void btnUDodaj_Click(object sender, EventArgs e)
        {

            if
         (txtUNazwa.Text.Length < 2 || txtUCena.Text.Length < 1 || txtUCzas.Text.Length < 2)
            {
                MessageBox.Show("Proszę uzupełnić dane!");
            }
            else
            {
                string cena = txtUCena.Text.Replace(",", ".");
                string query = $"INSERT INTO uslugi SET nazwa='{txtUNazwa.Text}', cena= '{cena}', czas='{txtUCzas.Text}',opis='{txtUOpis.Text}';";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();


                    conn.Close();

                    MessageBox.Show("Usługa została dodana do bazy danych.");
                }

                catch (Exception komunikat)
                {
                    MessageBox.Show(komunikat.Message);

                }

                pokaz_uslugi(txtUSzukaj, dgUslugi);

            }

        }


        // Zdarzenie po kliknięciu w DataGridView - Usługi
        // Z siatki dynamicznie zostają przeniesione wartości do pól tekstowych.
        private void dgUslugi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                id_rekordu = Convert.ToInt32(dgUslugi.Rows[e.RowIndex].Cells[0].Value);
                txtUNazwa.Text = dgUslugi.Rows[e.RowIndex].Cells["nazwa"].Value.ToString();
                txtUCena.Text = dgUslugi.Rows[e.RowIndex].Cells["cena"].Value.ToString();
                txtUCzas.Text = dgUslugi.Rows[e.RowIndex].Cells["czas"].Value.ToString();
                txtUOpis.Text = dgUslugi.Rows[e.RowIndex].Cells["opis"].Value.ToString();

                btnUUsun.Enabled = true;
                btnUModyfikuj.Enabled = true;

            }


        }


        // Modyfikacja informacji o usłudze
        private void btnUModyfikuj_Click(object sender, EventArgs e)
        {
            if
                    (txtUNazwa.Text.Length < 2 || txtUCena.Text.Length < 1 || txtUCzas.Text.Length < 2)
            {
                MessageBox.Show("Proszę uzupełnić dane!");
            }

            else
            {

                conn.Open();

                string cena = txtUCena.Text.Replace(",", ".");
                string query = $"UPDATE uslugi SET nazwa='{txtUNazwa.Text}', cena= '{cena}', czas='{txtUCzas.Text}',opis='{txtUOpis.Text}' WHERE uslugi_id = '{id_rekordu}';";
                MySqlCommand cmd = new MySqlCommand(query, conn);


                try
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }

                catch (Exception komunikat)
                {
                    MessageBox.Show(komunikat.Message);

                }
                MessageBox.Show("Informacje o usłudze zostały zmodyfikowane.");
                pokaz_uslugi(txtUSzukaj, dgUslugi);
            }

        }



        // Usuwanie usługi
        private void btnUUsun_Click(object sender, EventArgs e)
        {

            try
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć daną usługę?", "UWAGA !", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {

                    string query = $"DELETE FROM uslugi WHERE uslugi_id='{id_rekordu}';";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                    conn.Close();

                }

            }

            catch (Exception komunikat)
            {
                MessageBox.Show(komunikat.Message);

            }
            MessageBox.Show("Usługa została usunięta.");
            pokaz_uslugi(txtUSzukaj, dgUslugi);
            wyczysc(gbUslugi);
        }


        // Wyświetlenie pracowników w dgPUPracownik przez funkcję "Szukaj pracowników"
        private void btnPUSzukaj_Click(object sender, EventArgs e)
        {
            szukaj_pracownikow(txtPUSzukaj, dgPUPracownik);
            dgPUPracownik.Columns[0].Visible = false;
        }


        // Wyszukanie usług, które wykonuje pracownik
        private void dgPUPracownik_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                id_rekordu = Convert.ToInt32(dgPUPracownik.Rows[e.RowIndex].Cells[0].Value);
                txtPUImie.Text = dgPUPracownik.Rows[e.RowIndex].Cells["imie"].Value.ToString();
                txtPUNazwisko.Text = dgPUPracownik.Rows[e.RowIndex].Cells["nazwisko"].Value.ToString();



                string query = $"SELECT uslugi.uslugi_id, uslugi.nazwa, uslugi.cena, uslugi.czas FROM uslugi, uzytkownik_usluga WHERE uslugi.uslugi_id = uzytkownik_usluga.uslugi_id AND uzytkownik_usluga.id_uzytkownik = '{id_rekordu}' ORDER BY nazwa;";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();

                try
                {
                    MySqlDataAdapter moja = new MySqlDataAdapter();
                    moja.SelectCommand = cmd;
                    DataTable tabela = new DataTable();
                    moja.Fill(tabela);

                    BindingSource zrodlo = new BindingSource();
                    zrodlo.DataSource = tabela;
                    dgPUUslugi.DataSource = zrodlo;
                    moja.Update(tabela);

                    conn.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string query2 = $"SELECT * FROM uslugi ORDER BY nazwa;";
                MySqlCommand cmd2 = new MySqlCommand(query2, conn);

                conn.Open();

                try
                {
                    MySqlDataAdapter moja = new MySqlDataAdapter();
                    moja.SelectCommand = cmd2;
                    DataTable tabela = new DataTable();
                    moja.Fill(tabela);

                    BindingSource zrodlo = new BindingSource();
                    zrodlo.DataSource = tabela;
                    dgPUUslugiNowe.DataSource = zrodlo;
                    moja.Update(tabela);

                    conn.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                dgPUUslugiNowe.Columns[0].Visible = false;
                dgPUUslugi.Columns[0].Visible = false;
            }

        }

        // Zdarzenie związane z naciśnięciem w siatce
        // Dodanie nowej usługi do usług wykonywanych przez pracownika.
        private void dgPUUslugiNowe_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                id_usluga = Convert.ToInt32(dgPUUslugiNowe.Rows[e.RowIndex].Cells[0].Value);

                if (MessageBox.Show("Czy chcesz dodać nową usługę pracownikowi?", "Uwaga!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {


                    try
                    {
                        string query3 = $"INSERT INTO uzytkownik_usluga SET id_uzytkownik= '{id_rekordu}', uslugi_id = '{id_usluga}';";
                        MySqlCommand cmd3 = new MySqlCommand(query3, conn);

                        conn.Open();
                        cmd3.ExecuteNonQuery();


                        conn.Close();

                        MessageBox.Show("Usługa została dodana do usług oferowanych przez pracownika.");
                    }

                    catch (Exception komunikat)
                    {
                        MessageBox.Show(komunikat.Message);

                    }

                }

                string query = $"SELECT uslugi.uslugi_id, uslugi.nazwa, uslugi.cena, uslugi.czas FROM uslugi, uzytkownik_usluga WHERE uslugi.uslugi_id = uzytkownik_usluga.uslugi_id AND uzytkownik_usluga.id_uzytkownik = '{id_rekordu}' ORDER BY nazwa;";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();

                try
                {
                    MySqlDataAdapter moja = new MySqlDataAdapter();
                    moja.SelectCommand = cmd;
                    DataTable tabela = new DataTable();
                    moja.Fill(tabela);

                    BindingSource zrodlo = new BindingSource();
                    zrodlo.DataSource = tabela;
                    dgPUUslugi.DataSource = zrodlo;
                    moja.Update(tabela);

                    conn.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                dgPUUslugi.Columns[0].Visible = false;

                string query2 = $"SELECT * FROM uslugi ORDER BY nazwa;";
                MySqlCommand cmd2 = new MySqlCommand(query2, conn);

                conn.Open();

                try
                {
                    MySqlDataAdapter moja = new MySqlDataAdapter();
                    moja.SelectCommand = cmd2;
                    DataTable tabela = new DataTable();
                    moja.Fill(tabela);

                    BindingSource zrodlo = new BindingSource();
                    zrodlo.DataSource = tabela;
                    dgPUUslugiNowe.DataSource = zrodlo;
                    moja.Update(tabela);

                    conn.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }


        // Usuwanie usługi z listy usług wykonywanych przez pracownika
        private void dgPUUslugi_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                id_usluga = Convert.ToInt32(dgPUUslugi.Rows[e.RowIndex].Cells[0].Value);

                if (MessageBox.Show("Czy chcesz usunąć usługę pracownikowi?", "Uwaga!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {


                    try
                    {
                        string query3 = $"DELETE FROM uzytkownik_usluga WHERE id_uzytkownik= '{id_rekordu}' AND uslugi_id = '{id_usluga}';";
                        MySqlCommand cmd3 = new MySqlCommand(query3, conn);

                        conn.Open();
                        cmd3.ExecuteNonQuery();


                        conn.Close();

                        MessageBox.Show("Usługa została usunięta z usług pracownika.");
                    }

                    catch (Exception komunikat)
                    {
                        MessageBox.Show(komunikat.Message);

                    }


                }

                string query = $"SELECT uslugi.uslugi_id, uslugi.nazwa, uslugi.cena, uslugi.czas FROM uslugi, uzytkownik_usluga WHERE uslugi.uslugi_id = uzytkownik_usluga.uslugi_id AND uzytkownik_usluga.id_uzytkownik = '{id_rekordu}' ORDER BY nazwa;";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();

                try
                {
                    MySqlDataAdapter moja = new MySqlDataAdapter();
                    moja.SelectCommand = cmd;
                    DataTable tabela = new DataTable();
                    moja.Fill(tabela);

                    BindingSource zrodlo = new BindingSource();
                    zrodlo.DataSource = tabela;
                    dgPUUslugi.DataSource = zrodlo;
                    moja.Update(tabela);

                    conn.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string query2 = $"SELECT * FROM uslugi ORDER BY nazwa;";
                MySqlCommand cmd2 = new MySqlCommand(query2, conn);

                conn.Open();

                try
                {
                    MySqlDataAdapter moja = new MySqlDataAdapter();
                    moja.SelectCommand = cmd2;
                    DataTable tabela = new DataTable();
                    moja.Fill(tabela);

                    BindingSource zrodlo = new BindingSource();
                    zrodlo.DataSource = tabela;
                    dgPUUslugiNowe.DataSource = zrodlo;
                    moja.Update(tabela);

                    conn.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }


        }

        // Funkcja szukaj klientów
        private void szukajKlientow(System.Windows.Forms.TextBox pole, System.Windows.Forms.DataGridView siatka)
        {

            string query = $"SELECT * FROM klienci WHERE CONCAT(imie,' ',nazwisko,' ', miejscowosc) LIKE '%{pole.Text}%' ORDER BY nazwisko;";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();

            try
            {
                MySqlDataAdapter moja = new MySqlDataAdapter();
                moja.SelectCommand = cmd;
                DataTable tabela = new DataTable();
                moja.Fill(tabela);

                BindingSource zrodlo = new BindingSource();
                zrodlo.DataSource = tabela;
                siatka.DataSource = zrodlo;
                moja.Update(tabela);

                conn.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        // Wywołanie funkcji szukaj klientów
        private void btnKSzukaj_Click(object sender, EventArgs e)
        {
            szukajKlientow(txtKSzukaj, dgKlienci);
            dgKlienci.Columns[0].Visible = false;
            dgKlienci.Columns["modyfikacja"].Visible = false;

        }


        // Dodanie nowego klienta do bazy
        private void btnKDodaj_Click(object sender, EventArgs e)
        {
            if
          (txtKImie.Text.Length < 2 || txtKNazwisko.Text.Length < 2)
            {
                MessageBox.Show("Proszę uzupełnić dane!");
            }
            else
            {


                string query = $"INSERT INTO klienci SET imie='{txtKImie.Text}', nazwisko='{txtKNazwisko.Text}', email='{txtKEmail.Text}',telefon='{txtKTelefon.Text}', ulica='{txtKUlica.Text}', numer='{txtKNumer.Text}', miejscowosc='{txtKMiejscowosc.Text}', poczta='{txtKKod.Text}';";
                MySqlCommand cmd = new MySqlCommand(query, conn);


                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();


                    conn.Close();

                    MessageBox.Show("Klient " + txtKImie.Text + " " + txtKNazwisko.Text + " został dodany do listy klientów.");
                }

                catch (Exception komunikat)
                {
                    MessageBox.Show(komunikat.Message);

                }

                szukajKlientow(txtKSzukaj, dgKlienci);
                dgKlienci.Columns[0].Visible = false;
                dgKlienci.Columns["modyfikacja"].Visible = false;
            }


        }

        private void btnRSzukajU_Click(object sender, EventArgs e)
        {
            pokaz_uslugi(txtRSzukajU, dgRUslugi);
            dgRUslugi.Columns[0].Visible = false;
        }

        private void btnRSzukajK_Click(object sender, EventArgs e)
        {
            szukajKlientow(txtRSzukajK, dgRKlienci);
            dgRKlienci.Columns[0].Visible = false;
            dgRKlienci.Columns["modyfikacja"].Visible = false;
        }

        private void btnRSzukajP_Click(object sender, EventArgs e)
        {
            szukaj_pracownikow(txtRSzukajP, dgRPracownik);
            dgRPracownik.Columns[0].Visible = false;
        }

        private void dgRUslugi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                id_rekordu = Convert.ToInt32(dgRUslugi.Rows[e.RowIndex].Cells[0].Value);
                txtUslugaW.Text = dgRUslugi.Rows[e.RowIndex].Cells["nazwa"].Value.ToString();


            }
        }

        private void dgRKlienci_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
              
                txtKlientW.Text = dgRKlienci.Rows[e.RowIndex].Cells["imie"].Value.ToString() + " " + dgRKlienci.Rows[e.RowIndex].Cells["nazwisko"].Value.ToString();
                id_klient = Convert.ToInt32(dgRKlienci.Rows[e.RowIndex].Cells[0].Value);

            }

        }

        private void dgRPracownik_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            String czas;
            int licznik = 0;
            //  String praca_od;
            //  String praca_do;
            //  int g_start;
            //  int g_stop;

            DateTime dzien_tygodnia = Convert.ToDateTime(data_w);
            int dzien = Convert.ToInt16(dzien_tygodnia.DayOfWeek);


            /*
            gbRGodziny.Controls.Clear();


            if (e.RowIndex >= 0)
            {
                id_pracownik = Convert.ToInt32(dgRPracownik.Rows[e.RowIndex].Cells[0].Value);

            }

            switch (dzien)
            {
                case 1:
                    praca_od = "pon_od";
                    praca_do = "pon_do";
                    break;
                case 2:
                    praca_od = "wt_od";
                    praca_do = "wt_do";
                    break;
                case 3:
                    praca_od = "sr_od";
                    praca_do = "sr_do";
                    break;
                case 4:
                    praca_od = "cz_od";
                    praca_do = "cz_do";
                    break;
                case 5:
                    praca_od = "pt_od";
                    praca_do = "pt_do";
                    break;
                case 6:
                    praca_od = "so_od";
                    praca_do = "so_do";
                    break;
                case 0:
                    MessageBox.Show("Wybrany pracownik nie pracuje w niedzielę! \n Wyświetlam poniedziałek.");

                    praca_od = "pon_od";
                    praca_do = "pon_do";
                    break;
            }

            String query = $"SELECT date_format( 'praca_od', '%H') AS g_start, date_format( 'praca_do', '%H') AS g_stop FROM godziny WHERE id_uzytkownik = '{id_pracownik}'";
            MySqlCommand cmd = new MySqlCommand(query, conn);


            conn.Open();
            MySqlDataReader dane = cmd.ExecuteReader();

            // if (dane.HasRows)
            //  {
            g_start = Convert.ToInt32(dane.GetInt32("g_start"));
            g_stop = Convert.ToInt32(dane.GetInt32("g_stop"));
           //  }

           

                    for (int g = g_start; g<=g_stop; g++)
                    */



            for (int g = 7; g <= 16; g++)
            {
                for (int m = 0; m < 60; m += 30)
                {
                    czas = txtRTerminW.Text + " " + g + ":" + m + ":00";

                    DateTime godzina_r = Convert.ToDateTime(czas);


                    System.Windows.Forms.TextBox poleGodziny = new System.Windows.Forms.TextBox();
                    gbRGodziny.Controls.Add(poleGodziny);
                    poleGodziny.Width = 120;
                    poleGodziny.Top = 22 * licznik;
                    poleGodziny.Left = 20;
                    poleGodziny.Text = String.Format(godzina_r.ToShortTimeString());
                    poleGodziny.Click += new System.EventHandler(this.poleClick);
                    licznik++;


                }

            }

        }

        private void poleClick(object sender, EventArgs e)
        {
            TextBox pole = (TextBox)sender;
            godzina_w = pole.Text;
            txtRTerminW.Text = data_w + " " + godzina_w;
        }


        //Wybór daty
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            data_w = string.Format(e.Start.ToShortDateString());
            txtRTerminW.Text = data_w;
        }


        // Dodanie rezerwacji dla klienta
        private void btnRDodaj_Click(object sender, EventArgs e)
        {

            if
         (txtUslugaW.Text.Length < 2 || txtKlientW.Text.Length < 2 || txtRTerminW.Text.Length <2)
            {
                MessageBox.Show("Proszę uzupełnić dane!");
            }
            else
            {


                string query = $"INSERT INTO wizyty SET klienci_id='{id_klient}', uslugi_id='{id_usluga}', id_uzytkownik='{uzytkownik_typ}';";
                MySqlCommand cmd = new MySqlCommand(query, conn);


                try
                {
                //    conn.Open();
                  //  cmd.ExecuteNonQuery();


                   /// conn.Close();

                    MessageBox.Show("Usługa dla " + txtKlientW.Text + " została wprowadzona.");
                }

                catch (Exception komunikat)
                {
                    MessageBox.Show(komunikat.Message);

                }

                szukajKlientow(txtKSzukaj, dgKlienci);
                dgKlienci.Columns[0].Visible = false;
                dgKlienci.Columns["modyfikacja"].Visible = false;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
    }




        
    

