using System;
using System.Collections.Generic;
using System.Text;

namespace SI.Models
{
    class GenericItem
    {
        public int Id { get; set; }
        public Subject Subject { get; set; }
        public int TeacherId { get; set; }
        public Group Group { get; set; }
        public LessonTime Time { get; set; }
    }
}
