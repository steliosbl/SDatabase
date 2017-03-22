#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SDatabase.Attributes.SDBConstructor.cs">
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

namespace SDatabase.Attributes
{
    using System;

    /// <summary>
    /// Constructor attribute that explicitly informs the converter which constructor should be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
    public sealed class SDBConstructor : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDBConstructor"/> class. 
        /// </summary>
        public SDBConstructor()
        {
        }
    }
}
