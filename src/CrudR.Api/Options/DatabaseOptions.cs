using System.ComponentModel.DataAnnotations;
using CrudR.DAL.Options;

namespace CrudR.Api.Options
{
    /// <summary>
    /// Database configuration options
    /// </summary>
    internal class DatabaseOptions : IDatabaseOptions
    {
        /// <summary>
        /// The connection string of the database
        /// </summary>
        [Required]
        public string ConnectionString { get; set; }

        /// <summary>
        /// The name of the database to use
        /// </summary>
        [Required]
        public string DatabaseName { get; set; }
    }
}
