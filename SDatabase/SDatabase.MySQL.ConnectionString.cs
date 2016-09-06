#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SDatabase.MySQL.ConnectionString.cs">
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
/// Contains classes and methods pertaining to MySQL databases.
/// </summary>
namespace SDatabase.MySQL
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// MySQL connection string class.
    /// </summary>
    public class ConnectionString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionString" /> class.
        /// </summary>
        /// <param name="server">The server's address.</param>
        /// <param name="port">The server's port (usually 3306).</param>
        /// <param name="database">The name of the target schema.</param>
        /// <param name="uid">The username.</param>
        /// <param name="pwd">The password.</param>
        public ConnectionString(string server, int port, string database, string uid, string pwd)
        {
            this.Server = server;
            this.Port = port;
            this.Database = database;
            this.Uid = uid;
            this.Pwd = pwd;
            this.Generate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionString" /> class.
        /// </summary>
        /// <param name="text">The connection string.</param>
        public ConnectionString(string text)
        {
            this.Text = text.Trim();
            this.Parse();
        }

        /// <summary>
        /// Gets the server's address.
        /// </summary>
        public string Server { get; private set; }

        /// <summary>
        /// Gets the server's port (usually 3306).
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Gets the name of the target schema.
        /// </summary>
        public string Database { get; private set; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Uid { get; private set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Pwd { get; private set; }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Generates a valid connection string using the connection data provided.
        /// </summary>
        private void Generate()
        {
            // User error checks:
            if (string.IsNullOrWhiteSpace(this.Server) || this.Server == string.Empty)
            {
                throw new ArgumentException("Server address required!", "Server");
            }
            else if (this.Port <= 0 || this.Port > 65535)
            {
                throw new ArgumentException("Valid port required!", "Port");
            }
            else if (string.IsNullOrWhiteSpace(this.Database) || this.Database == string.Empty)
            {
                throw new ArgumentException("Database name required!", "Database");
            }
            else if (string.IsNullOrWhiteSpace(this.Uid) || this.Uid == string.Empty)
            {
                throw new ArgumentException("Username required!", "Uid");
            }
            else if (string.IsNullOrWhiteSpace(this.Pwd) || this.Pwd == string.Empty)
            {
                throw new ArgumentException("Password required!", "Pwd");
            }

            // Connection string generation:
            string connectionString = string.Empty;
            connectionString += "Server=" + this.Server + "; ";
            connectionString += "Port=" + this.Port.ToString() + "; ";
            connectionString += "Database=" + this.Database + "; ";
            connectionString += "Uid=" + this.Uid + "; ";
            connectionString += "Pwd=" + this.Pwd + "; ";
            this.Text = connectionString.Trim();
        }

        /// <summary>
        /// Parses the given connection string and extracts the connection data.
        /// </summary>
        private void Parse()
        {
            string[] requiredElements = { "Server", "Port", "Database", "Uid", "Pwd" };

            // User error check:
            if (string.IsNullOrWhiteSpace(this.Text) || this.Text == string.Empty)
            {
                throw new ArgumentException("Valid connection string required!", this.Text);
            }

            // Connection string conversion:
            var connectionData = new Dictionary<string, string>();
            var connectionStringElements = this.Text.Split(';');

            // Split array should be one element larger than that containing required elements.
            // This is because the connection string ends with a ';'.
            if (connectionStringElements.Length != requiredElements.Length + 1)
            {
                throw new ArgumentException("Invalid connection string!", this.Text);
            }

            foreach (string element in connectionStringElements)
            {
                if (element != string.Empty)
                {
                    var splitElement = element.Split('=');
                    connectionData.Add(splitElement[0].Trim(), splitElement[1].Trim());
                }
            }

            // Connection string verification:
            foreach (string element in requiredElements)
            {
                if (!connectionData.ContainsKey(element))
                {
                    throw new ArgumentException("Connection string missing element {" + element + "}!", element);
                }
                else if (connectionData[element] == string.Empty)
                {
                    throw new ArgumentException("Connection string element {" + element + "} invalid!", element);
                }
            }

            try
            {
                int port = System.Convert.ToInt32(connectionData["Port"]);
                if (port > 0 || port < 65535)
                {
                    this.Server = connectionData["Server"];
                    this.Port = System.Convert.ToInt32(connectionData["Port"]);
                    this.Database = connectionData["Database"];
                    this.Uid = connectionData["Uid"];
                    this.Pwd = connectionData["Pwd"];
                }
                else
                {
                    throw new ArgumentException("Connection string element {Port} invalid!", "Port");
                }
            }
            catch (FormatException)
            {
                throw new ArgumentException("Connection string element {Port} invalid!", "Port");
            }
        }
    }
}
