namespace CrudR.DAL.Integration
{
    /// <summary>
    /// The result of a data modification operation on the database
    /// </summary>
    public class DataModificationResult
    {
        /// <summary>
        /// The data modification result constructor
        /// </summary>
        /// <param name="recordsModified">The number of records that was modified</param>
        public DataModificationResult(long recordsModified) =>
            RecordsModified = recordsModified;

        /// <summary>
        /// The number of records that was modified
        /// </summary>
        public long RecordsModified { get; }
    }
}
