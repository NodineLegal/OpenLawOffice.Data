// -----------------------------------------------------------------------
// <copyright file="FormFieldMatter.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    public static class FormFieldMatter
    {
        public static Common.Models.Forms.FormFieldMatter Get(
            Guid id,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Forms.FormFieldMatter, DBOs.Forms.FormFieldMatter>(
                "SELECT * FROM \"form_field_matter\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Forms.FormFieldMatter Get(
            Transaction t,
            Guid id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Forms.FormFieldMatter Get(
            Guid matterId, 
            int formFieldId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Forms.FormFieldMatter, DBOs.Forms.FormFieldMatter>(
                "SELECT * FROM \"form_field_matter\" WHERE \"matter_id\"=@MatterId AND \"form_field_id\"=@FormFieldId AND \"utc_disabled\" is null",
                new { MatterId = matterId, FormFieldId = formFieldId }, conn, closeConnection);
        }

        public static Common.Models.Forms.FormFieldMatter Get(
            Transaction t,
            Guid matterId,
            int formFieldId)
        {
            return Get(matterId, formFieldId, t.Connection, false);
        }

        public static List<Common.Models.Forms.FormFieldMatter> ListForMatter(
            Guid matterId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            List<Common.Models.Forms.FormFieldMatter> list;
            List<Common.Models.Forms.FormField> fieldList;

            fieldList = Data.Forms.FormField.List(conn, false);
            list = new List<Common.Models.Forms.FormFieldMatter>();

            fieldList.ForEach(x =>
            {
                Common.Models.Forms.FormFieldMatter ffm;

                ffm = Data.Forms.FormFieldMatter.Get(matterId, x.Id.Value, conn, false);

                if (ffm == null)
                    ffm = new Common.Models.Forms.FormFieldMatter() 
                    { 
                        FormField = x
                    };
                else
                    ffm.FormField = x;

                list.Add(ffm);
            });

            DataHelper.Close(conn, closeConnection);

            return list;
        }

        public static List<Common.Models.Forms.FormFieldMatter> ListForMatter(
            Transaction t,
            Guid matterId)
        {
            return ListForMatter(matterId, t.Connection, false);
        }

        public static Common.Models.Forms.FormFieldMatter Create(
            Common.Models.Forms.FormFieldMatter model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Forms.FormFieldMatter dbo = Mapper.Map<DBOs.Forms.FormFieldMatter>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("INSERT INTO \"form_field_matter\" (\"id\", \"matter_id\", \"form_field_id\", \"value\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @MatterId, @FormFieldId, @Value, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Forms.FormFieldMatter Create(
            Transaction t,
            Common.Models.Forms.FormFieldMatter model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Forms.FormFieldMatter Edit(
            Common.Models.Forms.FormFieldMatter model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Forms.FormFieldMatter dbo = Mapper.Map<DBOs.Forms.FormFieldMatter>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"form_field_matter\" SET " +
                "\"matter_id\"=@MatterId, \"form_field_id\"=@FormFieldId, \"value\"=@Value, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Forms.FormFieldMatter Edit(
            Transaction t,
            Common.Models.Forms.FormFieldMatter model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }
    }
}
