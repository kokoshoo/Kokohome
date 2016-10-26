using Decks;
using MySite.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MySite.Attributes
{
    public class IsUserDeck : AuthorizeAttribute
    {
        private DecksDb db = new DecksDb();
        public string UserId;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
            {
                return false;
            }

            var rd = httpContext.Request.RequestContext.RouteData;

            int id = (int)rd.Values["id"];

            return db.UserOwnsDeck(UserId, id);
        }
    }
}