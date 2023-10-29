using Api.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class Md5Tests
    {
        [Fact]
        public void Hash_WhenInputStringsAreASCII_ShouldReturnMd5RepresentationOfStrings()
        {
            //Arrange
            string value = "";
            string pepper = "";
            Md5 md5 = new Md5();

            //Act
            string result = md5.Hash(value, pepper);

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal("d41d8cd98f00b204e9800998ecf8427e", result);
        }

        [Fact]
        public void Hash_WhenAnyInputIsNull_ShouldThrowArgumentNullException()
        {
            //Arrange
            string value = "";
            string pepper = null;
            Md5 md5 = new Md5();

            //Act + Assert
            Assert.Throws<ArgumentNullException>(() => md5.Hash(value, pepper));
        }

        [Fact]
        public void Hash_WhenAnyInputContainsNonASCIICharacters_ShouldReturnMd5RepresentationOfStringsWithNonASCIICharactersRemoved()
        {
            //Arrange
            string value = "界value";
            string pepper = "value";
            Md5 md5 = new Md5();

            //Act
            string result = md5.Hash(value, pepper);

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal("fcc7a4b78ecd8fa7e713f8cfa0f51017", result);
        }
    }
}
