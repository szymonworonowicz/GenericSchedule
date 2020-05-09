using System;
using System.Collections.Generic;
using System.Text;

namespace SI.Models
{
    class Subject
    {
        public int Id { get; set; }
        public List<Teacher> Teachers { get; set; }
        public Room Room { get; set; }
        public string Name { get; set; }
    }
}
