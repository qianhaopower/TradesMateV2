
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;
using System.Web;
using EF.Data;

namespace DataService.Controllers
{

    public class StorageController : ApiController
{
	
        /// <summary>
        /// Uploads one or more blob files.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<BlobUploadModel>))]
        public async Task<IHttpActionResult> PostBlobUpload(int entityId, string type)
        {
            try
            {
                // This endpoint only supports multipart form data
                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    return StatusCode(HttpStatusCode.UnsupportedMediaType);
                }

                AttachmentEntityType typeParsed;
                bool typeValid = Enum.TryParse<AttachmentEntityType>(type, out typeParsed);
                if (typeValid == false)
                {
                    return BadRequest(string.Format("{0} is not a valid entity type for attachments", type)) ;
                }

                var repo = new StorageRepository();
                // Call service to perform upload, then check result to return as content
                var result = await repo.UploadBlobs(Request.Content,  entityId, typeParsed, User.Identity.Name);
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }

                // Otherwise
                return BadRequest("Upload failed");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Downloads a blob file.
        /// </summary>
        /// <param name="blobId">The ID of the blob.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetBlobDownload(int entityId, string type, int attachmentId)
        {
            // IMPORTANT: This must return HttpResponseMessage instead of IHttpActionResult

            try
            {

                AttachmentEntityType typeParsed;
                bool typeValid = Enum.TryParse<AttachmentEntityType>(type, out typeParsed);
                if (typeValid == false)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("{0} is not a valid entity type for attachments", type));
                }
                var repo = new StorageRepository();
                var result = await repo.DownloadBlob(entityId, typeParsed, attachmentId);
                if (result == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

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