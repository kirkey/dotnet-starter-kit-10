using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FSH.Module.Catalog.Tests")]

internal static class AssemblyInfo
{
    public static Assembly Assembly => typeof(AssemblyInfo).Assembly;
}
