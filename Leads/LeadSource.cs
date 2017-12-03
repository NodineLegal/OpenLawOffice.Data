// -----------------------------------------------------------------------
// <copyright file="LeadSource.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Leads
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using AutoMapper;
    using Dapper;
    using System.Linq;

    public class LeadSource
    {
        public static Common.Models.Leads.LeadSource Get(
            int id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Leads.LeadSource, DBOs.Leads.LeadSource>(
                "SELECT * FROM \"lead_source\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Leads.LeadSource Get(
            Transaction t,
            int id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Leads.LeadSource Create(
            Common.Models.Leads.LeadSource model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Leads.LeadSource dbo = Mapper.Map<DBOs.Leads.LeadSource>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"lead_source\" (\"type_id\", \"contact_id\", \"title\", \"additional_question_1\", \"additional_data_1\", \"additional_question_2\", \"additional_data_2\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@TypeId, @ContactId, @Title, @AdditionalQuestion1, @AdditionalData1, @AdditionalQuestion2, @AdditionalData2, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Leads.LeadSource>("SELECT currval(pg_get_serial_sequence('lead_source', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Leads.LeadSource Create(
            Transaction t,
            Common.Models.Leads.LeadSource model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Leads.LeadSource Edit(
            Common.Models.Leads.LeadSource model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Leads.Lead dbo = Mapper.Map<DBOs.Leads.Lead>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"lead_source\" SET " +
                "\"type_id\"=@TypeId, \"contact_id\"=@ContactId, \"title\"=@Title, \"additional_question_1\"=@AdditionalQuestion1, \"additional_data_1\"=@AdditionalData1, \"additional_question_1\"=@AdditionalQuestion2, \"additional_data_2\"=@AdditionalData2, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Leads.LeadSource Edit(
            Transaction t,
            Common.Models.Leads.LeadSource model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static List<Common.Models.Leads.LeadSource> List(
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Leads.LeadSource, DBOs.Leads.LeadSource>(
                "SELECT * FROM \"lead_source\" WHERE \"utc_disabled\" is null ORDER BY \"title\" ASC",
                null, conn, closeConnection);
        }

        public static List<Common.Models.Leads.LeadSource> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static List<Common.Models.Leads.LeadSource> List(
            string title,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            title = title.ToLower();

            return DataHelper.List<Common.Models.Leads.LeadSource, DBOs.Leads.LeadSource>(
                "SELECT * FROM \"lead_source\" WHERE \"utc_disabled\" is null AND " +
                "LOWER(\"title\") LIKE '%' || @Title || '%' ORDER BY \"title\" ASC",
                new { Title = title }, conn, closeConnection);
        }

        public static List<Common.Models.Leads.LeadSource> List(
            Transaction t,
            string title)
        {
            return List(title, t.Connection, false);
        }
    }
}