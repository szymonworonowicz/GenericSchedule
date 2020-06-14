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
        public List<LessonTime> Times{ get; private set; } = null;
        public List<Room> Rooms { get; private set; } = null;
        public  void GetData()
        {
            Subjects = new List<Subject>();
            Times = new List<LessonTime>();
            
            using(StreamReader CourseReader = File.OpenText("../../../Data/Courses.json")) 
            using(StreamReader TimeReader = File.OpenText("../../../Data/LessonTime.json"))
            using(StreamReader RoomsReader = File.OpenText("../../../Data/Rooms.json"))
            {
                var courses= CourseReader.ReadToEnd();
                var times = TimeReader.ReadToEnd();
                var rooms = RoomsReader.ReadToEnd();
                Subjects = JsonConvert.DeserializeObject<List<Subject>>(courses);
                Times = JsonConvert.DeserializeObject<List<LessonTime>>(times);
                Rooms = JsonConvert.DeserializeObject<List<Room>>(rooms);
            }
        }

    }
}
