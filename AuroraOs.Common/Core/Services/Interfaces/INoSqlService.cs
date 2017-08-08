using AuroraOs.Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Services.Interfaces
{
    public interface INoSqlService : IAuroraService
    {
        void Insert<T>(T obj);

        void Insert<T>(IEnumerable<T> list);

        List<T> Select<T>(Expression<Func<T, bool>> func);

        long Delete<T>(Expression<Func<T, bool>> func);

        List<T> SelectAll<T>();
    }
}
