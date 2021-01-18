using System;
using System.Collections.Generic;
using System.Text;

namespace Simple_CRUD.Model
{
    class Query
    {
        public readonly string connectionString;

        public string select = "SELECT * FROM {0};";

        public string insert = "INSERT INTO {0} ({1}) VALUES ({2});";

        public string update = "UPDATE {0} SET {1} WHERE id = @id;";

        public Query(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
