using Cicero.Data.Entities;
using Cicero.Data.Entities.SimpleTransfer;
using Microsoft.EntityFrameworkCore;
using System;
using static Cicero.Data.Enumerations;
using CountryList = Cicero.Data.Entities.SimpleTransfer.CountryList;

namespace Cicero.Data.Extensions
{
    public static class SimpleTransferSeedExtensions
    {
        public static void SeedSimpleTransfer(this ModelBuilder builder)
        {
            builder.Entity<RateSupplier>().HasData(
                new RateSupplier { Id = 1, Name = "Tranfast", SystemId = "1AB804F0-9252-4A7E-885A-276B65540D84", Username = "transfast", Password = "P@ssw0rd", IsActive = true, CreatedAt = new DateTime(2020, 1, 1), RatePriority = 1 }
                //new RateSupplier { Id = 2, Name = "NecMoney", SystemId = "9D6D65E4-EFF1-4752-892D-3F637A56C159", Username = "necmoney", Password = "P@ssw0rd", IsActive = true, CreatedAt = new DateTime(2020, 1, 1), RatePriority = 2 },
                //new RateSupplier { Id = 3, Name = "Safkhan", SystemId = "D93C65D4-7AA9-4A84-B23F-BEBB8D324476", Username = "safkhan", Password = "P@ssw0rd", IsActive = true, CreatedAt = new DateTime(2020, 1, 1), RatePriority = 3 }
                );
            builder.Entity<CountryList>().HasData(
                new CountryList { Id = 1, Code = "AF", Name = "Afghanistan",CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="",CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 2, Code = "AL", Name = "Albania", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 3, Code = "DZ", Name = "Algeria", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 4, Code = "AS", Name = "American Samoa", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 5, Code = "AD", Name = "Andorra", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 6, Code = "AO", Name = "Angola", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 7, Code = "AI", Name = "Anguilla", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 8, Code = "AQ", Name = "Antarctica", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 9, Code = "AG", Name = "Antigua and Barbuda", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 10, Code = "AR", Name = "Argentina", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 11, Code = "AM", Name = "Armenia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 12, Code = "AW", Name = "Aruba", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 13, Code = "AU", Name = "Australia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 14, Code = "AT", Name = "Austria", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 15, Code = "AZ", Name = "Azerbaijan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 16, Code = "BS", Name = "Bahamas", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 17, Code = "BH", Name = "Bahrain", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 18, Code = "BD", Name = "Bangladesh", CurrencyCode="BDT",FlagCode="",DisplayOrder=0,CurrencyName="",CountryPhoneCode="+880",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 19, Code = "BB", Name = "Barbados", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 20, Code = "BY", Name = "Belarus", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 21, Code = "BE", Name = "Belgium", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 22, Code = "BZ", Name = "Belize", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 23, Code = "BJ", Name = "Benin", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 24, Code = "BM", Name = "Bermuda", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 25, Code = "BT", Name = "Bhutan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 26, Code = "BO", Name = "Bolivia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 27, Code = "BA", Name = "Bosnia and Herzegovina", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 28, Code = "BW", Name = "Botswana", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 29, Code = "BV", Name = "Bouvet Island", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 30, Code = "BR", Name = "Brazil", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 31, Code = "IO", Name = "British Indian Ocean Territory", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 32, Code = "BN", Name = "Brunei Darussalam", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 33, Code = "BG", Name = "Bulgaria", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 34, Code = "BF", Name = "Burkina Faso", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 35, Code = "BI", Name = "Burundi", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 36, Code = "KH", Name = "Cambodia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 37, Code = "CM", Name = "Cameroon", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 38, Code = "CA", Name = "Canada", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 39, Code = "CV", Name = "Cape Verde", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 40, Code = "KY", Name = "Cayman Islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 41, Code = "CF", Name = "Central African Republic", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 42, Code = "TD", Name = "Chad", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 43, Code = "CL", Name = "Chile", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 44, Code = "CN", Name = "China", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 45, Code = "CX", Name = "Christmas Island", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 46, Code = "CC", Name = "Cocos (Keeling) Islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 47, Code = "CO", Name = "Colombia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 48, Code = "KM", Name = "Comoros", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 49, Code = "CG", Name = "Congo", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 50, Code = "CK", Name = "Cook Islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 51, Code = "CR", Name = "Costa Rica", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 52, Code = "HR", Name = "Croatia (Hrvatska)", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 53, Code = "CU", Name = "Cuba", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 54, Code = "CY", Name = "Cyprus", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 55, Code = "CZ", Name = "Czech Republic", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 56, Code = "DK", Name = "Denmark", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 57, Code = "DJ", Name = "Djibouti", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 58, Code = "DM", Name = "Dominica", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 59, Code = "DO", Name = "Dominican Republic", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 60, Code = "TP", Name = "East Timor", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 61, Code = "EC", Name = "Ecuador", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 62, Code = "EG", Name = "Egypt", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 63, Code = "SV", Name = "El Salvador", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 64, Code = "GQ", Name = "Equatorial Guinea", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 65, Code = "ER", Name = "Eritrea", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 66, Code = "EE", Name = "Estonia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 67, Code = "ET", Name = "Ethiopia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 68, Code = "FK", Name = "Falkland Islands (Malvinas)", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 69, Code = "FO", Name = "Faroe Islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 70, Code = "FJ", Name = "Fiji", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 71, Code = "FI", Name = "Finland", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 72, Code = "FR", Name = "France", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 73, Code = "FX", Name = "France, Metropolitan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 74, Code = "GF", Name = "French Guiana", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 75, Code = "PF", Name = "French Polynesia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 76, Code = "TF", Name = "French Southern Territories", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 77, Code = "GA", Name = "Gabon", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 78, Code = "GM", Name = "Gambia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 79, Code = "GE", Name = "Georgia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 80, Code = "DE", Name = "Germany", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 81, Code = "GH", Name = "Ghana", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 82, Code = "GI", Name = "Gibraltar", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 83, Code = "GK", Name = "Guernsey", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 84, Code = "GR", Name = "Greece", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 85, Code = "GL", Name = "Greenland", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 86, Code = "GD", Name = "Grenada", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 87, Code = "GP", Name = "Guadeloupe", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 88, Code = "GU", Name = "Guam", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 89, Code = "GT", Name = "Guatemala", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 90, Code = "GN", Name = "Guinea", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 91, Code = "GW", Name = "Guinea-Bissau", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 92, Code = "GY", Name = "Guyana", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 93, Code = "HT", Name = "Haiti", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 94, Code = "HM", Name = "Heard and Mc Donald Islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 95, Code = "HN", Name = "Honduras", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 96, Code = "HK", Name = "Hong Kong", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 97, Code = "HU", Name = "Hungary", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 98, Code = "IS", Name = "Iceland", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 99, Code = "IN", Name = "India", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 100, Code = "IM", Name = "Isle of Man", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 101, Code = "ID", Name = "Indonesia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 102, Code = "IR", Name = "Iran (Islamic Republic of)", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 103, Code = "IQ", Name = "Iraq", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 104, Code = "IE", Name = "Ireland", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 105, Code = "IL", Name = "Israel", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 106, Code = "IT", Name = "Italy", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 107, Code = "CI", Name = "Ivory Coast", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 108, Code = "JE", Name = "Jersey", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 109, Code = "JM", Name = "Jamaica", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 110, Code = "JP", Name = "Japan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 111, Code = "JO", Name = "Jordan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 112, Code = "KZ", Name = "Kazakhstan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 113, Code = "KE", Name = "Kenya", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 114, Code = "KI", Name = "Kiribati", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 115, Code = "KP", Name = "Korea, Democratic Peopl's Republic of", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 116, Code = "KR", Name = "Korea, Republic of", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 117, Code = "XK", Name = "Kosovo", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 118, Code = "KW", Name = "Kuwait", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 119, Code = "KG", Name = "Kyrgyzstan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 120, Code = "LA", Name = "Lao People's Democratic Republic", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 121, Code = "LV", Name = "Latvia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 122, Code = "LB", Name = "Lebanon", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 123, Code = "LS", Name = "Lesotho", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 124, Code = "LR", Name = "Liberia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 125, Code = "LY", Name = "Libyan Arab Jamahiriya", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 126, Code = "LI", Name = "Liechtenstein", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 127, Code = "LT", Name = "Lithuania", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 128, Code = "LU", Name = "Luxembourg", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 129, Code = "MO", Name = "Macau", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 130, Code = "MK", Name = "Macedonia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 131, Code = "MG", Name = "Madagascar", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 132, Code = "MW", Name = "Malawi", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 133, Code = "MY", Name = "Malaysia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 134, Code = "MV", Name = "Maldives", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 135, Code = "ML", Name = "Mali", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 136, Code = "MT", Name = "Malta", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 137, Code = "MH", Name = "Marshall Islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 138, Code = "MQ", Name = "Martinique", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 139, Code = "MR", Name = "Mauritania", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 140, Code = "MU", Name = "Mauritius", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 141, Code = "TY", Name = "Mayotte", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 142, Code = "MX", Name = "Mexico", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 143, Code = "FM", Name = "Micronesia, Federated States of", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 144, Code = "MD", Name = "Moldova, Republic of", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 145, Code = "MC", Name = "Monaco", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 146, Code = "MN", Name = "Mongolia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 147, Code = "ME", Name = "Montenegro", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 148, Code = "MS", Name = "Montserrat", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 149, Code = "MA", Name = "Morocco", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 150, Code = "MZ", Name = "Mozambique", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 151, Code = "MM", Name = "Myanmar", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 152, Code = "NA", Name = "Namibia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 153, Code = "NR", Name = "Nauru", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 154, Code = "NP", Name = "Nepal", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 155, Code = "NL", Name = "Netherlands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 156, Code = "AN", Name = "Netherlands Antilles", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 157, Code = "NC", Name = "New Caledonia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 158, Code = "NZ", Name = "New Zealand", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 159, Code = "NI", Name = "Nicaragua", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 160, Code = "NE", Name = "Niger", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 161, Code = "NG", Name = "Nigeria", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 162, Code = "NU", Name = "Niue", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 163, Code = "NF", Name = "Norfolk Island", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 164, Code = "MP", Name = "Northern Mariana Islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 165, Code = "NO", Name = "Norway", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 166, Code = "OM", Name = "Oman", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 167, Code = "PK", Name = "Pakistan", CurrencyCode= "PKR", FlagCode="",DisplayOrder=0,CurrencyName="",CountryPhoneCode="+92",IsActive=false,FlagImageUrl="",CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 168, Code = "PW", Name = "Palau", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 169, Code = "PS", Name = "Palestine", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 170, Code = "PA", Name = "Panama", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 171, Code = "PG", Name = "Papua New Guinea", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 172, Code = "PY", Name = "Paraguay", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 173, Code = "PE", Name = "Peru", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 174, Code = "PH", Name = "Philippines", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 175, Code = "PN", Name = "Pitcairn", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 176, Code = "PL", Name = "Poland", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 177, Code = "PT", Name = "Portugal", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 178, Code = "PR", Name = "Puerto Rico", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 179, Code = "QA", Name = "Qatar", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 180, Code = "RE", Name = "Reunion", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 181, Code = "RO", Name = "Romania", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 182, Code = "RU", Name = "Russian Federation", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 183, Code = "RW", Name = "Rwanda", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 184, Code = "KN", Name = "Saint Kitts and Nevis", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 185, Code = "LC", Name = "Saint Lucia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 186, Code = "VC", Name = "Saint Vincent and the Grenadines", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 187, Code = "WS", Name = "Samoa", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 188, Code = "SM", Name = "San Marino", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 189, Code = "ST", Name = "Sao Tome and Principe", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 190, Code = "SA", Name = "Saudi Arabia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 191, Code = "SN", Name = "Senegal", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 192, Code = "RS", Name = "Serbia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 193, Code = "SC", Name = "Seychelles", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 194, Code = "SL", Name = "Sierra Leone", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 195, Code = "SG", Name = "Singapore", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 196, Code = "SK", Name = "Slovakia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 197, Code = "SI", Name = "Slovenia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 198, Code = "SB", Name = "Solomon Islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 199, Code = "SO", Name = "Somalia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 200, Code = "ZA", Name = "South Africa", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 201, Code = "GS", Name = "South Georgia South Sandwich Islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 202, Code = "SS", Name = "South Sudan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 203, Code = "ES", Name = "Spain", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 204, Code = "LK", Name = "Sri Lanka", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 205, Code = "SH", Name = "St. Helena", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 206, Code = "PM", Name = "St. Pierre and Miquelon", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 207, Code = "SD", Name = "Sudan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 208, Code = "SR", Name = "Suriname", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 209, Code = "SJ", Name = "Svalbard and Jan Mayen Islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 210, Code = "SZ", Name = "Swaziland", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 211, Code = "SE", Name = "Sweden", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 212, Code = "CH", Name = "Switzerland", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 213, Code = "SY", Name = "Syrian Arab Republic", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 214, Code = "TW", Name = "Taiwan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 215, Code = "TJ", Name = "Tajikistan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 216, Code = "TZ", Name = "Tanzania, United Republic of", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 217, Code = "TH", Name = "Thailand", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 218, Code = "TG", Name = "Togo", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 219, Code = "TK", Name = "Tokelau", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 220, Code = "TO", Name = "Tonga", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 222, Code = "TT", Name = "Trinidad and Tobago", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 223, Code = "TN", Name = "Tunisia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 224, Code = "TR", Name = "Turkey", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 225, Code = "TM", Name = "Turkmenistan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 226, Code = "TC", Name = "Turks and Caicos Islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 227, Code = "TV", Name = "Tuvalu", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 228, Code = "UG", Name = "Uganda", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 229, Code = "UA", Name = "Ukraine", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 230, Code = "AE", Name = "United Arab Emirates", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 231, Code = "GB", Name = "United Kingdom", CurrencyCode="GBP",FlagCode="",DisplayOrder=0,CurrencyName="",CountryPhoneCode="+44",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 232, Code = "US", Name = "United States", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 233, Code = "UM", Name = "United States minor outlying islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 234, Code = "UY", Name = "Uruguay", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 235, Code = "UZ", Name = "Uzbekistan", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 236, Code = "VU", Name = "Vanuatu", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 237, Code = "VA", Name = "Vatican City State", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 238, Code = "VE", Name = "Venezuela", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 239, Code = "VN", Name = "Vietnam", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 240, Code = "VG", Name = "Virgin Islands (British)", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 241, Code = "VI", Name = "Virgin Islands (U.S.)", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 242, Code = "WF", Name = "Wallis and Futuna Islands", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 243, Code = "EH", Name = "Western Sahara", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 244, Code = "YE", Name = "Yemen", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 245, Code = "ZR", Name = "Zaire", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 246, Code = "ZM", Name = "Zambia", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) },
                new CountryList { Id = 247, Code = "ZW", Name = "Zimbabwe", CurrencyCode="",FlagCode="",DisplayOrder=0,CurrencyName="",IsActive=false,FlagImageUrl="", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", CreatedDate = new DateTime(2020, 1, 1), UpdatedDate = new DateTime(2020, 1, 1) }
                );
            builder.Entity<Gender>().HasData(
               new Gender { Id = 1,TenantId=14, Name = GenderEnum.Male.ToDescription(), Code="M", Status = true, CreatedDate = new DateTime(2020, 1, 1), CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6",UpdatedBy="",UpdatedDate=new DateTime(2020, 1, 1) },
               new Gender { Id = 2, TenantId =14, Name = GenderEnum.Female.ToDescription(), Code = "F", Status = true, CreatedDate = new DateTime(2020, 1, 1), CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", UpdatedBy = "", UpdatedDate = new DateTime(2020, 1, 1) }
               );
            builder.Entity<ApplicationRole>().HasData(
             new ApplicationRole { Id = ApplicationConstants.CustomerRoleId,Name="Customer SimpleTransfer",NormalizedName="CUSTOMER SIMPLETRANSFER",ConcurrencyStamp="0ed60e11-8c45-4934-b0c3-86207c874046",Type= 1,Status=   1,UpdatedAt=new DateTime(2020, 1, 1),CreatedAt=new DateTime(2020, 1, 1),TenantId=14,DisplayName=" Customer",CreatedBy="df0d5fc1-b3c9-448f-afea-a43cd08005a6",OrganizationName="Simple Transfer" },
             new ApplicationRole { Id = ApplicationConstants.AdministratorRoleId, Name = "Administrator SimpleTransfer", NormalizedName = "Administrator SIMPLETRANSFER", ConcurrencyStamp = "0ed60e11-8c45-4934-b0c3-86207c874046", Type = 1, Status = 1, UpdatedAt = new DateTime(2020, 1, 1), CreatedAt = new DateTime(2020, 1, 1), TenantId = 14, DisplayName = " Administrator", CreatedBy = "df0d5fc1-b3c9-448f-afea-a43cd08005a6", OrganizationName = "Simple Transfer" }
             );
            builder.Entity<PayoutModeConfig>().HasData(
                new PayoutModeConfig { Id = (int)PayoutMode.BankTransfer, PayoutModeName = PayoutMode.BankTransfer.ToDescription(), Status = true },
                new PayoutModeConfig { Id = (int)PayoutMode.CashPickup, PayoutModeName = PayoutMode.CashPickup.ToDescription(), Status = true },
                new PayoutModeConfig { Id = (int)PayoutMode.AirtimeTopup, PayoutModeName = PayoutMode.AirtimeTopup.ToDescription(), Status = true },
                new PayoutModeConfig { Id = (int)PayoutMode.MobileMoney, PayoutModeName = PayoutMode.MobileMoney.ToDescription(), Status = true }
            );
            builder.Entity<Permission>().HasData(
                 new Permission
                 {
                     Id = 51,
                     Name = "View Country"
                 },
                new Permission
                {
                    Id = 52,
                    Name = "Create Country"
                },
                new Permission
                {
                    Id = 53,
                    Name = "Update Country"
                },
                new Permission
                {
                    Id = 54,
                    Name = "Delete Country"
                },
                new Permission
                {
                    Id = 55,
                    Name = "View Country Payout Mode"
                },
                new Permission
                {
                    Id = 56,
                    Name = "Create Country Payout Mode"
                },
                new Permission
                {
                    Id = 57,
                    Name = "Update Country Payout Mode"
                }, new Permission
                {
                    Id = 58,
                    Name = "Delete Country Payout Mode"
                }, new Permission
                {
                    Id = 60,
                    Name = "View Bank Mapper"
                },
                  new Permission
                  {
                      Id = 61,
                      Name = "Create Bank Mapper"
                  }, new Permission
                  {
                      Id = 62,
                      Name = "Update Bank Mapper"
                  },
                   new Permission
                   {
                       Id = 63,
                       Name = "Delete Bank Mapper"
                   },
                new Permission
                {
                    Id = 64,
                    Name = "View Branch Mapper"
                },
                new Permission
                {
                    Id = 65,
                    Name = "Create Branch Mapper"
                },
                new Permission
                {
                    Id = 66,
                    Name = "Update Branch Mapper"
                },
                new Permission
                {
                    Id = 67,
                    Name = "Delete Branch Mapper"
                },
                new Permission
                {
                    Id = 68,
                    Name = "View City Config"
                },
                new Permission
                {
                    Id = 69,
                    Name = "Create City Config"
                },
                new Permission
                {
                    Id = 70,
                    Name = "Update City Config "
                }, new Permission
                {
                    Id = 71,
                    Name = "Delete City Config"
                }, new Permission
                {
                    Id = 72,
                    Name = "View Rate Supplier"
                },
                  new Permission
                  {
                      Id = 73,
                      Name = "Create Rate Supplier"
                  }, new Permission
                  {
                      Id = 74,
                      Name = "Update Rate Supplier"
                  },
                   new Permission
                   {
                       Id = 75,
                       Name = "Delete Rate Supplier"
                   },
                new Permission
                {
                    Id = 76,
                    Name = "View ExchangeRates"
                },
                new Permission
                {
                    Id = 77,
                    Name = "Create ExchangeRates"
                },
                new Permission
                {
                    Id = 78,
                    Name = "Update ExchangeRates"
                },
                new Permission
                {
                    Id = 79,
                    Name = "Delete ExchangeRates"
                },
                new Permission
                {
                    Id = 80,
                    Name = "View Correspondent Bank"
                },
                new Permission
                {
                    Id = 81,
                    Name = "Create Correspondent Bank"
                },
                new Permission
                {
                    Id = 82,
                    Name = "Update Correspondent Bank"
                }, new Permission
                {
                    Id = 83,
                    Name = "Delete Correspondent Bank"
                }, new Permission
                {
                    Id = 84,
                    Name = "View Relationship Config"
                }, new Permission
                {
                    Id = 85,
                    Name = "Create Relationship Config"
                },
                  new Permission
                  {
                      Id = 86,
                      Name = "Update Relationship Config"
                  }, new Permission
                  {
                      Id = 87,
                      Name = "Delete Relationship Config"
                  },
                   new Permission
                   {
                       Id = 88,
                       Name = "View Marital Status Config"
                   },
                new Permission
                {
                    Id = 89,
                    Name = "Create Marital Status Config"
                },
                new Permission
                {
                    Id = 90,
                    Name = "Update Marital Status Config"
                },
                new Permission
                {
                    Id = 91,
                    Name = "Delete Marital Status Config"
                },
                new Permission
                {
                    Id = 92,
                    Name = "View Gender Config"
                },
                new Permission
                {
                    Id = 93,
                    Name = "Create Gender Config"
                },
                new Permission
                {
                    Id = 94,
                    Name = "Update Gender Config"
                },
                new Permission
                {
                    Id = 95,
                    Name = "Delete Gender Config"
                }, new Permission
                {
                    Id = 96,
                    Name = "View IdType Config"
                }, new Permission
                {
                    Id = 97,
                    Name = "Create IdType Config"
                },
                  new Permission
                  {
                      Id = 98,
                      Name = "Update IdType Config"
                  }, new Permission
                  {
                      Id = 99,
                      Name = "Delete IdType Config"
                  },
                   new Permission
                   {
                       Id = 100,
                       Name = "View Payment Purpose Config"
                   },
                new Permission
                {
                    Id = 101,
                    Name = "Create Payment Purpose Config"
                },
                   new Permission
                   {
                       Id = 102,
                       Name = "Update Payment Purpose Config"
                   },
                new Permission
                {
                    Id = 103,
                    Name = "Delete Payment Purpose Config"
                },
                   new Permission
                   {
                       Id = 104,
                       Name = "View Rate Supplier Fee Config"
                   },
                new Permission
                {
                    Id = 105,
                    Name = "Create Rate Supplier Fee Config"
                },
                   new Permission
                   {
                       Id = 106,
                       Name = "Update Rate Supplier Fee Config"
                   },
                new Permission
                {
                    Id = 107,
                    Name = "Delete Rate Supplier Fee Config"
                },
                   new Permission
                   {
                       Id = 108,
                       Name = "View Txn Limit Config"
                   },
                new Permission
                {
                    Id = 109,
                    Name = "Create Txn Limit Config"
                },
                   new Permission
                   {
                       Id = 110,
                       Name = "Update Txn Limit Config"
                   },
                new Permission
                {
                    Id = 111,
                    Name = "Delete Txn Limit Config"
                }
             );
            builder.Entity<PermissionGroup>().HasData(
               new PermissionGroup
               {
                   Id = 14,
                   Name = "CountryConfig",
                   PermissionIds = "51,52,53,54"
               }
               ,new PermissionGroup
               {
                   Id =15 ,
                   Name = "CountryPayoutMode",
                   PermissionIds = "55,56,57,58"
               }, new PermissionGroup
               {
                   Id = 16,
                   Name = "BankMapper",
                   PermissionIds = "60,61,62,63"
               }, new PermissionGroup
               {
                   Id = 17,
                   Name = "BranchMapper",
                   PermissionIds = "64,65,66,67"
               }, new PermissionGroup
               {
                   Id =18 ,
                   Name = "CityConfig",
                   PermissionIds = "68,69,70,71"
               }, new PermissionGroup
               {
                   Id = 19,
                   Name = "RateSupplier",
                   PermissionIds = "72,73,74,75"
               }, new PermissionGroup
               {
                   Id = 20,
                   Name = "ExchangeRates",
                   PermissionIds = "76,77,78,79"
               }, new PermissionGroup
               {
                   Id = 21,
                   Name = "CorrespondentBank",
                   PermissionIds = "80,81,82,83"
               }, new PermissionGroup
               {
                   Id = 22,
                   Name = "RelationshipConfig",
                   PermissionIds = "84,85,86,87"
               }, new PermissionGroup
               {
                   Id = 23,
                   Name = "MaritalStatusConfig",
                   PermissionIds = "88,89,90,91"
               }, new PermissionGroup
               {
                   Id = 24,
                   Name = "GenderConfig",
                   PermissionIds = "92,93,94,95"
               }, new PermissionGroup
               {
                   Id =25 ,
                   Name = "IdTypeConfig",
                   PermissionIds = "96,97,98,99"
               }, new PermissionGroup
               {
                   Id = 26,
                   Name = "PaymentPurposeConfig",
                   PermissionIds = "100,101,102,103"
               }, new PermissionGroup
               {
                   Id = 27,
                   Name = "RateSupplierFeeConfig",
                   PermissionIds = "104,105,106,107"
               }, new PermissionGroup
               {
                   Id =28 ,
                   Name = "TxnLimitConfig",
                   PermissionIds = "108,109,110,111"
               }
               );
            builder.Entity<UatSetting>().HasData(
               new UatSetting { Id = 1, PhoneNumber = "", Status = false });
        }
    }
}
