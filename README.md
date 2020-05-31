# Enum Utilities
Generic Enum Utility for .NET Standard

- This library provides generic type safe enum operations - something that the Enum class lacks.
- Written because the .NET `HasFlag` function for an enumeration is very slow and does type checking AT RUNTIME.
- Supports all enumeration types from `Byte` to `UInt64`

### API

### Type Safe Wrappers
```csharp
// Enum.GetUnderlyingType(typeof(T))
Type underlyingType = EnumUtil<YourEnum>.GetUnderlyingType();

// Enum.GetName(typeof(T), value)
string nameFoo = EnumUtil<YourEnum>.GetName(YourEnum.Foo);

// (T)Enum.Parse(typeof(T), value)
YourEnum valueFoo = EnumUtil<YourEnum>.Parse("Foo");

// (T)Enum.Parse(typeof(T), value, ignoreCase)
YourEnum valueFoo2 = EnumUtil<YourEnum>.Parse("foo", ignoreCase: true);

// (T[])typeof(T).GetEnumValues()
YourEnum[] values = EnumUtil<YourEnum>.GetValues();

// typeof(T).GetEnumNames()
string[] names = EnumUtil<YourEnum>.GetNames();

// Enum.TryParse(value, out result)
YourEnum possibleValue;
bool success = EnumUtil<YourEnum>.TryParse("foob", out possibleValue);

// Enum.TryParse(value, ignoreCase, out result)
YourEnum possibleValue2;
bool sucess2 = EnumUtil<YourEnum>.TryParse("foo", true, out possibleValue2);

// Enum.IsDefined(typeof(T), name)
bool isDef2 = EnumUtil<YourEnum>.IsDefined("Foo");
```
### Bitwise Operations
```csharp
// a | b
YourEnum fooOrBar = EnumUtil<YourEnum>.BitwiseOr(YourEnum.Foo, YourEnum.Bar);

// a & b
YourEnum fooAndBar = EnumUtil<YourEnum>.BitwiseAnd(YourEnum.Foo, YourEnum.Bar);

// a ^ b
YourEnum fooXorBar = EnumUtil<YourEnum>.BitwiseExclusiveOr(YourEnum.Foo, YourEnum.Bar);

// ~ a
YourEnum notBar = EnumUtil<YourEnum>.BitwiseNot(YourEnum.Bar);
```
#### [Flag] Operations
```csharp
// Has Flag?
bool hasFlag = EnumUtil<YourEnum>.HasFlag(YourEnum.Foo | YourEnum.Bar, YourEnum.Bar);

// Set Flag
YourEnum barSet = EnumUtil<YourEnum>.SetFlag(default(YourEnum), YourEnum.Bar);

// Unset (Remove) Flag
YourEnum barUnset = EnumUtil<YourEnum>.UnsetFlag(barSet, YourEnum.Bar);

// Toggle Flag On / Off
YourEnum fooSet = EnumUtil<YourEnum>.ToggleFlag(barUnset, YourEnum.Foo);

// Toggle Flag based on a passed in boolean
YourEnum fooUnset = EnumUtil<YourEnum>.ToggleFlag(fooSet, YourEnum.Foo, false);

// Checks whether the FlagsAttribute it defined on the Enum
// Note: Toggle, Set, Unset, and HasFlag functions do not ensure that
// FlagsAttribute is defined
bool hasFlagsShortcut = EnumUtil<YourEnum>.HasFlagsAttribute();
```
#### Is Defined
```csharp
bool enumValDefined = EnumUtil<YourEnum>.IsDefined((YourEnum)2);
bool enumNameDefined = EnumUtil<YourEnum>.IsDefined("Foo");

// Passed in number types get converted automatically
// to the correct underlying type
// unlike the vanilla Enum.IsDefined which throws an exception 
bool byteValDefined = EnumUtil<YourEnum>.IsDefined((byte)2);
bool sbyteValDefined = EnumUtil<YourEnum>.IsDefined((sbyte)2);
bool shortValDefined = EnumUtil<YourEnum>.IsDefined((short)2);
bool ushortValDefined = EnumUtil<YourEnum>.IsDefined((ushort)2);
bool intValDefined = EnumUtil<YourEnum>.IsDefined((int)2);
bool uintValDefined = EnumUtil<YourEnum>.IsDefined((uint)2);
bool longValDefined = EnumUtil<YourEnum>.IsDefined((long)2);
bool ulongValDefined = EnumUtil<YourEnum>.IsDefined((ulong)2);
bool floatValDefined = EnumUtil<YourEnum>.IsDefined((float)2);
bool doubleValDefined = EnumUtil<YourEnum>.IsDefined((double)2);
```
#### Conversion From A Number Type
```csharp
YourEnum val0 = EnumUtil.FromByte<YourEnum>(2);
YourEnum val1 = EnumUtil.FromSByte<YourEnum>(2);
YourEnum val2 = EnumUtil.FromInt16<YourEnum>(2);
YourEnum val3 = EnumUtil.FromUInt16<YourEnum>(2);
YourEnum val4 = EnumUtil.FromInt32<YourEnum>(2);
YourEnum val5 = EnumUtil.FromUInt32<YourEnum>(2);
YourEnum val6 = EnumUtil.FromInt64<YourEnum>(2L);
YourEnum val7 = EnumUtil.FromUInt64<YourEnum>(2UL);
YourEnum val8 = EnumUtil.FromSingle<YourEnum>(2f);
YourEnum val9 = EnumUtil.FromDouble<YourEnum>(2.0);
```
#### Conversion To A Number Type
```csharp
// Conversion from an enum type to a numeric type
byte byteVal = EnumUtil<YourEnum>.ToByte(YourEnum.Foo);
sbyte sbyteVal = EnumUtil<YourEnum>.ToSByte(YourEnum.Foo);
short shortVal = EnumUtil<YourEnum>.ToInt16(YourEnum.Foo);
ushort ushortVal = EnumUtil<YourEnum>.ToUInt16(YourEnum.Foo);
int intVal = EnumUtil<YourEnum>.ToInt32(YourEnum.Foo);
uint uintVal = EnumUtil<YourEnum>.ToUInt32(YourEnum.Bar);
long longVal = EnumUtil<YourEnum>.ToInt64(YourEnum.Foo);
ulong ulongVal = EnumUtil<YourEnum>.ToUInt64(YourEnum.Foo);
float floatVal = EnumUtil<YourEnum>.ToSingle(YourEnum.Bar);
double doubleVal = EnumUtil<YourEnum>.ToDouble(YourEnum.Bar);
```
#### Reflected Information
```csharp
// Shortcut for typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static)
FieldInfo[] fields = EnumUtil<YourEnum>.GetEnumFields();

// On the Enumeration itself
FlagsAttribute attr = EnumUtil<YourEnum>.GetAttribute<FlagsAttribute>();
IEnumerable<DescriptionAttribute> attrs = EnumUtil<YourEnum>.GetAttributes<DescriptionAttribute>();
bool hasFlagsAttr = EnumUtil<YourEnum>.HasAttribute<FlagsAttribute>();
bool hasFlagsShortcut = EnumUtil<YourEnum>.HasFlagsAttribute();

// On a field in the enumeration
DescriptionAttribute attr2 = EnumUtil<YourEnum>.GetAttribute<DescriptionAttribute>(YourEnum.Bar);
IEnumerable<DescriptionAttribute> attrs3 = EnumUtil<YourEnum>.GetAttributes<DescriptionAttribute>(YourEnum.Bar);

// Various Read Only Dictionaries
// with data about the members of an enumeration
var valueDescription = EnumUtil<YourEnum>.GetValueDescription();
var valueNameDescription = EnumUtil<YourEnum>.GetValueNameDescription();
var valueNameAttributes = EnumUtil<YourEnum>.GetValueNameAttributes();
var nameValueAttribute = EnumUtil<YourEnum>.GetNameValueAttribute<DescriptionAttribute>();
var valueNameAttribute = EnumUtil<YourEnum>.GetValueNameAttribute<DescriptionAttribute>();
var valueAttribute = EnumUtil<YourEnum>.GetValueAttribute<DescriptionAttribute>();
var nameValue = EnumUtil<YourEnum>.GetNameValue();
var valueName = EnumUtil<YourEnum>.GetValueName();
```
### Usage with Generics
## With C# 7.3
```csharp
// C# 7.3
private static void YourFunction<TEnum>()
	where TEnum : struct, Enum, IComparable, IFormattable, IConvertible
{
	// Call EnumUtilBase within the function
	TEnum[] values = EnumUtil<TEnum>.GetValues();
	// Your Code Here
}
```

## With F#
```fsharp
[<Flags>]
type FSFlagsEnum =
    | One = 1
    | Two = 2
    | Fourt = 4
    | Eight = 8

let isFlagsEnum<'enum when 'enum : (new : unit -> 'enum) and 'enum : struct and 'enum :> Enum>() = 
    EnumUtil.HasFlagsAttribute<'enum>()

let bitWiseOr a b = EnumUtil.BitwiseOr(a, b)

let three = bitWiseOr FSFlagsEnum.One FSFlagsEnum.Two
```

### FAQ

#### 1. How does the library work?

**Constraints**

The library uses C# 7.3 ability to constrain a generic to an Enum.

**Functions**

Many functions are just type safe wrappers aroung the built-in `Enum` class.
Others use Expressions (see below) or simple reflection.

**Bitwise Operations**

In .NET languages `ClassName<int>` and `ClassName<double>` do not share static state.
Enum Utilities uses this fact to generate and compile bitwise expressions in the static initializer.
An expression is created that casts down your enumeration to its underLying type (usually `Int32`).
So the bitwise operation happen on the underlying type. The compiled expression is then stored in a
`static readonly` field.

#### 2. Can I use this with an enumeration of type (`byte`, `long`, `short`, ...)?
Yes. The library supports all types of enumerations. See "Bitwise Operations" above.

#### 3. Are there any functional differences between the built-in `HasFlag(..)` and your `HasFlag`?
Enum Utilities does not check for `FlagsAttribute`.

### Installation
To use in your project, install the
[`EnumUtilities`](https://www.nuget.org/packages/EnumUtilities)
package.
