// -----------------------------------------------------------------------
// <copyright file="Asset.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Assets
{
    using System;
    using AutoMapper;

    [Common.Models.MapMe]
    public class Asset : Core
    {
        [ColumnMapping(Name = "id")]
        public Guid? Id { get; set; }

        [ColumnMapping(Name = "id_int")]
        public long? IdInt { get; set; }

        [ColumnMapping(Name = "date")]
        public DateTime Date { get; set; }

        [ColumnMapping(Name = "title")]
        public string Title { get; set; }

        [ColumnMapping(Name = "is_final")]
        public bool IsFinal { get; set; }

        [ColumnMapping(Name = "is_court_filed")]
        public bool IsCourtFiled { get; set; }

        [ColumnMapping(Name = "is_attorney_work_product")]
        public bool IsAttorneyWorkProduct { get; set; }

        [ColumnMapping(Name = "is_privileged")]
        public bool IsPrivileged { get; set; }

        [ColumnMapping(Name = "is_discoverable")]
        public bool IsDiscoverable { get; set; }

        [ColumnMapping(Name = "provided_in_discovery_at")]
        public DateTime? ProvidedInDiscoveryAt { get; set; }

        [ColumnMapping(Name = "checked_out_by_id")]
        public Guid? CheckedOutById { get; set; }

        [ColumnMapping(Name = "checked_out_at")]
        public DateTime? CheckedOutAt { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Asset), new ColumnAttributeTypeMapper<Asset>());
            Mapper.CreateMap<DBOs.Assets.Asset, Common.Models.Assets.Asset>()
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
                .ForMember(dst => dst.IdInt, opt => opt.MapFrom(src => src.IdInt))
                .ForMember(dst => dst.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.IsFinal, opt => opt.MapFrom(src => src.IsFinal))
                .ForMember(dst => dst.IsCourtFiled, opt => opt.MapFrom(src => src.IsCourtFiled))
                .ForMember(dst => dst.IsAttorneyWorkProduct, opt => opt.MapFrom(src => src.IsAttorneyWorkProduct))
                .ForMember(dst => dst.IsPrivileged, opt => opt.MapFrom(src => src.IsPrivileged))
                .ForMember(dst => dst.IsDiscoverable, opt => opt.MapFrom(src => src.IsDiscoverable))
                .ForMember(dst => dst.ProvidedInDiscoveryAt, opt => opt.MapFrom(src => src.ProvidedInDiscoveryAt))
                .ForMember(dst => dst.CheckedOutBy, opt => opt.ResolveUsing(db =>
                {
                    if (!db.CheckedOutById.HasValue) return null;
                    return new Common.Models.Account.Users()
                    {
                        PId = db.CheckedOutById.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.CheckedOutAt, opt => opt.MapFrom(src => src.CheckedOutAt));

            Mapper.CreateMap<Common.Models.Assets.Asset, DBOs.Assets.Asset>()
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
                .ForMember(dst => dst.IdInt, opt => opt.MapFrom(src => src.IdInt))
                .ForMember(dst => dst.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.IsFinal, opt => opt.MapFrom(src => src.IsFinal))
                .ForMember(dst => dst.IsCourtFiled, opt => opt.MapFrom(src => src.IsCourtFiled))
                .ForMember(dst => dst.IsAttorneyWorkProduct, opt => opt.MapFrom(src => src.IsAttorneyWorkProduct))
                .ForMember(dst => dst.IsPrivileged, opt => opt.MapFrom(src => src.IsPrivileged))
                .ForMember(dst => dst.IsDiscoverable, opt => opt.MapFrom(src => src.IsDiscoverable))
                .ForMember(dst => dst.ProvidedInDiscoveryAt, opt => opt.MapFrom(src => src.ProvidedInDiscoveryAt))
                .ForMember(dst => dst.CheckedOutById, opt => opt.ResolveUsing(model =>
                {
                    if (model.CheckedOutBy == null) return null;
                    return model.CheckedOutBy.PId;
                }))
                .ForMember(dst => dst.CheckedOutAt, opt => opt.MapFrom(src => src.CheckedOutAt));
        }
    }
}
