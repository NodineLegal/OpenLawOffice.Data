// -----------------------------------------------------------------------
// <copyright file="Note.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;
    using System.Data;
    using AutoMapper;
    using Dapper;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Note
    {
        public static Common.Models.Notes.Note Get(
            Guid id,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Notes.Note, DBOs.Notes.Note>(
                "SELECT * FROM \"note\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Notes.Note Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Matters.Matter GetMatter(
            Guid noteId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"id\" IN (SELECT \"matter_id\" FROM " +
                "\"note_matter\" WHERE \"note_id\"=@NoteId AND \"utc_disabled\" is null) " +
                "AND \"matter\".\"utc_disabled\" is null",
                new { NoteId = noteId }, conn, closeConnection);
        }

        public static Common.Models.Matters.Matter GetMatter(
            Transaction t,
            Guid noteId)
        {
            return GetMatter(noteId, t.Connection, false);
        }

        public static Common.Models.Tasks.Task GetTask(
            Guid noteId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tasks.Task, DBOs.Tasks.Task>(
                "SELECT * FROM \"task\" WHERE \"id\" IN (SELECT \"task_id\" FROM " +
                "\"note_task\" WHERE \"note_id\"=@NoteId AND \"utc_disabled\" is null) " +
                "AND \"task\".\"utc_disabled\" is null",
                new { NoteId = noteId }, conn, closeConnection);
        }

        public static Common.Models.Tasks.Task GetTask(
            Transaction t,
            Guid noteId)
        {
            return GetTask(noteId, t.Connection, false);
        }

        public static List<Common.Models.Notes.Note> ListForMatter(
            Guid matterId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Notes.Note, DBOs.Notes.Note>(
                "SELECT \"note\".* FROM \"note\" JOIN \"note_matter\" ON " +
                "\"note\".\"id\"=\"note_matter\".\"note_id\" " +
                "WHERE \"note_matter\".\"matter_id\"=@MatterId " +
                "AND \"note\".\"utc_disabled\" is null " +
                "AND \"note_matter\".\"utc_disabled\" is null",
                new { MatterId = matterId }, conn, closeConnection);
        }

        public static List<Common.Models.Notes.Note> ListForMatter(
            Transaction t,
            Guid matterId)
        {
            return ListForMatter(matterId, t.Connection, false);
        }

        public static List<Common.Models.Notes.Note> ListForTask(
            long taskId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Notes.Note, DBOs.Notes.Note>(
                "SELECT \"note\".* FROM \"note\" JOIN \"note_task\" ON " +
                "\"note\".\"id\"=\"note_task\".\"note_id\" " +
                "WHERE \"note_task\".\"task_id\"=@TaskId " +
                "AND \"note\".\"utc_disabled\" is null " +
                "AND \"note_task\".\"utc_disabled\" is null",
                new { TaskId = taskId }, conn, closeConnection);
        }

        public static List<Common.Models.Notes.Note> ListForTask(
            Transaction t,
            long taskId)
        {
            return ListForTask(taskId, t.Connection, false);
        }

        public static Common.Models.Notes.Note Create(
            Common.Models.Notes.Note model, 
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Notes.Note dbo = Mapper.Map<DBOs.Notes.Note>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("INSERT INTO \"note\" (\"id\", \"title\", \"body\", \"timestamp\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @Title, @Body, @Timestamp, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Notes.Note Create(
            Transaction t,
            Common.Models.Notes.Note model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Notes.NoteMatter RelateMatter(
            Common.Models.Notes.Note model,
            Guid matterId, 
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return NoteMatter.Create(new Common.Models.Notes.NoteMatter()
            {
                Id = Guid.NewGuid(),
                Note = model,
                Matter = new Common.Models.Matters.Matter() { Id = matterId },
                CreatedBy = creator,
                ModifiedBy = creator,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            }, creator, conn, closeConnection);
        }

        public static Common.Models.Notes.NoteMatter RelateMatter(
            Transaction t,
            Common.Models.Notes.Note model,
            Guid matterId,
            Common.Models.Account.Users creator)
        {
            return RelateMatter(model, matterId, creator, t.Connection, false);
        }

        public static Common.Models.Notes.NoteTask RelateTask(
            Common.Models.Notes.Note model,
            long taskId, 
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return NoteTask.Create(new Common.Models.Notes.NoteTask()
            {
                Id = Guid.NewGuid(),
                Note = model,
                Task = new Common.Models.Tasks.Task { Id = taskId },
                CreatedBy = creator,
                ModifiedBy = creator,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            }, creator, conn, closeConnection);
        }

        public static Common.Models.Notes.NoteTask RelateTask(
            Transaction t,
            Common.Models.Notes.Note model,
            long taskId,
            Common.Models.Account.Users creator)
        {
            return RelateTask(model, taskId, creator, t.Connection, false);
        }

        public static Common.Models.Events.EventNote RelateEvent(
            Common.Models.Notes.Note model,
            Guid eventId, 
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return Events.EventNote.Create(new Common.Models.Events.EventNote()
            {
                Id = Guid.NewGuid(),
                Note = model,
                Event = new Common.Models.Events.Event() { Id = eventId },
                CreatedBy = creator,
                ModifiedBy = creator,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            }, creator, conn, closeConnection);
        }

        public static Common.Models.Events.EventNote RelateEvent(
            Transaction t,
            Common.Models.Notes.Note model,
            Guid eventId,
            Common.Models.Account.Users creator)
        {
            return RelateEvent(model, eventId, creator, t.Connection, false);
        }

        public static Common.Models.Notes.Note Edit(
            Common.Models.Notes.Note model,
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

        public static Common.Models.Notes.Note Edit(
            Transaction t,
            Common.Models.Notes.Note model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }
    }
}