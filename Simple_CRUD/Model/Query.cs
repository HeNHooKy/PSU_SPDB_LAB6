using System;
using System.Collections.Generic;
using System.Text;

namespace Simple_CRUD.Model
{
    public class Query
    {
        public string connection { get; }

        public Query()
        {
            connection = "Host = localhost; Port = 5432; Database = postgres; Username = postgres; Password = pereterebi123";
        }

        public string select_country_name = "SELECT id, land, population, name FROM countries WHERE name = @name";
        public string select_country_id = "SELECT id, land, population, name FROM countries WHERE id = @id";
        public string select_countries = "SELECT id, land, population, name FROM countries";
        public string insert_country = "INSERT INTO countries (land, population, name) VALUES (@land, @population, @name)";
        public string update_country = "UPDATE countries SET land = @land, population = @population, name = @name " +
            "WHERE id = @id";
        public string delete_country = "DELETE FROM countries WHERE id = @id";


        public string select_man_id = "SELECT id, name, surname, country_born_id FROM people WERE id = @id";
        public string select_people = "SELECT id, name, surname, country_born_id FROM people";
        public string insert_man = "INSERT INTO people (name, surname, country_born_id) VALUES (@name, @surname, @country_born_id)";
        public string update_man = "UPDATE people SET name = @name, surname = @surname, country_born_id = @country_born_id " +
            "WHERE id = @id";
        public string delete_man = "DELETE FROM people WHERE id = @id";

    }
}
