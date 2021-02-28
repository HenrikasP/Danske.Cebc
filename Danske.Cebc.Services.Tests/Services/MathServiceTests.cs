using System;
using AutoFixture;
using Danske.Cebc.Services.Services;
using NUnit.Framework;

namespace Danske.Cebc.Services.Tests.Services
{
    namespace Danske.Cebc.Services.Tests.Services.MathServiceTests
    {
        public class MathServiceBase
        {
            protected Fixture Fixture;
            protected MathService Service;

            
            [SetUp]
            public void Setup()
            {
                Fixture = new Fixture();
                Service = new MathService();
            }
        }

        public class PowTests : MathServiceBase
        {
            [Test]
            public void ItShouldThrowExceptionIfPowIsNegative()
            {
                var x = Fixture.Create<decimal>();
                var y = - Fixture.Create<int>();

                Assert.Throws<ArgumentException>(() => Service.Pow(x, y));
            }
            
            [Test]
            public void ItShouldReturnOneIfPowIsZero()
            {
                var x = Fixture.Create<decimal>();
                var y = 0;

                var response = Service.Pow(x, y);

                Assert.That(response, Is.EqualTo(1m));
            }
            
            [Test]
            public void ItShouldReturnCorrectly()
            {
                var random = new Random();
                var x = random.NextDouble();
                var y = random.Next(1, 5);

                var response = Service.Pow((decimal)x, y);

                Assert.That(response, Is.EqualTo(Math.Pow(x, y)).Within(.00000000000001));
            }
        }
    }
}