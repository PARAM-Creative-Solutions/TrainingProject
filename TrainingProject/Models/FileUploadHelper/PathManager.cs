using TrainingProject.Security;
using TrainingProjectDataLayer.Constants;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace TrainingProject.Models.FileUploadHelper
{
    /// <summary>
    /// A class for getting the path for documents to be saved
    /// </summary>
    public class PathManager
    {
        #region Properties
        /// <summary>
        /// UnitofWork
        /// </summary>
        public UnitofWork uow{ get; set; }
        /// <summary>
        /// Property for So Common Docs
        /// </summary>
        public List<string> soCommonDocs { get; set; }
        /// <summary>
        /// Property for Projec tCommon Docs
        /// </summary>
        public List<string> SalesOrderCommonDocs { get ; set; }
        /// <summary>
        /// UploadedFile
        /// </summary>
        public UploadedFile uploadedfile { get; set; }

        /// <summary>
        /// property for User
        /// </summary>
        public User user { get; set; }

        /// <summary>
        /// FilePath
        /// </summary>
        public string FilePath { get; set; }

        CustomPrincipal CurrentUser { get { return HttpContext.Current.User as CustomPrincipal; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PathManager(UnitofWork uow)
        {
            this.uow = uow;
        }

        /// <summary>
        /// Constructor used for upload image in user profile
        /// </summary>
        /// <param name="user"></param>
        /// <param name="uow"></param>
        public PathManager(User user, UnitofWork uow)
        {
            this.uow = uow;
            this.user = user;
        }


        #endregion

        #region Methods

        #region SaveUploadedFileData
        /// <summary>
        /// to save uploaded file details in upload file table
        /// added by priyanka (24/06/2019)
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public UploadedFile SaveUploadedFileData(HttpPostedFileBase File)
        {
            CustomPrincipal CurrentUser = HttpContext.Current.User as CustomPrincipal;
            UploadedFile file = uow.UserRepository.Get(g => g.UserId == CurrentUser.Id).UploadedFile;
            if(file==null)
            {
                file = new UploadedFile();
                file.ContentType = File.ContentType;
                file.FileName = Path.GetFileName(File.FileName);
                file.UploadedOn = DateTime.Now;
                file.CreatedBy = CurrentUser.Id;
                file.FileExtention = Path.GetExtension(File.FileName);
                file.FilePath = FilePath;
                uow.UploadedFileRepository.Add(file);
            }
            else
            {
                file.ContentType = File.ContentType;
                file.FileName = Path.GetFileName(File.FileName);
                file.UploadedOn = DateTime.Now;
                file.CreatedBy = CurrentUser.Id;
                file.FileExtention = Path.GetExtension(File.FileName);
                file.FilePath = FilePath;
            }   
            uow.SaveChanges();
            var destinationPath = Path.Combine(FilePath, Convert.ToString(file.Id));
            var filePath = Path.Combine(FilePath,file.Id.ToString(),Path.GetFileName(File.FileName));
            //create directory if not exists
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }
            //**********delete the file under folder if exists
            else
            {
                DirectoryInfo directory = new DirectoryInfo(destinationPath);
                directory.GetFiles().ToList().ForEach(f => f.Delete());
            }

            if (Constants.IsEncryptionDecryptionEnable)
            {
                EncryptDecrypt.EncryptFile(File, filePath);
            }
            else
            {
              File.SaveAs(filePath);
            }
            return file;
        }
        #endregion

        #region UpdateUploadedFileData
        /// <summary>
        /// to update uploaded file details in upload file table
        /// added by priyanka (24/06/2019)
        /// </summary>
        /// <param name="File"></param>
        /// <param name="upfileId"></param>
        /// <returns></returns>
        public UploadedFile UpdateUploadedFileData(HttpPostedFileBase File, long? upfileId)
        {
            CustomPrincipal CurrentUser = HttpContext.Current.User as CustomPrincipal;
            uploadedfile = uow.UploadedFileRepository.Get(x => x.Id == upfileId);

            uploadedfile.ContentType = File.ContentType;
            uploadedfile.FileName = Path.GetFileName(File.FileName);
            //upfile.FileData = bytes;
            uploadedfile.UploadedOn = DateTime.Now;
            uploadedfile.LastModifiedBy = CurrentUser.Id;
            uploadedfile.FileExtention = Path.GetExtension(File.FileName);
            uploadedfile.FilePath = FilePath;
            uow.UploadedFileRepository.Update(uploadedfile);
            uow.SaveChanges();
            var destinationPath = Path.Combine(FilePath, uploadedfile.Id.ToString());
            var filePath = Path.Combine(FilePath, uploadedfile.Id.ToString(), Path.GetFileName(File.FileName));
            //create directory if not exists
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }
            //**********delete the file under folder if exists
            else
            {
                DirectoryInfo directory = new DirectoryInfo(destinationPath);
                directory.GetFiles().ToList().ForEach(f => f.Delete());
            }
            if (Constants.IsEncryptionDecryptionEnable)
            {
                EncryptDecrypt.EncryptFile(File, filePath);
            }
            else
            {
                File.SaveAs(filePath);
            }
            return uploadedfile;
        }
        #endregion

        #region CopyAllFiles

        /// <summary>
        /// Method for Copy multiple files And folders from one folder to another added by priyanka gaharwal 
        /// </summary>

        public bool CopyAllFiles(string sourcePath, string destinationPath)
        {
            try
            {
                if (Directory.Exists(sourcePath))
                {
                    if (!Directory.Exists(destinationPath))
                    {
                        Directory.CreateDirectory(destinationPath);
                    }
                    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*",
                             SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

                    foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",
                    SearchOption.AllDirectories))
                        System.IO.File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
                  
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                if (Directory.Exists(destinationPath))
                { //delete copied files and folders if problem ocuured in between to copy folder
                    Directory.Delete(destinationPath, true);
                }
                return false;
            }
        }

        #endregion

        #region DownloadDocument
        /// <summary>
        /// Downloads the Documents(SODocuments)
        /// 
        /// </summary>
        /// <param name="id">SO_Doc_Id from SO_Document_List</param>
        /// <param name="version">Latest_Version from SO_Document_List</param>
        /// <param name="type">type of document</param>
        /// <returns></returns>
        //public byte[] DownloadDocument(long id, int? version, SystemEnums.SubmissionType type)
        //{
        //    byte[] filedata = null;
        //    uploadedfile = uow.UploadedFileRepository.Get(x => x.Id == id);
        //    FilePath = uploadedfile.FilePath;
        //    var sourcePath = Path.Combine(FilePath, Convert.ToString(id),uploadedfile.FileName);
        //    if (Constants.IsEncryptionDecryptionEnable)
        //    {
        //        filedata = EncryptDecrypt.DecryptFile(sourcePath);
        //    }
        //    else
        //    {
        //        filedata = File.ReadAllBytes(sourcePath);
        //    }
        //    DocumentAccessLog DocStatus = new DocumentAccessLog();
        //    DocStatus.UserId = CurrentUser.Id;
        //    DocStatus.LogStatus = Convert.ToInt16(SystemEnums.DocumentAccessLogType.Downloaded);
        //    DocStatus.SODocumentVersionsId = version == null? 0 : (int)version;
        //    DocStatus.SubmissionType = (int)type;
        //    DocStatus.UploadedFileId = id;
        //    DocStatus.AccessTime = DateTime.Now;
        //    uow.DocumentAccessLogRepository.Add(DocStatus);
        //    uow.SaveChanges();

        //    return filedata;
        //}
        #endregion

        #region DownloadDocument
        /// <summary>
        /// Downloads the Documents(SODocuments)
        /// 
        /// </summary>
        /// <param name="id">SO_Doc_Id from SO_Document_List</param>
        /// <param name="version">Latest_Version from SO_Document_List</param>
        /// <param name="type">type of document</param>
        /// <returns></returns>
        public byte[] DownloadDocument(long id)
        {
            byte[] filedata = null;
            uploadedfile = uow.UploadedFileRepository.Get(x => x.Id == id);
            var sourcePath = Path.Combine(FilePath, Convert.ToString(id), uploadedfile.FileName);
            if (Constants.IsEncryptionDecryptionEnable)
            {
                filedata = EncryptDecrypt.DecryptFile(sourcePath);
            }
            else
            {
                filedata = File.ReadAllBytes(sourcePath);
            }
            //DocumentAccessLog DocStatus = new DocumentAccessLog();
            //DocStatus.UserId = CurrentUser.Id;
            //DocStatus.LogStatus = Convert.ToInt16(SystemEnums.DocumentAccessLogType.Downloaded);
            //DocStatus.SODocumentVersionsId = 0;
            //DocStatus.SubmissionType = 0;
            //DocStatus.UploadedFileId = id;
            //DocStatus.AccessTime = DateTime.Now;
            //uow.DocumentAccessLogRepository.Add(DocStatus);
            //uow.SaveChanges();

            return filedata;
        }
        #endregion


        #endregion
    }
}

