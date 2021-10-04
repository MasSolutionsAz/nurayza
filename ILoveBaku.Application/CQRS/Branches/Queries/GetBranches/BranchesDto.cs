using System.Collections.Generic;

namespace ILoveBaku.Application.CQRS.Branches.Queries.GetBranches
{
    public class BranchesDto
    {
        public string Address { get; set; }
        public List<string> Addresses { get; set; }
        public string ContactEmail { get; set; }
        public bool IsOnlineBranch { get; set; }
        public string Phone { get; set; }
    }
}