// -----------------------------------------------------------------------
// <copyright file="UserTaskSettings.cs" company="Nodine Legal, LLC">
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
// ------------------------------------------
namespace OpenLawOffice.Data.Settings
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
    public class UserTaskSettings
    {
        public static Common.Models.Settings.TagFilter GetTagFilter(
            long id,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Settings.TagFilter, DBOs.Settings.TagFilter>(
                "SELECT * FROM \"tag_filter\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Settings.TagFilter GetTagFilter(
            Transaction t,
            long id)
        {
            return GetTagFilter(id, t.Connection, false);
        }

        public static List<Common.Models.Settings.TagFilter> ListTagFiltersFor(
            Common.Models.Account.Users user,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Settings.TagFilter, DBOs.Settings.TagFilter>(
                "SELECT * FROM \"tag_filter\" WHERE \"user_pid\"=@UserPId AND \"utc_disabled\" is null",
                new { UserPId = user.PId.Value }, conn, closeConnection);
        }

        public static List<Common.Models.Settings.TagFilter> ListTagFiltersFor(
            Transaction t,
            Common.Models.Account.Users user)
        {
            return ListTagFiltersFor(user, t.Connection, false);
        }

        public static Common.Models.Settings.TagFilter CreateTagFilter(
            Common.Models.Settings.TagFilter model, 
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Settings.TagFilter dbo = Mapper.Map<DBOs.Settings.TagFilter>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"tag_filter\" (\"user_pid\", \"category\", \"tag\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@UserPId, @Category, @Tag, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Settings.TagFilter>("SELECT currval(pg_get_serial_sequence('tag_filter', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Settings.TagFilter CreateTagFilter(
            Transaction t,
            Common.Models.Settings.TagFilter model, 
            Common.Models.Account.Users creator)
        {
            return CreateTagFilter(model, creator, t.Connection, false);
        }

        public static Common.Models.Settings.TagFilter EditTagFilter(
            Common.Models.Settings.TagFilter model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Settings.TagFilter dbo = Mapper.Map<DBOs.Settings.TagFilter>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"tag_filter\" SET " +
                "\"user_pid\"=@UserPId, \"category\"=@Category, \"tag\"=@Tag, " +
                "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Settings.TagFilter EditTagFilter(
            Transaction t,
            Common.Models.Settings.TagFilter model,
            Common.Models.Account.Users modifier)
        {
            return EditTagFilter(model, modifier, t.Connection, false);
        }

        public static void DeleteTagFilter(
            Common.Models.Settings.TagFilter model,
            Common.Models.Account.Users deleter,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            DBOs.Settings.TagFilter dbo = Mapper.Map<DBOs.Settings.TagFilter>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("DELETE FROM \"tag_filter\" WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);
        }

        public static void DeleteTagFilter(
            Transaction t,
            Common.Models.Settings.TagFilter model,
            Common.Models.Account.Users deleter)
        {
            DeleteTagFilter(model, deleter, t.Connection, false);
        }
    }
}