// -----------------------------------------------------------------------
// <copyright file="Document.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Documents
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
    public static class Document
    {
        public static Common.Models.Documents.Document Get(Guid id,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Documents.Document, DBOs.Documents.Document>(
                "SELECT * FROM \"document\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                new { Id = id }, conn, closeConnection);
        }

        public static Common.Models.Matters.Matter GetMatter(Guid documentId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT \"matter\".* FROM \"matter\" JOIN \"document_matter\" ON " +
                "\"matter\".\"id\"=\"document_matter\".\"matter_id\" " +
                "WHERE \"document_matter\".\"document_id\"=@DocumentId " +
                "AND \"matter\".\"utc_disabled\" is null " +
                "AND \"document_matter\".\"utc_disabled\" is null",
                new { DocumentId = documentId }, conn, closeConnection);
        }

        public static Common.Models.Tasks.Task GetTask(Guid documentId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tasks.Task, DBOs.Tasks.Task>(
                "SELECT \"task\".* FROM \"task\" JOIN \"document_task\" ON " +
                "\"task\".\"id\"=\"document_task\".\"task_id\" " +
                "WHERE \"document_task\".\"document_id\"=@DocumentId " +
                "AND \"task\".\"utc_disabled\" is null " +
                "AND \"document_task\".\"utc_disabled\" is null",
                new { DocumentId = documentId }, conn, closeConnection);
        }

        public static List<Common.Models.Documents.Document> ListForMatter(Guid matterId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Documents.Document, DBOs.Documents.Document>(
                "SELECT \"document\".* FROM \"document\" JOIN \"document_matter\" ON " +
                "\"document\".\"id\"=\"document_matter\".\"document_id\" " +
                "WHERE \"document_matter\".\"matter_id\"=@MatterId " +
                "AND \"document\".\"utc_disabled\" is null " +
                "AND \"document_matter\".\"utc_disabled\" is null " +
                "ORDER BY \"document\".\"date\" DESC, \"document\".\"title\" ASC",
                new { MatterId = matterId }, conn, closeConnection);
        }

        public static List<Common.Models.Documents.Document> ListForTask(long taskId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Documents.Document, DBOs.Documents.Document>(
                "SELECT \"document\".* FROM \"document\" JOIN \"document_task\" ON " +
                "\"document\".\"id\"=\"document_task\".\"document_id\" " +
                "WHERE \"document_task\".\"task_id\"=@TaskId " +
                "AND \"document\".\"utc_disabled\" is null " +
                "AND \"document_task\".\"utc_disabled\" is null " +
                "ORDER BY \"document\".\"date\" DESC, \"document\".\"title\" ASC",
                new { TaskId = taskId }, conn, closeConnection);
        }

        public static Common.Models.Documents.Document Create(Common.Models.Documents.Document model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Documents.Document dbo = Mapper.Map<DBOs.Documents.Document>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"document\" (\"id\", \"date\", \"title\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @Date, @Title, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Documents.Document>("SELECT currval(pg_get_serial_sequence('document', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Documents.DocumentMatter RelateMatter(Common.Models.Documents.Document model,
            Guid matterId, Common.Models.Account.Users creator,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DocumentMatter.Create(new Common.Models.Documents.DocumentMatter()
            {
                Id = Guid.NewGuid(),
                Document = model,
                Matter = new Common.Models.Matters.Matter() { Id = matterId },
                CreatedBy = creator,
                ModifiedBy = creator,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            }, creator, conn, closeConnection);
        }

        public static Common.Models.Documents.DocumentTask RelateTask(Common.Models.Documents.Document model,
            long taskId, Common.Models.Account.Users creator,
            IDbConnection conn = null, bool closeConnection = true)
        {
            Common.Models.Tasks.TaskMatter taskMatter = Tasks.TaskMatter.GetFor(taskId);

            RelateMatter(model, taskMatter.Matter.Id.Value, creator);

            return DocumentTask.Create(new Common.Models.Documents.DocumentTask()
            {
                Id = Guid.NewGuid(),
                Document = model,
                Task = new Common.Models.Tasks.Task { Id = taskId },
                CreatedBy = creator,
                ModifiedBy = creator,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            }, creator, conn, closeConnection);
        }

        public static Common.Models.Documents.Document Edit(Common.Models.Documents.Document model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Documents.Document dbo = Mapper.Map<DBOs.Documents.Document>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"document\" SET " +
                "\"date\"=@Date, \"title\"=@Title, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Documents.Version GetCurrentVersion(Guid documentId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Documents.Version, DBOs.Documents.Version>(
                "SELECT * FROM \"version\" WHERE \"document_id\"=@DocumentId AND \"utc_disabled\" is null ORDER BY \"version_number\" DESC LIMIT 1",
                new { DocumentId = documentId }, conn, closeConnection);
        }

        public static List<Common.Models.Documents.Version> GetVersions(Guid documentId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Documents.Version, DBOs.Documents.Version>(
                "SELECT * FROM \"version\" WHERE \"document_id\"=@DocumentId AND \"utc_disabled\" is null ORDER BY \"version_number\" DESC",
                new { DocumentId = documentId }, conn, closeConnection);
        }

        public static Common.Models.Documents.Version CreateNewVersion(Guid documentId,
            Common.Models.Documents.Version model, Common.Models.Account.Users creator,
            IDbConnection conn = null, bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;

            Common.Models.Documents.Version currentVersion = GetCurrentVersion(documentId);
            if (currentVersion == null)
                model.VersionNumber = 1;
            else
                model.VersionNumber = currentVersion.VersionNumber + 1;

            DBOs.Documents.Version dbo = Mapper.Map<DBOs.Documents.Version>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"version\" (\"id\", \"document_id\", \"version_number\", \"mime\", \"filename\", " +
                "\"extension\", \"size\", \"md5\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @DocumentId, @VersionNumber, @Mime, @Filename, @Extension, @Size, @Md5, " +
                "@UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Documents.Version>("SELECT currval(pg_get_serial_sequence('version', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }
    }
}