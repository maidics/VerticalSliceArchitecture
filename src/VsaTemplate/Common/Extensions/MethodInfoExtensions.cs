using System.Reflection;

namespace VsaTemplate.Common.Extensions;

// credit: https://github.com/jasontaylordev/CleanArchitecture
public static class MethodInfoExtensions
{
    private static readonly char[] AnonymousMethodChars = ['<', '>'];

    extension(MethodInfo method)
    {
        public bool IsAnonymous() => method.Name.Any(AnonymousMethodChars.Contains);
    }
}
