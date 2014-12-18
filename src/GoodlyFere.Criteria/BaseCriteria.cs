#region License

// ------------------------------------------------------------------------------------------------------------------
//  <copyright file="BaseCriteria.cs">
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
using System.Linq.Expressions;
using GoodlyFere.Criteria.Exceptions;

#endregion

namespace GoodlyFere.Criteria
{
    public abstract class BaseCriteria<T> : ICriteria<T>
    {
        #region Constants and Fields

        private Func<T, bool> _compiledSatisfier;

        #endregion

        #region Public Properties

        public abstract Expression<Func<T, bool>> Satisfier { get; }

        #endregion

        #region Properties

        protected virtual Func<T, bool> CompiledSatisfier
        {
            get
            {
                return _compiledSatisfier ?? (_compiledSatisfier = Satisfier.Compile());
            }
        }

        #endregion

        #region Public Methods

        public virtual void Accept(ICriteriaVisitor<T> visitor)
        {
            visitor.Visit(this);
        }

        public virtual bool IsNotSatisfiedBy(T item)
        {
            return !CompiledSatisfier.Invoke(item);
        }

        public virtual bool IsSatisfiedBy(T item)
        {
            return CompiledSatisfier.Invoke(item);
        }

        public virtual void ThrowIfNotSatisfiedBy(T item)
        {
            if (IsNotSatisfiedBy(item))
            {
                throw new SpecificationException(GetType(), item == null ? "null" : item.ToString());
            }
        }

        #endregion
    }
}