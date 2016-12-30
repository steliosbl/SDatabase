#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SDatabase.Attributes.AssignManually.cs">
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

namespace SDatabase.Attributes
{
    using System;

    /// <summary>
    /// Property attribute that allows the converter to know which properties must be set manually and not through a constructor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class AssignManually : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssignManually"/> class with identical Name and DatabaseEquivalent properties.
        /// </summary>
        /// <param name="name">The name of the property and of the column that corresponds to it.</param>
        public AssignManually(string name) : this(name, name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignManually"/> class.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="databaseEquivalent">The name of the column that corresponds to the property.</param>
        public AssignManually(string name, string databaseEquivalent)
        {
            this.Name = name;
            this.DatabaseEquivalent = databaseEquivalent;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the name of the column that corresponds to the property.
        /// </summary>
        /// <value>
        /// The name of the column that corresponds to the property.
        /// </value>
        public string DatabaseEquivalent { get; private set; }
    }
}
