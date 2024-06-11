using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;

namespace AdFormTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [RequestTimeout(5000)]
    public class SquaresController : Controller
    {
        // !!!!!TEST COORDINATES!!!!!
        // [(-1;1), (1;1), (1;-1), (-1;-1)]

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
            string newPointsString = pointsString.Insert(pointsString.Length - 1, ", (" + x.ToString() + ";" + y.ToString() + ")");
            return Ok(new 
            {
                newPointsString = newPointsString
            });
        }
    }
}
