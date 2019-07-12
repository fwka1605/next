using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Rac.Util.CodeGen
{
    public class MultipleFileWriter
    {
        public static void WriteFiles(string basefile,
            IEnumerable<Type> types,
            StringBuilder builder,
            Action<Type> handler)
        {
            foreach (var type in types)
            {
                var dir = Path.GetDirectoryName(basefile);
                dir = Path.Combine(dir, "ts");
                if (!Directory.Exists(dir))
                {
                    try
                    {
                        Directory.CreateDirectory(dir);
                    }
                    catch (Exception)
                    {
                    }
                }
                var path = Path.Combine(dir, type.GetTypeScriptFileName());
                try
                {
                    handler(type);
                    File.WriteAllText(path, builder.ToString(), new UTF8Encoding(false));
                }
                catch (Exception ex)
                {
                    File.WriteAllText(path + "error.log", ex.Message);
                }
                finally
                {
                    builder.Clear();
                }

            }
        }
    }
}
