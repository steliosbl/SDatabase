namespace SDatabase.MySQL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class TypeLookup
    {
        public static Type GetCodeType(string mySQLType)
        {
            switch (mySQLType)
            {
                case "INT":
                case "INTEGER":
                    return typeof(int);

                case "TEXT":
                    return typeof(string);

                case "DATETIME":
                    return typeof(DateTime);

                case "BOOLEAN":
                    return typeof(bool);

                default:
                    return null;

            }
        }

        public static string GetMySQLType(Type type)
        {
            string typeName = type.Name;
            switch(typeName)
            {
                case "Int32":
                    return "INTEGER";

                case "String":
                    return "TEXT";

                case "DateTime":
                    return "DATETIME";

                case "Boolean":
                    return "BOOLEAN";

                default:
                    return null;
            }
        }
    }
}
