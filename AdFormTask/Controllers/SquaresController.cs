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
        // [(-1;1), (1;1), (1;-1), (-1;-1)]
        [HttpGet]
        public IActionResult GetSquares(string pointsString)
        {
            List<string> squareCoordinates = new List<string>();
            pointsString = squareService.FormatPointString(pointsString);
            string[] coordinates = pointsString.Split(", ");
            List<Point> points = squareService.GetPoints(coordinates);
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
