using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NebulaMigration.ViewModels
{
    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public IList<string> Roles { get; set; }

        public bool HasRegistered { get; set; }
    }
}
