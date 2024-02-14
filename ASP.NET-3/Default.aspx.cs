using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace ASP.NET_3
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string ConnectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();
                string queryPrenotazioni = "SELECT * FROM Prenotazioni";
                SqlCommand cmdPrenotazioni = new SqlCommand(queryPrenotazioni, conn);

                SqlDataReader readerPrenotazioni = cmdPrenotazioni.ExecuteReader();

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
                risultato.InnerText = "Errore: " + ex.Message;
            }

            finally
            {
                conn.Close();
            }

            try
            {
                conn.Open();

                string queryCountPrenotazioni = "SELECT sala,Ridotto, COUNT(*) AS nPrenotazioni FROM Prenotazioni  GROUP BY sala,Ridotto ORDER BY Sala ASC";
                SqlCommand cmdCount = new SqlCommand(queryCountPrenotazioni, conn);

                SqlDataReader readerCount = cmdCount.ExecuteReader();

                while (readerCount.Read())
                {
                    countSala.InnerHtml += "<p>";
                    for (int i = 0; i < readerCount.FieldCount; i++)
                    {
                        countSala.InnerHtml += $"{readerCount.GetName(i)}: {readerCount.GetValue(i)} ";
                    }
                    countSala.InnerHtml += "</p>";
                }

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

        protected void Prenota_Click(object sender, EventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();
                string query = "INSERT INTO Prenotazioni (Nome, Cognome, Sala,Ridotto) VALUES (@Nome, @Cognome, @Sala, @Ridotto)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", nome.Text);
                cmd.Parameters.AddWithValue("@Cognome", cognome.Text);
                cmd.Parameters.AddWithValue("@Sala", sala.Text);
                cmd.Parameters.AddWithValue("@Ridotto", ridotto.Checked);

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
    }
}