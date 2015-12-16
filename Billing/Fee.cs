// -----------------------------------------------------------------------
// <copyright file="Fee.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using AutoMapper;
    using Dapper;
    
    public static class Fee
    {
        public static Common.Models.Billing.Fee Get(
            Guid id,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                "SELECT * FROM \"fee\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Billing.Fee Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Matters.Matter GetMatter(
            Guid feeId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"id\" IN (SELECT \"matter_id\" FROM " +
                "\"fee_matter\" WHERE \"fee_id\"=@FeeId AND \"utc_disabled\" is null) " +
                "AND \"matter\".\"utc_disabled\" is null",
                new { FeeId = feeId }, conn, closeConnection);
        }

        public static Common.Models.Matters.Matter GetMatter(
            Transaction t,
            Guid feeId)
        {
            return GetMatter(feeId, t.Connection, false);
        }

        public static List<Common.Models.Billing.Fee> ListForMatter(
            Guid matterId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                "SELECT * FROM \"fee\" WHERE \"id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "\"utc_disabled\" is null ORDER BY \"incurred\" ASC",
                new { MatterId = matterId }, conn, closeConnection);
        }

        public static List<Common.Models.Billing.Fee> ListForMatter(
            Transaction t,
            Guid matterId)
        {
            return ListForMatter(matterId, t.Connection, false);
        }

        public static List<Common.Models.Billing.Fee> ListBilledFeesForMatter(
            Guid matterId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                "SELECT * FROM \"fee\" WHERE " +
                "   \"id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "   \"id\" IN (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"fee_id\"=\"fee\".\"id\") AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId }, conn, closeConnection);
        }

        public static List<Common.Models.Billing.Fee> ListBilledFeesForMatter(
            Transaction t,
            Guid matterId)
        {
            return ListBilledFeesForMatter(matterId, t.Connection, false);
        }

        public static List<Common.Models.Billing.Fee> ListUnbilledFeesForMatter(
            Guid matterId,
            DateTime? start = null, 
            DateTime? stop = null,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!start.HasValue && !stop.HasValue)
                return DataHelper.List<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                    "SELECT * FROM \"fee\" WHERE " +
                    "   \"id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                    "   \"id\" NOT IN (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"fee_id\"=\"fee\".\"id\") AND " +
                    "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                    new { MatterId = matterId }, conn, closeConnection);
            else if (start.HasValue && !stop.HasValue)
                return DataHelper.List<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                    "SELECT * FROM \"fee\" WHERE " +
                    "   \"id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                    "   \"id\" NOT IN (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"fee_id\"=\"fee\".\"id\") AND " +
                    "\"utc_disabled\" is null AND \"incurred\" >= @Start ORDER BY \"utc_created\" ASC",
                    new { MatterId = matterId, Start = start.Value.ToDbTime() }, conn, closeConnection);
            else if (!start.HasValue && stop.HasValue)
                return DataHelper.List<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                    "SELECT * FROM \"fee\" WHERE " +
                    "   \"id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                    "   \"id\" NOT IN (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"fee_id\"=\"fee\".\"id\") AND " +
                    "\"utc_disabled\" is null AND \"incurred\" <= @Stop ORDER BY \"utc_created\" ASC",
                    new
                    {
                        MatterId = matterId,
                        Stop = stop.Value.Date.AddDays(1).AddMilliseconds(-1).ToDbTime()
                    }, conn, closeConnection);
            else
                return DataHelper.List<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                    "SELECT * FROM \"fee\" WHERE " +
                    "   \"id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                    "   \"id\" NOT IN (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"fee_id\"=\"fee\".\"id\") AND " +
                    "\"utc_disabled\" is null AND \"incurred\" >= @Start AND \"incurred\" <= @Stop ORDER BY \"utc_created\" ASC",
                    new
                    {
                        MatterId = matterId,
                        Start = start.Value.ToDbTime(),
                        Stop = stop.Value.Date.AddDays(1).AddMilliseconds(-1).ToDbTime()
                    }, conn, closeConnection);
        }

        public static List<Common.Models.Billing.Fee> ListUnbilledFeesForMatter(
            Transaction t,
            Guid matterId,
            DateTime? start = null,
            DateTime? stop = null)
        {
            return ListUnbilledFeesForMatter(matterId, start, stop, t.Connection, false);
        }

        public static decimal SumUnbilledFeesForMatter(
            Guid matterId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                "SELECT SUM(\"amount\") AS \"amount\" FROM \"fee\" WHERE \"id\" IN " +
                "   (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId AND \"fee_id\" NOT IN " +
                "       (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"utc_disabled\" is NULL) " +
                "   )",
                new { MatterId = matterId }, conn, closeConnection).Amount;
        }

        public static decimal SumUnbilledFeesForMatter(
            Transaction t,
            Guid matterId)
        {
            return SumUnbilledFeesForMatter(matterId, t.Connection, false);
        }

        public static Common.Models.Billing.Fee Create(
            Common.Models.Billing.Fee model, 
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Billing.Fee dbo = Mapper.Map<DBOs.Billing.Fee>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("INSERT INTO \"fee\" (\"id\", \"incurred\", \"amount\", \"details\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @Incurred, @Amount, @Details, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Billing.Fee Create(
            Transaction t,
            Common.Models.Billing.Fee model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Billing.Fee Edit(
            Common.Models.Billing.Fee model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Billing.Fee dbo = Mapper.Map<DBOs.Billing.Fee>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"fee\" SET " +
                "\"incurred\"=@Incurred, \"amount\"=@Amount, \"details\"=@Details, " +
                "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Billing.Fee Edit(
            Transaction t,
            Common.Models.Billing.Fee model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static Common.Models.Billing.FeeMatter RelateMatter(
            Common.Models.Billing.Fee model,
            Guid matterId, 
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return FeeMatter.Create(new Common.Models.Billing.FeeMatter()
            {
                Id = Guid.NewGuid(),
                Fee = model,
                Matter = new Common.Models.Matters.Matter() { Id = matterId },
                CreatedBy = creator,
                ModifiedBy = creator,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            }, creator, conn, closeConnection);
        }

        public static Common.Models.Billing.FeeMatter RelateMatter(
            Transaction t,
            Common.Models.Billing.Fee model,
            Guid matterId, 
            Common.Models.Account.Users creator)
        {
            return RelateMatter(model, matterId, creator, t.Connection, false);
        }
    }
}
