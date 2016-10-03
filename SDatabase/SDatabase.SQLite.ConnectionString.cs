#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SDatabase.SQLite.ConnectionString.cs">
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

namespace SDatabase.SQLite
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// SQLite connection string class.
    /// </summary>
    public class ConnectionString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionString"/> class. 
        /// </summary>
        /// <param name="dataSource">The database's data source (i.e. filename).</param>
        /// <param name="version">The version of SQLite to be used (latest is 3).</param>
        public ConnectionString(string dataSource, int version)
        {
            this.DataSource = dataSource;
            this.Version = version;
            this.Generate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionString"/> class. 
        /// </summary>
        /// <param name="text">The connection string.</param>
        public ConnectionString(string text)
        {
            this.Text = text;
            this.Parse();
        }

        /// <summary>
        /// Gets database's data source.
        /// </summary>
        public string DataSource { get; private set; }

        /// <summary>
        /// Gets the version of SQLite being used.
        /// </summary>
        public int Version { get; private set; }

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
            if (string.IsNullOrWhiteSpace(this.DataSource) || this.DataSource == string.Empty)
            {
                throw new ArgumentException("Data source required!", "DataSource");
            }
            else if (this.Version <= 0 || this.Version > 3)
            {
                throw new ArgumentException("Valid version number required!", "Version");
            }

            string connectionString = string.Empty;
            connectionString += "Data Source=" + this.DataSource + ";";
            connectionString += "Version=" + this.Version.ToString() + ";";
            this.Text = connectionString;
        }

        /// <summary>
        /// Parses the given connection string and extracts the connection data.
        /// </summary>
        private void Parse()
        {
            string[] requiredElements = { "Data Source", "Version" };

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
                int version = System.Convert.ToInt32(connectionData["Version"]);
                if (version > 0 || version < 4)
                {
                    this.DataSource = connectionData["Data Source"];
                    this.Version = version;
                }
                else
                {
                    throw new ArgumentException("Connection string element {Version} is invalid!", "Version");
                }
            }
            catch (FormatException)
            {
                throw new ArgumentException("Connection string element {Version} is invalid!", "Version");
            }
        }
    }
}