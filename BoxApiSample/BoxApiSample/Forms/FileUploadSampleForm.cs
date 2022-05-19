using BoxApiSample.Api;
using BoxApiSample.Data;
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


                // 対象のフォルダに yyyy/MM/dd/HH/mm/ss というフォルダ階層を作ってみます
                var now = DateTime.Now;
                var directories = await api.CreateFolders(
                    _textBoxRootDirectoryId.Text,
                    new List<BoxFolderItem>()
                    {
                        new BoxFolderItem
                        {
                            Name = now.Year.ToString("D4")
                        },
                        new BoxFolderItem
                        {
                            Name = now.Month.ToString("D2")
                        },
                        new BoxFolderItem
                        {
                            Name = now.Day.ToString("D2")
                        },
                        new BoxFolderItem
                        {
                               Name = now.Hour.ToString("D2")
                        },
                        new BoxFolderItem
                        {
                            Name = now.Minute.ToString("D2")
                        },

                        new BoxFolderItem
                        {
                            Name = now.Second.ToString("D2")
                        },

                    });




                Console.WriteLine("つくった");
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

        private async void _buttonUpload_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("アップロードします。\n\nよろしいですか？", "アップロード確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
            {
                return;
            }
            // 選択したフォルダをアップロード
            if (!Directory.Exists(_textBoxUploadDirectory.Text))
            {
                MessageBox.Show("アップロードするフォルダを指定してください。");
                return;
            }

            _buttonUpload.Enabled = false;
            _uploadProgressBar.Visible = true;
            var taskE = await Task.Run(async () =>
            {
                try
                {
                    var api = new BoxApi
                    {
                        ConfigJsonFile = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString(), "box_config.json")
                    };
                    // 接続
                    await api.Connect();

                    // yyyyMMdd_HHmmss フォルダを作ります
                    var createFolderReq = new List<BoxFolderItem>()
                    {
                        new BoxFolderItem
                        {
                            Name = DateTime.Now.ToString("yyyyMMdd_HHmmss")
                        }
                    };
                    var createFolderRes = await api.CreateFolders(_textBoxRootDirectoryId.Text, createFolderReq);
                    var targetId = createFolderRes[0].Id;

                    // フォルダをアップロード
                    await api.UploadFolder(targetId, _textBoxUploadDirectory.Text);
                }
                catch (Exception ex)
                {
                    return ex;
                }

                return null;
            });
            _uploadProgressBar.Visible = false;
            _buttonUpload.Enabled = true;

            if (taskE != null)
            {
                MessageBox.Show(taskE.ToString());
            }
            else
            {
                MessageBox.Show("アップロードが完了しました。");
            }
        }
    }
}
