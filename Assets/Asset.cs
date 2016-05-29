// -----------------------------------------------------------------------
// <copyright file="Asset.cs" company="Nodine Legal, LLC">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Asset
    {
        public static Common.Models.Assets.Asset Get(
            Guid id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Assets.Asset, DBOs.Assets.Asset>(
                "SELECT * FROM \"asset\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Assets.Asset Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Matters.Matter GetRelatedMatter(
            Guid assetId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT \"matter\".* FROM \"matter\" JOIN \"asset_matter\" ON \"matter\".\"id\"=\"asset_matter\".\"matter_id\" " +
                "WHERE \"asset_id\"=@AssetId AND \"matter\".\"utc_disabled\" is null AND \"asset_matter\".\"utc_disabled\" is null",
                new { AssetId = assetId }, conn, closeConnection);
        }

        public static List<Common.Models.Assets.Asset> ListForMatter(
            Guid matterId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Assets.Asset, DBOs.Assets.Asset>(
                "SELECT * FROM \"asset\" WHERE \"utc_disabled\" is null AND " +
                "\"id\" IN (SELECT \"asset_id\" FROM \"asset_matter\" WHERE \"matter_id\"=@MatterId) " +
                "ORDER BY \"date\" DESC",
                new { MatterId = matterId }, conn, closeConnection);
        }

        public static List<Common.Models.Assets.Asset> ListForMatter(
            Transaction t,
            Guid matterId)
        {
            return ListForMatter(matterId, t.Connection, false);
        }

        public static List<Common.Models.Assets.Asset> ListForMatter(
            Guid matterId,
            DateTime? dateFromFilter = null,
            DateTime? dateToFilter = null,
            string flagFilter = null,
            string tagFilter = null,
            string titleFilter = null,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            string query = "SELECT * FROM \"asset\" WHERE \"utc_disabled\" is null AND ";

            if (dateFromFilter != null && dateFromFilter.HasValue)
                query += "\"date\">=@DateFromFilter AND ";
            if (dateToFilter != null && dateToFilter.HasValue)
            {
                dateToFilter = dateToFilter.Value.Date.AddDays(1).AddMilliseconds(-1);
                query += "\"date\"<=@DateToFilter AND ";
            }
            if (!string.IsNullOrEmpty(flagFilter))
            {
                switch(flagFilter)
                {
                    case "Court Filed":
                        query += "\"is_court_filed\"=TRUE AND ";
                        break;
                    case "Attorney Work Product":
                        query += "\"is_attorney_work_product\"=TRUE AND ";
                        break;
                    case "Privileged":
                        query += "\"is_privileged\"=TRUE AND ";
                        break;
                    case "Discoverable":
                        query += "\"is_discoverable\"=TRUE AND ";
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrEmpty(tagFilter))
                query += "\"id\" IN (SELECT \"asset_id\" FROM \"asset_asset_tag\" WHERE \"asset_tag_id\" IN (SELECT \"id\" FROM \"asset_tag\" WHERE LOWER(\"name\") LIKE '%' || LOWER(@TagFilter) || '%')) AND ";
            if (!string.IsNullOrEmpty(titleFilter))
                query += "LOWER(\"title\") LIKE '%' || LOWER(@TitleFilter) || '%' AND ";

            query += "\"id\" IN (SELECT \"asset_id\" FROM \"asset_matter\" WHERE \"matter_id\"=@MatterId) " +
                "ORDER BY \"date\" DESC";

            return DataHelper.List<Common.Models.Assets.Asset, DBOs.Assets.Asset>(
                query,
                new { MatterId = matterId, DateFromFilter = dateFromFilter,
                    DateToFilter = dateToFilter, TagFilter = tagFilter,
                    TitleFilter = titleFilter }, conn, closeConnection);
        }

        public static List<Common.Models.Assets.Asset> ListForMatter(
            Transaction t,
            Guid matterId,
            DateTime? dateFromFilter = null,
            DateTime? dateToFilter = null,
            string flagFilter = null,
            string tagFilter = null,
            string titleFilter = null)
        {
            return ListForMatter(matterId, dateFromFilter, dateToFilter, flagFilter, tagFilter, titleFilter, t.Connection, false);
        }

        public static List<Common.Models.Assets.File> ListFilesForMostRecentVersion(
            Guid assetId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Assets.File, DBOs.Assets.File>(
                "SELECT * FROM \"file\" WHERE \"version_id\" IN " +
                "(SELECT \"id\" FROM \"version\" WHERE \"asset_id\"=@AssetId ORDER BY \"sequence_number\" DESC LIMIT 1)",
                new { AssetId = assetId }, conn, closeConnection);
        }

        public static List<Common.Models.Assets.File> ListFilesForMostRecentVersion(
            Transaction t,
            Guid assetId)
        {
            return ListFilesForMostRecentVersion(assetId, t.Connection, false);
        }

        public static Common.Models.Assets.Asset Create(
            Common.Models.Assets.Asset model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Assets.Asset dbo = Mapper.Map<DBOs.Assets.Asset>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"asset\" (\"id\", \"date\", \"title\", \"is_final\", \"is_court_filed\", " +
                "\"is_attorney_work_product\", \"is_privileged\", \"is_discoverable\", \"provided_in_discovery_at\", " +
                "\"checked_out_by_id\", \"checked_out_at\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @Date, @Title, @IsFinal, @IsCourtFiled, @IsAttorneyWorkProduct, @IsPrivileged, @IsDiscoverable, @ProvidedInDiscoveryAt, @CheckedOutById, @CheckedOutAt, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model = Get(model.Id.Value, conn, false);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Assets.Asset Create(
            Transaction t,
            Common.Models.Assets.Asset model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Assets.Asset Edit(
            Common.Models.Assets.Asset model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Assets.Asset dbo = Mapper.Map<DBOs.Assets.Asset>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"asset\" SET " +
                "\"date\"=@Date, \"title\"=@Title, \"is_final\"=@IsFinal, " +
                "\"is_court_filed\"=@IsCourtFiled, \"is_attorney_work_product\"=@IsAttorneyWorkProduct, \"is_privileged\"=@IsPrivileged, \"is_discoverable\"=@IsDiscoverable, " +
                "\"provided_in_discovery_at\"=@ProvidedInDiscoveryAt, \"checked_out_by_id\"=@CheckedOutById, \"checked_out_at\"=@CheckedOutAt, " +
                "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);
            
            return model;
        }

        public static Common.Models.Assets.Asset Edit(
            Transaction t,
            Common.Models.Assets.Asset model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static int GetNextVersionSequenceNumber(
            Guid assetId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            conn = DataHelper.OpenIfNeeded(conn);

            Common.Models.Assets.Version currentVersion = 
                DataHelper.Get<Common.Models.Assets.Version, DBOs.Assets.Version>(
                "SELECT * FROM \"version\" WHERE \"asset_id\"=@AssetId AND " +
                "\"utc_disabled\" is null ORDER BY \"sequence_number\" DESC LIMIT 1",
                new { AssetId = assetId }, conn, closeConnection);

            if (currentVersion == null)
                return 1;
            else
                return currentVersion.SequenceNumber.Value + 1;
        }

        public static int GetNextVersionSequenceNumber(
            Transaction t,
            Guid assetId)
        {
            return GetNextVersionSequenceNumber(assetId, t.Connection, false);
        }

        public static void Checkout(
            Guid assetId,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"asset\" SET " +
                "\"checked_out_by_id\"=@CheckedOutById, \"checked_out_at\"=@CheckedOutAt " +
                "WHERE \"id\"=@AssetId", 
                new
                {
                    AssetId = assetId,
                    CheckedOutById = modifier.PId,
                    CheckedOutAt = DateTime.Now.ToDbTime()
                });

            DataHelper.Close(conn, closeConnection);
        }

        public static void Checkout(
            Transaction t,
            Guid assetId,
            Common.Models.Account.Users modifier)
        {
            Checkout(assetId, modifier, t.Connection, false);
        }

        public static void Checkin(
            Guid assetId,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"asset\" SET " +
                "\"checked_out_by_id\"=null, \"checked_out_at\"=null " +
                "WHERE \"id\"=@AssetId", new { AssetId = assetId });

            DataHelper.Close(conn, closeConnection);
        }

        public static void Checkin(
            Transaction t,
            Guid assetId,
            Common.Models.Account.Users modifier)
        {
            Checkin(assetId, modifier, t.Connection, false);
        }
    }
}
