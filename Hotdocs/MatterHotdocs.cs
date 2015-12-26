// -----------------------------------------------------------------------
// <copyright file="TaskMatter.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Hotdocs
{
    using System;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    public static class MatterHotdocs
    {
        public static Common.Models.Hotdocs.MatterHotdocs Get(
            long id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Hotdocs.MatterHotdocs, DBOs.Hotdocs.MatterHotdocs>(
                "SELECT * FROM \"matter_hotdocs\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Hotdocs.MatterHotdocs Get(
            Transaction t,
            long id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Hotdocs.MatterHotdocs GetFor(
            Guid matterId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Hotdocs.MatterHotdocs, DBOs.Hotdocs.MatterHotdocs>(
                "SELECT * FROM \"matter_hotdocs\" WHERE \"matter_id\"=@MatterId AND \"utc_disabled\" is null",
                new { MatterId = matterId }, conn, closeConnection);
        }

        public static Common.Models.Hotdocs.MatterHotdocs GetFor(
            Transaction t,
            Guid matterId)
        {
            return GetFor(matterId, t.Connection, false);
        }

        public static Common.Models.Hotdocs.MatterHotdocs Create(
            Common.Models.Hotdocs.MatterHotdocs model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Tasks.TaskMatter dbo = Mapper.Map<DBOs.Tasks.TaskMatter>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            throw new NotImplementedException();
            //conn.Execute("INSERT INTO \"task_matter\" (\"id\", \"task_id\", \"matter_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
            //    "VALUES (@Id, @TaskId, @MatterId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
            //    dbo);

            return model;
        }

        public static Common.Models.Hotdocs.MatterHotdocs Create(
            Transaction t,
            Common.Models.Hotdocs.MatterHotdocs model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Hotdocs.MatterHotdocs Edit(
            Common.Models.Hotdocs.MatterHotdocs model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Notes.Note dbo = Mapper.Map<DBOs.Notes.Note>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"note\" SET " +
                "\"title\"=@Title, \"body\"=@Body, \"timestamp\"=@Timestamp, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Hotdocs.MatterHotdocs Edit(
            Transaction t,
            Common.Models.Hotdocs.MatterHotdocs model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }
    }
}
