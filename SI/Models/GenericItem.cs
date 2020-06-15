﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SI.Models
{
    public class GenericItem
    {
        public int Id { get; set; }
        public Subject Subject { get; set; }
        public int TeacherId { get; set; }
        public LessonTime Time { get; set; }
        public Room Room { get; set; }
        public string TeacherName {get; set;}
    }
}
