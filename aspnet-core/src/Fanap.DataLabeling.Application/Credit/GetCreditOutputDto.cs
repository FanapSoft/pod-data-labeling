﻿using Fanap.DataLabeling.Targets;

namespace Fanap.DataLabeling.Credit
{
    public class GetCreditOutput
    {
        public decimal Credit { get; internal set; }
        public TargetDefinition Target { get; internal set; }
    }
}