#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SDatabase.MySQL.TypeLookup.cs">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Contains methods that convert MySQL and C# types.
    /// </summary>
    public static class TypeLookup
    {
        /// <summary>
        /// Converts MySQL types to C# types.
        /// </summary>
        /// <param name="mySQLType">The MySQL type in the form of a string.</param>
        /// <returns>The C# type that corresponds to the given MySQL type.</returns>
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

                case "FLOAT":
                    return typeof(float);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Converts C# types to MySQL types.
        /// </summary>
        /// <param name="type">The C# type to be converted.</param>
        /// <returns>The corresponding MySQL type in string form.</returns>
        public static string GetMySQLType(Type type)
        {
            string typeName = type.Name;
            switch (typeName)
            {
                case "Int32":
                    return "INTEGER";

                case "String":
                    return "TEXT";

                case "DateTime":
                    return "DATETIME";

                case "Boolean":
                    return "BOOLEAN";

                case "List":
                    return "TEXT";

                case "Float":
                    return "FLOAT";

                default:
                    return null;
            }
        }
    }
}
