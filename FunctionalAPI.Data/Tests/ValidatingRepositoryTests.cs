using Xunit;

namespace FunctionalAPI.Data.Tests
{
    public class ValidatingRepositoryTests
    {
        [Fact]
        public void Version_Validator__Succeeds_When_Versions_Match()
        {
            Assert.True(
                ValidatingRepository.ValidateOnVersion(new Domain.Item() { Version = 1 }, new Domain.Item() { Version = 1 })
                .HasValue);
        }

        [Fact]
        public void Version_Validator__Fails_When_Versions_Mismatch()
        {
            Assert.False(
                ValidatingRepository.ValidateOnVersion(new Domain.Item() { Version = 1 }, new Domain.Item() { Version = 2 })
                .HasValue);
        }
    }
}
