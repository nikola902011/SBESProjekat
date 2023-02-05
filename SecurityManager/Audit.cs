using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public  class Audit : IDisposable
    {
        private static EventLog customLog = null;
        const string SourceName = "SecurityManager.Audit";
        const string LogName = "MySecTest";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }


        public static void AuthenticationSuccessful(string username, string reason)
        {
            if (customLog == null)
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthenticationSuccessful));
            }

            string AuthenticationSuccessful = AuditEvents.AuthenticationSuccessful;
            string message = String.Format(AuthenticationSuccessful, username, reason);
            customLog.WriteEntry(message);
        }

        public static void AuthenticationFailed(string username, string reason)
        {
            if (customLog == null)
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthenticationFailed));
            }
            string AuthenticationFailed = AuditEvents.AuthenticationFailed;
            string message = String.Format(AuthenticationFailed, username, reason);
            customLog.WriteEntry(message);
        }

        public static void CreatedSuccessfully(string filename, string reason)
        {
            if (customLog == null)
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.CreatedSuccessfully));
            }

            string CreatedSuccessfully = AuditEvents.CreatedSuccessfully;
            string message = String.Format(CreatedSuccessfully, filename, reason);
            customLog.WriteEntry(message);
        }

        public static void CreateFailed(string filename, string reason)
        {
            if (customLog == null)
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.CreateFailed));
            }
            string CreateFailed = AuditEvents.CreateFailed;
            string message = String.Format(CreateFailed, filename, reason);
            customLog.WriteEntry(message);
        }


        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
