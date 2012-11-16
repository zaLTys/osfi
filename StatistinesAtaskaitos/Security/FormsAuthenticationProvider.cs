using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using System.Security.Cryptography;
using NHibernate;
using NHibernate.Linq;
using Vic.ZubStatistika.Entities;

namespace StatistinesAtaskaitos.Security
{
    public interface IAuthenticationProvider
    {
        bool TryAuthenticate();
        bool LogIn(string username, string password, bool rememberMe);
        void LogOut();
    }

    public class FormsAuthenticationProvider : IAuthenticationProvider
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly HashAlgorithm _hashAlgorithm;

        public FormsAuthenticationProvider(ISessionFactory sessionFactory, HashAlgorithm hashAlgorithm)
        {
            _sessionFactory = sessionFactory;
            _hashAlgorithm = hashAlgorithm;
        }

        public virtual void SetUser(IIdentity identity, User user)
        {
            var userInformation = new UserInformation
                                      {
                                          Id = user.Id,
                                          Name = (user.Vardas + " " + user.Pavarde).Trim(),
                                          Role = user.Role
                                      };

            var principal = new OsfiPrincipal(identity, userInformation);
            HttpContext.Current.User = principal;
        }

        public virtual void SetAnonymousUser()
        {
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(string.Empty), new string[0]);
        }

        public virtual bool TryAuthenticate()
        {
            // get user from cookie
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = null;
                try { ticket = FormsAuthentication.Decrypt(authCookie.Value); }
                catch { }

                if (ticket != null && !ticket.Expired)
                {
                    var user = GetValidatedUser(ticket.Name);
                    if (user != null)
                    {
                        var newTicket = FormsAuthentication.RenewTicketIfOld(ticket);

                        SetUser(new FormsIdentity(newTicket), user);

                        if (newTicket != ticket)
                        {
                            FormsAuthentication.SetAuthCookie(ticket.Name, ticket.IsPersistent);
                        }

                        return true;
                    }

                }
            }

            // TODO: mark user as not authenticated
            SetAnonymousUser();

            return false;
        }

        /// <summary>
        /// Used to authenticate user when he logs in
        /// </summary>
        /// <param name="username"></param>
        public virtual bool LogIn(string username, string password, bool rememberMe)
        {
            var user = GetUser(username);

            if (ValidateUser(user, password))
            {
                FormsAuthentication.SetAuthCookie(username, rememberMe);

                return true;
            }

            return false;
        }

        public virtual void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        protected virtual User GetUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            var lowercaseUser = username.ToLower();

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                return session.Query<User>().FirstOrDefault(x => x.Username.ToLower() == lowercaseUser);
            }
        }

        protected virtual User GetValidatedUser(string username)
        {
            var user = GetUser(username);
            if (ValidateUser(user, null))
                return user;
            else
                return null;
        }

        protected virtual bool ValidateUser(User user, string password)
        {
            if (user == null) return false;
            if (password == null) return true;

            var hash = _hashAlgorithm.GetHashedString(password);
            return user.Password == hash;
        }
    }

    public class UserInformation
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class OsfiPrincipal : IPrincipal
    {
        private readonly IIdentity _identity;
        private readonly UserInformation _user;

        public OsfiPrincipal(IIdentity identity, UserInformation user)
        {
            _identity = identity;
            _user = user;
        }

        public UserInformation User
        {
            get { return _user; }
        }

        public bool IsInRole(string role)
        {
            return _user.Role.Equals(role, StringComparison.InvariantCultureIgnoreCase);
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }
    }
}