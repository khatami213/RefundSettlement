using Logging;
using RefundSettelment.ExternalServices.JibitServices.Models;
using RefundSettelment.ExternalServices.JibitServices;
using RefundSettelment.Helper_Code;
using RefundSettelment.Model;
using RefundSettelment.Model.RefundModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using System.Threading;
using RefundSettelment.Enums;

namespace RefundSettelment
{
    public partial class RefundSettelment : ServiceBase
    {
        //lock 
        private static readonly object LockObject = new object();
        private static bool _inProgress;

        //logger
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(RefundSettelment));

        //Timer 
        private System.Timers.Timer _mainTimer;
        private bool _timerTaskSuccess;

        public RefundSettelment()
        {
            InitializeComponent();
            //RefundSettelmentLog = new EventLog();
            //if (!EventLog.SourceExists("RefundSettelmentLogSource"))
            //    EventLog.CreateEventSource(
            //        "RefundSettelmentLogSource", "RefundSettelmentLog");
            RefundSettelmentLog.Source = "RefundSettelmentLogSource";
            RefundSettelmentLog.Log = "RefundSettelmentLog";
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Logger.Info("-------------------------------------------------------------------------------------------------------------------------------------------------------");
                Logger.Info("Refund Settelment Started");
                var schedulerInterval = ConfigurationManager.AppSettings["SchedulerInterval"];
                _mainTimer = new System.Timers.Timer { Interval = Convert.ToDouble(schedulerInterval) };
                _mainTimer.Elapsed += Timer_Elapsed;
                _mainTimer.AutoReset = false;
                _mainTimer.Start();
                _timerTaskSuccess = false;
            }
            catch (Exception ex)
            {
                RefundSettelmentLog.WriteEntry("Start Service - " + ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                Logger.Info("Refund Settelment Stopped");
                Logger.Info("-------------------------------------------------------------------------------------------------------------------------------------------------------");
                RefundSettelmentLog.WriteEntry("Refund Settelment Stopped");
                _mainTimer.Stop();
                _mainTimer.Dispose();
                _mainTimer = null;
            }
            catch (Exception ex)
            {
                RefundSettelmentLog.WriteEntry($"Stop Service - {ex.Message}");
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_inProgress) return;
            lock (LockObject)
            {
                try
                {
                    _inProgress = true;
                    Logger.Info("-------------------------------------------------------------------------------------------------------------------------------------------------------");
                    Logger.Info("Refund Settelment Scheduler Started");
                    RefundSettelmentService();
                    Thread.Sleep(1000);
                    ProviderRefundSettlementService();
                    Thread.Sleep(1000);
                    Logger.Info("Refund Settelment Scheduler Ended");
                    Logger.Info("-------------------------------------------------------------------------------------------------------------------------------------------------------");
                    _timerTaskSuccess = true;
                }
                catch (Exception ex)
                {
                    RefundSettelmentLog.WriteEntry($"Scheduler - {ex.Message}");
                    _timerTaskSuccess = false;
                }
                finally
                {
                    if (_timerTaskSuccess)
                        _mainTimer.Start();
                    _inProgress = false;
                }
            }
        }

        public void RefundSettelmentService()
        {
            var isCardNotFound = false;
            try
            {
                using (var db = new PNA_RefundServiceEntities())
                {
                    //var bankCardsInfo1 = Helpers.GetCardInfo(20000, "ansar", out var bankCardInfoId2);

                    var cardTransfer = db.Database.SqlQuery<CardTransferModel>("select * from CardTransfer join RefundRequest on CardTransfer.RefundRefrenceNumber = RefundRequest.RefrenceNumber where RefundStatus ='01' and CardTransfer.Status ='-1'").ToList();

                    Logger.Info($"cardTransfer Count:{cardTransfer?.Count}");

                    foreach (var item in cardTransfer)
                    {
                        #region بدست آوردن اطلاعات مربوط به ریفاند و کارت به کارت ها و بروزرسانی فیلد وضعیت آن ها

                        var refundRequest = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                        if (refundRequest != null)
                        {
                            Logger.Info("Update Refund and Card Transfer Status In RefundSettelmentService To -99 and -2");
                            refundRequest.RefundStatus = "-99"; //یعنی عملیات ریفاند در حال انجام است
                            var cardTransferUpdate = db.CardTransfer.FirstOrDefault(x => x.Id == item.Id);
                            if (cardTransferUpdate != null)
                                cardTransferUpdate.Status = "-2"; //یعنی در حال انجام عملیات کارت به کارت می باشد
                            db.SaveChanges();
                        }

                        #endregion

                        #region انتخاب کارت

                        Logger.Info("انتخاب کارت");
                        var bankCardsInfo = Helpers.GetCardInfo(item.Amount, item.UserName, out var bankCardInfoId);
                        //Logger.Info($"bankCardsInfo:{JsonConvert.SerializeObject(bankCardsInfo)} - bankCardInfoId:{bankCardInfoId}");
                        Logger.Info($" bankCardInfoId:{bankCardInfoId} - Balance:{bankCardsInfo.Balance}");
                        #endregion

                        #region انجام عملیات کارت به کارت

                        if (bankCardsInfo != null)
                            try
                            {
                                var bankCardsInfoUpdateTrue = db.CardsInfo.FirstOrDefault(x => x.Id == bankCardInfoId);
                                if (bankCardsInfoUpdateTrue != null) bankCardsInfoUpdateTrue.IsUsing = true;
                                db.SaveChanges();
                                Helpers.CardTransfer(bankCardsInfo, item, out var bankCardInfoIdForUpdate);
                                var bankCardsInfoUpdateFalse = db.CardsInfo.FirstOrDefault(x => x.Id == bankCardInfoIdForUpdate);
                                if (bankCardsInfoUpdateFalse != null) bankCardsInfoUpdateFalse.IsUsing = false;
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex.Message);
                                var bankCardsInfoUpdateFalse = db.CardsInfo.FirstOrDefault(x => x.Id == bankCardInfoId);
                                if (bankCardsInfoUpdateFalse != null) bankCardsInfoUpdateFalse.IsUsing = false;
                                db.SaveChanges();
                            }
                        else
                            try
                            {
                                Logger.Error($"Card Not Found For Card Transfer RefrenceNumber:{item.RefrenceNumber}");
                                isCardNotFound = true;
                                var refundRequestCardNotFound = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                if (refundRequestCardNotFound != null)
                                {
                                    Logger.Info("Update Card Transfer Status In RefundSettelmentService When Card Not Found To -1");
                                    refundRequestCardNotFound.RefundStatus = "01"; //یعنی عملیات موفق/تسویه ناقص
                                    var cardTransferUpdate = db.CardTransfer.FirstOrDefault(x => x.Id == item.Id);
                                    if (cardTransferUpdate != null)
                                        cardTransferUpdate.Status = "-1"; //یعنی منتظر عملیات تسویه
                                    db.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                isCardNotFound = true;
                                var description = ex.InnerException != null ? $"{ex.Message} -- {ex.InnerException.Message}" : ex.Message;
                                Logger.Error($"Error: {description}");
                            }

                        #endregion

                        #region بروز رسانی لیست کارت به کارت ها و فیلد وضعیت ریفاند و کارت به کارت ها با توجه به شرایط مختلف پیش آمده در کارت به کارت

                        if (refundRequest != null && !isCardNotFound)
                        {
                            try
                            {
                                Logger.Info("Update Refund Status In RefundSettelmentService To 01 When Finish Card Transfer");
                                refundRequest.RefundStatus = "01";
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Logger.Info($"Update Refund Status In RefundSettelmentService To 01 When Finish Card Transfer,Description: {ex.Message},Refund Request Refrence Number: {refundRequest.RefrenceNumber}");
                            }
                        }

                        if (!isCardNotFound)
                        {
                            var countRefundCardTransfer = db.CardTransfer.Count(x => x.RefundRefrenceNumber == item.RefrenceNumber);
                            var countRefundCardTransferReconcilation = db.CardTransfer.Count(x => x.RefundRefrenceNumber == item.RefrenceNumber && x.Status == "02");
                            var countRefundCardTransferSettled = db.CardTransfer.Count(x => x.RefundRefrenceNumber == item.RefrenceNumber && x.Status == "00");
                            if (countRefundCardTransfer == countRefundCardTransferReconcilation)
                            {
                                var refundDescriptionUpdate = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                if (refundDescriptionUpdate != null)
                                {
                                    Logger.Info("Update Refund Status In RefundSettelmentService To 02 When Finish Card Transfer And All Card Transfer Has Been 02");
                                    refundDescriptionUpdate.RefundDescription = "دارای مغایرت/خطای اصالت سنجی کارت مقصد";
                                    refundDescriptionUpdate.RefundStatus = "02";
                                }
                                db.SaveChanges();
                            }
                            else if (countRefundCardTransfer == countRefundCardTransferSettled)
                            {
                                var refundDescriptionUpdate = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                if (refundDescriptionUpdate != null)
                                {
                                    Logger.Info("Update Refund Status In RefundSettelmentService To 00 When Finish Card Transfer And All Card Transfer Has Been 00");
                                    refundDescriptionUpdate.RefundDescription = "موفق";
                                    refundDescriptionUpdate.RefundStatus = "00";
                                }
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            Logger.Info($"isCardNotFound:{isCardNotFound}");
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                var description = ex.InnerException != null ? $"{ex.Message} -- {ex.InnerException.Message}" : ex.Message;
                Logger.Fatal($"Error: {description}");
            }
        }

        public void ProviderRefundSettlementService()
        {
            try
            {
                using (var db = new PNA_RefundServiceEntities())
                {
                    var query = "select * from IBANTransfer it join RefundRequest rr on it.RefundRefrenceNumber = rr.RefrenceNumber where rr.RefundStatus = '01' and it.[Status] = '-1'";
                    var ibanTransfer = db.Database.SqlQuery<IBANTransferModel>(query).ToList();

                    var jibitService = new JibitService();

                    foreach (var item in ibanTransfer)
                    {
                        #region بدست آوردن اطلاعات مربوط به ریفاند بروزرسانی فیلد وضعیت آن ها

                        var refundRequest = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                        var providerId = db.WebServiceUser.FirstOrDefault(x => x.UserName == refundRequest.UserName).ProviderId;

                        if (refundRequest == null)
                            continue;

                        var ibanTransferUpdate = db.IBANTransfer.FirstOrDefault(x => x.RefundRefrenceNumber == item.RefrenceNumber);
                        var transferDetailes = db.ProviderTransferDetailes.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);

                        Logger.Info("Update Refund and IBAN Transfer Status In RefundSettelmentService To -99 and -2");
                        refundRequest.RefundStatus = "-99"; //یعنی عملیات ریفاند در حال انجام است
                        if (ibanTransferUpdate != null)
                            ibanTransferUpdate.Status = "-2"; //یعنی در حال انجام عملیات تسویه می باشد
                        db.SaveChanges();

                        #endregion

                        #region استعلام موجودی

                        transferDetailes.Status = typeof(JibitServiceStatusEnum).GetItems().FirstOrDefault(x => x.Id == (int)JibitServiceStatusEnum.GetBalance).Title;
                        db.SaveChanges();

                        var jibitResult = jibitService.GetBalance().Result;
                        var jibitBalance = jibitResult.SettleableBalance;

                        if (jibitResult.Status == false)
                        {
                            if (transferDetailes != null)
                            {
                                transferDetailes.ResponseStatus = jibitResult.Status;
                                transferDetailes.Message = $"{jibitResult.Message}";
                                transferDetailes.ErrorMessage = $"Jibit GetBalance : {jibitResult.Message}";
                                //transferDetailes.Status = typeof(JibitServiceStatusEnum).GetItems().FirstOrDefault(x => x.Id == (int)JibitServiceStatusEnum.Failed).Title;
                                db.SaveChanges();
                                continue;
                            }
                        }
                            
                        if (jibitResult.Status == true && (jibitBalance == null || jibitBalance <= item.TransactionAmount + item.InquiryCommision))
                        {
                            Logger.Info("Update Refund and IBAN Transfer Status To 51 When Jibit Not Enough Balance");
                            refundRequest.RefundStatus = "51"; //یعنی عملیات ریفاند به علت عدم موجودی انجام نشد
                            refundRequest.RefundDescription = "عدم موجودی";
                            if (ibanTransferUpdate != null)
                                ibanTransferUpdate.Status = "51"; //یعنی عملیات تسویه به علت عدم موجودی انجام نشد

                            if (transferDetailes != null)
                            {
                                transferDetailes.Message = "51-NotEnoghBalance";
                                transferDetailes.ErrorMessage = "51-عملیات تسویه به علت عدم موجودی انجام نشد";
                                transferDetailes.Status = typeof(JibitServiceStatusEnum).GetItems().FirstOrDefault(x => x.Id == (int)JibitServiceStatusEnum.Failed).Title;
                            }

                            db.SaveChanges();
                            continue;
                        }

                        #endregion

                        #region ثبت اطلاعات مربوط به انتقال

                        transferDetailes.Status = typeof(JibitServiceStatusEnum).GetItems().FirstOrDefault(x => x.Id == (int)JibitServiceStatusEnum.SettlementInProgress).Title;
                        db.SaveChanges();

                        if (transferDetailes.CardStatus.ToLower() == "BLOCK_WITHOUT_DEPOSIT".ToLower() ||
                            transferDetailes.CardStatus.ToLower() == "UNKNOWN".ToLower())
                        {
                            Logger.Info("Update IBAN Transfer And Refund Status To 57 When Card Is DeActive");
                            refundRequest.RefundStatus = "57"; //یعنی کارت غیر مجاز میباشد
                            refundRequest.RefundDescription = "کارت غیر مجاز";
                            if (ibanTransferUpdate != null)
                            {
                                ibanTransferUpdate.Status = "57"; //یعنی کارت غیر مجاز میباشد
                            }

                            if (transferDetailes != null)
                            {
                                transferDetailes.Message = "57-InActiveCard";
                                transferDetailes.ErrorMessage = "57-کارت غیرفعال است";
                                transferDetailes.Status = typeof(JibitServiceStatusEnum).GetItems().FirstOrDefault(x => x.Id == (int)JibitServiceStatusEnum.Failed).Title;
                            }

                            db.SaveChanges();
                            continue;
                        }

                        var transferId = Helpers.GenerateUniqueId();

                        var transferRequest = new TransferRequestModel()
                        {
                            BatchId = $"{Helpers.GenerateUniqueId()}",
                            SubmissionMode = "TRANSFER",
                            Transfers = new List<TransferInfo>()
                            {
                                new TransferInfo()
                                {
                                    Amount = item.TransactionAmount,
                                    TransferID = transferId,
                                    Destination = Helpers.DecryptIban(item.EncryptedIBAN),
                                    Description = item.Description,
                                }
                            },
                        };

                        var transferResult = jibitService.Transfer(transferRequest).Result;
                        var txnDatetime = DateTime.Now;

                        transferDetailes.TransferId = transferId;
                        transferDetailes.ResponseStatus = transferResult.Status;
                        transferDetailes.Message = transferResult.Message;
                        transferDetailes.TransferAmount = item.TransactionAmount;
                        transferDetailes.TransferCount = 1;
                        db.SaveChanges();

                        if (transferResult.Status == false)
                        {
                            var errorMessage = string.Empty;
                            if (transferResult.Errors != null)
                            {
                                foreach (var error in transferResult.Errors)
                                {
                                    errorMessage += error.Code + "-" + error.Message;
                                }
                            }

                            transferDetailes.Status = typeof(JibitServiceStatusEnum).GetItems().FirstOrDefault(x => x.Id == (int)JibitServiceStatusEnum.Failed).Title;
                            transferDetailes.ErrorMessage = $"{errorMessage}";
                            db.SaveChanges();

                            if (refundRequest != null && transferResult.Message == "rejection" || errorMessage.Contains("موجودی کافی نیست"))
                            {
                                Logger.Info("Update IBAN Transfer And Refund Status To 61 When Transfer Amount Is Not Allowed");
                                refundRequest.RefundStatus = "61"; //یعنی مبلغ انتقال غیر مجاز میباشد
                                refundRequest.RefundDescription = "مبلغ انتقال غیر مجاز";
                                if (ibanTransferUpdate != null)
                                {
                                    ibanTransferUpdate.Status = "61"; //یعنی مبلغ انتقال غیر مجاز میباشد
                                    ibanTransferUpdate.TransferId = transferId;
                                    ibanTransferUpdate.TransactionDateTime = txnDatetime;
                                }

                                if (transferDetailes != null)
                                {
                                    transferDetailes.Message = "61-NotAllowedAmount";
                                    transferDetailes.ErrorMessage = $"61-مبلغ انتقال غیر مجاز است";
                                }

                                db.SaveChanges();
                                continue;
                            }
                        }

                        if (transferResult.Status == true)
                        {
                            transferDetailes.Status = typeof(JibitServiceStatusEnum).GetItems().FirstOrDefault(x => x.Id == (int)JibitServiceStatusEnum.Done).Title;
                            var updateIbanTransfer = db.IBANTransfer.FirstOrDefault(x => x.RefundRefrenceNumber == item.RefrenceNumber);
                            if (updateIbanTransfer != null)
                            {
                                updateIbanTransfer.TransactionDateTime = txnDatetime;
                                updateIbanTransfer.TransferId = transferId;
                                updateIbanTransfer.Status = "00";
                                db.SaveChanges();
                            }

                            if (refundRequest != null)
                            {
                                refundRequest.RefundStatus = "00";
                                refundRequest.RefundDescription = "موفق";
                                refundRequest.TransferDateTime = txnDatetime;
                                refundRequest.TransferRrn = $"{transferId}";
                                db.SaveChanges();
                            }
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                var description = ex.InnerException != null ? $"{ex.Message} -- {ex.InnerException.Message}" : ex.Message;
                Logger.Fatal($"Error: {description}");
            }
        }
    }
}