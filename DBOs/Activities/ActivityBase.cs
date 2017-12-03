// -----------------------------------------------------------------------
// <copyright file="ActivityBase.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Activities
{
    using System;
    using AutoMapper;

    [Common.Models.MapMe]
    public class ActivityBase : Core
    {
        [ColumnMapping(Name = "id")]
        public long? Id { get; set; }

        [ColumnMapping(Name = "type")]
        public int? Type { get; set; }

        [ColumnMapping(Name = "is_campaign_response")]
        public bool? IsCampaignResponse { get; set; }

        [ColumnMapping(Name = "subject")]
        public string Subject { get; set; }

        [ColumnMapping(Name = "body")]
        public string Body { get; set; }

        [ColumnMapping(Name = "owner")]
        public int? Owner { get; set; }

        [ColumnMapping(Name = "priority")]
        public int? Priority { get; set; }

        [ColumnMapping(Name = "due")]
        public DateTime? Due { get; set; }

        [ColumnMapping(Name = "state")]
        public bool? State { get; set; }

        [ColumnMapping(Name = "status_reason")]
        public int? StatusReason { get; set; }

        [ColumnMapping(Name = "duration")]
        public int? Duration { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(ActivityBase), new ColumnAttributeTypeMapper<ActivityBase>());
            Mapper.CreateMap<ActivityBase, Common.Models.Activities.ActivityBase>()
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
                    if (!db.Type.HasValue) return null;
                    return new Common.Models.Activities.ActivityType()
                    {
                        Id = db.Type.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.IsCampaignResponse, opt => opt.MapFrom(src => src.IsCampaignResponse))
                .ForMember(dst => dst.Subject, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dst => dst.Body, opt => opt.MapFrom(src => src.Body))
                .ForMember(dst => dst.Owner, opt => opt.ResolveUsing(db =>
                {
                    if (!db.Owner.HasValue) return null;
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.Owner.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Priority, opt => opt.ResolveUsing(db =>
                {
                    if (!db.Priority.HasValue) return null;
                    return new Common.Models.Activities.ActivityPriority()
                    {
                        Id = db.Priority.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Due, opt => opt.ResolveUsing(db =>
                {
                    return db.Due.ToSystemTime();
                }))
                .ForMember(dst => dst.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dst => dst.StatusReason, opt => opt.ResolveUsing(db =>
                {
                    if (!db.StatusReason.HasValue) return null;
                    return new Common.Models.Activities.ActivityStatusReason()
                    {
                        Id = db.StatusReason.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Duration, opt => opt.MapFrom(src => src.Duration));

            Mapper.CreateMap<Common.Models.Activities.ActivityBase, ActivityBase>()
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
                .ForMember(dst => dst.Type, opt => opt.ResolveUsing(model =>
                {
                    if (model.Type == null) return null;
                    return model.Type.Id;
                }))
                .ForMember(dst => dst.IsCampaignResponse, opt => opt.MapFrom(src => src.IsCampaignResponse))
                .ForMember(dst => dst.Subject, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dst => dst.Body, opt => opt.MapFrom(src => src.Body))
                .ForMember(dst => dst.Owner, opt => opt.ResolveUsing(model =>
                {
                    if (model.Owner == null) return null;
                    return model.Owner.Id;
                }))
                .ForMember(dst => dst.Priority, opt => opt.ResolveUsing(model =>
                {
                    if (model.Priority == null) return null;
                    return model.Priority.Id;
                }))
                .ForMember(dst => dst.Due, opt => opt.ResolveUsing(model =>
                {
                    return model.Due.ToSystemTime();
                }))
                .ForMember(dst => dst.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dst => dst.StatusReason, opt => opt.ResolveUsing(model =>
                {
                    if (model.StatusReason == null) return null;
                    return model.StatusReason.Id;
                }))
                .ForMember(dst => dst.Duration, opt => opt.MapFrom(src => src.Duration));
        }
    }
}
