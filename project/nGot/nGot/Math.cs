using System;

namespace nGot
{
    class Math
    {
        /// <summary>
        /// Clamp returns a number which is compared to a minimum and maximum allowable value.
        /// </summary>
        /// <typeparam name="T">A comparable value.</typeparam>
        /// <param name="value">The value to compare.</param>
        /// <param name="min">The minimum allowable result.</param>
        /// <param name="max">The maximum allowable result.</param>
        /// <returns></returns>
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            T result;

            // If the value is above or below the allowable values, constrain it to the closest allowable value.
            result = value.CompareTo(min) < 0 ? min : value;
            result = result.CompareTo(max) > 0 ? max : result;

            return result;
        }
    }
}
