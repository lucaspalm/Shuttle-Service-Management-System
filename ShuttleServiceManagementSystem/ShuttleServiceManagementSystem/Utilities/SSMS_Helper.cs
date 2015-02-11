using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using SSMSDataModel.DAL;

namespace ShuttleServiceManagementSystem.Utilities
{
    public class SSMS_Helper
    {
        private SDSU_SchoolEntities db = new SDSU_SchoolEntities();

        /// <summary>
        /// This method creates a log record that is entered into the database for security purposes.
        /// </summary>
        public void CreateSystemLog()
        {
            // Variable Declarations
            string userID = "";
            string currentDate = "";
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Get the ID of the current user
            userID = HttpContext.Current.User.Identity.GetUserId();

            // Get the current datetime
            currentDate = DateTime.Now.ToString();

            // Create the query
            query = "INSERT INTO [SDSU_School].[4Moxie].[SYSTEM_LOGS] VALUES (@userid, @datetime)";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userID));
            paramList.Add(new SqlParameter("@datetime", currentDate));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            db.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// This method creates and returns a salt string used for hashing a user's password.
        /// </summary>
        /// <returns>string</returns>
        public string CreatePasswordSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[10];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// This method creats and returns a hashed string that is the result of hashing the user's password with a randomly generated salt value.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>string</returns>
        public string CreatePasswordHash(string password, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            SHA256Managed sha256hashString = new SHA256Managed();
            byte[] hashValue = sha256hashString.ComputeHash(bytes);

            return hashValue.ToString();
        }
    }
}