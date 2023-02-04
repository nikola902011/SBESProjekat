using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public enum AuditEventTypes
    {
        AuthenticationSuccessful = 0,
        AuthenticationFailed = 1,
        CreatedSuccessfully = 2,
        CreateFailed = 3
    }
    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager
                            (typeof(AuditEventFile).ToString(),
                            Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        public static string AuthenticationSuccessful
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthenticationSuccessful.ToString());
            }
        }

        public static string AuthenticationFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthenticationFailed.ToString());
            }
        }

        public static string CreatedSuccessfully
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.CreatedSuccessfully.ToString());
            }
        }

        public static string CreateFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.CreateFailed.ToString());
            }
        }
    }
}
