// -----------------------------------------------------------------------
// <copyright file="MatterContact.cs" company="Nodine Legal, LLC">
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
    public static class MatterContact
    {
        public static Common.Models.Matters.MatterContact Get(
            int id,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Matters.MatterContact, DBOs.Matters.MatterContact>(
                "SELECT * FROM \"matter_contact\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Matters.MatterContact Get(
            Transaction t,
            int id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Matters.MatterContact Get(
            Guid matterId, 
            int contactId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Matters.MatterContact, DBOs.Matters.MatterContact>(
                "SELECT * FROM \"matter_contact\" WHERE \"matter_id\"=@MatterId AND \"contact_id\"=@ContactId AND \"utc_disabled\" is null",
                new { MatterId = matterId, ContactId = contactId }, conn, closeConnection);
        }

        public static Common.Models.Matters.MatterContact Get(
            Transaction t,
            Guid matterId,
            int contactId)
        {
            return Get(matterId, contactId, t.Connection, false);
        }

        public static List<Common.Models.Matters.MatterContact> ListForMatter(
            Guid matterId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            List<Common.Models.Matters.MatterContact> list =
                DataHelper.List<Common.Models.Matters.MatterContact, DBOs.Matters.MatterContact>(
                "SELECT * FROM \"matter_contact\" WHERE \"matter_id\"=@MatterId AND \"utc_disabled\" is null",
                new { MatterId = matterId }, conn, false);

            list.ForEach(x =>
            {
                x.Contact = Contacts.Contact.Get(x.Contact.Id.Value, conn, false);
            });

            DataHelper.Close(conn, closeConnection);

            return list;
        }

        public static List<Common.Models.Matters.MatterContact> ListForMatter(
            Transaction t,
            Guid matterId)
        {
            return ListForMatter(matterId, t.Connection, false);
        }

        public static Common.Models.Matters.MatterContact Create(
            Common.Models.Matters.MatterContact model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Matters.MatterContact dbo = Mapper.Map<DBOs.Matters.MatterContact>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"matter_contact\" (\"matter_id\", \"contact_id\", " +
                "\"is_client\", \"is_client_contact\", \"is_appointed\", \"is_party\", \"party_title\", \"is_judge\", " +
                "\"is_witness\", \"is_attorney\", \"attorney_for_contact_id\", \"is_lead_attorney\", \"is_support_staff\", " +
                "\"support_staff_for_contact_id\", \"is_third_party_payor\", \"third_party_payor_for_contact_id\", " +
                "\"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@MatterId, @ContactId, @IsClient, @IsClientContact, @IsAppointed, @IsParty, " +
                "@PartyTitle, @IsJudge, @IsWitness, @IsAttorney, @AttorneyForContactId, @IsLeadAttorney, " +
                "@IsSupportStaff, @SupportStaffForContactId, @IsThirdPartyPayor, @ThirdPartyPayorForContactId, " +
                "@UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Matters.MatterContact>("SELECT currval(pg_get_serial_sequence('matter_contact', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.MatterContact Create(
            Transaction t,
            Common.Models.Matters.MatterContact model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Matters.MatterContact Edit(
            Common.Models.Matters.MatterContact model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Matters.MatterContact dbo = Mapper.Map<DBOs.Matters.MatterContact>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"matter_contact\" SET " +
                "\"matter_id\"=@MatterId, \"contact_id\"=@ContactId, " +
                "\"is_client\"=@IsClient, \"is_client_contact\"=@IsClientContact, \"is_appointed\"=@IsAppointed, " +
                "\"is_party\"=@IsParty, \"party_title\"=@PartyTitle, \"is_judge\"=@IsJudge, " +
                "\"is_witness\"=@IsWitness, \"is_attorney\"=@IsAttorney, " +
                "\"attorney_for_contact_id\"=@AttorneyForContactId, \"is_lead_attorney\"=@IsLeadAttorney, \"is_support_staff\"=@IsSupportStaff, " +
                "\"support_staff_for_contact_id\"=@SupportStaffForContactId, \"is_third_party_payor\"=@IsThirdPartyPayor, \"third_party_payor_for_contact_id\"=@ThirdPartyPayorForContactId, " +
                "\"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.MatterContact Edit(
            Transaction t,
            Common.Models.Matters.MatterContact model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static Common.Models.Matters.MatterContact Disable(
            Common.Models.Matters.MatterContact model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Matters.MatterContact,
                DBOs.Matters.MatterContact>("matter_contact", disabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.MatterContact Disable(
            Transaction t,
            Common.Models.Matters.MatterContact model,
            Common.Models.Account.Users disabler)
        {
            return Disable(model, disabler, t.Connection, false);
        }

        public static Common.Models.Matters.MatterContact Enable(
            Common.Models.Matters.MatterContact model,
            Common.Models.Account.Users enabler,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Matters.MatterContact,
                DBOs.Matters.MatterContact>("matter_contact", enabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.MatterContact Enable(
            Transaction t,
            Common.Models.Matters.MatterContact model,
            Common.Models.Account.Users enabler)
        {
            return Enable(model, enabler, t.Connection, false);
        }
    }
}