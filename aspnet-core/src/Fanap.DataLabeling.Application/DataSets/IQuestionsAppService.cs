using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    public interface IQuestionsAppService
    {
        Task<QuestionDto> GetQuestion(GetQuestionInput input);
    }
}
