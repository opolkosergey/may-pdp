using System;
using Api;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ApiTests
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            Assert.True(true);
        }

        [Theory]
        [InlineData(0, 1, 2)]
        [InlineData(1, 1, 1)]
        public void Test2(int x, int y, int z)
        {
            // arrange

            // act
            var result = x + y + z;

            // assert
            Assert.Equal(3, result);
        }

        [Fact]
        public void Get_GivenUnauthorizedUser_ReturnsJsonResult()
        {
            // arrange
            var sut = new IdentityController();

            // act
            var response = sut.Get();

            // assert
            Assert.IsType<JsonResult>(response);
        }

        [Fact]
        public void Do_GivenServiceCallResultTrue_DoesNotThrowException()
        {
            // arrange
            var service = new Mock<IService>();
            service.Setup(x => x.ServiceCall()).Returns(true);
            var sut = new ClassWithDependencies(service.Object);

            // act
            sut.Do();

            // assert
            service.Verify(x => x.ServiceCall(), Times.Once);
        }

        [Fact]
        public void Do_GivenServiceCallResultFalse_ThrowsException()
        {
            // arrange
            var service = new Mock<IService>();
            service.Setup(x => x.ServiceCall()).Returns(false);
            var sut = new ClassWithDependencies(service.Object);

            // act

            // assert
            Assert.Throws<Exception>(() => sut.Do());
        }
    }
}