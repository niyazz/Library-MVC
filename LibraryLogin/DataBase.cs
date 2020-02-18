using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using LibraryLogin.Models;

namespace LibraryLogin
{
    public class DataBase 
    {
        string localhost = "localhost";
        string dbName = "library";
        string dbPassword = "root";
        string userName = "root";
        public string ConnectionString { get; set; }
        public DataBase()
        {
            this.ConnectionString = $"Server = '{localhost}'; database = '{dbName}'; User Id = {userName}; password = '{dbPassword}'";
        }      
        public User RequestToUser(User enterUser)
        {
            User loginedUser = null;         
            string query = $"SELECT * FROM users WHERE name = '{enterUser.Name}' AND password = '{enterUser.Password}'";

            using (MySqlConnection myConnection = new MySqlConnection(ConnectionString))
            {
                myConnection.Open();
                MySqlCommand command = new MySqlCommand(query, myConnection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {                      
                        loginedUser = new User(
                            reader.GetString(0).ToString(), 
                            reader.GetString(1).ToString(),
                            reader.GetString(2).ToString()
                            );
                    }

                }
            }
            return loginedUser;
        }
        public List<Book> RequestToBooks(Book filterBook)
        {
            string query = "SELECT id, name, author FROM books WHERE amount > 0"; ;
            if (filterBook == null)
            {
                query = "SELECT id, name, author FROM books WHERE amount > 0"; 
            } 
            else if (filterBook != null && filterBook.Name !=  null && filterBook.Author == null)
            {
                query = $"SELECT id, name, author FROM books WHERE amount > 0 AND name = '{filterBook.Name}'";
            }
            else if(filterBook != null && filterBook.Author != null && filterBook.Name == null)
            {
                query = $"SELECT id, name, author FROM books WHERE amount > 0 AND author = '{filterBook.Author}'";
            }
            else if (filterBook != null && filterBook.Name != null && filterBook.Author != null)
            {
                query = $"SELECT id, name, author FROM books WHERE amount > 0 AND name = '{filterBook.Name}' AND name = '{filterBook.Name}'";
            }
            
            List<Book> books = new List<Book>();
            using (MySqlConnection myConnection = new MySqlConnection(ConnectionString))
            {
                myConnection.Open();
                MySqlCommand command = new MySqlCommand(query, myConnection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                            books.Add(new Book(
                               reader.GetString(0),
                               reader.GetString(1),
                               reader.GetString(2)
                               ));  
                    }
                }
            }
            
            return books;
        }
        public Book RequestToBookDetails(string book_id)
        {
            Book selectedBook = null;
            using (MySqlConnection myConnection = new MySqlConnection(ConnectionString))
            {
                myConnection.Open();
                MySqlCommand command = new MySqlCommand($"SELECT * FROM books WHERE id = '{book_id}'", myConnection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        selectedBook = new Book(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4)
                            );
                    }
                }
            }
            return selectedBook;
        }
        public bool RequestToOrderBook(string book_id, string user_id)
        {

            MySqlConnection myConnection = new MySqlConnection(ConnectionString);
            myConnection.Open();

            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            string date = time.ToString(format);

            MySqlCommand command = new MySqlCommand($"INSERT INTO orders VALUES(null, '{user_id}', '{book_id}', '{date}');", myConnection);
            int succesful = command.ExecuteNonQuery();
            myConnection.Close();

            return succesful == 1 ? true : false;
        }
        public bool RequestToReturnBook(string book_id, string user_id)
        {

            MySqlConnection myConnection = new MySqlConnection(ConnectionString);
            myConnection.Open();

            MySqlCommand command = new MySqlCommand($"DELETE FROM orders WHERE user_id = {user_id} AND book_id = {book_id}", myConnection);
            int succesful = command.ExecuteNonQuery();
            myConnection.Close();

            return succesful == 1 ? true : false;
        }
        public bool RequestToDecreaseBook(string book_id)
        {
            MySqlConnection myConnection = new MySqlConnection(ConnectionString);
            myConnection.Open();
            MySqlCommand command = new MySqlCommand($"UPDATE books SET amount = amount {-1} WHERE id = {book_id}", myConnection);
            int succesful = command.ExecuteNonQuery();

            return succesful == 1 ? true : false;
        }
        public bool RequestToIncreaseBook(string book_id)
        {
            MySqlConnection myConnection = new MySqlConnection(ConnectionString);
            myConnection.Open();
            MySqlCommand command = new MySqlCommand($"UPDATE books SET amount = amount + {1} WHERE id = {book_id}", myConnection);
            int succesful = command.ExecuteNonQuery();

            return succesful == 1 ? true : false;
        }
        public List<Book> RequestToUserBooks(string user_id)
        {
            string query = $"SELECT book_id FROM orders WHERE user_id = '{user_id}'";
            List<string> booksId = new List<string>();
            List<Book> user_books = new List<Book>();
            using (MySqlConnection myConnection = new MySqlConnection(ConnectionString))
            {
                myConnection.Open();
                MySqlCommand command = new MySqlCommand(query, myConnection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //if (!booksId.Contains(reader.GetString(0))) // если книги еще нет
                        //{
                            booksId.Add(reader.GetString(0)); // получаем id книг
                        //}
                      
                    }
                }
            }
            using (MySqlConnection myConnection = new MySqlConnection(ConnectionString))
            {
                myConnection.Open();
                for (int i = 0; i < booksId.Count; i++)
                {
                    query = $"SELECT name, author FROM books WHERE id = '{booksId[i]}'";
                    MySqlCommand command = new MySqlCommand(query, myConnection);
                    using (MySqlDataReader reader2 = command.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            user_books.Add(new Book(
                                booksId[i], 
                                reader2.GetString(0), 
                                reader2.GetString(1))
                                ); // получаем название нужных книг
                        }
                    }
                }              
            }

            return user_books;
        }
    }
}