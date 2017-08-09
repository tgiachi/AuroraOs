using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Entities
{
    public class BaseNoSqlEntity
    {
        public ObjectId Id { get; set; }
    }
}
