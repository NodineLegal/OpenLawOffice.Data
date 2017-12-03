// -----------------------------------------------------------------------
// <copyright file="LeadFee.cs" company="Nodine Legal, LLC">
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

    public class LeadFee
    {
        public static Common.Models.Leads.LeadFee Get(
            int id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Leads.LeadFee, DBOs.Leads.LeadFee>(
                "SELECT * FROM \"lead_fee\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Leads.LeadFee Get(
            Transaction t,
            int id)
        {
            return Get(id, t.Connection, false);
        }

        public static Common.Models.Leads.LeadFee Create(
            Common.Models.Leads.LeadFee model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Leads.LeadFee dbo = Mapper.Map<DBOs.Leads.LeadFee>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"lead_fee\" (\"is_eligible\", \"amount\", \"to_id\", \"paid\", \"additional_data\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@IsEligible, @Amount, @ToId, @Paid, @AdditionalData, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Leads.LeadFee>("SELECT currval(pg_get_serial_sequence('lead_fee', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Leads.LeadFee Create(
            Transaction t,
            Common.Models.Leads.LeadFee model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Leads.LeadFee Edit(
            Common.Models.Leads.LeadFee model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Leads.LeadFee dbo = Mapper.Map<DBOs.Leads.LeadFee>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"lead_fee\" SET " +
                "\"is_eligible\"=@IsEligible, \"amount\"=@Amount, \"to_id\"=@ToId, \"paid\"=@Paid, \"additional_data\"=@AdditionalData, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Leads.LeadFee Edit(
            Transaction t,
            Common.Models.Leads.LeadFee model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }
    }
}
