﻿// -----------------------------------------------------------------------
// <copyright file="OpportunityContact.cs" company="Nodine Legal, LLC">
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
    public class OpportunityContact : Core
    {
        [ColumnMapping(Name = "id")]
        public long Id { get; set; }

        [ColumnMapping(Name = "opportunity_id")]
        public long OpportunityId { get; set; }

        [ColumnMapping(Name = "contact_id")]
        public int ContactId { get; set; }
        
        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(OpportunityContact), new ColumnAttributeTypeMapper<OpportunityContact>());
            Mapper.CreateMap<DBOs.Opportunities.OpportunityContact, Common.Models.Opportunities.OpportunityContact>()
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
                .ForMember(dst => dst.Opportunity, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Opportunities.Opportunity()
                    {
                        Id = db.OpportunityId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Contact, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.ContactId,
                        IsStub = true
                    };
                }));

            Mapper.CreateMap<Common.Models.Opportunities.OpportunityContact, DBOs.Opportunities.OpportunityContact>()
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
                .ForMember(dst => dst.OpportunityId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Opportunity == null || !model.Opportunity.Id.HasValue)
                        throw new Exception("Opportunity cannot be null");
                    return model.Opportunity.Id.Value;
                }))
                .ForMember(dst => dst.ContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Contact == null)
                        return null;
                    return model.Contact.Id;
                }));
        }
    }
}
