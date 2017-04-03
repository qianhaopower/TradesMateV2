

using EF.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataService.Models
{
    public class AttachmentModel
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public long SizeInBytes { get; set; }
		public AttachmentEntityType EntityType { get; set; }
		public int EntityId { get; set; }
		public int BlobId { get; set; }
		public AttachmentType Type { get; set; }


	}


}