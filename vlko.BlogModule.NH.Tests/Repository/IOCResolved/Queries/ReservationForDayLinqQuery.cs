﻿using System;
using System.Linq;
using vlko.BlogModule.NH.Tests.Repository.IOCResolved.Model;
using vlko.core.NH.Repository;
using vlko.core.Repository;

namespace vlko.BlogModule.NH.Tests.Repository.IOCResolved.Queries
{
    public class ReservationForDayLinqQuery : LinqQuery<Reservation>, IReservationForDayQuery
    {
        /// <summary>
        /// Wheres the date.
        /// </summary>
        /// <param name="reservationDate">The reservation date.</param>
        /// <returns>
        /// List of reservations with resolved room relation.
        /// </returns>
        public IQueryResult<Reservation> WhereDate(DateTime reservationDate)
        {
			return Result(Queryable.Where(reserv => reserv.CompositeKey.ReservationDate == reservationDate));
        }
    }
}
