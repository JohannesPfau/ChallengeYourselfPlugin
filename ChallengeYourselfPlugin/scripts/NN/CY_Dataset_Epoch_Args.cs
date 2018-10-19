using System;

namespace ChallengeYourself
{
    public delegate void CY_Dataset_Epoch_Handler(object sender, CY_Dataset_Epoch_Args e);
    
    public class CY_Dataset_Epoch_Args : EventArgs
    {
        private int i;
        private CY_NN_Dataset set;
        public int iter
        {
            get { return i; }
        }
        public CY_NN_Dataset Set
        {
            get { return set; }
        }        
        public CY_Dataset_Epoch_Args(int trainingIteration, CY_NN_Dataset set)
        {
            this.set = set;
            this.i = trainingIteration;
        }
    }
}
