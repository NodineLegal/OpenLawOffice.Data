// -----------------------------------------------------------------------
// <copyright file="Lead.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Leads
{
    using System;
    using AutoMapper;

    [Common.Models.MapMe]
    public class Lead : Core
    {
        [ColumnMapping(Name = "id")]
        public long? Id { get; set; }

        [ColumnMapping(Name = "status_id")]
        public int? StatusId { get; set; }

        [ColumnMapping(Name = "contact_id")]
        public int? ContactId { get; set; }

        [ColumnMapping(Name = "source_id")]
        public int? SourceId { get; set; }

        [ColumnMapping(Name = "fee_id")]
        public int? FeeId { get; set; }

        [ColumnMapping(Name = "closed")]
        public DateTime? Closed { get; set; }

        [ColumnMapping(Name = "details")]
        public string Details { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Lead), new ColumnAttributeTypeMapper<Lead>());
            Mapper.CreateMap<Lead, Common.Models.Leads.Lead>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.Created, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcCreated.ToSystemTime();
                }))
                .ForMember(dst => dst.Modified, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcModified.ToSystemTime();
                }))
                .ForMember(dst => dst.Disabled, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcDisabled.ToSystemTime();
                }))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Account.Users()
                    {
                        PId = db.CreatedByUserPId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Account.Users()
                    {
                        PId = db.ModifiedByUserPId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (!db.DisabledByUserPId.HasValue) return null;
                    return new Common.Models.Account.Users()
                    {
                        PId = db.DisabledByUserPId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Status, opt => opt.ResolveUsing(db =>
                {
                    if (!db.StatusId.HasValue) return null;
                    return new Common.Models.Leads.LeadStatus()
                    {
                        Id = db.StatusId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Contact, opt => opt.ResolveUsing(db =>
                {
                    if (!db.ContactId.HasValue) return null;
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.ContactId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Source, opt => opt.ResolveUsing(db =>
                {
                    if (!db.SourceId.HasValue) return null;
                    return new Common.Models.Leads.LeadSource()
                    {
                        Id = db.SourceId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Fee, opt => opt.ResolveUsing(db =>
                {
                    if (!db.FeeId.HasValue) return null;
                    return new Common.Models.Leads.LeadFee()
                    {
                        Id = db.FeeId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Closed, opt => opt.ResolveUsing(db =>
                {
                    return db.Closed.ToSystemTime();
                }))
                .ForMember(dst => dst.Details, opt => opt.MapFrom(src => src.Details));

            Mapper.CreateMap<Common.Models.Leads.Lead, Lead>()
                .ForMember(dst => dst.UtcCreated, opt => opt.ResolveUsing(db =>
                {
                    return db.Created.ToDbTime();
                }))
                .ForMember(dst => dst.UtcModified, opt => opt.ResolveUsing(db =>
                {
                    return db.Modified.ToDbTime();
                }))
                .ForMember(dst => dst.UtcDisabled, opt => opt.ResolveUsing(db =>
                {
                    return db.Disabled.ToDbTime();
                }))
                .ForMember(dst => dst.CreatedByUserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.CreatedBy == null || !model.CreatedBy.PId.HasValue)
                        return Guid.Empty;
                    return model.CreatedBy.PId.Value;
                }))
                .ForMember(dst => dst.ModifiedByUserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.ModifiedBy == null || !model.ModifiedBy.PId.HasValue)
                        return Guid.Empty;
                    return model.ModifiedBy.PId.Value;
                }))
                .ForMember(dst => dst.DisabledByUserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.DisabledBy == null) return null;
                    return model.DisabledBy.PId;
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.StatusId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Status == null) return null;
                    return model.Status.Id;
                }))
                .ForMember(dst => dst.ContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Contact == null) return null;
                    return model.Contact.Id;
                }))
                .ForMember(dst => dst.SourceId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Source == null) return null;
                    return model.Source.Id;
                }))
                .ForMember(dst => dst.FeeId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Fee == null) return null;
                    return model.Fee.Id;
                }))
                .ForMember(dst => dst.Closed, opt => opt.ResolveUsing(db =>
                {
                    return db.Closed.ToDbTime();
                }))
                .ForMember(dst => dst.Details, opt => opt.MapFrom(src => src.Details));
        }
    }
}
