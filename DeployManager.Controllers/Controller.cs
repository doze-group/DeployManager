using DeployManager.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeployManager
{
    public class Controller
    {
        private OpenFileDialog ofd = new OpenFileDialog();

        List<AppExec> AppItem_List = new List<AppExec>();

        private const string Title_form = "DeployManager";
        private dynamic resMsg;
        public Controller()
        {
            resMsg = new
            {
                insert_name = "Insert the new NAME",
                confirmQe_newName = "Are you shure that you wanna change item's NAME?",
                name3 = "Are you shure that you wanna change item's PATH?",
                name4 = "Are you shure that you wanna delete this item?",
                name5 = "batch files (*.bat, *.exe)|*.bat; *.exe",
                name6 = "",
                name7 = "",
                name8 = "",
                name9 = "",
                name10 = ""
            };
        }

        public bool Create(AppExec app)
        {
            try
            {
                AppItem_List.Add(app);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Delete(int index)
        {
            try
            {
                AppItem_List.RemoveAt(index);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Update(AppExec app, int index)
        {
            try
            {
                AppItem_List[index] = app;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public AppExec FindItem(string id)
        {
            return AppItem_List.Find(x => x.Id == id);
        }
        private int FindItemIndex(string id)
        {
            return AppItem_List.FindIndex(x => x.Id == id);
        }
        public List<AppExec> getAppItem_List() => AppItem_List;
        public void Pre_Config(DataGridView tbl)
        {
            ofd.Filter = resMsg.name5;
            ListAll_tbl(tbl);
        }

        //this method works to get column value of row, My method
        private string tbl_rowItem(DataGridViewCellEventArgs e, String Column, DataGridView tbl) => tbl.Rows[e.RowIndex].Cells[Column].FormattedValue.ToString();
        //this method works to get row value of column, My method
        public DataGridViewColumn tbl_colItem(DataGridViewCellEventArgs e, DataGridView tbl) => tbl.Columns[e.ColumnIndex];
        private string InputBox(string Prompt, string Title, string DefaultResponse)
            => Microsoft.VisualBasic.Interaction.InputBox(Prompt, Title, DefaultResponse);
        public object[] rowBase(AppExec item) => new object[] { item.Name, item.Path, "", "", "", item.Id };
        private int FindIndex_tbl(DataGridViewCellEventArgs e, DataGridView tbl) => FindItemIndex(tbl_rowItem(e, "ID", tbl));
        private void ListAll_tbl(DataGridView tbl)
        {
            try
            {
                tbl.Rows.Clear();
                AppItem_List = AppItem_List.Deserialize<AppExec>();
                AppItem_List.ForEach(item =>
                {
                    tbl.Rows.Add(rowBase(item));
                });
            }
            catch (Exception)
            {
                if (AppItem_List.Count == 0) AppItem_List.Serialize();
            }

        }
        private bool BackupAndList(DataGridView tbl)
        {
            try
            {
                AppItem_List.Serialize();
                ListAll_tbl(tbl);
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }
        public void NameEvent(DataGridViewCellEventArgs e, DataGridView tbl)
        {
            string input = InputBox(resMsg.insert_name, Title_form, tbl_rowItem(e, "Name", tbl));         
            if (!input.Equals(""))
            {
                DialogResult dr = MessageBox.Show(
               resMsg.confirmQe_newName,
               Title_form, MessageBoxButtons.YesNoCancel,
               MessageBoxIcon.Information);

                if (dr == DialogResult.Yes)
                {              
                    int ListIndex = FindIndex_tbl(e, tbl);
                    AppExec temp = AppItem_List[ListIndex];
                    temp.Name = input;
                    Update(temp, ListIndex);
                    BackupAndList(tbl);
                }
            }
        }

        public void PathEvent(DataGridViewCellEventArgs e, DataGridView tbl)
        {
            DialogResult dr1 = MessageBox.Show(resMsg.name3
                        ,
                        Title_form, MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Information);

            if (dr1 == DialogResult.Yes)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    int ListIndex = FindIndex_tbl(e, tbl);
                    AppExec temp = AppItem_List[ListIndex];
                    temp.Path = ofd.FileName;
                    Update(temp, ListIndex);
                    BackupAndList(tbl);
                }
            }
        }

        public void DeleteEvent(DataGridViewCellEventArgs e, DataGridView tbl)
        {
            DialogResult del = MessageBox.Show(
                       resMsg.name4,
                       Title_form, MessageBoxButtons.YesNoCancel,
                       MessageBoxIcon.Information);

            if (del == DialogResult.Yes)
            {
                int ListIndex = FindIndex_tbl(e, tbl);               
                Delete(ListIndex);
                BackupAndList(tbl);
            }
        }

        public void DeployEvent(DataGridViewCellEventArgs e, DataGridView tbl)
        {
            var file_path = tbl_rowItem(e, "Path", tbl);
            var dir_path = new FileInfo(file_path).Directory.FullName;
            Process batch_exec = new Process();
            batch_exec.StartInfo.FileName = file_path;
            batch_exec.StartInfo.WorkingDirectory = dir_path;
            batch_exec.Start();
        }

        public void PreviewEvent(DataGridViewCellEventArgs e, DataGridView tbl)
        {
            string fullPath = tbl_rowItem(e, "Path", tbl);
            var FileInfo = new FileInfo(fullPath);
            // opens the folder in explorer
            Process.Start(@FileInfo.Directory.FullName);
        }
        public void IdEvent(DataGridViewCellEventArgs e, DataGridView tbl)
        {

            string fullPath = tbl_rowItem(e, "Path", tbl);
            var FileInfo = new FileInfo(fullPath);

            MessageBox.Show(String.Format(
                "[ Name File: ] \n{0}\n\n[ Directory: ] \n>{1}\n\n[ FullPath: ] \n{2}\n\n[ ID: ] \n{3}\n\n[ Description: ] \n{4}",
                FileInfo.Name,
                FileInfo.Directory.FullName,
                fullPath,
                tbl_rowItem(e, "ID", tbl),
                tbl_rowItem(e, "Tag", tbl)
                ), Title_form);
        }

        public void findPath(TextBox txt)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txt.Text = ofd.FileName;
            }
        }
    }
}

