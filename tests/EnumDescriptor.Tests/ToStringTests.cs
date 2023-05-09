using EnumDescriptor.Core;

namespace EnumDescriptor.Tests;

public class ToStringTests
{
    [Fact]
    public void TestFasterToString()
    {
        const string value1String = "Value1";
        const string value2String = "Value2";
        const string value3String = "Value3";

        var value1GeneratedString = Test.Value1.OptimizedToString();
        var value2GeneratedString = Test.Value2.OptimizedToString();
        var value3GeneratedString = Test.Value3.OptimizedToString();

        Assert.Equal(value1String, value1GeneratedString);
        Assert.Equal(value2String, value2GeneratedString);
        Assert.Equal(value3String, value3GeneratedString);
    }

    [Fact]
    public void TestNaiveToString()
    {
        const string value1String = "Value1";
        const string value2String = "Value2";
        const string value3String = "Value3";

        var value1GeneratedString = Test.Value1.ToStringNaive();
        var value2GeneratedString = Test.Value2.ToStringNaive();
        var value3GeneratedString = Test.Value3.ToStringNaive();

        Assert.Equal(value1String, value1GeneratedString);
        Assert.Equal(value2String, value2GeneratedString);
        Assert.Equal(value3String, value3GeneratedString);
    }
}