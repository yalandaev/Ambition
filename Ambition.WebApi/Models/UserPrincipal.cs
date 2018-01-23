using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Ambition.WebApi.Models
{
    public class ShopperPrincipal : ClaimsPrincipal
    {
        public ShopperPrincipal(ClaimsIdentity identity)
            : base(new ShopperIdentity(identity))
        {
        }

        public ShopperIdentity ShopperIdentity
        {
            get
            {
                return Identity as ShopperIdentity;
            }
        }

        public new static ShopperPrincipal Current
        {
            get
            {
                return Thread.CurrentPrincipal as ShopperPrincipal;
            }
        }
    }

    public class ShopperIdentity : Open360IdentityBase
    {
        public ShopperIdentity(Shopper shopper, LoginType loginType, ClientPlatform platform)
            : base(GetBaseClaims(shopper, loginType, platform, Constants.SHOPPER_ROLE))
        {
        }

        public ShopperIdentity(ClaimsIdentity identity)
            : base(identity)
        {
        }

        public static ShopperIdentity Current
        {
            get
            {
                var current = ShopperPrincipal.Current;
                if (current == null)
                    return null;

                return current.ShopperIdentity;
            }
        }
    }

    public class Open360IdentityBase : ClaimsIdentity
    {
        public const string SESSION_ID_CLAIM = "http://schemas.open360.com/ws/2015/04/identity/claims/session_id";
        public const string LOGIN_TYPE_CLAIM = "http://schemas.open360.com/ws/2015/04/identity/claims/login_type";
        public const string CLIENT_PLATFORM_CLAIM = "http://schemas.open360.com/ws/2015/04/identity/claims/client_platform";

        private readonly Lazy<long> m_UserID;
        public long UserID
        {
            get
            {
                return m_UserID.Value;
            }
        }

        private readonly Lazy<string> m_Role;
        public string Role
        {
            get
            {
                return m_Role.Value;
            }
        }



        private readonly Lazy<Guid> m_SessionID;
        public Guid SessionID
        {
            get
            {
                return m_SessionID.Value;
            }
        }

        private readonly Lazy<LoginType> m_LoginType;
        public LoginType LoginType
        {
            get
            {
                return m_LoginType.Value;
            }
        }

        private readonly Lazy<ClientPlatform> m_ClientPlatform;
        public ClientPlatform ClientPlatform
        {
            get
            {
                return m_ClientPlatform.Value;
            }
        }

        protected static IEnumerable<Claim> GetBaseClaims(IOpen360User user, LoginType loginType, ClientPlatform platform, string role = null)
        {
            return new[]
            {
                new Claim(ClaimTypes.Sid, user.ID.ToString(CultureInfo.InvariantCulture)),
                new Claim(SESSION_ID_CLAIM, Guid.NewGuid().ToString()),
                new Claim(LOGIN_TYPE_CLAIM, loginType.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,  string.IsNullOrEmpty(role) ? string.Empty : role),
                new Claim(CLIENT_PLATFORM_CLAIM, platform.ToString())
        };
        }

        protected Open360IdentityBase(IEnumerable<Claim> claims)
            : base(claims, "API")
        {
            m_SessionID = new Lazy<Guid>(() => Guid.Parse(FindFirst(SESSION_ID_CLAIM).Value));
            m_UserID = new Lazy<long>(() => long.Parse(FindFirst(ClaimTypes.Sid).Value));
            m_LoginType = new Lazy<LoginType>(() => (LoginType)Enum.Parse(typeof(LoginType), FindFirst(LOGIN_TYPE_CLAIM).Value, true));
            m_Role = new Lazy<string>(() => FindFirst(ClaimTypes.Role).Value);
            m_ClientPlatform = new Lazy<ClientPlatform>(() =>
            {
                var claim = FindFirst(CLIENT_PLATFORM_CLAIM);
                if (claim == null)
                {
                    var request = HttpContext.Current?.Request;
                    if (request != null)
                    {
                        var userAgent = request.Headers["User-Agent"];
                        return !string.IsNullOrEmpty(userAgent) ?
                            (userAgent.ToLower().Contains("ios") ? ClientPlatform.iOS :
                                (userAgent.ToLower().Contains("android") ? ClientPlatform.Android : ClientPlatform.Unknown)) : ClientPlatform.iOS;
                    }
                    else
                        return ClientPlatform.Unknown;
                }
                return (ClientPlatform)Enum.Parse(typeof(ClientPlatform), claim.Value, true);
            });
        }

        protected Open360IdentityBase(ClaimsIdentity identity)
            : this(identity.Claims)
        {
        }

        public override string ToString()
        {
            return string.Format("Identity: ID={0}, LoginType={1}, ClientPlatform={2}", UserID, LoginType, ClientPlatform);
        }
    }

    public enum LoginType
    {
        Password = 0,
        AdminPassword,
        Token
    }

    public enum ClientPlatform
    {
        Unknown = 0,
        iOS = 1,
        Android = 2,
        Web = 3
    }
}