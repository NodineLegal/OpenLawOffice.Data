// -----------------------------------------------------------------------
// <copyright file="Opportunity.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Opportunities
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using AutoMapper;
    using Dapper;
    using System.Linq;

    public class Opportunity
    {
        public static Common.Models.Opportunities.Opportunity Get(
            long id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Opportunities.Opportunity, DBOs.Opportunities.Opportunity>(
                "SELECT * FROM \"opportunity\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Opportunities.Opportunity Get(
            Transaction t,
            long id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Opportunities.Opportunity Create(
            Common.Models.Opportunities.Opportunity model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Opportunities.Opportunity dbo = Mapper.Map<DBOs.Opportunities.Opportunity>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"opportunity\" (\"account_id\", \"stage_id\", \"probability\", \"amount\", \"closed\", \"lead_id\", \"matter_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@AccountId, @StageId, @Probability, @Amount, @Closed, @LeadId, @MatterId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Opportunities.Opportunity>("SELECT currval(pg_get_serial_sequence('opportunity', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Opportunities.Opportunity Create(
            Transaction t,
            Common.Models.Opportunities.Opportunity model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Opportunities.Opportunity Edit(
            Common.Models.Opportunities.Opportunity model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Opportunities.Opportunity dbo = Mapper.Map<DBOs.Opportunities.Opportunity>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"opportunity\" SET " +
                "\"account_id\"=@AccountId, \"stage_id\"=@StageId, \"probability\"=@Probability, \"amount\"=@Amount, \"closed\"=@Closed, \"lead_id\"=@LeadId, \"matter_id\"=@MatterId, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Opportunities.Opportunity Edit(
            Transaction t,
            Common.Models.Opportunities.Opportunity model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static List<Common.Models.Opportunities.OpportunityBySource> OpportunitiesBySource(
            DateTime NewerThanDate,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Opportunities.OpportunityBySource, DBOs.Opportunities.OpportunityBySource>(
                "SELECT \"source_id\", (SELECT \"title\" FROM \"lead_source\" WHERE \"lead_source\".\"id\" = \"source_id\") as \"title\", COUNT(*) AS \"count\" FROM \"lead\" JOIN \"opportunity\" ON \"lead\".\"id\"=\"opportunity\".\"lead_id\" WHERE \"opportunity\".\"utc_created\" > @Date GROUP BY \"source_id\" ORDER BY \"count\" DESC",
                new { Date = NewerThanDate }, conn, closeConnection);
        }

        public static List<Common.Models.Opportunities.OpportunityMonthCount> ListSinceGroupingByMonth(
            DateTime NewerThanDate,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Opportunities.OpportunityMonthCount, DBOs.Opportunities.OpportunityMonthCount>(
                "SELECT date_trunc('month', utc_created) as \"mon\", COUNT(*) as \"count\" FROM \"opportunity\" WHERE utc_created > @Date GROUP BY \"mon\" ORDER BY \"mon\" ASC",
                new { Date = NewerThanDate }, conn, closeConnection);
        }

        public static List<Common.Models.Opportunities.OpportunityMonthCount> ListMatterConversionsGroupingByMonth(
            DateTime NewerThanDate,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Opportunities.OpportunityMonthCount, DBOs.Opportunities.OpportunityMonthCount>(
                "SELECT date_trunc('month', utc_created) as \"mon\", COUNT(*) as \"count\" FROM \"opportunity\" WHERE \"matter_id\" IS NOT NULL AND utc_created > @Date GROUP BY \"mon\" ORDER BY \"mon\" ASC",
                new { Date = NewerThanDate }, conn, closeConnection);
        }

        public static Common.Models.Opportunities.Opportunity GetForLead(
            long leadId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Opportunities.Opportunity, DBOs.Opportunities.Opportunity>(
                "SELECT * FROM \"opportunity\" WHERE \"lead_id\"=@id AND \"utc_disabled\" is null",
                new { id = leadId }, conn, closeConnection);
        }

        public static Common.Models.Opportunities.Opportunity GetForLead(
            Transaction t,
            long leadId)
        {
            return GetForLead(leadId, t.Connection, false);
        }

        public static Common.Models.Opportunities.Opportunity GetForMatter(
            Guid matterId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Opportunities.Opportunity, DBOs.Opportunities.Opportunity>(
                "SELECT * FROM \"opportunity\" WHERE \"matter_id\"=@matterId AND \"utc_disabled\" is null",
                new { matterId = matterId }, conn, closeConnection);
        }

        public static Common.Models.Opportunities.Opportunity GetForMatter(
            Transaction t,
            Guid matterId)
        {
            return GetForMatter(matterId, t.Connection, false);
        }

        public static List<Common.Models.Activities.ActivityBase> GetActivities(
            long opportunityId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Activities.ActivityBase, DBOs.Activities.ActivityBase>(
                "SELECT DISTINCT * FROM \"activity_base\" WHERE \"id\" IN " +
                "(SELECT \"activity\" FROM \"activity_regarding_opportunity\" WHERE \"opportunity\"=@id " +
                "UNION " +
                "SELECT \"activity\" FROM \"activity_regarding_lead\" WHERE \"lead\" IN (SELECT \"lead_id\" FROM \"opportunity\" WHERE \"id\"=@id)) " +
                "ORDER BY \"utc_created\" DESC ",
                new { id = opportunityId }, conn, closeConnection);
        }

        public static List<Common.Models.Activities.ActivityBase> GetActivities(
            Transaction t,
            long opportunityId)
        {
            return GetActivities(opportunityId, t.Connection, false);
        }

        public static List<Common.Models.Opportunities.Opportunity> List(
            string accountFilter,
            int? probabilityFilter,
            int? stageFilter,
            bool? closedFilter,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            decimal? pfD = null;
            string sql = "SELECT * FROM \"opportunity\" WHERE \"utc_disabled\" is null AND \"matter_id\" IS NULL ";

            if (accountFilter != null)
                accountFilter = accountFilter.ToLower();
            if (probabilityFilter.HasValue)
            {
                if (probabilityFilter.Value > 1)
                    pfD = (decimal)(probabilityFilter.Value) / 100;
                else
                    pfD = (decimal)(probabilityFilter.Value);
            }

            if (!string.IsNullOrEmpty(accountFilter))
            {
                sql += " AND \"account_id\" IN (SELECT \"id\" FROM \"contact\" WHERE LOWER(\"display_name\") LIKE '%' || @AccountFilter || '%')";
            }
            if (probabilityFilter.HasValue)
            {
                sql += " AND \"probability\" >= @ProbabilityFilter";
            }
            if (stageFilter.HasValue)
            {
                sql += " AND \"stage_id\"=@StageFilter";
            }
            if (closedFilter.HasValue)
            {
                if (closedFilter.Value)
                    sql += " AND \"closed\" IS NOT NULL ";
                else
                    sql += " AND \"closed\" IS NULL ";
            }

            return DataHelper.List<Common.Models.Opportunities.Opportunity, DBOs.Opportunities.Opportunity>(sql,
                new
                {
                    AccountFilter = accountFilter,
                    ProbabilityFilter = pfD,
                    StageFilter = stageFilter,
                    ClosedFilter = closedFilter
                }, conn, closeConnection);
        }

        public static List<Common.Models.Opportunities.Opportunity> List(
            Transaction t,
            string accountFilter,
            int? probabilityFilter,
            int? stageFilter,
            bool? closedFilter)
        {
            return List(accountFilter, probabilityFilter, stageFilter, closedFilter, t.Connection, false);
        }

        public static Common.Models.Opportunities.Opportunity Close(
            Common.Models.Opportunities.Opportunity model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Opportunities.Opportunity dbo = Mapper.Map<DBOs.Opportunities.Opportunity>(model);
            dbo.Closed = DateTime.UtcNow;

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"opportunity\" SET " +
                "\"closed\"=@Closed, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Opportunities.Opportunity Close(
            Transaction t,
            Common.Models.Opportunities.Opportunity model,
            Common.Models.Account.Users modifier)
        {
            return Close(model, modifier, t.Connection, false);
        }
    }
}
