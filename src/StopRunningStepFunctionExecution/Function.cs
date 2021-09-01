using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace StopRunningStepFunctionExecution
{
    public class Function
    {
        private static readonly AmazonStepFunctionsClient StepFunctionsClient = new AmazonStepFunctionsClient();
        
        public async Task<string> FunctionHandler(ILambdaContext context)
        {
            var sfListRequest = new ListExecutionsRequest
            {
                StateMachineArn = $"arn:aws:states:eu-central-1:xxxxxx:stateMachine:myStateMachine-{System.Environment.GetEnvironmentVariable("environment")}",
                MaxResults = 100,
                StatusFilter = ExecutionStatus.RUNNING
            };
            
            string token;
            ListExecutionsResponse jobs;
            var count = 0;

            do
            {
                jobs = await StepFunctionsClient.ListExecutionsAsync(sfListRequest);
                token = jobs.NextToken;
                sfListRequest.NextToken = token;
                foreach (var execution in jobs.Executions)
                {
                        try
                        {
                            var sfStopRequest = new StopExecutionRequest
                            {
                                ExecutionArn = execution.ExecutionArn,
                                Cause = "CLEAN_UP",
                                Error = "NONE"
                            };
                            await StepFunctionsClient.StopExecutionAsync(sfStopRequest);
                            count++;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                }
            } while (!string.IsNullOrEmpty(token) && jobs.Executions.Count > 0);
            
            return $"Executions stopped {count}";
        }
    }
}

