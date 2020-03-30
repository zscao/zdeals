using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ZDeals.Storage
{
    public interface IBlobService
    {
        /// <summary>
        /// set/get the container name on instance
        /// </summary>
        string Container { get; set; }

        Task<bool> CreateContainerAsync(string container, CancellationToken cancellationToken = default);

        Task<BlobDescriptor> GetBlobDescriptorAsync(string blobName, CancellationToken cancellationToken = default);

        Task<BlobDescriptor> GetBlobDescriptorAsync(string container, string blobName, CancellationToken cancellationToken = default);

        Task<Stream> GetBlobAsync(string blobName, CancellationToken cancellationToken = default);

        Task<Stream> GetBlobAsync(string container, string blobName, CancellationToken cancellationToken = default);

        Task<bool> UploadBlobAsync(string blobName, Stream stream, BlobProperties properties = null, CancellationToken cancellationToken = default);

        Task<bool> UploadBlobAsync(string container, string blobName, Stream stream, BlobProperties properties = null, CancellationToken cancellationToken = default);
    }
}
