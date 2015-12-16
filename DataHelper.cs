// -----------------------------------------------------------------------
// <copyright file="DataHelper.cs" company="Nodine Legal, LLC">
// Licensed to Nodine Legal, LLC under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  Nodine Legal, LLC licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenLawOffice.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal static class DataHelper
    {
        internal static TReturn Get<TReturn, TDbo>(string sql, object anon = null,
            IDbConnection conn = null, bool closeConnection = true)
            where TReturn : class
            where TDbo : class
        {
            TDbo dbo = null;

            conn = OpenIfNeeded(conn);

            dbo = conn.Query<TDbo>(sql, anon).SingleOrDefault();

            Close(conn, closeConnection);

            if (dbo == null) return null;

            return Mapper.Map<TReturn>(dbo);
        }

        internal static TReturn Get<TReturn, TDbo>(string sql, object anon,
            Transaction t)
            where TReturn : class
            where TDbo : class
        {
            TDbo dbo = null;

            dbo = t.Connection.Query<TDbo>(sql, anon).SingleOrDefault();
            
            if (dbo == null) return null;

            return Mapper.Map<TReturn>(dbo);
        }

        internal static List<TReturn> List<TReturn, TDbo>(string sql, object anon = null,
            IDbConnection conn = null, bool closeConnection = true)
            where TReturn : class
            where TDbo : class
        {
            List<TReturn> ret = new List<TReturn>();
            List<TDbo> dbo = null;

            conn = OpenIfNeeded(conn);

            IEnumerable<TDbo> asd = conn.Query<TDbo>(sql, anon);
            dbo = asd.ToList<TDbo>();

            Close(conn, closeConnection);

            if (dbo == null) return null;

            dbo.ForEach(x =>
            {
                ret.Add(Mapper.Map<TReturn>(x));
            });

            return ret;
        }

        internal static List<TReturn> List<TReturn, TDbo>(string sql, object anon,
            Transaction t)
            where TReturn : class
            where TDbo : class
        {
            List<TReturn> ret = new List<TReturn>();
            List<TDbo> dbo = null;

            IEnumerable<TDbo> asd = t.Connection.Query<TDbo>(sql, anon);
            dbo = asd.ToList<TDbo>();

            if (dbo == null) return null;

            dbo.ForEach(x =>
            {
                ret.Add(Mapper.Map<TReturn>(x));
            });

            return ret;
        }

        internal static void Disable<TReturn, TDbo>(string tableName, Guid userPId, object id,
            IDbConnection conn = null, bool closeConnection = true)
            where TReturn : class
            where TDbo : class
        {
            DateTime Now = DateTime.UtcNow;

            conn = OpenIfNeeded(conn);

            if (id.GetType() == typeof(Guid))
            {
                conn.Execute("UPDATE \"" + tableName + "\" SET " +
                    "\"utc_disabled\"=@Now, \"disabled_by_user_pid\"=@UserPId " +
                    "WHERE \"id\"=@Id",
                    new { Now = Now, UserPId = userPId, Id = (Guid)id });
            }
            else if (id.GetType() == typeof(int))
            {
                conn.Execute("UPDATE \"" + tableName + "\" SET " +
                    "\"utc_disabled\"=@Now, \"disabled_by_user_pid\"=@UserPId " +
                    "WHERE \"id\"=@Id",
                    new { Now = Now, UserPId = userPId, Id = (int)id });
            }
            else
            {
                conn.Execute("UPDATE \"" + tableName + "\" SET " +
                    "\"utc_disabled\"=@Now, \"disabled_by_user_pid\"=@UserPId " +
                    "WHERE \"id\"=@Id",
                    new { Now = Now, UserPId = userPId, Id = id });
            }

            Close(conn, closeConnection);
        }

        internal static void Disable<TReturn, TDbo>(string tableName, Guid userPId, object id,
            Transaction t)
            where TReturn : class
            where TDbo : class
        {
            DateTime Now = DateTime.UtcNow;

            if (id.GetType() == typeof(Guid))
            {
                t.Connection.Execute("UPDATE \"" + tableName + "\" SET " +
                    "\"utc_disabled\"=@Now, \"disabled_by_user_pid\"=@UserPId " +
                    "WHERE \"id\"=@Id",
                    new { Now = Now, UserPId = userPId, Id = (Guid)id });
            }
            else if (id.GetType() == typeof(int))
            {
                t.Connection.Execute("UPDATE \"" + tableName + "\" SET " +
                    "\"utc_disabled\"=@Now, \"disabled_by_user_pid\"=@UserPId " +
                    "WHERE \"id\"=@Id",
                    new { Now = Now, UserPId = userPId, Id = (int)id });
            }
            else
            {
                t.Connection.Execute("UPDATE \"" + tableName + "\" SET " +
                    "\"utc_disabled\"=@Now, \"disabled_by_user_pid\"=@UserPId " +
                    "WHERE \"id\"=@Id",
                    new { Now = Now, UserPId = userPId, Id = id });
            }
        }

        internal static void Enable<TReturn, TDbo>(string tableName, Guid userPId, object id,
            IDbConnection conn = null, bool closeConnection = true)
            where TReturn : class
            where TDbo : class
        {
            DateTime Now = DateTime.UtcNow;

            conn = OpenIfNeeded(conn);

            if (id.GetType() == typeof(Guid))
            {
                conn.Execute("UPDATE \"" + tableName + "\" SET " +
                    "\"utc_modified\"=@Now, \"modified_by_user_pid\"=@UserPId, \"utc_disabled\"=null, \"disabled_by_user_pid\"=null " +
                    "WHERE \"id\"=@Id",
                    new { Now = Now, UserPId = userPId, Id = (Guid)id });
            }
            else
            {
                conn.Execute("UPDATE \"" + tableName + "\" SET " +
                    "\"utc_modified\"=@Now, \"modified_by_user_pid\"=@UserPId, \"utc_disabled\"=null, \"disabled_by_user_pid\"=null " +
                    "WHERE \"id\"=@Id",
                    new { Now = Now, UserPId = userPId, Id = id });
            }

            Close(conn, closeConnection);
        }

        internal static void Enable<TReturn, TDbo>(string tableName, Guid userPId, object id,
            Transaction t)
            where TReturn : class
            where TDbo : class
        {
            DateTime Now = DateTime.UtcNow;

            if (id.GetType() == typeof(Guid))
            {
                t.Connection.Execute("UPDATE \"" + tableName + "\" SET " +
                    "\"utc_modified\"=@Now, \"modified_by_user_pid\"=@UserPId, \"utc_disabled\"=null, \"disabled_by_user_pid\"=null " +
                    "WHERE \"id\"=@Id",
                    new { Now = Now, UserPId = userPId, Id = (Guid)id });
            }
            else
            {
                t.Connection.Execute("UPDATE \"" + tableName + "\" SET " +
                    "\"utc_modified\"=@Now, \"modified_by_user_pid\"=@UserPId, \"utc_disabled\"=null, \"disabled_by_user_pid\"=null " +
                    "WHERE \"id\"=@Id",
                    new { Now = Now, UserPId = userPId, Id = id });
            }
        }

        internal static IDbConnection OpenIfNeeded(IDbConnection conn = null)
        {
            if (conn == null)
            {
                conn = Database.Instance.GetConnection();
                conn.Open();
            }

            return conn;
        }

        internal static void Close(IDbConnection conn = null, bool closeConnection = false)
        {
            if (closeConnection && conn != null)
                conn.Close();
        }
    }
}