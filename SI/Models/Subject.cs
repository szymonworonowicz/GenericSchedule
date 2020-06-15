using System;
using System.Collections.Generic;
using System.Text;

namespace SI.Models
{
   public  class Subject
    {
        public int Id { get; set; }
        public List<Teacher> Teachers { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
    }
}
