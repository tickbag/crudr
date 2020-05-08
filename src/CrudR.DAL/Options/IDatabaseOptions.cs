using System.ComponentModel.DataAnnotations;

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
        [Required]
        string ConnectionString { get; }

        /// <summary>
        /// Database name
        /// </summary>
        [Required]
        string DatabaseName { get; }
    }
}
