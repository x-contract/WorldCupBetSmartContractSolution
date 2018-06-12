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
        // 账户信息
        private static string _address = "XQ3oFBKf7HH5ZYxd79sdsiEFysExbCbQHv";
        private static string _addressHex = "101d568f515ade12a7c19aa4bf3e659a0f314d8b";
        // 0219bb9cbd9e00730469d34cd959f3efb6e840474af5cdda0298e1499ad505b5ff
        private static byte[] _publicKey = ThinNeo.Helper.HexString2Bytes("0219bb9cbd9e00730469d34cd959f3efb6e840474af5cdda0298e1499ad505b5ff");
        // 6628084b9180ae7491fa1587638566524a39d6f8e8a90d7b5b6a96320cf0a6fd
        private static byte[] _privateKey = ThinNeo.Helper.HexString2Bytes("6628084b9180ae7491fa1587638566524a39d6f8e8a90d7b5b6a96320cf0a6fd");

        // 合约地址,该地址随合约部署每次都必须修改！！！
        private static UInt160 _contractHash = UInt160.Parse("0x8173136213db1cdd4f021ce48a7c319341830265");
        private static Hash160 _addressHash = ThinNeo.Helper.GetScriptHashFromPublicKey(_publicKey);

        private static  string OddsList = 
            "242404,20180614 23:00,俄罗斯,沙地阿拉伯,1940,1920,1340,8400,4150" + "\r\n" +
            "242405,20180615 20:00,埃及,乌拉圭,1800,2060,5850,1500,3700" + "\r\n" +
            "242406,20180615 23:00,摩洛哥,伊朗,1980,1880,2200,3200,2950" + "\r\n" +
            "242407,20180616 02:00,葡萄牙,西班牙,1920,1940,3650,1900,3300" + "\r\n" +
            "242408,20180616 18:00,法国,澳洲,1900,1960,1160,14000,5700" + "\r\n" +
            "242410,20180616 21:00,阿根廷,冰岛,1840,2020,1280,9500,4500" + "\r\n" +
            "242409,20180616 23:59,秘鲁,丹麦,1780,2080,3280,2050,3200" + "\r\n" +
            "242411,20180617 03:00,克罗地亚,尼日利亚,1860,2000,1850,3750,3350" + "\r\n" +
            "242412,20180617 20:00,哥斯达黎加,塞尔维亚,18400,20200,3600,1950,3200" + "\r\n" +
            "242413,20180617 23:00,德国,墨西哥,2100,1760,1380,6800,4200" + "\r\n" +
            "242414,20180618 02:00,巴西,瑞士,1880,1980,1320,7600,4600" + "\r\n" +
            "242424,20180618 20:00,瑞典,南韩,2080,1780,2080,3220,3200" + "\r\n" +
            "242425,20180618 23:00,比利时,巴拿马,1860,2000,1120,1600,6650" + "\r\n" +
            "242426,20180619 02:00,突尼西亚,英格兰,1780,2080,8600,1280,4750" + "\r\n" +
            "242427,20180619 20:00,哥伦比亚,日本,1960,1900,1700,4480,3350" + "\r\n" +
            "242428,20180619 23:00,波兰,塞内加尔,2000,1860,21800,3150,3050";

        private static string OddsList1 = "242404,20180614 23:00,俄罗斯,沙地阿拉伯,1940,1920,1340,8400,4150" + "\r\n";
        private static byte[] MatchResult = new byte[] { 228, 178, 3, 0, 3, 2, 229, 178, 3, 0, 1, 1, 230, 178, 3, 0, 1, 3 };

        public static void Main(string[] args)
        {
            string requestBody = string.Empty;
            string code = string.Empty;

            //code = TESTGetOddsData();
            //SendRequest(code).GetAwaiter();


            //code = TESTOddsLine();
            //SendRequest(code).GetAwaiter();
            //code = TESTCollectAward();
            //SendRequest(code).GetAwaiter();

            //code = TestApplyChips();
            //SendRequest(code).GetAwaiter();
            //System.Threading.Thread.Sleep(20000);
            //code = TestBalanceOf();
            //SendRequest(code).GetAwaiter();
            //code = TESTPushOddsData();
            //SendRequest(code).GetAwaiter();
            //code = TESTInputMatchResult();
            //SendRequest(code).GetAwaiter();
            //System.Threading.Thread.Sleep(20000);

            //code = TestPushOddsList();
            //SendRequest(code).GetAwaiter();
            //System.Threading.Thread.Sleep(30000);
            //code = TESTGetOddsList();
            //SendRequest(code).GetAwaiter();

            //code = TESTResetAcount();
            //SendRequest(code).GetAwaiter();

            //code = TESTGetTimeStamp();
            //SendRequest(code).GetAwaiter();
            //code = TESTGetIntOdds();
            //SendRequest(code).GetAwaiter();

            //PrepareForBet();
            //code = TESTBet();
            //SendRequest(code).GetAwaiter();
            //System.Threading.Thread.Sleep(20000);

            //PrepareForCollectAward();
            //code = TESTGetMatchResult();
            //SendRequest(code).GetAwaiter();
            //code = TESTGetBetRecord();
            //code = TestCollectAward();
            //SendRequest(code).GetAwaiter();

            //code = TESTGetAcountInfo();
            //SendRequest(code).GetAwaiter();

            //code = TESTAmount();
            //SendRequest(code).GetAwaiter();
            //code = TestGetMatchID();
            //SendRequest(code).GetAwaiter();
            //code = TestCalcMatchResult();
            //SendRequest(code).GetAwaiter();

            //code = TESTGetAcountInfo();
            //SendRequest(code).GetAwaiter();
            code = TESTGetBetHistory();
            SendRequest(code).GetAwaiter();

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
            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(_rnd.Next());
            sb.Emit(Neo.VM.OpCode.DROP);
                sb.EmitPush(OddsList);
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
            sb.EmitPush(1);  // 押主队赢
            sb.EmitPush(242404); // 赛事ID
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
            sb.EmitPush(242404); // 赛事ID
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
            sb.EmitPush(242404); // 赛事ID
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
            sb.EmitPush(242404); // 赛事ID
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
            sb.EmitPush(242404); // 赛事ID
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
            sb.EmitPush(242404);
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
            sb.EmitPush(242404); // 赛事ID
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
    }
}
