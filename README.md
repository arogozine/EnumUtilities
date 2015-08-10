# Enum Utilities
Generic Enum Utility for .NET

- This library provides generic type safe enum operations - something that the Enum class itself lacks.
- Written because the .NET HasFlag function for an enum is very slow and does type checking AT RUNTIME.

### Functionality
```csharp
            // Type Safe and Fast HasFlag
            bool hasFlag = EnumUtil.HasFlag(YourEnum.Foo | YourEnum.Bar, YourEnum.Foo);
            // Type Safe Bitwise Operators
            YourEnum bitEnd = EnumUtil.BitwiseAnd(YourEnum.Foo, YourEnum.Bar);
            YourEnum bitXor = EnumUtil.BitwiseExclusiveOr(YourEnum.Foo, YourEnum.Bar);
            YourEnum bitOr = EnumUtil.BitwiseOr(YourEnum.Foo, YourEnum.Bar);
            // Attributes on Enums
            var attributeOnEnum = EnumUtil.GetAttribute<DescriptionAttribute, YourEnum>();
            var attributeOnEnumValue = EnumUtil.GetAttribute<DescriptionAttribute, YourEnum>(YourEnum.Foo);
            var attributesOnEnum = EnumUtil.GetAttributes<DescriptionAttribute, YourEnum>();
            var attributesOnEnumValue = EnumUtil.GetAttributes<DescriptionAttribute, YourEnum>(YourEnum.Foo);
            bool isFlags = EnumUtil.HasAttribute<FlagsAttribute, YourEnum>();
            bool isFlagsShortCut = EnumUtil.HasFlagsAttribute<YourEnum>();
            // Values and Names
            string name = EnumUtil.GetName(YourEnum.Foo);
            string[] names = EnumUtil.GetNames<YourEnum>();
            YourEnum[] values = EnumUtil.GetValues<YourEnum>();
            YourEnum fooEnum = EnumUtil.Parse<YourEnum>("Foo");
            bool parsedOk = Enum.TryParse("Foo", out fooEnum);
            bool parsedOkIgnoreCase = Enum.TryParse("foo", true, out fooEnum);
            // Other
            Type underLyingType = EnumUtil.GetUnderlyingType<YourEnum>();
```
### Usage with Generics
You may find the above EnumUtil class to be impossible to use in your own generic code.
If that is the case, please use the EnumUtilBase class instead,
```csharp
            EnumUtilBase<YourEnum>.GetValues<YourEnum>();
```
Do note that EnumUtilBase is not type safe itself and will throw a runtime exception if a non-enum type is passed in as a generic argument.
