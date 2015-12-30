// -----------------------------------------------------------------------
// <copyright file="Matter.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Matters
{
    using System;
    using AutoMapper;
    using System.Data;
    using Dapper;
    using System.Collections.Generic;
    using System.Linq;
    
    public static class CourtSittingInCity
    {
        public static Common.Models.Matters.CourtSittingInCity Get(
            int id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Matters.CourtSittingInCity, DBOs.Matters.CourtSittingInCity>(
                "SELECT * FROM \"court_sitting_in_city\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Matters.CourtSittingInCity Get(
            Transaction t,
            int id)
        {
            return Get(id, t.Connection, false);
        }

        public static List<Common.Models.Matters.CourtSittingInCity> List(
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Matters.CourtSittingInCity, DBOs.Matters.CourtSittingInCity>(
                "SELECT * FROM \"court_sitting_in_city\" WHERE \"utc_disabled\" is null", null, conn, closeConnection);
        }

        public static List<Common.Models.Matters.CourtSittingInCity> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static Common.Models.Matters.CourtSittingInCity Create(
            Common.Models.Matters.CourtSittingInCity model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Matters.CourtSittingInCity dbo = Mapper.Map<DBOs.Matters.CourtSittingInCity>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"court_sitting_in_city\" (\"title\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Title, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Matters.CourtSittingInCity>("SELECT currval(pg_get_serial_sequence('court_sitting_in_city', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.CourtSittingInCity Create(
            Transaction t,
            Common.Models.Matters.CourtSittingInCity model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Matters.CourtSittingInCity Edit(
            Common.Models.Matters.CourtSittingInCity model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Matters.CourtSittingInCity dbo = Mapper.Map<DBOs.Matters.CourtSittingInCity>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"court_sitting_in_city\" SET " +
                "\"title\"=@Title, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.CourtSittingInCity Edit(
            Transaction t,
            Common.Models.Matters.CourtSittingInCity model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static Common.Models.Matters.CourtSittingInCity Disable(
            Common.Models.Matters.CourtSittingInCity model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Matters.CourtSittingInCity,
                DBOs.Matters.CourtSittingInCity>("court_sitting_in_city", disabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.CourtSittingInCity Disable(
            Transaction t,
            Common.Models.Matters.CourtSittingInCity model,
            Common.Models.Account.Users disabler)
        {
            return Disable(model, disabler, t.Connection, false);
        }
    }
}
