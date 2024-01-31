using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NotesBusiness : INotesBusiness
    {
        public readonly IUserNotes userNotes;

        public NotesBusiness(IUserNotes userNotes)
        {
            this.userNotes = userNotes;
        }
        public NotesEntity AddNewNotes(AddNotesModel addNotes, int userId)
        {
            return userNotes.AddNewNotes(addNotes, userId);
        }

        public NotesEntity GetNotesById(int id, int userId)
        {
            return userNotes.GetNotesById(id,userId);
        }

        // public NotesEntity GetAllNotes()
        public IEnumerable<NotesEntity> GetAllNotes(int userId)
        {
            return userNotes.GetAllNotes(userId);
        }

        public int DeleteNotes(int id, int userId)
        {
            return userNotes.DeleteNotes(id,userId);
        }

        public NotesEntity UpdateNotes(UpdateModel model, int userId)
        {
            return userNotes.UpdateNotes(model,userId);
        }
    }
}
