// -----------------------------------------------------------------------
// <copyright file="Users.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Account
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
    public static class Users
    {
        public static Common.Models.Account.Users Get(
            Guid pid,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Account.Users, DBOs.Account.Users>(
                "SELECT * FROM \"Users\" WHERE \"pId\"=@PId",
                new { PId = pid }, conn, closeConnection);
        }

        public static Common.Models.Account.Users Get(
            Transaction t,
            Guid pid)
        {
            return Get(pid, t.Connection, false);
        }

        public static Common.Models.Account.Users Get(
            string username,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Account.Users, DBOs.Account.Users>(
                "SELECT * FROM \"Users\" WHERE \"Username\"=@Username",
                new { Username = username }, conn, closeConnection);
        }

        public static Common.Models.Account.Users Get(
            Transaction t,
            string username)
        {
            return Get(username, t.Connection, false);
        }

        public static List<Common.Models.Account.Users> List(
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Account.Users, DBOs.Account.Users>(
                "SELECT * FROM \"Users\"", null, conn, closeConnection);
        }

        public static List<Common.Models.Account.Users> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }
    }
}
