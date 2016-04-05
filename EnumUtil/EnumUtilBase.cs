using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EnumUtilities
{
    /// <summary>
    /// <para>Provides all the functions of EnumUtil, but requires two constrains - first must be System.Enum and second a specific enumeration type</para>
    /// <para>Do not use this class directly if possible; Go through <c>EnumUtil</c> instead.</para>
    /// </summary>
    /// <typeparam name="E">System.Enum</typeparam>
    public abstract class EnumUtilBase<E> : EnumUtilUnsafe<E>
        where E : class, IComparable, IFormattable, IConvertible
    {

    }

    /// <summary>
    /// Unsafe Version.
    /// Use only when necessary.
    /// </summary>
    /// <typeparam name="E">System.Enum</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class EnumUtilUnsafe<E>
        where E : IComparable, IFormattable, IConvertible
    {
        internal EnumUtilUnsafe() { }

        #region Wrapped Functions

        /// <summary>
        /// Returns the underlying type of the specified enumeration.        
        /// Wrapper for <seealso cref="Enum.GetUnderlyingType(Type)"/>
        /// </summary>
        /// <typeparam name="T">The enumeration whose underlying type will be retrieved.</typeparam>
        /// <returns>The underlying type of T.</returns>
        public static Type GetUnderlyingType<T>() where T : struct, E
            => Enum.GetUnderlyingType(typeof(T));

        /// <summary>
        /// Returns the Name of a certain Enum Value.
        /// Wrapper for <seealso cref="Enum.GetName(Type, object)"/>
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>Name of the enum value</returns>
        public static string GetName<T>(T value) where T : struct, E
            => Enum.GetName(typeof(T), value);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// Wrapper for <seealso cref="Enum.Parse(Type, string)"/>
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>An object of type T whose value is represented by value.</returns>
        public static T Parse<T>(string value) where T : struct, E
            => (T)Enum.Parse(typeof(T), value);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.
        /// Wrapper for <seealso cref="Enum.Parse(Type, string, bool)"/>
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <returns>An object of type T whose value is represented by value.</returns>
        public static T Parse<T>(string value, bool ignoreCase) where T : struct, E
            => (T)Enum.Parse(typeof(T), value, ignoreCase);

        /// <summary>
        /// Retrieves an array of the values of the constants in a specified enumeration.        
        /// Wrapper for <seealso cref="Enum.GetValues(Type)"/>
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>An array that contains the values of the constants in T.</returns>
        public static T[] GetValues<T>() where T : struct, E
            => (T[])Enum.GetValues(typeof(T));

        /// <summary>
        /// Retrieves an array of the names of the constants in a specified enumeration.        
        /// Wrapper for <seealso cref="Enum.GetNames(Type)"/>
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>A string array of the names of the constants in T.</returns>
        public static string[] GetNames<T>() where T : struct, E
            => Enum.GetNames(typeof(T));

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// Wrapper for <seealso cref="Enum.TryParse{TEnum}(string, out TEnum)"/>
        /// </summary>
        /// <typeparam name="T">The enumeration type to which to convert value.</typeparam>
        /// <param name="value">
        /// The string representation of the enumeration name or underlying value to convert.
        /// </param>
        /// <param name="result">Contains an object of type <c>T</c> whose value is represented by <c>value</c> if the parse operation succeeds</param>
        /// <returns>
        /// true if the value parameter was converted successfully; otherwise, false.
        /// </returns>
        public static bool TryParse<T>(string value, out T result) where T : struct, E
            => Enum.TryParse(value, out result);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// A parameter specifies whether the operation is case-sensitive.
        /// The return value indicates whether the conversion succeeded.
        /// <seealso cref="Enum.TryParse{TEnum}(string, bool, out TEnum)"/>
        /// </summary>
        /// <typeparam name="T">The enumeration type to which to convert value.</typeparam>
        /// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to consider case.</param>
        /// <param name="result">Contains an object of type <c>T</c> whose value is represented by <c>value</c> if the parse operation succeeds</param>
        /// <returns>true if the value parameter was converted successfully; otherwise, false.</returns>
        public static bool TryParse<T>(string value, bool ignoreCase, out T result)
            where T : struct, E
            => Enum.TryParse(value, ignoreCase, out result);

        #endregion

        #region Bitwise Operators

        /// <summary>
        /// Bitwise OR
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="left">An enumeration value</param>
        /// <param name="right">An enumeration value</param>
        /// <returns><c>left | right</c></returns>
        public static T BitwiseOr<T>(T left, T right) where T : struct, E
            => EnumCompiledCache<T>.BitwiseOr(left, right);

        /// <summary>
        /// Bitwise AND
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="left">An enumeration value</param>
        /// <param name="right">An enumeration value</param>
        /// <returns><c>left &amp; right</c></returns>
        public static T BitwiseAnd<T>(T left, T right) where T : struct, E
            => EnumCompiledCache<T>.BitwiseAnd(left, right);

        /// <summary>
        /// Bitwise Exclusive OR
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="left">An enumeration value</param>
        /// <param name="right">An enumeration value</param>
        /// <returns><c>left ^ right</c></returns>
        public static T BitwiseExclusiveOr<T>(T left, T right) where T : struct, E
            => EnumCompiledCache<T>.BitwiseExclusiveOr(left, right);

        /// <summary>
        /// Bitwise NOT
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns><c>~value</c></returns>
        public static T BitwiseNot<T>(T value) where T : struct, E
            => EnumCompiledCache<T>.BitwiseNot(value);

        /// <summary>
        /// Checks whether a flag exists.
        /// This function does not check for <c>FlagsAttribute</c>.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <returns>
        /// true if the bit field or bit fields that are set in flag are also set in the current instance; otherwise, false.
        /// </returns>
        public static bool HasFlag<T>(T value, T flag) where T : struct, E
            => EnumCompiledCache<T>.HasFlag(value, flag);

        /// <summary>
        /// Returns the result of bitwise or between the passed in value and flag.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <returns>Value with the specified flag set</returns>
        public static T SetFlag<T>(T value, T flag) where T : struct, E
            => BitwiseOr(value, flag);

        /// <summary>
        /// Returns the result of bitwise and for value and bitwise not of the flag.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <returns>Value with the specified flag unset</returns>
        public static T UnsetFlag<T>(T value, T flag) where T : struct, E
            => EnumCompiledCache<T>.UnsetFlag(value, flag);

        /// <summary>
        /// Returns the result of bitwise exclusive or for the value and the flag.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <returns>Value with the specified flag toggled</returns>
        public static T ToggleFlag<T>(T value, T flag) where T : struct, E
            => BitwiseExclusiveOr(value, flag);

        /// <summary>
        /// Sets or Unsets a specified flag.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <param name="flagSet">true to set and false to unset</param>
        /// <returns>Value with the specified flag toggled</returns>
        public static T ToggleFlag<T>(T value, T flag, bool flagSet) where T : struct, E
            => (flagSet ? (Func<T, T, T>)SetFlag : UnsetFlag)(value, flag);

        #endregion

        #region IsDefined

        /// <summary>
        /// Checks if the value is valid for the enum.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        /// true if <paramref name="value"/> is valid for <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        ///   <para>
        ///     For the sake of providing a typesafe API alternative to <c>Enum.IsDefined(typeof(T), value)</c>.
        ///   </para>
        /// </remarks>
        public static bool IsDefined<T>(T value) where T : struct, E
            => Array.IndexOf(ReflectionCache<T>.FieldValues, value) >= 0;

        /// <summary>
        /// Returns an indication whether a constant with a specified name exists in a specified
        /// enumeration.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="name">An enumeration value</param>
        /// <returns>
        /// <c>true</c> if <paramref name="name"/> is valid for <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="name"/> is null
        /// </exception>
        public static bool IsDefined<T>(string name) where T : struct, E
            => Enum.IsDefined(typeof(T), name);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="T"/>.
        /// </returns>
        public static bool IsDefined<T>(sbyte value) where T : struct, E
            => EnumCompiledCache<T>.IsDefinedSByte(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="T"/>.
        /// </returns>
        public static bool IsDefined<T>(byte value) where T : struct, E
            => EnumCompiledCache<T>.IsDefinedByte(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="T"/>.
        /// </returns>
        public static bool IsDefined<T>(ushort value) where T : struct, E
            => EnumCompiledCache<T>.IsDefinedUInt16(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="T"/>.
        /// </returns>
        public static bool IsDefined<T>(short value) where T : struct, E
            => EnumCompiledCache<T>.IsDefinedInt16(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="T"/>.
        /// </returns>
        public static bool IsDefined<T>(int value) where T : struct, E
            => EnumCompiledCache<T>.IsDefinedInt32(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="T"/>.
        /// </returns>
        public static bool IsDefined<T>(uint value) where T : struct, E
            => EnumCompiledCache<T>.IsDefinedUInt32(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="T"/>.
        /// </returns>
        public static bool IsDefined<T>(long value) where T : struct, E
            => EnumCompiledCache<T>.IsDefinedInt64(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="T"/>.
        /// </returns>
        public static bool IsDefined<T>(ulong value) where T : struct, E
            => EnumCompiledCache<T>.IsDefinedUInt64(value);

        #endregion

        #region From Numeric Type

        /// <summary>
        /// Converts a byte to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static T FromByte<T>(byte value) where T : struct, E
            => EnumCompiledCache<T>.FromByte(value);

        /// <summary>
        /// Converts a sbyte to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static T FromSByte<T>(sbyte value) where T : struct, E
            => EnumCompiledCache<T>.FromSByte(value);

        /// <summary>
        /// Converts a short to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static T FromInt16<T>(short value) where T : struct, E
            => EnumCompiledCache<T>.FromInt16(value);

        /// <summary>
        /// Converts a ushort to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static T FromUInt16<T>(ushort value) where T : struct, E
            => EnumCompiledCache<T>.FromUInt16(value);

        /// <summary>
        /// Converts an int to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static T FromInt32<T>(int value) where T : struct, E
            => EnumCompiledCache<T>.FromInt32(value);

        /// <summary>
        /// Converts a uint to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static T FromUInt32<T>(uint value) where T : struct, E
            => EnumCompiledCache<T>.FromUInt32(value);

        /// <summary>
        /// Converts a long to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static T FromInt64<T>(long value) where T : struct, E
            => EnumCompiledCache<T>.FromInt64(value);

        /// <summary>
        /// Converts a ulong to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static T FromUInt64<T>(ulong value) where T : struct, E
            => EnumCompiledCache<T>.FromUInt64(value);

        /// <summary>
        /// Converts a float to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static T FromSingle<T>(float value) where T : struct, E
            => EnumCompiledCache<T>.FromSingle(value);

        /// <summary>
        /// Converts a double to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static T FromDouble<T>(double value) where T : struct, E
            => EnumCompiledCache<T>.FromDouble(value);

        #endregion

        #region To Numeric Type

        /// <summary>
        /// Converts the value of the specified enumeration to a byte.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>An 8-bit signed integer that is equivalent to the enumeration value</returns>
        public static byte ToByte<T>(T value) where T : struct, E
            => EnumCompiledCache<T>.ToByte(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a signed byte.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>An 8-bit unsigned integer that is cast equivalent to the enumeration value</returns>
        public static sbyte ToSByte<T>(T value) where T : struct, E
            => EnumCompiledCache<T>.ToSByte(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a short.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 16-bit signed integer that is cast equivalent to the enumeration value</returns>
        public static short ToInt16<T>(T value) where T : struct, E
            => EnumCompiledCache<T>.ToInt16(value);

        /// <summary>
        /// Converts the value of the specified enumeration to an unsigned short.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 16-bit unsigned integer that is cast equivalent to the enumeration value</returns>
        public static ushort ToUInt16<T>(T value) where T : struct, E
            => EnumCompiledCache<T>.ToUInt16(value);

        /// <summary>
        /// Converts the value of the specified enumeration to an int.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 32-bit signed integer that is cast equivalent to the enumeration value</returns>
        public static int ToInt32<T>(T value) where T : struct, E
            => EnumCompiledCache<T>.ToInt32(value);

        /// <summary>
        /// Converts the value of the specified enumeration to an unsigned int.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 32-bit unsigned integer that is cast equivalent to the enumeration value</returns>
        public static uint ToUInt32<T>(T value) where T : struct, E
            => EnumCompiledCache<T>.ToUInt32(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a long.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 64-bit signed integer that is cast equivalent to the enumeration value</returns>
        public static long ToInt64<T>(T value) where T : struct, E
            => EnumCompiledCache<T>.ToInt64(value);

        /// <summary>
        /// Converts the value of the specified enumeration to an unsigned long.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 64-bit unsigned integer that is cast equivalent to the enumeration value</returns>
        public static ulong ToUInt64<T>(T value) where T : struct, E
            => EnumCompiledCache<T>.ToUInt64(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a float.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A single precision floating point number that is case equivalent to the enumeration value</returns>
        public static float ToSingle<T>(T value) where T : struct, E
            => EnumCompiledCache<T>.ToSingle(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a double.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A double precision floating point number that is case equivalent to the enumeration value</returns>
        public static double ToDouble<T>(T value) where T : struct, E
            => EnumCompiledCache<T>.ToDouble(value);

        #endregion

        #region Reflection

        /// <summary>
        /// Retrieves a specific attribute on the enumeration type
        /// </summary>
        /// <typeparam name="Y">Attribute to Retrieve</typeparam>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>Custom Attribute on the Enum</returns>
        public static Y GetAttribute<Y, T>()
            where T : struct, E
            where Y : Attribute
        {
            return typeof(T).GetCustomAttribute<Y>();
        }

        /// <summary>
        /// Retrieves a specific attributes on the Enum
        /// </summary>
        /// <typeparam name="Y">Attributes to Retrieve</typeparam>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>Custom Attributes on the Enum</returns>
        public static IEnumerable<Y> GetAttributes<Y, T>()
            where T : struct, E
            where Y : Attribute
        {
            return typeof(T).GetCustomAttributes<Y>();
        }

        /// <summary>
        /// Retrieves a specific attribute on an enum value
        /// </summary>
        /// <typeparam name="Y">Attribute Type</typeparam>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">Enum Value with the Attribute</param>
        /// <returns>Attribute Y on the enum value</returns>
        public static Y GetAttribute<Y, T>(T value)
            where T : struct, E
            where Y : Attribute
        {
            FieldInfo field = typeof(T)
                .GetField(Enum.GetName(typeof(T), value), BindingFlags.Public | BindingFlags.Static);

            return field.GetCustomAttribute<Y>();
        }

        /// <summary>
        /// Retrieves specific attributes on an enum value
        /// </summary>
        /// <typeparam name="Y">Attribute Type</typeparam>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">Enum Value with Attributes</param>
        /// <returns>Attributes on the enum value</returns>
        public static IEnumerable<Y> GetAttributes<Y, T>(T value)
            where T : struct, E
            where Y : Attribute
        {
            FieldInfo field = typeof(T)
                .GetField(Enum.GetName(typeof(T), value), BindingFlags.Public | BindingFlags.Static);

            return field.GetCustomAttributes<Y>();
        }

        /// <summary>
        /// Tells whether the Enum has a specific attribute.
        /// </summary>
        /// <typeparam name="Y">An enumeration type</typeparam>
        /// <typeparam name="T">Attribute Type</typeparam>
        /// <returns>Whether or not the enum has a certain attribute</returns>
        public static bool HasAttribute<Y, T>()
            where T : struct, E
            where Y : Attribute
        {
            return typeof(T).IsDefined(typeof(Y), false);
        }

        /// <summary>
        /// Returns whether or not the Enum has the FlagsAttribute
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>Whether or not the Enum has FlagsAttribute</returns>
        public static bool HasFlagsAttribute<T>() where T : struct, E
            => typeof(T).IsDefined(typeof(FlagsAttribute), false);


        /// <summary>
        /// Returns FieldInfo array for the enumeration.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>FieldInfo Array for the defined values in the Enumeration</returns>
        public static FieldInfo[] GetEnumFields<T>() where T : struct, E
            => ReflectionCache<T>.Fields;

        /// <summary>
        /// Generates a Dictionary of enumeration value to DescriptionAttribute.
        /// <seealso cref="DescriptionAttribute"/>
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>Map of enumeration value to its description</returns>
        public static IReadOnlyDictionary<T, DescriptionAttribute> GetValueDescription<T>()
            where T : struct, E
        {
            return GetValueAttribute<T, DescriptionAttribute>();
        }

        /// <summary>
        /// Generates a Dictionary of enum value to name and the description attribute.
        /// <seealso cref="DescriptionAttribute"/>
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>Map of enumeration value to name and description</returns>
        public static IReadOnlyDictionary<T, NameAttribute<DescriptionAttribute>> GetValueNameDescription<T>()
            where T : struct, E
        {
            return GetValueNameAttribute<T, DescriptionAttribute>();
        }

        /// <summary>
        /// Generates a Dictionary of enum value to enum name and attributes
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>Map of value to name and attributes</returns>
        public static IReadOnlyDictionary<T, Tuple<string, IEnumerable<Attribute>>> GetValueNameAttributes<T>()
            where T : struct, E
        {
            FieldInfo[] fields = ReflectionCache<T>.Fields;

            var dict = new Dictionary<T, Tuple<string, IEnumerable<Attribute>>>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(T)field.GetRawConstantValue()] = Tuple.Create(field.Name, field.GetCustomAttributes());
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Value to Enum Value and Attribute
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <typeparam name="Y">The type of attribute</typeparam>
        /// <returns>Map of name to value and attribute</returns>
        public static IReadOnlyDictionary<string, ValueAttribute<T, Y>> GetNameValueAttribute<T, Y>()
            where T : struct, E
            where Y : Attribute
        {
            FieldInfo[] fields = ReflectionCache<T>.Fields;

            var dict = new Dictionary<string, ValueAttribute<T, Y>>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[field.Name] = new ValueAttribute<T, Y>((T)field.GetRawConstantValue(), field.GetCustomAttribute<Y>());
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Value to Name and Attribute
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <typeparam name="Y">The type of attribute</typeparam>
        /// <returns>Map of value to name and attribute</returns>
        public static IReadOnlyDictionary<T, NameAttribute<Y>> GetValueNameAttribute<T, Y>()
            where T : struct, E
            where Y : Attribute
        {
            FieldInfo[] fields = ReflectionCache<T>.Fields;

            var dict = new Dictionary<T, NameAttribute<Y>>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(T)field.GetRawConstantValue()] = new NameAttribute<Y>(field.Name, field.GetCustomAttribute<Y>());
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Value to Attribute Y
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <typeparam name="Y">The type of attribute</typeparam>
        /// <returns>Map of value to attribute</returns>
        public static IReadOnlyDictionary<T, Y> GetValueAttribute<T, Y>()
            where T : struct, E
            where Y : Attribute
        {
            FieldInfo[] fields = ReflectionCache<T>.Fields;

            var dict = new Dictionary<T, Y>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(T)field.GetRawConstantValue()] = field.GetCustomAttribute<Y>();
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Name to Enum Value
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>Map of name to value</returns>
        public static IReadOnlyDictionary<string, T> GetNameValue<T>()
            where T : struct, E
        {
            FieldInfo[] fields = ReflectionCache<T>.Fields;

            var dict = new Dictionary<string, T>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[field.Name] = (T)field.GetRawConstantValue();
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Value to Enum Name
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>Map of value to name</returns>
        public static IReadOnlyDictionary<T, string> GetValueName<T>()
            where T : struct, E
        {
            FieldInfo[] fields = ReflectionCache<T>.Fields;

            var dict = new Dictionary<T, string>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(T)field.GetRawConstantValue()] = field.Name;
            }

            return dict;
        }

        #endregion
    }
}