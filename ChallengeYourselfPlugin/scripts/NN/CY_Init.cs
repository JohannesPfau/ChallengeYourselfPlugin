namespace ChallengeYourself
{
    public interface CY_Init
    {
        void init(CY_NeuronLayer activationLayer);
        void init(CY_NeuronToNeuronBackprop connector);
    }
}