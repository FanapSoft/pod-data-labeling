using Fanap.DataLabeling.Targets;

namespace Fanap.DataLabeling.Credit
{
    public class GetCreditOutput
    {
        public double Credit { get; internal set; }
        public TargetDefinitionDto Target { get; internal set; }
    }
}