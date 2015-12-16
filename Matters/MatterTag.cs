// -----------------------------------------------------------------------
// <copyright file="MatterTag.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class MatterTag
    {
        public static Common.Models.Matters.MatterTag Get(
            Guid id,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            Common.Models.Matters.MatterTag model =
                DataHelper.Get<Common.Models.Matters.MatterTag, DBOs.Matters.MatterTag>(
                "SELECT * FROM \"matter_tag\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, false);

            if (model == null) return null;

            if (model.TagCategory != null)
                model.TagCategory = Tagging.TagCategory.Get(model.TagCategory.Id, conn, false);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.MatterTag Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static List<Common.Models.Matters.MatterTag> List(
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Matters.MatterTag, DBOs.Matters.MatterTag>(
                "SELECT * FROM \"matter_tag\" WHERE \"utc_disabled\" is null", null, conn, closeConnection);
        }

        public static List<Common.Models.Matters.MatterTag> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static List<Common.Models.Matters.MatterTag> ListForMatter(
            Guid matterId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            List<Common.Models.Matters.MatterTag> list =
                DataHelper.List<Common.Models.Matters.MatterTag, DBOs.Matters.MatterTag>(
                "SELECT * FROM \"matter_tag\" WHERE \"matter_id\"=@MatterId AND \"utc_disabled\" is null",
                new { MatterId = matterId }, conn, closeConnection);

            list.ForEach(x =>
            {
                x.TagCategory = Tagging.TagCategory.Get(x.TagCategory.Id, conn, false);
            });

            DataHelper.Close(conn, closeConnection);

            return list;
        }

        public static List<Common.Models.Matters.MatterTag> ListForMatter(
            Transaction t,
            Guid matterId)
        {
            return ListForMatter(matterId, t.Connection, false);
        }

        public static Common.Models.Matters.MatterTag Create(
            Common.Models.Matters.MatterTag model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;

            conn = DataHelper.OpenIfNeeded(conn);

            Common.Models.Tagging.TagCategory existingTagCat = Tagging.TagCategory.Get(model.TagCategory.Name, conn, false);

            if (existingTagCat == null)
            {
                existingTagCat = Tagging.TagCategory.Create(model.TagCategory, creator, conn, false);
            }

            model.TagCategory = existingTagCat;
            DBOs.Matters.MatterTag dbo = Mapper.Map<DBOs.Matters.MatterTag>(model);
            
            if (conn.Execute("INSERT INTO \"matter_tag\" (\"id\", \"matter_id\", \"tag_category_id\", \"tag\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @MatterId, @TagCategoryId, @Tag, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Matters.MatterTag>("SELECT currval(pg_get_serial_sequence('matter_tag', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.MatterTag Create(
            Transaction t,
            Common.Models.Matters.MatterTag model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Matters.MatterTag Edit(
            Common.Models.Matters.MatterTag model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Matters.MatterTag dbo = Mapper.Map<DBOs.Matters.MatterTag>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"matter_tag\" SET " +
                "\"matter_id\"=@MatterId, \"tag\"=@Tag, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            model.TagCategory = UpdateTagCategory(model, modifier, conn, false);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.MatterTag Edit(
            Transaction t,
            Common.Models.Matters.MatterTag model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        private static Common.Models.Tagging.TagCategory UpdateTagCategory(
            Common.Models.Matters.MatterTag model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            Common.Models.Matters.MatterTag currentTag = Get(model.Id.Value, conn, false);

            if (currentTag.TagCategory != null)
            {
                if (model.TagCategory != null && !string.IsNullOrEmpty(model.TagCategory.Name))
                { // If current has tag & new has tag
                    // Are they the same - ignore if so
                    if (currentTag.Tag != model.Tag)
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

                    conn.Execute("UPDATE \"matter_tag\" SET \"tag_category_id\"=null WHERE \"id\"=@Id",
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
            Common.Models.Matters.MatterTag model,
            Common.Models.Account.Users modifier)
        {
            return UpdateTagCategory(model, modifier, t.Connection, false);
        }

        private static Common.Models.Tagging.TagCategory AddOrChangeTagCategory(
            Common.Models.Matters.MatterTag tag,
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

            conn = DataHelper.OpenIfNeeded(conn);

            // Update MatterTag's TagCategoryId
            conn.Execute("UPDATE \"matter_tag\" SET \"tag_category_id\"=@TagCategoryId WHERE \"id\"=@Id",
                new { Id = tag.Id.Value, TagCategoryId = tag.TagCategory.Id });

            DataHelper.Close(conn, closeConnection);

            return tag.TagCategory;
        }

        private static Common.Models.Tagging.TagCategory AddOrChangeTagCategory(
            Transaction t,
            Common.Models.Matters.MatterTag tag,
            Common.Models.Account.Users modifier)
        {
            return AddOrChangeTagCategory(tag, modifier, t.Connection, false);
        }

        public static Common.Models.Matters.MatterTag Disable(
            Common.Models.Matters.MatterTag model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Matters.MatterTag,
                DBOs.Matters.MatterTag>("matter_tag", disabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.MatterTag Disable(
            Transaction t,
            Common.Models.Matters.MatterTag model,
            Common.Models.Account.Users disabler)
        {
            return Disable(model, disabler, t.Connection, false);
        }

        public static Common.Models.Matters.MatterTag Enable(
            Common.Models.Matters.MatterTag model,
            Common.Models.Account.Users enabler,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Matters.MatterTag,
                DBOs.Matters.MatterTag>("matter_tag", enabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.MatterTag Enable(
            Transaction t,
            Common.Models.Matters.MatterTag model,
            Common.Models.Account.Users enabler)
        {
            return Enable(model, enabler, t.Connection, false);
        }

        public static List<Common.Models.Matters.MatterTag> Search(
            string text,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            List<Common.Models.Matters.MatterTag> list = new List<Common.Models.Matters.MatterTag>();
            List<DBOs.Matters.MatterTag> dbo = null;

            conn = DataHelper.OpenIfNeeded(conn);

            dbo = conn.Query<DBOs.Matters.MatterTag>(
                "SELECT * FROM \"matter_tag\" WHERE LOWER(\"tag\") LIKE '%' || @Query || '%'",
                new { Query = text }).ToList();

            dbo.ForEach(x =>
            {
                Common.Models.Matters.MatterTag mt = Mapper.Map<Common.Models.Matters.MatterTag>(x);
                mt.TagCategory = Tagging.TagCategory.Get(mt.TagCategory.Id, conn, closeConnection);
                mt.Matter = Matter.Get(mt.Matter.Id.Value, conn, closeConnection);
                list.Add(mt);
            });

            DataHelper.Close(conn, closeConnection);

            return list;
        }

        public static List<Common.Models.Matters.MatterTag> Search(
            Transaction t,
            string text)
        {
            return Search(text, t.Connection, false);
        }
    }
}