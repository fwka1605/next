using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Environment;

namespace Rac.VOne.Client.Common.Settings
{
    public enum SettingPath
    {
        Local,
        Roaming,
        AllUsers,
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SettingFileAttribute : Attribute
    {
        public SettingPath Path { get; set; }
        public string FileName { get; set; }
        public Type ContainerType { get; set; }

        public string GetFilePath()
        {
            string filePath = null;
            switch (Path)
            {
                case SettingPath.Local:
                    filePath = GetFolderPath(SpecialFolder.LocalApplicationData); break;
                case SettingPath.Roaming:
                    filePath = GetFolderPath(SpecialFolder.ApplicationData); break;
                case SettingPath.AllUsers:
                    filePath = GetFolderPath(SpecialFolder.CommonApplicationData); break;
            }

            string racFolder = System.IO.Path.Combine(filePath, "Rac");
            if (!Directory.Exists(racFolder))
            {
                Directory.CreateDirectory(racFolder);
            }

            string voneFolder = System.IO.Path.Combine(racFolder, "VOne");
            if (!Directory.Exists(voneFolder))
            {
                Directory.CreateDirectory(voneFolder);
            }

            return System.IO.Path.Combine(voneFolder, FileName);
        }
    }

    public interface IIndividualUserSetting
    {
        string CompanyCode { get; set; }

        string UserCode { get; set; }
    }

    public class Settings
    {
        public static Settings Singleton { get; set; } = new Settings();

        public static string RestorePath<TModel>(ILogin login)
        {
            DialogInfo info = Singleton.ReadInternal<DialogInfo>(
                    login, d => d.Key == typeof(TModel).Name);
            return info?.Value;
        }

        public static void SavePath<TModel>(ILogin login, string path)
        {
            var info = new DialogInfo
            {
                CompanyCode = login.CompanyCode,
                UserCode = login.UserCode,
                Key = typeof(TModel).Name,
                Value = path,
            };
            Singleton.SaveInternal(info);
        }

        public static void SaveControlValue<TScreen>(
                ILogin login, string controlName, object value)
        {
            var info = new DialogInfo
            {
                CompanyCode = login.CompanyCode,
                UserCode = login.UserCode,
                Key = $"{typeof(TScreen).Name}.{controlName}",
                Value = value.ToString(),
            };
            Singleton.SaveInternal(info);
        }

        public static string RestoreControlValue<TScreen>(
                ILogin login, string controlName)
        {
            DialogInfo info = Singleton.ReadInternal<DialogInfo>(
                login, d => d.Key == $"{typeof(TScreen).Name}.{controlName}");
            return info?.Value;
        }

        /// <summary>
        ///  設定値を型指定して取得
        ///  <seealso cref="Nullable{T}"/>となるので、値型は初期値を指定すること
        ///  未設定の場合は <see cref="null"/>を返す
        /// </summary>
        /// <typeparam name="TScreen"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="login"></param>
        /// <param name="controlName"></param>
        /// <returns></returns>
        public static T? RestoreControlValue<TScreen, T>(
            ILogin login, string controlName)
            where T : struct
        {
            var result = default(T?);
            var value = RestoreControlValue<TScreen>(login, controlName);

            if (string.IsNullOrWhiteSpace(value)) return null;

            try
            {
                var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                result = (T?)converter.ConvertFromInvariantString(value);
            }
            catch (NotSupportedException)
            {
            }
            return result;
        }

        public static void SetCheckBoxValue<TScreen>(ILogin login, CheckBox cbx)
        {
            cbx.Checked = RestoreControlValue<TScreen, bool>(login, cbx.Name) ?? false;
        }

        public static void Save<T>(T setting)
            where T : class, IEquatable<T>, new()
        {
            Singleton.SaveInternal(setting);
        }

        public T ReadInternal<T>(ILogin login, Func<T, bool> where = null)
            where T : class, IEquatable<T>, new()
        {
            SettingFileAttribute attr = GetSettingAttrute<T>();
            IEnumerable<T> settings = Singleton?.ReadFile<T>(attr);

            if (settings == null) return null;

            bool isIndividualUser = typeof(IIndividualUserSetting)
                    .IsAssignableFrom(typeof(T));
            if (isIndividualUser && login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (isIndividualUser)
            {
                IEnumerable<T> values = (where == null)
                        ? settings
                        : settings.Where(where);
                T individual = values
                        .Cast<IIndividualUserSetting>()
                        .FirstOrDefault(i => i.CompanyCode == login.CompanyCode
                            && i.UserCode == login.UserCode) as T;
                return individual;
            }

            return (where == null)
                    ? settings.FirstOrDefault()
                    : settings.FirstOrDefault(where);
        }

        public virtual void SaveInternal<T>(T setting)
            where T : class, IEquatable<T>, new()
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            SettingFileAttribute attr = GetSettingAttrute<T>();
            IList<T> settings = Singleton.ReadFile<T>(attr);

            object target = null;
            if (attr.ContainerType == null)
            {
                target = setting;
            }
            else
            {
                if (settings == null)
                {
                    settings = Activator.CreateInstance(attr.ContainerType) as IList<T>;
                }
                T original = settings.FirstOrDefault(i => i.Equals(setting));
                if (original == null)
                {
                    settings.Add(setting);
                }
                else
                {
                    int index = settings.IndexOf(original);
                    settings[index] = setting;
                }
                target = settings;
            }

            Singleton.SaveFile<T>(attr, target);
        }

        private SettingFileAttribute GetSettingAttrute<T>()
            where T : class, IEquatable<T>, new()
        {
            SettingFileAttribute setting = typeof(T)
                    .GetCustomAttributes(typeof(SettingFileAttribute), false)
                    .FirstOrDefault() as SettingFileAttribute;

            if (setting == null)
            {
                throw new InvalidOperationException();
            }

            return setting;
        }

        public virtual IList<T> ReadFile<T>(SettingFileAttribute attr)
            where T : class, IEquatable<T>, new()
        {
            string filePath = attr.GetFilePath();
            if (!System.IO.File.Exists(filePath)) return null;

            var serializer = new DataContractSerializer(attr.ContainerType ?? typeof(T));
            using (var reader = XmlReader.Create(filePath))
            {
                if (attr.ContainerType == null)
                {
                    return new T[] { serializer.ReadObject(reader) as T };
                }
                else
                {
                    return serializer.ReadObject(reader) as IList<T>;
                }
            }
        }

        public virtual void SaveFile<T>(SettingFileAttribute attr, object target)
        {
            string filePath = attr.GetFilePath();
            Type rootType = attr.ContainerType ?? typeof(T);
            var serializer = new DataContractSerializer(rootType, rootType.Name, string.Empty);

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var writer = XmlWriter.Create(stream,
                    new XmlWriterSettings() { Encoding = Encoding.GetEncoding(932) }))
            {
                serializer.WriteObject(writer, target);
            }
        }
    }
}
