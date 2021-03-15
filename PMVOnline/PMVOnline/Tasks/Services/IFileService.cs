using PMVOnline.Api;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Tasks.Services
{
    public interface IFileService
    {
        Task<FileModel> UploadAsync(Stream file, string fileName);
        Task<byte[]> DownloadAsync(Guid id);
    }

    public class FileService : AuthApiProvider<AppApi>, IFileService
    {
        public Task<byte[]> DownloadAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<FileModel> UploadAsync(Stream file, string fileName)
        {
            var result = await Api.Upload(new Refit.StreamPart(file, fileName));
            if (result.Content != null)
            {
                return new FileModel
                {
                    FileName = result.Content.Name,
                    FullPath = result.Content.Path,
                    Id = result.Content.Id
                };
            }
            return null;
        }
    }
}
