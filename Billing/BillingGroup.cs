// -----------------------------------------------------------------------
// <copyright file="BillingGroup.cs" company="Nodine Legal, LLC">
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

    public static class BillingGroup
    {
        public static Common.Models.Billing.BillingGroup Get(
            int id,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Billing.BillingGroup, DBOs.Billing.BillingGroup>(
                "SELECT * FROM \"billing_group\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Billing.BillingGroup Get(
            Transaction t,
            int id)
        {
            return Get(id, t.Connection, false);
        }

        public static List<Common.Models.Billing.BillingGroup> List(
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Billing.BillingGroup, DBOs.Billing.BillingGroup>(
                "SELECT * FROM \"billing_group\" WHERE \"utc_disabled\" is null", null, conn, closeConnection);
        }

        public static List<Common.Models.Billing.BillingGroup> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static List<Common.Models.Billing.Invoice> ListInvoicesForGroup(
            int billingGroupId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Billing.Invoice, DBOs.Billing.Invoice>(
                "SELECT * FROM \"invoice\" WHERE \"billing_group_id\"=@BillingGroupId AND \"utc_disabled\" is null",
                new { BillingGroupId = billingGroupId }, conn, closeConnection);
        }

        public static List<Common.Models.Billing.Invoice> ListInvoicesForGroup(
            Transaction t,
            int billingGroupId)
        {
            return ListInvoicesForGroup(billingGroupId, t.Connection, false);
        }

        public static Common.Models.Billing.BillingGroup Create(
            Common.Models.Billing.BillingGroup model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Billing.BillingGroup dbo = Mapper.Map<DBOs.Billing.BillingGroup>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"billing_group\" (\"title\", \"last_run\", \"next_run\", \"amount\", \"bill_to_contact_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Title, @LastRun, @NextRun, @Amount, @BillToContactId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Billing.BillingGroup>("SELECT currval(pg_get_serial_sequence('billing_group', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Billing.BillingGroup Create(
            Transaction t,
            Common.Models.Billing.BillingGroup model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }

        public static Common.Models.Billing.BillingGroup Edit(
            Common.Models.Billing.BillingGroup model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Billing.BillingGroup dbo = Mapper.Map<DBOs.Billing.BillingGroup>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"billing_group\" SET " +
                "\"title\"=@Title, \"last_run\"=@LastRun, \"next_run\"=@NextRun, \"amount\"=@Amount, \"bill_to_contact_id\"=@BillToContactId, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Billing.BillingGroup Edit(
            Transaction t,
            Common.Models.Billing.BillingGroup model,
            Common.Models.Account.Users modifier)
        {
            return Edit(model, modifier, t.Connection, false);
        }

        public static List<Common.Models.Matters.Matter> ListMattersForGroup(
            int billingGroupId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"billing_group_id\"=@BillingGroupId AND \"utc_disabled\" is null",
                new { BillingGroupId = billingGroupId }, conn, closeConnection);
        }

        public static List<Common.Models.Matters.Matter> ListMattersForGroup(
            Transaction t,
            int billingGroupId)
        {
            return ListMattersForGroup(billingGroupId, t.Connection, false);
        }

        public static decimal SumExpensesForGroup(
            int billingGroupId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            conn = DataHelper.OpenIfNeeded(conn);

            IEnumerable<dynamic> result = conn.Query("SELECT SUM(\"amount\") AS \"Amount\" FROM \"expense\" WHERE \"id\" IN " +
                "(SELECT \"expense_id\" FROM \"expense_matter\" WHERE \"matter_id\" IN " +
                    "(SELECT \"id\" FROM \"matter\" WHERE \"billing_group_id\"=@BillingGroupId) " +
                ")", new { BillingGroupId = billingGroupId });

            IEnumerator<dynamic> enumerator = result.GetEnumerator();

            DataHelper.Close(conn, closeConnection);

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Amount == null)
                    return 0;

                return enumerator.Current.Amount;
            }
            
            return 0;
        }

        public static decimal SumExpensesForGroup(
            Transaction t,
            int billingGroupId)
        {
            return SumExpensesForGroup(billingGroupId, t.Connection, false);
        }

        public static decimal SumBillableExpensesForGroup(
            int billingGroupId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            conn = DataHelper.OpenIfNeeded(conn);

            IEnumerable<dynamic> result = conn.Query("SELECT SUM(\"amount\") AS \"Amount\" FROM \"expense\" WHERE \"id\" IN " +
                "(SELECT \"expense_id\" FROM \"expense_matter\" WHERE \"matter_id\" IN " +
                    "(SELECT \"id\" FROM \"matter\" WHERE \"billing_group_id\"=@BillingGroupId) " +
                "AND \"expense_id\" NOT IN " +
                    "(SELECT \"expense_id\" FROM \"invoice_expense\" WHERE \"utc_disabled\" is NULL) " +
                ")", new { BillingGroupId = billingGroupId });

            IEnumerator<dynamic> enumerator = result.GetEnumerator();

            DataHelper.Close(conn, closeConnection);

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Amount == null)
                    return 0;

                return enumerator.Current.Amount;
            }

            return 0;
        }

        public static decimal SumBillableExpensesForGroup(
            Transaction t,
            int billingGroupId)
        {
            return SumBillableExpensesForGroup(billingGroupId, t.Connection, false);
        }

        public static decimal SumFeesForGroup(
            int billingGroupId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            conn = DataHelper.OpenIfNeeded(conn);

            IEnumerable<dynamic> result = conn.Query("SELECT SUM(\"amount\") AS \"Amount\" FROM \"fee\" WHERE \"id\" IN " +
                "(SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\" IN " +
                    "(SELECT \"id\" FROM \"matter\" WHERE \"billing_group_id\"=@BillingGroupId) " +
                ")", new { BillingGroupId = billingGroupId });

            IEnumerator<dynamic> enumerator = result.GetEnumerator();

            DataHelper.Close(conn, closeConnection);

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Amount == null)
                    return 0;

                return enumerator.Current.Amount;
            }

            return 0;
        }

        public static decimal SumFeesForGroup(
            Transaction t,
            int billingGroupId)
        {
            return SumFeesForGroup(billingGroupId, t.Connection, false);
        }

        public static decimal SumBillableFeesForGroup(
            int billingGroupId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            conn = DataHelper.OpenIfNeeded(conn);

            IEnumerable<dynamic> result = conn.Query("SELECT SUM(\"amount\") AS \"Amount\" FROM \"fee\" WHERE \"id\" IN " +
                "(SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\" IN " +
                    "(SELECT \"id\" FROM \"matter\" WHERE \"billing_group_id\"=@BillingGroupId) " +
                "AND \"fee_id\" NOT IN " +
                    "(SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"utc_disabled\" is NULL) " +
                ")", new { BillingGroupId = billingGroupId });

            IEnumerator<dynamic> enumerator = result.GetEnumerator();

            DataHelper.Close(conn, closeConnection);

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Amount == null)
                    return 0;

                return enumerator.Current.Amount;
            }

            return 0;
        }

        public static decimal SumBillableFeesForGroup(
            Transaction t,
            int billingGroupId)
        {
            return SumBillableFeesForGroup(billingGroupId, t.Connection, false);
        }
    }
}
