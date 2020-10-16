using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    public interface IQuestionsAppService
    {
        Task<IEnumerable<LabelDto>> GetRandomLabel(GetRandomLabelInput input);
        Task<QuestionDto> GetQuestion(GetQuestionInput input);
    }
}
