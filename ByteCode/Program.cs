using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Neo;
using Neo.VM;
using ThinNeo;

namespace WorldCup.UnitTest
{
    public class Program
    {
        private static string _uri = "http://139.219.9.59:10332";
        private static Random _rnd = new Random(DateTime.Now.Millisecond);
        private static DateTime _startDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
        // 账户信息
        private static string _address = "XQ3oFBKf7HH5ZYxd79sdsiEFysExbCbQHv";
        private static string _addressHex = "101d568f515ade12a7c19aa4bf3e659a0f314d8b";
        // 0219bb9cbd9e00730469d34cd959f3efb6e840474af5cdda0298e1499ad505b5ff
        private static byte[] _publicKey = ThinNeo.Helper.HexString2Bytes("0219bb9cbd9e00730469d34cd959f3efb6e840474af5cdda0298e1499ad505b5ff");
        // 6628084b9180ae7491fa1587638566524a39d6f8e8a90d7b5b6a96320cf0a6fd
        private static byte[] _privateKey = ThinNeo.Helper.HexString2Bytes("6628084b9180ae7491fa1587638566524a39d6f8e8a90d7b5b6a96320cf0a6fd");

        // 合约地址,该地址随合约部署每次都必须修改！！！
        private static UInt160 _contractHash = UInt160.Parse("0xbd66d6c405ac0bb5f44246ce39d8ec1dd05df3e0");
        private static Hash160 _addressHash = ThinNeo.Helper.GetScriptHashFromPublicKey(_publicKey);

        // private static string OddsList1 = "242404,20180614 23:00,俄罗斯,沙地阿拉伯,1940,1920,1340,8400,4150" + "\r\n";
        private static byte[] MatchResult = new byte[] { 6, 179, 3, 0, 3, 2, 229, 178, 3, 0, 1, 1, 230, 178, 3, 0, 1, 3 };

        public static void Main(string[] args)
        {
            string requestBody = string.Empty;
            string code = string.Empty;

            //code = TESTGetOddsData();
            //SendRequest(code).GetAwaiter();

            code = TESTReset();
            SendRequest(code).GetAwaiter();
            code = TESTResetAcount();
            SendRequest(code).GetAwaiter();

            code = TestApplyChips();
            SendRequest(code).GetAwaiter();
            System.Threading.Thread.Sleep(20000);
            code = TestBalanceOf();
            SendRequest(code).GetAwaiter();
            code = TESTPushOddsData();
            SendRequest(code).GetAwaiter();
            System.Threading.Thread.Sleep(20000);

            code = TestPushOddsList();
            SendRequest(code).GetAwaiter();
            System.Threading.Thread.Sleep(30000);
            code = TESTGetOddsList();
            SendRequest(code).GetAwaiter();

            code = TESTBet();
            SendRequest(code).GetAwaiter();
            System.Threading.Thread.Sleep(20000);

            code = TESTInputMatchResult();
            SendRequest(code).GetAwaiter();
            System.Threading.Thread.Sleep(30000);
            code = TestCollectAward();
            SendRequest(code).GetAwaiter();

            //code = TESTGetAcountInfo();
            //SendRequest(code).GetAwaiter();

            //code = TESTAmount();
            //SendRequest(code).GetAwaiter();
            //code = TestGetMatchID();
            //SendRequest(code).GetAwaiter();
            //code = TestCalcMatchResult();
            //SendRequest(code).GetAwaiter();

            System.Threading.Thread.Sleep(30000);
            //code = TESTGetAcountInfo();
            //SendRequest(code).GetAwaiter();
            code = TESTGetBetHistory();
            SendRequest(code).GetAwaiter();

            //code = TESTSetCalculate();
            //SendRequest(code).GetAwaiter();

            //code = TESTPushOddsData();
            //SendRequest(code).GetAwaiter();
            //System.Threading.Thread.Sleep(20000);
            //code = TESTIsLockdown();
            //SendRequest(code).GetAwaiter();

            Console.WriteLine("Completed!!!");
            Console.ReadLine();
        }
        private async static Task SendRequest(string tansaction)
        {
            string requestBody = "{\"jsongrpc\" : \"2.0\", \"method\" : \"sendrawtransaction\", \"params\" : [\""
                                    + tansaction
                                     + "\"], \"id\" : \"1\"}";

            //string requestBody = "{\"jsongrpc\" : \"2.0\", \"method\" : \"invokescript\", \"params\" : [\""
            //            + tansaction
            //             + "\"], \"id\" : \"1\"}";

            Console.WriteLine("Request JSON:");
            Console.WriteLine(requestBody);
            string result = await SendAsync(requestBody);
            Console.WriteLine("Response:");
            Console.WriteLine(result);
        }
        private async static Task<string> SendAsync(string requestBody)
        {
            HttpClient client = new HttpClient();
            // client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            StringContent content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(_uri, content);
            return await response.Content.ReadAsStringAsync();
        }
        private static string TestName()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(_publicKey);
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("Name");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);
            return str2;
        }
        private static string TestSymbol()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(_publicKey);
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("Symbol");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);
            return str2;
        }
        private static string TestBalanceOf()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(Neo.Helper.HexToBytes(_addressHex));
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("BalanceOf");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            att.usage = TransactionAttributeUsage.Script;
            tx.attributes = new ThinNeo.Attribute[] { att };
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TestApplyChips()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(Neo.Helper.HexToBytes(_addressHex));
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("ApplyChips");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);


            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            att.usage = TransactionAttributeUsage.Script;
            tx.attributes = new ThinNeo.Attribute[] { att };
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet; 
        }
        private static string TestPushOddsList()
        {
            OddsCaptureCore.MacauSlot macauSlot = new OddsCaptureCore.MacauSlot();
            DataTable oddsList = macauSlot.AnalyzeDataAsync(false).GetAwaiter().GetResult();
            StringBuilder sbOddsList = new StringBuilder();
            foreach (DataRow row in oddsList.Rows)
            {
                foreach (DataColumn col in oddsList.Columns)
                {
                    sbOddsList.Append(row[col].ToString());
                    sbOddsList.Append(",");
                }
                sbOddsList.Remove(sbOddsList.Length - 1, 1);
                sbOddsList.Append("\r\n");
            }

            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
                sb.EmitPush(sbOddsList.ToString());
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("PushOddsList");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);       

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            att.usage = TransactionAttributeUsage.Script;
            tx.attributes = new ThinNeo.Attribute[] { att };
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;

        }
        private static string TESTCollectAward()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(Neo.Helper.HexToBytes(_addressHex));
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("CollectAward");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Console.WriteLine(str2);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.usage = TransactionAttributeUsage.Script;
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;

        }
        private static string TESTInputMatchResult()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(MatchResult);
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("InputMatchResult");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Console.WriteLine(str2);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            att.usage = TransactionAttributeUsage.Script;
            tx.attributes = new ThinNeo.Attribute[] { att };
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTPushOddsData()
        {
            OddsCaptureCore.MacauSlot macauSlot = new OddsCaptureCore.MacauSlot();
            DataTable dt = macauSlot.AnalyzeDataAsync(false).GetAwaiter().GetResult();
            List<byte> arr = new List<byte>();
            List<byte> line = new List<byte>();
            foreach (DataRow r in dt.Rows)
            {
                line.AddRange(BitConverter.GetBytes((int.Parse(r["id"].ToString()))));
                line.AddRange(BitConverter.GetBytes((int)((float)r["主队让球"] * 1000)));
                line.AddRange(BitConverter.GetBytes((int)((float)r["客队让球"] * 1000)));
                line.AddRange(BitConverter.GetBytes((int)((float)r["主队赢"] * 1000)));
                line.AddRange(BitConverter.GetBytes((int)((float)r["客队赢"] * 1000)));
                line.AddRange(BitConverter.GetBytes((int)((float)r["平局"] * 1000)));
                string datetime = r["比赛时间"].ToString();
                datetime = datetime.Substring(0, 4) + "-" + datetime.Substring(4, 2) + "-"
                    + datetime.Substring(6, 2) + datetime.Substring(8, datetime.Length - 8);
                DateTime time = DateTime.Parse(datetime);
                line.AddRange(BitConverter.GetBytes(GetTimeStamp(time)));
                line.Add((byte)(int)r["让球"]);
                if ("A" == r["让球队伍"].ToString())
                    line.AddRange(BitConverter.GetBytes(false));
                else
                    line.AddRange(BitConverter.GetBytes(true));

                arr.AddRange(line.ToArray());
                line.Clear();
            }
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            byte[] v = arr.ToArray();
            byte[] v1 = new byte[768];
            for (int i = 0; i < 768; i++)
                v1[i] = v[i];
            sb.EmitPush(v1);
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("PushOddsData");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTBet()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            // 多参数输入，必须从右至左压栈！！！！
            sb.EmitPush(1000); // 押注额度
            sb.EmitPush(4);  // 押主队赢
            sb.EmitPush(242438); // 赛事ID
            sb.EmitPush(Neo.Helper.HexToBytes(_addressHex)); //地址
            sb.EmitPush(4);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("Bet");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            att.usage = TransactionAttributeUsage.Script;
            tx.attributes = new ThinNeo.Attribute[] { att };
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTBetRecordArray()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            // 多参数输入，必须从右至左压栈！！！
            sb.EmitPush(1000); // 押注额度
            sb.EmitPush(1320); // 赔率
            sb.EmitPush(1);  // 押主队赢
            sb.EmitPush(242438); // 赛事ID
            sb.EmitPush(4);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("BetRecordArray");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            att.usage = TransactionAttributeUsage.Script;
            tx.attributes = new ThinNeo.Attribute[] { att };
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTOddsLine()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            // sb.EmitPush(Neo.Helper.HexToBytes(_addressHex)); //地址
            sb.EmitPush(242438); // 赛事ID
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("GetOddsLine");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTGetOddsData()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(242438); // 赛事ID
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("GetOddsData");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTGetOddsList()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(242438); // 赛事ID
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("GetOddsList");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTReset()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(Neo.Helper.HexToBytes(_addressHex));
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("Reset");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTResetAcount()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(Neo.Helper.HexToBytes(_addressHex));
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("ResetAccount");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTGetAcountInfo()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(Neo.Helper.HexToBytes(_addressHex));
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("GetAccountInfo");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTGetBetHistory()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(Neo.Helper.HexToBytes(_addressHex));
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("GetBetHistory");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTAmount()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(Neo.Helper.HexToBytes(_addressHex));
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("TestAmount");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTGetTimeStamp()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(Neo.Helper.HexToBytes(_addressHex));
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("TestGetTimeStamp");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTGetIntOdds()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            // 多参数调用时，必须从右至左压栈！！！
            sb.EmitPush(1);
            sb.EmitPush(242438);
            sb.EmitPush(2);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("GetIntOdds");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TestCollectAward()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(Neo.Helper.HexToBytes(_addressHex));
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("CollectAward");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);


            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            att.usage = TransactionAttributeUsage.Script;
            tx.attributes = new ThinNeo.Attribute[] { att };
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TestGetMatchID()
        {
            byte[] betRecord = new byte[] { 228, 178, 3, 0, 1, 0, 3, 2};
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(betRecord);
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("GetMatchID");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);


            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            att.usage = TransactionAttributeUsage.Script;
            tx.attributes = new ThinNeo.Attribute[] { att };
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TestCalcMatchResult()
        {

            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(MatchResult);
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("CalcMatchResult");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);


            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            att.usage = TransactionAttributeUsage.Script;
            tx.attributes = new ThinNeo.Attribute[] { att };
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTGetMatchResult()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(242438); // 赛事ID
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("GetMatchResult");

            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static void PrepareForBet()
        {
            string code = string.Empty;
            code = TestApplyChips();
            SendRequest(code).GetAwaiter();
            code = TestBalanceOf();
            SendRequest(code).GetAwaiter();
            code = TESTPushOddsData();
            SendRequest(code).GetAwaiter();

        }
        private static void PrepareForCollectAward()
        {
            PrepareForBet();
            System.Threading.Thread.Sleep(20000);
            string code = string.Empty;
            code = TESTBet();
            SendRequest(code).GetAwaiter();
            System.Threading.Thread.Sleep(20000);
            code = TESTInputMatchResult();
            SendRequest(code).GetAwaiter();
            System.Threading.Thread.Sleep(20000);
            code = TESTGetMatchResult();
            SendRequest(code).GetAwaiter();
        }
        private static string TestGetBetRecord()
        {
            byte[] betRecord = new byte[] { 228, 178, 3, 0, 1, 0, 3, 2 };
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(betRecord);
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("GetBetRecord");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);


            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            att.usage = TransactionAttributeUsage.Script;
            tx.attributes = new ThinNeo.Attribute[] { att };
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static string TESTSetCalculate()
        {
            byte[] betRecord = new byte[] { 0x06, 0xb3, 0x03, 0x00, 0x01, 0x00, 0xdc, 0x0a, 0x00, 0x00, 0xe8, 0x03, 0x00, 0x00 };
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(true);
            sb.EmitPush(betRecord);
            sb.EmitPush(2);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("SetCalcaute");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
        private static int GetTimeStamp(DateTime dt)
        {
            // 提前15分钟封盘
            return (int)((dt - _startDateTime).TotalSeconds) - 900;
        }
        private static string TESTIsLockdown()
        {
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
            sb.EmitPush(242438); // 赛事ID
            sb.EmitPush(1);
            sb.Emit(Neo.VM.OpCode.PACK);
            sb.EmitPush("IsLockdown");
            //调用已发布的合约，最后加一条EmitAppCall即可
            var addr = _contractHash;
            sb.EmitAppCall(addr.ToArray());
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Transaction tx = new Transaction();
            ThinNeo.Attribute att = new ThinNeo.Attribute();
            att.data = _addressHash;
            tx.attributes = new ThinNeo.Attribute[] { att };
            att.usage = TransactionAttributeUsage.Script;
            tx.version = 0x01;
            tx.type = TransactionType.InvocationTransaction;
            tx.inputs = new TransactionInput[0];
            tx.outputs = new TransactionOutput[0];
            InvokeTransData data = new InvokeTransData();
            data.gas = new ThinNeo.Fixed8(0);
            data.script = sb.ToArray();
            tx.extdata = data;
            MemoryStream ms = new MemoryStream();
            tx.SerializeUnsigned(ms);
            byte[] signdata = ThinNeo.Helper.Sign(ms.ToArray(), _privateKey);
            ms.Close();
            tx.AddWitness(signdata, _publicKey, _address);
            MemoryStream msRet = new MemoryStream();
            tx.Serialize(msRet);
            string sRet = ThinNeo.Helper.Bytes2HexString(msRet.ToArray());
            msRet.Close();
            return sRet;
        }
    }
}
