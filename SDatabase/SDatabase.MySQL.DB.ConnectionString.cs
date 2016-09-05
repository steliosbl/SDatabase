namespace SDatabase.MySQL
{
    using System;
    using System.Collections.Generic;

    public class ConnectionString
    {
        public ConnectionString(string server, int port, string database, string uid, string pwd)
        {
            this.Server = server;
            this.Port = port;
            this.Database = database;
            this.Uid = uid;
            this.Pwd = pwd;
            this.Generate();
        }

        public ConnectionString(string text)
        {
            this.Text = text.Trim();
            this.Parse();
        }

        public string Server { get; private set; }

        public int Port { get; private set; }

        public string Database { get; private set; }

        public string Uid { get; private set; }

        public string Pwd { get; private set; }

        public string Text { get; private set; }

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
