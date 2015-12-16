// -----------------------------------------------------------------------
// <copyright file="ExternalSession.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.External
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    public static class ExternalSession
    {
        public static Common.Models.External.ExternalSession Get(
            Guid token,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.External.ExternalSession, DBOs.External.ExternalSession>(
                "SELECT * FROM \"external_session\" WHERE \"id\"=@Id",
                new { Id = token }, conn, closeConnection);
        }

        public static Common.Models.External.ExternalSession Get(
            Transaction t,
            Guid token)
        {
            return Get(token, t.Connection, false);
        }

        public static Common.Models.External.ExternalSession Get(
            string appName, 
            Guid machineId, 
            string username,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.External.ExternalSession, DBOs.External.ExternalSession>(
                "SELECT * FROM \"external_session\" WHERE \"app_name\"=@AppName AND \"machine_id\"=@MachineId " +
                "AND \"user_pid\" IN (SELECT \"pId\" FROM \"Users\" WHERE \"Username\"=@Username)",
                new { AppName = appName, MachineId = machineId, Username = username }, conn, closeConnection);
        }

        public static Common.Models.External.ExternalSession Get(
            Transaction t,
            string appName,
            Guid machineId,
            string username)
        {
            return Get(appName, machineId, username, t.Connection, false);
        }

        public static Common.Models.External.ExternalSession Create(
            Common.Models.External.ExternalSession model,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.Id = Guid.NewGuid();
            model.Created = DateTime.UtcNow;
            model.Timeout = 15 * 60; // 15 minutes
            model.Expires = model.Created.AddSeconds(model.Timeout);

            DBOs.External.ExternalSession dbo = Mapper.Map<DBOs.External.ExternalSession>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"external_session\" (\"id\", \"user_pid\", \"app_name\", \"utc_created\", \"utc_expires\", \"timeout\", \"machine_id\") " +
                "VALUES (@Id, @UserPId, @AppName, @UtcCreated, @UtcExpires, @Timeout, @MachineId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.External.ExternalSession>("SELECT currval(pg_get_serial_sequence('external_session', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.External.ExternalSession Create(
            Transaction t,
            Common.Models.External.ExternalSession model)
        {
            return Create(model, t.Connection, false);
        }

        public static Common.Models.External.ExternalSession Update(
            Common.Models.External.ExternalSession model,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            Delete(model, conn, false);
            return Create(model, conn, closeConnection);
        }

        public static Common.Models.External.ExternalSession Update(
            Transaction t,
            Common.Models.External.ExternalSession model)
        {
            return Update(model, t.Connection, false);
        }

        public static Common.Models.External.ExternalSession Renew(
            Common.Models.External.ExternalSession model,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            Common.Models.External.ExternalSession curSes = Get(model.Id.Value);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"external_session\" SET " +
                "\"utc_expires\"=@UtcExpires WHERE \"id\"=@Id",
                new { Id = model.Id.Value, UtcExpires = DateTime.UtcNow.AddSeconds(curSes.Timeout) });

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.External.ExternalSession Renew(
            Transaction t,
            Common.Models.External.ExternalSession model)
        {
            return Renew(model, t.Connection, false);
        }

        public static Common.Models.External.ExternalSession Delete(
            Common.Models.External.ExternalSession model,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("DELETE FROM \"external_session\" WHERE \"id\"=@Id",
                new { Id = model.Id.Value });

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.External.ExternalSession Delete(
            Transaction t,
            Common.Models.External.ExternalSession model)
        {
            return Delete(model, t.Connection, false);
        }
    }
}
