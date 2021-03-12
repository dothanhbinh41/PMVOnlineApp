using PMVOnline.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Tasks.Services
{
    public interface IFileService
    {
        Task<Guid> UploadAsync(Stream file, string fileName);
        Task<byte[]> DownloadAsync(Guid id);
    }

    public class FileService : AuthApiProvider<AppApi>, IFileService
    {
        public Task<byte[]> DownloadAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> UploadAsync(Stream file, string fileName)
        {
            var result = await Api.Upload(new Refit.StreamPart(file, fileName));
            if (result.Content == null)
            {
                return Guid.Empty;
            }
            return result.Content.Id;
        }
    }
}
