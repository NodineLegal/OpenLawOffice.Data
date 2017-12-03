// -----------------------------------------------------------------------
// <copyright file="ActivityRegardingType.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    public static class ActivityRegardingType
    {
        public static Common.Models.Activities.ActivityRegardingType Get(
            int id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Activities.ActivityRegardingType, DBOs.Activities.ActivityRegardingType>(
                "SELECT * FROM \"activity_regarding_type\" WHERE \"id\"=@id",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Activities.ActivityRegardingType Get(
            Transaction t,
            int id)
        {
            return Get(id, t.Connection, false);
        }

        public static List<Common.Models.Activities.ActivityRegardingType> List(
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Activities.ActivityRegardingType, DBOs.Activities.ActivityRegardingType>(
                "SELECT * FROM \"activity_regarding_type\" ORDER BY \"order\" ASC", null, conn, closeConnection);
        }

        public static List<Common.Models.Activities.ActivityRegardingType> List(
            Transaction t)
        {
            return List(t.Connection, false);
        }

        public static Common.Models.Activities.ActivityRegardingType GetFromActivityId(
            long id,
            IDbConnection conn = null,
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Activities.ActivityRegardingType, DBOs.Activities.ActivityRegardingType>(
                "SELECT * FROM \"activity_regarding_type\" WHERE \"id\" IN (SELECT \"type\" FROM \"activity_regarding_base\" WHERE \"activity\"=@id)",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Activities.ActivityRegardingType GetFromActivityId(
            Transaction t,
            long id)
        {
            return GetFromActivityId(id, t.Connection, false);
        }
    }
}
