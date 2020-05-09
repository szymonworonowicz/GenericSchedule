using SI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;


namespace SI.Controller
{
    class DataController
    {
        public List<Subject> Subjects { get; private set; } = null;
        public List<Group> Groups { get; private set; } = null;
        public List<LessonTime> Times{ get; private set; } = null;
        public  void GetData()
        {
            Subjects = new List<Subject>();
            Groups = new List<Group>();
            Times = new List<LessonTime>();
            
            using(StreamReader CourseReader = File.OpenText("../../../Data/Courses.json")) 
            using(StreamReader GroupReader = File.OpenText("../../../Data/Groups.json"))
            using(StreamReader TimeReader = File.OpenText("../../../Data/LessonTime.json"))
            {
                var courses= CourseReader.ReadToEnd();
                var groups = GroupReader.ReadToEnd();
                var times = TimeReader.ReadToEnd();
                Subjects = JsonConvert.DeserializeObject<List<Subject>>(courses);
                Groups = JsonConvert.DeserializeObject<List<Group>>(groups);
                Times = JsonConvert.DeserializeObject<List<LessonTime>>(times);
            }
        }

    }
}
