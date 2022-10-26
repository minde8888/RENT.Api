using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using FluentAssertions.Extensions;
using RENT.Data.Filter;
using RENT.Data.Helpers;
using RENT.Data.Interfaces;
using RENT.Domain.Entities;

namespace Rent.Xunit.Static
{
    public class StatikMethodsTest
    {
        [Fact]
        public void Random()
        {
            string unique36 = RandomString.RandString(36);
            string unique25 = RandomString.RandString(25);
            unique36.Should().HaveLength(36);
            unique25.Should().HaveLength(25);
        }
        [Fact]
        public void DateOffsetChack()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(1000);
            long unixTimeStampInSeconds = dateTimeOffset.ToUnixTimeSeconds();
            var expDate = UnixTimeStamp.UnixTimeStampToDateTime(unixTimeStampInSeconds);

            expDate.Should().BeMoreThan(1.Days()).Before(DateTime.UtcNow);
        }
        [Fact]
        public void Pagination()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization()
            {
                ConfigureMembers = true
            });
            var filter = fixture.Create<PaginationFilter>();
            var uri = fixture.Create<IUriService>();
            var uriService = uri.GetPageUri(filter, "test.com");
            uriService.AbsolutePath.Should().Be("/");

            List<Products> product = new();
            product.Add(new Products());
            var value = PaginationHelper.CreatePagedReponse(product, filter, 10, uri, "test.com");
            value.TotalRecords.Should().Be(10);
        }
    }
}