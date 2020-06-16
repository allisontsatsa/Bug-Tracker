using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Bug_Tracker.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUserAvatar(this IIdentity user)
        {
            var ClaimsUser = (ClaimsIdentity)user;
            var claim = ClaimsUser.Claims.FirstOrDefault(c => c.Type == "AvatarPath");
            return claim != null ? claim.Value : null;
        }
        public static string GetFullName(this IIdentity user)
        {
            var ClaimsUser = (ClaimsIdentity)user;
            var claim = ClaimsUser.Claims.FirstOrDefault(c => c.Type == "FullName");
            return claim != null ? claim.Value : null;
        }
    }
}