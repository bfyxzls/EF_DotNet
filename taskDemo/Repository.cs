using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace taskDemo
{
    public abstract class Repository<T> where T : class
    {
        DbContext db;
        public Repository(DbContext db)
        {
            this.db = db;
        }
        public T Find(params object[] id)
        {
            return db.Set<T>().Find(id);
        }
        /// <summary>
        /// 4.0 修改，如：
        /// T u = new T() { uId = 1, uLoginName = "asdfasdf" };
        /// this.Modify(u, "uLoginName");
        /// </summary>
        /// <param name="model">要修改的实体对象</param>
        /// <param name="proNames">要修改的 属性 名称</param>
        /// <returns></returns>
        public int Modify(T model, params string[] proNames)
        {
            //4.1将 对象 添加到 EF中
            DbEntityEntry<T> entry = db.Entry<T>(model);
            //4.2先设置 对象的包装 状态为 Modified
            entry.State = System.Data.EntityState.Unchanged;
            //4.3循环 被修改的属性名 数组
            foreach (var property in typeof(T).GetProperties()
               .Where(i => !proNames.Contains(i.Name)))
            {
                entry.Property(property.Name).IsModified = false;
            }
            //4.4一次性 生成sql语句到数据库执行
            return db.SaveChanges();
        }


        /// <summary>
        /// 4.0 修改，如：
        /// </summary>
        /// <param name="model">要修改的实体对象</param>
        /// <param name="properties">要修改的 属性 名称</param>
        /// <returns></returns>
        public int Modify(T model, params Expression<Func<T, object>>[] properties)
        {
            db.Configuration.AutoDetectChangesEnabled = false;
            var entry = db.Entry(model);
            entry.State = System.Data.EntityState.Unchanged;

            //4.3循环 被修改的属性名 数组
            foreach (var proName in properties)
            {
                entry.Property(GetName(proName)).IsModified = true;
            }

            //保存数据，不进行检查
            return  ((IObjectContextAdapter)db)
                .ObjectContext.SaveChanges();
        }

        public string GetName<TSource, TField>(Expression<Func<TSource, TField>> Field)
        {
            if (object.Equals(Field, null))
            {
                throw new NullReferenceException("Field is required");
            }

            MemberExpression expr = null;

            if (Field.Body is MemberExpression)
            {
                expr = (MemberExpression)Field.Body;
            }
            else if (Field.Body is UnaryExpression)
            {
                expr = (MemberExpression)((UnaryExpression)Field.Body).Operand;
            }
            else
            {
                const string Format = "Expression '{0}' not supported.";
                string message = string.Format(Format, Field);

                throw new ArgumentException(message, "Field");
            }

            return expr.Member.Name;
        }
    }
}
