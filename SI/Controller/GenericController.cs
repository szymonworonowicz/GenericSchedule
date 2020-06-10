using SI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<List<GenericItem>> Generate(int CountofGeneration, double ProbabilityofCrosover = 0.8, double ProbabilityofMutation = 0.2, int Elitism = 1)
        {
            int CountofCrosover = (Data.Subjects.Count - Elitism) / 2; // CountofGeneration
            int RangeSum = 0;
            for (int i = 1; i <= CountofGeneration; i++)
                RangeSum += i;
            List<List<GenericItem>> list = new List<List<GenericItem>>(); // uznajemy 10 osobnikow na ktorych pracujemy , uznajemy narazie 6 lekcji w ciagu dnia
            Random rand = new Random();
            for (int i = 0; i < CountofGeneration; i++)
            {
                var ChromosomList = new List<GenericItem>();

                for (int j = 0; j < Data.Subjects.Count; j++)
                {
                    var item = new GenericItem
                    {
                        Id = j,
                        Group = Data.Groups[rand.Next(0, Data.Groups.Count)],
                        Subject = Data.Subjects[rand.Next(0, Data.Subjects.Count)],
                        Time = Data.Times[rand.Next(0, Data.Times.Count)],
                        Room = Data.Rooms[rand.Next(0, Data.Rooms.Count)]
                    };
                    item.TeacherId = rand.Next(0, item.Subject.Teachers.Count);
                    ChromosomList.Add(item);
                }

                list.Add(ChromosomList);
            }

            for (int i = 0; i < CountofGeneration; i++)
            {
                var FitnessValue = Fitness(list).ToArray(); // przystosowania 

                List<List<GenericItem>> generation = new List<List<GenericItem>>();

                var RangeArray = new int[FitnessValue.Length]; // tablica rang chromosomu o naym id  - nr miejsca w tablicy
                var ProbabilityParent = new double[FitnessValue.Length];// prawdopodobienstwo rodzica

                for (int j = 0; j < FitnessValue.Length; j++)
                {
                    RangeArray[FitnessValue[j].ChromosomID] = j+1;
                }
                for(int j = 0;j < FitnessValue.Length; j++)
                {
                    ProbabilityParent[FitnessValue[j].ChromosomID] = ((-RangeArray[FitnessValue[j].ChromosomID] + CountofGeneration + 1) / (double) RangeSum);  
                }
                var RandomArray = new double[FitnessValue.Length];
                RandomArray[0] = ProbabilityParent[0]; // tablica sum czesciowych do losowania zostania rodzicem
                for (int j = 1; j < FitnessValue.Length; j++)
                {
                    RandomArray[j] = RandomArray[j - 1] + ProbabilityParent[j];
                }
                for (int j = 0; j < Elitism; j++)
                {
                    generation.Add(list[FitnessValue[i].ChromosomID]);
                }
            }

            ;
            ;
            return null;
        }

        private IEnumerable<GenericFitnessElem> Fitness(List<List<GenericItem>> genericItems)
        {
            var Fitnesses = new GenericFitnessElem[genericItems.Count];
            int CountofItem = 0;

            foreach (var item in genericItems)
            {
                int localfitness = 1;
                for (int i = 0; i < item.Count; i++)
                {
                    for (int j = i + 1; j < item.Count; j++)
                    {
                        if (item[i].Group.CountofPerson > item[j].Room.Capacity)
                        {
                            localfitness++;
                        }
                        else if (item[i].Subject.Id == item[j].Subject.Id && (item[i].Time.Start == item[j].Time.Start && item[i].Time.End == item[j].Time.End))
                        {
                            localfitness++;
                        }
                        else if (item[i].TeacherId == item[j].TeacherId && (item[i].Time.Start == item[j].Time.Start && item[i].Time.End == item[j].Time.End))
                        {
                            localfitness++;
                        }
                    }
                }
                Fitnesses[CountofItem] = new GenericFitnessElem
                {
                    ChromosomID = CountofItem,
                    Fitness = 1.0 / localfitness,
                };
                CountofItem++;
            }

            return Fitnesses.OrderByDescending(x => x.Fitness);
        }

    }
}
