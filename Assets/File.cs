// -----------------------------------------------------------------------
// <copyright file="File.cs" company="Nodine Legal, LLC">
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
    using AutoMapper;
    using Dapper;
    using System.Linq;

    public static class File
    {
        public static Common.Models.Assets.File Get(
            Guid id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Assets.File, DBOs.Assets.File>(
                "SELECT * FROM \"file\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Assets.File Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static List<Common.Models.Assets.File> ListForVersion(
            Guid versionId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Assets.File, DBOs.Assets.File>(
                "SELECT * FROM \"file\" WHERE \"utc_disabled\" is null AND " +
                "\"version_id\"=@VersionId " +
                "ORDER BY \"id\" ASC",
                new { VersionId = versionId }, conn, closeConnection);
        }

        public static List<Common.Models.Assets.File> ListForVersion(
            Transaction t,
            Guid versionId)
        {
            return ListForVersion(versionId, t.Connection, false);
        }

        public static Common.Models.Assets.File Create(
            Common.Models.Assets.File model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Assets.File dbo = Mapper.Map<DBOs.Assets.File>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("INSERT INTO \"file\" (\"id\", \"version_id\", \"is_source\", " +
                "\"content_length\", \"content_type\", \"extension\", " +
                "\"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @VersionId, @IsSource, @ContentLength, @ContentType, @Extension, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Assets.File Create(
            Transaction t,
            Common.Models.Assets.File model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Assets.File Edit(
            Common.Models.Assets.File model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Assets.File dbo = Mapper.Map<DBOs.Assets.File>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"file\" SET " +
                "\"is_source\"=@IsSource, " +
                "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Assets.File Edit(
            Transaction t,
            Common.Models.Assets.File model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }
    }
}
