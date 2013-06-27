using System;
using NUnit.Framework.Constraints;

namespace Makrill.Tests
{
    internal class Json
    {
        internal static IResolveConstraint IsEqualTo(Object expected)
        {
            return new JsonEqualsConstraint(expected);
        }
    }
}