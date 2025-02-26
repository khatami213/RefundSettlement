using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Logging;
using RefundSettelment.Helper_Code.Types;
using RefundSettelment.Model;
using RefundSettelment.Model.RefundModel;
using RefundSettelment.YaghutWebRefrence;
using RefundWindowsService.Security;

namespace RefundSettelment.Helper_Code
{
    public static class Helpers
    {
        //کلیدهای رمزنگاری سرور
        private const string PublicKey = "<RSAKeyValue><Modulus>wOmcBUMLQU9EVavB8t/EWefZrWG+thJOI8ccyTrvcUcU/knO5OXMq7/we0turVjjM6gswsWwjPqqax9JHQdTR0B1VMVOOEKn/3HMOK6S/mZLF+XdEmd2g+6GRkS7sJiM9IKrfu8cYTawS7wgBhtW6/j5h1mj6ja4BSGBvoOozxzHWoFEfpulJ5JzEfhL6UqvNBnRe8eLP/rJKDYiYX2zX/r/cBPLCFyBs05nDrzuZ/1GNEisBYJ8COBV8UrxyE5Utw+wgSLwp+Jf2QWGatT4EANNVjUO+Nzz13cOCXJQ+XQIFDFTDVf4pNQPJeIRJc8QGZWndWkEEiX8AidgCmGnjQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        private const string PrivateKey = "<RSAKeyValue><Modulus>wOmcBUMLQU9EVavB8t/EWefZrWG+thJOI8ccyTrvcUcU/knO5OXMq7/we0turVjjM6gswsWwjPqqax9JHQdTR0B1VMVOOEKn/3HMOK6S/mZLF+XdEmd2g+6GRkS7sJiM9IKrfu8cYTawS7wgBhtW6/j5h1mj6ja4BSGBvoOozxzHWoFEfpulJ5JzEfhL6UqvNBnRe8eLP/rJKDYiYX2zX/r/cBPLCFyBs05nDrzuZ/1GNEisBYJ8COBV8UrxyE5Utw+wgSLwp+Jf2QWGatT4EANNVjUO+Nzz13cOCXJQ+XQIFDFTDVf4pNQPJeIRJc8QGZWndWkEEiX8AidgCmGnjQ==</Modulus><Exponent>AQAB</Exponent><P>4ZFdS9A6j/HDKxK3Emq8qVXS1LS5AwXGIbrbOG8lLHLLwsneMhiBdBw5QdcXQHJZ9Oc/JjQ+28Y/ETsc/FIxlZJBntSJiJUoNvKJaFXerxYhh6dNO/BqxhccSOdswz/gQqB0qkg5/jrSB6UasmbINVIF7mk6XdaOzJ0QgBG/X3s=</P><Q>2vBmecwFtWkt6Tp8b9lV5Bsm8Zqr8izZyQh+KjWwg2FwZdt1rH1u6IZtHh6LmAg6eAWH1MWfJ6gjrPNIzyvyOa/46L7KDmdvSwdbEfjDOuRJ2yWGGIM0I9XbAQPN3TKf30qScvPP5Lp13mxkUjJlrKPQ4Lg2zWNOjF0OfdXWIpc=</Q><DP>SVzg7h5sXZKw+lpc5oWGlMCQEJQytDP1i9TdJc6oVXuEn/bN6Jcly2C+kpZlPpWygj+Pv1ows4QX0P/b3ojRDaeC5iiUDrMMYEqjvCZphaJ6B0e3i+4WnBS6I0/5hMtKogDT0Ooqym/RDaF6PFnHdegWe8MHs6tryEqxKiYbiu8=</DP><DQ>v/yBEBrtgpgZ72QfDIG7xMxeiQzF7RaRX6033VG5WGwQgPFCLiDMKdD/TKMibA4DH45R/y3Qk5jot9eaqDj0Lsv17DqpupnPSS7JGGhY4oKflTFBdqtPBIGaizhHxMmI0eh1paHRUtSDWakZC88vw4TfPL+tJswHbCSJ+aSTIz8=</DQ><InverseQ>vVj+x7ssSIxrEPIoJVaQRB2UzC9drk1TljpADYFtiPkBZyxSo/n2KJzqO5KatCBkzvAZwKpANCsJbH2CQ81AcATa3ONnfgWeypV9uKcyZPD63F0VXwGbJM7AUVuU6zXCkStSEmRJ1io6N5ugtl30x0KmNmQNtNdJ3qanNtzJBtQ=</InverseQ><D>APot/CjWycHpCrYQCXbwu7Pc+m/gU3PMSYocrzhJNj2x8YfWMHqpisUyJq2/JcmpfP2BHIt71Xr/mgNSj38WAOpmrcNCHi7YQwcEjdT0ka1a/AgCErHLe+edboWynbZoIGT5EW+MqUFpqziMwPsqeY+NVA40Ml+MlxoQWjK4jDQK3gY3ulXUKmk23bBRhTC/mwlyhQsKz46CSeiEYCRJwE97LxPB0uKRXYA8aPgk3EsHrsENcI2ovr+cITSf+Qg2HN1nk6zVO3TpotO8B/iKPbnDSmbgVwR1cGatS6nkjwp1RAgetay7l2QSYTQPQDcg0LWKywIaax4TFVtVg++c0Q==</D></RSAKeyValue>";

        //جدول کارمزدها 
        private static readonly string Under10000000Rilas = ConfigurationManager.AppSettings["0-10,000,000"];
        private static readonly string From10000001To20000000Rilas = ConfigurationManager.AppSettings["10,000,001-20,000,000"];

        private static readonly string From20000001To30000000Rilas = ConfigurationManager.AppSettings["20,000,001-30,000,000"];

        //عدد آستانه کارمزد و کارت به کارت ها
        private static readonly string AmountThreshold = ConfigurationManager.AppSettings["CheckAmountThreshold"];

        //بازه های کارمزد
        private static readonly string CommissionPeriod0 = ConfigurationManager.AppSettings["CommissionPeriod0"];
        private static readonly string CommissionPeriod10000000 = ConfigurationManager.AppSettings["CommissionPeriod10000000"];
        private static readonly string CommissionPeriod10000001 = ConfigurationManager.AppSettings["CommissionPeriod10000001"];
        private static readonly string CommissionPeriod20000000 = ConfigurationManager.AppSettings["CommissionPeriod20000000"];
        private static readonly string CommissionPeriod20000001 = ConfigurationManager.AppSettings["CommissionPeriod20000001"];

        private static readonly string CommissionPeriod30000000 = ConfigurationManager.AppSettings["CommissionPeriod30000000"];

        //logger
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(Helpers));

        public static string GetMaskedPan(string pan)
        {
            var maskedPan = pan.Substring(0, 6) + "******" + pan.Substring(12);
            return maskedPan;
        }

        public static long GetCommission(long amount)
        {
            long commission = 0;

            if (amount > long.Parse(AmountThreshold))
            {
                var quotient = Math.DivRem(long.Parse(amount.ToString()), long.Parse(AmountThreshold), out var remainder);
                commission = quotient * long.Parse(From20000001To30000000Rilas);
                if (remainder > long.Parse(CommissionPeriod0) && remainder <= long.Parse(CommissionPeriod10000000))
                    commission += long.Parse(Under10000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod10000001) && remainder <= long.Parse(CommissionPeriod20000000))
                    commission += long.Parse(From10000001To20000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod20000001) && remainder <= long.Parse(CommissionPeriod30000000))
                    commission += long.Parse(From20000001To30000000Rilas);
            } else
            {
                if (amount > long.Parse(CommissionPeriod0) && amount <= long.Parse(CommissionPeriod10000000))
                    commission += long.Parse(Under10000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod10000001) && amount <= long.Parse(CommissionPeriod20000000))
                    commission += long.Parse(From10000001To20000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod20000001) && amount <= long.Parse(CommissionPeriod30000000))
                    commission += long.Parse(From20000001To30000000Rilas);
            }

            return commission;
        }

        public static bool CardTransfer(CardsInfo bankCardsInfo, CardTransferModel cardTransferModel, out int bankCardInfoIdForUpdate)
        {
            var cardTransferService = new Yaghut_MobileApp();
            bool result;
            bankCardInfoIdForUpdate = 0;

            try
            {
                using (var db = new PNA_RefundServiceEntities())
                {
                    #region برگرداندن اطلاعات رمز شده کارت های مبدا برای عملیات کارت به کارت

                    string cardInfoDecrpted = null;
                    try
                    {
                        cardInfoDecrpted = CryptorEngine.Decryption(bankCardsInfo.EncryptedData, PrivateKey);
                    } catch (Exception ex)
                    {
                        Logger.Error($"Error in decryption bank card info,description: {ex.Message}");
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

                    #endregion

                    #region نگاشت اطلاعات بازیابی شده کارت به مدل کارت

                    var cardsInfoModel = new CardsInfoModel();
                    try
                    {
                        if (cardInfoDecrpted != null)
                        {
                            bankCardInfoIdForUpdate = bankCardsInfo.Id;
                            cardsInfoModel.Id = bankCardsInfo.Id.ToString();
                            cardsInfoModel.Pan = cardInfoDecrpted.Split('|')[0];
                            cardsInfoModel.ExpDate = cardInfoDecrpted.Split('|')[1];
                            cardsInfoModel.Cvv2 = cardInfoDecrpted.Split('|')[2];
                            cardsInfoModel.Pin2 = cardInfoDecrpted.Split('|')[3];
                            cardsInfoModel.Balance = bankCardsInfo.Balance;

                            Logger.Info($"Card Info --> Pan: {GetMaskedPan(cardsInfoModel.Pan)} and ExpDate: {cardsInfoModel.ExpDate} CVV2: {cardsInfoModel.Cvv2.Length} and PIN: {cardsInfoModel.Pin2.Length}");
                        }
                    } catch (Exception ex)
                    {
                        Logger.Error($"Error in mapping decrpted card info to card info model,description: {ex.Message}");
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

                    #endregion

                    #region کارت مبدا را براي عمليات هاي مغايرت گيري احتمالي بعدي ذخيره مي کند

                    try
                    {
                        cardTransferModel.EncryptedSourcePan = CryptorEngine.Encryption(cardsInfoModel.Pan, PublicKey);
                        db.SaveChanges();
                    } catch (Exception ex)
                    {
                        Logger.Error($"Error in encryption cards info pan,description: {ex.Message}");
                        if (ex.InnerException != null)
                        {
                            Logger.Fatal($"Inner Exception: {ex.InnerException.Message}");
                            if (ex.InnerException.InnerException != null)
                            {
                                Logger.Fatal($"Inner Exception: {ex.InnerException?.InnerException?.Message}");
                                if (ex.InnerException?.InnerException?.InnerException != null)
                                    Logger.Fatal($"Inner Exception: {ex.InnerException?.InnerException?.InnerException?.Message}");
                            }
                        }
                    }

                    #endregion

                    #region عمليات کارت به کارت

                    var resps = new[] {"0990", "Error In Card Transfer"};
                    var destinationPan = "";
                    try
                    {
                        destinationPan = CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", "");

                        var checkCardTransferAmount = db.CardTransfer.Where(x => x.RefundRefrenceNumber == cardTransferModel.RefundRefrenceNumber && x.Status == "00").ToList();
                        var sumAmount = checkCardTransferAmount.Sum(x => x.Amount);
                        var refundRequestModel = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == cardTransferModel.RefundRefrenceNumber);
                        var checkAmount = sumAmount + cardTransferModel.Amount;
                        if (checkAmount <= refundRequestModel?.TransactionAmount)
                        {
                            var cardTransferRetryCountCheck = db.CardTransfer.FirstOrDefault(x => x.Id == cardTransferModel.Id);
                            if (cardTransferRetryCountCheck != null && cardTransferRetryCountCheck.RetryCount < 10)
                            {
                                cardTransferRetryCountCheck.RetryCount++;
                                //var response = "00|465465456|465456456456";
                                var response = cardTransferService.CardTransfer(
                                    cardsInfoModel.Pan,
                                    cardsInfoModel.Pin2,
                                    cardsInfoModel.Cvv2,
                                    cardsInfoModel.ExpDate,
                                    destinationPan,
                                    cardTransferModel.Amount
                                );
                                if (response != null)
                                    resps = response.Split('|');
                                Logger.Info($"Card Transfer To Card Number {GetMaskedPan(destinationPan)}, Response: {response}");

                                #region Card Transfer Retry In Error Code 78 and 55

                                if (resps[0] == "78" || resps[0] == "55")
                                {
                                    var secondRetryResponse = CardTransferRetry(bankCardsInfo.Id, cardTransferModel, resps[0], out var secondCardTransferRetry, out var nextBankCardInfoId);
                                    resps = secondRetryResponse?.Split('|') ?? new[] {resps[0], $"Error In Second Retry Of Card Transfer In Response Code {resps[0]},Card Id: {nextBankCardInfoId}"};
                                    if (secondCardTransferRetry)
                                    {
                                        var updateLastBankCardInfo = db.CardsInfo.FirstOrDefault(x => x.Id == bankCardsInfo.Id);
                                        if (updateLastBankCardInfo != null)
                                        {
                                            updateLastBankCardInfo.Balance = 0;
                                            updateLastBankCardInfo.IsUsing = false;
                                            db.SaveChanges();
                                        }

                                        bankCardsInfo = db.CardsInfo.FirstOrDefault(x => x.Id == nextBankCardInfoId);
                                        bankCardInfoIdForUpdate = nextBankCardInfoId;
                                    } else
                                    {
                                        var updateNextBankCardInfoId = db.CardsInfo.FirstOrDefault(x => x.Id == nextBankCardInfoId);
                                        if (updateNextBankCardInfoId != null)
                                        {
                                            updateNextBankCardInfoId.IsUsing = false;
                                            db.SaveChanges();
                                        }
                                    }
                                }

                                #endregion
                            } else
                            {
                                resps[0] = "78";
                                resps[1] = "retry time exceeded";
                            }
                        } else
                        {
                            Logger.Error($"Warning in card transfer,description: card transfer has been settled before, for card number: {GetMaskedPan(destinationPan)}");
                        }
                    } catch (Exception ex)
                    {
                        resps[0] = "0990";
                        resps[1] = "Error In Card Transfer";
                        Logger.Error($"Error in card transfer,description: {ex.Message}");
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

                    #endregion

                    #region در صورت موفق بودن عمليات کارت به کارت فيلد وضعيت به 00 يعني اتمام موفق عمليات

                    switch (resps[0])
                    {
                        case "00":
                        {
                            Logging.Log("Card Transfer", cardTransferModel.RefrenceNumber.ToString(), resps[1], cardTransferModel.Amount.ToString(), GetMaskedPan(CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", "")), $"Completed card transfer for refund request refrence number : {cardTransferModel.RefrenceNumber}, Card Id: {bankCardsInfo.Id} ", LogType.Info.ToString(), "RefundSettelment");
                            Logger.Info($"Completed card transfer for refund request refrence number : {cardTransferModel.RefrenceNumber}, Card Id: {bankCardsInfo.Id} ");

                            var cardTransferUpdated = db.CardTransfer.FirstOrDefault(x => x.Id == cardTransferModel.Id);
                            if (cardTransferUpdated != null)
                                try
                                {
                                    Logger.Info("Update Card Transfer Item Status In CardTransferService To 00");
                                    cardTransferUpdated.Status = "00";
                                    var eghtesadNovinBank = destinationPan.Substring(0, 6);
                                    var cardtransferCommission = eghtesadNovinBank != "627412" ? GetCommission(cardTransferModel.Amount) : 0;
                                    var cardtransferCommissionPlusCardTransferAmount = cardTransferModel.Amount + cardtransferCommission;
                                    var cardBalance = bankCardsInfo.Balance - cardtransferCommissionPlusCardTransferAmount;
                                    var bankCardInfoBalanceUpdate = db.CardsInfo.FirstOrDefault(x => x.Id == bankCardsInfo.Id);
                                    bankCardInfoBalanceUpdate.Balance = cardBalance;
                                    cardTransferUpdated.Rrn = resps[1];
                                    cardTransferUpdated.TransactionDateTime = DateTime.Now;
                                    db.SaveChanges();
                                    Logger.Info($"Card with card number: {GetMaskedPan(CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", ""))} and refrence number: {cardTransferModel.RefrenceNumber} Setteled, Card Id: {bankCardsInfo.Id} ");
                                } catch (Exception ex)
                                {
                                    Logger.Error($"Error in update bank card info balance,description: {ex.Message}");
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

                                    #region اگر هنگام اصلاح باقیمانده کارت به خطا خوردیم مقدار باقیمانده را صفر می کنیم ، اگر در هنگام صفر کردن باقیمانده کارت نیز به خطا خوردیم ، یک خطا تولید می کنیم و لاگ می زنیم که مقدار باقیمانده کارت را دستی به صفر اصلاح کنید

                                    try
                                    {
                                        bankCardsInfo.Balance = 0;
                                        cardTransferUpdated.Rrn = resps[1];
                                        cardTransferUpdated.TransactionDateTime = DateTime.Now;
                                        db.SaveChanges();
                                        Logger.Info($"Card with card number: {GetMaskedPan(CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", ""))} and refrence number: {cardTransferModel.RefrenceNumber} Setteled, Card Id: {bankCardsInfo.Id}");
                                    } catch (Exception exception)
                                    {
                                        Logger.Error($"Card Id: {bankCardsInfo.Id} Balance wasn't Updated. Update it to 0 manually,Description: {exception.Message}");
                                        if (exception.InnerException != null)
                                        {
                                            Logger.Fatal($"Inner Exception: {exception.InnerException.Message}");
                                            if (exception.InnerException.InnerException != null)
                                            {
                                                Logger.Fatal($"Inner Exception: {exception.InnerException.InnerException.Message}");
                                                if (exception.InnerException.InnerException.InnerException != null)
                                                    Logger.Fatal($"Inner Exception: {exception.InnerException.InnerException.InnerException.Message}");
                                            }
                                        }
                                    }

                                    #endregion
                                }

                            result = true;
                            break;
                        }
                        case "78":
                        {
                            long refundReconcilationId;
                            try
                            {
                                #region بروزرسانی کد پاسخ به 02 یعنی دارای مغایرت

                                Logging.Log("Card Transfer", cardTransferModel.RefrenceNumber.ToString(), "-", cardTransferModel.Amount.ToString(), GetMaskedPan(CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", "")), $"Completed card transfer with error for refund refrence number : {cardTransferModel.RefrenceNumber},Desacription: {resps[0]}|{resps[1]}, Card Id: {bankCardsInfo.Id} ", LogType.Error.ToString(), "RefundSettelment");
                                Logger.Error($"Card with card number: {GetMaskedPan(CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", ""))} and refrence number: {cardTransferModel.RefrenceNumber} completed with error: {resps[0]}|{resps[1]}, Card Id: {bankCardsInfo.Id} ");
                                Logger.Info("Update Card Transfer Item Status In CardTransferService To 02 In Response Code 78");
                                var cardTransferUpdate = db.CardTransfer.FirstOrDefault(x => x.Id == cardTransferModel.Id);
                                cardTransferUpdate.Status = "02";
                                db.SaveChanges();

                                #endregion

                                #region اضافه کردن درخواست به جدول RefundRequestReconcilation

                                // در صورتی که عملیات کارت به کارت موفق نبود یا به خطا خورد
                                // جهت مغایرت گیری به جدول مغایرت ها اضافه می گردد
                                // هم اصل مبلغ تراکنش کارت به کارت و هم تراکنش جابجایی کارمزد گرفته شده از دیجی کالا                                     
                                try
                                {
                                    #region اضافه کردن درخواست به جدول RefundRequestReconcilation

                                    var refundRequestModel = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == cardTransferModel.RefundRefrenceNumber);

                                    var eghtesadNovinBank = destinationPan.Substring(0, 6);
                                    long reverseCommission = 0;
                                    if (eghtesadNovinBank != "627412")
                                    {
                                        reverseCommission = GetCommission(cardTransferModel.Amount);
                                    }

                                    var reverseAmount = refundRequestModel.TransactionAmount + reverseCommission;

                                    var refundRequestReconciliation = new RefundRequestReconciliation
                                    {
                                        RefrenceNumber = refundRequestModel.RefrenceNumber,
                                        EncryptedPan = refundRequestModel.EncryptedPan,
                                        CardTransferEncryptedSourcePan = cardTransferModel.EncryptedSourcePan,
                                        InsertDateTime = DateTime.Now,
                                        ReverseAmount = reverseAmount,
                                        ReverseStatus = "02",
                                        ReverseDateTime = null,
                                        ReverseRrn = string.Empty
                                    };
                                    db.RefundRequestReconciliation.Add(refundRequestReconciliation);
                                    db.SaveChanges();
                                    refundReconcilationId = refundRequestReconciliation.Id;

                                    #endregion

                                    #region برگرداندن اصل مبلغ و کارمزد تراکنش

                                    var refundRequestUserInfo = db.WebServiceUser.FirstOrDefault(x => x.UserName == refundRequestModel.UserName);

                                    try
                                    {
                                        Logger.Info($"send reverse amount: {reverseAmount} for user: {refundRequestModel.UserName} to yaghut card transfer web service method");
                                        var reverseCardNumber = CryptorEngine.Decryption(refundRequestUserInfo.ReverseCardNumber, PrivateKey);
                                        var response = cardTransferService.CardTransfer(
                                            cardsInfoModel.Pan,
                                            cardsInfoModel.Pin2,
                                            cardsInfoModel.Cvv2,
                                            cardsInfoModel.ExpDate,
                                            reverseCardNumber,
                                            (decimal) reverseAmount
                                        );
                                        if (response != null)
                                        {
                                            resps = response.Split('|');
                                            Logger.Info($"refund request amount: {reverseAmount} reverse to user: {refundRequestModel.UserName} with to card number {GetMaskedPan(refundRequestUserInfo.ReverseCardNumber)} response: {response}");
                                            if (resps[0] == "00")
                                            {
                                                var transactionAmountUpdateStatus = db.RefundRequestReconciliation.FirstOrDefault(x => x.Id == refundReconcilationId);
                                                transactionAmountUpdateStatus.ReverseStatus = "00";
                                                transactionAmountUpdateStatus.ReverseRrn = resps[1];
                                                transactionAmountUpdateStatus.ReverseDateTime = DateTime.Now;
                                                db.SaveChanges();
                                                Logger.Error($"Reversing the refund request amount: {reverseAmount} for user: {refundRequestModel.UserName} with card number {GetMaskedPan(refundRequestUserInfo.ReverseCardNumber)} has completed successfully, response: {response}");
                                            }
                                        } else
                                        {
                                            Logger.Error($"In reversing the refund request amount: {reverseAmount} for user: {refundRequestModel.UserName} with card number {GetMaskedPan(refundRequestUserInfo.ReverseCardNumber)} error occurred, response: {response}");
                                        }
                                    } catch (Exception ex)
                                    {
                                        resps[0] = "02";
                                        resps[1] = $"Error reversing the refund request amount: {reverseAmount} for user: {refundRequestModel.UserName} with card number {GetMaskedPan(refundRequestUserInfo.ReverseCardNumber)}";
                                        Logger.Error($"Error reversing the refund request amount: {reverseAmount} for user: {refundRequestModel.UserName} with card number {GetMaskedPan(refundRequestUserInfo.ReverseCardNumber)},description: {ex.Message}");
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

                                    #endregion

                                } catch (Exception exception)
                                {
                                    Logger.Error($"Error in add refund request to reconciliation table, please check,refund refrence number : {cardTransferModel.RefrenceNumber}, Desacription: {exception.Message}, Card Id: {bankCardsInfo.Id}");
                                    if (exception.InnerException != null)
                                    {
                                        Logger.Fatal($"Inner Exception: {exception.InnerException.Message}");
                                        if (exception.InnerException.InnerException != null)
                                        {
                                            Logger.Fatal($"Inner Exception: {exception.InnerException.InnerException.Message}");
                                            if (exception.InnerException.InnerException.InnerException != null)
                                                Logger.Fatal($"Inner Exception: {exception.InnerException.InnerException.InnerException.Message}");
                                        }
                                    }
                                }

                                #endregion
                            } catch (Exception ex)
                            {
                                Logger.Error($"Error in update refund request and card transfer items status to 02,please check,refund refrence number : {cardTransferModel.RefrenceNumber},Desacription: {ex.Message}, Card Id: {bankCardsInfo.Id} ");
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

                            result = false;
                            break;
                        }
                        case "55":
                        {
                            long refundReconcilationId;
                            try
                            {
                                #region بروزرسانی کد پاسخ به 02 یعنی دارای مغایرت

                                Logging.Log("Card Transfer", cardTransferModel.RefrenceNumber.ToString(), "-", cardTransferModel.Amount.ToString(), GetMaskedPan(CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", "")), $"Completed card transfer with error for refund refrence number : {cardTransferModel.RefrenceNumber},Desacription: {resps[0]}|{resps[1]}, Card Id: {bankCardsInfo.Id} ", LogType.Error.ToString(), "RefundSettelment");
                                Logger.Error($"Card with card number: {GetMaskedPan(CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", ""))} and refrence number: {cardTransferModel.RefrenceNumber} completed with error: {resps[0]}|{resps[1]}, Card Id: {bankCardsInfo.Id} ");
                                Logger.Info("Update Card Transfer Item Status In CardTransferService To 02 In Response Code 55");
                                var cardTransferUpdate = db.CardTransfer.FirstOrDefault(x => x.Id == cardTransferModel.Id);
                                cardTransferUpdate.Status = "02";
                                db.SaveChanges();

                                #endregion

                                #region اضافه کردن درخواست به جدول RefundRequestReconcilation

                                // در صورتی که عملیات کارت به کارت موفق نبود یا به خطا خورد
                                // جهت مغایرت گیری به جدول مغایرت ها اضافه می گردد
                                // هم اصل مبلغ تراکنش کارت به کارت و هم تراکنش جابجایی کارمزد گرفته شده از دیجی کالا                                     
                                try
                                {
                                    #region اضافه کردن درخواست به جدول RefundRequestReconcilation

                                    var refundRequestModel = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == cardTransferModel.RefundRefrenceNumber);

                                    var eghtesadNovinBank = destinationPan.Substring(0, 6);
                                    long reverseCommission = 0;
                                    if (eghtesadNovinBank != "627412")
                                    {
                                        reverseCommission = GetCommission(cardTransferModel.Amount);
                                    }

                                    var reverseAmount = refundRequestModel.TransactionAmount + reverseCommission;

                                    var refundRequestReconciliation = new RefundRequestReconciliation
                                    {
                                        RefrenceNumber = refundRequestModel.RefrenceNumber,
                                        EncryptedPan = refundRequestModel.EncryptedPan,
                                        CardTransferEncryptedSourcePan = cardTransferModel.EncryptedSourcePan,
                                        InsertDateTime = DateTime.Now,
                                        ReverseAmount = reverseAmount,
                                        ReverseStatus = "02",
                                        ReverseDateTime = null,
                                        ReverseRrn = string.Empty
                                    };
                                    db.RefundRequestReconciliation.Add(refundRequestReconciliation);
                                    db.SaveChanges();
                                    refundReconcilationId = refundRequestReconciliation.Id;

                                    #endregion

                                    #region برگرداندن اصل مبلغ و کارمزد تراکنش

                                    var refundRequestUserInfo = db.WebServiceUser.FirstOrDefault(x => x.UserName == refundRequestModel.UserName);
                                    var reverseCardNumber = CryptorEngine.Decryption(refundRequestUserInfo.ReverseCardNumber, PrivateKey);
                                    try
                                    {
                                        Logger.Info($"send reverse amount: {reverseAmount} for user: {refundRequestModel.UserName} to yaghut card transfer web service method");
                                        var response = cardTransferService.CardTransfer(
                                            cardsInfoModel.Pan,
                                            cardsInfoModel.Pin2,
                                            cardsInfoModel.Cvv2,
                                            cardsInfoModel.ExpDate,
                                            reverseCardNumber,
                                            (decimal) reverseAmount
                                        );
                                        if (response != null)
                                        {
                                            resps = response.Split('|');
                                            Logger.Info($"refund request amount: {reverseAmount} reverse to user: {refundRequestModel.UserName} with to card number {GetMaskedPan(refundRequestUserInfo.ReverseCardNumber)} response: {response}");
                                            if (resps[0] == "00")
                                            {
                                                var transactionAmountUpdateStatus = db.RefundRequestReconciliation.FirstOrDefault(x => x.Id == refundReconcilationId);
                                                transactionAmountUpdateStatus.ReverseStatus = "00";
                                                transactionAmountUpdateStatus.ReverseRrn = resps[1];
                                                transactionAmountUpdateStatus.ReverseDateTime = DateTime.Now;
                                                db.SaveChanges();
                                                Logger.Error($"Reversing the refund request amount: {reverseAmount} for user: {refundRequestModel.UserName} with card number {GetMaskedPan(refundRequestUserInfo.ReverseCardNumber)} has completed successfully, response: {response}");
                                            }
                                        } else
                                        {
                                            Logger.Error($"In reversing the refund request amount: {reverseAmount} for user: {refundRequestModel.UserName} with card number {GetMaskedPan(refundRequestUserInfo.ReverseCardNumber)} error occurred, response: {response}");
                                        }
                                    } catch (Exception ex)
                                    {
                                        resps[0] = "02";
                                        resps[1] = $"Error reversing the refund request amount: {reverseAmount} for user: {refundRequestModel.UserName} with card number {GetMaskedPan(refundRequestUserInfo.ReverseCardNumber)}";
                                        Logger.Error($"Error reversing the refund request amount: {reverseAmount} for user: {refundRequestModel.UserName} with card number {GetMaskedPan(refundRequestUserInfo.ReverseCardNumber)},description: {ex.Message}");
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

                                    #endregion

                                } catch (Exception exception)
                                {
                                    Logger.Error($"Error in add refund request to reconciliation table, please check,refund refrence number : {cardTransferModel.RefrenceNumber}, Desacription: {exception.Message}, Card Id: {bankCardsInfo.Id}");
                                    if (exception.InnerException != null)
                                    {
                                        Logger.Fatal($"Inner Exception: {exception.InnerException.Message}");
                                        if (exception.InnerException.InnerException != null)
                                        {
                                            Logger.Fatal($"Inner Exception: {exception.InnerException.InnerException.Message}");
                                            if (exception.InnerException.InnerException.InnerException != null)
                                                Logger.Fatal($"Inner Exception: {exception.InnerException.InnerException.InnerException.Message}");
                                        }
                                    }
                                }

                                #endregion
                            } catch (Exception ex)
                            {
                                Logger.Error($"Error in update refund request and card transfer items status to 02,please check,refund refrence number : {cardTransferModel.RefrenceNumber},Desacription: {ex.Message}, Card Id: {bankCardsInfo.Id} ");
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

                            result = false;
                            break;
                        }
                        case "13":
                        {
                            try
                            {
                                Logging.Log("Card Transfer", cardTransferModel.RefrenceNumber.ToString(), "-", cardTransferModel.Amount.ToString(), GetMaskedPan(CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", "")), $"Completed card transfer with error for refund refrence number : {cardTransferModel.RefrenceNumber},Desacription: {resps[0]}|{resps[1]}, Card Id: {bankCardsInfo.Id} ", LogType.Error.ToString(), "RefundSettelment");
                                Logger.Error($"Card with card number: {GetMaskedPan(CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", ""))} and refrence number: {cardTransferModel.RefrenceNumber} completed with error: {resps[0]}|{resps[1]}, Card Id: {bankCardsInfo.Id} ");
                                var bankCardInfoBalanceUpdate = db.CardsInfo.FirstOrDefault(x => x.Id == bankCardsInfo.Id);
                                bankCardInfoBalanceUpdate.Balance = 0;
                                var cardTransferUpdated = db.CardTransfer.FirstOrDefault(x => x.Id == cardTransferModel.Id);
                                Logger.Info("Update Card Transfer Item Status In CardTransferService To -1 In Response Code 13");
                                if (cardTransferUpdated != null) cardTransferUpdated.Status = "-1";
                                db.SaveChanges();
                            } catch (Exception ex)
                            {
                                Logger.Error($"Card Id: {bankCardsInfo.Id} Balance wasn't Updated. Update it to 0 manually,Description: {ex.Message}, refund refrence number: {cardTransferModel.RefrenceNumber}");
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

                            result = false;
                            break;
                        }
                        default:
                        {
                            Logging.Log("Card Transfer", cardTransferModel.RefrenceNumber.ToString(), "-", cardTransferModel.Amount.ToString(), GetMaskedPan(CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", "")), $"Completed card transfer with error for refund refrence number : {cardTransferModel.RefrenceNumber},Desacription: {resps[0]}|{resps[1]}, Card Id: {bankCardsInfo.Id} ", LogType.Error.ToString(), "RefundSettelment");
                            var cardTransferUpdated = db.CardTransfer.FirstOrDefault(x => x.Id == cardTransferModel.Id);
                            if (cardTransferUpdated != null)
                            {
                                cardTransferUpdated.Status = "-1";
                                db.SaveChanges();
                            }

                            Logger.Info("Update Card Transfer Item Status In CardTransferService To -1 When Can't Finish Card Transfer Item");
                            Logger.Error($"Card with card number: {GetMaskedPan(CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", ""))} and refrence number: {cardTransferModel.RefrenceNumber} completed with error: {resps[0]}|{resps[1]}, Card Id: {bankCardsInfo.Id} ");

                            result = false;
                            break;
                        }
                    }

                    #endregion
                }
            } catch (Exception ex)
            {
                using (var db = new PNA_RefundServiceEntities())
                {
                    var cardTransferUpdated = db.CardTransfer.FirstOrDefault(x => x.Id == cardTransferModel.Id);
                    if (cardTransferUpdated != null)          
                    {
                        cardTransferUpdated.Status = "-1";
                        db.SaveChanges();
                    }
                }

                Logger.Info("Update card transfer item status to -1 in CardTransferService catch block");
                var description = ex.InnerException != null ? $"{ex.Message} -- {ex.InnerException.Message}" : ex.Message;
                Logger.Fatal($"In card transfer for user: {cardTransferModel.UserName} with card number: {GetMaskedPan(CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", ""))} and refrence number: {cardTransferModel.RefrenceNumber} error occurred: {description}, Card Id: {bankCardsInfo.Id} ");
                throw new Exception(description);
            }

            return result;
        }

        public static CardsInfo GetCardInfo(long amount,string userName, out int bankCardInfoId)
        {
            bankCardInfoId = 0;
            try
            {
                using (var db = new PNA_RefundServiceEntities())
                {
                    CardsInfo bankCardsInfo;

                    var userInfo = db.WebServiceUser.FirstOrDefault(x => x.UserName == userName);
                    if (amount == 30000000)
                    {
                        //اولین کارت با باقیمانده 3 میلیون تومان انتخاب می شود‌
                        bankCardsInfo = db.CardsInfo.FirstOrDefault(x => x.Balance == 30000000 && x.UserId == userInfo.UserId && x.IsUsing == false);
                        if (bankCardsInfo != null) bankCardInfoId = bankCardsInfo.Id;
                    } else
                    {
                        /*
                            برای بدست آوردن اطلاعات کارت  مورد نظر برای عملیات کارت به کارت
                            1-کارت هایی که باقیمانده آن ها بزرگتر مساوی با مبلغ قابل انتقال هست را برمی گردانیم
                            2-سپس لیست کارتهای به دست آمده رو به صورتی صعودی مرتب می کنیم
                            3-اولین آیتم لیست را به عنوان کارت برای عملیات کارت به کارت برمی داریم
                            در این الگوریتم سعی شده بهینه ترین استفاده از کارتها شود تا نزدیکترین کارت با مبلغ باقیمانده به مبلغ قابل انتقال انتخاب شود 
                            */
                        var bankCardsInfoList = db.CardsInfo.Where(x => x.Balance >= amount && x.UserId == userInfo.UserId && x.IsUsing == false).ToList();
                        bankCardsInfoList = new List<CardsInfo>(bankCardsInfoList.OrderBy(x => x.Balance));
                        bankCardsInfo = bankCardsInfoList.FirstOrDefault();
                        if (bankCardsInfo != null) bankCardInfoId = bankCardsInfo.Id;
                    }

                    return bankCardsInfo;
                }
            } catch (Exception ex)
            {
                var description = ex.InnerException != null ? $"{ex.Message} -- {ex.InnerException.Message}" : ex.Message;
                Logger.Fatal($"Error in get card info,description: {description}");
                return null;
            }
        }

        public static CardsInfo GetCardInfoForCardTransferRetry(long amount, int currentCardInfoId)
        {
            try
            {
                using (var db = new PNA_RefundServiceEntities())
                {
                    CardsInfo bankCardsInfo;

                    if (amount == 30000000)
                    {
                        //اولین کارت با باقیمانده 3 میلیون تومان انتخاب می شود‌
                        bankCardsInfo = db.CardsInfo.FirstOrDefault(x => x.Balance == 30000000 && x.IsUsing == false && x.Id != currentCardInfoId);
                    } else
                    {
                        /*
                            برای بدست آوردن اطلاعات کارت  مورد نظر برای عملیات کارت به کارت
                            1-کارت هایی که باقیمانده آن ها بزرگتر مساوی با مبلغ قابل انتقال هست را برمی گردانیم
                            2-سپس لیست کارتهای به دست آمده رو به صورتی صعودی مرتب می کنیم
                            3-اولین آیتم لیست را به عنوان کارت برای عملیات کارت به کارت برمی داریم
                            در این الگوریتم سعی شده بهینه ترین استفاده از کارتها شود تا نزدیکترین کارت با مبلغ باقیمانده به مبلغ قابل انتقال انتخاب شود 
                            */
                        var bankCardsInfoList = db.CardsInfo.Where(x => x.Balance >= amount && x.IsUsing == false && x.Id != currentCardInfoId).ToList();
                        bankCardsInfoList = new List<CardsInfo>(bankCardsInfoList.OrderBy(x => x.Balance));
                        bankCardsInfo = bankCardsInfoList.FirstOrDefault();
                    }

                    return bankCardsInfo;
                }
            } catch (Exception ex)
            {
                var description = ex.InnerException != null ? $"{ex.Message} -- {ex.InnerException.Message}" : ex.Message;
                Logger.Fatal($"Error in get card info,description: {description}");
                return null;
            }
        }

        public static string CardTransferRetry(int currentCardInfoId, CardTransferModel cardTransferModel, string responseCode, out bool secondCardTransferRetry, out int nextBankCardInfoId)
        {
            var cardTransferService = new Yaghut_MobileApp();
            string response = null;
            secondCardTransferRetry = false;
            nextBankCardInfoId = 0;

            using (var db = new PNA_RefundServiceEntities())
            {
                #region انتخاب کارت

                var bankCardsInfo = GetCardInfoForCardTransferRetry(cardTransferModel.Amount, currentCardInfoId);

                #endregion

                if (bankCardsInfo != null)
                {
                    Logger.Info($"Card With Card Id: {bankCardsInfo.Id} Selected To Card Transfer Second Retry In Response: {responseCode}");

                    #region " IsUsing = true "برزورسانی وضعیت کارت به درحال استفاده      

                    var bankCardsInfoUpdateTrue = db.CardsInfo.FirstOrDefault(x => x.Id == bankCardsInfo.Id);
                    if (bankCardsInfoUpdateTrue != null)
                    {
                        bankCardsInfoUpdateTrue.IsUsing = true;
                        db.SaveChanges();
                    }

                    #endregion

                    #region برگرداندن اطلاعات رمز شده کارت های مبدا برای عملیات کارت به کارت

                    string cardInfoDecrpted = null;
                    try
                    {
                        cardInfoDecrpted = CryptorEngine.Decryption(bankCardsInfo.EncryptedData, PrivateKey);
                    } catch (Exception ex)
                    {
                        Logger.Error($"Error in decryption bank card info,description: {ex.Message}");
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

                    #endregion

                    #region نگاشت اطلاعات بازیابی شده کارت به مدل کارت

                    var cardsInfoModel = new CardsInfoModel();
                    try
                    {
                        if (cardInfoDecrpted != null)
                        {
                            nextBankCardInfoId = bankCardsInfo.Id;
                            cardsInfoModel.Id = bankCardsInfo.Id.ToString();
                            cardsInfoModel.Pan = cardInfoDecrpted.Split('|')[0];
                            cardsInfoModel.ExpDate = cardInfoDecrpted.Split('|')[1];
                            cardsInfoModel.Cvv2 = cardInfoDecrpted.Split('|')[2];
                            cardsInfoModel.Pin2 = cardInfoDecrpted.Split('|')[3];
                            cardsInfoModel.Balance = bankCardsInfo.Balance;

                            Logger.Info($"Card Info --> Pan: {GetMaskedPan(cardsInfoModel.Pan)} and ExpDate: {cardsInfoModel.ExpDate} CVV2: {cardsInfoModel.Cvv2.Length} and PIN: {cardsInfoModel.Pin2.Length}");
                        }
                    } catch (Exception ex)
                    {
                        Logger.Error($"Error in mapping decrpted card info to card info model,description: {ex.Message}");
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

                    #endregion

                    #region کارت مبدا را براي عمليات هاي مغايرت گيري احتمالي بعدي ذخيره مي کند

                    try
                    {
                        cardTransferModel.EncryptedSourcePan = CryptorEngine.Encryption(cardsInfoModel.Pan, PublicKey);
                        db.SaveChanges();
                    } catch (Exception ex)
                    {
                        Logger.Error($"Error in encryption cards info pan,description: {ex.Message}");
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

                    #endregion

                    #region عمليات کارت به کارت

                    var resps = new[] {responseCode, $"Error In Second Retry Of Card Transfer In Response Code {responseCode}"};
                    try
                    {
                        var destinationPan = CryptorEngine.Decryption(cardTransferModel.EncryptedPan, PrivateKey).Replace("-", "");

                        var checkCardTransferAmount = db.CardTransfer.Where(x => x.RefundRefrenceNumber == cardTransferModel.RefundRefrenceNumber && x.Status == "00").ToList();
                        var sumAmount = checkCardTransferAmount.Sum(x => x.Amount);
                        var refundRequestModel = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == cardTransferModel.RefundRefrenceNumber);
                        var checkAmount = sumAmount + cardTransferModel.Amount;
                        if (checkAmount <= refundRequestModel?.TransactionAmount)
                        {
                            var cardTransferRetryCountCheck = db.CardTransfer.FirstOrDefault(x => x.Id == cardTransferModel.Id);
                            if (cardTransferRetryCountCheck != null && cardTransferRetryCountCheck.RetryCount < 10)
                            {
                                cardTransferRetryCountCheck.RetryCount++;
                                response = cardTransferService.CardTransfer(
                                    cardsInfoModel.Pan,
                                    cardsInfoModel.Pin2,
                                    cardsInfoModel.Cvv2,
                                    cardsInfoModel.ExpDate,
                                    destinationPan,
                                    cardTransferModel.Amount
                                );
                                if (response != null)
                                    resps = response.Split('|');
                                if (resps[0] == "00") secondCardTransferRetry = true;

                                Logger.Info($"Card Transfer To Card Number {GetMaskedPan(destinationPan)} In Second Retry Of Card Transfer In Response Code {responseCode}, Response: {response},Card Id: {cardsInfoModel.Id}");
                            } else
                            {
                                resps[0] = "78";
                                resps[1] = "retry time exceeded";
                            }
                        } else
                        {
                            Logger.Error($"Warning In Second Retry Of Card Transfer In Response Code {responseCode},description: card transfer has been settled before, for card number: {GetMaskedPan(destinationPan)}");
                        }
                    } catch (Exception ex)
                    {
                        resps[0] = "0990";
                        resps[1] = $"Error In Second Retry Of Card Transfer In Response Code {responseCode}";
                        Logger.Error($"Error In Second Retry Of Card Transfer In Response Code {responseCode},description: {ex.Message}");
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

                    #endregion
                } else
                {
                    return $"{responseCode}|Card Not Found In Second Retry Of Card Transfer In Response Code {responseCode}";
                }
            }

            return response;
        }

        public static long GenerateUniqueId()
        {
            long unixTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            return unixTimestamp;
        }

        public static string DecryptIban(string encryptIban)
        {
            return CryptorEngine.Decryption(encryptIban, PrivateKey);
        }
    }
}