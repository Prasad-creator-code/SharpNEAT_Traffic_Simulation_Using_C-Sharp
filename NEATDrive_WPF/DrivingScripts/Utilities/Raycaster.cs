using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NEATDrive_WPF.DrivingScripts.Utilities
{
    public class RaycastResult
    {
        public Point CollisionPoint { get; set; }
        public double Distance { get; set; }
        public object CollidedObject { get; set; }
    }

    public class Raycaster
    {
        private Canvas carCanvas;
        private List<UIElement> environmentObjects;
        private Point startPoint;

        public Raycaster(Canvas carCanvas, List<UIElement> environmentObjects)
        {
            this.carCanvas = carCanvas;
            this.environmentObjects = environmentObjects;
            //this.carCanvas = ApplicationManager.instance.simWindow.HeroCar_Sprite;
            //this.environmentObjects.Add(ApplicationManager.instance.simWindow.PotHole_Obstacle_1);
            startPoint = new Point(carCanvas.ActualWidth / 2, carCanvas.ActualHeight / 2);
        }

        public List<RaycastResult> PerformRaycasts()
        {
            List<RaycastResult> results = new();

            // Ray directions and lengths
            Dictionary<Vector, double> rayDirections = new()
        {
            { new Vector(0, -1), 10 },  // Front
            { new Vector(0, 1), 5 },    // Back
            { new Vector(-1, 0), 5 },   // Left
            { new Vector(1, 0), 5 },    // Right
            { new Vector(-1, -1), 5 },  // Top-left
            { new Vector(1, -1), 5 }    // Top-right
        };

            foreach (var direction in rayDirections)
            {
                Point endPoint = startPoint + direction.Key * direction.Value;

                RaycastResult result = PerformRaycast(startPoint, endPoint);
                results.Add(result);
            }

            return results;
        }

        private RaycastResult PerformRaycast(Point startPoint, Point endPoint)
        {
            RaycastResult result = new();

            Line ray = new()
            {
                X1 = startPoint.X,
                Y1 = startPoint.Y,
                X2 = endPoint.X,
                Y2 = endPoint.Y,
                Stroke = Brushes.Red,
                StrokeThickness = 1
            };

            carCanvas.Children.Add(ray);

            foreach (UIElement environmentObject in environmentObjects)
            {
                GeometryHitTestParameters hitTestParams = new(new LineGeometry(startPoint, endPoint));
                VisualTreeHelper.HitTest(environmentObject, null, HitTestResultCallback, hitTestParams);
            }

            carCanvas.Children.Remove(ray);

            return result;
        }

        private HitTestResultBehavior HitTestResultCallback(HitTestResult hitTestResult)
        {
            if (hitTestResult.VisualHit != null && hitTestResult.VisualHit != carCanvas)
            {
                Point collisionPoint;

                if (hitTestResult is PointHitTestResult pointHitTestResult)
                {
                    collisionPoint = pointHitTestResult.PointHit;
                    Debug.WriteLine("AYEE GANDU!");
                }
                else
                {
                    // If the hit test result is not a PointHitTestResult, skip processing
                    return HitTestResultBehavior.Continue;
                }

                double distance = (startPoint - collisionPoint).Length;

                RaycastResult result = new()
                {
                    CollisionPoint = collisionPoint,
                    Distance = distance,
                    CollidedObject = hitTestResult.VisualHit

                };

                // Process the raycast result as needed

                return HitTestResultBehavior.Stop;
            }

            return HitTestResultBehavior.Continue;
        }
    }
}