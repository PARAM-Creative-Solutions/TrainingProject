
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Repository.Implementation;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation
{
    /// <summary>
    /// Unit of work having all repositorie's propertiy
    /// </summary>
    public class UnitofWork : IUnitofWork, IDisposable
    {
        #region Properties

        private TrainingProjectEntities _context;
        /// <summary>
        /// Contex of Entity
        /// </summary>
        public TrainingProjectEntities Db
        {
            get { return _context; }
        }

     
        #endregion

        #region System Details
        private Repository<SystemDetail> _SystemDetailRepository;
        /// <summary>
        /// System Details Repository
        /// </summary>
        public Repository<SystemDetail> SystemDetailRepository
        {
            get
            {
                if (_SystemDetailRepository == null)
                {
                    _SystemDetailRepository = new Repository<SystemDetail>(this);
                }
                return _SystemDetailRepository;
            }
        }

        private Repository<SystemVersion> _SystemVersionRepository;
        /// <summary>
        /// System Version Details Repository
        /// </summary>
        public Repository<SystemVersion> SystemVersionRepository
        {
            get
            {
                if (_SystemVersionRepository == null)
                {
                    _SystemVersionRepository = new Repository<SystemVersion>(this);
                }
                return _SystemVersionRepository;
            }
        }

        #endregion

        #region ServerPath_DetailsRepository
        private Repository<ServerPathDetail> _ServerPath_DetailsRepository;
        /// <summary>
        /// System Details Repository
        /// </summary>
        public Repository<ServerPathDetail> ServerPath_DetailsRepository
        {
            get
            {
                if (_ServerPath_DetailsRepository == null)
                {
                    _ServerPath_DetailsRepository = new Repository<ServerPathDetail>(this);
                }
                return _ServerPath_DetailsRepository;
            }
        }
        #endregion

        #region _PlantRepository
        private Repository<Plant> _PlantRepository;
        /// <summary>
        /// Plant Repository
        /// </summary>
        public Repository<Plant> PlantRepository
        {
            get
            {
                if (_PlantRepository == null)
                {
                    _PlantRepository = new Repository<Plant>(this);
                }
                return _PlantRepository;
            }
        }
        #endregion

        #region _DepartmentRepository
        private Repository<Department> _DepartmentRepository;
        /// <summary>
        /// Department Repository
        /// </summary>
        public Repository<Department> DepartmentRepository
        {
            get
            {
                if (_DepartmentRepository == null)
                {
                    _DepartmentRepository = new Repository<Department>(this);
                }
                return _DepartmentRepository;
            }
        }
        #endregion

        #region _UserRepository
        private UserRepository _UserRepository;
        /// <summary>
        /// User Repository
        /// </summary>
        public UserRepository UserRepository
        {
            get
            {
                if (_UserRepository == null)
                {
                    _UserRepository = new UserRepository(this);
                }
                return _UserRepository;
            }
        }
        #endregion

        #region _UserLogRepository
        private Repository<UserLog> _UserLogRepository;
        /// <summary>
        /// UserLog Repository
        /// </summary>
        public Repository<UserLog> UserLogsRepository
        {
            get
            {
                if (_UserLogRepository == null)
                {
                    _UserLogRepository = new Repository<UserLog>(this);
                }
                return _UserLogRepository;
            }
        }
        #endregion

        #region _UserRoleRepository
        private Repository<UserRole> _UserRoleRepository;
        /// <summary>
        /// UserRole Repository
        /// </summary>
        public Repository<UserRole> UserRoleRepository
        {
            get
            {
                if (_UserRoleRepository == null)
                {
                    _UserRoleRepository = new Repository<UserRole>(this);
                }
                return _UserRoleRepository;
            }
        }
        #endregion

        #region _RoleRepository
        private RoleRepository _RoleRepository;
        /// <summary>
        /// Role Repository
        /// </summary>
        public RoleRepository RoleRepository
        {
            get
            {
                if (_RoleRepository == null)
                {
                    _RoleRepository = new RoleRepository(this);
                }
                return _RoleRepository;
            }
        }
        #endregion

        #region _ExceptionLogRepository
        private Repository<ExceptionLog> _ExceptionLogRepository;
        /// <summary>
        /// Exception_Logs Repository
        /// </summary>
        public Repository<ExceptionLog> ExceptionLogsRepository
        {
            get
            {
                if (_ExceptionLogRepository == null)
                {
                    _ExceptionLogRepository = new Repository<ExceptionLog>(this);
                }
                return _ExceptionLogRepository;
            }
        }
        #endregion


        #region _VendorListRepository
        private Repository<Vendor> _VendorListRepository;
        /// <summary>
        /// Master_Document_List Repository
        /// </summary>
        public Repository<Vendor> VendorRepository
        {
            get
            {
                if (_VendorListRepository == null)
                {
                    _VendorListRepository = new Repository<Vendor>(this);
                }
                return _VendorListRepository;
            }
        }
        #endregion


        #region RoleWiseRightRepository
        private Repository<RoleWiseRight> _RoleWiseRightRepository;
        /// <summary>
        /// RoleWiseRight Repository
        /// </summary>
        public Repository<RoleWiseRight> RoleWiseRightRepository
        {
            get
            {
                if (_RoleWiseRightRepository == null)
                {
                    _RoleWiseRightRepository = new Repository<RoleWiseRight>(this);
                }
                return _RoleWiseRightRepository;
            }
        }
        #endregion

        #region RightRepository
        private RightRepository _RightRepository;
        /// <summary>
        /// Right Repository
        /// </summary>
        public RightRepository RightRepository
        {
            get
            {
                if (_RightRepository == null)
                {
                    _RightRepository = new RightRepository(this);
                }
                return _RightRepository;
            }
        }
        #endregion

        #region UploadedFile Repository
        private Repository<UploadedFile> _UploadedFileRepository;
        /// <summary>
        /// UploadedFile Repository
        /// </summary>
        public Repository<UploadedFile> UploadedFileRepository
        {
            get
            {
                if (_UploadedFileRepository == null)
                {
                    _UploadedFileRepository = new Repository<UploadedFile>(this);
                }
                return _UploadedFileRepository;
            }
        }
        #endregion


        #region UserStatu Repository

        #region UserStatu
        private Repository<UserStatu> _UserStatusRepository;
        /// <summary>
        /// FolderType Repository
        /// </summary>
        public Repository<UserStatu> UserStatusRepository
        {
            get
            {
                if (_UserStatusRepository == null)
                {
                    _UserStatusRepository = new Repository<UserStatu>(this);
                }
                return _UserStatusRepository;
            }
        }
        #endregion
        #region UserStatu
        private Repository<PlantStatu> _PlantStatusRepository;
        /// <summary>
        /// FolderType Repository
        /// </summary>
        public Repository<PlantStatu> PlantStatusRepository
        {
            get
            {
                if (_PlantStatusRepository == null)
                {
                    _PlantStatusRepository = new Repository<PlantStatu>(this);
                }
                return _PlantStatusRepository;
            }
        }
        #endregion

        #region Colors

        private Repository<Color> _ColorRepository;
        /// <summary>
        /// FolderType Repository
        /// </summary>
        public Repository<Color> ColorRepository
        {
            get
            {
                if (_ColorRepository == null)
                {
                    _ColorRepository = new Repository<Color>(this);
                }
                return _ColorRepository;
            }
        }
        #endregion

        #endregion


        #region ReportType Repository
        private Repository<ReportsType> _ReportTypeRepository;
        /// <summary>
        /// ReportType Repository
        /// </summary>
        public Repository<ReportsType> ReportTypeRepository
        {
            get
            {
                if (_ReportTypeRepository == null)
                {
                    _ReportTypeRepository = new Repository<ReportsType>(this);
                }
                return _ReportTypeRepository;
            }
        }
        #endregion

        

        #region Constructor

        /// <summary>
        /// Initialize Context
        /// </summary>
        /// <param name="pContext"></param>
        public UnitofWork(TrainingProjectEntities pContext)
        {
            _context = pContext;
        }

        #endregion

        #region Methods




        #region REPORT
        #region GET REPORT DATA
        /// <summary>
        /// Fetch View name from ReportType
        /// Collects all Data in datatable
        /// </summary>
        /// <param name="ReportTypeId">Selected Report data</param>
        /// <returns>Returns All Report Data </returns>
        public DataSet GetReportData(int ReportTypeId)
        {
            #region DECLARATION
            SqlCommand cmd = null;
            SqlDataAdapter adpt = null;
            #endregion
            DataSet ds = new DataSet();

            #region FETCH DATA FOR TABLE IN REPORT
               #region Get Join Name for table in report and Create Select Query 
            string TableJoinName = ReportTypeRepository.Get(ReportTypeId).ReportTableView;

            string TableSqlQuery = "select * from " + TableJoinName;
            #endregion
            if (TableSqlQuery != null && TableSqlQuery != string.Empty)
            {
                #region Fill DataTable 

                //ASk Vikrant
                 cmd = new SqlCommand(TableSqlQuery, (SqlConnection)Db.Database.Connection);
                 adpt = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Table");
                adpt.Fill(dt);

                ds.Tables.Add(dt);
                #endregion
            }
            #endregion


            //ONLY IF REQUIRED DEPEND ON ReportDesignType
            #region FETCH DATA FOR DETAIL VIEW IN REPORT

            string DetailJoinName = ReportTypeRepository.Get(ReportTypeId).ReportDetailView;

            #region CHECK IF Detail View Table data is Required Or not
            if (DetailJoinName != null && DetailJoinName != string.Empty)
            {
                string DetailSqlQuery = "select * from " + DetailJoinName;
              

                #region Fill DataTable 

                //ASk Vikrant
                cmd = new SqlCommand(DetailSqlQuery, (SqlConnection)Db.Database.Connection);
                adpt = new SqlDataAdapter(cmd);
                DataTable detaildt = new DataTable("Detail");
                adpt.Fill(detaildt);

                ds.Tables.Add(detaildt);
                #endregion
            }
            #endregion
            #endregion


            return ds;
        }


        #endregion
        #endregion

        /// <summary>
        /// Save changes to the server
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return Db.SaveChanges();
        }

        private bool disposed = false;

        /// <summary>
        /// Status of disposing object
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


        #region Holiday Repository
        private Repository<Holiday> _HolidayRepository;
        /// <summary>
        /// Holiday Repository
        /// </summary>
        public Repository<Holiday> HolidayRepository
        {
            get
            {
                if (_HolidayRepository == null)
                {
                    _HolidayRepository = new Repository<Holiday>(this);
                }
                return _HolidayRepository;
            }
        }
        #endregion
    }
}
