using ILoveBaku.Application.CQRS.Branches.Queries.GetBranches;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.Contact.Models
{
    public class ContactVM
    {
        [Required]
        public string Name { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Message { get; set; }

        public List<BranchesDto> Branches { get; set; }
    }
}
