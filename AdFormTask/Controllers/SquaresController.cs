using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using AdFormTask.Models;
using AdFormTask.Services;

namespace AdFormTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [RequestTimeout(5000)]
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
            pointsString = squareService.FormatPointString(pointsString);
            string[] coordinates = pointsString.Split(", ");
            List<Point> points = squareService.GetPoints(coordinates);
            foreach (Point point1 in points)
            {
                foreach (Point point2 in points)
                {
                    if (Object.Equals(point1, point2))
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
                                        if (squareCoordinates.Count == 0)
                                        {
                                            if (squareService.IsSquare(point1, point2, point3, point4))
                                                squareCoordinates.Add("[(" + point1.x.ToString() + ";" + point1.y.ToString() + "), ("
                                                    + point2.x.ToString() + ";" + point2.y.ToString() + "), (" + point3.x.ToString() + ";" + point3.y.ToString() + "), ("
                                                    + point4.x.ToString() + ";" + point4.y.ToString() + ")]");
                                            break;
                                        }
                                        foreach (string squareCoordinate in squareCoordinates)
                                        {
                                            if (squareCoordinate.IndexOf(point1.x.ToString() + ";" + point1.y.ToString()) > 0
                                                && squareCoordinate.IndexOf(point2.x.ToString() + ";" + point2.y.ToString()) > 0
                                                && squareCoordinate.IndexOf(point3.x.ToString() + ";" + point3.y.ToString()) > 0
                                                && squareCoordinate.IndexOf(point4.x.ToString() + ";" + point4.y.ToString()) > 0)
                                            {
                                                coordinatesAdded = true;
                                                break;
                                            }
                                        }
                                        if(coordinatesAdded == false && squareService.IsSquare(point1, point2, point3, point4))
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
            {
                squaresCoordinates = squareCoordinates,
                squaresCount = squareCoordinates.Count
            });
        }

        [HttpDelete]
        public IActionResult DeleteCoordinatePoint(string pointsString, int x, int y) 
        {
            string deletedPointCoordinateString = "(" + x.ToString() + ";" + y.ToString() + ")";
            int stringIndex = pointsString.IndexOf(deletedPointCoordinateString);
            if (stringIndex > 0) 
            {
                pointsString = pointsString.Replace(deletedPointCoordinateString, "");
                int middleRemovedIndex = pointsString.IndexOf(" , ");
                int firstRemovedIndex = pointsString.IndexOf("[, (");
                int lastRemovedIndex = pointsString.IndexOf(", ]");
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

        [HttpPut]
        public IActionResult PutCoordinate(string pointsString, int x, int y)
        {
            pointsString = pointsString.Insert(pointsString.Length - 1, ", (" + x.ToString() + ";" + y.ToString() + ")");
            return Ok(new 
            {
                newPointsString = pointsString
            });
        }
    }
}
