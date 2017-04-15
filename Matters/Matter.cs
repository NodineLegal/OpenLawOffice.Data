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
            int? courtTypeFilter,
            int? courtGeographicalJurisdictionFilter,
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

            if (active.HasValue)
            {
                sql += " AND \"active\"=@Active ";
            }
            if (!string.IsNullOrEmpty(contactFilter))
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
            if (courtTypeFilter.HasValue)
            {
                sql += " AND \"court_type_id\"=@CourtTypeFilter ";
            }
            if (courtGeographicalJurisdictionFilter.HasValue)
            {
                sql += " AND \"court_geographical_jurisdiction_id\"=@CourtGeographicalJurisdictionFilter ";
            }

            if (active.HasValue)
                return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(sql,
                    new
                    {
                        Active = active.Value,
                        ContactFilter = contactFilter,
                        TitleFilter = titleFilter,
                        CaseNumberFilter = caseNumberFilter,
                        CourtTypeFilter = courtTypeFilter,
                        CourtGeographicalJurisdictionFilter = courtGeographicalJurisdictionFilter
                    }, conn, closeConnection);
            else
                return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(sql,
                    new
                    {
                        ContactFilter = contactFilter,
                        TitleFilter = titleFilter,
                        CaseNumberFilter = caseNumberFilter,
                        CourtTypeFilter = courtTypeFilter,
                        CourtGeographicalJurisdictionFilter = courtGeographicalJurisdictionFilter
                    }, conn, closeConnection);
        }

        public static List<Common.Models.Matters.Matter> List(
            Transaction t,
            bool? active,
            string contactFilter,
            string titleFilter,
            string caseNumberFilter,
            int? courtTypeFilter,
            int? courtGeographicalJurisdictionFilter)
        {
            return List(active, contactFilter, titleFilter, caseNumberFilter, courtTypeFilter, courtGeographicalJurisdictionFilter, t.Connection, false);
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
                "\"attorney_for_party_title\", \"court_type_id\", \"court_geographical_jurisdiction_id\", \"court_sitting_in_city_id\", \"caption_plaintiff_or_subject_short\", " +
                "\"caption_plaintiff_or_subject_regular\", \"caption_plaintiff_or_subject_long\", \"caption_other_party_short\", " +
                "\"caption_other_party_regular\", \"caption_other_party_long\", " +
                "\"utc_created\", \"utc_modified\", " +
                "\"created_by_user_pid\", \"modified_by_user_pid\", \"case_number\", \"bill_to_contact_id\") " +
                "VALUES (@Id, @MatterTypeId, @Title, @Active, @ParentId, @Synopsis, @MinimumCharge, @EstimatedCharge, @MaximumCharge, @DefaultBillingRateId, @BillingGroupId, @OverrideMatterRateWithEmployeeRate, " +
                "@AttorneyForPartyTitle, @CourtTypeId, @CourtGeographicalJurisdictionId, @CourtSittingInCityId, @CaptionPlaintiffOrSubjectShort, " +
                "@CaptionPlaintiffOrSubjectRegular, @CaptionPlaintiffOrSubjectLong, @CaptionOtherPartyShort, @CaptionOtherPartyRegular, @CaptionOtherPartyLong, " +
                "@UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId, " +
                "@CaseNumber, @BillToContactId)",
                dbo);

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
            DBOs.Matters.Matter dbo = Mapper.Map<DBOs.Matters.Matter>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"matter\" SET \"matter_type_id\"=@MatterTypeId, " +
                "\"title\"=@Title, \"active\"=@Active, \"parent_id\"=@ParentId, \"synopsis\"=@Synopsis, \"utc_modified\"=@UtcModified, " +
                "\"minimum_charge\"=@MinimumCharge, \"estimated_charge\"=@EstimatedCharge, \"maximum_charge\"=@MaximumCharge, " +
                "\"default_billing_rate_id\"=@DefaultBillingRateId, \"billing_group_id\"=@BillingGroupId, \"override_matter_rate_with_employee_rate\"=@OverrideMatterRateWithEmployeeRate, " +
                "\"attorney_for_party_title\"=@AttorneyForPartyTitle, \"court_type_id\"=@CourtTypeId, \"court_geographical_jurisdiction_id\"=@CourtGeographicalJurisdictionId, " +
                "\"court_sitting_in_city_id\"=@CourtSittingInCityId, \"caption_plaintiff_or_subject_short\"=@CaptionPlaintiffOrSubjectShort, " +
                "\"caption_plaintiff_or_subject_regular\"=@CaptionPlaintiffOrSubjectRegular, \"caption_plaintiff_or_subject_long\"=@CaptionPlaintiffOrSubjectLong, " +
                "\"caption_other_party_short\"=@CaptionOtherPartyShort, " +
                "\"caption_other_party_regular\"=@CaptionOtherPartyRegular, \"caption_other_party_long\"=@CaptionOtherPartyLong, " +
                "\"modified_by_user_pid\"=@ModifiedByUserPId, \"case_number\"=@CaseNumber, \"bill_to_contact_id\"=@BillToContactId " +
                "WHERE \"id\"=@Id", dbo);
            
            return model;
        }

        public static Common.Models.Matters.Matter Edit(
            Transaction t,
            Common.Models.Matters.Matter model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static List<Common.Models.Matters.Matter> ListAllActiveMatters(
           IDbConnection conn = null,
           bool closeConnection = true)
        {
            List<Common.Models.Matters.Matter> list;

            conn = DataHelper.OpenIfNeeded(conn);

            list = DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null AND \"active\"=TRUE",
                null, conn, closeConnection);

            DataHelper.Close(conn, closeConnection);

            return list;
        }

        public static int CountAllActiveMatters(
           IDbConnection conn = null,
           bool closeConnection = true)
        {
            int val = -1;

            conn = DataHelper.OpenIfNeeded(conn);

            val = conn.Query<int>("SELECT COUNT(*) FROM \"matter\" WHERE \"utc_disabled\" is null AND \"active\"=TRUE").SingleOrDefault();

            DataHelper.Close(conn, closeConnection);

            return val;
        }

        public static List<Common.Models.Matters.Matter> ListAllMattersWithoutActiveTasks(
           IDbConnection conn = null,
           bool closeConnection = true)
        {
            List<Common.Models.Matters.Matter> list;

            conn = DataHelper.OpenIfNeeded(conn);

            list = DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null AND \"active\"=TRUE AND \"id\" NOT IN (SELECT \"matter_id\" FROM \"task\" JOIN \"task_matter\" ON \"task\".\"id\"=\"task_matter\".\"task_id\" WHERE \"active\"=TRUE)",
                null, conn, closeConnection);

            DataHelper.Close(conn, closeConnection);

            return list;
        }

        public static List<Common.Models.Matters.Matter> ListMattersWithoutActiveTasks(
            int? quantity = null,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            string sql = "";
            List<Common.Models.Matters.Matter> list;

            conn = DataHelper.OpenIfNeeded(conn);

            sql = "SELECT * FROM \"matter\" WHERE \"utc_disabled\" is null AND \"active\"=TRUE AND \"id\" NOT IN (SELECT \"matter_id\" FROM \"task\" JOIN \"task_matter\" ON \"task\".\"id\"=\"task_matter\".\"task_id\" WHERE \"active\"=TRUE) ORDER BY random() ";
            if (quantity != null && quantity.HasValue && quantity.Value > 0)
                sql += "limit @qty";
            list = DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(sql, new { qty = quantity }, conn, closeConnection);

            DataHelper.Close(conn, closeConnection);

            return list;
        }

        public static int CountAllMattersWithoutActiveTasks(
           IDbConnection conn = null,
           bool closeConnection = true)
        {
            int val = -1;

            conn = DataHelper.OpenIfNeeded(conn);

            val = conn.Query<int>("SELECT COUNT(*) FROM \"matter\" WHERE \"utc_disabled\" is null AND \"active\"=TRUE AND \"id\" NOT IN (SELECT \"matter_id\" FROM \"task\" JOIN \"task_matter\" ON \"task\".\"id\"=\"task_matter\".\"task_id\" WHERE \"active\"=TRUE)").SingleOrDefault();

            DataHelper.Close(conn, closeConnection);

            return val;
        }

        public static List<Tuple<int?, string, int>> CountListForMatterTypes(
           IDbConnection conn = null,
           bool closeConnection = true)
        {
            List<Tuple<int?, string, int>> ret = new List<Tuple<int?, string, int>>();

            conn = DataHelper.OpenIfNeeded(conn);

            using (Npgsql.NpgsqlCommand dbCommand = (Npgsql.NpgsqlCommand)conn.CreateCommand())
            {
                dbCommand.CommandText =
                    "SELECT	m.matter_type_id, t.title, COUNT(*) AS count FROM matter m LEFT JOIN matter_type t " +
                    "ON t.id = m.matter_type_id GROUP BY matter_type_id, t.title ORDER BY count DESC";
                
                try
                {
                    dbCommand.Prepare();

                    using (Npgsql.NpgsqlDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int? matterTypeId;

                            if (reader.IsDBNull(0))
                                matterTypeId = null;
                            else
                                matterTypeId = reader.GetInt32(0);

                            string title = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            int count = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);

                            ret.Add(new Tuple<int?, string, int>(matterTypeId, title, count));
                        }
                    }
                }
                catch (Npgsql.NpgsqlException e)
                {
                    System.Diagnostics.Trace.WriteLine(e.ToString());
                    throw;
                }
            }

            DataHelper.Close(conn, closeConnection);

            return ret;
        }
    }
}