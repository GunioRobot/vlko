﻿using NHibernate.Criterion;
using NHibernate.LambdaExtensions;
using vlko.model.Repository;
using vlko.model.Repository.NH;
using vlko.model.Tests.Repository.IOCResolved.Model;

namespace vlko.model.Tests.Repository.IOCResolved.Queries
{
    public class QueryActionProjectionCriterion : BaseCriterionQueryAction<Room>, IQueryActionProjection
    {
        public IQueryResult<RoomWithHotelProjection> DoProjection()
        {
            Hotel Hotel = null;
            string RoomName = null, HotelName = null;
            return new ProjectionQueryResult<Room, RoomWithHotelProjection>(
                    Criteria
                            .CreateAlias<Room>(room => room.Hotel, () => Hotel),
                    Projections.ProjectionList()
                                       .Add(LambdaProjection.Property<Room>(room => room.Name)
                                            .As(() => RoomName))
                                       .Add(LambdaProjection.Property<Room>(room => room.Hotel.Name)
                                            .As(() => HotelName)));
        }

    }
}
