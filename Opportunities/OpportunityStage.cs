// -----------------------------------------------------------------------
// <copyright file="OpportunityStage.cs" company="Nodine Legal, LLC">
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

    public class OpportunityStage
    {
        public static Common.Models.Opportunities.OpportunityStage Get(
            int id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Opportunities.OpportunityStage, DBOs.Opportunities.OpportunityStage>(
                "SELECT * FROM \"opportunity_stage\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Opportunities.OpportunityStage Get(
            Transaction t,
            int id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Opportunities.OpportunityStage Create(
            Common.Models.Opportunities.OpportunityStage model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Opportunities.OpportunityStage dbo = Mapper.Map<DBOs.Opportunities.OpportunityStage>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"opportunity_stage\" (\"order\", \"title\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Order, @Title, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Opportunities.OpportunityStage>("SELECT currval(pg_get_serial_sequence('opportunity_stage', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Opportunities.OpportunityStage Create(
            Transaction t,
            Common.Models.Opportunities.OpportunityStage model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Opportunities.OpportunityStage Edit(
            Common.Models.Opportunities.OpportunityStage model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Opportunities.OpportunityStage dbo = Mapper.Map<DBOs.Opportunities.OpportunityStage>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"opportunity_stage\" SET " +
                "\"order\"=@Order, \"title\"=@Title, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Opportunities.OpportunityStage Edit(
            Transaction t,
            Common.Models.Opportunities.OpportunityStage model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static List<Common.Models.Opportunities.OpportunityStage> List(
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Opportunities.OpportunityStage, DBOs.Opportunities.OpportunityStage>(
                "SELECT * FROM \"opportunity_stage\" WHERE \"utc_disabled\" is null ORDER BY \"order\" ASC",
                null, conn, closeConnection);
        }

        public static List<Common.Models.Opportunities.OpportunityStage> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static List<Common.Models.Opportunities.OpportunityStage> List(
            string title,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            title = title.ToLower();

            return DataHelper.List<Common.Models.Opportunities.OpportunityStage, DBOs.Opportunities.OpportunityStage>(
                "SELECT * FROM \"opportunity_stage\" WHERE \"utc_disabled\" is null AND " +
                "LOWER(\"title\") LIKE '%' || @Title || '%' ORDER BY \"order\" ASC",
                new { Title = title }, conn, closeConnection);
        }

        public static List<Common.Models.Opportunities.OpportunityStage> List(
            Transaction t,
            string title)
        {
            return List(title, t.Connection, false);
        }

        public static Common.Models.Opportunities.OpportunityStage Disable(
            Common.Models.Opportunities.OpportunityStage model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Opportunities.OpportunityStage,
                DBOs.Opportunities.OpportunityStage>("opportunity_stage", disabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Opportunities.OpportunityStage Disable(
            Transaction t,
            Common.Models.Opportunities.OpportunityStage model,
            Common.Models.Account.Users disabler)
        {
            return Disable(model, disabler, t.Connection, false);
        }
    }
}