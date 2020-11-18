using Abp.Application.Services.Dto;
using System;

namespace Fanap.DataLabeling.Reports
{
    public class ScoreboardInput: PagedResultRequestDto
    {
        public DateTime? From { get; set; }
        public Guid? DataSetId { get; set; }
    }
}
