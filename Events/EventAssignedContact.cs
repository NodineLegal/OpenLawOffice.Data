// -----------------------------------------------------------------------
// <copyright file="EventAssignedContact.cs" company="Nodine Legal, LLC">
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
    public static class EventAssignedContact
    {
        public static Common.Models.Events.EventAssignedContact Get(Guid id,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Events.EventAssignedContact, DBOs.Events.EventAssignedContact>(
                "SELECT * FROM \"event_assigned_contact\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Events.EventAssignedContact Get(Guid eventId, int contactId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Events.EventAssignedContact, DBOs.Events.EventAssignedContact>(
                "SELECT * FROM \"event_assigned_contact\" WHERE \"event_id\"=@EventId AND \"contact_id\"=@ContactId AND \"utc_disabled\" is null",
                new { EventId = eventId, ContactId = contactId }, conn, closeConnection);
        }

        public static List<Common.Models.Events.EventAssignedContact> ListForEvent(Guid eventId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Events.EventAssignedContact, DBOs.Events.EventAssignedContact>(
                "SELECT * FROM \"event_assigned_contact\" WHERE " +
                "\"event_id\"=@EventId AND \"utc_disabled\" is null",
                new { EventId = eventId }, conn, closeConnection);
        }

        public static Common.Models.Events.EventAssignedContact Create(Common.Models.Events.EventAssignedContact model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Events.EventAssignedContact dbo = Mapper.Map<DBOs.Events.EventAssignedContact>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"event_assigned_contact\" (\"id\", \"event_id\", \"contact_id\", \"role\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @EventId, @ContactId, @Role, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Events.EventAssignedContact>("SELECT currval(pg_get_serial_sequence('event_assigned_contact', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Events.EventAssignedContact Edit(Common.Models.Events.EventAssignedContact model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Events.EventAssignedContact dbo = Mapper.Map<DBOs.Events.EventAssignedContact>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"event_assigned_contact\" SET " +
                "\"event_id\"=@EventId, \"contact_id\"=@ContactId, \"role\"=@Role, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Events.EventAssignedContact Disable(Common.Models.Events.EventAssignedContact model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null, bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Events.EventAssignedContact,
                DBOs.Events.EventAssignedContact>("event_assigned_contact", disabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Events.EventAssignedContact Enable(Common.Models.Events.EventAssignedContact model,
            Common.Models.Account.Users enabler,
            IDbConnection conn = null, bool closeConnection = true)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Events.EventAssignedContact,
                DBOs.Events.EventAssignedContact>("event_assigned_contact", enabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }
    }
}
