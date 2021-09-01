using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CreateStepFunctionExecution
{
    public class Function
    {
        private static readonly AmazonStepFunctionsClient StepFunctionsClient = new AmazonStepFunctionsClient();
        
        public async Task FunctionHandler(ILambdaContext context)
        {
            var tasks = Enumerable.Range(1, 1000).Select(async i =>
            {
                await StepFunctionsClient.StartExecutionAsync(new StartExecutionRequest
                {
                    StateMachineArn =
                        $"arn:aws:states:eu-central-1:xxxxxx:stateMachine:myStateMachine-{System.Environment.GetEnvironmentVariable("environment")}",
                    Input = "some payload",
                    Name = $"execution-{i}-{DateTime.Now.ToFileTime()}"
                });
            });
            await Task.WhenAll(tasks);
        }
    }
    
}