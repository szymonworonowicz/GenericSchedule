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


        private int CountOfGroups;

        public GenericController(int cntofGroups)
        {
            Data = new DataController();
            Data.GetData();
            this.CountOfGroups = cntofGroups;
        }

        public List<List<GenericItem>> Generate(int CountofGeneration, double ProbabilityofCrosover = 0.8, double ProbabilityofMutation = 0.2, int Elitism = 1)
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
                            Subject = Data.Subjects[rand.Next(0, Data.Subjects.Count)],
                            Time = Data.Times[rand.Next(0, Data.Times.Count)],
                            Room = Data.Rooms[rand.Next(0, Data.Rooms.Count)]
                        };
                        item.TeacherId = rand.Next(1, item.Subject.Teachers.Count + 1);
                        ChromosomList.Add(item);
                    }
                    list[k].Add(ChromosomList);
                }
                ;

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
                    var RandomArray = new double[FitnessValue.Length + 1];
                    RandomArray[0] = 0; // tablica sum czesciowych do losowania zostania rodzicem
                    for (int j = 1; j <= FitnessValue.Length; j++)
                    {
                        RandomArray[j] = RandomArray[j - 1] + ProbabilityParent[j - 1];
                    }
                    int[] tab = new int[Elitism];
                    for (int j = 0; j < Elitism; j++)
                    {
                        generation[k].Add(list[k][FitnessValue[j].ChromosomID]);
                        list[k][FitnessValue[j].ChromosomID] = null;
                    }


                    int crossover = (CountofGeneration - Elitism) / 2;
                    for (int j = 0; j < crossover; j++)
                    {
                        _ = new List<GenericItem>();
                        _ = new List<GenericItem>();
                        double propparent1 = rand.NextDouble();
                        double propparent2 = rand.NextDouble();
                        List<GenericItem> item = null;
                        List<GenericItem> crossItem = null;
                        for (int r = 1; r < RandomArray.Length; r++)
                        {
                            if (propparent1 > RandomArray[r - 1] && propparent1 < RandomArray[r])
                            {
                                item = list[k][r - 1];
                                break;
                            }
                        }
                        for (int r = 1; r < RandomArray.Length; r++)
                        {
                            if (propparent2 > RandomArray[r - 1] && propparent2 < RandomArray[r])
                            {
                                crossItem = list[k][r - 1];
                                break;
                            }
                        }
                        if (crossItem == item || crossItem == null || item == null)
                        {
                            continue;
                        }
                        var ChromosomTempList = new List<GenericItem>();
                        var ChromosomTempList1 = new List<GenericItem>();

                        if (rand.NextDouble() > ProbabilityofCrosover)
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
                        list[k][list[k].IndexOf(item)] = null;
                        list[k][list[k].IndexOf(crossItem)] = null;
                        generation[k].Add(ChromosomTempList);
                        generation[k].Add(ChromosomTempList1);
                    }


                    for (int j = 0; j < list[k].Count; j++)
                    {
                        double mutation = rand.NextDouble();
                        if (mutation < ProbabilityofMutation && list[k][j] != null)
                        {
                            int typemutation = rand.Next(0, 4);
                            int currentmutation = rand.Next(0, Data.Subjects.Count);
                            switch (typemutation)
                            {
                                case 1:
                                    list[k][j][currentmutation].Room = Data.Rooms[rand.Next(0, Data.Rooms.Count)];
                                    break;
                                case 2:
                                    list[k][j][currentmutation].Subject = Data.Subjects[rand.Next(0, Data.Subjects.Count)];
                                    break;
                                case 3:
                                    list[k][j][currentmutation].TeacherId = rand.Next(0, list[k][j][currentmutation].Subject.Teachers.Count);
                                    break;
                                case 4:
                                    list[k][j][currentmutation].Time = Data.Times[rand.Next(0, Data.Times.Count)];
                                    break;
                            }
                            generation[k].Add(list[k][j]);
                        }
                    }

                    for (int j = generation[k].Count; j < CountofGeneration; j++)
                    {
                        var ChromosomList = new List<GenericItem>();
                        for (int m = 0; m < Data.Subjects.Count; m++)
                        {
                            var item = new GenericItem
                            {
                                Id = m,
                                Subject = Data.Subjects[rand.Next(0, Data.Subjects.Count)],
                                Time = Data.Times[rand.Next(0, Data.Times.Count)],
                                Room = Data.Rooms[rand.Next(0, Data.Rooms.Count)]
                            };
                            item.TeacherId = rand.Next(1, item.Subject.Teachers.Count + 1);
                            ChromosomList.Add(item);
                        }
                        generation[k].Add(ChromosomList);
                    }
                }
                list = generation.Clone() as List<List<GenericItem>>[]; //kopia gleboka bo rozwala referencje
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < Data.Subjects.Count - 1; j++)
                {
                    for (int k = 0; k < Data.Subjects.Count - 1; k++)
                    {
                        GenericItem temp = new GenericItem();
                        if (list[i][0][k].Time.Start > list[i][0][k + 1].Time.Start)
                        {
                            temp = list[i][0][k];
                            list[i][0][k] = list[i][0][k + 1];
                            list[i][0][k + 1] = temp;
                        }
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < Data.Subjects.Count; j++)
                {
                    Teacher temp = new Teacher();
                    temp = list[i][0][j].Subject.Teachers.Find(x => x.Id == list[i][0][j].TeacherId);
                    list[i][0][j].TeacherName = temp.Identity;
                }
            }

            return new List<List<GenericItem>> { list[0][0], list[1][0], list[2][0], list[3][0], list[4][0] };
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
                        if (CountOfGroups > item[j].Room.Capacity)
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
