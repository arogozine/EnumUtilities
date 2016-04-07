# Enum Utilities
Generic Enum Utility for .NET

- This library provides generic type safe enum operations - something that the Enum class lacks.
- Written because the .NET `HasFlag` function for an enumeration is very slow and does type checking AT RUNTIME.
- Supports all enumeration types from `Byte` to `UInt64`

### API

### Type Safe Wrappers
```csharp
// Enum.GetUnderlyingType(typeof(T))
Type underlyingType = EnumUtil.GetUnderlyingType<YourEnum>();

// Enum.GetName(typeof(T), value)
string nameFoo = EnumUtil.GetName(YourEnum.Foo);

// (T)Enum.Parse(typeof(T), value)
YourEnum valueFoo = EnumUtil.Parse<YourEnum>("Foo");

// (T)Enum.Parse(typeof(T), value, ignoreCase)
YourEnum valueFoo2 = EnumUtil.Parse<YourEnum>("foo", ignoreCase: true);

// (T[])typeof(T).GetEnumValues()
YourEnum[] values = EnumUtil.GetValues<YourEnum>();

// typeof(T).GetEnumNames()
string[] names = EnumUtil.GetNames<YourEnum>();

// Enum.TryParse(value, out result)
YourEnum possibleValue;
bool success = EnumUtil.TryParse("foob", out possibleValue);

// Enum.TryParse(value, ignoreCase, out result)
YourEnum possibleValue2;
bool sucess2 = EnumUtil.TryParse("foo", true, out possibleValue2);

// Enum.IsDefined(typeof(T), name)
bool isDef2 = EnumUtil.IsDefined<YourEnum>("Foo");
```
### Bitwise Operations
```csharp
// a | b
YourEnum fooOrBar = EnumUtil.BitwiseOr(YourEnum.Foo, YourEnum.Bar);

// a & b
YourEnum fooAndBar = EnumUtil.BitwiseAnd(YourEnum.Foo, YourEnum.Bar);

// a ^ b
YourEnum fooXorBar = EnumUtil.BitwiseExclusiveOr(YourEnum.Foo, YourEnum.Bar);

// ~ a
YourEnum notBar = EnumUtil.BitwiseNot(YourEnum.Bar);
```
#### [Flag] Operations
```csharp
// Set Flag
YourEnum barSet = EnumUtil.SetFlag(default(YourEnum), YourEnum.Bar);

// Unset (Remove) Flag
YourEnum barUnset = EnumUtil.UnsetFlag(barSet, YourEnum.Bar);

// Toggle Flag On / Off
YourEnum fooSet = EnumUtil.ToggleFlag(barUnset, YourEnum.Foo);

// Toggle Flag based on boolean
YourEnum fooUnset = EnumUtil.ToggleFlag(fooSet, YourEnum.Foo, false);
```
#### Is Defined
```csharp
bool isDef0 = EnumUtil.IsDefined((YourEnum)2);
bool isDef1 = EnumUtil.IsDefined<YourEnum>("Foo");

// Passed in number types get converted automatically
// to the correct underlying type
bool isDef3 = EnumUtil.IsDefined<YourEnum>((byte)2);
bool isDef4 = EnumUtil.IsDefined<YourEnum>((sbyte)2);
bool isDef5 = EnumUtil.IsDefined<YourEnum>((short)2);
bool isDef6 = EnumUtil.IsDefined<YourEnum>((ushort)2);
bool isDef7 = EnumUtil.IsDefined<YourEnum>((int)2);
bool isDef8 = EnumUtil.IsDefined<YourEnum>((uint)2);
bool isDef9 = EnumUtil.IsDefined<YourEnum>((long)2);
bool isDef10 = EnumUtil.IsDefined<YourEnum>((ulong)2);
```
#### Conversion From A Value Type
```csharp
// Conversion from a value type to an enum type
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
#### Conversion To A Value Type
```csharp
// Conversion from an enum type to a value type
byte byteVal = EnumUtil.ToByte(YourEnum.Foo);
sbyte sbyteVal = EnumUtil.ToSByte(YourEnum.Foo);
short shortVal = EnumUtil.ToInt16(YourEnum.Foo);
ushort ushortVal = EnumUtil.ToUInt16(YourEnum.Foo);
int intVal = EnumUtil.ToInt32(YourEnum.Foo);
uint uintVal = EnumUtil.ToUInt32(YourEnum.Bar);
long longVal = EnumUtil.ToInt64(YourEnum.Foo);
ulong ulongVal = EnumUtil.ToUInt64(YourEnum.Foo);
float floatVal = EnumUtil.ToSingle(YourEnum.Bar);
double doubleVal = EnumUtil.ToDouble(YourEnum.Bar);
```
#### Reflected Information
```csharp
FlagsAttribute attr = EnumUtil.GetAttribute<FlagsAttribute, YourEnum>();
IEnumerable<DescriptionAttribute> attrs = EnumUtil.GetAttributes<DescriptionAttribute, YourEnum>();

DescriptionAttribute attr2 = EnumUtil.GetAttribute<DescriptionAttribute, YourEnum>(YourEnum.Bar);
IEnumerable<DescriptionAttribute> attrs3 = EnumUtil.GetAttributes<DescriptionAttribute, YourEnum>(YourEnum.Bar);

bool hasFlagsAttr = EnumUtil.HasAttribute<FlagsAttribute, YourEnum>();
bool hasFlagsShortcut = EnumUtil.HasFlagsAttribute<YourEnum>();

FieldInfo[] fiedls = EnumUtil.GetEnumFields<YourEnum>();

// Various Read Only Dictionaries
var valueDescription = EnumUtil.GetValueDescription<YourEnum>();
var valueNameDescription = EnumUtil.GetValueNameDescription<YourEnum>();
var valueNameAttributes = EnumUtil.GetValueNameAttributes<YourEnum>();
var nameValueAttribute = EnumUtil.GetNameValueAttribute<YourEnum, DescriptionAttribute>();
var valueNameAttribute = EnumUtil.GetValueNameAttribute<YourEnum, DescriptionAttribute>();
var valueAttribute = EnumUtil.GetValueAttribute<YourEnum, DescriptionAttribute>();
var nameValue = EnumUtil.GetNameValue<YourEnum>();
var valueName = EnumUtil.GetValueName<YourEnum>();
```


### Usage with Generics
You may find using `EnumUtil` to be impossible in a generic function.
If that is the case, please use the EnumUtilBase class instead,
```csharp
// Create a function with two
// generic constrains like this
private static void YourFunction<E, T>()
	where E : class, IComparable, IFormattable, IConvertible
    where T : struct, E
{
	// Call EnumUtilBase within the function
	T[] values = EnumUtilBase<E>.GetValues<T>();
}

public static void OtherFunction()
{
	// Call the generic function
	// specifying the first argument to be the Enum class
	// and the second argument your Enumeration
	YourFunction<Enum, YourEnum>();
}
```

Note: When consuming the library from F\# this is not necessary. You will be able to write generic functions using `EnumUtil` in F\#,

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

An enumeration can be cast down to the `abstract class Enum` (`Enum foo = YourEnum.Foo`).
Simultaneously, an enumeration is a `struct` and can be passed into functions and classes with a generic constraint of `struct`.
While C# does not allow a generic constraint of `where E : Enum`,
C# does allow a generic constraint where one generic type implements the other (`where T : E`)
thus we can constraint `E` to be a `class, IComparable, IFormattable, IConvertible` and `T` to be `struct, E`.
This allows generic code where you pass `Enum` as the first generic constraint and your enumeration type as the second constraint to `EnumUtilBase`.
You also can bypass needing to pass two constraints by having a derived class (`EnumUtil`) pass in `Enum` as the first constraint,
the only issue is that you can't use `EnumUtil` in generic C# code.

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
