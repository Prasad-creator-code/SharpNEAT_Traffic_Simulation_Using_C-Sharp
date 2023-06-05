using NEATDrive_WPF.DrivingScripts.CarScripts.CivilianCar;
using SharpNeat.Phenomes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace NEATDrive_WPF.DrivingScripts
{
    /// <summary>
    /// This class contains the simulation parameters to be used during the sim and the NEAT working scripts
    /// </summary>
    class SimulationManager : IBlackBox
    {
        public static SimulationManager? instance;
        public CivilianCar Car;
        public IBlackBox? Agent;

        public int ID;
        public double CrashTime;
        public int Crossings;
        public int PedInteractions;
        public int MaxGenerations;
        public int Crashes;


        DataGrid myDataGrid;
        public List<MyData> dataList = new()
        {
            new MyData() { ID = 1, CrashTime = 25.6565, Crossings = 4, PedInteractions=2, MaxGenerations=53, Crashes=4},
            new MyData() { ID = 2, CrashTime = 30.45336, Crossings = 2, PedInteractions=3, MaxGenerations=68, Crashes=2},
            new MyData() { ID = 3, CrashTime = 35.87878, Crossings = 6, PedInteractions=7, MaxGenerations=34, Crashes=1 }
        };

        public int InputCount => throw new System.NotImplementedException();

        public int OutputCount => throw new System.NotImplementedException();

        public ISignalArray InputSignalArray => throw new System.NotImplementedException();

        public ISignalArray OutputSignalArray => throw new System.NotImplementedException();

        public bool IsStateValid => throw new System.NotImplementedException();

        public Memory<double> Inputs => throw new NotImplementedException();

        public Memory<double> Outputs => throw new NotImplementedException();

        public class MyData
        {
            public int ID { get; set; }
            public double CrashTime { get; set; }
            public int Crossings { get; set; }
            public int PedInteractions { get; set; }
            public int MaxGenerations { get; set; }
            public int Crashes { get; set; }
        }

        public void CycleStats(double crashTime, int crossings, int pedInteractions, int maxGens, int crashes)
        {
            ID = dataList.Last<MyData>().ID;
            CrashTime = crashTime;
            Crossings = crossings;
            PedInteractions = pedInteractions;
            MaxGenerations = maxGens;
            Crashes = crashes;

        }

        public void AddDataToGridList()
        {
            dataList.Add(new MyData() { ID = ID, CrashTime = CrashTime, Crossings = Crossings, PedInteractions = PedInteractions, MaxGenerations = MaxGenerations, Crashes = Crashes });
        }

        public void FillData(DataGrid dataGrid)
        {
            myDataGrid = dataGrid;
            myDataGrid.Columns.Clear();
            // Define the columns
            myDataGrid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            myDataGrid.Columns.Add(new DataGridTextColumn { Header = "Avg Crash Time (in mins)", Binding = new Binding("CrashTime") });
            myDataGrid.Columns.Add(new DataGridTextColumn { Header = "Crossings", Binding = new Binding("Crossings") });
            myDataGrid.Columns.Add(new DataGridTextColumn { Header = "Ped Interactions", Binding = new Binding("PedInteractions") });
            myDataGrid.Columns.Add(new DataGridTextColumn { Header = "Max Generations", Binding = new Binding("MaxGenerations") });
            myDataGrid.Columns.Add(new DataGridTextColumn { Header = "Crashes", Binding = new Binding("Crashes") });

            // Add data to the DataGrid
            myDataGrid.ItemsSource = dataList;
        }


        public bool RunTrial(IBlackBox agent)
        {
            //RUN SIM HERE!?! retutn bool
            //return true;
            Agent = agent;
            Debug.WriteLine("RunTrial");
            // Initialise world state.
            InitPositions();

            // Clear any prior agent (neural network) state.
            agent.ResetState();




            // The prey gets a head start. Here we simulate the world as normal for a number of timesteps, during which
            // the agent's sensors receive inputs as normal, but the it is prevented from moving.
            //int t = 0;
            //for (; t < _preyInitMoves; t++)
            //{
            //    SetAgentInputsAndActivate(agent);
            //    MovePrey();
            //}

            //// Let the chase begin!
            //for (; t < _maxTimesteps; t++)
            //{
            //    SetAgentInputsAndActivate(agent);
            //    MoveAgent(agent);
            //    if (IsPreyCaptured())
            //        return true;

            //    MovePrey();
            //    if (IsPreyCaptured())
            //    {   // The prey walked directly into the agent.
            //        return true;
            //    }
            //}

            // Agent failed to capture prey in the allotted time.
            return false;
        }

        void InitPositions()
        {
            //Implement car and ped positions here

        }

        public void MoveCar()
        {
            //ISignalArray inputArr = _currentPhenome.Inputs;
            // Set the input nodes of the neural network based on the car's state
            Agent.InputSignalArray[0] = Car.GetSpeed();
            Agent.InputSignalArray[1] = Car.GetRotation();
            // Add more input nodes for other relevant information

            // Let the NEAT algorithm compute the outputs of the neural network
            Agent.Activate();

            // Get the outputs from the neural network
            double moveForwardOutput = Agent.OutputSignalArray[0];
            double brakeOutput = Agent.OutputSignalArray[1];
            double rotateRightOutput = Agent.OutputSignalArray[2];
            double rotateLeftOutput = Agent.OutputSignalArray[3];
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

        public void SetAgentInputsAndActivate(IBlackBox agent)
        {
            // I THINK I HAVE A SOLUTION! I can just use the control system i have created and press buttons when i want

            /*
            // Set agent inputs and activate.

            const float PI_over_8 = MathF.PI / 8f;
            const float Four_over_PI = 4f / MathF.PI;
            const float Quarter = 1f / 4f;

            // Calc prey's position relative to the agent, in polar coordinates.
            CartesianToPolar(
                _preyPos - _agentPos,
                out int relPosRadiusSqr,
                out float relPosAzimuth);

            // Determine agent sensor input values.
            // Reset all inputs.
            var inputs = agent.Inputs.Span;
            inputs.Clear();

            // Bias input.
            inputs[0] = 1.0;

            // Test if prey is in sensor range.
            if (relPosRadiusSqr <= _sensorRangeSqr)
            {
                // Determine which sensor segment the prey is within - [0,7]. There are eight segments and they are tilted 22.5 degrees (half a segment)
                // such that due North, East South and West are each in the centre of a sensor segment (rather than on a segment boundary).
                float thetaAdjusted = relPosAzimuth - PI_over_8;
                if (thetaAdjusted < 0f) thetaAdjusted += 2f * MathF.PI;
                int segmentIdx = 1 + (int)MathF.Floor(thetaAdjusted * Four_over_PI);

                // Set sensor segment's input.
                inputs[segmentIdx] = 1.0;
            }

            // Prey closeness detector.
            inputs[9] = relPosRadiusSqr <= 4 ? 1.0 : 0.0;

            // Wall detectors - N,E,S,W.
            // North.
            int d = (__gridSize - 1) - _agentPos.Y;
            if (d <= 4)
                inputs[10] = (4 - d) * Quarter;

            // East.
            d = (__gridSize - 1) - _agentPos.X;
            if (d <= 4)
                inputs[11] = (4 - d) * Quarter;

            // South.
            if (_agentPos.Y <= 4)
                inputs[12] = (4 - _agentPos.Y) * Quarter;

            // West.
            if (_agentPos.X <= 4)
                inputs[13] = (4 - _agentPos.X) * Quarter;

            // Activate agent.
            agent.Activate();
            */
        }

        public void Activate()
        {
            throw new System.NotImplementedException();
        }

        public void ResetState()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

