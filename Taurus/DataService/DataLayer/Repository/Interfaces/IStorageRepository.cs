using DataService.Entities;
using DataService.Infrastructure;
using DataService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace EF.Data
{

    public interface IStorageRepository 
    {

        Task<List<BlobUploadModel>> UploadBlobs(HttpContent content, int entityId, AttachmentEntityType type, string userName);

        Task<BlobDownloadModel> DownloadBlob(int entityId, AttachmentEntityType type, int attachmentId, string userName);


        Task<bool> DeleteBlob(int attachmentId, int entityId, AttachmentEntityType type, string userName);


        List<Attachment> GetEntityAttachments(int entityId, AttachmentEntityType type, string userName);

        List<Attachment> GetPropertyWorkItemsAttachments(int propertyId, string userName);

        bool IsImageExtension(string fileName);

    }
    
}