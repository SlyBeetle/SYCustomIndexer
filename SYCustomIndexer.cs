using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Adam.Core;
using Adam.Core.References;
using Adam.Core.Indexer;
using Adam.Tools;
using Adam.Core.Records;
using Adam.Core.Classifications;
using Adam.Core.Fields;


namespace SYCustomIndexer
{
    public class SYFields
    {
        public string SY_AssetName;
        public string SY_Author;
        

        public SYFields ReadXml(string filename)
        {
            SYFields syFields = new SYFields();
            XmlDocument xml = new XmlDocument();
            xml.Load(filename);
            foreach (XmlElement element in xml.GetElementsByTagName("fileInfo"))
            {
                foreach (XmlElement e in element)
                {

                    switch (e.Name)
                    {
                        case "Author":
                            syFields.SY_Author = e.InnerText;
                            break;
                        case "AssetName":
                            syFields.SY_AssetName = e.InnerText;
                            break;
                    }
                }
            }
            return syFields;
        }
    }

   
    public class CustomIndexerProcessEngine : IndexMaintenanceJob
    {
        public CustomIndexerProcessEngine(Application app)
            : base(app)
        {
        }

        protected override void OnPreCatalog(PreCatalogEventArgs e)
        {
            base.OnPreCatalog(e);
            
        }

        protected override void OnCatalog(CatalogEventArgs e)
        {
            base.OnCatalog(e);

            string pathToXml = Path.GetDirectoryName(e.Path) + @"\" + Path.GetFileNameWithoutExtension(e.Path) + ".xml"; 
            var syFields = new SYFields();
            syFields = syFields.ReadXml(pathToXml);
        
            e.Record.Fields.GetField<TextField>("SY_AssetName").SetValue(syFields.SY_AssetName);
            e.Record.Fields.GetField<TextField>("SY_Author").SetValue(syFields.SY_Author);
            //e.Record.Fields.GetField<TextField>("Duration").SetValue(e.FileVersion.GetInfoSection("Duration").Value);
            //e.Record.Fields.GetField<TextField>("Filesize").SetValue(e.Record.Files.Master.Versions.Latest.Filesize.ToString());
            e.Record.Save();
        }



        //protected override void OnPostCatalog(PostCatalogEventArgs e)
        //{
        //    base.OnPostCatalog(e);
        //    //string folder = @"\\evbyminsd1981\IndexerTest\SimpleYoutubeIndexer"; 
        //    //string path2 = Path.Combine(folder, "videoPreview.mp4");
        //    //string saveThumbnailTo = System.IO.Path.Combine(@"\\evbyminsd1981\IndexerTest\SimpleYoutubeIndexer", DateTime.Now.Year + ".jpg");
        //    //System.IO.File.Delete(saveThumbnailTo);
        //    //System.IO.File.Delete(path2);

        //}
    }
}
