using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TrainingProject.Security
{
    /// <summary>
    /// Class for Converting Byet[] to HttpPostedFileBase
    /// </summary>
    public class MemoryPostedFile : HttpPostedFileBase
    {
        /// <summary>
        /// Byte[]
        /// </summary>
        private readonly byte[] fileBytes;
        /// <summary>
        /// parameterize Constructor for MemoryPostedFile
        /// </summary>
        /// <param name="fileBytes">byte[]</param>
        /// <param name="fileName">Name Of File</param>
        /// <param name="contentType">Content Type of File</param>
        public MemoryPostedFile(byte[] fileBytes, string fileName, string contentType)
        {
            this.fileBytes = fileBytes;
            this.FileName = fileName;
            this.ContentType = contentType;
            this.InputStream = new MemoryStream(fileBytes);

        }
        /// <summary>
        /// Length Of File
        /// </summary>
        public override int ContentLength => fileBytes.Length;

        /// <summary>
        /// File Name
        /// </summary>
        public override string FileName { get; }

        /// <summary>
        ///When overridden in a derived class, gets the MIME content type of an uploaded  file.
        /// </summary>
        public override string ContentType { get; }

        /// <summary>
        /// Input Stream
        /// </summary>
        public override Stream InputStream { get; }

        /// <summary>
        /// When overridden in a derived class, saves the contents of an uploaded file.
        /// </summary>
        /// <param name="filename">The name of the file to save.</param>
        public override void SaveAs(string filename)
        {
            using (var file = File.Open(filename, FileMode.CreateNew))
                InputStream.CopyTo(file);
        }

    }

}
