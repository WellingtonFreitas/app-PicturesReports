using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PicturesReports.Model
{
    public class Galery
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Picture Cover { get; set; }
        public virtual ICollection<Picture> Images { get; set; }
    }
}
