﻿using Xunit;

namespace HotChocolate.Validation
{
    public class ArgumentUniquenessRuleTests
        : DocumentValidatorVisitorTestBase
    {
        public ArgumentUniquenessRuleTests()
            : base(services => services.AddArgumentsAreValidRule())
        {
        }

        [Fact]
        public void NoDuplicateArgument()
        {
            ExpectValid(@"
                query {
                    arguments {
                        ... goodNonNullArg
                    }
                }

                fragment goodNonNullArg on Arguments {
                    nonNullBooleanArgField(nonNullBooleanArg: true)
                }
            ");
        }

        [Fact]
        public void DuplicateArgument()
        {
            ExpectErrors(@"
                query {
                    arguments {
                        ... goodNonNullArg
                    }
                }

                fragment goodNonNullArg on Arguments {
                    nonNullBooleanArgField(nonNullBooleanArg: true, nonNullBooleanArg: true)
                }
            ",
            t => Assert.Equal(
                "More than one argument with the same name in an argument " +
                "set is ambiguous and invalid.",
                t.Message));
        }
    }
}
