#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SDatabase.Common.Convert.cs">
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

namespace SDatabase.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Container for functions used by all SDatabase converters.
    /// </summary>
    public static partial class Convert
    {
        /// <summary>
        /// Finds the ideal constructor to use given a dictionary containing the names and types of a set of columns.
        /// </summary>
        /// <typeparam name="T">The type for which to find the constructor.</typeparam>
        /// <param name="columns">Dictionary containing the names (key) and types (value) of a set of columns.</param>
        /// <returns>The ideal constructor to use.</returns>
        public static ConstructorInfo GetIdealConstructor<T>(Dictionary<string, Type> columns)
        {
            int maxCommon = 0;
            var constructors = new List<ConstructorInfo>();
            foreach (var constructor in typeof(T).GetConstructors())
            {
                // Return immediately if the constructor to be used is explicitly set
                if (constructor.GetCustomAttribute<Attributes.SDBConstructor>() != null)
                {
                    return constructor;
                }

                var parameters = new Dictionary<string, Type>();
                foreach (var parameter in constructor.GetParameters())
                {
                    if (parameter.ParameterType.Name == "List`1")
                    {
                        parameters.Add(parameter.Name, typeof(string));
                    }
                    else
                    {
                        parameters.Add(parameter.Name, parameter.ParameterType);
                    }
                }

                int temp = columns.Keys.ToList().Intersect(parameters.Keys.ToList(), StringComparer.InvariantCultureIgnoreCase).Select(param => parameters.ContainsKey(param) && parameters[param] == columns[param]).Count();
                if (temp > maxCommon)
                {
                    maxCommon = temp;
                    constructors.Add(constructor);
                }
            }

            // $constructors is reversed before iteration so that the constructor with the most common params is first instead of last
            constructors.Reverse();

            foreach (var constructor in constructors)
            {
                if (constructor.GetParameters().Count() <= columns.Count())
                {
                    return constructor;
                }
            }

            return null;
        }
    }
}
