using Chapter_TURMA14.Controllers;
using Chapter_TURMA14.Interfaces;
using Chapter_TURMA14.Models;
using Chapter_TURMA14.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace Teste_xUnit
{
    public class LoginControllerTest
    {
        [Fact]
        public void LoginController_Retorna_Usuario_Invalido()
        {
            //Arrange

            var RepositoryEspelhado = new Mock<IUsuarioRepository>();

            RepositoryEspelhado.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((Usuario)null);
            var Controller = new LoginController(RepositoryEspelhado.Object);

            LoginViewModelcs dadosUsuario = new LoginViewModelcs();

            dadosUsuario.email = "wilsonrogerio@email.com";

            dadosUsuario.senha = "12345";

            //Act

           var resultado = Controller.Login(dadosUsuario);

            //Assert

            Assert.IsType<UnauthorizedObjectResult>(resultado);

        }
        [Fact]
        public void LoginController_RetornarToken()
        {
            //Arrange

            Usuario usuarioRetornado = new Usuario();
            usuarioRetornado.Email = "wilson@email.com";
            usuarioRetornado.Senha = "123456";
            usuarioRetornado.Tipo = "0";
            usuarioRetornado.Id = 1;


            var RepositoryEspelhado = new Mock<IUsuarioRepository>();
            RepositoryEspelhado.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(usuarioRetornado);


            LoginViewModelcs dadosUsuario = new LoginViewModelcs();

            dadosUsuario.email = "wilsonrogerio@email.com";

            dadosUsuario.senha = "12345";

            var Controller = new LoginController(RepositoryEspelhado.Object);

            string issuerValido = "chapter.webapi";

            //Act

           OkObjectResult resultado = (OkObjectResult)Controller.Login(dadosUsuario);

           string TokenString = resultado.Value.ToString().Split(' ')[3];

           var jwtHandler = new JwtSecurityTokenHandler();

           var tokeJwt = jwtHandler.ReadToken(TokenString);

            // Assert

            Assert.Equal(issuerValido, tokeJwt.Issuer);

        }


    }
}