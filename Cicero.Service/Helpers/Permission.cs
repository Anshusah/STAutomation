using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cicero.Service.Helpers
{

    public class Permission
    {
        private readonly IUserService UserService;
        private List<PermissionViewModel> AllowedPermissions;
        private readonly ApplicationDbContext db;
        private readonly IRolePermissionService RolePermissionService;

        public Permission(IUserService _UserService, ApplicationDbContext _db, IRolePermissionService _RolePermissionService)
        {
            UserService = _UserService;
            db = _db;
            RolePermissionService = _RolePermissionService;

        }
        public bool Can(int _permission_, int? page = 0)
        {
            if (UserService.IsSuperAdmin().Result == true)
            {
                return true;
            }
            if (AllowedPermissions == null || AllowedPermissions.Count() == 0)
            {
                string userId = (string)UserService.getLoggedInUserId();
                var role = db.UserRoles.Where(x => x.UserId == userId).FirstOrDefault();
                if (role != null)
                {

                    AllowedPermissions = RolePermissionService.GetPermissionListByRoleId(role.RoleId);

                }
                else
                {
                    List<PermissionViewModel> newlist = new List<PermissionViewModel>();
                    newlist.Add(new PermissionViewModel { Id = 38, Name = "Create Form", GroupId = 10 });
                    AllowedPermissions = newlist;
                }

            }
            if (page != 0)
            {
                int GroupId = RolePermissionService.GetPermissionGroupByFormId((int)page);
                if (AllowedPermissions.Any(c => c.Id == _permission_ && c.GroupId == GroupId))
                {
                    return true;
                }
            }
            else
            {
                if (AllowedPermissions.Any(c => c.Id == _permission_))
                {
                    return true;
                }

            }
            return false;
        }

    }
    public static class UserCan
    {

        #region Claim
        public static int View_Claim { get { return 1; } }
        public static int Create_Claim { get { return 2; } }
        public static int Update_Claim { get { return 3; } }
        public static int Delete_Claim { get { return 4; } }
        #endregion Claim
        #region User
        public static int View_User { get { return 5; } }
        public static int Create_User { get { return 6; } }
        public static int Update_User { get { return 7; } }
        public static int Delete_User { get { return 8; } }
        public static int Edit_Profile { get { return 9; } }
        #endregion User
        #region Article
        public static int View_Article { get { return 10; } }
        public static int Create_Article { get { return 11; } }
        public static int Update_Article { get { return 12; } }
        public static int Delete_Article { get { return 13; } }
        #endregion Article
        #region Media
        public static int View_Media { get { return 14; } }
        public static int Create_Media { get { return 15; } }
        public static int Update_Media { get { return 16; } }
        public static int Delete_Media { get { return 17; } }
        #endregion Media
        #region Menu
        public static int View_Menu { get { return 18; } }
        public static int Create_Menu { get { return 19; } }
        public static int Update_Menu { get { return 20; } }
        public static int Delete_Menu { get { return 21; } }
        #endregion Menu
        #region Role
        public static int View_Role { get { return 22; } }
        public static int Create_Role { get { return 23; } }
        public static int Update_Role { get { return 24; } }
        public static int Delete_Role { get { return 25; } }
        #endregion Role
        #region Setting
        public static int View_Setting { get { return 26; } }
        public static int Create_Setting { get { return 27; } }
        public static int Update_Setting { get { return 28; } }
        public static int Delete_Setting { get { return 29; } }
        #endregion Setting
        #region Message
        public static int View_Message { get { return 30; } }
        public static int Create_Message { get { return 31; } }
        public static int Reply_Message { get { return 32; } }
        public static int Reply_All_Message { get { return 33; } }
        #endregion
        #region Queue
        public static int View_Queue { get { return 34; } }
        public static int Create_Queue { get { return 35; } }
        public static int Update_Queue { get { return 36; } }
        public static int Delete_Queue { get { return 37; } }
        #endregion
        #region Form
        public static int View_Form { get { return 38; } }
        public static int Create_Form { get { return 39; } }
        public static int Update_Form { get { return 40; } }
        public static int Delete_Form { get { return 41; } }
        #endregion
        #region Tenant
        public static int View_Tenant { get { return 42; } }
        public static int Create_Tenant { get { return 100000002; } }
        public static int Update_Tenant { get { return 100000003; } }
        public static int Delete_Tenant { get { return 100000004; } }
        #endregion
        #region Dashboard Layout
        public static int Admin_Layout { get { return 43; } }
        public static int Worker_Layout { get { return 44; } }
        #endregion Dashboard Layout
        #region Tools
        public static int View_Tools { get { return 49; } }
        public static int Edit_Tools { get { return 50; } }
        #endregion
        #region New
        public static int View { get { return 45; } }
        public static int Create { get { return 46; } }
        public static int Update { get { return 47; } }
        public static int Delete { get { return 48; } }
        #endregion

        #region CountryConfig
        public static int View_CountryConfig { get { return 51; } }
        public static int Create_CountryConfig { get { return 52; } }
        public static int Update_CountryConfig { get { return 53; } }
        public static int Delete_CountryConfig { get { return 54; } }
        #endregion CountryConfig
        #region CountryPayoutMode
        public static int View_CountryPayoutMode { get { return 55; } }
        public static int Create_CountryPayoutMode { get { return 56; } }
        public static int Update_CountryPayoutMode { get { return 57; } }
        public static int Delete_CountryPayoutMode { get { return 58; } }
        #endregion CountryPayoutMode
        #region BankMapper
        public static int View_BankMapper { get { return 60; } }
        public static int Create_BankMapper { get { return 61; } }
        public static int Update_BankMapper { get { return 62; } }
        public static int Delete_BankMapper { get { return 63; } }
        #endregion BankMapper
        #region BranchMapper
        public static int View_BranchMapper { get { return 64; } }
        public static int Create_BranchMapper { get { return 65; } }
        public static int Update_BranchMapper { get { return 66; } }
        public static int Delete_BranchMapper { get { return 67; } }
        #endregion BranchMapper
        #region CityConfig
        public static int View_CityConfig { get { return 68; } }
        public static int Create_CityConfig { get { return 69; } }
        public static int Update_CityConfig { get { return 70; } }
        public static int Delete_CityConfig { get { return 71; } }
        #endregion CityConfig
        #region RateSupplier
        public static int View_RateSupplier { get { return 72; } }
        public static int Create_RateSupplier { get { return 73; } }
        public static int Update_RateSupplier { get { return 74; } }
        public static int Delete_RateSupplier { get { return 75; } }
        #endregion RateSupplier
        #region ExchangeRates
        public static int View_ExchangeRates { get { return 76; } }
        public static int Create_ExchangeRates { get { return 77; } }
        public static int Update_ExchangeRates { get { return 78; } }
        public static int Delete_ExchangeRates { get { return 79; } }
        #endregion ExchangeRates
        #region CorrespondentBank
        public static int View_CorrespondentBank { get { return 80; } }
        public static int Create_CorrespondentBank { get { return 81; } }
        public static int Update_CorrespondentBank { get { return 82; } }
        public static int Delete_CorrespondentBank { get { return 83; } }
        #endregion CorrespondentBank
        #region RelationshipConfig
        public static int View_RelationshipConfig { get { return 84; } }
        public static int Create_RelationshipConfig { get { return 85; } }
        public static int Update_RelationshipConfig { get { return 86; } }
        public static int Delete_RelationshipConfig { get { return 87; } }
        #endregion RelationshipConfig
        #region MaritalStatusConfig
        public static int View_MaritalStatusConfig { get { return 88; } }
        public static int Create_MaritalStatusConfig { get { return 89; } }
        public static int Update_MaritalStatusConfig { get { return 90; } }
        public static int Delete_MaritalStatusConfig { get { return 91; } }
        #endregion MaritalStatusConfig
        #region GenderConfig
        public static int View_GenderConfig { get { return 92; } }
        public static int Create_GenderConfig { get { return 93; } }
        public static int Update_GenderConfig { get { return 94; } }
        public static int Delete_GenderConfig { get { return 95; } }
        #endregion GenderConfig
        #region IdTypeConfig
        public static int View_IdTypeConfig { get { return 96; } }
        public static int Create_IdTypeConfig { get { return 97; } }
        public static int Update_IdTypeConfig { get { return 98; } }
        public static int Delete_IdTypeConfig { get { return 99; } }
        #endregion IdTypeConfig
        #region PaymentPurposeConfig
        public static int View_PaymentPurposeConfig { get { return 100; } }
        public static int Update_PaymentPurposeConfig { get { return 101; } }
        public static int Delete_PaymentPurposeConfig { get { return 102; } }
        public static int Create_PaymentPurposeConfig { get { return 103; } }
        #endregion PaymentPurposeConfig
        #region RateSupplierFeeConfig
        public static int View_RateSupplierFeeConfig { get { return 104; } }
        public static int Create_RateSupplierFeeConfig { get { return 105; } }
        public static int Update_RateSupplierFeeConfig { get { return 106; } }
        public static int Delete_RateSupplierFeeConfig { get { return 107; } }
        #endregion RateSupplierFeeConfig
        #region TxnLimitConfig
        public static int View_TxnLimitConfig { get { return 108; } }
        public static int Create_TxnLimitConfig { get { return 109; } }
        public static int Update_TxnLimitConfig { get { return 110; } }
        public static int Delete_TxnLimitConfig { get { return 111; } }
        #endregion TxnLimitConfig
    }

}
