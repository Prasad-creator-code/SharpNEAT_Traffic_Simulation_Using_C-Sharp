namespace NEATDrive_WPF.DrivingScripts.Utilities.SharpNEAT.Legacy.AI.Experiment
{
    public class GameExperiment : SimpleNeatExperiment
    {
        public override IPhenomeEvaluator<IBlackBox> PhenomeEvaluator
        {
            get { return new GameEvaluator(); }
        }

        public override int InputCount
        {
            get { return 9; }
        }

        public override int OutputCount
        {
            get { return 9; }
        }

        public override bool EvaluateParents
        {
            get { return true; }
        }
    }
}
