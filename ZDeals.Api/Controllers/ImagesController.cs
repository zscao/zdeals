using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZDeals.Api.Contract;
using ZDeals.Storage;

namespace ZDeals.Api.Controllers
{
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IBlobService _blobService;

        public ImagesController(IBlobService blobService)
        {
            blobService.Container = "test";
            _blobService = blobService;
        }

        [HttpGet(ApiRoutes.Images.GetImageById)]
        public async Task<IActionResult> GetImage(string imageId)
        {
            try
            {
                var descriptor = await _blobService.GetBlobDescriptorAsync(imageId);
                if (string.IsNullOrEmpty(descriptor?.Url))
                {
                    return NotFound();
                }
                var blob = await _blobService.GetBlobAsync(imageId);
                return File(blob, descriptor.ContentType);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost(ApiRoutes.Images.CreateImage)]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var properties = new BlobProperties
            {
                ContentType = file.ContentType,
                Security = BlobSecurity.Public
            };

            var stream = file.OpenReadStream();
            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName)?.ToLower();

            var isUploaded = await _blobService.UploadBlobAsync(fileName, stream, properties);
            if (!isUploaded)
            {
                return StatusCode(500, "Failed to save file.");
            }

            return Ok(new { file.ContentType, file.Length, FileName = fileName });
        }
    }
}