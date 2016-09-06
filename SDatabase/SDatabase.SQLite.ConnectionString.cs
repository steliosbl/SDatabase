namespace SDatabase.SQLite
{
    using System;
    using System.Collections.Generic;

    public class ConnectionString
    {
        public ConnectionString(string dataSource, int version)
        {
            this.DataSource = dataSource;
            this.Version = version;
            this.Generate();
        }

        public ConnectionString(string text)
        {
            this.Text = text;
            this.Parse();
        }

        public string DataSource { get; private set; }

        public int Version { get; private set; }

        public string Text { get; private set; }

        private void Generate()
        {
            // User error checks:
            if (string.IsNullOrWhiteSpace(this.DataSource) || this.DataSource == string.Empty)
            {
                throw new ArgumentException("Data source required!", "DataSource");
            }
            else if (this.Version <=0 || this.Version > 3)
            {
                throw new ArgumentException("Valid version number required!", "Version");
            }

            string connectionString = string.Empty;
            connectionString += "Data Source=" + this.DataSource + ";";
            connectionString += "Version=" + this.Version.ToString() + ";";
            this.Text = connectionString;
        }

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