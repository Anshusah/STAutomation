using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Cicero.Service
{
    public class SimpleTransferEnums
    {
        public enum RateSupplier
        {
            [Description("Transfast")]
            Transfast=1,
            //[Description("NecMoney")]
            //NecMoney=2,
            //[Description("Safkhan")]
            //Safkhan=3
        }
        public enum SchedulerList
        {
            [Description("Transfast")]
            Transfast=1,
            //[Description("NecMoney")]
            //NecMoney=2
        }

    }
}
