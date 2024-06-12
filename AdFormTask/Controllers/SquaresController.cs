using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using AdFormTask.Models;
using AdFormTask.Services;

namespace AdFormTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [RequestTimeout(5000)] // Set timeout for 5 seconds
    public class SquaresController : Controller
    {
        private SquareService squareService;
        public SquaresController(SquareService squareService)
        {
            this.squareService = squareService;
        }
        // !!!!!TEST COORDINATES!!!!!
        // [(-1;1), (1;1), (1;-1), (-1;-1), (0;0), (2;2), (0;2), (2;0), (3;0)]
        [HttpGet]
        public IActionResult GetSquares(string pointsString)
        {
            List<string> squareCoordinates = new List<string>();
            pointsString = squareService.FormatPointString(pointsString); // Format the imported coordinates
            string[] coordinates = pointsString.Split(", "); // Split up the coordinates
            List<Point> points = squareService.GetPoints(coordinates); // Add the coordinates to a list of Points
            foreach (Point point1 in points) // Use cycles to test each coordinate
            {
                foreach (Point point2 in points)
                {
                    if (Object.Equals(point1, point2)) // Ensure that the tested coordinates are not duplicated
                        continue;
                    else
                    {
                        foreach (Point point3 in points)
                        {
                            if (Object.Equals(point1, point3) || Object.Equals(point2, point3))
                                continue;
                            else
                            {
                                foreach (Point point4 in points)
                                {
                                    if (Object.Equals(point1, point4) || Object.Equals(point2, point4) || Object.Equals(point3, point4))
                                        continue;
                                    else
                                    {
                                        bool coordinatesAdded = false;
                                        if (squareCoordinates.Count == 0) // If no square coordinates have been added, test immediatly
                                        {
                                            if (squareService.IsSquare(point1, point2, point3, point4))
                                                squareCoordinates.Add("[(" + point1.x.ToString() + ";" + point1.y.ToString() + "), ("
                                                    + point2.x.ToString() + ";" + point2.y.ToString() + "), (" + point3.x.ToString() + ";" + point3.y.ToString() + "), ("
                                                    + point4.x.ToString() + ";" + point4.y.ToString() + ")]");
                                            break; // If coordinates make up a square break the loop and begin testing new coordinate
                                        }
                                        foreach (string squareCoordinate in squareCoordinates)
                                        { // Check if the set of coordinates have not been added, so not to add a duplicate result. If added, break loop and check next coordinate
                                            if (squareCoordinate.IndexOf(point1.x.ToString() + ";" + point1.y.ToString()) > 0
                                                && squareCoordinate.IndexOf(point2.x.ToString() + ";" + point2.y.ToString()) > 0
                                                && squareCoordinate.IndexOf(point3.x.ToString() + ";" + point3.y.ToString()) > 0
                                                && squareCoordinate.IndexOf(point4.x.ToString() + ";" + point4.y.ToString()) > 0)
                                            {
                                                coordinatesAdded = true;
                                                break;
                                            }
                                        } // If coordinates have not been added and they make up a square, add them to the results list
                                        if (coordinatesAdded == false && squareService.IsSquare(point1, point2, point3, point4)) 
                                            squareCoordinates.Add("[(" + point1.x.ToString() + ";" + point1.y.ToString() + "), ("
                                                + point2.x.ToString() + ";" + point2.y.ToString() + "), (" + point3.x.ToString() + ";" + point3.y.ToString() + "), ("
                                                + point4.x.ToString() + ";" + point4.y.ToString() + ")]");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Ok(new
            { // Return the list of coordinates in the same format as importel coordinates.
                squaresCoordinates = squareCoordinates,
                squaresCount = squareCoordinates.Count // The sum of all squares i simply checked by counting how many results entries for Square coordinates are
            });
        }

        [HttpDelete] // Required the coordinates in expected format, x coordinate and y coordinate
        public IActionResult DeleteCoordinatePoint(string pointsString, int x, int y) 
        {
            string deletedPointCoordinateString = "(" + x.ToString() + ";" + y.ToString() + ")"; // Change the x and y coordinates to the expected coordinate format
            int stringIndex = pointsString.IndexOf(deletedPointCoordinateString);
            if (stringIndex > 0) // Check if such coordinates exist, if so, replace with empty string
            {
                pointsString = pointsString.Replace(deletedPointCoordinateString, "");
                // Index leftover symbols if the removal was from the first, last, or middle of the list
                int middleRemovedIndex = pointsString.IndexOf(" , ");
                int firstRemovedIndex = pointsString.IndexOf("[, (");
                int lastRemovedIndex = pointsString.IndexOf(", ]");
                // If such symbols exist replace them, so that they appear as the expected format
                if (middleRemovedIndex > 0)
                {
                    pointsString = pointsString.Replace(" , ", " ");
                }
                else if (firstRemovedIndex > -1) 
                {
                    pointsString = pointsString.Replace("[, (", "[(");
                }
                else if (lastRemovedIndex > 0) 
                {
                    pointsString = pointsString.Replace(", ]", "]");
                }
            }
            return Ok(new
            {
                newPointsString = pointsString
            });
        }

        [HttpPut] // Required the coordinates in expected format, x coordinate and y coordinate
        public IActionResult PutCoordinate(string pointsString, int x, int y)
        {
            // Insert the coordinates in expected format, just before the last element of the string
            pointsString = pointsString.Insert(pointsString.Length - 1, ", (" + x.ToString() + ";" + y.ToString() + ")");
            return Ok(new 
            {
                newPointsString = pointsString
            });
        }
    }
}
