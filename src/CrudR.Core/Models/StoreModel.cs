using System.Text.Json;

namespace CrudR.Core.Models
{
    /// <summary>
    /// Data model representing the Json payload to be stored
    /// </summary>
    public class StoreModel
    {
        /// <summary>
        /// The constructor to initialise the model properties
        /// </summary>
        /// <param name="id">Id of the Json payload</param>
        /// <param name="payLoad">The Json payload</param>
        public StoreModel(string id, JsonElement payLoad) =>
            (Id, Payload) = (id, payLoad);

        /// <summary>
        /// Id of the Json payload
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The Json payload
        /// </summary>
        public JsonElement Payload { get; }
    }
}
