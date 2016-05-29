// -----------------------------------------------------------------------
// <copyright file="AssetTag.cs" company="Nodine Legal, LLC">
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

    public static class AssetTag
    {
        public static Common.Models.Assets.AssetTag Get(
            Guid id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Assets.AssetTag, DBOs.Assets.AssetTag>(
                "SELECT * FROM \"asset_asset_tag\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, false);
        }

        public static Common.Models.Assets.AssetTag Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Assets.AssetTag Get(
            Guid assetId,
            int tagId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Assets.AssetTag, DBOs.Assets.AssetTag>(
                "SELECT * FROM \"asset_asset_tag\" WHERE \"asset_id\"=@AssetId AND \"asset_tag_id\"=@TagId AND \"utc_disabled\" is null",
                new { AssetId = assetId, TagId = tagId }, conn, false);
        }

        public static Common.Models.Assets.AssetTag Get(
            Transaction t,
            Guid assetId,
            int tagId)
        {
            return Get(assetId, tagId, t.Connection, false);
        }

        public static List<Common.Models.Assets.AssetTag> List(
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Assets.AssetTag, DBOs.Assets.AssetTag>(
                "SELECT * FROM \"asset_asset_tag\" WHERE \"utc_disabled\" is null", null, conn, closeConnection);
        }

        public static List<Common.Models.Assets.AssetTag> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static List<Common.Models.Assets.AssetTag> ListForAsset(
            Guid assetId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Assets.AssetTag, DBOs.Assets.AssetTag>(
                "SELECT * FROM \"asset_asset_tag\" WHERE \"asset_id\"=@AssetId AND \"utc_disabled\" is null",
                new { AssetId = assetId }, conn, closeConnection);
        }

        public static List<Common.Models.Assets.AssetTag> ListForAsset(
            Transaction t,
            Guid assetId)
        {
            return ListForAsset(assetId, t.Connection, false);
        }

        public static Common.Models.Assets.AssetTag Create(
            Common.Models.Assets.AssetTag model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;

            conn = DataHelper.OpenIfNeeded(conn);

            Common.Models.Assets.AssetTag existing = null;
            if (!model.Tag.Id.HasValue)
            {
                Common.Models.Assets.Tag tempTag = Tag.Get(model.Tag.Name.Trim());

                if (tempTag != null)
                {
                    model.Tag = tempTag;
                    existing = Get(model.Asset.Id.Value, model.Tag.Id.Value);
                }
            }
            else
            {
                existing = Get(model.Asset.Id.Value, model.Tag.Id.Value);
            }

            DBOs.Assets.AssetTag dbo = Mapper.Map<DBOs.Assets.AssetTag>(model);

            //if (existing == null)
            //{
                conn.Execute("INSERT INTO \"asset_asset_tag\" (\"id\", \"asset_id\", \"asset_tag_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @AssetId, @TagId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            //}
            //else
            //{
            //    model = existing;
            //}

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Assets.AssetTag Create(
            Transaction t,
            Common.Models.Assets.AssetTag model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Assets.AssetTag Edit(
            Common.Models.Assets.AssetTag model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Assets.AssetTag dbo = Mapper.Map<DBOs.Assets.AssetTag>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"asset_asset_tag\" SET " +
                "\"asset_id\"=@AssetId, \"asset_tag_id\"=@TagId, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Assets.AssetTag Edit(
            Transaction t,
            Common.Models.Assets.AssetTag model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static Common.Models.Assets.AssetTag Disable(
            Common.Models.Assets.AssetTag model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Assets.AssetTag,
                DBOs.Assets.AssetTag>("asset_asset_tag", disabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Assets.AssetTag Disable(
            Transaction t,
            Common.Models.Assets.AssetTag model,
            Common.Models.Account.Users disabler)
        {
            return Disable(model, disabler, t.Connection, false);
        }

        public static Common.Models.Assets.AssetTag Enable(
            Common.Models.Assets.AssetTag model,
            Common.Models.Account.Users enabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Assets.AssetTag,
                DBOs.Assets.AssetTag>("asset_asset_tag", enabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Assets.AssetTag Enable(
            Transaction t,
            Common.Models.Assets.AssetTag model,
            Common.Models.Account.Users enabler)
        {
            return Enable(model, enabler, t.Connection, false);
        }

        public static void DeleteAllForAsset(
            Guid assetId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("DELETE FROM \"asset_asset_tag\" WHERE " +
                "\"asset_id\"=@AssetId", new { AssetId = assetId });

            DataHelper.Close(conn, closeConnection);
        }

        public static void DeleteAllForAsset(
            Transaction t,
            Guid assetId)
        {
            DeleteAllForAsset(assetId, t.Connection, false);
        }
    }
}
