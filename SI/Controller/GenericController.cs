using SI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;

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

        public IEnumerable<List<List<GenericItem>>> Generate(int CountofGeneration, double ProbabilityofCrosover = 0.8, double ProbabilityofMutation = 0.2, int Elitism = 1)
        {
            int CountofCrosover = (Data.Subjects.Count - Elitism) / 2; // CountofGeneration
            int RangeSum = 0;
            for (int i = 1; i <= CountofGeneration; i++)
                RangeSum += i;
            List<List<GenericItem>>[] list = new List<List<GenericItem>>[5]; // uznajemy 10 osobnikow na ktorych pracujemy , uznajemy narazie 6 lekcji w ciagu dnia
            Random rand = new Random();
            for (int k = 0; k < 5; k++)
            {
                list[k] = new List<List<GenericItem>>();
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
                        item.TeacherId = rand.Next(1, item.Subject.Teachers.Count + 1);
                        ChromosomList.Add(item);
                    }
                    //            list[k][i] = new List<GenericItem>();
                    list[k].Add(ChromosomList);
                    // list[k][i]=ChromosomList;
                }
            }




            List<List<GenericItem>>[] generation = new List<List<GenericItem>>[5];



            for (int i = 0; i < CountofGeneration; i++)
            {

                for (int k = 0; k < 5; k++)
                {
                    generation[k] = new List<List<GenericItem>>();


                    var FitnessValue = Fitness(list[k]).ToArray(); // przystosowania              
                    var RangeArray = new int[FitnessValue.Length]; // tablica rang chromosomu o danym id  - nr miejsca w tablicy
                    var ProbabilityParent = new double[FitnessValue.Length];// prawdopodobienstwo rodzica

                    for (int j = 0; j < FitnessValue.Length; j++)
                    {
                        RangeArray[FitnessValue[j].ChromosomID] = j + 1;
                    }
                    for (int j = 0; j < FitnessValue.Length; j++)
                    {
                        ProbabilityParent[FitnessValue[j].ChromosomID] = ((-RangeArray[FitnessValue[j].ChromosomID] + CountofGeneration + 1) / (double)RangeSum);
                    }
                    var RandomArray = new double[FitnessValue.Length];
                    RandomArray[0] = ProbabilityParent[0]; // tablica sum czesciowych do losowania zostania rodzicem
                    for (int j = 1; j < FitnessValue.Length; j++)
                    {
                        RandomArray[j] = RandomArray[j - 1] + ProbabilityParent[j];
                    }
                    int[] tab = new int[Elitism];
                    for (int j = 0; j < Elitism; j++)
                    {
                        generation[k].Add(list[k][FitnessValue[i].ChromosomID]);
                        tab[j] = FitnessValue[i].ChromosomID;


                    }



                    for (int j = 0; j < CountofGeneration / 2; j++)
                    {
                        _ = new List<GenericItem>();
                        _ = new List<GenericItem>();
                        List<GenericItem> item = list[k][j];
                        List<GenericItem> crossItem = list[k][rand.Next(j, CountofGeneration - 1)];
                        var ChromosomTempList = new List<GenericItem>();
                        var ChromosomTempList1 = new List<GenericItem>();

                        if (tab.Where(x => x == item[0].Id) != null && tab.Where(x => x == crossItem[0].Id) != null)
                        {
                            continue;
                        }
                        if (rand.NextDouble() > ProbabilityofCrosover)
                        {
                            continue;
                        }
                        if (rand.NextDouble() > ProbabilityParent[item[0].Id] || rand.NextDouble() > ProbabilityParent[crossItem[0].Id])
                        {
                            continue;
                        }



                        int howMany = rand.Next(0, Data.Subjects.Count);
                        //krzyzowanie
                        for (int l = 0; l < howMany; l++)
                        {
                            ChromosomTempList.Add(item[l]);
                            ChromosomTempList1.Add(crossItem[l]);
                        }
                        for (int l = howMany; l < Data.Subjects.Count; l++)
                        {
                            ChromosomTempList.Add(crossItem[l]);
                            ChromosomTempList1.Add(item[l]);
                        }
                        generation[k].Add(ChromosomTempList);
                        generation[k].Add(ChromosomTempList1);


                    }
                }
                list = generation.Clone() as List<List<GenericItem>>[];
            }

            return new List<List<List<GenericItem>>> { list[0], list[1], list[2], list[3], list[4] };
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
