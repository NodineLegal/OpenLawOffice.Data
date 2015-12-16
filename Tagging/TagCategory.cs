// -----------------------------------------------------------------------
// <copyright file="TagCategory.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Tagging
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
    public static class TagCategory
    {
        public static Common.Models.Tagging.TagCategory Get(
            int id,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tagging.TagCategory, DBOs.Tagging.TagCategory>(
                "SELECT * FROM \"tag_category\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Tagging.TagCategory Get(
            Transaction t,
            int id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Tagging.TagCategory Get(
            string name,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tagging.TagCategory, DBOs.Tagging.TagCategory>(
                "SELECT * FROM \"tag_category\" WHERE \"name\"=@name AND \"utc_disabled\" is null",
                new { name = name }, conn, closeConnection);
        }

        public static Common.Models.Tagging.TagCategory Get(
            Transaction t,
            string name)
        {
            return Get(name, t.Connection, false);
        }

        public static List<Common.Models.Tagging.TagCategory> List(
            string name,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Tagging.TagCategory, DBOs.Tagging.TagCategory>(
                "SELECT * FROM \"tag_category\" WHERE LOWER(\"name\") LIKE '%' || @Query || '%' AND \"utc_disabled\" is null",
                new { Query = name.ToLower() }, conn, closeConnection);
        }

        public static List<Common.Models.Tagging.TagCategory> List(
            Transaction t,
            string name)
        {
            return List(name, t.Connection, false);
        }

        public static Common.Models.Tagging.TagCategory Create(
            Common.Models.Tagging.TagCategory model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Tagging.TagCategory dbo = Mapper.Map<DBOs.Tagging.TagCategory>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"tag_category\" (\"name\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Name, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Tagging.TagCategory>("SELECT currval(pg_get_serial_sequence('tag_category', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Tagging.TagCategory Create(
            Transaction t,
            Common.Models.Tagging.TagCategory model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Tagging.TagCategory Disable(
            Common.Models.Tagging.TagCategory model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Tagging.TagCategory,
                DBOs.Tagging.TagCategory>("tag_category", disabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Tagging.TagCategory Disable(
            Transaction t,
            Common.Models.Tagging.TagCategory model,
            Common.Models.Account.Users disabler)
        {
            return Disable(model, disabler, t.Connection, false);
        }

        public static Common.Models.Tagging.TagCategory Enable(
            Common.Models.Tagging.TagCategory model,
            Common.Models.Account.Users enabler,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Tagging.TagCategory,
                DBOs.Tagging.TagCategory>("tag_category", enabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Tagging.TagCategory Enable(
            Transaction t,
            Common.Models.Tagging.TagCategory model,
            Common.Models.Account.Users enabler)
        {
            return Enable(model, enabler, t.Connection, false);
        }
    }
}