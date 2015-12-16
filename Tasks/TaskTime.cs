// -----------------------------------------------------------------------
// <copyright file="TaskTime.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Tasks
{
    using System;
    using System.Data;
    using AutoMapper;
    using Dapper;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class TaskTime
    {
        public static Common.Models.Tasks.TaskTime Get(
            Guid id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskTime, DBOs.Tasks.TaskTime>(
                "SELECT * FROM \"task_time\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Tasks.TaskTime Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskTime Get(
            long taskId, 
            Guid timeId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskTime, DBOs.Tasks.TaskTime>(
                "SELECT * FROM \"task_time\" WHERE \"task_id\"=@TaskId AND \"time_id\"=@TimeId AND \"utc_disabled\" is null",
                new { TaskId = taskId, TimeId = timeId }, conn, closeConnection);
        }

        public static Common.Models.Tasks.TaskTime Get(
            Transaction t,
            long taskId,
            Guid timeId)
        {
            return Get(taskId, timeId, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskTime Create(
            Common.Models.Tasks.TaskTime model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Tasks.TaskTime dbo = Mapper.Map<DBOs.Tasks.TaskTime>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"task_time\" (\"id\", \"task_id\", \"time_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @TaskId, @TimeId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Tasks.TaskResponsibleUser>("SELECT currval(pg_get_serial_sequence('task_time', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskTime Create(
            Transaction t,
            Common.Models.Tasks.TaskTime model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }
    }
}