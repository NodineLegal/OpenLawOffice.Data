// -----------------------------------------------------------------------
// <copyright file="OpportunityContact.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Opportunities
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    public class OpportunityContact
    {
        public static Common.Models.Opportunities.OpportunityContact Get(
            long id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Opportunities.OpportunityContact, DBOs.Opportunities.OpportunityContact>(
                "SELECT * FROM \"opportunity_contact\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Opportunities.OpportunityContact Get(
            Transaction t,
            long id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Opportunities.OpportunityContact Get(
            long OpportunityId,
            int contactId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Opportunities.OpportunityContact, DBOs.Opportunities.OpportunityContact>(
                "SELECT * FROM \"opportunity_contact\" WHERE \"opportunity_id\"=@OpportunityId AND \"contact_id\"=@ContactId AND \"utc_disabled\" is null",
                new { OpportunityId = OpportunityId, ContactId = contactId }, conn, closeConnection);
        }

        public static Common.Models.Opportunities.OpportunityContact Get(
            Transaction t,
            long OpportunityId,
            int contactId)
        {
            return Get(OpportunityId, contactId, t.Connection, false);
        }

        public static List<Common.Models.Opportunities.OpportunityContact> ListForOpportunity(
            long OpportunityId,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            List<Common.Models.Opportunities.OpportunityContact> list =
                DataHelper.List<Common.Models.Opportunities.OpportunityContact, DBOs.Opportunities.OpportunityContact>(
                "SELECT * FROM \"opportunity_contact\" WHERE \"opportunity_id\"=@OpportunityId AND \"utc_disabled\" is null",
                new { OpportunityId = OpportunityId }, conn, false);

            list.ForEach(x =>
            {
                x.Contact = Contacts.Contact.Get(x.Contact.Id.Value, conn, false);
            });

            DataHelper.Close(conn, closeConnection);

            return list;
        }

        public static List<Common.Models.Opportunities.OpportunityContact> ListForOpportunity(
            Transaction t,
            long OpportunityId)
        {
            return ListForOpportunity(OpportunityId, t.Connection, false);
        }

        public static Common.Models.Opportunities.OpportunityContact Create(
            Common.Models.Opportunities.OpportunityContact model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            Common.Models.Opportunities.OpportunityContact orig =
                DataHelper.Get<Common.Models.Opportunities.OpportunityContact, DBOs.Opportunities.OpportunityContact>(
                "SELECT * FROM \"opportunity_contact\" WHERE\"opportunity_id\"=@OpportunityId AND \"contact_id\"=@ContactId",
                new { OpportunityId = model.Opportunity.Id.Value, ContactId = model.Contact.Id.Value }, conn, closeConnection);

            if (orig != null)
            {
                if (orig.Disabled.HasValue || orig.DisabledBy != null)
                {
                    model.Id = orig.Id;
                    orig = Edit(model, creator, conn, false);
                    orig = Enable(model, creator, conn, false);
                }
            }
            else
            {
                model.Created = model.Modified = DateTime.UtcNow;
                model.CreatedBy = model.ModifiedBy = creator;

                DBOs.Opportunities.OpportunityContact dbo = Mapper.Map<DBOs.Opportunities.OpportunityContact>(model);

                conn = DataHelper.OpenIfNeeded(conn);

                if (conn.Execute("INSERT INTO \"opportunity_contact\" (\"opportunity_id\", \"contact_id\", " +
                    "\"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@OpportunityId, @ContactId, " +
                    "@UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo) > 0)
                    model.Id = conn.Query<DBOs.Opportunities.OpportunityContact>("SELECT currval(pg_get_serial_sequence('opportunity_contact', 'id')) AS \"id\"").Single().Id;
            }
            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Opportunities.OpportunityContact Create(
            Transaction t,
            Common.Models.Opportunities.OpportunityContact model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Opportunities.OpportunityContact Edit(
            Common.Models.Opportunities.OpportunityContact model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Opportunities.OpportunityContact dbo = Mapper.Map<DBOs.Opportunities.OpportunityContact>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"opportunity_contact\" SET " +
                "\"opportunity_id\"=@OpportunityId, \"contact_id\"=@ContactId, " +
                "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Opportunities.OpportunityContact Edit(
            Transaction t,
            Common.Models.Opportunities.OpportunityContact model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static Common.Models.Opportunities.OpportunityContact Disable(
            Common.Models.Opportunities.OpportunityContact model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Opportunities.OpportunityContact,
                DBOs.Opportunities.OpportunityContact>("opportunity_contact", disabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Opportunities.OpportunityContact Disable(
            Transaction t,
            Common.Models.Opportunities.OpportunityContact model,
            Common.Models.Account.Users disabler)
        {
            return Disable(model, disabler, t.Connection, false);
        }

        public static Common.Models.Opportunities.OpportunityContact Enable(
            Common.Models.Opportunities.OpportunityContact model,
            Common.Models.Account.Users enabler,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Opportunities.OpportunityContact,
                DBOs.Opportunities.OpportunityContact>("opportunity_contact", enabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Opportunities.OpportunityContact Enable(
            Transaction t,
            Common.Models.Opportunities.OpportunityContact model,
            Common.Models.Account.Users enabler)
        {
            return Enable(model, enabler, t.Connection, false);
        }
    }
}
