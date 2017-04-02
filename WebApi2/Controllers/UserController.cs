using System.Web.Http;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class UserController : ApiController
    {
        public User Get(int id)
        {
            return new User
            {
                id = id,
                firstName = "Aditya",
                lastName = "Magotra"
            };
        }
    }
}
