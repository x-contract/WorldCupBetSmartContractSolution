/**************************************************************************************************************************
 * The X-Contract foundation is a organzation of dedicating on smart contract evolution.
 * This smart contract is for world cup 2018,russia. It can help players bet match with blockchain chips.
 * The brand new blockchain chips named XC Chips. It worthless only for fun.
 * X-Contract website: http://www.x-contract.org
 * Author： Michael Li
 * Date: 29/May/2018
 * ========================================================================================================================
 * Below are defientation of World Cup Russia Bet Smart Contract
 * 
 * 1. uint ApplyChips(byte[] address)
 * This method used for player to apply for WCC Chips by daily. Initial amount is 20000, fixed amount 1000
 * to encourage continusly watch by daily.
 * 
 * 2. bool InputMatchResult(byte[] matchResultList)
 * This method used for administrator input result of soccer matches after a round completed.
 * The string format of match result(a line per record):
 * Match Result:
 * MatchID(FixtureID),2,1
 * 4 bytes      4 bytes:0x00000002   4 bytes:0x00000001
 * ID       |    1st team score    |   2nd team score
 * 
 * 3. PushOddsData(byte[] oddsData)
 * Input formatted binary odds data for players bet.
 * The format of odds raw data(total 30 bytes):
 * MatchID(Fixture),      主队让球,           客队让球,           主队赢,            客队赢,            平局            比赛时间Timestamp       让球      主队让球
 *      4bytes     |       4bytes     |      4bytes     |      4bytes     |      4bytes     |      4bytes     |     4 bytes         |   1 bytes  |   1 byte
 * 4. bool PushOddsList(string oddsList)
 * Input CSV format odds data for players bet.
 * The format of odds list string(a line per record):
 * MatchID(Fixture),比赛时间,主队,客队,主队让球,客队让球,主队赢,客队赢,平局
 * 
 * 5. BalanceOf(byte[] address)
 * Get the balance of specified player address.
 * 
 * 6. string GetOddsList()
 * Retrieve world cup odds information for conversion.
 * 
 * 5. CollectAward(byte[] address)
 * Get bet award to specified player address. 
 * It will affect balance of chips specified address.
 * 
 * 6. byte[] Bet(byte[] address, int fixtureID, int betType, uint amount)
 * Player bets match.
 * address: player address
 * fixtureID: The ID of soccer match
 * betType: 0x01 home team win, 0x02 away team win, 0x03 draw, 0x04 Home win(Concede), 0x05 Away win(Concede)
 * amount: bet chips amount
 * 
 * 7. bool Reset()
 * Clean all odds data and match result.
 * 
 * 8. bool ResetAccount(byte[] address)
 * address: The player address.
 * Clear specified player's account data.
 * 
 * 9. CollectAward(byte[] address)
 * Player awards.
 * address: The address of players.
 * 
 * ========================================================================================================================
 * The structure of byte storage
 * ------------------------------------------------------------------------------------------------------------------------
 * Due to complication of World Cup game, we have to store a lot of data. Below information describe the 
 * serialization of byte array.
 * 
 * 1 byte                   4 bytes uint array        4 bytes uint array             4 bytes uint array           bet record array
 * The length of balance  | The balance amount   |  Last apply chips timestamp   |  The amount of bet records   | The bets record.......
 * 
 * A sample of bet record:
 * 4 bytes           1 byte (Win:0x1,Lose:0x2,Draw:0x3)         1 byte              4 bytes       4 bytes
 * Match ID     |               bet type                  |  is Calculated    |   odds x 1000 |  Win Amount   
 * 
 * MatchResult array(6 bytes):
 * 4 bytes              1 byte              1 byte
 * Match ID     |   TeamA Score     |   TeamB Score
 * 
 * ========================================================================================================================
 * About Concede points(gg and g):
 * ------------------------------------------------------------------------------------------------------------------------
 * fixtureID = 242438, 塞尔维亚:瑞士, gg = 1, 塞爾維亞 [0] 瑞士 [0]
 * fixtureID = 242460, 英格兰:比利时, gg = 2, 英格蘭 [0/+0.5] 比利時 [0/-0.5]
 * fixtureID = 242456, 瑞士:哥斯达黎加, gg = 3, 瑞士 [-0.5] 哥斯達黎加 [+0.5]
 * fixtureID = 242452, 冰岛:克罗地亚, gg = 4, 冰島 [+0.5/+1] 克羅地亞 [-0.5/-1]
 * fixtureID = 242441, 德国:瑞典, gg = 5, 	德國 [-1] 瑞典 [+1]
 * 
 * ========================================================================================================================
 * 
 * 
 * ************************************************************************************************************************/

using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace WorldCupBetSmartContract
{
    public class WorldCupBetSmartContract : SmartContract
    {
        #region --- Constants ---
        /// <summary>
        /// Seconds in a day.
        /// </summary>
        private static readonly uint _secondsOneDay = 86400;
        /// <summary>
        /// The name of chips.
        /// </summary>
        private static readonly string _coinName = "World Cup Coin";

        /// <summary>
        /// The symbol of XC chips.
        /// </summary>
        private static readonly string _coinSymbol = "WCC";

        /// <summary>
        /// 1st Chips issue amount.
        /// </summary>
        private static readonly uint _AccountInitialAmount = 20000;

        /// <summary>
        /// Daily chips issue amount.
        /// </summary>
        private static readonly uint _DailyIssueAmount = 1000;

        /// <summary>
        /// The key of Match result
        /// </summary>
        private static readonly string _keyMatchResult = "MATCHRESULT";

        /// <summary>
        /// The key of Odds list.
        /// </summary>
        private static readonly string _keyOddsList = "ODDSLIST";

        /// <summary>
        /// 
        /// </summary>
        private static readonly string _keyOddsData = "ODDSDATA_WORLDCUP";
        #endregion

        /// <summary>
        /// The Smart Contract Invocation Entry
        /// </summary>
        public static object Main(string method, object[] args)
        {
            if (TriggerType.Application == Runtime.Trigger)
            {
                if ("Name" == method)
                    return Name();
                else if ("Symbol" == method)
                    return Symbol();
                else if ("ApplyChips" == method)
                {
                    if (1 != args.Length)
                        return false;
                    byte[] address = (byte[])args[0];
                    return ApplyChips(address);
                }
                else if ("InputMatchResult" == method)
                    return InputMatchResult((byte[])args[0]);
                else if ("PushOddsList" == method)
                    return PushOddsList((string)args[0]);
                else if ("GetOddsList" == method)
                    return GetOddsList();
                else if ("PushOddsData" == method)
                    return PushOddsData((byte[])args[0]);
                else if ("GetOddsData" == method)
                    return GetOddsData();
                else if ("BalanceOf" == method)
                    return BalanceOf((byte[])args[0]);
                else if ("Bet" == method)
                    return Bet((byte[])args[0], (int)args[1], (int)args[2], (uint)args[3]);
                else if ("Reset" == method)
                    return Reset();
                else if ("ResetAccount" == method)
                    return ResetAccount((byte[])args[0]);
                else if ("CollectAward" == method)
                    return CollectAward((byte[])args[0]);
                else if ("GetBetHistory" == method)
                    return GetBetHistory((byte[])args[0]);

                //  === Below are methods for test. ===
                else if ("GetAccountInfo" == method)
                    return GetAccountInfo((byte[])args[0]);
                else if ("GetOddsLine" == method)
                    return GetOddsLine((int)args[0]);
                else if ("TestAmount" == method)
                    return TestAmount();
                else if ("TestGetTimStamp" == method)
                    return GetTimStamp();
                else if ("GetIntOdds" == method)
                    return GetIntOdds((int)args[0], (int)args[1]);
                else if ("BetRecordArray" == method)
                    return GetBetRecordArray((int)args[0], (int)args[1], (int)args[2], (uint)args[3]);
                else if ("GetMatchID" == method)
                    return GetMatchID((byte[])args[0]);
                else if ("CalcMatchResult" == method)
                    return CalcMatchResult((byte[])args[0]);
                else if ("GetMatchResult" == method)
                    return GetMatchResult();
                else if ("GetOddsData" == method)
                    return GetOddsData();
                else if ("SetCalcaute" == method)
                    return SetCalcaute((byte[])args[0]);
                else if ("GetConcedePoints" == method)
                    return GetConcedePoints((int)args[0], (bool)args[1]);
                else
                    return "UNKNOWN CALL";
            }
            else
                return false;
        }


        #region --- Methods Implementations ---
        public static string Name()
        {
            return _coinName;
        }
        public static string Symbol()
        {
            return _coinSymbol;
        }
        public static uint ApplyChips(byte[] address)
        {
            //if (!Runtime.CheckWitness(address))
            //    return 0;
            byte[] account = Storage.Get(Storage.CurrentContext, address);
            uint nowTimeStamp = Runtime.Time;
            if (_secondsOneDay > nowTimeStamp - GetLastApplyChipsTimeStamp(account))
                return 0;       // Only apply once in one day.

            uint applyAmount = 0;
            uint balance = 0;
            if (account.Length == 0)
            {
                // The account never created.
                account = CreateNewAccountArray();
                applyAmount = _AccountInitialAmount;
            }
            else
            {
                //retrieve balance from bytes array.
                balance = GetBalance(account);
                applyAmount = _DailyIssueAmount;
            }

            // Add amount.
            balance += applyAmount;
            account = UpdateBalanceAmount(account, balance);
            account = SetTimeStamp(account);
            Storage.Put(Storage.CurrentContext, address, account);

            return applyAmount;
        }
        public static byte[] GetAccountInfo(byte[] address)
        {
            return Storage.Get(Storage.CurrentContext, address);
        }
        public static uint BalanceOf(byte[] address)
        {
            byte[] account = Storage.Get(Storage.CurrentContext, address);
            // The account not be created yet.
            if (account.Length == 0)
                return 0;

            byte[] arr = GetBalanceArray(account);
            uint ret = BytesToUInt(arr, 0);
            return ret;
        }
        public static bool InputMatchResult(byte[] matchResultList)
        {
            if (0 == matchResultList.Length)
                return false;
            byte[] matchResult = Storage.Get(Storage.CurrentContext, _keyMatchResult);
            matchResult.Concat(matchResultList);
            // Have to put newer match result in front of the byte array.
            if (0 == matchResult.Length)
            {
                Storage.Put(Storage.CurrentContext, _keyMatchResult, matchResultList);
            }
            else
            {
                matchResult = matchResultList.Concat(matchResult);
                Storage.Put(Storage.CurrentContext, _keyMatchResult, matchResult);
            }
            return true;
        }
        public static bool PushOddsList(string oddsList)
        {     
            if (0 == oddsList.Length)
                return false;

            // Store string data.
            Storage.Put(Storage.CurrentContext, _keyOddsList, oddsList);
            return true;
        }
        public static string GetOddsList()
        {
            string oddsList = Storage.Get(Storage.CurrentContext, _keyOddsList).AsString();
            return oddsList;
        }
        public static bool PushOddsData(byte[] oddsData)
        {
            if (0 == oddsData.Length)
                return false;
            // Store binary data.
            Storage.Put(Storage.CurrentContext, _keyOddsData, oddsData);
            return true;
        }
        public static byte[] GetOddsData()
        {
            // return new byte[] { 0xaa, 0xbb, 0xcc, 0xdd };
            byte[] oddsData = Storage.Get(Storage.CurrentContext, _keyOddsData);
            return oddsData;
        }
        public static byte[] Bet(byte[] address, int fixtureID, int betType, uint amount)
        {
            //if (!Runtime.CheckWitness(address))
            //    return false;

            byte[] ret = new byte[0];
            byte[] account = Storage.Get(Storage.CurrentContext, address);
            if (0 == account.Length)
                return new byte[] { 0x00 };

            int odds = GetIntOdds(fixtureID, betType);
            if (0 == odds)
                return new byte[] { 0xbb };
            if (1 == odds)  // Bet has been lockdown.
                return new byte[] { 0xcc };
            if (0 < GetMatchResult(fixtureID).Length)   //The match has been finished.
                return new byte[] { 0xee };

            byte[] betRecord = GetBetRecordArray(fixtureID, betType, odds, amount);
            account = AddBetRecord(account, betRecord);

            uint balance = GetBalance(account);
            balance = balance - amount;
            account = UpdateBalanceAmount(account, balance);
            // Write record.
            Storage.Put(Storage.CurrentContext, address, account);
            return betRecord;
        }
        public static uint CollectAward(byte[] address)
        {
            //if (!Runtime.CheckWitness(address))
            //    return 0;
            byte[] account = Storage.Get(Storage.CurrentContext, address);
            if (0 == account.Length)
                return 0;
            uint totalWinAmount = 0;
            int betCount = GetBetRecordCount(account);
            for (int i = 0; i < betCount; i++)      // Calcuate Bet record one by one.
            {
                byte[] betRecord = GetBetRecord(account, i);
                if (0 == betRecord.Length)
                    return 0xbb;
                if (IsCalcatued(betRecord))
                    continue;               
                int matchID = GetMatchID(betRecord);
                if (0 == matchID)
                    return 0xcc;
                byte[] matchResult = GetMatchResult(matchID);
                if (0 == matchResult.Length)
                    continue;
                if (3 <= GetBetType(betRecord))
                {
                    // Player wins 胜负平
                    if (GetBetType(betRecord) == CalcMatchResult(matchResult))
                    {
                        int odds = GetOddsFromBetRecord(betRecord);
                        if (0 < odds)      // already find match result.
                        {
                            uint award = ((uint)(GetBetAmount(betRecord) * odds) / 1000);
                            uint balance = GetBalance(account);
                            balance = balance + award;
                            // Update account data.
                            account = UpdateBetRecord(account, i, betRecord);
                            account = UpdateBalanceAmount(account, balance);
                            totalWinAmount = totalWinAmount + award;
                        }
                    }
                }
                else
                {
                    // Player wins 让球
                    int odds = GetOddsFromBetRecord(betRecord);
                    byte concedeResult = CalcConcedeMatchResult(matchResult);
                    if (0x06 == concedeResult)
                    {
                        uint award = ((uint)(GetBetAmount(betRecord) * odds) / 1000) / 2;   // 打平拿一半
                        uint balance = GetBalance(account);
                        balance = balance + award;
                        // Update account data.
                        account = UpdateBetRecord(account, i, betRecord);
                        account = UpdateBalanceAmount(account, balance);
                        totalWinAmount = totalWinAmount + award;
                    }
                    else if (GetBetType(betRecord) == concedeResult)
                    {
                        uint award = ((uint)(GetBetAmount(betRecord) * odds) / 1000);
                        uint balance = GetBalance(account);
                        balance = balance + award;
                        // Update account data.
                        account = UpdateBetRecord(account, i, betRecord);
                        account = UpdateBalanceAmount(account, balance);
                        totalWinAmount = totalWinAmount + award;
                    }
                }
            }
            //Write to Storage.
            Storage.Put(Storage.CurrentContext, address, account);
            return totalWinAmount;
        }
        public static byte[] GetBetHistory(byte[] address)
        {
            byte[] account = GetAccountInfo(address);
            if (account.Length >= 13)
                return account.Range(9, account.Length - 9);
            else
                return account;
        }
        public static bool Reset()
        {
            Storage.Delete(Storage.CurrentContext, _keyMatchResult);
            Storage.Delete(Storage.CurrentContext, _keyOddsData);
            Storage.Delete(Storage.CurrentContext, _keyOddsList);
            return true;
        }
        public static bool ResetAccount(byte[] account)
        {
            Storage.Delete(Storage.CurrentContext, account);
            return true;
        }
        #endregion

        #region --- Private functions ---
        public static uint GetTimStamp()
        {
            return Runtime.Time;
        }
        private static uint TestAmount()
        {
            uint amount = 1000;
            int odds = 1320;
            uint award = ((uint)(amount * odds) / 1000);
            return award;
        }
        private static byte[] CreateNewAccountArray()
        {
            return new byte[] { 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }
        private static uint GetBalance(byte[] account)
        {
            if (0 == account.Length)
                return 0;

            return BytesToUInt(account, 1);
        }
        private static byte[] GetBalanceArray(byte[] accountArray)
        {
            if (0 == accountArray.Length)
                return new byte[0];

            return accountArray.Range(1, 4);
        }
        private static byte[] UpdateBalanceAmount(byte[] account, uint balance)
        {
            byte[] ret = account.Range(0, 1);
            byte[] temp = UIntToBytes(balance);
            ret = ret.Concat(temp);
            ret = ret.Concat(account.Range(5, account.Length - 5));

            return ret;
        }
        private static byte[] GetBetRecordArray(int matchID, int betType, int odds, uint amount)
        {
            return IntToBytes(matchID)
                .Concat(GetBetTypeByte(betType))
                .Concat(new byte[] { 0x00})
                .Concat(IntToBytes(odds))
                .Concat(UIntToBytes(amount));
        }
        private static byte[] AddBetRecord(byte[] account, byte[] betRecord)
        {
            if (13 > account.Length)
                return account;     // account array error.
            account = account.Concat(betRecord);
            uint count = BytesToUInt(account, 9);
            count++;
            byte[] temp = UIntToBytes(count);
            byte[] ret = account.Range(0, 9).Concat(temp);
            ret = ret.Concat(account.Range(13, account.Length - 13));
            return ret;
        }
        private static byte[] GetBetRecord(byte[] account, int index)
        {
            // By default, the length of one bet record is 14 bytes.
            // The account header is 13 bytes. 
            if (0 == account.Length 
                || (13 + (index + 1) * 14) > account.Length) // Index out of range.
                return new byte[0];

            byte[] betRecord = account.Range(13 + index * 14, 14);
            return betRecord;
        }
        private static byte[] UpdateBetRecord(byte[] account, int index, byte[] betRecord)
        {
            // By default, the length of bet record is 15 bytes.
            // The account header is 13 bytes. 

            if (0 == account.Length
                || (13 + (index + 1) * 14) > account.Length) // Index out of range.
                return new byte[0];

            byte[] ret = account.Range(0, 13 + index * 14).Concat(betRecord.Range(0, 5));
            ret = ret.Concat(new byte[] { 0x0f });
            ret = ret.Concat(betRecord.Range(6, betRecord.Length - 6));
            ret = ret.Concat(account.Range(13 + (index + 1) * 14, account.Length - (13 + (index + 1) * 14)));

            return ret;
        }
        private static int GetMatchID(byte[] betRecord)
        {
            if (0 == betRecord.Length)
                return 0;
            return BytesToInt(betRecord, 0);
        }
        private static byte GetBetType(byte[] betRecord)
        {
            if (0 == betRecord.Length)
                return 0x00;
            return betRecord[4];
        }
        private static bool IsCalcatued(byte[] betRecord)
        {
            if (0 == betRecord.Length)
                return false;
            if (0x00 == betRecord[5])
                return false;
            else
                return true;
        }
        private static byte[] SetCalcaute(byte[] betRecord)
        {
            if (0 == betRecord.Length)
                return new byte[0];

            byte[] temp = new byte[] { 0x0f };
            byte[] ret = betRecord.Range(0, 5).Concat(temp);
            ret = ret.Concat(betRecord.Range(6, betRecord.Length - 6));
            return ret;
        }
        private static int GetOddsFromBetRecord(byte[] betRecord)
        {
            if (0 == betRecord.Length)
                return 0;
            int odds = BytesToInt(betRecord, 6);
            return odds;
        }
        private static uint GetBetAmount(byte[] betRecord)
        {
            if (0 == betRecord.Length)
                return 0x00;
            uint amount = BytesToUInt(betRecord, 10);
            return amount;
        }
        private static int GetBetRecordCount(byte[] account)
        {
            return BytesToInt(account, 9);
        }
        private static byte[] GetBetTypeByte(int betType)
        {
            if (1 == betType)
                return new byte[] { 0x01 };
            else if (2 == betType)
                return new byte[] { 0x02 };
            else if (3 == betType)
                return new byte[] { 0x03 };
            else
                return new byte[] { 0x00 };
        }
        private static int GetIntOdds(int matchID, int betType)
        {
            byte[] oddsLine = GetOddsLine(matchID);
            int temp = 0;
            if (0 == oddsLine.Length)
                return temp;
            if (1 == betType)
            {
                temp = BytesToInt(oddsLine, 12);
            }
            else if (2 == betType)
            {
                temp = BytesToInt(oddsLine, 16);
            }
            else if (3 == betType)
            {
                temp = BytesToInt(oddsLine, 20);
            }
            else if (4 == betType)
            {
                temp = BytesToInt(oddsLine, 4);
            }
            else if (5 == betType)
            {
                temp = BytesToInt(oddsLine, 8);
            }
            // Check bet timeslot is lockdown.
            uint lockdown = BytesToUInt(oddsLine, 24);
            if (Runtime.Time > lockdown)
                return 1;
            else
                return temp;
        }
        private static byte[] GetOddsLine(int matchID)
        {
            byte[] ret = new byte[0];
            byte[] oddsRawData = Storage.Get(Storage.CurrentContext, _keyOddsData);
            if (0 == oddsRawData.Length)
                return ret;

            for (int i = 0; i < oddsRawData.Length; i += 30)
            {
                if (BytesToInt(oddsRawData, i) == matchID)
                {
                    ret = oddsRawData.Range(i, 30);
                    break;
                }
            }
            return ret;
        }
        private static byte[] GetConcedeData(int matchID)
        {
            byte[] oddsLine = GetOddsLine(matchID);
            if (0 == oddsLine.Length)
                return new byte[0];
            return oddsLine.Range(28, 2);
        }
        private static uint GetLastApplyChipsTimeStamp(byte[] account)
        {
            return BytesToUInt(account, 5);
        }
        private static byte[] SetTimeStamp(byte[] account)
        {
            uint now = Runtime.Time;
            byte[] time = UIntToBytes(now);

            byte[] ret = account.Range(0, 5).Concat(time);
            ret = ret.Concat(account.Range(9, account.Length - 9));
            return ret;
        }
        private static byte[] GetMatchResult(int matchID)
        {
            byte[] ret = new byte[0]; 
            byte[] matchResult = Storage.Get(Storage.CurrentContext, _keyMatchResult);
            if (0 == matchResult.Length)
                return ret;

            for (int i = 0; i < matchResult.Length; i += 6)
            {
                if (matchID == BytesToInt(matchResult, i))
                {
                    ret = matchResult.Range(i, 6);
                    break;
                }
            }
            return ret;
        }
        private static byte CalcMatchResult(byte[] matchResult)
        {
            if (0 == matchResult.Length)
                return 0x00;
            byte scoreA = matchResult.Range(4, 1)[0];
            byte scoreB = matchResult.Range(5, 1)[0];
            if (scoreA > scoreB)
                return 0x01;
            else if (scoreA < scoreB)
                return 0x02;
            else
                return 0x03;
        }
        private static byte[] GetMatchResult()
        {
            return Storage.Get(Storage.CurrentContext, _keyMatchResult);
        }
        private static byte[] GetConcedePoints(int gg, bool isHomeConcede)
        {
            int i = (gg - 1) / 2;
            int j = i * 5;
            if (false == isHomeConcede)
                j = 0 - j;
            byte[] ret = IntToBytes(j);
            if (0 == gg % 2)
            {
                int k = (i + 1) * 5;
                if (false == isHomeConcede)
                    k = 0 - k;
                ret = ret.Concat(IntToBytes(k));
            }
            return ret;
        }
        private static byte CalcConcedeMatchResult(byte[] matchResult)
        {
            if (0 == matchResult.Length)
                return 0x00;
            int fixtureID = BytesToInt(matchResult, 4);
            int scoreA = matchResult.Range(4, 1)[0];
            int scoreB = matchResult.Range(5, 1)[0];
            byte[] gg = GetConcedeData(fixtureID);
            bool isHomeConcede = false;
            if (0 < gg[1])
                isHomeConcede = true;
            byte[] concedeResult = GetConcedePoints(gg[0], isHomeConcede);
            if (8 == concedeResult.Length)
            {
                int point = BytesToInt(concedeResult, 4);
                if (scoreA * 10 - point == scoreB * 10)
                    return 0x06;    //  Draw in case of conceded.
            }
            else    // 4 == concedeResult.Length
            {
                int point = BytesToInt(concedeResult, 0);
                if (scoreA * 10 - point > scoreB * 10)
                    return 0x04;
                else
                    return 0x05;
            }
            return 0x00;
        }
        #endregion

        #region --- Utilities ---
        private static int BytesToInt(byte[] src, int offset)
        {
            int value;
            value = (int)((src[offset] & 0xFF)
                    | ((src[offset + 1] & 0xFF) << 8)
                    | ((src[offset + 2] & 0xFF) << 16)
                    | ((src[offset + 3] & 0xFF) << 24));
            return value;
        }
        private static uint BytesToUInt(byte[] src, int offset)
        {
            uint value;
            value = (uint)((src[offset] & 0xFF)
                    | ((src[offset + 1] & 0xFF) << 8)
                    | ((src[offset + 2] & 0xFF) << 16)
                    | ((src[offset + 3] & 0xFF) << 24));
            return value;
        }
        private static byte[] IntToBytes(int value)
        {
            byte[] bytes = new byte[0];
            byte[] temp = value.Serialize();
            bytes = temp.Range(2, temp.Length - 2);
            int len = bytes.Length;
            for (int i = 0; i < 4 - len; i++)
                bytes = bytes.Concat(new byte[] { 0x00 });

            return bytes;
        }
        private static byte[] UIntToBytes(uint value)
        {
            byte[] bytes = new byte[0];
            byte[] temp = value.Serialize();
            bytes = temp.Range(2, temp.Length - 2);
            int len = bytes.Length;
            for (int i = 0; i < 4 - len; i++)
                bytes = bytes.Concat(new byte[] { 0x00 });
         
            return bytes;
        }
        private static int StringToInt(string s)
        {
            int ret = 0;
            int len = s.Length;
            int digits = 1;
            char t = '\0';
            for (int i = len - 1; i >= 0; i--)
            {
                t = s[i];
                // Is a number in ASCII
                if (48 <= (int)t || (int)t <= 57)
                {
                    for (int k = 0; k < (len - i - 1); k++)
                        digits = digits * 10;
                    ret += (t - 48) * digits;
                    digits = 1;
                }
            }
            return ret;
        }

        #endregion
    }
}
