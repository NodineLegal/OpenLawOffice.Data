// -----------------------------------------------------------------------
// <copyright file="ActivityRegardingType.cs" company="Nodine Legal, LLC">
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
    public class ActivityRegardingType : DboBase
    {
        [ColumnMapping(Name = "id")]
        public int? Id { get; set; }

        [ColumnMapping(Name = "title")]
        public string Title { get; set; }

        [ColumnMapping(Name = "order")]
        public int? Order { get; set; }

        [ColumnMapping(Name = "default")]
        public bool? Default { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(ActivityRegardingType), new ColumnAttributeTypeMapper<ActivityRegardingType>());
            Mapper.CreateMap<ActivityRegardingType, Common.Models.Activities.ActivityRegardingType>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Default, opt => opt.MapFrom(src => src.Default))
                .ForMember(dst => dst.Order, opt => opt.MapFrom(src => src.Order));

            Mapper.CreateMap<Common.Models.Activities.ActivityRegardingType, ActivityRegardingType>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Default, opt => opt.MapFrom(src => src.Default))
                .ForMember(dst => dst.Order, opt => opt.MapFrom(src => src.Order));
        }
    }
}
