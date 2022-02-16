using TrainingProjectDataLayer.Constants;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TrainingProject.Security
{
    /// <summary>
    /// Appication hub which keeps the track of the connected clients to the hub
    /// </summary>
    [HubName("myapplicationhub")]
    public class MyApplicationHub : Hub
    {
        /// <summary>
        /// Current Loged In User
        /// </summary>
        public CustomPrincipal CurrentUser
        {
            get
            {
                return Context.User as CustomPrincipal;
            }
        }
        #region Methods

        #region Hello

        /// <summary>
        /// Displays the hello messages to all the logged in users
        /// </summary>
        public void Hello()
        {
            Clients.All.hello();
        }

        #endregion

        #region OnConnected
        /// <summary>
        /// Gets called when any client is connected to this hub
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            using (UnitofWork uow = new UnitofWork(new TrainingProjectEntities()))
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<MyApplicationHub>();
                string CLientIP_Address = HelperMethods.GetIPAddress(Context.Request.GetHttpContext());
                if (CurrentUser != null)
                {
                    UserLog uo = uow.Db.UserLogs.Where(x => x.SessionId.ToString() == CurrentUser.SessionId).FirstOrDefault();
                    if (uo != null)
                    {
                        //uo.SystemInfo = clientMachineName;
                        uo.LastAcessedOn = DateTime.Now;
                        uo.OnlineStatus = true;
                        uo.ConnectionId = Context.ConnectionId;
                        uo.IPAddress = CLientIP_Address;
                        uow.UserLogsRepository.Update(uo);
                        uow.SaveChanges();
                    }
                }
            }
            UpdateOnlineUsersCount();
            return base.OnConnected();
        }

        #endregion

        #region OnDisconnected

        /// <summary>
        /// Gets called when any client is disconnected from this hub i.e Application
        /// </summary>
        /// <param name="stopCalled">stop is called by user or not</param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            using (UnitofWork uow = new UnitofWork(new TrainingProjectEntities()))
            {
                if (CurrentUser != null)
                {
                    UserLog uo = uow.Db.UserLogs.Where(x => x.SessionId.ToString() == CurrentUser.SessionId).FirstOrDefault();
                    if (uo != null)
                    {
                        uo.LastAcessedOn = DateTime.Now;
                        uo.OnlineStatus = false;
                        uow.SaveChanges();
                    }
                }
            }
            UpdateOnlineUsersCount();
            return base.OnDisconnected(stopCalled);
        }

        #endregion

        #region OnReconnected

        /// <summary>
        /// Gets called when client is reconnected to the hub
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            UpdateOnlineUsersCount();
            return base.OnReconnected();
        }

        #endregion

        #endregion

        /// <summary>
        /// Set online Users count
        /// </summary>
        public static void UpdateOnlineUsersCount()
        {
            using (UnitofWork uow = new UnitofWork(new TrainingProjectEntities()))
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<MyApplicationHub>();
                int _userCount = uow.Db.UserLogs.Where(x => x.OnlineStatus).DistinctBy(w => w.UserId).Count();
                context.Clients.All.online(_userCount);
            }
        }

        /// <summary>
        /// Log out user by connection Id
        /// </summary>
        /// <param name="ConnectionId"></param>
        public static void LogOutUser(string ConnectionId)
        { 
            try
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<MyApplicationHub>();
                context.Clients.Client(ConnectionId).RedirectToLoggoff();
            }
            catch (Exception ex)
            {
            }
        }

    }
}