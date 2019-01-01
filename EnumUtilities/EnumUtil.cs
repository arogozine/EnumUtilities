using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EnumUtilities
{
    /// <summary>
    /// Enum Utilities.
    /// 
    /// Provides type safe Enum extension methods
    /// by "bypassing" .NET restrictions.
    /// </summary>
    public class EnumUtil<E>
        where E : struct, Enum, IComparable, IFormattable, IConvertible
    {
        private EnumUtil() { }

        #region Wrapped Functions

        /// <summary>
        /// Returns the underlying type of the specified enumeration.        
        /// Wrapper for <seealso cref="Enum.GetUnderlyingType(Type)"/>
        /// </summary>
        /// <typeparam name="T">The enumeration whose underlying type will be retrieved.</typeparam>
        /// <returns>The underlying type of T.</returns>
        public static Type GetUnderlyingType()
            => Enum.GetUnderlyingType(typeof(E));

        /// <summary>
        /// Returns the Name of a certain Enum Value.
        /// Wrapper for <seealso cref="Enum.GetName(Type, object)"/>
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>Name of the enum value</returns>
        public static string GetName(E value)
            => Enum.GetName(typeof(E), value);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// Wrapper for <seealso cref="Enum.Parse(Type, string)"/>
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>An object of type T whose value is represented by value.</returns>
        public static E Parse(string value)
            => (E)Enum.Parse(typeof(E), value);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.
        /// Wrapper for <seealso cref="Enum.Parse(Type, string, bool)"/>
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <returns>An object of type T whose value is represented by value.</returns>
        public static E Parse(string value, bool ignoreCase)
            => (E)Enum.Parse(typeof(E), value, ignoreCase);

        /// <summary>
        /// Retrieves an array of the values of the constants in a specified enumeration.        
        /// Wrapper for <seealso cref="Enum.GetValues(Type)"/>
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>An array that contains the values of the constants in T.</returns>
        public static E[] GetValues()
            => (E[])typeof(E).GetEnumValues();

        /// <summary>
        /// Retrieves an array of the names of the constants in a specified enumeration.        
        /// Wrapper for <seealso cref="Enum.GetNames(Type)"/>
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>A string array of the names of the constants in T.</returns>
        public static string[] GetNames()
            => typeof(E).GetEnumNames();

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// Wrapper for <seealso cref="Enum.TryParse{TEnum}(string, out TEnum)"/>
        /// </summary>
        /// <typeparam name="T">The enumeration type to which to converE value.</typeparam>
        /// <param name="value">
        /// The string representation of the enumeration name or underlying value to convert.
        /// </param>
        /// <param name="result">Contains an object of type <c>T</c> whose value is represented by <c>value</c> if the parse operation succeeds</param>
        /// <returns>
        /// true if the value parameter was converted successfully; otherwise, false.
        /// </returns>
        public static bool TryParse(string value, out E result)
            => Enum.TryParse(value, out result);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// A parameter specifies whether the operation is case-sensitive.
        /// The return value indicates whether the conversion succeeded.
        /// <seealso cref="Enum.TryParse{TEnum}(string, bool, out TEnum)"/>
        /// </summary>
        /// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to consider case.</param>
        /// <param name="result">Contains an object of type <c>T</c> whose value is represented by <c>value</c> if the parse operation succeeds</param>
        /// <returns>true if the value parameter was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string value, bool ignoreCase, out E result)
            => Enum.TryParse(value, ignoreCase, out result);

        #endregion

        #region Bitwise Operators

        /// <summary>
        /// Bitwise OR
        /// </summary>
        /// <param name="left">An enumeration value</param>
        /// <param name="right">An enumeration value</param>
        /// <returns><c>left | right</c></returns>
        public static E BitwiseOr(E left, E right)
            => EnumCompiledCache<E>.BitwiseOr(left, right);

        /// <summary>
        /// Bitwise AND
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="left">An enumeration value</param>
        /// <param name="right">An enumeration value</param>
        /// <returns><c>left &amp; right</c></returns>
        public static E BitwiseAnd(E left, E right)
            => EnumCompiledCache<E>.BitwiseAnd(left, right);

        /// <summary>
        /// Bitwise Exclusive OR
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="left">An enumeration value</param>
        /// <param name="right">An enumeration value</param>
        /// <returns><c>left ^ right</c></returns>
        public static E BitwiseExclusiveOr(E left, E right)
            => EnumCompiledCache<E>.BitwiseExclusiveOr(left, right);

        /// <summary>
        /// Bitwise NOT
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns><c>~value</c></returns>
        public static E BitwiseNot(E value)
            => EnumCompiledCache<E>.BitwiseNot(value);

        /// <summary>
        /// Checks whether a flag exists.
        /// This function does not check for <c>FlagsAttribute</c>.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <returns>
        /// true if the bit field or bit fields that are set in flag are also set in the current instance; otherwise, false.
        /// </returns>
        public static bool HasFlag(E value, E flag)
            => EnumCompiledCache<E>.HasFlag(value, flag);

        /// <summary>
        /// Returns the result of bitwise or between the passed in value and flag.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <returns>Value with the specified flag set</returns>
        public static E SetFlag(E value, E flag)
            => BitwiseOr(value, flag);

        /// <summary>
        /// Returns the result of bitwise and for value and bitwise not of the flag.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <returns>Value with the specified flag unset</returns>
        public static E UnsetFlag(E value, E flag)
            => EnumCompiledCache<E>.UnsetFlag(value, flag);

        /// <summary>
        /// Returns the result of bitwise exclusive or for the value and the flag.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <returns>Value with the specified flag toggled</returns>
        public static E ToggleFlag(E value, E flag)
            => BitwiseExclusiveOr(value, flag);

        /// <summary>
        /// Sets or Unsets a specified flag.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <param name="flagSet">true to set and false to unset</param>
        /// <returns>Value with the specified flag toggled</returns>
        public static E ToggleFlag(E value, E flag, bool flagSet)
            => (flagSet ? (Func<E, E, E>)SetFlag : UnsetFlag)(value, flag);

        #endregion

        #region IsDefined

        /// <summary>
        /// Checks if the value is valid for the enum.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        /// true if <paramref name="value"/> is valid for <typeparamref name="E"/>.
        /// </returns>
        /// <remarks>
        ///   <para>
        ///     For the sake of providing a typesafe API alternative to <c>Enum.IsDefined(typeof(E), value)</c>.
        ///   </para>
        /// </remarks>
        public static bool IsDefined(E value)
            => Array.IndexOf(ReflectionCache<E>.FieldValues, value) >= 0;

        /// <summary>
        /// Returns an indication whether a constant with a specified name exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="name">An enumeration value</param>
        /// <returns>
        /// <c>true</c> if <paramref name="name"/> is valid for <typeparamref name="E"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="name"/> is null
        /// </exception>
        public static bool IsDefined(string name)
            => Enum.IsDefined(typeof(E), name);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="E"/>.
        /// </returns>
        public static bool IsDefined(sbyte value)
            => EnumCompiledCache<E>.IsDefinedSByte(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="E"/>.
        /// </returns>
        public static bool IsDefined(byte value)
            => EnumCompiledCache<E>.IsDefinedByte(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="E"/>.
        /// </returns>
        public static bool IsDefined(ushort value)
            => EnumCompiledCache<E>.IsDefinedUInt16(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="E"/>.
        /// </returns>
        public static bool IsDefined(short value)
            => EnumCompiledCache<E>.IsDefinedInt16(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="E"/>.
        /// </returns>
        public static bool IsDefined(int value)
            => EnumCompiledCache<E>.IsDefinedInt32(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="E"/>.
        /// </returns>
        public static bool IsDefined(uint value)
            => EnumCompiledCache<E>.IsDefinedUInt32(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="E"/>.
        /// </returns>
        public static bool IsDefined(long value)
            => EnumCompiledCache<E>.IsDefinedInt64(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="E"/>.
        /// </returns>
        public static bool IsDefined(ulong value)
            => EnumCompiledCache<E>.IsDefinedUInt64(value);

        /// <summary>
        /// Returns an idication whether a constant with a specified value exists in a specified
        /// enumeration
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="E"/>.
        /// </returns>
        public static bool IsDefined(float value)
            => EnumCompiledCache<E>.IsDefinedSingle(value);

        /// <summary>
        /// Returns an idication whether a constant with a specified value exists in a specified
        /// enumeration
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="T"/>.
        /// </returns>
        public static bool IsDefined(double value)
            => EnumCompiledCache<E>.IsDefinedDouble(value);

        #endregion

        #region From Numeric Type

        /// <summary>
        /// Converts a byte to a specified enumeration <typeparamref name="E"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static E FromByte(byte value)
            => EnumCompiledCache<E>.FromByte(value);

        /// <summary>
        /// Converts a sbyte to a specified enumeration <typeparamref name="E"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static E FromSByte(sbyte value)
            => EnumCompiledCache<E>.FromSByte(value);

        /// <summary>
        /// Converts a short to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static E FromInt16(short value)
            => EnumCompiledCache<E>.FromInt16(value);

        /// <summary>
        /// Converts a ushort to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static E FromUInt16(ushort value)
            => EnumCompiledCache<E>.FromUInt16(value);

        /// <summary>
        /// Converts an int to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static E FromInt32(int value)
            => EnumCompiledCache<E>.FromInt32(value);

        /// <summary>
        /// Converts a uint to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static E FromUInt32(uint value)
            => EnumCompiledCache<E>.FromUInt32(value);

        /// <summary>
        /// Converts a long to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static E FromInt64(long value)
            => EnumCompiledCache<E>.FromInt64(value);

        /// <summary>
        /// Converts a ulong to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static E FromUInt64(ulong value)
            => EnumCompiledCache<E>.FromUInt64(value);

        /// <summary>
        /// Converts a float to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static E FromSingle(float value)
            => EnumCompiledCache<E>.FromSingle(value);

        /// <summary>
        /// Converts a double to a specified enumeration <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static E FromDouble(double value)
            => EnumCompiledCache<E>.FromDouble(value);

        #endregion

        #region To Numeric Type

        /// <summary>
        /// Converts the value of the specified enumeration to a byte.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>An 8-bit signed integer that is equivalent to the enumeration value</returns>
        public static byte ToByte(E value)
            => EnumCompiledCache<E>.ToByte(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a signed byte.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>An 8-bit unsigned integer that is cast equivalent to the enumeration value</returns>
        public static sbyte ToSByte(E value)
            => EnumCompiledCache<E>.ToSByte(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a short.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 16-bit signed integer that is cast equivalent to the enumeration value</returns>
        public static short ToInt16(E value)
            => EnumCompiledCache<E>.ToInt16(value);

        /// <summary>
        /// Converts the value of the specified enumeration to an unsigned short.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 16-bit unsigned integer that is cast equivalent to the enumeration value</returns>
        public static ushort ToUInt16(E value)
            => EnumCompiledCache<E>.ToUInt16(value);

        /// <summary>
        /// Converts the value of the specified enumeration to an int.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 32-bit signed integer that is cast equivalent to the enumeration value</returns>
        public static int ToInt32(E value)
            => EnumCompiledCache<E>.ToInt32(value);

        /// <summary>
        /// Converts the value of the specified enumeration to an unsigned int.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 32-bit unsigned integer that is cast equivalent to the enumeration value</returns>
        public static uint ToUInt32(E value)
            => EnumCompiledCache<E>.ToUInt32(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a long.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 64-bit signed integer that is cast equivalent to the enumeration value</returns>
        public static long ToInt64(E value)
            => EnumCompiledCache<E>.ToInt64(value);

        /// <summary>
        /// Converts the value of the specified enumeration to an unsigned long.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 64-bit unsigned integer that is cast equivalent to the enumeration value</returns>
        public static ulong ToUInt64(E value)
            => EnumCompiledCache<E>.ToUInt64(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a float.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A single precision floating point number that is case equivalent to the enumeration value</returns>
        public static float ToSingle(E value)
            => EnumCompiledCache<E>.ToSingle(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a double.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">An enumeration value</param>
        /// <returns>A double precision floating point number that is case equivalent to the enumeration value</returns>
        public static double ToDouble(E value)
            => EnumCompiledCache<E>.ToDouble(value);

        #endregion

        #region Reflection

        /// <summary>
        /// Retrieves a specific attribute on the enumeration type
        /// </summary>
        /// <typeparam name="Y">Attribute to Retrieve</typeparam>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>Custom Attribute on the Enum</returns>
        public static Y GetAttribute<Y>()           
            where Y : Attribute
        {
            return typeof(E).GetCustomAttribute<Y>();
        }

        /// <summary>
        /// Retrieves a specific attributes on the Enum
        /// </summary>
        /// <typeparam name="Y">Attributes to Retrieve</typeparam>
        /// <returns>Custom Attributes on the Enum</returns>
        public static IEnumerable<Y> GetAttributes<Y>()
            where Y : Attribute
        {
            return typeof(E).GetCustomAttributes<Y>();
        }

        /// <summary>
        /// Retrieves a specific attribute on an enum value
        /// </summary>
        /// <typeparam name="Y">Attribute Type</typeparam>
        /// <param name="value">Enum Value with the Attribute</param>
        /// <returns>Attribute Y on the enum value</returns>
        public static Y GetAttribute<Y>(E value)
           
            where Y : Attribute
        {
            FieldInfo field = typeof(E)
                .GetField(Enum.GetName(typeof(E), value), BindingFlags.Public | BindingFlags.Static);

            return field.GetCustomAttribute<Y>();
        }

        /// <summary>
        /// Retrieves specific attributes on an enum value
        /// </summary>
        /// <typeparam name="Y">Attribute Type</typeparam>
        /// <param name="value">Enum Value with Attributes</param>
        /// <returns>Attributes on the enum value</returns>
        public static IEnumerable<Y> GetAttributes<Y>(E value)
           
            where Y : Attribute
        {
            FieldInfo field = typeof(E)
                .GetField(Enum.GetName(typeof(E), value), BindingFlags.Public | BindingFlags.Static);

            return field.GetCustomAttributes<Y>();
        }

        /// <summary>
        /// Tells whether the Enum has a specific attribute.
        /// </summary>
        /// <typeparam name="Y">An enumeration type</typeparam>
        /// <typeparam name="T">Attribute Type</typeparam>
        /// <returns>Whether or not the enum has a certain attribute</returns>
        public static bool HasAttribute<Y>()
           
            where Y : Attribute
        {
            return typeof(E).IsDefined(typeof(Y), false);
        }

        /// <summary>
        /// Returns whether or not the Enum has the FlagsAttribute
        /// </summary>
        /// <returns>Whether or not the Enum has FlagsAttribute</returns>
        public static bool HasFlagsAttribute()
            => typeof(E).IsDefined(typeof(FlagsAttribute), false);


        /// <summary>
        /// Returns FieldInfo array for the enumeration.
        /// </summary>
        /// <returns>FieldInfo Array for the defined values in the Enumeration</returns>
        public static FieldInfo[] GetEnumFields()
            => ReflectionCache<E>.Fields;

        /// <summary>
        /// Generates a Dictionary of enumeration value to DescriptionAttribute.
        /// <seealso cref="DescriptionAttribute"/>
        /// </summary>
        /// <returns>Map of enumeration value to its description</returns>
        public static IReadOnlyDictionary<E, DescriptionAttribute> GetValueDescription()           
        {
            return GetValueAttribute<DescriptionAttribute>();
        }

        /// <summary>
        /// Generates a Dictionary of enum value to name and the description attribute.
        /// <seealso cref="DescriptionAttribute"/>
        /// </summary>
        /// <returns>Map of enumeration value to name and description</returns>
        public static IReadOnlyDictionary<E, NameAttribute<DescriptionAttribute>> GetValueNameDescription()
        {
            return GetValueNameAttribute<DescriptionAttribute>();
        }

        /// <summary>
        /// Generates a Dictionary of enum value to enum name and attributes
        /// </summary>
        /// <returns>Map of value to name and attributes</returns>
        public static IReadOnlyDictionary<E, Tuple<string, IEnumerable<Attribute>>> GetValueNameAttributes()           
        {
            FieldInfo[] fields = ReflectionCache<E>.Fields;

            var dict = new Dictionary<E, Tuple<string, IEnumerable<Attribute>>>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(E)field.GetRawConstantValue()] = Tuple.Create(field.Name, field.GetCustomAttributes());
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Value to Enum Value and Attribute
        /// </summary>
        /// <typeparam name="Y">The type of attribute</typeparam>
        /// <returns>Map of name to value and attribute</returns>
        public static IReadOnlyDictionary<string, ValueAttribute<E, Y>> GetNameValueAttribute<Y>()           
            where Y : Attribute
        {
            FieldInfo[] fields = ReflectionCache<E>.Fields;

            var dict = new Dictionary<string, ValueAttribute<E, Y>>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[field.Name] = new ValueAttribute<E, Y>((E)field.GetRawConstantValue(), field.GetCustomAttribute<Y>());
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Value to Name and Attribute
        /// </summary>
        /// <typeparam name="Y">The type of attribute</typeparam>
        /// <returns>Map of value to name and attribute</returns>
        public static IReadOnlyDictionary<E, NameAttribute<Y>> GetValueNameAttribute<Y>()           
            where Y : Attribute
        {
            FieldInfo[] fields = ReflectionCache<E>.Fields;
            var dict = new Dictionary<E, NameAttribute<Y>>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(E)field.GetRawConstantValue()] = new NameAttribute<Y>(field.Name, field.GetCustomAttribute<Y>());
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Value to Attribute Y
        /// </summary>
        /// <typeparam name="Y">The type of attribute</typeparam>
        /// <returns>Map of value to attribute</returns>
        public static IReadOnlyDictionary<E, Y> GetValueAttribute<Y>()
            where Y : Attribute
        {
            FieldInfo[] fields = ReflectionCache<E>.Fields;

            var dict = new Dictionary<E, Y>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(E)field.GetRawConstantValue()] = field.GetCustomAttribute<Y>();
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Name to Enum Value
        /// </summary>
        /// <returns>Map of name to value</returns>
        public static IReadOnlyDictionary<string, E> GetNameValue()
        {
            FieldInfo[] fields = ReflectionCache<E>.Fields;

            var dict = new Dictionary<string, E>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[field.Name] = (E)field.GetRawConstantValue();
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Value to Enum Name
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>Map of value to name</returns>
        public static IReadOnlyDictionary<E, string> GetValueName()
        {
            FieldInfo[] fields = ReflectionCache<E>.Fields;

            var dict = new Dictionary<E, string>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(E)field.GetRawConstantValue()] = field.Name;
            }

            return dict;
        }

        #endregion
    }
}