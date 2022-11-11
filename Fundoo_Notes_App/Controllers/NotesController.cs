using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using RepositoryLayer.Interface;
using BusinessLayer.Service;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Service;

namespace Fundoo_App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
   
    public class NotesController : ControllerBase
    {
        private readonly INotesRl notesBl;
        private readonly FundooContext fundooContext;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<NotesController> logger;
        public NotesController(INotesRl notesBl ,INotesRl notesRl , IMemoryCache memoryCache, IDistributedCache distributedCache, FundooContext fundooContext, ILogger<NotesController> logger)
        {
            this.notesBl = notesBl;
            this.fundooContext = fundooContext;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.logger = logger;
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult CreateNotes(NotesModel notesModel)
        {
            try
            {

                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = notesBl.CreateNote(userId, notesModel);
                if (result != null)
                {
                    logger.LogInformation("Note Created Successfully ");
                    return Ok(new { sucess = true, message = "Note Created Successful", data = result });
                }
                else
                {
                    logger.LogError("Note Not Created");
                    return BadRequest(new { success = false, message = "Note Created Unsuccessful" });
                }
            }
            catch (Exception)
            {
                logger.LogError(ToString());
                throw;
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteNotes(long NoteId)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var delete = notesBl.DeleteNotes(NoteId);
                if (delete != null)
                {
                    logger.LogInformation("Notes Deleted Successfully");
                    return this.Ok(new { Success = true, message = "Notes Deleted Successfully" });
                }
                else
                {
                    logger.LogError("Notes Not Deleted");
                    return this.BadRequest(new { Success = false, message = "Notes Deleted Unsuccessful" });
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
        public IActionResult UpdateNotes(NotesModel notesModel, long NoteId)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var result = notesBl.UpdateNote(notesModel, NoteId);
                if (result != null)
                {
                    logger.LogInformation("Notes Updated Successfully");
                    return this.Ok(new { Success = true, message = "Notes Updated Successfully", data = result });
                }
                else
                {
                    logger.LogError("Notes Not Updated");
                    return this.BadRequest(new { Success = false, message = "No Notes Found" });
                }
            }
            catch (Exception )
            {
                logger.LogError(ToString());
                throw;
            }
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllNotes(long userId)
        {
            try
            {
                var notes = notesBl.GetAllNotes(userId);
                if (notes != null)
                {
                    logger.LogInformation("All notes Showing Successfully");
                    return Ok(new { Success = true, message = "All notes found Successfully", data = notes });

                }
                else
                {
                    logger.LogError("Notes Not Found");
                    return BadRequest(new { Success = false, message = "No Notes Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpGet]
        [Route("Read")]
        public IActionResult GetNote(long NotesId)
        {
            try
            {
                long note = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "UserId").Value);
                List<NotesEntity> result = notesBl.GetNote(NotesId);
                if (result != null)
                {
                    logger.LogInformation(" Note Display Successful");
                    return this.Ok(new { Success = true, message = " Note Display Successfully", data = result });
                }
                else
                {
                    logger.LogError("Note Not Found");
                    return this.BadRequest(new { Success = false, message = "Note not Available" });
                }
            }
            catch (Exception )
            {
                logger.LogError(ToString());
                throw;
            }
        }
        [HttpPut]
        [Route("Archive")]
        public IActionResult ArchiveNote(long NoteId)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "UserId").Value);
                var result = notesBl.ArchiveNote(NoteId, userid);
                if (result != null)
                {
                    logger.LogInformation("Archived Successfully");
                    return this.Ok(new { Success = true, message = "Archived Successfully", data = result });
                }
                else
                {
                    logger.LogError("Archive Unsuccessful");
                    return this.BadRequest(new { Success = false, Message = "Archived Unsuccessful" });
                }
            }
            catch (Exception )
            {
                logger.LogError(ToString());
                throw;
            }
        }

        [HttpPut]
        [Route("Pin")]
        public IActionResult PinNote(long NoteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(p => p.Type == "UserId").Value);
                var result = notesBl.PinNote(NoteId, userId);
                if (result != null)
                {
                    logger.LogInformation("Note Pinned Successfully");
                    return this.Ok(new { Success = true, message = "Note Pinned Successfully", data = result });
                }
                else
                {
                    logger.LogError("Note Pinned Unsuccessful");
                    return this.BadRequest(new { Success = false, message = "Note Pinned Unsuccessful" });
                }
            }
            catch (Exception )
            {
                logger.LogError(ToString());
                throw;
            }
        }

        [HttpPut]
        [Route("Trash")]
        public IActionResult TrashNote(long NotesId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(t => t.Type == "UserId").Value);
                var result = notesBl.TrashNote(NotesId, userId);
                if (result != null)
                {
                    logger.LogInformation("Trashed Successfully");
                    return this.Ok(new { Success = true, message = "Trashed Successfully", data = result });
                }
                else
                {
                    logger.LogError("Trashed Unsuccessful");
                    return this.BadRequest(new { Success = false, message = "Trash Unuccessful" });
                }
            }
            catch (Exception)
            {
                logger.LogError(ToString());
                throw;
            }
        }

        [HttpPut]
        [Route("Color")]
        public IActionResult NoteColor(long NoteId, string addcolor)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var result = notesBl.NoteColor(NoteId, addcolor);
                if (result != null)
                {
                    logger.LogInformation("Color Added Successfully");
                    return this.Ok(new { Success = true, message = "Color Added Successfully", data = result });
                }
                else
                {
                    logger.LogError("Unable to add Color");
                    return this.BadRequest(new { Success = false, message = " Unsuccessful to add Color" });
                }
            }
            catch (Exception)
            {
                logger.LogError(ToString());
                throw;
            }
        }
        [HttpPut]
        [Route("AddImage")]
        public IActionResult AddImage(long noteId, string filePath)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
                var result = notesBl.AddImage(filePath, userId, noteId);

                if (result != null) 
                { 
                    logger.LogInformation("Image Uploded Successfully");
                    return Ok(new { success = true, message = "Done", data = result });
                }

                else
                {
                    logger.LogError("Image Uploading Unsuccessful");
                    return BadRequest(new { success = false, message = "something went wrong" });
                }
                   
            }
            catch (Exception)
            {
                logger.LogError(ToString());
                throw;
            }
        }

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCustomersUsingRedisCache()
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
            var cacheKey = "NotesList";
            string serializedNotesList;
            var NotesList = new List<NotesEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<NotesEntity>>(serializedNotesList);
            }
            else
            {
                NotesList = fundooContext.NotesTable.ToList();
                serializedNotesList = JsonConvert.SerializeObject(NotesList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return Ok(NotesList);
        }


    }
}
