namespace EnumDescriptor.Sample
{
    [DescribeEnum]
    public enum AnotherTestEnum
    {
        Test1,
        Test2,
        Test3
    }

    public class TestContainer
    {
        [DescribeEnum]
        public enum InternalEnum
        {
            Test1,
            Test2,
            Test3
        }
    }

    public class TestContainer1
    {
        public class TestContainer2
        {
            [DescribeEnum]
            public enum Internal2Enum
            {
                Test5,
                Test6,
                Test8
            }
        }
    }
}