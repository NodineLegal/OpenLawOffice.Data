// -----------------------------------------------------------------------
// <copyright file="Matter.cs" company="Nodine Legal, LLC">
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
    using AutoMapper;
    using Dapper;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Matter
    {
        public static List<Common.Models.Matters.Matter> ListPossibleDuplicates(
            Common.Models.Matters.Matter model,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            DBOs.Matters.Matter dbo = Mapper.Map<DBOs.Matters.Matter>(model);
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE (LOWER(\"title\") LIKE '%' || LOWER(@Title) || '%') OR " +
                //"OR (LOWER(\"title\") LIKE '%' || @Title || '%') OR " +
                "(\"court_type_id\"=@CourtTypeId AND \"court_geographical_jurisdiction_id\"=@CourtGeographicalJurisdictionId AND " +
                "\"case_number\"=@CaseNumber) OR (\"bill_to_contact_id\"=@BillToContactId AND \"case_number\"=@CaseNumber) AND \"utc_disabled\" is null",
                dbo, conn, closeConnection);
        }

        public static List<Common.Models.Matters.Matter> ListPossibleDuplicates(
            Transaction t,
            Common.Models.Matters.Matter model)
        {
            return ListPossibleDuplicates(model, t.Connection, false);
        }

        public static Common.Models.Matters.Matter Get(
            Guid id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Matters.Matter Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static List<Common.Models.Matters.Matter> List(
            bool? active,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!active.HasValue)
                return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                    "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null", null, conn, closeConnection);
            else
                return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                    "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null AND \"active\"=@Active",
                    new { Active = active.Value }, conn, closeConnection);
        }

        public static List<Common.Models.Matters.Matter> List(
            Transaction t,
            bool? active)
        {
            return List(active, t.Connection, false);
        }

        public static List<Common.Models.Matters.Matter> List(
            bool? active, 
            string contactFilter,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!active.HasValue)
            {
                if (!string.IsNullOrEmpty(contactFilter))
                    return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null AND " +
                        "\"id\" IN (SELECT \"matter_id\" FROM \"matter_contact\" WHERE " +
                        "\"contact_id\" IN (SELECT \"id\" FROM \"contact\" WHERE LOWER(\"display_name\") LIKE '%' || @ContactFilter || '%'))",
                        new { ContactFilter = contactFilter.ToLower() }, conn, closeConnection);
                else
                    return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null", null, conn, closeConnection);
            }
            else
            {
                if (!string.IsNullOrEmpty(contactFilter))
                    return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null AND \"active\"= @Active AND " +
                        "\"id\" IN (SELECT \"matter_id\" FROM \"matter_contact\" WHERE " +
                        "\"contact_id\" IN (SELECT \"id\" FROM \"contact\" WHERE LOWER(\"display_name\") LIKE '%' || @ContactFilter || '%'))",
                        new { ContactFilter = contactFilter.ToLower(), Active = active.Value }, conn, closeConnection);
                else
                    return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                        "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null AND \"active\"=@Active",
                        new { Active = active.Value }, conn, closeConnection);
            }
        }

        public static List<Common.Models.Matters.Matter> List(
            Transaction t,
            bool? active,
            string contactFilter)
        {
            return List(active, contactFilter, t.Connection, false);
        }

        public static List<Common.Models.Matters.Matter> List(
            bool? active, 
            string contactFilter, 
            string titleFilter, 
            string caseNumberFilter, 
            string jurisdictionFilter,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            string sql = "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null ";

            if (contactFilter != null)
                contactFilter = contactFilter.ToLower();
            if (titleFilter != null)
                titleFilter = titleFilter.ToLower();
            if (caseNumberFilter != null)
                caseNumberFilter = caseNumberFilter.ToLower();
            if (jurisdictionFilter != null)
                jurisdictionFilter = jurisdictionFilter.ToLower();

            if (active.HasValue)
            {
                sql += " AND \"active\"=@Active ";
            }
            if (!string.IsNullOrEmpty(contactFilter)) ***FLAGS***
            {
                sql += " AND \"id\" IN (SELECT \"matter_id\" FROM \"matter_contact\" WHERE " +
                    "\"contact_id\" IN (SELECT \"id\" FROM \"contact\" WHERE LOWER(\"display_name\") LIKE '%' || @ContactFilter || '%'))";
            }
            if (!string.IsNullOrEmpty(titleFilter))
            {
                sql += " AND LOWER(\"title\") LIKE '%' || @TitleFilter || '%' ";
            }
            if (!string.IsNullOrEmpty(caseNumberFilter))
            {
                sql += " AND LOWER(\"case_number\") LIKE '%' || @CaseNumberFilter || '%' ";
            }
            if (!string.IsNullOrEmpty(jurisdictionFilter))
            {
                sql += " AND LOWER(\"jurisdiction\") LIKE '%' || @JurisdictionFilter || '%' ";
            }

            if (active.HasValue)
                return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(sql,
                    new
                    {
                        Active = active.Value,
                        ContactFilter = contactFilter,
                        TitleFilter = titleFilter,
                        CaseNumberFilter = caseNumberFilter,
                        JurisdictionFilter = jurisdictionFilter
                    }, conn, closeConnection);
            else
                return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(sql,
                    new
                    {
                        ContactFilter = contactFilter,
                        TitleFilter = titleFilter,
                        CaseNumberFilter = caseNumberFilter,
                        JurisdictionFilter = jurisdictionFilter
                    }, conn, closeConnection);
        }

        public static List<Common.Models.Matters.Matter> List(
            Transaction t,
            bool? active,
            string contactFilter,
            string titleFilter,
            string caseNumberFilter,
            string jurisdictionFilter)
        {
            return List(active, contactFilter, titleFilter, caseNumberFilter, jurisdictionFilter, t.Connection, false);
        }

        public static List<Common.Models.Matters.Matter> ListTitlesOnly(
            string title,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!string.IsNullOrEmpty(title))
                title = title.ToLower();
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT DISTINCT \"title\" FROM \"matter\" WHERE \"utc_disabled\" is null AND LOWER(\"title\") LIKE '%' || @Title || '%'",
                new { Title = title }, conn, closeConnection);
        }

        public static List<Common.Models.Matters.Matter> ListTitlesOnly(
            Transaction t,
            string title)
        {
            return ListTitlesOnly(title, t.Connection, false);
        }

        public static List<Common.Models.Matters.Matter> ListCaseNumbersOnly(
            string caseNumber,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!string.IsNullOrEmpty(caseNumber))
                caseNumber = caseNumber.ToLower();
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT DISTINCT \"case_number\" FROM \"matter\" WHERE \"utc_disabled\" is null AND LOWER(\"case_number\") LIKE '%' || @CaseNumber || '%'",
                new { CaseNumber = caseNumber }, conn, closeConnection);
        }

        public static List<Common.Models.Matters.Matter> ListCaseNumbersOnly(
            Transaction t,
            string caseNumber)
        {
            return ListCaseNumbersOnly(caseNumber, t.Connection, false);
        }

        public static List<Common.Models.Matters.Matter> ListJurisdictionsOnly(
            string jurisdiction,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!string.IsNullOrEmpty(jurisdiction))
                jurisdiction = jurisdiction.ToLower();
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT DISTINCT \"jurisdiction\" FROM \"matter\" WHERE \"utc_disabled\" is null AND LOWER(\"jurisdiction\") LIKE '%' || @Jurisdiction || '%'",
                new { Jurisdiction = jurisdiction }, conn, closeConnection);
        }

        public static List<Common.Models.Matters.Matter> ListJurisdictionsOnly(
            Transaction t,
            string jurisdiction)
        {
            return ListJurisdictionsOnly(jurisdiction, t.Connection, false);
        }

        public static List<Common.Models.Matters.Matter> ListChildren(
            Guid? parentId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            List<Common.Models.Matters.Matter> list = new List<Common.Models.Matters.Matter>();
            IEnumerable<DBOs.Matters.Matter> ie = null;

            conn = DataHelper.OpenIfNeeded(conn);

            if (parentId.HasValue)
                ie = conn.Query<DBOs.Matters.Matter>(
                    "SELECT \"matter\".* FROM \"matter\" " +
                    "WHERE \"matter\".\"utc_disabled\" is null  " +
                    "AND \"matter\".\"parent_id\"=@ParentId", new { ParentId = parentId.Value });
            else
                ie = conn.Query<DBOs.Matters.Matter>(
                    "SELECT \"matter\".* FROM \"matter\" " +
                    "WHERE \"matter\".\"utc_disabled\" is null  " +
                    "AND \"matter\".\"parent_id\" is null");

            DataHelper.Close(conn, closeConnection);

            foreach (DBOs.Matters.Matter dbo in ie)
                list.Add(Mapper.Map<Common.Models.Matters.Matter>(dbo));

            return list;
        }

        public static List<Common.Models.Matters.Matter> ListChildren(
            Transaction t,
            Guid? parentId)
        {
            return ListChildren(parentId, t.Connection, false);
        }

        public static List<Common.Models.Matters.Matter> ListChildrenForContact(
            Guid? parentId, 
            int contactId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            List<Common.Models.Matters.Matter> list = new List<Common.Models.Matters.Matter>();
            IEnumerable<DBOs.Matters.Matter> ie = null;

            conn = DataHelper.OpenIfNeeded(conn);

            if (parentId.HasValue)
                ie = conn.Query<DBOs.Matters.Matter>(
                    "SELECT \"matter\".* FROM \"matter\" " +
                    "WHERE \"matter\".\"utc_disabled\" is null  " +
                    "AND \"matter\".\"parent_id\"=@ParentId " + 
                    "AND \"matter\".\"id\" IN (SELECT \"matter_id\" FROM \"matter_contact\" WHERE \"contact_id\"=@ContactId)", 
                    new { ParentId = parentId.Value, ContactId = contactId });
            else
                ie = conn.Query<DBOs.Matters.Matter>(
                    "SELECT \"matter\".* FROM \"matter\" " +
                    "WHERE \"matter\".\"utc_disabled\" is null  " +
                    "AND \"matter\".\"parent_id\" is null " +
                    "AND \"matter\".\"id\" IN (SELECT \"matter_id\" FROM \"matter_contact\" WHERE \"contact_id\"=@ContactId)",
                    new { ContactId = contactId });

            DataHelper.Close(conn, closeConnection);

            foreach (DBOs.Matters.Matter dbo in ie)
                list.Add(Mapper.Map<Common.Models.Matters.Matter>(dbo));

            return list;
        }

        public static List<Common.Models.Matters.Matter> ListChildrenForContact(
            Transaction t,
            Guid? parentId,
            int contactId)
        {
            return ListChildrenForContact(parentId, contactId, t.Connection, false);
        }

        public static List<Common.Models.Matters.Matter> ListAllMattersForContact(
            int contactId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            List<Common.Models.Matters.Matter> list = new List<Common.Models.Matters.Matter>();
            IEnumerable<DBOs.Matters.Matter> ie = null;

            conn = DataHelper.OpenIfNeeded(conn);

            ie = conn.Query<DBOs.Matters.Matter>(
                "SELECT \"matter\".* FROM \"matter\" WHERE \"matter\".\"utc_disabled\" is null  " +
                "AND \"matter\".\"id\" IN (SELECT \"matter_id\" FROM \"matter_contact\" WHERE \"contact_id\"=@ContactId)",
                new { ContactId = contactId });

            DataHelper.Close(conn, closeConnection);

            foreach (DBOs.Matters.Matter dbo in ie)
                list.Add(Mapper.Map<Common.Models.Matters.Matter>(dbo));

            return list;
        }

        public static List<Common.Models.Matters.Matter> ListAllMattersForContact(
            Transaction t,
            int contactId)
        {
            return ListAllMattersForContact(contactId, t.Connection, false);
        }

        public static List<Common.Models.Notes.NoteTask> ListAllNotes(
            Common.Models.Matters.Matter model,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            List<Common.Models.Notes.NoteTask> list = new List<Common.Models.Notes.NoteTask>();
            IEnumerable<DBOs.Notes.NoteTask> ie = null;

            conn = DataHelper.OpenIfNeeded(conn);

            ie = conn.Query<DBOs.Notes.NoteTask>(
                "SELECT \"note_id\", \"task_id\", \"note\".\"id\" AS \"note_id\", \"note_task\".\"id\" AS \"id\" " +
                //\"note_task\".\"id\", \"note_task\".\"created_by_user_pid\", \"note_task\".\"modified_by_user_pid\", \"note_task\".\"disabled_by_user_pid\", " +
                //"\"note_task\".\"utc_created\", \"note_task\".\"utc_modified\", \"note_task\".\"utc_disabled\", \"note\".\"id\", \"note_task\".\"id\" 
                "FROM \"note_task\" " + 
                "FULL OUTER JOIN \"note\" ON \"note\".\"id\"=\"note_task\".\"note_id\" " +
                "WHERE \"task_id\" IN (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId) OR " +
	            "\"note\".\"id\" IN (SELECT \"note_id\" FROM \"note_matter\" WHERE \"matter_id\"=@MatterId) " +
                "ORDER BY \"note\".\"timestamp\" DESC", new { MatterId = model.Id });

            DataHelper.Close(conn, closeConnection);

            foreach (DBOs.Notes.NoteTask dbo in ie)
                list.Add(Mapper.Map<Common.Models.Notes.NoteTask>(dbo));

            return list;
        }

        public static List<Common.Models.Notes.NoteTask> ListAllNotes(
            Transaction t,
            Common.Models.Matters.Matter model)
        {
            return ListAllNotes(model, t.Connection, false);
        }

        public static List<Common.Models.Notes.NoteTask> ListAllTaskNotes(
            Common.Models.Matters.Matter model,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            List<Common.Models.Notes.NoteTask> list = new List<Common.Models.Notes.NoteTask>();
            IEnumerable<DBOs.Notes.NoteTask> ie = null;

            conn = DataHelper.OpenIfNeeded(conn);

            ie = conn.Query<DBOs.Notes.NoteTask>(
                "SELECT * FROM \"note_task\" JOIN \"note\" ON \"note_task\".\"note_id\"=\"note\".\"id\"  " +
                "WHERE \"note_task\".\"utc_disabled\" is null AND \"task_id\" IN (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId) " +
                "ORDER BY note.timestamp DESC",
                new { MatterId = model.Id });

            DataHelper.Close(conn, closeConnection);

            foreach (DBOs.Notes.NoteTask dbo in ie)
                list.Add(Mapper.Map<Common.Models.Notes.NoteTask>(dbo));

            return list;
        }

        public static List<Common.Models.Notes.NoteTask> ListAllTaskNotes(
            Transaction t,
            Common.Models.Matters.Matter model)
        {
            return ListAllTaskNotes(model, t.Connection, false);
        }

        public static Common.Models.Matters.Matter Create(
            Common.Models.Matters.Matter model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            // Matter
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Matters.Matter dbo = Mapper.Map<DBOs.Matters.Matter>(model);
        
            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("INSERT INTO \"matter\" (\"id\", \"matter_type_id\", \"title\", \"active\", \"parent_id\", \"synopsis\", " +
                "\"minimum_charge\", \"estimated_charge\", \"maximum_charge\", \"default_billing_rate_id\", \"billing_group_id\", \"override_matter_rate_with_employee_rate\", " +
                "\"utc_created\", \"utc_modified\", " +
                "\"created_by_user_pid\", \"modified_by_user_pid\", \"jurisdiction\", \"case_number\", \"lead_attorney_contact_id\", \"bill_to_contact_id\") " +
                "VALUES (@Id, @MatterTypeId, @Title, @Active, @ParentId, @Synopsis, @MinimumCharge, @EstimatedCharge, @MaximumCharge, @DefaultBillingRateId, @BillingGroupId, @OverrideMatterRateWithEmployeeRate, " +
                "@UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId, " +
                "@Jurisdiction, @CaseNumber, @LeadAttorneyContactId, @BillToContactId)",
                dbo);

            MatterContact.Create(new Common.Models.Matters.MatterContact()
            {
                Matter = model,
                Contact = new Common.Models.Contacts.Contact() { Id = dbo.LeadAttorneyContactId.Value },
                Role = "Lead Attorney"
            }, creator, conn, closeConnection);

            return model;
        }

        public static Common.Models.Matters.Matter Create(
            Transaction t,
            Common.Models.Matters.Matter model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Matters.Matter Edit(
            Common.Models.Matters.Matter model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            List<Common.Models.Matters.MatterContact> leadAttorneyMatches;
            DBOs.Matters.Matter dbo = Mapper.Map<DBOs.Matters.Matter>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"matter\" SET \"matter_type_id\"=@MatterTypeId, " +
                "\"title\"=@Title, \"active\"=@Active, \"parent_id\"=@ParentId, \"synopsis\"=@Synopsis, \"utc_modified\"=@UtcModified, " +
                "\"minimum_charge\"=@MinimumCharge, \"estimated_charge\"=@EstimatedCharge, \"maximum_charge\"=@MaximumCharge, " +
                "\"default_billing_rate_id\"=@DefaultBillingRateId, \"billing_group_id\"=@BillingGroupId, \"override_matter_rate_with_employee_rate\"=@OverrideMatterRateWithEmployeeRate, " +
                "\"modified_by_user_pid\"=@ModifiedByUserPId, \"jurisdiction\"=@Jurisdiction, \"case_number\"=@CaseNumber, \"lead_attorney_contact_id\"=@LeadAttorneyContactId, \"bill_to_contact_id\"=@BillToContactId " +
                "WHERE \"id\"=@Id", dbo);

            leadAttorneyMatches = MatterContact.ListForMatterByRole(dbo.Id, "Lead Attorney", conn, false);

            if (leadAttorneyMatches.Count > 1)
                throw new Exception("More than one Lead Attorney found.");
            else if (leadAttorneyMatches.Count < 1)
            {   // Insert only
                MatterContact.Create(new Common.Models.Matters.MatterContact()
                {
                    Matter = model,
                    Contact = new Common.Models.Contacts.Contact() { Id = dbo.LeadAttorneyContactId.Value },
                    Role = "Lead Attorney"
                }, modifier, conn, closeConnection);
            }
            else
            {   // Replace
                leadAttorneyMatches[0].Contact.Id = dbo.LeadAttorneyContactId.Value;
                MatterContact.Edit(leadAttorneyMatches[0], modifier, conn, closeConnection);
            }

            return model;
        }

        public static Common.Models.Matters.Matter Edit(
            Transaction t,
            Common.Models.Matters.Matter model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }
    }
}