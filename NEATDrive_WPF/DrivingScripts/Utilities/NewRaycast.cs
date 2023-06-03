using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class NewRaycast
{
    private Canvas carCanvas;
    private Canvas obstacleCanvas;
    private double maxRaycastLength;
    private FrameworkElement closestElement;
    private double closestDistance;

    public NewRaycast(Canvas car, Canvas obstacle, double maxRayLength)
    {
        carCanvas = car;
        obstacleCanvas = obstacle;
        maxRaycastLength = maxRayLength;
    }

    public void PerformRaycast()
    {
        ClearRaycastLines();

        // Get the center position of the carCanvas
        Point carCenter = new(carCanvas.ActualWidth / 2, carCanvas.ActualHeight / 2);

        // Create raycast directions (6 directions in this case)
        Point[] directions =
        {
            new Point(-1, -1), // Top Left
            new Point(0, -1), // Top
            new Point(1, -1), // Top Right
            new Point(1, 1), // Bottom Right
            new Point(0, 1), // Bottom
            new Point(-1, 1) // Bottom Left
        };

        foreach (Point direction in directions)
        {
            // Calculate the raycast endpoint based on the direction and max length
            Point endpoint = new(carCenter.X + direction.X * maxRaycastLength, carCenter.Y + direction.Y * maxRaycastLength);

            // Perform the hit test and get the closest intersected element
            GetClosestIntersectedElement(carCenter, endpoint);

            // Print the name of the intersected element
            if (closestElement != null)
            {
                Debug.WriteLine($"Intersected element: {closestElement.Tag}");
            }

            // Draw a debug line representing the raycast
            DrawRaycastLine(carCenter, endpoint, Colors.Red);
        }
    }

    private void GetClosestIntersectedElement(Point startPoint, Point endPoint)
    {
        // Convert the endpoint from carCanvas coordinates to obstacleCanvas coordinates
        Point transformedEndPoint = carCanvas.TransformToVisual(obstacleCanvas).Transform(endPoint);

        // Perform a hit test on the obstacleCanvas
        HitTestResult hitResult = VisualTreeHelper.HitTest(obstacleCanvas, transformedEndPoint);

        // Check if a hit was detected
        if (hitResult != null && hitResult.VisualHit is FrameworkElement visualElement && visualElement.Parent == obstacleCanvas)
        {
            // Calculate the intersection point between the ray and the hit element
            PointHitTestResult pointHitResult = hitResult as PointHitTestResult;
            Point hitPoint = pointHitResult?.PointHit ?? endPoint;

            // Calculate the distance between the start point and the hit point
            double distance = (hitPoint - startPoint).Length;

            // Update the closest element if it's null or the new distance is smaller

            closestElement = visualElement;
            closestDistance = distance;

        }
    }



    private void DrawRaycastLine(Point startPoint, Point endPoint, Color color)
    {
        // Calculate the direction vector
        Vector direction = endPoint - startPoint;

        // Normalize the direction vector
        direction.Normalize();

        // Calculate the endpoint based on the length
        Point endpoint = startPoint + direction * maxRaycastLength;

        // Create a line to represent the raycast
        Line line = new()
        {
            X1 = startPoint.X,
            Y1 = startPoint.Y,
            X2 = endpoint.X,
            Y2 = endpoint.Y,
            Stroke = new SolidColorBrush(color),
            StrokeThickness = 2
        };

        // Add the line to the carCanvas
        carCanvas.Children.Add(line);
    }

    private void ClearRaycastLines()
    {
        // Remove all existing lines from the carCanvas
        var raycastLines = carCanvas.Children.OfType<Line>().ToList();
        foreach (var line in raycastLines)
        {
            carCanvas.Children.Remove(line);
        }
    }
}
