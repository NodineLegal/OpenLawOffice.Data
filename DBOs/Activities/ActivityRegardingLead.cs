// -----------------------------------------------------------------------
// <copyright file="ActivityRegardingLead.cs" company="Nodine Legal, LLC">
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
    public class ActivityRegardingLead : ActivityRegardingBase
    {
        [ColumnMapping(Name = "lead")]
        public long? Lead { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(ActivityRegardingLead), new ColumnAttributeTypeMapper<ActivityRegardingLead>());
            Mapper.CreateMap<ActivityRegardingLead, Common.Models.Activities.ActivityRegardingLead>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Type, opt => opt.ResolveUsing(db =>
                {
                    if (!db.Type.HasValue) return null;
                    return new Common.Models.Activities.ActivityRegardingType()
                    {
                        Id = db.Type.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Activity, opt => opt.ResolveUsing(db =>
                {
                    if (!db.Activity.HasValue) return null;
                    
                    Common.Models.Activities.ActivityType type = Data.Activities.ActivityType.GetByActivityId(db.Activity.Value);
                    
                    if (type.Title == "Phone Call")
                        return new Common.Models.Activities.ActivityPhonecall()
                        {
                            Id = db.Activity,
                            IsStub = true
                        };
                    else if (type.Title == "Email")
                        return new Common.Models.Activities.ActivityEmail()
                        {
                            Id = db.Activity,
                            IsStub = true
                        };
                    else if (type.Title == "Letter")
                        return new Common.Models.Activities.ActivityLetter()
                        {
                            Id = db.Activity,
                            IsStub = true
                        };
                    else if (type.Title == "Task")
                        return new Common.Models.Activities.ActivityTask()
                        {
                            Id = db.Activity,
                            IsStub = true
                        };
                    else
                        throw new System.InvalidOperationException("db.Activity.Type of unknown value");
                }))
                .ForMember(dst => dst.Lead, opt => opt.ResolveUsing(db =>
                {
                    if (!db.Lead.HasValue) return null;
                    return new Common.Models.Leads.Lead()
                    {
                        Id = db.Lead.Value,
                        IsStub = true
                    };
                }));      
            
            Mapper.CreateMap<Common.Models.Activities.ActivityRegardingLead, ActivityRegardingLead>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Type, opt => opt.ResolveUsing(model =>
                {
                    if (model.Type == null) return null;
                    return model.Type.Id;
                }))
                .ForMember(dst => dst.Activity, opt => opt.ResolveUsing(model =>
                {
                    if (model.Activity == null) return null;
                    return model.Activity.Id;
                }))
                .ForMember(dst => dst.Lead, opt => opt.ResolveUsing(model =>
                {
                    if (model.Lead == null) return null;
                    return model.Lead.Id;
                }));
        }
    }
}
