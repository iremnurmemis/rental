

using System.Diagnostics;

namespace Business
{
    public class PythonOCRModelManager : IOCRModelService
    {
        public string RunPythonScript(byte[] imageBytes)
        {

            // string pythonScriptPath = @"C:\Users\ılayda\Downloads\python_ocr_model.py";
            string pythonScriptPath = @"C:\Users\ılayda\Downloads\compare_using_sift.py";

           
            var startInfo = new ProcessStartInfo
            {
                FileName = @"C:\Users\ılayda\AppData\Local\Programs\Python\Launcher\py.exe", // Python yolu
                Arguments = pythonScriptPath, // Betik yolu
                RedirectStandardInput = true,  
                RedirectStandardOutput = true,  
                UseShellExecute = false,  
                CreateNoWindow = true  
            };

            using (var process = Process.Start(startInfo))
            {
                // Python'a byte dizisini gönder
                using (var writer = process.StandardInput)
                {
                    writer.BaseStream.Write(imageBytes, 0, imageBytes.Length);
                }

                // Python'dan gelen çıktıyı oku
                using (var reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd(); 
                    Console.WriteLine($"Python çıktısı: {result}");
                    return result;  
                }
            }
        }



    }
}
