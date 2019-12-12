using System;
using System.Threading.Tasks;
using Carter;

namespace CarterDemos.StatusCodeHandlerDemo
{
    public class RandomErrorModule : CarterModule
    {
        public RandomErrorModule()
        {
            Get("randomerror", (req, res) =>
            {
                var random = new Random();
                var next = random.Next(100);
                res.StatusCode = (next % 2 == 0) ? 200 : 500;
                return Task.CompletedTask;
            });
        }
    }
}