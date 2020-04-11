using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

using System;
using System.IO;
using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Common.Constants;
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
            blobService.Container = DefaultValues.DealPicturesContainer;
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
            using (var stream = ReadImage(file, out string contentType, out string fileExtension))
            {
                var properties = new BlobProperties
                {
                    ContentType = contentType,
                    Security = BlobSecurity.Public
                };

                var fileName = Path.GetRandomFileName() + fileExtension;
                var isUploaded = await _blobService.UploadBlobAsync(fileName, stream, properties);

                if (!isUploaded)
                {
                    return StatusCode(500, "Failed to save file.");
                }

                return Ok(new { file.ContentType, file.Length, FileName = fileName });
            }
        }

        private Stream ReadImage(IFormFile file, out string contentType, out string fileExtension)
        {
            var input = file.OpenReadStream();
            var image = Image.Load(input);

            if (image.Height > DefaultValues.MaxPictureSize || image.Width > DefaultValues.MaxPictureSize)
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new SixLabors.Primitives.Size(600),
                    Mode = ResizeMode.Max
                }));

                contentType = "image/jpeg";
                fileExtension = ".jpg";
                var outstream = new MemoryStream();
                image.SaveAsJpeg(outstream, new JpegEncoder());
                outstream.Position = 0;

                return outstream;
            }

            contentType = file.ContentType;
            fileExtension = Path.GetExtension(file.FileName);

            input.Position = 0;
            return input;
        }
    }
}