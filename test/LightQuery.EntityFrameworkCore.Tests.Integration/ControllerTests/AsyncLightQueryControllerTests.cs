using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightQuery.Client;
using LightQuery.IntegrationTestsServer;
using Xunit;

namespace LightQuery.EntityFrameworkCore.Tests.Integration.ControllerTests
{
    public class AsyncLightQueryControllerTests : ControllerTestBase
    {
        [Fact]
        public async Task DoesNotPaginateWithoutQuery()
        {
            var url = "AsyncLightQuery";
            var actualResponse = await GetResponse<List<User>>(url);
            Assert.NotNull(actualResponse);
            Assert.IsType<List<User>>(actualResponse);
        }

        [Fact]
        public async Task SortById()
        {
            var url = "AsyncLightQuery?sort=id";
            var actualResponse = await GetResponse<List<User>>(url);
            for (var i = 1; i < actualResponse.Count; i++)
            {
                var previousValueIsSmaller = actualResponse[i].Id > actualResponse[i - 1].Id;
                Assert.True(previousValueIsSmaller);
            }
        }

        [Fact]
        public async Task SortByIdWithCamelCase()
        {
            var url = "AsyncLightQuery?sort=Id";
            var actualResponse = await GetResponse<List<User>>(url);
            for (var i = 1; i < actualResponse.Count; i++)
            {
                var previousValueIsSmaller = actualResponse[i].Id > actualResponse[i - 1].Id;
                Assert.True(previousValueIsSmaller);
            }
        }

        [Fact]
        public async Task SortByIdDescending()
        {
            var url = "AsyncLightQuery?sort=id desc";
            var actualResponse = await GetResponse<List<User>>(url);
            for (var i = 1; i < actualResponse.Count; i++)
            {
                var previousValueIsSmaller = actualResponse[i].Id > actualResponse[i-1].Id;
                Assert.False(previousValueIsSmaller);
            }
        }

        [Fact]
        public async Task SortByUserName()
        {
            var url = "AsyncLightQuery?sort=userName";
            var actualResponse = await GetResponse<List<User>>(url);
            for (var i = 1; i < actualResponse.Count; i++)
            {
                var previousValueIsSmaller = actualResponse[i].UserName.CompareTo(actualResponse[i - 1].UserName) > 0;
                Assert.True(previousValueIsSmaller);
            }
        }

        [Fact]
        public async Task SortByUserNameWithCamelCase()
        {
            var url = "AsyncLightQuery?sort=UserName";
            var actualResponse = await GetResponse<List<User>>(url);
            for (var i = 1; i < actualResponse.Count; i++)
            {
                var previousValueIsSmaller = actualResponse[i].UserName.CompareTo(actualResponse[i - 1].UserName) > 0;
                Assert.True(previousValueIsSmaller);
            }
        }

        [Fact]
        public async Task SortByUserNameDescending()
        {
            var url = "AsyncLightQuery?sort=userName desc";
            var actualResponse = await GetResponse<List<User>>(url);
            for (var i = 1; i < actualResponse.Count; i++)
            {
                var previousValueIsSmaller = actualResponse[i].UserName.CompareTo(actualResponse[i - 1].UserName) > 0;
                Assert.False(previousValueIsSmaller);
            }
        }

        [Fact]
        public async Task SortByUserNameDescendingWithCamelCase()
        {
            var url = "AsyncLightQuery?sort=UserName desc";
            var actualResponse = await GetResponse<List<User>>(url);
            for (var i = 1; i < actualResponse.Count; i++)
            {
                var previousValueIsSmaller = actualResponse[i].UserName.CompareTo(actualResponse[i - 1].UserName) > 0;
                Assert.False(previousValueIsSmaller);
            }
        }

        [Fact]
        public async Task SortByEmail()
        {
            var url = "AsyncLightQuery?sort=email";
            var actualResponse = await GetResponse<List<User>>(url);
            for (var i = 1; i < actualResponse.Count; i++)
            {
                var previousValueIsSmaller = actualResponse[i].Email.CompareTo(actualResponse[i - 1].Email) > 0;
                Assert.True(previousValueIsSmaller);
            }
        }

        [Fact]
        public async Task SortByEmailDescending()
        {
            var url = "AsyncLightQuery?sort=email desc";
            var actualResponse = await GetResponse<List<User>>(url);
            for (var i = 1; i < actualResponse.Count; i++)
            {
                var previousValueIsSmaller = actualResponse[i].Email.CompareTo(actualResponse[i - 1].Email) > 0;
                Assert.False(previousValueIsSmaller);
            }
        }

        [Fact]
        public async Task DontSortWithoutSortParameter()
        {
            var url = "AsyncLightQuery";
            var response1 = await GetResponse<List<User>>(url);
            var response2 = await GetResponse<List<User>>(url);
            var response3 = await GetResponse<List<User>>(url);
            var response4 = await GetResponse<List<User>>(url);
            Func<IEnumerable<User>, string> aggregateEmails = users =>
                users
                    .Select(u => u.Email)
                    .Aggregate((current, next) => current + next);
            var firstAggregate = aggregateEmails(response1) + aggregateEmails(response2);
            var secondAggregate = aggregateEmails(response3) + aggregateEmails(response4);
            Assert.NotEqual(firstAggregate, secondAggregate);
        }

        [Fact]
        public async Task DontSortWithInvalidSortParameter()
        {
            var url = "AsyncLightQuery?sort=firstName";
            var response1 = await GetResponse<List<User>>(url);
            var response2 = await GetResponse<List<User>>(url);
            var response3 = await GetResponse<List<User>>(url);
            var response4 = await GetResponse<List<User>>(url);
            Func<IEnumerable<User>, string> aggregateEmails = users =>
                users
                    .Select(u => u.Email)
                    .Aggregate((current, next) => current + next);
            var firstAggregate = aggregateEmails(response1) + aggregateEmails(response2);
            var secondAggregate = aggregateEmails(response3) + aggregateEmails(response4);
            Assert.NotEqual(firstAggregate, secondAggregate);
        }

        [Fact]
        public async Task ReturnsPaginationResultOnRequestingPageWithSortParam()
        {
            var url = "AsyncLightQuery?sort=userName&page=1";
            var response = await GetResponse<PaginationResult<User>>(url);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task ReturnsPaginationResultOnRequestingPageSizeWithSortParam()
        {
            var url = "AsyncLightQuery?sort=userName&pageSize=2";
            var response = await GetResponse<PaginationResult<User>>(url);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task ReturnsPaginationResultOnRequestingPageAndPageSizeWithSortParam()
        {
            var url = "AsyncLightQuery?sort=userName&page=2&pageSize=3";
            var response = await GetResponse<PaginationResult<User>>(url);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task ReturnsPaginationResultOnRequestingPageWithoutSortParam()
        {
            var url = "AsyncLightQuery?page=1";
            var response = await GetResponse<PaginationResult<User>>(url);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task ReturnsPaginationResultOnRequestingPageSizeWithoutSortParam()
        {
            var url = "AsyncLightQuery?pageSize=2";
            var response = await GetResponse<PaginationResult<User>>(url);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task ReturnsPaginationResultOnRequestingPageAndPageSizeWithoutSortParam()
        {
            var url = "AsyncLightQuery?page=2&pageSize=3";
            var response = await GetResponse<PaginationResult<User>>(url);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task AppliesDefaultSortWithoutClientSortParameter()
        {
            var url = "AsyncLightQueryWithDefaultSort";
            var actualResponse = await GetResponse<List<User>>(url);
            for (var i = 1; i < actualResponse.Count; i++)
            {
                var previousValueIsSmaller = actualResponse[i].Email.CompareTo(actualResponse[i - 1].Email) > 0;
                Assert.True(previousValueIsSmaller);
            }
        }

        [Fact]
        public async Task CanOverrideDefaultSort()
        {
            var url = "AsyncLightQueryWithDefaultSort?sort=userName";
            var actualResponse = await GetResponse<List<User>>(url);
            for (var i = 1; i < actualResponse.Count; i++)
            {
                var previousValueIsSmaller = actualResponse[i].UserName.CompareTo(actualResponse[i - 1].UserName) > 0;
                Assert.True(previousValueIsSmaller);
            }
        }

        [Fact]
        public async Task CanFilterByNestedProperty()
        {
            var url = "AsyncLightQueryWithDefaultSort?sort=favoriteAnimal.name";
            var actualResponse = await GetResponse<List<User>>(url);
            for (var i = 1; i < actualResponse.Count; i++)
            {
                var previousValueIsSmaller = actualResponse[i].UserName.CompareTo(actualResponse[i - 1].UserName) > 0;
                Assert.False(previousValueIsSmaller);
            }
        }
    }
}
