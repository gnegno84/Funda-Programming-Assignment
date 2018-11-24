using System;
using System.Threading.Tasks;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.ResilienceWrapper
{
    public interface IResiliencePolicyWrapper
    {
        Task<TRes> Execute<TE, TRes>(Func<Task<TE>> funcToWrap, Func<TE, TRes> funcToExecuteOnSuccess);
    }
}