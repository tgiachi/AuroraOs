using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Entities;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Common.Core.Services.Interfaces;
using MongoDB.Driver;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Services
{
    [AuroraService("System")]
    public class NoSqlService : INoSqlService
    {

        private string _connectionString = null;
        private string _databaseName = null;

        private MongoClient _mongoClient;
        private IMongoDatabase _database;
        private ILogger _logger = LogManager.GetCurrentClassLogger();
      
        public NoSqlService()
        {
            CheckConfig();

            Init();

        }

        private void CheckConfig()
        {
            _connectionString = ConfigManager.Instance.GetConfigValue<NoSqlService>("connectionString");
            _databaseName = ConfigManager.Instance.GetConfigValue<NoSqlService>("databaseName");

            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = "mongodb://localhost:27017";

                ConfigManager.Instance.SetConfig<NoSqlService>("connectionString", _connectionString);
            }

            if (string.IsNullOrEmpty(_databaseName))
            {
                _databaseName = "auroraos_db";
                ConfigManager.Instance.SetConfig<NoSqlService>("databaseName", _databaseName);
            }
        }

        private void Init()
        {
            try
            {
                _logger.Info($"Connecting to {_connectionString} - Database {_databaseName}");
                _mongoClient = new MongoClient(_connectionString);
                _database = _mongoClient.GetDatabase(_databaseName);
                _logger.Info($"Connected to {_connectionString} - Database {_databaseName}!");

            }
            catch (Exception ex)
            {
                _logger.Error($"Error during connecting to {_connectionString}!");
                _logger.Error(ex);
            }
        }


        public void Insert<T>(T obj) where T : BaseNoSqlEntity
        {
            try
            {
                _database.GetCollection<T>(GetCollectionByName<T>()).InsertOne(obj);
            }
            catch
            {
                Update<T>(obj);

            }
          
        }

        public void Update<T>(T obj) where T: BaseNoSqlEntity
        {
            _database.GetCollection<T>(GetCollectionByName<T>()).ReplaceOne(e => e.Id == obj.Id, obj);
        }

        public long Delete<T>(Expression<Func<T, bool>> func) where T : BaseNoSqlEntity
        {
            var dr = _database.GetCollection<T>(GetCollectionByName<T>()).DeleteMany(func);
            return dr.DeletedCount;
        }

        public void Insert<T>(IEnumerable<T> list) where T : BaseNoSqlEntity
        {
            _database.GetCollection<T>(GetCollectionByName<T>()).InsertMany(list);
        }


        public List<T> Select<T>(Expression<Func<T, bool>> func)
        {
            return _database.GetCollection<T>(GetCollectionByName<T>()).AsQueryable<T>().Where(func).ToList();
        }

        private string GetCollectionByName<T>()
        {
            var attr = typeof(T).GetCustomAttribute<MongoDocumentAttribute>();

            if (attr == null) throw new Exception($"Type {typeof(T)} must implement [MongoDocument] attribute");

            return attr.CollectionName;
        }

        public List<T> SelectAll<T>()
        {
            return _database.GetCollection<T>(GetCollectionByName<T>()).AsQueryable<T>().ToList();
        }


        public void Dispose()
        {
            
        }
    }
}
