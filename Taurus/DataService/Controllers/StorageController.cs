using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using DataService.Models;
using EF.Data;

namespace DataService.Controllers
{
    [Authorize]
    [RoutePrefix("api/storage")]
    public class StorageController : ApiController
    {
        private readonly IStorageRepository _storageRepo;
        private IAuthRepository _authRepo;

        public StorageController(IAuthRepository authRepo, IStorageRepository storageRepo)
        {
            _authRepo = authRepo;
            _storageRepo = storageRepo;
        }

        [HttpPost]
        [ResponseType(typeof(List<BlobUploadModel>))]
        [Route("upload")]
        public async Task<IHttpActionResult> PostBlobUpload(int entityId, string type)
        {
            try
            {
                // This endpoint only supports multipart form data
                if (!Request.Content.IsMimeMultipartContent("form-data"))
                    return StatusCode(HttpStatusCode.UnsupportedMediaType);

                var typeValid = Enum.TryParse<AttachmentEntityType>(type, out var typeParsed);
                if (typeValid == false) return BadRequest($"{type} is not a valid entity type for upload");


                // Call service to perform upload, then check result to return as content
                var result = await _storageRepo.UploadBlobs(Request.Content, entityId, typeParsed, User.Identity.Name);
                if (result != null && result.Count > 0) return Ok(result);

                // Otherwise
                return BadRequest("Upload failed");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Route("")]
        public IHttpActionResult GetBlobModels(int entityId, string entityType)
        {
            AttachmentEntityType typeParsed;
            var typeValid = Enum.TryParse(entityType, out typeParsed);
            if (typeValid == false)
                return BadRequest($"{entityType} is not a valid entity type for attachments");

            var result = _storageRepo.GetEntityAttachments(entityId, typeParsed, User.Identity.Name);

            var returnList = result.Select(Mapper.Map<Attachment, AttachmentModel>).ToList();
            return Ok(returnList);
        }

        [HttpDelete]
        [Route("")]
        public async Task<IHttpActionResult> DeleteBlob(int entityId, string entityType, int attachmentId)
        {
            AttachmentEntityType typeParsed;
            var typeValid = Enum.TryParse(entityType, out typeParsed);
            if (typeValid == false)
                return BadRequest(string.Format("{0} is not a valid entity type for attachments", entityType));

            var result = await _storageRepo.DeleteBlob(attachmentId, entityId, typeParsed, User.Identity.Name);
            if (result)
                return StatusCode(HttpStatusCode.NoContent);
            return BadRequest();
        }


        /// <summary>
        ///     Downloads a blob file.
        /// </summary>
        /// <param name="blobId">The ID of the blob.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("download")]
        public async Task<HttpResponseMessage> GetBlobDownload(int entityId, string type, int attachmentId)
        {
            // IMPORTANT: This must return HttpResponseMessage instead of IHttpActionResult

            try
            {
                AttachmentEntityType typeParsed;
                var typeValid = Enum.TryParse(type, out typeParsed);
                if (typeValid == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        $"{type} is not a valid entity type for attachments");

                var result = await _storageRepo.DownloadBlob(entityId, typeParsed, attachmentId, User.Identity.Name);
                if (result == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

                // Reset the stream position; otherwise, download will not work
                result.BlobStream.Position = 0;

                // Create response message with blob stream as its content
                var message = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(result.BlobStream)
                };

                // Set content headers
                message.Content.Headers.ContentLength = result.BlobLength;
                message.Content.Headers.ContentType = new MediaTypeHeaderValue(result.BlobContentType);
                message.Content.Headers.Add("Access-Control-Expose-Headers", "content-disposition");
                message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = HttpUtility.UrlDecode(result.BlobFileName),
                    Size = result.BlobLength
                };

                return message;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(ex.Message)
                };
            }
        }
    }
}