using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Update.Internal;
using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RepositoryLayer.Services
{
    public class UserNotes : IUserNotes
    {
        public readonly FundooContext fundooContext;
        public UserNotes(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public NotesEntity AddNewNotes(AddNotesModel addNotes,int userId)
        {
            IEnumerable<ImageEntity> imageList = null;

            if (userId != 0)
            {
                var user=fundooContext.Users.FirstOrDefault(x => x.UserId == userId);
                if(user != null)
                {
                    NotesEntity entity = new NotesEntity();
                    entity.Title = addNotes.Title;
                    entity.Description = addNotes.Description;
                    entity.Color = addNotes.Color;
                    //entity.ImagePaths= addNotes.ImagePaths;
                    entity.Reminder = addNotes.Reminder;
                    entity.IsArchive = addNotes.IsArchive;
                    entity.IsPinned = addNotes.IsPinned;
                    entity.IsTrash = addNotes.IsTrash;
                    entity.CreatedAt = DateTime.Now;
                    entity.ModifiedAt = DateTime.Now;
                    entity.UserId = userId;

                    fundooContext.Notes.Add(entity);
                    fundooContext.SaveChanges();

                    if(addNotes.ImagePaths != null)
                    {
                        imageList = AddImages(entity.NoteId, userId, addNotes.ImagePaths);
                    }
                    return entity;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
           
        }


        public IEnumerable<ImageEntity> AddImages(int noteId, int userId, ICollection<IFormFile> files)
        {
            try
            {
                NotesEntity resNote = null;
                var user = fundooContext.Notes.FirstOrDefault(n => n.UserId == userId);
                if (user != null)
                {
                    resNote = fundooContext.Notes.Where(n => n.UserId == userId && n.NoteId == noteId).FirstOrDefault();
                    if (resNote != null)
                    {
                        IList<ImageEntity> images = new List<ImageEntity>();
                        foreach (var file in files)
                        {
                            ImageEntity img = new ImageEntity();
                            var uploadImageRes = UploadImage(file);
                            img.NoteId = noteId;
                            img.ImageUrl = uploadImageRes.ToString();
                            img.ImageName = file.FileName;
                            images.Add(img);
                            fundooContext.Images.Add(img);
                            fundooContext.SaveChanges();
                            resNote.ModifiedAt = DateTime.Now;
                            fundooContext.Notes.Update(resNote);
                            fundooContext.SaveChanges();
                        }
                        return images;
                    }
                    else
                    {
                        return null;
                    }
                }

                // Return a default value if the 'if (user != null)' condition is not satisfied
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string UploadImage(IFormFile formFile)
        {
            try
            {
                string originalFileName = formFile.FileName;
                string uniqueFileName = $"{Guid.NewGuid()}_{DateTime.Now.Ticks}{Path.GetExtension(originalFileName)}";
                //Filehelper is in model layer
                string filePath = Path.Combine(FileHelper.GetFilePath(""), uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyToAsync(fileStream);
                }

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                // Log the exception details, and either rethrow it or return a meaningful result
                // Logging example:
                // _logger.LogError($"Error uploading image: {ex}");
                throw;
            }
        }


        public NotesEntity GetNotesById(int id,int userId)
        {
            if (userId != 0)
            {
                var getRecord = fundooContext.Notes.FirstOrDefault(x => x.NoteId == id && x.UserId == userId);
                if (getRecord != null)
                {
                    return getRecord;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
            
        }

        public IEnumerable<NotesEntity> GetAllNotes(int userId)
        {
            //var getRecord =(from x in fundooContext.Notes select x);
            // var getRecord = fundooContext.Notes;
            if (userId != 0)
            {
                var userRecords = fundooContext.Notes.Where(x => x.UserId == userId).ToList();
                if (userRecords != null)
                    return userRecords;
                else
                    return null;
            }
            return null;
          
        }

        //public NotesEntity UpdateNotes(int id)
        //{

        //}

        public int DeleteNotes(int id,int userId)
        {
            var record=fundooContext.Notes.FirstOrDefault(x=>x.NoteId==id && x.UserId==userId);
            if(record == null)
            {
                return 0;
            }

            fundooContext.Notes.Remove(record);
            var imgRecord = fundooContext.Images.Where(x => x.ImageId == id);
            foreach( var i in imgRecord)
            fundooContext.Images.Remove(i);
            return fundooContext.SaveChanges();
        }

        public NotesEntity UpdateNotes(UpdateModel model,int userId)
        {
            if (userId != null)
            {
                IEnumerable<ImageEntity> imageList = null;

                var getRecord = fundooContext.Notes.FirstOrDefault(x => x.NoteId == model.NoteId);
                if (getRecord != null)
                {
                    getRecord.Title = model.Title;
                    getRecord.Description = model.Description;
                    getRecord.Reminder = model.Reminder;
                    getRecord.Color = model.Color;
                    getRecord.ModifiedAt = DateTime.Now;
                    getRecord.IsArchive = model.IsArchive;
                    getRecord.IsTrash = model.IsTrash;
                    getRecord.IsPinned = model.IsPinned;
                   
                    fundooContext.SaveChanges();
                    if(model.ImagePaths  != null)
                    {
                        imageList= UpdateImages(getRecord.NoteId,userId,model.ImagePaths); 
                    }
                    return getRecord;

                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<ImageEntity> UpdateImages(int noteId, int userId, ICollection<IFormFile> files)
        {
            try
            {
                NotesEntity resNote = null;
                ImageEntity resImg = null;
                var user = fundooContext.Notes.FirstOrDefault(n => n.UserId == userId);
                if (user != null)
                {
                    resNote = fundooContext.Notes.Where(n => n.UserId == userId && n.NoteId == noteId).FirstOrDefault();
                    resImg = fundooContext.Images.FirstOrDefault(n => n.NoteId == noteId);

                    if (resNote != null && resImg!=null)
                    {
                        IList<ImageEntity> images = new List<ImageEntity>();
                        foreach (var file in files)
                        {
                            ImageEntity img = new ImageEntity();
                            var uploadImageRes = UploadImage(file);
                            resImg.NoteId = noteId;
                            resImg.ImageUrl = uploadImageRes.ToString();
                            resImg.ImageName = file.FileName;
                            images.Add(img);
                            fundooContext.Images.Update(resImg);
                            fundooContext.SaveChanges();
                            resNote.ModifiedAt = DateTime.Now;
                            fundooContext.Notes.Update(resNote);
                            fundooContext.SaveChanges();
                        }
                        return images;
                    }
                    else
                    {
                        return null;
                    }
                }

                // Return a default value if the 'if (user != null)' condition is not satisfied
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
