using Core.Common.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Identity
{
    public class RolePermission
    {
        public string RoleId { get; set; } = string.Empty;
        public long PermissionId { get; set; }

        public AppRole Role { get; set; } = default!;
        public Permission Permission { get; set; }  = default!;
    }
}
