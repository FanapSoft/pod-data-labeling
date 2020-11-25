namespace Fanap.DataLabeling.Credit
{
    public class GetCreditOutput
    {
        public decimal Credit { get; internal set; }
        public int Correct { get; internal set; }
        public int G { get; internal set; }
        public int UMin { get; internal set; }
        public int Umax { get; internal set; }
        public int GoldCounts { get; internal set; }
    }
}