#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SDatabase.SQLite.Convert.cs">
//
// Copyright (C) 2016 Stelio Logothetis
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// <summary>
// SDatabase database interface library for C#.
// Email: stel.logothetis@gmail.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

/// <summary>
/// Contains classes and methods pertaining to SQLite databases.
/// </summary>
namespace SDatabase.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Contains conversion methods to write and read C# objects from SQLite databases.
    /// </summary>
    public static class Convert
    {
        /// <summary>
        /// Creates an object of the specified type using the SQLite server's response to the given command.
        /// </summary>
        /// <typeparam name="T">The type of object to be created and returned.</typeparam>
        /// <param name="cmd">The query used to acquire data.</param>
        /// <returns>An instance of the specified type with all the data acquired from the server.</returns>
        public static T DeserializeObject<T>(SQLiteCommand cmd)
        {
            // User error checks:
            if (string.IsNullOrWhiteSpace(cmd.CommandText) || cmd.CommandText == string.Empty)
            {
                throw new ArgumentException("Command must have valid text!", "CommandText");
            }
            else if (cmd.Connection == null)
            {
                throw new ArgumentException("Command must have valid connection!", "Connection");
            }
            else if (cmd.Connection.State == System.Data.ConnectionState.Closed)
            {
                throw new ArgumentException("Connection must be open!", "Connecton");
            }

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                    var types = Enumerable.Range(0, reader.FieldCount).Select(reader.GetFieldType).ToArray();

                    var constructor = typeof(T).GetConstructor(types);
                    if (constructor == null)
                    {
                        throw new ArgumentException("Type does not have a matching constructor!", "constructor");
                    }

                    var passParams = new List<object>();

                    foreach (var param in constructor.GetParameters())
                    {
                        foreach (var column in columns)
                        {
                            if (column.Equals(param.Name, StringComparison.InvariantCultureIgnoreCase))
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

        /// <summary>
        /// Writes the data from the provided object to the database through the given connection. Data is written to the table with the same name as the type of the object.
        /// </summary>
        /// <param name="conn">The connection to the SQLite server.</param>
        /// <param name="obj">The object to be serialized.</param>
        public static void SerializeObject(SQLiteConnection conn, object obj)
        {
            SerializeObject(conn, obj, obj.GetType().Name);
        }

        /// <summary>
        /// Writes the data from the provided object to the database through the given connection, in the table that has the given name.
        /// </summary>
        /// <param name="conn">The connection to the SQLite server.</param>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="table">The name of the table to write the data to.</param>
        public static void SerializeObject(SQLiteConnection conn, object obj, string table)
        {
            // User error checks:
            if (string.IsNullOrWhiteSpace(table) || table == string.Empty)
            {
                throw new ArgumentException("Valid table name required!", "table");
            }

            string cmdstr = "INSERT INTO " + table + " VALUES (";
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                cmdstr += "@" + property.Name + ",";
            }

            cmdstr = cmdstr.Remove(cmdstr.Length - 1);
            cmdstr += ");";
            using (var trans = conn.BeginTransaction())
            {
                using (var cmd = new SQLiteCommand(cmdstr, conn))
                {
                    cmd.Prepare();
                    foreach (var property in properties)
                    {
                        cmd.Parameters.AddWithValue("@" + property.Name, property.GetValue(obj));
                    }

                    cmd.ExecuteNonQuery();
                }

                trans.Commit();
            }
        }
    }
}
