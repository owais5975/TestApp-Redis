using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TestApp.DTOs;
using TestApp.Helpers;
using TestApp.Services;

namespace TestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : ControllerBase
    {

        private readonly ILogger<AppController> _logger;
        private readonly IDistributedCache _cache;
        private readonly IUserRepo _userRepo;

        public AppController(ILogger<AppController> logger, IDistributedCache cache, IUserRepo userRepo)
        {
            _logger = logger;
            _cache = cache;
            _userRepo = userRepo;
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(AddUserDTO dto)
        {
            try
            {
                await _userRepo.AddUser(dto);
                return Ok(new JSONResponse { Status = true, Message = "User added successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex.InnerException.ToString() });
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO dto)
        {
            try
            {
                await _userRepo.UpdateUser(dto);
                return Ok(new JSONResponse { Status = true, Message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex.InnerException.ToString() });
            }
        }

        [HttpGet("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            try
            {
                await _userRepo.DeleteUser(Id);
                return Ok(new JSONResponse { Status = true, Message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex.InnerException.ToString() });
            }
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(int Id)
        {
            try
            {
                var user = _userRepo.GetUser(Id);
                return Ok(new JSONResponse { Status = true, Data = user });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex.InnerException.ToString() });
            }
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                List<GetUserDTO>? users = null;
                var cachedData = await _cache.GetStringAsync("users");
                if (cachedData == null)
                {
                    var distributedCacheEntryOptions = CacheHelper.CacheOptions();
                    users = _userRepo.GetUsers();
                    var serializedData = JsonConvert.SerializeObject(users);
                    await _cache.SetStringAsync("users", serializedData, distributedCacheEntryOptions);
                }
                else
                {
                    users = JsonConvert.DeserializeObject<List<GetUserDTO>>(cachedData);
                }

                return Ok(new JSONResponse { Status = true, Data = users });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex.InnerException.ToString() });
            }
        }
    }

    public class JSONResponse
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDescription { get; set; }
        public object Data { get; set; }
    }
}