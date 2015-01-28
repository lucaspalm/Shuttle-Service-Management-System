using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Net;
using System.Web.UI;
using System.Xml;
using System.Data;

namespace ShuttleServiceManagementSystem.Utilities
{
    public class Database_Helper
    {
        public string GetConnString()
        {
            return "Data Source=edgeofthemists.net;Initial Catalog=SDSU_School;Integrated Security=False;User ID=4Moxie;Password=jacks";
        }

        public string ReturnSQLFirstValue(string query)
        {
            // This function will return a single value from the top of a query.  
            // This is useful for doing quick and dirty queries to pull out an individual value.
            string value = "";

            using (SqlConnection conn1 = new SqlConnection(GetConnString()))
            {
                // Create new commands.
                SqlCommand cmd = new SqlCommand(query, conn1);
                SqlDataReader reader = null;

                try
                {
                    // Open connection.
                    conn1.Open();

                    // Read in from the database.
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    // Convert value.
                    value = Convert.ToString(reader[0]);
                }
                catch
                {
                    value = "";
                }
                finally
                {
                    reader.Dispose();
                    cmd.Dispose();
                    conn1.Close();
                }

            }

            return value;
        }

        public bool RunEmptySQLQuery(string query)
        {
            bool validQuery = false;

            using (SqlConnection conn1 = new SqlConnection(GetConnString()))
            {
                // Create Commands.
                SqlCommand cmd = new SqlCommand(query, conn1);

                // Open connection, run the query, exit.
                try
                {
                    conn1.Open();
                    cmd.ExecuteNonQuery();
                    validQuery = true;
                }
                catch
                {
                    validQuery = false;
                }
                finally
                {
                    conn1.Close();
                }

                return validQuery;
            }
        }

        public DataTable ReturnTable(string query)
        {
            DataTable result = new DataTable();

            using (SqlConnection conn1 = new SqlConnection(GetConnString()))
            {
                SqlCommand cmd = new SqlCommand(query, conn1);
                SqlDataReader reader = null;

                try
                {
                    conn1.Open();
                    reader = cmd.ExecuteReader();
                    result.Load(reader);
                }
                catch
                {
                    result = null;
                }
                finally
                {
                    reader.Dispose();
                    cmd.Dispose();
                    conn1.Close();
                }
            }

            return result;
        }
    }
}