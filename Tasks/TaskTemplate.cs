// -----------------------------------------------------------------------
// <copyright file="TaskTemplate.cs" company="Nodine Legal, LLC">
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
    public static class TaskTemplate
    {
        public static Common.Models.Tasks.TaskTemplate Get(
            long id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tasks.TaskTemplate, DBOs.Tasks.TaskTemplate>(
                "SELECT * FROM \"task_template\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Tasks.TaskTemplate Get(
            Transaction t,
            long id)
        {
            return Get(id, t.Connection, false);
        }

        public static List<Common.Models.Tasks.TaskTemplate> List(
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Tasks.TaskTemplate, DBOs.Tasks.TaskTemplate>(
                "SELECT * FROM \"task_template\" WHERE \"utc_disabled\" is null",
                null, conn, closeConnection);
        }

        public static List<Common.Models.Tasks.TaskTemplate> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static Common.Models.Tasks.TaskTemplate Create(
            Common.Models.Tasks.TaskTemplate model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Tasks.TaskTemplate dbo = Mapper.Map<DBOs.Tasks.TaskTemplate>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"task_template\" (\"task_template_title\", \"title\", \"description\", \"projected_start\", \"due_date\", \"projected_end\", " +
                "\"actual_end\", \"active\", \"utc_created\", " +
                "\"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@TaskTemplateTitle, @Title, @Description, @ProjectedStart, @DueDate, @ProjectedEnd, @ActualEnd, " +
                "@Active, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Tasks.TaskTemplate>("SELECT currval(pg_get_serial_sequence('task_template', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskTemplate Create(
            Transaction t,
            Common.Models.Tasks.TaskTemplate model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskTemplate Edit(
            Common.Models.Tasks.TaskTemplate model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Tasks.TaskTemplate dbo = Mapper.Map<DBOs.Tasks.TaskTemplate>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"task_template\" SET \"task_template_title\"=@TaskTemplateTitle, " +
                "\"title\"=@Title, \"description\"=@Description, \"projected_start\"=@ProjectedStart, " +
                "\"due_date\"=@DueDate, \"projected_end\"=@ProjectedEnd, \"actual_end\"=@ActualEnd, " +
                "\"active\"=@Active, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            //if (model.Parent != null && model.Parent.Id.HasValue)
            //    UpdateGroupingTaskProperties(OpenLawOffice.Data.Tasks.Task.Get(model.Parent.Id.Value));

            return model;
        }

        public static Common.Models.Tasks.TaskTemplate Edit(
            Transaction t,
            Common.Models.Tasks.TaskTemplate model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskTemplate Disable(
            Common.Models.Tasks.TaskTemplate model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Tasks.TaskTemplate,
                DBOs.Tasks.TaskTemplate>("task_template", disabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskTemplate Disable(
            Transaction t,
            Common.Models.Tasks.TaskTemplate model,
            Common.Models.Account.Users disabler)
        {
            return Disable(model, disabler, t.Connection, false);
        }
    }
}
