﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers;
using SimpleBank.AcctManage.API.DTModels.v1.Requests;
using SimpleBank.AcctManage.API.DTModels.v1.Responses;
using SimpleBank.AcctManage.API.Mapping.v1;
using SimpleBank.AcctManage.Core.Application.Contracts.Business.v1;

namespace SimpleBank.AcctManage.API.Controllers.v1
{
    /// <summary> User related API actions. </summary>
    [ApiController, ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenthicationProvider _authenthicationProvider;
        private readonly IUserBusiness _userBusiness;
        private readonly IEntityMapper _entityMapper;

        /// <summary>  Users controller. </summary>
        /// <param name="authenthicationProvider">Infrastructure related to auth.</param>
        /// <param name="userBusiness">User related application.</param>
        /// <param name="entityMapper">Profiler.</param>
        /// <exception cref="ArgumentNullException">If <see cref="IAuthenthicationProvider"/>, <see cref="IUserBusiness"/> or <see cref="IEntityMapper"/> are null.</exception>
        public UsersController(
            IAuthenthicationProvider authenthicationProvider,
            IUserBusiness userBusiness,
            IEntityMapper entityMapper)
        {
            _authenthicationProvider = authenthicationProvider ?? throw new ArgumentNullException(nameof(authenthicationProvider));
            _userBusiness = userBusiness ?? throw new ArgumentNullException(nameof(userBusiness));
            _entityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
        }


        /// <summary>Create a new user.</summary>
        /// <param name="createUserRequest">All new user details.</param>
        /// <returns>The newly created user.</returns>
        [AllowAnonymous]
        [HttpPost]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateUserResponse>> Create(CreateUserRequest createUserRequest)
        {
            User user = new()
            {
                Username = createUserRequest.Username,
                Email = createUserRequest.Email,
                Fullname = createUserRequest.Fullname
            };

            var newUser = await _userBusiness.CreateUserAsync(user, createUserRequest.Password);
            if (newUser == null) { return BadRequest("Error on creating. Username or email already in use."); }

            var userResponse = _entityMapper.MapUserToResponse(newUser);

            return Created(nameof(GetUserInfo), userResponse);
        }


        /// <summary>Gets the user info (as CreateUserResponse).</summary>
        /// <returns>User info.</returns>
        [HttpGet("Profile")]
        [Produces("application/json")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateUserResponse>> GetUserInfo()
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            var user = await _userBusiness.GetUserAsync(userId);
            if (user == null) { return BadRequest("Error on getting info."); }

            var userResponse = _entityMapper.MapUserToResponse(user);

            return Ok(userResponse);
        }


    }
}


/*
         [HttpPost]
        [AllowAnonymous]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> UploadFile([FromForm] IFormFile file)
        {
            byte[] fileValue;
            using (Stream fileStream = file.OpenReadStream())
            {
                using (BinaryReader br = new BinaryReader(fileStream))
                {
                    fileValue = br.ReadBytes((Int32)fileStream.Length);
                }

                return Ok(new {
                    FilePath = Path.GetTempFileName(),
                    FilePath2 = Path.GetFileName(file.FileName),
                    FileType = file.ContentType, 
                    ContentDisposition = file.ContentDisposition,
                    sizeBy = (double) file.Length,
                    sizeKb = (double) file.Length / 1024,
                    sizeMb = (double) (file.Length / 1024) / 1024,
                    sizeGb = (double) ((file.Length / 1024) / 1024) / 1024,
                    FileValue = fileValue });
            }
        }
 */