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
    public enum Roles { CUSTOMER = 1, DRIVER = 2, MANAGER = 3, ADMINISTRATOR = 4 };

    public class CalendarEventObject
    {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string color { get; set; }
    }

    public class DriverDDLObject
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public class OrderInfoObject
    {
        public string customerName { get; set; }
        public string dateOrderPlaced { get; set; }
        public string departureDate { get; set; }
        public string departureAddress { get; set; }
        public string departureCity { get; set; }
        public string departureState { get; set; }
        public string departureZipCode { get; set; }
        public string destination { get; set; }
        public string numPassengers { get; set; }
        public string flightDetails { get; set; }
        public string comments { get; set; }
    }

    public class SSMS_Helper
    {
        private SDSU_SchoolEntities db = new SDSU_SchoolEntities();

        /// <summary>
        /// This method creates a log record that is entered into the database for security purposes.
        /// </summary>
        public void CreateSystemLog(string userid)
        {
            // Variable Declarations
            string userID = "";
            string currentDate = "";
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Get the ID of the current user
            userID = userid;  // GetUserID(username);

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
        /// This method will get the USER_ID for the passed in username.
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
        /// This method will get the username for the specified user ID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetUserName(string userID)
        {
            // Variable Declarations
            string query = "";
            string userName = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT UserName FROM [SDSU_School].[4Moxie].[USER_ACCOUNTS] WHERE USER_ID = @userid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userID));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            userName = db.Database.SqlQuery<string>(query, parameters).FirstOrDefault<string>();

            return userName;
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
        public void InsertNewUserInfo(string userID, string firstName, string lastName, string streetAddress, string city, string state, string zipCode, string emailAddress, string cellNumber, string cellCarrierID, bool receiveText, bool receiveEmail)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "INSERT INTO [SDSU_School].[4Moxie].[USER_INFO] VALUES (@userid, @firstname, @lastname, @streetaddress, @city, @state, @zipcode, @emailaddress, @cellnumber, @cellcarrierid, @receivetext, @receiveemail)";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userID));
            paramList.Add(new SqlParameter("@firstname", firstName));
            paramList.Add(new SqlParameter("@lastname", lastName));
            paramList.Add(new SqlParameter("@streetaddress", streetAddress));
            paramList.Add(new SqlParameter("@city", city));
            paramList.Add(new SqlParameter("@state", state));
            paramList.Add(new SqlParameter("@zipcode", zipCode));
            paramList.Add(new SqlParameter("@emailaddress", emailAddress));
            paramList.Add(new SqlParameter("@cellcarrierid", cellCarrierID));
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
        public void UpdateExistingUserInfo(string userID, string firstName, string lastName, string streetAddress, string city, string state, string zipCode, string emailAddress, string cellNumber, string cellCarrierID, bool receiveText, bool receiveEmail)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "UPDATE [SDSU_School].[4Moxie].[USER_INFO] SET FIRST_NAME = @firstname, LAST_NAME = @lastname, STREET_ADDRESS = @streetaddress, CITY = @city, STATE = @state, ZIP_CODE = @zipcode, EMAIL_ADDRESS = @emailaddress, CELL_NUMBER = @cellnumber, CELL_CARRIER_ID = @cellcarrierid, RECEIVE_TEXT = @receivetext, RECEIVE_EMAIL = @receiveemail WHERE USER_ID = @userid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userID));
            paramList.Add(new SqlParameter("@firstname", firstName));
            paramList.Add(new SqlParameter("@lastname", lastName));
            paramList.Add(new SqlParameter("@streetaddress", streetAddress));
            paramList.Add(new SqlParameter("@city", city));
            paramList.Add(new SqlParameter("@state", state));
            paramList.Add(new SqlParameter("@zipcode", zipCode));
            paramList.Add(new SqlParameter("@emailaddress", emailAddress));
            paramList.Add(new SqlParameter("@cellcarrierid", cellCarrierID));
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
        /// This method will get the speciied users profile information
        /// </summary>
        /// <param name="userID"></param>
        public USER_INFO GetUserProfileInfo(string userID)
        {
            // Variable Declarations
            string query = "";
            USER_INFO info = new USER_INFO();
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT * FROM [SDSU_School].[4Moxie].[USER_INFO] WHERE USER_ID = @userid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userID));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            info = db.USER_INFO.SqlQuery(query, parameters).FirstOrDefault<USER_INFO>();

            return info;
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
        public void InsertNewOrderInfo(string userID, string orderNumber, string orderDate, string departureDate, string departureStreetAddress, string departureCity, string departureState, string departureZipCode, string destinationID, string numberOfPassengers, string flightDetails, string comments)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "INSERT INTO [SDSU_School].[4Moxie].[ORDERS] VALUES (@ordernumber, @userid, @orderdate, @departuredate, @departurestreetaddress, @departurecity, @departurestate, @departurezipcode, @destinationID, @numberofpassengers, @flightdetails, @comments)";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@ordernumber", orderNumber));
            paramList.Add(new SqlParameter("@userid", userID));
            paramList.Add(new SqlParameter("@orderdate", orderDate));
            paramList.Add(new SqlParameter("@departuredate", departureDate));
            paramList.Add(new SqlParameter("@departurestreetaddress", departureStreetAddress));
            paramList.Add(new SqlParameter("@departurecity", departureCity));
            paramList.Add(new SqlParameter("@departurestate", departureState));
            paramList.Add(new SqlParameter("@departurezipcode", departureZipCode));
            paramList.Add(new SqlParameter("@destinationID", destinationID));
            paramList.Add(new SqlParameter("@numberofpassengers", numberOfPassengers));
            paramList.Add(new SqlParameter("@flightdetails", flightDetails));
            paramList.Add(new SqlParameter("@comments", comments));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            db.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// This method will update/submit changes to the specified order.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <param name="orderDate"></param>
        /// <param name="departureDate"></param>
        /// <param name="departureStreetAddress"></param>
        /// <param name="departureCity"></param>
        /// <param name="departureState"></param>
        /// <param name="departureZipCode"></param>
        /// <param name="destinationID"></param>
        /// <param name="numberOfPassengers"></param>
        /// <param name="flightDetails"></param>
        /// <param name="comments"></param>
        public void UpdateExistingOrderInfo(string orderNumber, string departureDate, string departureStreetAddress, string departureCity, string departureState, string departureZipCode, string destinationID, string numberOfPassengers, string flightDetails, string comments)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "UPDATE [SDSU_School].[4Moxie].[ORDERS] SET DEPARTURE_DATETIME = @departuredate, DEPARTURE_STREET_ADDRESS = @departurestreetaddress," +
                    "DEPARTURE_CITY = @departurecity, DEPARTURE_STATE = @departurestate, DEPARTURE_ZIPCODE = @departurezipcode, DESTINATION_ID = @destinationID," +
                    "NUMBER_OF_PASSENGERS = @numberofpassengers, FLIGHT_DETAILS = @flightdetails, COMMENTS = @comments WHERE ORDER_NUMBER = @ordernumber";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@ordernumber", orderNumber));
            paramList.Add(new SqlParameter("@departuredate", departureDate));
            paramList.Add(new SqlParameter("@departurestreetaddress", departureStreetAddress));
            paramList.Add(new SqlParameter("@departurecity", departureCity));
            paramList.Add(new SqlParameter("@departurestate", departureState));
            paramList.Add(new SqlParameter("@departurezipcode", departureZipCode));
            paramList.Add(new SqlParameter("@destinationID", destinationID));
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

        /// <summary>
        /// This method will insert a new work availability date/time into the database for a specified driver.
        /// </summary>
        /// <param name="availabilityID"></param>
        /// <param name="driverUserID"></param>
        /// <param name="date"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public void InsertNewAvailability(string availabilityID, string driverUserID, string date, string startTime, string endTime)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "INSERT INTO [SDSU_School].[4Moxie].[DRIVER_TIME_AVAILABILITIES] VALUES (@availabilityid, @driveruserid, @date, @starttime, @endtime)";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@availabilityid", availabilityID));
            paramList.Add(new SqlParameter("@driveruserid", driverUserID));
            paramList.Add(new SqlParameter("@date", date));
            paramList.Add(new SqlParameter("@starttime", startTime));
            paramList.Add(new SqlParameter("@endtime", endTime));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            db.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// This method will delete the specified availability from the drivers timesheet table.
        /// </summary>
        /// <param name="availabilityID"></param>
        public void DeleteAvailability(string availabilityID)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "DELETE FROM [SDSU_School].[4Moxie].[DRIVER_TIME_AVAILABILITIES] WHERE ID = @availabilityid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@availabilityid", availabilityID));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            db.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// This method will get the next available ID number that can be used for a new driver availability.
        /// </summary>
        /// <returns></returns>
        public int GetNextAvailabilityID()
        {
            // Variable Declarations
            string query = "";
            int queryResult = 0;

            // Create the query
            query = "SELECT ISNULL(MAX(ID), 0) FROM [SDSU_School].[4Moxie].[DRIVER_TIME_AVAILABILITIES]";

            // Execute the query
            queryResult = db.Database.SqlQuery<int>(query).FirstOrDefault<int>();

            // Return the next ID number
            return Convert.ToInt32(queryResult) + 1;
        }

        /// <summary>
        /// This method will get all of the timesheet events for the specified driver.
        /// </summary>
        /// <param name="driverUserID"></param>
        /// <returns></returns>
        public List<DRIVER_TIME_AVAILABILITIES> GetDriverTimesheet(string driverUserID, string startDate, string endDate)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT * FROM [SDSU_School].[4Moxie].[DRIVER_TIME_AVAILABILITIES] WHERE DRIVER_USER_ID = @driveruserid AND DATE > @startDate AND DATE < @endDate";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@driveruserid", driverUserID));
            paramList.Add(new SqlParameter("@startDate", startDate));
            paramList.Add(new SqlParameter("@endDate", endDate));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            var orderList = db.DRIVER_TIME_AVAILABILITIES.SqlQuery(query, parameters).ToList<DRIVER_TIME_AVAILABILITIES>();

            // Return the list of orders
            return orderList;
        }

        /// <summary>
        /// This method will get a list of all of the orders that the specified driver has been assigned to drive.
        /// </summary>
        /// <param name="driverUserID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<ORDER> GetDriverOrders(string driverUserID, string startDate, string endDate)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT orders.ORDER_NUMBER, orders.USER_ID, orders.DATETIME_ORDER_PLACED, orders.DEPARTURE_DATETIME, orders.DEPARTURE_STREET_ADDRESS, orders.DEPARTURE_CITY, orders.DEPARTURE_STATE, orders.DEPARTURE_ZIPCODE, orders.DESTINATION_ID, orders.NUMBER_OF_PASSENGERS, orders.FLIGHT_DETAILS, orders.COMMENTS " +
            "FROM [SDSU_School].[4Moxie].[ORDERS] orders, [SDSU_School].[4Moxie].[DRIVER_TRIP_ASSIGNMENTS] assigns WHERE orders.DEPARTURE_DATETIME > @startDate AND orders.DEPARTURE_DATETIME < @endDate AND orders.ORDER_NUMBER = assigns.ORDER_NUMBER AND assigns.DRIVER_USER_ID = @driveruserid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@startDate", startDate));
            paramList.Add(new SqlParameter("@endDate", endDate));
            paramList.Add(new SqlParameter("@driveruserid", driverUserID));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            var orderList = db.ORDERS.SqlQuery(query, parameters).ToList<ORDER>();

            // Return the list of orders
            return orderList;
        }

        /// <summary>
        /// This method will return a list of all the trip orders.
        /// </summary>
        /// <returns></returns>
        public List<ORDER> GetAllTripOrders(string startDate, string endDate)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT * FROM [SDSU_School].[4Moxie].[ORDERS] WHERE DEPARTURE_DATETIME > @startDate AND DEPARTURE_DATETIME < @endDate";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@startDate", startDate));
            paramList.Add(new SqlParameter("@endDate", endDate));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            var orderList = db.ORDERS.SqlQuery(query, parameters).ToList<ORDER>();

            // Return the list of orders
            return orderList;
        }

        /// <summary>
        /// This method will return a list of all the current orders for the specified user.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>List<ORDER></returns>
        public List<ORDER> GetCurrentUserOrders(string userid)
        {
            // Variable Declarations
            string query = "";
            string currentDate = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Get todays date/time
            currentDate = DateTime.Now.ToString();

            // Create the query
            query = "SELECT * FROM [SDSU_School].[4Moxie].[ORDERS] WHERE USER_ID = @userid AND DEPARTURE_DATETIME > @currentdate ORDER BY ORDER_NUMBER ASC";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userid));
            paramList.Add(new SqlParameter("@currentdate", currentDate));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            var orderList = db.ORDERS.SqlQuery(query, parameters).ToList<ORDER>();

            // Return the list of orders
            return orderList;
        }

        /// <summary>
        /// This method will return a list of all the past orders for the specified user.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<ORDER> GetPastUserOrders(string userid)
        {
            // Variable Declarations
            string query = "";
            string currentDate = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Get todays date/time
            currentDate = DateTime.Now.ToString();

            // Create the query
            query = "SELECT * FROM [SDSU_School].[4Moxie].[ORDERS] WHERE USER_ID = @userid AND DEPARTURE_DATETIME < @currentdate ORDER BY ORDER_NUMBER ASC";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userid));
            paramList.Add(new SqlParameter("@currentdate", currentDate));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            var orderList = db.ORDERS.SqlQuery(query, parameters).ToList<ORDER>();

            // Return the list of orders
            return orderList;
        }

        /// <summary>
        /// This method will get all the supported trip destinations.
        /// </summary>
        /// <returns></returns>
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
        /// This method returns the name corresponding to the specified destination ID.
        /// </summary>
        /// <param name="destinationid"></param>
        /// <returns></returns>
        public string GetDestinationName(string destinationid)
        {
            // Variable Declarations
            string destinationName = "";
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT DESTINATION_NAME FROM [SDSU_School].[4Moxie].[DESTINATIONS] WHERE DESTINATION_ID = @destinationid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@destinationid", destinationid));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            destinationName = db.Database.SqlQuery<string>(query, parameters).FirstOrDefault<string>();

            return destinationName;
        }

        /// <summary>
        /// This method will get all the available drivers for the specified order.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public List<string> GetAllAvailableDriversForOrder(string orderNumber)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT DRIVER_USER_ID FROM [SDSU_School].[4Moxie].[DRIVER_TIME_AVAILABILITIES] avails, [SDSU_School].[4Moxie].[ORDERS] orders WHERE orders.DEPARTURE_DATETIME BETWEEN CONVERT(DATETIME, CONVERT(CHAR(8), avails.DATE, 112) + ' ' + CONVERT(CHAR(8), avails.START_TIME, 108)) AND CONVERT(DATETIME, CONVERT(CHAR(8), avails.DATE, 112) + ' ' + CONVERT(CHAR(8), avails.END_TIME, 108)) AND orders.ORDER_NUMBER = @ordernumber"; 

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@ordernumber", orderNumber));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            var destinationList = db.Database.SqlQuery<string>(query, parameters).ToList();

            return destinationList;
        }

        /// <summary>
        /// This method will get the user ID of the driver that has been assigned to the specified order.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public string GetAssignedDriverForOrder(string orderNumber)
        {
            // Variable Declarations
            string driverUserID = "";
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT DRIVER_USER_ID FROM [SDSU_School].[4Moxie].[DRIVER_TRIP_ASSIGNMENTS] WHERE ORDER_NUMBER = @ordernumber";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@orderNumber", orderNumber));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            driverUserID = db.Database.SqlQuery<string>(query, parameters).FirstOrDefault<string>();

            return driverUserID;
        }

        /// <summary>
        /// This method will get the first and last name for the user who placed the specified order.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public string GetUserFirstandLastNameFromOrder(string orderNumber)
        {
            // Variable Declarations
            string name = "";
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT (user_info.FIRST_NAME + ' ' + user_info.LAST_NAME) FROM [SDSU_School].[4Moxie].[USER_INFO] user_info, [SDSU_School].[4Moxie].[ORDERS] orders WHERE user_info.USER_ID = orders.USER_ID AND orders.ORDER_NUMBER = @ordernumber";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@orderNumber", orderNumber));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            name = db.Database.SqlQuery<string>(query, parameters).FirstOrDefault<string>();

            return name;
        }

        /// <summary>
        /// This method will insert a new record, including the user ID and role ID, into the USER_ROLES table.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="roleid"></param>
        public void InsertNewUserRole(string userid, int roleid)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "INSERT INTO [SDSU_School].[4Moxie].[USER_ROLES] VALUES (@userid, @roleid)";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userid));
            paramList.Add(new SqlParameter("@roleid", roleid));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            db.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// This method will update the specified user's role type.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="roleid"></param>
        public void UpdateExistingUserRole(string userid, int roleid)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "UPDATE [SDSU_School].[4Moxie].[USER_ROLES] SET RoleId = @roleid WHERE UserId = @userid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userid));
            paramList.Add(new SqlParameter("@roleid", roleid));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            db.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// This method will get the cell carrier domain for the specified user.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserCellCarrierDomain(string userid)
        {
            // Variable Declarations
            string domain = "";
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT CARRIER_DOMAIN FROM [SDSU_School].[4Moxie].[CELL_CARRIERS] carriers, [SDSU_School].[4Moxie].[USER_INFO] userinfo WHERE userinfo.CELL_CARRIER_ID = carriers.CARRIER_ID AND USER_ID = @userid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userid));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            domain = db.Database.SqlQuery<string>(query, parameters).FirstOrDefault<string>();

            return domain;
        }

        /// <summary>
        /// This method will return true if the user has chosen to receive text message alerts, false if they have not.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool UserReceiveText(string userid)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT RECEIVE_TEXT FROM [SDSU_School].[4Moxie].[USER_INFO] WHERE USER_ID = @userid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userid));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            return db.Database.SqlQuery<bool>(query, parameters).FirstOrDefault<bool>();
        }

        /// <summary>
        /// This method will return true if the user has chosen to reveive email alerts, false if they have not.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool UserReceiveEmail(string userid)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT RECEIVE_EMAIL FROM [SDSU_School].[4Moxie].[USER_INFO] WHERE USER_ID = @userid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userid));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            return db.Database.SqlQuery<bool>(query, parameters).FirstOrDefault<bool>();
        }

        /// <summary>
        /// This method will send an order confirmation email to the specified user.
        /// </summary>
        /// <param name="userid"></param>
        public void SendOrderConfirmationAlerts(string userid)
        {
            // Variable Declarations
            string recipientEmail = "";
            string recipientText = "";
            string messageSubject = "";
            string messageBody = "";

            // Populate the parameters
            messageSubject = "Trip Order Confirmation";
            messageBody = "Your order has been successfully placed!";

            // Check if the user wants email alerts
            if (UserReceiveEmail(userid))
            {
                recipientEmail = GetUserEmailAddress(userid);
                SendEmail(recipientEmail, messageSubject, messageBody);
            }

            // Check if the user wants text alerts
            if (UserReceiveText(userid))
            {
                recipientText = GetUserCellNumber(userid) + "@" + GetUserCellCarrierDomain(userid);
                SendEmail(recipientText, messageSubject, messageBody);
            }     
        }

        /// <summary>
        /// This method will send the specified driver any alerts that they have chosen to receive.
        /// </summary>
        /// <param name="userid"></param>
        public void SendDriverAssignmentAlerts(string userid)
        {
            // Variable Declarations
            string recipientEmail = "";
            string recipientText = "";
            string messageSubject = "";
            string messageBody = "";

            // Populate the parameters
            messageSubject = "New Trip Assignment";
            messageBody = "You have been assigned a new trip to drive.";

            // Check if the user wants email alerts
            if (UserReceiveEmail(userid))
            {
                recipientEmail = GetUserEmailAddress(userid);
                SendEmail(recipientEmail, messageSubject, messageBody);
            }

            // Check if the user wants text alerts
            if (UserReceiveText(userid))
            {
                recipientText = GetUserCellNumber(userid) + "@" + GetUserCellCarrierDomain(userid);
                SendEmail(recipientText, messageSubject, messageBody);
            }     
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

            // Create a new network credentials object object
            NetworkCredential cred = new NetworkCredential("ridesrus.shuttleservice@gmail.com", "ridesrus");

            // Setup the smtp client and send the emails
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.Credentials = cred;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(message);
        }

        /// <summary>
        /// This method will insert a new trip assignment into the DRIVER_TRIP_ASSIGNMENTS table.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <param name="driverUserID"></param>
        public void InsertNewDriverAssignment(string orderNumber, string driverUserID)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "INSERT INTO [SDSU_School].[4Moxie].[DRIVER_TRIP_ASSIGNMENTS] VALUES (@ordernumber, @driveruserid)";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@ordernumber", orderNumber));
            paramList.Add(new SqlParameter("@driveruserid", driverUserID));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            db.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// This method will update an existing row in the DRIVER_TRIP_ASSIGNMENTS table.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <param name="newDriverUserID"></param>
        public void UpdateExistingDriverAssignment(string orderNumber, string newDriverUserID)
        {
            // Variable Declarations
            string query = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "UPDATE [SDSU_School].[4Moxie].[DRIVER_TRIP_ASSIGNMENTS] SET DRIVER_USER_ID = @driveruserid WHERE ORDER_NUMBER = @ordernumber";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@ordernumber", orderNumber));
            paramList.Add(new SqlParameter("@driveruserid", newDriverUserID));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            db.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <summary>
        /// This method will get the specified user's cell number.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserCellNumber(string userid)
        {
            // Variable Declarations
            string query = "";
            string cellNumber = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT CELL_NUMBER FROM [SDSU_School].[4Moxie].[USER_INFO] WHERE USER_ID = @userid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userid));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            cellNumber = db.Database.SqlQuery<string>(query, parameters).FirstOrDefault<string>();

            return cellNumber;
        }

        /// <summary>
        /// This method will get the specified user's email address.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserEmailAddress(string userid)
        {
            // Variable Declarations
            string query = "";
            string emailAddress = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Create the query
            query = "SELECT EMAIL_ADDRESS FROM [SDSU_School].[4Moxie].[USER_INFO] WHERE USER_ID = @userid";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@userid", userid));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            emailAddress = db.Database.SqlQuery<string>(query, parameters).FirstOrDefault<string>();

            return emailAddress;
        }

        /// <summary>
        /// This method will return a list of all the trips that the specified driver has driven in the past.
        /// </summary>
        /// <param name="driverUserID"></param>
        /// <returns></returns>
        public List<ORDER> GetDriverTripHistory(string driverUserID)
        {
            // Variable Declarations
            string query = "";
            string currentDate = "";
            List<SqlParameter> paramList = new List<SqlParameter>();

            // Get todays date/time
            currentDate = DateTime.Now.ToString();

            // Create the query
            query = "SELECT orders.ORDER_NUMBER, orders.USER_ID, orders.DATETIME_ORDER_PLACED, orders.DEPARTURE_DATETIME, orders.DEPARTURE_STREET_ADDRESS, orders.DEPARTURE_CITY, orders.DEPARTURE_STATE, orders.DEPARTURE_ZIPCODE, orders.DESTINATION_ID, orders.NUMBER_OF_PASSENGERS, orders.FLIGHT_DETAILS, orders.COMMENTS " +
            "FROM [SDSU_School].[4Moxie].[ORDERS] orders, [SDSU_School].[4Moxie].[DRIVER_TRIP_ASSIGNMENTS] assigns WHERE orders.ORDER_NUMBER = assigns.ORDER_NUMBER AND assigns.DRIVER_USER_ID = @driveruserid AND orders.DEPARTURE_DATETIME < @currentdate";

            // Populate the list of parameters
            paramList.Add(new SqlParameter("@driveruserid", driverUserID));
            paramList.Add(new SqlParameter("@currentdate", currentDate));
            SqlParameter[] parameters = paramList.ToArray();

            // Execute the query
            var orderList = db.ORDERS.SqlQuery(query, parameters).ToList<ORDER>();

            // Return the list of orders
            return orderList;
        }
    }
}