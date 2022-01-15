using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Worms
{
    public class CfgReader
    {
        static public string ValueRead(string path, string param)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    byte[] msg = new byte[128];
                    fs.Read(msg, 0, msg.Length);
                    string str = Encoding.UTF8.GetString(msg);

                    string[] strLines = str.Split('\n', '\0', '\r');
                    for (int i = 0; i < strLines.Length; i++)
                    {
                        string[] paramtrs = strLines[i].Split('=');
                        if (paramtrs[0] == param)
                            return paramtrs[1];
                    }
                }
            }
            catch
            {
                Environment.Exit(0);
            }

            return "";
        }
    }
}
