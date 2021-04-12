using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PicturesReports.Model
{
    public class Picture
    {
        public Guid Id { get; set; }
        public byte[] ImageBinary { get; set; }
        public Guid GaleryId { get; set; }

    }
}