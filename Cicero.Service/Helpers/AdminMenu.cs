using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cicero.Service.Helpers
{
    public class AdminMenu
    {

        public static List<Menu> getAdminMenus(string tenant_identifier = "")
        {
            string tenant = "";
            string dashboard = "";
            if (!string.IsNullOrEmpty(tenant_identifier))
            {
                tenant = tenant_identifier + "/";
                dashboard = "/" + tenant_identifier;
            }

            List<Menu> MenuList = new List<Menu>();
            MenuList.Add(new Menu(
                "home",
                "Dashboard",
                "~/admin" + dashboard + ".html",
                "0",
                // "fas fa-tachometer-alt fa-fw mr-3",
                "ri-dashboard-line", // ri-dashboard-line
                0
            ));

            MenuList.Add(new Menu(
                "tenant",
                "Tenants",
                "~/admin/" + tenant + "tenants.html",
                "0",
                // "fas fa-building",
                "ri-community-line",
                UserCan.View_Tenant
            ));

            MenuList.Add(new Menu(
                "user",
                "Users",
                "~/admin/" + tenant + "users.html",
                "0",
                // "fas fa-users",
                "ri-user-3-line",
                UserCan.View_User
            ));

            MenuList.Add(new Menu(
                "role",
                "Roles",
                "~/admin/" + tenant + "roles.html",
                "0",
                // "fa fa-key",
                "ri-shield-user-line",
                UserCan.View_Role
            ));


            //needed later muted for demo
            MenuList.Add(new Menu(
                "article",
                "Articles",
                "~/admin/" + tenant + "articles.html",
                "0",
                // "fas fa-pen-nib",
                "ri-article-line",
                UserCan.View_Article
            ));


            //MenuList.Add(new Menu(
            //    "queue",
            //    "Work Flow",
            //    "~/admin/" + tenant + "manage.html",
            //    "0",
            //    "fas fa-tasks",
            //    UserCan.View_Queue
            //));

            //MenuList.Add(new Menu(
            //    "queueindex",
            //    "Queues",
            //    "~/admin/" + tenant + "manage/queues.html",
            //    "queue",
            //    "fas fa-bezier-curve",
            //    UserCan.View_Queue
            //));

            //MenuList.Add(new Menu(
            //    "stateindex",
            //    "States",
            //    "~/admin/" + tenant + "manage/states.html",
            //    "queue",
            //    "fas fa-bezier-curve",
            //    UserCan.View_Queue
            //));

            MenuList.Add(new Menu(
                "stateworkflow",
                "Workflow Builder",
                "~/admin/" + tenant + "manage/workflow.html",
                "0",
                // "fas fa-bezier-curve",
                "ri-shape-line",
                UserCan.View_Queue
            ));

            MenuList.Add(new Menu(
                "tools",
                "Tools",
                "javascript:void(0);",
                "0",
                // "fa fa-cogs",
                "ri-list-settings-line",
                UserCan.View_Tools
            ));
            MenuList.Add(new Menu(
                "formbuilder",
                "Form Builder",
                "~/admin/" + tenant + "builderforms.html",
                "tools",
                "fab fa-simplybuilt",
                UserCan.View_Form
            ));
            MenuList.Add(new Menu(
                "component",
                "Components",
                "~/admin/" + tenant + "components.html",
                "tools",
                "fab fa-simplybuilt",
                UserCan.View_Form
            ));
            MenuList.Add(new Menu(
               "template",
               "Email Templates",
               "~/admin/" + tenant + "templates.html",
               "tools",
               "fas fa-envelope-square",
               UserCan.View_Article
           ));
            // needed later muted for demo
            // MenuList.Add(new Menu(
            //     "formbuilder",
            //     "Case Synchronization",
            //     "~/admin/" + tenant + "case-sync.html",
            //     "tools",
            //     "fab fa-simplybuilt",
            //     UserCan.View_Form
            // ));

            MenuList.Add(new Menu(
                "appearance",
                "Appearance",
                "javascript:void(0)",
                "0",
                // "fas fa-feather",
                "ri-quill-pen-line",
                UserCan.View_Menu
            ));
            MenuList.Add(new Menu(
                "menu",
                "Theme",
                "~/admin/" + tenant + "themes.html",
                "appearance",
                "fas fa-bezier-curve",
                UserCan.View_Menu
            ));

            //needed later muted for demo
            //MenuList.Add(new Menu(
            //    "widget",
            //    "Widgets",
            //    "~/admin/" + tenant + "widgets.html",
            //    "appearance",
            //    "fas fa-bezier-curve",
            //    UserCan.View_Menu
            //));

            //MenuList.Add(new Menu(
            //    "menu",
            //    "Menus",
            //    "~/admin/" + tenant + "menus.html",
            //    "appearance",
            //    "fas fa-bezier-curve",
            //    UserCan.View_Menu
            //));
            MenuList.Add(new Menu(
                "media",
                "Media",
                "~/admin/" + tenant + "medias.html",
                "appearance",
                "fas fa-images",
                UserCan.View_Media
            ));
            //MenuList.Add(new Menu(
            //    "setting",
            //    "Settings",
            //    "~/admin/" + tenant + "setting.html",
            //    "appearance",
            //    "fa fa-cogs",
            //    UserCan.View_Setting
            //));
            MenuList.Add(new Menu(
                "guide",
                "User Guide",
                "~/admin/userguide.html",
                "0",
                // "far fa-file-pdf",
                "ri-book-2-line",
                UserCan.View_Article
            ));
            //    MenuList.Add(new Menu(
            //    "beneficiary",
            //    "Beneficiary Setup",
            //    "~/admin/beneficiarysetup.html",
            //    "0",
            //    // "fas fa-feather",
            //    "ri-user-3-line",
            //    UserCan.View_Menu
            //));
            MenuList.Add(new Menu(
             "Configuration",
             "Configuration",
             "javascript:void(0)",
             "0",
             // "fas fa-feather",
             "list-settings",
             UserCan.View_Menu
         ));
            MenuList.Add(new Menu(
               "country",
               "Country",
               "~/admin/country.html",
               "Configuration",
               "book-2",
               UserCan.View_CountryConfig
           ));
            MenuList.Add(new Menu(
             "country payment method",
             "Country Payment Method",
             "~/admin/countryPayout.html",
             "Configuration",
             "book-2",
             UserCan.View_CountryPayoutMode
         ));

            //      MenuList.Add(new Menu(
            //       "bankmapper",
            //       "Bank Mapper",
            //       "~/admin/bankmapper.html",
            //       "Configuration",
            //       "book-2",
            //       UserCan.View_BankMapper
            //   ));

            //      MenuList.Add(new Menu(
            //    "branchmapper",
            //    "Branch Mapper",
            //    "~/admin/branchmapper.html",
            //    "Configuration",
            //    "book-2",
            //    UserCan.View_BranchMapper
            //));

            MenuList.Add(new Menu(
            "fileuploader",
            "File Uploader",
            "~/admin/fileuploader.html",
            "Configuration",
            "book-2",
            UserCan.View_CorrespondentBank
        ));

            MenuList.Add(new Menu(
        "city",
        "City Configuration",
        "~/admin/city.html",
        "Configuration",
        "book-2",
        UserCan.View_CityConfig
    ));

            MenuList.Add(new Menu(
                "ratesupplier",
                "Rate Supplier",
               "~/admin/ratesupplier.html",
                "Configuration",
                "book-2",
                UserCan.View_RateSupplier
            ));

            MenuList.Add(new Menu(
               "paymentmethod",
               "Payment Method",
              "~/admin/payoutmode.html",
               "Configuration",
               "book-2",
               UserCan.View_Menu
           ));

            MenuList.Add(new Menu(
             "autoschedulersetting",
             "Auto Scheduler Setting",
             "~/admin/autoschedulersetting.html",
             "Configuration",
             "book-2",
             UserCan.View_Menu
         ));
            MenuList.Add(new Menu(
             "exchangeratesetting",
             "Exchange Rates",
            "~/admin/exchangeratesetting.html",
             "Configuration",
             "book-2",
             UserCan.View_ExchangeRates
         )); MenuList.Add(new Menu(
             "banksetting",
             "Correspondent Bank",
            "~/admin/banksetting.html",
             "Configuration",
             "book-2",
             UserCan.View_CorrespondentBank
         ));
            MenuList.Add(new Menu(
            "relationshipsetup",
            "Beneficiary Relationship",
           "~/admin/relationshipsetup.html",
            "Configuration",
            "book-2",
            UserCan.View_RelationshipConfig
        ));
            //    MenuList.Add(new Menu(
            //    "maritalstatussetup",
            //    "Marital Status",
            //   "~/admin/maritalstatussetup.html",
            //    "Configuration",
            //    "book-2",
            //    UserCan.View_Menu
            //));
            MenuList.Add(new Menu(
            "gendersetup",
            "Gender",
           "~/admin/gendersetup.html",
            "Configuration",
            "book-2",
            UserCan.View_GenderConfig
        ));
            MenuList.Add(new Menu(
            "itentificationtypesetup",
            "Identification Type",
           "~/admin/identificationtypesetup.html",
            "Configuration",
            "book-2",
            UserCan.View_IdTypeConfig
        ));
            MenuList.Add(new Menu(
            "paymentpurposesetup",
            "Payment Purpose",
           "~/admin/paymentpurposesetup.html",
            "Configuration",
            "book-2",
            UserCan.View_PaymentPurposeConfig
        ));
            MenuList.Add(new Menu(
           "RateSupplierFeeConfig",
           "Rate Supplier Fee Config",
          "~/admin/RateSupplierFeeConfig.html",
           "Configuration",
           "book-2",
           UserCan.View_RateSupplierFeeConfig
       ));

            MenuList.Add(new Menu(
         "TransactionLimitConfig",
         "Transaction Limit Config",
        "~/admin/transactionlimitconfig.html",
         "Configuration",
         "book-2",
         UserCan.View_TxnLimitConfig
     ));
            return MenuList;
        }
        public static List<Menu> getSTAdminMenus(string tenant_identifier = "")
        {
            string tenant = "";
            string dashboard = "";
            if (!string.IsNullOrEmpty(tenant_identifier))
            {
                tenant = tenant_identifier + "/";
                dashboard = "/" + tenant_identifier;
            }

            List<Menu> MenuList = new List<Menu>();

            MenuList.Add(new Menu(
               "transactionmgmtroot",
               "Transaction Management",
               "~/admin/transactionmgmt.html",
               "0",
               "ri-settings-5-line",
               UserCan.View_Menu
           ));
            MenuList.Add(new Menu(
                "businesssetuproot",
                "Business Setup",
                "",
                "0",
                "ri-settings-5-line",
                UserCan.View_Menu
            ));
            MenuList.Add(new Menu(
                "userroot",
                "User",
                "",
                "0",
                "ri-user-3-line",
                UserCan.View_Menu
            ));
            MenuList.Add(new Menu(
               "newsroot",
               "News",
               "",
               "0",
               "ri-newspaper-line",
               UserCan.View_Menu
           ));
            // MenuList.Add(new Menu(
            //    "customerroot",
            //    "Customer",
            //    "",
            //    "0",
            //    "ri-group-2-line",
            //    UserCan.View_Menu
            //));
            MenuList.Add(new Menu(
               "moneytransmitterroot",
               "Money Transmitter",
               "",
               "0",
               "ri-bank-line",
               UserCan.View_Menu
           ));
            MenuList.Add(new Menu(
               "complianceroot",
               "Compliance",
               "",
               "0",
               "ri-list-check",
               UserCan.View_Menu
           ));
            MenuList.Add(new Menu(
               "correspondentsroot",
               "Correspondents/Supplier",
               "",
               "0",
               "ri-group-line",
               UserCan.View_Menu
           ));
            MenuList.Add(new Menu(
               "currencyroot",
               "Currency",
               "",
               "0",
               "ri-money-pound-circle-line",
               UserCan.View_Menu
           ));
            MenuList.Add(new Menu(
               "feesroot",
               "Fees",
               "",
               "0",
               "ri-ticket-line",
               UserCan.View_Menu
           ));
            MenuList.Add(new Menu(
               "exchangeratemgmtroot",
               "Exchange Rate Management",
               "",
               "0",
               "ri-exchange-line",
               UserCan.View_Menu
           ));
            MenuList.Add(new Menu(
               "reportsroot",
               "Reports",
               "",
               "0",
               "ri-file-chart-line",
               UserCan.View_Menu
           ));
            MenuList.Add(new Menu(
               "findtransactionroot",
               "Find Transaction",
               "",
               "0",
               "ri-refresh-line",
               UserCan.View_Menu
           ));
            MenuList.Add(new Menu(
               "administratorroot",
               "Administrator",
               "",
               "0",
               "ri-lock-password-line",
               UserCan.View_Menu
           ));
            MenuList.Add(new Menu(
                "tenant",
                "Tenants",
                "~/admin/" + tenant + "tenants.html",
                "businesssetuproot",
                "ri-community-line",
                UserCan.View_Tenant
            ));

            MenuList.Add(new Menu(
               "customerUser",
               "Customers",
               "~/st/admin/" + tenant + "customers.html",
               "userroot",
               // "fas fa-users",
               "ri-user-3-line",
               UserCan.View_User
           ));

            MenuList.Add(new Menu(
                "user",
                "Users",
                "~/admin/" + tenant + "users.html",
                "userroot",
                "ri-user-3-line",
                UserCan.View_User
            ));

            MenuList.Add(new Menu(
                "role",
                "Roles",
                "~/admin/" + tenant + "roles.html",
                "userroot",
                "ri-shield-user-line",
                UserCan.View_Role
            ));
            //needed later muted for demo
            MenuList.Add(new Menu(
                "article",
                "Articles",
                "~/admin/" + tenant + "articles.html",
                "administratorroot",
                "ri-article-line",
                UserCan.View_Article
            ));
            MenuList.Add(new Menu(
                "stateworkflow",
                "Workflow Builder",
                "~/admin/" + tenant + "manage/workflow.html",
                "administratorroot",
                "ri-shape-line",
                UserCan.View_Queue
            ));

            MenuList.Add(new Menu(
                "tools",
                "Tools",
                "javascript:void(0);",
                "0",
                "ri-list-settings-line",
                UserCan.View_Tools
            ));
            MenuList.Add(new Menu(
                "formbuilder",
                "Form Builder",
                "~/admin/" + tenant + "builderforms.html",
                "tools",
                "fab fa-simplybuilt",
                UserCan.View_Form
            ));
            MenuList.Add(new Menu(
                "component",
                "Components",
                "~/admin/" + tenant + "components.html",
                "tools",
                "fab fa-simplybuilt",
                UserCan.View_Form
            ));
            MenuList.Add(new Menu(
               "template",
               "Email Templates",
               "~/admin/" + tenant + "templates.html",
               "tools",
               "fas fa-envelope-square",
               UserCan.View_Article
           ));
            MenuList.Add(new Menu(
                "appearance",
                "Appearance",
                "javascript:void(0)",
                "0",
                "ri-quill-pen-line",
                UserCan.View_Menu
            ));
            MenuList.Add(new Menu(
                "menu",
                "Theme",
                "~/admin/" + tenant + "themes.html",
                "appearance",
                "fas fa-bezier-curve",
                UserCan.View_Menu
            ));
            MenuList.Add(new Menu(
                "media",
                "Media",
                "~/admin/" + tenant + "medias.html",
                "appearance",
                "fas fa-images",
                UserCan.View_Media
            ));

            MenuList.Add(new Menu(
                "guide",
                "User Guide",
                "~/admin/userguide.html",
                "0",
                "ri-book-2-line",
                UserCan.View_Article
            ));

            MenuList.Add(new Menu(
               "country",
               "Country",
               "~/admin/country.html",
               "businesssetuproot",
               "book-2",
               UserCan.View_CountryConfig
           ));
            MenuList.Add(new Menu(
             "country payment method",
             "Country Payment Method",
             "~/admin/countryPayout.html",
             "moneytransmitterroot",
             "book-2",
             UserCan.View_CountryPayoutMode
         ));

            //      MenuList.Add(new Menu(
            //       "bankmapper",
            //       "Bank Mapper",
            //       "~/admin/bankmapper.html",
            //       "correspondentsroot",
            //       "book-2",
            //       UserCan.View_BankMapper
            //   ));

            MenuList.Add(new Menu(
          "branch",
          "Branch",
          "~/admin/branch.html",
          "correspondentsroot",
          "book-2",
          UserCan.View_BranchMapper
      ));

            MenuList.Add(new Menu(
            "fileuploader",
            "File Uploader",
            "~/admin/fileuploader.html",
            "correspondentsroot",
            "book-2",
            UserCan.View_CorrespondentBank
        ));

            MenuList.Add(new Menu(
        "city",
        "City",
        "~/admin/city.html",
        "businesssetuproot",
        "book-2",
        UserCan.View_CityConfig
    ));

            MenuList.Add(new Menu(
                "ratesupplier",
                "Rate Supplier",
               "~/admin/ratesupplier.html",
                "businesssetuproot",
                "book-2",
                UserCan.View_RateSupplier
            ));

            MenuList.Add(new Menu(
               "paymentmethod",
               "Payment Method",
              "~/admin/payoutmode.html",
               "businesssetuproot",
               "book-2",
               UserCan.View_Menu
           ));

            MenuList.Add(new Menu(
             "autoschedulersetting",
             "Auto Scheduler Setting",
             "~/admin/autoschedulersetting.html",
             "Configuration",
             "book-2",
             UserCan.View_Menu
         ));
            MenuList.Add(new Menu(
             "exchangeratesetting",
             "Exchange Rates",
            "~/admin/exchangeratesetting.html",
             "feesroot",
             "book-2",
             UserCan.View_ExchangeRates
         )); MenuList.Add(new Menu(
             "banksetting",
             "Correspondent Bank",
            "~/admin/banksetting.html",
             "correspondentsroot",
             "book-2",
             UserCan.View_CorrespondentBank
         ));
            MenuList.Add(new Menu(
            "relationshipsetup",
            "Beneficiary Relationship",
           "~/admin/relationshipsetup.html",
            "complianceroot",
            "book-2",
            UserCan.View_RelationshipConfig
        ));
            //    MenuList.Add(new Menu(
            //    "maritalstatussetup",
            //    "Marital Status",
            //   "~/admin/maritalstatussetup.html",
            //    "complianceroot",
            //    "book-2",
            //    UserCan.View_Menu
            //));
            MenuList.Add(new Menu(
            "gendersetup",
            "Gender",
           "~/admin/gendersetup.html",
            "complianceroot",
            "book-2",
            UserCan.View_GenderConfig
        ));
            MenuList.Add(new Menu(
            "itentificationtypesetup",
            "Identification Type",
           "~/admin/identificationtypesetup.html",
            "complianceroot",
            "book-2",
            UserCan.View_IdTypeConfig
        ));
            MenuList.Add(new Menu(
            "paymentpurposesetup",
            "Payment Purpose",
           "~/admin/paymentpurposesetup.html",
            "complianceroot",
            "book-2",
            UserCan.View_PaymentPurposeConfig
        ));
            MenuList.Add(new Menu(
          "sourceoffundsetup",
          "Source of Fund",
         "~/admin/sourceoffundsetup.html",
          "complianceroot",
          "book-2",
          UserCan.View_Menu
      ));
            MenuList.Add(new Menu(
           "RateSupplierFeeConfig",
           "Rate Supplier Fee Config",
          "~/admin/RateSupplierFeeConfig.html",
           "feesroot",
           "book-2",
           UserCan.View_RateSupplierFeeConfig
       ));

            MenuList.Add(new Menu(
         "TransactionLimitConfig",
         "Transaction Limit Config",
        "~/admin/transactionlimitconfig.html",
         "complianceroot",
         "book-2",
         UserCan.View_TxnLimitConfig
     ));
            return MenuList;
        }

        public static ArrayList getMenuLocations()
        {
            ArrayList MenuLocation = new ArrayList();
            MenuLocation.Add("Primary");
            MenuLocation.Add("Bottom");
            return MenuLocation;
        }
        public static string getRandNumber()
        {

            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return String.Format("{0:D8}", random);
        }
    }

    public class Menu
    {
        private string id;
        private string name;
        private string url;
        private string icon;
        private string parent;
        private int permissionId;

        public Menu(string id, string name = "", string url = "", string parent = "0", string icon = "", int _permissionId = 0)
        {
            this.id = id;
            this.name = name;
            this.url = url;
            this.parent = parent;
            this.icon = icon;
            this.permissionId = _permissionId;

        }
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        public string Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }
        public int Permission
        {
            get { return permissionId; }
            set { permissionId = value; }
        }

    }
}