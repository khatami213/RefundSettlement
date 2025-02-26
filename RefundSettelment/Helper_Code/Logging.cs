using System;
using Logging;
using RefundSettelment.Model;

namespace RefundSettelment.Helper_Code
{
    public class Logging
    {
        //logger
        private static readonly ILogger Logger = LogManager.GetLogger("RefundSettelment");

        public static void Log(string functionName, string refrenceNumber, string rrn, string amount, string maskedDestinationPan, string refundDescription, string type, string userName)
        {
            try
            {
                using (var db = new PNA_RefundServiceEntities())
                {
                    switch (type)
                    {
                        case "Info":
                            Logger.Info($"Function Name: {functionName}  Refrence Number: {refrenceNumber}   RRN: {rrn}   Amount: {amount}   Masked Destination Pan: {maskedDestinationPan}   Refund Description: {refundDescription}   Type: {type}   UserName: {userName}");
                            break;
                        case "Warning":
                            Logger.Warn($"Function Name: {functionName}  Refrence Number: {refrenceNumber}   RRN: {rrn}   Amount: {amount}   Masked Destination Pan: {maskedDestinationPan}   Refund Description: {refundDescription}   Type: {type}   UserName: {userName}");
                            break;
                        case "Error":
                            Logger.Error($"Function Name: {functionName}  Refrence Number: {refrenceNumber}   RRN: {rrn}   Amount: {amount}   Masked Destination Pan: {maskedDestinationPan}   Refund Description: {refundDescription}   Type: {type}   UserName: {userName}");
                            break;
                        case "Exception":
                            Logger.Fatal($"Function Name: {functionName}  Refrence Number: {refrenceNumber}   RRN: {rrn}   Amount: {amount}   Masked Destination Pan: {maskedDestinationPan}   Refund Description: {refundDescription}   Type: {type}   UserName: {userName}");
                            break;
                        default:
                            Logger.Info($"Function Name: {functionName}  Refrence Number: {refrenceNumber}   RRN: {rrn}   Amount: {amount}   Masked Destination Pan: {maskedDestinationPan}   Refund Description: {refundDescription}   Type: {type}   UserName: {userName}");
                            break;
                    }

                    var logEntity = new Log
                    {
                        FunctionName = functionName,
                        Amount = long.Parse(amount),
                        MaskedDestinationPan = maskedDestinationPan,
                        Type = type,
                        RefrenceNumber = long.Parse(refrenceNumber),
                        InsertDateTime = DateTime.Now,
                        RefundDescription = refundDescription,
                        Rrn = rrn,
                        UserName = userName
                    };
                    db.Log.Add(logEntity);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal($"Exception: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Logger.Fatal($"Inner Exception: {ex.InnerException.Message}");
                    if (ex.InnerException.InnerException != null)
                    {
                        Logger.Fatal($"Inner Exception: {ex.InnerException.InnerException.Message}");
                        if (ex.InnerException.InnerException.InnerException != null)
                            Logger.Fatal($"Inner Exception: {ex.InnerException.InnerException.InnerException.Message}");
                    }
                }
            }
        }
    }
}