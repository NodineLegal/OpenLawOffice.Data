// -----------------------------------------------------------------------
// <copyright file="Tag.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Assets
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
    public static class Tag
    {
        public static Common.Models.Assets.Tag Get(
            Guid id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Assets.Tag, DBOs.Assets.Tag>(
                "SELECT * FROM \"asset_tag\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, false);
        }

        public static Common.Models.Assets.Tag Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Assets.Tag Get(
            string name,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Assets.Tag, DBOs.Assets.Tag>(
                "SELECT * FROM \"asset_tag\" WHERE LOWER(\"name\")=LOWER(@Name) AND \"utc_disabled\" is null",
                new { Name = name }, conn, false);
        }

        public static Common.Models.Assets.Tag Get(
            Transaction t,
            string name)
        {
            return Get(name, t.Connection, false);
        }

        public static List<Common.Models.Assets.Tag> List(
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Assets.Tag, DBOs.Assets.Tag>(
                "SELECT * FROM \"asset_tag\" WHERE \"utc_disabled\" is null", null, conn, closeConnection);
        }

        public static List<Common.Models.Assets.Tag> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static List<Common.Models.Assets.Tag> List(
            string name,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Assets.Tag, DBOs.Assets.Tag>(
                "SELECT * FROM \"asset_tag\" WHERE \"utc_disabled\" is null AND " +
                "LOWER(\"name\") LIKE '%' || LOWER(@Name) || '%' ORDER BY \"name\" ASC", 
                new { Name = name }, conn, closeConnection);
        }

        public static List<Common.Models.Assets.Tag> List(
            Transaction t,
            string name)
        {
            return List(name, t.Connection, false);
        }

        public static List<Common.Models.Assets.Tag> ListOrderedByFrequency(
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Assets.Tag, DBOs.Assets.Tag>(
                "SELECT COUNT(\"asset_id\") as \"cnt\", \"asset_tag\".\"id\", \"asset_tag\".\"name\" FROM " +
                "\"asset_asset_tag\" LEFT JOIN \"asset_tag\" ON \"asset_tag\".\"id\"=\"asset_asset_tag\".\"asset_tag_id\" " +
                "WHERE \"asset_tag\".\"utc_disabled\" is null " +
                "GROUP BY \"asset_tag\".\"id\", \"asset_tag\".\"name\", \"asset_tag_id\" " +
                "ORDER BY \"cnt\" DESC, \"asset_tag\".\"id\" ASC", null, conn, closeConnection);
        }

        public static List<Common.Models.Assets.Tag> ListOrderedByFrequency(
            Transaction t)
        {
            return ListOrderedByFrequency(t.Connection, false);
        }

        public static List<Common.Models.Assets.Tag> ListForAsset(
            Guid assetId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Assets.Tag, DBOs.Assets.Tag>(
                "SELECT * FROM \"asset_tag\" WHERE \"id\" IN (SELECT \"asset_tag_id\" FROM \"asset_asset_tag\" WHERE \"asset_id\"=@AssetId AND \"utc_disabled\" is null)",
                new { AssetId = assetId }, conn, closeConnection);
        }

        public static List<Common.Models.Assets.Tag> ListForAsset(
            Transaction t,
            Guid matterId)
        {
            return ListForAsset(matterId, t.Connection, false);
        }

        public static Common.Models.Assets.Tag Create(
            Common.Models.Assets.Tag model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Assets.Tag dbo = Mapper.Map<DBOs.Assets.Tag>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            Common.Models.Assets.Tag existing = Get(model.Name, conn, false);

            if (existing == null)
            {
                if (conn.Execute("INSERT INTO \"asset_tag\" (\"name\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Name, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo) > 0)
                    model.Id = conn.Query<DBOs.Assets.Tag>("SELECT currval(pg_get_serial_sequence('asset_tag', 'id')) AS \"id\"").Single().Id;
            }
            else
                model = existing;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Assets.Tag Create(
            Transaction t,
            Common.Models.Assets.Tag model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Assets.Tag Edit(
            Common.Models.Assets.Tag model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Assets.Tag dbo = Mapper.Map<DBOs.Assets.Tag>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"asset_tag\" SET " +
                "\"name\"=@Name, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Assets.Tag Edit(
            Transaction t,
            Common.Models.Assets.Tag model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }
    }
}
