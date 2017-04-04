﻿using DataService.Entities;
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

    public class StorageRepository : IDisposable
    {
        private EFDbContext _ctx;

        // Interface in place so you can resolve with IoC container of your choice
        private readonly IBlobService _service = new BlobService();

        private UserManager<ApplicationUser> _userManager;

        private  readonly string[] _validExtensions = { "jpg", "bmp", "gif", "png", "jpeg" }; //  etc

        public StorageRepository(EFDbContext ctx = null)
        {
            if (ctx != null)
            {
                _ctx = ctx;
            }
            else
            {
                _ctx = new EFDbContext();
            }
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }

        public async Task<List<BlobUploadModel>> UploadBlobs(HttpContent content, int entityId, AttachmentEntityType type, string userName)
        {
            
            if(CheckUserPermissionForEntity(entityId, type, userName))
            {
                var result = await _service.UploadBlobs(content);

                foreach(var item in result)
                {

                    Attachment newAttachment = new Attachment
                    {
                        Name = item.FileName,
                        Url = item.FileUrl,
                        SizeInBytes = item.FileSizeInBytes,
                        EntityType = type,
                        EntityId = entityId,
                      
                        Type = IsImageExtension(item.FileName) ? AttachmentType.Image : AttachmentType.Document,
                        AddedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                    };
                    _ctx.Entry(newAttachment).State = EntityState.Added;
                }

                _ctx.SaveChanges();

                return result;
            }
            else
            {
                throw new Exception(string.Format("User {0} has no permission to add attachment on {1} with id {2}", userName, type, entityId));
            }

        }

        public async Task<BlobDownloadModel> DownloadBlob(int entityId, AttachmentEntityType type, int attachmentId)
		{
            var allBlobs = _ctx.Attchments.Where(p =>  //add the type and entityId here just to make sure right attachment has been fetched.
			p.EntityType == type
			&& p.EntityId == entityId
			&& p.Id == attachmentId).ToList();
            if (allBlobs.Any())
            {
                return await _service.DownloadBlob(allBlobs.First().Name);
            }
            else
            {
                return await Task.FromResult<BlobDownloadModel>(null);
            }
        }

		public  List<Attachment> GetEntityAttachments(int entityId, AttachmentEntityType type )
		{
			var allBlobs = _ctx.Attchments.Where(p =>  //add the type and entityId here just to make sure right attachment has been fetched.
			p.EntityType == type
			&& p.EntityId == entityId
			).ToList();
			return allBlobs;
		}

		
		public bool IsImageExtension(string fileName)
        {
            return _validExtensions.Any(p=> fileName.Contains(p));
        }

        /// <summary>
        /// Check if user has permission for the entity selected. 
        /// </summary>
        /// <returns></returns>
        private bool CheckUserPermissionForEntity(int entityId, AttachmentEntityType type, string userName)
        {
            var valid = false;
            var propertyId = 0;
            if(type == AttachmentEntityType.Property)
            {
                propertyId = entityId;
            }
            else if(type == AttachmentEntityType.WorkItem)
            {
                var workItem = _ctx.WorkItems.Where(p => p.Id == entityId).First();
                
                if(workItem != null)
                {
                    var section = _ctx.Sections.Where(p => p.Id == workItem.SectionId).First();
                    if (section != null)
                        propertyId = section.PropertyId;
                }
            }
            var allowedProperties = new PropertyRepository(_ctx).GetPropertyForUser(userName).ToList().Select(p => p.Id);
            valid = allowedProperties.Where(p => p == propertyId).Count() == 1;//found
            return valid;

        }




        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }

      
    }

    public interface IBlobService
    {
        Task<List<BlobUploadModel>> UploadBlobs(HttpContent httpContent);
        Task<BlobDownloadModel> DownloadBlob(string blobName);
    }

    public class BlobService : IBlobService
    {
        public async Task<List<BlobUploadModel>> UploadBlobs(HttpContent httpContent)
        {
            var blobUploadProvider = new BlobStorageUploadProvider();

            var list = await httpContent.ReadAsMultipartAsync(blobUploadProvider)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        throw task.Exception;
                    }

                    var provider = task.Result;
                    return provider.Uploads.ToList();
                });

            // TODO: Use data in the list to store blob info in your
            // database so that you can always retrieve it later.

            return list;
        }

        public async Task<BlobDownloadModel> DownloadBlob(string blobName)
        {
            // TODO: You must implement this helper method. It should retrieve blob info
            // from your database, based on the blobId. The record should contain the
            // blobName, which you should return as the result of this helper method.
            //var blobName = string.Empty;// GetBlobName(blobId);

            if (!String.IsNullOrEmpty(blobName))
            {
                var container = BlobHelper.GetBlobContainer();
                var blob = container.GetBlockBlobReference(blobName);

                // Download the blob into a memory stream. Notice that we're not putting the memory
                // stream in a using statement. This is because we need the stream to be open for the
                // API controller in order for the file to actually be downloadable. The closing and
                // disposing of the stream is handled by the Web API framework.
                var ms = new MemoryStream();
                await blob.DownloadToStreamAsync(ms);

                // Strip off any folder structure so the file name is just the file name
                var lastPos = blob.Name.LastIndexOf('/');
                var fileName = blob.Name.Substring(lastPos + 1, blob.Name.Length - lastPos - 1);
				//remove the timestamp tag
				fileName = fileName.Split('_')[1];

                // Build and return the download model with the blob stream and its relevant info
                var download = new BlobDownloadModel
                {
                    BlobStream = ms,
                    BlobFileName = fileName,
                    BlobLength = blob.Properties.Length,
                    BlobContentType = blob.Properties.ContentType
                };

                return download;
            }

            // Otherwise
            return null;
        }
    }

    public class BlobUploadModel
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public long FileSizeInBytes { get; set; }
        public long FileSizeInKb { get { return (long)Math.Ceiling((double)FileSizeInBytes / 1024); } }
    }

    public class BlobDownloadModel
    {
        public MemoryStream BlobStream { get; set; }
        public string BlobFileName { get; set; }
        public string BlobContentType { get; set; }
        public long BlobLength { get; set; }
    }

    public static class BlobHelper
    {
        public static CloudBlobContainer GetBlobContainer()
        {
            // Pull these from config
            var blobStorageConnectionString = ConfigurationManager.AppSettings["BlobStorageConnectionString"];
            var blobStorageContainerName = ConfigurationManager.AppSettings["BlobStorageContainerName"];

            // Create blob client and return reference to the container
            var blobStorageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);
            var blobClient = blobStorageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(blobStorageContainerName);
        }
    }

    public class BlobStorageUploadProvider : MultipartFileStreamProvider
    {
        public List<BlobUploadModel> Uploads { get; set; }

        public BlobStorageUploadProvider() : base(Path.GetTempPath())
        {
            Uploads = new List<BlobUploadModel>();
        }

        public override Task ExecutePostProcessingAsync()
        {
            // NOTE: FileData is a property of MultipartFileStreamProvider and is a list of multipart
            // files that have been uploaded and saved to disk in the Path.GetTempPath() location.

            foreach (var fileData in FileData)
            {
                if (fileData.Headers.ContentDisposition.FileName == null)
                    continue;
                // Sometimes the filename has a leading and trailing double-quote character
                // when uploaded, so we trim it; otherwise, we get an illegal character exception
                var fileName = Path.GetFileName(fileData.Headers.ContentDisposition.FileName.Trim('"'));
                fileName = DateTime.UtcNow.Ticks.ToString() + "_" + fileName; //make each time we upload a different file name
                                                                              // Retrieve reference to a blob
                var blobContainer = BlobHelper.GetBlobContainer();
                var blob = blobContainer.GetBlockBlobReference(fileName);

                // Set the blob content type
                blob.Properties.ContentType = fileData.Headers.ContentType.MediaType;

                // Upload file into blob storage, basically copying it from local disk into Azure
                using (var fs = File.OpenRead(fileData.LocalFileName))
                {
                    blob.UploadFromStream(fs);
                }

                // Delete local file from disk
                File.Delete(fileData.LocalFileName);

                // Create blob upload model with properties from blob info
                var blobUpload = new BlobUploadModel
                {
                    FileName = blob.Name,
                    FileUrl = blob.Uri.AbsoluteUri,
                    FileSizeInBytes = blob.Properties.Length
                };

                // Add uploaded blob to the list
                Uploads.Add(blobUpload);
            }

            return base.ExecutePostProcessingAsync();
        }
    }
}