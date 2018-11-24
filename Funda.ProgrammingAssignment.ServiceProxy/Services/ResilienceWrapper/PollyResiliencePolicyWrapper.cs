using System;
using System.Net;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.ServiceProxy.Exceptions;
using Polly;
using RestSharp;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.ResilienceWrapper
{
    public class PollyResiliencePolicyWrapper : IResiliencePolicyWrapper
    {
        public async Task<TRes> Execute<TE, TRes>(Func<Task<IRestResponse<TE>>> funcToWrap, Func<IRestResponse<TE>, TRes> funcToExecuteOnSuccess)
        {
            var execRes = await Policy
                .HandleResult<IRestResponse<TE>>(r => r.StatusCode == HttpStatusCode.Unauthorized)
                .Or<Exception>()
                .WaitAndRetryAsync(
                    6,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetryAsync: (result, span, retryCount) =>
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"**** Retrying caused by {result?.Result.StatusDescription} - Retry Number: {retryCount.Count} - Will try again in {span} seconds. Sorry for the delay!");
                        Console.ResetColor();
                        return Task.CompletedTask;
                    }
                ).ExecuteAndCaptureAsync(funcToWrap);

            if (execRes.Outcome == OutcomeType.Successful)
            {
                var response = execRes.Result;
                return funcToExecuteOnSuccess.Invoke(response);
            }

            throw new RemoteApiInvocationException($"Exception while trying to invoke api method. See inner exception for further details", execRes.FinalException);
        }
    }
}