// -----------------------------------------------------------------------
// <copyright file="Lead.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Leads
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using AutoMapper;
    using Dapper;
    using System.Linq;

    public class Lead
    {
        public static Common.Models.Leads.Lead Get(
            long id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Leads.Lead, DBOs.Leads.Lead>(
                "SELECT * FROM \"lead\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Leads.Lead Get(
            Transaction t,
            long id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Leads.Lead Create(
            Common.Models.Leads.Lead model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Leads.Lead dbo = Mapper.Map<DBOs.Leads.Lead>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"lead\" (\"status_id\", \"contact_id\", \"source_id\", \"fee_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@StatusId, @ContactId, @SourceId, @FeeId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Leads.Lead>("SELECT currval(pg_get_serial_sequence('lead', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Leads.Lead Create(
            Transaction t,
            Common.Models.Leads.Lead model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Leads.Lead Edit(
            Common.Models.Leads.Lead model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Leads.Lead dbo = Mapper.Map<DBOs.Leads.Lead>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"lead\" SET " +
                "\"status_id\"=@StatusId, \"contact_id\"=@ContactId, \"source_id\"=@SourceId, \"fee_id\"=@FeeId, \"closed\"=@Closed, \"details\"=@Details, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Leads.Lead Edit(
            Transaction t,
            Common.Models.Leads.Lead model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static List<Common.Models.Leads.LeadBySource> LeadsBySource(
            DateTime NewerThanDate,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Leads.LeadBySource, DBOs.Leads.LeadBySource>(
                "SELECT \"source_id\", (SELECT \"title\" FROM \"lead_source\" WHERE \"lead_source\".\"id\" = \"source_id\") as \"title\", COUNT(*) AS \"count\" FROM \"lead\" WHERE \"utc_created\" > @Date GROUP BY \"source_id\" ORDER BY \"count\" DESC",
                new { Date = NewerThanDate }, conn, closeConnection);
        }

        public static List<Common.Models.Leads.Lead> List(
            string contactFilter,
            string sourceFilter,
            int? sourceTypeFilter,
            int? statusFilter,
            bool? closedFilter,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            string sql = "SELECT * FROM \"lead\" WHERE \"utc_disabled\" is null AND \"id\" NOT IN (SELECT \"lead_id\" FROM \"opportunity\") ";

            if (contactFilter != null)
                contactFilter = contactFilter.ToLower();
            if (sourceFilter != null)
                sourceFilter = sourceFilter.ToLower();
            
            if (!string.IsNullOrEmpty(contactFilter))
            {
                sql += " AND \"contact_id\" IN (SELECT \"id\" FROM \"contact\" WHERE LOWER(\"display_name\") LIKE '%' || @ContactFilter || '%')";
            }
            if (!string.IsNullOrEmpty(sourceFilter))
            {
                sql += " AND \"source_id\" IN (SELECT \"id\" FROM \"lead_source\" WHERE LOWER(\"title\") LIKE '%' || @SourceFilter || '%')";
            }
            if (sourceTypeFilter.HasValue)
            {
                sql += " AND \"source_id\" IN (SELECT \"id\" FROM \"lead_source\" WHERE \"type_id\"=@SourceTypeId)";
            }
            if (statusFilter.HasValue)
            {
                sql += " AND \"status_id\"=@StatusId ";
            }
            if (closedFilter.HasValue)
            {
                if (closedFilter.Value)
                    sql += " AND \"closed\" IS NOT NULL ";
                else
                    sql += " AND \"closed\" IS NULL ";
            }

            return DataHelper.List<Common.Models.Leads.Lead, DBOs.Leads.Lead>(sql,
                new
                {
                    ContactFilter = contactFilter,
                    SourceFilter = sourceFilter,
                    SourceTypeId = sourceTypeFilter,
                    StatusId = statusFilter,
                    ClosedFilter = closedFilter
                }, conn, closeConnection);
        }

        public static List<Common.Models.Leads.Lead> List(
            Transaction t,
            string contactFilter,
            string sourceFilter,
            int? sourceTypeFilter,
            int? statusFilter,
            bool? closedFilter)
        {
            return List(contactFilter, sourceFilter, sourceTypeFilter, statusFilter, closedFilter, t.Connection, false);
        }

        public static Common.Models.Leads.Lead Close(
            Common.Models.Leads.Lead model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Leads.Lead dbo = Mapper.Map<DBOs.Leads.Lead>(model);
            dbo.Closed = DateTime.UtcNow;

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"lead\" SET " +
                "\"closed\"=@Closed, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Leads.Lead Close(
            Transaction t,
            Common.Models.Leads.Lead model,
            Common.Models.Account.Users modifier)
        {
            return Close(model, modifier, t.Connection, false);
        }

        public static List<Common.Models.Activities.ActivityBase> GetActivities(
            long leadId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Activities.ActivityBase, DBOs.Activities.ActivityBase>(
                "SELECT DISTINCT * FROM \"activity_base\" WHERE \"id\" IN " +
                "(SELECT \"activity\" FROM \"activity_regarding_lead\" WHERE \"lead\"=@id) " +
                "ORDER BY \"utc_created\" DESC ",
                new { id = leadId }, conn, closeConnection);
        }

        public static List<Common.Models.Activities.ActivityBase> GetActivities(
            Transaction t,
            long leadId)
        {
            return GetActivities(leadId, t.Connection, false);
        }
    }
}
