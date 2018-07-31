using System;

namespace ShanoLibraries.MVVM.Infrastructure
{
    public static class Ensure
    {
        public static T NotNull<T>(T value, string valueName) where T : class =>
            value is null ? throw CreateNullReferenceException<T>(valueName) : value;

        public static T? NotNull<T>(T? value, string valueName) where T : struct =>
            value is null ? throw CreateNullReferenceException<T>(valueName, "?") : value;

        private static void Evaluate(this object o) { }

        private static NullReferenceException CreateNullReferenceException<T>(string valueName, string typeSuffix = "") =>
            new NullReferenceException(
                $"Value '{valueName}' must be of type {typeof(T).Name}{typeSuffix} cannot be null."
            );
    }
}
