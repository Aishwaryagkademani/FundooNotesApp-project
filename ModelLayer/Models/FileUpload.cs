using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace ModelLayer.Models
{
    public class FileUpload
    {
        public IFormFile files { get; set; }
    }
}
