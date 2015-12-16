// -----------------------------------------------------------------------
// <copyright file="TaskResponsibleUser.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class TaskResponsibleUser
    {
        public static Common.Models.Tasks.TaskResponsibleUser Get(
            Guid id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskResponsibleUser, DBOs.Tasks.TaskResponsibleUser>(
                "SELECT * FROM \"task_responsible_user\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Tasks.TaskResponsibleUser Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskResponsibleUser Get(
            long taskId, 
            Guid userId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskResponsibleUser, DBOs.Tasks.TaskResponsibleUser>(
                "SELECT * FROM \"task_responsible_user\" WHERE \"task_id\"=@TaskId AND \"user_pid\"=@UserPId AND \"utc_disabled\" is null",
                new { TaskId = taskId, UserPId = userId }, conn, closeConnection);
        }

        public static Common.Models.Tasks.TaskResponsibleUser Get(
            Transaction t,
            long taskId,
            Guid userId)
        {
            return Get(taskId, userId, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskResponsibleUser GetIgnoringDisable(
            long taskId, 
            Guid userId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskResponsibleUser, DBOs.Tasks.TaskResponsibleUser>(
                "SELECT * FROM \"task_responsible_user\" WHERE \"task_id\"=@TaskId AND \"user_pid\"=@UserPId",
                new { TaskId = taskId, UserPId = userId }, conn, closeConnection);
        }

        public static Common.Models.Tasks.TaskResponsibleUser GetIgnoringDisable(
            Transaction t,
            long taskId,
            Guid userId)
        {
            return GetIgnoringDisable(taskId, userId, t.Connection, false);
        }

        public static List<Common.Models.Tasks.TaskResponsibleUser> ListForTask(
            long taskId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            List<Common.Models.Tasks.TaskResponsibleUser> list =
                DataHelper.List<Common.Models.Tasks.TaskResponsibleUser, DBOs.Tasks.TaskResponsibleUser>(
                "SELECT * FROM \"task_responsible_user\" WHERE \"task_id\"=@TaskId AND \"utc_disabled\" is null",
                new { TaskId = taskId }, conn, closeConnection);

            list.ForEach(x =>
            {
                x.User = Account.Users.Get(x.User.PId.Value);
            });

            return list;
        }

        public static List<Common.Models.Tasks.TaskResponsibleUser> ListForTask(
            Transaction t,
            long taskId)
        {
            return ListForTask(taskId, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskResponsibleUser Create(
            Common.Models.Tasks.TaskResponsibleUser model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Tasks.TaskResponsibleUser dbo = Mapper.Map<DBOs.Tasks.TaskResponsibleUser>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"task_responsible_user\" (\"id\", \"task_id\", \"user_pid\", \"responsibility\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @TaskId, @UserPId, @Responsibility, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Tasks.TaskResponsibleUser>("SELECT currval(pg_get_serial_sequence('task_responsible_user', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskResponsibleUser Create(
            Transaction t,
            Common.Models.Tasks.TaskResponsibleUser model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskResponsibleUser Edit(
            Common.Models.Tasks.TaskResponsibleUser model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            Common.Models.Tasks.TaskResponsibleUser currentModel = Data.Tasks.TaskResponsibleUser.Get(model.Id.Value);
            model.Task = currentModel.Task;
            DBOs.Tasks.TaskResponsibleUser dbo = Mapper.Map<DBOs.Tasks.TaskResponsibleUser>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"task_responsible_user\" SET " +
                "\"task_id\"=@TaskId, \"user_pid\"=@UserPId, \"responsibility\"=@Responsibility, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskResponsibleUser Edit(
            Transaction t,
            Common.Models.Tasks.TaskResponsibleUser model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskResponsibleUser Disable(
            Common.Models.Tasks.TaskResponsibleUser model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Tasks.TaskResponsibleUser,
                DBOs.Tasks.TaskResponsibleUser>("task_responsible_user", disabler.PId.Value, 
                model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskResponsibleUser Disable(
            Transaction t,
            Common.Models.Tasks.TaskResponsibleUser model,
            Common.Models.Account.Users disabler)
        {
            return Disable(model, disabler, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskResponsibleUser Enable(
            Common.Models.Tasks.TaskResponsibleUser model,
            Common.Models.Account.Users enabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Tasks.TaskResponsibleUser,
                DBOs.Tasks.TaskResponsibleUser>("task_responsible_user", enabler.PId.Value, 
                model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskResponsibleUser Enable(
            Transaction t,
            Common.Models.Tasks.TaskResponsibleUser model,
            Common.Models.Account.Users enabler)
        {
            return Enable(model, enabler, t.Connection, false);
        }
    }
}