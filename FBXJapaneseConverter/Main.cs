using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Web;
using System.Net;

namespace FBXJapaneseConverter
{
    public partial class Main : Form
    {
        [DataContract]
        public class AdmAccessToken
        {
            [DataMember]
            public string access_token { get; set; }
            [DataMember]
            public string token_type { get; set; }
            [DataMember]
            public string expires_in { get; set; }
            [DataMember]
            public string scope { get; set; }
        }

        public class AdmAuthentication
        {
            public static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
            private string clientId;
            private string cientSecret;
            private string request;

            public AdmAuthentication(string clientId, string clientSecret)
            {
                this.clientId = clientId;
                this.cientSecret = clientSecret;
                //If clientid or client secret has special characters, encode before sending request
                this.request = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret));
            }

            public AdmAccessToken GetAccessToken()
            {
                return HttpPost(DatamarketAccessUri, this.request);
            }

            private AdmAccessToken HttpPost(string DatamarketAccessUri, string requestDetails)
            {
                //Prepare OAuth request 
                WebRequest webRequest = WebRequest.Create(DatamarketAccessUri);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";
                byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
                webRequest.ContentLength = bytes.Length;
                using (Stream outputStream = webRequest.GetRequestStream())
                {
                    outputStream.Write(bytes, 0, bytes.Length);
                    outputStream.Close();
                }
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                    //Get deserialized object from JSON stream
                    AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
                    webResponse.Close();
                    return token;
                }
            }
        }

        public Main()
        {
            InitializeComponent();
        }

        private class JapaneseString
        {
            public string m_Source = "";
            public string m_Convert = "";
            public List<int> m_Lines = new List<int>();
        }

        private List<JapaneseString> m_Japaneses = new List<JapaneseString>();
        private List<string> m_FbxLines = new List<string>();
        private string m_TranslateText = "";

        private bool ReadFbx(string filename)
        {
            string line = "";
            m_FbxLines.Clear();
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    m_FbxLines.Add(line);
                }
            }

            // 開始30行以内にFBXを示すヘッダ文字列が見つかるかどうか調べる
            // 30行調べれば十分でしょう
            int numSearch = System.Math.Min(30, m_FbxLines.Count);
            for (int i = 0; i < numSearch; ++i)
            {
                if (m_FbxLines[i].Contains("FBXHeaderExtension"))
                {
                    return true;
                }
            }
            return false;
        }

        private void AddJapanese(string japanese, int line)
        {
            // 登録済みの文字列かどうか調べる
            foreach (var s in m_Japaneses)
            {
                if (s.m_Source == japanese)
                {
                    s.m_Lines.Add(line);
                    return;
                }
            }

            // 登録されていないので新規登録
            var ns = new JapaneseString();
            ns.m_Source = japanese;
            ns.m_Lines.Add(line);
            m_Japaneses.Add(ns);
        }

        private void SearchJapanese(string str, int line)
        {
            bool isAscii = true;
            string j = "";
            Encoding enc = Encoding.UTF8;
            foreach (char c in str)
            {
                char[] ca = new char[1]{c};
                if (isAscii)
                {
                    if (enc.GetByteCount(ca) != 1)
                    {
                        isAscii = false;
                        j += c;
                    }
                }
                else
                {
                    if (enc.GetByteCount(ca) == 1)
                    {
                        isAscii = true;
                        AddJapanese(j, line);
                        j = "";
                    }
                    else
                    {
                        j += c;
                    }
                }
            }
            if (!string.IsNullOrEmpty(j))
            {
                AddJapanese(j, line);
            }
        }

        private void AnalyzeLines()
        {
            int line = 0;
            foreach (var str in m_FbxLines)
            {
                SearchJapanese(str, line);
                ++line;
            }
        }

        private void DisplayJapanese()
        {
            gridConv.Rows.Clear();
            foreach (var j in m_Japaneses)
            {
                int row = gridConv.Rows.Add();
                gridConv.Rows[row].Cells[0].Value = j.m_Source;
                gridConv.Rows[row].Cells[1].Value = j.m_Convert;
            }
        }

        private void CopyCellValue()
        {
            foreach (DataGridViewRow row in gridConv.Rows)
            {
                m_Japaneses[row.Index].m_Convert = (string)row.Cells[1].Value;
            }
        }

        private void ConvertJapanese(JapaneseString j)
        {
            foreach (var line in j.m_Lines)
            {
                m_FbxLines[line] = m_FbxLines[line].Replace(j.m_Source, j.m_Convert);
            }
        }

        private void ConvertStrings()
        {
            foreach (var j in m_Japaneses)
            {
                if (!string.IsNullOrEmpty(j.m_Convert))
                {
                    ConvertJapanese(j);
                }
            }
        }

        private void WriteFbx(string filename)
        {
            StreamWriter sw = new StreamWriter(filename, false, new UTF8Encoding(false));
            sw.NewLine = "\n";
            foreach (var str in m_FbxLines)
            {
                sw.WriteLine(str);
            }
            sw.Close();
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            string[] filename = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (!string.IsNullOrEmpty(filename[0]))
            {
                tboxFile.Text = filename[0];

                // FBXファイルを読み込む
                if (!ReadFbx(tboxFile.Text))
                {
                    tboxFile.Text = "";
                    return;
                }

                // 各行を検索し、日本語文字列をチェック
                AnalyzeLines();

                // 表示
                DisplayJapanese();

                m_TranslateText = "";
            }
        }

        private void btnConvSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tboxFile.Text))
            {
                // 元のファイルをバックアップ
                System.IO.File.Copy(tboxFile.Text, tboxFile.Text + ".bak");

                // セルの内容をコピー
                CopyCellValue();

                // 文字列変換
                ConvertStrings();

                // ファイルを保存
                WriteFbx(tboxFile.Text);

                // ダイアログ表示
                MessageBox.Show("正常終了しました", "変換＆保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ProcessWebException(WebException e)
        {
            //Console.WriteLine("{0}", e.ToString());
            System.Diagnostics.Debug.WriteLine("ProcessWebException : Error " + e.Message);
            // Obtain detailed error information
            string strResponse = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)e.Response)
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(responseStream, System.Text.Encoding.ASCII))
                    {
                        strResponse = sr.ReadToEnd();
                    }
                }
            }
            //Console.WriteLine("Http status code={0}, error message={1}", e.Status, strResponse);
            System.Diagnostics.Debug.WriteLine("ProcessWebException : Http status code={0}, error message={1}", e.Status, strResponse);
        }

        public string Translate(string header, string input)
        {
            string translation = "";

            string from = "ja";
            string to = "en";
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?&text=" + System.Web.HttpUtility.UrlEncode(input) + "&from=" + from + "&to=" + to + "&contentType=text%2fplain";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", header);
            //httpWebRequest.KeepAlive = false;
            WebResponse response = null;

            try
            {
                response = httpWebRequest.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
                    translation = (string)dcs.ReadObject(stream);
                    stream.Close();
                }
            }
            catch (WebException e)
            {
                ProcessWebException(e);
                return "";
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }

            return translation;
        }
        private void btnTranslate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tboxFile.Text))
            {
                string kClientID = "FBXJapaneseConverter";
                string kClientSecret = "/k8GYTwvGWMT0J0ugw2T561ZRbVlbzx/JdINC5mbB74=";

                var admAuth = new AdmAuthentication(kClientID, kClientSecret);
                AdmAccessToken admToken;
                string headerValue;
                try
                {
                    admToken = admAuth.GetAccessToken();
                    headerValue = "Bearer " + admToken.access_token;
                }
                catch (WebException we)
                {
                    ProcessWebException(we);
                    return;
                }

                if (string.IsNullOrEmpty(m_TranslateText))
                {
                    // 翻訳する文字列を作成する
                    string inputText = "";
                    foreach (var j in m_Japaneses)
                    {
                        inputText += j.m_Source;
                        inputText += "\r\n";
                    }

                    // 翻訳
                    m_TranslateText = Translate(headerValue, inputText);
                }

                if (string.IsNullOrEmpty(m_TranslateText))
                {
                    MessageBox.Show("翻訳に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 1行ずつ加工して書き込む
                var reader = new StringReader(m_TranslateText);
                foreach (var j in m_Japaneses)
                {
                    string line = reader.ReadLine();

                    // 空白を発見したらそれを削除し、その後の文字を大文字に変換する
                    int idx;
                    while ((idx = line.IndexOf(' ')) >= 0)
                    {
                        char[] s = {line[idx + 1]};
                        line = line.Remove(idx, 2);
                        line = line.Insert(idx, (new string(s)).ToUpper());
                    }

                    j.m_Convert = line;
                }

                // 表示
                DisplayJapanese();
            }
        }

    }
}
