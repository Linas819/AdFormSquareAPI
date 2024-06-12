using AdFormTask.Models;

namespace AdFormTask.Services
{
    public class SquareService
    {
        public string FormatPointString(string pointsString) // Formatting the imported coordinates
        {
            pointsString = pointsString.Substring(1);
            pointsString = pointsString.Remove(pointsString.Length - 1);
            return pointsString;
        }

        public List<Point> GetPoints(string[] coordinates) // Converts coordinates to Point objects, for easier
        {
            List<Point> points = new List<Point>();
            foreach (string coordinatesString in coordinates)
            {
                Point point = new Point();
                string coordinateString = FormatPointString(coordinatesString);
                string[] pointCoordinates = coordinateString.Split(";");
                point.x = Convert.ToInt32(pointCoordinates[0]);
                point.y = Convert.ToInt32(pointCoordinates[1]);
                points.Add(point);
            }
            return points;
        }

        public bool IsSquare(Point point1, Point point2, Point point3, Point point4) // Checks distance between adjecent square coordinates and checks if all distances are equal
        {
            bool isSquare = false;
            int distance1 = PointsDistance(point1, point2);
            int distance2 = PointsDistance(point2, point3);
            int distance3 = PointsDistance(point3, point4);
            int distance4 = PointsDistance(point1, point4);
            if (distance1 == distance2 && distance1 == distance3 && distance1 == distance4)
                isSquare = true;
            return isSquare;
        }

        public int PointsDistance(Point point1, Point point2) // Calculates a distance between two coordinate Points
        {
            return (point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y);
        }
    }
}
