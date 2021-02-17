using Simple_CRUD.Model.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple_CRUD.View.Support
{
    class Tables
    {
        public enum Numbers
        {
            Country,
            Engine,
            Game,
            Man,
            Playground,
            Price,
            Publisher,
            Studio,
            User
        }

        public string GetTableName(int number)
        {
            switch(number)
            {
                case (int)Numbers.Country:
                    return "Country";
                case (int)Numbers.Engine:
                    return "Engine";
                case (int)Numbers.Game:
                    return "Game";
                case (int)Numbers.Man:
                    return "Man";
                case (int)Numbers.Playground:
                    return "Playground";
                case (int)Numbers.Price:
                    return "Price";
                case (int)Numbers.Publisher:
                    return "Publisher";
                case (int)Numbers.Studio:
                    return "Studio";
                default:
                    return "";
            }
        }
    }
}
