using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mediver_server
{
    public class MediverUser : IdentityUser
    {
        public float Weight { get; set; } = 1;
    }
}
