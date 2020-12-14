using Fanap.DataLabeling.Targets;

namespace Fanap.DataLabeling.Credit
{
    public class GetCreditOutput
    {
        public double Credit { get; internal set; }
        public TargetDefinitionDto Target { get; internal set; }
        public int Correct { get; internal set; }
        public int Incorrect { get; internal set; }
        public double Middle { get; internal set; }
    }
}