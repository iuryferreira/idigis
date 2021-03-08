// using System;
// using System.IdentityModel.Tokens.Jwt;
// using Authentication.Contracts;
// using Authentication.Services;
// using DotNetEnv;
// using Microsoft.VisualStudio.TestTools.UnitTesting;

// namespace Tests._Core.Authentication
// {
//     [TestClass]
//     public class JwtServiceTest
//     {
//         [TestInitialize]
//         public void BeforeEach ()
//         {
//             Env.TraversePath().Load();
//         }



//         [TestMethod]
//         public void Must_Return_A_Valid_Jwt_If_All_Data_Is_Provided ()
//         {
//             IJwtService sut = new JwtService();
//             var data = new { Email = "valid_email@email.com", Id = Guid.NewGuid().ToString() };
//             var tokenHandler = new JwtSecurityTokenHandler();
//             var result = sut.GenerateToken(data.Email, data.Id);
//             Assert.IsNotNull(result);
//             Assert.AreEqual(36, result.Length);
//             Assert.IsTrue(tokenHandler.CanReadToken(result));
//         }
//     }
// }
