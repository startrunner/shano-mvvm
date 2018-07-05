using System;
using System.Diagnostics;

namespace ShanoMVVM.Infrastructure
{
    public static class Guard
    {
        public static void NotNull(params (object variable, string variablename)[] testables)
        {
            foreach ((object variable, string variablename) in testables)
            {
                NotNull(variable, variablename);
            }
        }
        public static void NotNull(object variable, string variableName = null)
        {
            if (variable is null) throw new NullReferenceException(
                $"Value or parameter {(!string.IsNullOrEmpty(variableName) ? $"{variableName} " : "")}cannot be null."
            );
        }
    }
}
