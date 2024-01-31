using ModelLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface INotesBusiness
    {
        public NotesEntity AddNewNotes(AddNotesModel addNotes, int userId);
        public NotesEntity GetNotesById(int id, int userId);
        // public NotesEntity GetAllNotes();
        public IEnumerable<NotesEntity> GetAllNotes(int userId);

        public int DeleteNotes(int id, int userId);

        public NotesEntity UpdateNotes(UpdateModel model, int userId);
    }
}
