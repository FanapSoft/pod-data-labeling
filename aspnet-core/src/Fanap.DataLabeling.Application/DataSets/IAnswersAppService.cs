using Abp.Application.Services;
using System;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    public interface IAnswersAppService: IAsyncCrudAppService<AnswerLogDto, Guid, GetAllAnswerLogsInput>
    {
        Task<SubmitAnswerOutput> SubmitAnswer(SubmitAnswerInput input);
    }
}
