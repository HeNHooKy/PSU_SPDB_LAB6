using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Simple_CRUD.Model
{
    class DataProvider
    {
        Query query;

        public DataProvider()
        {
            this.query = new Query("Host = localhost; Port = 5432; Database = postgres; Username = postgres; Password = pereterebi123");
        }

        /// <summary>
        /// Метод сохраняет все актуальные данные из выбранной таблицы в списке объектов
        /// </summary>
        /// <param name="table">table name</param>
        /// <returns>
        /// Item1: types
        /// Item2: headers
        /// Item3: list of data
        /// </returns>
        public (List<String>, List<String>, List<List<object>>) Select(string table)
        {
            List<String> types = new List<String>();
            List<String> headers = new List<String>();
            List<List<object>> list = new List<List<object>>();
            using (var connection = new NpgsqlConnection(query.connectionString))
            {
                connection.Open();
                var reuqest = String.Format(query.select, table);
                var command = new NpgsqlCommand(reuqest, connection);
                var reader = command.ExecuteReader();
                int columns = reader.FieldCount;

                for(int i = 0; i < columns; i++)
                {
                    types.Add(reader.GetDataTypeName(i));
                    headers.Add(reader.GetName(i));
                }

                while(reader.Read())
                {
                    List<object> row = new List<object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetValue(i));
                    }
                    list.Add(row);
                }
            }
            return (types, headers, list);
        }

        /// <summary>
        /// Вставить новую запись с данными row в таблицу
        /// </summary>
        /// <param name="row">Объект с данными</param>
        /// <returns>Состояние вставки</returns>
        public bool Insert(string table, dynamic row)
        {
            return true;
        }

        /// <summary>
        /// Обновить данные о записи в таблице
        /// </summary>
        /// <param name="row">Объект с новыми данными</param>
        /// <returns>Состояние обновления</returns>
        public bool Update(string table, dynamic row)
        {
            return false;
        }
    }
}
