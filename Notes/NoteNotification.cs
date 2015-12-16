// -----------------------------------------------------------------------
// <copyright file="NoteNotification.cs" company="Nodine Legal, LLC">
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
    public static class NoteNotification
    {
        public static Common.Models.Notes.NoteNotification Get(
            Guid id,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Notes.NoteNotification, DBOs.Notes.NoteNotification>(
                "SELECT * FROM \"note_notification\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                new { Id = id }, conn, closeConnection);
        }

        public static Common.Models.Notes.NoteNotification Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Notes.NoteNotification Get(
            Guid noteId, 
            int contactId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Notes.NoteNotification, DBOs.Notes.NoteNotification>(
                "SELECT * FROM \"note_notification\" WHERE \"note_id\"=@NoteId AND \"contact_id\"=@ContactId AND \"utc_disabled\" is null",
                new { NoteId = noteId, ContactId = contactId }, conn, closeConnection);
        }

        public static Common.Models.Notes.NoteNotification Get(
            Transaction t,
            Guid noteId,
            int contactId)
        {
            return Get(noteId, contactId, t.Connection, false);
        }

        public static List<Common.Models.Notes.NoteNotification> ListForNote(
            Guid noteId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Notes.NoteNotification, DBOs.Notes.NoteNotification>(
                "SELECT * FROM \"note_notification\" WHERE \"note_id\"=@NoteId AND " +
                "\"utc_disabled\" is null ORDER BY \"cleared\" DESC",
                new { NoteId = noteId }, conn, closeConnection);
        }

        public static List<Common.Models.Notes.NoteNotification> ListForNote(
            Transaction t,
            Guid noteId)
        {
            return ListForNote(noteId, t.Connection, false);
        }

        public static List<Common.Models.Notes.NoteNotification> ListAllNoteNotificationsForContact(
            int contactId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Notes.NoteNotification, DBOs.Notes.NoteNotification>(
                "SELECT * FROM \"note_notification\" WHERE \"contact_id\"=@ContactId " +
                "AND \"cleared\" is null AND \"utc_disabled\" is null",
                new { ContactId = contactId }, conn, closeConnection);
        }

        public static List<Common.Models.Notes.NoteNotification> ListAllNoteNotificationsForContact(
            Transaction t,
            int contactId)
        {
            return ListAllNoteNotificationsForContact(contactId, t.Connection, false);
        }

        public static Common.Models.Notes.NoteNotification Create(
            Common.Models.Notes.NoteNotification model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Notes.NoteNotification dbo = Mapper.Map<DBOs.Notes.NoteNotification>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            Common.Models.Notes.NoteNotification currentModel = Get(model.Note.Id.Value, model.Contact.Id.Value, conn, false);

            if (currentModel != null)
            { // Update
                dbo = Mapper.Map<DBOs.Notes.NoteNotification>(currentModel);
                conn.Execute("UPDATE \"note_notification\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId, " +
                    "\"utc_disabled\"=null, \"disabled_by_user_pid\"=null, \"cleared\"=null WHERE \"id\"=@Id", dbo);
                model.Created = currentModel.Created;
                model.CreatedBy = currentModel.CreatedBy;
            }
            else
            { // Create
                if (conn.Execute("INSERT INTO \"note_notification\" (\"id\", \"note_id\", \"contact_id\", \"cleared\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @NoteId, @ContactId, @Cleared, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo) > 0)
                    model.Id = conn.Query<DBOs.Notes.NoteNotification>("SELECT currval(pg_get_serial_sequence('note_notification', 'id')) AS \"id\"").Single().Id;
            }

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Notes.NoteNotification Create(
            Transaction t,
            Common.Models.Notes.NoteNotification model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Notes.NoteNotification Clear(
            Common.Models.Notes.NoteNotification model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.Cleared = DateTime.Now;
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Notes.NoteNotification dbo = Mapper.Map<DBOs.Notes.NoteNotification>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"note_notification\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId, " +
                "\"cleared\"=@Cleared WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Notes.NoteNotification Clear(
            Transaction t,
            Common.Models.Notes.NoteNotification model,
            Common.Models.Account.Users modifier)
        {
            return Clear(model, modifier, t.Connection, false);
        }
    }
}
