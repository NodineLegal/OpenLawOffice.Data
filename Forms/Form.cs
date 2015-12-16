// -----------------------------------------------------------------------
// <copyright file="Form.cs" company="Nodine Legal, LLC">
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

    public class Form
    {
        public static Common.Models.Forms.Form Get(
            int id,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Forms.Form, DBOs.Forms.Form>(
                "SELECT * FROM \"form\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Forms.Form Get(
            Transaction t,
            int id)
        {
            return Get(id, t.Connection, false);
        }

        public static List<Common.Models.Forms.Form> List(
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Forms.Form, DBOs.Forms.Form>(
                "SELECT * FROM \"form\" WHERE \"utc_disabled\" is null", null, conn, closeConnection);
        }

        public static List<Common.Models.Forms.Form> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static List<Common.Models.Forms.Form> ListForMatter(
            Guid matterId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Forms.Form, DBOs.Forms.Form>(
                "SELECT * FROM \"form\" WHERE \"matter_type_id\"=(SELECT \"matter_type_id\" FROM \"matter\" WHERE \"id\"=@MatterId) AND \"utc_disabled\" is null",
                new { MatterId = matterId }, conn, closeConnection);
        }

        public static List<Common.Models.Forms.Form> ListForMatter(
            Transaction t,
            Guid matterId)
        {
            return ListForMatter(matterId, t.Connection, false);
        }

        public static Common.Models.Forms.Form Create(
            Common.Models.Forms.Form model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Forms.Form dbo = Mapper.Map<DBOs.Forms.Form>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"form\" (\"title\", \"matter_type_id\", \"path\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Title, @MatterTypeId, @Path, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Forms.Form>("SELECT currval(pg_get_serial_sequence('form', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Forms.Form Create(
            Transaction t,
            Common.Models.Forms.Form model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Forms.Form Edit(
            Common.Models.Forms.Form model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Forms.Form dbo = Mapper.Map<DBOs.Forms.Form>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"form\" SET " +
                "\"title\"=@Title, \"matter_type_id\"=@MatterTypeId, \"path\"=@Path, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Forms.Form Edit(
            Transaction t,
            Common.Models.Forms.Form model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static Common.Models.Forms.Form Disable(
            Common.Models.Forms.Form model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Forms.Form,
                DBOs.Forms.Form>("form", disabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Forms.Form Disable(
            Transaction t,
            Common.Models.Forms.Form model,
            Common.Models.Account.Users disabler)
        {
            return Disable(model, disabler, t.Connection, false);
        }
    }
}
