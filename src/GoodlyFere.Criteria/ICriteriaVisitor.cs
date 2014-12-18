#region License

// ------------------------------------------------------------------------------------------------------------------
//  <copyright file="ICriteriaVisitor.cs">
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

namespace GoodlyFere.Criteria
{
    public interface ICriteriaVisitor<T>
    {
        #region Public Methods

        void Visit(ICriteria<T> criteria);

        void Visit(BinaryCriteria<T> criteria);

        void Visit(UnaryCriteria<T> criteria);

        void Visit(AndCriteria<T> criteria);

        void Visit(NotCriteria<T> criteria);

        void Visit(OrCriteria<T> criteria);

        #endregion
    }
}