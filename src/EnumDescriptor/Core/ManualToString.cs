namespace EnumDescriptor.Core
{
    public static class ManualToString
    {
        public static string OptimizedToString(this Test testValue)
        {
            return testValue switch
            {
                Test.Value1 => "Value1",
                Test.Value2 => "Value2",
                Test.Value3 => "Value3",
                Test.Value4 => "Value4",
                Test.Value5 => "Value5",
                _ => throw new NotImplementedException()
            };
        }

        public static string ToStringNaive(this Test testValue)
        {
            return testValue.ToString();
        }
    }
}