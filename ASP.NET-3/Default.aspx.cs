using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace ASP.NET_3
{
    public partial class _Default : Page
    {
        // dichiaro tre variabili per tenere traccia del numero di prenotazioni per ogni sala
        int CountSalaNord;
        int CountSalaSud;
        int CountSalaEst;

        protected void Page_Load(object sender, EventArgs e)
        {
            // creo la connessione al database tramite la stringa di connessione
            string ConnectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                // apro la connessione al database e seleziono tutte le prenotazioni
                conn.Open();
                string queryPrenotazioni = "SELECT * FROM Prenotazioni";
                SqlCommand cmdPrenotazioni = new SqlCommand(queryPrenotazioni, conn);

                // eseguo la query e salvo i risultati in un SqlDataReader
                SqlDataReader readerPrenotazioni = cmdPrenotazioni.ExecuteReader();

                // scorro i risultati e li stampo a schermo
                while (readerPrenotazioni.Read())
                {
                    listaPrenotazioni.InnerHtml += "<p>";
                    for (int i = 0; i < readerPrenotazioni.FieldCount; i++)
                    {
                        listaPrenotazioni.InnerHtml += $"{readerPrenotazioni.GetName(i)}: {readerPrenotazioni.GetValue(i)} ";
                    }
                    listaPrenotazioni.InnerHtml += "</p>";
                }
            }

            catch (Exception ex)
            {
                // in caso di errore stampo un messaggio di errore
                risultato.InnerText = "Errore: " + ex.Message;
            }

            finally
            {
                // chiudo la connessione al database
                conn.Close();
            }

            try
            {
                conn.Open();
                // seleziono il numero di prenotazioni per ogni sala e per ogni tipo di biglietto
                // e li ordino per sala in ordine crescente
                string queryCountPrenotazioni = "SELECT sala,Ridotto, COUNT(*) AS nPrenotazioni FROM Prenotazioni  GROUP BY sala,Ridotto ORDER BY Sala ASC";
                SqlCommand cmdCount = new SqlCommand(queryCountPrenotazioni, conn);

                // eseguo la query e salvo i risultati in un SqlDataReader
                SqlDataReader readerCount = cmdCount.ExecuteReader();


                while (readerCount.Read())
                {
                    // controllo a quale sala appartiene la prenotazione e aggiorno il contatore
                    string sala = readerCount["sala"].ToString();
                    int count = Convert.ToInt32(readerCount["nPrenotazioni"]);

                    switch (sala)
                    {
                        case "SALA NORD":
                            CountSalaNord += count;
                            break;
                        case "SALA SUD":
                            CountSalaSud += count;
                            break;
                        case "SALA EST":
                            CountSalaEst += count;
                            break;
                    }
                    // stampo a schermo i risultati
                    countSala.InnerHtml += "<p>";
                    for (int i = 0; i < readerCount.FieldCount; i++)
                    {
                        countSala.InnerHtml += $"{readerCount.GetValue(i)} ";
                    }
                    countSala.InnerHtml += "</p>";
                }
                countSala.InnerHtml += $"SALA NORD-{CountSalaNord}  SALA SUD-{CountSalaSud}  SALA EST-{CountSalaEst} ";

            }
            catch (Exception ex)
            {
                risultato.InnerText = "Errore: " + ex.Message;
            }
            finally
            {
                conn.Close();
            }

        }
        // metodo per prenotare un posto in sala 
        // eseguo una query di inserimento nel database
        // e stampo un messaggio di conferma
        protected void Prenota_Click(object sender, EventArgs e)
        {
            // controllo se la sala è piena 
            if (sala.Text == "SALA NORD" && CountSalaNord < 120)
            {
                // Inserisci la prenotazione
                InsertPrenotazione(nome.Text, cognome.Text, sala.Text, ridotto.Checked);
            }
            else if (sala.Text == "SALA EST" && CountSalaEst < 120)
            {
                InsertPrenotazione(nome.Text, cognome.Text, sala.Text, ridotto.Checked);
            }
            else if (sala.Text == "SALA SUD" && CountSalaSud < 120)
            {
                InsertPrenotazione(nome.Text, cognome.Text, sala.Text, ridotto.Checked);
            }
            else
            {
                // stampo un messaggio di errore
                risultato.InnerText = "La sala è piena. Non è possibile effettuare la prenotazione.";
            }

        }
        // metodo per inserire una prenotazione nel database
        protected void InsertPrenotazione(string nome, string cognome, string sala, bool ridotto)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();
                string query = "INSERT INTO Prenotazioni (Nome, Cognome, Sala,Ridotto) VALUES (@Nome, @Cognome, @Sala, @Ridotto)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", nome);
                cmd.Parameters.AddWithValue("@Cognome", cognome);
                cmd.Parameters.AddWithValue("@Sala", sala);
                cmd.Parameters.AddWithValue("@Ridotto", ridotto);

                cmd.ExecuteNonQuery();

                risultato.InnerText = "Prenotazione effettuata con successo!";
            }
            catch (Exception ex)
            {
                risultato.InnerText = "Errore: " + ex.Message;
            }
            finally
            {
                conn.Close();
                Response.Redirect(Request.RawUrl);

            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();
                string query = "DELETE FROM Prenotazioni";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                risultato.InnerText = "Prenotazioni cancellate con successo!";
            }
            catch (Exception ex)
            {
                risultato.InnerText = "Errore: " + ex.Message;
            }
            finally
            {
                conn.Close();
                Response.Redirect(Request.RawUrl);
            }

        }
    }
}