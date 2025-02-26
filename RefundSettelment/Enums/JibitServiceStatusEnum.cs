using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundSettelment.Enums
{
    public enum JibitServiceStatusEnum
    {
        Failed = -1,
        Done = 0,
        IbanInquiry = 1,
        TransferInserted = 2,
        GetBalance = 3,
        SettlementInProgress = 4
    }
}
