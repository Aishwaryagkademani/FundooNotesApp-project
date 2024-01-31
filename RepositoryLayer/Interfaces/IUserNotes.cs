using ModelLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IUserNotes
    {
        public NotesEntity AddNewNotes(AddNotesModel addNotes,int userId);
        public NotesEntity GetNotesById(int id, int userId);
        public IEnumerable<NotesEntity> GetAllNotes(int userId);
        //public NotesEntity GetAllNotes();

        public int DeleteNotes(int id, int userId);
        public NotesEntity UpdateNotes(UpdateModel model, int userId);

    }
}
