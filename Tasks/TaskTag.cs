// -----------------------------------------------------------------------
// <copyright file="TaskTag.cs" company="Nodine Legal, LLC">
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
    public static class TaskTag
    {
        public static Common.Models.Tasks.TaskTag Get(
            Guid id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            Common.Models.Tasks.TaskTag model =
                DataHelper.Get<Common.Models.Tasks.TaskTag, DBOs.Tasks.TaskTag>(
                "SELECT * FROM \"task_tag\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, false);

            if (model == null) return null;

            if (model.TagCategory != null)
                model.TagCategory = Tagging.TagCategory.Get(model.TagCategory.Id, conn, false);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskTag Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static List<Common.Models.Tasks.TaskTag> List(
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Tasks.TaskTag, DBOs.Tasks.TaskTag>(
                "SELECT * FROM \"task_tag\" WHERE \"utc_disabled\" is null", null, conn, closeConnection);
        }

        public static List<Common.Models.Tasks.TaskTag> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static List<Common.Models.Tasks.TaskTag> ListForTask(
            long taskId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            List<Common.Models.Tasks.TaskTag> list =
                DataHelper.List<Common.Models.Tasks.TaskTag, DBOs.Tasks.TaskTag>(
                "SELECT * FROM \"task_tag\" WHERE \"task_id\"=@TaskId AND \"utc_disabled\" is null",
                new { TaskId = taskId }, conn, false);

            list.ForEach(x =>
            {
                x.TagCategory = Tagging.TagCategory.Get(x.TagCategory.Id, conn, false);
            });

            DataHelper.Close(conn, closeConnection);

            return list;
        }

        public static List<Common.Models.Tasks.TaskTag> ListForTask(
            Transaction t,
            long taskId)
        {
            return ListForTask(taskId, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskTag Create(
            Common.Models.Tasks.TaskTag model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;

            Common.Models.Tagging.TagCategory existingTagCat = 
                Tagging.TagCategory.Get(model.TagCategory.Name, conn, false);

            if (existingTagCat == null)
            {
                existingTagCat = Tagging.TagCategory.Create(model.TagCategory, creator,
                    conn, false);
            }

            model.TagCategory = existingTagCat;
            DBOs.Tasks.TaskTag dbo = Mapper.Map<DBOs.Tasks.TaskTag>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("INSERT INTO \"task_tag\" (\"id\", \"task_id\", \"tag_category_id\", \"tag\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @TaskId, @TagCategoryId, @Tag, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskTag Create(
            Transaction t,
            Common.Models.Tasks.TaskTag model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskTag Edit(
            Common.Models.Tasks.TaskTag model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Tasks.TaskTag dbo = Mapper.Map<DBOs.Tasks.TaskTag>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"task_tag\" SET " +
                "\"task_id\"=@TaskId, \"tag\"=@Tag, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            model.TagCategory = UpdateTagCategory(model, modifier, conn, false);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskTag Edit(
            Transaction t,
            Common.Models.Tasks.TaskTag model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        private static Common.Models.Tagging.TagCategory UpdateTagCategory(
            Common.Models.Tasks.TaskTag model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            Common.Models.Tasks.TaskTag currentTag = Get(model.Id.Value, conn, false);

            if (currentTag.TagCategory != null)
            {
                if (model.TagCategory != null && !string.IsNullOrEmpty(model.TagCategory.Name))
                { // If current has tag & new has tag
                    // Are they the same - ignore if so
                    if (currentTag.TagCategory.Name != model.TagCategory.Name)
                    {
                        // Update - change tagcat
                        model.TagCategory = AddOrChangeTagCategory(model, modifier, conn, false);
                    }
                }
                else
                {
                    // If current has tag & new !has tag
                    // Update - drop tagcat
                    currentTag.TagCategory = null;
                    
                    conn.Execute("UPDATE \"task_tag\" SET \"tag_category_id\"=null WHERE \"id\"=@Id",
                        new { Id = model.Id.Value });
                }
            }
            else
            {
                if (model.TagCategory != null && !string.IsNullOrEmpty(model.TagCategory.Name))
                { // If current !has tag & new has tag
                    // Update - add tagcat
                    model.TagCategory = AddOrChangeTagCategory(model, modifier, conn, false);
                }

                // If current !has tag & new !has tag - do nothing
            }

            DataHelper.Close(conn, closeConnection);

            return model.TagCategory;
        }

        private static Common.Models.Tagging.TagCategory UpdateTagCategory(
            Transaction t,
            Common.Models.Tasks.TaskTag model,
            Common.Models.Account.Users modifier)
        {
            return UpdateTagCategory(model, modifier, t.Connection, false);
        }

        private static Common.Models.Tagging.TagCategory AddOrChangeTagCategory(
            Common.Models.Tasks.TaskTag tag,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            Common.Models.Tagging.TagCategory newTagCat = null;

            // Check for existing name
            if (tag.TagCategory != null && !string.IsNullOrEmpty(tag.TagCategory.Name))
            {
                newTagCat = Tagging.TagCategory.Get(tag.TagCategory.Name, conn, false);
            }

            // Either need to use existing or create a new tag category
            if (newTagCat != null)
            {
                // Can use existing
                tag.TagCategory = newTagCat;

                // If new tagcat was disabled, it needs enabled
                if (newTagCat.Disabled.HasValue)
                {
                    tag.TagCategory = Tagging.TagCategory.Enable(tag.TagCategory, modifier, conn, false);
                }
            }
            else
            {
                // Add one
                tag.TagCategory = Tagging.TagCategory.Create(tag.TagCategory, modifier, conn, false);
            }

            // Update MatterTag's TagCategoryId
            conn.Execute("UPDATE \"task_tag\" SET \"tag_category_id\"=@TagCategoryId WHERE \"id\"=@Id",
                new { Id = tag.Id.Value, TagCategoryId = tag.TagCategory.Id });

            DataHelper.Close(conn, closeConnection);

            return tag.TagCategory;
        }

        private static Common.Models.Tagging.TagCategory AddOrChangeTagCategory(
            Transaction t,
            Common.Models.Tasks.TaskTag tag,
            Common.Models.Account.Users modifier)
        {
            return AddOrChangeTagCategory(tag, modifier, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskTag Disable(
            Common.Models.Tasks.TaskTag model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Matters.MatterContact,
                DBOs.Matters.MatterContact>("task_tag", disabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskTag Disable(
            Transaction t,
            Common.Models.Tasks.TaskTag model,
            Common.Models.Account.Users disabler)
        {
            return Disable(model, disabler, t.Connection, false);
        }

        public static Common.Models.Tasks.TaskTag Enable(
            Common.Models.Tasks.TaskTag model,
            Common.Models.Account.Users enabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Matters.MatterContact,
                DBOs.Matters.MatterContact>("task_tag", enabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Tasks.TaskTag Enable(
            Transaction t,
            Common.Models.Tasks.TaskTag model,
            Common.Models.Account.Users enabler)
        {
            return Enable(model, enabler, t.Connection, false);
        }

        public static List<Common.Models.Tasks.TaskTag> ListForTask(
            Guid taskId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            List<Common.Models.Tasks.TaskTag> list =
                DataHelper.List<Common.Models.Tasks.TaskTag, DBOs.Tasks.TaskTag>(
                "SELECT * FROM \"matter_tag\" WHERE \"task_id\"=@TaskId AND \"utc_disabled\" is null",
                new { TaskId = taskId }, conn, false);

            list.ForEach(x =>
            {
                x.TagCategory = Tagging.TagCategory.Get(x.TagCategory.Id, conn, false);
            });

            DataHelper.Close(conn, closeConnection);

            return list;
        }

        public static List<Common.Models.Tasks.TaskTag> ListForTask(
            Transaction t,
            Guid taskId)
        {
            return ListForTask(taskId, t.Connection, false);
        }

        public static List<Common.Models.Tasks.TaskTag> Search(
            string text,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            List<Common.Models.Tasks.TaskTag> list = new List<Common.Models.Tasks.TaskTag>();
            List<DBOs.Tasks.TaskTag> dbo = null;

            conn = DataHelper.OpenIfNeeded(conn);

            dbo = conn.Query<DBOs.Tasks.TaskTag>(
                "SELECT * FROM \"task_tag\" WHERE LOWER(\"tag\") LIKE '%' || @Query || '%'",
                new { Query = text }).ToList();

            dbo.ForEach(x =>
            {
                Common.Models.Tasks.TaskTag tt = Mapper.Map<Common.Models.Tasks.TaskTag>(x);
                tt.TagCategory = Tagging.TagCategory.Get(tt.TagCategory.Id, conn, false);
                tt.Task = Task.Get(tt.Task.Id.Value, conn, false);
                list.Add(tt);
            });

            DataHelper.Close(conn, closeConnection);

            return list;
        }

        public static List<Common.Models.Tasks.TaskTag> Search(
            Transaction t,
            string text)
        {
            return Search(text, t.Connection, false);
        }
    }
}