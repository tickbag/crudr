using System;
using MongoDB.Bson;

namespace CrudR.DAL.Entities
{
    /// <summary>
    /// Store Entity
    /// </summary>
    internal class StoreEntity : IEntity
    {
        /// <summary>
        /// Unique Id of the entity
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Data revision of the entity
        /// </summary>
        public Guid Revision { get; set; }

        /// <summary>
        /// Json payload of the entity
        /// </summary>
        public BsonDocument Payload { get; set; }
    }
}
