// -----------------------------------------------------------------------
// <copyright file="MatterContact.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Matters
{
    using System;
    using AutoMapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Common.Models.MapMe]
    public class MatterContact : Core
    {
        [ColumnMapping(Name = "id")]
        public int Id { get; set; }

        [ColumnMapping(Name = "matter_id")]
        public Guid MatterId { get; set; }

        [ColumnMapping(Name = "contact_id")]
        public int ContactId { get; set; }

        [ColumnMapping(Name = "role")]
        public string Role { get; set; }

        [ColumnMapping(Name = "is_client")]
        public bool IsClient { get; set; }

        [ColumnMapping(Name = "is_client_contact")]
        public bool IsClientContact { get; set; }

        [ColumnMapping(Name = "is_appointed")]
        public bool IsAppointed { get; set; }

        [ColumnMapping(Name = "is_party")]
        public bool IsParty { get; set; }

        [ColumnMapping(Name = "party_title")]
        public string PartyTitle { get; set; }

        [ColumnMapping(Name = "is_judge")]
        public bool IsJudge { get; set; }

        [ColumnMapping(Name = "is_witness")]
        public bool IsWitness { get; set; }

        [ColumnMapping(Name = "is_attorney")]
        public bool IsAttorney { get; set; }

        [ColumnMapping(Name = "attorney_for_contact_id")]
        public int? AttorneyForContactId { get; set; }

        [ColumnMapping(Name = "is_lead_attorney")]
        public bool IsLeadAttorney { get; set; }

        [ColumnMapping(Name = "is_support_staff")]
        public bool IsSupportStaff { get; set; }

        [ColumnMapping(Name = "support_staff_for_contact_id")]
        public int? SupportStaffForContactId { get; set; }

        [ColumnMapping(Name = "is_third_party_payor")]
        public bool IsThirdPartyPayor { get; set; }

        [ColumnMapping(Name = "third_party_payor_for_contact_id")]
        public int? ThirdPartyPayorForContactId { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(MatterContact), new ColumnAttributeTypeMapper<MatterContact>());
            Mapper.CreateMap<DBOs.Matters.MatterContact, Common.Models.Matters.MatterContact>()
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
                .ForMember(dst => dst.Matter, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Matters.Matter()
                    {
                        Id = db.MatterId,
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
                }))
                .ForMember(dst => dst.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dst => dst.IsClient, opt => opt.MapFrom(src => src.IsClient))
                .ForMember(dst => dst.IsClientContact, opt => opt.MapFrom(src => src.IsClientContact))
                .ForMember(dst => dst.IsAppointed, opt => opt.MapFrom(src => src.IsAppointed))
                .ForMember(dst => dst.IsParty, opt => opt.MapFrom(src => src.IsParty))
                .ForMember(dst => dst.PartyTitle, opt => opt.MapFrom(src => src.PartyTitle))
                .ForMember(dst => dst.IsJudge, opt => opt.MapFrom(src => src.IsJudge))
                .ForMember(dst => dst.IsWitness, opt => opt.MapFrom(src => src.IsWitness))
                .ForMember(dst => dst.IsAttorney, opt => opt.MapFrom(src => src.IsAttorney))
                .ForMember(dst => dst.AttorneyFor, opt => opt.ResolveUsing(db =>
                {
                    if (!db.AttorneyForContactId.HasValue) return null;
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.AttorneyForContactId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.IsLeadAttorney, opt => opt.MapFrom(src => src.IsLeadAttorney))
                .ForMember(dst => dst.IsSupportStaff, opt => opt.MapFrom(src => src.IsSupportStaff))
                .ForMember(dst => dst.SupportStaffFor, opt => opt.ResolveUsing(db =>
                {
                    if (!db.SupportStaffForContactId.HasValue) return null;
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.SupportStaffForContactId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.IsThirdPartyPayor, opt => opt.MapFrom(src => src.IsThirdPartyPayor))
                .ForMember(dst => dst.ThirdPartyPayorFor, opt => opt.ResolveUsing(db =>
                {
                    if (!db.ThirdPartyPayorForContactId.HasValue) return null;
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.ThirdPartyPayorForContactId,
                        IsStub = true
                    };
                }));

            Mapper.CreateMap<Common.Models.Matters.MatterContact, DBOs.Matters.MatterContact>()
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
                .ForMember(dst => dst.MatterId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Matter == null || !model.Matter.Id.HasValue)
                        throw new Exception("Matter cannot be null");
                    return model.Matter.Id.Value;
                }))
                .ForMember(dst => dst.ContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Contact == null)
                        return null;
                    return model.Contact.Id;
                }))
                .ForMember(dst => dst.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dst => dst.IsClient, opt => opt.MapFrom(src => src.IsClient))
                .ForMember(dst => dst.IsClientContact, opt => opt.MapFrom(src => src.IsClientContact))
                .ForMember(dst => dst.IsAppointed, opt => opt.MapFrom(src => src.IsAppointed))
                .ForMember(dst => dst.IsParty, opt => opt.MapFrom(src => src.IsParty))
                .ForMember(dst => dst.PartyTitle, opt => opt.MapFrom(src => src.PartyTitle))
                .ForMember(dst => dst.IsJudge, opt => opt.MapFrom(src => src.IsJudge))
                .ForMember(dst => dst.IsWitness, opt => opt.MapFrom(src => src.IsWitness))
                .ForMember(dst => dst.IsAttorney, opt => opt.MapFrom(src => src.IsAttorney))
                .ForMember(dst => dst.AttorneyForContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.AttorneyFor == null)
                        return null;
                    return model.AttorneyFor.Id;
                }))
                .ForMember(dst => dst.IsLeadAttorney, opt => opt.MapFrom(src => src.IsLeadAttorney))
                .ForMember(dst => dst.IsSupportStaff, opt => opt.MapFrom(src => src.IsSupportStaff))
                .ForMember(dst => dst.SupportStaffForContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.SupportStaffFor == null)
                        return null;
                    return model.SupportStaffFor.Id;
                }))
                .ForMember(dst => dst.IsThirdPartyPayor, opt => opt.MapFrom(src => src.IsThirdPartyPayor))
                .ForMember(dst => dst.ThirdPartyPayorForContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.ThirdPartyPayorFor == null)
                        return null;
                    return model.ThirdPartyPayorFor.Id;
                }));
        }
    }
}