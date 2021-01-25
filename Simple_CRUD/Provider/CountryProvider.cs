using Npgsql;
using Simple_CRUD.Model;
using Simple_CRUD.Model.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Simple_CRUD.Provider
{
    public interface ICountryProvider
    {
        List<Country> Select();
        Country Select(string name);
        List<Country> Select(int? id);
        bool Insert(Country country);
        bool Update(Country country);
        bool Delete(Country country);
    }
    class CountryProvider : ICountryProvider
    {
        Query query { get; set; }
        public CountryProvider(Query query)
        {
            this.query = query;
        }
        /// <summary>
        /// Возвращает все записи из таблицы countries
        /// </summary>
        /// <returns>спиосок всех стран</returns>
        public List<Country> Select()
        {
            return Select((int?)null);
        }

        /// <summary>
        /// Возвращает запись с указанным id из таблицы countries
        /// Если id - null возвращает все записи
        /// </summary>
        /// <param name="id">идентификатор записи</param>
        public List<Country> Select(int? id)
        {
            var result = new List<Country>();

            using (var con = new NpgsqlConnection(query.connection))
            {
                con.Open();
                string queryString = id == null ? query.select_countries : query.select_country_id;
                var command = new NpgsqlCommand(queryString, con);

                if (id != null)
                {
                    command.Parameters.AddWithValue("@id", id.Value);
                }

                var reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    result.Add(new Country
                    {
                        Id = (int)reader["id"],
                        Land = (string)reader["land"],
                        Name = (string)reader["name"],
                        Population = (int)reader["population"]
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает запись с указанным name из таблицы countries
        /// </summary>
        /// <param name="name">имя страны</param>
        public Country Select(string name)
        {
            Country result;

            using (var con = new NpgsqlConnection(query.connection))
            {
                con.Open();
                string queryString = query.select_country_name;
                var command = new NpgsqlCommand(queryString, con);

                command.Parameters.AddWithValue("@name", name);

                var reader = command.ExecuteReader();
                reader.Read();

                result = new Country
                {
                    Id = (int)reader["id"],
                    Land = (string)reader["land"],
                    Name = (string)reader["name"],
                    Population = (int)reader["population"]
                };
            }

            return result;
        }

        /// <summary>
        /// Вставляет в бд запись о стране
        /// </summary>
        /// <param name="country">страна</param>
        public bool Insert(Country country)
        {
            try
            {
                using (var con = new NpgsqlConnection(query.connection))
                {
                    con.Open();
                    var queryString = query.insert_country;
                    var command = new NpgsqlCommand(queryString, con);

                    command.Parameters.AddWithValue("@land", country.Land);
                    command.Parameters.AddWithValue("@population", country.Population);
                    command.Parameters.AddWithValue("@name", country.Name);

                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("An unhandled insert exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;            
            }
            return true;
        }

        /// <summary>
        /// обновляет в бд запись о стране
        /// </summary>
        /// <param name="country">страна</param>
        public bool Update(Country country)
        {
            try
            {
                using (var con = new NpgsqlConnection(query.connection))
                {
                    con.Open();
                    var queryString = query.update_country;
                    var command = new NpgsqlCommand(queryString, con);

                    command.Parameters.AddWithValue("@land", country.Land);
                    command.Parameters.AddWithValue("@population", country.Population);
                    command.Parameters.AddWithValue("@name", country.Name);
                    command.Parameters.AddWithValue("@id", country.Id);

                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("An unhandled update exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;            
            }
            return true;
        }

        /// <summary>
        /// Удаляет страну из таблицы стран
        /// </summary>
        /// <param name="country">Экземпляр страны</param>
        public bool Delete(Country country)
        {
            try
            {
                using (var con = new NpgsqlConnection(query.connection))
                {
                    con.Open();
                    var queryString = query.delete_country;
                    var command = new NpgsqlCommand(queryString, con);

                    command.Parameters.AddWithValue("@id", country.Id);

                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("An unhandled delete exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;            
            }
            return true;
        }
    }
}
