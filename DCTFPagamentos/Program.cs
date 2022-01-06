using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using DCTFPagamentos.Entities;
using DCTFPagamentos.Entities.Enums;
using DCTFPagamentos.Services;

namespace DCTFPagamentos
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding utf8WithoutBom = new UTF8Encoding(false);
            Console.WriteLine("DCTF - Lançador de Pagamentos");
            Console.Write("Cole ao lado o endereço onde está salvo os arquivos da Domínio: ");
            string path = @Console.ReadLine();
            Directory.CreateDirectory(path + @"\Cópias ajustadas");
            IEnumerable<string> files = Directory.EnumerateFiles(path, "*.RFB");
            List<string> fileNames = new List<string>();
            foreach (string s in files)
            {
                fileNames.Add(s);
            }

            for (int i = 0; i < fileNames.Count; i++)
            {
                string[] FilePathNameSplit = fileNames[i].Split("\\");
                string fileName = FilePathNameSplit[FilePathNameSplit.Length - 1];
                string newPath = path + @"\Cópias ajustadas\" + @fileName;
                string[] lines = File.ReadAllLines(fileNames[i]);
                string cnpj = lines[0].Substring(21, 14);
                if (!ChecagemMalsPagadores.ChecarPagador(cnpj))
                {
                    StreamWriter sw = new StreamWriter(newPath, true, utf8WithoutBom);
                    string compet = lines[0].Substring(139, 8);
                    string competFormatted = compet.Substring(0, 2) + "/" + compet.Substring(2, 2) + "/" + compet.Substring(4, 4);
                    DateTime competencia = DateTime.Parse(competFormatted);
                    sw.AutoFlush = true;
                    sw.WriteLine(lines[0]);
                    sw.WriteLine(lines[1]);
                    sw.WriteLine(lines[2]);
                    sw.WriteLine(lines[3]);
                    int count = 0;
                    for (int j = 4; j < lines.Length - 1; j++)
                    {
                        count++;
                        sw.WriteLine(lines[j]);
                        string codImposto = lines[j].Substring(34, 4);
                        string valorImposto = lines[j].Substring(80, 4);
                        Impostos imp = new Impostos();
                        if (codImposto == "8109" || codImposto == "6912")
                        {
                            imp = Impostos.PIS;
                        }
                        else if (codImposto == "2172" || codImposto == "5856")
                        {
                            imp = Impostos.COFINS;
                        }
                        else if (codImposto == "2372")
                        {
                            imp = Impostos.CSLL;
                        }
                        else if (codImposto == "2089")
                        {
                            imp = Impostos.IRPJ;
                        }
                        else if (codImposto == "5952")
                        {
                            imp = Impostos.CSRF;
                        }
                        else if (codImposto == "5123")
                        {
                            imp = Impostos.IPI;
                        }
                        else
                        {
                            imp = Impostos.IRRF;
                        }
                        string vcto = CalculadoraDeVencimento.CalcularVencimento(imp, competencia);
                        string[] vctoProcess = vcto.Split('/');
                        string year = vctoProcess[vctoProcess.Length - 1];
                        string vencimento = "";
                        foreach (string s in vctoProcess)
                        {
                            vencimento += s;
                        }
                        StringBuilder sb = new StringBuilder();
                        sb.Append("R11");
                        sb.Append(lines[j].Substring(3, 67));
                        sb.Append(compet);
                        sb.Append(cnpj);
                        sb.Append(codImposto);
                        sb.Append(vencimento);
                        sb.Append("                 0000000000");
                        sb.Append(valorImposto);
                        sb.Append("00000000000000000000000000000000000000");
                        sb.Append(valorImposto);
                        sb.Append("          ");
                        sw.WriteLine(sb.ToString());
                    }
                    string lastLine = lines[lines.Length - 1];
                    int lineCount = int.Parse(lastLine.Substring(1, 1));
                    lineCount += count;
                    StringBuilder sB = new StringBuilder();
                    sB.Append(lastLine.Substring(0, 35));
                    sB.Append(lineCount);
                    int lengthTest = sB.Length;
                    for (int j = lengthTest - 1; j < 103; j++)
                    {
                        sB.Append(" ");
                    }
                    sw.WriteLine(sB.ToString());
                }
            }
        }
    }
}
