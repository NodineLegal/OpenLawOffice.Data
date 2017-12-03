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

namespace OpenLawOffice.Data.DBOs.Leads
{
    using System;
    using AutoMapper;

    [Common.Models.MapMe]
    public class LeadFee : Core
    {
        [ColumnMapping(Name = "id")]
        public int? Id { get; set; }

        [ColumnMapping(Name = "is_eligible")]
        public bool? IsEligible { get; set; }

        [ColumnMapping(Name = "amount")]
        public decimal? Amount { get; set; }

        [ColumnMapping(Name = "to_id")]
        public int? ToId { get; set; }

        [ColumnMapping(Name = "paid")]
        public DateTime? Paid { get; set; }

        [ColumnMapping(Name = "additional_data")]
        public string AdditionalData { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(LeadFee), new ColumnAttributeTypeMapper<LeadFee>());
            Mapper.CreateMap<LeadFee, Common.Models.Leads.LeadFee>()
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
                .ForMember(dst => dst.IsEligible, opt => opt.MapFrom(src => src.IsEligible))
                .ForMember(dst => dst.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dst => dst.To, opt => opt.ResolveUsing(db =>
                {
                    if (!db.ToId.HasValue) return null;
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.ToId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Paid, opt => opt.MapFrom(src => src.Paid))
                .ForMember(dst => dst.AdditionalData, opt => opt.MapFrom(src => src.AdditionalData));

            Mapper.CreateMap<Common.Models.Leads.LeadFee, LeadFee>()
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
                .ForMember(dst => dst.IsEligible, opt => opt.MapFrom(src => src.IsEligible))
                .ForMember(dst => dst.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dst => dst.ToId, opt => opt.ResolveUsing(model =>
                {
                    if (model.To == null) return null;
                    return model.To.Id;
                }))
                .ForMember(dst => dst.Paid, opt => opt.MapFrom(src => src.Paid))
                .ForMember(dst => dst.AdditionalData, opt => opt.MapFrom(src => src.AdditionalData));
        }
    }
}
