using Bytescout.BarCode;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using word = Microsoft.Office.Interop.Word;
using System.IO;
using System.Reflection;
using System.Threading;

namespace warehouse2 {
    class BarcodeService {

        public static void SaveBarcodesInFile(string[] list) {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Word Document|*.docx";
            MSWordService service = new MSWordService();
            if (saveDialog.ShowDialog() == true) {
                if (saveDialog.FileName.IndexOf(".docx") == -1)
                    saveDialog.FileName += ".docx";
                service.SetOutputPath(saveDialog.FileName);
            service.ValueList = list;
            Thread thread = new Thread(service.GenerateBarcodesToOutput);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            }
        }

    }

    public class MSWordService {
        private object fileInputPath;
        private object fileOutputPath;
        private string FILE_OUTPUT;
        private word.Application MSWord;

        public MSWordService() {
            MSWord = new word.Application();
            MSWord.Visible = false;
            SetInputPath(AppDomain.CurrentDomain.BaseDirectory + "\\tamplate.docx");
        }
        public string[] ValueList {
            get; set;
        }

        public void GenerateBarcodesToOutput() {
            const float c_pictureWidth = 170;
            string c_picFile = AppDomain.CurrentDomain.BaseDirectory + "\\pic.jpg";
            System.Drawing.Image[] barcodes = new System.Drawing.Image[ValueList.Length];
            Barcode barcode = GetNewBarcode();
            for (int b = 0; b < ValueList.Length; b++) {
                barcode.Value = ValueList[b];
                barcodes[b] = barcode.GetImage();
            }

            word.Application msWord = null;
            word.Document doc = null;
            object oMissing = System.Reflection.Missing.Value;
            object oEndOfDoc = "\\endofdoc";
            try {
                msWord = new word.Application();
                doc = msWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            if (msWord != null && doc != null) {
                //doc.Range().PageSetup.Orientation = word.WdOrientation.wdOrientLandscape;
                word.Table newTable;
                word.Range wrdRange = doc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                newTable = doc.Tables.Add(wrdRange, 1, 2, ref oMissing, oMissing);
                newTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                newTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                newTable.AllowAutoFit = true;
                
                for (int i = 0; i < barcodes.Length; i += 2) {
                    //System.Windows.Clipboard.SetDataObject(barcodes[i]);
                    //newTable.Cell(newTable.Rows.Count, 1).Range.Paste();
                    barcodes[i].Save(c_picFile);
                    var picture = newTable.Cell(newTable.Rows.Count, 1).Range.InlineShapes.AddPicture(c_picFile);
                    picture.Width = c_pictureWidth;
                    if (i < barcodes.Length - 1) {
                        //System.Windows.Clipboard.SetDataObject(barcodes[i + 1]);
                        //newTable.Cell(newTable.Rows.Count, 2).Range.Paste();
                        barcodes[i + 1].Save(c_picFile);
                        picture = newTable.Cell(newTable.Rows.Count, 2).Range.InlineShapes.AddPicture(c_picFile);
                        picture.Width = c_pictureWidth;
                    }
                    if (i < barcodes.Length - 2) {
                        newTable.Rows.Add();
                    }
                }

                File.Delete(c_picFile);
                if (fileOutputPath != null)
                    doc.SaveAs(ref fileOutputPath);
                msWord.Visible = true;
                //doc.Close();
            }
        }

        private void SetInputPath(string path) {
            fileInputPath = path;
        }
        public void SetOutputPath(string path) {
            fileOutputPath = path;
            FILE_OUTPUT = path;
        }
        private Barcode GetNewBarcode() {
            Barcode barcode = new Barcode();
            //barcode.Value = value;
            barcode.Symbology = SymbologyType.Code128;
            return barcode;
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
