using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Fanap.DataLabeling.Controllers;
using Fanap.DataLabeling.Datasets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
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
        [HttpGet]
        [Route("file/dataset/item/{id}/original")]
        public IActionResult GetOriginalFile(string id)
        {
            try
            {
                var dataListItem = itemRepo.GetAllIncluding(ff => ff.Dataset, ff => ff.Label).SingleOrDefault(ff => ff.Id == Guid.Parse(id));
                if (dataListItem == null)
                    return NotFound();
                var itemsSourcePath = dataListItem.Dataset.ItemsSourcePath;
                if (itemsSourcePath.IsNullOrEmpty())
                    throw new UserFriendlyException("Items source path not provided in dataset");
                var thisItemFileName = Path.GetFileNameWithoutExtension(dataListItem.FileName).Split("__")[0];
                var thisItemFullPath = Path.Combine(itemsSourcePath, dataListItem.Label.Name, $"{thisItemFileName}{dataListItem.FileExtension}");

                if (!System.IO.File.Exists(thisItemFullPath))
                    return NotFound();
                byte[] fileBytes = System.IO.File.ReadAllBytes(thisItemFullPath);
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
