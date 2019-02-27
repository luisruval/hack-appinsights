// Class to use custom TelemetryInitiliazer that sets the User ID
/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace Microsoft.eShopWeb.Web
{
     // Custom TelemetryInitializer that sets the user ID.
     
    public class WebSessionTelemetryInitializer : TelemetryInitializerBase
    {
        public WebSessionTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
             : base(httpContextAccessor)
        {
        }

        /// <summary>
        /// Called when initialize telemetry.
        /// </summary>
        /// <param name="platformContext">The platform context.</param>
        /// <param name="requestTelemetry">The request telemetry.</param>
        /// <param name="telemetry">The telemetry.</param>
        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (platformContext?.User?.Identity == null)
            {
                return;
            }

            telemetry.Context.User.Id = platformContext.User.Identity.Name;
            // telemetry.Context.User.AccountId = // get account from claims

            if (platformContext.Request.Cookies != null && platformContext.Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
            {
                var sessionCookieValue = platformContext.Request.Cookies[Constants.BASKET_COOKIENAME];
                if (!string.IsNullOrEmpty(sessionCookieValue))
                {
                    var sessionCookieParts = sessionCookieValue.Split('|');
                    if (sessionCookieParts.Length > 0)
                    {
                        // Currently SessionContext takes in only SessionId. The cookies has
                        // SessionAcquisitionDate and SessionRenewDate as well that we are not
                        // picking for now.
                        requestTelemetry.Context.Session.Id = sessionCookieParts[0];
                    }
                }
            }
        }

    }
}
*/