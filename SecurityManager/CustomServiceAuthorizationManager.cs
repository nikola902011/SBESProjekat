using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class CustomServiceAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            CustomPrincipal principal = operationContext.ServiceSecurityContext.
                AuthorizationContext.Properties["Principal"] as CustomPrincipal;

            bool retValue = principal.IsInRole("Viewer");

            if (!retValue)
            {
                throw new FaultException("Failed to authorize, you need to have a role of a Viewer!");
            }

            return retValue;
        }
    }
}
