namespace CrudR.DAL.Options
{
    /// <summary>
    /// Abstract interface for database settings
    /// </summary>
    public interface IDatabaseOptions
    {
        /// <summary>
        /// Database connection string
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Database name
        /// </summary>
        string DatabaseName { get; }
    }
}
