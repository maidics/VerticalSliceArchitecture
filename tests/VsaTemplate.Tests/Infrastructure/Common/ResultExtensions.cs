using VsaTemplate.Common.Models;

namespace VsaTemplate.Tests.Infrastructure.Common;

public static class ResultExtensions
{
    extension(Result)
    {
        public void ShouldBeFailed(ResultType failureType)
        {
            throw new NotImplementedException();
        }
    }
}
