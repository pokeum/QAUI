using System.ComponentModel;

public enum DataType
{
    [Description("Int")] Int,
    [Description("Long")] Long,
    [Description("Float")] Float,
    [Description("Double")] Double,
    [Description("Boolean")] Boolean,
    [Description("String")] String,
}