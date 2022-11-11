using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using System.Linq;
using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Fundoo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollabController : Controller
    {
        private readonly ICollabBl collabBl;
        private readonly FundooContext fundooContext;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<CollabController> logger;
        public CollabController(ICollabBl collabBl, IMemoryCache memoryCache, IDistributedCache distributedCache, FundooContext fundooContext, ILogger<CollabController> logger)
        {
            this.collabBl = collabBl;
            this.fundooContext = fundooContext;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.logger = logger;
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult AddCollab(CollabModel collabModel)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var collab = fundooContext.NotesTable.Where(X => X.NoteID == collabModel.NoteID).FirstOrDefault();
                if (collab.UserId == userId)
                {
                    var result = collabBl.AddCollab(collabModel);
                    if (result != null)
                    {   
                        logger.LogInformation("Collaboration successful");
                        return Ok(new { Success = true, message = "Collaboration successful", data = result });
                    }
                    else
                    {
                        logger.LogError("Collaboration Failed");
                        return BadRequest(new { Sucess = false, message = "Collaboration Failed" });
                    }
                }
                else
                {
                    logger.LogError("Collaboration Failed");
                    return Unauthorized(new { Sucess = false, message = "Collaboration Failed" });
                }
            }
            catch (Exception)
            {
                logger.LogError(ToString());
                throw;
            }
        }

        [HttpDelete]
        [Route("Remove")]
        public IActionResult RemoveCollab(long collabID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "UserId").Value);
                var delete = collabBl.RemoveCollab(collabID, userId);
                if (delete != null)
                {
                    logger.LogInformation("Collaboration Removed Successfully");
                    return Ok(new { Success = true, message = "Collaboration Removed" });
                }
                else
                {
                    logger.LogError("Unsuccessful to Remove Collaboration");
                    return BadRequest(new { Success = false, message = "Unsuccessful" });
                }
            }
            catch (Exception)
            {
                logger.LogError(ToString());
                throw;
            }
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult GetAllCollabs(long noteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var notes = collabBl.GetCollab(noteId, userId);
                if (notes != null)
                {
                    logger.LogInformation("Collaboration Found Successfully");
                    return Ok(new { Success = true, message = "Collaboration Successful", data = notes });

                }
                else
                {
                    logger.LogError("No Collaboration Found");
                    return BadRequest(new { Success = false, message = "No Collaboration Found" });
                }
            }
            catch (Exception)
            {
                logger.LogError(ToString());
                throw;
            }
        }

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCollabUsingRedisCache()
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
            var cacheKey = "CollabList";
            string serializedCollabList;
            var CollabList = new List<CollabEntity>();
            var redisCollabList = await distributedCache.GetAsync(cacheKey);
            if (redisCollabList != null)
            {
                serializedCollabList = Encoding.UTF8.GetString(redisCollabList);
                CollabList = JsonConvert.DeserializeObject<List<CollabEntity>>(serializedCollabList);
            }
            else
            {
                CollabList = fundooContext.CollabTable.ToList();
                serializedCollabList = JsonConvert.SerializeObject(CollabList);
                redisCollabList = Encoding.UTF8.GetBytes(serializedCollabList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCollabList, options);
            }
            return Ok(CollabList);
        }

    }
}
