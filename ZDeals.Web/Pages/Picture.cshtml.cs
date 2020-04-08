using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.Threading.Tasks;

using ZDeals.Storage;

namespace ZDeals.Web.Pages
{
    public class PictureModel : PageModel
    {
        private readonly IBlobService _blobService;

        public PictureModel(IBlobService blobService)
        {
            blobService.Container = "test";
            _blobService = blobService;
        }

        public async Task<IActionResult> OnGet(string pic)
        {
            try
            {
                var descriptor = await _blobService.GetBlobDescriptorAsync(pic);
                if (string.IsNullOrEmpty(descriptor?.Url))
                {
                    return NotFound();
                }
                var blob = await _blobService.GetBlobAsync(pic);
                return File(blob, descriptor.ContentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}