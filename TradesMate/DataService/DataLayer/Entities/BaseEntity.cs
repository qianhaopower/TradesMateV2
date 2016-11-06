using System;

namespace EF.Data
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime AddedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
       
    }
}
