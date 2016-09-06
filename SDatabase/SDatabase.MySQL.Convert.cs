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
            using (var reader = cmd.ExecuteReader())
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

        public static void SerializeObject(MySqlConnection conn, object obj)
        {
            SerializeObject(conn, obj, obj.GetType().Name);
        }

        public static void SerializeObject(MySqlConnection conn, object obj, string table)
        {
            string cmdstr = "INSERT INTO " + table + " VALUES (";
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                cmdstr += "@" + property.Name + ",";
            }
            cmdstr = cmdstr.Remove(cmdstr.Length - 1);
            cmdstr += ");";
            using (var cmd = new MySqlCommand(cmdstr, conn))
            {
                cmd.Prepare();
                foreach (var property in properties)
                {
                    cmd.Parameters.AddWithValue("@" + property.Name, property.GetValue(obj));
                }
                cmd.ExecuteNonQuery();
            }
        }
    }
}
