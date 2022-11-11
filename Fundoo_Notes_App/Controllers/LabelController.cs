using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using System.Linq;
using System;
using BusinessLayer.Service;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
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
    public class LabelController : ControllerBase
    {
        private readonly ILabelBl labelBl;
        private readonly FundooContext fundooContext;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<LabelController> logger;

        public LabelController(ILabelBl labelBl, FundooContext fundooContext, IMemoryCache memoryCache, IDistributedCache distributedCache, ILogger<LabelController> logger)
        {
            this.labelBl = labelBl;
            this.fundooContext = fundooContext;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddLabel(LabelModel labelModel)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var labelNote = fundooContext.NotesTable.Where(x => x.NoteID == labelModel.NoteID).FirstOrDefault();
                if (labelNote.UserId == userid)
                {
                    var result = labelBl.AddLabel(labelModel);
                    if (result != null)
                    {
                        logger.LogInformation("Label created successfully");
                        return Ok(new { Success = true, Message = "Label created successfully", data = result });
                    }
                    else
                    {
                        logger.LogError("Label not created");
                        return BadRequest(new { Success = false, Message = "Label is not created" });
                    }
                }
                else 
                {
                    logger.LogError("Unauthorized User");
                    return this.Unauthorized(new { Success = false, Message = "Unauthorized Access" });
                }
                
            }
            catch (Exception )
            {
                logger.LogError(ToString());
                throw;
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllLabel(long userId)
        {
            try
            {
                var label = labelBl.GetAllLabel(userId);
                if (label != null)
                {
                    logger.LogInformation("Displaying All labels Successfully");
                    return Ok(new { Success = true, Message = " Displaying Label Successfully", data = label });
                }
                else
                {
                    logger.LogError("No label found");
                    return BadRequest(new { Success = false, Message = "No label found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Getlabel(long NoteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "UserId").Value);
                var label = labelBl.Getlabel(NoteId, userId);
                if (label != null)
                {
                    logger.LogInformation("Label found Successfully");
                    return Ok(new { Success = true, message = "Label found Successfully", data = label });
                }
                else
                { 
                    logger.LogError("No label found");
                    return BadRequest(new { Success = false, message = "Label not Found" });
                }
                    
            }
            catch (Exception )
            {
                logger.LogError(ToString());
                throw;
            }
        }
        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateLabel(LabelModel labelModel, long labelID)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "UserId").Value);
                var result = labelBl.UpdateLabel(labelModel, labelID);
                if (result != null)
                {
                    logger.LogInformation("Label Updated Successfully");
                    return Ok(new { Success = true, message = "Label Updated Successfully", data = result });
                }
                else
                {
                    logger.LogError("Label Not Updated");
                    return BadRequest(new { Success = false, message = "Label Not Updated" });
                }
            }
            catch (Exception )
            {
                logger.LogError(ToString());
                throw;
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteLabel(long labelID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "UserId").Value);
                var delete = labelBl.DeleteLabel(labelID, userId);
                if (delete != null)
                {
                    logger.LogInformation("Label Deleted Successfully");
                    return Ok(new { Success = true, message = "Label Deleted Successfully" });
                }
                else
                {
                    logger.LogError("Label Not Deleted");
                    return BadRequest(new { Success = false, message = "Label not Deleted" });
                }
            }
            catch (Exception )
            {
                logger.LogError(ToString());
                throw;
            }
        }

        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "LabelList";
            string serializedLabelList;
            var labelList = new List<LabelEntity>();
            var redisLabelList = await this.distributedCache.GetAsync(cacheKey);
            if (redisLabelList != null)
            {
                serializedLabelList = Encoding.UTF8.GetString(redisLabelList);
                labelList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelList);
            }
            else
            {
                labelList = await this.fundooContext.LabelTable.ToListAsync();
                serializedLabelList = JsonConvert.SerializeObject(labelList);
                redisLabelList = Encoding.UTF8.GetBytes(serializedLabelList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await this.distributedCache.SetAsync(cacheKey, redisLabelList, options);
            }

            return this.Ok(labelList);
        }

    }
}
