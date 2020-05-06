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
        public string ConnectionString { get; set; }

        /// <summary>
        /// The name of the database to use
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
