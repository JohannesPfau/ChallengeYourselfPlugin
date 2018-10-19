using System;
using System.Collections.Generic;

namespace ChallengeYourself
{
    public class CY_NN_Dataset
    {
        public IList<CY_NN_Dataset_Entry> trainingSamples;
        
        public CY_NN_Dataset_Entry this[int index]
        {
            get { return trainingSamples[index]; }
        }
        
        public CY_NN_Dataset()
        {
            this.trainingSamples = new List<CY_NN_Dataset_Entry>();
        }
    }
}