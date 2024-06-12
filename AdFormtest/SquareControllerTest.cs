using AdFormTask.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AdFormTest
{
    public class SquareControllerTest
    {
        [Fact]
        public void GetSquaresTest()
        {
            string[] expectedSquaresCoordinates = { "[(-1;1), (1;1), (1;-1), (-1;-1)]", "[(-1;1), (0;0), (1;1), (0;2)]", "[(1;1), (0;0), (1;-1), (2;0)]", "[(0;0), (0;2), (2;2), (2;0)]" };
            int expectedSquaresCount = 4;
            SquaresController controller = new SquaresController(new AdFormTask.Services.SquareService());
            OkObjectResult result = controller.GetSquares("[(-1; 1), (1; 1), (1; -1), (-1; -1), (0; 0), (2; 2), (0; 2), (2; 0), (3; 0)]") as OkObjectResult;
            List<string> resultSquaresCoordinates = result.Value.GetType().GetProperty("squaresCoordinates").GetValue(result.Value) as List<string>;
            int resultSquaresCount = (int)result.Value.GetType().GetProperty("squaresCount").GetValue(result.Value);
            Assert.Equal(expectedSquaresCoordinates.ToList<string>(), resultSquaresCoordinates);
            Assert.Equal(expectedSquaresCount, resultSquaresCount);
        }

        [Fact]
        public void DeleteCoordinatePointTest() 
        {
            string expectedSquareCoordinates = "[(-1;1), (1;1), (1;-1), (-1;-1)]";
            SquaresController controller = new SquaresController(new AdFormTask.Services.SquareService());
            OkObjectResult result = controller.DeleteCoordinatePoint("[(-1;1), (1;1), (1;-1), (-1;-1), (0;0)]", 0, 0) as OkObjectResult;
            string newPointsString = (string)result.Value.GetType().GetProperty("newPointsString").GetValue(result.Value);
            Assert.Equal(expectedSquareCoordinates, newPointsString);
        }
        [Fact]
        public void PutCoordinateTest()
        {
            string expectedSquareCoordinates = "[(-1;1), (1;1), (1;-1), (-1;-1), (0;0)]";
            SquaresController controller = new SquaresController(new AdFormTask.Services.SquareService());
            OkObjectResult result = controller.PutCoordinate("[(-1;1), (1;1), (1;-1), (-1;-1)]", 0, 0) as OkObjectResult;
            string newPointsString = (string)result.Value.GetType().GetProperty("newPointsString").GetValue(result.Value);
            Assert.Equal(expectedSquareCoordinates, newPointsString);
        }
    }
}