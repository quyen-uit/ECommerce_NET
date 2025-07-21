using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Accounts
{
    public class PermissionResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required  string Description { get; set; }
        public required string Module { get; set; }
        public required string Action { get; set; }
    }
}
