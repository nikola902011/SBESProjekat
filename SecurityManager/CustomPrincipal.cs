using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Security.Principal;

namespace SecurityManager
{
    public  class CustomPrincipal : IPrincipal
    {
        IIdentity identity = null;
        public CustomPrincipal(IIdentity certificateIdentity)
        {
            identity = certificateIdentity;
        }

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string role)
        {
            Type x509IdentityType = identity.GetType();

            // The certificate is stored inside a private field of this class
            FieldInfo certificateField = x509IdentityType.GetField("certificate", BindingFlags.Instance | BindingFlags.NonPublic);

            X509Certificate2 certificate = (X509Certificate2)certificateField.GetValue(identity);

            string name = certificate.SubjectName.Name;
            string[] clientName = name.Split(';');
            string[] parts = clientName[0].Split(',');
            string[] roleName = parts[1].Split('=');
            if (roleName[1].Contains(':'))
            {
                string[] multipleRoles = roleName[1].Split(':');
                if (role.Equals(multipleRoles[0]) || role.Equals(multipleRoles[1]))
                {
                    return true;
                }

            }

            if (role.Equals(roleName[1]))
            {
                return true;
            }

            return false;
        }
    }
}