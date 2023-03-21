using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyProgram.Users;

[ApiController]
[Route("/users")]
public class UsersController : ControllerBase
{
    private readonly static Dictionary<int, LoyaltyProgramUser> RegisteredUsers = new();

    [HttpGet("{userId:int}")]
    public ActionResult<LoyaltyProgramUser> GetUser(int userId)
    {
        return RegisteredUsers.ContainsKey(userId)
            ? Ok(RegisteredUsers[userId])
            : NotFound();
    }

    [HttpPost("")]
    public ActionResult<LoyaltyProgramUser> CreatUser([FromBody] LoyaltyProgramUser user)
    {
        if(user == null)
        {
            return BadRequest();
        }

        var newUser = RegisterUser(user);
        return Created(new Uri($"/users/{newUser.Id}", UriKind.Relative), newUser);
    }

    [HttpPut("{userId:int}")]
    public LoyaltyProgramUser UpdateUser(int userId, [FromBody] LoyaltyProgramUser user)
    {
       return RegisteredUsers[userId] = user;
    }

    private LoyaltyProgramUser RegisterUser(LoyaltyProgramUser user)
    {
        var userId = RegisteredUsers.Count;

        RegisteredUsers[userId] = user with {Id = userId};

        return RegisteredUsers[userId];
    }
}
