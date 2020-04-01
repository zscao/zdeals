using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.IO;
using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Storage;

namespace ZDeals.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route(ApiRoutes.Pictures.Base)]
    public class PicturesController : ControllerBase
    {
        private readonly IBlobService _blobService;

        public PicturesController(IBlobService blobService)
        {
            blobService.Container = "test";
            _blobService = blobService;
        }

        [AllowAnonymous]
        [HttpGet("{pictureId}")]
        public async Task<IActionResult> GetImage(string pictureId)
        {
            try
            {
                var descriptor = await _blobService.GetBlobDescriptorAsync(pictureId);
                if (string.IsNullOrEmpty(descriptor?.Url))
                {
                    return NotFound();
                }
                var blob = await _blobService.GetBlobAsync(pictureId);
                return File(blob, descriptor.ContentType);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
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