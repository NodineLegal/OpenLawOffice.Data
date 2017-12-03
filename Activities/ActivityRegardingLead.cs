// -----------------------------------------------------------------------
// <copyright file="ActivityRegardingLead.cs" company="Nodine Legal, LLC">
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

    public class ActivityRegardingLead
    {
        public static Common.Models.Activities.ActivityRegardingLead Get(
            long id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Activities.ActivityRegardingLead, DBOs.Activities.ActivityRegardingLead>(
                "SELECT * FROM \"activity_regarding_lead\" WHERE \"id\"=@id",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Activities.ActivityRegardingLead Get(
            Transaction t,
            long id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Activities.ActivityRegardingLead GetFromActivityId(
            long id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Activities.ActivityRegardingLead, DBOs.Activities.ActivityRegardingLead>(
                "SELECT * FROM \"activity_regarding_lead\" WHERE \"activity\"=@id",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Activities.ActivityRegardingLead GetFromActivityId(
            Transaction t,
            long id)
        {
            return GetFromActivityId(id, t.Connection, false);
        }

        public static Common.Models.Activities.ActivityRegardingLead Create(
            Common.Models.Activities.ActivityRegardingLead model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            DBOs.Activities.ActivityRegardingLead dbo = Mapper.Map<DBOs.Activities.ActivityRegardingLead>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"activity_regarding_lead\" (\"type\", \"activity\", \"lead\") " +
                "VALUES (@Type, @Activity, @Lead)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Activities.ActivityRegardingLead>("SELECT currval(pg_get_serial_sequence('activity_regarding_lead', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Activities.ActivityRegardingLead Create(
            Transaction t,
            Common.Models.Activities.ActivityRegardingLead model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Activities.ActivityRegardingLead Edit(
            Common.Models.Activities.ActivityRegardingLead model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            DBOs.Activities.ActivityRegardingLead dbo = Mapper.Map<DBOs.Activities.ActivityRegardingLead>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"activity_regarding_lead\" SET " +
                "\"type\"=@Type, \"activity\"=@Activity, \"lead\"=@Lead " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Activities.ActivityRegardingLead Edit(
            Transaction t,
            Common.Models.Activities.ActivityRegardingLead model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static List<Common.Models.Activities.ActivityRegardingLead> List(
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Activities.ActivityRegardingLead, DBOs.Activities.ActivityRegardingLead>(
                "SELECT * FROM \"activity_regarding_lead\" WHERE \"utc_disabled\" is null",
                null, conn, closeConnection);
        }

        public static List<Common.Models.Activities.ActivityRegardingLead> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        //public static List<Common.Models.Activities.ActivityRegardingLead> List(
        //    string subject,
        //    IDbConnection conn = null,
        //    bool closeConnection = true)
        //{
        //    subject = subject.ToLower();

        //    return DataHelper.List<Common.Models.Activities.ActivityRegardingLead, DBOs.Activities.ActivityRegardingLead>(
        //        "SELECT * FROM \"activity_regarding_lead\" WHERE \"utc_disabled\" is null AND " +
        //        "LOWER(\"subject\") LIKE '%' || @Subject || '%' ",
        //        new { Subject = subject }, conn, closeConnection);
        //}

        //public static List<Common.Models.Activities.ActivityRegardingLead> List(
        //    Transaction t,
        //    string title)
        //{
        //    return List(title, t.Connection, false);
        //}
    }
}