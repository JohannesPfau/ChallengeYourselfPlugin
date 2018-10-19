namespace ChallengeYourself
{
    public class CY_NN_LinLearnFunction
    {
        public double rateStart;
        public double rateEnd;

        public CY_NN_LinLearnFunction(double start, double end)
        {
            this.rateStart = start;
            this.rateEnd = end;
        }
        public double getRate(int i, int max_i)
        {
            double d = rateEnd - rateStart;
            d *= i;
            d /= max_i;
            d += rateStart;
            return d;
        }
    }
}
