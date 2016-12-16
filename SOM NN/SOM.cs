using System;
using System.Drawing;
using System.Linq;
using System.Windows.Controls.Primitives;
using Color = System.Windows.Media.Color;

namespace SOM_NN
{
    public class SOM
    {
        private int dimSize;

        public double TimeConst { get; set; }

        public int MapRadius => dimSize/2;

        public Node[,] Map { get; set; }

        public Color[] Colors => Map.OfType<Node>().Select(node => node.Color).ToArray();

        public double LearningRate { get; set; } = 0.9;

        public SOM(int size)
        {
            dimSize = size;
            Map = new Node[size,size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Map[j,i] = new Node();
                }
            }
        }

        public void Learn(Color trainingVector, int iterationNumber)
        {
            var bmuPosition = FindBmu(trainingVector);
            var neighborhoodRadius = GetRadius(iterationNumber); //////////////////

            for (int y = 0; y < dimSize; y++)
            {
                for (int x = 0; x < dimSize; x++)
                {
                    var eukidleanDistFromBmu = (Math.Pow(x - bmuPosition.X, 2)
                        + Math.Pow(y - bmuPosition.Y, 2))/10;
                    if (eukidleanDistFromBmu < neighborhoodRadius)
                    {
                        Map[x, y].Weights[0] = (byte)(Map[x, y].Weights[0]
                                                           + GetDistanceFromBmu(eukidleanDistFromBmu, neighborhoodRadius)
                                                           * GetLearningRate(iterationNumber)
                                                           * (trainingVector.R - Map[x, y].Color.R));

                        Map[x, y].Weights[1] = (byte)(Map[x, y].Weights[1]
                                                           + GetDistanceFromBmu(eukidleanDistFromBmu, neighborhoodRadius)
                                                           * GetLearningRate(iterationNumber)
                                                           * (trainingVector.G - Map[x, y].Color.G));

                        Map[x, y].Weights[2] = (byte)(Map[x, y].Weights[2]
                                                           + GetDistanceFromBmu(eukidleanDistFromBmu, neighborhoodRadius)
                                                           * GetLearningRate(iterationNumber)
                                                           * (trainingVector.B - Map[x, y].Color.B));
                    }
                }
            }
        }

        private double GetLearningRate(int currentIteration)
        {
            return LearningRate * Math.Exp(-currentIteration / TimeConst);
        }

        private double GetDistanceFromBmu(double eukidleanDistFromBmu, double neighborhoodRadius)
        {
            return Math.Exp(-eukidleanDistFromBmu/(2*neighborhoodRadius));
        }

        private double GetRadius(int currentIteration)
        {
            return MapRadius* Math.Exp(-currentIteration/TimeConst);
        }

        public Point FindBmu(Color trainingVector)
        {
            var dist = 195075d; // Math.Pow(255, 2)*3;
            var result = new Point(0,0);
            for (int y = 0; y < dimSize; y++)
            {
                for (int x = 0; x < dimSize; x++)
                {
                    var currDist = Map[x,y].DistFromInput(trainingVector);
                    if (currDist < dist)
                    {
                        dist = currDist;
                        result.X = x;
                        result.Y = y;
                    }
                }
            }
            return result;
        }
    }
}