using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.Threading.Tasks;
using ZDeals.Common.Constants;
using ZDeals.Storage;

namespace ZDeals.Web.Pages
{
    public class PictureModel : PageModel
    {
        private readonly IBlobService _blobService;

        public PictureModel(IBlobService blobService)
        {
            blobService.Container = DefaultValues.DealPicturesContainer;
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