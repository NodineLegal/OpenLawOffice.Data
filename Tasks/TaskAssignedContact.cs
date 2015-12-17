// -----------------------------------------------------------------------
// <copyright file="TaskAssignedContact.cs" company="Nodine Legal, LLC">
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
    public static class TaskAssignedContact
    {
        public static Common.Models.Tasks.TaskAssignedContact Get(
            Guid id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskAssignedContact, DBOs.Tasks.TaskAssignedContact>(
                "SELECT * FROM \"task_assigned_contact\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Tasks.TaskAssignedContact Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskAssignedContact Get(
            long taskId, 
            int contactId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskAssignedContact, DBOs.Tasks.TaskAssignedContact>(
                "SELECT * FROM \"task_assigned_contact\" WHERE \"task_id\"=@TaskId AND \"contact_id\"=@ContactId AND \"utc_disabled\" is null",
                new { TaskId = taskId, ContactId = contactId }, conn, closeConnection);
        }

        public static Common.Models.Tasks.TaskAssignedContact Get(
            Transaction t,
            long taskId,
            int contactId)
        {
            return Get(taskId, contactId, t.Connection, false);
        }

        public static List<Common.Models.Tasks.TaskAssignedContact> ListForTask(
            long taskId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Tasks.TaskAssignedContact, DBOs.Tasks.TaskAssignedContact>(
                "SELECT * FROM \"task_assigned_contact\" WHERE \"task_id\"=@TaskId AND \"utc_disabled\" is null",
                new { TaskId = taskId }, conn, closeConnection);
        }

        public static List<Common.Models.Tasks.TaskAssignedContact> ListForTask(
            Transaction t,
            long taskId)
        {
            return ListForTask(taskId, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskAssignedContact Create(
            Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Tasks.TaskAssignedContact dbo = Mapper.Map<DBOs.Tasks.TaskAssignedContact>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("INSERT INTO \"task_assigned_contact\" (\"id\", \"task_id\", \"contact_id\", \"assignment_type\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @TaskId, @ContactId, @AssignmentType, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskAssignedContact Create(
            Transaction t,
            Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskAssignedContact Edit(
            Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Tasks.TaskAssignedContact dbo = Mapper.Map<DBOs.Tasks.TaskAssignedContact>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"task_assigned_contact\" SET " +
                "\"task_id\"=@TaskId, \"contact_id\"=@ContactId, \"assignment_type\"=@AssignmentType, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskAssignedContact Edit(
            Transaction t,
            Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskAssignedContact Disable(
            Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Tasks.TaskAssignedContact,
                DBOs.Tasks.TaskAssignedContact>("task_assigned_contact", disabler.PId.Value, 
                model.Id, conn, closeConnection);

            return model;
        }
        
        public static Common.Models.Tasks.TaskAssignedContact Disable(
            Transaction t,
            Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Account.Users disabler)
        {
            return Disable(model, disabler, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskAssignedContact Enable(
            Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Account.Users enabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Tasks.TaskAssignedContact,
                DBOs.Tasks.TaskAssignedContact>("task_assigned_contact", enabler.PId.Value, 
                model.Id, conn, closeConnection);

            return model;
        }
        
        public static Common.Models.Tasks.TaskAssignedContact Enable(
            Transaction t,
            Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Account.Users enabler)
        {
            return Enable(model, enabler, t.Connection, false);
        }
    }
}