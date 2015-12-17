// -----------------------------------------------------------------------
// <copyright file="Event.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Events
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
    public static class Event
    {
        public static Common.Models.Events.Event Get(Guid id,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Events.Event, DBOs.Events.Event>(
                "SELECT * FROM \"event\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static List<Common.Models.Events.Event> List(
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                "SELECT * FROM \"event\" WHERE \"utc_disabled\" is null", null, conn, closeConnection);
        }

        public static List<Common.Models.Events.Event> List(double start, double? stop,
            IDbConnection conn = null, bool closeConnection = true)
        {
            if (stop.HasValue)
                return List(Common.Utilities.UnixTimeStampToDateTime(start),
                    Common.Utilities.UnixTimeStampToDateTime(stop.Value), conn, closeConnection);
            else
                return List(Common.Utilities.UnixTimeStampToDateTime(start), null, conn, closeConnection);
        }

        public static List<Common.Models.Events.Event> ListForUser(Guid userId, double start, double? stop,
            IDbConnection conn = null, bool closeConnection = true)
        {
            if (stop.HasValue)
                return ListForUser(userId, Common.Utilities.UnixTimeStampToDateTime(start),
                    Common.Utilities.UnixTimeStampToDateTime(stop.Value), conn, closeConnection);
            else
                return ListForUser(userId, Common.Utilities.UnixTimeStampToDateTime(start), null, conn, closeConnection);
        }

        public static List<Common.Models.Events.Event> ListForUserAndContact(Guid userId, int contactId, double start, double? stop,
            IDbConnection conn = null, bool closeConnection = true)
        {
            if (stop.HasValue)
                return ListForUserAndContact(userId, contactId, Common.Utilities.UnixTimeStampToDateTime(start),
                    Common.Utilities.UnixTimeStampToDateTime(stop.Value), conn, closeConnection);
            else
                return ListForUserAndContact(userId, contactId, Common.Utilities.UnixTimeStampToDateTime(start), null, conn, closeConnection);
        }

        public static List<Common.Models.Events.Event> ListForContact(int contactId, double start, double? stop,
            IDbConnection conn = null, bool closeConnection = true)
        {
            if (stop.HasValue)
                return ListForContact(contactId, Common.Utilities.UnixTimeStampToDateTime(start),
                    Common.Utilities.UnixTimeStampToDateTime(stop.Value), conn, closeConnection);
            else
                return ListForContact(contactId, Common.Utilities.UnixTimeStampToDateTime(start), null, conn, closeConnection);
        }

        public static List<Common.Models.Events.Event> List(DateTime start, DateTime? stop,
            IDbConnection conn = null, bool closeConnection = true)
        {
            List<Common.Models.Events.Event> events;

            if (stop.HasValue)
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT * FROM \"event\" WHERE \"utc_disabled\" is null AND \"start\" BETWEEN @Start AND @Stop",
                    new { Start = start, Stop = stop }, conn, closeConnection);
            else
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT * FROM \"event\" WHERE \"utc_disabled\" is null AND \"start\">=@Start",
                    new { Start = start }, conn, closeConnection);

            return events;
        }

        public static List<Common.Models.Events.Event> ListForUser(Guid userId, DateTime start, DateTime? stop,
            IDbConnection conn = null, bool closeConnection = true)
        {
            List<Common.Models.Events.Event> events;

            if (stop.HasValue)
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT * FROM \"event\" WHERE \"id\" IN (SELECT \"event_id\" FROM \"event_responsible_user\" WHERE \"user_pid\"=@UserPId) " +
                    "AND \"utc_disabled\" is null AND \"start\" BETWEEN @Start AND @Stop ORDER BY \"start\" ASC",
                    new { UserPId = userId, Start = start, Stop = stop }, conn, closeConnection);
            else
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT * FROM \"event\" WHERE \"id\" IN (SELECT \"event_id\" FROM \"event_responsible_user\" WHERE \"user_pid\"=@UserPId) " +
                    "AND \"utc_disabled\" is null AND \"start\">=@Start ORDER BY \"start\" ASC",
                    new { UserPId = userId, Start = start }, conn, closeConnection);

            return events;
        }

        public static List<Common.Models.Events.Event> ListForUserAndContact(Guid userId, int contactId, DateTime start, DateTime? stop,
            IDbConnection conn = null, bool closeConnection = true)
        {
            List<Common.Models.Events.Event> events;

            if (stop.HasValue)
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT DISTINCT * FROM \"event\" WHERE (\"id\" IN (SELECT \"event_id\" FROM \"event_responsible_user\" WHERE \"user_pid\"=@UserPId) " +
                    "OR \"id\" IN (SELECT \"event_id\" FROM \"event_assigned_contact\" WHERE \"contact_id\"=@ContactId)) " +
                    "AND \"utc_disabled\" is null AND \"start\" BETWEEN @Start AND @Stop ORDER BY \"start\" ASC",
                    new { UserPId = userId, ContactId = contactId, Start = start, Stop = stop }, conn, closeConnection);
            else
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT DISTINCT * FROM \"event\" WHERE (\"id\" IN (SELECT \"event_id\" FROM \"event_responsible_user\" WHERE \"user_pid\"=@UserPId) " +
                    "OR \"id\" IN (SELECT \"event_id\" FROM \"event_assigned_contact\" WHERE \"contact_id\"=@ContactId)) " +
                    "AND \"utc_disabled\" is null AND \"start\">=@Start ORDER BY \"start\" ASC",
                    new { UserPId = userId, ContactId = contactId, Start = start }, conn, closeConnection);

            return events;
        }

        public static List<Common.Models.Events.Event> ListForContact(int contactId, DateTime start, DateTime? stop,
            IDbConnection conn = null, bool closeConnection = true)
        {
            List<Common.Models.Events.Event> events;

            start = start.ToDbTime();

            if (stop.HasValue)
            {
                stop = stop.Value.ToDbTime();
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT * FROM \"event\" WHERE \"id\" IN (SELECT \"event_id\" FROM \"event_assigned_contact\" WHERE \"contact_id\"=@ContactId) " +
                    "AND \"utc_disabled\" is null AND \"start\" BETWEEN @Start AND @Stop ORDER BY \"start\" ASC",
                    new { ContactId = contactId, Start = start, Stop = stop }, conn, closeConnection);
            }
            else
                events = DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                    "SELECT * FROM \"event\" WHERE \"id\" IN (SELECT \"event_id\" FROM \"event_assigned_contact\" WHERE \"contact_id\"=@ContactId) " +
                    "AND \"utc_disabled\" is null AND \"start\">=@Start ORDER BY \"start\" ASC",
                    new { ContactId = contactId, Start = start }, conn, closeConnection);

            return events;
        }

        public static List<Common.Models.Matters.Matter> ListMattersFor(Guid eventId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"id\" IN (SELECT \"matter_id\" FROM \"event_matter\" WHERE \"event_id\"=@EventId) " +
                "AND \"utc_disabled\" is null",
                new { EventId = eventId }, conn, closeConnection);
        }

        public static List<Common.Models.Events.Event> ListForMatter(Guid matterId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                "SELECT * FROM \"event\" WHERE \"id\" in (SELECT \"event_id\" FROM \"event_matter\" WHERE \"matter_id\"=@MatterId) " +
                "AND \"utc_disabled\" is null",
                new { MatterId = matterId }, conn, closeConnection);
        }

        public static List<Common.Models.Notes.Note> ListNotesFor(Guid eventId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Notes.Note, DBOs.Notes.Note>(
                "SELECT * FROM \"notes\" WHERE \"id\" IN (SELECT \"note_id\" FROM \"event_note\" WHERE \"event_id\"=@EventId) " +
                "AND \"utc_disabled\" is null",
                new { EventId = eventId }, conn, closeConnection);
        }

        public static List<Common.Models.Matters.Matter> ListTasksFor(Guid eventId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"task\" WHERE \"id\" IN (SELECT \"task_id\" FROM \"event_task\" WHERE \"event_id\"=@EventId) " +
                "AND \"utc_disabled\" is null",
                new { EventId = eventId }, conn, closeConnection);
        }

        public static List<Common.Models.Events.Event> ListForTask(long taskId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Events.Event, DBOs.Events.Event>(
                "SELECT * FROM \"event\" WHERE \"id\" in (SELECT \"event_id\" FROM \"event_task\" WHERE \"task_id\"=@TaskId) " +
                "AND \"utc_disabled\" is null",
                new { TaskId = taskId }, conn, closeConnection);
        }

        public static Common.Models.Events.Event Create(Common.Models.Events.Event model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Events.Event dbo = null;

            dbo = Mapper.Map<DBOs.Events.Event>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("INSERT INTO \"event\" (\"id\", \"title\", \"allday\", \"start\", \"end\", \"location\", \"description\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @Title, @AllDay, @Start, @End, @Location, @Description, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo);
            
            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Events.Event Edit(Common.Models.Events.Event model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Events.Event dbo = Mapper.Map<DBOs.Events.Event>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"event\" SET " +
                "\"title\"=@Title, \"allday\"=@AllDay, \"start\"=@Start, " +
                "\"end\"=@End, \"location\"=@Location, \"description\"=@Description, " +
                "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Events.EventMatter RelateToMatter(Common.Models.Events.Event model,
            Guid matterId,
            Common.Models.Account.Users actor,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return RelateToMatter(model, new Common.Models.Matters.Matter() { Id = matterId }, actor, conn, closeConnection);
        }

        public static Common.Models.Events.EventMatter RelateToMatter(Common.Models.Events.Event model,
            Common.Models.Matters.Matter matter,
            Common.Models.Account.Users actor,
            IDbConnection conn = null, bool closeConnection = true)
        {
            Common.Models.Events.EventMatter em;
            DBOs.Events.EventMatter dbo = null;

            em = Data.Events.EventMatter.Get(model.Id.Value, matter.Id.Value);

            if (em != null)
                return em;

            em = new Common.Models.Events.EventMatter();
            em.Id = Guid.NewGuid();
            em.CreatedBy = em.ModifiedBy = actor;
            em.Created = em.Modified = DateTime.UtcNow;
            em.Event = model;
            em.Matter = matter;

            dbo = Mapper.Map<DBOs.Events.EventMatter>(em);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"event_matter\" (\"id\", \"event_id\", \"matter_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @EventId, @MatterId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Events.EventMatter>("SELECT currval(pg_get_serial_sequence('event_matter', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return em;
        }

        public static Common.Models.Events.EventTask RelateToTask(Common.Models.Events.Event model,
            long taskId,
            Common.Models.Account.Users actor,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return RelateToTask(model, new Common.Models.Tasks.Task() { Id = taskId }, actor, conn, closeConnection);
        }

        public static Common.Models.Events.EventTask RelateToTask(Common.Models.Events.Event model,
            Common.Models.Tasks.Task task,
            Common.Models.Account.Users actor,
            IDbConnection conn = null, bool closeConnection = true)
        {
            Common.Models.Events.EventTask et;
            DBOs.Events.EventTask dbo = null;

            et = Data.Events.EventTask.Get(task.Id.Value, model.Id.Value);

            if (et != null)
                return et;

            et = new Common.Models.Events.EventTask();
            et.Id = Guid.NewGuid();
            et.CreatedBy = et.ModifiedBy = actor;
            et.Created = et.Modified = DateTime.UtcNow;
            et.Event = model;
            et.Task = task;

            dbo = Mapper.Map<DBOs.Events.EventTask>(et);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"event_task\" (\"id\", \"event_id\", \"task_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @EventId, @TaskId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Events.EventTask>("SELECT currval(pg_get_serial_sequence('event_task', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return et;
        }
    }
}
