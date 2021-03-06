﻿#region License

// ------------------------------------------------------------------------------------------------------------------
//  <copyright file="NotCriteria.cs">
//  GoodlyFere.Criteria
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
using System.Linq.Expressions;

#endregion

namespace GoodlyFere.Criteria
{
    public class NotCriteria<T> : UnaryCriteria<T>
    {
        #region Constructors and Destructors

        public NotCriteria(ICriteria<T> criteria)
        {
            Operand = criteria;
        }

        #endregion

        #region Public Properties

        public override Expression<Func<T, bool>> Satisfier
        {
            get
            {
                return Operand.Not().Satisfier;
            }
        }

        #endregion

        #region Public Methods

        public override void Accept(ICriteriaVisitor<T> visitor)
        {
            visitor.Visit(this);
        }

        #endregion
    }
}