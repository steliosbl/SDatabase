#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SDatabase.MySQL.ConnectionData.cs">
//
// Copyright (C) 2016 Stelios Boulitsakis
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
// Email: styboulits@gmail.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SDatabase.MySQL
{
    using System;

    /// <summary>
    /// MySQL connection data class.
    /// </summary>
    public class ConnectionData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionData" /> class.
        /// </summary>
        /// <param name="server">The server's address.</param>
        /// <param name="port">The server's port (usually 3306).</param>
        /// <param name="database">The name of the target schema.</param>
        /// <param name="uid">The username.</param>
        /// <param name="pwd">The password.</param>
        public ConnectionData(string server, int port, string database, string uid, string pwd)
        {
            // User error checks:
            if (string.IsNullOrWhiteSpace(server) || server == string.Empty)
            {
                throw new ArgumentException("Server address required!", "Server");
            }
            else if (port <= 0 || port > 65535)
            {
                throw new ArgumentException("Valid port required!", "Port");
            }
            else if (string.IsNullOrWhiteSpace(database) || database == string.Empty)
            {
                throw new ArgumentException("Database name required!", "Database");
            }
            else if (string.IsNullOrWhiteSpace(uid) || uid == string.Empty)
            {
                throw new ArgumentException("Username required!", "Uid");
            }
            else if (string.IsNullOrWhiteSpace(pwd) || pwd == string.Empty)
            {
                throw new ArgumentException("Password required!", "Pwd");
            }

            this.Server = server;
            this.Port = port;
            this.Database = database;
            this.Uid = uid;
            this.Pwd = pwd;
        }

        /// <summary>
        /// Gets the server's address.
        /// </summary>
        /// <value> 
        /// The server's address.
        /// </value>
        public string Server { get; private set; }

        /// <summary>
        /// Gets the server's port (usually 3306).
        /// </summary>
        /// <value>
        /// The server's port (usually 3306).
        /// </value>
        public int Port { get; private set; }

        /// <summary>
        /// Gets the name of the target schema.
        /// </summary>
        /// <value>
        /// The name of the target schema.
        /// </value>
        public string Database { get; private set; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Uid { get; private set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Pwd { get; private set; }
    }
}
