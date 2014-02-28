using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace Aventyrliga_Kontakter.Model.DAL
{
    public class ContactDAL : DALBase
    {

        // Hämtar alla kunder i databasen.
        public  IEnumerable<Contact> GetContacts()
        {
            // Using stänger automatik efter att man är klar .
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    // Skapar det List-objekt som initialt har plats för 100 referenser till Contact-objekt
                    var contacts = new List<Contact>(100);

                    // exekveras specifierad lagrad procedur.
                    SqlCommand cmd = new SqlCommand("Person.uspGetContacts", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    // passar antal objekter som vi får tillbaka med hjälp av nya listan som vi har skapat, för att inte ska ta mer minne än vad det behöves.
                    contacts.TrimExcess();
                    // Öppnar anslutningen till databasen.
                    conn.Open();

                    // Läsa post för post.
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Istället för att kodhåda 1,2,3,4 så bättre att använda mig av Index för att blir i rätt ordning.
                        var contactIdIndex = reader.GetOrdinal("ContactId");
                        var firstNameIndex = reader.GetOrdinal("FirstName");
                        var lastNameIndex = reader.GetOrdinal("LastName");
                        var emailAddressIndex = reader.GetOrdinal("EmailAddress");

                        // Så länge som metoden Read retunerar true  finns data att hämta.
                        while (reader.Read())
                        {
                            // Innehåller refrens till contact
                            contacts.Add(new Contact

                            {
                                // Varje post översätter den med c#.
                                ContactId = reader.GetInt32(contactIdIndex),
                                FirstName = reader.GetString(firstNameIndex),
                                LastName = reader.GetString(lastNameIndex),
                                EmailAddress = reader.GetString(emailAddressIndex)
                            });
                        }

                    }


                    // avallokerar minne som inte används.
                    contacts.TrimExcess();
                    return contacts;



                }
                catch
                {
                    throw new ApplicationException("Ett fel inträffade medan hämtar contacts från databasen.");

                }
            }

        }



        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    var contacts = new List<Contact>(100);

                    SqlCommand cmd = new SqlCommand("Person.uspGetContactsPageWise", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PageSize", SqlDbType.Int, 4).Value = maximumRows;
                    cmd.Parameters.Add("@PageIndex", SqlDbType.Int, 4).Value = (startRowIndex / maximumRows) + 1;

                    cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    totalRowCount = (int)cmd.Parameters["@RecordCount"].Value;

                    // Skapar referens till data utläst från databasen.
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Tar här reda på vilket index min databas olika kolumner har.
                        var contactIdIndex = reader.GetOrdinal("ContactId");
                        var firstNameIndex = reader.GetOrdinal("FirstName");
                        var lastNameIndex = reader.GetOrdinal("LastName");
                        var emailAddressIndex = reader.GetOrdinal("EmailAddress");

                        while (reader.Read())
                        {
                            contacts.Add(new Contact
                            {
                                ContactId = reader.GetInt32(contactIdIndex),
                                FirstName = reader.GetString(firstNameIndex),
                                LastName = reader.GetString(lastNameIndex),
                                EmailAddress = reader.GetString(emailAddressIndex)
                            });
                        }
                    }

                    contacts.TrimExcess();
                    return contacts;
                }
                catch
                {
                    throw new ApplicationException("An error occured when trying to access and get data from database.");
                }
            }
        }


        public Contact GetContactById(int contactId)
        {
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Person.uspGetContact", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@ContactId", contactId);

                    // Lägger till de paramertrar den lagrade proceduren kräver.Svårare sät men ASP.NET behöver ej jobba så mycket.
                    cmd.Parameters.Add("@ContactId", SqlDbType.Int, 4).Value = contactId;

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        var contactIdIndex = reader.GetOrdinal("ContactId");
                        var firstNameIndex = reader.GetOrdinal("FirstName");
                        var lastNameIndex = reader.GetOrdinal("LastName");
                        var emailAddressIndex = reader.GetOrdinal("EmailAddress");

                        if (reader.Read())
                        {
                            return new Contact
                            {
                                ContactId = reader.GetInt32(contactIdIndex),
                                FirstName = reader.GetString(firstNameIndex),
                                LastName = reader.GetString(lastNameIndex),
                                EmailAddress = reader.GetString(emailAddressIndex)
                            };
                        }
                            return null;
                        
                        
                    }
                }
                catch
                {
                    throw new ApplicationException("Ett fel inträffade medan hämtar contacts från data");
                }
            }
        }


        public void InsertContact(Contact contact)
        {
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Person.uspAddContact", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Lägger till de paramterar den lagrade proceduren kräver.
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = contact.FirstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = contact.LastName;
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 50).Value = contact.EmailAddress;

                    //hämtar ut data
                    cmd.Parameters.Add("@ContactId", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    conn.Open();

                    // ExecuteNonQuery används för att exekvera den lagrade proceduren.
                    cmd.ExecuteNonQuery();

                    contact.ContactId = (int)cmd.Parameters["@ContactId"].Value;
                }
                catch
                {
                    throw new ApplicationException("An error occured when trying to add a customer to the database.");
                }
            }
        }



        public void UpdateContact(Contact contact)
        {
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Person.uspUpdateContact", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    
                    cmd.Parameters.Add("@ContactId", SqlDbType.VarChar, 50).Value = contact.ContactId;
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = contact.FirstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = contact.LastName;
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 50).Value = contact.EmailAddress;

                    
              
                    conn.Open();

        
                    cmd.ExecuteNonQuery();

                }
                catch
                {
                    throw new ApplicationException("An error occured when trying to update a customer in the database.");
                }
            }
        }



        public void DeleteContact(int contactId)
        {
            using (SqlConnection conn = CreateConnection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Person.uspRemoveContact", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ContactId", SqlDbType.Int, 4).Value = contactId;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw new ApplicationException("Ett fel inträffade då contacts hämtas från databasen.");
                }
            }
        }








    }
}