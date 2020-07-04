using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Fanap.DataLabeling.Localization
{
    public static class DataLabelingLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(DataLabelingConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(DataLabelingLocalizationConfigurer).GetAssembly(),
                        "Fanap.DataLabeling.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
