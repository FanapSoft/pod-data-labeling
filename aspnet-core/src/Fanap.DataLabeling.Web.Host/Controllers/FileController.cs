using Abp.Domain.Repositories;
using Fanap.DataLabeling.Controllers;
using Fanap.DataLabeling.Datasets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Web.Host.Controllers
{
    public class FilesController : DataLabelingControllerBase
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IRepository<DatasetItem, Guid> itemRepo;

        public FilesController(IHostingEnvironment hostingEnvironment, IRepository<DatasetItem, Guid> itemRepo)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.itemRepo = itemRepo;
        }
        [HttpGet]
        [Route("file/dataset/item/{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                var found = itemRepo.GetAll().SingleOrDefault(ff => ff.Id == Guid.Parse(id));
                if (found == null)
                    return NotFound();
                var filePath = found.FilePath;
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                var extension = Path.GetExtension(id).ToLower();
                return File(fileBytes, MimeTypeMap.GetMimeType(extension), $"{id}{extension}");
            }
            catch (Exception ex)
            {
                Logger.Error("Error in getting file", ex);
                return StatusCode(500);
            }
        }
    }

}
