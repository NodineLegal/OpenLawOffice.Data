// -----------------------------------------------------------------------
// <copyright file="BillingRate.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    public static class BillingRate
    {
        public static Common.Models.Billing.BillingRate Get(
            int id,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Billing.BillingRate, DBOs.Billing.BillingRate>(
                "SELECT * FROM \"billing_rate\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Billing.BillingRate Get(
            Transaction t,
            int id)
        {
            return Get(id, t.Connection, false);
        }

        public static List<Common.Models.Billing.BillingRate> List(
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Billing.BillingRate, DBOs.Billing.BillingRate>(
                "SELECT * FROM \"billing_rate\" WHERE \"utc_disabled\" is null", null, conn, closeConnection);
        }

        public static List<Common.Models.Billing.BillingRate> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static Common.Models.Billing.BillingRate Create(
            Common.Models.Billing.BillingRate model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Billing.BillingRate dbo = Mapper.Map<DBOs.Billing.BillingRate>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"billing_rate\" (\"title\", \"price_per_unit\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Title, @PricePerUnit, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Billing.BillingRate>("SELECT currval(pg_get_serial_sequence('billing_rate', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Billing.BillingRate Create(
            Transaction t,
            Common.Models.Billing.BillingRate model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Billing.BillingRate Edit(
            Common.Models.Billing.BillingRate model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Billing.BillingRate dbo = Mapper.Map<DBOs.Billing.BillingRate>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"billing_rate\" SET " +
                "\"title\"=@Title, \"price_per_unit\"=@PricePerUnit, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Billing.BillingRate Edit(
            Transaction t,
            Common.Models.Billing.BillingRate model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }
    }
}