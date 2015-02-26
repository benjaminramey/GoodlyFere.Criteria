#region License

// ------------------------------------------------------------------------------------------------------------------
//  <copyright file="SpecificationException.cs">
// GoodlyFere.Criteria
//  
//  Copyright (C) 2014 Ben Ramey
//  
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//  Lesser General Public License for more details.
//  
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//  
//  http://www.gnu.org/licenses/lgpl-2.1-standalone.html
//  
//  You can contact me at ben.ramey@gmail.com.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

#endregion

#region Usings

using System;
using System.Linq;

#endregion

namespace GoodlyFere.Criteria.Exceptions
{
    public class CriteriaException : Exception
    {
        #region Constants and Fields

        private readonly Type _criteriaType;
        private readonly string _value;

        #endregion

        #region Constructors and Destructors

        public CriteriaException(Type criteriaType, string value)
        {
            _criteriaType = criteriaType;
            _value = value;
        }

        public CriteriaException(Type criteriaType, string value, string message)
            : base(message)
        {
            _criteriaType = criteriaType;
            _value = value;
        }

        public CriteriaException(Type criteriaType, string value, string message, Exception innerException)
            : base(message, innerException)
        {
            _criteriaType = criteriaType;
            _value = value;
        }

        #endregion

        #region Public Properties

        public override string Message
        {
            get
            {
                return string.Format("{0} was not satisified by {1}! {2}", _criteriaType.Name, _value, base.Message);
            }
        }

        #endregion
    }
}