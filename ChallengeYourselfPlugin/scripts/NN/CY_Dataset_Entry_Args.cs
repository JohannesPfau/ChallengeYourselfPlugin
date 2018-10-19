using System;

namespace ChallengeYourself
{
    public delegate void CY_Dataset_Entry_Handler(object sender, CY_Dataset_Entry_Args e);
    
    public class CY_Dataset_Entry_Args : EventArgs
    {
        private int i;
        private CY_NN_Dataset_Entry e;
        
        public int iter
        {
            get { return i; }
        }
        
        public CY_NN_Dataset_Entry entry
        {
            get { return e; }
        }
        
        public CY_Dataset_Entry_Args(int i, CY_NN_Dataset_Entry e)
        {
            this.i = i;
            this.e = e;
        }
    }
}
