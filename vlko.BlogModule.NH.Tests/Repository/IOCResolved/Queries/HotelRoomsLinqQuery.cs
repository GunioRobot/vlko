﻿using System.Linq;
using vlko.BlogModule.NH.Tests.Repository.IOCResolved.Model;
using vlko.core.NH.Repository;
using vlko.core.Repository;

namespace vlko.BlogModule.NH.Tests.Repository.IOCResolved.Queries
{
    public class HotelRoomsLinqQuery : LinqQuery<Room>, IHotelRoomsQuery
    {
        /// <summary>
        /// Wheres the name of the hotel.
        /// </summary>
        /// <param name="hotelName">Name of the hotel.</param>
        /// <returns>Room result filtered with hotel name.</returns>
        public IQueryResult<Room> WhereHotelName(string hotelName)
        {
			return Result(Queryable.Where(room => room.Hotel.Name == hotelName));
        }
    }
}
