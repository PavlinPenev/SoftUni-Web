using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_Ge.Services.EmailSender
{
    public class EmailAttachment
    {
        public byte[] Content { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }
    }
}
