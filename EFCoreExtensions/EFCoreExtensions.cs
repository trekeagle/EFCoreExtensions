using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CodeTreker.Utils.DB
{
    public static class EFCoreExtensions
    {
        private static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection connection, params object[] parameters)
        {
            var conn = facade.GetDbConnection();
            connection = conn;
            conn.Open();
            var cmd = conn.CreateCommand();
            if (facade.IsSqlServer())
            {
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
            }
            return cmd;
        }

        public static DataTable SqlQuery(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            var reader = command.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }

        public static List<T> SqlQuery<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : class, new()
        {
            var dt = SqlQuery(facade, sql, parameters);
            return dt.ToList<T>();
        }

        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            var propertyInfos = typeof(T).GetProperties();
            var list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                var t = new T();
                foreach (PropertyInfo p in propertyInfos)
                {
                    if (dt.Columns.IndexOf(p.Name) != -1 && row[p.Name] != DBNull.Value)
                        p.SetValue(t, row[p.Name], null);
                }
                list.Add(t);
            }
            return list;
        }
        public static IEnumerable<T> ToEnumerable<T>(this DataTable dt)
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            T[] ts = new T[dt.Rows.Count];
            int i = 0;
            bool isIntac = typeof(T).IsValueType;
            foreach (DataRow row in dt.Rows)
            {
                T t;
                if (isIntac)
                {
                    t = default(T);
                    t = (T)Convert.ChangeType(row[0], typeof(T));
                }
                else
                {
                    t = (T)Activator.CreateInstance(typeof(T));
                    foreach (PropertyInfo p in propertyInfos)
                    {
                        if (dt.Columns.IndexOf(p.Name) != -1 && row[p.Name] != DBNull.Value)
                            p.SetValue(t, row[p.Name], null);
                    }
                }
                ts[i] = t;
                i++;
            }
            return ts;
        }
    }

}
