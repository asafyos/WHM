using Bytescout.BarCode;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.IO;
using System.Reflection;

namespace warehouse2 {
    class BarcodeService {




        public void SaveBarcodesInFile(string[] list) {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Word Document|*.docx";
            MSWordService service = new MSWordService();
            if (saveDialog.ShowDialog() == true) {
                if (saveDialog.FileName.IndexOf(".docx") == -1)
                    saveDialog.FileName += ".docx";
                service.SetOutputPath(saveDialog.FileName);
                service.GenerateBarcodesToOutput(list);
            }
        }

    }

    public class MSWordService {
        private object fileInputPath;
        private object fileOutputPath;
        private string FILE_OUTPUT;
        private Application MSWord;

        public MSWordService() {
            MSWord = new Application();
            MSWord.Visible = false;

            SetInputPath(AppDomain.CurrentDomain.BaseDirectory + "\\tamplate.docx");
        }

        public void GenerateBarcodesToOutput(string[] valueList) {
            var wordDocument = GetDocument();
            var bookmarks = wordDocument.Bookmarks;

            Barcode barcode = GetNewBarcode();

            int i = 0;
            int j = 1;
            do {
                wordDocument = GetDocument();
                bookmarks = wordDocument.Bookmarks;
                foreach (Bookmark mark in bookmarks) {
                    if (i >= valueList.Length) {
                        break;
                    }
                    barcode.Value = valueList[i];
                    var bookmarkRange = GetBookmarkLocation(mark.Name, wordDocument);
                    var barcodeImage = barcode.GetImage();

                    System.Windows.Clipboard.SetDataObject(barcodeImage);
                    bookmarkRange.Paste();
                    i++;
                }
                wordDocument.SaveAs(ref fileOutputPath);
                wordDocument.Close();
                string[] headers = FILE_OUTPUT.ToString().Split('.');
                headers[headers.Length - 2] += "_00" + j.ToString();
                string temp = "";
                for (int s = 0; s < headers.Length; s++) {
                    temp += headers[s] + (s == headers.Length - 1 ? "" : ".");
                }
                fileOutputPath = temp;
                j++;
            } while (i < valueList.Length);

            QuitWord(MSWord);
        }

        private void SetInputPath(string path) {
            fileInputPath = path;
        }
        public void SetOutputPath(string path) {
            fileOutputPath = path;
            FILE_OUTPUT = path;
        }
        public Document GetDocument() {
            object readOnly = true;
            object convert = false;
            Document docWord = MSWord.Documents.Open(ref fileInputPath, ref convert, ref readOnly);
            return docWord;
        }
        private Range GetBookmarkLocation(object bookmarkName, Document document) {
            Range bookmarkLocations = document.Bookmarks.get_Item(ref bookmarkName).Range;
            return bookmarkLocations;
        }

        private Barcode GetNewBarcode() {
            Barcode barcode = new Barcode();
            //barcode.Value = value;
            barcode.Symbology = SymbologyType.Code128;
            return barcode;
        }

        private void QuitWord(Application app) {
            object saveChanges = false;
            app.Quit(ref saveChanges);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
        }
    }

    public class BarcodeViewModel : ObservableObject {
        private string value;

        public string Value {
            get { return this.value; }
            set {
                this.value = value;
                onPropertyChanged("Value");
            }
        }
        private SymbologyType symbology;

        public SymbologyType Symbology {
            get { return symbology; }
            set {
                symbology = value;
                onPropertyChanged("Symbology");
            }
        }

    }

    public class ObservableObject : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void onPropertyChanged(string prop) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
