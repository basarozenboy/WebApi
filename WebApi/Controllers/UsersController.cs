﻿namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Users;
using WebApi.Services;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    private IMapper _mapper;

    public UsersController(
        IUserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetById(id);
        return Ok(user);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create(CreateUser model)
    {
        var serviceResult = _userService.Create(model);
        if (!serviceResult.Success)
            return BadRequest(new { message = serviceResult.Message }); 
        return Ok(new { message = "User created" });
    }

    [HttpPost ("GenerateAutoData")]
    [Authorize]
    public IActionResult Create(int itemCount)
    {
        _userService.GenerateAutoData(itemCount);
        return Ok(new { message = "Users created" });
    }

    [HttpPut("{id}")]
    [Authorize]
    public IActionResult Update(int id, UpdateUser model)
    {
        _userService.Update(id, model);
        return Ok(new { message = "User updated" });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult Delete(int id)
    {
        _userService.Delete(id);
        return Ok(new { message = "User deleted" });
    }
}