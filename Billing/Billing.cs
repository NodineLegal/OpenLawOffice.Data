// -----------------------------------------------------------------------
// <copyright file="Billing.cs" company="Nodine Legal, LLC">
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

    public static class Billing
    {
        public static Common.Models.Billing.Invoice SingleMatterBill(
            Common.Models.Billing.Invoice invoice,
            List<Common.Models.Billing.InvoiceExpense> invoiceExpenseList,
            List<Common.Models.Billing.InvoiceFee> invoiceFeeList,
            List<Common.Models.Billing.InvoiceTime> invoiceTimeList,
            Common.Models.Account.Users currentUser,
            Transaction trans)
        {
            decimal subtotal = 0;

            invoice = Data.Billing.Invoice.Create(trans, invoice, currentUser);

            invoiceExpenseList.ForEach(invoiceExpense =>
            {
                subtotal += invoiceExpense.Amount;
                Data.Billing.InvoiceExpense.Create(trans, invoiceExpense, currentUser);
            });

            invoiceFeeList.ForEach(invoiceFee =>
            {
                subtotal += invoiceFee.Amount;
                Data.Billing.InvoiceFee.Create(trans, invoiceFee, currentUser);
            });

            invoiceTimeList.ForEach(invoiceTime =>
            {
                subtotal += ((decimal)invoiceTime.Duration.TotalHours * invoiceTime.PricePerHour);
                Data.Billing.InvoiceTime.Create(trans, invoiceTime, currentUser);
            });

            invoice.Subtotal = subtotal;
            invoice.Total = invoice.Subtotal + invoice.TaxAmount;

            Data.Billing.Invoice.Edit(trans, invoice, currentUser);

            return invoice;
        }

        public static Common.Models.Billing.Invoice SingleGroupBill(
            Common.Models.Billing.BillingGroup billingGroup,
            Common.Models.Billing.Invoice invoice,
            List<Common.Models.Billing.InvoiceExpense> invoiceExpenseList,
            List<Common.Models.Billing.InvoiceFee> invoiceFeeList,
            List<Common.Models.Billing.InvoiceTime> invoiceTimeList,
            Common.Models.Account.Users currentUser,
            Transaction trans)
        {
            decimal subtotal = 0;

            invoice = Data.Billing.Invoice.Create(trans, invoice, currentUser);

            invoiceExpenseList.ForEach(invoiceExpense =>
            {
                subtotal += invoiceExpense.Amount;
                Data.Billing.InvoiceExpense.Create(trans, invoiceExpense, currentUser);
            });

            invoiceFeeList.ForEach(invoiceFee =>
            {
                subtotal += invoiceFee.Amount;
                Data.Billing.InvoiceFee.Create(trans, invoiceFee, currentUser);
            });

            invoiceTimeList.ForEach(invoiceTime =>
            {
                subtotal += ((decimal)invoiceTime.Duration.TotalHours * invoiceTime.PricePerHour);
                Data.Billing.InvoiceTime.Create(trans, invoiceTime, currentUser);
            });

            invoice.Subtotal = subtotal + billingGroup.Amount;
            invoice.Total = invoice.Subtotal + invoice.TaxAmount;

            Data.Billing.Invoice.Edit(trans, invoice, currentUser);

            billingGroup.LastRun = DateTime.Now;
            billingGroup.NextRun = billingGroup.NextRun;
            Data.Billing.BillingGroup.Edit(trans, billingGroup, currentUser);
            
            return invoice;
        }

        public static List<Tuple<int?, string, decimal>> ListTotalBillingsForContactsForLastYear(
           IDbConnection conn = null,
           bool closeConnection = true)
        {
            List<Tuple<int?, string, decimal>> ret = new List<Tuple<int?, string, decimal>>();

            conn = DataHelper.OpenIfNeeded(conn);

            using (Npgsql.NpgsqlCommand dbCommand = (Npgsql.NpgsqlCommand)conn.CreateCommand())
            {
                dbCommand.CommandText =
                    "SELECT bill_to_contact_id as id, (SELECT display_name FROM contact WHERE id=bill_to_contact_id) as contact, SUM(total) as total FROM invoice WHERE date > date - INTERVAL '1 year' GROUP BY bill_to_contact_id ORDER BY total DESC";

                try
                {
                    dbCommand.Prepare();

                    using (Npgsql.NpgsqlDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int? id;

                            if (reader.IsDBNull(0))
                                id = null;
                            else
                                id = reader.GetInt32(0);

                            string contact = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            decimal amount = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);

                            ret.Add(new Tuple<int?, string, decimal>(id, contact, amount));
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