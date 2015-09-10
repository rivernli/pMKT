using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.DirectoryServices;
using System.Collections;

    public class LDAP : IDisposable
    {
        private string _path;
        private string _name;
        private string _uid;
        private string _group;
        private string _department;
        //private string _domain;

        public string name { get { return _name; } }
        public string uid { get { return _uid; } }
        public string group { get { return _group; } }
        public string department { get { return _department; } }
        //public string domain { get { return _domain; } }
        public string title;
        public string tel;
        public string email;
        public string fax;
        public string mobile;
        public string ipPhone;

        public string message;

        public LDAP(string path)
        {
            _path = path;
        }
        public void Dispose()
        {
            _path = "";
            _name = "";
            _uid = "";
        }
        public bool isAuth(string domain, string username, string password)
        {
            if (domain.Trim().Length == 0)
                return false;
            if (username.Trim().Length == 0)
                return false;
            if (password.Trim().Length == 0)
                return false;

            if (_path == "")
                _path = @"LDAP://DC=" + domain + ",DC=ad,DC=flextronics,DC=com";
            string domain_user = domain + "\\" + username;
            //DirectoryEntry entry = new DirectoryEntry(_path, domain + @"\" + username, password);
            DirectoryEntry entry = new DirectoryEntry(_path,domain_user , password);
            try
            {
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                return __defineUser(ref search);
            }
            catch (Exception ex)
            {
                message = "Error authenticating user." + ex.Message;
                return false;
            }
            finally
            {
                entry.Dispose();
            }
        }


        public bool findUser(string user_id, string domain)
        {
            DirectoryEntry entry = new DirectoryEntry(@"LDAP://DC=" + domain + ",DC=ad,DC=flextronics,DC=com");
            DirectorySearcher search = new DirectorySearcher(entry, "sAMAccountName=" + user_id);
            return __defineUser(ref search);
        }

        private string __getGroupName(string groupname)
        {
            string pp = "";
            int commaIndex = groupname.IndexOf(",", 1);
            if (commaIndex >= 0)
            {
                groupname = groupname.Substring(0,commaIndex);
                string fileNameRexp = @"^([A-Z]{2,4}[\=])([\\][\#][\w]+ [-] )?(?<name>[ \w\-\&]+)$";
                Regex regx = new Regex(fileNameRexp, RegexOptions.IgnoreCase);
                MatchCollection ms = regx.Matches(groupname);
                if (ms.Count > 0)
                {
                    pp = ms[0].Groups["name"].ToString();
                }
            }
            return pp;
        }
        private bool __defineUser(ref DirectorySearcher search)
        {
            //load properites for user;
            search.PropertiesToLoad.Add("cn");
            search.PropertiesToLoad.Add("telephoneNumber");
            search.PropertiesToLoad.Add("facsimileTelephoneNumber");
            search.PropertiesToLoad.Add("mobile");
            search.PropertiesToLoad.Add("ipphone");
            search.PropertiesToLoad.Add("memberof");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("department");
            search.PropertiesToLoad.Add("title");
            search.PropertiesToLoad.Add("sAMAccountName");
            //search.PropertiesToLoad.Add("dc");
            SearchResult result = search.FindOne();
            if (result == null)
                return false;

            _path = result.Path;
            _uid = result.Properties["sAMAccountName"][0].ToString();
            _name = (result.Properties["cn"].Count > 0) ? (string)result.Properties["cn"][0] : _uid;
            _department = setProperity(ref result,"department");// (result.Properties["department"].Count > 0) ? (string)result.Properties["department"][0] : "";
            tel = setProperity(ref result, "telephoneNumber");
            title = setProperity(ref result, "title");
            email = setProperity(ref result, "mail");
            fax = setProperity(ref result, "facsimileTelephoneNumber");
            mobile = setProperity(ref result, "mobile");
            ipPhone = setProperity(ref result, "ipphone");
            //_domain = setProperity(ref result, "DC");
            __setGroup(ref result);
            return true;
        }
        private void __setGroup(ref SearchResult result)
        {
            _group = "";
            if (result.Properties["memberof"].Count > 0)
            {
                for (int i = 0; i < result.Properties["memberof"].Count; i++)
                {
                    _group += __getGroupName((string)result.Properties["memberof"][i]) + ";";
                }
                _group = _group.Substring(0, _group.LastIndexOf(";"));
            }
        }
        private string setProperity(ref SearchResult rs, string properity)
        {
            return rs.Properties[properity].Count > 0 ? (string)rs.Properties[properity][0] : "";
        }

        
        public static string getUsername(string user_id,string domain)
        {
            
            DirectoryEntry de = new DirectoryEntry(@"LDAP://DC="+ domain +",DC=ad,DC=flextronics,DC=com");
            DirectorySearcher ds = new DirectorySearcher(de, "SAMAccountName=" + user_id);

            SearchResult result = ds.FindOne();
            if (result == null)
                return "";
            else
            {
                string n = result.Path.ToString();// (result.Properties["cn"].Count > 0) ? (string)result.Properties["cn"][0] : user_id;
                return n;
            }
        }
    }
