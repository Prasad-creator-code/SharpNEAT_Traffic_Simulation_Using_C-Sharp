using NEATDrive_WPF.DrivingScripts.CarScripts.CivilianCar;
using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace NEATDrive_WPF.DrivingScripts.Utilities.SharpNEAT.Legacy.AI
{
    internal class NEATDriveEvaluator : IPhenomeEvaluator<IBlackBox>
    {

        private ulong _evalCount;
        private bool _stopConditionSatisfied;

        readonly SimulationManager _world;
        readonly int _trialsPerEvaluation;
        IBlackBox _currentPhenome;
        public CivilianCar Car;



        public NEATDriveEvaluator(CivilianCar car,

                                   int trialsPerEvaluation)
        {
            // Construct a re-usable instance of the prey capture world.
            //_world = new PreyCaptureWorld(preyInitMoves, preySpeed, sensorRange, maxTimesteps);
            _world = SimulationManager.instance;
            _trialsPerEvaluation = trialsPerEvaluation;
            Car = car;
        }

        //private void SetInputSignalArray(ISignalArray inputSignalArray, CivilianCar _car)
        //{
        //    _currentPhenome.InputSignalArray[0] = _car.GetSpeed();
        //    _currentPhenome.InputSignalArray[1] = _car.GetRotation();
        //}

        //public void Start
        public void UpdateCarSimulation()
        {
            //ISignalArray inputArr = _currentPhenome.Inputs;
            // Set the input nodes of the neural network based on the car's state
            _currentPhenome.InputSignalArray[0] = Car.GetSpeed();
            _currentPhenome.InputSignalArray[1] = Car.GetRotation();
            // Add more input nodes for other relevant information

            // Let the NEAT algorithm compute the outputs of the neural network
            _currentPhenome.Activate();

            // Get the outputs from the neural network
            double moveForwardOutput = _currentPhenome.OutputSignalArray[0];
            double brakeOutput = _currentPhenome.OutputSignalArray[1];
            double rotateRightOutput = _currentPhenome.OutputSignalArray[2];
            double rotateLeftOutput = _currentPhenome.OutputSignalArray[3];
            // Add more outputs based on your specific actions

            // Apply the car controls based on the neural network outputs
            if (moveForwardOutput > 0.5)
            {
                Car.MoveForward();
            }
            if (brakeOutput > 0.5)
            {
                Car.Brake();
            }
            if (rotateRightOutput > 0.5)
            {
                Car.RotateRight();
            }
            if (rotateLeftOutput > 0.5)
            {
                Car.RotateLeft();
            }
        }

        public ulong EvaluationCount
        {
            get { return _evalCount; }
        }

        public bool StopConditionSatisfied
        {
            get { return _stopConditionSatisfied; }
        }

        public FitnessInfo Evaluate(IBlackBox phenome)
        {
            //_currentPhenome = phenome;
            // Perform multiple independent trials.
            int fitness = 0;
            for (int i = 0; i < _trialsPerEvaluation; i++)
            {
                // TODO: Change RunTrial() to return 0 or 1, so that we can sum the result without performing a conditional branch.
                // Run a single trial, and record if the prey was captured.
                if (_world.RunTrial(phenome))
                    fitness++;
            }

            // Fitness is given by the number of trials in which the agent caught the prey.
            return new FitnessInfo(fitness, null);
        }

        public void Reset()
        {
            //RESET SIMULATION HERE?
            //throw new NotImplementedException();
        }


    }
}
