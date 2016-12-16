using System;
using System.Windows.Media;

namespace SOM_NN
{
    public class Node
    {
        public byte[] Weights;

        public Color Color => Color.FromRgb(Weights[0], Weights[1], Weights[2]);
        public Node()
        {
            Weights = new byte[3];
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = RandomGenerator.NextByte();
            }
        }

        public double DistFromInput(Color trainingVector)
        {
            return Math.Pow(trainingVector.R - Color.R, 2)
                   + Math.Pow(trainingVector.G - Color.G, 2)
                   + Math.Pow(trainingVector.B - Color.B, 2);
        }
    }
}