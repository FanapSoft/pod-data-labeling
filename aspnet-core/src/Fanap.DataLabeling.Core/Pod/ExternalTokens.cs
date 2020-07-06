using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Pod
{

    public class ExternalToken : Entity
    {
        public string Provider { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long UserId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? UsageTime { get; set; }
    }
}
