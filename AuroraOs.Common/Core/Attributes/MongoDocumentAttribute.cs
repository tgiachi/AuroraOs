using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Attributes
{
    /// <summary>
    /// Class for map Entities to MongoDB Document
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoDocumentAttribute : Attribute
    {
        /// <summary>
        /// Name of collection
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="collectionName"></param>
        public MongoDocumentAttribute(string collectionName = "")
        {
            CollectionName = collectionName;
        }
    }
}
