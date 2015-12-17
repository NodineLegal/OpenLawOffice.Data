// -----------------------------------------------------------------------
// <copyright file="NoteMatter.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Notes
{
    using System;
    using System.Data;
    using AutoMapper;
    using Dapper;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NoteMatter
    {
        public static Common.Models.Notes.NoteMatter Get(
            Guid id,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Notes.NoteMatter, DBOs.Notes.NoteMatter>(
                "SELECT * FROM \"note_matter\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                new { Id = id }, conn, closeConnection);
        }

        public static Common.Models.Notes.NoteMatter Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Notes.NoteMatter Get(
            Guid matterId, 
            Guid noteId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Notes.NoteMatter, DBOs.Notes.NoteMatter>(
                "SELECT * FROM \"note_matter\" WHERE \"matter_id\"=@MatterId AND \"note_id\"=@NoteId AND \"utc_disabled\" is null",
                new { MatterId = matterId, NoteId = noteId }, conn, closeConnection);
        }

        public static Common.Models.Notes.NoteMatter Get(
            Transaction t,
            Guid matterId,
            Guid noteId)
        {
            return Get(matterId, noteId, t.Connection, false);
        }

        public static Common.Models.Notes.NoteMatter GetIgnoringDisable(
            Guid matterId, 
            Guid noteId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Notes.NoteMatter, DBOs.Notes.NoteMatter>(
                "SELECT * FROM \"note_matter\" WHERE \"matter_id\"=@MatterId AND \"note_id\"=@NoteId",
                new { MatterId = matterId, NoteId = noteId }, conn, closeConnection);
        }

        public static Common.Models.Notes.NoteMatter GetIgnoringDisable(
            Transaction t,
            Guid matterId,
            Guid noteId)
        {
            return GetIgnoringDisable(matterId, noteId, t.Connection, false);
        }

        public static Common.Models.Matters.Matter GetRelatedMatter(
            Guid noteId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT \"matter\".* FROM \"note_matter\" JOIN \"matter\" ON \"note_matter\".\"matter_id\"=\"matter\".\"id\" " +
                "WHERE \"note_matter\".\"note_id\"=@NoteId " +
                "AND \"note_matter\".\"utc_disabled\" is null " +
                "AND \"matter\".\"utc_disabled\" is null ",
                new { NoteId = noteId }, conn, closeConnection);
        }

        public static Common.Models.Matters.Matter GetRelatedMatter(
            Transaction t,
            Guid noteId)
        {
            return GetRelatedMatter(noteId, t.Connection, false);
        }

        public static List<Common.Models.Notes.Note> ListForMatter(
            Guid matterId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Notes.Note, DBOs.Notes.Note>(
                "SELECT * FROM \"note\" WHERE \"id\" IN (SELECT \"note_id\" FROM \"note_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "\"utc_disabled\" is null ORDER BY \"timestamp\" DESC",
                new { MatterId = matterId }, conn, closeConnection);
        }

        public static List<Common.Models.Notes.Note> ListForMatter(
            Transaction t,
            Guid matterId)
        {
            return ListForMatter(matterId, t.Connection, false);
        }

        public static Common.Models.Notes.NoteMatter Create(
            Common.Models.Notes.NoteMatter model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Notes.NoteMatter dbo = Mapper.Map<DBOs.Notes.NoteMatter>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            Common.Models.Notes.NoteMatter currentModel = Get(model.Matter.Id.Value, model.Note.Id.Value, conn, false);

            if (currentModel != null)
            { // Update
                conn.Execute("UPDATE \"note_matter\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "\"utc_disabled\"=null, \"disabled_by_user_pid\"=null WHERE \"id\"=@Id", dbo);
                model.Created = currentModel.Created;
                model.CreatedBy = currentModel.CreatedBy;
            }
            else
            { // Create
                conn.Execute("INSERT INTO \"note_matter\" (\"id\", \"note_id\", \"matter_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @NoteId, @MatterId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Notes.NoteMatter Create(
            Transaction t,
            Common.Models.Notes.NoteMatter model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }
    }
}