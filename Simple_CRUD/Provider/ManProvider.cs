using Npgsql;
using Simple_CRUD.Model;
using Simple_CRUD.Model.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Simple_CRUD.Provider
{
    public interface IManProvider
    {
        List<Man> Select();
        List<Man> Select(int? id);
        public bool Insert(Man man);
        public bool Update(Man man);
        public bool Delete(Man man);
    }
    public class ManProvider : IManProvider
    {
        Query query { get; set; }
        ICountryProvider CountryProvider { get; set; }
        public ManProvider(Query query, ICountryProvider countryProvider) 
        {
            CountryProvider = countryProvider;
            this.query = query;
        }

        /// <summary>
        /// Возвращает список всех people
        /// </summary>
        /// <returns> список людей </returns>
        public List<Man> Select()
        {
            return Select(null);
        }

        /// <summary>
        /// Возвращает запись с указанным id из таблицы people
        /// Если id - null возвращает все записи
        /// </summary>
        /// <param name="id">идентификатор записи</param>
        public List<Man> Select(int? id)
        {
            try
            {
                var result = new List<Man>();

                using (var con = new NpgsqlConnection(query.connection))
                {
                    con.Open();
                    string queryString = id == null ? query.select_people : query.select_man_id;
                    var command = new NpgsqlCommand(queryString, con);

                    if (id != null)
                    {
                        command.Parameters.AddWithValue("@id", id.Value);
                    }

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(new Man
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                            Surname = (string)reader["surname"],
                            CountryBorn = CountryProvider.Select((int)reader["country_born_id"]).First()
                        });
                    }
                }

                return result;
            }
            catch(Exception e)
            {
                MessageBox.Show("An unhandled select exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
        }
        /// <summary>
        /// Вставляет в бд запись о человеке
        /// </summary>
        /// <param name="man">человек</param>
        public bool Insert(Man man)
        {
            try
            {
                var country = CountryProvider.Select(man.CountryBorn.Name);

                using (var con = new NpgsqlConnection(query.connection))
                {
                    con.Open();
                    var queryString = query.insert_man;
                    var command = new NpgsqlCommand(queryString, con);
                    MessageBox.Show($"{man.Name} {man.Surname}");
                    command.Parameters.AddWithValue("@name", man.Name);
                    command.Parameters.AddWithValue("@surname", man.Surname);
                    command.Parameters.AddWithValue("@country_born_id", country.Id);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An unhandled insert exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// обновляет в бд запись о человеке
        /// </summary>
        /// <param name="man">человек</param>
        public bool Update(Man man)
        {
            try
            {
                var country = CountryProvider.Select(man.CountryBorn.Name);
                using (var con = new NpgsqlConnection(query.connection))
                {
                    con.Open();
                    var queryString = query.update_man;
                    var command = new NpgsqlCommand(queryString, con);

                    command.Parameters.AddWithValue("@surname", man.Surname);
                    command.Parameters.AddWithValue("@country_born_id", country.Id);
                    command.Parameters.AddWithValue("@name", man.Name);
                    command.Parameters.AddWithValue("@id", man.Id);

                    command.ExecuteNonQuery();
                }

                CountryProvider.Update(man.CountryBorn);
            }
            catch (Exception e)
            {
                MessageBox.Show("An unhandled update exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Удаляет человека из таблицы людей
        /// </summary>
        /// <param name="man">Человек</param>
        public bool Delete(Man man)
        {
            try
            {
                using (var con = new NpgsqlConnection(query.connection))
                {
                    con.Open();
                    var queryString = query.delete_man;
                    var command = new NpgsqlCommand(queryString, con);

                    command.Parameters.AddWithValue("@id", man.Id);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An unhandled delete exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
