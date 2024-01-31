using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    public class AddNotesModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public ICollection<IFormFile> ImagePaths { get; set; }
      //  public string ImagePaths { get; set; }
        

        [DefaultValue("2024-01-26T12:12:55.389Z")]
        public DateTime Reminder { get; set; }
        public bool IsArchive { get; set; }
        public bool IsPinned { get; set; }
        public bool IsTrash { get; set; }
       
    }
}
