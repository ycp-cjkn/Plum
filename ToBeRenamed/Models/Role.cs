using System.Collections.Generic;

namespace ToBeRenamed.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ISet<Privilege> Privileges { get; set; }
    }
}
