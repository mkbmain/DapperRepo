using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace Mkb.DapperRepo.Repo
{
    public class SqlRepo : SqlRepoBase
    {
        public SqlRepo(string connectionString) : base(connectionString)
        {
        }
        
        public virtual T QuerySingle<T>(string sql)
        {
            return BaseGetAll((connection, sql2) => connection.QueryFirstOrDefault<T>(sql2), sql);
        }

        public virtual IEnumerable<T> QueryMany<T>(string sql)
        {
            return BaseGetAll((connection, sql2) => connection.Query<T>(sql2), sql);
        }
        
        public virtual IEnumerable<T> GetAllByX<T,PropT>(string property, object term) where T : class, new()
        {
            return GetAllByX<T,PropT>(new T(), property, term);
        }
        
        public virtual IEnumerable<T> GetAllByX<T,PropT>(T item, string property,object valueToSearchBy)
        {
            var theField = ReflectionUtils.GetPropertyInfoOfType<T>(typeof(PropT), property);
            theField.SetValue(item, valueToSearchBy);
            return BaseGetAllByX<T, IEnumerable<T>>((connection, s) => connection.Query<T>(s, item), property);
        }

        public virtual T GetById<T>(T element)
        {
            return BaseGet<T, T>((connection, s) => connection.QueryFirstOrDefault<T>(s, element));
        }

        public virtual IEnumerable<T> GetAll<T>()
        {
            return BaseGetAll<T, IEnumerable<T>>((connection, s) => connection.Query<T>(s));
        }

        public virtual IEnumerable<T> Search<T>(string property, string term) where T : class, new()
        {
            return Search(new T(), property, term);
        }

        public virtual IEnumerable<T> Search<T>(T item, string property, string term)
        {
            var theField = ReflectionUtils.GetPropertyInfoOfType<T>(typeof(string), property);
            theField.SetValue(item, term);
            return BaseSearch<T, IEnumerable<T>>((connection, s) => connection.Query<T>(s, item), property);
        }

        public virtual T Add<T>(T element)
        {
            return BaseAdd(new[] {element}, (connection, s) => connection.QuerySingle<T>(s, element), true);
        }

        public virtual void AddMany<T>(IEnumerable<T> elements)
        {
            BaseAdd(elements, (connection, s) =>
            {
                connection.Execute(s, elements);
                return Task.CompletedTask;
            }, false);
        }

        public virtual void Update<T>(T element, bool ignoreNullProperties = false)
        {
            BaseUpdate(element, ignoreNullProperties, (connection, s) =>
            {
                connection.Execute(s, new[] {element});
                return Task.CompletedTask;
            });
        }

        public virtual void Delete<T>(T element)
        {
            BaseDelete<T>((connection, s) =>
            {
                connection.Execute(s, new[] {element});
                return Task.CompletedTask;
            });
        }
    }
}