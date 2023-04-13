using Microsoft.Data.SqlClient;
using System.Data;

namespace AddressBookWebApp.Models
{
    public class AddressBookDataAccess
    {
        SqlConnection connection = null;
        SqlCommand command = null;
        public static IConfiguration Configuration { get; set; }

        // Get connection
        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("DefaultConnection");
        }

        // Get all data in address book
        public List<AddressBook> GetAllPersons()
        {
            List<AddressBook> addressBookList = new List<AddressBook>();
            using (connection = new SqlConnection(GetConnectionString()))
            {
                command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[usp_GetAllPersons]";
                connection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    AddressBook ab = new AddressBook();
                    ab.AddressBookId = (int)dataReader["AddressBookId"];
                    ab.FirstName = (string)dataReader["FirstName"];
                    ab.MiddleName = (dataReader["MiddleName"] == DBNull.Value) ? string.Empty : (string)dataReader["MiddleName"];
                    ab.LastName = (string)dataReader["LastName"];
                    ab.AddressLine = (string)dataReader["AddressLine"];
                    ab.City = (string)dataReader["City"];
                    ab.Province = (string)dataReader["Province"];
                    ab.PhoneNumber = (string)dataReader["PhoneNumber"];
                    addressBookList.Add(ab);
                }
                connection.Close();
            }

            return addressBookList;
        }
        
        // Insert person to address book
        public bool Insert(AddressBook ab)
        {
            int flag = 0;

            using (connection = new SqlConnection(GetConnectionString()))
            {
                command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[usp_InsertPerson]";

                command.Parameters.AddWithValue("@FirstName", ab.FirstName);
                if (ab.MiddleName != null)
                {
                    command.Parameters.AddWithValue("@MiddleName", ab.MiddleName);
                }
                command.Parameters.AddWithValue("@LastName", ab.LastName);
                command.Parameters.AddWithValue("@AddressLine", ab.AddressLine);
                command.Parameters.AddWithValue("@City", ab.City);
                command.Parameters.AddWithValue("@Province", ab.Province);
                command.Parameters.AddWithValue("@PhoneNumber", ab.PhoneNumber);

                connection.Open();

                flag = command.ExecuteNonQuery();

                connection.Close();
            }

            // For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command
            // .ExecuteNonQuery();
            return flag > 0 ? true : false;
        }
        
        // Get person data to address book by id
        public AddressBook GetPersonById(int id)
        {
            AddressBook ab = new AddressBook();
            using (connection = new SqlConnection(GetConnectionString()))
            {
                command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[usp_GetPersonById]";
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    ab.AddressBookId = (int) dataReader["AddressBookId"];
                    ab.FirstName = (string)dataReader["FirstName"];
                    ab.MiddleName = (dataReader["MiddleName"] == DBNull.Value) ? string.Empty : (string)dataReader["MiddleName"];
                    ab.LastName = (string)dataReader["LastName"];
                    ab.AddressLine = (string)dataReader["AddressLine"];
                    ab.City = (string)dataReader["City"];
                    ab.Province = (string)dataReader["Province"];
                    ab.PhoneNumber = (string)dataReader["PhoneNumber"];

                }
                connection.Close();
            }
            return ab;
        }
        
        // Update person in address book
        public bool Update(AddressBook ab)
        {
            int flag = 0;
            using (connection = new SqlConnection(GetConnectionString()))
            {
                command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[usp_UpdatePerson]";

                command.Parameters.AddWithValue("@Id", ab.AddressBookId);
                command.Parameters.AddWithValue("@FirstName", ab.FirstName);
                if (ab.MiddleName != null)
                {
                    command.Parameters.AddWithValue("@MiddleName", ab.MiddleName);
                }
                command.Parameters.AddWithValue("@LastName", ab.LastName);
                command.Parameters.AddWithValue("@AddressLine", ab.AddressLine);
                command.Parameters.AddWithValue("@City", ab.City);
                command.Parameters.AddWithValue("@Province", ab.Province);
                command.Parameters.AddWithValue("@PhoneNumber", ab.PhoneNumber);

                connection.Open();
                flag = command.ExecuteNonQuery();
                connection.Close();
            }
            return flag > 0 ? true : false;
        }
        
        // Delete person in address book
        public bool Delete(int id)
        {
            int flag = 0;

            using (connection = new SqlConnection(GetConnectionString()))
            {
                command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[usp_DeletePerson]";
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                flag = command.ExecuteNonQuery();

                connection.Close();
            }

            return flag > 0 ? true : false;
        }
    }
}
