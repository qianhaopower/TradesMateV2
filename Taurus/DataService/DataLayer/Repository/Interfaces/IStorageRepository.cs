using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EF.Data
{

    public interface IStorageRepository : IBaseRepository
    {

        Task<List<BlobUploadModel>> UploadBlobs(HttpContent content, int entityId, AttachmentEntityType type, string userName);

        Task<BlobDownloadModel> DownloadBlob(int entityId, AttachmentEntityType type, int attachmentId, string userName);


        Task<bool> DeleteBlob(int attachmentId, int entityId, AttachmentEntityType type, string userName);


        List<Attachment> GetEntityAttachments(int entityId, AttachmentEntityType type, string userName);

        bool IsImageExtension(string fileName);

    }
    
}