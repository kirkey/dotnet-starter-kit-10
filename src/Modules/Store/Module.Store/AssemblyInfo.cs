using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FSH.Module.Store.Tests")]

internal static class AssemblyInfo
{
    public static Assembly Assembly => typeof(AssemblyInfo).Assembly;
}
