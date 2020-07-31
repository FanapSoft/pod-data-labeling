using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    public interface IAnswersAppService
    {
        Task<SubmitAnswerOutput> SubmitAnswer(SubmitAnswerInput input);
    }
}
