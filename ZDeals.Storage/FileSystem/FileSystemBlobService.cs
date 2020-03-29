using Microsoft.AspNetCore.StaticFiles;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ZDeals.Storage.FileSystem
{
    public class FileSystemBlobService : IBlobService
    {
        private readonly FileSystemStorageConfig _config;
        public string Container
        {
            get; set;
        }

        public FileSystemBlobService(FileSystemStorageConfig config)
        {
            _config = config;
        }

        public Task<bool> CreateContainerAsync(string container, CancellationToken cancellationToken = default)
        {
            var path = Path.Combine(_config.Directory, container);
            if (Directory.Exists(path)) return Task.FromResult(true);

            var dir = Directory.CreateDirectory(path);
            return Task.FromResult(dir.Exists);
        }

        public async Task<Stream> GetBlobAsync(string id, CancellationToken cancellationToken = default)
        {
            var path = GetFilePath(id);
            return await ReadFileStream(path, cancellationToken);
        }

        public async Task<Stream> GetBlobAsync(string container, string id, CancellationToken cancellationToken = default)
        {
            var path = GetFilePath(container, id);
            return await ReadFileStream(path, cancellationToken);
        }

        public Task<BlobDescriptor> GetBlobDescriptorAsync(string id, CancellationToken cancellationToken = default)
        {
            var path = GetFilePath(id);
            return Task.FromResult(GetFileDescriptor(path));
        }

        public Task<BlobDescriptor> GetBlobDescriptorAsync(string container, string id, CancellationToken cancellationToken = default)
        {
            var path = GetFilePath(container, id);
            return Task.FromResult(GetFileDescriptor(path));
        }

        public async Task<bool> UploadBlobAsync(string id, Stream stream, BlobProperties properties = null, CancellationToken cancellationToken = default)
        {
            var path = GetFilePath(id);
            return await WriteFileStream(stream, path, cancellationToken);
        }

        public async Task<bool> UploadBlobAsync(string container, string id, Stream stream, BlobProperties properties = null, CancellationToken cancellationToken = default)
        {
            var path = GetFilePath(container, id);
            return await WriteFileStream(stream, path, cancellationToken);
        }

        private string GetFilePath(string id)
        {
            if (string.IsNullOrEmpty(this.Container))
                return Path.Combine(_config.Directory, id);
            else
                return GetFilePath(this.Container, id);
        }

        private string GetFilePath(string container, string id)
        {
            return Path.Combine(_config.Directory, container, id);
        }


        private BlobDescriptor GetFileDescriptor(string path)
        {
            var info = new FileInfo(path);

            if (!info.Exists) throw new IOException($"Could not find the file {path}");

            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if(!provider.TryGetContentType(info.Name, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return new BlobDescriptor
            {
                Name = info.Name,
                Url = info.FullName,
                LastModified = info.LastWriteTime,
                Length = info.Length,
                Security = BlobSecurity.Private,
                ContentType = contentType
            };
        }

        private async Task<Stream> ReadFileStream(string path, CancellationToken cancellationToken)
        {
            if (File.Exists(path) == false) throw new IOException($"Could not find file {path}");

            var file = new FileStream(path, FileMode.Open, FileAccess.Read);
            int size = (int)file.Length;

            var stream = new MemoryStream(size);
            await file.CopyToAsync(stream, size, cancellationToken);

            stream.Position = 0;
            return stream;
        }


        private async Task<bool> WriteFileStream(Stream stream, string path, CancellationToken cancellationToken)
        {
            var size = (int)stream.Length;
            stream.Position = 0;

            var file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            await stream.CopyToAsync(file, size, cancellationToken);
            file.Close();

            return true;
        }

    }
}
