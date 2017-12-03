// -----------------------------------------------------------------------
// <copyright file="ActivityLetter.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using AutoMapper;
    using Dapper;
    using System.Linq;

    public class ActivityLetter
    {
        public static Common.Models.Activities.ActivityLetter Get(
            long id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Activities.ActivityLetter, DBOs.Activities.ActivityLetter>(
                "SELECT * FROM \"activity_letter\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Activities.ActivityLetter Get(
            Transaction t,
            long id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Activities.ActivityLetter Create(
            Common.Models.Activities.ActivityLetter model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Activities.ActivityLetter dbo = Mapper.Map<DBOs.Activities.ActivityLetter>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"activity_letter\" (\"type\", \"is_campaign_response\", \"subject\", \"body\", \"owner\", \"priority\", \"due\", \"state\", \"status_reason\", " +
                "\"sender\", \"recipient\", \"direction\", \"duration\", \"address\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Type, @IsCampaignResponse, @Subject, @Body, @Owner, @Priority, @Due, @State, @StatusReason, @Sender, @Recipient, @Direction, @Duration, @Address, " +
                "@UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Activities.ActivityLetter>("SELECT currval(pg_get_serial_sequence('activity_base', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Activities.ActivityLetter Create(
            Transaction t,
            Common.Models.Activities.ActivityLetter model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Activities.ActivityLetter Edit(
            Common.Models.Activities.ActivityLetter model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Activities.ActivityLetter dbo = Mapper.Map<DBOs.Activities.ActivityLetter>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"activity_letter\" SET " +
                "\"type\"=@Type, \"is_campaign_response\"=@IsCampaignResponse, \"subject\"=@Subject, \"body\"=@Body, \"owner\"=@Owner, \"priority\"=@Priority, \"due\"=@Due, " +
                "\"state\"=@State, \"status_reason\"=@StatusReason, \"sender\"=@Sender, \"recipient\"=@Recipient, \"direction\"=@Direction, \"address\"=@Address, " +
                "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Activities.ActivityLetter Edit(
            Transaction t,
            Common.Models.Activities.ActivityLetter model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static List<Common.Models.Activities.ActivityLetter> List(
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Activities.ActivityLetter, DBOs.Activities.ActivityLetter>(
                "SELECT * FROM \"activity_letter\" WHERE \"utc_disabled\" is null",
                null, conn, closeConnection);
        }

        public static List<Common.Models.Activities.ActivityLetter> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static List<Common.Models.Activities.ActivityLetter> List(
            string subject,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            subject = subject.ToLower();

            return DataHelper.List<Common.Models.Activities.ActivityLetter, DBOs.Activities.ActivityLetter>(
                "SELECT * FROM \"activity_letter\" WHERE \"utc_disabled\" is null AND " +
                "LOWER(\"subject\") LIKE '%' || @Subject || '%' ",
                new { Subject = subject }, conn, closeConnection);
        }

        public static List<Common.Models.Activities.ActivityLetter> List(
            Transaction t,
            string title)
        {
            return List(title, t.Connection, false);
        }
    }
}