using DinkToPdf;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ConvertFormats
{
    public partial class Form1 : Form
    {
        static string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static string inputPath = assemblyPath + "\\ConvertIn\\";
        string outputPath = inputPath + "\\ConvertOut\\";
        static DirectoryInfo directoryInfo = new DirectoryInfo(inputPath);
        //private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();
        int listBoxLogLenght = 10;
        static long logLine = 1;
        List<string> listBoxSource = new List<string>
        {
            "0 - O programa iniciou normalmente."
        };
        
        public Form1()
        {
            InitializeComponent();
            try
            {
                if (!Directory.Exists(inputPath))
                    Directory.CreateDirectory(inputPath);
                if (!Directory.Exists(outputPath))
                    Directory.CreateDirectory(outputPath);
            }
            catch (Exception)
            {
                MessageBox.Show(@"Erro ao criar diretórios de destino.
Tente executar como administrador.
Se o problema persistir contate o desenvolvedor: raythan100@yahoo.com.br", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            try
            {
                listBoxLog.DataSource = listBoxSource.ToArray();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            string fileNameWithoutExtension;
            FileInfo[] files = directoryInfo.GetFiles("*.docx");
            foreach (FileInfo file in files)
            {
                try
                {
                    fileNameWithoutExtension = file.Name.Split('.').FirstOrDefault();

                    GenerateLog($"Convertendo arquivo {file.Name}.");
                    using (var source = Package.Open(inputPath + file.Name))
                    {
                        using (var document = WordprocessingDocument.Open(source))
                        {
                            System.Xml.Linq.XElement html = HtmlConverter.ConvertToHtml(document, new HtmlConverterSettings());
                            using (var writer = File.CreateText(outputPath + fileNameWithoutExtension + ".html"))
                                writer.WriteLine(html.ToString());
                        }
                    }
                    
                    string htmlIn = File.ReadAllText(outputPath + fileNameWithoutExtension + ".html");
                    using (var pdfzim = PdfGenerator.GeneratePdf(htmlIn, PdfSharp.PageSize.A4))
                    {
                        GenerateLog($"Salvando arquivo {fileNameWithoutExtension}.pdf ...");
                        pdfzim.Save(outputPath + fileNameWithoutExtension + ".pdf");
                        GenerateLog($"Arquivo {fileNameWithoutExtension}.pdf gerado com sucesso!");
                    }
                }
                catch (Exception ex)
                {
                    GenerateLog($"Erro na geração do arquivo {file.Name}");
                }
            }
            
            if(files == null || files.Count() == 0)
                GenerateLog("Não foram localizados arquivos para conversão.");
        }

        public void GenerateLog(string message)
        {
            listBoxSource.Add(string.Format("{0} - {1}", logLine, message));

            if (listBoxSource.Count > listBoxLogLenght)
                listBoxSource.RemoveAt(0);

            listBoxLog.DataSource = listBoxSource.ToArray();
            logLine++;
        }
    }
}
