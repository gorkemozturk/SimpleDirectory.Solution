using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleDirectory.Domain.Models;
using SimpleDirectory.Extension.Interfaces;
using SimpleDirectory.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace SimpleDirectory.Test
{
    public class DirectoriesControllerUnitTest
    {
        private readonly Mock<IPersonService> _person = new Mock<IPersonService>();
        private readonly Mock<IContactService> _contact = new Mock<IContactService>();


        [Fact]
        public async Task GetPersonShouldReturnRightObjectWhenRouteIsCorrect()
        {
            // Arrange
            var id = Guid.NewGuid();
            var person = new PersonDetailDTO() { Id = id };

            _person.Setup(c => c.GetPerson(id)).ReturnsAsync(person);
            var controller = new DirectoriesController(_person.Object, _contact.Object);

            // Act
            var request = await controller.GetPerson(id);

            // Assert
            var result = Assert.IsType<ActionResult<PersonDetailDTO>>(request);
            var model = Assert.IsAssignableFrom<PersonDetailDTO>(result.Value);

            Assert.Equal(model.Id, id);
        }

        [Fact]
        public async Task DeletePersonShouldReturnNoContentWhenPersonExists()
        {
            // Arrange
            var person = new Person() { Id = Guid.NewGuid() };

            _person.Setup(c => c.DeleteResource(person)).Returns(person);
            var controller = new DirectoriesController(_person.Object, _contact.Object);

            // Act
            var request = await controller.DeletePerson(person.Id);

            // Assert
            Assert.Null(request.Value);
        }

        [Fact]
        public async Task DeleteContactShouldReturnNoContentWhenContactExists()
        {
            // Arrange
            var contact = new Contact() { Id = 1 };

            _contact.Setup(c => c.DeleteResource(contact)).Returns(contact);
            var controller = new DirectoriesController(_person.Object, _contact.Object);

            // Act
            var request = await controller.DeleteContact(contact.Id);

            // Assert
            Assert.Null(request.Value);
        }
    }
}
