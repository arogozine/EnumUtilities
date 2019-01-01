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
    public static class EnumUtil<TEnum>
        where TEnum : struct, Enum, IComparable, IFormattable, IConvertible
    {
        #region Wrapped Functions

        /// <summary>
        /// Returns the underlying type of the specified enumeration.        
        /// Wrapper for <seealso cref="Enum.GetUnderlyingType(Type)"/>
        /// </summary>
        /// <returns>The underlying type of T.</returns>
        public static Type GetUnderlyingType()
            => Enum.GetUnderlyingType(typeof(TEnum));

        /// <summary>
        /// Returns the Name of a certain Enum Value.
        /// Wrapper for <seealso cref="Enum.GetName(Type, object)"/>
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>Name of the enum value</returns>
        public static string GetName(TEnum value)
            => Enum.GetName(typeof(TEnum), value);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// Wrapper for <seealso cref="Enum.Parse(Type, string)"/>
        /// </summary>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>An object of type T whose value is represented by value.</returns>
        public static TEnum Parse(string value)
            => (TEnum)Enum.Parse(typeof(TEnum), value);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.
        /// Wrapper for <seealso cref="Enum.Parse(Type, string, bool)"/>
        /// </summary>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <returns>An object of type T whose value is represented by value.</returns>
        public static TEnum Parse(string value, bool ignoreCase)
            => (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);

        /// <summary>
        /// Retrieves an array of the values of the constants in a specified enumeration.        
        /// Wrapper for <seealso cref="Enum.GetValues(Type)"/>
        /// </summary>
        /// <returns>An array that contains the values of the constants in T.</returns>
        public static TEnum[] GetValues()
            => (TEnum[])typeof(TEnum).GetEnumValues();

        /// <summary>
        /// Retrieves an array of the names of the constants in a specified enumeration.        
        /// Wrapper for <seealso cref="Enum.GetNames(Type)"/>
        /// </summary>
        /// <returns>A string array of the names of the constants in T.</returns>
        public static string[] GetNames()
            => typeof(TEnum).GetEnumNames();

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// Wrapper for <seealso cref="Enum.TryParse{TEnum}(string, out TEnum)"/>
        /// </summary>
        /// <param name="value">
        /// The string representation of the enumeration name or underlying value to convert.
        /// </param>
        /// <param name="result">Contains an object of type <c>E</c> whose value is represented by <c>value</c> if the parse operation succeeds</param>
        /// <returns>
        /// true if the value parameter was converted successfully; otherwise, false.
        /// </returns>
        public static bool TryParse(string value, out TEnum result)
            => Enum.TryParse(value, out result);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// A parameter specifies whether the operation is case-sensitive.
        /// The return value indicates whether the conversion succeeded.
        /// <seealso cref="Enum.TryParse{TEnum}(string, bool, out TEnum)"/>
        /// </summary>
        /// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to consider case.</param>
        /// <param name="result">Contains an object of type <c>E</c> whose value is represented by <c>value</c> if the parse operation succeeds</param>
        /// <returns>true if the value parameter was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string value, bool ignoreCase, out TEnum result)
            => Enum.TryParse(value, ignoreCase, out result);

        #endregion

        #region Bitwise Operators

        /// <summary>
        /// Bitwise OR
        /// </summary>
        /// <param name="left">An enumeration value</param>
        /// <param name="right">An enumeration value</param>
        /// <returns><c>left | right</c></returns>
        public static TEnum BitwiseOr(TEnum left, TEnum right)
            => EnumCompiledCache<TEnum>.BitwiseOr(left, right);

        /// <summary>
        /// Bitwise AND
        /// </summary>
        /// <param name="left">An enumeration value</param>
        /// <param name="right">An enumeration value</param>
        /// <returns><c>left &amp; right</c></returns>
        public static TEnum BitwiseAnd(TEnum left, TEnum right)
            => EnumCompiledCache<TEnum>.BitwiseAnd(left, right);

        /// <summary>
        /// Bitwise Exclusive OR
        /// </summary>
        /// <param name="left">An enumeration value</param>
        /// <param name="right">An enumeration value</param>
        /// <returns><c>left ^ right</c></returns>
        public static TEnum BitwiseExclusiveOr(TEnum left, TEnum right)
            => EnumCompiledCache<TEnum>.BitwiseExclusiveOr(left, right);

        /// <summary>
        /// Bitwise NOT
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns><c>~value</c></returns>
        public static TEnum BitwiseNot(TEnum value)
            => EnumCompiledCache<TEnum>.BitwiseNot(value);

        /// <summary>
        /// Checks whether a flag exists.
        /// This function does not check for <c>FlagsAttribute</c>.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <returns>
        /// true if the bit field or bit fields that are set in flag are also set in the current instance; otherwise, false.
        /// </returns>
        public static bool HasFlag(TEnum value, TEnum flag)
            => EnumCompiledCache<TEnum>.HasFlag(value, flag);

        /// <summary>
        /// Returns the result of bitwise or between the passed in value and flag.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <returns>Value with the specified flag set</returns>
        public static TEnum SetFlag(TEnum value, TEnum flag)
            => BitwiseOr(value, flag);

        /// <summary>
        /// Returns the result of bitwise and for value and bitwise not of the flag.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <returns>Value with the specified flag unset</returns>
        public static TEnum UnsetFlag(TEnum value, TEnum flag)
            => EnumCompiledCache<TEnum>.UnsetFlag(value, flag);

        /// <summary>
        /// Returns the result of bitwise exclusive or for the value and the flag.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <returns>Value with the specified flag toggled</returns>
        public static TEnum ToggleFlag(TEnum value, TEnum flag)
            => BitwiseExclusiveOr(value, flag);

        /// <summary>
        /// Sets or Unsets a specified flag.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <param name="flag">An enumeration value</param>
        /// <param name="flagSet">true to set and false to unset</param>
        /// <returns>Value with the specified flag toggled</returns>
        public static TEnum ToggleFlag(TEnum value, TEnum flag, bool flagSet)
            => (flagSet ? (Func<TEnum, TEnum, TEnum>)SetFlag : UnsetFlag)(value, flag);

        #endregion

        #region IsDefined

        /// <summary>
        /// Checks if the value is valid for the enum.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        /// true if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </returns>
        /// <remarks>
        ///   <para>
        ///     For the sake of providing a typesafe API alternative to <c>Enum.IsDefined(typeof(E), value)</c>.
        ///   </para>
        /// </remarks>
        public static bool IsDefined(TEnum value)
            => Array.IndexOf(ReflectionCache<TEnum>.FieldValues, value) >= 0;

        /// <summary>
        /// Returns an indication whether a constant with a specified name exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="name">An enumeration value</param>
        /// <returns>
        /// <c>true</c> if <paramref name="name"/> is valid for <typeparamref name="TEnum"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="name"/> is null
        /// </exception>
        public static bool IsDefined(string name)
            => Enum.IsDefined(typeof(TEnum), name);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </returns>
        public static bool IsDefined(sbyte value)
            => EnumCompiledCache<TEnum>.IsDefinedSByte(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </returns>
        public static bool IsDefined(byte value)
            => EnumCompiledCache<TEnum>.IsDefinedByte(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </returns>
        public static bool IsDefined(ushort value)
            => EnumCompiledCache<TEnum>.IsDefinedUInt16(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </returns>
        public static bool IsDefined(short value)
            => EnumCompiledCache<TEnum>.IsDefinedInt16(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </returns>
        public static bool IsDefined(int value)
            => EnumCompiledCache<TEnum>.IsDefinedInt32(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </returns>
        public static bool IsDefined(uint value)
            => EnumCompiledCache<TEnum>.IsDefinedUInt32(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </returns>
        public static bool IsDefined(long value)
            => EnumCompiledCache<TEnum>.IsDefinedInt64(value);

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </returns>
        public static bool IsDefined(ulong value)
            => EnumCompiledCache<TEnum>.IsDefinedUInt64(value);

        /// <summary>
        /// Returns an idication whether a constant with a specified value exists in a specified
        /// enumeration
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </returns>
        public static bool IsDefined(float value)
            => EnumCompiledCache<TEnum>.IsDefinedSingle(value);

        /// <summary>
        /// Returns an idication whether a constant with a specified value exists in a specified
        /// enumeration
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </returns>
        public static bool IsDefined(double value)
            => EnumCompiledCache<TEnum>.IsDefinedDouble(value);

        #endregion

        #region From Numeric Type

        /// <summary>
        /// Converts a byte to a specified enumeration <typeparamref name="TEnum"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static TEnum FromByte(byte value)
            => EnumCompiledCache<TEnum>.FromByte(value);

        /// <summary>
        /// Converts a sbyte to a specified enumeration <typeparamref name="TEnum"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static TEnum FromSByte(sbyte value)
            => EnumCompiledCache<TEnum>.FromSByte(value);

        /// <summary>
        /// Converts a short to a specified enumeration <typeparamref name="TEnum"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static TEnum FromInt16(short value)
            => EnumCompiledCache<TEnum>.FromInt16(value);

        /// <summary>
        /// Converts a ushort to a specified enumeration <typeparamref name="TEnum"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static TEnum FromUInt16(ushort value)
            => EnumCompiledCache<TEnum>.FromUInt16(value);

        /// <summary>
        /// Converts an int to a specified enumeration <typeparamref name="TEnum"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static TEnum FromInt32(int value)
            => EnumCompiledCache<TEnum>.FromInt32(value);

        /// <summary>
        /// Converts a uint to a specified enumeration <typeparamref name="TEnum"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static TEnum FromUInt32(uint value)
            => EnumCompiledCache<TEnum>.FromUInt32(value);

        /// <summary>
        /// Converts a long to a specified enumeration <typeparamref name="TEnum"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static TEnum FromInt64(long value)
            => EnumCompiledCache<TEnum>.FromInt64(value);

        /// <summary>
        /// Converts a ulong to a specified enumeration <typeparamref name="TEnum"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static TEnum FromUInt64(ulong value)
            => EnumCompiledCache<TEnum>.FromUInt64(value);

        /// <summary>
        /// Converts a float to a specified enumeration <typeparamref name="TEnum"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static TEnum FromSingle(float value)
            => EnumCompiledCache<TEnum>.FromSingle(value);

        /// <summary>
        /// Converts a double to a specified enumeration <typeparamref name="TEnum"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns><paramref name="value"/> as the enumeration type</returns>
        public static TEnum FromDouble(double value)
            => EnumCompiledCache<TEnum>.FromDouble(value);

        #endregion

        #region To Numeric Type

        /// <summary>
        /// Converts the value of the specified enumeration to a byte.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>An 8-bit signed integer that is equivalent to the enumeration value</returns>
        public static byte ToByte(TEnum value)
            => EnumCompiledCache<TEnum>.ToByte(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a signed byte.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>An 8-bit unsigned integer that is cast equivalent to the enumeration value</returns>
        public static sbyte ToSByte(TEnum value)
            => EnumCompiledCache<TEnum>.ToSByte(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a short.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 16-bit signed integer that is cast equivalent to the enumeration value</returns>
        public static short ToInt16(TEnum value)
            => EnumCompiledCache<TEnum>.ToInt16(value);

        /// <summary>
        /// Converts the value of the specified enumeration to an unsigned short.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 16-bit unsigned integer that is cast equivalent to the enumeration value</returns>
        public static ushort ToUInt16(TEnum value)
            => EnumCompiledCache<TEnum>.ToUInt16(value);

        /// <summary>
        /// Converts the value of the specified enumeration to an int.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 32-bit signed integer that is cast equivalent to the enumeration value</returns>
        public static int ToInt32(TEnum value)
            => EnumCompiledCache<TEnum>.ToInt32(value);

        /// <summary>
        /// Converts the value of the specified enumeration to an unsigned int.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 32-bit unsigned integer that is cast equivalent to the enumeration value</returns>
        public static uint ToUInt32(TEnum value)
            => EnumCompiledCache<TEnum>.ToUInt32(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a long.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 64-bit signed integer that is cast equivalent to the enumeration value</returns>
        public static long ToInt64(TEnum value)
            => EnumCompiledCache<TEnum>.ToInt64(value);

        /// <summary>
        /// Converts the value of the specified enumeration to an unsigned long.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>A 64-bit unsigned integer that is cast equivalent to the enumeration value</returns>
        public static ulong ToUInt64(TEnum value)
            => EnumCompiledCache<TEnum>.ToUInt64(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a float.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>A single precision floating point number that is case equivalent to the enumeration value</returns>
        public static float ToSingle(TEnum value)
            => EnumCompiledCache<TEnum>.ToSingle(value);

        /// <summary>
        /// Converts the value of the specified enumeration to a double.
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <returns>A double precision floating point number that is case equivalent to the enumeration value</returns>
        public static double ToDouble(TEnum value)
            => EnumCompiledCache<TEnum>.ToDouble(value);

        #endregion

        #region Reflection

        /// <summary>
        /// Retrieves a specific attribute on the enumeration type
        /// </summary>
        /// <typeparam name="Y">Attribute to Retrieve</typeparam>
        /// <returns>Custom Attribute on the Enum</returns>
        public static Y GetAttribute<Y>()           
            where Y : Attribute
        {
            return typeof(TEnum).GetCustomAttribute<Y>();
        }

        /// <summary>
        /// Retrieves a specific attributes on the Enum
        /// </summary>
        /// <typeparam name="Y">Attributes to Retrieve</typeparam>
        /// <returns>Custom Attributes on the Enum</returns>
        public static IEnumerable<Y> GetAttributes<Y>()
            where Y : Attribute
        {
            return typeof(TEnum).GetCustomAttributes<Y>();
        }

        /// <summary>
        /// Retrieves a specific attribute on an enum value
        /// </summary>
        /// <typeparam name="Y">Attribute Type</typeparam>
        /// <param name="value">Enum Value with the Attribute</param>
        /// <returns>Attribute Y on the enum value</returns>
        public static Y GetAttribute<Y>(TEnum value)
           
            where Y : Attribute
        {
            FieldInfo field = typeof(TEnum)
                .GetField(Enum.GetName(typeof(TEnum), value), BindingFlags.Public | BindingFlags.Static);

            return field.GetCustomAttribute<Y>();
        }

        /// <summary>
        /// Retrieves specific attributes on an enum value
        /// </summary>
        /// <typeparam name="Y">Attribute Type</typeparam>
        /// <param name="value">Enum Value with Attributes</param>
        /// <returns>Attributes on the enum value</returns>
        public static IEnumerable<Y> GetAttributes<Y>(TEnum value)
           
            where Y : Attribute
        {
            FieldInfo field = typeof(TEnum)
                .GetField(Enum.GetName(typeof(TEnum), value), BindingFlags.Public | BindingFlags.Static);

            return field.GetCustomAttributes<Y>();
        }

        /// <summary>
        /// Tells whether the Enum has a specific attribute.
        /// </summary>
        /// <typeparam name="Y">An enumeration type</typeparam>
        /// <returns>Whether or not the enum has a certain attribute</returns>
        public static bool HasAttribute<Y>()
           
            where Y : Attribute
        {
            return typeof(TEnum).IsDefined(typeof(Y), false);
        }

        /// <summary>
        /// Returns whether or not the Enum has the FlagsAttribute
        /// </summary>
        /// <returns>Whether or not the Enum has FlagsAttribute</returns>
        public static bool HasFlagsAttribute()
            => typeof(TEnum).IsDefined(typeof(FlagsAttribute), false);


        /// <summary>
        /// Returns FieldInfo array for the enumeration.
        /// </summary>
        /// <returns>FieldInfo Array for the defined values in the Enumeration</returns>
        public static FieldInfo[] GetEnumFields()
            => ReflectionCache<TEnum>.Fields;

        /// <summary>
        /// Generates a Dictionary of enumeration value to DescriptionAttribute.
        /// <seealso cref="DescriptionAttribute"/>
        /// </summary>
        /// <returns>Map of enumeration value to its description</returns>
        public static IReadOnlyDictionary<TEnum, DescriptionAttribute> GetValueDescription()           
        {
            return GetValueAttribute<DescriptionAttribute>();
        }

        /// <summary>
        /// Generates a Dictionary of enum value to name and the description attribute.
        /// <seealso cref="DescriptionAttribute"/>
        /// </summary>
        /// <returns>Map of enumeration value to name and description</returns>
        public static IReadOnlyDictionary<TEnum, NameAttribute<DescriptionAttribute>> GetValueNameDescription()
        {
            return GetValueNameAttribute<DescriptionAttribute>();
        }

        /// <summary>
        /// Generates a Dictionary of enum value to enum name and attributes
        /// </summary>
        /// <returns>Map of value to name and attributes</returns>
        public static IReadOnlyDictionary<TEnum, Tuple<string, IEnumerable<Attribute>>> GetValueNameAttributes()           
        {
            FieldInfo[] fields = ReflectionCache<TEnum>.Fields;

            var dict = new Dictionary<TEnum, Tuple<string, IEnumerable<Attribute>>>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(TEnum)field.GetRawConstantValue()] = Tuple.Create(field.Name, field.GetCustomAttributes());
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Value to Enum Value and Attribute
        /// </summary>
        /// <typeparam name="Y">The type of attribute</typeparam>
        /// <returns>Map of name to value and attribute</returns>
        public static IReadOnlyDictionary<string, ValueAttribute<TEnum, Y>> GetNameValueAttribute<Y>()           
            where Y : Attribute
        {
            FieldInfo[] fields = ReflectionCache<TEnum>.Fields;

            var dict = new Dictionary<string, ValueAttribute<TEnum, Y>>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[field.Name] = new ValueAttribute<TEnum, Y>((TEnum)field.GetRawConstantValue(), field.GetCustomAttribute<Y>());
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Value to Name and Attribute
        /// </summary>
        /// <typeparam name="Y">The type of attribute</typeparam>
        /// <returns>Map of value to name and attribute</returns>
        public static IReadOnlyDictionary<TEnum, NameAttribute<Y>> GetValueNameAttribute<Y>()           
            where Y : Attribute
        {
            FieldInfo[] fields = ReflectionCache<TEnum>.Fields;
            var dict = new Dictionary<TEnum, NameAttribute<Y>>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(TEnum)field.GetRawConstantValue()] = new NameAttribute<Y>(field.Name, field.GetCustomAttribute<Y>());
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Value to Attribute Y
        /// </summary>
        /// <typeparam name="Y">The type of attribute</typeparam>
        /// <returns>Map of value to attribute</returns>
        public static IReadOnlyDictionary<TEnum, Y> GetValueAttribute<Y>()
            where Y : Attribute
        {
            FieldInfo[] fields = ReflectionCache<TEnum>.Fields;

            var dict = new Dictionary<TEnum, Y>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(TEnum)field.GetRawConstantValue()] = field.GetCustomAttribute<Y>();
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Name to Enum Value
        /// </summary>
        /// <returns>Map of name to value</returns>
        public static IReadOnlyDictionary<string, TEnum> GetNameValue()
        {
            FieldInfo[] fields = ReflectionCache<TEnum>.Fields;

            var dict = new Dictionary<string, TEnum>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[field.Name] = (TEnum)field.GetRawConstantValue();
            }

            return dict;
        }

        /// <summary>
        /// Generates a Dictionary of Enum Value to Enum Name
        /// </summary>
        /// <returns>Map of value to name</returns>
        public static IReadOnlyDictionary<TEnum, string> GetValueName()
        {
            FieldInfo[] fields = ReflectionCache<TEnum>.Fields;

            var dict = new Dictionary<TEnum, string>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(TEnum)field.GetRawConstantValue()] = field.Name;
            }

            return dict;
        }

        #endregion
    }
}