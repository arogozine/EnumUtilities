# Enum Utilities
Generic Enum Utility for .NET

- This library provides generic type safe enum operations - something that the Enum class lacks.
- Written because the .NET `HasFlag` function for an enumeration is very slow and does type checking AT RUNTIME.
- Supports all enumeration types from `Byte` to `UInt64`

### API
#### [Flag] Operations
```csharp
// Type Safe and Fast HasFlag
bool hasFlag = EnumUtil.HasFlag(YourEnum.Foo | YourEnum.Bar, YourEnum.Foo);
// Adding a Flag
yourEnum = EnumUtil.SetFlag(yourEnum, YourEnum.Foo);
// Removing a Flag
yourEnum = EnumUtil.UnsetFlag(yourEnum, YourEnum.Foo);
// Toggling a Flag
yourEnum = EnumUtil.ToggleFlag(yourEnum, YourEnum.Bar);
bool onOffToggle = SomeFunction();
yourEnum = EnumUtil.ToggleFLag(yourEnum, YourEnum.Bar, onOffToggle);
```
#### Bitwise Operations
```csharp
// &
YourEnum bitEnd = EnumUtil.BitwiseAnd(YourEnum.Foo, YourEnum.Bar);
// ^
YourEnum bitXor = EnumUtil.BitwiseExclusiveOr(YourEnum.Foo, YourEnum.Bar);
// |
YourEnum bitOr = EnumUtil.BitwiseOr(YourEnum.Foo, YourEnum.Bar);
// ~
YourEnum bitNot = EnumUtil.BitwiseNot(YourEnum.Foo);
```
#### Attributes, Values, and Names
```csharp
// Values and Names
string name = EnumUtil.GetName(YourEnum.Foo);
string[] names = EnumUtil.GetNames<YourEnum>();
YourEnum[] values = EnumUtil.GetValues<YourEnum>();
YourEnum fooEnum = EnumUtil.Parse<YourEnum>("Foo");
bool parsedOk = EnumUtil.TryParse("Foo", out fooEnum);
bool parsedOkIgnoreCase = EnumUtil.TryParse("foo", true, out fooEnum);
// Attributes on Enums
var attributeOnEnum = EnumUtil.GetAttribute<DescriptionAttribute, YourEnum>();
var attributeOnEnumValue = EnumUtil.GetAttribute<DescriptionAttribute, YourEnum>(YourEnum.Foo);
var attributesOnEnum = EnumUtil.GetAttributes<DescriptionAttribute, YourEnum>();
var attributesOnEnumValue = EnumUtil.GetAttributes<DescriptionAttribute, YourEnum>(YourEnum.Foo);
bool isFlags = EnumUtil.HasAttribute<FlagsAttribute, YourEnum>();
bool isFlagsShortCut = EnumUtil.HasFlagsAttribute<YourEnum>();
```
#### Other
```csharp
Type underlyingType = EnumUtil.GetUnderlyingType<YourEnum>();
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
