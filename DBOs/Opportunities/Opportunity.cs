// -----------------------------------------------------------------------
// <copyright file="Opportunity.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Opportunities
{
    using System;
    using AutoMapper;

    [Common.Models.MapMe]
    public class Opportunity : Core
    {
        [ColumnMapping(Name = "id")]
        public long? Id { get; set; }

        [ColumnMapping(Name = "account_id")]
        public int? AccountId { get; set; }

        [ColumnMapping(Name = "stage_id")]
        public int? StageId { get; set; }

        [ColumnMapping(Name = "probability")]
        public decimal? Probability { get; set; }

        [ColumnMapping(Name = "amount")]
        public decimal? Amount { get; set; }

        [ColumnMapping(Name = "closed")]
        public DateTime? Closed { get; set; }

        [ColumnMapping(Name = "lead_id")]
        public int? LeadId { get; set; }

        [ColumnMapping(Name = "matter_id")]
        public Guid? MatterId { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Opportunity), new ColumnAttributeTypeMapper<Opportunity>());
            Mapper.CreateMap<Opportunity, Common.Models.Opportunities.Opportunity>()
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
                .ForMember(dst => dst.Account, opt => opt.ResolveUsing(db =>
                {
                    if (!db.AccountId.HasValue) return null;
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.AccountId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Contacts, opt => opt.Ignore())
                .ForMember(dst => dst.Stage, opt => opt.ResolveUsing(db =>
                {
                    if (!db.StageId.HasValue) return null;
                    return new Common.Models.Opportunities.OpportunityStage()
                    {
                        Id = db.StageId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Probability, opt => opt.MapFrom(src => src.Probability))
                .ForMember(dst => dst.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dst => dst.Closed, opt => opt.MapFrom(src => src.Closed))
                .ForMember(dst => dst.Lead, opt => opt.ResolveUsing(db =>
                {
                    if (!db.LeadId.HasValue) return null;
                    return new Common.Models.Leads.Lead()
                    {
                        Id = db.LeadId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Matter, opt => opt.ResolveUsing(db =>
                {
                    if (!db.MatterId.HasValue) return null;
                    return new Common.Models.Matters.Matter()
                    {
                        Id = db.MatterId.Value,
                        IsStub = true
                    };
                }));

            Mapper.CreateMap<Common.Models.Opportunities.Opportunity, Opportunity>()
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
                .ForMember(dst => dst.AccountId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Account == null) return null;
                    return model.Account.Id;
                }))
                .ForMember(dst => dst.StageId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Stage == null) return null;
                    return model.Stage.Id;
                }))
                .ForMember(dst => dst.Probability, opt => opt.MapFrom(src => src.Probability))
                .ForMember(dst => dst.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dst => dst.Closed, opt => opt.MapFrom(src => src.Closed))
                .ForMember(dst => dst.LeadId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Lead == null) return null;
                    return model.Lead.Id;
                }))
                .ForMember(dst => dst.MatterId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Matter == null) return null;
                    return model.Matter.Id;
                }));
        }
    }
}
