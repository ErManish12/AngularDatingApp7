using System.Security.Claims;
using API.Controllers;
using API.DTOs;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

[Authorize]
public class UsersController : BaseApiController
{
    
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _repository.GetMembersAsync();

        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUsers(string username)
    {
        return await _repository.GetMemberAsync(username);
        
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _repository.GetUserByUsernameAsync(username);

        if(user == null) return NotFound();

        _mapper.Map(memberUpdateDto, user);

        if(await _repository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user !");

    }
}
