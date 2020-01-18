using System;

namespace LiamHT.NullChecker.Core.Tests
{
    internal class TestableObject
    {
        public string FullName { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int MethodWithReturnType()
        {
            return 6;
        }
    }

    internal class OtherTestableObject
    {
        public string InvalidProperty { get; set; }
    }
}
