using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fanap.DataLabeling.Users.Dto;
using System;

namespace Fanap.DataLabeling.Targets
{
    [AutoMap(typeof(UserTarget))]
    public class UserTargetDto : FullAuditedEntityDto<Guid>
    {
        public long OwnerId { get; set; }
        public UserDto Owner { get; set; }
        public Guid TargetDefinitionId { get; set; }
        public TargetDefinitionDto TargetDefinition { get; set; }
    }
}
