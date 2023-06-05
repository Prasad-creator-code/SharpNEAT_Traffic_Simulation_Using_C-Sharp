using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace NEATDrive_WPF.DrivingScripts.Utilities.SharpNEAT.Legacy.AI
{
    internal class NeatDriveExperiment : SimpleExperiment
    {
        public override IPhenomeEvaluator<IBlackBox> PhenomeEvaluator
        {
            get { return new NEATDriveEvaluator(SimulationManager.instance.Car, 2); }
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
