namespace SDatabase.MySQL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;

    public static class Convert
    {
        public static T DeserializeObject<T>(MySqlCommand cmd)
        { 
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                    var types = Enumerable.Range(0, reader.FieldCount).Select(reader.GetFieldType).ToArray();

                    var constructor = typeof(T).GetConstructor(types);

                    var passParams = new List<object>();

                    foreach (var param in constructor.GetParameters())
                    {
                        foreach (var column in columns)
                        {
                            if (column == param.Name)
                            {
                                passParams.Add(reader[column]);
                            }
                        }
                    }

                    return (T)Activator.CreateInstance(typeof(T), passParams.ToArray());
                }
            }
            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}
