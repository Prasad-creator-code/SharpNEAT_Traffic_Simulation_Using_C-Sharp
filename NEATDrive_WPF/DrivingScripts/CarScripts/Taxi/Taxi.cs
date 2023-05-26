namespace NEATDrive_WPF.DrivingScripts.CarScripts
{
    class Taxi : Vehicle
    {
        public enum TaxiState
        {
            Idle = 0,
            Finding = 1,
            PedEntering = 2,
            DestinationSeeking = 3,
        }

    }
}
