using BoxApiSample.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoxApiSample.Forms
{
    public partial class FileUploadSampleForm : Form
    {
        public FileUploadSampleForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var api = new BoxApi
                {
                    ConfigJsonFile = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString(), "box_config.json")
                };
                // 接続
                await api.Connect();

                // とりあえずルートディレクトリ（ "0" ）の内容を取り出してみる
                var items = await api.GetDirectories("0");
                foreach (var i in items)
                {
                    Console.WriteLine("{0}: {1}", i.Id, i.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void _buttonUploadDirectoryRef_Click(object sender, EventArgs e)
        {
            var selectedPath = _textBoxUploadDirectory.Text;

            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "アップロードするフォルダを選択してください。";
                if (File.Exists(selectedPath))
                {
                    dialog.SelectedPath = selectedPath;
                }
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    selectedPath = dialog.SelectedPath;
                }
            }

            if (selectedPath != _textBoxUploadDirectory.Text)
            {
                _textBoxUploadDirectory.Text = selectedPath;
            }
        }
    }
}
