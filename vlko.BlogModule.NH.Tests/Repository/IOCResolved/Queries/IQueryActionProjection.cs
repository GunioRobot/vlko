﻿using vlko.core.Repository;
using vlko.BlogModule.Tests.Repository.IOCResolved.Model;

namespace vlko.BlogModule.Tests.Repository.IOCResolved.Queries
{
	public interface IQueryActionProjection : IAction<Room>
    {

        /// <summary>
        /// Does the projection.
        /// </summary>
        /// <returns>Room with hotel projection.</returns>
        IQueryResult<RoomWithHotelProjection> DoProjection();
    }
}
