// -----------------------------------------------------------------------
// <copyright file="LeadSource.cs" company="Nodine Legal, LLC">
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
    public class LeadSource : Core
    {
        [ColumnMapping(Name = "id")]
        public int? Id { get; set; }

        [ColumnMapping(Name = "type_id")]
        public int? TypeId { get; set; }

        [ColumnMapping(Name = "contact_id")]
        public int? ContactId { get; set; }

        [ColumnMapping(Name = "title")]
        public string Title { get; set; }

        [ColumnMapping(Name = "additional_question_1")]
        public string AdditionalQuestion1 { get; set; }

        [ColumnMapping(Name = "additional_data_1")]
        public string AdditionalData1 { get; set; }

        [ColumnMapping(Name = "additional_question_2")]
        public string AdditionalQuestion2 { get; set; }

        [ColumnMapping(Name = "additional_data_2")]
        public string AdditionalData2 { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(LeadSource), new ColumnAttributeTypeMapper<LeadSource>());
            Mapper.CreateMap<LeadSource, Common.Models.Leads.LeadSource>()
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
                .ForMember(dst => dst.Type, opt => opt.ResolveUsing(db =>
                {
                    if (!db.TypeId.HasValue) return null;
                    return new Common.Models.Leads.LeadSourceType()
                    {
                        Id = db.TypeId.Value,
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
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.AdditionalQuestion1, opt => opt.MapFrom(src => src.AdditionalQuestion1))
                .ForMember(dst => dst.AdditionalData1, opt => opt.MapFrom(src => src.AdditionalData1))
                .ForMember(dst => dst.AdditionalQuestion2, opt => opt.MapFrom(src => src.AdditionalQuestion2))
                .ForMember(dst => dst.AdditionalData2, opt => opt.MapFrom(src => src.AdditionalData2));

            Mapper.CreateMap<Common.Models.Leads.LeadSource, LeadSource>()
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
                .ForMember(dst => dst.TypeId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Type == null) return null;
                    return model.Type.Id;
                }))
                .ForMember(dst => dst.ContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Contact == null) return null;
                    return model.Contact.Id;
                }))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.AdditionalQuestion1, opt => opt.MapFrom(src => src.AdditionalQuestion1))
                .ForMember(dst => dst.AdditionalData1, opt => opt.MapFrom(src => src.AdditionalData1))
                .ForMember(dst => dst.AdditionalQuestion2, opt => opt.MapFrom(src => src.AdditionalQuestion2))
                .ForMember(dst => dst.AdditionalData2, opt => opt.MapFrom(src => src.AdditionalData2));
        }
    }
}
