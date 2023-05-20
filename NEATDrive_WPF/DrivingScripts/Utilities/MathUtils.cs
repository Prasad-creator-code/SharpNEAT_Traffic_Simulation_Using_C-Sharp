namespace NEATDrive_WPF.DrivingScripts.Utilities
{
    public static class MathUtils
    {
        public static double Lerp(double startValue, double endValue, double t)
        {
            return startValue + (endValue - startValue) * t;
        }
    }
}
