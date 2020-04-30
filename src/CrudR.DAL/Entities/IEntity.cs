using System;

namespace CrudR.DAL.Entities
{
    /// <summary>
    /// Entity interface representing the most basic form of storage
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Entity unique Id
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Entity data revision
        /// </summary>
        Guid Revision { get; set; }
    }
}
