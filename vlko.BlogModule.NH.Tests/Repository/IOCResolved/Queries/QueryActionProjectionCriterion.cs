﻿using NHibernate.Criterion;
using vlko.core.NH.Repository;
using vlko.core.Repository;
using vlko.BlogModule.Tests.Repository.IOCResolved.Model;

namespace vlko.BlogModule.Tests.Repository.IOCResolved.Queries
{
    public class QueryActionProjectionCriterion : BaseCriterionQueryAction<Room>, IQueryActionProjection
    {
        public IQueryResult<RoomWithHotelProjection> DoProjection()
        {
			Hotel Hotel = null;
			string RoomName = null, HotelName = null;
        	return new ProjectionQueryResult<Room, RoomWithHotelProjection>(
        		Criteria.JoinAlias(room => room.Hotel, () => Hotel),
        		Projections.ProjectionList()
        			.Add(Projections.Property<Room>(room => room.Name).WithAlias(() => RoomName))
					.Add(Projections.Property<Room>(room => room.Hotel.Name).WithAlias(() => HotelName)));
        }

    }
}
