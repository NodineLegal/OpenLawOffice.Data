// -----------------------------------------------------------------------
// <copyright file="Version.cs" company="Nodine Legal, LLC">
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

    public static class Version
    {
        public static Common.Models.Assets.Version Get(
            Guid id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Assets.Version, DBOs.Assets.Version>(
                "SELECT * FROM \"version\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Assets.Version Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static List<Common.Models.Assets.Version> ListForAsset(
            Guid assetId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Assets.Version, DBOs.Assets.Version>(
                "SELECT * FROM \"version\" WHERE \"utc_disabled\" is null AND " +
                "\"asset_id\"=@AssetId " +
                "ORDER BY \"sequence_number\" DESC",
                new { AssetId = assetId }, conn, closeConnection);
        }

        public static List<Common.Models.Assets.Version> ListForAsset(
            Transaction t,
            Guid assetId)
        {
            return ListForAsset(assetId, t.Connection, false);
        }

        public static Common.Models.Assets.Version Create(
            Common.Models.Assets.Version model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Assets.Version dbo = Mapper.Map<DBOs.Assets.Version>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("INSERT INTO \"version\" (\"id\", \"sequence_number\", " +
                "\"change_details\", \"asset_id\", " +
                "\"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @SequenceNumber, @ChangeDetails, @AssetId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Assets.Version Create(
            Transaction t,
            Common.Models.Assets.Version model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Assets.Version Edit(
            Common.Models.Assets.Version model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Assets.Version dbo = Mapper.Map<DBOs.Assets.Version>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"version\" SET " +
                "\"change_details\"=@ChangeDetails, " +
                "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Assets.Version Edit(
            Transaction t,
            Common.Models.Assets.Version model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }
    }
}
