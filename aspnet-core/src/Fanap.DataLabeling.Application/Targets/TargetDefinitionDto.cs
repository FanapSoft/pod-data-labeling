using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fanap.DataLabeling.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Targets
{
    [AutoMap(typeof(TargetDefinition))]
    public class TargetDefinitionDto: FullAuditedEntityDto<Guid>
    {
        public TargetType Type { get; set; }
        public long OwnerId { get; set; }
        public UserDto Owner { get; set; }
        public int AnswerCount { get; set; }

    }
}
