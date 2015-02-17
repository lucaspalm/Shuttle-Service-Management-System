using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using SSMSDataModel.DAL;
using System.Net.Mail;
using System.Net;

namespace ShuttleServiceManagementSystem.Utilities
{
    public class SSMS_Helper
    {
        private SDSU_SchoolEntities db = new SDSU_SchoolEntities();

        /// <summary>
        /// This method creates a log record that is entered into the database for security purposes.
        /// </summary>
        public void CreateSystemLog(string username)
        {
            // Variable Declarations
            string userID = "";
            string currentDate = "";
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Get the ID of the current user
            userID = GetUserID(username);

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
        /// This method will get the USER_ID for the passed in username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>string</returns>
        public string GetUserID(string username)
        {
            // Variable Declarations
            string userID = "";
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT USER_ID FROM [SDSU_School].[4Moxie].[USER_ACCOUNTS] WHERE UserName = @username";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@username", username));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            userID = db.Database.SqlQuery<string>(query, parameters).FirstOrDefault<string>();

            return userID;
        }

        /// <summary>
        /// This method will insert a new record into the USER_INFO table.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="streetAddress"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zipCode"></param>
        /// <param name="emailAddress"></param>
        /// <param name="cellNumber"></param>
        /// <param name="receiveText"></param>
        /// <param name="receiveEmail"></param>
        public void InsertNewUserInfo(string userID, string firstName, string lastName, string streetAddress, string city, string state, string zipCode, string emailAddress, string cellNumber, string receiveText, string receiveEmail)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "INSERT INTO [SDSU_School].[4Moxie].[USER_INFO] VALUES (@userid, @firstname, @lastname, @streetaddress, @city, @state, @zipcode, @emailaddress, @cellnumber, @receivetext, @receiveemail)";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userID));
            paramList.Add(new SqlParameter("@firstname", firstName));
            paramList.Add(new SqlParameter("@lastname", lastName));
            paramList.Add(new SqlParameter("@streetaddress", streetAddress));
            paramList.Add(new SqlParameter("@city", city));
            paramList.Add(new SqlParameter("@state", state));
            paramList.Add(new SqlParameter("@zipcode",zipCode ));
            paramList.Add(new SqlParameter("@emailaddress", emailAddress));
            paramList.Add(new SqlParameter("@cellnumber", cellNumber));
            paramList.Add(new SqlParameter("@receivetext", receiveText));
            paramList.Add(new SqlParameter("@receiveemail", receiveEmail));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            db.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// This method will update the specified users profile information in the USER_INFO table.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="streetAddress"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zipCode"></param>
        /// <param name="emailAddress"></param>
        /// <param name="cellNumber"></param>
        /// <param name="receiveText"></param>
        /// <param name="receiveEmail"></param>
        public void UpdateUserInfo(string userID, string firstName, string lastName, string streetAddress, string city, string state, string zipCode, string emailAddress, string cellNumber, string receiveText, string receiveEmail)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "UPDATE [SDSU_School].[4Moxie].[USER_INFO] SET USER_ID = @userid, FIRST_NAME = @firstname, LAST_NAME = @lastname, STREET_ADDRESS = @streetaddress, CITY = @city, STATE = @state, ZIP_CODE = @zipcode, EMAIL_ADDRESS = @emailaddress, CELL_NUMBER = @cellnumber, RECEIVE_TEXT = @receivetext, RECEIVE_TEXT = @receiveemail";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userID));
            paramList.Add(new SqlParameter("@firstname", firstName));
            paramList.Add(new SqlParameter("@lastname", lastName));
            paramList.Add(new SqlParameter("@streetaddress", streetAddress));
            paramList.Add(new SqlParameter("@city", city));
            paramList.Add(new SqlParameter("@state", state));
            paramList.Add(new SqlParameter("@zipcode", zipCode));
            paramList.Add(new SqlParameter("@emailaddress", emailAddress));
            paramList.Add(new SqlParameter("@cellnumber", cellNumber));
            paramList.Add(new SqlParameter("@receivetext", receiveText));
            paramList.Add(new SqlParameter("@receiveemail", receiveEmail));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            db.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// This method will delete the specified user's info from the USER_INFO table.
        /// </summary>
        /// <param name="userID"></param>
        public void DeleteUserInfo(string userID)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "DELETE FROM [SDSU_School].[4Moxie].[USER_INFO] WHERE USER_ID = @userid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userID));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            db.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// This method will return true if the specified user has previously entered their profile information.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>bool</returns>
        public bool CheckIfUserInfoExists(string userID)
        {
            // Variable Declarations
            string query = "";
            int queryResult = 0;
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT COUNT(*) FROM [SDSU_School].[4Moxie].[USER_INFO] WHERE USER_ID = @userid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userID));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            queryResult = db.Database.SqlQuery<int>(query, parameters).FirstOrDefault<int>();

            // Check if the query has returned a count > 0
            if (queryResult >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This method will insert a newly created order record into the ORDERS table.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="orderNumber"></param>
        /// <param name="orderDate"></param>
        /// <param name="departureDate"></param>
        /// <param name="departureStreetAddress"></param>
        /// <param name="departureCity"></param>
        /// <param name="departureState"></param>
        /// <param name="departureZipCode"></param>
        /// <param name="destinationName"></param>
        /// <param name="numberOfPassengers"></param>
        /// <param name="flightDetails"></param>
        /// <param name="comments"></param>
        public void InsertNewOrderInfo(string userID, string orderNumber, string orderDate, string departureDate, string departureStreetAddress, string departureCity, string departureState, string departureZipCode, string destinationName, string numberOfPassengers, string flightDetails, string comments)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "INSERT INTO [SDSU_School].[4Moxie].[ORDERS] VALUES (@ordernumber, @userid, @orderdate, @departuredate, @departurestreetaddress, @departurecity, @departurestate, @departurezipcode, @destinationname, @numberofpassengers, @flightdetails, @comments)";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@ordernumber", orderNumber));
            paramList.Add(new SqlParameter("@userid", userID));
            paramList.Add(new SqlParameter("@orderdate", orderDate));
            paramList.Add(new SqlParameter("@departuredate", departureDate));
            paramList.Add(new SqlParameter("@departurestreetaddress", departureStreetAddress));
            paramList.Add(new SqlParameter("@departurecity", departureCity));
            paramList.Add(new SqlParameter("@departurestate", departureState));
            paramList.Add(new SqlParameter("@departurezipcode", departureZipCode));
            paramList.Add(new SqlParameter("@destinationname", destinationName));
            paramList.Add(new SqlParameter("@numberofpassengers", numberOfPassengers));
            paramList.Add(new SqlParameter("@flightdetails", flightDetails));
            paramList.Add(new SqlParameter("@comments", comments));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            db.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// This method will get the next available order number that is to be assigned to the next order.
        /// </summary>
        /// <returns>int</returns>
        public int GetNextOrderNumber()
        {
            // Variable Declarations
            string query = "";
            int queryResult = 0;

            // Create the query
            query = "SELECT ISNULL(MAX(ORDER_NUMBER), 0) FROM [SDSU_School].[4Moxie].[ORDERS]";

            // Execute the query
            queryResult = db.Database.SqlQuery<int>(query).FirstOrDefault<int>();

            // Return the next order number
            return Convert.ToInt32(queryResult) + 1;       
        }

        public void CreateNewTrip(string tripNumber, string driverUserID, string orderNumber)
        {
            // Variable Declarations
            string query = "";

        }

        public void AddOrderToExistingTrip(string orderNumber)
        {

        }

        /// <summary>
        /// This method will return a list of all of the specified user's orders.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>List<ORDER></returns>
        public List<ORDER> GetAllUserOrders(string userid)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT * FROM [SDSU_School].[4Moxie].[ORDERS] WHERE USER_ID = @userid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userid));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            var orderList = db.ORDERS.SqlQuery(query, parameters).ToList<ORDER>();

            // Return the list of orders
            return orderList;

        }

        public void GetUserInfo(string userid)
        {
            // Variable Declarations
        }

        public List<string> GetDestinationList()
        {
            // Variable Declarations
            string query = "";

            // Create the query
            query = "SELECT DESTINATION_NAME FROM [SDSU_School].[4Moxie].[DESTINATIONS]";

            // Execute the query
            var destinationList = db.Database.SqlQuery<string>(query).ToList();

            // Return the list of destinations
            return destinationList;
        }

        /// <summary>
        /// This method can be used to send an email to someone.
        /// </summary>
        /// <param name="recipientEmail"></param>
        /// <param name="messageSubject"></param>
        /// <param name="messageBody"></param>
        public void SendEmail(string recipientEmail, string messageSubject, string messageBody)
        {
            // Create a new mail message object
            MailMessage message = new MailMessage();
            message.To.Add(recipientEmail);
            message.From = new MailAddress("ridesrus.shuttleservice@gmail.com");
            message.Subject = messageSubject;
            message.Body = messageBody;

            // Create a new email account object
            NetworkCredential cred = new NetworkCredential("ridesrus.shuttleservice@gmail.com", "ridesrus");

            // Setup the smtp client and send the emails
            SmtpClient client = new SmtpClient("smtp.gmail.com", 465);
            client.Credentials = cred;
            client.EnableSsl = true;
            client.Send(message);
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