using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cicero.Data.Enumerations;

namespace Cicero.Service.Services.API
{
    public interface IBankBranchServcie
    {
        bool CreateOrUpdateBankBranch(string countryCode, RateSupplierEnum supplierEnum, List<TransfastBankBranchModel> transfastBanks = null, List<NecBankBranchViewModel> necBanks = null);
        Task<bool> CreateOrUpdateBank(string countryCode, RateSupplierEnum supplierEnum, List<TransfastBankModel> transfastBanks = null, List<NecBankViewModel> necBanks = null);
        Task<bool> CreateOrUpdateCity(List<SupplierCity> cities, string countryCode);
        bool CreateBankBranch(string countryCode, RateSupplierEnum supplierEnum, TransfastBankBranchModel transfastBankBranches = null, List<NecBankBranchViewModel> necBankBranches = null);
 }

    public class BankBranchServcie : IBankBranchServcie
    {
        private readonly SimpleTransferApplicationDbContext _db;

        public BankBranchServcie(SimpleTransferApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateOrUpdateBankBranch (string countryCode, RateSupplierEnum supplierEnum, List<TransfastBankBranchModel> transfastBankBranches = null, List<NecBankBranchViewModel> necBankBranches = null)
        {
            try
            {
                List<SupplierBankBranch> branches = new List<SupplierBankBranch>();
                //if (supplierEnum == RateSupplierEnum.NecMoney)
                //{
                //    foreach (NecBankBranchViewModel item in necBankBranches)
                //    {
                //        var branch = new SupplierBankBranch()
                //        {
                //            SupplierId = (int)RateSupplierEnum.NecMoney,
                //            CountryCode = countryCode.ToUpper(),
                //            BankCode = item.BankCode.ToUpper(),
                //            BranchCode = item.Code.ToUpper(),
                //            BranchName = item.Name.ToUpper(),
                //            CreatedAt = DateTime.Now,
                //            UpdatedAt = DateTime.Now,
                //            Status = true,
                //            CreatedBy = "System",
                //            CityCode= item.City
                //        };
                //        branches.Add(branch);
                //        var isCity = _db.SupplierCity.Where(x => x.CityName == item.City.ToUpper() && x.CountryCode == item.CountryIsoCode.ToUpper() && x.SupplierId == (int)RateSupplierEnum.NecMoney).FirstOrDefault();
                //        if (isCity == null)
                //        {
                //            var city = new SupplierCity()
                //            {
                //                SupplierId = (int)RateSupplierEnum.NecMoney,
                //                CityCode = "",
                //                CityName = item.City.ToUpper(),
                //                CountryCode = item.CountryIsoCode.ToUpper(),
                //                Status = true,
                //                CreatedBy = "System"
                //            };
                //            _db.SupplierCity.Add(city);
                //        }
                //        _db.SaveChanges();
                //    }
                //}
                if (supplierEnum == RateSupplierEnum.Transfast)
                {
                    foreach (TransfastBankBranchModel item in transfastBankBranches)
                    {
                        if (item.BankBranchID == "" || item.BankBranchName == "")
                        {
                            continue;
                        }
                        var branch = new SupplierBankBranch()
                        {
                            SupplierId = (int)RateSupplierEnum.Transfast,
                            CityCode = item.CityCode,
                            CountryCode = countryCode.ToUpper(),
                            BankCode = item.BankID.ToUpper(),
                            BranchCode = item.BankBranchID.ToUpper(),
                            BranchName = item.BankBranchName.ToUpper(),
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            Status = true,
                            CreatedBy = "System"
                        };
                        branches.Add(branch);
                    }
                }
                _db.SupplierBankBranch.AddRange(branches);                
                _db.SaveChanges();
                return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CreateBankBranch(string countryCode, RateSupplierEnum supplierEnum, TransfastBankBranchModel transfastBankBranches = null, List<NecBankBranchViewModel> necBankBranches = null)
        {
            try
            {
                var branch = new SupplierBankBranch()
                {
                    SupplierId = (int)RateSupplierEnum.Transfast,
                    CityCode = transfastBankBranches.CityCode,
                    CountryCode = countryCode.ToUpper(),
                    BankCode = transfastBankBranches.BankID.ToUpper(),
                    BranchCode = transfastBankBranches.BankBranchID.ToUpper(),
                    BranchName = transfastBankBranches.BankBranchName.ToUpper(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Status = true,
                    CreatedBy = "System"
                };
                _db.SupplierBankBranch.Add(branch);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CreateOrUpdateBank(string countryCode, RateSupplierEnum supplierEnum, List<TransfastBankModel> transfastBanks = null, List<NecBankViewModel> necBanks = null)
        {
            try
            {
                RateSupplierEnum a = (RateSupplierEnum)1;
                List<SupplierBank> banks = new List<SupplierBank>();
                //if (supplierEnum == RateSupplierEnum.NecMoney)
                //{
                //    foreach (var item in necBanks)
                //    {
                //        var bankExists = _db.SupplierBank.Where(x => x.BankCode == item.Code && x.CountryCode.ToUpper() == item.CountryIsoCode.ToUpper()
                //        && x.SupplierId==(int)RateSupplierEnum.NecMoney);
                //        if (bankExists != null)
                //        {
                //            var bank = new SupplierBank()
                //            {
                //                SupplierId = (int)RateSupplierEnum.NecMoney,
                //                CountryCode = countryCode.ToUpper(),
                //                BankCode = item.Code.ToUpper(),
                //                BankName = item.Name.ToUpper(),
                //                CreatedAt = DateTime.Now,
                //                UpdatedAt = DateTime.Now,
                //                Status = true,
                //                CreatedBy = "System"
                //            };
                //            banks.Add(bank);
                //        }
                //        else
                //        {
                //            var bank = bankExists.FirstOrDefault();
                //            bank.BankName = item.Name;
                //            _db.SupplierBank.Update(bank);
                //        }
                //    }
                //}
                if (supplierEnum == RateSupplierEnum.Transfast)
                {
                    foreach (var item in transfastBanks)
                    {
                        var bankExists = _db.SupplierBank.Where(x => x.BankCode == item.Id && x.CountryCode.ToUpper() == countryCode.ToUpper()
                          && x.SupplierId == (int)RateSupplierEnum.Transfast);
                        if (bankExists != null)
                        {
                            var bank = new SupplierBank()
                            {
                                SupplierId = (int)RateSupplierEnum.Transfast,
                                CountryCode = countryCode.ToUpper(),
                                BankCode = item.Id.ToUpper(),
                                BankName = item.Name.ToUpper(),
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                                Status = true,
                                CreatedBy = "System"
                            };
                            banks.Add(bank);
                        }
                        else
                        {
                            var bank = bankExists.FirstOrDefault();
                            bank.BankName = item.Name;
                            _db.SupplierBank.Update(bank);
                        }
                    }
                }
                _db.SupplierBank.AddRange(banks);
                await _db.SaveChangesAsync();
                return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CreateOrUpdateCity(List<SupplierCity> cities,string countryCode)
        {
            var citylist = _db.SupplierCity.Where(x => x.SupplierId == (int)RateSupplierEnum.Transfast && x.CountryCode.ToUpper() == countryCode.ToUpper());
            _db.SupplierCity.RemoveRange(citylist);   
            _db.SupplierCity.AddRange(cities);
            _db.SaveChanges();
            return true;
            
        }
    }
}
