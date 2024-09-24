using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsContacts
{
    public class DataAccessLayer
    {
        //definimos la conexión a la DB
        private SqlConnection _connection = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=WinFormsContacts;Data Source=LAPTOP-284RB2AS\\SQLEXPRESS");

        public void InsertContact(Contact contact)
        {
            try
            {
                //conectamos a la DB
                _connection.Open();
                //@ permite escribir la query en varias líneas
                string query = @"
                                INSERT INTO Contacts (FirstName, LastName, Phone, Address)
                                VALUES (@FirstName, @LastName, @Phone, @Address)";
                //definimos a qué corresponde cada parámetro, dos formas distintas
                SqlParameter FirstName = new SqlParameter();
                FirstName.ParameterName = "@FirstName";
                FirstName.Value = contact.FirstName;
                FirstName.DbType = System.Data.DbType.String;

                SqlParameter SecondName = new SqlParameter("@LastName", contact.LastName);
                SqlParameter Phone = new SqlParameter("@Phone", contact.Phone);
                SqlParameter Address = new SqlParameter("@Address", contact.Address);

                //definimos el comando sql a ejecutar y añadimos los parámetros
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.Add(FirstName);
                command.Parameters.Add(SecondName);
                command.Parameters.Add(Phone);
                command.Parameters.Add(Address);

                //ejecutamos el comando sql
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                //cerramos la conexión haya un error o no
                _connection.Close(); 
            }
        }

        public void UpdateContact(Contact contact)
        {
            try
            {
                //conectamos a la DB
                _connection.Open();

                //@ permite escribir la query en varias líneas
                string query = @"
                                UPDATE Contacts SET FirstName = @FirstName, 
                                LastName=@LastName, Phone = @Phone, Address = @Address
                                WHERE Id = @Id";
                //definimos a qué corresponde cada parámetro
                SqlParameter Id = new SqlParameter("@Id", contact.Id);
                SqlParameter FirstName = new SqlParameter("@FirstName", contact.FirstName);
                SqlParameter SecondName = new SqlParameter("@LastName", contact.LastName);
                SqlParameter Phone = new SqlParameter("@Phone", contact.Phone);
                SqlParameter Address = new SqlParameter("@Address", contact.Address);

                //definimos el comando sql a ejecutar y añadimos los parámetros
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.Add(Id);
                command.Parameters.Add(FirstName);
                command.Parameters.Add(SecondName);
                command.Parameters.Add(Phone);
                command.Parameters.Add(Address);

                //ejecutamos el comando sql
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //cerramos la conexión haya un error o no
                _connection.Close();
            }
        }

        public void DeleteContact(int id)
        {
            try
            {
                //conectamos a la DB
                _connection.Open();

                //@ permite escribir la query en varias líneas
                string query = @"
                                DELETE FROM Contacts WHERE Id = @Id";
                //definimos a qué corresponde cada parámetro
                SqlParameter Id = new SqlParameter("@Id", id);

                //definimos el comando sql a ejecutar y añadimos los parámetros
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.Add(Id);

                //ejecutamos el comando sql
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //cerramos la conexión haya un error o no
                _connection.Close();
            }
        }

        public List<Contact> GetContacts(string searchText = null)
        {
            List<Contact> list = new List<Contact>();
            try
            {
                //conectamos a la DB
                _connection.Open();

                string query = "SELECT * FROM Contacts;";

                //definimos el comando sql a ejecutar
                SqlCommand command = new SqlCommand();

                if(!string.IsNullOrEmpty(searchText))
                {
                    query = @"  SELECT * FROM Contacts 
                                WHERE FirstName LIKE @Search OR LastName LIKE @Search
                                OR Phone LIKE @Search OR Address LIKE @Search;";

                    //lo que haya entre los % se ignora
                    command.Parameters.Add(new SqlParameter("@Search", $"%{searchText}%"));
                }

                //ahora añadimos al comando la conexión y la query
                command.CommandText = query;
                command.Connection = _connection;

                //definimos el objeto lector que contendrá todas las filas
                SqlDataReader reader = command.ExecuteReader();

                //añadimos a la lista las columnas obtenidas por el reader
                while (reader.Read())
                {
                    list.Add(new Contact
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Address = reader["Address"].ToString(),
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            { 
                _connection.Close(); 
            }

            return list;
        }

    }
}
