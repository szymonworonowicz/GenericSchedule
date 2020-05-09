using System;
using System.Collections.Generic;
using System.Text;

namespace SI.Controller
{
    class GenericController
    {
        private readonly DataController Data;

        public GenericController()
        {
            Data = new DataController();
            Data.GetData();
        }
    }
}
