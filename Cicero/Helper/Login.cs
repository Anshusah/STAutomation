using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Helper
{
    [DataContract]
    public class LoginRequest
    {        
        [DataMember]
        public string SystemId { get; set; }
        [DataMember]
        public string Username { get; set; }     

        [DataMember]
        public string Password { get; set; }
    }

    public class LoginResponse : ValidationFailureError
    {
        private string _accessToken;
        private string _username;
        private string _expiryDate;
        private string _issueDate;
        #region Properties
        [DataMember]
        public string AccessToken
        {
            get { return _accessToken ?? string.Empty; }
            set { _accessToken = value; }
        }
        [DataMember]
        public string UserName
        {
            get { return _username ?? string.Empty; }
            set { _username = value; }
        }
        [DataMember]
        public string ExpiryDate
        {
            get { return _expiryDate ?? string.Empty; }
            set { _expiryDate = value; }
        }
        [DataMember]
        public string IssueDate
        {
            get { return _issueDate ?? string.Empty; }
            set { _issueDate = value; }
        }
        #endregion
    }

    public class RefreshTokenResponse : ValidationFailureError
    {
        #region Variables

        private string _accessToken;
        private string _username;
        private string _expiryDate;
        private string _issueDate;
        #endregion

        #region Properties
        public string AccessToken
        {
            get { return _accessToken ?? string.Empty; }
            set { _accessToken = value; }
        }
        public string UserName
        {
            get { return _username ?? string.Empty; }
            set { _username = value; }
        }
        public string ExpiryDate
        {
            get { return _expiryDate ?? string.Empty; }
            set { _expiryDate = value; }
        }
        public string IssueDate
        {
            get { return _issueDate ?? string.Empty; }
            set { _issueDate = value; }
        }
        #endregion

    }
}
