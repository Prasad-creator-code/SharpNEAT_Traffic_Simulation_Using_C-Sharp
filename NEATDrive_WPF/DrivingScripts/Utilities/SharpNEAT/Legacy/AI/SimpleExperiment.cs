using SharpNeat.Core;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.DistanceMetrics;
using SharpNeat.Evaluation;
//using SharpNeat.Evaluation;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.EvolutionAlgorithms.ComplexityRegulation;
using SharpNeat.Experiments;
//using SharpNeat.Experiments;
using SharpNeat.Genomes.Neat;
using SharpNeat.Neat.EvolutionAlgorithm;
using SharpNeat.Neat.Reproduction.Asexual;
using SharpNeat.Neat.Reproduction.Sexual;
//using SharpNeat.Neat.ComplexityRegulation;
//using SharpNeat.Neat.EvolutionAlgorithm;
//using SharpNeat.Neat.Reproduction.Asexual;
//using SharpNeat.Neat.Reproduction.Sexual;
using SharpNeat.Phenomes;
using SharpNeat.SpeciationStrategies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace NEATDrive_WPF.DrivingScripts.Utilities.SharpNEAT.Legacy.AI
{
    internal abstract class SimpleExperiment : INeatExperiment<double>
    {
        public string FactoryId => throw new NotImplementedException();

        public string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IBlackBoxEvaluationScheme<double> EvaluationScheme => throw new NotImplementedException();

        public bool IsAcyclic { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int CyclesPerActivation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ActivationFnName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public NeatEvolutionAlgorithmSettings EvolutionAlgorithmSettings => throw new NotImplementedException();

        public NeatReproductionAsexualSettings ReproductionAsexualSettings => throw new NotImplementedException();

        public NeatReproductionSexualSettings ReproductionSexualSettings => throw new NotImplementedException();

        public int PopulationSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double InitialInterconnectionsProportion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double ConnectionWeightScale { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IComplexityRegulationStrategy ComplexityRegulationStrategy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int DegreeOfParallelism { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EnableHardwareAcceleratedNeuralNets { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EnableHardwareAcceleratedActivationFunctions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        NeatEvolutionAlgorithmParameters _eaParams;
        NeatGenomeParameters _neatGenomeParams;
        string _name;
        int _populationSize = 100;
        int _specieCount = 100;
        NetworkActivationScheme _activationScheme;
        string _complexityRegulationStr;
        int? _complexityThreshold;
        string _description;
        ParallelOptions _parallelOptions;

        #region Abstract properties that subclasses must implement
        public abstract SharpNeat.Core.IPhenomeEvaluator<IBlackBox> PhenomeEvaluator { get; }
        public abstract int InputCount { get; }
        public abstract int OutputCount { get; }
        public abstract bool EvaluateParents { get; }
        #endregion



        #region INeatExperiment Members


        /// <summary>
        /// Gets the default population size to use for the experiment.
        /// </summary>
        public int DefaultPopulationSize
        {
            get { return _populationSize; }
        }

        /// <summary>
        /// Gets the NeatEvolutionAlgorithmParameters to be used for the experiment. Parameters on this object can be 
        /// modified. Calls to CreateEvolutionAlgorithm() make a copy of and use this object in whatever state it is in 
        /// at the time of the call.
        /// </summary>
        public NeatEvolutionAlgorithmParameters NeatEvolutionAlgorithmParameters
        {
            get { return _eaParams; }
        }

        /// <summary>
        /// Gets the NeatGenomeParameters to be used for the experiment. Parameters on this object can be modified. Calls
        /// to CreateEvolutionAlgorithm() make a copy of and use this object in whatever state it is in at the time of the call.
        /// </summary>
        public NeatGenomeParameters NeatGenomeParameters
        {
            get { return _neatGenomeParams; }
        }

        public ComplexityRegulationMode CurrentMode => throw new NotImplementedException();

        SharpNeat.Neat.ComplexityRegulation.IComplexityRegulationStrategy INeatExperiment<double>.ComplexityRegulationStrategy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }



        /// <summary>
        /// Initialize the experiment with some optional XML configutation data.
        /// </summary>
        public void Initialize(string name, XmlElement xmlConfig)
        {
            _name = name;
            _populationSize = 100;// XmlUtils.GetValueAsInt(xmlConfig, "PopulationSize");
            _specieCount = 20;//XmlUtils.GetValueAsInt(xmlConfig, "SpecieCount");
            _activationScheme = SharpNeat.Decoders.NetworkActivationScheme.CreateCyclicFixedTimestepsScheme(2);
            _complexityRegulationStr = 50.ToString(); //XmlUtils.TryGetValueAsString(xmlConfig, "ComplexityRegulationStrategy");
            _complexityThreshold = 50; //XmlUtils.TryGetValueAsInt(xmlConfig, "ComplexityThreshold");
            _description = 50.ToString();//XmlUtils.TryGetValueAsString(xmlConfig, "Description");
            //_parallelOptions = (int)50;//ExperimentUtils.ReadParallelOptions(xmlConfig);

            _eaParams = new NeatEvolutionAlgorithmParameters();
            _eaParams.SpecieCount = _specieCount;
            _neatGenomeParams = new NeatGenomeParameters();

        }

        /// <summary>
        /// Load a population of genomes from an XmlReader and returns the genomes in a new list.
        /// The genome2 factory for the genomes can be obtained from any one of the genomes.
        /// </summary>
        public List<NeatGenome> LoadPopulation(XmlReader xr)
        {
            //return LoadPopulation(xr, false, this.InputCount, this.OutputCount);
            return LoadPopulation(xr);
        }

        /// <summary>
        /// Save a population of genomes to an XmlWriter.
        /// </summary>
        public void SavePopulation(XmlWriter xw, IList<NeatGenome> genomeList)
        {
            // Writing node IDs is not necessary for NEAT.
            NeatGenomeXmlIO.WriteComplete(xw, genomeList, false);
        }

        /// <summary>
        /// Create a genome2 factory for the experiment.
        /// Create a genome2 factory with our neat genome2 parameters object and the appropriate number of input and output neuron genes.
        /// </summary>
        public IGenomeFactory<NeatGenome> CreateGenomeFactory()
        {
            if (_neatGenomeParams == null)
            {
                _neatGenomeParams = new NeatGenomeParameters();
                // Handle the case where _neatGenomeParams is not initialized
                //throw new InvalidOperationException("NeatGenomeParameters has not been initialized.");
            }

            return new NeatGenomeFactory(InputCount, OutputCount, _neatGenomeParams);
        }

        /// <summary>
        /// Create and return a NeatEvolutionAlgorithm object ready for running the NEAT algorithm/search. Various sub-parts
        /// of the algorithm are also constructed and connected up.
        /// This overload requires no parameters and uses the default population size.
        /// </summary>
        public SharpNeat.EvolutionAlgorithms.NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm()
        {
            return CreateEvolutionAlgorithm(DefaultPopulationSize);
        }

        /// <summary>
        /// Create and return a NeatEvolutionAlgorithm object ready for running the NEAT algorithm/search. Various sub-parts
        /// of the algorithm are also constructed and connected up.
        /// This overload accepts a population size parameter that specifies how many genomes to create in an initial randomly
        /// generated population.
        /// </summary>
        public SharpNeat.EvolutionAlgorithms.NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(int populationSize)
        {
            // Create a genome2 factory with our neat genome2 parameters object and the appropriate number of input and output neuron genes.
            IGenomeFactory<NeatGenome> genomeFactory = CreateGenomeFactory();

            // Create an initial population of randomly generated genomes.
            List<NeatGenome> genomeList = genomeFactory.CreateGenomeList(populationSize, 0);

            // Create evolution algorithm.
            return CreateEvolutionAlgorithm(genomeFactory, genomeList);
        }

        /// <summary>
        /// Create and return a NeatEvolutionAlgorithm object ready for running the NEAT algorithm/search. Various sub-parts
        /// of the algorithm are also constructed and connected up.
        /// This overload accepts a pre-built genome2 population and their associated/parent genome2 factory.
        /// </summary>
        public SharpNeat.EvolutionAlgorithms.NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeFactory<NeatGenome> genomeFactory, List<NeatGenome> genomeList)
        {
            // Create distance metric. Mismatched genes have a fixed distance of 10; for matched genes the distance is their weigth difference.
            IDistanceMetric distanceMetric = new ManhattanDistanceMetric(1.0, 0.0, 10.0);
            ISpeciationStrategy<NeatGenome> speciationStrategy = new ParallelKMeansClusteringStrategy<NeatGenome>(distanceMetric, _parallelOptions);

            // Create complexity regulation strategy.
            //IComplexityRegulationStrategy complexityRegulationStrategy =  ExperimentUtils.CreateComplexityRegulationStrategy(_complexityRegulationStr, _complexityThreshold);

            // Create the evolution algorithm.
            SharpNeat.EvolutionAlgorithms.NeatEvolutionAlgorithm<NeatGenome> ea = new(_eaParams, speciationStrategy, (SharpNeat.EvolutionAlgorithms.ComplexityRegulation.IComplexityRegulationStrategy)_parallelOptions);

            // Create genome2 decoder.
            SharpNeat.Core.IGenomeDecoder<NeatGenome, IBlackBox> genomeDecoder = new NeatGenomeDecoder(_activationScheme);

            // Create a genome2 list evaluator. This packages up the genome2 decoder with the genome2 evaluator.
            SharpNeat.Core.IGenomeListEvaluator<NeatGenome> genomeListEvaluator = new SharpNeat.Core.ParallelGenomeListEvaluator<NeatGenome, IBlackBox>(genomeDecoder, PhenomeEvaluator, _parallelOptions);

            // Wrap the list evaluator in a 'selective' evaulator that will only evaluate new genomes. That is, we skip re-evaluating any genomes
            // that were in the population in previous generations (elite genomes). This is determiend by examining each genome2's evaluation info object.
            if (!EvaluateParents)
                genomeListEvaluator = new SelectiveGenomeListEvaluator<NeatGenome>(genomeListEvaluator,
                                         SelectiveGenomeListEvaluator<NeatGenome>.CreatePredicate_OnceOnly());

            // Initialize the evolution algorithm.
            ea.Initialize(genomeListEvaluator, genomeFactory, genomeList);

            // Finished. Return the evolution algorithm
            return ea;
        }

        /// <summary>
        /// Creates a new genome decoder that can be used to convert a genome into a phenome.
        /// </summary>
        public SharpNeat.Core.IGenomeDecoder<NeatGenome, IBlackBox> CreateGenomeDecoder()
        {
            return new NeatGenomeDecoder(_activationScheme);
        }

        #endregion
    }
}
