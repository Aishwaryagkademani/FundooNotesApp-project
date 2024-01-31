using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        public readonly INotesBusiness notesBusiness;
        public static IWebHostEnvironment _webHostEnvironment;

        public NotesController(INotesBusiness notesBusiness, IWebHostEnvironment webHostEnvironment)
        {
            this.notesBusiness = notesBusiness;
            _webHostEnvironment = webHostEnvironment;

        }

        [HttpPost]
        [Route("NoteCreation")]
        [Authorize]
        public IActionResult AddNote([FromForm]AddNotesModel model)
        {
            try
            {

                int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
                var result = notesBusiness.AddNewNotes(model, userId);
                if (result != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Success = true, Message = "notes added successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Success = false, Message = "notes adding is not successfull" });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetNotesById")]
        [Authorize]
        public IActionResult getNotesById(int id)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result=notesBusiness.GetNotesById(id,userId);
            if (result != null)
            {
                return Ok(new ResponseModel<NotesEntity> { Success = true, Message = "notes retrived successfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<NotesEntity> { Success = false, Message = "notes retrival is not successfull" });
            }
        }


        [HttpGet]
        [Route("Get All Notes of perticular user")]
        [Authorize]
        public ActionResult<IEnumerable<NotesEntity>> getAllRecord()
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = notesBusiness.GetAllNotes(userId);
            if (result != null)
            {
                return Ok(result);
                //return Ok(new ResponseModel<NotesEntity> { Success = true, Message = "notes retrived successfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<NotesEntity> { Success = false, Message = "notes retrival is not successfull" });
            }
        }

        [HttpDelete]
        [Route("Delete Records by Id")]
        public IActionResult DeleteRecords(int id)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result=notesBusiness.DeleteNotes(id,userId);
            if(result!= 0)
            {
                return Ok(new ResponseModel<int> { Success = true, Message = "notes deleted successfully", Data = result });

            }
            else
                return Ok(new ResponseModel<int> { Success = false, Message = "notes not deleted successfully", Data = result });
        }


        [HttpPut("update notes")]
        [Authorize]
        public IActionResult UpdateRecords([FromForm]UpdateModel model)
        {
            
            try
            {
                int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);

                var result = notesBusiness.UpdateNotes(model, userId);
                if (result != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Success = true, Message = "notes updated successfully", Data = result });

                }
                else
                    return Ok(new ResponseModel<NotesEntity> { Success = false, Message = "notes not updated successfully" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

       
        [HttpPost]
        [Route("Image upload")]
        public async Task<string> ImageUpload([FromForm] FileUpload fileUpload)
        {
            try
            {
                if (fileUpload.files.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + fileUpload.files.FileName))
                    {
                        fileUpload.files.CopyTo(fileStream);
                        fileStream.Flush();
                        return "Upload Done";
                    }
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
