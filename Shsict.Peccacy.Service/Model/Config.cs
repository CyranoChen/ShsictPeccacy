using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Shsict.Peccacy.Service.DbHelper;

namespace Shsict.Peccacy.Service.Model
{
    [Table("PECCACY_CONFIG")]
    public class Config : Entity<int>
    {
        public Config() { }

        public static void UpdateAssemblyInfo(Assembly assembly)
        {
            if (assembly != null)
            {
                //[assembly: AssemblyTitle("Arsenalcn.Core")]
                //[assembly: AssemblyDescription("沪ICP备12045527号")]
                //[assembly: AssemblyConfiguration("webmaster@arsenalcn.com")]
                //[assembly: AssemblyCompany("Arsenal China Official Supporters Club")]
                //[assembly: AssemblyProduct("Arsenalcn.com")]
                //[assembly: AssemblyCopyright("© 2015")]
                //[assembly: AssemblyTrademark("ArsenalCN")]
                //[assembly: AssemblyCulture("")]
                //[assembly: AssemblyVersion("1.8.*")]
                //[assembly: AssemblyFileVersion("1.8.2")]

                using (IRepository repo = new Repository())
                {
                    //AssemblyTitle
                    var c = repo.Single<Config>(x => x.ConfigKey == "AssemblyTitle");
                    c.ConfigValue = ((AssemblyTitleAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)))?.Title;

                    repo.Save(c);

                    //AssemblyDescription
                    c = repo.Single<Config>(x => x.ConfigKey == "AssemblyDescription");
                    c.ConfigValue = ((AssemblyDescriptionAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute)))?.Description;

                    repo.Save(c);

                    //AssemblyConfiguration
                    c = repo.Single<Config>(x => x.ConfigKey == "AssemblyConfiguration");
                    c.ConfigValue = ((AssemblyConfigurationAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyConfigurationAttribute)))?.Configuration;

                    repo.Save(c);

                    //AssemblyCompany
                    c = repo.Single<Config>(x => x.ConfigKey == "AssemblyCompany");
                    c.ConfigValue = ((AssemblyCompanyAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute)))?.Company;

                    repo.Save(c);

                    //AssemblyProduct
                    c = repo.Single<Config>(x => x.ConfigKey == "AssemblyProduct");
                    c.ConfigValue = ((AssemblyProductAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute)))?.Product;

                    repo.Save(c);

                    //AssemblyCopyright
                    c = repo.Single<Config>(x => x.ConfigKey == "AssemblyCopyright");
                    c.ConfigValue = ((AssemblyCopyrightAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute)))?.Copyright;

                    repo.Save(c);

                    //AssemblyTrademark
                    c = repo.Single<Config>(x => x.ConfigKey == "AssemblyTrademark");
                    c.ConfigValue = ((AssemblyTrademarkAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyTrademarkAttribute)))?.Trademark;

                    repo.Save(c);

                    //AssemblyCulture
                    c = repo.Single<Config>(x => x.ConfigKey == "AssemblyCulture");
                    c.ConfigValue = ((AssemblyCultureAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyCultureAttribute)))?.Culture;

                    repo.Save(c);

                    //AssemblyVersion
                    var assemblyName = assembly.GetName();
                    var version = assemblyName.Version;

                    c = repo.Single<Config>(x => x.ConfigKey == "AssemblyVersion");
                    c.ConfigValue = version?.ToString();

                    repo.Save(c);

                    //AssemblyFileVersion
                    c = repo.Single<Config>(x => x.ConfigKey == "AssemblyFileVersion");
                    c.ConfigValue = ((AssemblyFileVersionAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyFileVersionAttribute)))?.Version;

                    repo.Save(c);
                }
            }
        }

        public static class Cache
        {
            public static List<Config> ConfigList;

            static Cache()
            {
                InitCache();
            }

            public static void RefreshCache()
            {
                InitCache();
            }

            private static void InitCache()
            {
                using (IRepository repo = new Repository())
                {
                    ConfigList = repo.All<Config>();
                }
            }

            public static Config Load(string key)
            {
                return ConfigList.Find(x => x.ConfigKey.Equals(key));
            }

            public static string LoadDict(string key)
            {
                return GetDictionaryByConfigSystem()[key];
            }

            public static Dictionary<string, string> GetDictionaryByConfigSystem()
            {
                var list = ConfigList;

                if (list.Count > 0)
                {
                    var dict = new Dictionary<string, string>();

                    foreach (var c in list)
                    {
                        try
                        {
                            dict.Add(c.ConfigKey, c.ConfigValue);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    return dict;
                }
                return null;
            }
        }

        #region Members and Properties

        [Column("CONFIGKEY")]
        public string ConfigKey { get; set; }

        [Column("CONFIGVALUE")]
        public string ConfigValue { get; set; }

        #endregion
    }
}